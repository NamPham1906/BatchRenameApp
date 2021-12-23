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

namespace ReplaceCharactersRule
{
    /// <summary>
    /// Interaction logic for RuleWindow.xaml
    /// </summary>
    public partial class RuleWindow : UserControl
    {
        ReplaceCharacters rule;

        public RuleWindow(ReplaceCharacters rule)
        {
            this.rule = rule;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }

        private void needleInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.rule.Needle = needleInput.Text;
        }

        private void replacerInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.rule.Replacer = replacerInput.Text;
        }

        private void ValidationTextBox1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[\/:*?""<>|]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ValidationTextBox2(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[\/:*?""<>|]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
