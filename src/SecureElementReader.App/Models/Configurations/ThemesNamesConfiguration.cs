using SecureElementReader.App.Enums;
using System.Collections.Generic;

namespace SecureElementReader.App.Models.Configurations
{
    public class ThemesNamesConfiguration
    {
        public Dictionary<Theme, string> ThemeToResourceMapping { get; set; }
    }
}
