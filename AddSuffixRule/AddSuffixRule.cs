using System;
using Contract;

namespace AddSuffixRule
{
    public class AddSuffixRule : IRule
    {
        public string Suffix { get; set; }

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

        public string Rename(string original)
        {
            string result = "";

            result = $"{original}{Suffix}";

            return result;
        }
    }
}
