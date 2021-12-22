using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Contract;

namespace AddPrefixRule
{
    public class AddPrefixRule : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }
        
        public RuleWindow ConfigurationUI { get; set; }
        
        public string Prefix { get; set; }

        public AddPrefixRule()
        {
            this.Name = "Add Prefix";
            this.Prefix = "";
            ConfigurationUI = new RuleWindow(this);
        }

        public AddPrefixRule(string prefix)
        {
            this.Name = "Add Prefix";
            this.Prefix = prefix;
            ConfigurationUI = new RuleWindow(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new AddPrefixRule(Prefix);
        }

        public List<string> Rename(List<string> originals)
        {
            List<string> results = new List<string>();

            foreach (var original in originals)
            {
                string nonex = "";

                var tokens = original.Split(".");

                nonex += tokens[0];
                for (int i = 1; i < tokens.Length - 1; i++)
                    nonex += "." + tokens[i];

                string result = "";

                result = $"{Prefix}{nonex}";

                results.Add(result);
            }

            return results;
        }

        public UserControl GetUI()
        {
            return ConfigurationUI;
        }
    }
}
