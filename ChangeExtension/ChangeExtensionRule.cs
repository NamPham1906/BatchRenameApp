using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using Contract;

namespace ChangeExtension
{
    public class ChangeExtensionRule: IRule, INotifyPropertyChanged
    {
        public string Extension { get; set; }
        public string Name { get; set; }
        public ChangeExtensionWindow ConfigurationUI { get; set; }
        public bool IsInUse { get; set; }
        public bool IsUse()
        {
            return IsInUse;
        }

        public ChangeExtensionRule()
        {
            Name = "Change Extension";
            this.Extension = "";
            ConfigurationUI = new ChangeExtensionWindow(this);
        }

        public ChangeExtensionRule(string extension)
        {
            Name = "Change Extension";
            this.Extension = extension;
            ConfigurationUI = new ChangeExtensionWindow(this);
        }

        public List<string> Rename(List<string> originals, int type)
        {
            string newe = Extension;
            List<string> result = new List<string>();
            if (type == 1)
            {
                foreach (string item in originals)
                {
                    string[] str = item.Split('.');
                    int strlen = str.Length;

                    string olde = str[strlen - 1];
                    string[] str1 = item.Split($".{olde}");

                    string newfilename = $"{str1[0]}.{newe}";
                    result.Add(newfilename);
                }

                return result;
            }
            else
            {
                return originals;
            } 
                
        }

        public IRule Clone()
        {
            return new ChangeExtensionRule(Extension);
        }

        public UserControl GetUI()
        {
            return ConfigurationUI;
        }

        public string GetName()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
