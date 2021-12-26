using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Contract;
using HandyControl.Data;

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

        public bool AddRuleFromDLL(string filepath)
        {
            IRule newRule = null;
            FileInfo file = new FileInfo(filepath);
            bool result = true;
            try
            {
                var assembly = Assembly.LoadFile(file.FullName);
                var types = assembly.GetTypes();

                foreach (var t in types)
                {
                    if (t.IsClass && typeof(IRule).IsAssignableFrom(t))
                    {
                        newRule = (IRule)Activator.CreateInstance(t);
                    }
                }
            }
            catch (Exception)
            {
                result = false;
                newRule = null;
            }
            if (result)
            {
                MyFile newfile = new MyFile(filepath);
                string exePath = Assembly.GetExecutingAssembly().Location;
                string folder = Path.GetDirectoryName(exePath);
                if (newfile.changeNameToFolder(folder + "\\DLL"))
                {
                    _prototypes.Add(newRule);
                }
                else
                {
                    MessageBoxResult msResult = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                    {
                        Message = $"This rule dll is already exits. Do you want to replace it?",
                        Caption = "Add new Rule",
                        Button = MessageBoxButton.YesNo,
                        IconBrushKey = ResourceToken.AccentBrush,
                        IconKey = ResourceToken.AskGeometry,
                        StyleKey = "MessageBoxCustom"
                    });
                    switch (msResult)
                    {
                        case MessageBoxResult.Yes:
                            File.Copy(filepath, folder + "\\DLL\\" + newfile.filename, true);
                            _prototypes = new List<IRule>();
                            var fis = new DirectoryInfo(folder + "\\DLL").GetFiles("*.dll");

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
                            return true;
                        case MessageBoxResult.No:
                            return false;
                        default:
                            return false;
                    }
                }
            }
            return result;
        }
    }
}
