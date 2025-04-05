using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Game;
using Game.Commands;
using Game.Data;
using KL.Utils;
using MessagePack;
using ModConfig.Misc;
using ModConfig.UI;

namespace ModConfig
{
    public static class ModConfigManager
    {
        public static string PathForModConfig => Persist.PersistentPath("Config");
        public const string extension = ".config";
        private static Dictionary<Assembly, ModInfo> CachedModInfos = new Dictionary<Assembly, ModInfo>();

        public static void SaveConfigFromMod(ModConfigs config) 
        {
            if (config != null)
            {
                ModInfo mod = Main.ModConfigsTypes.Keys.Where(k => Main.ModConfigsTypes[k] == config.GetType()).FirstOrDefault();
                if (mod == null)
                {
                    Printer.Error($"Could not save config from {config.GetType().Name} because there are no attached ids to it.");
                    return;
                }
                string pathForConfig = Path.Combine(PathForModConfig, mod.Id + extension);
                ConfigData data;
                if(Main.ConfigFromMod.TryGetValue(mod, out ConfigData configData))
                {
                    data = configData; 
                }
                else 
                {
                    data = ConfigData.LoadConfig(mod.Id);
                }
                File.WriteAllBytes(pathForConfig, MessagePackSerializer.Serialize(Main.ConfigFromMod[mod].GetType(), Main.ConfigFromMod[mod]));
            }
        }

        public static void SaveConfigFromMod(ConfigData data) 
        {

        }
        public static void GetConfigFromMod(Assembly assembly, ModInfo mod) 
        {
            try
            {
                var typeOfConfigs = GetModsConfigsTypesFromAssembly(assembly);
                if (typeOfConfigs.Item1 == null || typeOfConfigs.Item2 == null)
                    return;
                Printer.Warn($"Found configs for {mod.Id}");
                Main.ModConfigsTypes.Add(mod, typeOfConfigs.Item1);
                Main.ConfigDataTypes.Add(mod, typeOfConfigs.Item2);
                Main.AssemblyToModInfo.Add(assembly, mod);

            }
            catch (Exception ex)
            {
                Printer.Error($"Error while loading config from {mod.Id}\n{ex}");
            }
        }

        private static (Type, Type) GetModsConfigsTypesFromAssembly(Assembly assembly) 
        {
            if (assembly == null) return (null, null);
            Type modConfigsType = null;
            Type configDataType = null;
            try
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(ModConfigs)))
                    {
                        modConfigsType = type;
                    }
                    else if (type.IsSubclassOf(typeof(ConfigData)))
                    {
                        configDataType = type;
                    }
                    if(configDataType != null && modConfigsType != null) 
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                { Printer.Error(ex); }
            }
            return (modConfigsType, configDataType);
        }
    }
}
