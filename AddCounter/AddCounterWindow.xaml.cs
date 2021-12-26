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
            if(startInput.Text!="")
            {
                this.rule.Start = int.Parse(startInput.Text);
            }
        }

        private void stepInputt_TextChange(object sender, TextChangedEventArgs e)
        {
            if (stepInput.Text != "")
            {
                this.rule.Step = int.Parse(stepInput.Text);
            }
        }

        private void digitsInput_TextChange(object sender, TextChangedEventArgs e)
        {
            if (digitsInput.Text != "")
            {
                this.rule.Digits = int.Parse(digitsInput.Text);
            }
        }

        private void NumberValidationTextBox1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBox2(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBox3(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Prefix_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Position = 0;
        }

        private void Suffix_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Position = 1;
        }

        private void separationInput_TextChange(object sender, TextChangedEventArgs e)
        {
            this.rule.Separation = separationInput.Text;
        }

        private void ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[\/:*?""<>|\\]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
