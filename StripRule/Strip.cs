using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace StripRule
{
    public class StripData
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
        public bool StripLetter { get; set; }
        public bool StripNumber { get; set; }
        public bool StripSymbol { get; set; }
        public bool StripCustom { get; set; }
        public string CustomString { get; set; }
    }
    public class Strip : IRule, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public bool IsInUse { get; set; }
        public bool StripLetter { get; set; }
        public bool StripNumber { get; set; }
        public bool StripSymbol { get; set; }
        public bool StripCustom { get; set; }
        public string CustomString { get; set; }
        public RuleWindow ConfigurationUI { get; set; }
        public string Letter { get; set; }
        public string Number { get; set; }
        public string Symbol { get; set; }
        public Strip()
        {
            Name = "Strip";
            Letter = "abcdefjhijklmnopqrstuvwxyz";
            Number = "1234567890";
            Symbol = "!@#$%^&*()[]~-_=+,?;`{}"; ;
            IsInUse = false;
            StripLetter = false;
            StripNumber = false;
            StripSymbol = false;
            StripCustom = false;
            ConfigurationUI = new RuleWindow(this);
            CustomString = "";
        }
        public IRule Clone()
        {
            return new Strip();
        }

        public IRule Clone(string json)
        {
            StripData ruleData = (StripData)JsonSerializer.Deserialize(json, typeof(StripData));
            Strip rule = new Strip();
            rule.Name = ruleData.Name;
            rule.IsInUse = ruleData.IsInUse;
            rule.StripLetter = ruleData.StripLetter;
            rule.StripNumber = ruleData.StripNumber;
            rule.StripSymbol = ruleData.StripSymbol;
            rule.StripCustom = ruleData.StripCustom;
            rule.CustomString = ruleData.CustomString;
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
                        if (StripLetter)
                        {
                            temp = Regex.Replace(temp, $@"[{Letter}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        if (StripNumber)
                        {
                            temp = Regex.Replace(temp, $@"[{Number}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        if (StripSymbol)
                        {
                            temp = Regex.Replace(temp, $@"[{Symbol}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        if (StripCustom)
                        {
                            temp = Regex.Replace(temp, $@"[{CustomString}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        result.Add(temp + "." + tokens[tokens.Length - 1]);
                    }
                    break;
                case 2:
                    foreach (var str in originals)
                    {

                        string temp = str;
                        if (StripLetter)
                        {
                            temp = Regex.Replace(temp, $@"[{Letter}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        if (StripNumber)
                        {
                            temp = Regex.Replace(temp, $@"[{Number}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        if (StripSymbol)
                        {
                            temp = Regex.Replace(temp, $@"[{Symbol}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        if (StripCustom)
                        {
                            temp = Regex.Replace(temp, $@"[{CustomString}]/gi", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        result.Add(str);
                    }
                    break;
            }
            return result;
        }

        public string ToJson()
        {
            StripData data = new StripData();
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            data.StripLetter = this.StripLetter;
            data.StripNumber = this.StripNumber;
            data.StripSymbol = this.StripSymbol;
            data.StripCustom = this.StripCustom;
            data.CustomString = this.CustomString;
            string json = JsonSerializer.Serialize(data);
            return json;
        }
    }
}
