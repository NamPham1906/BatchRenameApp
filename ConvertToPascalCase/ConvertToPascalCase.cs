using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Contract;

namespace ConvertToPascalCase
{
    public class ConvertToPascalCase : IRule
    {
        public ConvertToPascalCase()
        {
            //Do nothing
        }

        public IRule Clone()
        {
            return new ConvertToPascalCase();
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
            return "Convert To PascalCase";
        }

        public List<string> Rename(List<string> originals)
        {
            List<string> results = new List<string>();

            foreach (var original in originals)
            {
                string nonex = "";

                var bases = original.Split(".");

                nonex += bases[0];
                for (int i = 1; i < bases.Length - 1; i++)
                    nonex += "." + bases[i];

                string result = "";

                var tokens = nonex.Split(" ");

                for (int i = 0; i < tokens.Length; i++)
                {
                    for (int j = 0; j < tokens[i].Length; j++)
                        if (j == 0)
                        {
                            if (tokens[i][j] >= 97 && tokens[i][j] <= 122)
                            {
                                result += (char)(tokens[i][j] - 32);
                            }
                            else
                            {
                                result += tokens[i][j];
                            }
                        }
                        else
                        {
                            if (tokens[i][j] >= 65 && tokens[i][j] <= 90)
                            {
                                result += (char)(tokens[i][j] + 32);
                            }
                            else
                            {
                                result += tokens[i][j];
                            }
                        }
                }

                results.Add(result);
            }

            return results;
        }
    }
}
