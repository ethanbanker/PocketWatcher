using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace PocketWatcher
{
    // Token: 0x02000004 RID: 4
    [BepInPlugin("eth3l.PocketWatcher", "PocketWatcher", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        // Token: 0x06000003 RID: 3 RVA: 0x0000206C File Offset: 0x0000026C
        private void Awake()
        {
            Plugin.Logging = base.Logger;
            Plugin.PluginDirectory = base.Info.Location;
            Plugin.VarietyChance = base.Config.Bind<int>("Variety", "VarietyChance", 100, "Integer number from 0 to 100 inclusive. A chance in percent to spawn SCP-173 instead of regular Coilhead. Values below 100 allow regular Coilheads to spawn.");
            bool flag = Plugin.VarietyChance.Value < 0;
            if (flag)
            {
                Plugin.VarietyChance.Value = 0;
            }
            bool flag2 = Plugin.VarietyChance.Value > 100;
            if (flag2)
            {
                Plugin.VarietyChance.Value = 100;
            }
            this.LoadAssets();
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            Plugin.Logging.LogInfo("Plugin eth3l.PocketWatcher is loaded!");
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002158 File Offset: 0x00000358
        private void LoadAssets()
        {
            try
            {
                Plugin.Bundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Plugin.PluginDirectory), "modasset"));
            }
            catch (Exception ex)
            {
                Plugin.Logging.LogError("Couldn't load asset bundle: " + ex.Message);
                return;
            }
            try
            {
                Plugin.MillerModel = Plugin.Bundle.LoadAsset<GameObject>("assets/miller/MillerGiant.prefab");
                base.Logger.LogInfo("Successfully loaded assets.");
            }
            catch (Exception ex2)
            {
                base.Logger.LogError("Couldn't load assets: " + ex2.Message);
            }
        }

        // Token: 0x04000002 RID: 2
        public static ManualLogSource Logging;

        // Token: 0x04000003 RID: 3
        public static string PluginDirectory;

        // Token: 0x04000004 RID: 4
        public static ConfigEntry<int> VarietyChance;

        // Token: 0x04000009 RID: 9
        public static GameObject MillerModel;

        // Token: 0x0400000A RID: 10
        public static AssetBundle Bundle;
    }
}
