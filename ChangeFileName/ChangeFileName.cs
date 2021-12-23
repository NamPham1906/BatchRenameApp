using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace ChangeFileNameRule
{
    public class ChangeFileName: IRule, INotifyPropertyChanged
    {
        public string NewName { get; set; }
        public string Name { get; set; }
        public ChangeFileNameWindow ConfigurationUI { get; set; }
        public bool IsInUse { get; set; }
        public bool IsUse()
        {
            return IsInUse;
        }

        public ChangeFileName()
        {
            Name = "Change File Name";
            this.NewName = "";
            ConfigurationUI = new ChangeFileNameWindow(this);
        }

        public ChangeFileName(string newname)
        {
            Name = "Change File Name";
            this.NewName = newname;
            ConfigurationUI = new ChangeFileNameWindow(this);

        }

        public List<string> Rename(List<string> originals, int type)
        {
            string newn = NewName;
            List<string> result = new List<string>();
            switch (type)
            {
                case 1:
                    {
                        foreach (string item in originals)
                        {
                            string[] str = item.Split('.');
                            int strlen = str.Length;

                            string extension = str[strlen - 1];
                            string newfilename = $"{newn}.{extension}";
                            result.Add(newfilename);
                        }

                        return result;
                    }
                case 2:
                    {
                        foreach (string item in originals)
                        {
                            string newfilename = $"{newn}";
                            result.Add(newfilename);
                        }
                        return result;
                    }
                default:
                    return originals;
            }

        }

        public IRule Clone()
        {
            return new ChangeFileName(NewName);
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
