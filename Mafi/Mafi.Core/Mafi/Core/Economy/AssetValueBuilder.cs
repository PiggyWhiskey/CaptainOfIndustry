// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Economy.AssetValueBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Economy
{
  public class AssetValueBuilder
  {
    [ThreadStatic]
    private static ObjectPool2<AssetValueBuilder> s_pool;
    private readonly Lyst<ProductQuantity> m_products;

    private AssetValueBuilder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_products = new Lyst<ProductQuantity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public static AssetValueBuilder GetPooledInstance()
    {
      if (AssetValueBuilder.s_pool == null)
        AssetValueBuilder.s_pool = new ObjectPool2<AssetValueBuilder>(4, (Func<ObjectPool2<AssetValueBuilder>, AssetValueBuilder>) (pool => new AssetValueBuilder()));
      AssetValueBuilder instance = AssetValueBuilder.s_pool.GetInstance();
      Assert.That<Lyst<ProductQuantity>>(instance.m_products).IsEmpty<ProductQuantity>();
      return instance;
    }

    public void Add(ProductProto product, Quantity quantity)
    {
      this.Add(new ProductQuantity(product, quantity));
    }

    public void Add(ProductQuantity pq)
    {
      if (pq.Quantity.IsNotPositive)
        return;
      for (int index = 0; index < this.m_products.Count; ++index)
      {
        ProductQuantity product = this.m_products[index];
        if ((Proto) product.Product == (Proto) pq.Product)
        {
          this.m_products[index] = new ProductQuantity(pq.Product, pq.Quantity + product.Quantity);
          return;
        }
      }
      this.m_products.AddIfNotPresent(pq);
    }

    public AssetValue GetAssetValueAndReturnToPool()
    {
      AssetValue valueAndReturnToPool = new AssetValue(this.m_products.ToSmallImmutableArrayAndClear());
      AssetValueBuilder assetValueBuilder = this;
      AssetValueBuilder.s_pool.ReturnInstance(ref assetValueBuilder);
      return valueAndReturnToPool;
    }
  }
}
