// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileSurfaceData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Serialization;
using Mafi.Utils;

#nullable disable
namespace Mafi.Core.Terrain
{
  [ExpectedStructSize(8)]
  [GenerateSerializer(false, null, 0)]
  public readonly struct TileSurfaceData
  {
    public const int SIZE_BYTES = 8;
    public const int IS_AUTO_PLACED_SHIFT = 10;
    public const int HEIGHT_BITS_INCL_SIGN = 13;
    /// <summary>
    /// Raw data (signed) are packed in the following way (from LSB to HSB):
    /// <code>
    /// [0-7] (8) surface ID, zero means no surface.
    /// [8-9] (2) texture rotation.
    /// [10] (1) is auto-placed.
    /// [11] (1) is ramp on/off
    /// [12-13] (2) ramp rotation
    /// [14-16] (3) unused, leave some bits for scale.
    /// [17-18] (2) ramp vertical offset {1|3|5|7}/8 tiles.
    /// [19-31] (13) signed min height as int, +- 4k range
    /// </code>
    /// </summary>
    [SerializeUsingNonVariableEncoding]
    public readonly int RawValue;
    /// <summary>
    /// Raw data (signed) are packed in the following way (from LSB to HSB):
    /// <code>
    /// [0-7] (8) decal ID, zero means no decal.
    /// [8] (1) decal flipped bit.
    /// [9-10] (2) decal rotation.
    /// [11-13] (3) 3-bit color key
    /// [14-31] (18) unused.
    /// </code>
    /// </summary>
    [SerializeUsingNonVariableEncoding]
    [NewInSaveVersion(140, null, null, null, null)]
    public readonly int RawValueDecal;

    public static void Serialize(TileSurfaceData value, BlobWriter writer)
    {
      writer.WriteIntNonVariable(value.RawValue);
      writer.WriteIntNonVariable(value.RawValueDecal);
    }

    public static TileSurfaceData Deserialize(BlobReader reader)
    {
      return new TileSurfaceData(reader.ReadIntNonVariable(), reader.LoadedSaveVersion >= 140 ? reader.ReadIntNonVariable() : 0);
    }

    public HeightTilesF Height
    {
      get
      {
        return new HeightTilesF((Fix32) (this.RawValue >> 19) + (this.IsRamp ? ((Fix32) (this.RawValue >> 17 & 3) + Fix32.Half) * Fix32.Quarter : (Fix32) 0));
      }
    }

    public TileSurfaceSlimId SurfaceSlimId => new TileSurfaceSlimId((byte) this.RawValue);

    public Direction90 TextureRotation => new Direction90((this.RawValue & 768) >> 8);

    public Direction90 RampRotation => new Direction90((this.RawValue & 12288) >> 12);

    public bool IsRamp => (this.RawValue & 2048) != 0;

    public bool IsAutoPlaced => (this.RawValue & 1024) != 0;

    public TileSurfaceDecalSlimId DecalSlimId
    {
      get => new TileSurfaceDecalSlimId((byte) this.RawValueDecal);
    }

    public Direction90 DecalRotation => new Direction90((this.RawValueDecal & 1536) >> 9);

    public bool IsDecalFlipped => (this.RawValueDecal >> 8 & 1) != 0;

    public int ColorKey => this.RawValueDecal >> 11 & 7;

    public bool IsValid => (this.RawValue & (int) byte.MaxValue) != 0;

    public bool IsNotValid => (this.RawValue & (int) byte.MaxValue) == 0;

    [LoadCtor]
    public TileSurfaceData(int rawValue, int rawValueDecal)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawValue = rawValue;
      this.RawValueDecal = rawValueDecal;
    }

    public TileSurfaceData(
      int rawValue,
      TileSurfaceDecalSlimId decalSlimId,
      bool decalFlipped,
      Rotation90 decalDirection,
      int colorKey)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<int>(colorKey).IsLess(8);
      this.RawValue = rawValue;
      this.RawValueDecal = (int) decalSlimId.Value | (decalFlipped ? 1 : 0) << 8 | decalDirection.AngleIndex << 9 | (colorKey & 15) << 11;
    }

    public static TileSurfaceData CreateFlat(
      HeightTilesI height,
      TileSurfaceSlimId surfaceId,
      Direction90 textureDirection,
      bool isAutoPlaced = false)
    {
      return new TileSurfaceData((int) surfaceId.Value | (isAutoPlaced ? 1024 : 0) | textureDirection.DirectionIndex << 8 | height.Value << 19, 0);
    }

    public static TileSurfaceData CreateRamp(
      HeightTilesF height,
      TileSurfaceSlimId surfaceId,
      Direction90 textureDirection,
      Direction90 rampDirection,
      bool isAutoPlaced = false)
    {
      int integerPartNonNegative = ((height.Value - (Fix32) height.TilesHeightFloored.Value) * 4).IntegerPartNonNegative;
      if (integerPartNonNegative > 3)
      {
        Log.Error("Fractional height expected to be <= 3");
        integerPartNonNegative &= 3;
      }
      return new TileSurfaceData((int) surfaceId.Value | rampDirection.DirectionIndex << 12 | 2048 | (isAutoPlaced ? 1024 : 0) | textureDirection.DirectionIndex << 8 | integerPartNonNegative << 17 | height.TilesHeightFloored.Value << 19, 0);
    }

    public TileSurfaceData WithDecal(
      TileSurfaceDecalSlimId decalSlimId,
      bool decalFlipped,
      Rotation90 decalDirection,
      int colorKey)
    {
      return new TileSurfaceData(this.RawValue, decalSlimId, decalFlipped, decalDirection, colorKey);
    }

    public TileSurfaceData WithoutDecal() => new TileSurfaceData(this.RawValue, 0);

    public TerrainTileSurfaceProto ResolveToProto(TerrainManager terrainManager)
    {
      return terrainManager.ResolveSlimSurface(this.SurfaceSlimId);
    }
  }
}
