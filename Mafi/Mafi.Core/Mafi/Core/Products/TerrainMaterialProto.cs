// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.TerrainMaterialProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  /// <summary>Material that is on terrain.</summary>
  public class TerrainMaterialProto : 
    Proto,
    IProtoWithSlimID<TerrainMaterialSlimId>,
    IProtoWithPropertiesUpdate,
    IProto
  {
    public const string SUFFIX = "_Terrain";
    public static readonly PartialQuantity MINED_QUANTITY_PER_TILE_CUBED;
    public static readonly Proto.ID PHANTOM_ID;
    public static readonly TerrainMaterialProto PhantomTerrainMaterialProto;
    /// <summary>
    /// Product that is obtained when this terrain product is mined.
    /// </summary>
    public readonly LooseProductProto MinedProduct;
    /// <summary>
    /// A multiplier for mined quantity. This multiplies the actual mined quantity when removing or dumping
    /// material from terrain. This one is set per proto and does not include the global multiplier
    /// </summary>
    public readonly Percent MinedQuantityMult;
    public readonly Proto.ID WeatheredMaterialId;
    public readonly Proto.ID DisruptedMaterialId;
    public readonly Proto.ID RecoveredMaterialId;
    /// <summary>
    /// Whether this material can recover under water. For example, dirt does not, but landfill does.
    /// </summary>
    public readonly bool RecoversUnderWater;
    /// <summary>
    /// Approximate time for this material to become undisrupted.
    /// </summary>
    public readonly Duration DisruptionRecoveryTime;
    /// <summary>Controls the speed of disruption.</summary>
    public readonly Percent DisruptionSpeedMult;
    /// <summary>
    /// Whether this material should be disrupted when collapsing. For example, grass has this set to true,
    /// but rock not.
    /// </summary>
    public readonly bool DisruptWhenCollapsing;
    /// <summary>
    /// Minimum height difference between neighboring tiles at which this material can collapse.
    /// </summary>
    /// <remarks>
    /// The probability of terrain collapsing is 0% at <see cref="F:Mafi.Core.Products.TerrainMaterialProto.MinCollapseHeightDiff" /> (or lower) and 100% at
    /// <see cref="F:Mafi.Core.Products.TerrainMaterialProto.MaxCollapseHeightDiff" /> (and higher). The probability is linear interpolated between. Terrain
    /// tile is considered for collapsing only when its height or height of its neighbors is changed.
    /// </remarks>
    public readonly ThicknessTilesF MinCollapseHeightDiff;
    /// <summary>
    /// Maximum height difference neighboring tiles at which this material can say without collapsing. Terrain always
    /// collapses if the height difference is greater than this value.
    /// </summary>
    /// <remarks>To make the material less likely to collapse set this value to high value (like 10-100).</remarks>
    public readonly ThicknessTilesF MaxCollapseHeightDiff;
    /// <summary>
    /// Whether crops can grow on this material. Used for farms.
    /// </summary>
    public readonly bool IsFarmable;
    /// <summary>
    /// We can convert it to forest floor. Most farmable materials should be convertible.
    /// </summary>
    public readonly bool CanBeConvertedToForestFloor;
    /// <summary>
    /// Whether this material can spread to nearby surfaces when grass growth is not possible.
    /// </summary>
    public readonly bool CanSpreadToNearbyMaterials;
    /// <summary>
    /// Whether this material should be ignored in the map editor in the list of available materials for placement.
    /// </summary>
    public readonly bool IgnoreInEditor;
    public readonly Percent GrassGrowthOnTop;
    public readonly TerrainMaterialProto.Gfx Graphics;
    private TerrainMaterialSlimId m_slimId;
    /// <summary>Whether this is a forest floor material.</summary>
    public readonly bool IsForestFloor;

    public PartialQuantity MinedQuantityPerTileCubed { get; private set; }

    /// <summary>
    /// Material that is the same as this one but exposed to elements for a long time (e.g. oxidized rock).
    /// </summary>
    public Option<TerrainMaterialProto> WeatheredMaterialProto { get; private set; }

    /// <summary>
    /// Replaces this material after disruption (mining, vehicles driving, construction, etc.).
    /// </summary>
    public Option<TerrainMaterialProto> DisruptedMaterialProto { get; private set; }

    /// <summary>
    /// Material that is replaced when recovering disruption (like grass from dirt).
    /// </summary>
    public Option<TerrainMaterialProto> RecoveredMaterialProto { get; private set; }

    /// <summary>Slim ID of this material.</summary>
    public TerrainMaterialSlimId SlimId => this.m_slimId;

    public TerrainMaterialProto(
      Proto.ID id,
      string nameInEditor,
      LooseProductProto minedProduct,
      Percent minedQuantityMult,
      ThicknessTilesF minCollapseHeightDiff,
      ThicknessTilesF maxCollapseHeightDiff,
      TerrainMaterialProto.Gfx graphics,
      bool isFarmable = false,
      bool canBeConvertedToForestFloor = false,
      bool isForestFloor = false,
      bool ignoreInEditor = false,
      bool canSpreadToNearbyMaterials = false,
      Proto.ID? weathersInto = null,
      Proto.ID? disruptsInto = null,
      Percent? disruptionSpeedMult = null,
      bool disruptWhenCollapsing = false,
      Proto.ID? recoversInto = null,
      bool recoversUnderWater = false,
      Duration recoveryTime = default (Duration),
      Percent grassGrowthOnTop = default (Percent))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.CreateStr(id, nameInEditor, translationComment: "HIDE"));
      if (recoversInto.HasValue != recoveryTime.IsPositive)
        throw new InvalidProtoException(string.Format("Proto '{0}' recover status ({1}) does mot match ", (object) this, (object) recoveryTime.IsPositive) + string.Format("existence of recovered material ({0}).", (object) disruptsInto));
      if (disruptWhenCollapsing && !disruptsInto.HasValue)
        throw new InvalidProtoException(string.Format("Proto '{0}' should be disrupted after collapsing but it does not have disrupted material set.", (object) this));
      if (recoversUnderWater && !recoversInto.HasValue)
        throw new InvalidProtoException(string.Format("Proto '{0}' is set to recover under water but the recovery material was not set.", (object) this));
      if (this.IsPhantom)
      {
        Assert.That<bool>(minedProduct.IsPhantom).IsTrue(string.Format("Mined product of phantom should be phantom but it is: {0}.", (object) minedProduct));
        this.MinedProduct = minedProduct.CheckNotNull<LooseProductProto>();
      }
      else
        this.MinedProduct = minedProduct.CheckNotNullOrPhantom<LooseProductProto>();
      Proto.ID? nullable = weathersInto;
      this.WeatheredMaterialId = nullable ?? TerrainMaterialProto.PHANTOM_ID;
      nullable = disruptsInto;
      this.DisruptedMaterialId = nullable ?? TerrainMaterialProto.PHANTOM_ID;
      this.DisruptionSpeedMult = (disruptionSpeedMult ?? Percent.Hundred).CheckPositive();
      this.DisruptWhenCollapsing = disruptWhenCollapsing;
      nullable = recoversInto;
      this.RecoveredMaterialId = nullable ?? TerrainMaterialProto.PHANTOM_ID;
      this.RecoversUnderWater = recoversUnderWater;
      this.MinedQuantityMult = minedQuantityMult.CheckPositive();
      this.MinedQuantityPerTileCubed = TerrainMaterialProto.MINED_QUANTITY_PER_TILE_CUBED.ScaledBy(this.MinedQuantityMult);
      this.CanSpreadToNearbyMaterials = canSpreadToNearbyMaterials;
      this.IsForestFloor = isForestFloor;
      this.MinCollapseHeightDiff = minCollapseHeightDiff.CheckPositive();
      this.MaxCollapseHeightDiff = maxCollapseHeightDiff.CheckWithinIncl(minCollapseHeightDiff, ThicknessTilesF.MaxValue);
      this.IgnoreInEditor = ignoreInEditor;
      this.DisruptionRecoveryTime = recoveryTime;
      this.IsFarmable = isFarmable;
      this.CanBeConvertedToForestFloor = canBeConvertedToForestFloor;
      this.GrassGrowthOnTop = grassGrowthOnTop;
      this.Graphics = graphics.CheckNotNull<TerrainMaterialProto.Gfx>();
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      TerrainMaterialProto proto;
      if (protosDb.TryGetProto<TerrainMaterialProto>(this.WeatheredMaterialId, out proto) && proto.IsNotPhantom)
      {
        if ((Proto) proto == (Proto) this)
          Log.Warning(string.Format("Weathered material of {0} is set to self, ignoring.", (object) this));
        else
          this.WeatheredMaterialProto = (Option<TerrainMaterialProto>) proto;
      }
      if (protosDb.TryGetProto<TerrainMaterialProto>(this.DisruptedMaterialId, out proto) && proto.IsNotPhantom)
      {
        if ((Proto) proto == (Proto) this)
          Log.Warning(string.Format("Weathered material of {0} is set to self, ignoring.", (object) this));
        else
          this.DisruptedMaterialProto = (Option<TerrainMaterialProto>) proto;
      }
      if (protosDb.TryGetProto<TerrainMaterialProto>(this.RecoveredMaterialId, out proto) && proto.IsNotPhantom)
      {
        if ((Proto) proto == (Proto) this)
          Log.Warning(string.Format("Weathered material of {0} is set to self, ignoring.", (object) this));
        else
          this.RecoveredMaterialProto = (Option<TerrainMaterialProto>) proto;
      }
      protosDb.TrackProperty((IProtoWithPropertiesUpdate) this, IdsCore.PropertyIds.MiningMultiplier.Value);
    }

    public void OnPropertyUpdated(IProperty property)
    {
      Percent percent;
      if (!property.TryGetValueAs<Percent>(IdsCore.PropertyIds.MiningMultiplier, out percent))
        return;
      this.MinedQuantityPerTileCubed = TerrainMaterialProto.MINED_QUANTITY_PER_TILE_CUBED.ScaledBy(this.MinedQuantityMult * percent);
    }

    void IProtoWithSlimID<TerrainMaterialSlimId>.SetSlimId(TerrainMaterialSlimId id)
    {
      if (this.m_slimId.Value != (byte) 0 && this.m_slimId != id)
        throw new InvalidOperationException(string.Format("Slim ID of '{0}' was already set to '{1}'.", (object) this, (object) this.m_slimId));
      this.m_slimId = id;
    }

    public ThicknessTilesF QuantityToThickness(Quantity quantity)
    {
      return this.QuantityToThickness(quantity.AsPartial);
    }

    public ThicknessTilesF QuantityToThickness(PartialQuantity quantity)
    {
      Percent scale = Percent.FromRatio(quantity.Value, this.MinedQuantityPerTileCubed.Value);
      return ThicknessTilesF.One.ScaledBy(scale);
    }

    public PartialQuantity ThicknessToQuantity(ThicknessTilesF thickness)
    {
      return this.MinedQuantityPerTileCubed.ScaledBy(Percent.FromFix32(thickness.Value));
    }

    public Quantity ThicknessToQuantityRounded(ThicknessTilesF thickness)
    {
      return this.ThicknessToQuantity(thickness).ToQuantityRounded();
    }

    static TerrainMaterialProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainMaterialProto.MINED_QUANTITY_PER_TILE_CUBED = new PartialQuantity(2);
      TerrainMaterialProto.PHANTOM_ID = new Proto.ID("__PHANTOM__TERRAIN_MATERIAL__");
      TerrainMaterialProto.PhantomTerrainMaterialProto = Proto.RegisterPhantom<TerrainMaterialProto>(new TerrainMaterialProto(TerrainMaterialProto.PHANTOM_ID, "Phantom", LooseProductProto.Phantom, Percent.Hundred, 0.5.TilesThick(), 5.0.TilesThick(), TerrainMaterialProto.Gfx.Empty, ignoreInEditor: true));
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly TerrainMaterialProto.Gfx Empty;
      /// <summary>
      /// Color of the material for UI. This color has always alpha equal to 255.
      /// </summary>
      public ColorUniversal Color;
      /// <summary>
      /// Particle color for effects. This color has always alpha equal to 255.
      /// </summary>
      public ColorUniversal ParticleColor;
      /// <summary>
      /// Texture with albedo in RGB channels and height in A channel.
      /// </summary>
      public readonly string AlbedoHeightTexturePath;
      /// <summary>
      /// Texture with normal RG in RG channels, smoothness in B, and ambient occlusion in A channel.
      /// </summary>
      public readonly string NormalSaoTexturePath;
      public readonly ImmutableArray<DetailLayerSpec> DetailLayers;
      public readonly float Dustiness;
      public readonly ColorUniversal DustColor;
      /// <summary>Smoothness delta of this material when fully wet.</summary>
      public readonly float FullyWetSmoothnessDelta;
      /// <summary>Brightness delta of this material when fully wet.</summary>
      public readonly float FullyWetBrightnessDelta;

      public Gfx(
        ColorRgba color,
        ColorRgba particleColor,
        string albedoHeightTexturePath,
        string normalSaoTexturePath,
        float dustiness = 0.0f,
        float fullyWetSmoothnessDelta = 0.2f,
        float fullyWetBrightnessDelta = -0.2f,
        ColorRgba dustColor = default (ColorRgba),
        ImmutableArray<DetailLayerSpec> detailLayers = default (ImmutableArray<DetailLayerSpec>))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Color = (ColorUniversal) color.SetA(byte.MaxValue);
        this.ParticleColor = (ColorUniversal) particleColor.SetA(byte.MaxValue);
        this.DetailLayers = detailLayers.IsValid ? detailLayers : ImmutableArray<DetailLayerSpec>.Empty;
        this.AlbedoHeightTexturePath = albedoHeightTexturePath;
        this.NormalSaoTexturePath = normalSaoTexturePath;
        this.Dustiness = dustiness;
        this.FullyWetSmoothnessDelta = fullyWetSmoothnessDelta.CheckWithinIncl(0.0f, 1f);
        this.FullyWetBrightnessDelta = fullyWetBrightnessDelta.CheckWithinIncl(-1f, 1f);
        this.DustColor = (ColorUniversal) dustColor;
      }

      public TerrainMaterialProto.Gfx WithReplaced(
        string albedoHeightTexturePath = null,
        string normalSaoTexturePath = null,
        float? dustiness = null,
        float? fullyWetSmoothnessDelta = null,
        float? fullyWetBrightnessDelta = null,
        ColorRgba? dustColor = null,
        ImmutableArray<DetailLayerSpec> detailLayers = default (ImmutableArray<DetailLayerSpec>))
      {
        ColorRgba rgba1 = this.Color.Rgba;
        ColorRgba rgba2 = this.ParticleColor.Rgba;
        string albedoHeightTexturePath1 = albedoHeightTexturePath ?? this.AlbedoHeightTexturePath;
        string normalSaoTexturePath1 = normalSaoTexturePath ?? this.NormalSaoTexturePath;
        float? nullable = dustiness;
        double dustiness1 = (double) nullable ?? (double) this.Dustiness;
        nullable = fullyWetSmoothnessDelta;
        double fullyWetSmoothnessDelta1 = (double) nullable ?? (double) this.FullyWetSmoothnessDelta;
        nullable = fullyWetBrightnessDelta;
        double fullyWetBrightnessDelta1 = (double) nullable ?? (double) this.FullyWetBrightnessDelta;
        ColorRgba dustColor1 = dustColor ?? this.DustColor.Rgba;
        ImmutableArray<DetailLayerSpec> detailLayers1 = detailLayers.IsValid ? detailLayers : this.DetailLayers;
        return new TerrainMaterialProto.Gfx(rgba1, rgba2, albedoHeightTexturePath1, normalSaoTexturePath1, (float) dustiness1, (float) fullyWetSmoothnessDelta1, (float) fullyWetBrightnessDelta1, dustColor1, detailLayers1);
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TerrainMaterialProto.Gfx.Empty = new TerrainMaterialProto.Gfx(ColorRgba.Magenta, ColorRgba.Magenta, "EMPTY", "EMPTY");
      }
    }
  }
}
