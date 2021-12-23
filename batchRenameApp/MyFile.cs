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


        public bool changeName(string newNameFileDir)
        {
            if (newNameFileDir.Length == 0)
            {
                return false;
            }
            else
            {
                FileInfo myfile = new FileInfo(newNameFileDir);
                this.filename = myfile.Name;
                this.newfilename = this.filename;
                this.fileextension = myfile.Extension;
                this.filepath = newNameFileDir;
                return true;
            }
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
    }
}
