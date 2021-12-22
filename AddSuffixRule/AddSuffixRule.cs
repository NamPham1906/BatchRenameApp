using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Contract;

namespace AddSuffixRule
{
    public class AddSuffixRule : IRule
    {
        public string Suffix { get; set; }

        public AddSuffixRule()
        {
            this.Suffix = "";
        }

        public AddSuffixRule(string suffix)
        {
            this.Suffix = suffix;
        }

        public IRule Clone()
        {
            return new AddSuffixRule(Suffix);
        }

        public string Name()
        {
            return "Add Suffix Rule";
        }

        public List<string> Rename(List<string> originals)
        {
            List<string> results = new List<string>();

            foreach (var original in originals)
            {
                string nonex = "";

                var tokens = original.Split(".");

                nonex += tokens[0];
                for (int i = 1; i < tokens.Length - 1; i++)
                    nonex += "." + tokens[i];

                string result = "";

                result = $"{nonex}{Suffix}";

                results.Add(result);
            }

            return results;
        }

        public UserControl GetUI()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
