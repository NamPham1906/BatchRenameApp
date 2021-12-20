using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IRule
    {
        List<string> Rename(List<string> originals);
        IRule Clone();
        string Name();
    }
}
