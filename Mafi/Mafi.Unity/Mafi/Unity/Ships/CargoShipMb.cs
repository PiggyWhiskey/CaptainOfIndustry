// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ships.CargoShipMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Entities;
using Mafi.Core.Environment;
using Mafi.Core.GameLoop;
using Mafi.Core.Utils;
using Mafi.Numerics;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Dynamic;
using Mafi.Unity.Ships.Modules;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ships
{
  internal class CargoShipMb : 
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
    private CargoShip m_cargoShip;
    private CargoShipModuleBaseMb[] m_modulesMb;
    private CargoShipModuleMbFactory m_moduleMbFactory;
    private AssetsDb m_assetsDb;
    private OptionalParticlesWrapper m_rearEngineParticles;
    private OptionalParticlesWrapper m_rearExhaustParticles;
    private Transform m_dockProxyTransform;
    private GameObject m_dockingAnimationProxy;
    private AnimationSyncer m_dockingAnimationSyncer;
    private string m_dockingAnimPath;
    private readonly LoopAnimationHandler m_animHandler;
    private CargoShip.ShipState m_previousState;
    private Option<EntitySoundMb> m_engineSound;
    private Option<EntitySoundMb> m_arrivalSound;
    private Option<EntitySoundMb> m_departureSound;
    private int? m_playArrivalSoundAfterStep;
    private int? m_playDepartureSoundAfterStep;
    private ShipRollHelper m_shipRollHelper;

    public void Initialize(
      CargoShip cargoShip,
      IGameLoopEvents gameLoop,
      AssetsDb assetsDb,
      CargoShipModuleMbFactory moduleMbFactory,
      EntityAudioManager audioManager,
      WeatherManager weatherManager,
      float startingRollAngle)
    {
      this.Initialize((IEntity) cargoShip);
      this.m_cargoShip = cargoShip;
      this.m_moduleMbFactory = moduleMbFactory;
      this.m_assetsDb = assetsDb;
      this.m_shipRollHelper = new ShipRollHelper(weatherManager, startingRollAngle);
      CargoShipProto.Gfx graphicsFor = cargoShip.Prototype.GetGraphicsFor(cargoShip.FuelProto);
      float num = graphicsFor.ModuleSlotLength.ToUnityUnits() * (float) this.m_cargoShip.Modules.Count;
      GameObject clonedPrefabOrEmptyGo1 = assetsDb.GetClonedPrefabOrEmptyGo(graphicsFor.FrontPrefabPath);
      GameObject clonedPrefabOrEmptyGo2 = assetsDb.GetClonedPrefabOrEmptyGo(graphicsFor.BackPrefabPath);
      clonedPrefabOrEmptyGo1.transform.SetParent(this.transform, false);
      clonedPrefabOrEmptyGo1.transform.localPosition = new Vector3(num / 2f, 0.0f, 0.0f);
      clonedPrefabOrEmptyGo2.transform.SetParent(this.transform, false);
      clonedPrefabOrEmptyGo2.transform.localPosition = new Vector3((float) (-(double) num / 2.0), 0.0f, 0.0f);
      this.m_animHandler.SetSpeed(0.8f);
      this.m_animHandler.LoadAnimationFor(clonedPrefabOrEmptyGo2);
      this.m_modulesMb = new CargoShipModuleBaseMb[this.m_cargoShip.Modules.Count];
      for (int index = 0; index < this.m_cargoShip.Modules.Count; ++index)
        this.createModuleForSlot(index, this.m_cargoShip.Modules[index]);
      BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
      boxCollider.size = graphicsFor.BasicBoxColliderSize.AddX((Fix32) (graphicsFor.ModuleSlotLength * this.m_cargoShip.Modules.Count).Value).ToVector3();
      boxCollider.center = new Vector3(0.0f, 0.0f, 0.0f);
      gameLoop.RegisterDelayedRenderEvent<int>((Action<Action<int>>) (action => cargoShip.OnModuleRemoved += action), new Action<int>(this.OnModuleRemoved));
      gameLoop.RegisterDelayedRenderEvent<CargoShipModule, int>((Action<Action<CargoShipModule, int>>) (action => cargoShip.OnModuleAdded += action), new Action<CargoShipModule, int>(this.OnModuleAdded));
      this.m_rearEngineParticles = new OptionalParticlesWrapper(clonedPrefabOrEmptyGo2, "RearEngineParticles");
      this.m_rearExhaustParticles = new OptionalParticlesWrapper(clonedPrefabOrEmptyGo2, "RearExhaustParticles", true);
      this.stopParticles();
      this.m_engineSound = audioManager.CreateSound((EntityMb) this, new SoundParams(cargoShip.Prototype.Graphics.EngineSoundPath, SoundSignificance.Medium));
      this.m_arrivalSound = audioManager.CreateSound((EntityMb) this, new SoundParams(cargoShip.Prototype.Graphics.ArrivalSoundPath, SoundSignificance.Medium, false, false, true));
      this.m_departureSound = audioManager.CreateSound((EntityMb) this, new SoundParams(cargoShip.Prototype.Graphics.DepartureSoundPath, SoundSignificance.Medium, false, false, true));
    }

    private void OnModuleRemoved(int slot)
    {
      CargoShipModuleBaseMb shipModuleBaseMb = this.m_modulesMb[slot];
      if (shipModuleBaseMb != null)
      {
        GameObject gameObject = shipModuleBaseMb.gameObject;
        if (gameObject != null)
          gameObject.Destroy();
      }
      this.createModuleForSlot(slot, (Option<CargoShipModule>) Option.None);
    }

    private void OnModuleAdded(CargoShipModule module, int slot)
    {
      CargoShipModuleBaseMb shipModuleBaseMb = this.m_modulesMb[slot];
      if (shipModuleBaseMb != null)
      {
        GameObject gameObject = shipModuleBaseMb.gameObject;
        if (gameObject != null)
          gameObject.Destroy();
      }
      this.createModuleForSlot(slot, (Option<CargoShipModule>) module);
    }

    public CargoShipModuleBaseMb GetModuleMb(int slotIndex) => this.m_modulesMb[slotIndex];

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (this.m_previousState != this.m_cargoShip.State)
      {
        this.m_previousState = this.m_cargoShip.State;
        GameObject gameObject = this.gameObject;
        bool flag;
        switch (this.m_cargoShip.State)
        {
          case CargoShip.ShipState.ArrivingFromWorld:
          case CargoShip.ShipState.Docked:
          case CargoShip.ShipState.DepartingToWorld:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        gameObject.SetActive(flag);
        this.refreshAnimationType();
        this.m_dockProxyTransform.localPosition = this.m_cargoShip.Position3f.ToVector3();
        this.m_dockProxyTransform.localRotation = UnitQuaternion4f.FromAxisAngle(Vector3f.UnitZ, this.m_cargoShip.Direction).ToUnityQuaternion();
        if (this.m_cargoShip.State == CargoShip.ShipState.ArrivingFromWorld)
        {
          this.m_dockingAnimationSyncer.PlayNew(CargoShipMb.ARRIVAL_ANIM_SATE_ID);
          this.playParticles(time);
        }
        else if (this.m_cargoShip.State == CargoShip.ShipState.DepartingToWorld)
        {
          this.m_dockingAnimationSyncer.PlayNew(CargoShipMb.DEPARTURE_ANIM_SATE_ID);
          this.playParticles(time);
        }
        else
        {
          this.m_dockingAnimationSyncer.Stop();
          this.stopParticles();
        }
        if (this.m_previousState == CargoShip.ShipState.DepartingToWorld)
          this.m_playDepartureSoundAfterStep = new int?(time.SimStepsCount + CargoShipMb.DEPARTURE_SOUND_DELAY);
        else if (this.m_previousState == CargoShip.ShipState.ArrivingFromWorld)
          this.m_playArrivalSoundAfterStep = new int?(time.SimStepsCount + CargoShipMb.ARRIVAL_SOUND_DELAY);
      }
      this.m_shipRollHelper.SyncUpdate(time, this.m_previousState == CargoShip.ShipState.Docked ? 0.1f : 1f);
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
      this.m_dockingAnimationSyncer.Sync((float) (1.0 - (double) this.m_cargoShip.RemainingTransitionDuration.Ticks / (double) this.m_cargoShip.DockTransitionDurationForCurrentJourney.Ticks), (float) time.GameSpeedMult);
      this.syncParticles(time);
      this.m_animHandler.SetEnabled(this.m_cargoShip.State != CargoShip.ShipState.Docked || this.m_cargoShip.LastDockedStatus != CargoShip.DockedStatus.NotEnoughFuel && this.m_cargoShip.LastDockedStatus != CargoShip.DockedStatus.NoModulesBuilt && this.m_cargoShip.LastDockedStatus != CargoShip.DockedStatus.NotEnoughWorkers && this.m_cargoShip.LastDockedStatus != CargoShip.DockedStatus.Paused);
      this.m_animHandler.SyncUpdate(time);
      foreach (CargoShipModuleBaseMb shipModuleBaseMb in this.m_modulesMb)
        shipModuleBaseMb.SyncUpdate(time);
    }

    private void refreshAnimationType()
    {
      int index = this.m_cargoShip.CargoDepot.ReservedOceanAreaState.FirstValidAreasSetIndex;
      if (index < 0)
        index = 0;
      string animationsPrefabPath = this.m_cargoShip.CargoDepot.Prototype.DockingAnimationsPrefabPaths[index];
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
      this.m_animHandler.RenderUpdate(time);
      foreach (CargoShipModuleBaseMb shipModuleBaseMb in this.m_modulesMb)
        shipModuleBaseMb.RenderUpdate(time);
      bool isPlaying = this.m_previousState == CargoShip.ShipState.ArrivingFromWorld || this.m_previousState == CargoShip.ShipState.DepartingToWorld;
      this.transform.localRotation = this.m_shipRollHelper.RenderUpdateGetRotation(time);
      if (this.m_engineSound.HasValue)
        this.m_engineSound.Value.RenderUpdate(time, isPlaying);
      this.m_arrivalSound.ValueOrNull?.RenderUpdate(time);
      this.m_departureSound.ValueOrNull?.RenderUpdate(time);
    }

    private void createModuleForSlot(int moduleIndex, Option<CargoShipModule> module)
    {
      float unityUnits = this.m_cargoShip.Prototype.Graphics.ModuleSlotLength.RelTile1f.ToUnityUnits();
      float x = (float) (((double) unityUnits - (double) unityUnits * (double) this.m_cargoShip.Modules.Count) / 2.0 + (double) moduleIndex * (double) unityUnits);
      CargoShipModuleBaseMb moduleMb = this.m_moduleMbFactory.CreateModuleMb(this.m_cargoShip, module);
      this.m_modulesMb[moduleIndex] = moduleMb;
      GameObject gameObject = moduleMb.gameObject;
      gameObject.transform.SetParent(this.transform, false);
      gameObject.transform.localPosition = new Vector3(x, 0.0f, 0.0f);
    }

    public CargoShipMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_animHandler = new LoopAnimationHandler();
      this.m_previousState = ~CargoShip.ShipState.ArrivingFromWorld;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static CargoShipMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CargoShipMb.ARRIVAL_ANIM_SATE_ID = Animator.StringToHash("Arrival");
      CargoShipMb.DEPARTURE_ANIM_SATE_ID = Animator.StringToHash("Departure");
      CargoShipMb.DEPARTURE_SOUND_DELAY = 9.Seconds().Ticks;
      CargoShipMb.ARRIVAL_SOUND_DELAY = 10.Seconds().Ticks;
    }
  }
}
