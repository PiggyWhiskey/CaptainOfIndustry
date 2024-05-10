// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainChunk
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Terrain
{
  public static class TerrainChunk
  {
    /// <summary>Number of bits used to represent tiles per edge.</summary>
    public const int BITS_TILES_PER_EDGE = 6;
    /// <summary>
    /// Bit mask that masks tile coordinate local to the chunk.
    /// </summary>
    public const int MASK_LOCAL_COORD = 63;
    /// <summary>Number of tiles per chunk edge. Must be power of two.</summary>
    /// <remarks>
    /// Value of 64 was carefully picked to be a balance between large tiles that does not have many seams and can be
    /// effectively used with LOD and small tiles that can do fast updates of data to GPU. Values of 128 or 32 are
    /// also feasible but not without rigorous testing.
    /// </remarks>
    public const int TILES_PER_EDGE = 64;
    /// <summary>Maximum value of a local coordinate.</summary>
    public const int MAX_LOCAL_COORD = 63;
    /// <summary>
    /// Number of <see cref="T:Mafi.Core.Terrain.TerrainTile" /> per one <see cref="T:Mafi.Core.Terrain.TerrainChunk" />.
    /// </summary>
    public const int TILES_PER_CHUNK = 4096;

    public static RelTile1i Size => new RelTile1i(64);

    public static RelTile2i Size2i => new RelTile2i(64, 64);
  }
}
