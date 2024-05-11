using HarmonyLib;
using Mafi;
using Mafi.Unity.Mine;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Towers;
using System.Reflection;
using Mafi.Core.Entities;
using Mafi.Unity.Utils;
using Mafi.Core.Terrain;
using TerrainTower.Extras;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Class to add Terrain Tower Manager to the vanilla code for TowerAreasRenderer
    /// </summary>
    [GlobalDependency(RegistrationMode.AsSelf)]
    [HarmonyPatch]
    public class ModPatches
    {
        private readonly Harmony m_harmony;
        private static TerrainTowersManager m_towerManager;

        ModPatches(TerrainTowersManager UTowerManager)
        {
            m_harmony = new Harmony("myPatch");
            m_harmony.PatchAll();
            Logger.Info("Harmony patches applied");
            m_towerManager = UTowerManager;
        }

        static int m_cnt = 0;
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TowerAreasRenderer), "rendererLoadState")]
        static void Postfix(TowerAreasRenderer __instance)
        {
            Logger.Info($"{m_cnt++} TowerAreasRender rendererLoadState");
            IndexableEnumerator<TerrainTowerEntity> enumerator = m_towerManager.AllTowers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Logger.Info($"Adding UTower");
                IAreaManagingTower current = enumerator.Current;
                typeof(TowerAreasRenderer).GetMethod("addTowerOrUpdateArea", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, (new object[] { current }));
            }

            FieldInfo fo = typeof(TowerAreasRenderer).GetField("m_delayedProcessing", BindingFlags.NonPublic | BindingFlags.Instance);
            DelayedItemsProcessing<IAreaManagingTower> x = (DelayedItemsProcessing<IAreaManagingTower>)fo.GetValue(__instance);

            m_towerManager.OnAreaChange.AddNonSaveable(__instance, delegate (TerrainTowerEntity tower, RectangleTerrainArea2i oldArea)
            {
                Logger.Info("UTHarmony area update");
                x.AddOnSim(tower);
            });

            m_towerManager.OnTTAdded.AddNonSaveable(__instance, delegate (TerrainTowerEntity tower, EntityAddReason reason)
            {
                Logger.Info("UTHarmony tower add");
                x.AddOnSim(tower);
            });

            m_towerManager.OnTTRemoved.AddNonSaveable(__instance, delegate (TerrainTowerEntity tower, EntityRemoveReason reason)
            {
                Logger.Info("UTHarmony tower remove");
                x.RemoveOnSim(tower);
            });

        }
    }
}
