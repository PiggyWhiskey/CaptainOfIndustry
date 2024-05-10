// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.DynamicGroundEntityMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles;
using Mafi.Unity.Audio;
using Mafi.Unity.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  public class DynamicGroundEntityMb : 
    EntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate,
    IEntityMbWithSimUpdateEnd
  {
    private DrivingEntity m_groundEntity;
    protected DynamicGroundEntityDeps Dependencies;
    private Vector3 m_currentPosition;
    private Vector3 m_futurePosition;
    private Vector3 m_futurePositionOnSim;
    private Quaternion m_currentRotation;
    private Quaternion m_futureRotation;
    private Quaternion m_futureRotationOnSim;
    private Tile2i m_lastVisitedTileOnSim;
    private DustinessData m_dustinessUnderEntity;
    private DustinessData m_dustinessUnderEntityOnSim;
    private float m_turnRate;
    private float m_turnRateOnSim;
    private float m_prevDirectionOnSim;
    private ImmutableArray<KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>> m_dustParticlers;
    private ImmutableArray<KeyValuePair<VehicleExhaustParticlesSpec, ParticleSystem>> m_exhaustParticlers;
    private float[] m_exhaustParticlersRates;
    private Option<EntitySoundMb> m_engineSound;
    private Option<EntitySoundMb> m_movementSound;
    private bool m_isMoving;
    private bool m_isEnabled;
    private bool m_firstUpdateAfterSync;
    private float m_randomHeightOffset;
    private bool m_updateExhaust;
    private bool m_updateExhaustOnSim;
    private int m_lastGameSpeed;

    public Vector3 CurrentPosition => this.m_currentPosition;

    public Quaternion CurrentRotation => this.m_currentRotation;

    public Vector3 FuturePosition => this.m_futurePosition;

    public Quaternion FutureRotation => this.m_futureRotation;

    internal void Initialize(
      DrivingEntity groundEntity,
      AssetsDb assetsDb,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      this.Initialize((IEntity) groundEntity);
      this.m_groundEntity = groundEntity.CheckNotNull<DrivingEntity>();
      this.Dependencies = dependencies;
      this.computeEntityPose(out this.m_futurePosition, out this.m_futureRotation);
      this.transform.position = this.m_futurePositionOnSim = this.m_futurePosition;
      this.transform.rotation = this.m_futureRotationOnSim = this.m_futureRotation;
      this.m_prevDirectionOnSim = this.m_groundEntity.Direction.Degrees.ToFloat();
      this.m_currentPosition = this.transform.position;
      this.m_currentRotation = this.transform.rotation;
      this.m_futurePosition = this.transform.position;
      this.m_futureRotation = this.transform.rotation;
      this.m_updateExhaust = true;
      this.m_randomHeightOffset = UnityEngine.Random.Range(-0.005f, 0.005f);
      this.m_dustParticlers = groundEntity.Prototype.Graphics.DustParticles.Select<KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>>((Func<DynamicEntityDustParticlesSpec, KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>>) (x => this.createDustParticler(x, assetsDb))).Where<KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>>((Func<KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>, bool>) (x => (UnityEngine.Object) x.Value != (UnityEngine.Object) null)).ToImmutableArray<KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>>();
      if (groundEntity.Prototype.Graphics.ExhaustParticlesSpec.HasValue)
      {
        VehicleExhaustParticlesSpec exhaustParticlesSpec = groundEntity.Prototype.Graphics.ExhaustParticlesSpec.Value;
        Lyst<KeyValuePair<VehicleExhaustParticlesSpec, ParticleSystem>> lyst = new Lyst<KeyValuePair<VehicleExhaustParticlesSpec, ParticleSystem>>();
        foreach (string gameObjectPath in exhaustParticlesSpec.GameObjectPaths)
        {
          GameObject resultGo;
          if (!this.gameObject.TryFindChild(gameObjectPath, out resultGo))
          {
            Log.Error("Unable to find exhaust child '" + gameObjectPath + "'");
          }
          else
          {
            ParticleSystem particleSystem;
            if (!this.tryCreateExhaustParticler(exhaustParticlesSpec, resultGo, out particleSystem))
              Log.Error("Failed to create particle system from '" + resultGo.name + "'.");
            else
              lyst.Add(Make.Kvp<VehicleExhaustParticlesSpec, ParticleSystem>(exhaustParticlesSpec, particleSystem));
          }
        }
        this.m_exhaustParticlers = lyst.ToImmutableArray();
      }
      else
        this.m_exhaustParticlers = ImmutableArray<KeyValuePair<VehicleExhaustParticlesSpec, ParticleSystem>>.Empty;
      this.m_exhaustParticlersRates = new float[this.m_exhaustParticlers.Length];
      this.m_engineSound = audioManager.CreateSound((EntityMb) this, new SoundParams(groundEntity.Prototype.Graphics.EngineSoundPath, SoundSignificance.Small));
      this.m_movementSound = audioManager.CreateSound((EntityMb) this, new SoundParams(groundEntity.Prototype.Graphics.MovementSoundPath, SoundSignificance.Small));
    }

    void IEntityMbWithSimUpdateEnd.SimUpdateEnd() => this.simUpdateEnd();

    private void simUpdateEnd()
    {
      if (!this.m_groundEntity.IsSpawned)
      {
        this.m_dustinessUnderEntityOnSim = new DustinessData();
        this.m_turnRateOnSim = 0.0f;
      }
      else
      {
        this.computeEntityPose(out this.m_futurePositionOnSim, out this.m_futureRotationOnSim);
        float num1 = this.m_groundEntity.Direction.Degrees.ToFloat();
        this.m_turnRateOnSim = (this.m_prevDirectionOnSim - num1).Abs();
        this.m_prevDirectionOnSim = num1;
        if (this.m_exhaustParticlersRates.Length != 0)
        {
          if (this.m_groundEntity.IsEngineOn)
          {
            Percent percent1 = this.m_groundEntity.AccelerationPercentOfPeak;
            if ((long) percent1.RawValue * (long) this.m_groundEntity.SpeedPercentOfPeak.RawValue < 0L)
            {
              percent1 = Percent.Zero;
            }
            else
            {
              percent1 = percent1.Abs();
              if (this.m_groundEntity.DrivingData.CanTurnInPlace)
                percent1 = percent1.Max(this.m_groundEntity.SteeringAccelerationPercent.Abs());
            }
            Percent percent2 = this.m_groundEntity.SpeedPercentOfPeak.Abs();
            for (int index = 0; index < this.m_exhaustParticlers.Length; ++index)
            {
              VehicleExhaustParticlesSpec key = this.m_exhaustParticlers[index].Key;
              float num2 = (float) ((double) key.BaseParticleRate + (double) key.ParticlesSpeedRate * (double) percent2.ToFloat() + (double) key.ParticlesAccelerationRate * (double) percent1.ToFloat());
              if ((double) this.m_exhaustParticlersRates[index] != (double) num2)
              {
                this.m_updateExhaustOnSim = true;
                this.m_exhaustParticlersRates[index] = num2;
              }
            }
          }
          else
          {
            for (int index = 0; index < this.m_exhaustParticlers.Length; ++index)
            {
              if ((double) this.m_exhaustParticlersRates[index] != 0.0)
              {
                this.m_updateExhaustOnSim = true;
                this.m_exhaustParticlersRates[index] = 0.0f;
              }
            }
          }
        }
        Tile2i groundPositionTile2i = this.m_groundEntity.GroundPositionTile2i;
        if (!(this.m_lastVisitedTileOnSim != groundPositionTile2i))
          return;
        this.m_lastVisitedTileOnSim = groundPositionTile2i;
        this.m_dustinessUnderEntityOnSim = this.Dependencies.ParticlesController.ComputeDustiness(this.m_groundEntity);
      }
    }

    public virtual void SyncUpdate(GameTime time)
    {
      Assert.That<DrivingEntity>(this.m_groundEntity).IsNotNull<DrivingEntity>("DynamicGroundEntityMb is not initialized.");
      this.m_firstUpdateAfterSync = true;
      this.m_currentPosition = this.m_futurePosition;
      this.m_currentRotation = this.m_futureRotation;
      this.m_futurePosition = this.m_futurePositionOnSim;
      this.m_futureRotation = this.m_futureRotationOnSim;
      this.m_dustinessUnderEntity = this.m_dustinessUnderEntity.Lerp(this.m_dustinessUnderEntityOnSim, 0.2f);
      this.m_turnRate = this.m_turnRateOnSim;
      this.m_updateExhaust = this.m_updateExhaustOnSim;
      if (this.m_groundEntity.IsSpawned != this.gameObject.activeSelf)
      {
        this.gameObject.SetActive(this.m_groundEntity.IsSpawned);
        this.m_currentPosition = this.m_futurePosition;
        this.m_currentRotation = this.m_futureRotation;
      }
      this.m_isMoving = this.m_groundEntity.IsMoving;
      this.m_isEnabled = this.m_groundEntity.IsEnabled;
    }

    public virtual void RenderUpdate(GameTime time)
    {
      if (!this.gameObject.activeSelf)
        return;
      this.transform.position = Vector3.LerpUnclamped(this.m_currentPosition, this.m_futurePosition, time.AbsoluteT);
      this.transform.rotation = Quaternion.SlerpUnclamped(this.m_currentRotation, this.m_futureRotation, time.AbsoluteT);
      float vehicleSpeedFloat = this.m_groundEntity.Speed.Value.ToFloat();
      float pitch = 0.0f;
      if (this.m_isEnabled && (this.m_engineSound.HasValue || this.m_movementSound.HasValue))
        pitch = (double) vehicleSpeedFloat >= 0.0 ? (float) (0.60000002384185791 + 0.40000000596046448 * ((double) vehicleSpeedFloat / (double) this.m_groundEntity.Prototype.DrivingData.MaxForwardsSpeed.Value.ToFloat())) : (float) (0.5 + 0.30000001192092896 * (-(double) vehicleSpeedFloat / (double) this.m_groundEntity.Prototype.DrivingData.MaxBackwardsSpeed.Value.ToFloat()));
      if (this.m_engineSound.HasValue)
      {
        EntitySoundMb entitySoundMb = this.m_engineSound.Value;
        if (this.m_isEnabled)
        {
          entitySoundMb.SetPitch(pitch);
          entitySoundMb.FadeToVolume(this.m_isMoving ? entitySoundMb.OriginalVolume : entitySoundMb.OriginalVolume * 0.75f);
        }
        entitySoundMb.RenderUpdate(time, this.m_isEnabled);
      }
      if (this.m_movementSound.HasValue)
      {
        EntitySoundMb entitySoundMb = this.m_movementSound.Value;
        if (this.m_isEnabled)
          entitySoundMb.SetPitch(pitch);
        entitySoundMb.RenderUpdate(time, this.m_isMoving);
      }
      if (!this.m_firstUpdateAfterSync)
        return;
      this.m_firstUpdateAfterSync = false;
      this.updateOnceAfterSync(time, vehicleSpeedFloat);
    }

    private void updateOnceAfterSync(GameTime time, float vehicleSpeedFloat)
    {
      if (this.m_lastGameSpeed != time.GameSpeedMult)
      {
        this.m_lastGameSpeed = time.GameSpeedMult;
        foreach (KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem> dustParticler in this.m_dustParticlers)
          dustParticler.Value.main.simulationSpeed = (float) time.GameSpeedMult;
        foreach (KeyValuePair<VehicleExhaustParticlesSpec, ParticleSystem> exhaustParticler in this.m_exhaustParticlers)
        {
          exhaustParticler.Value.main.simulationSpeed = (float) time.GameSpeedMult;
          ParticleSystem.InheritVelocityModule inheritVelocity = exhaustParticler.Value.inheritVelocity;
          if (inheritVelocity.enabled)
            inheritVelocity.curveMultiplier = time.GameSpeedMult == 0 ? 1f : 1f / (float) time.GameSpeedMult;
        }
      }
      if ((double) this.m_dustinessUnderEntity.Dustiness < 0.0099999997764825821)
      {
        foreach (KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem> dustParticler in this.m_dustParticlers)
        {
          ParticleSystem particleSystem = dustParticler.Value;
          if (particleSystem.particleCount <= 0)
          {
            if (particleSystem.isEmitting)
              particleSystem.Stop(false);
          }
          else
            particleSystem.emission.rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f;
        }
      }
      else
      {
        foreach (KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem> dustParticler in this.m_dustParticlers)
        {
          ParticleSystem particleSystem = dustParticler.Value;
          if (!particleSystem.isEmitting)
            particleSystem.Play(false);
          ParticleSystem.MainModule main = particleSystem.main with
          {
            startColor = (ParticleSystem.MinMaxGradient) this.m_dustinessUnderEntity.DustColor,
            startSpeed = (ParticleSystem.MinMaxCurve) (vehicleSpeedFloat * 2f)
          };
          DynamicEntityDustParticlesSpec key = dustParticler.Key;
          particleSystem.emission.rateOverTime = (ParticleSystem.MinMaxCurve) (this.m_dustinessUnderEntity.Dustiness * (this.m_groundEntity.Speed < key.MinSpeed ? this.m_turnRate * key.ParticlesPerDegreeMult : vehicleSpeedFloat * key.ParticlesPerSpeedMult));
        }
      }
      if (!this.m_updateExhaust)
        return;
      this.m_updateExhaust = false;
      for (int index = 0; index < this.m_exhaustParticlers.Length; ++index)
      {
        KeyValuePair<VehicleExhaustParticlesSpec, ParticleSystem> exhaustParticler = this.m_exhaustParticlers[index];
        float exhaustParticlersRate = this.m_exhaustParticlersRates[index];
        ParticleSystem particleSystem = exhaustParticler.Value;
        if (particleSystem.isEmitting)
        {
          if ((double) exhaustParticlersRate <= 0.0)
          {
            particleSystem.Stop();
            continue;
          }
        }
        else if ((double) exhaustParticlersRate > 0.0)
          particleSystem.Play(false);
        else
          continue;
        particleSystem.main.startSpeed = (ParticleSystem.MinMaxCurve) (exhaustParticler.Key.BaseEmitSpeed + exhaustParticler.Key.VariableEmitSpeed * exhaustParticlersRate * exhaustParticler.Key.InverseMaxParticleRate);
        particleSystem.emission.rateOverTime = (ParticleSystem.MinMaxCurve) exhaustParticlersRate;
      }
    }

    private KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem> createDustParticler(
      DynamicEntityDustParticlesSpec spec,
      AssetsDb assetsDb)
    {
      GameObject prefab;
      if (!assetsDb.TryGetClonedPrefab(spec.PrefabPath, out prefab))
      {
        Log.Warning("Failed to get particles for vehicle.");
        return new KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>();
      }
      ParticleSystem component = prefab.GetComponent<ParticleSystem>();
      if (!(bool) (UnityEngine.Object) component)
      {
        Log.Warning("Find particles MB for vehicle.");
        return new KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>();
      }
      prefab.transform.SetParent(this.transform, false);
      prefab.transform.localPosition = spec.RelativePosition.ToVector3();
      prefab.SetActive(true);
      component.Play();
      ParticleSystem.MainModule main = component.main;
      ParticleSystem.MinMaxCurve minMaxCurve1 = main.startLifetime;
      float num1 = minMaxCurve1.constant * spec.DustScale;
      main.startLifetime = (ParticleSystem.MinMaxCurve) num1;
      ref ParticleSystem.MainModule local = ref main;
      minMaxCurve1 = main.startSize;
      ParticleSystem.MinMaxCurve minMaxCurve2 = (ParticleSystem.MinMaxCurve) (minMaxCurve1.constant * spec.DustScale);
      local.startSize = minMaxCurve2;
      float num2 = 3f;
      DrivingEntityProto prototype = this.m_groundEntity.Prototype;
      if (prototype != null)
        num2 = prototype.DrivingData.MaxForwardsSpeed.Max(prototype.DrivingData.MaxBackwardsSpeed).ToUnityUnits();
      main.maxParticles = ((float) ((double) num1 * (double) spec.ParticlesPerSpeedMult * (double) num2 * 1.1000000238418579)).CeilToInt();
      component.emission.rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f;
      return new KeyValuePair<DynamicEntityDustParticlesSpec, ParticleSystem>(spec, component);
    }

    private bool tryCreateExhaustParticler(
      VehicleExhaustParticlesSpec spec,
      GameObject particlerGo,
      out ParticleSystem particleSystem)
    {
      particleSystem = particlerGo.GetComponent<ParticleSystem>();
      if (!(bool) (UnityEngine.Object) particleSystem)
        return false;
      particlerGo.transform.SetParent(this.transform, false);
      particlerGo.SetActive(true);
      particleSystem.Play();
      ParticleSystem.MainModule main = particleSystem.main;
      main.maxParticles = (main.startLifetime.constant * spec.MaxRate).CeilToInt();
      particleSystem.emission.rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f;
      return true;
    }

    public static void ComputeWheelPositions(
      DynamicGroundEntity entity,
      out Vector3 frontLeftWheel,
      out Vector3 frontRightWheel,
      out Vector3 rearLeftWheel,
      out Vector3 rearRightWheel)
    {
      Tile2f position2f = entity.Position2f;
      AngleDegrees1f direction = entity.Direction;
      IVehicleSurfaceProvider surfaceProvider = entity.SurfaceProvider;
      DynamicGroundEntityProto.Gfx graphics = entity.Prototype.Graphics;
      Tile2f coord1 = position2f + graphics.FrontContactPtsOffset.Rotate(direction);
      frontLeftWheel = coord1.ExtendHeight(surfaceProvider.GetInterpolatedVehicleSurfaceAt(coord1)).ToVector3();
      Tile2f tile2f1 = position2f;
      RelTile2f relTile2f1 = graphics.FrontContactPtsOffset;
      relTile2f1 = relTile2f1.MultiplyY((Fix32) -1);
      RelTile2f relTile2f2 = relTile2f1.Rotate(direction);
      Tile2f coord2 = tile2f1 + relTile2f2;
      frontRightWheel = coord2.ExtendHeight(surfaceProvider.GetInterpolatedVehicleSurfaceAt(coord2)).ToVector3();
      Tile2f tile2f2 = position2f;
      RelTile2f relTile2f3 = graphics.RearContactPtsOffset;
      relTile2f3 = relTile2f3.MultiplyY((Fix32) -1);
      RelTile2f relTile2f4 = relTile2f3.Rotate(direction);
      Tile2f coord3 = tile2f2 - relTile2f4;
      rearLeftWheel = coord3.ExtendHeight(surfaceProvider.GetInterpolatedVehicleSurfaceAt(coord3)).ToVector3();
      Tile2f coord4 = position2f - graphics.RearContactPtsOffset.Rotate(direction);
      rearRightWheel = coord4.ExtendHeight(surfaceProvider.GetInterpolatedVehicleSurfaceAt(coord4)).ToVector3();
    }

    private void computeEntityPose(out Vector3 outPosition, out Quaternion outRotation)
    {
      if (this.m_groundEntity.ForceFlatGround)
      {
        outPosition = this.m_groundEntity.Position3f.ToVector3();
        outRotation = Quaternion.AngleAxis(this.m_groundEntity.Direction.ToUnityAngleDegrees(), Vector3.up);
        outPosition.y += this.m_randomHeightOffset;
      }
      else
      {
        Vector3 frontLeftWheel;
        Vector3 frontRightWheel;
        Vector3 rearLeftWheel;
        Vector3 rearRightWheel;
        DynamicGroundEntityMb.ComputeWheelPositions((DynamicGroundEntity) this.m_groundEntity, out frontLeftWheel, out frontRightWheel, out rearLeftWheel, out rearRightWheel);
        DynamicGroundEntityProto.Gfx graphics = this.m_groundEntity.Prototype.Graphics;
        Vector3 vector3_1 = (frontLeftWheel + frontRightWheel) * 0.5f;
        Vector3 vector3_2 = (rearLeftWheel + rearRightWheel) * 0.5f;
        Fix32 fix32 = graphics.FrontContactPtsOffset.X + graphics.RearContactPtsOffset.X;
        outPosition = (graphics.FrontContactPtsOffset.X / fix32).ToFloat() * vector3_1 + (graphics.RearContactPtsOffset.X / fix32).ToFloat() * vector3_2;
        outPosition.y += this.m_randomHeightOffset;
        Vector3 lhs = vector3_1 - vector3_2;
        Vector3 rhs = (frontRightWheel + rearRightWheel) * 0.5f - (frontLeftWheel + rearLeftWheel) * 0.5f;
        outRotation = Quaternion.LookRotation(-rhs, Vector3.Cross(lhs, rhs));
      }
    }

    public DynamicGroundEntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lastGameSpeed = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
