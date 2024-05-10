// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.CargoDepotMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Utils;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  internal class CargoDepotMb : 
    StaticEntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private Option<CargoDepotMb.DepotModuleWrapper>[] m_cranes;
    private AssetsDb m_assetsDb;
    private DelayedEvent<int> m_onModuleRemovedEvent;
    private DelayedEvent<CargoDepotModule, int> m_onModuleAddedEvent;
    private DelayedEvent<CargoDepotModule, int> m_onModuleUpgradedEvent;

    public CargoDepot CargoDepot { get; private set; }

    public void Initialize(CargoDepot cargoDepot, IGameLoopEvents gameLoop, AssetsDb assetsDb)
    {
      this.Initialize((ILayoutEntity) cargoDepot);
      CargoDepotProto prototype = cargoDepot.Prototype;
      this.CargoDepot = cargoDepot;
      this.m_assetsDb = assetsDb;
      this.m_cranes = new Option<CargoDepotMb.DepotModuleWrapper>[prototype.ModuleSlots.Length];
      for (int slot = 0; slot < this.m_cranes.Length; ++slot)
      {
        Option<CargoDepotModule> module = this.CargoDepot.GetModule(slot);
        if (module.HasValue)
          this.OnModuleAdded(module.Value, slot);
        else
          this.OnModuleRemoved(slot);
      }
      this.m_onModuleRemovedEvent = gameLoop.RegisterDelayedRenderEvent<int>((Action<Action<int>>) (action => this.CargoDepot.OnModuleRemoved.AddNonSaveable<CargoDepotMb>(this, action)), new Action<int>(this.OnModuleRemoved));
      this.m_onModuleAddedEvent = gameLoop.RegisterDelayedRenderEvent<CargoDepotModule, int>((Action<Action<CargoDepotModule, int>>) (action => this.CargoDepot.OnModuleAdded.AddNonSaveable<CargoDepotMb>(this, action)), new Action<CargoDepotModule, int>(this.OnModuleAdded));
      this.m_onModuleUpgradedEvent = gameLoop.RegisterDelayedRenderEvent<CargoDepotModule, int>((Action<Action<CargoDepotModule, int>>) (action => this.CargoDepot.OnModuleUpgraded.AddNonSaveable<CargoDepotMb>(this, action)), new Action<CargoDepotModule, int>(this.OnModuleUpgraded));
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      for (int index = 0; index < this.m_cranes.Length; ++index)
        this.m_cranes[index].ValueOrNull?.SyncUpdate(time);
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      for (int index = 0; index < this.m_cranes.Length; ++index)
        this.m_cranes[index].ValueOrNull?.RenderUpdate(time);
    }

    public override void Destroy()
    {
      this.m_onModuleRemovedEvent?.UnRegister();
      this.m_onModuleAddedEvent?.UnRegister();
      this.m_onModuleUpgradedEvent?.UnRegister();
      base.Destroy();
    }

    private void OnModuleRemoved(int slot)
    {
      Option<CargoDepotMb.DepotModuleWrapper> crane = this.m_cranes[slot];
      if (crane.HasValue)
      {
        crane.Value.Destroy();
        this.m_cranes[slot] = Option<CargoDepotMb.DepotModuleWrapper>.None;
      }
      this.setEmptyCraneVisibility(slot, true);
    }

    private void OnModuleAdded(CargoDepotModule module, int slot)
    {
      Option<CargoDepotMb.DepotModuleWrapper> crane = this.m_cranes[slot];
      if (crane.IsNone || (Proto) crane.Value.Module.Prototype != (Proto) module.Prototype)
      {
        if (crane.HasValue)
          crane.Value.Destroy();
        this.m_cranes[slot] = (Option<CargoDepotMb.DepotModuleWrapper>) this.createCrane(slot, module);
      }
      this.setEmptyCraneVisibility(slot, false);
    }

    private void OnModuleUpgraded(CargoDepotModule module, int slot)
    {
      this.OnModuleRemoved(slot);
      this.OnModuleAdded(module, slot);
    }

    private void setEmptyCraneVisibility(int slotIndex, bool isVisible)
    {
      GameObject result;
      if (this.gameObject.TryFindNameInHierarchy(string.Format("EmptyCrane{0}", (object) slotIndex), out result))
        result.SetActive(isVisible);
      else
        Log.Error(string.Format("Failed to find empty crane placeholder 'EmptyCrane{0}'", (object) slotIndex));
    }

    private CargoDepotMb.DepotModuleWrapper createCrane(int slotIndex, CargoDepotModule module)
    {
      GameObject clonedPrefabOrEmptyGo = this.m_assetsDb.GetClonedPrefabOrEmptyGo(module.Prototype.Graphics.CranePrefabPath);
      Animator component = clonedPrefabOrEmptyGo.GetComponent<Animator>();
      CargoDepotCraneMb craneMb = clonedPrefabOrEmptyGo.AddComponent<CargoDepotCraneMb>();
      craneMb.Initialize(this, slotIndex);
      clonedPrefabOrEmptyGo.transform.SetParent(this.gameObject.transform, false);
      float z = (float) ((double) slotIndex * 10.0 - (double) this.m_cranes.Length / 2.0 * 10.0 + 10.0);
      clonedPrefabOrEmptyGo.transform.localPosition = new Vector3(-29f, 0.0f, z);
      clonedPrefabOrEmptyGo.SetActive(true);
      return !module.Prototype.HasCraneAnimation ? (CargoDepotMb.DepotModuleWrapper) new CargoDepotMb.PumpWrapper(craneMb, (Option<Animator>) component, module) : (CargoDepotMb.DepotModuleWrapper) new CargoDepotMb.CraneWrapper(craneMb, (Option<Animator>) component, module);
    }

    public CargoDepotMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private abstract class DepotModuleWrapper
    {
      public readonly CargoDepotModule Module;

      protected DepotModuleWrapper(CargoDepotModule module)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Module = module;
      }

      public abstract void SyncUpdate(GameTime time);

      public abstract void RenderUpdate(GameTime time);

      public abstract void Destroy();
    }

    private sealed class PumpWrapper : CargoDepotMb.DepotModuleWrapper
    {
      private static readonly int ANIM_DOWN_STATE_ID;
      private static readonly int ANIM_UP_STATE_ID;
      private static readonly int ANIM_PUMP_STATE_ID;
      private readonly Percent m_pumpLoopSpeed;
      private readonly Option<Animator> m_animator;
      private readonly CargoDepotCraneMb m_craneMb;
      private Percent m_syncPipeMovementProgress;
      private float m_renderPipeMovementProgress;
      private bool m_syncIsPipeDown;
      private bool m_syncIsPumping;
      private bool m_renderIsPumping;
      private int m_syncSpeedMult;
      private bool m_syncIsPipeMoving;

      public PumpWrapper(
        CargoDepotCraneMb craneMb,
        Option<Animator> animator,
        CargoDepotModule module)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_pumpLoopSpeed = 40.Percent();
        // ISSUE: explicit constructor call
        base.\u002Ector(module);
        this.m_craneMb = craneMb.CheckNotNull<CargoDepotCraneMb>();
        this.m_animator = animator;
        this.SyncUpdate(new GameTime());
        this.RenderUpdate(new GameTime());
      }

      public override void SyncUpdate(GameTime time)
      {
        this.m_syncIsPipeMoving = this.Module.IsPipeMoving;
        this.m_syncPipeMovementProgress = this.Module.PipeMovementProgress;
        this.m_syncIsPipeDown = this.Module.IsPipeDown;
        this.m_syncIsPumping = this.Module.IsCargoTransferAnimating;
        this.m_syncSpeedMult = time.GameSpeedMult;
      }

      public override void RenderUpdate(GameTime time)
      {
        if (this.m_animator.IsNone)
          return;
        Animator animator = this.m_animator.Value;
        if (this.m_syncIsPipeMoving)
        {
          this.m_renderPipeMovementProgress = this.m_renderPipeMovementProgress.Lerp(this.m_syncPipeMovementProgress.ToFloat(), time.RelativeT);
          animator.Play(this.m_syncIsPipeDown ? CargoDepotMb.PumpWrapper.ANIM_UP_STATE_ID : CargoDepotMb.PumpWrapper.ANIM_DOWN_STATE_ID, 0, this.m_renderPipeMovementProgress);
          this.m_renderIsPumping = false;
        }
        else
        {
          if (this.m_renderIsPumping != this.m_syncIsPumping)
          {
            this.m_renderIsPumping = this.m_syncIsPumping;
            if (this.m_renderIsPumping)
              animator.Play(CargoDepotMb.PumpWrapper.ANIM_PUMP_STATE_ID);
          }
          animator.speed = this.m_syncIsPumping ? this.m_pumpLoopSpeed.Apply((float) this.m_syncSpeedMult) : 0.0f;
        }
      }

      public override void Destroy() => this.m_craneMb.gameObject.Destroy();

      static PumpWrapper()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        CargoDepotMb.PumpWrapper.ANIM_DOWN_STATE_ID = Animator.StringToHash("Down");
        CargoDepotMb.PumpWrapper.ANIM_UP_STATE_ID = Animator.StringToHash("Up");
        CargoDepotMb.PumpWrapper.ANIM_PUMP_STATE_ID = Animator.StringToHash("Pump");
      }
    }

    private sealed class CraneWrapper : CargoDepotMb.DepotModuleWrapper
    {
      private static readonly int ANIM_MAIN_STATE_ID;
      private readonly CargoDepotCraneMb m_craneMb;
      private readonly Option<Animator> m_animator;
      private bool m_syncIsReversed;
      private float m_syncCargoAnimationProgress;
      private float m_renderCargoAnimationProgress;
      private float m_syncDelta;

      public CraneWrapper(
        CargoDepotCraneMb craneMb,
        Option<Animator> animator,
        CargoDepotModule module)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(module);
        this.m_craneMb = craneMb.CheckNotNull<CargoDepotCraneMb>();
        this.m_animator = animator;
        this.SyncUpdate(new GameTime());
        this.RenderUpdate(new GameTime());
      }

      public override void SyncUpdate(GameTime time)
      {
        bool isAnimatingImport = this.Module.IsAnimatingImport;
        this.m_renderCargoAnimationProgress = this.m_syncCargoAnimationProgress;
        double num1;
        if (!isAnimatingImport)
        {
          num1 = (double) this.Module.CargoAnimationProgress.ToFloat();
        }
        else
        {
          Percent percent = this.Module.CargoAnimationProgress;
          percent = percent.InverseTo100();
          num1 = (double) percent.ToFloat();
        }
        float num2 = (float) num1;
        this.m_syncDelta = num2 - this.m_syncCargoAnimationProgress;
        this.m_syncCargoAnimationProgress = num2;
        if ((double) this.m_syncDelta > 0.5)
          this.m_syncDelta = (float) -(1.0 - (double) this.m_syncDelta);
        else if ((double) this.m_syncDelta < -0.5)
          this.m_syncDelta = 1f + this.m_syncDelta;
        if (this.m_syncIsReversed == isAnimatingImport)
          return;
        this.m_syncIsReversed = isAnimatingImport;
        this.m_renderCargoAnimationProgress = this.m_syncCargoAnimationProgress;
      }

      public override void RenderUpdate(GameTime time)
      {
        if (!this.m_syncDelta.IsNear(0.0f))
        {
          float normalizedTime = this.m_renderCargoAnimationProgress + 0.0f.Lerp(this.m_syncDelta, time.AbsoluteT);
          if ((double) normalizedTime < 0.0)
            normalizedTime = 1f + normalizedTime;
          else if ((double) normalizedTime > 1.0)
            --normalizedTime;
          this.m_animator.ValueOrNull?.Play(CargoDepotMb.CraneWrapper.ANIM_MAIN_STATE_ID, 0, normalizedTime);
        }
        else
          this.m_animator.ValueOrNull?.Play(CargoDepotMb.CraneWrapper.ANIM_MAIN_STATE_ID, 0, this.m_renderCargoAnimationProgress);
      }

      public override void Destroy() => this.m_craneMb.gameObject.Destroy();

      static CraneWrapper()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        CargoDepotMb.CraneWrapper.ANIM_MAIN_STATE_ID = Animator.StringToHash("Main");
      }
    }
  }
}
