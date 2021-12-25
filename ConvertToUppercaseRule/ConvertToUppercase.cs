using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Windows.Controls;

namespace ConvertToUppercaseRule
{
    public class ConvertToUppercaseData
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }

    public class ConvertToUppercase : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }

        public RuleWindow ConfigurationUI { get; set; }

        public bool IsInUse { get; set; }
        public void SetIsUse(bool use)
        {
            IsInUse = use;
        }
        public ConvertToUppercase()
        {
            this.Name = "Convert To Uppercase";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public ConvertToUppercase(bool isInUse)
        {
            this.Name = "Convert To Uppercase";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = isInUse;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new ConvertToUppercase(IsInUse);
        }

        public IRule Clone(string json)
        {
            ConvertToUppercaseData ruleData = (ConvertToUppercaseData)JsonSerializer.Deserialize(json, typeof(ConvertToUppercaseData));
            ConvertToUppercase rule = new ConvertToUppercase();
            rule.Name = ruleData.Name;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
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
                if (type == 2 && tokens.Length > 1) nonex += "." + tokens[tokens.Length - 1];

                string temp = nonex;

                temp = temp.Replace(" ", "");

                string result = "";

                temp = temp.ToUpper(new CultureInfo("vi-VI", false));

                result = $"{temp}{ex}";

                results.Add(result);
            }

            return results;
        }

        public string ToJson()
        {
            ConvertToUppercaseData data = new ConvertToUppercaseData();
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }
    }

}
