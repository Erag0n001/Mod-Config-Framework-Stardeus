using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Game.Data;
using Game.Mods;
using Game.UI;
using HarmonyLib;
using ModConfig.Misc;

namespace ModConfig.Patches
{
    public static class ModLoaderPatch
    {
        [HarmonyPatch(typeof(ModLoader), "LoadDLLFile")]
        public static class LoadModPatch 
        {
            [HarmonyPrefix]
            public static void Prefix(string path, ModInfo info) 
            {
                Assembly assembly = Assembly.LoadFrom(path);
                ModConfigManager.GetConfigFromMod(assembly, info);
            }
        }
    }
}
