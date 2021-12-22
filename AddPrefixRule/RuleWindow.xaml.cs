using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        AddPrefixRule rule;

        public RuleWindow(AddPrefixRule rule)
        {
            this.rule = rule;
            InitializeComponent();
        }

        private void prefixInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.rule.Prefix = prefixInput.Text;
        }
    }
}
