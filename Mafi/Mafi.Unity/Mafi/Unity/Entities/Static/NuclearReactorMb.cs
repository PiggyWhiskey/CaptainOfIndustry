// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.NuclearReactorMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.NuclearReactors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class NuclearReactorMb : 
    StaticEntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private static readonly int ANIM_MAIN_STATE_ID;
    private NuclearReactor m_reactor;
    private Option<Animator> m_rodsAnimator;
    private Option<Animator> m_fuelExchangeAnimator;
    private LoopAnimationHandler m_continuousAnimHandler;
    private int m_fuelRemovedInSim;
    private Percent m_reactorUtilSync;
    private int m_syncSpeedMult;
    private int m_currentSpeedMult;

    public void Initialize(NuclearReactor reactor)
    {
      this.Initialize((ILayoutEntity) reactor);
      this.m_reactor = reactor;
      reactor.OnSpentFuelGenerated += new Action<Quantity>(this.reactorOnOnFuelGenerated);
      GameObject[] source = new GameObject[this.transform.childCount];
      for (int index = 0; index < source.Length; ++index)
        source[index] = this.transform.GetChild(index).gameObject;
      this.m_rodsAnimator = (Option<Animator>) ((IEnumerable<GameObject>) source).FirstOrDefault<GameObject>((Func<GameObject, bool>) (x => x.name == "Rods"))?.GetComponent<Animator>();
      this.m_rodsAnimator.ValueOrNull?.Play(NuclearReactorMb.ANIM_MAIN_STATE_ID);
      this.m_fuelExchangeAnimator = (Option<Animator>) ((IEnumerable<GameObject>) source).FirstOrDefault<GameObject>((Func<GameObject, bool>) (x => x.name == "FuelExchange"))?.GetComponent<Animator>();
      this.m_continuousAnimHandler = new LoopAnimationHandler();
      this.m_continuousAnimHandler.LoadAnimationFor(this.gameObject);
      this.m_continuousAnimHandler.SetSpeed(1f);
    }

    private void reactorOnOnFuelGenerated(Quantity quantity)
    {
      if (quantity.Value <= 0)
        return;
      this.m_fuelRemovedInSim += quantity.Value;
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      this.m_syncSpeedMult = time.GameSpeedMult;
      this.m_reactorUtilSync = this.m_reactor.CurrentPowerLevel / this.m_reactor.MaxPowerLevel;
      if (this.m_fuelRemovedInSim >= 2)
      {
        this.m_fuelRemovedInSim = 0;
        this.m_fuelExchangeAnimator.ValueOrNull?.Play(NuclearReactorMb.ANIM_MAIN_STATE_ID);
      }
      this.m_continuousAnimHandler.SetEnabled(this.m_reactor.IsEnabled);
      this.m_continuousAnimHandler.SyncUpdate(time);
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      this.m_rodsAnimator.ValueOrNull?.Play(NuclearReactorMb.ANIM_MAIN_STATE_ID, 0, 1f - this.m_reactorUtilSync.ToFloat());
      this.m_continuousAnimHandler.RenderUpdate(time);
      if (this.m_currentSpeedMult == this.m_syncSpeedMult || !this.m_fuelExchangeAnimator.HasValue)
        return;
      this.m_currentSpeedMult = this.m_syncSpeedMult;
      this.m_fuelExchangeAnimator.Value.speed = (float) this.m_currentSpeedMult;
    }

    public NuclearReactorMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static NuclearReactorMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      NuclearReactorMb.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
    }
  }
}
