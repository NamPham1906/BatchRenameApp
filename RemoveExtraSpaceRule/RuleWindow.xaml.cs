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

namespace RemoveExtraSpaceRule
{
    /// <summary>
    /// Interaction logic for RuleWindow.xaml
    /// </summary>
    public partial class RuleWindow : UserControl
    {
        RemoveExtraSpace rule;
        public RuleWindow(RemoveExtraSpace rule)
        {
            this.rule = rule;
            InitializeComponent();

        }

        private void LeadingRadio_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Configuration = -1;
        }

        private void TrailingRadio_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Configuration = 0;
        }

        private void BothRadio_Checked(object sender, RoutedEventArgs e)
        {
            this.rule.Configuration = 1;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }
    }
}
