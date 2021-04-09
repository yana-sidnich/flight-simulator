using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace FlightGearTestExec.Converters
{
    class TimeConverter : IValueConverter
    {
        const int TICKS_PER_SEC = 10;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int x = (int)((int)value / 10);
            if (x % 60 >= 10)
            {
                return "" + x / 60 + ":" + x % 60;
            }
            else
            {
                return "" + x / 60 + ":0" + x % 60;
            }

        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
