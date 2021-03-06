# FFPR-UltimaFont
A BepInEx plugin to allow the loading of arbitrary fonts and system fonts in the FINAL FANTASY PIXEL REMASTERS. 

System fonts will be loaded by default, while non-system fonts must be loaded in the form of an AssetBundle mod (like normal font mods for the PRs) placed within a specific directory.


# Installation:
1. Install a Bleeding-Edge IL2CPP build of BepInEx 6, which can be found [here](https://builds.bepis.io/projects/bepinex_be)
2. Drop the BepInEx folder from the mod into the game's main directory and merge folders if prompted
3. Place any further font mods (in .bundle form) in the "BepInEx/plugins/Ultima Font" folder. Make sure each bundle is renamed to be indicative of the mod in question (ie "font_en.bundle" to "FF6SNES.bundle"). The font will be referred to by the plugin under this name.
4. Optional: in order to change fonts while the game is running, [sinai-dev's BepInExConfigManager](https://github.com/sinai-dev/BepInExConfigManager) will need to be installed

# Font Indices
* Font1 - The main font used by the game for menus and UI
* Font2 - Unknown
* Font3 - The font used for the save select information (Time, Character Name, Location, etc)
* Font4 - Unknown
* Font5 - The font used for all numbers and strings related to numerical data
* Font6 - The font used for the keyboard control help
* Font7 - Unknown

# Credits:
* The BepInEx Official Discord Server - for answering all of my dumb Unity questions
