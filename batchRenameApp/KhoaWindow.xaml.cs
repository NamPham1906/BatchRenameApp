using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
    /// Interaction logic for KhoaWindow.xaml
    /// </summary>
    public class MyPikaFile : INotifyPropertyChanged
    {
        public MyPikaFile()
        {
            Name = "   Pikachu   .txt";
        }
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public partial class KhoaWindow : Window
    {
        int totalRule = 0;
        List<IRule> allRules = new List<IRule>();
        BindingList<IRule> userRules = new BindingList<IRule>();
        List<UserControl> userControls = new List<UserControl>();
        MyPikaFile testingFile = new MyPikaFile();
        string backupName = "    Pikachu   .txt";
        public KhoaWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //get all rule from DLL
            totalRule = RuleFactory.GetInstance().RuleAmount();
            for (int i = 0; i < totalRule; i++)
            {
                allRules.Add(RuleFactory.GetInstance().Create(i));
                userControls.Add(allRules[i].GetUI());
                userRules.Add(allRules[i]);
            }
            userRules.Add(RuleFactory.GetInstance().Create(0));
            RuleList.ItemsSource = userRules;
            this.DataContext = testingFile;
        }


        private void RuleList_LayoutUpdated(object sender, EventArgs e)
        {
            testingFile.Name = backupName;
            for (int i = 0; i < userRules.Count(); i++)
            {
                if (userRules[i].IsUse())
                {
                    List<string> temp = userRules[i].Rename(new List<string> { backupName });
                    string newName = temp[0];
                    testingFile.Name = newName;
                }
            }
        }

        private void RuleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = (ListView)sender;
            if (listView.SelectedItems.Count > 1 )
            {
                IRule rule = (IRule)listView.SelectedItems[listView.SelectedItems.Count - 1];
                listView.UnselectAll();
                listView.SelectedItems.Add(rule);
            }


        }

        private void Remove_Rule_Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            IRule rule = b.CommandParameter as IRule;
            userRules.Remove(rule);
        }
    }
}
