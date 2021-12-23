using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace batchRenameApp
{
    class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if (value.ToString().Contains("Error:"))
                {
                    return new SolidColorBrush(Colors.IndianRed);
                }
                else
                {
                    return new SolidColorBrush(Colors.Green);
                }
               
            } 

            if (value == null)
            {
                return new SolidColorBrush(Colors.Black);
            }
            throw new Exception("Invalid Value");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
