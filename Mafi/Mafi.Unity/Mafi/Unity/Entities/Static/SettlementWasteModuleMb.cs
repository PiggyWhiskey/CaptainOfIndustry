// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.SettlementWasteModuleMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Unity.Terrain;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  /// <summary>
  /// IMPORTANT: This is a strong copy-pasta of LooseStorageMb
  /// </summary>
  internal class SettlementWasteModuleMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithRenderUpdate
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private static readonly string PILE_GO_NAME;
    private SettlementWasteModule m_wasteModule;
    private float m_prevPercentFull;
    private float m_syncPercentFull;
    private float m_renderedPercentFull;
    private Option<Animator> m_pileToShow;
    private bool m_wasEmpty;
    private Option<ProductProto> m_storedProduct;
    private LooseProductTextureSetter m_pileMaterialSetter;

    public void Initialize(
      SettlementWasteModule module,
      LooseProductMaterialManager looseProductMaterialManager)
    {
      this.Initialize((ILayoutEntity) module);
      this.m_wasteModule = module;
      GameObject resultGo;
      Animator component1;
      Renderer component2;
      if (this.gameObject.TryFindChild(SettlementWasteModuleMb.PILE_GO_NAME, out resultGo) && resultGo.TryGetComponent<Animator>(out component1) && resultGo.TryGetComponent<Renderer>(out component2))
      {
        component1.Play(SettlementWasteModuleMb.ANIM_MAIN_STATE_ID);
        component1.speed = 0.0f;
        this.m_pileToShow = (Option<Animator>) component1;
        this.m_pileMaterialSetter = looseProductMaterialManager.SetupSharedMaterialFor(component2);
        resultGo.SetActive(false);
        this.m_syncPercentFull = module.PercentFull.ToFloat();
        this.m_storedProduct = module.StoredProduct;
        this.m_wasEmpty = module.IsEmpty;
        if (!this.m_storedProduct.HasValue || !module.IsNotEmpty || !(this.m_storedProduct.Value is LooseProductProto looseProductProto))
          return;
        this.m_pileMaterialSetter.SetTexture(looseProductProto.LooseSlimId);
        if ((double) this.m_syncPercentFull <= 0.0 || !this.m_pileToShow.HasValue)
          return;
        this.m_pileToShow.Value.gameObject.SetActive(true);
        this.m_pileToShow.Value.Play(SettlementWasteModuleMb.ANIM_MAIN_STATE_ID, 0, this.m_syncPercentFull);
      }
      else
        Log.Warning(string.Format("Failed to find material pile with animator on object '{0}'.", (object) this.gameObject));
    }

    /// <summary>
    /// IMPORTANT: This is a strong copy-pasta of LooseStorageMb
    /// </summary>
    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (time.IsGamePaused || this.m_pileToShow.IsNone)
        return;
      float num = (float) time.GameSpeedMult * 0.02f;
      float self = this.m_wasteModule.PercentFull.ToFloat();
      this.m_prevPercentFull = this.m_syncPercentFull;
      if ((double) this.m_syncPercentFull < (double) self)
        this.m_syncPercentFull = self.Min(this.m_syncPercentFull + num);
      else if ((double) this.m_syncPercentFull > (double) self)
        this.m_syncPercentFull = self.Max(this.m_syncPercentFull - num);
      if (!(this.m_wasteModule.StoredProduct != this.m_storedProduct) && this.m_wasEmpty == this.m_wasteModule.IsEmpty)
        return;
      this.m_storedProduct = this.m_wasteModule.StoredProduct;
      this.m_wasEmpty = this.m_wasteModule.IsEmpty;
      this.m_pileToShow.ValueOrNull?.gameObject.SetActive(false);
      if (this.m_wasteModule.IsEmpty)
        Assert.That<float>(self).IsZero();
      else if (this.m_storedProduct.HasValue && this.m_storedProduct.Value is LooseProductProto looseProductProto)
      {
        this.m_pileMaterialSetter.SetTexture(looseProductProto.LooseSlimId);
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
        this.m_pileToShow.Value.Play(SettlementWasteModuleMb.ANIM_MAIN_STATE_ID, 0, this.m_renderedPercentFull);
      }
      else
      {
        if (!this.m_pileToShow.Value.gameObject.activeSelf)
          return;
        this.m_pileToShow.Value.Play(SettlementWasteModuleMb.ANIM_MAIN_STATE_ID, 0, 0.0f);
        this.m_pileToShow.Value.gameObject.SetActive(false);
      }
    }

    public SettlementWasteModuleMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static SettlementWasteModuleMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SettlementWasteModuleMb.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
      SettlementWasteModuleMb.PILE_GO_NAME = "Pile";
    }
  }
}
