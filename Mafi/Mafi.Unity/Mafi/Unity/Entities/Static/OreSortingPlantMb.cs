// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.OreSortingPlantMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Unity.Terrain;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class OreSortingPlantMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithRenderUpdate
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private OreSortingPlant m_sortingPlant;
    private float m_prevPercentFull;
    private float m_syncPercentFull;
    private float m_renderedPercentFull;
    private Option<Animator> m_smoothPileAnimator;
    private LooseMixedProductsTextureSetter m_smoothPileMaterialSetter;
    private readonly Lyst<ProductQuantity> m_products;
    private bool m_isNewProduct;
    private int m_totalQuantity;

    public void Initialize(
      OreSortingPlant sortingPlant,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      this.Initialize((ILayoutEntity) sortingPlant);
      this.m_sortingPlant = sortingPlant;
      OreSortingPlantProto prototype = sortingPlant.Prototype;
      GameObject resultGo;
      Animator component1;
      Renderer component2;
      if (this.gameObject.TryFindChild(prototype.Graphics.SmoothPileObjectPath, out resultGo) && resultGo.TryGetComponent<Animator>(out component1) && resultGo.TryGetComponent<Renderer>(out component2))
      {
        this.m_smoothPileAnimator = (Option<Animator>) component1;
        component1.Play(OreSortingPlantMb.ANIM_MAIN_STATE_ID);
        component1.speed = 0.0f;
        this.m_smoothPileMaterialSetter = looseProductMaterialManager.SetupMixedSharedMaterialsFor(component2, prototype.Graphics.PileTextureParams);
        MeshFilter component3;
        if (resultGo.TryGetComponent<MeshFilter>(out component3) && (Object) component3.mesh != (Object) null)
        {
          this.setUvs(component3.mesh);
        }
        else
        {
          SkinnedMeshRenderer component4;
          if (resultGo.TryGetComponent<SkinnedMeshRenderer>(out component4) && (Object) component4.sharedMesh != (Object) null)
            this.setUvs(component4.sharedMesh);
          else
            Log.Warning(string.Format("Failed to find mesh of pile '{0}'.", (object) resultGo));
        }
        resultGo.SetActive(false);
      }
      else
        Log.Warning(string.Format("Failed to find smooth material pile with animator on object '{0}'.", (object) this.gameObject));
      this.m_syncPercentFull = sortingPlant.PercentFull.ToFloat();
      this.m_totalQuantity = this.m_sortingPlant.MixedTotal.Value;
      this.m_products.Clear();
      foreach (KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData> keyValuePair in (IEnumerable<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>>) this.m_sortingPlant.ProductsData)
        this.m_products.Add(new ProductQuantity(keyValuePair.Key, keyValuePair.Value.UnsortedQuantity));
      if (!this.m_smoothPileAnimator.HasValue || (double) this.m_syncPercentFull <= 0.0)
        return;
      this.m_smoothPileAnimator.Value.gameObject.SetActive(true);
      this.m_smoothPileAnimator.Value.Play(OreSortingPlantMb.ANIM_MAIN_STATE_ID, 0, this.m_syncPercentFull);
    }

    private void setUvs(Mesh mesh)
    {
      if (mesh.isReadable)
      {
        float num = float.MaxValue;
        float self = float.MinValue;
        foreach (Vector2 vector2 in mesh.uv)
        {
          num = num.Min(vector2.x);
          self = self.Max(vector2.x);
        }
        this.m_smoothPileMaterialSetter.SetUVs(num, (float) (1.0 / ((double) self - (double) num)));
      }
      else
        Log.Warning(string.Format("Mesh of {0} is not readable.", (object) mesh));
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (time.IsGamePaused)
        return;
      int index = 0;
      if (this.m_sortingPlant.ProductsData.Count != this.m_products.Count)
      {
        this.m_isNewProduct = true;
      }
      else
      {
        foreach (KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData> keyValuePair in (IEnumerable<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>>) this.m_sortingPlant.ProductsData)
        {
          if ((Proto) this.m_products[index].Product != (Proto) keyValuePair.Key)
          {
            this.m_isNewProduct = true;
            break;
          }
          ++index;
        }
      }
      this.m_totalQuantity = this.m_sortingPlant.MixedTotal.Value;
      this.m_products.Clear();
      foreach (KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData> keyValuePair in (IEnumerable<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>>) this.m_sortingPlant.ProductsData)
        this.m_products.Add(new ProductQuantity(keyValuePair.Key, keyValuePair.Value.UnsortedQuantity));
      float num = (float) time.GameSpeedMult * 0.02f;
      float self = ((float) this.m_sortingPlant.MixedTotal.Value / (float) this.m_sortingPlant.Capacity.Value).Clamp01();
      this.m_prevPercentFull = this.m_syncPercentFull;
      if ((double) this.m_syncPercentFull < (double) self)
      {
        this.m_syncPercentFull = self.Min(this.m_syncPercentFull + num);
      }
      else
      {
        if ((double) this.m_syncPercentFull <= (double) self)
          return;
        this.m_syncPercentFull = self.Max(this.m_syncPercentFull - num);
      }
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      Assert.That<OreSortingPlant>(this.m_sortingPlant).IsNotNull<OreSortingPlant>();
      if (this.m_isNewProduct)
      {
        this.m_isNewProduct = false;
        if (this.m_products.IsNotEmpty)
        {
          Assert.That<int>(this.m_products.Count).IsLessOrEqual(LooseMixedProductsTextureSetter.MAX_TEXTURE_COUNT);
          for (int index = 0; index < this.m_products.Count; ++index)
          {
            ProductQuantity product1 = this.m_products[index];
            if (product1.Product is LooseProductProto product2)
              this.m_smoothPileMaterialSetter.SetTexture(product2.LooseSlimId, index);
            else
              Log.Warning(string.Format("{0}#{1} is holding non-loose product: {2}", (object) this.m_sortingPlant.Prototype.Id, (object) this.m_sortingPlant.Id, (object) product1.Product));
          }
        }
      }
      if (!this.m_smoothPileAnimator.HasValue || (double) this.m_renderedPercentFull == (double) this.m_syncPercentFull)
        return;
      this.m_renderedPercentFull = (double) this.m_prevPercentFull == (double) this.m_syncPercentFull ? this.m_syncPercentFull : this.m_prevPercentFull.Lerp(this.m_syncPercentFull, time.AbsoluteT);
      if ((double) this.m_syncPercentFull != 0.0)
      {
        this.m_smoothPileAnimator.Value.gameObject.SetActive(true);
        this.m_smoothPileAnimator.Value.Play(OreSortingPlantMb.ANIM_MAIN_STATE_ID, 0, this.m_renderedPercentFull);
        if (this.m_totalQuantity <= 0)
          return;
        float num = 1f / (float) this.m_totalQuantity;
        this.m_smoothPileMaterialSetter.SetRatios(this.m_products.Count >= 1 ? (float) this.m_products[0].Quantity.Value * num : 0.0f, this.m_products.Count >= 2 ? (float) this.m_products[1].Quantity.Value * num : 0.0f, this.m_products.Count >= 3 ? (float) this.m_products[2].Quantity.Value * num : 0.0f, this.m_products.Count >= 4 ? (float) this.m_products[3].Quantity.Value * num : 0.0f, this.m_products.Count >= 5 ? (float) this.m_products[4].Quantity.Value * num : 0.0f, this.m_products.Count >= 6 ? (float) this.m_products[5].Quantity.Value * num : 0.0f, this.m_products.Count >= 7 ? (float) this.m_products[6].Quantity.Value * num : 0.0f, this.m_products.Count >= 8 ? (float) this.m_products[7].Quantity.Value * num : 0.0f);
      }
      else
      {
        if (!this.m_smoothPileAnimator.Value.gameObject.activeSelf)
          return;
        this.m_smoothPileAnimator.Value.Play(OreSortingPlantMb.ANIM_MAIN_STATE_ID, 0, 0.0f);
        this.m_smoothPileAnimator.Value.gameObject.SetActive(false);
      }
    }

    public OreSortingPlantMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_products = new Lyst<ProductQuantity>(8);
      this.m_isNewProduct = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static OreSortingPlantMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      OreSortingPlantMb.ANIM_MAIN_STATE_ID = Animator.StringToHash("Pile");
    }
  }
}
