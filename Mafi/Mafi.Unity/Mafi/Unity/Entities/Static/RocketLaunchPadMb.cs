// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.RocketLaunchPadMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Simulation;
using Mafi.Core.SpaceProgram;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities.Rockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class RocketLaunchPadMb : 
    StaticEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IRocketHoldingEntityMb,
    IEntityMbWithRenderUpdate
  {
    private RocketLaunchPad m_launchPad;
    private IRocketLaunchManager m_rocketLaunchManager;
    private IInputScheduler m_inputScheduler;
    private bool m_sprinklersAreOn;
    private ParticleSystem[] m_waterSprinklers;
    private Animator[] m_waterPumpsAnimators;
    private Animator[] m_bridgeAttachAnimators;
    private Animator m_rocketHolderAnimator;
    private Animator m_crawlerBridgeAnimator;
    private RocketLaunchPadState m_launchPadState;
    private bool m_crawlerBridgeErected;
    private Animator m_elevatorAnimator;
    private Animator m_cargoTunnelAnimator;
    private int m_syncedGameSpeed;
    private ImmutableArray<Option<EntitySoundMb>> m_countdownSounds;
    private int? m_lastSecondPlayed;

    public Transform RocketParent => this.transform;

    public Quaternion? LocalRotation => new Quaternion?(Quaternion.identity);

    public void Initialize(
      RocketLaunchPad launchPad,
      EntityAudioManager audioManager,
      IRocketLaunchManager rocketLaunchManager,
      IInputScheduler inputScheduler)
    {
      this.Initialize((ILayoutEntity) launchPad);
      this.m_launchPad = launchPad;
      this.m_launchPadState = launchPad.State;
      this.m_rocketLaunchManager = rocketLaunchManager;
      this.m_inputScheduler = inputScheduler;
      GameObject[] source = new GameObject[this.transform.childCount];
      for (int index = 0; index < source.Length; ++index)
        source[index] = this.transform.GetChild(index).gameObject;
      this.m_sprinklersAreOn = launchPad.IsSprinklingWater;
      this.m_waterSprinklers = ((IEnumerable<GameObject>) source).Where<GameObject>((Func<GameObject, bool>) (x => x.name.StartsWith("PadSprinkler", StringComparison.Ordinal))).Select<GameObject, ParticleSystem>((Func<GameObject, ParticleSystem>) (x => x.GetComponent<ParticleSystem>())).ToArray<ParticleSystem>();
      if (this.m_sprinklersAreOn)
      {
        foreach (ParticleSystem waterSprinkler in this.m_waterSprinklers)
        {
          waterSprinkler.Simulate(waterSprinkler.main.startLifetime.constant);
          waterSprinkler.Play();
        }
      }
      this.m_waterPumpsAnimators = ((IEnumerable<GameObject>) source).Where<GameObject>((Func<GameObject, bool>) (x => x.name.StartsWith("WaterPump", StringComparison.Ordinal))).Select<GameObject, Animator>((Func<GameObject, Animator>) (x => x.GetComponent<Animator>())).ToArray<Animator>();
      foreach (Animator waterPumpsAnimator in this.m_waterPumpsAnimators)
      {
        waterPumpsAnimator.Play("PumpAnimation");
        waterPumpsAnimator.enabled = this.m_sprinklersAreOn;
      }
      this.m_bridgeAttachAnimators = ((IEnumerable<GameObject>) source).Where<GameObject>((Func<GameObject, bool>) (x => x.name.StartsWith("BridgeAttach", StringComparison.Ordinal))).Select<GameObject, Animator>((Func<GameObject, Animator>) (x => x.GetComponent<Animator>())).ToArray<Animator>();
      bool flag = launchPad.State == RocketLaunchPadState.AttachingRocket || launchPad.State == RocketLaunchPadState.RocketAttached || launchPad.State == RocketLaunchPadState.LaunchCountdown;
      string stateName = flag ? "BridgeAttach" : "BridgeDetach";
      foreach (Animator bridgeAttachAnimator in this.m_bridgeAttachAnimators)
        bridgeAttachAnimator.Play(stateName, 0, 1f);
      this.m_rocketHolderAnimator = ((IEnumerable<GameObject>) source).First<GameObject>((Func<GameObject, bool>) (x => x.name == "RocketHolder")).GetComponent<Animator>();
      this.m_rocketHolderAnimator.Play(flag ? "RocketHolderDown" : "RocketHolderUp", 0, 1f);
      this.m_crawlerBridgeErected = launchPad.IsCrawlerBridgeErected;
      this.m_crawlerBridgeAnimator = ((IEnumerable<GameObject>) source).First<GameObject>((Func<GameObject, bool>) (x => x.name == "CrawlerBridge")).GetComponent<Animator>();
      this.m_crawlerBridgeAnimator.Play(launchPad.IsCrawlerBridgeErected ? "Erect" : "Hide", 0, 1f);
      this.m_elevatorAnimator = ((IEnumerable<GameObject>) source).First<GameObject>((Func<GameObject, bool>) (x => x.name == "Elevator")).GetComponent<Animator>();
      this.m_elevatorAnimator.Play(flag ? "ElevatorUp" : "ElevatorDown", 0, 1f);
      this.m_cargoTunnelAnimator = ((IEnumerable<GameObject>) source).First<GameObject>((Func<GameObject, bool>) (x => x.name == "CargoTunnel")).GetComponent<Animator>();
      this.m_cargoTunnelAnimator.Play(flag ? "CargoTunnelAttach" : "CargoTunnelDetach", 0, 1f);
      string[] strArray = new string[6]
      {
        "Assets/Base/Buildings/RocketLaunchPad/Audio/LiftOff.prefab",
        "Assets/Base/Buildings/RocketLaunchPad/Audio/1.prefab",
        "Assets/Base/Buildings/RocketLaunchPad/Audio/2.prefab",
        "Assets/Base/Buildings/RocketLaunchPad/Audio/3.prefab",
        "Assets/Base/Buildings/RocketLaunchPad/Audio/4.prefab",
        "Assets/Base/Buildings/RocketLaunchPad/Audio/5.prefab"
      };
      Lyst<Option<EntitySoundMb>> lyst = new Lyst<Option<EntitySoundMb>>();
      foreach (string prefabPath in strArray)
      {
        Option<EntitySoundMb> sound = audioManager.CreateSound((EntityMb) this, new SoundParams(prefabPath, SoundSignificance.High, false, false, true));
        lyst.Add(sound);
      }
      this.m_countdownSounds = lyst.ToImmutableArray();
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      if (time.GameSpeedMult != this.m_syncedGameSpeed)
      {
        this.m_syncedGameSpeed = time.GameSpeedMult;
        float syncedGameSpeed = (float) this.m_syncedGameSpeed;
        this.m_cargoTunnelAnimator.speed = syncedGameSpeed;
        this.m_elevatorAnimator.speed = syncedGameSpeed;
        this.m_rocketHolderAnimator.speed = syncedGameSpeed;
        this.m_crawlerBridgeAnimator.speed = syncedGameSpeed;
        foreach (Animator bridgeAttachAnimator in this.m_bridgeAttachAnimators)
          bridgeAttachAnimator.speed = syncedGameSpeed;
        foreach (Animator waterPumpsAnimator in this.m_waterPumpsAnimators)
          waterPumpsAnimator.speed = syncedGameSpeed;
        foreach (ParticleSystem waterSprinkler in this.m_waterSprinklers)
          waterSprinkler.main.simulationSpeed = syncedGameSpeed;
      }
      if (this.m_launchPadState != this.m_launchPad.State)
      {
        this.m_launchPadState = this.m_launchPad.State;
        switch (this.m_launchPadState)
        {
          case RocketLaunchPadState.WaitingForRocket:
            break;
          case RocketLaunchPadState.AttachingRocket:
            foreach (Animator bridgeAttachAnimator in this.m_bridgeAttachAnimators)
              bridgeAttachAnimator.Play("BridgeAttach");
            break;
          case RocketLaunchPadState.RocketAttached:
            this.m_cargoTunnelAnimator.Play("CargoTunnelAttach");
            this.m_elevatorAnimator.Play("ElevatorUp");
            break;
          case RocketLaunchPadState.LaunchCountdown:
            this.m_cargoTunnelAnimator.Play("CargoTunnelDetach");
            if (this.m_rocketLaunchManager.LaunchesCount == 0)
            {
              this.m_inputScheduler.ScheduleInputCmd<GameSpeedChangeCmd>(new GameSpeedChangeCmd(1));
              break;
            }
            break;
          case RocketLaunchPadState.RocketLaunching:
            foreach (Animator bridgeAttachAnimator in this.m_bridgeAttachAnimators)
              bridgeAttachAnimator.Play("BridgeDetach");
            this.m_rocketHolderAnimator.Play("RocketHolderUp");
            this.m_elevatorAnimator.Play("ElevatorDown");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      if (this.m_crawlerBridgeErected != this.m_launchPad.IsCrawlerBridgeErected)
      {
        this.m_crawlerBridgeErected = this.m_launchPad.IsCrawlerBridgeErected;
        if (this.m_crawlerBridgeErected)
        {
          this.m_crawlerBridgeAnimator.Play("Erect");
          this.m_rocketHolderAnimator.Play("RocketHolderDown");
        }
        else
          this.m_crawlerBridgeAnimator.Play("Hide");
      }
      if (this.m_sprinklersAreOn != this.m_launchPad.IsSprinklingWater)
      {
        this.m_sprinklersAreOn = this.m_launchPad.IsSprinklingWater;
        if (this.m_sprinklersAreOn)
        {
          foreach (ParticleSystem waterSprinkler in this.m_waterSprinklers)
            waterSprinkler.Play();
        }
        else
        {
          foreach (ParticleSystem waterSprinkler in this.m_waterSprinklers)
            waterSprinkler.Stop();
        }
        foreach (Behaviour waterPumpsAnimator in this.m_waterPumpsAnimators)
          waterPumpsAnimator.enabled = this.m_sprinklersAreOn;
      }
      Duration? launchCountdown = this.m_launchPad.LaunchCountdown;
      if (launchCountdown.HasValue)
      {
        launchCountdown = this.m_launchPad.LaunchCountdown;
        int intCeiled = launchCountdown.Value.Seconds.ToIntCeiled();
        int num = intCeiled;
        int? lastSecondPlayed = this.m_lastSecondPlayed;
        int valueOrDefault = lastSecondPlayed.GetValueOrDefault();
        if (num == valueOrDefault & lastSecondPlayed.HasValue)
          return;
        this.m_lastSecondPlayed = new int?(intCeiled);
        if (intCeiled < 1 || intCeiled >= this.m_countdownSounds.Length)
          return;
        this.m_countdownSounds[intCeiled].ValueOrNull?.Play();
      }
      else
      {
        if (this.m_lastSecondPlayed.HasValue)
          this.m_countdownSounds[0].ValueOrNull?.Play();
        this.m_lastSecondPlayed = new int?();
      }
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      foreach (Option<EntitySoundMb> countdownSound in this.m_countdownSounds)
        countdownSound.ValueOrNull?.RenderUpdate(time);
    }

    public RocketLaunchPadMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
