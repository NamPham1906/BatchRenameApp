using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Windows.Controls;

namespace ConvertToCamelCaseRule
{
    public class ConvertToCamelCaseData
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }

    public class ConvertToCamelCase : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }

        public RuleWindow ConfigurationUI { get; set; }

        public bool IsInUse { get; set; }
        public void SetIsUse(bool use)
        {
            IsInUse = use;
        }
        public ConvertToCamelCase()
        {
            Name = "Convert To camelCase";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public ConvertToCamelCase(bool isInUse)
        {
            Name = "Convert To camelCase";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = isInUse;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new ConvertToCamelCase(IsInUse);
        }

        public IRule Clone(string json)
        {
            ConvertToCamelCaseData ruleData = (ConvertToCamelCaseData)JsonSerializer.Deserialize(json, typeof(ConvertToCamelCaseData));
            ConvertToCamelCase rule = new ConvertToCamelCase();
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

                var bases = original.Split(".");

                nonex += bases[0];
                for (int i = 1; i < bases.Length - 1; i++)
                    nonex += "." + bases[i];

                if (type == 1) ex = "." + bases[bases.Length - 1];
                if (type == 2 && bases.Length > 1) nonex += "." + bases[bases.Length - 1];

                string result = "";

                var tokens = nonex.Split(" ");

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (i == 0)
                    {
                        result += tokens[i].ToLower(new CultureInfo("vi-VI", false));
                    }
                    else
                    {
                        for (int j = 0; j < tokens[i].Length; j++)
                            if (j == 0)
                            {
                                result += char.ToUpper(tokens[i][j], new CultureInfo("vi-VI"));
                            }
                            else
                            {
                                result += char.ToLower(tokens[i][j], new CultureInfo("vi-VI"));
                            }
                    }
                }

                result = $"{result}{ex}";

                results.Add(result);
            }

            return results;
        }

        public string ToJson()
        {
            ConvertToCamelCaseData data = new ConvertToCamelCaseData();
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }
    }
}
