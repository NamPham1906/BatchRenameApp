using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Contract;

namespace AddPrefixRule
{
    public class AddPrefix : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }
        
        public RuleWindow ConfigurationUI { get; set; }

        public string Prefix { get; set; }

        public bool IsInUse { get; set; }

        public AddPrefix()
        {
            this.Name = "Add Prefix";
            this.Prefix = "";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public AddPrefix(string prefix)
        {
            this.Name = "Add Prefix";
            this.Prefix = prefix;
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new AddPrefix(Prefix);
        }

        public List<string> Rename(List<string> originals, int type)
        {
            List<string> results = new List<string>();

            foreach (var original in originals)
            {
                string nonex = "";
                string ex = "";

                var tokens = original.Split(".");

                nonex += tokens[0];
                for (int i = 1; i < tokens.Length - 1; i++)
                    nonex += "." + tokens[i];

                if (type == 1) ex = "." + tokens[tokens.Length - 1];
                if (type == 2) nonex = "." + tokens[tokens.Length - 1];

                string result = "";

                result = $"{Prefix}{nonex}{ex}";

                results.Add(result);
            }

            return results;
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
    }
}
