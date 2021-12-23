using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Controls;
using Contract;

namespace AddSuffixRule
{
    public class AddSuffixData
    {
        public string Suffix { get; set; }
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }
    public class AddSuffix : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }

        public RuleWindow ConfigurationUI { get; set; }

        public bool IsInUse { get; set; }

        public string Suffix { get; set; }

        public AddSuffix()
        {
            this.Suffix = "";
            this.Name = "Add Suffix";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public AddSuffix(string suffix)
        {
            this.Suffix = suffix;
            this.Name = "Add Suffix";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ToJson()
        {
            AddSuffixData data = new AddSuffixData();
            data.Suffix = this.Suffix;
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            AddSuffixData ruleData = (AddSuffixData)JsonSerializer.Deserialize(json, typeof(AddSuffixData));
            AddSuffix rule = new AddSuffix();
            rule.Name = ruleData.Name;
            rule.Suffix = ruleData.Suffix;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }
        public IRule Clone()
        {
            return new AddSuffix(Suffix);
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
                if (type == 2 && tokens.Length > 1) nonex = "." + tokens[tokens.Length - 1];

                string result = "";

                result = $"{nonex}{Suffix}{ex}";

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
