using System;
using Windows.UI.Xaml.Data;

namespace HareTortoiseGame.Common
{
    /// <summary>
    /// 值轉換器，可將 true 轉譯成 false (或反過來)。
    /// </summary>
    public sealed class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }
}
