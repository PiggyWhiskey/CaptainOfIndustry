// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.TransportPreviewVisualizer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Ports.Io;
using Mafi.Unity.Entities;
using Mafi.Unity.Factory.Transports;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  public class TransportPreviewVisualizer
  {
    private readonly TransportPreviewManager m_manager;
    private readonly Lyst<ObjectHighlightSpec> m_highlightedObjects;
    private readonly Lyst<EntityHighlightSpec> m_highlightedEntities;
    private readonly Lyst<PortPreview> m_shownPorts;
    private Option<GameObject> m_shownTransportOk;
    private Option<GameObject> m_shownTransportErr;
    private Option<GameObject> m_shownTransportHighlight;
    private Option<LayoutEntityPreview> m_miniZipperPreview1;
    private Option<LayoutEntityPreview> m_miniZipperPreview2;
    private ImmutableArray<PillarVisualsSpec> m_shownPillars1;
    private ImmutableArray<PillarVisualsSpec> m_shownPillars2;
    private PooledArray<RenderedPillarData> m_shownPillarsData1;
    private PooledArray<RenderedPillarData> m_shownPillarsData2;

    public TransportPreviewSpec PreviewSpec { get; private set; }

    internal TransportPreviewVisualizer(TransportPreviewManager previewManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_highlightedObjects = new Lyst<ObjectHighlightSpec>();
      this.m_highlightedEntities = new Lyst<EntityHighlightSpec>();
      this.m_shownPorts = new Lyst<PortPreview>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_manager = previewManager;
    }

    public void ShowPreview(TransportPreviewSpec previewSpec)
    {
      this.PreviewSpec = previewSpec;
      this.clearAllPreviewsAndHighlights(true);
      Assert.That<Option<GameObject>>(this.m_shownTransportOk).IsNone<GameObject>();
      Assert.That<Option<GameObject>>(this.m_shownTransportErr).IsNone<GameObject>();
      Assert.That<Option<GameObject>>(this.m_shownTransportHighlight).IsNone<GameObject>();
      ColorRgba? highlightColor = previewSpec.IsReadyToBuild ? new ColorRgba?(HighlightColors.OK_GREEN) : new ColorRgba?();
      showPillars(previewSpec.OkPillars, ref this.m_shownPillars1, ref this.m_shownPillarsData1);
      this.m_shownTransportOk = tryShowTransport(previewSpec.OkTraj, highlightColor, showStartPort: previewSpec.ShowStartPort, showEndPort: previewSpec.ShowEndPort);
      showPillars(previewSpec.ErrPillars, ref this.m_shownPillars2, ref this.m_shownPillarsData2);
      this.m_shownTransportErr = tryShowTransport(previewSpec.ErrTraj, new ColorRgba?(HighlightColors.ERROR_RED));
      this.m_shownTransportHighlight = tryShowTransport(previewSpec.HighlightTraj, new ColorRgba?(HighlightColors.OK_GREEN), true);
      tryShowMiniZipper(previewSpec.MiniZipAtStart, ref this.m_miniZipperPreview1);
      tryShowMiniZipper(previewSpec.MiniZipAtEnd, ref this.m_miniZipperPreview2);
      tryHighlightEntity(previewSpec.HighlightEntity1);

      void highlight(ObjectHighlightSpec hlSpec)
      {
        this.m_manager.ObjectHighlighter.Highlight(hlSpec);
        this.m_highlightedObjects.Add(hlSpec);
      }

      Option<GameObject> tryShowTransport(
        Option<TransportTrajectory> trajMaybe,
        ColorRgba? highlightColor,
        bool highlightOnly = false,
        bool showStartPort = false,
        bool showEndPort = false)
      {
        if (trajMaybe.IsNone)
          return Option<GameObject>.None;
        TransportTrajectory trajectory = trajMaybe.Value;
        GameObject model = this.m_manager.TransportModelFactory.CreateModel(trajectory, true, true, true);
        this.m_manager.EntityPreviewManager.SetCachedPreviewMaterialFor((IEntityProto) trajectory.TransportProto, model);
        if (highlightOnly)
          model.SetActive(false);
        if (highlightColor.HasValue)
          highlight(new ObjectHighlightSpec(model, highlightColor.Value));
        if (showStartPort)
        {
          PortPreview portPreviewPooled = this.m_manager.PortPreviewManager.GetPortPreviewPooled();
          portPreviewPooled.Initialize(trajectory.Pivots.First, trajectory.StartDirection.ToDirection903d(), trajectory.TransportProto.PortsShape, IoPortType.Input, Option<IoPortShapeProto>.None, false, true, true);
          this.m_shownPorts.Add(portPreviewPooled);
        }
        if (showEndPort)
        {
          PortPreview portPreviewPooled = this.m_manager.PortPreviewManager.GetPortPreviewPooled();
          portPreviewPooled.Initialize(trajectory.Pivots.Last, trajectory.EndDirection.ToDirection903d(), trajectory.TransportProto.PortsShape, IoPortType.Output, Option<IoPortShapeProto>.None, false, true, true);
          this.m_shownPorts.Add(portPreviewPooled);
        }
        return (Option<GameObject>) model;
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

      void tryShowMiniZipper(MiniZipperAtResult zipData, ref Option<LayoutEntityPreview> preview)
      {
        if (!zipData.IsValid)
        {
          if (!preview.HasValue)
            return;
          preview.Value.DestroyAndReturnToPool();
          preview = Option<LayoutEntityPreview>.None;
        }
        else
        {
          TileTransform transform = new TileTransform(zipData.Position);
          if (preview.IsNone)
            preview = this.m_manager.EntityPreviewManager.CreatePreview((ILayoutEntityProto) zipData.ZipperProto, EntityPlacementPhase.FirstAndFinal, transform, true, true).SomeOption<LayoutEntityPreview>();
          else if (preview.Value.EntityProto == zipData.ZipperProto)
          {
            preview.Value.SetTransform(transform);
          }
          else
          {
            preview.Value.DestroyAndReturnToPool();
            preview = this.m_manager.EntityPreviewManager.CreatePreview((ILayoutEntityProto) zipData.ZipperProto, EntityPlacementPhase.FirstAndFinal, transform, true, true).SomeOption<LayoutEntityPreview>();
          }
        }
      }

      void tryHighlightEntity(EntityHighlightSpec hlSpec)
      {
        if (!hlSpec.IsValid || hlSpec.Entity.IsDestroyed)
          return;
        this.m_highlightedEntities.Add(hlSpec);
        this.m_manager.EntityHighlighter.Highlight(hlSpec.Entity, hlSpec.Color);
      }
    }

    public void Clear()
    {
      this.clearAllPreviewsAndHighlights();
      this.PreviewSpec = new TransportPreviewSpec();
    }

    /// <summary>Removes the transport preview from the scene.</summary>
    private void clearAllPreviewsAndHighlights(bool keepLayoutEntityPreviews = false)
    {
      if (this.m_shownTransportOk.HasValue)
      {
        this.m_shownTransportOk.Value.Destroy();
        this.m_shownTransportOk = Option<GameObject>.None;
      }
      if (this.m_shownTransportErr.HasValue)
      {
        this.m_shownTransportErr.Value.Destroy();
        this.m_shownTransportErr = Option<GameObject>.None;
      }
      if (this.m_shownTransportHighlight.HasValue)
      {
        this.m_shownTransportHighlight.Value.Destroy();
        this.m_shownTransportHighlight = Option<GameObject>.None;
      }
      if (this.m_shownPillarsData1.IsValid)
      {
        this.m_manager.PillarsRenderer.RemovePillarVisualImmediate(ref this.m_shownPillarsData1);
        this.m_shownPillars1 = new ImmutableArray<PillarVisualsSpec>();
        this.m_shownPillarsData1 = new PooledArray<RenderedPillarData>();
      }
      if (this.m_shownPillarsData2.IsValid)
      {
        this.m_manager.PillarsRenderer.RemovePillarVisualImmediate(ref this.m_shownPillarsData2);
        this.m_shownPillars2 = new ImmutableArray<PillarVisualsSpec>();
        this.m_shownPillarsData2 = new PooledArray<RenderedPillarData>();
      }
      if (!keepLayoutEntityPreviews)
      {
        this.m_miniZipperPreview1.ValueOrNull?.DestroyAndReturnToPool();
        this.m_miniZipperPreview1 = Option<LayoutEntityPreview>.None;
        this.m_miniZipperPreview2.ValueOrNull?.DestroyAndReturnToPool();
        this.m_miniZipperPreview2 = Option<LayoutEntityPreview>.None;
      }
      foreach (EntityHighlightSpec highlightedEntity in this.m_highlightedEntities)
        this.m_manager.EntityHighlighter.RemoveHighlight(highlightedEntity.Entity);
      this.m_highlightedEntities.Clear();
      foreach (ObjectHighlightSpec highlightedObject in this.m_highlightedObjects)
        this.m_manager.ObjectHighlighter.RemoveHighlight(highlightedObject);
      this.m_highlightedObjects.Clear();
      this.m_shownPorts.ForEachAndClear((Action<PortPreview>) (x => x.DestroyAndReturnToPool()));
    }
  }
}
