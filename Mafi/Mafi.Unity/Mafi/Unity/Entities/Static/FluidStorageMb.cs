// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.FluidStorageMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Unity.Factory.Transports;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class FluidStorageMb : 
    LayoutEntityWithSignMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithRenderUpdate
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private Option<Animator> m_lidAnimator;
    private Storage m_storage;
    private float m_prevPercentFull;
    private float m_syncPercentFull;
    private float m_renderedPercentFull;
    private Option<FluidIndicatorState> m_fluidIndicatorState;
    private bool m_wasEmpty;

    public void Initialize(AssetsDb assetsDb, Storage storage, FluidStorageProto fluidStorageProto)
    {
      this.Initialize(assetsDb, (Mafi.Core.Entities.Static.Layout.LayoutEntity) storage);
      this.m_storage = storage;
      this.m_signChildName = fluidStorageProto.Graphics.SignObjectPath;
      Animator component1;
      if (this.gameObject.TryGetComponent<Animator>(out component1))
      {
        Assert.That<Animator>(component1).IsValidUnityObject<Animator>();
        this.m_lidAnimator = (Option<Animator>) component1;
        component1.Play(FluidStorageMb.ANIM_MAIN_STATE_ID);
        component1.speed = 0.0f;
      }
      Transform resultTransform;
      MeshRenderer component2;
      if (this.transform.TryFindChild(fluidStorageProto.Graphics.FluidIndicatorObjectPath, out resultTransform) && resultTransform.TryGetComponent<MeshRenderer>(out component2))
        this.m_fluidIndicatorState = (Option<FluidIndicatorState>) new FluidIndicatorState(component2, fluidStorageProto.Graphics.FluidIndicatorParams);
      else
        Log.Warning("Failed to locate fluid indicator on game object '" + this.gameObject.name + "'.");
      this.m_prevPercentFull = this.m_syncPercentFull = this.m_storage.PercentFull.ToFloat();
      if (this.m_lidAnimator.HasValue)
        this.m_lidAnimator.Value.Play(FluidStorageMb.ANIM_MAIN_STATE_ID, 0, 1f - this.m_syncPercentFull);
      this.m_productToRender = storage.StoredProduct;
      this.m_wasEmpty = storage.IsEmpty;
      if (this.m_productToRender.HasValue && storage.IsNotEmpty && this.m_fluidIndicatorState.HasValue)
      {
        this.m_fluidIndicatorState.Value.SetColor(this.m_productToRender.Value.Graphics.Color.ToColor());
        this.m_fluidIndicatorState.Value.SkipTransition();
      }
      this.initializeSign(0.55f);
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (this.m_productToRender != this.m_storage.StoredProduct || this.m_wasEmpty != this.m_storage.IsEmpty)
      {
        this.m_productToRender = this.m_storage.StoredProduct;
        this.m_wasEmpty = this.m_storage.IsEmpty;
        this.m_fluidIndicatorState.ValueOrNull?.SetColor(!this.m_productToRender.HasValue || !this.m_storage.IsNotEmpty ? Color.black : this.m_productToRender.Value.Graphics.Color.ToColor());
        this.updateSign();
      }
      if (time.IsGamePaused)
        return;
      float num = (float) time.GameSpeedMult * 0.01f;
      float self = this.m_storage.PercentFull.ToFloat();
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
      if ((double) this.m_renderedPercentFull != (double) this.m_syncPercentFull && this.m_lidAnimator.HasValue)
      {
        this.m_renderedPercentFull = (double) this.m_prevPercentFull == (double) this.m_syncPercentFull ? this.m_syncPercentFull : this.m_prevPercentFull.Lerp(this.m_syncPercentFull, time.AbsoluteT);
        this.m_lidAnimator.Value.Play(FluidStorageMb.ANIM_MAIN_STATE_ID, 0, 1f - this.m_renderedPercentFull);
      }
      this.m_fluidIndicatorState.ValueOrNull?.RenderUpdate(time);
    }

    public FluidStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FluidStorageMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FluidStorageMb.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
    }
  }
}
