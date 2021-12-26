using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.Json;

namespace batchRenameApp
{
    public class MyFile : INotifyPropertyChanged
    {

        public string fileimage { get; set; }
        public string filename { get; set; }
        public string newfilename { get; set; }
        public string fileextension { get; set; }
        public string filepath { get; set; }

        public string status { get; set; }

        private String BatchingSuccessStatus = "Batching successfully";
        private String BatchingUnsuccessErrorStatus = "Error: Batching unsuccessfully";
        private String FileDulicateErrorStatus = "Error: File duplicate";
        private String FileNotExistErrorStatus = "Error: File not exists";
        private String FileNameNotChangeErrorStatus = "Error: File name unchanged";
        private  String FileNameNotValidErrorStatus = "Error: Invalid file name";
        private String FileNameLengthErrorStatus = "Error: File name exceeds 255 characters";
        private String FileNameTooShortErrorStatus = "Error: File name is too short";

        public event PropertyChangedEventHandler PropertyChanged;

        public MyFile(string filedir)
        {

            FileInfo myfile = new FileInfo(filedir);
            this.filename = myfile.Name;
            this.newfilename = this.filename;
            this.fileextension = myfile.Extension;
            this.fileimage = ImageForExtension(this.fileextension);
            this.filepath = filedir;
            this.status = "";
        }

        public string ImageForExtension(string extension)
        {
            string image;
            switch (extension)
            {
                case ".png":
                case ".PNG":
                    image = "images/extension-icon/png-file.png";
                    break;
                case ".doc":
                case ".docx":
                    image = "images/extension-icon/doc-file.png";
                    break;
                case ".DOC":
                case ".DOCX":
                    image = "images/extension-icon/docx-file.png";
                    break;
                case ".txt":
                case ".TXT":
                    image = "images/extension-icon/txt-file.png";
                    break;
                case ".csv":
                case ".CSV":
                    image = "images/extension-icon/csv-file.png";
                    break;
                case ".dll":
                case ".DLL":
                    image = "images/extension-icon/dll-file.png";
                    break;
                case ".pdf":
                case ".PDF":
                    image = "images/extension-icon/pdf-file.png";
                    break;
                case ".ppt":
                case ".pptx":
                case ".PPT":
                case ".PPTX":
                    image = "images/extension-icon/ppt-file.png";
                    break;
                case ".xls":
                case ".XLS":
                    image = "images/extension-icon/xls-file.png";
                    break;
                case ".zip":
                case ".ZIP":
                    image = "images/extension-icon/zip-file.png";
                    break;
                case ".jpeg":
                case ".JPEG":
                    image = "images/extension-icon/jpeg-file.png";
                    break;
                case ".jpg":
                case ".JPG":
                    image = "images/extension-icon/jpg-file.png";
                    break;
                case ".mp3":
                case ".MP3":
                    image = "images/extension-icon/mp3-file.png";
                    break;
                case ".mp4":
                case ".MP4":
                    image = "images/extension-icon/mp4-file.png";
                    break;
                case ".exe":
                case ".EXE":
                    image = "images/extension-icon/exe-file.png";
                    break;
                case ".wmv":
                case ".WMV":
                    image = "images/extension-icon/wmv-file.png";
                    break;
                case ".rar":
                case ".RAR":
                    image = "images/extension-icon/rar-file.png";
                    break;
                case ".ai":
                case ".AI":
                    image = "images/extension-icon/ai-file.png";
                    break;
                case ".bat":
                case ".BAT":
                    image = "images/extension-icon/bat-file.png";
                    break;
                case ".bin":
                case ".BIN":
                    image = "images/extension-icon/bin-file.png";
                    break;
                case ".chm":
                case ".CHM":
                    image = "images/extension-icon/chm-file.png";
                    break;
                case ".css":
                case ".CSS":
                    image = "images/extension-icon/css-file.png";
                    break;
                case ".cur":
                case ".CUR":
                    image = "images/extension-icon/cur-file.png";
                    break;
                case ".dat":
                case ".DAT":
                    image = "images/extension-icon/dat-file.png";
                    break;
                case ".tif":
                case ".TIF":
                    image = "images/extension-icon/tif-file.png";
                    break;
                case ".gho":
                case ".GHO":
                    image = "images/extension-icon/gho-file.png";
                    break;
                case ".gif":
                case ".GIF":
                    image = "images/extension-icon/gif-file.png";
                    break;
                case ".jar":
                case ".JAR":
                    image = "images/extension-icon/jar-file.png";
                    break;
                case ".java":
                case ".JAVA":
                    image = "images/extension-icon/java-file.png";
                    break;
                case ".ink":
                case ".INK":
                    image = "images/extension-icon/ink-file.png";
                    break;
                case ".mov":
                case ".MOV":
                    image = "images/extension-icon/mov-file.png";
                    break;
                default:
                    image = "images/extension-icon/file.png";
                    break;
            }
            return image;
        }
        public MyFile()
        {
            this.filename = "";
            this.newfilename = "";
            this.fileextension ="";
            this.filepath = "";
            this.status = "";
        }




