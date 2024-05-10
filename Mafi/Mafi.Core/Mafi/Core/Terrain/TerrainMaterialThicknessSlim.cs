// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainMaterialThicknessSlim
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Serialization;
using Mafi.Utils;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <summary>
  /// Slim version of <see cref="T:Mafi.Core.Terrain.TerrainMaterialThickness" />. This struct is just 4 bytes: 1 byte for material ID and
  /// 3 bytes for thickness. The <see cref="T:Mafi.Core.Terrain.TerrainMaterialThickness" /> is 8 + 4 bytes (but needs 8 B alignment
  /// so it is 16 B effectively). This is used in terrain tile to represent layers.
  /// 
  /// This struct does not support negative values and the max positive representable value is ~2^12 = 4096.
  /// TODO: It is possible to hit thickness of this value, make this safe, possibly by reducing the fraction precision
  /// or by splitting really large layers by some thin layer of bedrock or something similar.
  /// 
  /// Use <see cref="M:Mafi.Core.Terrain.TerrainMaterialThicknessSlim.ToFull(Mafi.Core.Terrain.TerrainManager)" /> to resolve this to full <see cref="T:Mafi.Core.Terrain.TerrainMaterialThickness" />.
  /// </summary>
  [ExpectedStructSize(4)]
  [GenerateSerializer(false, null, 0)]
  public readonly struct TerrainMaterialThicknessSlim : IEquatable<TerrainMaterialThicknessSlim>
  {
    public readonly uint RawData;

    public static void Serialize(TerrainMaterialThicknessSlim value, BlobWriter writer)
    {
      writer.WriteUInt(value.RawData);
    }

    public static TerrainMaterialThicknessSlim Deserialize(BlobReader reader)
    {
      return new TerrainMaterialThicknessSlim(reader.ReadUInt());
    }

    public TerrainMaterialSlimId SlimId
    {
      get => new TerrainMaterialSlimId((byte) (this.RawData & (uint) byte.MaxValue));
    }

    public int SlimIdRaw => (int) this.RawData & (int) byte.MaxValue;

    public ThicknessTilesF Thickness
    {
      get => new ThicknessTilesF(Fix32.FromRaw((int) (this.RawData >> 8)));
    }

    [LoadCtor]
    public TerrainMaterialThicknessSlim(uint rawData)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawData = rawData;
    }

    public TerrainMaterialThicknessSlim(TerrainMaterialSlimId slimId, ThicknessTilesF thickness)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawData = (uint) slimId.Value | (uint) (thickness.Value.RawValue << 8);
    }

    public TerrainMaterialThicknessSlim(TerrainMaterialProto material, ThicknessTilesF thickness)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new TerrainMaterialThicknessSlim(material.SlimId, thickness);
    }

    /// <summary>
    /// Whether this material thickness is default struct.
    /// WARNING: Zero thickness of phantom proto is considered None.
    /// </summary>
    public bool IsNone => this.RawData == 0U;

    /// <summary>
    /// Whether this material thickness is not a default struct.
    /// WARNING: Zero thickness of phantom proto is considered None.
    /// </summary>
    public bool HasValue => this.RawData > 0U;

    /// <summary>
    /// Whether this product's thickness is zero (or negative).
    /// </summary>
    public bool IsEmpty => this.Thickness.IsNotPositive;

    public bool IsPositive => this.Thickness.IsPositive;

    public TerrainMaterialThickness ToFull(TerrainManager manager)
    {
      return new TerrainMaterialThickness(manager.ResolveSlimMaterial(this.SlimId), this.Thickness);
    }

    public TerrainMaterialThicknessSlim WithNewId(TerrainMaterialSlimId newId)
    {
      return new TerrainMaterialThicknessSlim(this.RawData & 4294967040U | (uint) newId.Value);
    }

    public TerrainMaterialThicknessSlim WithThickness(ThicknessTilesF thickness)
    {
      return new TerrainMaterialThicknessSlim(this.SlimId, thickness);
    }

    public override string ToString()
    {
      return string.Format("{0} of #{1}", (object) this.Thickness, (object) this.SlimId);
    }

    public PartialProductQuantity ToPartialProductQuantity(TerrainManager manager)
    {
      TerrainMaterialProto full = this.SlimId.ToFull(manager);
      return new PartialProductQuantity((ProductProto) full.MinedProduct, full.ThicknessToQuantity(this.Thickness));
    }

    /// <summary>
    /// Removes up to <paramref name="maxThicknessRemoved" /> from this quantity and returns amount of removed. Also
    /// fills up the remainder quantity.
    /// </summary>
    [Pure]
    public TerrainMaterialThicknessSlim RemoveAsMuchAs(
      ThicknessTilesF maxThicknessRemoved,
      out TerrainMaterialThicknessSlim remainder)
    {
      TerrainMaterialThicknessSlim materialThicknessSlim = new TerrainMaterialThicknessSlim(this.SlimId, this.Thickness.RemoveAsMuchAs(ref maxThicknessRemoved));
      remainder = new TerrainMaterialThicknessSlim(this.SlimId, maxThicknessRemoved);
      return materialThicknessSlim;
    }

    public bool Equals(TerrainMaterialThicknessSlim other)
    {
      return (int) this.RawData == (int) other.RawData;
    }

    public override bool Equals(object obj)
    {
      return obj is TerrainMaterialThicknessSlim other && this.Equals(other);
    }

    public override int GetHashCode() => (int) this.RawData;

    public static bool operator ==(
      TerrainMaterialThicknessSlim lhs,
      TerrainMaterialThicknessSlim rhs)
    {
      return (int) lhs.RawData == (int) rhs.RawData;
    }

    public static bool operator !=(
      TerrainMaterialThicknessSlim lhs,
      TerrainMaterialThicknessSlim rhs)
    {
      return (int) lhs.RawData != (int) rhs.RawData;
    }

    public static TerrainMaterialThicknessSlim operator +(
      TerrainMaterialThicknessSlim lhs,
      ThicknessTilesF rhs)
    {
      return new TerrainMaterialThicknessSlim(lhs.SlimId, lhs.Thickness + rhs);
    }

    public static TerrainMaterialThicknessSlim operator -(
      TerrainMaterialThicknessSlim lhs,
      ThicknessTilesF rhs)
    {
      return new TerrainMaterialThicknessSlim(lhs.SlimId, lhs.Thickness - rhs);
    }
  }
}
