using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json;
using System.IO;

namespace batchRenameApp
{
    public class Folder : INotifyPropertyChanged
    {

        public string folderimage { get; set; }
        public string foldername { get; set; }

        public string newfoldername { get; set; }

        public string folderpath { get; set; }
        public string status { get; set; }


        private String BatchingSuccessStatus = "Batching successfully";
        private String BatchingUnsuccessErrorStatus = "Error: Batching unsuccessfully";
        private String FolderDulicateErrorStatus = "Error: Folder duplicate";
        private String FolderNotExistErrorStatus = "Error: Folder not exists";
        private String FolderNameNotChangeErrorStatus = "Error: Folder name unchanged";
        private String FolderNameNotValidErrorStatus = "Error: Invalid folder name";
        private String FolderNameLengthErrorStatus = "Error: Folder name exceeds 255 characters";
        private String FolderNameTooShortErrorStatus = "Error: Folder name is too short";

        public event PropertyChangedEventHandler PropertyChanged;

        public Folder(string folderdir)
        {
            this.foldername = folderdir.Substring(folderdir.LastIndexOf(@"\") + 1, folderdir.Length - (folderdir.LastIndexOf(@"\") + 1));
            this.newfoldername = this.foldername;
            //volumedir = volumedir.Substring(0, volumedir.LastIndexOf(@"\") + 1);
            this.folderpath = folderdir;
            this.status = "";
        }

        public Folder()
        {
            folderimage = "";
            foldername = "";
            newfoldername = "";
            folderpath = "";
            status = "";
        }

        public bool changeName(string newName)
        {
            if (newName.Length == 0)
            {
                return false;
            }
            else
            {
                this.foldername = newName;
                return true;
            }
        }
        public String ToJson()
        {
            string json = JsonSerializer.Serialize(this);
            return json;
        }

        public static Folder Parser(string json)
        {
            Folder result = (Folder)JsonSerializer.Deserialize(json, typeof(Folder));
            return result;
        }


        static bool isValidName(string filename, int type)
        {
            string[] invalid = { " ", ".", "AUX", "PRN", "NUL", "CON", "COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                            "LPT0", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

            if (filename.Contains(@"\") || filename.Contains("/") || filename.Contains(":") || filename.Contains("*") || filename.Contains("?") || filename.Contains("\"") || filename.Contains("<") || filename.Contains(">") || filename.Contains("|"))
            {
                return false;
            }

            string nonex = "";
            if (type == 1)
            {
                var tokens = filename.Split(".");

                nonex += tokens[0];
                for (int i = 1; i < tokens.Length - 1; i++)
                    nonex += "." + tokens[i];
            }
            if (type == 2)
            {
                nonex = filename;
            }

            for (int i = 0; i < invalid.Length; i++)
            {
                if (nonex.ToUpper() == invalid[i]) return false;
            }

            return true;
        }

        public bool changeName()
        {
            string newfolderpath = this.folderpath.Substring(0, this.folderpath.LastIndexOf(@"\") + 1);
            newfolderpath += this.newfoldername;
            DirectoryInfo newfolder = new DirectoryInfo(newfolderpath);
            if (!Directory.Exists(this.folderpath))
            {
                this.status = FolderNotExistErrorStatus;
                return false;
            }
            else if (this.foldername.Equals(this.newfoldername))
            {
                this.status = FolderNameNotChangeErrorStatus;
                return false;
            }
            else if (!isValidName(this.newfoldername, 2))
            {
                this.status = FolderNameNotValidErrorStatus;
                return false;
            }
            else if (this.newfoldername.Length > 255)
            {

                this.status = FolderNameLengthErrorStatus;
                return false;
            }
            else if (Directory.Exists(newfolderpath))
            {
                this.status = FolderDulicateErrorStatus;
                return false;
            }
            else if (newfolderpath.Length <= 0)
            {
                this.status = FolderNameTooShortErrorStatus;
                return false;
            }
            else if (Directory.Exists(this.folderpath) && !Directory.Exists(newfolderpath))
            {
                try
                {
                    System.IO.Directory.Move(this.folderpath, newfolderpath);
                    DirectoryInfo myfolder = new DirectoryInfo(newfolderpath);
                    this.foldername = myfolder.Name;
                    this.newfoldername = this.foldername;
                    this.folderpath = newfolderpath;
                    this.status = BatchingSuccessStatus;
                    return true;
                }
                catch (Exception e)
                {
                    this.status = BatchingUnsuccessErrorStatus;
                    return false;
                }

            }
            return false;
        }


        public bool changeNameToFolder(string folderDir)
        {
            string newfolderpath = folderDir + @"\";
            newfolderpath += this.newfoldername;
            if (!Directory.Exists(this.folderpath))
            {
                this.status = FolderNotExistErrorStatus;
                return false;
            }
            else if (this.foldername.Equals(this.newfoldername))
            {
                this.status = FolderNameNotChangeErrorStatus;
                return false;
            }
            else if (!isValidName(this.newfoldername, 2))
            {
                this.status = FolderNameNotValidErrorStatus;
                return false;
            }
            else if (this.newfoldername.Length > 255)
            {

                this.status = FolderNameLengthErrorStatus;
                return false;
            }
            else if (Directory.Exists(newfolderpath))
            {
                this.status = FolderDulicateErrorStatus;
                return false;
            }
            else if (newfolderpath.Length <= 0)
            {
                this.status = FolderNameTooShortErrorStatus;
                return false;
            }
            else if (Directory.Exists(this.folderpath) && !Directory.Exists(newfolderpath))
            {
                try
                {
                    DirectoryCopy(this.folderpath, newfolderpath, true);
                 
                    DirectoryInfo myfolder = new DirectoryInfo(newfolderpath);
                    this.foldername = myfolder.Name;
                    this.newfoldername = this.foldername;
                    this.folderpath = newfolderpath;
                    this.status = BatchingSuccessStatus;
                    return true;
                }
                catch (Exception e)
                {
                    this.status = BatchingUnsuccessErrorStatus;
                    return false;
                }

            }
            return false;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }


        public bool checkExist()
        {
            if (!Directory.Exists(this.folderpath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
