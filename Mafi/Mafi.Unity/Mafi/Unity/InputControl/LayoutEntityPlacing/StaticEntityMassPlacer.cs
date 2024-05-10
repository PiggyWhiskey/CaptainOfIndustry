// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.LayoutEntityPlacing.StaticEntityMassPlacer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Lifts;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl.Blueprints;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.Inspectors.Buildings;
using Mafi.Unity.InputControl.ResVis;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl.Tools;
using Mafi.Unity.Mine;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.Terrain;
using Mafi.Unity.Trees;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.LayoutEntityPlacing
{
  /// <summary>
  /// A tool to place a <see cref="T:Mafi.Core.Entities.Static.Layout.LayoutEntity" /> on the ground.
  /// </summary>
  public class StaticEntityMassPlacer
  {
    private static readonly float HEIGHT_CHANGER_TOOL_SENSITIVITY;
    private readonly TerrainCursor m_terrainCursor;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly LayoutEntityToolbox m_toolbox;
    private readonly CameraController m_cameraController;
    private readonly TransportPreviewManager m_transportPreviewManager;
    private readonly FloatingPricePopup m_pricePopup;
    private readonly IoPortsRenderer m_portsRenderer;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly LastUsedStaticEntityTransform m_lastUsedTransform;
    private readonly LayoutEntityPreviewManager m_layoutEntityPreviewManager;
    private readonly EntitiesCloneConfigHelper m_configCloneHelper;
    private readonly CursorPickingManager m_picker;
    private readonly StaticEntityMassPlacer.LiftPlacementHelper m_liftPlacementHelper;
    private readonly ProtosDb m_protosDb;
    private readonly Lyst<KeyValuePair<IStaticEntityPreview, EntityConfigData>> m_entityPreviews;
    private readonly Lyst<EntityConfigData> m_entitiesToAdd;
    private readonly Lyst<TileSurfaceCopyPasteData> m_surfacesToAdd;
    private readonly Dict<IoPortKey, IoPortType> m_portLocationsTmp;
    private bool m_isNonCopiedPlacement;
    private Vector3 m_initialMousePosition;
    private ThicknessTilesI m_lastHeightDelta;
    private ThicknessTilesI m_phase2RelativeHeight;
    private EntityPlacementPhase m_placementPhase;
    private readonly Cursoor m_cursor;
    private readonly AudioSource m_rotateSound;
    private readonly AudioSource m_unselectEntitySound;
    private readonly AudioSource m_invalidSound;
    private readonly AudioSource m_transportBindSound;
    private readonly AudioSource m_placedSound;
    private readonly LayoutEntitySlotPlacerHelper m_helper;
    private bool m_isFlipAllowed;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly IActivator m_treeDesignationActivator;
    private readonly IActivator m_oceanAreasActivator;
    private readonly ResVisBarsRenderer.Activator m_resVisActivator;
    private HighlightId? m_currentPortsHighlight;
    private readonly Lyst<IInputCommand> m_ongoingCmds;
    private Option<object> m_owner;
    private Option<Action> m_onCancelled;
    private Option<Action> m_onEntityPlaced;
    private ThicknessIRange m_allowedHeightRange;
    private bool m_singleEntityMode;
    private StaticEntityMassPlacer.ApplyConfigMode m_configApplyMode;
    private TileSurfaceData[] m_surfacePreviewData;
    private RectangleTerrainArea2i m_surfacePlacementArea;

    public bool IsActive => this.m_owner.HasValue;

    public StaticEntityMassPlacer(
      NewInstanceOf<TerrainCursor> terrainCursor,
      ShortcutsManager shortcutsManager,
      CursorManager cursorManager,
      NewInstanceOf<FloatingPricePopup> pricePopup,
      NewInstanceOf<FloatingPricePopup> liftHeightPopup,
      IoPortsRenderer portsRenderer,
      TowerAreasRenderer towerAreasRenderer,
      TerrainRenderer terrainRenderer,
      TreeHarvestingDesignatorRenderer treeDesignationRenderer,
      ResVisBarsRenderer resVisRenderer,
      LastUsedStaticEntityTransform lastUsedTransform,
      LayoutEntityPreviewManager layoutEntityPreviewManager,
      NewInstanceOf<LayoutEntityToolbox> toolbox,
      NewInstanceOf<LayoutEntitySlotPlacerHelper> layoutEntityPlacerHelper,
      CameraController cameraController,
      TransportPreviewManager transportPreviewManager,
      OceanAreasOverlayRenderer oceanOverlayRenderer,
      EntitiesCloneConfigHelper configCloneHelper,
      CursorPickingManager picker,
      ProtosDb protosDb,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_entityPreviews = new Lyst<KeyValuePair<IStaticEntityPreview, EntityConfigData>>();
      this.m_entitiesToAdd = new Lyst<EntityConfigData>();
      this.m_surfacesToAdd = new Lyst<TileSurfaceCopyPasteData>();
      this.m_portLocationsTmp = new Dict<IoPortKey, IoPortType>();
      this.m_ongoingCmds = new Lyst<IInputCommand>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_shortcutsManager = shortcutsManager;
      this.m_pricePopup = pricePopup.Instance;
      this.m_portsRenderer = portsRenderer;
      this.m_terrainRenderer = terrainRenderer;
      this.m_lastUsedTransform = lastUsedTransform;
      this.m_layoutEntityPreviewManager = layoutEntityPreviewManager;
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
      this.m_treeDesignationActivator = treeDesignationRenderer.CreateActivator();
      this.m_oceanAreasActivator = oceanOverlayRenderer.CreateActivator();
      this.m_resVisActivator = resVisRenderer.CreateActivator();
      this.m_helper = layoutEntityPlacerHelper.Instance;
      this.m_toolbox = toolbox.Instance;
      this.m_cameraController = cameraController;
      this.m_transportPreviewManager = transportPreviewManager;
      this.m_configCloneHelper = configCloneHelper;
      this.m_picker = picker;
      this.m_protosDb = protosDb;
      this.m_liftPlacementHelper = new StaticEntityMassPlacer.LiftPlacementHelper(this, liftHeightPopup.Instance);
      this.m_cursor = cursorManager.RegisterCursor(builder.Style.Cursors.Build);
      this.m_rotateSound = builder.AudioDb.GetSharedAudio(builder.Audio.Rotate);
      this.m_unselectEntitySound = builder.AudioDb.GetSharedAudio(builder.Audio.EntityUnselect);
      this.m_invalidSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.m_transportBindSound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/TransportBind.prefab");
      this.m_placedSound = builder.AudioDb.GetSharedAudio(builder.Audio.BuildingPlaced);
      this.m_toolbox.SetOnRotate(new Action(this.rotate));
      this.m_toolbox.SetOnFlip(new Action(this.flip));
      this.m_toolbox.SetOnUp(new Func<bool?>(this.cursorUp));
      this.m_toolbox.SetOnDown(new Func<bool?>(this.cursorDown));
      this.m_toolbox.SetOnToggleZipperPlacement(new Action(this.toggleZipperPlacement));
      this.m_toolbox.SetOnTogglePricePopup(new Action(this.togglePricePopup));
      RelTile1i relTile1i = BaseEntityCursorInputController<IStaticEntity>.MAX_AREA_EDGE_SIZE.Max(BlueprintCreationController.MAX_AREA_EDGE_SIZE);
      if (relTile1i.Value > 2048)
      {
        Log.Warning("Very large potential blueprint size. Could cause issues! Truncating to 2048.");
        relTile1i = new RelTile1i(2048);
      }
      this.m_surfacePreviewData = new TileSurfaceData[relTile1i.Value.Squared()];
    }

    public void Activate(object owner, Action onEntityPlaced = null, Action onCancel = null)
    {
      if (this.m_owner.HasValue)
      {
        Log.Warning("LayoutEntityPlacer is already activated by '" + this.m_owner.Value.GetType().Name + "'.");
      }
      else
      {
        this.m_owner = (Option<object>) owner;
        this.m_onEntityPlaced = (Option<Action>) onEntityPlaced;
        this.m_onCancelled = (Option<Action>) onCancel;
        this.clearPreviews();
        this.m_toolbox.Show();
        this.m_cursor.Show();
        this.m_terrainCursor.Activate();
        this.m_toolbox.DisplayPricePopupDisabled(this.m_pricePopup.IsTemporarilyHidden);
        this.m_liftPlacementHelper.Activate();
      }
    }

    public void Deactivate(bool playSound = false)
    {
      if (!this.IsActive)
      {
        Log.Warning("LayoutEntityPlacer is already deactivated.");
      }
      else
      {
        this.m_owner = Option<object>.None;
        if (this.m_entityPreviews.IsNotEmpty | playSound)
          this.m_unselectEntitySound.Play();
        Action valueOrNull = this.m_onCancelled.ValueOrNull;
        if (valueOrNull != null)
          valueOrNull();
        this.m_onCancelled = Option<Action>.None;
        this.m_onEntityPlaced = Option<Action>.None;
        this.m_helper.Deactivate();
        this.m_toolbox.Hide();
        this.clearPreviews();
        this.clearPortHighlights();
        this.m_cursor.Hide();
        this.m_pricePopup.Hide();
        this.m_terrainCursor.Deactivate();
        this.m_towerAreasAndDesignatorsActivator.DeactivateIfActive();
        this.m_treeDesignationActivator.DeactivateIfActive();
        this.m_oceanAreasActivator.DeactivateIfActive();
        this.m_resVisActivator.HideAll();
        this.m_liftPlacementHelper.Deactivate();
      }
    }

    private void clearPortHighlights()
    {
      if (!this.m_currentPortsHighlight.HasValue)
        return;
      this.m_portsRenderer.ClearPortsHighlight(this.m_currentPortsHighlight.Value);
      this.m_currentPortsHighlight = new HighlightId?();
    }

    private Tile3i getCursorPosition() => this.m_terrainCursor.Tile3f.Tile3iRounded;

    private bool? cursorUp()
    {
      if (this.m_terrainCursor.RelativeHeight < this.m_allowedHeightRange.To)
      {
        this.m_terrainCursor.RelativeHeight += ThicknessTilesI.One;
        return new bool?(true);
      }
      if (this.m_placementPhase == EntityPlacementPhase.Final && this.m_phase2RelativeHeight + this.m_lastHeightDelta < this.m_allowedHeightRange.To)
      {
        this.m_terrainCursor.RelativeHeight += ThicknessTilesI.One;
        return new bool?(true);
      }
      bool flag = true;
      foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
      {
        EntityValidationResult? validationResult = entityPreview.Key.ValidationResult;
        if (!validationResult.HasValue)
          return new bool?();
        validationResult = entityPreview.Key.ValidationResult;
        if (validationResult.Value.IsSuccess && entityPreview.Key.CanMoveUpDownIfValid())
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return new bool?(false);
      this.m_terrainCursor.RelativeHeight += ThicknessTilesI.One;
      return new bool?(true);
    }

    private bool? cursorDown()
    {
      if (this.m_terrainCursor.RelativeHeight > this.m_allowedHeightRange.From)
      {
        this.m_terrainCursor.RelativeHeight -= ThicknessTilesI.One;
        return new bool?(true);
      }
      bool flag = true;
      foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
      {
        EntityValidationResult? validationResult = entityPreview.Key.ValidationResult;
        if (!validationResult.HasValue)
          return new bool?();
        validationResult = entityPreview.Key.ValidationResult;
        if (validationResult.Value.IsSuccess && entityPreview.Key.CanMoveUpDownIfValid())
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return new bool?(false);
      this.m_terrainCursor.RelativeHeight -= ThicknessTilesI.One;
      return new bool?(true);
    }

    private void togglePricePopup()
    {
      this.m_pricePopup.SetTemporarilyHidden(!this.m_pricePopup.IsTemporarilyHidden);
      this.m_toolbox.DisplayPricePopupDisabled(this.m_pricePopup.IsTemporarilyHidden);
    }

    private void toggleZipperPlacement() => Log.Error("Not implemented toggle zipper placement");

    public void SetLayoutEntityToPlace(
      ILayoutEntityProto proto,
      StaticEntityMassPlacer.ApplyConfigMode configApplyMode = StaticEntityMassPlacer.ApplyConfigMode.NeverApply,
      TileTransform? transform = null,
      Option<EntityConfigData> config = default (Option<EntityConfigData>))
    {
      if (!this.IsActive)
      {
        Log.Error("Placer is not activated");
      }
      else
      {
        this.clearPreviews();
        this.m_helper.Activate(proto);
        this.m_allowedHeightRange = proto.Layout.PlacementHeightRange;
        this.m_isFlipAllowed = !proto.CannotBeReflected;
        this.m_singleEntityMode = true;
        this.m_configApplyMode = configApplyMode;
        this.m_toolbox.SetDoNotCopyConfigVisibility(configApplyMode == StaticEntityMassPlacer.ApplyConfigMode.ApplyIfNotDisabled);
        TileTransform transform1 = !transform.HasValue ? new TileTransform(this.getCursorPosition(), this.m_lastUsedTransform.LastRotation, this.m_isFlipAllowed && this.m_lastUsedTransform.LastReflection) : transform.Value;
        this.m_isNonCopiedPlacement = !transform.HasValue;
        this.m_placementPhase = EntityPlacementPhase.FirstAndFinal;
        IStaticEntityPreview preview;
        if (proto is LiftProto liftProto && this.m_entityPreviews.Count == 0)
        {
          this.m_placementPhase = this.m_isNonCopiedPlacement ? EntityPlacementPhase.First : EntityPlacementPhase.FirstAndFinal;
          preview = (IStaticEntityPreview) this.m_layoutEntityPreviewManager.CreatePreview(proto, this.m_placementPhase, transform1, enableMiniZipperPlacement: true, disablePortPreviewsPredicate: this.m_isNonCopiedPlacement ? (Predicate<IoPortTemplate>) (p => p.Type == IoPortType.Input) : (Predicate<IoPortTemplate>) null);
          this.m_liftPlacementHelper.SetNewProto(liftProto);
        }
        else
          preview = (IStaticEntityPreview) this.m_layoutEntityPreviewManager.CreatePreview(proto, this.m_placementPhase, transform1);
        this.m_terrainCursor.RelativeHeight = this.m_allowedHeightRange.ClampToRange(transform.HasValue ? preview.GetEstPlacementHeight() : this.m_lastUsedTransform.LastHeight);
        this.m_helper.SetTransform(transform1);
        this.updateLastTransformStore();
        if (config.IsNone)
        {
          switch (proto)
          {
            case LayoutEntityProto entityProto1:
              config = (Option<EntityConfigData>) new EntityConfigData(EntityId.Invalid, (IProto) entityProto1, this.m_configCloneHelper.ConfigContext);
              config.Value.Transform = new TileTransform?(transform1);
              break;
            case TreeProto entityProto2:
              config = (Option<EntityConfigData>) new EntityConfigData(EntityId.Invalid, (IProto) entityProto2, this.m_configCloneHelper.ConfigContext);
              config.Value.Transform = new TileTransform?(transform1);
              break;
            default:
              Log.Warning(string.Format("{0} has config but is not LayoutEntityProto or TreeProto", (object) proto));
              break;
          }
        }
        this.m_entityPreviews.Add(Make.Kvp<IStaticEntityPreview, EntityConfigData>(preview, config.Value));
        if (!(proto is ILiftProto) || this.m_entityPreviews.Count != 1)
          this.m_toolbox.SetPortSnappingButtonEnabled(false);
        this.calculatePrice();
        LayoutEntityProto.VisualizedLayers visualizedLayers = proto.Graphics.VisualizedLayers;
        this.m_resVisActivator.ShowExactly(visualizedLayers.AllVisualizedProducts);
        this.m_towerAreasAndDesignatorsActivator.SetActive(true);
        this.m_treeDesignationActivator.SetActive(visualizedLayers.TreeDesignators);
        if (!preview.ShowOceanAreas)
          return;
        this.m_oceanAreasActivator.ActivateIfNotActive();
      }
    }

    /// <summary>Starts preview with a set of entities to clone from.</summary>
    public void SetEntitiesToClone(
      IIndexable<EntityConfigData> configs,
      Option<IIndexable<TileSurfaceCopyPasteData>> surfaces,
      StaticEntityMassPlacer.ApplyConfigMode configApplyMode,
      bool isHeightNormalizedToZero = false)
    {
      if (!this.IsActive)
      {
        Log.Error("Placer is not activated");
      }
      else
      {
        bool flag1 = surfaces.IsNone || surfaces.Value.IsEmpty<TileSurfaceCopyPasteData>();
        if (configs.IsEmpty<EntityConfigData>() & flag1)
          return;
        if (configs.Count == 1 & flag1 && configs.First<EntityConfigData>().Prototype.ValueOrNull is LayoutEntityProto valueOrNull)
        {
          this.SetLayoutEntityToPlace((ILayoutEntityProto) valueOrNull, configApplyMode, configs.First<EntityConfigData>().Transform, (Option<EntityConfigData>) configs.First<EntityConfigData>());
        }
        else
        {
          this.m_toolbox.SetDoNotCopyConfigVisibility(configApplyMode == StaticEntityMassPlacer.ApplyConfigMode.ApplyIfNotDisabled);
          this.clearPreviews();
          this.m_isFlipAllowed = true;
          this.m_singleEntityMode = false;
          this.m_configApplyMode = configApplyMode;
          this.m_helper.ActivateNoSlotsSnapping();
          this.m_terrainCursor.RelativeHeight = ThicknessTilesI.Zero;
          this.m_allowedHeightRange = new ThicknessIRange(ThicknessTilesI.MinValue, ThicknessTilesI.MaxValue);
          Tile3i min = Tile3i.MaxValue;
          Tile2i max = Tile2i.MinValue;
          bool flag2 = false;
          foreach (EntityConfigData config in configs)
          {
            IStaticEntityPreview preview;
            if (this.tryCreatePreview(config, out preview))
            {
              addPreview(preview, config);
              flag2 |= preview.ShowOceanAreas;
            }
          }
          if (!flag1)
          {
            Tile2i origin = Tile2i.MaxValue;
            Tile2i tile2i = Tile2i.MinValue;
            if (configs.Count == 0)
              min = new Tile3i(int.MaxValue, int.MaxValue, 0);
            foreach (TileSurfaceCopyPasteData surfaceCopyPasteData in surfaces.Value)
            {
              this.m_surfacesToAdd.Add(surfaceCopyPasteData);
              min = new Tile3i(min.X.Min(surfaceCopyPasteData.Position.X), min.Y.Min(surfaceCopyPasteData.Position.Y), min.Z);
              max = max.Max(surfaceCopyPasteData.Position);
              origin = origin.Min(surfaceCopyPasteData.Position);
              tile2i = tile2i.Max(surfaceCopyPasteData.Position);
            }
            this.m_allowedHeightRange = this.m_allowedHeightRange.Intersect(new ThicknessIRange(0, 0));
            if (surfaces.Value.Count < this.m_surfacePreviewData.Length)
            {
              this.m_surfacePlacementArea = new RectangleTerrainArea2i(origin, tile2i - origin + RelTile2i.One);
              this.updateSurfacePreview();
              this.m_terrainRenderer.SetSurfacePastePreviewData(this.m_surfacePlacementArea, this.m_surfacePreviewData, true);
            }
            else
              Log.Error(string.Format("Insufficient space for surface preview data. Need {0}, have {1}.", (object) surfaces.Value.Count, (object) this.m_surfacePreviewData.Length));
          }
          if (this.m_entityPreviews.IsEmpty && this.m_surfacesToAdd.IsEmpty)
            return;
          try
          {
            if (this.m_allowedHeightRange.Height.IsNegative)
              this.m_allowedHeightRange = new ThicknessIRange();
          }
          catch (Exception ex)
          {
            Log.Exception(ex);
            this.m_allowedHeightRange = new ThicknessIRange();
          }
          this.m_helper.SetTransform(isHeightNormalizedToZero ? new TileTransform(min.Tile2i.Average(max).ExtendZ(0)) : new TileTransform(min.Tile2i.Average(max).ExtendZ(min.Z)));
          this.m_towerAreasAndDesignatorsActivator.SetActive(true);
          if (flag2)
            this.m_oceanAreasActivator.ActivateIfNotActive();
          this.m_portLocationsTmp.Clear();
          foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
            entityPreview.Key.GetPortLocations(this.m_portLocationsTmp);
          foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
            entityPreview.Key.HideConnectedPorts(this.m_portLocationsTmp);
          this.m_portLocationsTmp.Clear();
          this.calculatePrice();

          void addPreview(IStaticEntityPreview preview, EntityConfigData data)
          {
            this.m_entityPreviews.Add(Make.Kvp<IStaticEntityPreview, EntityConfigData>(preview, data));
            this.m_isNonCopiedPlacement = false;
            Tile3i basePosition;
            bool canBeReflected;
            ThicknessIRange allowedPlacementRange;
            preview.GetPlacementParams(out basePosition, out canBeReflected, out allowedPlacementRange);
            this.m_isFlipAllowed &= canBeReflected;
            min = min.Min(basePosition);
            max = max.Max(basePosition.Tile2i);
            this.m_allowedHeightRange = this.m_allowedHeightRange.Intersect(allowedPlacementRange);
          }
        }
      }
    }

    private void calculatePrice()
    {
      AssetValue empty = AssetValue.Empty;
      foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
        empty += entityPreview.Key.Price;
      this.m_pricePopup.SetBuyPrice(empty);
      this.m_pricePopup.UpdatePosition();
    }

    private void updateSurfacePreview()
    {
      if (this.m_surfacePreviewData.Length < this.m_surfacePlacementArea.AreaTiles)
      {
        Log.Error("Surface preview data buffer is smaller than the placement area. " + string.Format("It is {0}, the area is {1}", (object) this.m_surfacePreviewData.Length, (object) this.m_surfacePlacementArea.AreaTiles));
        this.m_surfacePreviewData = new TileSurfaceData[this.m_surfacePlacementArea.AreaTiles];
      }
      Array.Clear((Array) this.m_surfacePreviewData, 0, this.m_surfacePlacementArea.AreaTiles);
      foreach (TileSurfaceCopyPasteData surfaceCopyPasteData in this.m_surfacesToAdd)
        this.m_surfacePreviewData[(surfaceCopyPasteData.Position.Y - this.m_surfacePlacementArea.Origin.Y) * this.m_surfacePlacementArea.Size.X + surfaceCopyPasteData.Position.X - this.m_surfacePlacementArea.Origin.X] = surfaceCopyPasteData.SurfaceData;
    }

    private bool tryCreatePreview(EntityConfigData entityData, out IStaticEntityPreview preview)
    {
      Proto valueOrNull = entityData.Prototype.ValueOrNull;
      if (valueOrNull == (Proto) null)
      {
        preview = (IStaticEntityPreview) null;
        return false;
      }
      TileTransform? transform = entityData.Transform;
      if (transform.HasValue && valueOrNull is LayoutEntityProto proto)
      {
        preview = (IStaticEntityPreview) this.m_layoutEntityPreviewManager.CreatePreview((ILayoutEntityProto) proto, this.m_placementPhase, transform.Value);
        return true;
      }
      if (valueOrNull is TransportPillarProto)
      {
        preview = (IStaticEntityPreview) null;
        Log.Warning("Trying to mass-place transport pillar.");
        return false;
      }
      Option<TransportTrajectory> trajectory = entityData.Trajectory;
      if (trajectory.HasValue)
      {
        preview = (IStaticEntityPreview) this.m_transportPreviewManager.CreatePreviewPooled(trajectory.Value, entityData.Pillars ?? ImmutableArray<Tile2i>.Empty);
        return true;
      }
      Log.Warning(string.Format("Failed to create preview for entity '{0}'.", (object) entityData.Prototype));
      preview = (IStaticEntityPreview) null;
      return false;
    }

    private void clearPreviews()
    {
      this.m_placementPhase = EntityPlacementPhase.FirstAndFinal;
      this.m_entityPreviews.ForEachAndClear((Action<KeyValuePair<IStaticEntityPreview, EntityConfigData>>) (preview => preview.Key.DestroyAndReturnToPool()));
      this.m_surfacesToAdd.Clear();
      this.m_terrainRenderer.DisableSurfacePastePreview();
    }

    private void updateLastTransformStore()
    {
      this.m_lastUsedTransform.SetLastTransform(this.m_helper.Transform, this.m_terrainCursor.RelativeHeight);
    }

    private void reactivateForPlacingNextEntity()
    {
      this.m_helper.Deactivate();
      this.clearPortHighlights();
      this.m_placementPhase = EntityPlacementPhase.FirstAndFinal;
      if (this.m_entityPreviews.Count == 1 && this.m_entityPreviews.First.Key is LayoutEntityPreview key)
        this.m_helper.Activate(key.LayoutEntityProto);
      else
        this.m_helper.ActivateNoSlotsSnapping();
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        return false;
      this.m_toolbox.DoNotCopyConfigBtn.SetIsOn(this.m_shortcutsManager.IsOn(this.m_shortcutsManager.CopyExcludingSettings));
      this.m_toolbox.PlaceMultipleBtn.SetIsOn(this.m_shortcutsManager.IsOn(this.m_shortcutsManager.PlaceMultiple));
      this.m_toolbox.SetPortSnappingButtonEnabled(this.m_placementPhase == EntityPlacementPhase.First);
      if (this.m_entityPreviews.IsEmpty && this.m_surfacesToAdd.IsEmpty)
        return false;
      if (this.m_ongoingCmds.IsNotEmpty)
      {
        bool flag = false;
        foreach (IInputCommand ongoingCmd in this.m_ongoingCmds)
        {
          if (!ongoingCmd.IsProcessedAndSynced)
            return false;
          flag |= !ongoingCmd.HasError;
        }
        if (flag)
        {
          this.m_placedSound.Play();
          if (this.m_shortcutsManager.IsOn(this.m_shortcutsManager.PlaceMultiple))
          {
            this.reactivateForPlacingNextEntity();
          }
          else
          {
            Action valueOrNull = this.m_onEntityPlaced.ValueOrNull;
            if (valueOrNull != null)
              valueOrNull();
          }
        }
        else
          this.m_invalidSound.Play();
        this.m_ongoingCmds.Clear();
        return false;
      }
      if (this.m_placementPhase != EntityPlacementPhase.Final)
      {
        if (this.m_shortcutsManager.IsSecondaryActionUp)
        {
          this.Deactivate(true);
          return true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.LowerDown))
        {
          this.m_toolbox.OnDown();
          return true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.RaiseUp))
        {
          this.m_toolbox.OnUp();
          return true;
        }
      }
      this.m_pricePopup.UpdatePosition();
      this.m_pricePopup.SetErrorMessage(getErrorMsg());
      TileTransform transform = this.m_helper.Transform;
      if (this.m_placementPhase == EntityPlacementPhase.FirstAndFinal)
      {
        if (this.m_helper.SetPosition(this.getCursorPosition()))
          this.applyTransform(transform);
      }
      else if (this.m_placementPhase == EntityPlacementPhase.Final && this.m_terrainCursor.RelativeHeight != this.m_phase2RelativeHeight && this.m_helper.SetPosition(new Tile3i(this.m_helper.LastSetPosition.X, this.m_helper.LastSetPosition.Y, this.m_helper.LastSetPosition.Z + this.m_terrainCursor.RelativeHeight.Value - this.m_phase2RelativeHeight.Value)))
      {
        this.m_phase2RelativeHeight = this.m_terrainCursor.RelativeHeight;
        this.applyTransform(transform);
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Rotate))
      {
        this.rotate();
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TogglePricePopup))
      {
        this.m_toolbox.OnTogglePricePopup();
        return true;
      }
      if (this.m_isNonCopiedPlacement && this.m_entityPreviews.Count == 1 && this.m_entityPreviews[0].Key.EntityProto is LiftProto entityProto)
        return this.m_liftPlacementHelper.InputUpdate(inputScheduler, entityProto);
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Flip) && this.m_placementPhase != EntityPlacementPhase.Final)
      {
        this.flip();
        return true;
      }
      if (!this.m_currentPortsHighlight.HasValue)
      {
        Set<ShapeTypePair> set = new Set<ShapeTypePair>();
        foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
          entityPreview.Key.GetPortTypes(set);
        ImmutableArray<ShapeTypePair> pairsArr = set.ToImmutableArray<ShapeTypePair>();
        this.m_currentPortsHighlight = new HighlightId?(this.m_portsRenderer.HighlightPorts((Predicate<IoPort>) (x => x.IsNotConnected && pairsArr.Any((Func<ShapeTypePair, bool>) (pair => pair.Matches(x)))), true));
      }
      if (!this.m_shortcutsManager.IsPrimaryActionUp || EventSystem.current.IsPointerOverGameObject())
        return false;
      bool flag1 = this.m_configApplyMode == StaticEntityMassPlacer.ApplyConfigMode.NeverApply || this.m_configApplyMode == StaticEntityMassPlacer.ApplyConfigMode.ApplyIfNotDisabled && this.m_shortcutsManager.IsOn(this.m_shortcutsManager.CopyExcludingSettings);
      this.m_entitiesToAdd.Clear();
      foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
      {
        IStaticEntityPreview key = entityPreview.Key;
        EntityValidationResult? validationResult = key.ValidationResult;
        if (validationResult.HasValue)
        {
          validationResult = key.ValidationResult;
          if (validationResult.Value.IsError)
            continue;
        }
        key.UpdateConfigWithTransform(entityPreview.Value);
        this.m_entitiesToAdd.Add(entityPreview.Value.CreateCopy());
      }
      if (this.m_entitiesToAdd.IsNotEmpty)
        this.m_ongoingCmds.Add((IInputCommand) inputScheduler.ScheduleInputCmd<BatchCreateStaticEntitiesCmd>(new BatchCreateStaticEntitiesCmd(this.m_entitiesToAdd.ToImmutableArray(), applyConfiguration: !flag1)));
      if (this.m_surfacesToAdd.IsNotEmpty)
        this.m_ongoingCmds.Add((IInputCommand) inputScheduler.ScheduleInputCmd<BatchAddSurfaceDecalCmd>(new BatchAddSurfaceDecalCmd(this.m_surfacesToAdd.ToImmutableArray())));
      this.m_pricePopup.Hide();
      return true;

      LocStrFormatted getErrorMsg()
      {
        foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
        {
          IStaticEntityPreview key = entityPreview.Key;
          EntityValidationResult? validationResult = key.ValidationResult;
          if (validationResult.HasValue)
          {
            validationResult = key.ValidationResult;
            if (validationResult.Value.IsError)
            {
              validationResult = key.ValidationResult;
              return new LocStrFormatted(validationResult.Value.ErrorMessageForPlayer);
            }
          }
        }
        return LocStrFormatted.Empty;
      }
    }

    private void rotate()
    {
      TileTransform transform = this.m_helper.Transform;
      if (!this.m_helper.SetRotation(this.m_helper.LastSetRotation.RotatedMinus90))
        return;
      this.applyTransform(transform);
      this.m_rotateSound.Play();
    }

    private void flip()
    {
      if (!this.m_isFlipAllowed)
      {
        this.m_invalidSound.Play();
      }
      else
      {
        AngleDegrees1f normalized = this.m_cameraController.CameraModel.State.YawAngle.Normalized;
        bool flag = (!(normalized > 45.Degrees()) || !(normalized < 135.Degrees()) ? (!(normalized > 225.Degrees()) ? 0 : (normalized < 315.Degrees() ? 1 : 0)) : 1) != (this.m_helper.LastSetRotation.Is90Or270Deg ? 1 : 0);
        if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
          flag = !flag;
        TileTransform transform = this.m_helper.Transform;
        if (!this.m_helper.SetReflection(!this.m_helper.LastSetReflection))
          return;
        if (flag)
          this.m_helper.SetRotation(this.m_helper.LastSetRotation + Rotation90.Deg180);
        this.applyTransform(transform);
        this.m_rotateSound.Play();
      }
    }

    private void applyTransform(TileTransform oldTransform)
    {
      if (this.m_singleEntityMode && this.m_entityPreviews.Count == 1 && this.m_entityPreviews.First.Key is IStaticEntityPreviewDirect key)
      {
        key.SetTransform(this.m_helper.Transform);
      }
      else
      {
        RelTile3i deltaPosition = this.m_helper.Transform.Position - oldTransform.Position;
        Rotation90 deltaRotation = this.m_helper.Transform.Rotation - oldTransform.Rotation;
        bool deltaReflection = this.m_helper.Transform.IsReflected ^ oldTransform.IsReflected;
        Tile2i xy = oldTransform.Position.Xy;
        foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_entityPreviews)
          entityPreview.Key.ApplyTransformDelta(deltaPosition, deltaRotation, deltaReflection, xy);
        if (this.m_surfacesToAdd.IsNotEmpty)
        {
          for (int index = 0; index < this.m_surfacesToAdd.Count; ++index)
            this.m_surfacesToAdd[index] = this.m_surfacesToAdd[index].ApplyTransformDelta(deltaPosition, deltaRotation, deltaReflection, xy);
          Tile3i tile3i = Tile3i.MaxValue;
          Tile2i tile2i = Tile2i.MinValue;
          foreach (TileSurfaceCopyPasteData surfaceCopyPasteData in this.m_surfacesToAdd)
          {
            tile3i = tile3i.Min(surfaceCopyPasteData.Position.ExtendZ(0));
            tile2i = tile2i.Max(surfaceCopyPasteData.Position);
          }
          this.m_surfacePlacementArea = new RectangleTerrainArea2i(tile3i.Xy, tile2i - tile3i.Xy + RelTile2i.One);
          if (deltaReflection || deltaRotation.AngleIndex != 0)
          {
            this.updateSurfacePreview();
            this.m_terrainRenderer.SetSurfacePastePreviewData(this.m_surfacePlacementArea, this.m_surfacePreviewData, true);
          }
          else
            this.m_terrainRenderer.SetSurfacePastePreviewData(this.m_surfacePlacementArea, this.m_surfacePreviewData, false);
        }
      }
      this.updateLastTransformStore();
    }

    static StaticEntityMassPlacer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      StaticEntityMassPlacer.HEIGHT_CHANGER_TOOL_SENSITIVITY = 0.03f;
    }

    public enum ApplyConfigMode
    {
      NeverApply,
      AlwaysApply,
      ApplyIfNotDisabled,
    }

    private sealed class LiftPlacementHelper
    {
      private float MIN_MOUSE_DELTA_PX;
      private static readonly ColorRgba SELECTION_OK;
      private static readonly ColorRgba SELECTION_NOT_OK;
      private readonly StaticEntityMassPlacer m_parent;
      private bool m_disableSnapping;
      private bool m_isFlippedToOutput;
      private Vector3 m_lastMousePos;
      private Tile2i m_lastMouseTile;
      private StaticEntityMassPlacer.LiftPlacementHelper.PickResult m_previousPickResult;
      private Option<LiftProto> m_currentLiftProto;
      private readonly Predicate<IoPort> m_compatiblePortPredicate;
      private readonly FloatingPricePopup m_heightWarningPopup;

      public LiftPlacementHelper(
        StaticEntityMassPlacer parent,
        FloatingPricePopup heightWarningPopup)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.MIN_MOUSE_DELTA_PX = 5f;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_parent = parent;
        this.m_lastMousePos = Vector3.zero;
        this.m_compatiblePortPredicate = (Predicate<IoPort>) (x => !x.IsConnected && (Proto) x.ShapePrototype == (Proto) this.m_currentLiftProto.ValueOrNull?.PortsShape);
        this.m_heightWarningPopup = heightWarningPopup;
        this.m_heightWarningPopup.SetErrorMessage((LocStrFormatted) TrCore.AdditionWarning__HighLift);
        parent.m_toolbox.SetOnToggleSnapping(new Action(this.togglePortSnapping));
      }

      public void Activate()
      {
      }

      public void Deactivate() => this.m_heightWarningPopup.Hide();

      private StaticEntityMassPlacer.LiftPlacementHelper.PickResult pickAtCursorPosition()
      {
        Vector3 mousePosition = UnityEngine.Input.mousePosition;
        Tile2i tile2i = this.m_parent.m_terrainCursor.Tile2i;
        Vector3 vector3 = mousePosition - this.m_lastMousePos;
        if ((double) vector3.x * (double) vector3.x + (double) vector3.y * (double) vector3.y < (double) this.MIN_MOUSE_DELTA_PX * (double) this.MIN_MOUSE_DELTA_PX && tile2i == this.m_lastMouseTile)
          return this.m_previousPickResult;
        this.m_lastMousePos = mousePosition;
        this.m_lastMouseTile = tile2i;
        if (!this.m_disableSnapping)
        {
          Option<IRenderedEntity> option = this.m_parent.m_picker.PickEntity<IRenderedEntity>();
          IoPort port;
          if (this.pickAndSelectConnectablePort(out port) || option.ValueOrNull is IEntityWithPorts valueOrNull && !(option.ValueOrNull is Mafi.Core.Factory.Transports.Transport) && this.tryFindClosestMatchingPort(valueOrNull, this.m_parent.m_terrainCursor.Tile3i, out port))
          {
            Assert.That<bool>((Proto) this.m_currentLiftProto.Value.PortsShape == (Proto) port.ShapePrototype).IsTrue();
            RelTile3i zero = RelTile3i.Zero;
            if (port.Direction == Direction903d.PlusX)
              zero += RelTile3i.UnitX;
            else if (port.Direction == Direction903d.PlusY)
              zero += RelTile3i.UnitY;
            return this.m_previousPickResult = new StaticEntityMassPlacer.LiftPlacementHelper.PickResult(port.ExpectedConnectedPortCoord + zero, Option<TransportProto>.None, port.Type, new Direction903d?(port.Direction));
          }
        }
        return this.m_previousPickResult = new StaticEntityMassPlacer.LiftPlacementHelper.PickResult(Tile3i.MinValue);
      }

      private bool tryFindClosestMatchingPort(
        IEntityWithPorts ewp,
        Tile3i position,
        out IoPort port)
      {
        Assert.That<Predicate<IoPort>>(this.m_compatiblePortPredicate).IsNotNull<Predicate<IoPort>>();
        port = (IoPort) null;
        long num1 = long.MaxValue;
        foreach (IoPort port1 in ewp.Ports)
        {
          if (this.m_compatiblePortPredicate(port1))
          {
            long num2 = port1.Position.DistanceSqrTo(position);
            if (num2 < num1)
            {
              num1 = num2;
              port = port1;
            }
          }
        }
        return port != null;
      }

      private bool portConnectableMatcher(IoPort port, out ColorRgba color)
      {
        if (port.IsConnected)
        {
          color = ColorRgba.Empty;
          return false;
        }
        if (this.m_currentLiftProto.HasValue && (Proto) this.m_currentLiftProto.Value.PortsShape != (Proto) port.ShapePrototype)
        {
          color = StaticEntityMassPlacer.LiftPlacementHelper.SELECTION_NOT_OK;
          return false;
        }
        color = StaticEntityMassPlacer.LiftPlacementHelper.SELECTION_OK;
        return true;
      }

      /// <summary>
      /// Tries to pick a potential port under the cursor that is connectible.
      /// </summary>
      private bool pickAndSelectConnectablePort(out IoPort port)
      {
        port = this.m_parent.m_picker.PickPortAndSelect<IoPort>(new CursorPickingManager.PortPredicateReturningColor(this.portConnectableMatcher)).ValueOrNull;
        return port != null;
      }

      public void SetNewProto(LiftProto liftProto)
      {
        this.m_currentLiftProto = Option.Create<LiftProto>(liftProto);
        this.m_isFlippedToOutput = liftProto.HeightDelta.IsNegative;
        this.m_lastMousePos = Vector3.zero;
      }

      private void togglePortSnapping()
      {
        this.m_disableSnapping = !this.m_disableSnapping;
        this.m_parent.m_toolbox.DisplaySnappingDisabled(this.m_disableSnapping);
        this.m_lastMousePos = Vector3.zero;
      }

      public bool InputUpdate(IInputScheduler inputScheduler, LiftProto liftProto)
      {
        this.m_currentLiftProto = Option.Create<LiftProto>(liftProto);
        this.m_heightWarningPopup.UpdatePosition();
        if (this.m_parent.m_entityPreviews.IsEmpty)
        {
          Log.Error("No entity previews");
          return false;
        }
        Assert.That<int>(this.m_parent.m_entityPreviews.Count).IsEqualTo(1);
        LayoutEntitySlotPlacerHelper helper = this.m_parent.m_helper;
        if (this.m_parent.m_placementPhase == EntityPlacementPhase.First)
        {
          if (this.m_parent.m_shortcutsManager.IsDown(this.m_parent.m_shortcutsManager.LiftSnapping))
          {
            this.m_parent.m_toolbox.OnTogglePortSnapping();
            return true;
          }
          if (this.m_parent.m_shortcutsManager.IsDown(this.m_parent.m_shortcutsManager.Flip))
          {
            this.m_parent.m_rotateSound.Play();
            return this.flip();
          }
          if (!this.m_parent.m_currentPortsHighlight.HasValue)
          {
            Set<ShapeTypePair> set = new Set<ShapeTypePair>();
            foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_parent.m_entityPreviews)
              entityPreview.Key.GetPortTypes(set);
            ImmutableArray<ShapeTypePair> pairsArr = set.ToImmutableArray<ShapeTypePair>();
            this.m_parent.m_currentPortsHighlight = new HighlightId?(this.m_parent.m_portsRenderer.HighlightPorts((Predicate<IoPort>) (x => x.IsNotConnected && pairsArr.Any((Func<ShapeTypePair, bool>) (pair => pair.Matches(x))))));
          }
          StaticEntityMassPlacer.LiftPlacementHelper.PickResult pickResult = this.pickAtCursorPosition();
          TileTransform transform = helper.Transform;
          bool flag = false;
          if (pickResult.PortType == IoPortType.Input)
          {
            if (!this.m_isFlippedToOutput)
            {
              this.flip();
              this.m_isFlippedToOutput = true;
            }
          }
          else if (pickResult.PortType == IoPortType.Output && this.m_isFlippedToOutput)
          {
            this.flip();
            this.m_isFlippedToOutput = false;
          }
          if (pickResult.Direction.HasValue)
          {
            Rotation90 rotation = pickResult.Direction.Value.ToHorizontalOrError().ToRotation();
            if (rotation != transform.Rotation && helper.SetRotation(rotation))
              flag = true;
          }
          if (pickResult.Position == Tile3i.MinValue)
          {
            if (helper.SetPosition(this.m_parent.getCursorPosition()))
              flag = true;
          }
          else if (helper.SetPosition(pickResult.Position))
            flag = true;
          if (flag)
            this.m_parent.applyTransform(transform);
        }
        if (this.m_parent.m_placementPhase == EntityPlacementPhase.Final)
        {
          if (this.m_parent.m_shortcutsManager.IsSecondaryActionUp)
          {
            this.m_parent.m_placementPhase = EntityPlacementPhase.First;
            return true;
          }
          KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview = this.m_parent.m_entityPreviews[0];
          IStaticEntityPreview key = entityPreview.Key;
          if (!entityPreview.Value.Transform.HasValue)
          {
            Log.Error("Non initial placement with null transform.");
            return false;
          }
          if (this.m_parent.m_shortcutsManager.IsDown(this.m_parent.m_shortcutsManager.Flip))
          {
            ILiftProto newProto;
            if (liftProto.TryGetHeightReversedProto(this.m_parent.m_protosDb, out newProto))
            {
              key.DestroyAndReturnToPool();
              this.m_parent.m_phase2RelativeHeight = this.m_parent.m_terrainCursor.RelativeHeight;
              this.m_parent.m_entityPreviews[0] = Make.Kvp<IStaticEntityPreview, EntityConfigData>((IStaticEntityPreview) this.m_parent.m_layoutEntityPreviewManager.CreatePreview((ILayoutEntityProto) newProto, this.m_parent.m_placementPhase, helper.Transform, enableMiniZipperPlacement: true), new EntityConfigData(EntityId.Invalid, (IProto) newProto, this.m_parent.m_configCloneHelper.ConfigContext)
              {
                Transform = new TileTransform?(helper.Transform)
              });
              this.m_parent.clearPortHighlights();
              this.m_isFlippedToOutput = !this.m_isFlippedToOutput;
              int num = this.m_isFlippedToOutput ? 1 : -1;
              this.m_parent.m_lastHeightDelta = newProto.HeightDelta;
              this.m_parent.m_initialMousePosition = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y + (float) (num * newProto.HeightDelta.Value) / StaticEntityMassPlacer.HEIGHT_CHANGER_TOOL_SENSITIVITY, UnityEngine.Input.mousePosition.z);
              this.m_parent.m_rotateSound.Play();
              return true;
            }
            Log.Error("Failed to get height flipped proto");
          }
          int num1 = 0;
          bool flag = false;
          if (this.m_parent.m_shortcutsManager.IsDown(this.m_parent.m_shortcutsManager.LowerDown))
            num1 = -1;
          else if (this.m_parent.m_shortcutsManager.IsDown(this.m_parent.m_shortcutsManager.RaiseUp))
          {
            num1 = 1;
            flag = true;
          }
          if (this.m_isFlippedToOutput)
            num1 = -num1;
          int heightChange = liftProto.HeightDelta.Value + num1;
          if (heightChange == 0)
            heightChange += num1;
          ILiftProto newProto1;
          if (liftProto.TryGetProtoForHeightDelta(this.m_parent.m_protosDb, heightChange, out newProto1) && newProto1 != liftProto)
          {
            key.DestroyAndReturnToPool();
            if (flag)
              this.m_parent.m_toolbox.PlayUpSound(new bool?(true));
            else
              this.m_parent.m_toolbox.PlayDownSound(new bool?(true));
            if (this.m_isFlippedToOutput)
            {
              if (newProto1.HeightDelta.Value > 0)
                helper.SetPosition(helper.LastSetPosition.AddZ(-newProto1.HeightDelta.Value + this.m_parent.m_lastHeightDelta.Value.Max(0)));
              else if (this.m_parent.m_lastHeightDelta.Value > 0)
                helper.SetPosition(helper.LastSetPosition.AddZ(this.m_parent.m_lastHeightDelta.Value));
            }
            else if (newProto1.HeightDelta.Value < 0)
              helper.SetPosition(helper.LastSetPosition.AddZ(newProto1.HeightDelta.Value - this.m_parent.m_lastHeightDelta.Value.Min(0)));
            else if (this.m_parent.m_lastHeightDelta.Value < 0)
              helper.SetPosition(helper.LastSetPosition.AddZ(-this.m_parent.m_lastHeightDelta.Value));
            if (this.m_parent.m_lastHeightDelta.Value * newProto1.HeightDelta.Value < 0)
              helper.SetRotation(helper.LastSetRotation + Rotation90.Deg180);
            EntityConfigData entityConfigData = new EntityConfigData(EntityId.Invalid, (IProto) newProto1, this.m_parent.m_configCloneHelper.ConfigContext);
            entityConfigData.Transform = new TileTransform?(helper.Transform);
            this.m_parent.m_lastHeightDelta = newProto1.HeightDelta;
            this.m_parent.m_entityPreviews[0] = Make.Kvp<IStaticEntityPreview, EntityConfigData>((IStaticEntityPreview) this.m_parent.m_layoutEntityPreviewManager.CreatePreview((ILayoutEntityProto) newProto1, this.m_parent.m_placementPhase, helper.Transform, enableMiniZipperPlacement: true), entityConfigData);
            this.m_parent.clearPortHighlights();
            this.m_parent.calculatePrice();
            if (this.m_parent.m_pricePopup.IsVisibleWithError())
              this.m_heightWarningPopup.Hide();
            else if (newProto1.HeightDelta.Abs.Value >= TransportPillarProto.MAX_PILLAR_HEIGHT.Value)
              this.m_heightWarningPopup.Show();
            else
              this.m_heightWarningPopup.Hide();
            return true;
          }
          if (num1 != 0)
          {
            if (flag)
              this.m_parent.m_toolbox.PlayUpSound(new bool?(false));
            else
              this.m_parent.m_toolbox.PlayDownSound(new bool?(false));
          }
        }
        if (!this.m_parent.m_shortcutsManager.IsPrimaryActionUp || EventSystem.current.IsPointerOverGameObject())
          return false;
        bool flag1 = this.m_parent.m_configApplyMode == StaticEntityMassPlacer.ApplyConfigMode.NeverApply || this.m_parent.m_configApplyMode == StaticEntityMassPlacer.ApplyConfigMode.ApplyIfNotDisabled && this.m_parent.m_shortcutsManager.IsOn(this.m_parent.m_shortcutsManager.CopyExcludingSettings);
        this.m_parent.m_entitiesToAdd.Clear();
        foreach (KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview in this.m_parent.m_entityPreviews)
        {
          IStaticEntityPreview key = entityPreview.Key;
          EntityValidationResult? validationResult = key.ValidationResult;
          if (validationResult.HasValue)
          {
            validationResult = key.ValidationResult;
            if (validationResult.Value.IsError)
              continue;
          }
          key.UpdateConfigWithTransform(entityPreview.Value);
          if (this.m_parent.m_placementPhase == EntityPlacementPhase.First)
          {
            this.m_parent.m_initialMousePosition = UnityEngine.Input.mousePosition;
            this.m_parent.m_placementPhase = EntityPlacementPhase.Final;
            this.m_parent.m_lastHeightDelta = liftProto.HeightDelta;
            this.m_parent.m_phase2RelativeHeight = this.m_parent.m_terrainCursor.RelativeHeight;
            this.m_parent.m_transportBindSound.Play();
            key.SetPlacementPhase(this.m_parent.m_placementPhase);
            return true;
          }
          this.m_parent.m_entitiesToAdd.Add(entityPreview.Value.CreateCopy());
        }
        if (this.m_parent.m_entitiesToAdd.IsNotEmpty)
          this.m_parent.m_ongoingCmds.Add((IInputCommand) inputScheduler.ScheduleInputCmd<BatchCreateStaticEntitiesCmd>(new BatchCreateStaticEntitiesCmd(this.m_parent.m_entitiesToAdd.ToImmutableArray(), applyConfiguration: !flag1)));
        this.m_parent.m_pricePopup.Hide();
        this.m_heightWarningPopup.Hide();
        return true;
      }

      private bool flip()
      {
        KeyValuePair<IStaticEntityPreview, EntityConfigData> entityPreview = this.m_parent.m_entityPreviews[0];
        IStaticEntityPreview key = entityPreview.Key;
        if (!entityPreview.Value.Transform.HasValue)
        {
          Log.Error("Non initial placement with null transform.");
          return false;
        }
        LayoutEntitySlotPlacerHelper helper = this.m_parent.m_helper;
        ILiftProto newProto;
        if (this.m_currentLiftProto.Value.TryGetHeightReversedProto(this.m_parent.m_protosDb, out newProto))
        {
          key.DestroyAndReturnToPool();
          this.m_isFlippedToOutput = newProto.HeightDelta.IsNegative;
          this.m_parent.m_entityPreviews[0] = Make.Kvp<IStaticEntityPreview, EntityConfigData>((IStaticEntityPreview) this.m_parent.m_layoutEntityPreviewManager.CreatePreview((ILayoutEntityProto) newProto, this.m_parent.m_placementPhase, helper.Transform, enableMiniZipperPlacement: true, disablePortPreviewsPredicate: (Predicate<IoPortTemplate>) (p => p.Type == (this.m_isFlippedToOutput ? IoPortType.Output : IoPortType.Input))), new EntityConfigData(EntityId.Invalid, (IProto) newProto, this.m_parent.m_configCloneHelper.ConfigContext)
          {
            Transform = new TileTransform?(helper.Transform)
          });
          this.m_parent.clearPortHighlights();
        }
        else
          Log.Error("Failed to get height flipped proto");
        return true;
      }

      static LiftPlacementHelper()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        StaticEntityMassPlacer.LiftPlacementHelper.SELECTION_OK = new ColorRgba(0, (int) byte.MaxValue, 0, 192);
        StaticEntityMassPlacer.LiftPlacementHelper.SELECTION_NOT_OK = new ColorRgba((int) byte.MaxValue, 0, 0, 192);
      }

      private readonly struct PickResult
      {
        public readonly Tile3i Position;
        public readonly Option<TransportProto> TransportProto;
        public readonly IoPortType PortType;
        public readonly Direction903d? Direction;

        public PickResult(
          Tile3i position,
          Option<TransportProto> transportProto = default (Option<TransportProto>),
          IoPortType portType = IoPortType.Any,
          Direction903d? direction = null)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Position = position;
          this.TransportProto = transportProto;
          this.PortType = portType;
          this.Direction = direction;
        }
      }
    }
  }
}
