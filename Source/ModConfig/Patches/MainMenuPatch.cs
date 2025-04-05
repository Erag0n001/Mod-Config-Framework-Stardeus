﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.Data;
using Game.UI;
using HarmonyLib;
using ModConfig.Misc;
using ModConfig.UI;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace ModConfig.Patches
{
    [HarmonyPatch(typeof(MainMenu), "ShowMainMenu")]
    public static class MainMenuPatch
    {
        public static FieldInfo currentButtons;
        private static MethodInfo CreateButton;
        private static MethodInfo ShowMainMenu;
        private static bool patching;
        static MainMenuPatch()
        {
            currentButtons = AccessTools.Field(typeof(MainMenu), "currentButtons");
            CreateButton = AccessTools.Method(typeof(MainMenu), "CreateButton");
            ShowMainMenu = AccessTools.Method(typeof(MainMenu), "ShowMainMenu");
            
        }

        [HarmonyPostfix]
        public static void Postfix(MainMenu __instance)
        {
            if (patching)
                return;
            Printer.Warn("Patching main menu");
            List<MainMenuButton> buttons = (List<MainMenuButton>)currentButtons.GetValue(__instance);
            if (buttons == null)
                return;
            buttons.Insert(5, CreateConfigButton(__instance));
            patching = true;
            ShowMainMenu.Invoke(__instance, null);
            patching = false;
        }

        private static MainMenuButton CreateConfigButton(MainMenu __instance)
        {
            MainMenuButton button = (MainMenuButton)CreateButton.Invoke(__instance, new object[] { "Mod configs", null, null });
            foreach (KeyValuePair<ModInfo, Type> config in Main.ModConfigsTypes)
            {
                button.AddSubmenuItem(CreateConfigSubMenu(__instance, config.Key, config.Value));
            }
            return button;
        }

        private static MainMenuButton CreateConfigSubMenu(MainMenu __instance, ModInfo info, Type panelType) 
        {
            Action<MainMenuButton> action = delegate { 
                CreatePanelFromConfig(panelType); 
            };
            MainMenuButton button = (MainMenuButton)CreateButton.Invoke(__instance, new object[]
            {
                info.Name,
                action,
                null
            });
            return button;
        }

        private static void CreatePanelFromConfig(Type config) 
        {
            The.SysSig.ShowPanel.Send(new PanelDescriptor(config, withCloseButton: true, skipInGame: true));
        }
    }
}
