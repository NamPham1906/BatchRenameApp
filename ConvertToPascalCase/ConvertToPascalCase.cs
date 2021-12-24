using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Globalization;
using Contract;
using System.ComponentModel;
using System.Text.Json;

namespace ConvertToPascalCaseRule
{
    public class ConvertToPascalCaseData
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }

    public class ConvertToPascalCase : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }

        public RuleWindow ConfigurationUI { get; set; }

        public bool IsInUse { get; set; }

        public ConvertToPascalCase()
        {
            Name = "Convert To PascalCase";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public ConvertToPascalCase(bool isInUse)
        {
            Name = "Convert To PascalCase";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = isInUse;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new ConvertToPascalCase(IsInUse);
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
                if (type == 2 && bases.Length > 1) nonex = "." + bases[bases.Length - 1];

                string result = "";

                var tokens = nonex.Split(" ");

                for (int i = 0; i < tokens.Length; i++)
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

                result = $"{result}{ex}";

                results.Add(result);
            }

            return results;
        }

        public string ToJson()
        {
            ConvertToPascalCaseData data = new ConvertToPascalCaseData();
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            ConvertToPascalCaseData ruleData = (ConvertToPascalCaseData)JsonSerializer.Deserialize(json, typeof(ConvertToPascalCaseData));
            ConvertToPascalCase rule = new ConvertToPascalCase();
            rule.Name = ruleData.Name;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }
    }
}
