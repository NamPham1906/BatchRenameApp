using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Contract;

namespace AddPrefixRule
{
    public class AddPrefixRule : IRule
    {
        public string Prefix { get; set; }

        public AddPrefixRule()
        {
            this.Prefix = "";
        }

        public AddPrefixRule(string prefix)
        {
            this.Prefix = prefix;
        }

        public IRule Clone()
        {
            return new AddPrefixRule(Prefix);
        }

        public string Name()
        {
            return "Add Prefix Rule";
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

                result = $"{Prefix}{nonex}";

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
