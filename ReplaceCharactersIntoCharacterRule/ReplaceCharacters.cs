using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Controls;
using Contract;

namespace ReplaceCharactersRule
{
    public class ReplaceCharactersData
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
        public string Needle { get; set; }
        public string Replacer { get; set; }
    }

    public class ReplaceCharacters : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }

        public RuleWindow ConfigurationUI { get; set; }

        public bool IsInUse { get; set; }

        public string Needle { get; set; }

        public string Replacer { get; set; }

        public ReplaceCharacters()
        {
            this.Needle = "";
            this.Replacer = "";
            this.Name = "Replace Characters";
            this.IsInUse = false;
            this.ConfigurationUI = new RuleWindow(this);
        }

        public ReplaceCharacters(string needle, string replacer)
        {
            this.Needle = needle;
            this.Replacer = replacer;
            this.Name = "Replace Characters";
            this.IsInUse = false;
            this.ConfigurationUI = new RuleWindow(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IRule Clone()
        {
            return new ReplaceCharacters(Needle, Replacer);
        }

        public List<string> Rename(List<string> originals, int type)
        {
            if (Needle == "") return originals; 

            List<string> results = new List<string>();

            foreach (var original in originals)
            {
                string nonex = "";
                string ex = "";

                var tokens = original.Split(".");

                nonex += tokens[0];
                for (int i = 1; i < tokens.Length - 1; i++)
                    nonex += "." + tokens[i];

                if (type == 1) ex = "." + tokens[tokens.Length - 1];
                if (type == 2 && tokens.Length > 1) nonex = "." + tokens[tokens.Length - 1];

                string result = nonex;

                result = result.Replace(Needle, Replacer);
                
                result = $"{result}{ex}";

                results.Add(result);
            }

            return results;
        }

        public UserControl GetUI()
        {
            return ConfigurationUI;
        }

        public string GetName()
        {
            return Name;
        }

        public bool IsUse()
        {
            return IsInUse;
        }

        public string ToJson()
        {
            ReplaceCharactersData data = new ReplaceCharactersData();
            data.Needle = this.Needle;
            data.Replacer = this.Replacer;
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            ReplaceCharactersData ruleData = (ReplaceCharactersData)JsonSerializer.Deserialize(json, typeof(ReplaceCharactersData));
            ReplaceCharacters rule = new ReplaceCharacters();
            rule.Name = ruleData.Name;
            rule.Needle = ruleData.Needle;
            rule.Replacer = ruleData.Replacer;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }
    }
}
