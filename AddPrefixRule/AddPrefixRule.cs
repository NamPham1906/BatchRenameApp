using System;
using Contract;

namespace AddPrefixRule
{
    public class AddPrefixRule : IRule
    {
        public string Prefix { get; set; }

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

        public string Rename(string original)
        {
            string result = "";
            
            result = $"{Prefix}{original}";

            return result;
        }
    }
}
