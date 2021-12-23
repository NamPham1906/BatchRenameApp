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
    public class RuleFactory
    {
        private List<IRule> _prototypes = new List<IRule>();
        private static RuleFactory instance = null;
        private RuleFactory()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(exePath);
            var fis = new DirectoryInfo(folder+"\\DLL").GetFiles("*.dll");

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

        static public RuleFactory GetInstance()
        {
           if(instance == null)
            {
                instance = new RuleFactory();
            }
            return instance;
        }

        public IRule Create(int type)
        {
            return _prototypes[type].Clone();
        }

        public IRule Create(RuleContainer ruleContainer)
        {
            int totalRule = _prototypes.Count();
            IRule result = null;
            for(int i = 0; i < totalRule; i++)
            {
                if(_prototypes[i].GetName() == ruleContainer.Name)
                {
                    result = _prototypes[i].Clone(ruleContainer.Data);
                    break;
                }
            }
            return result;
        }

        public int RuleAmount()
        {
            return _prototypes.Count();
        }
    }
}
