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
        public string Rename(string original)
        {
            string result = original;

            foreach (var needle in Needles)
            {
                result = result.Replace(needle, Replacer);
            }

            return result;
        }

        public IRule Clone()
        {
            return new ReplaceCharactersRule(Needles, Replacer);
        }

        public string Name()
        {
            return "Replace Characters Rule";
        }
    }
}
