using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Controls;
using Contract;

namespace AddPrefixRule
{
    public class AddPrefixData
    {
        public string Prefix { get; set; }
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }

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

        public AddPrefix(string prefix, bool isInUse)
        {
            this.Name = "Add Prefix";
            this.Prefix = prefix;
            ConfigurationUI = new RuleWindow(this);
            IsInUse = isInUse;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ToJson()
        {
            AddPrefixData data = new AddPrefixData();
            data.Prefix = this.Prefix;
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            AddPrefixData ruleData = (AddPrefixData)JsonSerializer.Deserialize(json, typeof(AddPrefixData));
            AddPrefix rule = new AddPrefix();
            rule.Name = ruleData.Name;
            rule.Prefix = ruleData.Prefix;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }
        public IRule Clone()
        {
            return new AddPrefix(Prefix, IsInUse);
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
                if (type == 2 && tokens.Length > 1) nonex += "." + tokens[tokens.Length - 1];

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
