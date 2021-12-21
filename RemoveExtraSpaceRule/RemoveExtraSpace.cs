using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Contract;
namespace RemoveExtraSpaceRule
{
    public class RemoveExtraSpace : IRule, INotifyPropertyChanged

    {
        public int Configuration { get; set; }
        public string Name { get; set; }
        public RuleWindow ConfigurationUI { get; set; }
        public RemoveExtraSpace()
        {
            Name = "Remove Extra Space";
            Configuration = 1;
            ConfigurationUI = new RuleWindow(this);
        }
        public RemoveExtraSpace(int configuration = 1)
        {
            Name = "Remove Extra Space";
            Configuration = configuration;
            ConfigurationUI = new RuleWindow(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new RemoveExtraSpace(1);
        }


        public UserControl GetUI()
        {
            return ConfigurationUI;
        }
        public string GetName()
        {
            return Name;
        }

        public List<string> Rename(List<string> originals)
        {
            List<string> result = new List<string>();
            foreach(var str in originals)
            {
                int index = str.LastIndexOf(".");
                string[] tokens = str.Split(".");
                string temp = "";
                for (int i = 0; i < tokens.Length - 1; i++)
                {
                    temp += tokens[i] + ".";
                }
                temp = temp.Remove(temp.Length - 1);
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
            return result;
        }

       
    }
}
