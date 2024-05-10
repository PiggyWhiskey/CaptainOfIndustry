// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.ExcavatorMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Dynamic;
using Mafi.Unity.Terrain;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class ExcavatorMb : PathFindingEntityMb
  {
    private Excavator m_excavator;
    private Transform m_cabinTransform;
    private Animator m_animator;
    private float m_currentCabinAngle;
    private float m_futureCabinAngle;
    private ExcavatorShovelState m_shovelState;
    private bool m_setAnimation;
    private LooseProductQuantity m_cargo;
    private LooseProductQuantity m_prevCargo;
    private LooseProductMultiTextureSetter m_pileMaterialSetter;
    private GameObject m_activePile;
    private GameObject m_smoothPileHalf;
    private GameObject m_smoothPileFull;
    private GameObject m_roughPileHalf;
    private GameObject m_roughPileFull;
    private TracksAnimator m_tracksAnimator;
    private float m_cabinCompensationAngle;
    private VehicleLightsController m_lightsController;
    private ImmutableArray<EntitySoundMb> m_digSounds;
    private ImmutableArray<EntitySoundMb> m_dumpSounds;
    private Option<EntitySoundMb> m_currentShovelSound;
    private IRandom m_random;

    internal void Initialize(
      Excavator excavator,
      AssetsDb assetsDb,
      IWeatherManager weatherManager,
      LooseProductMaterialManager looseProductMaterialManager,
      EntityAudioManager audioManager,
      RandomProvider randomProvider,
      DynamicGroundEntityDeps dependencies)
    {
      this.m_excavator = excavator;
      this.m_random = randomProvider.GetNonSimRandomFor((object) this);
      this.m_animator = this.gameObject.GetComponentOrCreateNewAndLogError<Animator>();
      ExcavatorProto.Gfx graphics = excavator.Prototype.Graphics;
      this.m_cabinCompensationAngle = graphics.CabinAngleCompensation.ToUnityAngleDegrees();
      Lyst<Renderer> renderers = new Lyst<Renderer>(4);
      GameObject pileParent;
      if (this.gameObject.TryFindChild(graphics.PileParentPath, out pileParent))
      {
        setPile(graphics.SmoothPileHalf, out this.m_smoothPileHalf);
        setPile(graphics.SmoothPileFull, out this.m_smoothPileFull);
        setPile(graphics.RoughPileHalf, out this.m_roughPileHalf);
        setPile(graphics.RoughPileFull, out this.m_roughPileFull);
      }
      else
        Log.Error("Failed to find pile parent '" + graphics.PileParentPath + "' " + string.Format("on excavator '{0}'.", (object) excavator.Prototype.Id));
      this.m_pileMaterialSetter = looseProductMaterialManager.SetupSharedMaterialFor(renderers.Distinct<Renderer>().ToArray<Renderer>());
      if (!this.transform.TryFindChild(graphics.CabinModelName, out this.m_cabinTransform))
      {
        Log.Error(string.Format("Failed to find cabin '{0}' of '{1}'.", (object) graphics.CabinModelName, (object) excavator.Prototype.Id));
        this.m_cabinTransform = this.transform;
      }
      this.m_currentCabinAngle = this.m_futureCabinAngle = this.m_excavator.CabinDirectionRelative.ToUnityAngleDegrees().Modulo(360f);
      this.m_cabinTransform.localRotation = Quaternion.AngleAxis(this.m_futureCabinAngle - this.m_cabinCompensationAngle, Vector3.up);
      this.m_tracksAnimator = new TracksAnimator(this.transform, graphics.LeftTrackModelName, graphics.RightTrackModelName, graphics.SpacingBetweenTracks.ToUnityUnits(), graphics.TrackTextureLength.ToUnityUnits());
      this.m_lightsController = new VehicleLightsController(weatherManager, randomProvider.GetNonSimRandomFor((object) this, excavator.Id.ToString()), (DynamicGroundEntity) excavator, this.gameObject, new int?(Shader.PropertyToID("_EmissionStrength")));
      Lyst<EntitySoundMb> lyst = new Lyst<EntitySoundMb>();
      foreach (string digSound in excavator.Prototype.Graphics.DigSounds)
      {
        Option<EntitySoundMb> sound = audioManager.CreateSound((EntityMb) this, new SoundParams(digSound, SoundSignificance.Small, false, false, true));
        if (sound.HasValue)
          lyst.Add(sound.Value);
      }
      this.m_digSounds = lyst.ToImmutableArrayAndClear();
      foreach (string dumpSound in excavator.Prototype.Graphics.DumpSounds)
      {
        Option<EntitySoundMb> sound = audioManager.CreateSound((EntityMb) this, new SoundParams(dumpSound, SoundSignificance.Small, false, false, true));
        if (sound.HasValue)
          lyst.Add(sound.Value);
      }
      this.m_dumpSounds = lyst.ToImmutableArrayAndClear();
      this.Initialize((PathFindingEntity) excavator, assetsDb, audioManager, dependencies);

      void setPile(string pileName, out GameObject go)
      {
        if (pileParent.TryFindChild(pileName, out go))
        {
          go.SetActive(false);
          Renderer component;
          if (go.TryGetComponent<Renderer>(out component))
            renderers.Add(component);
          else
            Log.Warning(string.Format("Pile '{0}' does not have renderer on excavator '{1}'.", (object) pileName, (object) excavator.Prototype.Id));
        }
        else
        {
          Log.Warning(string.Format("Failed to find pile '{0}' on excavator '{1}'.", (object) pileName, (object) excavator.Prototype.Id));
          go = new GameObject("Dummy pile");
        }
      }
    }

    public override void SyncUpdate(GameTime time)
    {
      this.m_cargo = this.m_excavator.Cargo.FirstOrPhantom.Product is LooseProductProto product ? new LooseProductQuantity(product, this.m_excavator.Cargo.TotalQuantity) : LooseProductQuantity.None;
      this.m_currentCabinAngle = this.m_futureCabinAngle;
      this.m_futureCabinAngle = this.m_excavator.CabinDirectionRelative.ToUnityAngleDegrees().Modulo(360f);
      if (this.m_shovelState != this.m_excavator.NextShovelState)
      {
        this.m_shovelState = this.m_excavator.NextShovelState;
        if (this.m_shovelState == ExcavatorShovelState.Mine)
        {
          this.m_currentShovelSound = (Option<EntitySoundMb>) this.m_digSounds.SampleRandomOrDefault(this.m_random);
          this.m_currentShovelSound.ValueOrNull?.Play();
        }
        else if (this.m_shovelState == ExcavatorShovelState.Dump)
        {
          this.m_currentShovelSound = (Option<EntitySoundMb>) this.m_dumpSounds.SampleRandomOrDefault(this.m_random);
          this.m_currentShovelSound.ValueOrNull?.Play();
        }
        this.m_setAnimation = true;
      }
      else
        this.m_setAnimation = false;
      this.m_tracksAnimator.Sync(time, this.m_excavator.Speed, -this.m_excavator.SteeringAngle);
      this.m_lightsController.UpdateVehicleLights();
      base.SyncUpdate(time);
    }

    public override void RenderUpdate(GameTime time)
    {
      base.RenderUpdate(time);
      float num = (this.m_futureCabinAngle - this.m_currentCabinAngle).Modulo(360f);
      if ((double) num > 180.0)
        num -= 360f;
      Assert.That<float>(num).IsWithinIncl(-180f, 180f);
      this.m_cabinTransform.localRotation = Quaternion.AngleAxis(this.m_currentCabinAngle + num * time.AbsoluteT - this.m_cabinCompensationAngle, Vector3.up);
      if (this.m_setAnimation)
      {
        this.m_setAnimation = false;
        this.m_animator.Play(this.m_excavator.Prototype.Graphics.GetAnimatorStateName(this.m_shovelState));
      }
      this.m_animator.speed = this.m_excavator.SpeedFactor.ToFloat() * (float) time.GameSpeedMult;
      this.m_animator.enabled = !time.IsGamePaused;
      if ((Proto) this.m_cargo.Product != (Proto) this.m_prevCargo.Product)
        this.m_pileMaterialSetter.SetTexture(this.m_cargo.Product.LooseSlimId);
      if (this.m_cargo.Quantity != this.m_prevCargo.Quantity)
      {
        bool useRoughPileMeshes = this.m_cargo.Product.Graphics.UseRoughPileMeshes;
        GameObject gameObject = !(this.m_cargo.Quantity > 3 * this.m_excavator.Prototype.Capacity / 4) ? (!this.m_cargo.Quantity.IsPositive ? (GameObject) null : (useRoughPileMeshes ? this.m_roughPileHalf : this.m_smoothPileHalf)) : (useRoughPileMeshes ? this.m_roughPileFull : this.m_smoothPileFull);
        if ((Object) this.m_activePile != (Object) gameObject)
        {
          if ((bool) (Object) this.m_activePile)
            this.m_activePile.SetActive(false);
          if ((bool) (Object) gameObject)
            gameObject.SetActive(true);
          this.m_activePile = gameObject;
        }
      }
      this.m_currentShovelSound.ValueOrNull?.RenderUpdate(time);
      this.m_prevCargo = this.m_cargo;
      this.m_tracksAnimator.RenderUpdate(time);
    }

    protected void OnDrawGizmos()
    {
      Vector3 vector3_1 = this.m_excavator.Position3f.AddZ(Fix32.Half).ToVector3();
      Gizmos.color = Color.gray;
      Vector3 vector3_2 = (this.m_excavator.CabinDirection.DirectionVector * 3).ExtendZ((Fix32) 0).ToVector3();
      Gizmos.DrawLine(vector3_1, vector3_1 + vector3_2);
      if (!this.m_excavator.CurrentJob.HasValue || !(this.m_excavator.CurrentJob.Value is MiningJob miningJob))
        return;
      TerrainTile? tileToMine = miningJob.TileToMine;
      if (!tileToMine.HasValue)
        return;
      Gizmos.color = Color.yellow;
      tileToMine = miningJob.TileToMine;
      Gizmos.DrawSphere(tileToMine.Value.CenterTile3f.ToVector3(), 0.5f);
    }

    public override void Destroy() => base.Destroy();

    public ExcavatorMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
