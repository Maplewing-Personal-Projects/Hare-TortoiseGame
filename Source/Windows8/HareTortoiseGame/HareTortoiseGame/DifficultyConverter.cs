using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace HareTortoiseGame
{
    public class DifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 3)
            {
                return 0;
            }
            else if ((int)value == 12)
            {
                return 1;
            }
            else if ((int)value == 16)
            {
                return 2;
            }
            else return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0)
            {
                return 3;
            }
            else if ((int)value == 1)
            {
                return 12;
            }
            else if ((int)value == 2)
            {
                return 16;
            }
            else return 12;
        }
    }
}
