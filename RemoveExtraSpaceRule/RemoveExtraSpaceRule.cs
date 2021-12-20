using System;
using Contract;
namespace RemoveExtraSpaceRule
{
    public class RemoveExtraSpaceRule : IRule
    {
        public RemoveExtraSpaceRule()
        {
            //do not thing
        }
        public string Rename(string original)
        {
            return original.Trim();
        }

        public IRule Clone()
        {
            return new RemoveExtraSpaceRule();
        }

        public string Name()
        {
            return "Remove Extra Space Rule";
        }
    }
}
