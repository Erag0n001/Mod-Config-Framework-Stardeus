using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Data;
using Game.UI;
using ModConfig.Misc;
using ModConfig.Patches;
using UnityEngine;

namespace ModConfig.UI
{
    public abstract class ModConfigs : MonoBehaviour, IUIPanel
    {
        private void Awake() 
        {

        }
        private void Start() 
        {
            DoWindowContent();
        }

        public virtual void OnSave() 
        {
            Printer.Warn($"Saving {this.GetType()}");
            try
            {
                ModConfigManager.SaveConfigFromMod(this);
            }
            catch (Exception e)
            {
                Printer.Error($"Error while trying to save {this.GetType()}\n{e}");
            }
        }
        public abstract void DoWindowContent();

        public void SetActive(bool on)
        {
            base.gameObject.SetActive(on);
            if(!on)
                OnSave();
        }
    }
}
