using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace batchRenameApp
{
    class Folder : INotifyPropertyChanged
    {

        public string folderimage { get; set; }
        public string foldername { get; set; }

        public string newfoldername { get; set; }

        public string folderpath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Folder(string folderdir)
        {
            this.foldername = folderdir.Substring(folderdir.LastIndexOf(@"\") + 1, folderdir.Length - (folderdir.LastIndexOf(@"\") + 1));
           //volumedir = volumedir.Substring(0, volumedir.LastIndexOf(@"\") + 1);
            this.folderpath = folderdir;
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
    }
}
