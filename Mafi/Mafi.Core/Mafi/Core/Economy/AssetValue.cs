// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Economy.AssetValue
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Economy
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct AssetValue : IEquatable<AssetValue>
  {
    public static readonly AssetValue Empty;
    /// <summary>Cache for usage in mathematical operations.</summary>
    [ThreadStatic]
    private static Lyst<ProductQuantity> s_productsCache;
    public readonly SmallImmutableArray<ProductQuantity> Products;

    public static AssetValue FromProductId(ProductProto.ID productId, int quantity, ProtosDb db)
    {
      return new AssetValue(new ProductQuantity(db.GetOrThrow<ProductProto>((Proto.ID) productId), new Quantity(quantity)));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsEmpty => this.Products.IsEmpty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsNotEmpty => !this.IsEmpty;

    [LoadCtor]
    public AssetValue(SmallImmutableArray<ProductQuantity> products)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Products = products.CheckNotDefaultStruct<SmallImmutableArray<ProductQuantity>>();
    }

    public AssetValue(ImmutableArray<ProductQuantity> products)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new AssetValue(new SmallImmutableArray<ProductQuantity>(products));
    }

    public AssetValue(ProductQuantity product)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new AssetValue(product.IsEmpty ? SmallImmutableArray<ProductQuantity>.Empty : new SmallImmutableArray<ProductQuantity>(product));
    }

    public AssetValue(ProductQuantity product1, ProductQuantity product2)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new AssetValue(new SmallImmutableArray<ProductQuantity>(ImmutableArray.Create<ProductQuantity>(product1, product2)));
    }

    public AssetValue(ProductQuantity product1, ProductQuantity product2, ProductQuantity product3)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new AssetValue(new SmallImmutableArray<ProductQuantity>(ImmutableArray.Create<ProductQuantity>(product1, product2, product3)));
    }

    public AssetValue(
      ProductQuantity p1,
      ProductQuantity p2,
      ProductQuantity p3,
      ProductQuantity p4)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new AssetValue(new SmallImmutableArray<ProductQuantity>(ImmutableArray.Create<ProductQuantity>(p1, p2, p3, p4)));
    }

    public AssetValue(ProductProto product, Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new AssetValue(quantity.IsNotPositive ? SmallImmutableArray<ProductQuantity>.Empty : new SmallImmutableArray<ProductQuantity>(new ProductQuantity(product, quantity)));
    }

    public Quantity this[ProductProto product]
    {
      get
      {
        foreach (ProductQuantity product1 in this.Products)
        {
          if ((Proto) product1.Product == (Proto) product)
            return product1.Quantity;
        }
        return Quantity.Zero;
      }
    }

    [Pure]
    public AssetValue ScaledBy(Percent percent)
    {
      Lyst<ProductQuantity> staticProductsCache = AssetValue.getThreadStaticProductsCache();
      foreach (ProductQuantity product in this.Products)
      {
        ProductQuantity productQuantity = product.WithNewQuantity(product.Quantity.ScaledBy(percent));
        if (productQuantity.IsNotEmpty)
          staticProductsCache.Add(productQuantity);
      }
      return new AssetValue(staticProductsCache.ToSmallImmutableArrayAndClear());
    }

    [Pure]
    public AssetValue ScaledByCeiled(Percent percent)
    {
      Lyst<ProductQuantity> staticProductsCache = AssetValue.getThreadStaticProductsCache();
      foreach (ProductQuantity product in this.Products)
      {
        ProductQuantity productQuantity = product.WithNewQuantity(product.Quantity.ScaledByCeiled(percent));
        if (productQuantity.IsNotEmpty)
          staticProductsCache.Add(productQuantity);
      }
      return new AssetValue(staticProductsCache.ToSmallImmutableArrayAndClear());
    }

    [Pure]
    public AssetValue TakePositiveValuesOnly()
    {
      return new AssetValue(this.Products.Filter((Predicate<ProductQuantity>) (x => x.Quantity.IsPositive)));
    }

    [Pure]
    public AssetValue TakeNonVirtualOnly()
    {
      return new AssetValue(this.Products.Filter((Predicate<ProductQuantity>) (x => x.Product.Type != VirtualProductProto.ProductType)));
    }

    private static Lyst<ProductQuantity> getThreadStaticProductsCache()
    {
      if (AssetValue.s_productsCache == null)
        AssetValue.s_productsCache = new Lyst<ProductQuantity>(true);
      Mafi.Assert.That<Lyst<ProductQuantity>>(AssetValue.s_productsCache).IsEmpty<ProductQuantity>();
      return AssetValue.s_productsCache;
    }

    public static AssetValue operator *(AssetValue price, int times)
    {
      return new AssetValue(price.Products.Map<ProductQuantity>((Func<ProductQuantity, ProductQuantity>) (x => x.WithNewQuantity(x.Quantity * times))));
    }

    public static AssetValue operator -(AssetValue minuend, AssetValue subtrahend)
    {
      if (subtrahend.IsEmpty)
        return minuend;
      Lyst<ProductQuantity> staticProductsCache = AssetValue.getThreadStaticProductsCache();
      staticProductsCache.AddRange(minuend.Products);
      for (int index = 0; index < subtrahend.Products.Length; ++index)
      {
        ProductQuantity product = subtrahend.Products[index];
        int productIndexOf = AssetValue.getProductIndexOf(staticProductsCache, product.Product, index);
        if (productIndexOf < 0)
        {
          staticProductsCache.Add(new ProductQuantity(product.Product, -product.Quantity));
        }
        else
        {
          ProductQuantity productQuantity = staticProductsCache[productIndexOf];
          if (product.Quantity == productQuantity.Quantity)
            staticProductsCache.RemoveAtReplaceWithLast(productIndexOf);
          else
            staticProductsCache[productIndexOf] = productQuantity - product;
        }
      }
      return new AssetValue(staticProductsCache.ToSmallImmutableArrayAndClear());
    }

    public static AssetValue operator -(AssetValue minuend, ProductQuantity subtrahend)
    {
      if (subtrahend.IsEmpty)
        return minuend;
      Lyst<ProductQuantity> staticProductsCache = AssetValue.getThreadStaticProductsCache();
      staticProductsCache.AddRange(minuend.Products);
      int productIndexOf = AssetValue.getProductIndexOf(staticProductsCache, subtrahend.Product, 0);
      if (productIndexOf < 0)
      {
        staticProductsCache.Add(new ProductQuantity(subtrahend.Product, -subtrahend.Quantity));
      }
      else
      {
        ProductQuantity productQuantity = staticProductsCache[productIndexOf];
        if (subtrahend.Quantity == productQuantity.Quantity)
          staticProductsCache.RemoveAtReplaceWithLast(productIndexOf);
        else
          staticProductsCache[productIndexOf] = productQuantity - subtrahend;
      }
      return new AssetValue(staticProductsCache.ToSmallImmutableArrayAndClear());
    }

    public static AssetValue operator +(AssetValue a, AssetValue b)
    {
      if (b.IsEmpty)
        return a;
      if (a.IsEmpty)
        return b;
      Lyst<ProductQuantity> staticProductsCache = AssetValue.getThreadStaticProductsCache();
      staticProductsCache.AddRange(a.Products);
      for (int index = 0; index < b.Products.Length; ++index)
      {
        ProductQuantity product = b.Products[index];
        int productIndexOf = AssetValue.getProductIndexOf(staticProductsCache, product.Product, index);
        if (productIndexOf < 0)
          staticProductsCache.Add(product);
        else
          staticProductsCache[productIndexOf] += product;
      }
      return new AssetValue(staticProductsCache.ToSmallImmutableArrayAndClear());
    }

    private static int getProductIndexOf(
      Lyst<ProductQuantity> productQuantities,
      ProductProto product,
      int indexHint)
    {
      if (indexHint < productQuantities.Count && (Proto) productQuantities[indexHint].Product == (Proto) product)
        return indexHint;
      for (int index = 0; index < productQuantities.Count; ++index)
      {
        if ((Proto) productQuantities[index].Product == (Proto) product)
          return index;
      }
      return -1;
    }

    [Pure]
    public AssetValue FloorDiv(int divider)
    {
      return new AssetValue(this.Products.Map<ProductQuantity>((Func<ProductQuantity, ProductQuantity>) (x => x.WithNewQuantity(x.Quantity.FloorDiv(divider)))).Filter((Predicate<ProductQuantity>) (x => x.Quantity.IsPositive)));
    }

    [Pure]
    public AssetValue Apply(Percent multiplier)
    {
      return new AssetValue(this.Products.Map<ProductQuantity>((Func<ProductQuantity, ProductQuantity>) (x => x.WithNewQuantity(multiplier.Apply(x.Quantity.Value).Quantity()))).Filter((Predicate<ProductQuantity>) (x => x.Quantity.IsPositive)));
    }

    [Pure]
    public AssetValue CeilDiv(int divider)
    {
      return new AssetValue(this.Products.Map<ProductQuantity>((Func<ProductQuantity, ProductQuantity>) (x => x.WithNewQuantity(x.Quantity.CeilDiv(divider)))));
    }

    [Pure]
    public AssetValue RoundDiv(int divider)
    {
      return new AssetValue(this.Products.Map<ProductQuantity>((Func<ProductQuantity, ProductQuantity>) (x => x.WithNewQuantity(x.Quantity.RoundDiv(divider)))).Filter((Predicate<ProductQuantity>) (x => x.Quantity.IsPositive)));
    }

    [Pure]
    public AssetValue Mul(int multiplier)
    {
      return new AssetValue(this.Products.Map<ProductQuantity>((Func<ProductQuantity, ProductQuantity>) (x => x.WithNewQuantity(x.Quantity * multiplier))));
    }

    [Pure]
    public Quantity GetQuantityOf(ProductProto product)
    {
      foreach (ProductQuantity product1 in this.Products)
      {
        if ((Proto) product1.Product == (Proto) product)
          return product1.Quantity;
      }
      return Quantity.Zero;
    }

    [Pure]
    public Quantity GetQuantitySum()
    {
      Quantity zero = Quantity.Zero;
      foreach (ProductQuantity product in this.Products)
        zero += product.Quantity;
      return zero;
    }

    public static bool operator ==(AssetValue left, AssetValue right)
    {
      if (left.Products.Length != right.Products.Length)
        return false;
      for (int index = 0; index < left.Products.Length; ++index)
      {
        ProductQuantity product = left.Products[index];
        if (!(right.Products[index] == product) && right.Products.IndexOf(product) < 0)
          return false;
      }
      return true;
    }

    public static bool operator !=(AssetValue left, AssetValue right) => !(left == right);

    public bool Equals(AssetValue other) => this == other;

    public override bool Equals(object obj) => obj is AssetValue other && this.Equals(other);

    public override int GetHashCode()
    {
      if (this.Products.IsEmpty)
        return 0;
      int t = this.Products[0].GetHashCode();
      for (int index = 1; index < this.Products.Length; ++index)
        t = Hash.Combine<int, ProductQuantity>(t, this.Products[index]);
      return t;
    }

    public override string ToString()
    {
      return !this.Products.IsEmpty ? this.Products.Join(", ", (Func<ProductQuantity, string>) (x => x.ToString())) : "Empty";
    }

    public static void Serialize(AssetValue value, BlobWriter writer)
    {
      SmallImmutableArray<ProductQuantity>.Serialize(value.Products, writer);
    }

    public static AssetValue Deserialize(BlobReader reader)
    {
      return new AssetValue(SmallImmutableArray<ProductQuantity>.Deserialize(reader));
    }

    static AssetValue()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssetValue.Empty = new AssetValue(SmallImmutableArray<ProductQuantity>.Empty);
    }
  }
}
