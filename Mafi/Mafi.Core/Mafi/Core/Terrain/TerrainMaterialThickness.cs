// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainMaterialThickness
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TerrainMaterialThickness
  {
    public readonly TerrainMaterialProto Material;
    public readonly ThicknessTilesF Thickness;

    public static void Serialize(TerrainMaterialThickness value, BlobWriter writer)
    {
      writer.WriteGeneric<TerrainMaterialProto>(value.Material);
      ThicknessTilesF.Serialize(value.Thickness, writer);
    }

    public static TerrainMaterialThickness Deserialize(BlobReader reader)
    {
      return new TerrainMaterialThickness(reader.ReadGenericAs<TerrainMaterialProto>(), ThicknessTilesF.Deserialize(reader));
    }

    public TerrainMaterialThickness(TerrainMaterialProto material, ThicknessTilesF thickness)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Material = material.CheckNotNullOrPhantom<TerrainMaterialProto>();
      this.Thickness = thickness.CheckNotNegative();
    }

    /// <summary>Returns zero thickness of given product.</summary>
    public static TerrainMaterialThickness NoneOf(TerrainMaterialProto material)
    {
      return new TerrainMaterialThickness(material, ThicknessTilesF.Zero);
    }

    /// <summary>
    /// Whether this product's thickness is zero (or negative).
    /// </summary>
    public bool IsEmpty => this.Thickness.IsNotPositive;

    public bool IsNotEmpty => this.Thickness.IsPositive;

    public TerrainMaterialThicknessSlim AsSlim
    {
      get => new TerrainMaterialThicknessSlim(this.Material, this.Thickness);
    }

    public ProductQuantity ToProductQuantityRounded()
    {
      return new ProductQuantity((ProductProto) this.Material.MinedProduct, this.Material.ThicknessToQuantityRounded(this.Thickness));
    }

    public PartialProductQuantity ToPartialProductQuantity()
    {
      return new PartialProductQuantity((ProductProto) this.Material.MinedProduct, this.Material.ThicknessToQuantity(this.Thickness));
    }

    public LooseProductQuantity ToLooseProductQuantityRounded(LooseProductProto originalProduct)
    {
      if (!originalProduct.TerrainMaterial.IsNone)
        return new LooseProductQuantity(originalProduct, originalProduct.TerrainMaterial.Value.ThicknessToQuantityRounded(this.Thickness));
      Log.Error("Failed to convert back to original material.");
      return this.ToLooseProductQuantityRounded();
    }

    public LooseProductQuantity ToLooseProductQuantityRounded()
    {
      return new LooseProductQuantity(this.Material.MinedProduct, this.Material.ThicknessToQuantityRounded(this.Thickness));
    }

    /// <summary>
    /// Removes up to <paramref name="maxThicknessRemoved" /> from this quantity and returns amount of removed. Also
    /// fills up the remainder quantity.
    /// </summary>
    [Pure]
    public TerrainMaterialThickness RemoveAsMuchAs(
      ThicknessTilesF maxThicknessRemoved,
      out TerrainMaterialThickness remainder)
    {
      TerrainMaterialThickness materialThickness = new TerrainMaterialThickness(this.Material, this.Thickness.RemoveAsMuchAs(ref maxThicknessRemoved));
      remainder = new TerrainMaterialThickness(this.Material, maxThicknessRemoved);
      return materialThickness;
    }

    public static TerrainMaterialThickness operator +(
      TerrainMaterialThickness lhs,
      ThicknessTilesF rhs)
    {
      return new TerrainMaterialThickness(lhs.Material, lhs.Thickness + rhs);
    }

    public static TerrainMaterialThickness operator +(
      TerrainMaterialThickness lhs,
      TerrainMaterialThickness rhs)
    {
      Assert.That<TerrainMaterialProto>(lhs.Material).IsEqualTo<TerrainMaterialProto>(rhs.Material, "Adding incompatible materials!!");
      return new TerrainMaterialThickness(lhs.Material, lhs.Thickness + rhs.Thickness);
    }

    public static TerrainMaterialThickness operator -(
      TerrainMaterialThickness lhs,
      ThicknessTilesF rhs)
    {
      return new TerrainMaterialThickness(lhs.Material, lhs.Thickness - rhs);
    }

    public override string ToString()
    {
      return string.Format("{0} of {1}", (object) this.Thickness, (object) this.Material.Id);
    }
  }
}
