// Decompiled with JetBrains decompiler
// Type: Mafi.Core.LooseProductQuantity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core
{
  /// <summary>
  /// Immutable struct that represents loose product and its quantity.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [DebuggerDisplay("{Quantity} of {Product}")]
  public struct LooseProductQuantity
  {
    /// <summary>
    /// Product quantity of unknown product with zero quantity. Please use this only in situations where you don't
    /// have any reasonable product prototype to use with <see cref="M:Mafi.Core.LooseProductQuantity.NoneOf(Mafi.Core.Products.LooseProductProto)" />.
    /// </summary>
    public static readonly LooseProductQuantity None;
    public readonly LooseProductProto Product;
    public readonly Quantity Quantity;

    /// <summary>Returns zero quantity of given product.</summary>
    public static LooseProductQuantity NoneOf(LooseProductProto proto)
    {
      return new LooseProductQuantity(proto, Quantity.Zero);
    }

    /// <summary>Creates product quantity of given amount.</summary>
    public LooseProductQuantity(LooseProductProto product, Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Product = product.CheckNotNull<LooseProductProto>();
      this.Quantity = quantity.CheckNotNegative();
    }

    public ProductQuantity ProductQuantity
    {
      get => new ProductQuantity((ProductProto) this.Product, this.Quantity);
    }

    /// <summary>Whether quantity is zero.</summary>
    public bool IsEmpty => this.Quantity.Value <= 0;

    public bool IsNotEmpty => this.Quantity.Value > 0;

    /// <summary>Returns product thickness of this product quantity.</summary>
    public TerrainMaterialThickness ToTerrainThickness()
    {
      if (!this.Product.TerrainMaterial.IsNone)
        return new TerrainMaterialThickness(this.Product.TerrainMaterial.Value, this.Product.TerrainMaterial.Value.QuantityToThickness(this.Quantity));
      Mafi.Assert.Fail(string.Format("Product cannot be converted to terrain material: {0}", (object) this.Product));
      return TerrainMaterialThickness.NoneOf(TerrainMaterialProto.PhantomTerrainMaterialProto);
    }

    /// <summary>
    /// Adds given quantity to this quantity. The result quantity is never larger than <paramref name="maxQuantity" />. The remainder that can not fit returned in <paramref name="remainder" />.
    /// </summary>
    /// <example>
    /// <code>
    /// ProductQuantity remainder;
    /// m_product = m_product.Add(product, maxQuantity, out remainder);
    /// return remainder;
    /// </code>
    /// </example>
    public LooseProductQuantity Add(
      LooseProductQuantity quantity,
      Quantity maxQuantity,
      out LooseProductQuantity remainder)
    {
      if (!this.IsEmpty && (Proto) quantity.Product != (Proto) this.Product)
      {
        remainder = quantity;
        return this;
      }
      Quantity quantity1 = quantity.Quantity.Min(maxQuantity - this.Quantity);
      Quantity quantity2 = quantity.Quantity - quantity1;
      remainder = new LooseProductQuantity(quantity.Product, quantity2);
      return new LooseProductQuantity(quantity.Product, this.Quantity + quantity1);
    }

    /// <summary>
    /// Removes up to <paramref name="maxRemovedQuantity" /> from this quantity and returns amount of removed. Also
    /// fills up the remainder quantity.
    /// </summary>
    /// <example>
    /// <code>
    /// ProductQuantity remainder;
    /// ProductQuantity removed = m_product.Remove(maxToRemove, out remainder);
    /// m_product = remainder;
    /// return removed;
    /// </code>
    /// </example>
    public LooseProductQuantity Remove(
      Quantity maxRemovedQuantity,
      out LooseProductQuantity remainder)
    {
      remainder = new LooseProductQuantity(this.Product, Quantity.Zero.Max(this.Quantity - maxRemovedQuantity));
      return new LooseProductQuantity(this.Product, this.Quantity - remainder.Quantity);
    }

    public override string ToString()
    {
      return string.Format("{0} of {1}", (object) this.Quantity, (object) this.Product);
    }

    /// <summary>Adds product quantity and integer quantity amount.</summary>
    public static LooseProductQuantity operator +(LooseProductQuantity lhs, Quantity rhs)
    {
      return new LooseProductQuantity(lhs.Product, lhs.Quantity + rhs.CheckNotNegative());
    }

    public static LooseProductQuantity operator +(
      LooseProductQuantity lhs,
      LooseProductQuantity rhs)
    {
      Mafi.Assert.That<LooseProductProto>(lhs.Product).IsEqualTo<LooseProductProto>(rhs.Product, "Adding incompatible loose products!!");
      return new LooseProductQuantity(lhs.Product, lhs.Quantity + rhs.Quantity.CheckNotNegative());
    }

    /// <summary>
    /// Subtracts integer quantity amount from product quantity. Result quantity is never negative.
    /// </summary>
    public static LooseProductQuantity operator -(LooseProductQuantity lhs, Quantity rhs)
    {
      return new LooseProductQuantity(lhs.Product, Quantity.Zero.Max(lhs.Quantity - rhs));
    }

    public static LooseProductQuantity operator -(
      LooseProductQuantity lhs,
      LooseProductQuantity rhs)
    {
      Mafi.Assert.That<LooseProductProto>(lhs.Product).IsEqualTo<LooseProductProto>(rhs.Product, "Subtracting incompatible loose products!!");
      Mafi.Assert.That<Quantity>(lhs.Quantity).IsGreaterOrEqual(rhs.Quantity, "Subtracting results in negative quantity!!");
      return new LooseProductQuantity(lhs.Product, Quantity.Zero.Max(lhs.Quantity - rhs.Quantity));
    }

    public static void Serialize(LooseProductQuantity value, BlobWriter writer)
    {
      writer.WriteGeneric<LooseProductProto>(value.Product);
      Quantity.Serialize(value.Quantity, writer);
    }

    public static LooseProductQuantity Deserialize(BlobReader reader)
    {
      return new LooseProductQuantity(reader.ReadGenericAs<LooseProductProto>(), Quantity.Deserialize(reader));
    }

    static LooseProductQuantity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LooseProductQuantity.None = new LooseProductQuantity(LooseProductProto.Phantom, Quantity.Zero);
    }
  }
}
