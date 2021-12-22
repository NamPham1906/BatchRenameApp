using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AddCounter
{
    /// <summary>
    /// Interaction logic for AddCounterWindow.xaml
    /// </summary>
    public partial class AddCounterWindow : UserControl
    {
        AddCounterRule rule;
        public AddCounterWindow(AddCounterRule rule)
        {
            this.rule = rule;
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }

        private void startInput_TextChange(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            if (startInput.Text == "")
                this.rule.Start = 0;
            else if (regex.IsMatch(startInput.Text))
            {
                this.rule.Start = Int32.Parse(startInput.Text);
            }
            else
            {
                this.rule.Start = 0;
                startInput.Text = "0";
            }
        }

        private void stepInputt_TextChange(object sender, TextChangedEventArgs e)
        {
            if (stepInput.Text == "")
                this.rule.Step = 0;
            else
            {
                Regex regex = new Regex("^[0-9]+$");
                if (regex.IsMatch(stepInput.Text))
                    this.rule.Step = Int32.Parse(stepInput.Text);

                else
                    this.rule.Step = 0;
            }
        }

        private void digitsInput_TextChange(object sender, TextChangedEventArgs e)
        {
            if (digitsInput.Text == "")
                this.rule.Digits = 0;
            else
            {
                Regex regex = new Regex("^[0-9]+$");
                if(regex.IsMatch(digitsInput.Text))
                {
                    this.rule.Digits = Int32.Parse(digitsInput.Text);
                }    
                    
                else
                    this.rule.Digits = 0;
            }
        }
    }
}
