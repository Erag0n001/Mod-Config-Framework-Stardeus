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

        public static ConfigData LoadConfig(string modId) 
        {
            ModInfo info = The.ModLoader.ModInfos.Values.Where(x => x.Id == modId).FirstOrDefault();
            if (info == null)
            {
                Printer.Error($"Failed to find mod with id {modId}");
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