        public bool changeExtendsion(string fileExtension)
        {
            if (fileExtension.Length == 0)
            {
                return false;
            }
            else
            {
                this.fileextension = fileExtension;
                return true;
            }
        }
        public String ToJson()
        {
            string json = JsonSerializer.Serialize(this);
            return json;
        }

        public static MyFile Parser(string json)
        {
            MyFile result = (MyFile)JsonSerializer.Deserialize(json, typeof(MyFile));
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
            FileInfo file = new FileInfo(this.filepath);
            string newfilepath = this.filepath.Substring(0, this.filepath.LastIndexOf(@"\") + 1);
            newfilepath += this.newfilename;
            FileInfo newfile = new FileInfo(newfilepath);
            if (!file.Exists)
            {
                this.status = FileNotExistErrorStatus;
                return false;
            }
            else if (this.filename.Equals(this.newfilename))
            {
                this.status = FileNameNotChangeErrorStatus;
                return false;
            }
            else if (!isValidName(this.newfilename, 1))
            {
                this.status = FileNameNotValidErrorStatus;
                return false;
            }
            else if (this.newfilename.Length > 255)
            {

                this.status = FileNameLengthErrorStatus;
                return false;
            }
            else if (newfile.Exists)
            {
                this.status = FileDulicateErrorStatus;
                return false;
            }
            else if (newfilepath.Length <= 0)
            {
                this.status = FileNameTooShortErrorStatus;
                return false;
            }
            else if (file.Exists && !newfile.Exists)
            {

                try
                {
                    System.IO.File.Move(this.filepath, newfilepath);
                    FileInfo myfile = new FileInfo(newfilepath);
                    this.filename = myfile.Name;
                    this.newfilename = this.filename;
                    this.fileextension = myfile.Extension;
                    this.filepath = newfilepath;
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
            
            string newfilepath = folderDir + @"\";
            newfilepath += this.newfilename;
            FileInfo file = new FileInfo(this.filepath);
            FileInfo newfile = new FileInfo(newfilepath);
            if (!file.Exists)
            {
                this.status = FileNotExistErrorStatus;
                return false;
            }
            else if (this.filename.Equals(this.newfilename))
            {
                this.status = FileNameNotChangeErrorStatus;
                return false;
            }
            else if (!isValidName(this.newfilename, 1))
            {
                this.status = FileNameNotValidErrorStatus;
                return false;
            }
            else if (this.newfilename.Length > 255)
            {

                this.status = FileNameLengthErrorStatus;
                return false;
            }
            else if (newfile.Exists)
            {
                this.status = FileDulicateErrorStatus;
                return false;
            }
            else if (newfilepath.Length <= 0)
            {
                this.status = FileNameTooShortErrorStatus;
                return false;
            }
            else if (file.Exists && !newfile.Exists)
            {

                try
                {
                    File.Copy(this.filepath, newfilepath, true);
                    FileInfo myfile = new FileInfo(newfilepath);
                    this.filename = myfile.Name;
                    this.newfilename = this.filename;
                    this.fileextension = myfile.Extension;
                    this.filepath = newfilepath;
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



        public bool checkExist()
        {
            FileInfo file = new FileInfo(this.filepath);
            if (!file.Exists) {
                return false;
            } else{
                return true;
            }
        }

    }
}
