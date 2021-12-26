using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Contract;
using System.Text.Json;

namespace RemoveExtraSpaceRule
{

    public class RemoveExtraSpaceData 
    {
        public int Configuration { get; set; }
        public string Name { get; set; }

        public bool IsInUse { get; set; }
    }
    public class RemoveExtraSpace : IRule, INotifyPropertyChanged

    {
        public int Configuration { get; set; }
        public string Name { get; set; }
        public RuleWindow ConfigurationUI { get; set; }

        public bool IsInUse { get; set; }
        public void SetIsUse(bool use)
        {
            IsInUse = use;
        }
        public RemoveExtraSpace()
        {
            Name = "Remove Extra Space";
            Configuration = 1;
            IsInUse = false;
            ConfigurationUI = new RuleWindow(this);
        }
        public RemoveExtraSpace(int configuration = 1, bool isInUse = false)
        {
            Name = "Remove Extra Space";
            Configuration = configuration;
            IsInUse = isInUse;
            ConfigurationUI = new RuleWindow(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new RemoveExtraSpace(Configuration, IsInUse);
        }


        public UserControl GetUI()
        {
            return ConfigurationUI;
        }
        public string GetName()
        {
            return Name;
        }

        public bool IsUse()
        {
            return IsInUse;
        }

        public string ToJson()
        {
            RemoveExtraSpaceData data = new RemoveExtraSpaceData();
            data.Configuration = this.Configuration;
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }
        public List<string> Rename(List<string> originals, int type)
        {
            List<string> result = new List<string>();
            switch (type)
            {
                case 1:
                    foreach (var str in originals)
                    {
                        string[] tokens = str.Split(".");
                        string temp = "";
                        for (int i = 0; i < tokens.Length - 1; i++)
                        {
                            temp += tokens[i] + ".";
                        }
                        if(temp.Length >= 1)
                        {
                            temp = temp.Remove(temp.Length - 1);
                        }
                        switch (Configuration)
                        {
                            case -1:
                                temp = temp.TrimStart();
                                break;
                            case 0:
                                temp = temp.TrimEnd();
                                break;
                            case 1:
                                temp = temp.Trim();
                                break;
                        }
                        result.Add(temp + "." + tokens[tokens.Length - 1]);
                    }
                    break;
                case 2:
                    foreach (var str in originals)
                    {
                       
                        string temp = str;
                        switch (Configuration)
                        {
                            case -1:
                                temp = temp.TrimStart();
                                break;
                            case 0:
                                temp = temp.TrimEnd();
                                break;
                            case 1:
                                temp = temp.Trim();
                                break;
                        }
                        result.Add(str);
                    }
                    break;
                default:
                    return originals;
            }
            return result;
        }

        public IRule Clone(string json)
        {
            RemoveExtraSpaceData ruleData = (RemoveExtraSpaceData)JsonSerializer.Deserialize(json, typeof(RemoveExtraSpaceData));
            RemoveExtraSpace rule = new RemoveExtraSpace();
            rule.Name = ruleData.Name;
            rule.Configuration = ruleData.Configuration;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }
    }
}
