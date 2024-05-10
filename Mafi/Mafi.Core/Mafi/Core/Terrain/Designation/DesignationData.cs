// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.DesignationData
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
  public readonly struct DesignationData : IEquatable<DesignationData>
  {
    public readonly Tile2i OriginTile;
    public readonly HeightTilesI OriginTargetHeight;
    public readonly HeightTilesI PlusXTargetHeight;
    public readonly HeightTilesI PlusYTargetHeight;
    public readonly HeightTilesI PlusXyTargetHeight;
    public readonly HeightTilesF CenterTargetHeight;

    public static void Serialize(DesignationData value, BlobWriter writer)
    {
      Tile2i.Serialize(value.OriginTile, writer);
      HeightTilesI.Serialize(value.OriginTargetHeight, writer);
      HeightTilesI.Serialize(value.PlusXTargetHeight, writer);
      HeightTilesI.Serialize(value.PlusXyTargetHeight, writer);
      HeightTilesI.Serialize(value.PlusYTargetHeight, writer);
      HeightTilesF.Serialize(value.CenterTargetHeight, writer);
    }

    public static DesignationData Deserialize(BlobReader reader)
    {
      return new DesignationData(Tile2i.Deserialize(reader), HeightTilesI.Deserialize(reader), HeightTilesI.Deserialize(reader), HeightTilesI.Deserialize(reader), HeightTilesI.Deserialize(reader), HeightTilesF.Deserialize(reader));
    }

    public static Vector2i GetWithinChunkRelCoord(Tile2i originCoord)
    {
      return new Vector2i(originCoord.X >> 2 & 15, originCoord.Y >> 2 & 15);
    }

    public static int GetWithinChunkRelIndex(Tile2i originCoord)
    {
      return DesignationData.GetWithinChunkRelIndex(DesignationData.GetWithinChunkRelCoord(originCoord));
    }

    public static int GetWithinChunkRelIndex(Vector2i relWithinChunkCoord)
    {
      return relWithinChunkCoord.X | relWithinChunkCoord.Y << 4;
    }

    public static Vector2i GetWithinChunkRelCoord(int relWithinChunkIndex)
    {
      return new Vector2i(relWithinChunkIndex & 15, relWithinChunkIndex >> 4);
    }

    public Tile2i PlusXTileCoord => this.OriginTile.AddX(4);

    public Tile2i PlusYTileCoord => this.OriginTile.AddY(4);

    public Tile2i PlusXyTileCoord => this.OriginTile.AddXy(4);

    public Tile2i CenterTileCoord => this.OriginTile.AddXy(2);

    /// <summary>Relative chunk coordinate withing terrain chunk.</summary>
    public Vector2i WithinChunkRelCoord => DesignationData.GetWithinChunkRelCoord(this.OriginTile);

    public int WithinChunkRelIndex => DesignationData.GetWithinChunkRelIndex(this.OriginTile);

    /// <summary>Coordinate of terrain chunk this designation is on.</summary>
    public Chunk2i ChunkCoord => this.OriginTile.ChunkCoord2i;

    [LoadCtor]
    public DesignationData(
      Tile2i originTile,
      HeightTilesI originTargetHeight,
      HeightTilesI plusXTargetHeight,
      HeightTilesI plusXyTargetHeight,
      HeightTilesI plusYTargetHeight,
      HeightTilesF centerTargetHeight)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<Tile2i>(originTile % 4).IsZero("Given tile is not origin of a designation.");
      this.OriginTile = originTile;
      this.OriginTargetHeight = originTargetHeight;
      this.PlusXTargetHeight = plusXTargetHeight;
      this.PlusYTargetHeight = plusYTargetHeight;
      this.PlusXyTargetHeight = plusXyTargetHeight;
      this.CenterTargetHeight = centerTargetHeight;
    }

    public DesignationData(
      Tile2i originTile,
      HeightTilesI originTargetHeight,
      HeightTilesI plusXTargetHeight,
      HeightTilesI plusXyTargetHeight,
      HeightTilesI plusYTargetHeight)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<Tile2i>(originTile % 4).IsZero("Given tile is not origin of a designation.");
      this.OriginTile = originTile;
      this.OriginTargetHeight = originTargetHeight;
      this.PlusXTargetHeight = plusXTargetHeight;
      this.PlusYTargetHeight = plusYTargetHeight;
      this.PlusXyTargetHeight = plusXyTargetHeight;
      this.CenterTargetHeight = new HeightTilesF((originTargetHeight.Value + plusXTargetHeight.Value + plusYTargetHeight.Value + plusXyTargetHeight.Value).Over(4));
    }

    public DesignationData(Tile2i originTile, HeightTilesI allHeights)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<Tile2i>(originTile % 4).IsZero("Given tile is not origin of a designation.");
      this.OriginTile = originTile;
      this.OriginTargetHeight = allHeights;
      this.PlusXTargetHeight = allHeights;
      this.PlusYTargetHeight = allHeights;
      this.PlusXyTargetHeight = allHeights;
      this.CenterTargetHeight = allHeights.HeightTilesF;
    }

    public DesignationData OffsetBy(RelTile2i offset)
    {
      return new DesignationData(TerrainDesignation.GetOrigin(this.OriginTile + offset), this.OriginTargetHeight, this.PlusXTargetHeight, this.PlusYTargetHeight, this.PlusXyTargetHeight, this.CenterTargetHeight);
    }

    public bool Equals(DesignationData other)
    {
      return this.OriginTile.Equals(other.OriginTile) && this.OriginTargetHeight.Equals(other.OriginTargetHeight) && this.PlusXTargetHeight.Equals(other.PlusXTargetHeight) && this.PlusYTargetHeight.Equals(other.PlusYTargetHeight) && this.PlusXyTargetHeight.Equals(other.PlusXyTargetHeight) && this.CenterTargetHeight.Equals(other.CenterTargetHeight);
    }

    public override bool Equals(object obj) => obj is DesignationData other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<Tile2i, HeightTilesI, HeightTilesI, HeightTilesI, HeightTilesI>(this.OriginTile, this.OriginTargetHeight, this.PlusXTargetHeight, this.PlusYTargetHeight, this.PlusXyTargetHeight);
    }

    public override string ToString()
    {
      return string.Format("{0} heights ({1} c, {2} o, {3} +x, {4} +xy, {5} +y)", (object) this.OriginTile, (object) this.CenterTargetHeight, (object) this.OriginTargetHeight, (object) this.PlusXTargetHeight, (object) this.PlusXyTargetHeight, (object) this.PlusYTargetHeight);
    }
  }
}
