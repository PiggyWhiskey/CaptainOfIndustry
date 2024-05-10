// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.LayoutEntityPreview
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
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Lifts;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.Factory.Transports;
using Mafi.Unity.InputControl.Inspectors.Buildings;
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using Mafi.Unity.Terrain;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>
  /// This class helps to display a preview any layout entity.
  /// </summary>
  public class LayoutEntityPreview : IStaticEntityPreviewDirect, IStaticEntityPreview
  {
    private readonly LayoutEntityPreviewManager m_manager;
    private GameObject m_entityPreviewGo;
    private ColorRgba? m_highlightColor;
    private readonly Lyst<PortPreview> m_portPreviews;
    private bool m_disableValidation;
    private bool m_disablePortPreviews;
    private bool m_disableMiniZipperPlacement;
    private EntityValidationResult? m_lastValidationResultOnSim;
    private volatile int m_currentVersion;
    private int m_simProcessedVersion;
    private GameObject m_directionArrow;
    private TileTransform m_initialTransform;
    private int m_validityDataElementsCountOnSim;
    private TileValidityInstanceData[] m_validityData;
    private TileValidityInstanceData[] m_validityDataOnSim;
    private Lyst<CanPlaceMiniZipperAtResult> m_zipperData;
    private Lyst<CanPlaceMiniZipperAtResult> m_zipperDataOnSim;
    private readonly Lyst<TerrainAreaRenderer> m_areaData;
    private readonly Lyst<LayoutEntityPreview.OceanAreaData> m_areaDataOnSim;
    private bool m_applyHighlight;
    private ImmutableArray<PillarVisualsSpec> m_shownPillars;
    private PooledArray<RenderedPillarData> m_shownPillarsData;
    private ImmutableArray<PillarVisualsSpec> m_pillarsVisualsSpec;
    private ImmutableArray<PillarVisualsSpec> m_pillarsVisualsSpecOnSim;
    private int m_pillarsVersion;
    private EntityPlacementPhase m_placementPhase;
    private Predicate<IoPortTemplate> m_disablePortPreviewsPredicate;
    private readonly Lyst<Option<LayoutEntityPreview>> m_zipperPreviews;

    public EntityValidationResult? ValidationResult { get; private set; }

    public bool ShowOceanAreas
    {
      get
      {
        return this.LayoutEntityProto.Layout.CombinedConstraint.HasAnyConstraints(LayoutTileConstraint.Ocean);
      }
    }

    public ILayoutEntityProto LayoutEntityProto { get; private set; }

    public IStaticEntityProto EntityProto => (IStaticEntityProto) this.LayoutEntityProto;

    public AssetValue Price => this.LayoutEntityProto.Costs.Price;

    public TileTransform Transform { get; private set; }

    public bool IsDestroyed => this.LayoutEntityProto == null;

    public bool ValidityDataWasUpdated { get; private set; }

    public int ValidityDataElementsCount { get; private set; }

    public LayoutEntityPreview(LayoutEntityPreviewManager manager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_portPreviews = new Lyst<PortPreview>();
      this.m_validityData = Array.Empty<TileValidityInstanceData>();
      this.m_validityDataOnSim = Array.Empty<TileValidityInstanceData>();
      this.m_zipperData = new Lyst<CanPlaceMiniZipperAtResult>();
      this.m_zipperDataOnSim = new Lyst<CanPlaceMiniZipperAtResult>();
      this.m_areaData = new Lyst<TerrainAreaRenderer>();
      this.m_areaDataOnSim = new Lyst<LayoutEntityPreview.OceanAreaData>();
      this.m_zipperPreviews = new Lyst<Option<LayoutEntityPreview>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_manager = manager;
    }

    internal void Initialize(
      ILayoutEntityProto entityProto,
      TileTransform transform,
      EntityPlacementPhase placementPhase,
      bool disableValidation,
      bool disablePortPreviews,
      bool disableMiniZipperPlacement,
      Predicate<IoPortTemplate> disablePortPreviewsPredicate)
    {
      if (!this.IsDestroyed)
      {
        Log.Warning("Initializing non-cleared layout entity preview.");
        this.clear();
      }
      this.m_initialTransform = transform;
      this.LayoutEntityProto = entityProto.CheckNotNull<ILayoutEntityProto>();
      this.m_disableValidation = disableValidation;
      this.m_disablePortPreviews = disablePortPreviews;
      this.m_disableMiniZipperPlacement = disableMiniZipperPlacement;
      this.m_disablePortPreviewsPredicate = disablePortPreviewsPredicate;
      this.m_placementPhase = placementPhase;
      this.m_entityPreviewGo = this.m_manager.Resolver.InvokeFactoryHierarchy<GameObject>((object) entityProto);
      this.m_manager.SetCachedPreviewMaterialFor((IEntityProto) entityProto, this.m_entityPreviewGo);
      DrawArrowWileBuildingProtoParam paramValue;
      if (entityProto.TryGetParam<DrawArrowWileBuildingProtoParam>(out paramValue))
      {
        this.m_directionArrow = this.m_manager.AssetsDb.GetClonedPrefabOrEmptyGo("Assets/Base/Buildings/PreviewArrow.prefab");
        this.m_directionArrow.transform.localRotation = Quaternion.AngleAxis(180f - paramValue.AngleDegrees, Vector3.up);
        this.m_directionArrow.transform.localScale = new Vector3(paramValue.Scale, paramValue.Scale, paramValue.Scale);
        this.m_directionArrow.transform.SetParent(this.m_entityPreviewGo.transform, false);
      }
      foreach (ParticleSystem componentsInChild in this.m_entityPreviewGo.GetComponentsInChildren<ParticleSystem>())
        componentsInChild.Stop();
      this.m_entityPreviewGo.DestroyAllCollidersImmediate();
      if (!this.m_disablePortPreviews)
      {
        foreach (IoPortTemplate port in entityProto.Ports)
        {
          if (this.m_disablePortPreviewsPredicate == null || this.m_disablePortPreviewsPredicate(port))
          {
            Option<IoPortShapeProto> canConnectToShapeViaMiniZipper = this.m_disableMiniZipperPlacement ? Option<IoPortShapeProto>.None : (Option<IoPortShapeProto>) port.Shape;
            this.m_portPreviews.Add(this.m_manager.PreviewManager.GetPortPreviewPooled().Initialize(port, entityProto, transform, canConnectToShapeViaMiniZipper, entityProto is LiftProto, false));
          }
        }
      }
      this.SetTransform(transform);
    }

    public void GetPlacementParams(
      out Tile3i basePosition,
      out bool canBeReflected,
      out ThicknessIRange allowedPlacementRange)
    {
      canBeReflected = !this.LayoutEntityProto.CannotBeReflected;
      if (this.LayoutEntityProto.Layout.PlacementHeightRange.Height.IsPositive)
      {
        HeightTilesI tilesHeightRounded = this.m_manager.TerrainManager[this.m_initialTransform.Position.Xy].Height.TilesHeightRounded;
        ThicknessTilesI range = this.LayoutEntityProto.Layout.PlacementHeightRange.ClampToRange(this.m_initialTransform.Position.Height - tilesHeightRounded);
        basePosition = this.m_initialTransform.Position.Xy.ExtendHeight(tilesHeightRounded);
        allowedPlacementRange = this.LayoutEntityProto.Layout.PlacementHeightRange.ShiftBy(-range);
      }
      else
      {
        basePosition = this.m_initialTransform.Position;
        allowedPlacementRange = new ThicknessIRange(0, 0);
      }
    }

    public ThicknessTilesI GetEstPlacementHeight()
    {
      return this.m_initialTransform.Position.Height - this.m_manager.TerrainManager[this.m_initialTransform.Position.Xy].Height.TilesHeightRounded;
    }

    public void GetPortTypes(Set<ShapeTypePair> portsSet)
    {
      foreach (IoPortTemplate port in this.LayoutEntityProto.Ports)
        portsSet.Add(new ShapeTypePair(port.Shape, port.Type));
    }

    public void GetPortLocations(Dict<IoPortKey, IoPortType> ports)
    {
      EntityLayout layout = this.LayoutEntityProto.Layout;
      foreach (IoPortTemplate port in this.LayoutEntityProto.Ports)
      {
        Tile3i position = layout.Transform(port.RelativePosition, this.Transform);
        Direction903d as3d = this.Transform.Transform(port.RelativeDirection).As3d;
        ports.AddAndAssertNew(new IoPortKey(position, as3d), port.Type);
      }
    }

    public void HideConnectedPorts(Dict<IoPortKey, IoPortType> ports)
    {
      if (this.m_portPreviews.IsEmpty)
        return;
      EntityLayout layout = this.LayoutEntityProto.Layout;
      int index = 0;
      while (true)
      {
        int num = index;
        ImmutableArray<IoPortTemplate> ports1 = this.LayoutEntityProto.Ports;
        int length = ports1.Length;
        if (num < length)
        {
          ports1 = this.LayoutEntityProto.Ports;
          IoPortTemplate ioPortTemplate = ports1[index];
          Tile3i tile3i = layout.Transform(ioPortTemplate.RelativePosition, this.Transform);
          Direction903d as3d = this.Transform.Transform(ioPortTemplate.RelativeDirection).As3d;
          IoPortKey key = new IoPortKey(tile3i + as3d.ToTileDirection(), -as3d);
          if (ports.TryGetValue(key, out IoPortType _))
          {
            this.m_portPreviews[index].DestroyAndReturnToPool();
            this.m_portPreviews[index] = (PortPreview) null;
          }
          ++index;
        }
        else
          break;
      }
      this.m_portPreviews.RemoveWhere((Predicate<PortPreview>) (x => x == null));
    }

    public bool CanMoveUpDownIfValid() => !(this.LayoutEntityProto is MiniZipperProto);

    public void UpdateConfigWithTransform(EntityConfigData data)
    {
      data.Transform = new TileTransform?(this.Transform);
    }

    public void DestroyAndReturnToPool() => this.clear();

    private void clear()
    {
      if (this.LayoutEntityProto == null)
      {
        Assert.That<Lyst<PortPreview>>(this.m_portPreviews).IsEmpty<PortPreview>();
        Assert.That<GameObject>(this.m_entityPreviewGo).IsNull<GameObject>();
        Assert.That<Lyst<Option<LayoutEntityPreview>>>(this.m_zipperPreviews).IsEmpty<Option<LayoutEntityPreview>>();
      }
      else
      {
        this.clearPillars();
        this.unhighlight();
        foreach (Option<LayoutEntityPreview> zipperPreview in this.m_zipperPreviews)
          zipperPreview.ValueOrNull?.DestroyAndReturnToPool();
        this.m_zipperData.Clear();
        this.m_zipperPreviews.Clear();
        if (this.m_portPreviews.IsNotEmpty)
        {
          foreach (PortPreview portPreview in this.m_portPreviews)
            portPreview.DestroyAndReturnToPool();
          this.m_portPreviews.Clear();
        }
        if (this.m_areaData.IsNotEmpty)
        {
          foreach (TerrainAreaRenderer terrainAreaRenderer in this.m_areaData)
            terrainAreaRenderer.Destroy();
          this.m_areaData.Clear();
        }
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_entityPreviewGo);
        this.m_entityPreviewGo = (GameObject) null;
        this.LayoutEntityProto = (ILayoutEntityProto) null;
        this.Transform = new TileTransform();
        this.ValidationResult = new EntityValidationResult?();
        this.m_lastValidationResultOnSim = new EntityValidationResult?();
        this.m_currentVersion = 0;
        this.m_simProcessedVersion = 0;
        this.ValidityDataElementsCount = 0;
        this.ValidityDataWasUpdated = true;
      }
    }

    public void ApplyTransformDelta(
      RelTile3i deltaPosition,
      Rotation90 deltaRotation,
      bool deltaReflection,
      Tile2i pivot)
    {
      TileTransform transform = this.Transform;
      Rotation90 rotation = transform.Rotation + deltaRotation;
      bool isReflected = transform.IsReflected ^ deltaReflection;
      if (deltaReflection && rotation.Is90Or270Deg)
        rotation += Rotation90.Deg180;
      RelTile2i relTile2i = this.LayoutEntityProto.Layout.TransformRelative(RelTile2i.Zero, transform);
      RelTile2i v = transform.Position.Xy + relTile2i - pivot;
      Matrix2i m = Matrix2i.FromRotationFlip(deltaRotation, deltaReflection);
      this.SetTransform(new TileTransform((pivot + m.Transform(v) - this.LayoutEntityProto.Layout.TransformRelative(RelTile2i.Zero, new TileTransform(Tile3i.Zero, rotation, isReflected))).ExtendZ(transform.Position.Z) + deltaPosition, rotation, isReflected));
    }

    public void SetTransform(TileTransform transform)
    {
      if (this.IsDestroyed)
      {
        Log.Error("Setting transform on destroyed entity preview.");
      }
      else
      {
        Assert.That<GameObject>(this.m_entityPreviewGo).IsNotNull<GameObject>("Entity preview is not initialized.");
        if (this.Transform == transform)
          return;
        this.Transform = transform;
        this.applyTransform(transform);
        if (this.ValidityDataElementsCount != 0)
        {
          this.ValidityDataWasUpdated = true;
          this.ValidityDataElementsCount = 0;
        }
        ++this.m_currentVersion;
      }
    }

    private void applyTransform(TileTransform transform)
    {
      this.clearPillars();
      EntityLayout layout = this.LayoutEntityProto.Layout;
      StaticEntityTransform transformData = StaticEntityMb.GetTransformData(transform, this.LayoutEntityProto.Graphics.PrefabOrigin, layout);
      this.m_entityPreviewGo.transform.position = transformData.Position;
      this.m_entityPreviewGo.transform.localRotation = transformData.Rotation;
      this.m_entityPreviewGo.transform.localScale = transformData.LocalScale;
      if (!this.m_portPreviews.IsNotEmpty)
        return;
      foreach (PortPreview portPreview in this.m_portPreviews)
        portPreview.SetTransform(transform);
    }

    private void clearPillars()
    {
      if (!this.m_shownPillarsData.IsValid)
        return;
      this.m_manager.PillarsRenderer.RemovePillarVisualImmediate(ref this.m_shownPillarsData);
      this.m_shownPillars = new ImmutableArray<PillarVisualsSpec>();
      this.m_shownPillarsData = new PooledArray<RenderedPillarData>();
    }

    public void SetPlacementPhase(EntityPlacementPhase placementPhase)
    {
      this.m_placementPhase = placementPhase;
    }

    internal void RenderUpdate()
    {
      if (this.m_applyHighlight && this.ValidationResult.HasValue)
      {
        this.m_applyHighlight = false;
        this.highlight(this.ValidationResult.Value.IsSuccess ? ColorRgba.Green : ColorRgba.Red);
      }
      if (this.IsDestroyed)
      {
        Assert.That<Lyst<Option<LayoutEntityPreview>>>(this.m_zipperPreviews).IsEmpty<Option<LayoutEntityPreview>>();
      }
      else
      {
        if (this.LayoutEntityProto is ILayoutEntityProtoWithElevation layoutEntityProto && layoutEntityProto.CanBeElevated)
        {
          this.clearPillars();
          if (this.m_pillarsVersion == this.m_currentVersion)
            showPillars(this.m_pillarsVisualsSpec, ref this.m_shownPillars, ref this.m_shownPillarsData);
        }
        int index;
        for (index = 0; index < this.m_zipperData.Count; ++index)
        {
          CanPlaceMiniZipperAtResult zipData = this.m_zipperData[index];
          while (index >= this.m_zipperPreviews.Count)
          {
            this.m_zipperPreviews.Add(Option<LayoutEntityPreview>.None);
            Assert.That<int>(this.m_zipperPreviews.Count).IsEqualTo(index + 1);
          }
          Option<LayoutEntityPreview> zipperPreview = this.m_zipperPreviews[index];
          tryShowMiniZipper(zipData, ref zipperPreview);
          this.m_zipperPreviews[index] = zipperPreview;
        }
        for (; index < this.m_zipperPreviews.Count; ++index)
        {
          Option<LayoutEntityPreview> zipperPreview = this.m_zipperPreviews[index];
          if (zipperPreview.HasValue)
          {
            zipperPreview.Value.DestroyAndReturnToPool();
            this.m_zipperPreviews[index] = Option<LayoutEntityPreview>.None;
          }
        }
      }

      void showPillars(
        ImmutableArray<PillarVisualsSpec> pillars,
        ref ImmutableArray<PillarVisualsSpec> cache,
        ref PooledArray<RenderedPillarData> renderedData)
      {
        if (pillars.IsNotValid || pillars.IsEmpty)
        {
          cache = new ImmutableArray<PillarVisualsSpec>();
        }
        else
        {
          if (!(pillars != cache))
            return;
          cache = pillars;
          Assert.That<bool>(renderedData.IsValid).IsFalse();
          renderedData = this.m_manager.PillarsRenderer.AddPillarVisualImmediate(pillars);
        }
      }

      void tryShowMiniZipper(
        CanPlaceMiniZipperAtResult zipData,
        ref Option<LayoutEntityPreview> preview)
      {
        TileTransform transform = new TileTransform(zipData.CutOutResult.CutOutPosition);
        if (preview.IsNone)
          preview = this.m_manager.CreatePreview((ILayoutEntityProto) zipData.ZipperProto, EntityPlacementPhase.FirstAndFinal, transform, true, true).SomeOption<LayoutEntityPreview>();
        else if (preview.Value.EntityProto == zipData.ZipperProto)
        {
          preview.Value.SetTransform(transform);
        }
        else
        {
          preview.Value.DestroyAndReturnToPool();
          preview = this.m_manager.CreatePreview((ILayoutEntityProto) zipData.ZipperProto, EntityPlacementPhase.FirstAndFinal, transform, true, true).SomeOption<LayoutEntityPreview>();
        }
      }
    }

    internal void SimUpdate()
    {
      int currentVersion = this.m_currentVersion;
      TileTransform transform = this.Transform;
      ILayoutEntityProto layoutEntityProto = this.LayoutEntityProto;
      if (this.m_simProcessedVersion == currentVersion)
        return;
      this.m_simProcessedVersion = currentVersion;
      if (layoutEntityProto == null || this.m_disableValidation)
        return;
      LayoutEntityAddRequest requestFor = this.m_manager.LayoutEntityAddRequestFactory.CreateRequestFor<ILayoutEntityProto>(layoutEntityProto, new EntityAddRequestData(transform, this.m_disableMiniZipperPlacement, recordTileErrors: true));
      requestFor.SetPlacementPhase(this.m_placementPhase);
      EntityValidationResult validationResult = this.m_manager.EntitiesManager.CanAdd((IEntityAddRequest) requestFor, true);
      this.m_lastValidationResultOnSim = new EntityValidationResult?(validationResult);
      ReadOnlyArray<OccupiedTileRelative> occupiedTiles = requestFor.OccupiedTiles;
      if (this.m_validityDataOnSim.Length < occupiedTiles.Length)
        this.m_validityDataOnSim = new TileValidityInstanceData[occupiedTiles.Length + 8];
      Tile2i xy = requestFor.Transform.Position.Xy;
      if (validationResult.IsSuccess)
      {
        for (int index = 0; index < occupiedTiles.Length; ++index)
        {
          OccupiedTileRelative occupiedTileRelative = occupiedTiles[index];
          Tile2i tile2i = xy + occupiedTileRelative.RelCoord;
          this.m_validityDataOnSim[index] = new TileValidityInstanceData(tile2i.X, tile2i.Y, encodeColor(occupiedTileRelative.Constraint), (byte) 1);
        }
      }
      else
      {
        for (int index = 0; index < occupiedTiles.Length; ++index)
        {
          OccupiedTileRelative occupiedTileRelative = occupiedTiles[index];
          Tile2i tile2i = xy + occupiedTileRelative.RelCoord;
          this.m_validityDataOnSim[index] = new TileValidityInstanceData(tile2i.X, tile2i.Y, encodeColor(occupiedTileRelative.Constraint), (byte) !requestFor.GetTileError(index));
        }
      }
      if (layoutEntityProto is ILayoutEntityProtoWithElevation protoWithElevation && protoWithElevation.CanBeElevated)
        this.m_pillarsVisualsSpecOnSim = this.m_manager.CreatePillarsVisualSpec(requestFor);
      if (!this.m_disableMiniZipperPlacement && !(layoutEntityProto is MiniZipperProto))
      {
        this.m_zipperDataOnSim.Clear();
        foreach (IoPortTemplate port in layoutEntityProto.Ports)
        {
          Tile3i connectedPortPosition = port.GetExpectedConnectedPortPosition(layoutEntityProto.Layout, transform);
          Mafi.Core.Factory.Transports.Transport entity;
          CanPlaceMiniZipperAtResult result;
          if (this.m_manager.TerrainOccupancyManager.TryGetOccupyingEntityAt<Mafi.Core.Factory.Transports.Transport>(connectedPortPosition, out entity) && !((Proto) entity.Prototype.PortsShape != (Proto) port.Shape) && (!(entity.EndOutputPort.Position == connectedPortPosition) || !entity.EndOutputPort.IsNotConnected) && (!(entity.StartInputPort.Position == connectedPortPosition) || !entity.StartInputPort.IsNotConnected) && (!(entity.EndOutputPort.Position == connectedPortPosition) || !entity.EndOutputPort.IsNotConnected || port.Type == IoPortType.Input) && (!(entity.StartInputPort.Position == connectedPortPosition) || !entity.StartInputPort.IsNotConnected || port.Type == IoPortType.Output) && this.m_manager.TransportsConstructionHelper.CanPlaceMiniZipperAt(entity, connectedPortPosition, out result, out LocStrFormatted _))
            this.m_zipperDataOnSim.Add(result);
        }
      }
      this.m_validityDataElementsCountOnSim = occupiedTiles.Length;
      if (requestFor.HasAdditionalErrorTiles)
      {
        Lyst<Tile2i> errorTilesStorage = requestFor.GetAdditionalErrorTilesStorage();
        int elementsCountOnSim = this.m_validityDataElementsCountOnSim;
        this.m_validityDataElementsCountOnSim += errorTilesStorage.Count;
        if (this.m_validityDataOnSim.Length < this.m_validityDataElementsCountOnSim)
          Array.Resize<TileValidityInstanceData>(ref this.m_validityDataOnSim, this.m_validityDataElementsCountOnSim + 8);
        foreach (Tile2i tile2i in errorTilesStorage)
        {
          this.m_validityDataOnSim[elementsCountOnSim] = new TileValidityInstanceData(tile2i.X, tile2i.Y, new ColorRgba(64, 64, 64, (int) byte.MaxValue), (byte) 0);
          ++elementsCountOnSim;
        }
      }
      this.m_areaDataOnSim.Clear();
      if (requestFor.Metadata.HasValue)
      {
        foreach (IAddRequestMetadata addRequestMetadata in requestFor.Metadata.Value)
        {
          if (addRequestMetadata is OceanAreaValidationMetadata validationMetadata)
            this.m_areaDataOnSim.Add(new LayoutEntityPreview.OceanAreaData(validationMetadata.Area, validationMetadata.Color.ToColor(), requestFor.Transform.Position.Height.HeightTilesF, validationMetadata.StripesScale, validationMetadata.StripesAngle));
        }
      }
      requestFor.ReturnToPool();

      static ColorRgba encodeColor(LayoutTileConstraint constraint)
      {
        if (constraint.HasAnyConstraints(LayoutTileConstraint.Ocean))
          return ColorRgba.Blue;
        return constraint.HasAnyConstraints(LayoutTileConstraint.Ground) ? new ColorRgba(146, 128, 0, (int) byte.MaxValue) : new ColorRgba(64, 64, 64, (int) byte.MaxValue);
      }
    }

    internal void SyncUpdate()
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse("Syncing destroyed layout entity preview.");
      if (!this.m_lastValidationResultOnSim.HasValue)
        return;
      if (this.m_simProcessedVersion == this.m_currentVersion)
      {
        this.ValidationResult = this.m_lastValidationResultOnSim;
        this.ValidityDataWasUpdated = true;
        this.m_lastValidationResultOnSim = new EntityValidationResult?();
        this.m_applyHighlight = true;
        this.ValidityDataElementsCount = this.m_validityDataElementsCountOnSim;
        Swap.Them<TileValidityInstanceData[]>(ref this.m_validityData, ref this.m_validityDataOnSim);
        this.m_pillarsVersion = this.m_currentVersion;
        Swap.Them<ImmutableArray<PillarVisualsSpec>>(ref this.m_pillarsVisualsSpec, ref this.m_pillarsVisualsSpecOnSim);
      }
      if (this.m_areaDataOnSim.IsEmpty)
      {
        if (this.m_areaData.IsNotEmpty && this.m_areaData.First.IsShown)
        {
          foreach (TerrainAreaRenderer terrainAreaRenderer in this.m_areaData)
            terrainAreaRenderer.Hide();
        }
      }
      else
      {
        int index;
        for (index = 0; index < this.m_areaDataOnSim.Count; ++index)
        {
          TerrainAreaRenderer terrainAreaRenderer;
          if (index < this.m_areaData.Count)
          {
            terrainAreaRenderer = this.m_areaData[index];
          }
          else
          {
            terrainAreaRenderer = new TerrainAreaRenderer(this.m_manager.AssetsDb);
            this.m_areaData.Add(terrainAreaRenderer);
          }
          LayoutEntityPreview.OceanAreaData oceanAreaData = this.m_areaDataOnSim[index];
          ThicknessTilesF thicknessTilesF = new ThicknessTilesF((UnityEngine.Random.value * 0.05f).ToFix32());
          terrainAreaRenderer.ShowArea(oceanAreaData.Area, oceanAreaData.Color, OceanAreasOverlayRenderer.OCEAN_AREA_HEIGHT + thicknessTilesF, oceanAreaData.StripesScale, oceanAreaData.StripesAngle);
        }
        for (; index < this.m_areaData.Count; ++index)
          this.m_areaData[index].Hide();
        this.m_areaDataOnSim.Clear();
      }
      this.m_zipperData.Clear();
      if (!this.IsDestroyed)
        this.m_zipperData.AddRange(this.m_zipperDataOnSim);
      else
        this.m_zipperDataOnSim.Clear();
    }

    private void highlight(ColorRgba color)
    {
      ColorRgba? highlightColor = this.m_highlightColor;
      ColorRgba colorRgba = color;
      if ((highlightColor.HasValue ? (highlightColor.GetValueOrDefault() == colorRgba ? 1 : 0) : 0) != 0)
        return;
      this.unhighlight();
      this.m_manager.Highlighter.Highlight(this.m_entityPreviewGo, color);
      this.m_highlightColor = new ColorRgba?(color);
    }

    private void unhighlight()
    {
      if (!this.m_highlightColor.HasValue)
        return;
      this.m_manager.Highlighter.RemoveHighlight(this.m_entityPreviewGo, this.m_highlightColor.Value);
      this.m_highlightColor = new ColorRgba?();
    }

    internal KeyValuePair<TileValidityInstanceData[], int> GetValidityDataAndMarkUpdated()
    {
      this.ValidityDataWasUpdated = false;
      return new KeyValuePair<TileValidityInstanceData[], int>(this.m_validityData, this.ValidityDataElementsCount);
    }

    private readonly struct OceanAreaData
    {
      public readonly RectangleTerrainArea2i Area;
      public readonly Color Color;
      public readonly HeightTilesF Height;
      public readonly float StripesScale;
      public readonly float StripesAngle;

      public OceanAreaData(
        RectangleTerrainArea2i area,
        Color color,
        HeightTilesF height,
        float stripesScale,
        float stripesAngle)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Area = area;
        this.Color = color;
        this.Height = height;
        this.StripesScale = stripesScale;
        this.StripesAngle = stripesAngle;
      }
    }
  }
}
