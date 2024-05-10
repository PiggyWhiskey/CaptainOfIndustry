// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.ShipEntityMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Environment;
using Mafi.Core.Fleet;
using Mafi.Core.World;
using Mafi.Numerics;
using Mafi.Unity.Audio;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  internal class ShipEntityMb : 
    EntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private static readonly int ARRIVAL_ANIM_SATE_ID;
    private static readonly int DEPARTURE_ANIM_SATE_ID;
    private static readonly int DEPARTURE_SOUND_DELAY;
    private static readonly int ARRIVAL_SOUND_DELAY;
    private TravelingFleet m_fleet;
    private AssetsDb m_assetsDb;
    private string m_currentPrefabPath;
    private GameObject m_bridgeGo;
    private bool m_modificationUpdatePending;
    private bool m_syncHasCrew;
    private readonly LoopAnimationHandler m_animHandler;
    private Transform m_dockProxyTransform;
    private GameObject m_dockingAnimationProxy;
    private AnimationSyncer m_dockingAnimationSyncer;
    private FleetLocationState m_previousState;
    private string m_dockingAnimPath;
    private OptionalParticlesWrapper m_rearEngineParticles;
    private OptionalParticlesWrapper m_rearExhaustParticles;
    private Option<EntitySoundMb> m_engineSound;
    private Option<EntitySoundMb> m_arrivalSound;
    private Option<EntitySoundMb> m_departureSound;
    private int? m_playArrivalSoundAfterStep;
    private int? m_playDepartureSoundAfterStep;
    private ShipRollHelper m_shipRollHelper;

    public void Initialize(
      TravelingFleet fleet,
      AssetsDb assetsDb,
      EntityAudioManager audioManager,
      WeatherManager weatherManager,
      IRandom rng)
    {
      this.Initialize((IEntity) fleet);
      this.m_fleet = fleet;
      this.m_assetsDb = assetsDb;
      this.m_shipRollHelper = new ShipRollHelper(weatherManager, rng.NextAngle().ToUnityAngleDegrees());
      this.m_fleet.OnModificationsDone += (Action<TravelingFleet>) (f => this.m_modificationUpdatePending = true);
      this.updateModels(fleet);
      this.m_animHandler.SetSpeed(0.5f);
      this.refreshAnimationType();
      this.m_engineSound = audioManager.CreateSound((EntityMb) this, new SoundParams(fleet.Prototype.Graphics.EngineSoundPath, SoundSignificance.Medium));
      this.m_arrivalSound = audioManager.CreateSound((EntityMb) this, new SoundParams(fleet.Prototype.Graphics.ArrivalSoundPath, SoundSignificance.Medium, false, false, true));
      this.m_departureSound = audioManager.CreateSound((EntityMb) this, new SoundParams(fleet.Prototype.Graphics.DepartureSoundPath, SoundSignificance.Medium, false, false, true));
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (this.m_fleet == null)
        return;
      this.m_syncHasCrew = this.m_fleet.HasAllRequiredCrew;
      if (this.m_previousState != this.m_fleet.LocationState)
      {
        this.m_previousState = this.m_fleet.LocationState;
        GameObject gameObject = this.gameObject;
        bool flag;
        switch (this.m_fleet.LocationState)
        {
          case FleetLocationState.ArrivingFromWorld:
          case FleetLocationState.Docked:
          case FleetLocationState.DepartingToWorld:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        gameObject.SetActive(flag);
        this.refreshAnimationType();
        this.m_dockProxyTransform.localPosition = this.m_fleet.Position3f.ToVector3();
        this.m_dockProxyTransform.localRotation = UnitQuaternion4f.FromAxisAngle(Vector3f.UnitZ, this.m_fleet.Direction).ToUnityQuaternion();
        if (this.m_fleet.LocationState == FleetLocationState.ArrivingFromWorld)
        {
          this.m_dockingAnimationSyncer.PlayNew(ShipEntityMb.ARRIVAL_ANIM_SATE_ID);
          this.playParticles(time);
        }
        else if (this.m_fleet.LocationState == FleetLocationState.DepartingToWorld)
        {
          this.m_dockingAnimationSyncer.PlayNew(ShipEntityMb.DEPARTURE_ANIM_SATE_ID);
          this.playParticles(time);
        }
        else
        {
          this.m_dockingAnimationSyncer.Stop();
          this.stopParticles();
        }
        if (this.m_previousState == FleetLocationState.DepartingToWorld)
          this.m_playDepartureSoundAfterStep = new int?(time.SimStepsCount + ShipEntityMb.DEPARTURE_SOUND_DELAY);
        else if (this.m_previousState == FleetLocationState.ArrivingFromWorld)
          this.m_playArrivalSoundAfterStep = new int?(time.SimStepsCount + ShipEntityMb.ARRIVAL_SOUND_DELAY);
      }
      this.m_shipRollHelper.SyncUpdate(time, this.m_previousState == FleetLocationState.Docked ? 0.5f : 1f);
      if (this.m_playArrivalSoundAfterStep.HasValue && this.m_playArrivalSoundAfterStep.Value < time.SimStepsCount)
      {
        this.m_playArrivalSoundAfterStep = new int?();
        this.m_arrivalSound.ValueOrNull?.Play();
      }
      if (this.m_playDepartureSoundAfterStep.HasValue && this.m_playDepartureSoundAfterStep.Value < time.SimStepsCount)
      {
        this.m_playDepartureSoundAfterStep = new int?();
        this.m_departureSound.ValueOrNull?.Play();
      }
      this.m_dockingAnimationSyncer.Sync((float) (1.0 - (double) this.m_fleet.RemainingTransitionDuration.Ticks / (double) this.m_fleet.Prototype.DockTransitionDuration.Ticks), (float) time.GameSpeedMult);
      this.syncParticles(time);
      this.m_animHandler.SyncUpdate(time);
    }

    private void refreshAnimationType()
    {
      int index = this.m_fleet.Dock.ReservedOceanAreaState.FirstValidAreasSetIndex;
      if (index < 0)
        index = 0;
      string animationsPrefabPath = this.m_fleet.Dock.Prototype.DockingAnimationsPrefabPaths[index];
      if (this.m_dockingAnimPath == animationsPrefabPath)
        return;
      this.m_dockingAnimPath = animationsPrefabPath;
      this.m_dockingAnimationProxy = this.m_assetsDb.GetClonedPrefabOrEmptyGo(animationsPrefabPath);
      this.m_dockingAnimationSyncer = new AnimationSyncer(this.m_dockingAnimationProxy.GetComponent<Animator>());
      if ((UnityEngine.Object) this.m_dockProxyTransform == (UnityEngine.Object) null)
        this.m_dockProxyTransform = new GameObject("Dock proxy").transform;
      this.m_dockingAnimationProxy.transform.SetParent(this.m_dockProxyTransform);
      this.transform.SetParent(this.m_dockingAnimationProxy.transform, false);
      this.transform.localPosition = Vector3.zero;
      this.transform.localRotation = Quaternion.identity;
    }

    private void playParticles(GameTime time)
    {
      this.m_rearEngineParticles.Play(time);
      this.m_rearExhaustParticles.Play(time);
    }

    private void syncParticles(GameTime time)
    {
      this.m_rearEngineParticles.UpdateSpeedIfPlaying(time);
      this.m_rearExhaustParticles.UpdateSpeedIfPlaying(time);
    }

    private void stopParticles()
    {
      this.m_rearEngineParticles.Stop();
      this.m_rearExhaustParticles.Stop();
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      if (this.m_fleet == null)
        return;
      if (this.m_modificationUpdatePending)
      {
        this.m_modificationUpdatePending = false;
        this.updateModels(this.m_fleet);
      }
      this.m_animHandler.SetEnabled(this.m_syncHasCrew);
      this.m_animHandler.RenderUpdate(time);
      this.transform.localRotation = this.m_shipRollHelper.RenderUpdateGetRotation(time);
      if (this.m_engineSound.HasValue)
      {
        bool isPlaying = this.m_previousState == FleetLocationState.ArrivingFromWorld || this.m_previousState == FleetLocationState.DepartingToWorld;
        this.m_engineSound.Value.RenderUpdate(time, isPlaying);
      }
      this.m_arrivalSound.ValueOrNull?.RenderUpdate(time);
      this.m_departureSound.ValueOrNull?.RenderUpdate(time);
    }

    private void updateModels(TravelingFleet fleet)
    {
      this.updateMainBridge();
      foreach (FleetEntitySlot slot in fleet.FleetEntity.Slots)
      {
        foreach (FleetEntityPartProto eligibleItem in slot.Proto.EligibleItems)
          this.setGoActive(eligibleItem.Graphics.GameObjectToShow, false);
        this.setGoActive(slot.Proto.Graphics.GoToShowIfEmpty, false);
        if (slot.ExistingPart.HasValue)
          this.setGoActive(slot.ExistingPart.Value.Graphics.GameObjectToShow, true);
        else
          this.setGoActive(slot.Proto.Graphics.GoToShowIfEmpty, true);
      }
    }

    private void updateMainBridge()
    {
      if (this.m_fleet.GetMainPrefab() == this.m_currentPrefabPath)
        return;
      if ((UnityEngine.Object) this.m_bridgeGo != (UnityEngine.Object) null)
      {
        this.m_bridgeGo.Destroy();
        this.m_bridgeGo = (GameObject) null;
        foreach (UnityEngine.Object component in this.gameObject.GetComponents<BoxCollider>())
          UnityEngine.Object.Destroy(component);
      }
      this.m_bridgeGo = this.m_assetsDb.GetClonedPrefabOrEmptyGo(this.m_fleet.GetMainPrefab());
      this.m_bridgeGo.transform.SetParent(this.gameObject.transform, false);
      this.m_currentPrefabPath = this.m_fleet.GetMainPrefab();
      foreach (BoxCollider component in this.m_bridgeGo.GetComponents<BoxCollider>())
      {
        BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = component.size;
        boxCollider.center = component.center;
        UnityEngine.Object.Destroy((UnityEngine.Object) component);
      }
      this.m_animHandler.LoadAnimationFor(this.m_bridgeGo);
      this.m_rearEngineParticles = new OptionalParticlesWrapper(this.m_bridgeGo, "RearEngineParticles");
      this.m_rearExhaustParticles = new OptionalParticlesWrapper(this.m_bridgeGo, "RearExhaustParticles");
      this.m_rearEngineParticles.Stop();
      this.m_rearExhaustParticles.Stop();
    }

    private void setGoActive(Option<string> goName, bool active)
    {
      if (goName.IsNone)
        return;
      Transform transform = this.m_bridgeGo.transform.Find(goName.Value);
      if (!((UnityEngine.Object) transform != (UnityEngine.Object) null))
        return;
      transform.gameObject.SetActive(active);
    }

    public ShipEntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_currentPrefabPath = "";
      this.m_animHandler = new LoopAnimationHandler();
      this.m_previousState = ~FleetLocationState.AtWorld;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ShipEntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ShipEntityMb.ARRIVAL_ANIM_SATE_ID = Animator.StringToHash("Arrival");
      ShipEntityMb.DEPARTURE_ANIM_SATE_ID = Animator.StringToHash("Departure");
      ShipEntityMb.DEPARTURE_SOUND_DELAY = 9.Seconds().Ticks;
      ShipEntityMb.ARRIVAL_SOUND_DELAY = 8.Seconds().Ticks;
    }
  }
}
