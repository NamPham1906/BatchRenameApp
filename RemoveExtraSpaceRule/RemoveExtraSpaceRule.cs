using System;
using System.Windows;
using System.Windows.Controls;
using Contract;
namespace RemoveExtraSpaceRule
{
    public class RemoveExtraSpaceRule 
    {
        public int Configuration { get; set; }
        public RuleWindow ConfigurationUI { get; set; }
        public RemoveExtraSpaceRule()
        {
            
            Configuration = 1;
            ConfigurationUI = new RuleWindow(this);
        }
        public RemoveExtraSpaceRule(int configuration = 1)
        {
          
            Configuration = configuration;
            ConfigurationUI = new RuleWindow(this);
        }
        public string Rename(string original)
        {
           
            int index = original.LastIndexOf(".");
            string[] tokens = original.Split(".");
            string result = "";
            for(int i = 0; i < tokens.Length-1; i++)
            {
                result += tokens[i] + ".";
            }
            result = result.Remove(result.Length-1);
            switch (Configuration)
            {
                case -1:
                    result = result.TrimStart();
                    break;
                case 0:
                    result = result.TrimEnd();
                    break;
                case 1:
                    result = result.Trim();
                    break;
            }
            return result+"."+ tokens[tokens.Length-1];
        }

        public IRule Clone()
        {
            return new RemoveExtraSpaceRule(1);
        }

        

        public string Name()
        {
            return "Remove Extra Space";
        }

        public UserControl GetUI()
        {
            return ConfigurationUI;
        }
    }
}
