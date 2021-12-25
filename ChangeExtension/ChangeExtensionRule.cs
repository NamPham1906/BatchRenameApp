using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using Contract;

namespace ChangeExtension
{
    public class ChangeExtensionData
    {
        public string Extension { get; set; }
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }
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
        public void SetIsUse(bool use)
        {
            IsInUse = use;
        }
        public ChangeExtensionRule()
        {
            Name = "Change Extension";
            this.Extension = "";
            ConfigurationUI = new ChangeExtensionWindow(this);
            IsInUse = false;
        }

        public ChangeExtensionRule(string extension, bool isInUse)
        {
            Name = "Change Extension";
            this.Extension = extension;
            ConfigurationUI = new ChangeExtensionWindow(this);
            IsInUse = isInUse;

        }

        public List<string> Rename(List<string> originals, int type)
        {
            string newe = Extension;
            List<string> result = new List<string>();
            switch (type)
            {
                case 1:
                    {
                        foreach (string item in originals)
                        {
                            string[] str = item.Split('.');
                            int strlen = str.Length;
                            string temp = "";
                            for (int j = 0; j < strlen - 1; j++)
                            {
                                temp += str[j] + ".";
                            }

                            string newfilename = $"{temp}{newe}";
                            result.Add(newfilename);
                        }

                        return result;
                    }
                case 2:
                    {
                        return originals;
                    }
                default:
                    return originals;
            }
        }
        public string ToJson()
        {
            ChangeExtensionData data = new ChangeExtensionData();
            data.Extension = this.Extension;
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);

            return json;
        }

        public IRule Clone(string json)
        {
            ChangeExtensionData ruleData = (ChangeExtensionData)JsonSerializer.Deserialize(json, typeof(ChangeExtensionData));
            ChangeExtensionRule rule = new ChangeExtensionRule();
            rule.Name = ruleData.Name;
            rule.Extension = ruleData.Extension;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }

        public IRule Clone()
        {
            return new ChangeExtensionRule(Extension,IsInUse);
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
