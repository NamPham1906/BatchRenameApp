using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Controls;

namespace ChangeFileNameRule
{
    public class ChangeFileNameData
    {
        public string NewName { get; set; }
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }
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
        public void SetIsUse(bool use)
        {
            IsInUse = use;
        }
        public ChangeFileName()
        {
            Name = "Change File Name";
            this.NewName = "";
            IsInUse = false;
            ConfigurationUI = new ChangeFileNameWindow(this);
            IsInUse = false;
        }

        public ChangeFileName(string newname, bool isInUse)
        {
            Name = "Change File Name";
            this.NewName = newname;
            ConfigurationUI = new ChangeFileNameWindow(this);
            IsInUse = isInUse;
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

        public string ToJson()
        {
            ChangeFileNameData data = new ChangeFileNameData();
            data.NewName = this.NewName;
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            ChangeFileNameData ruleData = (ChangeFileNameData)JsonSerializer.Deserialize(json, typeof(ChangeFileNameData));
            ChangeFileName rule = new ChangeFileName();
            rule.Name = ruleData.Name;
            rule.NewName = ruleData.NewName;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }
        public IRule Clone()
        {
            return new ChangeFileName(NewName, IsInUse);
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
