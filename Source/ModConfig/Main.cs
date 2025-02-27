using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KL.Utils;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using Game;
using ModConfig.Misc;
using ModConfig.UI;
using Game.Data;
using Game.Mods;
using System.IO;
using MessagePack;
namespace ModConfig
{
    public static class Main
    {
        public static Harmony harmony;
        public static Dictionary<ModInfo, Type> ModConfigsTypes = new Dictionary<ModInfo, Type>();
        public static Dictionary<ModInfo, Type> ConfigDataTypes = new Dictionary<ModInfo, Type>();
        public static Dictionary<ModInfo, ConfigData> ConfigFromMod = new Dictionary<ModInfo, ConfigData>();
        public static Dictionary<Assembly, ModInfo> AssemblyToModInfo = new Dictionary<Assembly, ModInfo>();
        [RuntimeInitializeOnLoadMethod]
        static void StaticConstructorOnStartup() 
        {
            CheckDirectories();
            LoadHarmony();
            Printer.Warn("Loaded Mod Configs");
        }

        static void LoadHarmony() 
        {
            harmony = new Harmony("Eragon.ModConfig");
            harmony.PatchAll();
        }

        static void CheckDirectories() 
        {
            if (!Directory.Exists(ModConfigManager.PathForModConfig))
            {
                Directory.CreateDirectory(ModConfigManager.PathForModConfig);
            }
        }
    }
}
