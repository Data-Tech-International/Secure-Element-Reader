using System;
using System.IO;

namespace SecureElementReader
{
    public static class AppDataManager
    {
        public const string DefaultBaseDir = "SecureElementReader";
        public const string appSettingsFileName = "appsettings.json";
        public static string BaseDirPath { get; private set; }
        public static string appSettingsFilePath { get; set; }
        public static void Initialize()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string userProfilePath = Path.Combine(appDataPath, DefaultBaseDir);
            BaseDirPath = userProfilePath;
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
            string sourceAppFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appSettingsFileName);
            string destinationAppFile = Path.Combine(BaseDirPath, appSettingsFileName);
            if (!File.Exists(destinationAppFile))
            {
                File.Copy(sourceAppFile, destinationAppFile, true);
            }
        }
    }
}