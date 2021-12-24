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
            this.filepath = filedir;
            this.status = "";
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
    }
}
