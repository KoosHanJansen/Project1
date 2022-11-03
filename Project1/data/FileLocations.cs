using System;
using System.IO;

namespace Project1
{
    public class FileLocations
    {
        public static string MYGAME_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\MyGame";        
        public static string SAVES_DIRECTORY = MYGAME_DIRECTORY + "\\Saves";

        public static string SETTINGS_FILE = MYGAME_DIRECTORY + "\\settings.xml";

        public static bool Exists(string path)
        {
            return Directory.Exists(path) | File.Exists(path);
        }

        public static void CreateDirectories()
        {
            Directory.CreateDirectory(SAVES_DIRECTORY);
        }
    }
}
