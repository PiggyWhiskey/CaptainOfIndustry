// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.ProductType
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  public struct ProductType : IEquatable<ProductType>
  {
    public static readonly ProductType ANY;
    public static readonly ProductType NONE;
    private readonly Type m_protoType;

    public ProductType(Type protoType)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<Type>(protoType).IsNotNull<Type>();
      this.m_protoType = protoType;
    }

    public static bool operator ==(ProductType pt1, ProductType pt2)
    {
      if (pt1.m_protoType == (Type) null || pt2.m_protoType == (Type) null)
        return false;
      return pt1.m_protoType == typeof (ProductType) || pt2.m_protoType == typeof (ProductType) || (object) pt1.m_protoType == (object) pt2.m_protoType;
    }

    public static bool operator !=(ProductType pt1, ProductType pt2) => !(pt1 == pt2);

    public bool Equals(ProductType other) => this == other;

    public override bool Equals(object obj) => obj is ProductType other && this.Equals(other);

    public override int GetHashCode()
    {
      Type protoType = this.m_protoType;
      return (object) protoType == null ? 0 : protoType.GetHashCode();
    }

    public override string ToString()
    {
      return this.m_protoType == typeof (ProductType) ? "ANY" : this.m_protoType?.Name ?? "NONE";
    }

    static ProductType()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductType.ANY = new ProductType(typeof (ProductType));
      ProductType.NONE = new ProductType();
    }
  }
}
