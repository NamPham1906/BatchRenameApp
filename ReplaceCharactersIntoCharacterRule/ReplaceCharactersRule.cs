using System;
using System.Collections.Generic;
using Contract;

namespace ReplaceCharactersIntoCharacterRule
{
    public class ReplaceCharactersRule : IRule
    {
        public List<string> Needles { get; set; }
        public string Replacer { get; set; }

        public ReplaceCharactersRule()
        {
            this.Needles.Add("_");
            this.Needles.Add("-");
            this.Replacer = " ";
        }

        public ReplaceCharactersRule(List<string> needles, string replacer)
        {
            this.Needles = needles;
            this.Replacer = replacer;
        }
        
        public IRule Clone()
        {
            return new ReplaceCharactersRule(Needles, Replacer);
        }

        public string Name()
        {
            return "Replace Characters Rule";
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

                string result = nonex;

                foreach (var needle in Needles)
                {
                    result = result.Replace(needle, Replacer);
                }

                results.Add(result);
            }

            return results;
        }
    }
}
