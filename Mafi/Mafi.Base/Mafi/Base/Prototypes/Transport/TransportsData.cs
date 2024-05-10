// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Transport.TransportsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.Factory.Lifts;
using Mafi.Core.Factory.Sorters;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Gfx;
using Mafi.Core.Maintenance;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Transport
{
  /// <summary>
  /// Data in this class are based on 'Transport speed and throughput' sheet:
  /// https://docs.google.com/spreadsheets/d/15sNKHriVlNTiUOYw6VRSKcKbZBsRDE5R3eySpUyM-Eo/edit?usp=sharing
  /// </summary>
  public class TransportsData : IModData
  {
    /// <summary>
    /// This parameter makes the corners nearly exact circles. Other values look ugly.
    /// </summary>
    public static readonly Percent CIRCLE_SHARPNESS;
    /// <summary>Number of segments per curve for all transports.</summary>
    public const int SEGMENTS_COUNT = 5;
    /// <summary>Range of pillar (radius), same for all transports.</summary>
    public const int PILLAR_RANGE = 4;
    /// <summary>
    /// Height a lift can lift products. Same for all valid transports.
    /// </summary>
    public const int MAX_LIFT_HEIGHT_DELTA = 6;
    /// <summary>
    /// Length of transport across the texture. Transport texture will repeat after this distance.
    /// </summary>
    private static readonly RelTile1f TRANSPORT_UV_LENGTH;
    private static readonly Duration CONSTR_DUR_PER_PRODUCT;
    private static readonly Proto.Str MINI_ZIP_TITLE;
    private static readonly LocStr LIFT_LOOSE_NAME;
    private static readonly LocStr LIFT_LOOSE_DESC;
    private static readonly LocStr LIFT_FLAT_NAME;
    private static readonly LocStr LIFT_FLAT_DESC;

    public void RegisterData(ProtoRegistrator registrator)
    {
      this.createPillar(registrator);
      this.createFlatConveyor(registrator);
      this.createLooseMaterialConveyor(registrator);
      this.createMoltenMetalChannel(registrator);
      this.createPipe(registrator);
      this.createShaft(registrator);
    }

    private void createPillar(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<TransportPillarProto>(new TransportPillarProto(IdsCore.Transports.Pillar, Proto.CreateStr((Proto.ID) IdsCore.Transports.Pillar, "Pillar", "Pillar that supports transports."), new TransportPillarProto.Gfx("Assets/Base/Transports/Pillars/Pillars.prefab", "Assets/Base/Transports/Pillars/Base.prefab", "Assets/Base/Transports/Pillars/XFill.prefab", "Assets/Base/Transports/Pillars/PillarsWithFills.prefab")));
    }

    private Electricity withConfig(ProtoRegistrator registrator, Electricity power)
    {
      return !registrator.DisableAllProtoCosts ? power : Electricity.Zero;
    }

    private void createFlatConveyor(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      RelTile1f relTile1f1 = 0.7.Tiles();
      RelTile1f relTile1f2 = 0.8.Tiles();
      RelTile1f relTile1f3 = 0.25.Tiles();
      RelTile1f x1 = relTile1f1 / 2;
      RelTile1f x2 = relTile1f2 / 2;
      RelTile1f relTile1f4 = x2 - x1;
      RelTile2f v0 = new RelTile2f(-x2, -relTile1f3);
      RelTile2f v1 = new RelTile2f(-x2, -relTile1f4);
      RelTile2f v2 = new RelTile2f(-x1, RelTile1f.Zero);
      RelTile2f v3 = new RelTile2f(RelTile1f.Zero, RelTile1f.Zero);
      RelTile2f v4 = new RelTile2f(x1, RelTile1f.Zero);
      RelTile2f v5 = new RelTile2f(x2, -relTile1f4);
      RelTile2f v6 = new RelTile2f(x2, -relTile1f3);
      float[] uvs = TransportsData.buildUvs(v0, v1, v2, v3, v4, v5, v6).CheckLength<float>(7);
      float[] bottomUvs = TransportsData.buildUvs(v0, v6).CheckLength<float>(2);
      float num1 = uvs.Last<float>();
      float num2 = (float) (0.0099999997764825821 + (double) bottomUvs.Last<float>() + 0.0099999997764825821);
      TransportCrossSection crossSection1 = createCrossSection();
      IoPortShapeProto orThrow = prototypesDb.GetOrThrow<IoPortShapeProto>((Proto.ID) Ids.IoPortShapes.FlatConveyor);
      LocStr locStr = Loc.Str("FlatConveyorFormattedFirst__desc", "Transports units of solid products.", "description of transport");
      LocStr1 locStr1 = Loc.Str1("FlatConveyorFormattedNext__desc", "Transports units of solid products. Its throughput is {0} times increased compared to the previous tier.", "description of transport, '{0} times' used as '2 times' for instance.");
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID flatConveyorT3 = Ids.Transports.FlatConveyorT3;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Transports.FlatConveyorT3, "Flat conveyor III", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.FlatConveyorT3.Value + "__desc", locStr1.Format(2.ToString()).Value));
      ThicknessTilesF thicknessTilesF = relTile1f3.ThicknessTilesF;
      Quantity maxQuantityPerTransportedProduct1 = 3.Quantity();
      RelTile1f transportedProductsSpacing1 = 0.8.Tiles();
      RelTile1f speedPerTick1 = RelTile1f.FromTilesPerSecond(2.0);
      RelTile1i zStepLength = new RelTile1i(2);
      Option<TerrainTileSurfaceProto> tileSurfaceWhenOnGround = prototypesDb.Get<TerrainTileSurfaceProto>(Ids.TerrainTileSurfaces.DefaultConcrete);
      RelTile1i maxPillarSupportRadius1 = new RelTile1i(4);
      IoPortShapeProto portsShape = orThrow;
      Electricity baseElectricityCost1 = this.withConfig(registrator, 160.Kw());
      Percent circleSharpness = TransportsData.CIRCLE_SHARPNESS;
      EntityCosts entityCosts1 = Costs.Transports.FlatConveyorT3.MapToEntityCosts(registrator);
      RelTile1i lengthPerCost = Costs.Transports.LENGTH_PER_COST;
      Duration constrDurPerProduct = TransportsData.CONSTR_DUR_PER_PRODUCT;
      VirtualProductProto phantom = VirtualProductProto.Phantom;
      Quantity zero = Quantity.Zero;
      Option<TransportProto> none1 = (Option<TransportProto>) Option.None;
      VirtualProductProto maintenanceProduct = phantom;
      Quantity maintenancePerTile = zero;
      TransportCrossSection crossSection2 = crossSection1;
      RelTile1f transportUvLength = TransportsData.TRANSPORT_UV_LENGTH;
      Option<TransportProto.Gfx.FlowIndicatorSpec> none2 = Option<TransportProto.Gfx.FlowIndicatorSpec>.None;
      Option<string> none3 = Option<string>.None;
      Dict<TransportPillarAttachmentType, string> pillarAttachments = new Dict<TransportPillarAttachmentType, string>();
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToFlat_Straight, "Assets/Base/Transports/ConveyorUnit/FlatToFlat_Straight.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToFlat_Turn, "Assets/Base/Transports/ConveyorUnit/FlatToFlat_Turn.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.RampDownToRampUp_Turn, "Assets/Base/Transports/ConveyorUnit/RampDownToRampUp_Turn.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampUp_Straight, "Assets/Base/Transports/ConveyorUnit/FlatToRampUp_Straight.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampUp_Turn, "Assets/Base/Transports/ConveyorUnit/FlatToRampUp_Turn.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampDown_Straight, "Assets/Base/Transports/ConveyorUnit/FlatToRampDown_Straight.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampDown_Turn, "Assets/Base/Transports/ConveyorUnit/FlatToRampDown_Turn.prefab");
      Option<TransportProto.Gfx.TransportInstancedRenderingData> option = Option<TransportProto.Gfx.TransportInstancedRenderingData>.Create(new TransportProto.Gfx.TransportInstancedRenderingData());
      double uvShiftY1 = (double) num2 + 2.0 * ((double) num1 + 0.0099999997764825821);
      Percent hundred1 = Percent.Hundred;
      Option<TransportProto.Gfx.TransportInstancedRenderingData> instancedRenderingData = option;
      Option<string> customIconPath = new Option<string>();
      TransportProto.Gfx graphics = new TransportProto.Gfx(crossSection2, true, 5, "Assets/Base/Transports/ConveyorUnit/ConveyorUnit.mat", transportUvLength, true, "Assets/Base/Transports/Audio/TransportPlaced.prefab", none2, none3, (IReadOnlyDictionary<TransportPillarAttachmentType, string>) pillarAttachments, (float) uvShiftY1, hundred1, 1f, instancedRenderingData, customIconPath: customIconPath);
      TransportProto proto1 = new TransportProto(flatConveyorT3, str1, thicknessTilesF, maxQuantityPerTransportedProduct1, transportedProductsSpacing1, speedPerTick1, zStepLength, false, false, tileSurfaceWhenOnGround, maxPillarSupportRadius1, portsShape, baseElectricityCost1, circleSharpness, true, true, entityCosts1, lengthPerCost, constrDurPerProduct, none1, maintenanceProduct, maintenancePerTile, graphics);
      TransportProto transportProto1 = protosDb1.Add<TransportProto>(proto1);
      Assert.That<PartialQuantity>(transportProto1.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>(7.5.ToFix32(), "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto1);
      ProtosDb protosDb2 = prototypesDb;
      TransportProto source1 = transportProto1;
      StaticEntityProto.ID flatConveyorT2 = Ids.Transports.FlatConveyorT2;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Transports.FlatConveyorT2, "Flat conveyor II", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.FlatConveyorT2.Value + "__desc", locStr1.Format(3.ToString()).Value));
      RelTile1f? nullable1 = new RelTile1f?(0.9.Tiles());
      RelTile1f speedPerTick2 = RelTile1f.FromTilesPerSecond(1.5);
      RelTile1i? nullable2 = new RelTile1i?(new RelTile1i(4));
      Quantity? nullable3 = new Quantity?(2.Quantity());
      Electricity? nullable4 = new Electricity?(this.withConfig(registrator, 100.Kw()));
      EntityCosts entityCosts2 = Costs.Transports.FlatConveyorT2.MapToEntityCosts(registrator);
      Option<TransportProto> nextTier1 = (Option<TransportProto>) transportProto1;
      double uvShiftY2 = (double) num2 + (double) num1 + 0.0099999997764825821;
      Percent hundred2 = Percent.Hundred;
      Electricity? baseElectricityCost2 = nullable4;
      RelTile1i? maxPillarSupportRadius2 = nullable2;
      Quantity? maxQuantityPerTransportedProduct2 = nullable3;
      RelTile1f? transportedProductsSpacing2 = nullable1;
      TransportCrossSection? crossSection3 = new TransportCrossSection?();
      TransportProto proto2 = TransportsData.extendTransport(source1, flatConveyorT2, str2, speedPerTick2, entityCosts2, nextTier1, (float) uvShiftY2, hundred2, baseElectricityCost: baseElectricityCost2, maxPillarSupportRadius: maxPillarSupportRadius2, maxQuantityPerTransportedProduct: maxQuantityPerTransportedProduct2, transportedProductsSpacing: transportedProductsSpacing2, crossSection: crossSection3);
      TransportProto transportProto2 = protosDb2.Add<TransportProto>(proto2);
      Assert.That<PartialQuantity>(transportProto2.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>(3.3333.ToFix32(), "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto2);
      ProtosDb protosDb3 = prototypesDb;
      TransportProto source2 = transportProto2;
      StaticEntityProto.ID flatConveyor = Ids.Transports.FlatConveyor;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Transports.FlatConveyor, "Flat conveyor", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.FlatConveyor.Value + "__desc", locStr.TranslatedString));
      nullable1 = new RelTile1f?(1.0.Tiles());
      RelTile1f speedPerTick3 = RelTile1f.FromTilesPerSecond(1.0);
      nullable2 = new RelTile1i?(new RelTile1i(4));
      nullable3 = new Quantity?(1.Quantity());
      nullable4 = new Electricity?(this.withConfig(registrator, 70.Kw()));
      EntityCosts entityCosts3 = Costs.Transports.FlatConveyorT1.MapToEntityCosts(registrator);
      Option<TransportProto> nextTier2 = (Option<TransportProto>) transportProto2;
      double uvShiftY3 = (double) num2;
      Percent hundred3 = Percent.Hundred;
      Electricity? baseElectricityCost3 = nullable4;
      RelTile1i? maxPillarSupportRadius3 = nullable2;
      Quantity? maxQuantityPerTransportedProduct3 = nullable3;
      RelTile1f? transportedProductsSpacing3 = nullable1;
      TransportCrossSection? crossSection4 = new TransportCrossSection?();
      TransportProto proto3 = TransportsData.extendTransport(source2, flatConveyor, str3, speedPerTick3, entityCosts3, nextTier2, (float) uvShiftY3, hundred3, baseElectricityCost: baseElectricityCost3, maxPillarSupportRadius: maxPillarSupportRadius3, maxQuantityPerTransportedProduct: maxQuantityPerTransportedProduct3, transportedProductsSpacing: transportedProductsSpacing3, crossSection: crossSection4);
      TransportProto transportProto3 = protosDb3.Add<TransportProto>(proto3);
      Assert.That<PartialQuantity>(transportProto3.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>((Fix32) 1, "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto3);
      this.addZippers(registrator, "Flat balancer", orThrow, Costs.Transports.FlatZipper, "Assets/Base/Zippers/BalancerFlat.prefab", "Assets/Base/MiniZippers/ConnectorFlat.prefab");
      this.addSorter(registrator, "Flat sorter", Costs.Transports.FlatSorter, orThrow, "Assets/Base/Zippers/SorterFlat.prefab", (Func<ProductProto, bool>) (x => x.Type == CountableProductProto.ProductType));
      this.addLifts(registrator, (LocStrFormatted) TransportsData.LIFT_FLAT_NAME, (LocStrFormatted) TransportsData.LIFT_FLAT_DESC, Costs.Transports.LiftBase, Costs.Transports.LiftPerHeightDelta, orThrow, new string[6]
      {
        "Assets/Base/Transports/Lifts/LiftProduct-2.prefab",
        "Assets/Base/Transports/Lifts/LiftProduct-3.prefab",
        "Assets/Base/Transports/Lifts/LiftProduct-4.prefab",
        "Assets/Base/Transports/Lifts/LiftProduct-5.prefab",
        "Assets/Base/Transports/Lifts/LiftProduct-6.prefab",
        "Assets/Base/Transports/Lifts/LiftProduct-7.prefab"
      }, "Assets/Unity/Generated/Icons/LayoutEntity/LiftIoPortShape_FlatConveyor_3.png");

      TransportCrossSection createCrossSection()
      {
        Assert.That<float>(uvs.Last<float>()).IsLess(1f, "MAX UV is outside of texture.");
        return new TransportCrossSection(ImmutableArray.Create<CrossSectionVertex>(new CrossSectionVertex[5][]
        {
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v0, -Vector2f.UnitX, uvs[0]),
            new CrossSectionVertex(v1, -Vector2f.UnitX, uvs[1])
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v1, new Vector2f((Fix32) -1, (Fix32) 1), uvs[1]),
            new CrossSectionVertex(v2, new Vector2f((Fix32) -1, (Fix32) 1), uvs[2])
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v4, new Vector2f((Fix32) 1, (Fix32) 1), uvs[4]),
            new CrossSectionVertex(v5, new Vector2f((Fix32) 1, (Fix32) 1), uvs[5])
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v5, Vector2f.UnitX, uvs[5]),
            new CrossSectionVertex(v6, Vector2f.UnitX, uvs[6])
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v6, -Vector2f.UnitY, (float) -((double) bottomUvs[0] + 0.0099999997764825821)),
            new CrossSectionVertex(v0, -Vector2f.UnitY, (float) -((double) bottomUvs[1] + 0.0099999997764825821))
          }
        }), ImmutableArray.Create<CrossSectionVertex>(new CrossSectionVertex[1][]
        {
          new CrossSectionVertex[3]
          {
            new CrossSectionVertex(v2, Vector2f.UnitY, uvs[2]),
            new CrossSectionVertex(v3, Vector2f.UnitY, uvs[3]),
            new CrossSectionVertex(v4, Vector2f.UnitY, uvs[4])
          }
        }));
      }
    }

    private void createLooseMaterialConveyor(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      RelTile1f relTile1f1 = 0.2.Tiles();
      RelTile1f relTile1f2 = 0.6.Tiles();
      RelTile1f relTile1f3 = 0.8.Tiles();
      RelTile1f relTile1f4 = 0.15.Tiles();
      RelTile1f y = 0.2.Tiles();
      RelTile1f x1 = relTile1f1 / 2;
      RelTile1f x2 = relTile1f2 / 2;
      RelTile1f x3 = relTile1f3 / 2;
      // ISSUE: variable of a compiler-generated type
      TransportsData.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v0 = new RelTile2f(-x3, -relTile1f4);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v1 = new RelTile2f(-x3, y);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v2 = new RelTile2f(-x2, y);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v3 = new RelTile2f(-x1, RelTile1f.Zero);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v4 = new RelTile2f(x1, RelTile1f.Zero);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v5 = new RelTile2f(x2, y);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v6 = new RelTile2f(x3, y);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.v7 = new RelTile2f(x3, -relTile1f4);
      ref TransportsData.\u003C\u003Ec__DisplayClass18_0 local = ref cDisplayClass180;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Vector2f vector2f = (cDisplayClass180.v3 - cDisplayClass180.v2).Vector2f;
      vector2f = vector2f.LeftOrthogonalVector;
      Vector2f normalized = vector2f.Normalized;
      // ISSUE: reference to a compiler-generated field
      local.n2 = normalized;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.n3 = cDisplayClass180.n2.Average(Vector2f.UnitY).Normalized;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.uvs = TransportsData.buildUvs(cDisplayClass180.v0, cDisplayClass180.v1, cDisplayClass180.v2, cDisplayClass180.v3, cDisplayClass180.v4, cDisplayClass180.v5, cDisplayClass180.v6).CheckLength<float>(7);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.bottomUvs = TransportsData.buildUvs(cDisplayClass180.v0, cDisplayClass180.v7).CheckLength<float>(2);
      // ISSUE: reference to a compiler-generated field
      float num1 = cDisplayClass180.uvs.Last<float>();
      // ISSUE: reference to a compiler-generated field
      float num2 = (float) (0.0099999997764825821 + (double) cDisplayClass180.bottomUvs.Last<float>() + 0.0099999997764825821);
      TransportCrossSection createCrossSection180 = TransportsData.\u003CcreateLooseMaterialConveyor\u003Eg__createCrossSection\u007C18_0(ref cDisplayClass180);
      IoPortShapeProto orThrow = prototypesDb.GetOrThrow<IoPortShapeProto>((Proto.ID) Ids.IoPortShapes.LooseMaterialConveyor);
      LocStr locStr = Loc.Str("LooseConveyorFormattedFirst__desc", "Transports loose products.", "description of transport");
      LocStr1 locStr1 = Loc.Str1("LooseConveyorFormattedNext__desc", "Transports loose products. Its throughput is {0} times increased compared to the previous tier.", "description of transport, '{0} times' used as '2 times' for instance.");
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID materialConveyorT3 = Ids.Transports.LooseMaterialConveyorT3;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Transports.LooseMaterialConveyorT3, "U-shape conveyor III", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.LooseMaterialConveyorT3.Value + "__desc", locStr1.Format(2.ToString()).Value));
      ThicknessTilesF thicknessTilesF = relTile1f4.ThicknessTilesF;
      Quantity maxQuantityPerTransportedProduct1 = 3.Quantity();
      RelTile1f transportedProductsSpacing1 = 0.8.Tiles();
      RelTile1f speedPerTick1 = RelTile1f.FromTilesPerSecond(2.0);
      RelTile1i zStepLength = new RelTile1i(2);
      Option<TerrainTileSurfaceProto> tileSurfaceWhenOnGround = prototypesDb.Get<TerrainTileSurfaceProto>(Ids.TerrainTileSurfaces.DefaultConcrete);
      RelTile1i maxPillarSupportRadius1 = new RelTile1i(4);
      IoPortShapeProto portsShape = orThrow;
      Electricity baseElectricityCost1 = this.withConfig(registrator, 160.Kw());
      Percent circleSharpness = TransportsData.CIRCLE_SHARPNESS;
      EntityCosts entityCosts1 = Costs.Transports.LooseConveyorT3.MapToEntityCosts(registrator);
      RelTile1i lengthPerCost = Costs.Transports.LENGTH_PER_COST;
      Duration constrDurPerProduct = TransportsData.CONSTR_DUR_PER_PRODUCT;
      Option<TransportProto> none1 = (Option<TransportProto>) Option.None;
      VirtualProductProto phantom = VirtualProductProto.Phantom;
      Quantity zero = Quantity.Zero;
      TransportCrossSection crossSection1 = createCrossSection180;
      RelTile1f transportUvLength = TransportsData.TRANSPORT_UV_LENGTH;
      Option<TransportProto.Gfx.FlowIndicatorSpec> none2 = Option<TransportProto.Gfx.FlowIndicatorSpec>.None;
      Option<string> none3 = Option<string>.None;
      Dict<TransportPillarAttachmentType, string> pillarAttachments = new Dict<TransportPillarAttachmentType, string>();
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToFlat_Straight, "Assets/Base/Transports/ConveyorLoose/FlatToFlat_Straight.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToFlat_Turn, "Assets/Base/Transports/ConveyorLoose/FlatToFlat_Turn.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.RampDownToRampUp_Turn, "Assets/Base/Transports/ConveyorLoose/RampDownToRampUp_Turn.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampUp_Straight, "Assets/Base/Transports/ConveyorLoose/FlatToRampUp_Straight.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampUp_Turn, "Assets/Base/Transports/ConveyorLoose/FlatToRampUp_Turn.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampDown_Straight, "Assets/Base/Transports/ConveyorLoose/FlatToRampDown_Straight.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToRampDown_Turn, "Assets/Base/Transports/ConveyorLoose/FlatToRampDown_Turn.prefab");
      double uvShiftY1 = (double) num2 + 2.0 * ((double) num1 + 0.0099999997764825821);
      Percent hundred1 = Percent.Hundred;
      Option<TransportProto.Gfx.TransportInstancedRenderingData> instancedRenderingData = (Option<TransportProto.Gfx.TransportInstancedRenderingData>) new TransportProto.Gfx.TransportInstancedRenderingData();
      Option<string> customIconPath = new Option<string>();
      TransportProto.Gfx graphics = new TransportProto.Gfx(crossSection1, true, 5, "Assets/Base/Transports/ConveyorLoose/ConveyorLoose.mat", transportUvLength, true, "Assets/Base/Transports/Audio/TransportPlaced.prefab", none2, none3, (IReadOnlyDictionary<TransportPillarAttachmentType, string>) pillarAttachments, (float) uvShiftY1, hundred1, 1f, instancedRenderingData, customIconPath: customIconPath);
      TransportProto proto1 = new TransportProto(materialConveyorT3, str1, thicknessTilesF, maxQuantityPerTransportedProduct1, transportedProductsSpacing1, speedPerTick1, zStepLength, false, false, tileSurfaceWhenOnGround, maxPillarSupportRadius1, portsShape, baseElectricityCost1, circleSharpness, true, true, entityCosts1, lengthPerCost, constrDurPerProduct, none1, phantom, zero, graphics);
      TransportProto transportProto1 = protosDb1.Add<TransportProto>(proto1);
      Assert.That<PartialQuantity>(transportProto1.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>(7.5.ToFix32(), "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto1);
      ProtosDb protosDb2 = prototypesDb;
      TransportProto source1 = transportProto1;
      StaticEntityProto.ID materialConveyorT2 = Ids.Transports.LooseMaterialConveyorT2;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Transports.LooseMaterialConveyorT2, "U-shape conveyor II", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.LooseMaterialConveyorT2.Value + "__desc", locStr1.Format(3.ToString()).Value));
      RelTile1f speedPerTick2 = RelTile1f.FromTilesPerSecond(1.2);
      RelTile1f? nullable1 = new RelTile1f?(0.72.Tiles());
      Quantity? nullable2 = new Quantity?(2.Quantity());
      RelTile1i? nullable3 = new RelTile1i?(new RelTile1i(4));
      Electricity? nullable4 = new Electricity?(this.withConfig(registrator, 100.Kw()));
      EntityCosts entityCosts2 = Costs.Transports.LooseConveyorT2.MapToEntityCosts(registrator);
      Option<TransportProto> nextTier1 = (Option<TransportProto>) transportProto1;
      double uvShiftY2 = (double) num2 + (double) num1 + 0.0099999997764825821;
      Percent hundred2 = Percent.Hundred;
      Electricity? baseElectricityCost2 = nullable4;
      RelTile1i? maxPillarSupportRadius2 = nullable3;
      Quantity? maxQuantityPerTransportedProduct2 = nullable2;
      RelTile1f? transportedProductsSpacing2 = nullable1;
      TransportCrossSection? crossSection2 = new TransportCrossSection?();
      TransportProto proto2 = TransportsData.extendTransport(source1, materialConveyorT2, str2, speedPerTick2, entityCosts2, nextTier1, (float) uvShiftY2, hundred2, baseElectricityCost: baseElectricityCost2, maxPillarSupportRadius: maxPillarSupportRadius2, maxQuantityPerTransportedProduct: maxQuantityPerTransportedProduct2, transportedProductsSpacing: transportedProductsSpacing2, crossSection: crossSection2);
      TransportProto transportProto2 = protosDb2.Add<TransportProto>(proto2);
      Assert.That<PartialQuantity>(transportProto2.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>(3.3333.ToFix32(), "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto2);
      ProtosDb protosDb3 = prototypesDb;
      TransportProto source2 = transportProto2;
      StaticEntityProto.ID materialConveyor = Ids.Transports.LooseMaterialConveyor;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Transports.LooseMaterialConveyor, "U-shape conveyor", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.LooseMaterialConveyor.Value + "__desc", locStr.TranslatedString));
      RelTile1f speedPerTick3 = RelTile1f.FromTilesPerSecond(0.6);
      nullable1 = new RelTile1f?(0.6.Tiles());
      nullable2 = new Quantity?(1.Quantity());
      nullable3 = new RelTile1i?(new RelTile1i(4));
      nullable4 = new Electricity?(this.withConfig(registrator, 70.Kw()));
      EntityCosts entityCosts3 = Costs.Transports.LooseConveyorT1.MapToEntityCosts(registrator);
      Option<TransportProto> nextTier2 = (Option<TransportProto>) transportProto2;
      double uvShiftY3 = (double) num2;
      Percent hundred3 = Percent.Hundred;
      Electricity? baseElectricityCost3 = nullable4;
      RelTile1i? maxPillarSupportRadius3 = nullable3;
      Quantity? maxQuantityPerTransportedProduct3 = nullable2;
      RelTile1f? transportedProductsSpacing3 = nullable1;
      TransportCrossSection? crossSection3 = new TransportCrossSection?();
      TransportProto proto3 = TransportsData.extendTransport(source2, materialConveyor, str3, speedPerTick3, entityCosts3, nextTier2, (float) uvShiftY3, hundred3, baseElectricityCost: baseElectricityCost3, maxPillarSupportRadius: maxPillarSupportRadius3, maxQuantityPerTransportedProduct: maxQuantityPerTransportedProduct3, transportedProductsSpacing: transportedProductsSpacing3, crossSection: crossSection3);
      TransportProto transportProto3 = protosDb3.Add<TransportProto>(proto3);
      Assert.That<PartialQuantity>(transportProto3.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>((Fix32) 1, "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto3);
      this.addZippers(registrator, "U-shape balancer", orThrow, Costs.Transports.LooseZipper, "Assets/Base/Zippers/BalancerUShape.prefab", "Assets/Base/MiniZippers/ConnectorUShape.prefab");
      this.addSorter(registrator, "U-shape sorter", Costs.Transports.LooseSorter, orThrow, "Assets/Base/Zippers/SorterUShape.prefab", (Func<ProductProto, bool>) (x => x.Type == LooseProductProto.ProductType));
      this.addLifts(registrator, (LocStrFormatted) TransportsData.LIFT_LOOSE_NAME, (LocStrFormatted) TransportsData.LIFT_LOOSE_DESC, Costs.Transports.LiftBase, Costs.Transports.LiftPerHeightDelta, orThrow, new string[6]
      {
        "Assets/Base/Transports/Lifts/LiftLoose-2.prefab",
        "Assets/Base/Transports/Lifts/LiftLoose-3.prefab",
        "Assets/Base/Transports/Lifts/LiftLoose-4.prefab",
        "Assets/Base/Transports/Lifts/LiftLoose-5.prefab",
        "Assets/Base/Transports/Lifts/LiftLoose-6.prefab",
        "Assets/Base/Transports/Lifts/LiftLoose-7.prefab"
      }, "Assets/Unity/Generated/Icons/LayoutEntity/LiftIoPortShape_LooseMaterialConveyor_3.png");
    }

    private void createMoltenMetalChannel(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      RelTile1f y = 0.1.Tiles();
      RelTile1f relTile1f1 = 0.1.Tiles();
      RelTile1f relTile1f2 = 0.075.Tiles();
      RelTile1f relTile1f3 = 0.2.Tiles();
      RelTile1f relTile1f4 = 0.6.Tiles();
      RelTile1f x1 = relTile1f2 / 2;
      RelTile1f x2 = relTile1f3 / 2;
      RelTile1f x3 = relTile1f4 / 2;
      RelTile2f v0 = new RelTile2f(-x3, -relTile1f1);
      RelTile2f v1 = new RelTile2f(-x3, y);
      RelTile2f v2 = new RelTile2f(-x2, y);
      RelTile2f v3 = new RelTile2f(-x1, RelTile1f.Zero);
      RelTile2f v4 = new RelTile2f(x1, RelTile1f.Zero);
      RelTile2f v5 = new RelTile2f(x2, y);
      RelTile2f v6 = new RelTile2f(x3, y);
      RelTile2f v7 = new RelTile2f(x3, -relTile1f1);
      Vector2f n23 = (v3 - v2).Vector2f.LeftOrthogonalVector;
      Vector2f n45 = n23.ReflectX;
      float[] uvs = TransportsData.buildUvs(v0, v1, v2, v3, v4, v5, v6, v7, v0).CheckLength<float>(9);
      TransportCrossSection crossSection1 = createCrossSection(0.01f);
      IoPortShapeProto orThrow = prototypesDb.GetOrThrow<IoPortShapeProto>((Proto.ID) Ids.IoPortShapes.MoltenMetalChannel);
      LocStr locStr = Loc.Str("MoltenConveyorFormattedFirst__desc", "Transports molten products.", "description of transport");
      ProtosDb protosDb = prototypesDb;
      StaticEntityProto.ID moltenMetalChannel = Ids.Transports.MoltenMetalChannel;
      Proto.Str str = Proto.CreateStr((Proto.ID) Ids.Transports.MoltenMetalChannel, "Molten channel", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.MoltenMetalChannel.Value + "__desc", locStr.TranslatedString));
      ThicknessTilesF thicknessTilesF = relTile1f1.ThicknessTilesF;
      Quantity maxQuantityPerTransportedProduct = 1.Quantity();
      RelTile1f transportedProductsSpacing = 0.5.Tiles();
      RelTile1f speedPerTick = RelTile1f.FromTilesPerSecond(1.0);
      RelTile1i maxValue = RelTile1i.MaxValue;
      Option<TerrainTileSurfaceProto> tileSurfaceWhenOnGround = prototypesDb.Get<TerrainTileSurfaceProto>(Ids.TerrainTileSurfaces.DefaultConcrete);
      RelTile1i maxPillarSupportRadius = new RelTile1i(4);
      IoPortShapeProto portsShape = orThrow;
      Electricity zero1 = Electricity.Zero;
      Percent circleSharpness = TransportsData.CIRCLE_SHARPNESS;
      EntityCosts entityCosts = Costs.Transports.MoltenMetalChannel.MapToEntityCosts(registrator);
      RelTile1i lengthPerCost = Costs.Transports.LENGTH_PER_COST;
      Duration constrDurPerProduct = TransportsData.CONSTR_DUR_PER_PRODUCT;
      VirtualProductProto phantom = VirtualProductProto.Phantom;
      Quantity zero2 = Quantity.Zero;
      Option<TransportProto> none1 = (Option<TransportProto>) Option.None;
      VirtualProductProto maintenanceProduct = phantom;
      Quantity maintenancePerTile = zero2;
      TransportCrossSection crossSection2 = crossSection1;
      RelTile1f transportUvLength = TransportsData.TRANSPORT_UV_LENGTH;
      Option<TransportProto.Gfx.FlowIndicatorSpec> none2 = Option<TransportProto.Gfx.FlowIndicatorSpec>.None;
      Option<string> none3 = Option<string>.None;
      Dict<TransportPillarAttachmentType, string> pillarAttachments = new Dict<TransportPillarAttachmentType, string>();
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToFlat_Straight, "Assets/Base/Transports/MoltenChannel/FlatToFlat_Straight.prefab");
      pillarAttachments.Add(TransportPillarAttachmentType.FlatToFlat_Turn, "Assets/Base/Transports/MoltenChannel/FlatToFlat_Turn.prefab");
      Percent hundred = Percent.Hundred;
      Option<TransportProto.Gfx.TransportInstancedRenderingData> instancedRenderingData = (Option<TransportProto.Gfx.TransportInstancedRenderingData>) new TransportProto.Gfx.TransportInstancedRenderingData();
      Option<string> customIconPath = new Option<string>();
      TransportProto.Gfx graphics = new TransportProto.Gfx(crossSection2, true, 5, "Assets/Base/Transports/MoltenChannel/MoltenChannel.mat", transportUvLength, true, "Assets/Base/Transports/Audio/MoltenPlaced.prefab", none2, none3, (IReadOnlyDictionary<TransportPillarAttachmentType, string>) pillarAttachments, 0.0f, hundred, 1f, instancedRenderingData, customIconPath: customIconPath);
      TransportProto proto = new TransportProto(moltenMetalChannel, str, thicknessTilesF, maxQuantityPerTransportedProduct, transportedProductsSpacing, speedPerTick, maxValue, false, false, tileSurfaceWhenOnGround, maxPillarSupportRadius, portsShape, zero1, circleSharpness, false, true, entityCosts, lengthPerCost, constrDurPerProduct, none1, maintenanceProduct, maintenancePerTile, graphics);
      TransportProto transportProto = protosDb.Add<TransportProto>(proto);
      Assert.That<PartialQuantity>(transportProto.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>((Fix32) 2, "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto);
      this.addZippers(registrator, "Molten balancer", orThrow, Costs.Transports.MoltenZipper, "Assets/Base/Zippers/BalancerMolten.prefab", "Assets/Base/MiniZippers/ConnectorMolten.prefab");

      TransportCrossSection createCrossSection(float uvShiftY)
      {
        Assert.That<float>(uvs.Last<float>() + uvShiftY).IsLess(1f, "MAX UV is outside of texture.");
        return new TransportCrossSection(ImmutableArray.Create<CrossSectionVertex>(new CrossSectionVertex[6][]
        {
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v0, -Vector2f.UnitX, uvs[0] + uvShiftY),
            new CrossSectionVertex(v1, -Vector2f.UnitX, uvs[1] + uvShiftY)
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v1, Vector2f.UnitY, uvs[1] + uvShiftY),
            new CrossSectionVertex(v2, Vector2f.UnitY, uvs[2] + uvShiftY)
          },
          new CrossSectionVertex[4]
          {
            new CrossSectionVertex(v2, n23, uvs[2] + uvShiftY),
            new CrossSectionVertex(v3, n23.Average(Vector2f.UnitY), uvs[3] + uvShiftY),
            new CrossSectionVertex(v4, n45.Average(Vector2f.UnitY), uvs[4] + uvShiftY),
            new CrossSectionVertex(v5, n45, uvs[5] + uvShiftY)
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v5, Vector2f.UnitY, uvs[5] + uvShiftY),
            new CrossSectionVertex(v6, Vector2f.UnitY, uvs[6] + uvShiftY)
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v6, Vector2f.UnitX, uvs[6] + uvShiftY),
            new CrossSectionVertex(v7, Vector2f.UnitX, uvs[7] + uvShiftY)
          },
          new CrossSectionVertex[2]
          {
            new CrossSectionVertex(v7, Vector2f.UnitX, uvs[7] + uvShiftY),
            new CrossSectionVertex(v0, Vector2f.UnitX, uvs[8] + uvShiftY)
          }
        }), ImmutableArray<ImmutableArray<CrossSectionVertex>>.Empty);
      }
    }

    private void createPipe(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      RelTile1f radius1 = 0.15.Tiles();
      RelTile1f radius2 = 0.2.Tiles();
      RelTile1f radius3 = 0.25.Tiles();
      float maxUv;
      createCrossSection(radius1, 0.01f, out maxUv);
      float num1 = maxUv;
      createCrossSection(radius2, maxUv + 0.01f, out maxUv);
      float num2 = maxUv;
      createCrossSection(radius3, maxUv + 0.01f, out maxUv);
      TransportCrossSection crossSection1 = createCrossSection(radius1, 0.0f, out float _);
      IoPortShapeProto orThrow = prototypesDb.GetOrThrow<IoPortShapeProto>((Proto.ID) Ids.IoPortShapes.Pipe);
      LocStr locStr = Loc.Str("PipeFormattedFirst__desc", "Transports liquids and gasses. Pipes cannot transport more than one product type at a time.", "description of transport");
      LocStr1 locStr1 = Loc.Str1("PipeFormattedNext__desc", "Transports liquids and gasses. Its throughput is {0} times increased compared to the previous tier.", "description of transport, '{0} times' used as '2 times' for instance.");
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID pipeT3 = Ids.Transports.PipeT3;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Transports.PipeT3, "Pipe III", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.PipeT3.Value + "__desc", locStr1.Format(3.ToString()).Value));
      ThicknessTilesF surfaceRelativeHeight = 0.5.TilesThick();
      Quantity maxQuantityPerTransportedProduct1 = 3.Quantity();
      RelTile1f transportedProductsSpacing1 = 1.6.Tiles();
      RelTile1f speedPerTick1 = RelTile1f.FromTilesPerSecond(4.0);
      RelTile1i zStepLength = new RelTile1i(0);
      Option<TerrainTileSurfaceProto> none1 = Option<TerrainTileSurfaceProto>.None;
      RelTile1i maxPillarSupportRadius1 = new RelTile1i(4);
      IoPortShapeProto portsShape = orThrow;
      Electricity zero1 = Electricity.Zero;
      Percent circleSharpness = TransportsData.CIRCLE_SHARPNESS;
      EntityCosts entityCosts1 = Costs.Transports.PipeT3.MapToEntityCosts(registrator);
      RelTile1i lengthPerCost = Costs.Transports.LENGTH_PER_COST;
      Duration constrDurPerProduct = TransportsData.CONSTR_DUR_PER_PRODUCT;
      VirtualProductProto phantom = VirtualProductProto.Phantom;
      Quantity zero2 = Quantity.Zero;
      Option<TransportProto> none2 = (Option<TransportProto>) Option.None;
      VirtualProductProto maintenanceProduct = phantom;
      Quantity maintenancePerTile = zero2;
      TransportCrossSection crossSection2 = crossSection1;
      RelTile1f transportUvLength = TransportsData.TRANSPORT_UV_LENGTH;
      Option<TransportProto.Gfx.FlowIndicatorSpec> flowIndicator1 = (Option<TransportProto.Gfx.FlowIndicatorSpec>) new TransportProto.Gfx.FlowIndicatorSpec("Assets/Base/Transports/Pipes/FlowIndicator_Frame_T3.prefab", "Assets/Base/Transports/Pipes/FlowIndicator_Flow_T3.prefab", "Assets/Base/Transports/Pipes/FlowIndicator_Glass_T3.prefab", 0.9.Tiles(), 10.0.Tiles(), new FluidIndicatorGfxParams(1.2f, 0.6f, 1f));
      Option<string> verticalConnectorPrefabPath = (Option<string>) "Assets/Base/Transports/Pipes/Vertical_Connector.prefab";
      Dict<TransportPillarAttachmentType, string> pillarAttachments1 = new Dict<TransportPillarAttachmentType, string>();
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToFlat_Straight, "Assets/Base/Transports/Pipes/T3-FlatToFlat_Straight.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToFlat_Turn, "Assets/Base/Transports/Pipes/T3-FlatToFlat_Turn.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.RampDownToRampUp_Turn, "Assets/Base/Transports/Pipes/T3-RampDownToRampUp_Turn.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToRampUp_Straight, "Assets/Base/Transports/Pipes/T3-FlatToRampUp_Straight.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToRampUp_Turn, "Assets/Base/Transports/Pipes/T3-FlatToRampUp_Turn.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToRampDown_Straight, "Assets/Base/Transports/Pipes/T3-FlatToRampDown_Straight.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToRampDown_Turn, "Assets/Base/Transports/Pipes/T3-FlatToRampDown_Turn.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToVertical, "Assets/Base/Transports/Pipes/T3-FlatToVertical.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.VerticalToVertical, "Assets/Base/Transports/Pipes/T3-VerticalToVertical.prefab");
      pillarAttachments1.Add(TransportPillarAttachmentType.FlatToVertical_Down, "Assets/Base/Transports/Pipes/T3-FlatToVertical_Down.prefab");
      double uvShiftY1 = (double) num2 + 0.0099999997764825821;
      Percent crossSectionScale1 = Percent.FromRatio(radius3.Value, radius1.Value);
      double crossSectionRadius1 = (double) radius1.Value.ToFloat() * 2.0;
      Option<TransportProto.Gfx.TransportInstancedRenderingData> instancedRenderingData = (Option<TransportProto.Gfx.TransportInstancedRenderingData>) new TransportProto.Gfx.TransportInstancedRenderingData();
      Option<string> customIconPath = new Option<string>();
      TransportProto.Gfx graphics = new TransportProto.Gfx(crossSection2, false, 5, "Assets/Base/Transports/Pipes/Pipes.mat", transportUvLength, false, "Assets/Base/Transports/Audio/PipePlaced.prefab", flowIndicator1, verticalConnectorPrefabPath, (IReadOnlyDictionary<TransportPillarAttachmentType, string>) pillarAttachments1, (float) uvShiftY1, crossSectionScale1, (float) crossSectionRadius1, instancedRenderingData, true, customIconPath);
      TransportProto proto1 = new TransportProto(pipeT3, str1, surfaceRelativeHeight, maxQuantityPerTransportedProduct1, transportedProductsSpacing1, speedPerTick1, zStepLength, true, true, none1, maxPillarSupportRadius1, portsShape, zero1, circleSharpness, false, true, entityCosts1, lengthPerCost, constrDurPerProduct, none2, maintenanceProduct, maintenancePerTile, graphics);
      TransportProto transportProto1 = protosDb1.Add<TransportProto>(proto1);
      Assert.That<PartialQuantity>(transportProto1.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>(7.5.ToFix32(), "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto1);
      ProtosDb protosDb2 = prototypesDb;
      TransportProto source1 = transportProto1;
      StaticEntityProto.ID pipeT2 = Ids.Transports.PipeT2;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Transports.PipeT2, "Pipe II", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.PipeT2.Value + "__desc", locStr1.Format(3.ToString()).Value));
      Quantity? nullable1 = new Quantity?(2.Quantity());
      RelTile1f? nullable2 = new RelTile1f?(1.8.Tiles());
      RelTile1f speedPerTick2 = RelTile1f.FromTilesPerSecond(3.0);
      RelTile1i? nullable3 = new RelTile1i?(new RelTile1i(4));
      EntityCosts entityCosts2 = Costs.Transports.PipeT2.MapToEntityCosts(registrator);
      Option<TransportProto> nextTier1 = (Option<TransportProto>) transportProto1;
      TransportProto.Gfx.FlowIndicatorSpec flowIndicatorSpec1 = new TransportProto.Gfx.FlowIndicatorSpec("Assets/Base/Transports/Pipes/FlowIndicator_Frame_T2.prefab", "Assets/Base/Transports/Pipes/FlowIndicator_Flow_T2.prefab", "Assets/Base/Transports/Pipes/FlowIndicator_Glass_T2.prefab", 0.8.Tiles(), 10.0.Tiles(), new FluidIndicatorGfxParams(1f, 0.5f, 1f));
      IReadOnlyDictionary<TransportPillarAttachmentType, string> readOnlyDictionary1 = (IReadOnlyDictionary<TransportPillarAttachmentType, string>) new Dict<TransportPillarAttachmentType, string>()
      {
        {
          TransportPillarAttachmentType.FlatToFlat_Straight,
          "Assets/Base/Transports/Pipes/T2-FlatToFlat_Straight.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToFlat_Turn,
          "Assets/Base/Transports/Pipes/T2-FlatToFlat_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.RampDownToRampUp_Turn,
          "Assets/Base/Transports/Pipes/T2-RampDownToRampUp_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampUp_Straight,
          "Assets/Base/Transports/Pipes/T2-FlatToRampUp_Straight.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampUp_Turn,
          "Assets/Base/Transports/Pipes/T2-FlatToRampUp_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampDown_Straight,
          "Assets/Base/Transports/Pipes/T2-FlatToRampDown_Straight.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampDown_Turn,
          "Assets/Base/Transports/Pipes/T2-FlatToRampDown_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToVertical,
          "Assets/Base/Transports/Pipes/T2-FlatToVertical.prefab"
        },
        {
          TransportPillarAttachmentType.VerticalToVertical,
          "Assets/Base/Transports/Pipes/T2-VerticalToVertical.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToVertical_Down,
          "Assets/Base/Transports/Pipes/T2-FlatToVertical_Down.prefab"
        }
      };
      double uvShiftY2 = (double) num1 + 0.0099999997764825821;
      Percent crossSectionScale2 = Percent.FromRatio(radius2.Value, radius1.Value);
      double crossSectionRadius2 = (double) radius1.Value.ToFloat() * 2.0;
      Electricity? baseElectricityCost1 = new Electricity?();
      RelTile1i? maxPillarSupportRadius2 = nullable3;
      Quantity? maxQuantityPerTransportedProduct2 = nullable1;
      RelTile1f? transportedProductsSpacing2 = nullable2;
      TransportCrossSection? crossSection3 = new TransportCrossSection?();
      TransportProto.Gfx.FlowIndicatorSpec flowIndicator2 = flowIndicatorSpec1;
      IReadOnlyDictionary<TransportPillarAttachmentType, string> pillarAttachments2 = readOnlyDictionary1;
      TransportProto proto2 = TransportsData.extendTransport(source1, pipeT2, str2, speedPerTick2, entityCosts2, nextTier1, (float) uvShiftY2, crossSectionScale2, (float) crossSectionRadius2, baseElectricityCost1, maxPillarSupportRadius2, maxQuantityPerTransportedProduct2, transportedProductsSpacing2, crossSection: crossSection3, flowIndicator: flowIndicator2, pillarAttachments: pillarAttachments2);
      TransportProto transportProto2 = protosDb2.Add<TransportProto>(proto2);
      Assert.That<PartialQuantity>(transportProto2.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>(3.3333.ToFix32(), "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto2);
      ProtosDb protosDb3 = prototypesDb;
      TransportProto source2 = transportProto2;
      StaticEntityProto.ID pipeT1 = Ids.Transports.PipeT1;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Transports.PipeT1, "Pipe", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Transports.PipeT1.Value + "__desc", locStr.TranslatedString));
      nullable1 = new Quantity?(1.Quantity());
      nullable2 = new RelTile1f?(2.0.Tiles());
      RelTile1f speedPerTick3 = RelTile1f.FromTilesPerSecond(2.0);
      nullable3 = new RelTile1i?(new RelTile1i(4));
      EntityCosts entityCosts3 = Costs.Transports.Pipe.MapToEntityCosts(registrator);
      Option<TransportProto> nextTier2 = (Option<TransportProto>) transportProto2;
      TransportProto.Gfx.FlowIndicatorSpec flowIndicatorSpec2 = new TransportProto.Gfx.FlowIndicatorSpec("Assets/Base/Transports/Pipes/FlowIndicator_Frame_T1.prefab", "Assets/Base/Transports/Pipes/FlowIndicator_Flow_T1.prefab", "Assets/Base/Transports/Pipes/FlowIndicator_Glass_T1.prefab", 0.7.Tiles(), 10.0.Tiles(), new FluidIndicatorGfxParams(0.8f, 0.4f, 1f));
      IReadOnlyDictionary<TransportPillarAttachmentType, string> readOnlyDictionary2 = (IReadOnlyDictionary<TransportPillarAttachmentType, string>) new Dict<TransportPillarAttachmentType, string>()
      {
        {
          TransportPillarAttachmentType.FlatToFlat_Straight,
          "Assets/Base/Transports/Pipes/T1-FlatToFlat_Straight.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToFlat_Turn,
          "Assets/Base/Transports/Pipes/T1-FlatToFlat_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.RampDownToRampUp_Turn,
          "Assets/Base/Transports/Pipes/T1-RampDownToRampUp_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampUp_Straight,
          "Assets/Base/Transports/Pipes/T1-FlatToRampUp_Straight.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampUp_Turn,
          "Assets/Base/Transports/Pipes/T1-FlatToRampUp_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampDown_Straight,
          "Assets/Base/Transports/Pipes/T1-FlatToRampDown_Straight.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToRampDown_Turn,
          "Assets/Base/Transports/Pipes/T1-FlatToRampDown_Turn.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToVertical,
          "Assets/Base/Transports/Pipes/T1-FlatToVertical.prefab"
        },
        {
          TransportPillarAttachmentType.VerticalToVertical,
          "Assets/Base/Transports/Pipes/T1-VerticalToVertical.prefab"
        },
        {
          TransportPillarAttachmentType.FlatToVertical_Down,
          "Assets/Base/Transports/Pipes/T1-FlatToVertical_Down.prefab"
        }
      };
      Percent hundred = Percent.Hundred;
      double crossSectionRadius3 = (double) radius1.Value.ToFloat() * 2.0;
      Electricity? baseElectricityCost2 = new Electricity?();
      RelTile1i? maxPillarSupportRadius3 = nullable3;
      Quantity? maxQuantityPerTransportedProduct3 = nullable1;
      RelTile1f? transportedProductsSpacing3 = nullable2;
      TransportCrossSection? crossSection4 = new TransportCrossSection?();
      TransportProto.Gfx.FlowIndicatorSpec flowIndicator3 = flowIndicatorSpec2;
      IReadOnlyDictionary<TransportPillarAttachmentType, string> pillarAttachments3 = readOnlyDictionary2;
      TransportProto proto3 = TransportsData.extendTransport(source2, pipeT1, str3, speedPerTick3, entityCosts3, nextTier2, 0.01f, hundred, (float) crossSectionRadius3, baseElectricityCost2, maxPillarSupportRadius3, maxQuantityPerTransportedProduct3, transportedProductsSpacing3, "Assets/Base/Transports/Pipes/Pipes.mat", crossSection4, flowIndicator3, pillarAttachments: pillarAttachments3);
      TransportProto transportProto3 = protosDb3.Add<TransportProto>(proto3);
      Assert.That<PartialQuantity>(transportProto3.ThroughputPerTick * 1.Seconds().Ticks).IsNear<TransportProto>((Fix32) 1, "Imprecise throughput of {0}. This might be due to product spacing rounding.", transportProto3);
      this.addZippers(registrator, "Pipe balancer", orThrow, Costs.Transports.FluidZipper, "Assets/Base/Zippers/BalancerFluid.prefab", "Assets/Base/MiniZippers/ConnectorFluid.prefab");

      static TransportCrossSection createCrossSection(
        RelTile1f radius,
        float uvShiftY,
        out float maxUv)
      {
        ImmutableArrayBuilder<CrossSectionVertex> immutableArrayBuilder = new ImmutableArrayBuilder<CrossSectionVertex>(9);
        AngleDegrees1f angleDegrees1f = -AngleDegrees1f.Deg360 / 8;
        RelTile1f lengthTiles = (radius.Value * new Tile2f((Fix32) 1, (Fix32) 0) - radius.Value * new Tile2f(angleDegrees1f.DirectionVector)).LengthTiles;
        Assert.That<RelTile1f>(8 * lengthTiles).IsLess(TransportsData.TRANSPORT_UV_LENGTH);
        for (int i = 0; i <= 8; ++i)
        {
          Vector2f directionVector = (i * angleDegrees1f - AngleDegrees1f.Deg90).DirectionVector;
          float num = (float) i * lengthTiles.Value.ToFloat() / TransportsData.TRANSPORT_UV_LENGTH.Value.ToFloat();
          immutableArrayBuilder[i] = new CrossSectionVertex(new RelTile2f(radius.Value * directionVector), directionVector, num + uvShiftY);
        }
        maxUv = immutableArrayBuilder.Last.TextureCoordY;
        Assert.That<float>(maxUv).IsLess(1f, "MAX UV is outside of texture.");
        return new TransportCrossSection(ImmutableArray.Create<ImmutableArray<CrossSectionVertex>>(immutableArrayBuilder.GetImmutableArrayAndClear()), ImmutableArray<ImmutableArray<CrossSectionVertex>>.Empty);
      }
    }

    private void createShaft(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      IoPortShapeProto orThrow = prototypesDb.GetOrThrow<IoPortShapeProto>((Proto.ID) Ids.IoPortShapes.Shaft);
      ProtosDb protosDb = prototypesDb;
      StaticEntityProto.ID shaft = Ids.Transports.Shaft;
      Proto.Str str = Proto.CreateStr((Proto.ID) Ids.Transports.Shaft, "Shaft");
      ThicknessTilesF surfaceRelativeHeight = 0.5.TilesThick();
      Quantity maxQuantityPerTransportedProduct = 1.Quantity();
      RelTile1f transportedProductsSpacing = 1.0.Tiles();
      RelTile1f speedPerTick = RelTile1f.FromTilesPerSecond(1.0);
      RelTile1i maxValue = RelTile1i.MaxValue;
      Option<TerrainTileSurfaceProto> none1 = Option<TerrainTileSurfaceProto>.None;
      RelTile1i maxPillarSupportRadius = new RelTile1i(4);
      IoPortShapeProto portsShape = orThrow;
      Electricity zero1 = Electricity.Zero;
      Percent circleSharpness = TransportsData.CIRCLE_SHARPNESS;
      EntityCosts entityCosts = Costs.Transports.Shaft.MapToEntityCosts(registrator);
      RelTile1i lengthPerCost = Costs.Transports.LENGTH_PER_COST;
      Duration constrDurPerProduct = TransportsData.CONSTR_DUR_PER_PRODUCT;
      VirtualProductProto phantom = VirtualProductProto.Phantom;
      Quantity zero2 = Quantity.Zero;
      Option<TransportProto> none2 = (Option<TransportProto>) Option.None;
      VirtualProductProto maintenanceProduct = phantom;
      Quantity maintenancePerTile = zero2;
      TransportProto.Gfx graphics = new TransportProto.Gfx(TransportCrossSection.Empty, false, 5, "TODO", TransportsData.TRANSPORT_UV_LENGTH, false, "TODO", Option<TransportProto.Gfx.FlowIndicatorSpec>.None, Option<string>.None, (IReadOnlyDictionary<TransportPillarAttachmentType, string>) new Dict<TransportPillarAttachmentType, string>(), 0.0f, Percent.Hundred, 1f, (Option<TransportProto.Gfx.TransportInstancedRenderingData>) new TransportProto.Gfx.TransportInstancedRenderingData());
      TransportProto proto = new TransportProto(shaft, str, surfaceRelativeHeight, maxQuantityPerTransportedProduct, transportedProductsSpacing, speedPerTick, maxValue, true, false, none1, maxPillarSupportRadius, portsShape, zero1, circleSharpness, true, false, entityCosts, lengthPerCost, constrDurPerProduct, none2, maintenanceProduct, maintenancePerTile, graphics);
      protosDb.Add<TransportProto>(proto);
    }

    private static float[] buildUvs(params RelTile2f[] vertices)
    {
      Assert.That<RelTile2f[]>(vertices).IsNotEmpty<RelTile2f>();
      RelTile1f zero = RelTile1f.Zero;
      RelTile2f relTile2f = vertices.First<RelTile2f>();
      RelTile1f[] array = new RelTile1f[vertices.Length];
      int index = 1;
      for (int length = vertices.Length; index < length; ++index)
      {
        RelTile2f vertex = vertices[index];
        zero += (vertex - relTile2f).LengthTiles;
        relTile2f = vertex;
        array[index] = zero;
      }
      Assert.That<RelTile1f>(zero).IsLess(TransportsData.TRANSPORT_UV_LENGTH);
      return array.MapArray<RelTile1f, float>((Func<RelTile1f, float>) (x => x.Value.ToFloat() / TransportsData.TRANSPORT_UV_LENGTH.Value.ToFloat()));
    }

    private void addZippers(
      ProtoRegistrator registrator,
      string name,
      IoPortShapeProto portShape,
      EntityCostsTpl costs,
      string prefabPath,
      string miniZipperPath)
    {
      StaticEntityProto.ID zipperIdFor = IdsCore.Transports.GetZipperIdFor(portShape.Id);
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      StaticEntityProto.ID id1 = zipperIdFor;
      Proto.Str str = Proto.CreateStr((Proto.ID) zipperIdFor, name, "Allows distributing and prioritizing products using any of its two input and output ports.", "small machine that allows splitting and merging of transports");
      EntityCosts entityCosts = costs.MapToEntityCosts(registrator);
      Electricity electricity = this.withConfig(registrator, 20.Kw());
      EntityLayoutParser layoutParser1 = registrator.LayoutParser;
      ThicknessIRange? customPlacementRange = new ThicknessIRange?(new ThicknessIRange(0, TransportPillarProto.MAX_PILLAR_HEIGHT.Value - 1));
      EntityLayoutParams layoutParams1 = new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("|0|", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? nullable1 = new int?(0);
          int? nullable2 = new int?(0);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = nullable1;
          int? maxTerrainHeight = nullable2;
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightToExcl: heightToExcl, constraint: LayoutTileConstraint.UsingPillar, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }, portsCanOnlyConnectToTransports: true, customPlacementRange: customPlacementRange);
      string[] strArray1 = TransportsData.setPortChar(portShape.LayoutChar, "   D?+C?+   ", "E?+|1||1|+?B", "F?+|1||1|+?A", "   G?+H?+   ");
      EntityLayout layoutOrThrow1 = layoutParser1.ParseLayoutOrThrow(layoutParams1, strArray1);
      EntityCosts costs1 = entityCosts;
      Electricity electricityConsumed = electricity;
      string prefabPath1 = prefabPath;
      ColorRgba white = ColorRgba.White;
      ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Transports));
      RelTile3f prefabOrigin = new RelTile3f();
      Option<string> customIconPath = new Option<string>();
      ColorRgba color = white;
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories = nullable;
      ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx(prefabPath1, prefabOrigin, customIconPath, color, true, visualizedLayers, categories, true, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects, maxRenderedLod: 5);
      ZipperProto proto1 = new ZipperProto(id1, str, layoutOrThrow1, costs1, electricityConsumed, true, graphics1);
      prototypesDb1.Add<ZipperProto>(proto1);
      StaticEntityProto.ID miniZipperIdFor = IdsCore.Transports.GetMiniZipperIdFor(portShape.Id);
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      StaticEntityProto.ID id2 = miniZipperIdFor;
      Proto.Str miniZipTitle = TransportsData.MINI_ZIP_TITLE;
      EntityLayoutParser layoutParser2 = registrator.LayoutParser;
      customPlacementRange = new ThicknessIRange?(new ThicknessIRange(0, TransportPillarProto.MAX_PILLAR_HEIGHT.Value - 1));
      EntityLayoutParams layoutParams2 = new EntityLayoutParams(customPlacementRange: customPlacementRange);
      string[] strArray2 = TransportsData.setPortChar(portShape.LayoutChar, "   B?+   ", "+?C{1}A?+", "   D?+   ");
      EntityLayout layoutOrThrow2 = layoutParser2.ParseLayoutOrThrow(layoutParams2, strArray2);
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx(miniZipperPath, RelTile3f.Zero, Option<string>.None, ColorRgba.White, true, useInstancedRendering: true, maxRenderedLod: 3);
      MiniZipperProto proto2 = new MiniZipperProto(id2, miniZipTitle, layoutOrThrow2, graphics2);
      prototypesDb2.Add<MiniZipperProto>(proto2);
    }

    private void addSorter(
      ProtoRegistrator registrator,
      string name,
      EntityCostsTpl costs,
      IoPortShapeProto portShape,
      string prefabPath,
      Func<ProductProto, bool> productsFilter)
    {
      StaticEntityProto.ID sorterIdFor = IdsCore.Transports.GetSorterIdFor(portShape.Id);
      ProtosDb prototypesDb = registrator.PrototypesDb;
      StaticEntityProto.ID id = sorterIdFor;
      Proto.Str str = Proto.CreateStr((Proto.ID) sorterIdFor, name, "Allows sorting of products.", "small machine that allows sorting of products");
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("|0|", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? nullable1 = new int?(0);
          int? nullable2 = new int?(0);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = nullable1;
          int? maxTerrainHeight = nullable2;
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightToExcl: heightToExcl, constraint: LayoutTileConstraint.UsingPillar, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }, customPlacementRange: new ThicknessIRange?(new ThicknessIRange(0, TransportPillarProto.MAX_PILLAR_HEIGHT.Value - 1))), TransportsData.setPortChar(portShape.LayoutChar, "A?>|1||1|>?X", "   {1}|1|   ", "      v?S   "));
      Func<ProductProto, bool> productsFilter1 = productsFilter;
      EntityCosts entityCosts = costs.MapToEntityCosts(registrator);
      Electricity requiredPower = this.withConfig(registrator, 20.Kw());
      string prefabPath1 = prefabPath;
      ColorRgba white = ColorRgba.White;
      ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Transports));
      RelTile3f prefabOrigin = new RelTile3f();
      Option<string> customIconPath = new Option<string>();
      ColorRgba color = white;
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories = nullable;
      ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics = new LayoutEntityProto.Gfx(prefabPath1, prefabOrigin, customIconPath, color, true, visualizedLayers, categories, true, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects, maxRenderedLod: 5);
      SorterProto proto = new SorterProto(id, str, layoutOrThrow, productsFilter1, entityCosts, requiredPower, true, graphics);
      prototypesDb.Add<SorterProto>(proto);
    }

    private void addLifts(
      ProtoRegistrator registrator,
      LocStrFormatted name,
      LocStrFormatted desc,
      EntityCostsTpl baseCost,
      EntityCostsTpl costPerHeightDelta,
      IoPortShapeProto portShape,
      string[] prefabPaths,
      string customIconPath)
    {
      EntityCosts entityCosts1 = baseCost.MapToEntityCosts(registrator);
      AssetValue price = costPerHeightDelta.MapToEntityCosts(registrator).Price;
      IEnumerable<TransportProto> lyst1 = (IEnumerable<TransportProto>) registrator.PrototypesDb.Filter<TransportProto>((Func<TransportProto, bool>) (tp => (Proto) tp.PortsShape == (Proto) portShape)).ToLyst<TransportProto>();
      IEnumerable<ProductProto> lyst2 = (IEnumerable<ProductProto>) registrator.PrototypesDb.Filter<ProductProto>((Func<ProductProto, bool>) (p => p.Type == portShape.AllowedProductType)).ToLyst<ProductProto>();
      PartialQuantity partialQuantity = PartialQuantity.Zero;
      if (lyst2.IsEnumerableEmpty<ProductProto>())
      {
        Log.Info(string.Format("Lift {0} has no compatible products.", (object) name));
        partialQuantity = PartialQuantity.One;
      }
      else if (lyst1.IsEnumerableEmpty<TransportProto>())
      {
        Log.Info(string.Format("Lift {0} has no compatible transports.", (object) name));
        partialQuantity = PartialQuantity.One;
      }
      else
      {
        foreach (TransportProto transportProto in lyst1)
        {
          foreach (ProductProto productProto in lyst2)
            partialQuantity = partialQuantity.Max(transportProto.GetMaxThroughputPerTickFor(productProto));
        }
      }
      for (int index = -6; index <= 6; ++index)
      {
        if (index != 0)
        {
          int multiplier = index.Abs();
          StaticEntityProto.ID liftIdFor = IdsCore.Transports.GetLiftIdFor(portShape.Id, index);
          ImmutableArray<ToolbarCategoryProto> immutableArray1;
          if (index != 1)
            immutableArray1 = ImmutableArray<ToolbarCategoryProto>.Empty;
          else
            immutableArray1 = registrator.GetCategoriesProtos(Ids.ToolbarCategories.Transports);
          ImmutableArray<ToolbarCategoryProto> immutableArray2 = immutableArray1;
          EntityCosts entityCosts2 = new EntityCosts(entityCosts1.Price + price.Mul(multiplier), entityCosts1.DefaultPriority, entityCosts1.Workers, new MaintenanceCosts?(entityCosts1.Maintenance));
          ProtosDb prototypesDb = registrator.PrototypesDb;
          StaticEntityProto.ID id = liftIdFor;
          Proto.Str strFromLocalized = Proto.CreateStrFromLocalized((Proto.ID) liftIdFor, name, desc);
          EntityLayoutParser layoutParser = registrator.LayoutParser;
          ThicknessIRange? nullable1 = new ThicknessIRange?(new ThicknessIRange(0, TransportPillarProto.MAX_PILLAR_HEIGHT.Value - 1));
          CustomLayoutToken[] customTokens = new CustomLayoutToken[1]
          {
            new CustomLayoutToken("|0|", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
            {
              int heightToExcl = h;
              int? nullable2 = new int?(0);
              int? nullable3 = new int?(0);
              int? terrainSurfaceHeight = new int?();
              int? minTerrainHeight = nullable2;
              int? maxTerrainHeight = nullable3;
              Fix32? vehicleHeight = new Fix32?();
              Proto.ID? terrainMaterialId = new Proto.ID?();
              Proto.ID? surfaceId = new Proto.ID?();
              return new LayoutTokenSpec(heightToExcl: heightToExcl, constraint: LayoutTileConstraint.UsingPillar, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
            }))
          };
          Option<IEnumerable<KeyValuePair<char, int>>> option1 = (Option<IEnumerable<KeyValuePair<char, int>>>) (IEnumerable<KeyValuePair<char, int>>) new Lyst<KeyValuePair<char, int>>()
          {
            Make.Kvp<char, int>('X', multiplier + 1)
          };
          Proto.ID? hardenedFloorSurfaceId = new Proto.ID?();
          int? customCollapseVerticesThreshold = new int?();
          ThicknessIRange? customPlacementRange = nullable1;
          Option<IEnumerable<KeyValuePair<char, int>>> customPortHeights = option1;
          EntityLayoutParams layoutParams = new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) customTokens, hardenedFloorSurfaceId: hardenedFloorSurfaceId, customCollapseVerticesThreshold: customCollapseVerticesThreshold, customPlacementRange: customPlacementRange, customPortHeights: customPortHeights);
          string[] strArray = TransportsData.setPortChar(portShape.LayoutChar, index > 0 ? string.Format("A?>|{0}||{1}|>?X", (object) (multiplier + 2), (object) (multiplier + 2)) : string.Format("A?<|{0}||{1}|<?X", (object) (multiplier + 2), (object) (multiplier + 2)));
          EntityLayout layoutOrThrow = layoutParser.ParseLayoutOrThrow(layoutParams, strArray);
          EntityCosts costs = entityCosts2;
          Electricity requiredPower = this.withConfig(registrator, 2.Kw() + 2.Kw() * multiplier);
          IoPortShapeProto portsShape = portShape;
          ImmutableArray<AnimationParams> animationParams = ImmutableArray.Create<AnimationParams>((AnimationParams) new LoopAnimationParams(Percent.Hundred));
          ThicknessTilesI heightDelta = new ThicknessTilesI(index);
          ThicknessTilesI minHeightDelta = new ThicknessTilesI(-6);
          PartialQuantity maxThroughputPerStep = partialQuantity;
          string prefabPath = prefabPaths[multiplier - 1];
          Option<string> option2 = (Option<string>) customIconPath;
          ColorRgba white = ColorRgba.White;
          ImmutableArray<ToolbarCategoryProto>? nullable4 = new ImmutableArray<ToolbarCategoryProto>?(immutableArray2);
          ImmutableArray<string> immutableArray3 = ImmutableArray.Create<string>(index > 0 ? "arrow-in" : "arrow-out");
          RelTile3f prefabOrigin = new RelTile3f();
          Option<string> customIconPath1 = option2;
          ColorRgba color = white;
          LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
          ImmutableArray<ToolbarCategoryProto>? categories = nullable4;
          ImmutableArray<string> instancedRenderingExcludedObjects = immutableArray3;
          LiftProto.Gfx graphics = new LiftProto.Gfx(prefabPath, "arrow-out", "arrow-in", prefabOrigin, customIconPath1, color, true, visualizedLayers, categories, useSemiInstancedRendering: true, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects, maxRenderedLod: 5);
          LiftProto proto = new LiftProto(id, strFromLocalized, layoutOrThrow, costs, requiredPower, portsShape, animationParams, heightDelta, minHeightDelta, maxThroughputPerStep, graphics);
          prototypesDb.Add<LiftProto>(proto);
        }
      }
    }

    private static string[] setPortChar(char portChar, params string[] layout)
    {
      int index = 0;
      for (int length = layout.Length; index < length; ++index)
        layout[index] = layout[index].Replace('?', portChar);
      return layout;
    }

    private static TransportProto extendTransport(
      TransportProto source,
      StaticEntityProto.ID id,
      Proto.Str strings,
      RelTile1f speedPerTick,
      EntityCosts costs,
      Option<TransportProto> nextTier,
      float uvShiftY,
      Percent crossSectionScale,
      float crossSectionRadius = 1f,
      Electricity? baseElectricityCost = null,
      RelTile1i? maxPillarSupportRadius = null,
      Quantity? maxQuantityPerTransportedProduct = null,
      RelTile1f? transportedProductsSpacing = null,
      string materialPath = null,
      TransportCrossSection? crossSection = null,
      TransportProto.Gfx.FlowIndicatorSpec flowIndicator = null,
      string verticalConnectorPrefabPath = null,
      IReadOnlyDictionary<TransportPillarAttachmentType, string> pillarAttachments = null)
    {
      Option<TransportProto.Gfx.TransportInstancedRenderingData> option = crossSection.HasValue || materialPath != null && materialPath != source.Graphics.MaterialPath || flowIndicator != null && flowIndicator != source.Graphics.FlowIndicator || pillarAttachments != null && pillarAttachments != source.Graphics.PillarAttachments ? (Option<TransportProto.Gfx.TransportInstancedRenderingData>) new TransportProto.Gfx.TransportInstancedRenderingData() : source.Graphics.InstancedRenderingData;
      TransportCrossSection crossSection1 = crossSection ?? source.Graphics.CrossSection;
      int num1 = source.Graphics.RenderProducts ? 1 : 0;
      bool perProductColoring = source.Graphics.UsePerProductColoring;
      int perCurvedSegment = source.Graphics.SamplesPerCurvedSegment;
      string materialPath1 = materialPath ?? source.Graphics.MaterialPath;
      RelTile1f transportUvLength = source.Graphics.TransportUvLength;
      int num2 = source.Graphics.RenderTransportedProducts ? 1 : 0;
      string onBuildPrefabPath = source.Graphics.SoundOnBuildPrefabPath;
      TransportProto.Gfx.FlowIndicatorSpec flowIndicatorSpec = flowIndicator;
      Option<TransportProto.Gfx.FlowIndicatorSpec> flowIndicator1 = flowIndicatorSpec != null ? (Option<TransportProto.Gfx.FlowIndicatorSpec>) flowIndicatorSpec : source.Graphics.FlowIndicator;
      string str = verticalConnectorPrefabPath;
      Option<string> verticalConnectorPrefabPath1 = str != null ? (Option<string>) str : source.Graphics.VerticalConnectorPrefabPath;
      IReadOnlyDictionary<TransportPillarAttachmentType, string> pillarAttachments1 = pillarAttachments ?? source.Graphics.PillarAttachments;
      double uvShiftY1 = (double) uvShiftY;
      Percent crossSectionScale1 = crossSectionScale;
      double crossSectionRadius1 = (double) crossSectionRadius;
      Option<TransportProto.Gfx.TransportInstancedRenderingData> instancedRenderingData = option;
      int num3 = perProductColoring ? 1 : 0;
      Option<string> customIconPath = new Option<string>();
      TransportProto.Gfx gfx = new TransportProto.Gfx(crossSection1, num1 != 0, perCurvedSegment, materialPath1, transportUvLength, num2 != 0, onBuildPrefabPath, flowIndicator1, verticalConnectorPrefabPath1, pillarAttachments1, (float) uvShiftY1, crossSectionScale1, (float) crossSectionRadius1, instancedRenderingData, num3 != 0, customIconPath);
      StaticEntityProto.ID id1 = id;
      Proto.Str strings1 = strings;
      ThicknessTilesF surfaceRelativeHeight = source.SurfaceRelativeHeight;
      Quantity maxQuantityPerTransportedProduct1 = maxQuantityPerTransportedProduct ?? source.MaxQuantityPerTransportedProduct;
      RelTile1f transportedProductsSpacing1 = transportedProductsSpacing ?? source.TransportedProductsSpacing;
      RelTile1f speedPerTick1 = speedPerTick;
      RelTile1i zstepLength = source.ZStepLength;
      int num4 = source.NeedsPillarsAtGround ? 1 : 0;
      int num5 = source.CanBeBuried ? 1 : 0;
      Option<TerrainTileSurfaceProto> surfaceWhenOnGround = source.TileSurfaceWhenOnGround;
      RelTile1i maxPillarSupportRadius1 = maxPillarSupportRadius ?? source.MaxPillarSupportRadius;
      IoPortShapeProto portsShape = source.PortsShape;
      Electricity baseElectricityCost1 = baseElectricityCost ?? source.BaseElectricityCost;
      Percent sharpnessPercent = source.CornersSharpnessPercent;
      int num6 = source.AllowMixedProducts ? 1 : 0;
      int num7 = source.IsBuildable ? 1 : 0;
      EntityCosts costs1 = costs;
      RelTile1i lengthPerCost = source.LengthPerCost;
      Duration durationPerProduct = source.ConstructionDurationPerProduct;
      VirtualProductProto maintenanceProduct1 = source.MaintenanceProduct;
      Quantity maintenancePerTile1 = source.MaintenancePerTile;
      Option<TransportProto> nextTier1 = nextTier;
      VirtualProductProto maintenanceProduct2 = maintenanceProduct1;
      Quantity maintenancePerTile2 = maintenancePerTile1;
      TransportProto.Gfx graphics = gfx;
      return new TransportProto(id1, strings1, surfaceRelativeHeight, maxQuantityPerTransportedProduct1, transportedProductsSpacing1, speedPerTick1, zstepLength, num4 != 0, num5 != 0, surfaceWhenOnGround, maxPillarSupportRadius1, portsShape, baseElectricityCost1, sharpnessPercent, num6 != 0, num7 != 0, costs1, lengthPerCost, durationPerProduct, nextTier1, maintenanceProduct2, maintenancePerTile2, graphics);
    }

    public TransportsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TransportsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TransportsData.CIRCLE_SHARPNESS = 55.Percent();
      TransportsData.TRANSPORT_UV_LENGTH = 6.0.Tiles();
      TransportsData.CONSTR_DUR_PER_PRODUCT = 2.Ticks();
      TransportsData.MINI_ZIP_TITLE = new Proto.Str(Loc.Str("MiniZip_all", "Connector", "small box that allows splitting and merging of transports"));
      TransportsData.LIFT_LOOSE_NAME = Loc.Str("LiftLoose__name", "Loose lift", "small machine that allows raising / lowering loose products vertically");
      TransportsData.LIFT_LOOSE_DESC = Loc.Str("LiftLoose__desc", "Allows raising or lowering loose products vertically.", "small machine connected to transport belts that allows moving products vertically");
      TransportsData.LIFT_FLAT_NAME = Loc.Str("LiftFlat__name", "Flat lift", "small machine that allows raising / lowering units of solid products vertically");
      TransportsData.LIFT_FLAT_DESC = Loc.Str("LiftFlat__desc", "Allows raising or lowering units of solid products vertically.", "small machine connected to transport belts that allows moving products vertically");
    }
  }
}
