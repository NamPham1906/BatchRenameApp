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
using Contract;
namespace batchRenameApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int ruleAmount = 0;
        List<IRule> rules = new List<IRule>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ruleAmount = RuleFactory.GetInstance().RuleAmount();
            for(int i = 0; i < ruleAmount; i++)
            {
                rules.Add(RuleFactory.GetInstance().Create(i));
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if(ruleAmount > 0)
        //    {
        //        MyStackPanel.Children.Add(rules[0].GetUI());
        //    }
        //}
    }
}
