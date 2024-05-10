// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Maintenance;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <remarks>
  /// Important note about transport throughput. We can affect throughput by three values: Transport speed,
  /// product spacing, and stack quantity. However, speed is fixed to spacing since speed has to divide spacing
  /// without remainder. This gives us following relationship:
  /// 
  /// <c>Th = Spd / Spc * Stack</c>, where Th is throughput, Spd is speed/tick, Spc is spacing, and Stack is stacking.
  /// 
  /// But <c>Spc = Spd * w</c>, where w must be positive integer, so we have
  /// 
  /// <c>Th = Spd / (Spd * w) * Stack</c>, which is
  /// <c>Th = Stack / w</c>
  /// 
  /// This means tha for a selected value of stacking, only some throughput values are possible, regardless of
  /// speed or spacing. Table below shows possible values of throughput per 10 ticks (1 second):
  /// <code>
  /// Throughput / sec (10 ticks)
  /// |     w |   3   |   4   |   5   |   6   |   7   |   8   |   9   |  10   |
  /// | Stack +-------+-------+-------+-------+-------+-------+-------+-------+
  /// |     1 | 3.333 |  2.5  |   2   | 1.666 | 1.428 |  1.25 | 1.111 |   1   |
  /// |     2 | 6.666 |   5   |   4   | 3.333 | 2.857 |  2.5  | 2.222 |   2   |
  /// |     3 |  10   |  7.5  |   6   |   5   | 4.285 |  3.75 | 3.333 |   3   |
  /// |     4 | 13.33 |  10   |   8   | 6.666 | 5.714 |   5   | 4.444 |   4   |
  /// </code>
  /// To get speed and spacing for a chosen throughput, chose stacking row, in that row choose w with desired
  /// throughput. Then, choose spacing and speed is simply <c>Spd = w / Spc</c>.
  /// </remarks>
  public class TransportProto : 
    StaticEntityProto,
    IProtoWithUpgrade<TransportProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithIcon
  {
    /// <summary>
    /// Maximum terrain height that can penetrate into a transport and still be considered valid.
    /// </summary>
    public static readonly ThicknessTilesF MAX_TERRAIN_PENETRATION;
    /// <summary>
    /// Surface height of the transport relative to the tile. Transported products will be on this relative height.
    /// </summary>
    public readonly ThicknessTilesF SurfaceRelativeHeight;
    /// <summary>Maximum stacking quantity per transported product.</summary>
    public readonly Quantity MaxQuantityPerTransportedProduct;
    /// <summary>Spacing of transported products.</summary>
    public readonly RelTile1f TransportedProductsSpacing;
    /// <summary>Distance traveled per sim tick.</summary>
    public readonly RelTile1f SpeedPerTick;
    /// <summary>Amount of transported products per tick.</summary>
    public readonly PartialQuantity ThroughputPerTick;
    /// <summary>
    /// Spacing of transported products in the number of waypoints.
    /// </summary>
    public readonly int ProductSpacingWaypoints;
    /// <summary>Spacing of transported products.</summary>
    public readonly RelTile1f ProductSpacing;
    /// <summary>
    /// Number of straight tiles required for changing height by one. Value 1 means max 45 degrees up/down, 2 means
    /// 30 degrees, etc. Value <see cref="P:Mafi.RelTile1i.MaxValue" /> represents the transport cannot go up/down.
    /// </summary>
    public readonly RelTile1i ZStepLength;
    /// <summary>
    /// Maximum radius of pillars support. Each tile of transport has to be at most at this distance from a pivot.
    /// </summary>
    public readonly RelTile1i MaxPillarSupportRadius;
    /// <summary>Whether this transport needs pillars at ground level.</summary>
    public readonly bool NeedsPillarsAtGround;
    /// <summary>Whether transport collapses when buried under ground.</summary>
    public readonly bool CanBeBuried;
    /// <summary>
    /// If set, this transport will have this tile surface floor when it is on the ground.
    /// </summary>
    public readonly Option<TerrainTileSurfaceProto> TileSurfaceWhenOnGround;
    /// <summary>
    /// Shape of ports of this transport. Only transports with the same port can be connected.
    /// </summary>
    public readonly IoPortShapeProto PortsShape;
    /// <summary>
    /// Electricity consumed by the transport per ~450 tiles (given current formula).
    /// </summary>
    public readonly Electricity BaseElectricityCost;
    /// <summary>Sharpness of corners in percent.</summary>
    public readonly Percent CornersSharpnessPercent;
    /// <summary>Whether this transport is buildable by the player.</summary>
    public readonly bool IsBuildable;
    /// <summary>Length of transport per given cost.</summary>
    public readonly RelTile1i LengthPerCost;
    /// <summary>
    /// Whether this transport allows different products at the same time. If this is set to false, all products on
    /// the transport has to be identical.
    /// </summary>
    public readonly bool AllowMixedProducts;
    public readonly VirtualProductProto MaintenanceProduct;
    public readonly Quantity MaintenancePerTile;
    /// <summary>
    /// Graphics-only properties that does not affect game simulation and are not needed or accessed by the game
    /// simulation.
    /// </summary>
    public readonly TransportProto.Gfx Graphics;

    public override Type EntityType => typeof (Transport);

    public UpgradeData<TransportProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public string IconPath => this.Graphics.IconPath;

    /// <summary>
    /// Whether this transport can go up/down. Note that this depends on <see cref="F:Mafi.Core.Factory.Transports.TransportProto.ZStepLength" />,
    /// </summary>
    public bool CanGoUpDown => this.ZStepLength != RelTile1i.MaxValue;

    /// <summary>Whether this transport needs pillar supports.</summary>
    public bool NeedsPillars => this.MaxPillarSupportRadius != RelTile1i.MaxValue;

    public TransportProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      ThicknessTilesF surfaceRelativeHeight,
      Quantity maxQuantityPerTransportedProduct,
      RelTile1f transportedProductsSpacing,
      RelTile1f speedPerTick,
      RelTile1i zStepLength,
      bool needsPillarsAtGround,
      bool canBeBuried,
      Option<TerrainTileSurfaceProto> tileSurfaceWhenOnGround,
      RelTile1i maxPillarSupportRadius,
      IoPortShapeProto portsShape,
      Electricity baseElectricityCost,
      Percent cornersSharpnessPercent,
      bool allowMixedProducts,
      bool isBuildable,
      EntityCosts costs,
      RelTile1i lengthPerCost,
      Duration constructionDurationPerProduct,
      Option<TransportProto> nextTier,
      VirtualProductProto maintenanceProduct,
      Quantity maintenancePerTile,
      TransportProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, costs, constructionDurationPerProduct, new ThicknessIRange?(), (StaticEntityProto.Gfx) graphics);
      if (speedPerTick.IsNotPositive)
        throw new InvalidProtoException(string.Format("Speed per tick of transport '{0}' is not positive: {1}", (object) id, (object) speedPerTick));
      if (maxQuantityPerTransportedProduct.IsNotPositive)
        throw new InvalidProtoException(string.Format("Max quantity per transported product of transport '{0}' ", (object) id) + string.Format("is not positive: {0}", (object) maxQuantityPerTransportedProduct));
      if (maxQuantityPerTransportedProduct > IoPort.MAX_TRANSFER_PER_TICK)
        throw new InvalidProtoException(string.Format("Max quantity per product of transport '{0}' ", (object) id) + string.Format("cannot be larger than {0}", (object) IoPort.MAX_TRANSFER_PER_TICK));
      if (transportedProductsSpacing.IsNotPositive)
        throw new InvalidProtoException(string.Format("Transported product size of transport '{0}' not positive: {1}", (object) id, (object) transportedProductsSpacing));
      this.SurfaceRelativeHeight = surfaceRelativeHeight.CheckWithinExcl(ThicknessTilesF.Zero, ThicknessTilesF.One);
      this.SpeedPerTick = speedPerTick.CheckPositive();
      this.MaxQuantityPerTransportedProduct = maxQuantityPerTransportedProduct;
      this.TransportedProductsSpacing = transportedProductsSpacing;
      Fix32 fix32 = this.TransportedProductsSpacing.Value / speedPerTick.Value;
      this.ProductSpacingWaypoints = fix32.ToIntRounded();
      this.ThroughputPerTick = maxQuantityPerTransportedProduct.AsPartial / this.ProductSpacingWaypoints;
      if (!fix32.IsNear((Fix32) this.ProductSpacingWaypoints, 1.Percent()))
        throw new InvalidProtoException(string.Format("Transport '{0}' speed '{1}' must divide products ", (object) id, (object) speedPerTick) + string.Format("spacing '{0}' exactly ({1} vs {2} ", (object) this.TransportedProductsSpacing, (object) fix32, (object) this.ProductSpacingWaypoints) + string.Format("has delta {0}).", (object) (fix32 - (Fix32) this.ProductSpacingWaypoints).Abs()));
      this.ProductSpacing = this.ProductSpacingWaypoints * this.SpeedPerTick;
      this.ZStepLength = zStepLength.CheckNotNegative();
      this.NeedsPillarsAtGround = needsPillarsAtGround;
      this.CanBeBuried = canBeBuried;
      this.TileSurfaceWhenOnGround = tileSurfaceWhenOnGround;
      this.MaxPillarSupportRadius = maxPillarSupportRadius.CheckPositive();
      this.PortsShape = portsShape.CheckNotNull<IoPortShapeProto>();
      this.BaseElectricityCost = baseElectricityCost.CheckNotNegative();
      this.CornersSharpnessPercent = cornersSharpnessPercent.CheckWithinIncl(Percent.Zero, 200.Percent());
      this.AllowMixedProducts = allowMixedProducts;
      this.IsBuildable = isBuildable;
      this.LengthPerCost = lengthPerCost.CheckPositive();
      this.Upgrade = new UpgradeData<TransportProto>(this, nextTier);
      this.MaintenanceProduct = maintenanceProduct;
      this.MaintenancePerTile = maintenancePerTile;
      this.Graphics = graphics.CheckNotNull<TransportProto.Gfx>();
      this.Graphics.Initialize(this);
    }

    public AssetValue GetPriceFor(ImmutableArray<Tile3i> pivots)
    {
      return (this.Costs.Price * TransportProto.getLength(pivots)).CeilDiv(this.LengthPerCost.Value);
    }

    public Electricity GetElectricityConsumption(RelTile1f trajectoryLength)
    {
      return new Electricity((this.BaseElectricityCost.Value * trajectoryLength.Value.Pow(0.75.ToFix32()) / 100).ToIntCeiled());
    }

    public MaintenanceCosts GetMaintenanceCosts(RelTile1f trajectoryLength)
    {
      Assert.That<Quantity>(this.MaintenancePerTile).IsZero("TODO: Implement maintenance costs computation.");
      return new MaintenanceCosts(this.MaintenanceProduct, PartialQuantity.Zero);
    }

    public PartialQuantity GetMaxThroughputPerTickFor(ProductProto productProto)
    {
      return this.MaxQuantityPerTransportedProduct.Min(productProto.MaxQuantityPerTransportedProduct).AsPartial / this.ProductSpacingWaypoints;
    }

    private static int getLength(ImmutableArray<Tile3i> pivots)
    {
      int length = 1;
      Tile3i? nullable = new Tile3i?();
      foreach (Tile3i pivot in pivots)
      {
        if (nullable.HasValue)
        {
          int num = length;
          RelTile3i relTile3i = pivot - nullable.Value;
          relTile3i = relTile3i.AbsValue;
          int sum = relTile3i.Sum;
          length = num + sum;
        }
        nullable = new Tile3i?(pivot);
      }
      return length;
    }

    static TransportProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportProto.MAX_TERRAIN_PENETRATION = 0.2.TilesThick();
    }

    public new class Gfx : StaticEntityProto.Gfx
    {
      public static readonly TransportProto.Gfx Empty;
      /// <summary>
      /// Whether custom icon path was set. Otherwise, icon path is automatically generated.
      /// </summary>
      public readonly bool IconIsCustom;
      public readonly TransportCrossSection CrossSection;
      public readonly bool UsePerProductColoring;
      public readonly bool RenderProducts;
      /// <summary>
      /// Number of discrete samples per curved segment of this transports. More samples creates smoother curves
      /// but costs performance with model generation and drawing. This value is intentionally separate from the
      /// simulation because transports that do not show products may be simpler in sim but nice detailed in gfx.
      /// </summary>
      public readonly int SamplesPerCurvedSegment;
      /// <summary>Material of this transport.</summary>
      public readonly string MaterialPath;
      /// <summary>
      /// How long transport will be on the texture. This affect how much of the texture will be used for the
      /// transport because the same resolution of pixels is kept for both vertical and horizontal dimensions.
      /// </summary>
      public readonly RelTile1f TransportUvLength;
      /// <summary>Whether transported products should be rendered.</summary>
      public readonly bool RenderTransportedProducts;
      /// <summary>Sound that is played when the transport is built.</summary>
      public readonly string SoundOnBuildPrefabPath;
      /// <summary>
      /// Optional prefab of the flow indicator that will be placed on each segment.
      /// </summary>
      public readonly Option<TransportProto.Gfx.FlowIndicatorSpec> FlowIndicator;
      /// <summary>
      /// Optional prefab of a connector that will be placed on transition to vertical.
      /// </summary>
      public readonly Option<string> VerticalConnectorPrefabPath;
      /// <summary>
      /// Pillar attachments. Not all types need to be set. Same attachment prefab can be used for more types.
      /// </summary>
      public readonly IReadOnlyDictionary<TransportPillarAttachmentType, string> PillarAttachments;
      /// <summary>Y UV shift for the texture.</summary>
      public readonly float UvShiftY;
      /// <summary>Scale the cross section normally when rendering.</summary>
      public readonly Percent CrossSectionScale;
      /// <summary>
      /// The radius before the scale is applied. Only used when CrossSectionScale is not 100%.
      /// </summary>
      public readonly float CrossSectionRadius;
      public readonly bool UseInstancedRendering;
      public readonly int MaxRenderedLod;
      /// <summary>
      /// Instanced rendering data that can be shared with other protos, or be unique to this proto.
      /// </summary>
      public readonly Option<TransportProto.Gfx.TransportInstancedRenderingData> InstancedRenderingData;

      /// <summary>Path for icon sprite.</summary>
      /// <remarks>This path is valid only after <see cref="M:Mafi.Core.Factory.Transports.TransportProto.Gfx.Initialize(Mafi.Core.Factory.Transports.TransportProto)" /> was called.</remarks>
      public string IconPath { get; private set; }

      public Gfx(
        TransportCrossSection crossSection,
        bool renderProducts,
        int samplesPerCurvedSegment,
        string materialPath,
        RelTile1f transportUvLength,
        bool renderTransportedProducts,
        string soundOnBuildPrefabPath,
        Option<TransportProto.Gfx.FlowIndicatorSpec> flowIndicator,
        Option<string> verticalConnectorPrefabPath,
        IReadOnlyDictionary<TransportPillarAttachmentType, string> pillarAttachments,
        float uvShiftY,
        Percent crossSectionScale,
        float crossSectionRadius,
        Option<TransportProto.Gfx.TransportInstancedRenderingData> instancedRenderingData,
        bool usePerProductColoring = false,
        Option<string> customIconPath = default (Option<string>),
        bool useInstancedRendering = true,
        int maxRenderedLod = 2147483647)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty, false);
        this.CrossSection = crossSection;
        this.RenderProducts = renderProducts;
        this.SamplesPerCurvedSegment = samplesPerCurvedSegment.CheckWithinIncl(1, 16);
        this.MaterialPath = materialPath.CheckNotNullOrEmpty();
        this.TransportUvLength = transportUvLength.CheckPositive();
        this.RenderTransportedProducts = renderTransportedProducts;
        this.SoundOnBuildPrefabPath = soundOnBuildPrefabPath.CheckNotNullOrEmpty();
        this.FlowIndicator = flowIndicator;
        this.VerticalConnectorPrefabPath = verticalConnectorPrefabPath;
        this.PillarAttachments = pillarAttachments.CheckNotNull<IReadOnlyDictionary<TransportPillarAttachmentType, string>>();
        this.IconPath = customIconPath.ValueOrNull;
        this.IconIsCustom = customIconPath.HasValue;
        this.UvShiftY = uvShiftY;
        this.CrossSectionScale = crossSectionScale.CheckPositive();
        this.CrossSectionRadius = crossSectionRadius.CheckPositive();
        this.UseInstancedRendering = useInstancedRendering;
        if (useInstancedRendering && instancedRenderingData.IsNone)
          throw new InvalidProtoException("Transport using instanced rendering must have instanced rendering data class.");
        this.InstancedRenderingData = instancedRenderingData;
        this.UsePerProductColoring = usePerProductColoring;
        this.MaxRenderedLod = maxRenderedLod;
      }

      internal void Initialize(TransportProto proto)
      {
        Assert.That<TransportProto.Gfx>(proto.Graphics).IsEqualTo<TransportProto.Gfx>(this);
        if (this.IconIsCustom)
          return;
        Assert.That<string>(this.IconPath).IsNull<string>();
        this.IconPath = string.Format("{0}/Transport/{1}.png", (object) "Assets/Unity/Generated/Icons", (object) proto.Id);
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TransportProto.Gfx.Empty = new TransportProto.Gfx(TransportCrossSection.Empty, false, 1, "EMPTY", new RelTile1f(Fix32.One), true, "EMPTY", Option<TransportProto.Gfx.FlowIndicatorSpec>.None, Option<string>.None, (IReadOnlyDictionary<TransportPillarAttachmentType, string>) new Dict<TransportPillarAttachmentType, string>(), 0.0f, Percent.Hundred, 1f, (Option<TransportProto.Gfx.TransportInstancedRenderingData>) Option.None, customIconPath: (Option<string>) "EMPTY", useInstancedRendering: false);
      }

      public class TransportInstancedRenderingData
      {
        /// <summary>
        /// This is used solely by the implementation of IEntitiesRenderer that renders this entity.
        /// </summary>
        public ushort InstancedRendererIndex;

        public TransportInstancedRenderingData()
        {
          MBiHIp97M4MqqbtZOh.rMWAw2OR8();
          // ISSUE: explicit constructor call
          base.\u002Ector();
        }
      }

      public class FlowIndicatorSpec
      {
        public static readonly Fix32 BIAS_TOWARD_ENDS;
        public readonly string FramePrefabPath;
        /// <summary>Prefab path of flow indicator model.</summary>
        public readonly string FlowPrefabPath;
        /// <summary>Prefab path of glass indicator model.</summary>
        public readonly string GlassPrefabPath;
        /// <summary>
        /// How long space should be skipped in the actual transport model for the flow indicator. Zero means no
        /// skipping.
        /// </summary>
        public readonly RelTile1f SkipTransportLength;
        /// <summary>
        /// On long straight sections, this is the maximum gap between consecutive indicators.
        /// </summary>
        public readonly RelTile1f PlacementGap;
        public readonly FluidIndicatorGfxParams Parameters;

        public FlowIndicatorSpec(
          string framePrefabPath,
          string flowPrefabPath,
          string glassPrefabPath,
          RelTile1f skipTransportLength,
          RelTile1f placementGap,
          FluidIndicatorGfxParams parameters)
        {
          MBiHIp97M4MqqbtZOh.rMWAw2OR8();
          // ISSUE: explicit constructor call
          base.\u002Ector();
          this.Parameters = parameters;
          this.FramePrefabPath = framePrefabPath.CheckNotNullOrEmpty();
          this.FlowPrefabPath = flowPrefabPath.CheckNotNullOrEmpty();
          this.GlassPrefabPath = glassPrefabPath.CheckNotNullOrEmpty();
          this.SkipTransportLength = skipTransportLength.CheckNotNegative();
          this.PlacementGap = placementGap.CheckNotNegative();
          Assert.That<RelTile1f>(placementGap).IsGreater(this.SkipTransportLength);
        }

        static FlowIndicatorSpec()
        {
          MBiHIp97M4MqqbtZOh.rMWAw2OR8();
          TransportProto.Gfx.FlowIndicatorSpec.BIAS_TOWARD_ENDS = 0.3.ToFix32();
        }
      }
    }
  }
}
