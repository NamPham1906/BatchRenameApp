using System;
using System.Collections.Generic;
using System.Windows.Controls;
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
                            if (tokens[i][j] >= 97 && tokens[i][j] <= 122)
                            {
                                result += (char)(tokens[i][j] - 32);
                            }
                            else
                            {
                                result += tokens[i][j];
                            }
                        }
                        else
                        {
                            if (tokens[i][j] >= 65 && tokens[i][j] <= 90)
                            {
                                result += (char)(tokens[i][j] + 32);
                            }
                            else
                            {
                                result += tokens[i][j];
                            }
                        }
                }

                result = $"{result}{ex}";

                results.Add(result);
            }

            return results;
        }
    }
}
