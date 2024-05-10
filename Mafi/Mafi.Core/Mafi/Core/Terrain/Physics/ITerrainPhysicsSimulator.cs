// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Physics.ITerrainPhysicsSimulator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Terrain.Physics
{
  public interface ITerrainPhysicsSimulator
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

    void StartPhysicsSimulationAt(Tile2iAndIndex tileAndIndex);

    void StopPhysicsSimulationAt(Tile2iAndIndex tileAndIndex);

    int GetQueueSize();

    /// <summary>Update method called on the simulation thread.</summary>
    void Update();

    void Clear();
  }
}
