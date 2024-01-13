using System;
using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace PocketWatcher.Patches
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch]
    public class ForestGiantPatch : MonoBehaviour
    {
        // Token: 0x06000006 RID: 6 RVA: 0x0000227C File Offset: 0x0000047C
        [HarmonyPatch(typeof(EnemyAI), "Start")]
        [HarmonyPostfix]
        public static void SummonMiller(EnemyAI __instance)
        {
            bool flag = !(__instance is ForestGiantAI);
            if (!flag)
            {
                ForestGiantAI forestGiantAI;
                try
                {
                    forestGiantAI = (ForestGiantAI)__instance;
                }
                catch (Exception ex)
                {
                    ForestGiantPatch.Logging.LogError("Couldn't cast EnemyAI instance to ForestGiantAI: " + ex.Message);
                    return;
                }
                System.Random random = new System.Random();
                int num = random.Next(100);
                bool flag2 = num >= Plugin.VarietyChance.Value;
                if (!flag2)
                {
                    ForestGiantPatch.scps.Add(forestGiantAI);
                    UnityEngine.Object.Destroy(forestGiantAI.transform.Find("FGiantModelContainer").Find("BodyLOD0").gameObject.GetComponent<SkinnedMeshRenderer>());
                    UnityEngine.Object.Destroy(forestGiantAI.transform.Find("FGiantModelContainer").Find("BodyLOD1").gameObject.GetComponent<SkinnedMeshRenderer>());
                    UnityEngine.Object.Destroy(forestGiantAI.transform.Find("FGiantModelContainer").Find("BodyLOD2").gameObject.GetComponent<SkinnedMeshRenderer>());
                    ForestGiantPatch.RaiseTheMiller(forestGiantAI);
                    ForestGiantPatch.Logging.LogInfo("PocketWatcher resources are loaded.");
                }
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000023C8 File Offset: 0x000005C8
        private static void RaiseTheMiller(ForestGiantAI parent)
        {
            bool flag = Plugin.MillerModel == null || !ForestGiantPatch.scps.Contains(parent);
            if (!flag)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Plugin.MillerModel);
                gameObject.transform.SetParent(parent.transform.Find("FGiantModelContainer"));
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }
        }

        // Token: 0x0400000E RID: 14
        private static HashSet<ForestGiantAI> scps = new HashSet<ForestGiantAI>();

        // Token: 0x0400000F RID: 15
        public static ManualLogSource Logging = Plugin.Logging;
    }
}
