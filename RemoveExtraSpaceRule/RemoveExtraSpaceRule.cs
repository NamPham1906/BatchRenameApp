using System;
namespace RemoveExtraSpaceRule
{
    public class RemoveExtraSpaceRule 
    {
        public RemoveExtraSpaceRule()
        {
            //do not thing
        }
        public string Rename(string original)
        {
            return original.Trim();
        }

       // public IRule Clone()
       // {
       //     return new RemoveExtraSpaceRule();
       // }

        public string Name()
        {
            return "Remove Extra Space Rule";
        }
    }
}
