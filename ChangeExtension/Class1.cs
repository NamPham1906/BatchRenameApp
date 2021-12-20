using System;
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
        public string Rename(string original)
        {
            string[] str = original.Split('.');
            str[1] = _extension;
            return $"{str[0]}.{str[1]}";
        }
        public IRule Clone()
        {
            return new ChangeExtension(_extension);
        }

        public string Name => "Change Extension";
    }
}
