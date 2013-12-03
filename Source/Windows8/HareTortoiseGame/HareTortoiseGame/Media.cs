using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace HareTortoiseGame
{
    public static class Media
    {
        static SoundEffect _se = null;
        static SoundEffectInstance _sei = null;

        public static void Play(SoundEffect se)
        {
            if (_se != se)
            {
                if (_sei != null)
                {
                    _sei.Stop();
                }
                _se = se;
                _sei = _se.CreateInstance();
                _sei.Volume = ((float)SettingParameters.MusicVolume) / 100f;
                _sei.IsLooped = true;
                _sei.Play();
            }
        }

        public static void SetVolume()
        {
            if (_sei != null) _sei.Volume = ((float)SettingParameters.MusicVolume) / 100f;
        }
    }
}
