// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.TreeHarvesterMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Dynamic;
using Mafi.Unity.Terrain;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class TreeHarvesterMb : PathFindingEntityMb
  {
    private static readonly Quaternion s_gripperToNeutral;
    private static readonly Vector3 s_localX;
    private static readonly Vector3 s_localZ;
    private static readonly int COLOR_SHADER_ID;
    private static readonly Duration LIGHTS_OFF_DELAY;
    private TreeHarvester m_harvester;
    private TreeRenderer m_treeRenderer;
    private ObjectHighlighter m_objectHighlighter;
    private AssetsDb m_assetsDb;
    private Animator m_animator;
    private TracksAnimator m_tracksAnimator;
    private Transform m_treeParentTransform;
    private Transform m_cabinTransform;
    private float m_currentCabinAngle;
    private float m_futureCabinAngle;
    private Transform m_treeGripperTransform;
    private Quaternion m_treeGripperOriginalRotation;
    private Quaternion m_treeGripperOldRotation;
    private Quaternion m_treeGripperTargetRotation;
    private TreeHarvesterState m_harvesterState;
    private bool m_isNewState;
    private TreeHarvesterState m_prevHarvesterState;
    private Option<GameObject> m_currentTreeGo;
    private Option<TreeMb> m_currentTreeMb;
    private TreeData? m_currentTree;
    private float m_previousStatePerc;
    private float m_currentStatePerc;
    private int m_prevNumCuts;
    private Vector3 m_treeOriginalPosition;
    private Vector3 m_treeOriginalRotationEuler;
    private Option<GameObject> m_trimmedTreeGo;
    private float m_trimmedGoOriginalY;
    private float m_trimmedGoTargetY;
    private float m_trimmedGoOriginalLength;
    private int m_numSectionsPerCut;
    private int m_numSectionsToHide;
    private readonly List<MeshRenderer> m_currentTreeRenderers;
    private readonly Lyst<KeyValuePair<TreeProto, GameObject>> m_trimmedTreesGos;
    private VehicleLightsController m_lightsController;
    private Option<EntitySoundMb> m_cuttingSound;
    private Option<EntitySoundMb> m_trimmingSound;

    internal void Initialize(
      TreeHarvester harvester,
      TreeRenderer treeRenderer,
      ObjectHighlighter objectHighlighter,
      AssetsDb assetsDb,
      IWeatherManager weatherManager,
      RandomProvider randomProvider,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      this.m_treeRenderer = treeRenderer;
      this.m_objectHighlighter = objectHighlighter;
      this.m_assetsDb = assetsDb;
      this.m_harvester = harvester;
      this.m_animator = this.gameObject.GetComponentOrCreateNewAndLogError<Animator>();
      TreeHarvesterProto.Gfx graphics = harvester.Prototype.Graphics;
      if (!this.transform.TryFindChild(graphics.CabinObjectPath, out this.m_cabinTransform))
      {
        Log.Error(string.Format("Failed to find cabin '{0}' of '{1}'.", (object) graphics.CabinObjectPath, (object) harvester.Prototype.Id));
        this.m_cabinTransform = this.transform;
      }
      this.m_currentCabinAngle = this.m_futureCabinAngle = this.m_harvester.CabinDirectionRelative.ToUnityAngleDegrees().Modulo(360f);
      this.m_cabinTransform.localRotation = Quaternion.AngleAxis(this.m_futureCabinAngle, Vector3.up);
      if (!this.gameObject.transform.TryFindChild(graphics.HarvestedTreeParentObjectPath, out this.m_treeParentTransform))
      {
        Log.Error("Failed to find harvested tree parent '" + graphics.HarvestedTreeParentObjectPath + "' " + string.Format("of '{0}'.", (object) harvester.Prototype.Id));
        this.m_treeParentTransform = this.transform;
      }
      if (!this.gameObject.transform.TryFindChild(graphics.RotatingHandObjectPath, out this.m_treeGripperTransform))
      {
        Log.Error("Failed to find gripper parent '" + graphics.RotatingHandObjectPath + "' " + string.Format("of '{0}'.", (object) harvester.Prototype.Id));
        this.m_treeGripperTransform = this.transform;
      }
      this.m_treeGripperOriginalRotation = this.m_treeGripperTransform.localRotation;
      this.m_treeGripperOldRotation = this.m_treeGripperOriginalRotation;
      this.m_treeGripperTargetRotation = this.m_treeGripperOriginalRotation;
      this.m_tracksAnimator = new TracksAnimator(this.transform, graphics.LeftTrackObjectPath, graphics.RightTrackObjectPath, graphics.SpacingBetweenTracks.ToUnityUnits(), graphics.TrackTextureLength.ToUnityUnits());
      this.m_lightsController = new VehicleLightsController(weatherManager, randomProvider.GetNonSimRandomFor((object) this, harvester.Id.ToString()), (DynamicGroundEntity) harvester, this.gameObject, new int?(Shader.PropertyToID("_EmissionStrength")));
      this.m_cuttingSound = audioManager.CreateSound((EntityMb) this, new SoundParams("Assets/Base/Vehicles/TreeHarvester/CuttingSound.prefab", SoundSignificance.Normal, doNotLimit: true));
      this.m_trimmingSound = audioManager.CreateSound((EntityMb) this, new SoundParams("Assets/Base/Vehicles/TreeHarvester/TrimmingSound.prefab", SoundSignificance.Normal, doNotLimit: true));
      this.reinitializeAnimations(this.m_harvester.State);
      this.startAnimationFor(this.m_harvester.State);
      this.updateTreeAnimations(this.m_harvester.State, 1f, 1f);
      this.m_harvesterState = this.m_harvester.State;
      this.Initialize((PathFindingEntity) harvester, assetsDb, audioManager, dependencies);
    }

    private void reinitializeAnimations(TreeHarvesterState state)
    {
      switch (state)
      {
        case TreeHarvesterState.Idle:
          break;
        case TreeHarvesterState.PositioningArm:
          break;
        case TreeHarvesterState.CuttingTree:
          this.startAnimationFor(TreeHarvesterState.PositioningArm);
          this.updateTreeAnimations(TreeHarvesterState.PositioningArm, 1f, 1f);
          break;
        case TreeHarvesterState.LayingTreeDown:
          break;
        case TreeHarvesterState.BranchTrimming:
          this.startAnimationFor(TreeHarvesterState.LayingTreeDown);
          this.updateTreeAnimations(TreeHarvesterState.LayingTreeDown, 1f, 1f);
          break;
        case TreeHarvesterState.RaisingTreeUp:
          break;
        case TreeHarvesterState.TreeIsUp:
        case TreeHarvesterState.PositioningForUnload:
          this.startAnimationFor(TreeHarvesterState.RaisingTreeUp);
          this.updateTreeAnimations(TreeHarvesterState.RaisingTreeUp, 1f, 1f);
          break;
        case TreeHarvesterState.UnloadingTree:
          this.startAnimationFor(TreeHarvesterState.CuttingSection);
          this.updateTreeAnimations(TreeHarvesterState.CuttingSection, 1f, 1f);
          break;
        case TreeHarvesterState.ReturningFromUnloadWithCargo:
          break;
        case TreeHarvesterState.ReturningFromUnloadToIdle:
          break;
        case TreeHarvesterState.FoldingArm:
          break;
        case TreeHarvesterState.CuttingSection:
          break;
        default:
          Assert.Fail(string.Format("Unknown tree harvester animation state: '{0}'.", (object) this.m_harvester.State));
          break;
      }
    }

    private bool isSupportedTransition(TreeHarvesterState oldState, TreeHarvesterState newState)
    {
      switch (newState)
      {
        case TreeHarvesterState.Idle:
          return oldState == TreeHarvesterState.FoldingArm;
        case TreeHarvesterState.PositioningArm:
          return oldState == TreeHarvesterState.Idle || oldState == TreeHarvesterState.ReturningFromUnloadToIdle;
        case TreeHarvesterState.CuttingTree:
          return oldState == TreeHarvesterState.PositioningArm;
        case TreeHarvesterState.LayingTreeDown:
          return oldState == TreeHarvesterState.CuttingTree;
        case TreeHarvesterState.BranchTrimming:
          return oldState == TreeHarvesterState.LayingTreeDown;
        case TreeHarvesterState.RaisingTreeUp:
          return oldState == TreeHarvesterState.BranchTrimming;
        case TreeHarvesterState.TreeIsUp:
          return oldState == TreeHarvesterState.RaisingTreeUp || oldState == TreeHarvesterState.ReturningFromUnloadWithCargo;
        case TreeHarvesterState.PositioningForUnload:
          bool flag1;
          switch (oldState)
          {
            case TreeHarvesterState.RaisingTreeUp:
            case TreeHarvesterState.TreeIsUp:
            case TreeHarvesterState.ReturningFromUnloadWithCargo:
              flag1 = true;
              break;
            default:
              flag1 = false;
              break;
          }
          return flag1;
        case TreeHarvesterState.UnloadingTree:
          return oldState == TreeHarvesterState.CuttingSection;
        case TreeHarvesterState.ReturningFromUnloadWithCargo:
          return true;
        case TreeHarvesterState.ReturningFromUnloadToIdle:
          return oldState == TreeHarvesterState.UnloadingTree;
        case TreeHarvesterState.FoldingArm:
          return oldState == TreeHarvesterState.ReturningFromUnloadToIdle;
        case TreeHarvesterState.CuttingSection:
          bool flag2;
          switch (oldState)
          {
            case TreeHarvesterState.PositioningForUnload:
            case TreeHarvesterState.UnloadingTree:
              flag2 = true;
              break;
            default:
              flag2 = false;
              break;
          }
          return flag2;
        default:
          Assert.Fail(string.Format("Unknown tree harvester animation state: '{0}'.", (object) oldState));
          return false;
      }
    }

    public override void SyncUpdate(GameTime time)
    {
      base.SyncUpdate(time);
      this.m_currentTree = this.m_harvester.TreeToBeCut;
      this.m_currentCabinAngle = this.m_futureCabinAngle;
      this.m_futureCabinAngle = this.m_harvester.CabinDirectionRelative.ToUnityAngleDegrees();
      this.m_isNewState = this.m_harvesterState != this.m_harvester.State || this.m_prevNumCuts != this.m_harvester.NumCutsMade;
      if (this.m_isNewState)
      {
        this.m_prevHarvesterState = this.m_harvesterState;
        this.m_harvesterState = this.m_harvester.State;
        if (this.m_prevHarvesterState != this.m_harvesterState && !this.isSupportedTransition(this.m_prevHarvesterState, this.m_harvesterState))
        {
          Log.Warning(string.Format("Unsupported tree harvester transition from '{0}' to '{1}'", (object) this.m_prevHarvesterState, (object) this.m_harvesterState));
          this.reinitializeAnimations(this.m_harvesterState);
        }
        this.m_previousStatePerc = 0.0f;
        this.m_prevNumCuts = this.m_harvester.NumCutsMade;
      }
      else
        this.m_previousStatePerc = this.m_currentStatePerc;
      this.m_currentStatePerc = this.m_harvester.CurrentStateDuration.Ticks <= 0 ? 1f : (float) (1.0 - (double) this.m_harvester.CurrentStateRemaining.Ticks / (double) this.m_harvester.CurrentStateDuration.Ticks);
      this.m_animator.speed = this.m_harvester.ArmStateChangeSpeedFactor.ToFloat() * (float) time.GameSpeedMult;
      this.m_tracksAnimator.Sync(time, this.m_harvester.Speed, -this.m_harvester.SteeringAngle);
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
        this.startAnimationFor(this.m_harvesterState);
      }
      this.updateTreeAnimations(this.m_harvesterState, this.m_previousStatePerc.Lerp(this.m_currentStatePerc, time.AbsoluteT), time.AbsoluteT);
      this.m_tracksAnimator.RenderUpdate(time);
      if (this.m_cuttingSound.HasValue)
      {
        this.m_cuttingSound.Value.UpdatePlay(this.m_harvesterState == TreeHarvesterState.CuttingTree);
        this.m_cuttingSound.Value.RenderUpdate(time);
      }
      if (!this.m_trimmingSound.HasValue)
        return;
      this.m_trimmingSound.Value.UpdatePlay(this.m_harvesterState == TreeHarvesterState.BranchTrimming);
      this.m_trimmingSound.Value.RenderUpdate(time);
    }

    private Quaternion getGripperVerticalRotation() => this.m_treeGripperOriginalRotation;

    /// <summary>
    /// If we don't have a trimmed tree but we should, make one. This is useful after loading.
    /// </summary>
    private void recoverTrimmedTreeGo()
    {
      TreeProto proto;
      if (this.m_currentTree.HasValue)
        proto = this.m_currentTree.Value.Proto;
      else if (this.m_harvester.LastCutTreeProto.HasValue)
      {
        proto = this.m_harvester.LastCutTreeProto.Value;
      }
      else
      {
        Log.Error("Attempting to recover trimmed tree GO with no data.");
        return;
      }
      GameObject trimmedTreeFor = this.getTrimmedTreeFor(proto);
      if (trimmedTreeFor.transform.childCount < TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE)
      {
        Log.Error("Trimmed tree GO doesn't have enough children.");
      }
      else
      {
        float num = 1f;
        if (this.m_currentTreeGo.HasValue)
        {
          Vector3 localScale = this.m_currentTreeGo.Value.transform.localScale;
          trimmedTreeFor.transform.localScale = localScale;
          num = localScale.y;
        }
        trimmedTreeFor.SetActive(true);
        this.m_objectHighlighter.SetHasChanged(this.gameObject);
        this.m_trimmedTreeGo = (Option<GameObject>) trimmedTreeFor;
        if (this.m_harvester.NumSectionsToMake <= 0)
        {
          Assert.Fail(string.Format("Num sections to make must be greater than zero'{0}'", (object) this.m_harvester.NumSectionsToMake));
          this.m_numSectionsPerCut = 1;
          this.m_numSectionsToHide = 0;
        }
        else
        {
          for (int sectionsAtMaxTreeSize = TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE; sectionsAtMaxTreeSize >= 1; --sectionsAtMaxTreeSize)
          {
            if (sectionsAtMaxTreeSize % this.m_harvester.NumSectionsToMake == 0)
            {
              this.m_numSectionsPerCut = sectionsAtMaxTreeSize / this.m_harvester.NumSectionsToMake;
              this.m_numSectionsToHide = TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE - sectionsAtMaxTreeSize;
              break;
            }
          }
          this.m_trimmedGoOriginalLength = num * -proto.TreeGraphics.TrimmedTreeLength.ToUnityUnits() * (float) (TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE - this.m_numSectionsToHide) / (float) TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE;
          this.m_trimmedGoTargetY = (float) (-(double) this.m_harvester.Prototype.Graphics.TreeHolderOffset - (double) this.m_harvester.Prototype.Graphics.GripperWidth / 2.0 - (double) (this.m_numSectionsToHide / 2) * (double) this.m_trimmedTreeGo.Value.transform.localScale.x + (double) this.m_trimmedGoOriginalLength * (double) (this.m_harvester.NumCutsMade + 1) / (double) this.m_harvester.NumSectionsToMake);
          this.m_trimmedGoOriginalY = this.m_trimmedGoTargetY;
          this.m_trimmedTreeGo.Value.transform.localPosition = new Vector3(0.0f, this.m_trimmedGoTargetY, 0.0f);
          for (int index = 0; index < this.m_numSectionsToHide; index += 2)
          {
            trimmedTreeFor.transform.GetChild(index / 2).gameObject.SetActive(false);
            if (index + 1 < this.m_numSectionsToHide)
              trimmedTreeFor.transform.GetChild(TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE - 1 - index / 2).gameObject.SetActive(false);
          }
        }
      }
    }

    private void startAnimationFor(TreeHarvesterState state)
    {
      switch (state)
      {
        case TreeHarvesterState.Idle:
          if (this.m_currentTreeGo.HasValue)
          {
            this.m_currentTreeGo.Value.gameObject.Destroy();
            this.m_currentTreeGo = (Option<GameObject>) Option.None;
          }
          if (this.m_trimmedTreeGo.HasValue)
          {
            this.m_trimmedTreeGo.Value.gameObject.SetActive(false);
            this.m_objectHighlighter.SetHasChanged(this.gameObject);
            this.m_trimmedTreeGo = Option<GameObject>.None;
          }
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.IdleAnimStateName);
          break;
        case TreeHarvesterState.PositioningArm:
          if (this.m_currentTreeGo.HasValue)
          {
            this.m_currentTreeGo.Value.gameObject.Destroy();
            this.m_currentTreeGo = (Option<GameObject>) Option.None;
          }
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.PreparedForHarvestAnimStateName);
          this.m_treeGripperTargetRotation = this.getGripperVerticalRotation();
          this.m_currentTreeMb = this.m_currentTree.HasValue ? this.m_treeRenderer.GetMbOrNoneFor(this.m_currentTree.Value.Id) : Option<TreeMb>.None;
          break;
        case TreeHarvesterState.CuttingTree:
          Assert.That<Option<GameObject>>(this.m_currentTreeGo).IsNone<GameObject>();
          this.m_treeGripperTransform.localRotation = this.m_treeGripperTargetRotation;
          if (this.m_currentTreeMb.HasValue)
          {
            if (!this.m_currentTreeGo.IsNone)
              break;
            TreeMb treeMb = this.m_currentTreeMb.Value;
            if (!(bool) (Object) treeMb.gameObject)
            {
              Assert.Fail("TreeMb has no gameobject.");
              break;
            }
            if (!(bool) (Object) treeMb.gameObject.transform)
            {
              Assert.Fail("TreeMb's game object '" + treeMb.gameObject.name + "' has no transform.");
              break;
            }
            if (treeMb.gameObject.transform.childCount == 0)
            {
              Assert.Fail("TreeMb's game object's transform has no children.");
              break;
            }
            Transform child = treeMb.gameObject.transform.GetChild(0);
            if (!(bool) (Object) child)
            {
              Assert.Fail("TreeMb's GO has no children.");
              break;
            }
            MeshRenderer component = child.gameObject.GetComponent<MeshRenderer>();
            if (!(bool) (Object) component)
            {
              Assert.Fail("TreeMb's GO's first child has no mesh renderer.");
              break;
            }
            Material material = component.material;
            GameObject clonedPrefabOrEmptyGo = this.m_assetsDb.GetClonedPrefabOrEmptyGo(treeMb.CutTreePrefabPath);
            Transform transform = clonedPrefabOrEmptyGo.transform.Find("main");
            if (!(bool) (Object) transform)
            {
              Assert.Fail("Cut tree prefab '" + treeMb.CutTreePrefabPath + "' is missing 'main' child.");
              break;
            }
            GameObject gameObject = Object.Instantiate<GameObject>(transform.gameObject);
            clonedPrefabOrEmptyGo.Destroy();
            gameObject.transform.localPosition = treeMb.transform.localPosition;
            gameObject.transform.localScale = treeMb.transform.localScale;
            gameObject.transform.localRotation = treeMb.transform.localRotation;
            treeMb.BeingHarvested();
            treeMb.gameObject.SetActive(false);
            this.m_objectHighlighter.SetHasChanged(this.gameObject);
            this.m_currentTreeMb = (Option<TreeMb>) Option.None;
            gameObject.transform.SetParent(this.m_treeParentTransform, true);
            this.m_treeOriginalPosition = gameObject.transform.localPosition;
            this.m_treeOriginalRotationEuler = gameObject.transform.localRotation.eulerAngles;
            this.m_currentTreeRenderers.Clear();
            gameObject.gameObject.GetComponentsInChildren<MeshRenderer>(this.m_currentTreeRenderers);
            Assert.That<List<MeshRenderer>>(this.m_currentTreeRenderers).IsNotEmpty<MeshRenderer>();
            foreach (Renderer currentTreeRenderer in this.m_currentTreeRenderers)
              currentTreeRenderer.sharedMaterial = Object.Instantiate<Material>(material);
            this.m_currentTreeGo = (Option<GameObject>) gameObject;
            break;
          }
          Log.Warning("No tree MB found.");
          break;
        case TreeHarvesterState.LayingTreeDown:
          if (this.m_currentTreeGo.HasValue)
            this.m_currentTreeGo.Value.transform.localPosition = Vector3.zero;
          this.m_treeGripperTargetRotation = this.m_treeGripperOriginalRotation;
          this.m_treeGripperOldRotation = this.m_treeGripperTransform.localRotation;
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.TreeLayingDownAnimStateName);
          break;
        case TreeHarvesterState.BranchTrimming:
          this.m_treeGripperTransform.localRotation = this.m_treeGripperOriginalRotation;
          GameObject trimmedTreeFor;
          if (this.m_currentTree.HasValue)
            trimmedTreeFor = this.getTrimmedTreeFor(this.m_currentTree.Value.Proto);
          else if (this.m_harvester.LastCutTreeProto.HasValue)
          {
            trimmedTreeFor = this.getTrimmedTreeFor(this.m_harvester.LastCutTreeProto.Value);
          }
          else
          {
            Log.Error("Branch trimming but no current or last cut tree.");
            trimmedTreeFor = this.getTrimmedTreeFor((TreeProto) null);
          }
          trimmedTreeFor.transform.localPosition = Vector3.zero;
          float num1 = 1f;
          if (this.m_currentTreeGo.HasValue)
          {
            Vector3 localScale = this.m_currentTreeGo.Value.transform.localScale;
            trimmedTreeFor.transform.localScale = localScale;
            trimmedTreeFor.transform.localRotation = Quaternion.Euler(0.0f, this.m_currentTreeGo.Value.transform.rotation.eulerAngles.y, 0.0f);
            num1 = localScale.y;
          }
          trimmedTreeFor.SetActive(true);
          this.m_objectHighlighter.SetHasChanged(this.gameObject);
          this.m_trimmedTreeGo = (Option<GameObject>) trimmedTreeFor;
          this.m_trimmedGoOriginalY = 0.0f;
          if (this.m_harvester.NumSectionsToMake <= 0)
          {
            Assert.Fail(string.Format("Num sections to make must be greater than zero'{0}'", (object) this.m_harvester.NumSectionsToMake));
            this.m_numSectionsPerCut = 1;
            this.m_numSectionsToHide = 0;
            break;
          }
          for (int sectionsAtMaxTreeSize = TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE; sectionsAtMaxTreeSize >= 1; --sectionsAtMaxTreeSize)
          {
            if (sectionsAtMaxTreeSize % this.m_harvester.NumSectionsToMake == 0)
            {
              this.m_numSectionsPerCut = sectionsAtMaxTreeSize / this.m_harvester.NumSectionsToMake;
              this.m_numSectionsToHide = TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE - sectionsAtMaxTreeSize;
              break;
            }
          }
          double num2 = (double) num1;
          ref TreeData? local = ref this.m_currentTree;
          double num3 = local.HasValue ? -(double) local.GetValueOrDefault().Proto.TreeGraphics.TrimmedTreeLength.ToUnityUnits() : 0.0;
          this.m_trimmedGoOriginalLength = (float) (num2 * num3) * (float) (TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE - this.m_numSectionsToHide) / (float) TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE;
          this.m_trimmedGoTargetY = (float) ((double) this.m_trimmedGoOriginalLength - (double) this.m_harvester.Prototype.Graphics.TreeHolderOffset + (double) this.m_harvester.Prototype.Graphics.GripperWidth / 2.0);
          if (trimmedTreeFor.transform.childCount < TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE)
            break;
          for (int index = 0; index < this.m_numSectionsToHide; index += 2)
          {
            trimmedTreeFor.transform.GetChild(index / 2).gameObject.SetActive(false);
            if (index + 1 < this.m_numSectionsToHide)
              trimmedTreeFor.transform.GetChild(TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE - 1 - index / 2).gameObject.SetActive(false);
          }
          break;
        case TreeHarvesterState.RaisingTreeUp:
          if (this.m_trimmedTreeGo.IsNone)
            this.recoverTrimmedTreeGo();
          if (this.m_trimmedTreeGo.HasValue)
          {
            Transform transform = this.m_trimmedTreeGo.Value.transform;
            Vector3 localPosition = transform.localPosition with
            {
              y = this.m_trimmedGoTargetY
            };
            transform.localPosition = localPosition;
          }
          if (this.m_currentTreeGo.HasValue)
          {
            this.m_currentTreeGo.Value.gameObject.Destroy();
            this.m_currentTreeGo = Option<GameObject>.None;
          }
          this.m_trimmedGoOriginalY = this.m_trimmedGoTargetY;
          this.m_trimmedGoTargetY = (float) ((double) this.m_trimmedGoOriginalY / 2.0 + 1.0);
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.TreeAboveTruckAnimStateName);
          break;
        case TreeHarvesterState.TreeIsUp:
          if (this.m_trimmedTreeGo.IsNone)
            this.recoverTrimmedTreeGo();
          if (!this.m_trimmedTreeGo.HasValue || (double) this.m_trimmedGoTargetY <= 0.0)
            break;
          Transform transform1 = this.m_trimmedTreeGo.Value.transform;
          Vector3 localPosition1 = transform1.localPosition with
          {
            y = this.m_trimmedGoTargetY
          };
          transform1.localPosition = localPosition1;
          break;
        case TreeHarvesterState.PositioningForUnload:
          break;
        case TreeHarvesterState.UnloadingTree:
          break;
        case TreeHarvesterState.ReturningFromUnloadWithCargo:
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.TreeFromTruckAnimStateName);
          break;
        case TreeHarvesterState.ReturningFromUnloadToIdle:
          if (this.m_trimmedTreeGo.HasValue)
          {
            this.m_trimmedTreeGo.Value.gameObject.SetActive(false);
            this.m_objectHighlighter.SetHasChanged(this.gameObject);
            this.m_trimmedTreeGo = Option<GameObject>.None;
          }
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.TreeFromTruckAnimStateName);
          break;
        case TreeHarvesterState.FoldingArm:
          if (this.m_trimmedTreeGo.HasValue)
          {
            this.m_trimmedTreeGo.Value.gameObject.SetActive(false);
            this.m_objectHighlighter.SetHasChanged(this.gameObject);
            this.m_trimmedTreeGo = Option<GameObject>.None;
          }
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.FoldedAnimStateName);
          break;
        case TreeHarvesterState.CuttingSection:
          if (this.m_trimmedTreeGo.IsNone)
            this.recoverTrimmedTreeGo();
          if (this.m_trimmedTreeGo.HasValue)
          {
            this.m_objectHighlighter.SetHasChanged(this.gameObject);
            if (this.m_trimmedTreeGo.Value.transform.childCount >= TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE)
            {
              for (int index = 0; index < this.m_prevNumCuts * this.m_numSectionsPerCut + Mathf.CeilToInt((float) this.m_numSectionsToHide / 2f); ++index)
                this.m_trimmedTreeGo.Value.transform.GetChild(index).gameObject.SetActive(false);
            }
            this.m_trimmedGoOriginalY = this.m_trimmedTreeGo.Value.transform.localPosition.y;
            this.m_trimmedGoTargetY = this.m_harvester.NumCutsMade < this.m_harvester.NumSectionsToMake - 1 ? (float) (-(double) this.m_harvester.Prototype.Graphics.TreeHolderOffset - (double) this.m_harvester.Prototype.Graphics.GripperWidth / 2.0 - (double) (this.m_numSectionsToHide / 2) * (double) this.m_trimmedTreeGo.Value.transform.localScale.x + (double) this.m_trimmedGoOriginalLength * (double) (this.m_harvester.NumCutsMade + 1) / (double) this.m_harvester.NumSectionsToMake) : (float) (-(double) this.m_harvester.Prototype.Graphics.TreeHolderOffset - (double) this.m_harvester.Prototype.Graphics.GripperWidth / 2.0 - (double) (this.m_numSectionsToHide / 2) * (double) this.m_trimmedTreeGo.Value.transform.localScale.x + (double) this.m_trimmedGoOriginalLength * (double) this.m_harvester.NumCutsMade / (double) this.m_harvester.NumSectionsToMake);
          }
          this.m_animator.Play(this.m_harvester.Prototype.Graphics.TreeOnTruckAnimStateName);
          break;
        default:
          Assert.Fail(string.Format("Unknown tree harvester animation start state: '{0}'.", (object) state));
          break;
      }
    }

    private GameObject getTrimmedTreeFor(TreeProto treeProto)
    {
      GameObject trimmedTreeFor;
      if (!this.m_trimmedTreesGos.TryGetValue<TreeProto, GameObject>(treeProto, out trimmedTreeFor))
      {
        if ((Proto) treeProto == (Proto) null)
          trimmedTreeFor = new GameObject("Trimmed tree null");
        else if (treeProto.TreeGraphics.TrimmedTreePrefabPath.IsNone)
        {
          Log.Error(string.Format("No trimmed tree prefab for {0}.", (object) treeProto));
          trimmedTreeFor = new GameObject("Trimmed tree " + treeProto.Id.Value);
        }
        else
          trimmedTreeFor = this.m_assetsDb.GetClonedPrefabOrEmptyGo(treeProto.TreeGraphics.TrimmedTreePrefabPath.Value);
        this.m_trimmedTreesGos.Add<TreeProto, GameObject>(treeProto, trimmedTreeFor);
        trimmedTreeFor.transform.SetParent(this.m_treeParentTransform, false);
      }
      trimmedTreeFor.SetActive(true);
      this.m_objectHighlighter.SetHasChanged(this.gameObject);
      for (int index = 0; index < trimmedTreeFor.transform.childCount; ++index)
      {
        Transform child = trimmedTreeFor.transform.GetChild(index);
        child.transform.localPosition = new Vector3(0.0f, child.transform.localPosition.y, child.transform.localPosition.z);
        child.gameObject.SetActive(true);
        MeshRenderer component = child.GetComponent<MeshRenderer>();
        if ((Object) component == (Object) null)
        {
          Log.Error("Trimmed tree go has no mesh renderer.");
        }
        else
        {
          component.material.SetOverrideTag("RenderType", "Opaque");
          component.material.SetInt("_SrcBlend", 1);
          component.material.SetInt("_DstBlend", 0);
          component.material.SetInt("_ZWrite", 1);
          component.material.DisableKeyword("_ALPHATEST_ON");
          component.material.DisableKeyword("_ALPHABLEND_ON");
          component.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          component.material.renderQueue = 2000;
          component.material.SetColor(TreeHarvesterMb.COLOR_SHADER_ID, new Color(1f, 1f, 1f, 1f));
        }
      }
      Assert.That<bool>(trimmedTreeFor.activeSelf).IsTrue();
      return trimmedTreeFor;
    }

    private void updateTreeAnimations(
      TreeHarvesterState state,
      float animationPercent,
      float AbsoluteT)
    {
      switch (state)
      {
        case TreeHarvesterState.Idle:
          break;
        case TreeHarvesterState.PositioningArm:
          this.m_treeGripperTransform.localRotation = Quaternion.SlerpUnclamped(this.m_treeGripperOriginalRotation, this.m_treeGripperTargetRotation, animationPercent);
          break;
        case TreeHarvesterState.CuttingTree:
          if (!this.m_currentTreeGo.HasValue)
            break;
          this.m_currentTreeGo.Value.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(this.m_treeOriginalRotationEuler), Quaternion.Euler(0.0f, this.m_treeOriginalRotationEuler.y, 0.0f), animationPercent);
          this.m_currentTreeGo.Value.transform.localPosition = Vector3.Lerp(this.m_treeOriginalPosition, Vector3.zero, animationPercent);
          break;
        case TreeHarvesterState.LayingTreeDown:
          if (this.m_currentTreeGo.HasValue)
          {
            this.m_currentTreeGo.Value.transform.localPosition = Vector3.zero;
            this.m_currentTreeGo.Value.transform.localRotation = Quaternion.Euler(0.0f, this.m_treeOriginalRotationEuler.y, 0.0f);
          }
          this.m_treeGripperTransform.localRotation = Quaternion.SlerpUnclamped(this.m_treeGripperOriginalRotation, this.m_treeGripperTargetRotation, animationPercent);
          break;
        case TreeHarvesterState.BranchTrimming:
        case TreeHarvesterState.CuttingSection:
          this.m_treeGripperTransform.localRotation = this.m_treeGripperTargetRotation;
          if (this.m_currentTreeGo.HasValue)
          {
            this.m_currentTreeGo.Value.transform.localPosition = Vector3.zero;
            this.m_currentTreeGo.Value.transform.localRotation = Quaternion.Euler(0.0f, this.m_treeOriginalRotationEuler.y, 0.0f);
          }
          float num1 = this.m_trimmedGoOriginalY.Lerp(this.m_trimmedGoTargetY, MafiMath.EaseInOut(animationPercent));
          if (this.m_trimmedTreeGo.HasValue)
          {
            Transform transform = this.m_trimmedTreeGo.Value.transform;
            Vector3 localPosition = transform.localPosition with
            {
              y = num1
            };
            transform.localPosition = localPosition;
          }
          if (!this.m_currentTreeGo.HasValue)
            break;
          Transform transform1 = this.m_currentTreeGo.Value.transform;
          Vector3 localPosition1 = transform1.localPosition with
          {
            y = num1
          };
          transform1.localPosition = localPosition1;
          using (List<MeshRenderer>.Enumerator enumerator = this.m_currentTreeRenderers.GetEnumerator())
          {
            while (enumerator.MoveNext())
              enumerator.Current.sharedMaterial.SetColor(TreeHarvesterMb.COLOR_SHADER_ID, new Color(1f, 1f, 1f, 1f - animationPercent));
            break;
          }
        case TreeHarvesterState.RaisingTreeUp:
          float num2 = this.m_trimmedGoOriginalY.Lerp(this.m_trimmedGoTargetY, MafiMath.EaseInOut(animationPercent));
          if (!this.m_trimmedTreeGo.HasValue)
            break;
          Transform transform2 = this.m_trimmedTreeGo.Value.transform;
          Vector3 localPosition2 = transform2.localPosition with
          {
            y = num2
          };
          transform2.localPosition = localPosition2;
          break;
        case TreeHarvesterState.TreeIsUp:
          break;
        case TreeHarvesterState.PositioningForUnload:
          break;
        case TreeHarvesterState.UnloadingTree:
          if (this.m_trimmedTreeGo.IsNone || this.m_trimmedTreeGo.Value.transform.childCount < TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE)
            break;
          for (int index = 0; index < this.m_prevNumCuts * this.m_numSectionsPerCut + Mathf.CeilToInt((float) this.m_numSectionsToHide / 2f); ++index)
            this.m_trimmedTreeGo.Value.transform.GetChild(index).gameObject.SetActive(false);
          for (int index1 = 0; index1 < this.m_numSectionsPerCut; ++index1)
          {
            int index2 = this.m_prevNumCuts * this.m_numSectionsPerCut + index1 + Mathf.CeilToInt((float) this.m_numSectionsToHide / 2f);
            if (index2 < 0 || index2 >= this.m_trimmedTreeGo.Value.transform.childCount)
            {
              Assert.Fail(string.Format("Invalid child index for trimmed tree go: '{0}'.", (object) index2));
              break;
            }
            GameObject gameObject = this.m_trimmedTreeGo.Value.transform.GetChild(index2).gameObject;
            MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
            if ((Object) component == (Object) null)
            {
              Assert.Fail("Trimmed tree go has no mesh renderer.");
              break;
            }
            gameObject.transform.Translate(0.1f * AbsoluteT * animationPercent, 0.0f, 0.0f);
            component.material.SetOverrideTag("RenderType", "Fade");
            component.material.SetInt("_SrcBlend", 5);
            component.material.SetInt("_DstBlend", 10);
            component.material.SetInt("_ZWrite", 0);
            component.material.DisableKeyword("_ALPHATEST_ON");
            component.material.EnableKeyword("_ALPHABLEND_ON");
            component.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            component.material.renderQueue = 3000;
            component.material.SetColor(TreeHarvesterMb.COLOR_SHADER_ID, new Color(1f, 1f, 1f, Mathf.Max(0.0f, ((float) (1.0 - 2.0 * ((double) animationPercent - 0.20000000298023224))).Min(1f))));
          }
          break;
        case TreeHarvesterState.ReturningFromUnloadWithCargo:
          break;
        case TreeHarvesterState.ReturningFromUnloadToIdle:
          break;
        case TreeHarvesterState.FoldingArm:
          break;
        default:
          Assert.Fail(string.Format("Unknown tree harvester animation progress state: '{0}'.", (object) state));
          break;
      }
    }

    public TreeHarvesterMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_currentTreeRenderers = new List<MeshRenderer>();
      this.m_trimmedTreesGos = new Lyst<KeyValuePair<TreeProto, GameObject>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TreeHarvesterMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TreeHarvesterMb.s_gripperToNeutral = Quaternion.Euler(270f, 155f, 0.0f);
      TreeHarvesterMb.s_localX = TreeHarvesterMb.s_gripperToNeutral * new Vector3(1f, 0.0f, 0.0f);
      TreeHarvesterMb.s_localZ = TreeHarvesterMb.s_gripperToNeutral * new Vector3(0.0f, 0.0f, 1f);
      TreeHarvesterMb.COLOR_SHADER_ID = Shader.PropertyToID("_Color");
      TreeHarvesterMb.LIGHTS_OFF_DELAY = 10.Seconds();
    }
  }
}
