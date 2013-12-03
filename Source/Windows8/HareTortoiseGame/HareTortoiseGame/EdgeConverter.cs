using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using HareTortoiseGame.Component;

namespace HareTortoiseGame
{
    public class EdgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value - 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)value + 4;
        }
    }
}
