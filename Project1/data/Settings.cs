using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Project1
{
    class Settings
    {
        private static string SETTINGS_DIRECTORY;
        private static string SETTINGS_FILE_PATH;

        private readonly int MAX_POINTS;

        private GraphicsDeviceManager graphics;
        private GameWindow window;

        public struct GameSettings
        {
            //Graphics
            public int w, h;
            public bool vsync;
            public bool fullscreen, borderless;

            //Sound
            public float masterVolume;
            public float musicVolume;
            public float ambientVolume;
            public float sfxVolume;
        };

        private GameSettings gameSettings;

        public Settings(GraphicsDeviceManager graphics, GameWindow window)
        {
            this.graphics = graphics;
            this.window = window;

            SETTINGS_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\MyGame";
            SETTINGS_FILE_PATH = SETTINGS_DIRECTORY + "\\settings.xml";

            MAX_POINTS = 4;

            bool pathExists = Directory.Exists(SETTINGS_DIRECTORY);

            if (!pathExists)
            {
                Directory.CreateDirectory(SETTINGS_DIRECTORY);
                CreateNewSettingFile();
            }
            else
            {
                if (File.Exists(SETTINGS_FILE_PATH))
                    LoadSettings();
                else
                    CreateNewSettingFile();
            }
        }

        private void CreateNewSettingFile()
        {
            GameSettings gs = new GameSettings();

            gs.w = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            gs.h = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

            gs.vsync = false;

            gs.fullscreen = true;
            gs.borderless = false;

            gs.masterVolume = 50;
            gs.musicVolume = 50;
            gs.ambientVolume = 50;
            gs.sfxVolume = 50;

            SaveSettings(gs);
            LoadSettings();
        }

        private void LoadSettings()
        {
            gameSettings = new GameSettings();
            XmlTextReader reader = new XmlTextReader(SETTINGS_FILE_PATH);

            int pointTest = 0;

            try
            {
                reader.Read();

                while (reader.Read())
                {
                    reader.MoveToElement();
                    if (reader.AttributeCount == 0)
                        continue;

                    switch (reader.Name)
                    {
                        case "Resolution":
                            pointTest++;
                            gameSettings.w = int.Parse(reader.GetAttribute(0));
                            gameSettings.h = int.Parse(reader.GetAttribute(1));

                            graphics.PreferredBackBufferWidth = gameSettings.w;
                            graphics.PreferredBackBufferHeight = gameSettings.h;
                            break;
                        case "Vsync":
                            pointTest++;
                            gameSettings.vsync = bool.Parse(reader.GetAttribute(0));

                            graphics.SynchronizeWithVerticalRetrace = gameSettings.vsync;
                            break;
                        case "Window":
                            pointTest++;
                            gameSettings.fullscreen = bool.Parse(reader.GetAttribute(0));
                            gameSettings.borderless = bool.Parse(reader.GetAttribute(1));

                            graphics.IsFullScreen = gameSettings.fullscreen;
                            window.IsBorderless = gameSettings.borderless;
                            break;
                        case "Sound":
                            pointTest++;
                            gameSettings.masterVolume = float.Parse(reader.GetAttribute(0));
                            gameSettings.musicVolume = float.Parse(reader.GetAttribute(1));
                            gameSettings.ambientVolume = float.Parse(reader.GetAttribute(2));
                            gameSettings.sfxVolume = float.Parse(reader.GetAttribute(3));
                            break;
                    }
                }

                if (pointTest != MAX_POINTS)
                {
                    reader.Close();
                    CreateNewSettingFile();
                }
                else
                    graphics.ApplyChanges();
            }
            catch
            {
                reader.Close();
                CreateNewSettingFile();
            }
        }

        private void SaveSettings(GameSettings gs)
        {
            //Create settings.xml file in SETTINGS_PATH
            XmlTextWriter writer = new XmlTextWriter(SETTINGS_FILE_PATH, null);
            
            //Make it show proper formatting (easier debugging)
            writer.Formatting = Formatting.Indented;

            //Start writing
            writer.WriteStartDocument();

            writer.WriteComment("Application settings for MyGame");

            writer.WriteStartElement("Settings");

            //Graphic settings
            writer.WriteStartElement("Graphics");

            writer.WriteStartElement("Resolution");
            writer.WriteAttributeString("w", gs.w.ToString());
            writer.WriteAttributeString("h", gs.h.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Vsync");
            writer.WriteAttributeString("value", gs.vsync.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Window");
            writer.WriteAttributeString("fullscreen", gs.fullscreen.ToString());
            writer.WriteAttributeString("borderless", gs.borderless.ToString());            
            writer.WriteEndElement();

            writer.WriteEndElement();

            //Sound settings
            writer.WriteStartElement("Sound");
            writer.WriteAttributeString("master", gs.masterVolume.ToString());
            writer.WriteAttributeString("music", gs.musicVolume.ToString());
            writer.WriteAttributeString("ambient", gs.ambientVolume.ToString());
            writer.WriteAttributeString("sfx", gs.sfxVolume.ToString());
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        public void SetResolution(int w, int h)
        {
            gameSettings.w = w;
            gameSettings.h = h;
        }

        public void SetVsync(bool vsync)
        {
            gameSettings.vsync = vsync;
        }

        public void SetWindow(bool fullscreen, bool borderless)
        {
            gameSettings.fullscreen = fullscreen;
            gameSettings.borderless = borderless;
        }

        public void SetMasterVolume(float volume)
        {
            gameSettings.masterVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            gameSettings.musicVolume = volume;
        }

        public void SetAmbientVolume(float volume)
        {
            gameSettings.ambientVolume = volume;
        }

        public void SetSfxVolume(float volume)
        {
            gameSettings.sfxVolume = volume;
        }
    }
}
