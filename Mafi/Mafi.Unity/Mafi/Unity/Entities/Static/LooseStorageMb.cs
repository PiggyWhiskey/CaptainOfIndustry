// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.LooseStorageMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Unity.Terrain;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class LooseStorageMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithRenderUpdate
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private Storage m_storage;
    private float m_prevPercentFull;
    private float m_syncPercentFull;
    private float m_renderedPercentFull;
    private Option<Animator> m_smoothPileAnimator;
    private Option<Animator> m_roughPileAnimator;
    private Option<Animator> m_pileToShow;
    private bool m_wasEmpty;
    private Option<ProductProto> m_storedProduct;
    private LooseProductTextureSetter m_smoothPileMaterialSetter;
    private LooseProductTextureSetter m_roughPileMaterialSetter;

    public void Initialize(
      Storage storage,
      LooseStorageProto looseStorageProto,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      this.Initialize((ILayoutEntity) storage);
      this.m_storage = storage;
      GameObject resultGo1;
      Animator component1;
      Renderer component2;
      if (this.gameObject.TryFindChild(looseStorageProto.Graphics.SmoothPileObjectPath, out resultGo1) && resultGo1.TryGetComponent<Animator>(out component1) && resultGo1.TryGetComponent<Renderer>(out component2))
      {
        this.m_smoothPileAnimator = (Option<Animator>) component1;
        component1.Play(LooseStorageMb.ANIM_MAIN_STATE_ID);
        component1.speed = 0.0f;
        resultGo1.SetActive(false);
        this.m_smoothPileMaterialSetter = looseProductMaterialManager.SetupSharedMaterialFor(component2, looseStorageProto.Graphics.PileTextureParams);
      }
      else
        Log.Warning(string.Format("Failed to find smooth material pile with animator on object '{0}'.", (object) this.gameObject));
      if (looseStorageProto.Graphics.SmoothPileObjectPath == looseStorageProto.Graphics.RoughPileObjectPath)
      {
        this.m_roughPileAnimator = this.m_smoothPileAnimator;
        this.m_roughPileMaterialSetter = this.m_smoothPileMaterialSetter;
      }
      else
      {
        GameObject resultGo2;
        Animator component3;
        Renderer component4;
        if (this.gameObject.TryFindChild(looseStorageProto.Graphics.RoughPileObjectPath, out resultGo2) && resultGo2.TryGetComponent<Animator>(out component3) && resultGo2.TryGetComponent<Renderer>(out component4))
        {
          this.m_roughPileAnimator = (Option<Animator>) component3;
          component3.Play(LooseStorageMb.ANIM_MAIN_STATE_ID);
          component3.speed = 0.0f;
          resultGo2.SetActive(false);
          this.m_roughPileMaterialSetter = looseProductMaterialManager.SetupSharedMaterialFor(component4, looseStorageProto.Graphics.PileTextureParams);
        }
        else
          Log.Warning(string.Format("Failed to find rough material pile with animator on object '{0}'.", (object) this.gameObject));
      }
      this.m_syncPercentFull = storage.PercentFull.ToFloat();
      this.m_storedProduct = storage.StoredProduct;
      this.m_wasEmpty = storage.IsEmpty;
      if (!this.m_storedProduct.HasValue || !storage.IsNotEmpty || !(this.m_storedProduct.Value is LooseProductProto looseProductProto))
        return;
      if (looseProductProto.Graphics.UseRoughPileMeshes)
      {
        this.m_pileToShow = this.m_roughPileAnimator;
        this.m_roughPileMaterialSetter.SetTexture(looseProductProto.LooseSlimId);
      }
      else
      {
        this.m_pileToShow = this.m_smoothPileAnimator;
        this.m_smoothPileMaterialSetter.SetTexture(looseProductProto.LooseSlimId);
      }
      if (!this.m_pileToShow.HasValue || (double) this.m_syncPercentFull <= 0.0)
        return;
      this.m_pileToShow.Value.gameObject.SetActive(true);
      this.m_pileToShow.Value.Play(LooseStorageMb.ANIM_MAIN_STATE_ID, 0, this.m_syncPercentFull);
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (time.IsGamePaused)
        return;
      float num = (float) time.GameSpeedMult * 0.02f;
      float self = this.m_storage.PercentFull.ToFloat();
      this.m_prevPercentFull = this.m_syncPercentFull;
      if ((double) this.m_syncPercentFull < (double) self)
        this.m_syncPercentFull = self.Min(this.m_syncPercentFull + num);
      else if ((double) this.m_syncPercentFull > (double) self)
        this.m_syncPercentFull = self.Max(this.m_syncPercentFull - num);
      if (!(this.m_storage.StoredProduct != this.m_storedProduct) && this.m_wasEmpty == this.m_storage.IsEmpty)
        return;
      this.m_storedProduct = this.m_storage.StoredProduct;
      this.m_wasEmpty = this.m_storage.IsEmpty;
      this.m_smoothPileAnimator.ValueOrNull?.gameObject.SetActive(false);
      this.m_roughPileAnimator.ValueOrNull?.gameObject.SetActive(false);
      if (this.m_storage.IsEmpty)
        Assert.That<float>(self).IsZero();
      else if (this.m_storedProduct.HasValue && this.m_storedProduct.Value is LooseProductProto looseProductProto)
      {
        if (looseProductProto.Graphics.UseRoughPileMeshes)
        {
          this.m_pileToShow = this.m_roughPileAnimator;
          this.m_roughPileMaterialSetter.SetTexture(looseProductProto.LooseSlimId);
        }
        else
        {
          this.m_pileToShow = this.m_smoothPileAnimator;
          this.m_smoothPileMaterialSetter.SetTexture(looseProductProto.LooseSlimId);
        }
        this.m_prevPercentFull = 0.0f;
        this.m_renderedPercentFull = 0.0f;
      }
      else
        Log.Warning("Non-empty loose storage has non-loose product.");
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      if (!this.m_pileToShow.HasValue || (double) this.m_renderedPercentFull == (double) this.m_syncPercentFull)
        return;
      this.m_renderedPercentFull = (double) this.m_prevPercentFull == (double) this.m_syncPercentFull ? this.m_syncPercentFull : this.m_prevPercentFull.Lerp(this.m_syncPercentFull, time.AbsoluteT);
      if ((double) this.m_renderedPercentFull > 0.0)
      {
        this.m_pileToShow.Value.gameObject.SetActive(true);
        this.m_pileToShow.Value.Play(LooseStorageMb.ANIM_MAIN_STATE_ID, 0, this.m_renderedPercentFull);
      }
      else
      {
        if (!this.m_pileToShow.Value.gameObject.activeSelf)
          return;
        this.m_pileToShow.Value.Play(LooseStorageMb.ANIM_MAIN_STATE_ID, 0, 0.0f);
        this.m_pileToShow.Value.gameObject.SetActive(false);
      }
    }

    public LooseStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static LooseStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LooseStorageMb.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
    }
  }
}
