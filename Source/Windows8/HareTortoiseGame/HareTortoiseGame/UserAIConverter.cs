using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using HareTortoiseGame.Component;

namespace HareTortoiseGame
{
    public class UserAIConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((Board.Player)value == Board.Player.Computer)
            {
                return 0;
            }
            else return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0)
            {
                return Board.Player.Computer;
            }
            else return Board.Player.User;
        }
    }
}
