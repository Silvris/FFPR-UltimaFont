using BepInEx.Logging;
using Il2CppSystem.Fade;
using Il2CppSystems.Encryption;
using Last.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Last.Management.FontManager;

namespace FFPR_UltimaFont
{
    public sealed class ModComponent : MonoBehaviour
    {
        public static ModComponent Instance { get; private set; }
        public static Configuration Config { get; set; }
        public static ManualLogSource Log { get; private set; }
        public static FontManager FontManager { get; set; }
        public static Font[] DefaultFonts { get; set; }
        private bool _IsInitialized = false;
        public static Dictionary<string,Font> LoadedFonts { get; set; }
        private Boolean _isDisabled;
        private static string[] FontPaths =
        {
            "assets/gameassets/common/data/font/ja/fot-newrodinpro-db.otf",
            "assets/gameassets/common/data/font/en/se-alpstn__.ttf",
            "assets/gameassets/common/data/font/es/se-alpstn__.ttf",
            "assets/gameassets/common/data/font/fr/se-alpstn__.ttf",
            "assets/gameassets/common/data/font/pt/se-alpstn__.ttf",
            "assets/gameassets/common/data/font/it/se-alpstn__.ttf",
            "assets/gameassets/common/data/font/zht/untitled.otf",
            "assets/gameassets/common/data/font/zhc/untitled.otf",
            "assets/gameassets/common/data/font/th/untitled.otf",
            "assets/gameassets/common/data/font/ru/untitled.otf",
            "assets/gameassets/common/data/font/ko/untitled.otf"
        };//Thai has an added English font, no clue how needed it is but I'm excluding it from this at least
        public ModComponent(IntPtr ptr) : base(ptr)
        {
        }
        public void Awake()
        {
            try
            {
                Log = BepInEx.Logging.Logger.CreateLogSource("FFPR_UltimaFont");
                Instance = this;
                LoadedFonts = new Dictionary<string, Font>();
                List<string> userFonts = GetFontList();
                DefaultFonts = new Font[7];
                Config = new Configuration(EntryPoint.Instance.Config,userFonts);
                Log.LogMessage($"[{nameof(ModComponent)}].{nameof(Awake)}: Processed successfully.");
            }
            catch (Exception ex)
            {
                _isDisabled = true;
                Log.LogError($"[{nameof(ModComponent)}].{nameof(Awake)}(): {ex}");
                throw;
            }

        }
        public void Update()
        {
            try
            {
                if (_isDisabled)
                {
                    return;
                }
                if(FontManager is null)
                {
                    FontManager = FontManager.instance;
                    return;
                }
                if (!_IsInitialized)
                {
                    if (!DefaultsValid())
                    {
                        StoreDefaults();
                        return;
                    }
                    else
                    {
                        RefreshFonts();
                        _IsInitialized = true;
                    }

                }
            }
            catch (Exception ex)
            {
                _isDisabled = true;
                Log.LogError($"[{nameof(ModComponent)}].{nameof(Update)}(): {ex}");
                throw;
            }

        }
        public void StoreDefaults()
        {
            if (FontManager != null)
            {
                if (FontManager.initialized)
                {
                    foreach (FontParameter fp in FontManager.fontParams)
                    {
                        if (fp.FontInstance != null)
                        {
                            switch (fp.FontType)
                            {
                                case FontType.Font01:
                                    DefaultFonts[0] = fp.FontInstance;
                                    break;
                                case FontType.Font02:
                                    DefaultFonts[1] = fp.FontInstance;
                                    break;
                                case FontType.Font03:
                                    DefaultFonts[2] = fp.FontInstance;
                                    break;
                                case FontType.Font04:
                                    DefaultFonts[3] = fp.FontInstance;
                                    break;
                                case FontType.Font05:
                                    DefaultFonts[4] = fp.FontInstance;
                                    break;
                                case FontType.Font06:
                                    DefaultFonts[5] = fp.FontInstance;
                                    break;
                                case FontType.Font07:
                                    DefaultFonts[6] = fp.FontInstance;
                                    break;
                            }
                        }
                    }
                }
                
            }
        }
        public Font GetNearestNonNullDefault(int index)
        {
            Font rFont = DefaultFonts[index];
            while(rFont == null)
            {
                index++;
                if(index > 6)
                {
                    index = 0;
                }
                rFont = DefaultFonts[index];
            }
            return rFont;
        }
        public bool DefaultsValid()
        {
            //this one is *kinda* weird now, since all 7 fonts will not always be instantiated
            for(int i = 0; i < 7; i++)
            {
                if(DefaultFonts[i] != null)
                {
                    return true;
                }
            }
            return false;
        }
        public void RefreshFonts()
        {
            if(FontManager != null)
            {
                for(int i = 0; i < FontManager.fontParams.Count;i++)
                {
                    FontParameter fp = FontManager.fontParams[i];
                    int type = (int)fp.FontType;
                    if(type < 7) //7 is maximum, and stores the default font (Arial)
                    {
                        try
                        {
                            if (Config.Fonts[type] == "Default")
                            {
                                fp.FontInstance = DefaultFonts[type];
                            }
                            if (Config.Fonts[type] == "Random")
                            {
                                fp.FontInstance = LoadedFonts[LoadedFonts.Keys.ToList()[UnityEngine.Random.Range(0, LoadedFonts.Count - 1)]];
                            }
                            else
                            {
                                fp.FontInstance = LoadedFonts[Config.Fonts[type]];
                            }
                        }
                        catch(Exception ex)
                        {
                            Log.LogInfo($"Error occurred while setting Font0{type + 1}:{ex}");
                            fp.FontInstance = DefaultFonts[type];
                        }
                    }

                }
            }
        }
        public static Font LoadFontFromAssetBundle(string assetBundlePath)
        {
            try
            {
                byte[] encBytes = File.ReadAllBytes(assetBundlePath);
                byte[] decBytes = RijndaelUtil.Decrypt(encBytes, "TKX73OHHK1qMonoICbpVT0hIDGe7SkW0", "71Ba2p0ULBGaE6oJ7TjCqwsls1jBKmRL", 10, 256, 256);
                AssetBundle bundle = AssetBundle.LoadFromMemory(decBytes);
                while(bundle == null)
                {
                    bundle = AssetBundle.LoadFromMemory(decBytes);
                }
                foreach (string fontpath in FontPaths)
                {
                    Font nFont = bundle.LoadAsset<Font>(fontpath);
                    if (nFont != null)
                    {
                        Font rFont = Instantiate(nFont);
                        bundle.Unload(false);
                        return rFont;
                    }
                }
                return new Font();
            }
            catch(Exception ex)
            {
                Log.LogError($"Error occurred loading asset bundle {assetBundlePath}: {ex}");
                return new Font();
            }
        }
        public static List<string> GetFontList()
        {
            List<string> userFonts = new List<string>();
            userFonts.Add("Default");//not a real font, just a way to set the default font
            userFonts.Add("Random");
            string bepPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach(string file in Directory.GetFiles(bepPath,"*.bundle"))//hey, this should work at least
            {
                string FontName = Path.GetFileNameWithoutExtension(file);
                Font nFont = LoadFontFromAssetBundle(file);
                nFont.name = FontName;
                LoadedFonts.Add(FontName, nFont);
                userFonts.Add(FontName);
            }
            foreach(string font in Font.GetOSInstalledFontNames())
            {
                Font oFont = Font.CreateDynamicFontFromOSFont(font, 16);
                LoadedFonts.Add(font, oFont);
                userFonts.Add(font);
            }
            return userFonts;
        }
    }
}
