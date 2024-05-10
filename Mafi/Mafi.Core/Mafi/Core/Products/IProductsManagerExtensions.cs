// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.IProductsManagerExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Recipes;
using System;
using System.Threading;

#nullable disable
namespace Mafi.Core.Products
{
  public static class IProductsManagerExtensions
  {
    private static readonly ThreadLocal<Lyst<ProductQuantity>> s_inputsCache;
    private static readonly ThreadLocal<Lyst<ProductQuantity>> s_outputsCache;

    public static void ProductCreated(
      this IProductsManager mgr,
      ProductQuantity productQuantity,
      CreateReason reason)
    {
      if (productQuantity.IsEmpty)
        return;
      mgr.ProductCreated(productQuantity.Product, productQuantity.Quantity, reason);
    }

    public static void ProductCreated(
      this IProductsManager mgr,
      AssetValue value,
      CreateReason reason)
    {
      foreach (ProductQuantity product in value.Products)
        mgr.ProductCreated(product.Product, product.Quantity, reason);
    }

    public static void ProductDestroyed(
      this IProductsManager mgr,
      ProductQuantity productQuantity,
      DestroyReason reason)
    {
      if (productQuantity.IsEmpty)
        return;
      mgr.ProductDestroyed(productQuantity.Product, productQuantity.Quantity, reason);
    }

    public static void ProductDestroyed(
      this IProductsManager mgr,
      RecipeProduct processProduct,
      DestroyReason reason)
    {
      mgr.ProductDestroyed(processProduct.Product, processProduct.Quantity, reason);
    }

    public static void ProductDestroyed(
      this IProductsManager mgr,
      AssetValue value,
      DestroyReason reason)
    {
      foreach (ProductQuantity product in value.Products)
        mgr.ProductDestroyed(product.Product, product.Quantity, reason);
    }

    public static void ClearBufferAndReportProducts(
      this IProductsManager mgr,
      ProductBuffer buffer,
      DestroyReason reason)
    {
      mgr.ProductDestroyed(buffer.Product, buffer.Quantity, reason);
      buffer.Clear();
    }

    public static void ClearBuffersAndReportProducts(
      this IProductsManager mgr,
      ImmutableArray<ProductBuffer> buffers,
      DestroyReason reason)
    {
      foreach (ProductBuffer buffer in buffers)
      {
        mgr.ProductDestroyed(buffer.Product, buffer.Quantity, reason);
        buffer.Clear();
      }
    }

    public static void ClearProduct(this IProductsManager mgr, ProductQuantity productQuantity)
    {
      if (productQuantity.IsEmpty)
        return;
      mgr.ClearProduct(productQuantity.Product, productQuantity.Quantity);
    }

    public static void ClearProductNoChecks(
      this IProductsManager mgr,
      ProductQuantity productQuantity)
    {
      if (productQuantity.IsEmpty)
        return;
      mgr.ClearProductNoChecks(productQuantity.Product, productQuantity.Quantity);
    }

    /// <summary>
    /// Informs the manager of products transformation. Typically done by a machine. All quantities will get
    /// reported and also source products get transferred.
    /// </summary>
    public static void ReportProductsTransformation(
      this IProductsManager mgr,
      ProductQuantity input,
      ProductQuantity output,
      DestroyReason destroyReason,
      CreateReason createReason)
    {
      Lyst<ProductQuantity> inputs = IProductsManagerExtensions.s_inputsCache.Value.ClearAndReturn();
      inputs.Add(input);
      Lyst<ProductQuantity> outputs = IProductsManagerExtensions.s_outputsCache.Value.ClearAndReturn();
      outputs.Add(output);
      mgr.ReportProductsTransformation((IIndexable<ProductQuantity>) inputs, (IIndexable<ProductQuantity>) outputs, destroyReason, createReason);
    }

    static IProductsManagerExtensions()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IProductsManagerExtensions.s_inputsCache = new ThreadLocal<Lyst<ProductQuantity>>((Func<Lyst<ProductQuantity>>) (() => new Lyst<ProductQuantity>(true)));
      IProductsManagerExtensions.s_outputsCache = new ThreadLocal<Lyst<ProductQuantity>>((Func<Lyst<ProductQuantity>>) (() => new Lyst<ProductQuantity>(true)));
    }
  }
}
