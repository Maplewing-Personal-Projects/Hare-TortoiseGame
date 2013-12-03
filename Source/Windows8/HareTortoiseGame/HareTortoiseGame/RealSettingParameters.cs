using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HareTortoiseGame.Component;
using HareTortoiseGame.GameLogic;

namespace HareTortoiseGame
{
    public class RealSettingParameters : Common.BindableBase
    {
        #region Field
        int _maxPlyOrSecond = 12;
        Board.Player[] _players = { Board.Player.User, Board.Player.Computer };
        int _maxEdgeCount = 4;
        int _soundVolume = 100;
        int _musicVolume = 30;
        bool _gameWait = true;
        #endregion

        public RealSettingParameters()
        {
        }

        public int MaxPly { get { return _maxPlyOrSecond; } set { SetProperty(ref _maxPlyOrSecond, value); } }
        public Board.Player[] Players { get { return _players; } set { SetProperty(ref _players, value); } }
        public int MaxEdgeCount
        {
            get { return _maxEdgeCount; }
            set
            {
                SetProperty( ref _maxEdgeCount, value );

                BoardData.Row = new ulong[_maxEdgeCount];
                ulong row = 0;
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    row <<= 1;
                    row |= 1;
                }
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    BoardData.Row[i] = row;
                    row <<= _maxEdgeCount;
                }

                BoardData.Column = new ulong[_maxEdgeCount];
                ulong column = 0;
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    column <<= SettingParameters.MaxEdgeCount;
                    column |= 1;
                }
                for (int i = 0; i < _maxEdgeCount; ++i)
                {
                    BoardData.Column[i] = column;
                    column <<= 1;
                }
            }
        }
        public int SoundVolume { get { return _soundVolume; } set { SetProperty(ref _soundVolume, value); } }
        public int MusicVolume { get { return _musicVolume; } 
            set { 
                SetProperty(ref _musicVolume, value);
                Media.SetVolume();
            } 
        }
        public bool GameWait { get { return _gameWait; } set { SetProperty(ref _gameWait, value); } }
    }
}
