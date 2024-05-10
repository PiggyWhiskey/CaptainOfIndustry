// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.SurfaceDesignationData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct SurfaceDesignationData : IEquatable<SurfaceDesignationData>
  {
    public readonly Tile2iSlim OriginTile;
    /// <summary>
    /// 4x4 bitmap for tiles not assigned (X = i % 4, Y = i / 4).
    /// </summary>
    public readonly ushort UnassignedTilesBitmap;
    public readonly TileSurfaceSlimId SurfaceProtoSlimId;

    public static void Serialize(SurfaceDesignationData value, BlobWriter writer)
    {
      Tile2iSlim.Serialize(value.OriginTile, writer);
      writer.WriteUShort(value.UnassignedTilesBitmap);
      TileSurfaceSlimId.Serialize(value.SurfaceProtoSlimId, writer);
    }

    public static SurfaceDesignationData Deserialize(BlobReader reader)
    {
      return new SurfaceDesignationData((Tile2i) Tile2iSlim.Deserialize(reader), (uint) reader.ReadUShort(), TileSurfaceSlimId.Deserialize(reader));
    }

    public static Vector2i GetWithinChunkRelCoord(Tile2i originCoord)
    {
      return new Vector2i(originCoord.X >> 2 & 15, originCoord.Y >> 2 & 15);
    }

    public static int GetWithinChunkRelIndex(Tile2i originCoord)
    {
      return SurfaceDesignationData.GetWithinChunkRelIndex(SurfaceDesignationData.GetWithinChunkRelCoord(originCoord));
    }

    public static int GetWithinChunkRelIndex(Vector2i relWithinChunkCoord)
    {
      return relWithinChunkCoord.X | relWithinChunkCoord.Y << 4;
    }

    public static Vector2i GetWithinChunkRelCoord(int relWithinChunkIndex)
    {
      return new Vector2i(relWithinChunkIndex & 15, relWithinChunkIndex >> 4);
    }

    public Tile2i PlusXTileCoord => (Tile2i) this.OriginTile.AddX(4);

    public Tile2i PlusYTileCoord => (Tile2i) this.OriginTile.AddY(4);

    public Tile2i PlusXyTileCoord => (Tile2i) this.OriginTile.AddXy(4);

    public Tile2i CenterTileCoord => (Tile2i) this.OriginTile.AddXy(2);

    /// <summary>Relative chunk coordinate withing terrain chunk.</summary>
    public Vector2i WithinChunkRelCoord
    {
      get => SurfaceDesignationData.GetWithinChunkRelCoord((Tile2i) this.OriginTile);
    }

    public int WithinChunkRelIndex
    {
      get => SurfaceDesignationData.GetWithinChunkRelIndex((Tile2i) this.OriginTile);
    }

    /// <summary>Coordinate of terrain chunk this designation is on.</summary>
    public Chunk2i ChunkCoord => this.OriginTile.ChunkCoord2i;

    public SurfaceDesignationData(
      Tile2i originTile,
      uint unassignedTilesBitmap,
      TileSurfaceSlimId surfaceProtoSlimId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<Tile2i>(originTile % 4).IsZero("Given tile is not origin of a designation.");
      this.OriginTile = originTile.AsSlim;
      this.UnassignedTilesBitmap = (ushort) (unassignedTilesBitmap & (uint) ushort.MaxValue);
      this.SurfaceProtoSlimId = surfaceProtoSlimId;
    }

    public SurfaceDesignationData OffsetBy(RelTile2i offset)
    {
      return new SurfaceDesignationData(TerrainDesignation.GetOrigin((Tile2i) (this.OriginTile + offset)), (uint) this.UnassignedTilesBitmap, this.SurfaceProtoSlimId);
    }

    public SurfaceDesignationData WithNewUnassignedTiles(uint unassignedTilesBitmap)
    {
      return new SurfaceDesignationData((Tile2i) this.OriginTile, unassignedTilesBitmap, this.SurfaceProtoSlimId);
    }

    public bool Equals(SurfaceDesignationData other)
    {
      return this.OriginTile.Equals(other.OriginTile) && (int) this.UnassignedTilesBitmap == (int) other.UnassignedTilesBitmap && this.SurfaceProtoSlimId == other.SurfaceProtoSlimId;
    }

    public override bool Equals(object obj)
    {
      return obj is SurfaceDesignationData other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return Hash.Combine<Tile2iSlim, ushort, byte>(this.OriginTile, this.UnassignedTilesBitmap, this.SurfaceProtoSlimId.Value);
    }

    public override string ToString()
    {
      return string.Format("{0}. Unassigned: {1}", (object) this.OriginTile, (object) this.UnassignedTilesBitmap);
    }
  }
}
