// Decompiled with JetBrains decompiler
// Type: Mafi.Core.ProductQuantity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core
{
  /// <summary>
  /// Immutable struct that represents product and its quantity.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct ProductQuantity : IEquatable<ProductQuantity>
  {
    /// <summary>
    /// Product quantity of unknown product with zero quantity. Please use this only in situations where you don't
    /// have any reasonable product prototype to use with <see cref="M:Mafi.Core.ProductQuantity.NoneOf(Mafi.Core.Products.ProductProto)" />.
    /// </summary>
    public static readonly ProductQuantity None;
    public readonly ProductProto Product;
    public readonly Quantity Quantity;

    /// <summary>Returns zero quantity of given product.</summary>
    public static ProductQuantity NoneOf(ProductProto proto)
    {
      return new ProductQuantity(proto, Quantity.Zero);
    }

    /// <summary>Creates product quantity of given amount.</summary>
    public ProductQuantity(ProductProto product, Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<ProductProto>(product).IsNotNull<ProductProto>("Null product proto.");
      this.Product = product;
      this.Quantity = quantity;
    }

    /// <summary>Whether quantity is zero.</summary>
    public bool IsEmpty => this.Quantity.Value == 0;

    /// <summary>Whether quantity is not zero.</summary>
    public bool IsNotEmpty => this.Quantity.Value != 0;

    /// <summary>
    /// Adds given quantity to this quantity. The result quantity is never larger than <paramref name="maxQuantity" />. The remainder that can not fit returned in <paramref name="remainder" />.
    /// </summary>
    /// <example>
    /// ProductQuantity remainder; m_product = m_product.Add(product, maxQuantity, out remainder); return remainder;
    /// </example>
    public ProductQuantity Add(
      ProductQuantity quantity,
      Quantity maxQuantity,
      out ProductQuantity remainder)
    {
      if (this.IsNotEmpty && (Proto) quantity.Product != (Proto) this.Product)
      {
        remainder = quantity;
        return this;
      }
      Quantity quantity1 = quantity.Quantity.Min(maxQuantity - this.Quantity);
      Quantity quantity2 = quantity.Quantity - quantity1;
      remainder = new ProductQuantity(quantity.Product, quantity2);
      return new ProductQuantity(quantity.Product, this.Quantity + quantity1);
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
    [Pure]
    public ProductQuantity Remove(Quantity maxRemovedQuantity, out ProductQuantity remainder)
    {
      remainder = new ProductQuantity(this.Product, Quantity.Zero.Max(this.Quantity - maxRemovedQuantity));
      return new ProductQuantity(this.Product, this.Quantity - remainder.Quantity);
    }

    /// <summary>
    /// Removes up to requested amount from given product quantity. Returns amount removed.
    /// </summary>
    public static ProductQuantity RemoveFrom(
      ref ProductQuantity productQuantity,
      Quantity maxRemovedQuantity)
    {
      Quantity quantity = productQuantity.Quantity.Min(maxRemovedQuantity);
      productQuantity = new ProductQuantity(productQuantity.Product, productQuantity.Quantity - quantity);
      return new ProductQuantity(productQuantity.Product, quantity);
    }

    /// <summary>
    /// Combines this product quantity with given product quantity. If this quantity is phantom or empty, returns the
    /// given value. Otherwise it checks whether the products are compatible and add adds the quantities. If given
    /// products are not compatible, returns this and logs error.
    /// </summary>
    [Pure]
    public ProductQuantity Combine(ProductQuantity value)
    {
      if (this.IsEmpty || this.Product.IsPhantom)
        return value;
      if (value.IsEmpty || value.Product.IsPhantom)
        return this;
      if (!((Proto) this.Product != (Proto) value.Product))
        return new ProductQuantity(this.Product, this.Quantity + value.Quantity);
      Log.Warning(string.Format("Combining non-identical products: '{0}' and '{1}'. Returning the first one.", (object) this.Product, (object) value.Product));
      return this;
    }

    [Pure]
    public ProductQuantity WithNewQuantity(Quantity newQuantity)
    {
      return new ProductQuantity(this.Product, newQuantity);
    }

    [Pure]
    public LocStrFormatted Format()
    {
      return this.Product.QuantityFormatter.FormatNumberOnly((IProtoWithIconAndName) this.Product, (QuantityLarge) this.Quantity);
    }

    [Pure]
    public LocStrFormatted FormatNumberAndUnitOnly()
    {
      return this.Product.QuantityFormatter.FormatNumberAndUnitOnly((IProtoWithIconAndName) this.Product, (QuantityLarge) this.Quantity);
    }

    [Pure]
    public ProductQuantity ScaledBy(Percent percent)
    {
      return new ProductQuantity(this.Product, this.Quantity.ScaledBy(percent));
    }

    private static bool equals(ProductQuantity lhs, ProductQuantity rhs)
    {
      return (Proto) lhs.Product == (Proto) rhs.Product && lhs.Quantity == rhs.Quantity;
    }

    public bool Equals(ProductQuantity other) => ProductQuantity.equals(this, other);

    public override bool Equals(object obj)
    {
      return obj is ProductQuantity rhs && ProductQuantity.equals(this, rhs);
    }

    public override int GetHashCode()
    {
      return Hash.Combine<ProductProto, Quantity>(this.Product, this.Quantity);
    }

    public override string ToString()
    {
      return string.Format("{0} of {1}", (object) this.Quantity, (object) this.Product);
    }

    /// <summary>Adds product quantity and integer quantity amount.</summary>
    public static ProductQuantity operator +(ProductQuantity lhs, Quantity rhs)
    {
      return new ProductQuantity(lhs.Product, lhs.Quantity + rhs.CheckNotNegative());
    }

    /// <summary>
    /// Subtracts integer quantity amount from product quantity.
    /// </summary>
    public static ProductQuantity operator -(ProductQuantity lhs, Quantity rhs)
    {
      return new ProductQuantity(lhs.Product, lhs.Quantity - rhs);
    }

    public static ProductQuantity operator -(ProductQuantity lhs, ProductQuantity rhs)
    {
      Assert.That<ProductProto>(lhs.Product).IsEqualTo<ProductProto>(rhs.Product, "Subtracting incompatible products!!");
      return new ProductQuantity(lhs.Product, lhs.Quantity - rhs.Quantity);
    }

    public static ProductQuantity operator +(ProductQuantity lhs, ProductQuantity rhs)
    {
      Assert.That<ProductProto>(lhs.Product).IsEqualTo<ProductProto>(rhs.Product, "Summing incompatible products!!");
      return new ProductQuantity(lhs.Product, lhs.Quantity + rhs.Quantity);
    }

    public static ProductQuantity operator *(ProductQuantity lhs, int rhs)
    {
      return new ProductQuantity(lhs.Product, lhs.Quantity * rhs);
    }

    public static bool operator ==(ProductQuantity lhs, ProductQuantity rhs)
    {
      return ProductQuantity.equals(lhs, rhs);
    }

    public static bool operator !=(ProductQuantity lhs, ProductQuantity rhs)
    {
      return !ProductQuantity.equals(lhs, rhs);
    }

    public static void TransferFromTo(
      ref ProductQuantity source,
      ref ProductQuantity destination,
      Quantity maxDestinationQuantity)
    {
      Assert.That<Quantity>(source.Quantity).IsNotNegative();
      Assert.That<Quantity>(destination.Quantity).IsNotNegative();
      if (source.IsEmpty)
        return;
      if (destination.IsEmpty)
      {
        destination = source;
      }
      else
      {
        if ((Proto) destination.Product != (Proto) source.Product)
        {
          Assert.Fail("Transferring incompatible products!");
          return;
        }
        destination += source.Quantity;
      }
      if (destination.Quantity <= maxDestinationQuantity)
      {
        source = ProductQuantity.NoneOf(source.Product);
      }
      else
      {
        source = destination - maxDestinationQuantity;
        destination -= source.Quantity;
      }
    }

    public static void Serialize(ProductQuantity value, BlobWriter writer)
    {
      writer.WriteGeneric<ProductProto>(value.Product);
      Quantity.Serialize(value.Quantity, writer);
    }

    public static ProductQuantity Deserialize(BlobReader reader)
    {
      return new ProductQuantity(reader.ReadGenericAs<ProductProto>(), Quantity.Deserialize(reader));
    }

    static ProductQuantity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductQuantity.None = new ProductQuantity(ProductProto.Phantom, Quantity.Zero);
    }
  }
}
