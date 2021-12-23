﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json;

namespace batchRenameApp
{
    public class Folder : INotifyPropertyChanged
    {

        public string folderimage { get; set; }
        public string foldername { get; set; }

        public string newfoldername { get; set; }

        public string folderpath { get; set; }
        public string status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Folder(string folderdir)
        {
            this.foldername = folderdir.Substring(folderdir.LastIndexOf(@"\") + 1, folderdir.Length - (folderdir.LastIndexOf(@"\") + 1));
            this.newfoldername = this.foldername;
            //volumedir = volumedir.Substring(0, volumedir.LastIndexOf(@"\") + 1);
            this.folderpath = folderdir;
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
    }
}
