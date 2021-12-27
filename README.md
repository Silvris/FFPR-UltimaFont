# FFPR-UltimaFont
A BepInEx plugin to allow the loading of arbitrary fonts and system fonts in the FINAL FANTASY PIXEL REMASTERS. 

System fonts will be loaded by default, while non-system fonts must be loaded in the form of an AssetBundle mod (like normal font mods for the PRs) placed within a specific directory.


# Installation:
1. Install a Bleeding-Edge IL2CPP build of BepInEx 6, which can be found [here](https://builds.bepis.io/projects/bepinex_be)
2. Drop the BepInEx folder from the mod into the game's main directory and merge folders if prompted
3. Place any further font mods (in .bundle form) in the "BepInEx/plugins/Ultima Font" folder. Make sure each bundle is renamed to be indicative of the mod in question (ie "font_en.bundle" to "FF6SNES.bundle").
4. Optional: in order to change fonts while the game is running, [sinai-dev's BepInExConfigManager](https://github.com/sinai-dev/BepInExConfigManager) will need to be installed

# Credits:
* The BepInEx Official Discord Server - for answering all of my dumb Unity questions
