﻿using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
            
            //this.LoadViewFromUri("/AddAlphabetCounter;component/addalphabetrulewindow.xaml");
            //System.Windows.Application.LoadComponent(this, resourceLocater);

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

    public static class extension
    {
        public static void LoadViewFromUri(this UserControl userControl, string baseUri)
        {
            try
            {
                var resourceLocater = new Uri(baseUri, UriKind.Relative);
                var exprCa = (PackagePart)typeof(Application).GetMethod("GetResourceOrContentPart", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { resourceLocater });
                var stream = exprCa.GetStream();
                var uri = new Uri((Uri)typeof(BaseUriHelper).GetProperty("PackAppBaseUri", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null), resourceLocater);
                var parserContext = new ParserContext
                {
                    BaseUri = uri
                };
                typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { stream, parserContext, userControl, true });
            }
            catch (Exception)
            {
                //log
            }
        }
    }
}
