using System;
using System.Collections.Generic;
using System.IO;
using Contract;

namespace AddCounter 
{
    public class AddCounter: IRule
    {
        public AddCounter()
        {
            //do nothing
        }

        public List<string> Rename(List<string> originals)
        {
            List<string> result = new List<string>();
            int i = 1;
            foreach (string item in originals)
            {
                string count = i.ToString();
                string[] str = item.Split('.');

                int strlen = str.Length;
                string extension = str[strlen - 1];
                string[] str1 = item.Split($".{extension}");

                string newfilename = $"{str1[0]}{count}.{extension}";
                result.Add(newfilename);
                i++;
            }

            return result;
        }

        public IRule Clone()
        {
            return new AddCounter();
        }

        public string Name() => "Add Counter";
    }

}
