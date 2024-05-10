// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.NuclearWasteStorageMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Storages.NuclearWaste;
using Mafi.Core.Entities.Static;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class NuclearWasteStorageMb : 
    StaticEntityMb,
    IEntityMbWithRenderUpdateMaybe,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdateMaybe,
    IEntityMbWithSyncUpdate
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private Option<Animator> m_loadingAnimator;
    private NuclearWasteStorage m_storage;
    private int m_syncSpeedMult;
    private int m_currentSpeedMult;
    private Quantity m_syncQuantity;

    public void Initialize(NuclearWasteStorage storage)
    {
      this.Initialize((ILayoutEntity) storage);
      this.m_storage = storage;
      this.m_syncQuantity = storage.CurrentQuantity;
      Animator component;
      if (!this.gameObject.TryGetComponent<Animator>(out component))
        return;
      Assert.That<Animator>(component).IsValidUnityObject<Animator>();
      this.m_loadingAnimator = (Option<Animator>) component;
    }

    bool IEntityMbWithSyncUpdateMaybe.NeedsSyncUpdate => this.m_loadingAnimator.HasValue;

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (!this.m_loadingAnimator.HasValue)
        return;
      this.m_syncSpeedMult = time.GameSpeedMult;
      if (!(this.m_syncQuantity != this.m_storage.CurrentQuantity))
        return;
      this.m_syncQuantity = this.m_storage.CurrentQuantity;
      this.m_loadingAnimator.Value.Play(NuclearWasteStorageMb.ANIM_MAIN_STATE_ID);
    }

    bool IEntityMbWithRenderUpdateMaybe.NeedsRenderUpdate => this.m_loadingAnimator.HasValue;

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      if (!this.m_loadingAnimator.HasValue || this.m_currentSpeedMult == this.m_syncSpeedMult)
        return;
      if (!(bool) (Object) this.m_loadingAnimator.Value)
      {
        Log.Warning("Animator on storage (go '" + this.gameObject.name + "') is invalid, removing.");
        this.m_loadingAnimator = Option<Animator>.None;
      }
      else
      {
        this.m_currentSpeedMult = this.m_syncSpeedMult;
        this.m_loadingAnimator.Value.speed = (float) this.m_currentSpeedMult;
      }
    }

    public NuclearWasteStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_currentSpeedMult = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static NuclearWasteStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      NuclearWasteStorageMb.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
    }
  }
}
