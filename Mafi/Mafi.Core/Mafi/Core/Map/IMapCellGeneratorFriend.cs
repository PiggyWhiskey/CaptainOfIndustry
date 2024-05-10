// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.IMapCellGeneratorFriend
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Terrain.Generation;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// Internal functions that can be called only during map generation.
  /// </summary>
  public interface IMapCellGeneratorFriend
  {
    void SetIsOcean(bool isOcean);

    void SetState(MapCellState newState);

    void ResetState();

    void SetIsUnlockedByDefault(bool isUnlockedByDefault);

    void SetHeight(HeightTilesI height);

    void SetSurfaceGenerator(ICellSurfaceGenerator surfaceGenerator);

    void SetEdgeTerrainFactory(
      Option<IMapCellEdgeTerrainFactory> edgeTerrainFactory);
  }
}
