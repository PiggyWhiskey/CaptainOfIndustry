// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ResVis.ResVisBarsRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.GameLoop;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.ResVis
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ResVisBarsRenderer
  {
    private static readonly RelTile2i SAMPLED_TILE_IN_DESIGNATOR;
    private static readonly RelTile2i SAMPLED_TILE_IN_DESIGNATOR_VIRTUAL;
    private static readonly RelTile1f RES_BAR_SIZE;
    private static readonly RelTile1f VIRTUAL_RES_BAR_SIZE;
    private static readonly ThicknessTilesF MIN_BAR_SIZE;
    private readonly GameObject m_parentGo;
    private readonly TerrainResourcesProvider m_provider;
    private readonly AssetsDb m_assetsDb;
    private readonly TerrainManager m_terrainManager;
    private readonly VirtualResourceManager m_virtualResourceManager;
    private LystStruct<Chunk2i> m_dirtyChunks;
    private readonly Dict<ProductProto, Material> m_materials;
    private readonly Dict<Chunk2i, Dict<ProductProto, ResVisBarsMb>> m_resourcesMbs;
    private readonly Dict<Chunk2i, ResVisBarsMb> m_otherMbs;
    private readonly BitMap m_dirtyChunksMap;
    private readonly Dict<ProductProto, Set<Chunk2i>> m_chunksWithProducts;
    private readonly Dict<ProductProto, bool> m_lastVisible;
    private readonly Dict<IVirtualTerrainResource, Quantity> m_lastVirtualResourceQuantities;
    private readonly Material m_otherMat;
    /// <summary>
    /// Visibility counters for each products. If a counter for a given product is positive, the product is visible
    /// otherwise it is not.
    /// </summary>
    private readonly Dict<ProductProto, int> m_productsVisibility;
    /// <summary>
    /// Sum of all visibility counters in <see cref="F:Mafi.Unity.InputControl.ResVis.ResVisBarsRenderer.m_productsVisibility" /> for quick checking whether any product
    /// is visible.
    /// </summary>
    private int m_productsVisibilitySum;
    private LystStruct<ResVisBarsRenderer.ProductThickness> m_tileProductsCache;
    private readonly Lyst<ProductVirtualResource> m_virtualResourceCache;
    private readonly Stopwatch m_stopwatch;

    /// <summary>Whether the displaying is currently active.</summary>
    public bool IsActive { get; private set; }

    internal ResVisBarsRenderer(
      IGameLoopEvents gameLoopEvents,
      TerrainResourcesProvider provider,
      AssetsDb assetsDb,
      TerrainManager terrainManager,
      VirtualResourceManager virtualResourceManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_materials = new Dict<ProductProto, Material>();
      this.m_resourcesMbs = new Dict<Chunk2i, Dict<ProductProto, ResVisBarsMb>>();
      this.m_otherMbs = new Dict<Chunk2i, ResVisBarsMb>();
      this.m_chunksWithProducts = new Dict<ProductProto, Set<Chunk2i>>();
      this.m_lastVisible = new Dict<ProductProto, bool>();
      this.m_lastVirtualResourceQuantities = new Dict<IVirtualTerrainResource, Quantity>();
      this.m_tileProductsCache = new LystStruct<ResVisBarsRenderer.ProductThickness>(64);
      this.m_virtualResourceCache = new Lyst<ProductVirtualResource>(64, true);
      this.m_stopwatch = new Stopwatch();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_parentGo = new GameObject("ResourcesVisMeshes");
      this.m_provider = provider;
      this.m_assetsDb = assetsDb;
      this.m_terrainManager = terrainManager;
      this.m_virtualResourceManager = virtualResourceManager;
      this.m_dirtyChunksMap = new BitMap(this.m_terrainManager.Chunk64TotalCount);
      foreach (LooseProductProto key in provider.LooseTerrainProducts.All())
      {
        Material clonedMaterial = assetsDb.GetClonedMaterial("Assets/Core/Materials/ResVisBarsMaterial.mat");
        clonedMaterial.SetColor("_Color", key.Graphics.ResourcesVizColor.ToColor());
        this.m_materials[(ProductProto) key] = clonedMaterial;
      }
      foreach (VirtualResourceProductProto resourceProductProto in provider.VirtualResourceProducts.All())
      {
        Material clonedMaterial = assetsDb.GetClonedMaterial("Assets/Core/Materials/ResVisBarsMaterial.mat");
        clonedMaterial.SetColor("_Color", resourceProductProto.Graphics.ResourcesVizColor.SetA((byte) 0).ToColor());
        this.m_materials[resourceProductProto.Product] = clonedMaterial;
      }
      this.m_otherMat = assetsDb.GetClonedMaterial("Assets/Core/Materials/ResVisBarsMaterial.mat");
      this.m_otherMat.SetColor("_Color", Color.gray);
      this.m_productsVisibility = ((IEnumerable<ProductProto>) provider.LooseTerrainProducts.All()).Concat<ProductProto>(provider.VirtualResourceProducts.All().Select<VirtualResourceProductProto, ProductProto>((Func<VirtualResourceProductProto, ProductProto>) (x => x.Product))).ToDict<ProductProto, ProductProto, int>((Func<ProductProto, ProductProto>) (x => x), (Func<ProductProto, int>) (x => 0));
      terrainManager.HeightChanged.AddNonSaveable<ResVisBarsRenderer>(this, new Action<Tile2iAndIndex>(this.heightChanged));
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
    }

    private void initState()
    {
      this.m_stopwatch.Restart();
      foreach (ProductProto product in this.m_provider.LooseTerrainProducts.All())
        this.increaseVisibilityOf(product);
      foreach (VirtualResourceProductProto resourceProductProto in this.m_provider.VirtualResourceProducts.All())
        this.increaseVisibilityOf(resourceProductProto.Product);
      this.m_dirtyChunks.Clear();
      this.m_dirtyChunks.AddRange(this.m_terrainManager.TerrainAreaChunks.EnumerateChunks());
      this.updateResourcesBars();
      foreach (ProductProto product in this.m_provider.LooseTerrainProducts.All())
        this.decreaseVisibilityOf(product);
      foreach (VirtualResourceProductProto resourceProductProto in this.m_provider.VirtualResourceProducts.All())
        this.decreaseVisibilityOf(resourceProductProto.Product);
      if (this.IsActive)
        return;
      this.hide();
    }

    public ResVisBarsRenderer.Activator CreateActivator() => new ResVisBarsRenderer.Activator(this);

    internal void ForceSetActive(bool isActive) => this.m_parentGo.SetActive(isActive);

    private void hide()
    {
      this.m_parentGo.SetActive(false);
      this.IsActive = false;
    }

    private void show()
    {
      this.m_parentGo.SetActive(true);
      this.IsActive = true;
    }

    private void updateVisibility()
    {
      Mafi.Assert.That<int>(this.m_productsVisibilitySum).IsNotNegative();
      if (this.m_productsVisibilitySum == 0)
      {
        if (!this.IsActive)
          return;
        this.hide();
      }
      else
      {
        if (!this.IsActive)
          this.show();
        foreach (ProductProto key in this.m_materials.Keys)
        {
          Set<Chunk2i> set;
          if (this.isProductVisible(key) != CollectionExtensions.GetValueOrDefault<ProductProto, bool>((IReadOnlyDictionary<ProductProto, bool>) this.m_lastVisible, key, false) && this.m_chunksWithProducts.TryGetValue(key, out set))
          {
            foreach (Chunk2i chunk in set)
            {
              if (this.m_dirtyChunksMap.SetBitReportChanged(this.m_terrainManager.GetChunk64Index(chunk).Value))
                this.m_dirtyChunks.Add(chunk);
            }
          }
        }
        foreach (VirtualResourceProductProto resourceProductProto in this.m_provider.VirtualResourceProducts.All())
        {
          Set<Chunk2i> set;
          if (this.isProductVisible(resourceProductProto.Product) != CollectionExtensions.GetValueOrDefault<ProductProto, bool>((IReadOnlyDictionary<ProductProto, bool>) this.m_lastVisible, resourceProductProto.Product, false) && this.m_chunksWithProducts.TryGetValue(resourceProductProto.Product, out set))
          {
            foreach (Chunk2i chunk in set)
            {
              if (this.m_dirtyChunksMap.SetBitReportChanged(this.m_terrainManager.GetChunk64Index(chunk).Value))
                this.m_dirtyChunks.Add(chunk);
            }
          }
        }
        if (!this.m_dirtyChunks.IsNotEmpty)
          return;
        this.updateResourcesBars();
      }
    }

    private void heightChanged(Tile2iAndIndex tileAndIndex)
    {
      Chunk2i chunkCoord2i = tileAndIndex.ChunkCoord2i;
      if (!this.m_dirtyChunksMap.SetBitReportChanged(this.m_terrainManager.GetChunk64Index(chunkCoord2i).Value))
        return;
      this.m_dirtyChunks.Add(chunkCoord2i);
    }

    private void updateResourcesBars()
    {
      this.m_stopwatch.Restart();
      int updatedCount = 0;
      foreach (VirtualResourceProductProto product in this.m_provider.VirtualResourceProducts.All())
      {
        foreach (IVirtualTerrainResource key in this.m_virtualResourceManager.GetAllResourcesFor(product))
        {
          if (!(key.Quantity == CollectionExtensions.GetValueOrDefault<IVirtualTerrainResource, Quantity>((IReadOnlyDictionary<IVirtualTerrainResource, Quantity>) this.m_lastVirtualResourceQuantities, key, Quantity.MinValue)))
          {
            this.m_lastVirtualResourceQuantities[key] = key.Quantity;
            foreach (Chunk2i enumerateChunk in new RectangleTerrainArea2i(key.Position.Xy - key.MaxRadius.Value, 2 * new RelTile2i(key.MaxRadius, key.MaxRadius)).EnumerateChunks())
            {
              if (this.m_dirtyChunksMap.SetBitReportChanged(this.m_terrainManager.GetChunk64Index(enumerateChunk).Value))
                this.m_dirtyChunks.Add(enumerateChunk);
            }
          }
        }
      }
      foreach (Chunk2i dirtyChunk in this.m_dirtyChunks)
      {
        updateChunk(dirtyChunk);
        this.m_dirtyChunksMap.ClearBitsAround(this.m_terrainManager.GetChunk64Index(dirtyChunk).Value);
      }
      this.m_dirtyChunks.Clear();
      this.m_lastVisible.Clear();
      foreach (ProductProto key in this.m_materials.Keys)
        this.m_lastVisible[key] = this.isProductVisible(key);

      void updateChunk(Chunk2i chunk)
      {
        Dict<ProductProto, ResVisBarsMb> dict;
        if (this.m_resourcesMbs.TryGetValue(chunk, out dict))
        {
          foreach (Component component in dict.Values)
            UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
          dict.Clear();
          this.m_resourcesMbs.Remove(chunk);
        }
        ResVisBarsMb resVisBarsMb1;
        if (this.m_otherMbs.TryGetValue(chunk, out resVisBarsMb1))
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) resVisBarsMb1.gameObject);
          this.m_otherMbs.Remove(chunk);
        }
        this.addResourceBarDataOnChunk(chunk);
        this.addVirtualResourceBarsData(chunk);
        if (this.m_resourcesMbs.TryGetValue(chunk, out dict))
        {
          foreach (ResVisBarsMb resVisBarsMb2 in dict.Values)
            resVisBarsMb2.ApplyChanges();
        }
        if (this.m_otherMbs.TryGetValue(chunk, out resVisBarsMb1))
          resVisBarsMb1.ApplyChanges();
        ++updatedCount;
      }
    }

    public void InvalidateAllResourceBars()
    {
      foreach (Chunk2i enumerateChunk in this.m_terrainManager.TerrainAreaChunks.EnumerateChunks())
      {
        if (this.m_dirtyChunksMap.SetBitReportChanged(this.m_terrainManager.GetChunk64Index(enumerateChunk).Value))
          this.m_dirtyChunks.Add(enumerateChunk);
      }
      this.updateResourcesBars();
    }

    private void addResourceBarDataOnChunk(Chunk2i chunk)
    {
      TerrainManager terrainManager = this.m_terrainManager;
      foreach (ProductProto key in this.m_materials.Keys)
      {
        Set<Chunk2i> set;
        if (this.m_chunksWithProducts.TryGetValue(key, out set))
          set.Remove(chunk);
      }
      for (int y = chunk.Tile2i.Y + ResVisBarsRenderer.SAMPLED_TILE_IN_DESIGNATOR.Y; y < chunk.Tile2i.Y + 64; y += 8)
      {
        for (int x = chunk.Tile2i.X + ResVisBarsRenderer.SAMPLED_TILE_IN_DESIGNATOR.X; x < chunk.Tile2i.X + 64; x += 8)
        {
          Tile2iAndIndex tile2iAndIndex = terrainManager.ExtendTileIndex(x, y);
          if (!terrainManager.IsOcean(tile2iAndIndex.Index))
          {
            this.m_tileProductsCache.ClearSkipZeroingMemory();
            int num = -1;
            foreach (TerrainMaterialThicknessSlim enumerateLayer in terrainManager.EnumerateLayers(tile2iAndIndex.Index))
            {
              if (!(enumerateLayer.Thickness < ResVisBarsRenderer.MIN_BAR_SIZE))
              {
                LooseProductProto minedProduct = enumerateLayer.SlimId.ToFull(terrainManager).MinedProduct;
                this.m_tileProductsCache.Add(new ResVisBarsRenderer.ProductThickness((ProductProto) minedProduct, enumerateLayer.Thickness));
                if (this.m_materials.ContainsKey((ProductProto) minedProduct))
                {
                  Set<Chunk2i> set;
                  if (!this.m_chunksWithProducts.TryGetValue((ProductProto) minedProduct, out set))
                  {
                    set = new Set<Chunk2i>();
                    this.m_chunksWithProducts.Add((ProductProto) minedProduct, set);
                  }
                  set.Add(chunk);
                }
                if (this.isProductVisible((ProductProto) minedProduct))
                  num = this.m_tileProductsCache.Count - 1;
              }
            }
            if (num != -1)
            {
              HeightTilesF height = terrainManager.GetHeight(tile2iAndIndex.Index);
              for (int index = num; index >= 0; --index)
              {
                ResVisBarsRenderer.ProductThickness productThickness = this.m_tileProductsCache[index];
                if (this.m_materials.ContainsKey(productThickness.Product))
                {
                  Dict<ProductProto, ResVisBarsMb> dict;
                  if (!this.m_resourcesMbs.TryGetValue(chunk, out dict))
                  {
                    dict = new Dict<ProductProto, ResVisBarsMb>();
                    this.m_resourcesMbs[chunk] = dict;
                  }
                  ResVisBarsMb instance;
                  if (!dict.TryGetValue(productThickness.Product, out instance))
                  {
                    instance = ResVisBarsMb.CreateInstance(chunk.ToString() + productThickness.Product.Id.Value, this.m_materials[productThickness.Product], this.m_parentGo, ResVisBarsRenderer.RES_BAR_SIZE / 2);
                    dict.Add(productThickness.Product, instance);
                  }
                  height += instance.AppendBar(tile2iAndIndex.TileCoord, productThickness.Thickness, height);
                }
                else
                {
                  ResVisBarsMb instance;
                  if (!this.m_otherMbs.TryGetValue(chunk, out instance))
                  {
                    instance = ResVisBarsMb.CreateInstance(chunk.ToString() + "other", this.m_otherMat, this.m_parentGo, ResVisBarsRenderer.RES_BAR_SIZE / 3);
                    this.m_otherMbs.Add(chunk, instance);
                  }
                  height += instance.AppendBar(tile2iAndIndex.TileCoord, productThickness.Thickness, height);
                }
              }
            }
          }
        }
      }
    }

    private void addVirtualResourceBarsData(Chunk2i chunk)
    {
      TerrainManager terrainManager = this.m_terrainManager;
      int num1 = chunk.Tile2i.X + ResVisBarsRenderer.SAMPLED_TILE_IN_DESIGNATOR_VIRTUAL.X;
      int num2 = chunk.Tile2i.Y + ResVisBarsRenderer.SAMPLED_TILE_IN_DESIGNATOR_VIRTUAL.Y;
      int num3 = num1 + 64;
      int num4 = num2 + 64;
      for (int y = num2; y < num4; y += 4)
      {
        for (int x = num1; x < num3; x += 4)
        {
          this.m_virtualResourceCache.Clear();
          Tile2iAndIndex tile2iAndIndex = terrainManager.ExtendTileIndex(x, y);
          this.m_virtualResourceManager.AddAvailableTileResourcesToLyst(tile2iAndIndex.TileCoord, this.m_virtualResourceCache);
          HeightTilesF height = terrainManager.IsOcean(tile2iAndIndex.Index) ? HeightTilesF.Zero : terrainManager.GetHeight(tile2iAndIndex.Index);
          foreach (ProductVirtualResource productVirtualResource in this.m_virtualResourceCache)
          {
            ProductProto product = productVirtualResource.Product.Product;
            if (this.m_materials.ContainsKey(product))
            {
              Set<Chunk2i> set;
              if (!this.m_chunksWithProducts.TryGetValue(product, out set))
              {
                set = new Set<Chunk2i>();
                this.m_chunksWithProducts.Add(product, set);
              }
              set.Add(chunk);
            }
            if (this.isProductVisible(product))
            {
              Dict<ProductProto, ResVisBarsMb> dict;
              if (!this.m_resourcesMbs.TryGetValue(chunk, out dict))
              {
                dict = new Dict<ProductProto, ResVisBarsMb>();
                this.m_resourcesMbs[chunk] = dict;
              }
              ResVisBarsMb instance;
              if (!dict.TryGetValue(product, out instance))
              {
                instance = ResVisBarsMb.CreateInstance(chunk.ToString() + productVirtualResource.Product.Id.Value, this.m_materials[product], this.m_parentGo, ResVisBarsRenderer.VIRTUAL_RES_BAR_SIZE / 2, useCylinders: true);
                dict.Add(product, instance);
              }
              height += instance.AppendBar(tile2iAndIndex.TileCoord, productVirtualResource.VirtualThickness, height);
            }
          }
        }
      }
    }

    private bool isProductVisible(ProductProto product)
    {
      int num;
      return this.m_productsVisibility.TryGetValue(product, out num) && num > 0;
    }

    private void increaseVisibilityOf(ProductProto product)
    {
      int num;
      if (this.m_productsVisibility.TryGetValue(product, out num))
      {
        this.m_productsVisibility[product] = num + 1;
        ++this.m_productsVisibilitySum;
      }
      else
        Mafi.Log.Error(string.Format("incProductVisibility: Terrain product {0} cannot be visualized.", (object) product));
    }

    private void decreaseVisibilityOf(ProductProto product)
    {
      int num;
      if (this.m_productsVisibility.TryGetValue(product, out num))
      {
        Mafi.Assert.That<int>(num).IsPositive();
        Mafi.Assert.That<int>(this.m_productsVisibilitySum).IsPositive();
        this.m_productsVisibility[product] = num - 1;
        --this.m_productsVisibilitySum;
      }
      else
        Mafi.Log.Error(string.Format("incProductVisibility: Terrain product {0} cannot be visualized.", (object) product));
    }

    static ResVisBarsRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ResVisBarsRenderer.SAMPLED_TILE_IN_DESIGNATOR = TerrainDesignation.Size / 2;
      ResVisBarsRenderer.SAMPLED_TILE_IN_DESIGNATOR_VIRTUAL = new RelTile2i(0, 0);
      ResVisBarsRenderer.RES_BAR_SIZE = new RelTile1f(2);
      ResVisBarsRenderer.VIRTUAL_RES_BAR_SIZE = new RelTile1f(1);
      ResVisBarsRenderer.MIN_BAR_SIZE = new ThicknessTilesF(1);
    }

    private struct ProductThickness
    {
      public readonly ProductProto Product;
      public readonly ThicknessTilesF Thickness;

      public ProductThickness(ProductProto product, ThicknessTilesF thickness)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Product = product;
        this.Thickness = thickness;
      }
    }

    public class Activator
    {
      public readonly ResVisBarsRenderer Renderer;
      private readonly Set<ProductProto> m_activatedProducts;

      public bool AnyActive => this.m_activatedProducts.IsNotEmpty;

      internal Activator(ResVisBarsRenderer renderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_activatedProducts = new Set<ProductProto>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Renderer = renderer.CheckNotNull<ResVisBarsRenderer>();
      }

      public void Show(ProductProto product)
      {
        if (this.m_activatedProducts.Add(product))
        {
          this.Renderer.increaseVisibilityOf(product);
          this.Renderer.updateVisibility();
        }
        else
          Mafi.Assert.Fail(string.Format("Terrain material {0} already shown.", (object) product));
      }

      public void Hide(ProductProto product)
      {
        if (this.m_activatedProducts.Remove(product))
        {
          this.Renderer.decreaseVisibilityOf(product);
          this.Renderer.updateVisibility();
        }
        else
          Mafi.Assert.Fail(string.Format("Terrain material {0} not shown.", (object) product));
      }

      public void ShowExactly(IEnumerable<ProductProto> products)
      {
        this.hideActivatedProductsAndClear();
        foreach (ProductProto product in products)
        {
          if (this.m_activatedProducts.Add(product))
            this.Renderer.increaseVisibilityOf(product);
        }
        this.Renderer.updateVisibility();
      }

      public void ShowExactly(ImmutableArray<ProductProto> products)
      {
        this.hideActivatedProductsAndClear();
        foreach (ProductProto product in products)
        {
          if (this.m_activatedProducts.Add(product))
            this.Renderer.increaseVisibilityOf(product);
        }
        this.Renderer.updateVisibility();
      }

      public void ShowAll()
      {
        foreach (ProductProto key in this.Renderer.m_materials.Keys)
        {
          if (this.m_activatedProducts.Add(key))
          {
            this.Renderer.increaseVisibilityOf(key);
            this.Renderer.updateVisibility();
          }
        }
      }

      public void HideAll()
      {
        this.hideActivatedProductsAndClear();
        this.Renderer.updateVisibility();
      }

      private void hideActivatedProductsAndClear()
      {
        foreach (ProductProto activatedProduct in this.m_activatedProducts)
          this.Renderer.decreaseVisibilityOf(activatedProduct);
        this.m_activatedProducts.Clear();
      }
    }
  }
}
