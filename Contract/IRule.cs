using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Contract
{
    public interface IRule
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }

        List<string> Rename(List<string> originals, int type);
        IRule Clone();

        UserControl GetUI();

        string GetName();

        bool IsUse();

        string ToJson();

        void SetIsUse(bool use);
        IRule Clone(string json);

    }
}
