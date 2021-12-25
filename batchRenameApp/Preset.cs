using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace batchRenameApp
{
    public class Preset
    {
        public string PresetName { get; set; }
        public List<IRule> PresetRules { get; set; }
        public Preset()
        {
            PresetName = "";
            PresetRules = null;
        }
        public Preset(string name, List<IRule> rules)
        {
            PresetName = name;
            PresetRules = rules;
        }
    }
}
