using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Data;
using Game.UI;
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
            ModConfigManager.SaveConfigFromMod(this);
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
