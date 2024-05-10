// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainTile
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Resources;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <summary>
  /// Represents a terrain coordinate bundled with a terrain manager reference for easier work with tiles.
  /// Use this only where necessary, passing only <see cref="T:Mafi.Tile2i" /> or <see cref="T:Mafi.Tile2iSlim" /> may be sufficient
  /// in many cases and is smaller in memory.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct TerrainTile : IEquatable<TerrainTile>
  {
    /// <summary>
    /// Size of an edge of a tile in meters. This serves as a conversion between tiles and meters.
    /// </summary>
    /// <remarks>
    /// WARNING: Many game decisions and game assets rely on this constant. DO NOT CHANGE IT!! Value of 1 is too
    /// small. Cubic tile would have only 1 m^3 and single scoop of a big excavator would take tens of them. We do
    /// not need to be this detailed. Also things like path finding that works with tiles would be slower due to
    /// additional work. Value of 2 seems good compromise with a nice property that 2 divides 10, so we can measure
    /// tens of meters by highlighting tiles. Value of 3 is weird and 4 is too much and does not divide 10.
    /// </remarks>
    public const int TILE_SIZE_M = 2;
    public const int TILE_SIZE_M_HALF = 1;
    /// <summary>Area of a tile in squared meters.</summary>
    public const int TILE_AREA_M = 4;
    /// <summary>Volume of a tile in cubic meters.</summary>
    public const int TILE_VOLUME_M = 8;
    public const int RESERVED_FLAGS_COUNT = 3;
    public const int MAX_FLAGS_COUNT = 16;
    public const ushort FLAG_IS_ON_BOUNDARY = 1;
    public const ushort FLAG_IS_OFF_LIMITS = 2;
    public const ushort FLAG_IS_OCEAN = 4;
    public static readonly ThicknessTilesF MIN_LAYER_THICKNESS;
    public static readonly ThicknessTilesF MIN_LAYER_THICKNESS_PER_DEPTH;
    public readonly TerrainManager TerrainManager;
    /// <summary>Global tile coordinate.</summary>
    public readonly Tile2iSlim TileCoordSlim;
    /// <summary>
    /// Cached tile data index, a contiguous row-major index.
    /// Many tile operations need this index so it is pre-computed.
    /// </summary>
    [DoNotSave(0, null)]
    public readonly Tile2iIndex DataIndex;

    public static void Serialize(TerrainTile value, BlobWriter writer)
    {
      Tile2iSlim.Serialize(value.TileCoordSlim, writer);
      TerrainManager.Serialize(value.TerrainManager, writer);
    }

    public static TerrainTile Deserialize(BlobReader reader)
    {
      return new TerrainTile(Tile2iSlim.Deserialize(reader), TerrainManager.Deserialize(reader));
    }

    public Tile2i TileCoord => (Tile2i) this.TileCoordSlim;

    /// <summary>
    /// Constructor that should be called only by <see cref="T:Mafi.Core.Terrain.TerrainChunk" />. All neighbors are initialized to
    /// Phantom.
    /// </summary>
    [LoadCtor]
    internal TerrainTile(Tile2iSlim tileCoordSlim, TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.TileCoordSlim = tileCoordSlim;
      this.TerrainManager = terrainManager.CheckNotNull<TerrainManager>();
      this.DataIndex = this.TerrainManager.GetTileIndex(tileCoordSlim);
    }

    internal TerrainTile(Tile2i tileCoord, TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new TerrainTile(tileCoord.AsSlim, terrainManager);
    }

    /// <summary>
    /// This ctor is used in cases where data index is already known to avoid extra computations.
    /// </summary>
    private TerrainTile(Tile2iSlim tileCoord, TerrainManager terrainManager, Tile2iIndex dataIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.TileCoordSlim = tileCoord;
      this.TerrainManager = terrainManager.CheckNotNull<TerrainManager>();
      this.DataIndex = dataIndex;
    }

    public bool IsOnTerrain => this.TerrainManager.IsValidCoord(this.TileCoordSlim);

    public bool IsOffLimitsOrInvalid => this.TerrainManager.IsOffLimitsOrInvalid(this.TileCoord);

    public Chunk2i ChunkCoord2i => this.TileCoord.ChunkCoord2i;

    public Tile2iAndIndex CoordAndIndex
    {
      get => new Tile2iAndIndex(this.TileCoordSlim.X, this.TileCoordSlim.Y, this.DataIndex.Value);
    }

    public Tile3f CornerTile3f => this.TileCoordSlim.CornerTile2f.ExtendHeight(this.Height);

    public Tile2f CornerTile2f => this.TileCoordSlim.CornerTile2f;

    public Tile2f CenterTile2f => this.TileCoordSlim.CenterTile2f;

    public Tile3f CenterTile3f
    {
      get
      {
        Tile2f centerTile2f = this.TileCoordSlim.CenterTile2f;
        ref Tile2f local = ref centerTile2f;
        Fix32 fix32_1 = this.Height.Value + this.PlusXNeighbor.Height.Value + this.PlusYNeighbor.Height.Value;
        TerrainTile terrainTile = this.PlusXNeighbor;
        terrainTile = terrainTile.PlusYNeighbor;
        Fix32 fix32_2 = terrainTile.Height.Value;
        HeightTilesF height = new HeightTilesF((fix32_1 + fix32_2) / 4);
        return local.ExtendHeight(height);
      }
    }

    public Tile3i WithHeightFloored
    {
      get => this.TileCoordSlim.ExtendHeight(this.Height.TilesHeightFloored);
    }

    public HeightTilesF Height => this.TerrainManager.GetHeight(this.DataIndex);

    public void SetHeight(HeightTilesF height)
    {
      this.TerrainManager.SetHeight(this.CoordAndIndex, height);
    }

    public HeightTilesF MinHeightOfAllCorners()
    {
      TerrainTile plusXneighbor = this.PlusXNeighbor;
      HeightTilesF heightTilesF = this.Height;
      heightTilesF = heightTilesF.Min(plusXneighbor.Height);
      heightTilesF = heightTilesF.Min(this.PlusYNeighbor.Height);
      return heightTilesF.Min(plusXneighbor.PlusYNeighbor.Height);
    }

    public HeightTilesF MaxHeightOfAllCorners()
    {
      TerrainTile plusXneighbor = this.PlusXNeighbor;
      HeightTilesF heightTilesF = this.Height;
      heightTilesF = heightTilesF.Max(plusXneighbor.Height);
      heightTilesF = heightTilesF.Max(this.PlusYNeighbor.Height);
      return heightTilesF.Max(plusXneighbor.PlusYNeighbor.Height);
    }

    public void SetFlags(uint mask) => this.TerrainManager.SetTileFlags(this.CoordAndIndex, mask);

    public void ClearFlags(uint mask)
    {
      this.TerrainManager.ClearTileFlags(this.CoordAndIndex, mask);
    }

    public bool IsBlockingVehicles => this.TerrainManager.IsBlockingVehicles(this.DataIndex);

    public bool IsBlockingBuildings => this.TerrainManager.IsBlockingBuildings(this.DataIndex);

    public bool IsBlockingBuildingsOrVehicles
    {
      get => this.TerrainManager.IsBlockingBuildingsOrVehicles(this.DataIndex);
    }

    public bool IsOcean => this.TerrainManager.HasAllTileFlagsSet(this.DataIndex, 4U);

    public bool IsNotOcean => !this.IsOcean;

    /// <summary>Returns a tile relative to this tile.</summary>
    public TerrainTile this[RelTile2i relativePosition]
    {
      get => this.TerrainManager[this.TileCoordSlim.AsFull + relativePosition];
    }

    /// <summary>
    /// Returns +X neighbor. If this tile does not exist due to end of the map, returns self.
    /// </summary>
    public TerrainTile PlusXNeighbor
    {
      get
      {
        int x = (int) this.TileCoordSlim.X + 1;
        return x >= this.TerrainManager.TerrainWidth ? this : new TerrainTile(new Tile2iSlim((ushort) x, this.TileCoordSlim.Y), this.TerrainManager, this.DataIndex + 1);
      }
    }

    /// <summary>
    /// Returns -X neighbor. If this tile does not exist due to end of the map, returns self.
    /// </summary>
    public TerrainTile MinusXNeighbor
    {
      get
      {
        return this.TileCoordSlim.X <= (ushort) 0 ? this : new TerrainTile(this.TileCoordSlim.IncrementX, this.TerrainManager, this.DataIndex - 1);
      }
    }

    /// <summary>
    /// Returns +Y neighbor. If this tile does not exist due to end of the map, returns self.
    /// </summary>
    public TerrainTile PlusYNeighbor
    {
      get
      {
        int y = (int) this.TileCoordSlim.Y + 1;
        return y >= this.TerrainManager.TerrainHeight ? this : new TerrainTile(new Tile2iSlim(this.TileCoordSlim.X, (ushort) y), this.TerrainManager, this.DataIndex + this.TerrainManager.TerrainWidth);
      }
    }

    /// <summary>
    /// Returns -Y neighbor. If this tile does not exist due to end of the map, returns self.
    /// </summary>
    public TerrainTile MinusYNeighbor
    {
      get
      {
        return this.TileCoordSlim.Y <= (ushort) 0 ? this : new TerrainTile(this.TileCoordSlim.IncrementX, this.TerrainManager, this.DataIndex - this.TerrainManager.TerrainWidth);
      }
    }

    public TerrainTile PlusXyNeighbor
    {
      get
      {
        return this.TerrainManager[this.TerrainManager.ClampToTerrainBounds(new Tile2i((int) this.TileCoordSlim.X + 1, (int) this.TileCoordSlim.Y + 1))];
      }
    }

    /// <summary>
    /// Returns neighbor tile. If edge of map is hit, returns itself.
    /// </summary>
    public TerrainTile this[NeighborCoord neighborCoord]
    {
      get
      {
        switch (neighborCoord.Index)
        {
          case 0:
            return this.PlusXNeighbor;
          case 1:
            return this.MinusXNeighbor;
          case 2:
            return this.PlusYNeighbor;
          case 3:
            return this.MinusYNeighbor;
          default:
            Log.Warning("Invalid neighborhood index: " + neighborCoord.Index.ToString());
            return this;
        }
      }
    }

    /// <summary>
    /// Returns neighbor tile in the given direction. If edge of map is hit, returns itself.
    /// </summary>
    public TerrainTile this[Direction90 direction]
    {
      get
      {
        switch (direction.DirectionIndex)
        {
          case 0:
            return this.PlusXNeighbor;
          case 1:
            return this.PlusYNeighbor;
          case 2:
            return this.MinusXNeighbor;
          case 3:
            return this.MinusYNeighbor;
          default:
            Log.Warning("Invalid direction index: " + direction.DirectionIndex.ToString());
            return this;
        }
      }
    }

    /// <summary>
    /// Number of distinct layers, always positive, includes bedrock that is always present at some depth.
    /// </summary>
    public int LayersCount => this.LayersCountNoBedrock + 1;

    public int LayersCountNoBedrock => this.TerrainManager.GetLayersCountNoBedrock(this.DataIndex);

    /// <summary>
    /// Whether this tile is mined all the way to the bedrock and there are no other layers. Note that this is
    /// not equal to <c>TopProduct == bedrock</c> because bedrock can be dumped on the tile and other stuff might be
    /// below it (like diamonds:).
    /// </summary>
    public bool IsAtBedrock => this.LayersCountNoBedrock == 0;

    /// <summary>Enumerates all layers</summary>
    public TerrainLayerEnumerator EnumerateLayers()
    {
      return new TerrainLayerEnumerator(this.TerrainManager, this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetFirstLayerSlimOrNoneNoBedrock(Mafi.Tile2iIndex)" />
    public TerrainMaterialThicknessSlim FirstLayerSlimOrNoneNoBedrock
    {
      get => this.TerrainManager.GetFirstLayerSlimOrNoneNoBedrock(this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetFirstLayerSlim(Mafi.Tile2iIndex)" />
    public TerrainMaterialThicknessSlim FirstLayerSlim
    {
      get => this.TerrainManager.GetFirstLayerSlim(this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetFirstLayer(Mafi.Tile2iIndex)" />
    public TerrainMaterialThickness FirstLayer => this.TerrainManager.GetFirstLayer(this.DataIndex);

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetSecondLayerSlimOrNoneNoBedrock(Mafi.Tile2iIndex)" />
    public TerrainMaterialThicknessSlim SecondLayerSlimOrNoneNoBedrock
    {
      get => this.TerrainManager.GetSecondLayerSlimOrNoneNoBedrock(this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetSecondLayerSlim(Mafi.Tile2iIndex)" />
    public TerrainMaterialThicknessSlim SecondLayerSlim
    {
      get => this.TerrainManager.GetSecondLayerSlim(this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetSecondLayer(Mafi.Tile2iIndex)" />
    public TerrainMaterialThickness SecondLayer
    {
      get => this.TerrainManager.GetSecondLayer(this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetThirdLayerSlimOrNoneNoBedrock(Mafi.Tile2iIndex)" />
    public TerrainMaterialThicknessSlim ThirdLayerSlimOrNoneNoBedrock
    {
      get => this.TerrainManager.GetThirdLayerSlimOrNoneNoBedrock(this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetThirdLayerSlim(Mafi.Tile2iIndex)" />
    public TerrainMaterialThicknessSlim ThirdLayerSlim
    {
      get => this.TerrainManager.GetThirdLayerSlim(this.DataIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetThirdLayer(Mafi.Tile2iIndex)" />
    public TerrainMaterialThickness ThirdLayer => this.TerrainManager.GetThirdLayer(this.DataIndex);

    /// <inheritdoc cref="!:TerrainManager.GetTileLayerAt" />
    public TerrainMaterialThicknessSlim GetLayerAt(int layerIndex)
    {
      return this.TerrainManager.GetLayerAt(this.DataIndex, layerIndex);
    }

    /// <inheritdoc cref="M:Mafi.Core.Terrain.TerrainManager.GetThicknessOfMaterialInFirst4Layers(Mafi.Tile2iIndex,Mafi.Core.Products.TerrainMaterialSlimId)" />
    public ThicknessTilesF GetThicknessOfMaterialInFirst4Layers(TerrainMaterialSlimId slimId)
    {
      return this.TerrainManager.GetThicknessOfMaterialInFirst4Layers(this.DataIndex, slimId);
    }

    /// <summary>
    /// Updates the given list with all products that are in this tile. Only products from <paramref name="products" /> are returned. Returned products are ordered from the shallowest to the deepest.
    /// </summary>
    public void GetResourceDetails(
      HybridSet<LooseProductProto> products,
      Lyst<ProductResource> result)
    {
      Assert.That<int>(result.Count).IsZero();
      ThicknessTilesF zero = ThicknessTilesF.Zero;
      foreach (TerrainMaterialThicknessSlim enumerateLayer in this.TerrainManager.EnumerateLayers(this.DataIndex))
      {
        LooseProductProto minedProduct = enumerateLayer.SlimId.ToFull(this.TerrainManager).MinedProduct;
        if (products.Contains(minedProduct))
        {
          int index = result.FindIndex<LooseProductProto>(minedProduct, (Func<ProductResource, LooseProductProto, bool>) ((x, mp) => (Proto) x.Product == (Proto) mp));
          if (index >= 0)
            result[index] = result[index].AddHeight(enumerateLayer.Thickness);
          else
            result.Add(new ProductResource(minedProduct, enumerateLayer.Thickness, zero));
        }
        zero += enumerateLayer.Thickness;
      }
    }

    public bool HasTileSurface() => this.TerrainManager.HasTileSurface(this.DataIndex);

    public TileSurfaceData GetTileSurface() => this.TerrainManager.GetTileSurface(this.DataIndex);

    public override string ToString() => this.TileCoordSlim.ToString();

    public bool Equals(TerrainTile other) => this.TileCoordSlim == other.TileCoordSlim;

    public override bool Equals(object obj) => obj is TerrainTile other && this.Equals(other);

    public override int GetHashCode() => this.TileCoordSlim.GetHashCode();

    public static bool operator ==(TerrainTile lhs, TerrainTile rhs)
    {
      return lhs.TileCoordSlim == rhs.TileCoordSlim;
    }

    public static bool operator !=(TerrainTile lhs, TerrainTile rhs)
    {
      return lhs.TileCoordSlim != rhs.TileCoordSlim;
    }

    static TerrainTile()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainTile.MIN_LAYER_THICKNESS = (1.0 / 16.0).TilesThick();
      TerrainTile.MIN_LAYER_THICKNESS_PER_DEPTH = 0.25.TilesThick();
    }
  }
}
