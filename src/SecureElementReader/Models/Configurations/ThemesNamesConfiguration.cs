using SecureElementReader.Enums;
using System.Collections.Generic;

namespace SecureElementReader.Models.Configurations
{
    public class ThemesNamesConfiguration
    {
        public Dictionary<Theme, string> ThemeToResourceMapping { get; set; }
    }
}
