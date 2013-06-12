// 人工智慧作業三 龜兔賽跑程式之研發                
// 資工系103級 499470098 曹又霖
// 使用方法：                                       
// 使用Visual Studio 2012並灌入MonoGame後即可打開並編譯。
// 執行方法：
// 需先灌入.NET Framework 4 和 OpenAL 後，即可打開。
// 信箱：sinmaplewing@gmail.com

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using NAudio.Wave;

namespace HareTortoiseGame
{
    public static class LoadSong
    {
        public static string[] Songlist = { "25", "EmeraldHillClassic", "SunsetParkModern" };

        /*
        public static void SongInitialize()
        {
            foreach (var song in Songlist)
            {
                if (!File.Exists(@"Content/" + song + ".wav"))
                {
                    CompileSong(song);
                }
            }
        }

        */
        public static Song Load(Game game, string path)
        {
            /*
            if (!File.Exists(@"Content/" + path + ".wav"))
            {
                CompileSong(path);
            }
            */
            return game.Content.Load<Song>(path);
        }

        /*
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
        */ 
    }
}
