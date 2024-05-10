// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.ThermalStorageMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class ThermalStorageMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithRenderUpdate
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private Option<Animator> m_lidAnimator;
    private ThermalStorage m_storage;
    private float m_syncPercentFull;
    private float m_renderedPercentFull;

    public void Initialize(ThermalStorage storage)
    {
      this.Initialize((ILayoutEntity) storage);
      this.m_storage = storage;
      Animator component;
      if (this.gameObject.TryGetComponent<Animator>(out component))
      {
        Assert.That<Animator>(component).IsValidUnityObject<Animator>();
        this.m_lidAnimator = (Option<Animator>) component;
        component.Play(ThermalStorageMb.ANIM_MAIN_STATE_ID);
        component.speed = 0.0f;
      }
      this.m_syncPercentFull = this.m_storage.PercentFull.ToFloat();
      if (!this.m_lidAnimator.HasValue)
        return;
      this.m_lidAnimator.Value.Play(ThermalStorageMb.ANIM_MAIN_STATE_ID, 0, this.m_syncPercentFull);
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (time.IsGamePaused)
        return;
      this.m_syncPercentFull = this.m_storage.PercentFull.ToFloat();
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      this.m_renderedPercentFull = this.m_renderedPercentFull.Lerp(this.m_syncPercentFull, time.RelativeT);
      if (this.m_renderedPercentFull.IsNear(this.m_syncPercentFull))
        return;
      this.m_lidAnimator.Value.Play(ThermalStorageMb.ANIM_MAIN_STATE_ID, 0, this.m_renderedPercentFull);
    }

    public ThermalStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ThermalStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ThermalStorageMb.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
    }
  }
}
