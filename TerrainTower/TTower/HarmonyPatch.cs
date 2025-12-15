using System;

using HarmonyLib;

using Mafi;
using Mafi.Unity.Mine;

using TerrainTower.Extras;

#pragma warning disable

namespace TerrainTower.TTower
{
    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [HarmonyPatch]
    public class modPatches
    {
        private static TerrainTowerManager m_towerManager;
        private readonly Harmony m_harmony;

        private modPatches(TerrainTowerManager towerManager)
        {
            m_harmony = new Harmony("myPatch");
            m_harmony.PatchAll();
            Logger.Info("Harmony patches applied");
            m_towerManager = towerManager;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TowerAreasRenderer), "rendererLoadState")]
        private static void Postfix(TowerAreasRenderer __instance)
        {
            if (__instance is null) throw new ArgumentNullException(nameof(__instance));
            Logger.Info("TowerAreasRender rendererLoadState");
        }
    }
}