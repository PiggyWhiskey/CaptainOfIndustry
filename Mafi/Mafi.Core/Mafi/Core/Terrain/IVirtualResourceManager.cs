// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.IVirtualResourceManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Generation;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <summary>Convenience class for mining virtual resources.</summary>
  public interface IVirtualResourceManager
  {
    ImmutableArray<IVirtualTerrainResource> RetrieveResourcesAt(
      ProductProto product,
      Tile2i position);

    ImmutableArray<IVirtualTerrainResource> RetrieveAllResourcesAt(Tile2i position);
  }
}
