// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.StumpMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class StumpMbFactory : IFactory<TreeStumpData, StumpMb>
  {
    private readonly TerrainManager m_terrainManager;
    private readonly AssetsDb m_assetsDb;
    private readonly ICalendar m_calendar;

    public StumpMbFactory(TerrainManager terrainManager, AssetsDb assetsDb, ICalendar calendar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_assetsDb = assetsDb;
      this.m_calendar = calendar;
    }

    public StumpMb Create(TreeStumpData stump)
    {
      StumpMb stumpMb = new GameObject().AddComponent<StumpMb>();
      stumpMb.Initialize(stump, this.m_terrainManager[(Tile2i) stump.Id.Position], this.m_calendar, this.m_assetsDb);
      return stumpMb;
    }
  }
}
