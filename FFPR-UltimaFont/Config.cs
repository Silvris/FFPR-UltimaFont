using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FFPR_UltimaFont
{
    public class Configuration
    {
        public static string UltimaFont = "Fonts";
        public static string FontDescription = "The name of the font to use for this value. If set to Default, the original font for that slot will be used.";
        public ConfigEntry<string> Font1 { get; set; }
        public ConfigEntry<string> Font2 { get; set; }
        public ConfigEntry<string> Font3 { get; set; }
        public ConfigEntry<string> Font4 { get; set; }
        public ConfigEntry<string> Font5 { get; set; }
        public ConfigEntry<string> Font6 { get; set; }
        public ConfigEntry<string> Font7 { get; set; }

        public Configuration(ConfigFile file, List<string> loadedFonts)
        {
            Font1 = file.Bind(new ConfigDefinition(UltimaFont, nameof(Font1)), "Default", new ConfigDescription(FontDescription, new AcceptableValueList<string>(loadedFonts.ToArray())));
            Font2 = file.Bind(new ConfigDefinition(UltimaFont, nameof(Font2)), "Default", new ConfigDescription(FontDescription, new AcceptableValueList<string>(loadedFonts.ToArray())));
            Font3 = file.Bind(new ConfigDefinition(UltimaFont, nameof(Font3)), "Default", new ConfigDescription(FontDescription, new AcceptableValueList<string>(loadedFonts.ToArray())));
            Font4 = file.Bind(new ConfigDefinition(UltimaFont, nameof(Font4)), "Default", new ConfigDescription(FontDescription, new AcceptableValueList<string>(loadedFonts.ToArray())));
            Font5 = file.Bind(new ConfigDefinition(UltimaFont, nameof(Font5)), "Default", new ConfigDescription(FontDescription, new AcceptableValueList<string>(loadedFonts.ToArray())));
            Font6 = file.Bind(new ConfigDefinition(UltimaFont, nameof(Font6)), "Default", new ConfigDescription(FontDescription, new AcceptableValueList<string>(loadedFonts.ToArray())));
            Font7 = file.Bind(new ConfigDefinition(UltimaFont, nameof(Font7)), "Default", new ConfigDescription(FontDescription, new AcceptableValueList<string>(loadedFonts.ToArray())));

            Font1.SettingChanged += RefreshFonts;
            Font2.SettingChanged += RefreshFonts;
            Font3.SettingChanged += RefreshFonts;
            Font4.SettingChanged += RefreshFonts;
            Font5.SettingChanged += RefreshFonts;
            Font6.SettingChanged += RefreshFonts;
            Font7.SettingChanged += RefreshFonts;
        }

        public List<string> Fonts => new List<string> { Font1.Value, Font2.Value, Font3.Value,Font4.Value,Font5.Value,Font6.Value,Font7.Value};

        private void RefreshFonts(object sender, EventArgs e)
        {
            ModComponent.Instance.RefreshFonts();
        }

    }
}
