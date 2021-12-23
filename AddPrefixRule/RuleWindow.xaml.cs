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

namespace AddPrefixRule
{
    /// <summary>
    /// Interaction logic for RuleWindow.xaml
    /// </summary>
    public partial class RuleWindow : UserControl
    {
        AddPrefix rule;

        public RuleWindow(AddPrefix rule)
        {
            this.rule = rule;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }

        private void prefixInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.rule.Prefix = prefixInput.Text;
        }

        private void ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[\/:*?""<>|]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
