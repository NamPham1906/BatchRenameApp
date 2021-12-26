using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using Contract;

namespace AddCounter 
{
    public class AddCounterData
    {
        public int Start { get; set; }
        public int Step { get; set; }
        public int Digits { get; set; }
        public int Position { get; set; } // 0:prefix 1:suffix
        public string Separation { get; set; }
        public string Name { get; set; }

        public bool IsInUse { get; set; }
    }
    public class AddCounterRule: IRule, INotifyPropertyChanged
    {
        public int Start { get; set; }
        public int Step { get; set; }
        public int Digits { get; set; }
        public int Position { get; set; }
        public string Separation { get; set; }

        public AddCounterWindow ConfigurationUI { get; set; }
        public bool IsInUse { get; set; }
        public string Name { get; set; }
        public bool IsUse()
        {
            return IsInUse;
        }
        public void SetIsUse(bool use)
        {
            IsInUse = use;
        }
        public AddCounterRule()
        {
            Name = "Add Counter";
            IsInUse = false;
            Start = 1;
            Step = 1;
            Digits = 1;
            Position = 1;
            Separation = "";
            ConfigurationUI = new AddCounterWindow(this);
        }
        public AddCounterRule(int start, int step, int digits, int position, string separation, bool isInUse)
        {
            Name = "Add Counter";
            IsInUse = isInUse;
            Start = start;
            Step = step;
            Digits = digits;
            Position = position;
            IsInUse = isInUse;
            Separation = separation;
            ConfigurationUI = new AddCounterWindow(this);

        }

        public List<string> Rename(List<string> originals, int type)
        {
            List<string> result = new List<string>();
            int i = Start;
            switch (type)
            {
                case 1:
                    {
                        foreach (string item in originals)
                        {

                            string count = i.ToString().PadLeft(Digits, '0');
                            string[] str = item.Split('.');

                            int strlen = str.Length;
                            string extension = str[strlen - 1];

                            string temp = "";
                            for (int j = 0; j < strlen - 2; j++)
                            {
                                temp += str[j] + ".";
                            }
                            if(strlen>1)
                            {
                                temp += str[strlen - 2];
                            }    
                            
                            if(Position == 1)
                            {
                                string newfilename = $"{temp}{Separation}{count}.{extension}";
                                result.Add(newfilename);
                            }
                            else
                            {
                                string newfilename = $"{count}{Separation}{temp}.{extension}";
                                result.Add(newfilename);
                            }
                            i = i + Step;
                        }

                        return result;
                    }
                case 2:
                    {
                        foreach (string item in originals)
                        {
                            string count = i.ToString().PadLeft(Digits, '0');
                            if (Position == 1)
                            {
                                string newfilename = $"{item}{Separation}{count}";
                                result.Add(newfilename);
                            }
                            else
                            {
                                string newfilename = $"{count}{Separation}{item}";
                                result.Add(newfilename);
                            }
                            i = i + Step;
                        }

                        return result;
                    }
                default:
                    return originals;
            }
            
        }
        public string ToJson()
        {
            AddCounterData data = new AddCounterData();
            data.Start = this.Start;
            data.Step = this.Step;
            data.Digits = this.Digits;
            data.Name = this.Name;
            data.Separation = this.Separation;
            data.Position = this.Position;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            AddCounterData ruleData = (AddCounterData)JsonSerializer.Deserialize(json, typeof(AddCounterData));
            AddCounterRule rule = new AddCounterRule();
            rule.Name = ruleData.Name;
            rule.Start = ruleData.Start;
            rule.Step = ruleData.Step;
            rule.Digits = ruleData.Digits;
            rule.Position = ruleData.Position;
            rule.Separation = ruleData.Separation;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }

        public IRule Clone()
        {
            return new AddCounterRule(Start, Step, Digits, Position, Separation, IsInUse);
        }

        public UserControl GetUI()
        {
            return ConfigurationUI;
        }

        public string GetName()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
