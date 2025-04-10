using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.Commands;
using Game.Data;
using MessagePack;
using ModConfig.Misc;

namespace ModConfig
{
    [MessagePackObject]
    public abstract class ConfigData
    {
        protected ConfigData() {}
        private static bool PrintedErrorMessage;
        public static ConfigData LoadConfig(string modId) 
        {
            ModInfo info = The.ModLoader.ModInfos.Values.Where(x => x.Id.Substring(0, x.Id.LastIndexOf("_") != -1 ? x.Id.LastIndexOf("_"): x.Id.Length) == modId).FirstOrDefault();
            if (info == null)
            {
                Printer.Error($"Failed to find mod with id {modId}");
                if (!PrintedErrorMessage)
                {
                    string message = $"Current mods loaded:";
                    foreach(ModInfo mod in The.ModLoader.ModInfos.Values) 
                    {
                        message += $"\n{mod.Id}";
                    }
                    Printer.Warn(message);
                    PrintedErrorMessage = true;
                }
                return null;
            }
            return LoadConfig(info);
        }

        public static ConfigData LoadConfig(ModInfo info)
        {
            if (!Main.ConfigDataTypes.ContainsKey(info))
            {
                Printer.Error($"Tried loading mod {info.Id}'s config, but it does not have any.");
                return null;
            }

            ConfigData config;

            if (Main.ConfigFromMod.TryGetValue(info, out ConfigData data))
            {
                config = data;
                return config;
            }

            string pathForConfig = Path.Combine(ModConfigManager.PathForModConfig, info.Id + ModConfigManager.extension);

            if (File.Exists(pathForConfig))
            {
                config = (ConfigData)MessagePackSerializer.Deserialize(
                    Main.ConfigDataTypes[info],
                    File.ReadAllBytes(pathForConfig));
            }
            else
            {
                config = (ConfigData)Activator.CreateInstance(Main.ConfigDataTypes[info], true);
                File.WriteAllBytes(pathForConfig, MessagePackSerializer.Serialize(Main.ConfigDataTypes[info], config));
            }
            Main.ConfigFromMod[info] = config;
            return config;
        }
    }
}
