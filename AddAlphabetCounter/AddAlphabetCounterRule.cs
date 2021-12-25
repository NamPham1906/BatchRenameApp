using Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace AddAlphabetCounter
{
    public class AddAlphabetCounterData
    {
        public int Position { get; set; }
        public int Style { get; set; }
        public string Name { get; set; }
        public string Separation { get; set; }
        public bool IsInUse { get; set; }
    }
    public class AddAlphabetCounterRule : IRule, INotifyPropertyChanged
    {
        public int Position { get; set; } //0:prefix 1:suffix
        public int Style { get; set; } //0:lowercase 1:upppercase
        public string Separation { get; set; }
        public AddAlphabetRuleWindow ConfigurationUI { get; set; }
        public bool IsInUse { get; set; }
        public string Name { get; set; }
        public bool IsUse()
        {
            return IsInUse;
        }
        public AddAlphabetCounterRule()
        {
            Name = "Add Alphabet Counter";
            IsInUse = false;
            Position = 1;
            Style = 0;
            Separation = "";
            ConfigurationUI = new AddAlphabetRuleWindow(this);
        }
        public AddAlphabetCounterRule(int position, int style, string separation, bool isInUse)
        {
            Name = "Add Alphabet Counter";
            IsInUse = isInUse;
            Position = position;
            Style = style;
            Separation = separation;
            ConfigurationUI = new AddAlphabetRuleWindow(this);

        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public string intToChar(int number, int style)
        {
            
            string result = "";
            char getChar;
            if (style == 0)
            {
                while (number != 0)
                {
                    int temp = number % 27;
                    if (temp == 0)
                    {
                        result = "";
                        return result;
                    }
                    else
                    {
                        getChar = (char)(temp + 96);
                        result += $"{getChar}";
                        number = number / 27;
                    }
                }
            }
            else
            {
                while (number != 0)
                {
                    int temp = number % 27;
                    if (temp == 0)
                    {
                        result = "";
                        return result;
                    }
                    else
                    {
                        getChar = (char)(temp + 64);
                        result += $"{getChar}";
                        number = number / 27;
                    }
                }
            }    
               
            return result;
        }

        public List<string> alphabetCounterString(int total, int style)
        {
            List<string> alphabet = new List<string>();
            for (int i = 1; i <= total; i++)
            {
                string result = intToChar(i, style);
                result = Reverse(result);
                if (result != "")
                {
                    alphabet.Add(result);
                }
                else
                {
                    total++;
                }
            }
            return alphabet;
        }

        public List<string> Rename(List<string> originals, int type)
        {
            List<string> result = new List<string>();
            int totalFile = originals.Count;
            List<string> alphabet = alphabetCounterString(totalFile, Style);
            if(totalFile == 0)
            {
                return originals;
            }    
            switch (type)
            {
                case 1:
                    {
                        int i = 0;
                        foreach (string item in originals)
                        {

                            string[] str = item.Split('.');

                            int strlen = str.Length;
                            string extension = str[strlen - 1];

                            string temp = "";
                            
                            for (int j = 0; j < strlen - 2; j++)
                            {
                                temp += str[j] + ".";
                            }
                            if(strlen > 1)
                            {
                                temp += str[strlen - 2];
                            }

                            if (Position == 1)
                            {
                                string newfilename = $"{temp}{Separation}{alphabet[i]}.{extension}";
                                result.Add(newfilename);
                            }
                            else
                            {
                                string newfilename = $"{alphabet[i]}{Separation}{temp}.{extension}";
                                result.Add(newfilename);
                            }

                            i++;
                        }

                        return result;
                    }
                case 2:
                    {
                        int i = 0;
                        foreach (string item in originals)
                        {
                            if (Position == 1)
                            {
                                string newfilename = $"{item}{Separation}{alphabet[i++]}";
                                result.Add(newfilename);
                            }
                            else
                            {
                                string newfilename = $"{alphabet[i++]}{Separation}{item}";
                                result.Add(newfilename);
                            }
                        }
                        return result;
                    }
                default:
                    return originals;
            }
        }

        public string ToJson()
        {
            AddAlphabetCounterData data = new AddAlphabetCounterData();
            data.Position = this.Position;
            data.Style = this.Style;
            data.Separation = this.Separation;
            data.Name = this.Name;
            data.IsInUse = this.IsInUse;
            string json = JsonSerializer.Serialize(data);
            return json;
        }

        public IRule Clone(string json)
        {
            AddAlphabetCounterData ruleData = (AddAlphabetCounterData)JsonSerializer.Deserialize(json, typeof(AddAlphabetCounterData));
            AddAlphabetCounterRule rule = new AddAlphabetCounterRule();
            rule.Name = ruleData.Name;
            rule.Position = ruleData.Position;
            rule.Style = ruleData.Style;
            rule.Separation = ruleData.Separation;
            rule.IsInUse = ruleData.IsInUse;

            return rule;
        }

        public IRule Clone()
        {
            return new AddAlphabetCounterRule(Position, Style, Separation, IsInUse);
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
