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

namespace AddAlphabetCounter
{
    /// <summary>
    /// Interaction logic for AddAlphabetRuleWindow.xaml
    /// </summary>
    public partial class AddAlphabetRuleWindow : UserControl
    {
        AddAlphabetCounterRule rule;
        public AddAlphabetRuleWindow(AddAlphabetCounterRule rule)
        {
            this.rule = rule;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }

        private void Prefix_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Position = 0;
        }

        private void Suffix_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Position = 1;
        }

        private void Lowercase_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Style = 0;
        }

        private void Uppercase_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Style = 1;
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
