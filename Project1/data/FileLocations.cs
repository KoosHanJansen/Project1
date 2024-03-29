﻿using System;
using System.IO;
using System.Linq;

namespace Project1
{
    public class FileLocations
    {
        public static string MYGAME_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\MyGame";        
        public static string SAVES_DIRECTORY = MYGAME_DIRECTORY + "\\Saves";

        public static string SETTINGS_FILE = MYGAME_DIRECTORY + "\\settings.xml";

        public static unsafe void Write<T>(T[,] source, Stream stream) where T : unmanaged
        {
            fixed (void* asd = source)
                stream.Write(new Span<byte>(asd, source.Length * sizeof(T)));
        }
        public static unsafe bool Read<T>(T[,] source, Stream stream) where T : unmanaged
        {
            fixed (void* asd = source)
                return stream.Read(new Span<byte>(asd, source.Length * sizeof(T))) != 0;
        }

        public static bool Exists(string path)
        {
            return Directory.Exists(path) | File.Exists(path);
        }

        public static void CreateDirectories()
        {
            Directory.CreateDirectory(SAVES_DIRECTORY);
        }

        public static string[] GetSaves()
        {
            string[] saves = Directory.GetDirectories(SAVES_DIRECTORY);

            for (int i = 0; i < saves.Length; i++)
                saves[i] = saves[i].Split('\\').Last();

            return saves;
        }
    }
}
