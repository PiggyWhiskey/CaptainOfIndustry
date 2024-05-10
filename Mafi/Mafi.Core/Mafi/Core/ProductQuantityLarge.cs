// Decompiled with JetBrains decompiler
// Type: Mafi.Core.ProductQuantityLarge
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Serialization;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core
{
  /// <summary>
  /// Immutable struct that represents product and its large quantity.
  /// </summary>
  [DebuggerDisplay("{Product}={Quantity}")]
  [GenerateSerializer(false, null, 0)]
  public struct ProductQuantityLarge
  {
    /// <summary>
    /// Product quantity of unknown product with zero quantity. Please use this only in situations where you don't
    /// have any reasonable product prototype to use with <see cref="M:Mafi.Core.ProductQuantity.NoneOf(Mafi.Core.Products.ProductProto)" />.
    /// </summary>
    public static readonly ProductQuantityLarge None;
    public readonly ProductProto Product;
    public readonly QuantityLarge Quantity;

    /// <summary>Creates product quantity of given amount.</summary>
    public ProductQuantityLarge(ProductProto product, QuantityLarge quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Product = product.CheckNotNull<ProductProto>();
      this.Quantity = quantity;
    }

    /// <summary>Whether quantity is zero.</summary>
    public bool IsEmpty => this.Quantity.Value <= 0L;

    /// <summary>Whether quantity is positive.</summary>
    public bool IsNotEmpty => this.Quantity.Value > 0L;

    [Pure]
    public LocStrFormatted Format()
    {
      return this.Product.QuantityFormatter.FormatNumberOnly((IProtoWithIconAndName) this.Product, this.Quantity);
    }

    [Pure]
    public LocStrFormatted FormatNumberAndUnitOnly()
    {
      return this.Product.QuantityFormatter.FormatNumberAndUnitOnly((IProtoWithIconAndName) this.Product, this.Quantity);
    }

    public static void Serialize(ProductQuantityLarge value, BlobWriter writer)
    {
      writer.WriteGeneric<ProductProto>(value.Product);
      QuantityLarge.Serialize(value.Quantity, writer);
    }

    public static ProductQuantityLarge Deserialize(BlobReader reader)
    {
      return new ProductQuantityLarge(reader.ReadGenericAs<ProductProto>(), QuantityLarge.Deserialize(reader));
    }

    static ProductQuantityLarge()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductQuantityLarge.None = new ProductQuantityLarge(ProductProto.Phantom, QuantityLarge.Zero);
    }
  }
}
