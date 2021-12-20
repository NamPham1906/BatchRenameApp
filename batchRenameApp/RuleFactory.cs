using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Contract;
namespace batchRenameApp
{
    class RuleFactory
    {
        private List<IRule> _prototypes = null;
        private RuleFactory()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(exePath);
            var fis = new DirectoryInfo(folder).GetFiles("*.dll");

            foreach (var f in fis)
            {
                var assembly = Assembly.LoadFile(f.FullName);
                var types = assembly.GetTypes();

                foreach (var t in types)
                {
                    if (t.IsClass && typeof(IRule).IsAssignableFrom(t))
                    {
                        IRule c = (IRule)Activator.CreateInstance(t);
                        _prototypes.Add(c);
                    }
                }
            }
        }

        public IRule Create(int type)
        {
            return _prototypes[type].Clone();
        }
    }
}
