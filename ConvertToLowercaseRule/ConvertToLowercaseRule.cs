using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Contract;

namespace ConvertToLowercaseRule
{
    public class ConvertToLowercaseRule : IRule
    {
        public ConvertToLowercaseRule()
        {
            //Do nothing
        }

        public IRule Clone()
        {
            return new ConvertToLowercaseRule();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public UserControl GetUI()
        {
            throw new NotImplementedException();
        }

        public string Name()
        {
            return "Convert To Lowercase Rule";
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

                string temp = nonex;

                temp = temp.Replace(" ", "");

                string result = "";

                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] >= 65 && temp[i] <= 90)
                        result += (char)(temp[i] + 32);
                    else
                        result += temp[i];
                }

                results.Add(result);
            }

            return results;
        }
    }
}
