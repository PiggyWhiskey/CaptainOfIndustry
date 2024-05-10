// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Physics.ITerrainDisruptionSimulator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Terrain.Physics
{
  /// <summary>
  /// Implementation of this interface is responsible for simulating surface disruption.
  /// </summary>
  public interface ITerrainDisruptionSimulator
  {
    bool IsDisabled { get; }

    /// <summary>
    /// Whether physics simulator has tiles to process on next update.
    /// </summary>
    bool IsProcessingTiles { get; }

    /// <summary>
    /// Called once in the TerrainManager constructor. This should be used just to obtain reference to the terrain
    /// manager.
    /// </summary>
    void Initialize(TerrainManager terrainManager);

    /// <summary>
    /// When disabled, no tiles should be processed automatically on terrain changes.
    /// </summary>
    void SetDisabled(bool isDisabled);

    /// <summary>Update method called on the simulation thread.</summary>
    void Update();

    /// <summary>Recovers disruption on a specified tile.</summary>
    void RecoverTile(Tile2iAndIndex tileAndIndex);

    /// <summary>
    /// Gets event that is fired after material recovery for the given product.
    /// </summary>
    Event<MaterialConversionResult> GetMaterialRecoveryEvent(TerrainMaterialProto product);

    int GetQueueSize();

    /// <summary>Clears or scheduled disruption events.</summary>
    void Clear();
  }
}
