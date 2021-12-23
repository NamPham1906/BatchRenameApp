using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Globalization;
using Contract;

namespace ConvertToLowercaseRule
{
    public class ConvertToLowercase : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }

        public RuleWindow ConfigurationUI { get; set; }

        public bool IsInUse { get; set; }
        public ConvertToLowercase()
        {
            this.Name = "Convert To Lowercase";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new ConvertToLowercase();
        }

        public string GetName()
        {
            return Name;
        }

        public UserControl GetUI()
        {
            return ConfigurationUI;
        }

        public bool IsUse()
        {
            return IsInUse;
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

                string temp = nonex;

                temp = temp.Replace(" ", "");

                string result = "";

                temp = temp.ToLower(new CultureInfo("vi-VI", false));

                result = $"{temp}{ex}";

                results.Add(result);
            }

            return results;
        }
    }
}
