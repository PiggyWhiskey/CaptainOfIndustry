// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Props.TerrainPropData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Props
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TerrainPropData
  {
    public readonly TerrainPropProto Proto;
    public readonly TerrainPropId Id;
    public readonly RelTile2f PositionWithinTile;
    public readonly AngleSlim RotationYaw;
    public readonly AngleSlim RotationPitch;
    public readonly AngleSlim RotationRoll;
    public readonly Percent Scale;
    public readonly HeightTilesF PlacedAtHeight;
    public readonly TerrainPropData.PropVariant Variant;
    /// <summary>
    /// Vertical offset that is applied after rotation. Valid range is +- 2 tiles.
    /// </summary>
    public readonly ThicknessTilesF PlacementHeightOffset;

    public static void Serialize(TerrainPropData value, BlobWriter writer)
    {
      TerrainPropId.Serialize(value.Id, writer);
      writer.WriteGeneric<TerrainPropProto>(value.Proto);
      TerrainPropData.PropVariant.Serialize(value.Variant, writer);
      RelTile2f.Serialize(value.PositionWithinTile, writer);
      HeightTilesF.Serialize(value.PlacedAtHeight, writer);
      Percent.Serialize(value.Scale, writer);
      AngleSlim.Serialize(value.RotationYaw, writer);
      AngleSlim.Serialize(value.RotationPitch, writer);
      AngleSlim.Serialize(value.RotationRoll, writer);
      ThicknessTilesF.Serialize(value.PlacementHeightOffset, writer);
    }

    public static TerrainPropData Deserialize(BlobReader reader)
    {
      return new TerrainPropData(TerrainPropId.Deserialize(reader), reader.ReadGenericAs<TerrainPropProto>(), TerrainPropData.PropVariant.Deserialize(reader), RelTile2f.Deserialize(reader), HeightTilesF.Deserialize(reader), Percent.Deserialize(reader), AngleSlim.Deserialize(reader), AngleSlim.Deserialize(reader), AngleSlim.Deserialize(reader), ThicknessTilesF.Deserialize(reader));
    }

    public bool IsValid => (Mafi.Core.Prototypes.Proto) this.Proto != (Mafi.Core.Prototypes.Proto) null;

    public Tile2f Position => this.Id.Position.CornerTile2f + this.PositionWithinTile;

    [LoadCtor]
    private TerrainPropData(
      TerrainPropId id,
      TerrainPropProto proto,
      TerrainPropData.PropVariant variant,
      RelTile2f positionWithinTile,
      HeightTilesF placedAtHeight,
      Percent scale,
      AngleSlim rotationYaw = default (AngleSlim),
      AngleSlim rotationPitch = default (AngleSlim),
      AngleSlim rotationRoll = default (AngleSlim),
      ThicknessTilesF placementHeightOffset = default (ThicknessTilesF))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Id = id;
      this.Proto = proto.CheckNotNull<TerrainPropProto>();
      this.PlacedAtHeight = placedAtHeight;
      this.PositionWithinTile = positionWithinTile;
      this.Scale = scale.CheckPositive();
      this.RotationYaw = rotationYaw;
      this.RotationPitch = rotationPitch;
      this.RotationRoll = rotationRoll;
      this.Variant = variant;
      this.PlacementHeightOffset = placementHeightOffset;
    }

    public TerrainPropData(
      TerrainPropProto proto,
      TerrainPropData.PropVariant variant,
      Tile2f position,
      HeightTilesF placedAtHeight,
      Percent scale,
      AngleSlim rotationYaw = default (AngleSlim),
      AngleSlim rotationPitch = default (AngleSlim),
      AngleSlim rotationRoll = default (AngleSlim),
      ThicknessTilesF placementHeightOffset = default (ThicknessTilesF))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Id = new TerrainPropId(position.Tile2i.AsSlim);
      this.Proto = proto.CheckNotNull<TerrainPropProto>();
      this.Variant = variant.CheckNotDefaultStruct<TerrainPropData.PropVariant>();
      this.PositionWithinTile = position - this.Id.Position.CornerTile2f;
      this.PlacedAtHeight = placedAtHeight;
      this.Scale = scale.CheckPositive();
      this.RotationYaw = rotationYaw;
      this.RotationPitch = rotationPitch;
      this.RotationRoll = rotationRoll;
      this.PlacementHeightOffset = placementHeightOffset;
      if (this.Proto.DoesNotBlocksVehicles && this.Proto.Extents.X.ScaledBy(this.Scale) < Fix32.Sqrt2 / 8 && this.Proto.Extents.Y.ScaledBy(this.Scale) < Fix32.Sqrt2 / 8)
        Log.Warning(string.Format("Prop '{0}' at '{1}' with very small bounding area. Scale: '{2}'.", (object) proto, (object) this.Id, (object) scale));
      Assert.That<Tile2f>(position).IsEqualTo<Tile2f>(this.Position);
    }

    /// <summary>Fills list with occupied tiles. Does not clear list.</summary>
    public void CalculateOccupiedTiles(TerrainManager terrainManager, Lyst<Tile2i> tiles)
    {
      switch (this.Proto.BoundingShape)
      {
        case TerrainPropBoundingShape.Rectangle:
          Fix32 fix32_1 = this.Proto.Extents.X.ScaledBy(this.Scale).Max(Fix32.OneOverSqrt2 + Fix32.Epsilon);
          Fix32 other = this.Proto.Extents.Y.ScaledBy(this.Scale).Max(Fix32.OneOverSqrt2 + Fix32.Epsilon);
          int num1 = (fix32_1.Max(other) * Fix32.Sqrt2).ToIntCeiled() + 1;
          for (int x = -num1; x <= num1; ++x)
          {
            for (int y = -num1; y <= num1; ++y)
            {
              Vector2f vector2f = new Vector2f((Fix32) x, (Fix32) y).Rotate(-this.RotationYaw.ToAngleDegrees1f());
              if (!(vector2f.X < -fix32_1) && !(vector2f.X > fix32_1) && !(vector2f.Y < -other) && !(vector2f.Y > other))
              {
                Tile2i coord = (Tile2i) (this.Id.Position + new RelTile2i(x, y));
                if (terrainManager.IsValidCoord(coord))
                  tiles.Add(coord);
              }
            }
          }
          break;
        case TerrainPropBoundingShape.Circle:
          Fix32 fix32_2 = this.Proto.Extents.X.ScaledBy(this.Scale).Max(Fix32.OneOverSqrt2 + Fix32.Epsilon);
          int num2 = fix32_2.ToIntCeiled() + 1;
          Fix64 fix64 = fix32_2.Squared();
          for (int x = -num2; x <= num2; ++x)
          {
            for (int y = -num2; y <= num2; ++y)
            {
              Tile2i coord = this.Id.Position.AsFull + new RelTile2i(x, y);
              if (terrainManager.IsValidCoord(coord) && coord.CenterTile2f.DistanceSqrTo(this.Position) < fix64)
                tiles.Add(coord);
            }
          }
          break;
        default:
          Log.Warning("Unhandled bounding shape");
          break;
      }
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct PropVariant
    {
      public readonly uint UvOriginAndSizePackedTexIndex;

      public static void Serialize(TerrainPropData.PropVariant value, BlobWriter writer)
      {
        writer.WriteUInt(value.UvOriginAndSizePackedTexIndex);
      }

      public static TerrainPropData.PropVariant Deserialize(BlobReader reader)
      {
        return new TerrainPropData.PropVariant(reader.ReadUInt());
      }

      [LoadCtor]
      private PropVariant(uint uvOriginAndSizePackedTexIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.UvOriginAndSizePackedTexIndex = uvOriginAndSizePackedTexIndex;
      }

      private PropVariant(float uvOriginX, float uvOriginY, float uvSize)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.UvOriginAndSizePackedTexIndex = TerrainPropData.PropVariant.packUvOriginAndSize(uvOriginX, uvOriginY, uvSize);
      }

      public PropVariant(Vector2i uvCoord, int uvSplitCount)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this = new TerrainPropData.PropVariant((float) uvCoord.X / (float) uvSplitCount, (float) uvCoord.Y / (float) uvSplitCount, 1f / (float) uvSplitCount);
      }

      public PropVariant(
        TerrainMaterialSlimId materialSlimId,
        float offsetX,
        float offsetY,
        float uvSize)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.UvOriginAndSizePackedTexIndex = TerrainPropData.PropVariant.packUvOriginAndSize(offsetX, offsetY, uvSize) | (uint) materialSlimId.Value << 24;
      }

      private static uint packUvOriginAndSize(float originX, float originY, float scale)
      {
        return (uint) ((int) (uint) ((double) originX * 128.0) | (int) (uint) ((double) originY * 128.0) << 8 | ((int) (uint) ((double) scale * 64.0) & (int) byte.MaxValue) << 16);
      }
    }
  }
}
