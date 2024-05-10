// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.PortPreview
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.Utils;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  public class PortPreview
  {
    public static readonly ColorRgba BLUEPRINT_COLOR;
    private readonly PortPreviewManager m_portPreviewManager;
    private readonly IoPortsManager m_portsManager;
    private readonly IoPortsRenderer m_portsRenderer;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly TransportsConstructionHelper m_transportsConstructionHelper;
    private readonly ITransportsPredicates m_transportsPredicates;
    private readonly PortProductsResolver m_portProductResolver;
    private Tile3i m_position;
    private Direction903d m_direction;
    private IoPortType m_type;
    private bool m_canOnlyConnectToTransports;
    private bool m_ownerIsTransport;
    private Option<IoPortTemplate> m_portTemplate;
    private Option<ILayoutEntityProto> m_entityProto;
    private int m_currentVersion;
    private int m_simProcessedVersion;
    private PortPreview.PortConnStatus? m_portConnStatusOnSim;
    private MultiIconSpec? m_defaultIconSpec;
    private PortHighlightData? m_shownHighlightData;
    private Option<IoPort> m_portWithHiddenHighlights;
    private uint m_shownPortId;
    private Option<IoPortShapeProto> m_canConnectToShapeViaMiniZipper;

    public Option<IoPortShapeProto> ShapeProto { get; private set; }

    internal PortPreview(
      PortPreviewManager portPreviewManager,
      IoPortsManager portsManager,
      IoPortsRenderer portsRenderer,
      TerrainOccupancyManager occupancyManager,
      TransportsConstructionHelper transportsConstructionHelper,
      ITransportsPredicates transportsPredicates,
      PortProductsResolver portProductResolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_portPreviewManager = portPreviewManager;
      this.m_portsManager = portsManager;
      this.m_portsRenderer = portsRenderer;
      this.m_occupancyManager = occupancyManager;
      this.m_transportsConstructionHelper = transportsConstructionHelper;
      this.m_transportsPredicates = transportsPredicates;
      this.m_portProductResolver = portProductResolver;
    }

    public PortPreview Initialize(
      IoPortTemplate portTemplate,
      ILayoutEntityProto entityProto,
      TileTransform entityTransform,
      Option<IoPortShapeProto> canConnectToShapeViaMiniZipper,
      bool ownerIsTransport,
      bool isEndPort)
    {
      PortPreview portPreview = this.Initialize(entityProto.Layout.Transform(portTemplate.RelativePosition, entityTransform), entityTransform.Transform(portTemplate.RelativeDirection).As3d, portTemplate.Shape, portTemplate.Type, canConnectToShapeViaMiniZipper, portTemplate.Spec.CanOnlyConnectToTransports, ownerIsTransport, isEndPort);
      this.m_portTemplate = (Option<IoPortTemplate>) portTemplate;
      this.m_entityProto = Option<ILayoutEntityProto>.Create(entityProto);
      return portPreview;
    }

    public PortPreview Initialize(
      Tile3i position,
      Direction903d direction,
      IoPortShapeProto shape,
      IoPortType type,
      Option<IoPortShapeProto> canConnectToShapeViaMiniZipper,
      bool canOnlyConnectToTransports,
      bool ownerIsTransport,
      bool isEndPort)
    {
      Assert.That<Option<IoPortShapeProto>>(this.ShapeProto).IsNone<IoPortShapeProto>();
      this.ShapeProto = (Option<IoPortShapeProto>) shape;
      this.m_entityProto = Option<ILayoutEntityProto>.None;
      this.m_portTemplate = Option<IoPortTemplate>.None;
      this.m_type = type;
      this.m_canConnectToShapeViaMiniZipper = canConnectToShapeViaMiniZipper;
      this.m_canOnlyConnectToTransports = canOnlyConnectToTransports;
      this.m_ownerIsTransport = ownerIsTransport;
      this.m_simProcessedVersion = 0;
      this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?();
      this.m_defaultIconSpec = new MultiIconSpec?();
      this.m_position = position;
      this.m_direction = direction;
      ++this.m_currentVersion;
      this.m_shownPortId = this.m_portsRenderer.ShowPortImmediate(new IoPortVisual(position, direction, shape, isEndPort, PortPreview.BLUEPRINT_COLOR));
      return this;
    }

    internal void SimUpdate()
    {
      if (!this.m_defaultIconSpec.HasValue && this.m_entityProto.HasValue && this.m_portTemplate.HasValue)
        this.m_defaultIconSpec = new MultiIconSpec?(new MultiIconSpec(this.m_portProductResolver.GetPortProducts((IStaticEntityProto) this.m_entityProto.Value, this.m_portTemplate.Value.Spec).Map<string>((Func<ProductProto, string>) (x => x.Graphics.IconPath))));
      Option<IoPortShapeProto> shapeProto = this.ShapeProto;
      if (shapeProto.IsNone || this.m_simProcessedVersion == this.m_currentVersion)
        return;
      this.m_simProcessedVersion = this.m_currentVersion;
      if (this.m_occupancyManager.TerrainManager.IsOffLimitsOrInvalid(this.m_position.Xy))
      {
        this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?(new PortPreview.PortConnStatus(Option<IoPort>.None, false, false));
      }
      else
      {
        Tile3i tile3i = this.m_position + this.m_direction.ToTileDirection();
        Option<IoPort> neighborPort = this.m_portsManager[tile3i, -this.m_direction];
        if (neighborPort.HasValue)
        {
          bool canConnect = neighborPort.Value.CanConnectTo(this.m_position, this.m_direction, shapeProto.Value, this.m_type, this.m_ownerIsTransport, this.m_canOnlyConnectToTransports);
          this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?(new PortPreview.PortConnStatus(neighborPort, canConnect, false));
        }
        else
        {
          Mafi.Core.Factory.Transports.Transport entity;
          if (this.m_canConnectToShapeViaMiniZipper.HasValue && this.m_occupancyManager.TryGetOccupyingEntityAt<Mafi.Core.Factory.Transports.Transport>(tile3i, out entity) && entity.Prototype.PortsShape == this.ShapeProto)
          {
            if (!this.m_transportsConstructionHelper.CanPlaceMiniZipperAt(entity, tile3i, out CanPlaceMiniZipperAtResult _, out LocStrFormatted _))
            {
              bool isBlocked = this.m_occupancyManager.IsOccupiedAt(tile3i, this.m_transportsPredicates.IgnorePillarsPredicate);
              this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?(new PortPreview.PortConnStatus(Option<IoPort>.None, false, isBlocked));
            }
            else if (entity.EndOutputPort.Position == tile3i && entity.EndOutputPort.IsNotConnected && this.m_type != IoPortType.Input || entity.StartInputPort.Position == tile3i && entity.StartInputPort.IsNotConnected && this.m_type != IoPortType.Output)
              this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?(new PortPreview.PortConnStatus(Option<IoPort>.None, false, true));
            else
              this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?(new PortPreview.PortConnStatus(Option<IoPort>.None, true, false));
          }
          else
          {
            bool isBlocked = this.m_occupancyManager.IsOccupiedAt(tile3i, this.m_transportsPredicates.IgnorePillarsPredicate);
            this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?(new PortPreview.PortConnStatus(Option<IoPort>.None, false, isBlocked));
          }
        }
      }
    }

    internal void SyncUpdate()
    {
      if (!this.m_portConnStatusOnSim.HasValue)
        return;
      PortPreview.PortConnStatus portConnStatus = this.m_portConnStatusOnSim.Value;
      this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?();
      this.clearCurrentHighlight();
      PortHighlightSpec newHighlightSpec;
      if (portConnStatus.NeighborPort.HasValue)
      {
        this.m_portWithHiddenHighlights = portConnStatus.NeighborPort;
        this.m_portsRenderer.PauseHighlightsFor(portConnStatus.NeighborPort.Value, true);
        PortHighlightSpec portHighlightSpec;
        if (!portConnStatus.CanConnect)
        {
          ColorRgba? highlightColor = new ColorRgba?(IoPortsRenderer.HIGHLIGHT_ERR);
          MultiIconSpec? nullable = new MultiIconSpec?(IoPortsRenderer.ICON_PORT_CAN_NOT_CONNECT);
          ArrowSpec? arrowSpec = new ArrowSpec?();
          MultiIconSpec? iconSpec = nullable;
          portHighlightSpec = new PortHighlightSpec(highlightColor, arrowSpec, iconSpec);
        }
        else
        {
          ColorRgba? highlightColor = new ColorRgba?(IoPortsRenderer.HIGHLIGHT_OK);
          MultiIconSpec? nullable = new MultiIconSpec?(IoPortsRenderer.ICON_PORT_CAN_CONNECT);
          ArrowSpec? arrowSpec = new ArrowSpec?();
          MultiIconSpec? iconSpec = nullable;
          portHighlightSpec = new PortHighlightSpec(highlightColor, arrowSpec, iconSpec);
        }
        newHighlightSpec = portHighlightSpec;
      }
      else
      {
        PortHighlightSpec portHighlightSpec;
        if (!portConnStatus.CanConnect)
        {
          if (!portConnStatus.IsBlocked)
          {
            portHighlightSpec = new PortHighlightSpec(arrowSpec: new ArrowSpec?(new ArrowSpec(this.m_type)), iconSpec: this.m_defaultIconSpec);
          }
          else
          {
            ColorRgba? highlightColor = new ColorRgba?(IoPortsRenderer.HIGHLIGHT_ERR);
            MultiIconSpec? nullable = new MultiIconSpec?(IoPortsRenderer.ICON_PORT_BLOCKED);
            ArrowSpec? arrowSpec = new ArrowSpec?();
            MultiIconSpec? iconSpec = nullable;
            portHighlightSpec = new PortHighlightSpec(highlightColor, arrowSpec, iconSpec);
          }
        }
        else
        {
          ColorRgba? highlightColor = new ColorRgba?(IoPortsRenderer.HIGHLIGHT_OK);
          MultiIconSpec? nullable = new MultiIconSpec?(IoPortsRenderer.ICON_PORT_CAN_CONNECT);
          ArrowSpec? arrowSpec = new ArrowSpec?();
          MultiIconSpec? iconSpec = nullable;
          portHighlightSpec = new PortHighlightSpec(highlightColor, arrowSpec, iconSpec);
        }
        newHighlightSpec = portHighlightSpec;
      }
      this.m_shownHighlightData = new PortHighlightData?(this.m_portsRenderer.HighlightPortVisual(this.m_shownPortId, newHighlightSpec));
    }

    private void clearCurrentHighlight()
    {
      if (this.m_portWithHiddenHighlights.HasValue)
      {
        this.m_portsRenderer.RestoreHighlightsFor(this.m_portWithHiddenHighlights.Value);
        this.m_portWithHiddenHighlights = Option<IoPort>.None;
      }
      if (!this.m_shownHighlightData.HasValue)
        return;
      this.m_portsRenderer.RemovePortVisualHighlight(this.m_shownHighlightData.Value);
      this.m_shownHighlightData = new PortHighlightData?();
    }

    public void DestroyAndReturnToPool()
    {
      if (this.ShapeProto.IsNone)
        return;
      this.clearCurrentHighlight();
      this.m_portsRenderer.HidePortImmediate(this.m_shownPortId);
      this.m_shownPortId = 0U;
      this.ShapeProto = Option<IoPortShapeProto>.None;
      this.m_currentVersion = 0;
      this.m_simProcessedVersion = 0;
      this.m_portConnStatusOnSim = new PortPreview.PortConnStatus?();
      this.m_defaultIconSpec = new MultiIconSpec?();
      this.m_portPreviewManager.ReturnToPool(this);
    }

    public void SetTransform(Tile3i position, Direction903d direction)
    {
      if (this.m_shownPortId == 0U)
      {
        Log.Warning("Invalid port to set transform to.");
      }
      else
      {
        if (this.m_position == position && this.m_direction == direction)
          return;
        this.m_portsRenderer.UpdatePortImmediate(this.m_shownPortId, position, direction, PortPreview.BLUEPRINT_COLOR);
        if (this.m_shownHighlightData.HasValue)
          this.m_portsRenderer.UpdatePortVisualHighlight(this.m_shownHighlightData.Value, position, direction);
        this.m_position = position;
        this.m_direction = direction;
        ++this.m_currentVersion;
      }
    }

    public void SetTransform(TileTransform entityTransform)
    {
      if (this.m_entityProto.IsNone)
        Log.Error("Setting preview transform with TileTransform works only for ports initialized with IoPortTemplate and EntityLayout.");
      else
        this.SetTransform(this.m_entityProto.Value.Layout.Transform(this.m_portTemplate.Value.RelativePosition, entityTransform), entityTransform.Transform(this.m_portTemplate.Value.RelativeDirection).As3d);
    }

    static PortPreview()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      PortPreview.BLUEPRINT_COLOR = (ColorRgba) 2158427980U;
    }

    private readonly struct PortConnStatus
    {
      public readonly Option<IoPort> NeighborPort;
      public readonly bool CanConnect;
      public readonly bool IsBlocked;

      public PortConnStatus(Option<IoPort> neighborPort, bool canConnect, bool isBlocked)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.NeighborPort = neighborPort;
        this.CanConnect = canConnect;
        this.IsBlocked = isBlocked;
      }
    }
  }
}
