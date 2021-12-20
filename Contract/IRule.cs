using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IRule
    {
        string Rename(string original);
        IRule Clone();
        string Name();
    }
}
