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
            this.rule.Start = Int32.Parse(startInput.Text);
        }

        private void stepInputt_TextChange(object sender, TextChangedEventArgs e)
        {
            this.rule.Step = Int32.Parse(stepInput.Text);
        }

        private void digitsInput_TextChange(object sender, TextChangedEventArgs e)
        {
            this.rule.Digits = Int32.Parse(digitsInput.Text);
        }
    }
}
