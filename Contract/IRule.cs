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
        List<string> Rename(List<string> originals);
        IRule Clone();
        string Name();

        //Window GetWindow();
        UserControl GetUI();
      
    }
}
