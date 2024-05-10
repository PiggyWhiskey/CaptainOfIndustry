// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainPropMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Terrain.Props;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TerrainPropMbFactory : IFactory<TerrainPropData, TerrainPropMb>
  {
    private readonly AssetsDb m_assetsDb;

    public TerrainPropMbFactory(AssetsDb assetsDb, RandomProvider randomProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
    }

    public TerrainPropMb Create(TerrainPropData data)
    {
      TerrainPropMb terrainPropMb = this.m_assetsDb.GetClonedPrefabOrEmptyGo(data.Proto.Graphics.PrefabPath).AddComponent<TerrainPropMb>();
      terrainPropMb.Initialize(this.m_assetsDb, data);
      return terrainPropMb;
    }
  }
}
