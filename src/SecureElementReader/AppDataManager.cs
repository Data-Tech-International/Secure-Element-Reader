using System;
using System.IO;

namespace SecureElementReader
{
    public static class AppDataManager
    {
        public const string DefaultBaseDir = "SecureElementReader";
        public const string AppSettingsFileName = "appsettings.json";
        public static string BaseDirPath { get; private set; }
        public static string AppSettingsFilePath { get; set; }
        public static void Initialize()
        {
            string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string UserProfilePath = Path.Combine(AppDataPath, DefaultBaseDir);
            BaseDirPath = UserProfilePath;
            BaseDirPath = Path.GetFullPath(BaseDirPath);
            SetupBasePaths();
            CreateDefaultSettings();
        }
        private static void SetupBasePaths()
        {
            Directory.CreateDirectory(BaseDirPath);
        }

        private static void CreateDefaultSettings()
        {
            string SourceAppFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppSettingsFileName);
            string DestinationAppFile = Path.Combine(BaseDirPath, AppSettingsFileName);
            if (!File.Exists(DestinationAppFile))
            {
                File.Copy(SourceAppFile, DestinationAppFile, true);
            }
        }
    }
}