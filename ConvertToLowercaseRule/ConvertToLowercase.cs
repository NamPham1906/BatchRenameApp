using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Globalization;
using Contract;
using System.Text.Json;

namespace ConvertToLowercaseRule
{
    public class ConvertToLowercaseData
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }

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
                if (type == 2 && tokens.Length > 1) nonex = "." + tokens[tokens.Length - 1];

                string temp = nonex;

                temp = temp.Replace(" ", "");

                string result = "";

                temp = temp.ToLower(new CultureInfo("vi-VI", false));

                result = $"{temp}{ex}";

                results.Add(result);
            }

            return results;
        }

        public string ToJson()
        {
            ConvertToLowercaseData data = new ConvertToLowercaseData();
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            ConvertToLowercaseData ruleData = (ConvertToLowercaseData)JsonSerializer.Deserialize(json, typeof(ConvertToLowercaseData));
            ConvertToLowercase rule = new ConvertToLowercase();
            rule.Name = ruleData.Name;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }
    }
}
