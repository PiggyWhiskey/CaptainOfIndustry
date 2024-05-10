// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TreeMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TreeMbFactory : IFactory<TreeData, TreeMb>
  {
    private readonly ITreesManager m_treeManager;
    private readonly TerrainManager m_terrainManager;
    private readonly AssetsDb m_assetsDb;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ICalendar m_calendar;
    private readonly IRandom m_random;

    public TreeMbFactory(
      ITreesManager treeManager,
      TerrainManager terrainManager,
      AssetsDb assetsDb,
      RandomProvider randomProvider,
      IGameLoopEvents gameLoopEvents,
      ICalendar calendar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeManager = treeManager;
      this.m_terrainManager = terrainManager;
      this.m_assetsDb = assetsDb;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_calendar = calendar;
      this.m_random = randomProvider.GetNonSimRandomFor((object) this);
    }

    public TreeMb Create(TreeData tree)
    {
      Pair<string, string> prefabPath = tree.Proto.TreeGraphics.PrefabPaths[this.m_random];
      TreeMb treeMb = this.m_assetsDb.GetClonedPrefabOrEmptyGo(prefabPath.First).AddComponent<TreeMb>();
      treeMb.Initialize(tree, this.m_terrainManager[(Tile2i) tree.Id.Position], this.m_treeManager, this.m_gameLoopEvents, this.m_calendar, prefabPath.Second);
      return treeMb;
    }
  }
}
