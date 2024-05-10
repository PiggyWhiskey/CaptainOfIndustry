// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.TreePlanterMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities.Dynamic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class TreePlanterMb : PathFindingEntityMb
  {
    private static readonly Duration LIGHTS_OFF_DELAY;
    private TreePlanter m_planter;
    private Animator m_animator;
    private TracksAnimator m_tracksAnimator;
    private Transform m_cabinTransform;
    private float m_currentCabinAngle;
    private float m_futureCabinAngle;
    private TreePlanterState m_planterState;
    private bool m_isNewState;
    private VehicleLightsController m_lightsController;
    private ImmutableArray<GameObject> m_trees;
    private int m_lastSeenQuantity;

    internal void Initialize(
      TreePlanter planter,
      AssetsDb assetsDb,
      IWeatherManager weatherManager,
      RandomProvider randomProvider,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      this.m_planter = planter;
      this.m_animator = this.gameObject.GetComponentOrCreateNewAndLogError<Animator>();
      TreePlanterProto.Gfx graphics = planter.Prototype.Graphics;
      ImmutableArrayBuilder<GameObject> immutableArrayBuilder = new ImmutableArrayBuilder<GameObject>(graphics.NumTrees);
      for (int index = 1; index <= graphics.NumTrees; ++index)
      {
        Transform resultTransform;
        if (this.transform.TryFindChild(graphics.TreesBaseObjectPath + index.ToString(), out resultTransform))
          immutableArrayBuilder[index - 1] = resultTransform.gameObject;
        else
          Log.Warning("Failed to find tree expected on planter " + graphics.TreesBaseObjectPath + index.ToString());
      }
      this.m_trees = immutableArrayBuilder.GetImmutableArrayAndClear();
      if (!this.transform.TryFindChild(graphics.CabinObjectPath, out this.m_cabinTransform))
      {
        Log.Error(string.Format("Failed to find cabin '{0}' of '{1}'.", (object) graphics.CabinObjectPath, (object) planter.Prototype.Id));
        this.m_cabinTransform = this.transform;
      }
      this.m_currentCabinAngle = this.m_futureCabinAngle = this.m_planter.CabinDirectionRelative.ToUnityAngleDegrees().Modulo(360f);
      this.m_cabinTransform.localRotation = Quaternion.AngleAxis(this.m_futureCabinAngle, Vector3.up);
      this.m_tracksAnimator = new TracksAnimator(this.transform, graphics.LeftTrackObjectPath, graphics.RightTrackObjectPath, graphics.SpacingBetweenTracks.ToUnityUnits(), graphics.TrackTextureLength.ToUnityUnits());
      this.m_lightsController = new VehicleLightsController(weatherManager, randomProvider.GetNonSimRandomFor((object) this, planter.Id.ToString()), (DynamicGroundEntity) planter, this.gameObject, new int?(Shader.PropertyToID("_EmissionStrength")));
      this.m_lastSeenQuantity = -1;
      this.Initialize((PathFindingEntity) planter, assetsDb, audioManager, dependencies);
    }

    public override void SyncUpdate(GameTime time)
    {
      base.SyncUpdate(time);
      this.m_currentCabinAngle = this.m_futureCabinAngle;
      this.m_futureCabinAngle = this.m_planter.CabinDirectionRelative.ToUnityAngleDegrees();
      this.m_isNewState = this.m_planterState != this.m_planter.State;
      if (this.m_isNewState)
        this.m_planterState = this.m_planter.State;
      if (this.m_lastSeenQuantity != this.m_planter.Cargo.TotalQuantity.Value)
      {
        float num = (float) this.m_planter.Cargo.TotalQuantity.Value / (float) this.m_planter.Capacity.Value;
        for (int index = 0; index < this.m_planter.Prototype.Graphics.NumTrees; ++index)
        {
          if (this.m_trees != (ImmutableArray<GameObject>) (ImmutableArray.EmptyArray) null)
            this.m_trees[index].SetActive((double) index < (double) num * (double) this.m_planter.Prototype.Graphics.NumTrees);
          this.m_lastSeenQuantity = this.m_planter.Cargo.TotalQuantity.Value;
        }
      }
      this.m_animator.speed = this.m_planter.ArmStateChangeSpeedFactor.ToFloat() * (float) time.GameSpeedMult;
      this.m_tracksAnimator.Sync(time, this.m_planter.Speed, -this.m_planter.SteeringAngle);
      this.m_lightsController.UpdateVehicleLights();
    }

    public override void RenderUpdate(GameTime time)
    {
      base.RenderUpdate(time);
      float num = (this.m_futureCabinAngle - this.m_currentCabinAngle).Modulo(360f);
      if ((double) num > 180.0)
        num -= 360f;
      Assert.That<float>(num).IsWithinIncl(-180f, 180f);
      this.m_cabinTransform.localRotation = Quaternion.AngleAxis(this.m_currentCabinAngle + num * time.AbsoluteT, Vector3.up);
      if (this.m_isNewState)
      {
        this.m_isNewState = false;
        this.startAnimationFor(this.m_planterState);
      }
      this.m_tracksAnimator.RenderUpdate(time);
    }

    private void startAnimationFor(TreePlanterState state)
    {
      switch (state)
      {
        case TreePlanterState.Idle:
          break;
        case TreePlanterState.PlantingTree:
          this.m_animator.Play(this.m_planter.Prototype.Graphics.PlantingAnimStateName, 0, 0.0f);
          break;
        case TreePlanterState.ReturningToIdle:
          break;
        default:
          Assert.Fail(string.Format("Unknown tree planter animation start state: '{0}'.", (object) state));
          break;
      }
    }

    public TreePlanterMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TreePlanterMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TreePlanterMb.LIGHTS_OFF_DELAY = 10.Seconds();
    }
  }
}
