using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace HareTortoiseGame
{
    public static class LoadSong
    {
        public static void SongInitialize()
        {
            string[] songlist = { "25", "EmeraldHillClassic", "SunsetParkModern" };
            foreach (var song in songlist)
            {
                if (!File.Exists(@"Content/" + song + ".wav"))
                {
                    CompileSong(song);
                }
            }
        }

        public static Song Load(Game game, string path)
        {
            if (!File.Exists(@"Content/" + path + ".wav"))
            {
                CompileSong(path);
            }
            return game.Content.Load<Song>(path);
        }

        public static void CompileSong(string path)
        {
            StreamReader inputStream = new StreamReader(@"Content/" + path + ".mp3");
            StreamWriter outputStream = new StreamWriter(@"Content/" + path + ".wav");

            using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(inputStream.BaseStream)))
            using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream.BaseStream, waveStream.WaveFormat))
            {
                byte[] bytes = new byte[waveStream.Length];
                waveStream.Read(bytes, 0, (int)waveStream.Length);
                waveFileWriter.Write(bytes, 0, bytes.Length);
                waveFileWriter.Flush();
            }
        }
    }
}
