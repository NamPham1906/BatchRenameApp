using System;
using System.Collections.Generic;
using System.IO;
using Contract;

namespace ChangeExtension
{
    public class ChangeExtension : IRule
    {
        private string _extension { get; set; }
        public ChangeExtension(string extension)
        {
            this._extension = extension;
        }

        public List<string> Rename(List<string> originals)
        {
            string newe = _extension;
            List<string> result = new List<string>();

            foreach (string item in originals)
            {
                string[] str = item.Split('.');
                int strlen = str.Length;

                string olde = str[strlen - 1];
                string[] str1 = item.Split($".{olde}");

                string newfilename = $"{str1[0]}.{newe}";
                result.Add(newfilename);
            }    
            
            return result;
        }

        public IRule Clone()
        {
            return new ChangeExtension(_extension);
        }

        public string Name() => "Change Extension";
    }
}
