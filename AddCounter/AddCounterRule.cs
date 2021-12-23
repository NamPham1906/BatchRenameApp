using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using Contract;

namespace AddCounter 
{
    public class AddCounterRule: IRule, INotifyPropertyChanged
    {
        public int Start { get; set; }
        public int Step { get; set; }
        public int Digits { get; set; }
        public AddCounterWindow ConfigurationUI { get; set; }
        public bool IsInUse { get; set; }
        public string Name { get; set; }
        public bool IsUse()
        {
            return IsInUse;
        }
        public AddCounterRule()
        {
            Name = "Add Counter";
            IsInUse = false;
            Start = 1;
            Step = 1;
            Digits = 1;
            ConfigurationUI = new AddCounterWindow(this);
        }
        public AddCounterRule(int start, int step, int digits)
        {
            Name = "Add Counter";
            IsInUse = false;
            Start = start;
            Step = step;
            Digits = digits;
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
                            temp += str[strlen - 2];
                            string newfilename = $"{temp}{count}.{extension}";
                            result.Add(newfilename);
                            i = i + Step;
                        }

                        return result;
                    }
                case 2:
                    {
                        foreach (string item in originals)
                        {
                            string count = i.ToString().PadLeft(Digits, '0');
                            string newfilename = $"{item}{count}";
                            result.Add(newfilename);
                            i = i + Step;
                        }

                        return result;
                    }
                default:
                    return originals;
            }
            
        }

        public IRule Clone()
        {
            return new AddCounterRule();
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
