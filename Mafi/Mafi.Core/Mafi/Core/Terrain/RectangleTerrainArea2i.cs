// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.RectangleTerrainArea2i
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct RectangleTerrainArea2i : IEquatable<RectangleTerrainArea2i>
  {
    /// <summary>The tile with lowest X and Y coordinates in the area.</summary>
    public readonly Tile2i Origin;
    /// <summary>Size of the area. Size of zero represents empty area</summary>
    public readonly RelTile2i Size;

    public static void Serialize(RectangleTerrainArea2i value, BlobWriter writer)
    {
      Tile2i.Serialize(value.Origin, writer);
      RelTile2i.Serialize(value.Size, writer);
    }

    public static RectangleTerrainArea2i Deserialize(BlobReader reader)
    {
      return new RectangleTerrainArea2i(Tile2i.Deserialize(reader), RelTile2i.Deserialize(reader));
    }

    public int AreaTiles => this.Size.X * this.Size.Y;

    public bool IsEmpty => this.AreaTiles <= 0;

    public bool IsNotEmpty => this.AreaTiles > 0;

    public Tile2i CenterCoord => this.Origin + this.Size / 2;

    public Tile2f CenterCoordF => this.Origin.CornerTile2f + this.Size.RelTile2f / (Fix32) 2;

    public Tile2i PlusXTileIncl => this.Origin.AddX(this.Size.X - 1);

    public Tile2i PlusYTileIncl => this.Origin.AddY(this.Size.Y - 1);

    public Tile2i PlusXyTileIncl => this.Origin + this.Size - RelTile2i.One;

    /// <summary>
    /// +X corner of this area. The actual tile area is not part of this rectangle, only the vertex.
    /// </summary>
    public Tile2i PlusXCoordExcl => this.Origin.AddX(this.Size.X);

    /// <summary>
    /// +Y corner of this area. The actual tile area is not part of this rectangle, only the vertex.
    /// </summary>
    public Tile2i PlusYCoordExcl => this.Origin.AddY(this.Size.Y);

    /// <summary>
    /// +XY corner of this area. The actual tile area is not part of this rectangle, only the vertex.
    /// </summary>
    public Tile2i PlusXyCoordExcl => this.Origin + this.Size;

    public Chunk64Area Chunk64Area
    {
      get
      {
        Chunk2i chunkCoord2i1 = this.Origin.ChunkCoord2i;
        Chunk2i chunkCoord2i2 = this.PlusXyTileIncl.ChunkCoord2i;
        Vector2i vector2i1 = chunkCoord2i2.Vector2i;
        chunkCoord2i2 = this.Origin.ChunkCoord2i;
        Vector2i vector2i2 = chunkCoord2i2.Vector2i;
        Vector2i chunksCount = vector2i1 - vector2i2 + Vector2i.One;
        return new Chunk64Area(chunkCoord2i1, chunksCount);
      }
    }

    public RectangleTerrainArea2i(Tile2i origin, RelTile2i size)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<int>(size.X).IsNotNegative();
      Assert.That<int>(size.Y).IsNotNegative();
      this.Origin = origin;
      this.Size = size;
    }

    public static RectangleTerrainArea2i FromTwoPositions(Tile2i p1, Tile2i p2)
    {
      Tile2i origin = p1.Min(p2);
      Tile2i tile2i = p1.Max(p2);
      return new RectangleTerrainArea2i(origin, tile2i - origin + RelTile2i.One);
    }

    [Pure]
    public bool ContainsTile(Tile2i point)
    {
      return point >= this.Origin && point < this.Origin + this.Size;
    }

    [Pure]
    public bool ContainsTile(Tile2iSlim point)
    {
      return point >= this.Origin && point < this.Origin + this.Size;
    }

    [Pure]
    public bool ContainsVertex(Tile2i point)
    {
      return point >= this.Origin && point <= this.Origin + this.Size;
    }

    [Pure]
    public bool Contains(Tile2f point) => point >= this.Origin && point <= this.Origin + this.Size;

    [Pure]
    public bool FullyContains(RectangleTerrainArea2i area)
    {
      return this.Origin <= area.Origin && this.PlusXyCoordExcl >= area.PlusXyCoordExcl;
    }

    [Pure]
    public int GetTileIndex(Tile2i coord)
    {
      RelTile2i relTile2i = coord - this.Origin;
      return relTile2i.Y * this.Size.X + relTile2i.X;
    }

    [Pure]
    public RectangleTerrainArea2i ClampToTerrainBounds(TerrainManager manager)
    {
      return this.ClampToArea(manager.TerrainArea);
    }

    [Pure]
    public RectangleTerrainArea2i ClampToArea(RectangleTerrainArea2i area)
    {
      Tile2i plusXyCoordExcl = area.PlusXyCoordExcl;
      Tile2i tile2i1 = this.Origin.Max(area.Origin).Min(plusXyCoordExcl);
      Tile2i tile2i2 = this.PlusXyCoordExcl;
      tile2i2 = tile2i2.Max(area.Origin);
      Tile2i tile2i3 = tile2i2.Min(plusXyCoordExcl);
      return new RectangleTerrainArea2i(tile2i1.Min(plusXyCoordExcl - RelTile2i.One), tile2i3 - tile2i1);
    }

    /// <summary>
    /// Extends or shrinks the area (works for negative sizes).
    /// </summary>
    [Pure]
    public RectangleTerrainArea2i ExtendBy(RelTile2i extraOnEachSize)
    {
      return new RectangleTerrainArea2i(this.Origin - extraOnEachSize, this.Size + 2 * extraOnEachSize);
    }

    /// <summary>
    /// Extends or shrinks the area (works for negative sizes).
    /// </summary>
    [Pure]
    public RectangleTerrainArea2i ExtendBy(int extraOnEachSize)
    {
      return new RectangleTerrainArea2i(this.Origin - extraOnEachSize, this.Size + 2 * extraOnEachSize);
    }

    [Pure]
    public RectangleTerrainArea2i OffsetBy(RelTile2i offset)
    {
      return new RectangleTerrainArea2i(this.Origin + offset, this.Size);
    }

    [Pure]
    public RectangleTerrainArea2i WithNewOrigin(Tile2i origin)
    {
      return new RectangleTerrainArea2i(origin, this.Size);
    }

    /// <summary>
    /// Enumerates tiles in this area, inclusive on +x, +y boundaries.
    /// </summary>
    [Pure]
    public IEnumerable<Tile2i> EnumerateTiles()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2i>) new RectangleTerrainArea2i.\u003CEnumerateTiles\u003Ed__42(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this
      };
    }

    [Pure]
    public IEnumerable<Tile2iIndex> EnumerateTileIndices(TerrainManager terrainManager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2iIndex>) new RectangleTerrainArea2i.\u003CEnumerateTileIndices\u003Ed__43(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__terrainManager = terrainManager
      };
    }

    [Pure]
    public IEnumerable<Tile2iAndIndex> EnumerateTilesAndIndices(TerrainManager terrainManager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2iAndIndex>) new RectangleTerrainArea2i.\u003CEnumerateTilesAndIndices\u003Ed__44(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__terrainManager = terrainManager
      };
    }

    [Pure]
    public IEnumerable<TerrainTile> EnumerateTiles(TerrainManager manager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TerrainTile>) new RectangleTerrainArea2i.\u003CEnumerateTiles\u003Ed__45(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__manager = manager
      };
    }

    /// <summary>
    /// Enumerates tile corners in this area, inclusive on +x, +y boundaries.
    /// </summary>
    [Pure]
    public IEnumerable<TerrainTile> EnumerateTileVertices(TerrainManager manager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TerrainTile>) new RectangleTerrainArea2i.\u003CEnumerateTileVertices\u003Ed__46(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__manager = manager
      };
    }

    [Pure]
    public IEnumerable<Tile2iAndIndex> EnumerateBoundaryTilesAndIndices(TerrainManager manager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2iAndIndex>) new RectangleTerrainArea2i.\u003CEnumerateBoundaryTilesAndIndices\u003Ed__47(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__manager = manager
      };
    }

    [Pure]
    public IEnumerable<Tile2iIndex> EnumerateBoundaryIndices(TerrainManager manager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2iIndex>) new RectangleTerrainArea2i.\u003CEnumerateBoundaryIndices\u003Ed__48(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this,
        \u003C\u003E3__manager = manager
      };
    }

    /// <summary>Enumerates only boundary vertices of this area.</summary>
    [Pure]
    public IEnumerable<Tile2i> EnumerateTileVerticesBoundary()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2i>) new RectangleTerrainArea2i.\u003CEnumerateTileVerticesBoundary\u003Ed__49(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this
      };
    }

    /// <summary>
    /// Enumerates all affected chunks of tiles inside of this area (not affected vertices!).
    /// </summary>
    [Pure]
    public IEnumerable<Chunk2i> EnumerateChunks()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Chunk2i>) new RectangleTerrainArea2i.\u003CEnumerateChunks\u003Ed__50(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this
      };
    }

    [Pure]
    public IEnumerable<Chunk2iSlim> EnumerateChunksSlim()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Chunk2iSlim>) new RectangleTerrainArea2i.\u003CEnumerateChunksSlim\u003Ed__51(-2)
      {
        \u003C\u003E3__\u003C\u003E4__this = this
      };
    }

    /// <summary>
    /// Returns intersection between this and the given areas. If the intersection does not exist, returned area will
    /// have zero size.
    /// </summary>
    public RectangleTerrainArea2i Intersect(RectangleTerrainArea2i otherArea)
    {
      Tile2i origin = this.Origin.Max(otherArea.Origin);
      Tile2i tile2i = this.PlusXyCoordExcl.Min(otherArea.PlusXyCoordExcl);
      return new RectangleTerrainArea2i(origin, (tile2i - origin).Max(RelTile2i.Zero));
    }

    public RectangleTerrainArea2i Union(RectangleTerrainArea2i otherArea)
    {
      Tile2i origin = this.Origin.Min(otherArea.Origin);
      Tile2i tile2i = this.PlusXyCoordExcl.Max(otherArea.PlusXyCoordExcl);
      return new RectangleTerrainArea2i(origin, tile2i - origin);
    }

    public bool OverlapsWith(RectangleTerrainArea2i otherArea)
    {
      Tile2i plusXyCoordExcl1 = this.PlusXyCoordExcl;
      Tile2i plusXyCoordExcl2 = otherArea.PlusXyCoordExcl;
      return this.Origin.X < plusXyCoordExcl2.X && otherArea.Origin.X < plusXyCoordExcl1.X && this.Origin.Y < plusXyCoordExcl2.Y && otherArea.Origin.Y < plusXyCoordExcl1.Y;
    }

    public static bool operator ==(RectangleTerrainArea2i lhs, RectangleTerrainArea2i rhs)
    {
      return lhs.Equals(rhs);
    }

    public static bool operator !=(RectangleTerrainArea2i lhs, RectangleTerrainArea2i rhs)
    {
      return !lhs.Equals(rhs);
    }

    public bool Equals(RectangleTerrainArea2i other)
    {
      return this.Origin == other.Origin && this.Size == other.Size;
    }

    public override bool Equals(object obj)
    {
      return obj is RectangleTerrainArea2i other && this.Equals(other);
    }

    public override int GetHashCode() => Hash.Combine<Tile2i, RelTile2i>(this.Origin, this.Size);

    public override string ToString()
    {
      return string.Format("Area {0}+{1}", (object) this.Origin, (object) this.Size);
    }
  }
}
