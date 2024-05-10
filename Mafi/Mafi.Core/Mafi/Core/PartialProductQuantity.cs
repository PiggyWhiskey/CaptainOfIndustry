// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PartialProductQuantity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct PartialProductQuantity : IEquatable<PartialProductQuantity>
  {
    /// <summary>
    /// Product quantity of unknown product with zero quantity. Please use this only in situations where you don't
    /// have any reasonable product prototype to use with <see cref="M:Mafi.Core.PartialProductQuantity.NoneOf(Mafi.Core.Products.ProductProto)" />.
    /// </summary>
    public static readonly PartialProductQuantity None;
    public readonly ProductProto Product;
    public readonly PartialQuantity Quantity;

    /// <summary>Returns zero quantity of given product.</summary>
    public static PartialProductQuantity NoneOf(ProductProto proto)
    {
      return new PartialProductQuantity(proto, PartialQuantity.Zero);
    }

    /// <summary>Creates product quantity of given amount.</summary>
    public PartialProductQuantity(ProductProto product, PartialQuantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<ProductProto>(product).IsNotNull<ProductProto>("Null product proto.");
      this.Product = product;
      this.Quantity = quantity;
    }

    /// <summary>Whether quantity is zero.</summary>
    public bool IsEmpty => this.Quantity.IsZero;

    /// <summary>Whether quantity is not zero.</summary>
    public bool IsNotEmpty => this.Quantity.IsNotZero;

    public bool Equals(PartialProductQuantity other)
    {
      return object.Equals((object) this.Product, (object) other.Product) && this.Quantity.Equals(other.Quantity);
    }

    public override bool Equals(object obj)
    {
      return obj is PartialProductQuantity other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return Hash.Combine<ProductProto, PartialQuantity>(this.Product, this.Quantity);
    }

    public static void Serialize(PartialProductQuantity value, BlobWriter writer)
    {
      writer.WriteGeneric<ProductProto>(value.Product);
      PartialQuantity.Serialize(value.Quantity, writer);
    }

    public static PartialProductQuantity Deserialize(BlobReader reader)
    {
      return new PartialProductQuantity(reader.ReadGenericAs<ProductProto>(), PartialQuantity.Deserialize(reader));
    }

    static PartialProductQuantity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PartialProductQuantity.None = new PartialProductQuantity(ProductProto.Phantom, PartialQuantity.Zero);
    }
  }
}
