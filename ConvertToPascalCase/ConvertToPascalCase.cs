using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Globalization;
using Contract;

namespace ConvertToPascalCaseRule
{
    public class ConvertToPascalCase : IRule
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

        public IRule Clone()
        {
            return new ConvertToPascalCase();
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
                if (type == 2) nonex = "." + bases[bases.Length - 1];

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
    }
}
