using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace batchRenameApp
{
    class MyFile : INotifyPropertyChanged
    {

        public string fileimage { get; set; }
        public string filename { get; set; }
        public string newfilename { get; set; }
        public string fileextension { get; set; }

        public string filepath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MyFile(string filedir)
        {

            FileInfo myfile = new FileInfo(filedir);
            this.filename = myfile.Name;
            this.fileextension = myfile.Extension;
            this.filepath = filedir;
        }

        

        public bool changeName(string newName)
        {
            if (newName.Length == 0)
            {
                return false;
            }
            else
            {
                if (!newName.Contains('.'))
                {
                    newName += fileextension;
                }

                this.filename = newName;
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

    }
}
