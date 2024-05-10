// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IVirtualTerrainResourceGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface IVirtualTerrainResourceGenerator : ITerrainFeatureBase
  {
    ImmutableArray<IVirtualTerrainResource> GenerateResources();

    ImmutableArray<ProductQuantity> GetResourceStats();

    /// <summary>
    /// Returns resource info that will be displayed to players.
    /// Only generators that are resource nodes should return this.
    /// </summary>
    TerrainFeatureResourceInfo? GetResourceInfo();
  }
}
