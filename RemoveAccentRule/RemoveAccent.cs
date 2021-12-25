using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;
using Contract;
namespace RemoveAccentRule
{
    public class RemoveAccentData
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
    }
    public class RemoveAccent : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
        public RuleWindow ConfigurationUI { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RemoveAccent()
        {
            Name = "Remove Accent";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = false;
        }

        public RemoveAccent(bool isInUse)
        {
            Name = "Remove Accent";
            ConfigurationUI = new RuleWindow(this);
            IsInUse = isInUse;
        }
        public void SetIsUse(bool use)
        {
            IsInUse = use;
        }
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public IRule Clone()
        {
            return new RemoveAccent(IsInUse);
        }

        public IRule Clone(string json)
        {
            RemoveAccent ruleData = (RemoveAccent)JsonSerializer.Deserialize(json, typeof(RemoveAccent));
            RemoveAccent rule = new RemoveAccent();
            rule.Name = ruleData.Name;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }

        public string GetName()
        {
            return Name;
        }

        public UserControl GetUI()
        {
            return ConfigurationUI;
        }

        public bool IsUse()
        {
            return IsInUse;
        }

        public List<string> Rename(List<string> originals, int type)
        {
            List<string> result = new List<string>();
            switch (type)
            {
                case 1:
                    foreach (var str in originals)
                    {
                        string[] tokens = str.Split(".");
                        string temp = "";
                        for (int i = 0; i < tokens.Length - 1; i++)
                        {
                            temp += tokens[i] + ".";
                        }
                        if (temp.Length >= 1)
                        {
                            temp = temp.Remove(temp.Length - 1);
                        }
                        temp = RemoveDiacritics(temp);
                        result.Add(temp + "." + tokens[tokens.Length - 1]);
                    }
                    break;
                case 2:
                    foreach (var str in originals)
                    {

                        string temp = str;
                        temp = RemoveDiacritics(temp);
                        result.Add(str);
                    }
                    break;
            }
            return result;
        }

        public string ToJson()
        {
            RemoveAccent data = new RemoveAccent();
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }
    }
}
