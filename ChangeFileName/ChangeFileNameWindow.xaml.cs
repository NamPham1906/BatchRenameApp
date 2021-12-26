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

namespace ChangeFileNameRule
{
    /// <summary>
    /// Interaction logic for ChangeFileNameWindow.xaml
    /// </summary>
    public partial class ChangeFileNameWindow : UserControl
    {
        ChangeFileName rule;
        public ChangeFileNameWindow(ChangeFileName rule)
        {
            this.rule = rule;
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = rule;
        }

        private void nameInput_TextChange(object sender, TextChangedEventArgs e)
        {
            this.rule.NewName = nameInput.Text;
        }

        private void ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[\/:*?""<>|\\]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
