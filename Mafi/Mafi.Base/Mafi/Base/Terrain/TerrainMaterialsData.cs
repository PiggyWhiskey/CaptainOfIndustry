// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.TerrainMaterialsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Base.Terrain
{
  public class TerrainMaterialsData : IModData
  {
    private static readonly Percent DIRT_MINED_QUANTITY_MULT;
    private static readonly Percent ROCK_MINED_QUANTITY_MULT;
    private static readonly Percent COAL_MINED_QUANTITY_MULT;
    private static readonly Percent BEDROCK_MINED_QUANTITY_MULT;
    private static readonly Percent DISRUPTION_SPEED_MULT_SAND;
    private static readonly Percent DISRUPTION_SPEED_MULT_ROCK;
    private bool m_disableDetails;

    internal void DisableTerrainDetails() => this.m_disableDetails = true;

    public void RegisterData(ProtoRegistrator registrator)
    {
      // ISSUE: variable of a compiler-generated type
      TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 cDisplayClass80;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass80.db = registrator.PrototypesDb;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local1 = ref cDisplayClass80;
      // ISSUE: reference to a compiler-generated field
      ProtosDb db1 = cDisplayClass80.db;
      Proto.ID grass = Ids.TerrainMaterials.Grass;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow1 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Dirt);
      Proto.ID? nullable1 = new Proto.ID?(Ids.TerrainMaterials.Dirt);
      Percent minedQuantityMult1 = TerrainMaterialsData.DIRT_MINED_QUANTITY_MULT;
      ThicknessTilesF minCollapseHeightDiff1 = 0.9.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff1 = 1.8.TilesThick();
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics1 = new TerrainMaterialProto.Gfx((ColorRgba) 9127187, (ColorRgba) 6767914, "Assets/Base/Terrain/Textures/Grass-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Grass-2048-normal-s-ao.png", -0.5f, fullyWetBrightnessDelta: -0.3f, dustColor: new ColorRgba(7163699, 8), detailLayers: this.m_disableDetails ? ImmutableArray<DetailLayerSpec>.Empty : ImmutableArray.Create<DetailLayerSpec>(new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.Grass))));
      Proto.ID? weathersInto1 = new Proto.ID?();
      Proto.ID? disruptsInto1 = nullable1;
      Percent? disruptionSpeedMult1 = new Percent?();
      Proto.ID? recoversInto1 = new Proto.ID?();
      Duration recoveryTime1 = new Duration();
      Percent grassGrowthOnTop1 = new Percent();
      TerrainMaterialProto proto1 = new TerrainMaterialProto(grass, "Grass", orThrow1, minedQuantityMult1, minCollapseHeightDiff1, maxCollapseHeightDiff1, graphics1, true, true, weathersInto: weathersInto1, disruptsInto: disruptsInto1, disruptionSpeedMult: disruptionSpeedMult1, disruptWhenCollapsing: true, recoversInto: recoversInto1, recoveryTime: recoveryTime1, grassGrowthOnTop: grassGrowthOnTop1);
      TerrainMaterialProto terrainMaterialProto1 = db1.Add<TerrainMaterialProto>(proto1);
      // ISSUE: reference to a compiler-generated field
      local1.grass = terrainMaterialProto1;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local2 = ref cDisplayClass80;
      // ISSUE: reference to a compiler-generated field
      ProtosDb db2 = cDisplayClass80.db;
      Proto.ID dirt = Ids.TerrainMaterials.Dirt;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto minedProduct1 = cDisplayClass80.grass.MinedProduct;
      nullable1 = new Proto.ID?(Ids.TerrainMaterials.Grass);
      Duration duration1 = 20.Minutes();
      // ISSUE: reference to a compiler-generated field
      Percent minedQuantityMult2 = cDisplayClass80.grass.MinedQuantityMult;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF minCollapseHeightDiff2 = cDisplayClass80.grass.MinCollapseHeightDiff - 0.3.TilesThick();
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF maxCollapseHeightDiff2 = cDisplayClass80.grass.MaxCollapseHeightDiff - 0.6.TilesThick();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics2 = new TerrainMaterialProto.Gfx(cDisplayClass80.grass.Graphics.Color.Rgba, cDisplayClass80.grass.Graphics.ParticleColor.Rgba, "Assets/Base/Terrain/Textures/Dirt-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Dirt-2048-normal-s-ao.png", 1f, fullyWetBrightnessDelta: -0.4f, dustColor: new ColorRgba(7163699, 40));
      Proto.ID? weathersInto2 = new Proto.ID?();
      Proto.ID? disruptsInto2 = new Proto.ID?();
      Percent? disruptionSpeedMult2 = new Percent?();
      Proto.ID? recoversInto2 = nullable1;
      Duration recoveryTime2 = duration1;
      Percent grassGrowthOnTop2 = new Percent();
      TerrainMaterialProto proto2 = new TerrainMaterialProto(dirt, "Dirt", minedProduct1, minedQuantityMult2, minCollapseHeightDiff2, maxCollapseHeightDiff2, graphics2, true, true, ignoreInEditor: true, weathersInto: weathersInto2, disruptsInto: disruptsInto2, disruptionSpeedMult: disruptionSpeedMult2, recoversInto: recoversInto2, recoveryTime: recoveryTime2, grassGrowthOnTop: grassGrowthOnTop2);
      TerrainMaterialProto terrainMaterialProto2 = db2.Add<TerrainMaterialProto>(proto2);
      // ISSUE: reference to a compiler-generated field
      local2.dirt = terrainMaterialProto2;
      // ISSUE: reference to a compiler-generated field
      ProtosDb db3 = cDisplayClass80.db;
      Proto.ID dirtBare = Ids.TerrainMaterials.DirtBare;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto minedProduct2 = cDisplayClass80.dirt.MinedProduct;
      // ISSUE: reference to a compiler-generated field
      Percent minedQuantityMult3 = cDisplayClass80.dirt.MinedQuantityMult;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF collapseHeightDiff1 = cDisplayClass80.dirt.MinCollapseHeightDiff;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF collapseHeightDiff2 = cDisplayClass80.dirt.MaxCollapseHeightDiff;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics3 = cDisplayClass80.dirt.Graphics.WithReplaced("Assets/Base/Terrain/Textures/DirtBare-2048-albedo-height.png", "Assets/Base/Terrain/Textures/DirtBare-2048-normal-s-ao.png");
      Proto.ID? weathersInto3 = new Proto.ID?();
      Proto.ID? disruptsInto3 = new Proto.ID?();
      Percent? disruptionSpeedMult3 = new Percent?();
      Proto.ID? nullable2 = new Proto.ID?();
      Proto.ID? recoversInto3 = nullable2;
      Duration recoveryTime3 = new Duration();
      Percent grassGrowthOnTop3 = new Percent();
      TerrainMaterialProto proto3 = new TerrainMaterialProto(dirtBare, "Dirt (bare)", minedProduct2, minedQuantityMult3, collapseHeightDiff1, collapseHeightDiff2, graphics3, true, true, weathersInto: weathersInto3, disruptsInto: disruptsInto3, disruptionSpeedMult: disruptionSpeedMult3, recoversInto: recoversInto3, recoveryTime: recoveryTime3, grassGrowthOnTop: grassGrowthOnTop3);
      db3.Add<TerrainMaterialProto>(proto3);
      Proto.ID grassNoDetails = Ids.TerrainMaterials.GrassNoDetails;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics4 = cDisplayClass80.grass.Graphics;
      ImmutableArray<DetailLayerSpec> empty = ImmutableArray<DetailLayerSpec>.Empty;
      float? dustiness1 = new float?();
      float? fullyWetSmoothnessDelta1 = new float?();
      float? nullable3 = new float?();
      float? fullyWetBrightnessDelta1 = nullable3;
      ColorRgba? dustColor1 = new ColorRgba?();
      ImmutableArray<DetailLayerSpec> detailLayers1 = empty;
      TerrainMaterialProto.Gfx grassGraphics1 = graphics4.WithReplaced(dustiness: dustiness1, fullyWetSmoothnessDelta: fullyWetSmoothnessDelta1, fullyWetBrightnessDelta: fullyWetBrightnessDelta1, dustColor: dustColor1, detailLayers: detailLayers1);
      Proto.ID dirtNoDetails = Ids.TerrainMaterials.DirtNoDetails;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics5 = cDisplayClass80.dirt.Graphics;
      TerrainMaterialProto terrainMaterialProto3;
      ref TerrainMaterialProto local3 = ref terrainMaterialProto3;
      TerrainMaterialProto terrainMaterialProto4;
      ref TerrainMaterialProto local4 = ref terrainMaterialProto4;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local5 = ref cDisplayClass80;
      TerrainMaterialsData.\u003CRegisterData\u003Eg__createGrassDirtPair\u007C8_0(grassNoDetails, "no details", grassGraphics1, dirtNoDetails, graphics5, out local3, out local4, ref local5);
      Proto.ID grassLush = Ids.TerrainMaterials.GrassLush;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics6 = cDisplayClass80.grass.Graphics;
      nullable3 = new float?(0.4f);
      // ISSUE: reference to a compiler-generated field
      ImmutableArray<DetailLayerSpec> immutableArray1 = this.m_disableDetails ? ImmutableArray<DetailLayerSpec>.Empty : ImmutableArray.Create<DetailLayerSpec>(new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.GrassLush)));
      float? dustiness2 = new float?();
      float? fullyWetSmoothnessDelta2 = nullable3;
      float? fullyWetBrightnessDelta2 = new float?();
      ColorRgba? dustColor2 = new ColorRgba?();
      ImmutableArray<DetailLayerSpec> detailLayers2 = immutableArray1;
      TerrainMaterialProto.Gfx grassGraphics2 = graphics6.WithReplaced("Assets/Base/Terrain/Textures/GrassAlt-2048-albedo-height.png", "Assets/Base/Terrain/Textures/GrassAlt-2048-normal-s-ao.png", dustiness2, fullyWetSmoothnessDelta2, fullyWetBrightnessDelta2, dustColor2, detailLayers2);
      Proto.ID dirtLush = Ids.TerrainMaterials.DirtLush;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics7 = cDisplayClass80.dirt.Graphics;
      TerrainMaterialProto terrainMaterialProto5;
      ref TerrainMaterialProto local6 = ref terrainMaterialProto5;
      ref TerrainMaterialProto local7 = ref terrainMaterialProto4;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local8 = ref cDisplayClass80;
      TerrainMaterialsData.\u003CRegisterData\u003Eg__createGrassDirtPair\u007C8_0(grassLush, "lush", grassGraphics2, dirtLush, graphics7, out local6, out local7, ref local8);
      Proto.ID flowersWhite = Ids.TerrainMaterials.FlowersWhite;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics8 = cDisplayClass80.grass.Graphics;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ImmutableArray<DetailLayerSpec> immutableArray2 = this.m_disableDetails ? ImmutableArray<DetailLayerSpec>.Empty : ImmutableArray.Create<DetailLayerSpec>(new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.FlowersWhite), 0.75f), new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.Grass), 0.5f));
      float? dustiness3 = new float?();
      float? fullyWetSmoothnessDelta3 = new float?();
      float? fullyWetBrightnessDelta3 = new float?();
      ColorRgba? dustColor3 = new ColorRgba?();
      ImmutableArray<DetailLayerSpec> detailLayers3 = immutableArray2;
      TerrainMaterialProto.Gfx grassGraphics3 = graphics8.WithReplaced(dustiness: dustiness3, fullyWetSmoothnessDelta: fullyWetSmoothnessDelta3, fullyWetBrightnessDelta: fullyWetBrightnessDelta3, dustColor: dustColor3, detailLayers: detailLayers3);
      Proto.ID dirtFlowersWhite = Ids.TerrainMaterials.DirtFlowersWhite;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics9 = cDisplayClass80.dirt.Graphics;
      ref TerrainMaterialProto local9 = ref terrainMaterialProto4;
      ref TerrainMaterialProto local10 = ref terrainMaterialProto3;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local11 = ref cDisplayClass80;
      TerrainMaterialsData.\u003CRegisterData\u003Eg__createGrassDirtPair\u007C8_0(flowersWhite, "white flowers", grassGraphics3, dirtFlowersWhite, graphics9, out local9, out local10, ref local11);
      Proto.ID flowersYellowLush1 = Ids.TerrainMaterials.FlowersYellowLush;
      TerrainMaterialProto.Gfx graphics10 = terrainMaterialProto5.Graphics;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ImmutableArray<DetailLayerSpec> immutableArray3 = this.m_disableDetails ? ImmutableArray<DetailLayerSpec>.Empty : ImmutableArray.Create<DetailLayerSpec>(new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.FlowersYellowLush), 0.75f), new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.GrassLush), 0.5f));
      float? dustiness4 = new float?();
      float? fullyWetSmoothnessDelta4 = new float?();
      float? fullyWetBrightnessDelta4 = new float?();
      ColorRgba? dustColor4 = new ColorRgba?();
      ImmutableArray<DetailLayerSpec> detailLayers4 = immutableArray3;
      TerrainMaterialProto.Gfx grassGraphics4 = graphics10.WithReplaced(dustiness: dustiness4, fullyWetSmoothnessDelta: fullyWetSmoothnessDelta4, fullyWetBrightnessDelta: fullyWetBrightnessDelta4, dustColor: dustColor4, detailLayers: detailLayers4);
      Proto.ID flowersYellowLush2 = Ids.TerrainMaterials.DirtFlowersYellowLush;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics11 = cDisplayClass80.dirt.Graphics;
      ref TerrainMaterialProto local12 = ref terrainMaterialProto3;
      ref TerrainMaterialProto local13 = ref terrainMaterialProto4;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local14 = ref cDisplayClass80;
      TerrainMaterialsData.\u003CRegisterData\u003Eg__createGrassDirtPair\u007C8_0(flowersYellowLush1, "yellow flowers, lush", grassGraphics4, flowersYellowLush2, graphics11, out local12, out local13, ref local14);
      Proto.ID flowersRed = Ids.TerrainMaterials.FlowersRed;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics12 = cDisplayClass80.grass.Graphics;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ImmutableArray<DetailLayerSpec> immutableArray4 = this.m_disableDetails ? ImmutableArray<DetailLayerSpec>.Empty : ImmutableArray.Create<DetailLayerSpec>(new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.FlowersRed), 0.75f), new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.Grass), 0.5f));
      float? dustiness5 = new float?();
      float? fullyWetSmoothnessDelta5 = new float?();
      float? fullyWetBrightnessDelta5 = new float?();
      ColorRgba? dustColor5 = new ColorRgba?();
      ImmutableArray<DetailLayerSpec> detailLayers5 = immutableArray4;
      TerrainMaterialProto.Gfx grassGraphics5 = graphics12.WithReplaced(dustiness: dustiness5, fullyWetSmoothnessDelta: fullyWetSmoothnessDelta5, fullyWetBrightnessDelta: fullyWetBrightnessDelta5, dustColor: dustColor5, detailLayers: detailLayers5);
      Proto.ID dirtFlowersRed = Ids.TerrainMaterials.DirtFlowersRed;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics13 = cDisplayClass80.dirt.Graphics;
      ref TerrainMaterialProto local15 = ref terrainMaterialProto4;
      ref TerrainMaterialProto local16 = ref terrainMaterialProto3;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local17 = ref cDisplayClass80;
      TerrainMaterialsData.\u003CRegisterData\u003Eg__createGrassDirtPair\u007C8_0(flowersRed, "red flowers", grassGraphics5, dirtFlowersRed, graphics13, out local15, out local16, ref local17);
      Proto.ID flowersPurpleLush1 = Ids.TerrainMaterials.FlowersPurpleLush;
      TerrainMaterialProto.Gfx graphics14 = terrainMaterialProto5.Graphics;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ImmutableArray<DetailLayerSpec> immutableArray5 = this.m_disableDetails ? ImmutableArray<DetailLayerSpec>.Empty : ImmutableArray.Create<DetailLayerSpec>(new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.FlowersPurpleLush), 0.75f), new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.GrassLush), 0.5f));
      float? dustiness6 = new float?();
      float? fullyWetSmoothnessDelta6 = new float?();
      float? fullyWetBrightnessDelta6 = new float?();
      ColorRgba? dustColor6 = new ColorRgba?();
      ImmutableArray<DetailLayerSpec> detailLayers6 = immutableArray5;
      TerrainMaterialProto.Gfx grassGraphics6 = graphics14.WithReplaced(dustiness: dustiness6, fullyWetSmoothnessDelta: fullyWetSmoothnessDelta6, fullyWetBrightnessDelta: fullyWetBrightnessDelta6, dustColor: dustColor6, detailLayers: detailLayers6);
      Proto.ID flowersPurpleLush2 = Ids.TerrainMaterials.DirtFlowersPurpleLush;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics15 = cDisplayClass80.dirt.Graphics;
      ref TerrainMaterialProto local18 = ref terrainMaterialProto3;
      ref TerrainMaterialProto local19 = ref terrainMaterialProto4;
      ref TerrainMaterialsData.\u003C\u003Ec__DisplayClass8_0 local20 = ref cDisplayClass80;
      TerrainMaterialsData.\u003CRegisterData\u003Eg__createGrassDirtPair\u007C8_0(flowersPurpleLush1, "purple flowers, lush", grassGraphics6, flowersPurpleLush2, graphics15, out local18, out local19, ref local20);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db4 = cDisplayClass80.db;
      Proto.ID forestFloor = Ids.TerrainMaterials.ForestFloor;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto minedProduct3 = cDisplayClass80.grass.MinedProduct;
      nullable2 = new Proto.ID?(Ids.TerrainMaterials.ForestDirt);
      // ISSUE: reference to a compiler-generated field
      Percent minedQuantityMult4 = cDisplayClass80.dirt.MinedQuantityMult;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF minCollapseHeightDiff3 = cDisplayClass80.grass.MinCollapseHeightDiff + 0.1.TilesThick();
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF maxCollapseHeightDiff3 = cDisplayClass80.grass.MaxCollapseHeightDiff + 0.2.TilesThick();
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics16 = cDisplayClass80.grass.Graphics;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ImmutableArray<DetailLayerSpec> immutableArray6 = this.m_disableDetails ? ImmutableArray<DetailLayerSpec>.Empty : ImmutableArray.Create<DetailLayerSpec>(new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.ForestGrass), 0.5f), new DetailLayerSpec(cDisplayClass80.db.GetOrThrow<DetailLayerSpecProto>(Ids.TerrainDetails.DebrisFlat), 0.5f));
      float? dustiness7 = new float?();
      float? fullyWetSmoothnessDelta7 = new float?();
      float? fullyWetBrightnessDelta7 = new float?();
      ColorRgba? nullable4 = new ColorRgba?();
      ColorRgba? dustColor7 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers7 = immutableArray6;
      TerrainMaterialProto.Gfx graphics17 = graphics16.WithReplaced("Assets/Base/Terrain/Textures/ForestFloor-2048-albedo-height.png", "Assets/Base/Terrain/Textures/ForestFloor-2048-normal-s-ao.png", dustiness7, fullyWetSmoothnessDelta7, fullyWetBrightnessDelta7, dustColor7, detailLayers7);
      Proto.ID? weathersInto4 = new Proto.ID?();
      Proto.ID? disruptsInto4 = nullable2;
      Percent? disruptionSpeedMult4 = new Percent?();
      Proto.ID? recoversInto4 = new Proto.ID?();
      Duration recoveryTime4 = new Duration();
      Percent grassGrowthOnTop4 = new Percent();
      TerrainMaterialProto proto4 = new TerrainMaterialProto(forestFloor, "Forest floor", minedProduct3, minedQuantityMult4, minCollapseHeightDiff3, maxCollapseHeightDiff3, graphics17, true, true, true, weathersInto: weathersInto4, disruptsInto: disruptsInto4, disruptionSpeedMult: disruptionSpeedMult4, disruptWhenCollapsing: true, recoversInto: recoversInto4, recoveryTime: recoveryTime4, grassGrowthOnTop: grassGrowthOnTop4);
      TerrainMaterialProto terrainMaterialProto6 = db4.Add<TerrainMaterialProto>(proto4);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db5 = cDisplayClass80.db;
      Proto.ID forestDirt = Ids.TerrainMaterials.ForestDirt;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto minedProduct4 = cDisplayClass80.dirt.MinedProduct;
      nullable2 = new Proto.ID?(Ids.TerrainMaterials.ForestFloor);
      // ISSUE: reference to a compiler-generated field
      Duration disruptionRecoveryTime1 = cDisplayClass80.dirt.DisruptionRecoveryTime;
      // ISSUE: reference to a compiler-generated field
      Percent minedQuantityMult5 = cDisplayClass80.dirt.MinedQuantityMult;
      ThicknessTilesF minCollapseHeightDiff4 = terrainMaterialProto6.MinCollapseHeightDiff - 0.1.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff4 = terrainMaterialProto6.MaxCollapseHeightDiff - 0.2.TilesThick();
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics18 = cDisplayClass80.dirt.Graphics;
      Proto.ID? weathersInto5 = new Proto.ID?();
      Proto.ID? disruptsInto5 = new Proto.ID?();
      Percent? disruptionSpeedMult5 = new Percent?();
      Proto.ID? recoversInto5 = nullable2;
      Duration recoveryTime5 = disruptionRecoveryTime1;
      Percent grassGrowthOnTop5 = new Percent();
      TerrainMaterialProto proto5 = new TerrainMaterialProto(forestDirt, "Forest dirt", minedProduct4, minedQuantityMult5, minCollapseHeightDiff4, maxCollapseHeightDiff4, graphics18, true, true, true, true, weathersInto: weathersInto5, disruptsInto: disruptsInto5, disruptionSpeedMult: disruptionSpeedMult5, recoversInto: recoversInto5, recoveryTime: recoveryTime5, grassGrowthOnTop: grassGrowthOnTop5);
      db5.Add<TerrainMaterialProto>(proto5);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db6 = cDisplayClass80.db;
      Proto.ID farmGround = Ids.TerrainMaterials.FarmGround;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto minedProduct5 = cDisplayClass80.dirt.MinedProduct;
      Percent hundred1 = Percent.Hundred;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF collapseHeightDiff3 = cDisplayClass80.dirt.MinCollapseHeightDiff;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF collapseHeightDiff4 = cDisplayClass80.dirt.MaxCollapseHeightDiff;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics19 = cDisplayClass80.dirt.Graphics;
      Proto.ID? weathersInto6 = new Proto.ID?();
      Proto.ID? disruptsInto6 = new Proto.ID?();
      Percent? disruptionSpeedMult6 = new Percent?();
      Proto.ID? nullable5 = new Proto.ID?();
      Proto.ID? recoversInto6 = nullable5;
      Duration recoveryTime6 = new Duration();
      Percent grassGrowthOnTop6 = new Percent();
      TerrainMaterialProto proto6 = new TerrainMaterialProto(farmGround, "Farm ground", minedProduct5, hundred1, collapseHeightDiff3, collapseHeightDiff4, graphics19, true, ignoreInEditor: true, weathersInto: weathersInto6, disruptsInto: disruptsInto6, disruptionSpeedMult: disruptionSpeedMult6, recoversInto: recoversInto6, recoveryTime: recoveryTime6, grassGrowthOnTop: grassGrowthOnTop6);
      db6.Add<TerrainMaterialProto>(proto6);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db7 = cDisplayClass80.db;
      Proto.ID compost = Ids.TerrainMaterials.Compost;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow2 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Compost);
      nullable5 = new Proto.ID?(Ids.TerrainMaterials.GrassLush);
      // ISSUE: reference to a compiler-generated field
      Duration disruptionRecoveryTime2 = cDisplayClass80.dirt.DisruptionRecoveryTime;
      // ISSUE: reference to a compiler-generated field
      Percent minedQuantityMult6 = cDisplayClass80.dirt.MinedQuantityMult;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF collapseHeightDiff5 = cDisplayClass80.dirt.MinCollapseHeightDiff;
      // ISSUE: reference to a compiler-generated field
      ThicknessTilesF collapseHeightDiff6 = cDisplayClass80.dirt.MaxCollapseHeightDiff;
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto.Gfx graphics20 = cDisplayClass80.dirt.Graphics;
      Proto.ID? weathersInto7 = new Proto.ID?();
      Proto.ID? disruptsInto7 = new Proto.ID?();
      Percent? nullable6 = new Percent?();
      Percent? disruptionSpeedMult7 = nullable6;
      Proto.ID? recoversInto7 = nullable5;
      Duration recoveryTime7 = disruptionRecoveryTime2;
      Percent grassGrowthOnTop7 = new Percent();
      TerrainMaterialProto proto7 = new TerrainMaterialProto(compost, "Compost", orThrow2, minedQuantityMult6, collapseHeightDiff5, collapseHeightDiff6, graphics20, true, true, ignoreInEditor: true, weathersInto: weathersInto7, disruptsInto: disruptsInto7, disruptionSpeedMult: disruptionSpeedMult7, recoversInto: recoversInto7, recoveryTime: recoveryTime7, grassGrowthOnTop: grassGrowthOnTop7);
      db7.Add<TerrainMaterialProto>(proto7);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db8 = cDisplayClass80.db;
      Proto.ID rock = Ids.TerrainMaterials.Rock;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow3 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Rock);
      nullable5 = new Proto.ID?(Ids.TerrainMaterials.RockDisrupted);
      nullable6 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      Percent minedQuantityMult7 = TerrainMaterialsData.ROCK_MINED_QUANTITY_MULT;
      ThicknessTilesF minCollapseHeightDiff5 = 2.0.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff5 = 5.0.TilesThick();
      Percent hundred2 = Percent.Hundred;
      TerrainMaterialProto.Gfx graphics21 = new TerrainMaterialProto.Gfx((ColorRgba) 8421504, (ColorRgba) 6316128, "Assets/Base/Terrain/Textures/Rock-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Rock-2048-normal-s-ao.png", -0.2f, 0.5f, -0.3f, new ColorRgba(7697781, 8));
      Proto.ID? weathersInto8 = new Proto.ID?();
      Proto.ID? disruptsInto8 = nullable5;
      Percent? disruptionSpeedMult8 = nullable6;
      Proto.ID? recoversInto8 = new Proto.ID?();
      Duration recoveryTime8 = new Duration();
      Percent grassGrowthOnTop8 = hundred2;
      TerrainMaterialProto proto8 = new TerrainMaterialProto(rock, "Rock", orThrow3, minedQuantityMult7, minCollapseHeightDiff5, maxCollapseHeightDiff5, graphics21, weathersInto: weathersInto8, disruptsInto: disruptsInto8, disruptionSpeedMult: disruptionSpeedMult8, recoversInto: recoversInto8, recoveryTime: recoveryTime8, grassGrowthOnTop: grassGrowthOnTop8);
      TerrainMaterialProto terrainMaterialProto7 = db8.Add<TerrainMaterialProto>(proto8);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db9 = cDisplayClass80.db;
      Proto.ID rockDisrupted = Ids.TerrainMaterials.RockDisrupted;
      LooseProductProto minedProduct6 = terrainMaterialProto7.MinedProduct;
      Percent minedQuantityMult8 = terrainMaterialProto7.MinedQuantityMult;
      ThicknessTilesF minCollapseHeightDiff6 = 0.8.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff6 = 2.0.TilesThick();
      Percent hundred3 = Percent.Hundred;
      TerrainMaterialProto.Gfx graphics22 = terrainMaterialProto7.Graphics;
      float? dustiness8 = new float?(0.5f);
      nullable4 = new ColorRgba?(new ColorRgba(7697781, 40));
      float? fullyWetSmoothnessDelta8 = new float?();
      float? fullyWetBrightnessDelta8 = new float?();
      ColorRgba? dustColor8 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers8 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics23 = graphics22.WithReplaced("Assets/Base/Terrain/Textures/Rock-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Rock-disrupted-2048-normal-s-ao.png", dustiness8, fullyWetSmoothnessDelta8, fullyWetBrightnessDelta8, dustColor8, detailLayers8);
      Proto.ID? weathersInto9 = new Proto.ID?();
      Proto.ID? disruptsInto9 = new Proto.ID?();
      Percent? disruptionSpeedMult9 = new Percent?();
      Proto.ID? recoversInto9 = new Proto.ID?();
      Duration recoveryTime9 = new Duration();
      Percent grassGrowthOnTop9 = hundred3;
      TerrainMaterialProto proto9 = new TerrainMaterialProto(rockDisrupted, "Rock (disrupted)", minedProduct6, minedQuantityMult8, minCollapseHeightDiff6, maxCollapseHeightDiff6, graphics23, weathersInto: weathersInto9, disruptsInto: disruptsInto9, disruptionSpeedMult: disruptionSpeedMult9, recoversInto: recoversInto9, recoveryTime: recoveryTime9, grassGrowthOnTop: grassGrowthOnTop9);
      TerrainMaterialProto terrainMaterialProto8 = db9.Add<TerrainMaterialProto>(proto9);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db10 = cDisplayClass80.db;
      Proto.ID gravel = Ids.TerrainMaterials.Gravel;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow4 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Gravel);
      Percent hundred4 = Percent.Hundred;
      ThicknessTilesF minCollapseHeightDiff7 = terrainMaterialProto8.MinCollapseHeightDiff - 0.1.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff7 = terrainMaterialProto8.MaxCollapseHeightDiff - 0.4.TilesThick();
      TerrainMaterialProto.Gfx graphics24 = terrainMaterialProto8.Graphics;
      Proto.ID? weathersInto10 = new Proto.ID?();
      Proto.ID? disruptsInto10 = new Proto.ID?();
      Percent? nullable7 = new Percent?();
      Percent? disruptionSpeedMult10 = nullable7;
      Proto.ID? nullable8 = new Proto.ID?();
      Proto.ID? recoversInto10 = nullable8;
      Duration recoveryTime10 = new Duration();
      Percent grassGrowthOnTop10 = new Percent();
      TerrainMaterialProto proto10 = new TerrainMaterialProto(gravel, "Gravel", orThrow4, hundred4, minCollapseHeightDiff7, maxCollapseHeightDiff7, graphics24, ignoreInEditor: true, weathersInto: weathersInto10, disruptsInto: disruptsInto10, disruptionSpeedMult: disruptionSpeedMult10, recoversInto: recoversInto10, recoveryTime: recoveryTime10, grassGrowthOnTop: grassGrowthOnTop10);
      TerrainMaterialProto terrainMaterialProto9 = db10.Add<TerrainMaterialProto>(proto10);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db11 = cDisplayClass80.db;
      Proto.ID rockNoGrassCover = Ids.TerrainMaterials.RockNoGrassCover;
      LooseProductProto minedProduct7 = terrainMaterialProto7.MinedProduct;
      nullable8 = new Proto.ID?(Ids.TerrainMaterials.RockDisrupted);
      nullable7 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      Percent minedQuantityMult9 = terrainMaterialProto7.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff7 = terrainMaterialProto7.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff8 = terrainMaterialProto7.MaxCollapseHeightDiff;
      TerrainMaterialProto.Gfx graphics25 = terrainMaterialProto7.Graphics;
      Proto.ID? weathersInto11 = new Proto.ID?();
      Proto.ID? disruptsInto11 = nullable8;
      Percent? disruptionSpeedMult11 = nullable7;
      Proto.ID? recoversInto11 = new Proto.ID?();
      Duration recoveryTime11 = new Duration();
      Percent grassGrowthOnTop11 = new Percent();
      TerrainMaterialProto proto11 = new TerrainMaterialProto(rockNoGrassCover, "Rock (no grass cover)", minedProduct7, minedQuantityMult9, collapseHeightDiff7, collapseHeightDiff8, graphics25, weathersInto: weathersInto11, disruptsInto: disruptsInto11, disruptionSpeedMult: disruptionSpeedMult11, recoversInto: recoversInto11, recoveryTime: recoveryTime11, grassGrowthOnTop: grassGrowthOnTop11);
      db11.Add<TerrainMaterialProto>(proto11);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db12 = cDisplayClass80.db;
      Proto.ID bedrock = Ids.TerrainMaterials.Bedrock;
      LooseProductProto minedProduct8 = terrainMaterialProto7.MinedProduct;
      nullable8 = new Proto.ID?(Ids.TerrainMaterials.RockDisrupted);
      nullable7 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      Percent minedQuantityMult10 = TerrainMaterialsData.BEDROCK_MINED_QUANTITY_MULT;
      ThicknessTilesF collapseHeightDiff9 = terrainMaterialProto7.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff10 = terrainMaterialProto7.MaxCollapseHeightDiff;
      TerrainMaterialProto.Gfx graphics26 = terrainMaterialProto7.Graphics;
      Proto.ID? weathersInto12 = new Proto.ID?();
      Proto.ID? disruptsInto12 = nullable8;
      Percent? disruptionSpeedMult12 = nullable7;
      Proto.ID? recoversInto12 = new Proto.ID?();
      Duration recoveryTime12 = new Duration();
      Percent grassGrowthOnTop12 = new Percent();
      TerrainMaterialProto proto12 = new TerrainMaterialProto(bedrock, "Bedrock", minedProduct8, minedQuantityMult10, collapseHeightDiff9, collapseHeightDiff10, graphics26, ignoreInEditor: true, weathersInto: weathersInto12, disruptsInto: disruptsInto12, disruptionSpeedMult: disruptionSpeedMult12, recoversInto: recoversInto12, recoveryTime: recoveryTime12, grassGrowthOnTop: grassGrowthOnTop12);
      db12.Add<TerrainMaterialProto>(proto12);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db13 = cDisplayClass80.db;
      Proto.ID hardenedRock = Ids.TerrainMaterials.HardenedRock;
      LooseProductProto minedProduct9 = terrainMaterialProto7.MinedProduct;
      Percent hundred5 = Percent.Hundred;
      ThicknessTilesF minCollapseHeightDiff8 = 4.1.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff8 = 5.0.TilesThick();
      TerrainMaterialProto.Gfx graphics27 = terrainMaterialProto8.Graphics;
      Proto.ID? weathersInto13 = new Proto.ID?();
      Proto.ID? disruptsInto13 = new Proto.ID?();
      Percent? nullable9 = new Percent?();
      Percent? disruptionSpeedMult13 = nullable9;
      Proto.ID? nullable10 = new Proto.ID?();
      Proto.ID? recoversInto13 = nullable10;
      Duration recoveryTime13 = new Duration();
      Percent grassGrowthOnTop13 = new Percent();
      TerrainMaterialProto proto13 = new TerrainMaterialProto(hardenedRock, "Hardened rock", minedProduct9, hundred5, minCollapseHeightDiff8, maxCollapseHeightDiff8, graphics27, ignoreInEditor: true, weathersInto: weathersInto13, disruptsInto: disruptsInto13, disruptionSpeedMult: disruptionSpeedMult13, recoversInto: recoversInto13, recoveryTime: recoveryTime13, grassGrowthOnTop: grassGrowthOnTop13);
      db13.Add<TerrainMaterialProto>(proto13);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db14 = cDisplayClass80.db;
      Proto.ID sand = Ids.TerrainMaterials.Sand;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow5 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Sand);
      Percent hundred6 = Percent.Hundred;
      nullable10 = new Proto.ID?(Ids.TerrainMaterials.SandDisrupted);
      nullable9 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_SAND);
      ThicknessTilesF minCollapseHeightDiff9 = 0.8.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff9 = 1.0.TilesThick();
      TerrainMaterialProto.Gfx graphics28 = new TerrainMaterialProto.Gfx((ColorRgba) 16776960, (ColorRgba) 15121756, "Assets/Base/Terrain/Textures/Sand-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Sand-2048-normal-s-ao.png", 0.5f, fullyWetBrightnessDelta: -0.5f, dustColor: new ColorRgba(10911301, 16));
      Proto.ID? weathersInto14 = new Proto.ID?();
      Proto.ID? disruptsInto14 = nullable10;
      Percent? disruptionSpeedMult14 = nullable9;
      Proto.ID? recoversInto14 = new Proto.ID?();
      Duration recoveryTime14 = new Duration();
      Percent grassGrowthOnTop14 = new Percent();
      TerrainMaterialProto proto14 = new TerrainMaterialProto(sand, "Sand", orThrow5, hundred6, minCollapseHeightDiff9, maxCollapseHeightDiff9, graphics28, canSpreadToNearbyMaterials: true, weathersInto: weathersInto14, disruptsInto: disruptsInto14, disruptionSpeedMult: disruptionSpeedMult14, disruptWhenCollapsing: true, recoversInto: recoversInto14, recoveryTime: recoveryTime14, grassGrowthOnTop: grassGrowthOnTop14);
      TerrainMaterialProto terrainMaterialProto10 = db14.Add<TerrainMaterialProto>(proto14);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db15 = cDisplayClass80.db;
      Proto.ID sandDisrupted = Ids.TerrainMaterials.SandDisrupted;
      LooseProductProto minedProduct10 = terrainMaterialProto10.MinedProduct;
      Percent minedQuantityMult11 = terrainMaterialProto10.MinedQuantityMult;
      nullable10 = new Proto.ID?(Ids.TerrainMaterials.Sand);
      // ISSUE: reference to a compiler-generated field
      Duration duration2 = (cDisplayClass80.dirt.DisruptionRecoveryTime.Ticks / TerrainMaterialsData.DISRUPTION_SPEED_MULT_SAND.ToFix32()).ToIntCeiled().Ticks();
      ThicknessTilesF minCollapseHeightDiff10 = terrainMaterialProto10.MinCollapseHeightDiff - 0.1.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff10 = terrainMaterialProto10.MaxCollapseHeightDiff - 0.2.TilesThick();
      TerrainMaterialProto.Gfx graphics29 = terrainMaterialProto10.Graphics;
      float? dustiness9 = new float?(1f);
      nullable4 = new ColorRgba?(new ColorRgba(10911301, 40));
      float? fullyWetSmoothnessDelta9 = new float?();
      float? fullyWetBrightnessDelta9 = new float?();
      ColorRgba? dustColor9 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers9 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics30 = graphics29.WithReplaced("Assets/Base/Terrain/Textures/Sand-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Sand-disrupted-2048-normal-s-ao.png", dustiness9, fullyWetSmoothnessDelta9, fullyWetBrightnessDelta9, dustColor9, detailLayers9);
      Proto.ID? weathersInto15 = new Proto.ID?();
      Proto.ID? disruptsInto15 = new Proto.ID?();
      Percent? nullable11 = new Percent?();
      Percent? disruptionSpeedMult15 = nullable11;
      Proto.ID? recoversInto15 = nullable10;
      Duration recoveryTime15 = duration2;
      Percent grassGrowthOnTop15 = new Percent();
      TerrainMaterialProto proto15 = new TerrainMaterialProto(sandDisrupted, "Sand (disrupted)", minedProduct10, minedQuantityMult11, minCollapseHeightDiff10, maxCollapseHeightDiff10, graphics30, weathersInto: weathersInto15, disruptsInto: disruptsInto15, disruptionSpeedMult: disruptionSpeedMult15, recoversInto: recoversInto15, recoveryTime: recoveryTime15, grassGrowthOnTop: grassGrowthOnTop15);
      TerrainMaterialProto terrainMaterialProto11 = db15.Add<TerrainMaterialProto>(proto15);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db16 = cDisplayClass80.db;
      Proto.ID limestone = Ids.TerrainMaterials.Limestone;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow6 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Limestone);
      Percent hundred7 = Percent.Hundred;
      nullable10 = new Proto.ID?(Ids.TerrainMaterials.LimestoneDisrupted);
      nullable11 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      ThicknessTilesF minCollapseHeightDiff11 = 1.6.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff11 = 5.0.TilesThick();
      Percent hundred8 = Percent.Hundred;
      TerrainMaterialProto.Gfx graphics31 = new TerrainMaterialProto.Gfx((ColorRgba) 13421704, (ColorRgba) 11316376, "Assets/Base/Terrain/Textures/Limestone-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Limestone-2048-normal-s-ao.png", -0.1f, 0.3f, -0.4f, new ColorRgba(12232304, 8));
      Proto.ID? weathersInto16 = new Proto.ID?();
      Proto.ID? disruptsInto16 = nullable10;
      Percent? disruptionSpeedMult16 = nullable11;
      Proto.ID? recoversInto16 = new Proto.ID?();
      Duration recoveryTime16 = new Duration();
      Percent grassGrowthOnTop16 = hundred8;
      TerrainMaterialProto proto16 = new TerrainMaterialProto(limestone, "Limestone", orThrow6, hundred7, minCollapseHeightDiff11, maxCollapseHeightDiff11, graphics31, weathersInto: weathersInto16, disruptsInto: disruptsInto16, disruptionSpeedMult: disruptionSpeedMult16, recoversInto: recoversInto16, recoveryTime: recoveryTime16, grassGrowthOnTop: grassGrowthOnTop16);
      TerrainMaterialProto terrainMaterialProto12 = db16.Add<TerrainMaterialProto>(proto16);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db17 = cDisplayClass80.db;
      Proto.ID limestoneDisrupted = Ids.TerrainMaterials.LimestoneDisrupted;
      LooseProductProto minedProduct11 = terrainMaterialProto12.MinedProduct;
      Percent minedQuantityMult12 = terrainMaterialProto12.MinedQuantityMult;
      ThicknessTilesF minCollapseHeightDiff12 = terrainMaterialProto8.MinCollapseHeightDiff - 0.1.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff12 = terrainMaterialProto8.MaxCollapseHeightDiff - 0.4.TilesThick();
      Percent hundred9 = Percent.Hundred;
      TerrainMaterialProto.Gfx graphics32 = terrainMaterialProto12.Graphics;
      float? dustiness10 = new float?(0.6f);
      nullable4 = new ColorRgba?(new ColorRgba(12232304, 40));
      float? fullyWetSmoothnessDelta10 = new float?();
      float? fullyWetBrightnessDelta10 = new float?();
      ColorRgba? dustColor10 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers10 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics33 = graphics32.WithReplaced("Assets/Base/Terrain/Textures/Limestone-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Limestone-disrupted-2048-normal-s-ao.png", dustiness10, fullyWetSmoothnessDelta10, fullyWetBrightnessDelta10, dustColor10, detailLayers10);
      Proto.ID? weathersInto17 = new Proto.ID?();
      Proto.ID? disruptsInto17 = new Proto.ID?();
      Percent? nullable12 = new Percent?();
      Percent? disruptionSpeedMult17 = nullable12;
      Proto.ID? nullable13 = new Proto.ID?();
      Proto.ID? recoversInto17 = nullable13;
      Duration recoveryTime17 = new Duration();
      Percent grassGrowthOnTop17 = hundred9;
      TerrainMaterialProto proto17 = new TerrainMaterialProto(limestoneDisrupted, "Limestone (disrupted)", minedProduct11, minedQuantityMult12, minCollapseHeightDiff12, maxCollapseHeightDiff12, graphics33, weathersInto: weathersInto17, disruptsInto: disruptsInto17, disruptionSpeedMult: disruptionSpeedMult17, recoversInto: recoversInto17, recoveryTime: recoveryTime17, grassGrowthOnTop: grassGrowthOnTop17);
      db17.Add<TerrainMaterialProto>(proto17);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db18 = cDisplayClass80.db;
      Proto.ID quartz = Ids.TerrainMaterials.Quartz;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow7 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Quartz);
      Percent hundred10 = Percent.Hundred;
      nullable13 = new Proto.ID?(Ids.TerrainMaterials.QuartzDisrupted);
      nullable12 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      ThicknessTilesF minCollapseHeightDiff13 = 1.6.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff13 = 5.0.TilesThick();
      Percent hundred11 = Percent.Hundred;
      TerrainMaterialProto.Gfx graphics34 = new TerrainMaterialProto.Gfx((ColorRgba) 13679530, (ColorRgba) 12102045, "Assets/Base/Terrain/Textures/Quartz-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Quartz-2048-normal-s-ao.png", -0.1f, 0.5f, -0.3f, new ColorRgba(12033670, 8));
      Proto.ID? weathersInto18 = new Proto.ID?();
      Proto.ID? disruptsInto18 = nullable13;
      Percent? disruptionSpeedMult18 = nullable12;
      Proto.ID? recoversInto18 = new Proto.ID?();
      Duration recoveryTime18 = new Duration();
      Percent grassGrowthOnTop18 = hundred11;
      TerrainMaterialProto proto18 = new TerrainMaterialProto(quartz, "Quartz", orThrow7, hundred10, minCollapseHeightDiff13, maxCollapseHeightDiff13, graphics34, weathersInto: weathersInto18, disruptsInto: disruptsInto18, disruptionSpeedMult: disruptionSpeedMult18, recoversInto: recoversInto18, recoveryTime: recoveryTime18, grassGrowthOnTop: grassGrowthOnTop18);
      TerrainMaterialProto terrainMaterialProto13 = db18.Add<TerrainMaterialProto>(proto18);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db19 = cDisplayClass80.db;
      Proto.ID quartzDisrupted = Ids.TerrainMaterials.QuartzDisrupted;
      LooseProductProto minedProduct12 = terrainMaterialProto13.MinedProduct;
      Percent minedQuantityMult13 = terrainMaterialProto13.MinedQuantityMult;
      ThicknessTilesF minCollapseHeightDiff14 = terrainMaterialProto8.MinCollapseHeightDiff - 0.1.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff14 = terrainMaterialProto8.MaxCollapseHeightDiff - 0.4.TilesThick();
      Percent hundred12 = Percent.Hundred;
      TerrainMaterialProto.Gfx graphics35 = terrainMaterialProto13.Graphics;
      float? dustiness11 = new float?(0.6f);
      nullable4 = new ColorRgba?(new ColorRgba(12033670, 40));
      float? fullyWetSmoothnessDelta11 = new float?();
      float? fullyWetBrightnessDelta11 = new float?();
      ColorRgba? dustColor11 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers11 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics36 = graphics35.WithReplaced("Assets/Base/Terrain/Textures/Quartz-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Quartz-disrupted-2048-normal-s-ao.png", dustiness11, fullyWetSmoothnessDelta11, fullyWetBrightnessDelta11, dustColor11, detailLayers11);
      Proto.ID? weathersInto19 = new Proto.ID?();
      Proto.ID? disruptsInto19 = new Proto.ID?();
      Percent? disruptionSpeedMult19 = new Percent?();
      Proto.ID? recoversInto19 = new Proto.ID?();
      Duration recoveryTime19 = new Duration();
      Percent grassGrowthOnTop19 = hundred12;
      TerrainMaterialProto proto19 = new TerrainMaterialProto(quartzDisrupted, "Quartz (disrupted)", minedProduct12, minedQuantityMult13, minCollapseHeightDiff14, maxCollapseHeightDiff14, graphics36, weathersInto: weathersInto19, disruptsInto: disruptsInto19, disruptionSpeedMult: disruptionSpeedMult19, recoversInto: recoversInto19, recoveryTime: recoveryTime19, grassGrowthOnTop: grassGrowthOnTop19);
      db19.Add<TerrainMaterialProto>(proto19);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db20 = cDisplayClass80.db;
      Proto.ID quartzCrushed = Ids.TerrainMaterials.QuartzCrushed;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow8 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.QuartzCrushed);
      Percent minedQuantityMult14 = terrainMaterialProto13.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff11 = terrainMaterialProto9.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff12 = terrainMaterialProto9.MaxCollapseHeightDiff;
      TerrainMaterialProto.Gfx graphics37 = terrainMaterialProto13.Graphics;
      float? dustiness12 = new float?(1f);
      nullable4 = new ColorRgba?(new ColorRgba(12033670, 40));
      float? fullyWetSmoothnessDelta12 = new float?();
      float? fullyWetBrightnessDelta12 = new float?();
      ColorRgba? dustColor12 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers12 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics38 = graphics37.WithReplaced("Assets/Base/Terrain/Textures/Quartz-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Quartz-disrupted-2048-normal-s-ao.png", dustiness12, fullyWetSmoothnessDelta12, fullyWetBrightnessDelta12, dustColor12, detailLayers12);
      Proto.ID? weathersInto20 = new Proto.ID?();
      Proto.ID? disruptsInto20 = new Proto.ID?();
      Percent? disruptionSpeedMult20 = new Percent?();
      Proto.ID? nullable14 = new Proto.ID?();
      Proto.ID? recoversInto20 = nullable14;
      Duration recoveryTime20 = new Duration();
      Percent grassGrowthOnTop20 = new Percent();
      TerrainMaterialProto proto20 = new TerrainMaterialProto(quartzCrushed, "Quartz (crushed)", orThrow8, minedQuantityMult14, collapseHeightDiff11, collapseHeightDiff12, graphics38, ignoreInEditor: true, weathersInto: weathersInto20, disruptsInto: disruptsInto20, disruptionSpeedMult: disruptionSpeedMult20, recoversInto: recoversInto20, recoveryTime: recoveryTime20, grassGrowthOnTop: grassGrowthOnTop20);
      db20.Add<TerrainMaterialProto>(proto20);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db21 = cDisplayClass80.db;
      Proto.ID coal = Ids.TerrainMaterials.Coal;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow9 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Coal);
      Percent minedQuantityMult15 = TerrainMaterialsData.COAL_MINED_QUANTITY_MULT;
      nullable14 = new Proto.ID?(Ids.TerrainMaterials.CoalDisrupted);
      ThicknessTilesF minCollapseHeightDiff15 = 1.4.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff15 = 4.0.TilesThick();
      Percent percent1 = 80.Percent();
      TerrainMaterialProto.Gfx graphics39 = new TerrainMaterialProto.Gfx((ColorRgba) 5187584, (ColorRgba) 2105376, "Assets/Base/Terrain/Textures/Coal-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Coal-2048-normal-s-ao.png", 0.1f, 0.5f, -0.3f, new ColorRgba(3355443, 8));
      Proto.ID? weathersInto21 = new Proto.ID?();
      Proto.ID? disruptsInto21 = nullable14;
      Percent? disruptionSpeedMult21 = new Percent?();
      Proto.ID? recoversInto21 = new Proto.ID?();
      Duration recoveryTime21 = new Duration();
      Percent grassGrowthOnTop21 = percent1;
      TerrainMaterialProto proto21 = new TerrainMaterialProto(coal, "Coal", orThrow9, minedQuantityMult15, minCollapseHeightDiff15, maxCollapseHeightDiff15, graphics39, weathersInto: weathersInto21, disruptsInto: disruptsInto21, disruptionSpeedMult: disruptionSpeedMult21, recoversInto: recoversInto21, recoveryTime: recoveryTime21, grassGrowthOnTop: grassGrowthOnTop21);
      TerrainMaterialProto terrainMaterialProto14 = db21.Add<TerrainMaterialProto>(proto21);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db22 = cDisplayClass80.db;
      Proto.ID coalDisrupted = Ids.TerrainMaterials.CoalDisrupted;
      LooseProductProto minedProduct13 = terrainMaterialProto14.MinedProduct;
      Percent minedQuantityMult16 = terrainMaterialProto14.MinedQuantityMult;
      ThicknessTilesF minCollapseHeightDiff16 = 0.8.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff16 = 1.5.TilesThick();
      Percent percent2 = 80.Percent();
      TerrainMaterialProto.Gfx graphics40 = terrainMaterialProto14.Graphics;
      float? dustiness13 = new float?(0.8f);
      nullable4 = new ColorRgba?(new ColorRgba(3355443, 40));
      float? fullyWetSmoothnessDelta13 = new float?();
      float? fullyWetBrightnessDelta13 = new float?();
      ColorRgba? dustColor13 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers13 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics41 = graphics40.WithReplaced("Assets/Base/Terrain/Textures/Coal-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Coal-disrupted-2048-normal-s-ao.png", dustiness13, fullyWetSmoothnessDelta13, fullyWetBrightnessDelta13, dustColor13, detailLayers13);
      Proto.ID? weathersInto22 = new Proto.ID?();
      Proto.ID? disruptsInto22 = new Proto.ID?();
      Percent? disruptionSpeedMult22 = new Percent?();
      Proto.ID? recoversInto22 = new Proto.ID?();
      Duration recoveryTime22 = new Duration();
      Percent grassGrowthOnTop22 = percent2;
      TerrainMaterialProto proto22 = new TerrainMaterialProto(coalDisrupted, "Coal (disrupted)", minedProduct13, minedQuantityMult16, minCollapseHeightDiff16, maxCollapseHeightDiff16, graphics41, weathersInto: weathersInto22, disruptsInto: disruptsInto22, disruptionSpeedMult: disruptionSpeedMult22, recoversInto: recoversInto22, recoveryTime: recoveryTime22, grassGrowthOnTop: grassGrowthOnTop22);
      db22.Add<TerrainMaterialProto>(proto22);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      TerrainMaterialProto terrainMaterialProto15 = cDisplayClass80.db.Add<TerrainMaterialProto>(new TerrainMaterialProto(Ids.TerrainMaterials.Slag, "Slag", cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Slag), Percent.Hundred, terrainMaterialProto8.MinCollapseHeightDiff, terrainMaterialProto8.MaxCollapseHeightDiff - 0.2.TilesThick(), new TerrainMaterialProto.Gfx((ColorRgba) 6702165, (ColorRgba) 6316128, "Assets/Base/Terrain/Textures/Slag-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Slag-2048-normal-s-ao.png", 0.3f, 0.5f, -0.3f, new ColorRgba(9079434, 40))));
      // ISSUE: reference to a compiler-generated field
      ProtosDb db23 = cDisplayClass80.db;
      Proto.ID slagCrushed = Ids.TerrainMaterials.SlagCrushed;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow10 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.SlagCrushed);
      Percent hundred13 = Percent.Hundred;
      ThicknessTilesF minCollapseHeightDiff17 = terrainMaterialProto11.MinCollapseHeightDiff + 0.1.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff17 = terrainMaterialProto11.MaxCollapseHeightDiff + 0.2.TilesThick();
      TerrainMaterialProto.Gfx graphics42 = terrainMaterialProto15.Graphics;
      float? dustiness14 = new float?(0.5f);
      nullable4 = new ColorRgba?(new ColorRgba(3355443, 40));
      float? fullyWetSmoothnessDelta14 = new float?();
      float? fullyWetBrightnessDelta14 = new float?();
      ColorRgba? dustColor14 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers14 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics43 = graphics42.WithReplaced("Assets/Base/Terrain/Textures/Slag-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Slag-2048-normal-s-ao.png", dustiness14, fullyWetSmoothnessDelta14, fullyWetBrightnessDelta14, dustColor14, detailLayers14);
      Proto.ID? weathersInto23 = new Proto.ID?();
      Proto.ID? disruptsInto23 = new Proto.ID?();
      Percent? nullable15 = new Percent?();
      Percent? disruptionSpeedMult23 = nullable15;
      Proto.ID? nullable16 = new Proto.ID?();
      Proto.ID? recoversInto23 = nullable16;
      Duration recoveryTime23 = new Duration();
      Percent grassGrowthOnTop23 = new Percent();
      TerrainMaterialProto proto23 = new TerrainMaterialProto(slagCrushed, "Slag (crushed)", orThrow10, hundred13, minCollapseHeightDiff17, maxCollapseHeightDiff17, graphics43, ignoreInEditor: true, weathersInto: weathersInto23, disruptsInto: disruptsInto23, disruptionSpeedMult: disruptionSpeedMult23, recoversInto: recoversInto23, recoveryTime: recoveryTime23, grassGrowthOnTop: grassGrowthOnTop23);
      db23.Add<TerrainMaterialProto>(proto23);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db24 = cDisplayClass80.db;
      Proto.ID ironOre = Ids.TerrainMaterials.IronOre;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow11 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.IronOre);
      Percent hundred14 = Percent.Hundred;
      nullable16 = new Proto.ID?(Ids.TerrainMaterials.IronOreDisrupted);
      nullable15 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      ThicknessTilesF collapseHeightDiff13 = terrainMaterialProto7.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff14 = terrainMaterialProto7.MaxCollapseHeightDiff;
      Percent fifty1 = Percent.Fifty;
      TerrainMaterialProto.Gfx graphics44 = new TerrainMaterialProto.Gfx((ColorRgba) 16731960, (ColorRgba) 13460280, "Assets/Base/Terrain/Textures/IronOre-2048-albedo-height.png", "Assets/Base/Terrain/Textures/IronOre-2048-normal-s-ao.png", -0.3f, 0.5f, -0.3f, new ColorRgba(11356456, 8));
      Proto.ID? weathersInto24 = new Proto.ID?();
      Proto.ID? disruptsInto24 = nullable16;
      Percent? disruptionSpeedMult24 = nullable15;
      Proto.ID? recoversInto24 = new Proto.ID?();
      Duration recoveryTime24 = new Duration();
      Percent grassGrowthOnTop24 = fifty1;
      TerrainMaterialProto proto24 = new TerrainMaterialProto(ironOre, "Iron ore", orThrow11, hundred14, collapseHeightDiff13, collapseHeightDiff14, graphics44, weathersInto: weathersInto24, disruptsInto: disruptsInto24, disruptionSpeedMult: disruptionSpeedMult24, recoversInto: recoversInto24, recoveryTime: recoveryTime24, grassGrowthOnTop: grassGrowthOnTop24);
      TerrainMaterialProto terrainMaterialProto16 = db24.Add<TerrainMaterialProto>(proto24);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db25 = cDisplayClass80.db;
      Proto.ID ironOreDisrupted = Ids.TerrainMaterials.IronOreDisrupted;
      LooseProductProto minedProduct14 = terrainMaterialProto16.MinedProduct;
      Percent minedQuantityMult17 = terrainMaterialProto16.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff15 = terrainMaterialProto8.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff16 = terrainMaterialProto8.MaxCollapseHeightDiff;
      Percent fifty2 = Percent.Fifty;
      TerrainMaterialProto.Gfx graphics45 = terrainMaterialProto16.Graphics;
      float? dustiness15 = new float?(0.7f);
      nullable4 = new ColorRgba?(new ColorRgba(11356456, 40));
      float? fullyWetSmoothnessDelta15 = new float?();
      float? fullyWetBrightnessDelta15 = new float?();
      ColorRgba? dustColor15 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers15 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics46 = graphics45.WithReplaced("Assets/Base/Terrain/Textures/IronOre-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/IronOre-disrupted-2048-normal-s-ao.png", dustiness15, fullyWetSmoothnessDelta15, fullyWetBrightnessDelta15, dustColor15, detailLayers15);
      Proto.ID? weathersInto25 = new Proto.ID?();
      Proto.ID? disruptsInto25 = new Proto.ID?();
      Percent? disruptionSpeedMult25 = new Percent?();
      Proto.ID? recoversInto25 = new Proto.ID?();
      Duration recoveryTime25 = new Duration();
      Percent grassGrowthOnTop25 = fifty2;
      TerrainMaterialProto proto25 = new TerrainMaterialProto(ironOreDisrupted, "Iron ore (disrupted)", minedProduct14, minedQuantityMult17, collapseHeightDiff15, collapseHeightDiff16, graphics46, weathersInto: weathersInto25, disruptsInto: disruptsInto25, disruptionSpeedMult: disruptionSpeedMult25, recoversInto: recoversInto25, recoveryTime: recoveryTime25, grassGrowthOnTop: grassGrowthOnTop25);
      db25.Add<TerrainMaterialProto>(proto25);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db26 = cDisplayClass80.db;
      Proto.ID ironOreCrushed = Ids.TerrainMaterials.IronOreCrushed;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow12 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.IronOreCrushed);
      Percent minedQuantityMult18 = terrainMaterialProto16.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff17 = terrainMaterialProto9.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff18 = terrainMaterialProto9.MaxCollapseHeightDiff;
      TerrainMaterialProto.Gfx graphics47 = terrainMaterialProto16.Graphics;
      float? dustiness16 = new float?(1f);
      nullable4 = new ColorRgba?(new ColorRgba(11356456, 40));
      float? fullyWetSmoothnessDelta16 = new float?();
      float? fullyWetBrightnessDelta16 = new float?();
      ColorRgba? dustColor16 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers16 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics48 = graphics47.WithReplaced("Assets/Base/Terrain/Textures/IronOre-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/IronOre-disrupted-2048-normal-s-ao.png", dustiness16, fullyWetSmoothnessDelta16, fullyWetBrightnessDelta16, dustColor16, detailLayers16);
      Proto.ID? weathersInto26 = new Proto.ID?();
      Proto.ID? disruptsInto26 = new Proto.ID?();
      Percent? nullable17 = new Percent?();
      Percent? disruptionSpeedMult26 = nullable17;
      Proto.ID? nullable18 = new Proto.ID?();
      Proto.ID? recoversInto26 = nullable18;
      Duration recoveryTime26 = new Duration();
      Percent grassGrowthOnTop26 = new Percent();
      TerrainMaterialProto proto26 = new TerrainMaterialProto(ironOreCrushed, "Iron ore (crushed)", orThrow12, minedQuantityMult18, collapseHeightDiff17, collapseHeightDiff18, graphics48, ignoreInEditor: true, weathersInto: weathersInto26, disruptsInto: disruptsInto26, disruptionSpeedMult: disruptionSpeedMult26, recoversInto: recoversInto26, recoveryTime: recoveryTime26, grassGrowthOnTop: grassGrowthOnTop26);
      db26.Add<TerrainMaterialProto>(proto26);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db27 = cDisplayClass80.db;
      Proto.ID copperOre = Ids.TerrainMaterials.CopperOre;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow13 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.CopperOre);
      Percent hundred15 = Percent.Hundred;
      nullable18 = new Proto.ID?(Ids.TerrainMaterials.CopperOreDisrupted);
      nullable17 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      ThicknessTilesF collapseHeightDiff19 = terrainMaterialProto7.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff20 = terrainMaterialProto7.MaxCollapseHeightDiff;
      Percent fifty3 = Percent.Fifty;
      TerrainMaterialProto.Gfx graphics49 = new TerrainMaterialProto.Gfx((ColorRgba) 4251856, (ColorRgba) 3177319, "Assets/Base/Terrain/Textures/CopperOre-2048-albedo-height.png", "Assets/Base/Terrain/Textures/CopperOre-2048-normal-s-ao.png", -0.3f, 0.5f, -0.3f, new ColorRgba(2125145, 8));
      Proto.ID? weathersInto27 = new Proto.ID?();
      Proto.ID? disruptsInto27 = nullable18;
      Percent? disruptionSpeedMult27 = nullable17;
      Proto.ID? recoversInto27 = new Proto.ID?();
      Duration recoveryTime27 = new Duration();
      Percent grassGrowthOnTop27 = fifty3;
      TerrainMaterialProto proto27 = new TerrainMaterialProto(copperOre, "Copper ore", orThrow13, hundred15, collapseHeightDiff19, collapseHeightDiff20, graphics49, weathersInto: weathersInto27, disruptsInto: disruptsInto27, disruptionSpeedMult: disruptionSpeedMult27, recoversInto: recoversInto27, recoveryTime: recoveryTime27, grassGrowthOnTop: grassGrowthOnTop27);
      TerrainMaterialProto terrainMaterialProto17 = db27.Add<TerrainMaterialProto>(proto27);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db28 = cDisplayClass80.db;
      Proto.ID copperOreDisrupted = Ids.TerrainMaterials.CopperOreDisrupted;
      LooseProductProto minedProduct15 = terrainMaterialProto17.MinedProduct;
      Percent minedQuantityMult19 = terrainMaterialProto17.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff21 = terrainMaterialProto8.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff22 = terrainMaterialProto8.MaxCollapseHeightDiff;
      Percent fifty4 = Percent.Fifty;
      TerrainMaterialProto.Gfx graphics50 = terrainMaterialProto17.Graphics;
      float? dustiness17 = new float?(0.7f);
      nullable4 = new ColorRgba?(new ColorRgba(2125145, 40));
      float? fullyWetSmoothnessDelta17 = new float?();
      float? fullyWetBrightnessDelta17 = new float?();
      ColorRgba? dustColor17 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers17 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics51 = graphics50.WithReplaced("Assets/Base/Terrain/Textures/CopperOre-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/CopperOre-disrupted-2048-normal-s-ao.png", dustiness17, fullyWetSmoothnessDelta17, fullyWetBrightnessDelta17, dustColor17, detailLayers17);
      Proto.ID? weathersInto28 = new Proto.ID?();
      Proto.ID? disruptsInto28 = new Proto.ID?();
      nullable17 = new Percent?();
      Percent? disruptionSpeedMult28 = nullable17;
      Proto.ID? recoversInto28 = new Proto.ID?();
      Duration recoveryTime28 = new Duration();
      Percent grassGrowthOnTop28 = fifty4;
      TerrainMaterialProto proto28 = new TerrainMaterialProto(copperOreDisrupted, "Copper ore (disrupted)", minedProduct15, minedQuantityMult19, collapseHeightDiff21, collapseHeightDiff22, graphics51, weathersInto: weathersInto28, disruptsInto: disruptsInto28, disruptionSpeedMult: disruptionSpeedMult28, recoversInto: recoversInto28, recoveryTime: recoveryTime28, grassGrowthOnTop: grassGrowthOnTop28);
      db28.Add<TerrainMaterialProto>(proto28);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db29 = cDisplayClass80.db;
      Proto.ID copperOreCrushed = Ids.TerrainMaterials.CopperOreCrushed;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow14 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.CopperOreCrushed);
      Percent minedQuantityMult20 = terrainMaterialProto17.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff23 = terrainMaterialProto9.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff24 = terrainMaterialProto9.MaxCollapseHeightDiff;
      TerrainMaterialProto.Gfx graphics52 = terrainMaterialProto17.Graphics;
      float? dustiness18 = new float?(1f);
      nullable4 = new ColorRgba?(new ColorRgba(2125145, 40));
      float? fullyWetSmoothnessDelta18 = new float?();
      float? fullyWetBrightnessDelta18 = new float?();
      ColorRgba? dustColor18 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers18 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics53 = graphics52.WithReplaced("Assets/Base/Terrain/Textures/CopperOre-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/CopperOre-disrupted-2048-normal-s-ao.png", dustiness18, fullyWetSmoothnessDelta18, fullyWetBrightnessDelta18, dustColor18, detailLayers18);
      Proto.ID? weathersInto29 = new Proto.ID?();
      Proto.ID? disruptsInto29 = new Proto.ID?();
      nullable17 = new Percent?();
      Percent? disruptionSpeedMult29 = nullable17;
      Proto.ID? nullable19 = new Proto.ID?();
      Proto.ID? recoversInto29 = nullable19;
      Duration recoveryTime29 = new Duration();
      Percent grassGrowthOnTop29 = new Percent();
      TerrainMaterialProto proto29 = new TerrainMaterialProto(copperOreCrushed, "Copper ore (crushed)", orThrow14, minedQuantityMult20, collapseHeightDiff23, collapseHeightDiff24, graphics53, ignoreInEditor: true, weathersInto: weathersInto29, disruptsInto: disruptsInto29, disruptionSpeedMult: disruptionSpeedMult29, recoversInto: recoversInto29, recoveryTime: recoveryTime29, grassGrowthOnTop: grassGrowthOnTop29);
      db29.Add<TerrainMaterialProto>(proto29);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db30 = cDisplayClass80.db;
      Proto.ID goldOre = Ids.TerrainMaterials.GoldOre;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow15 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.GoldOre);
      Percent hundred16 = Percent.Hundred;
      nullable19 = new Proto.ID?(Ids.TerrainMaterials.GoldOreDisrupted);
      nullable17 = new Percent?(TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK);
      ThicknessTilesF collapseHeightDiff25 = terrainMaterialProto7.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff26 = terrainMaterialProto7.MaxCollapseHeightDiff;
      Percent fifty5 = Percent.Fifty;
      TerrainMaterialProto.Gfx graphics54 = new TerrainMaterialProto.Gfx((ColorRgba) 16760576, (ColorRgba) 15516236, "Assets/Base/Terrain/Textures/GoldOre-2048-albedo-height.png", "Assets/Base/Terrain/Textures/GoldOre-2048-normal-s-ao.png", -0.3f, 0.5f, -0.3f, new ColorRgba(9406568, 8));
      Proto.ID? weathersInto30 = new Proto.ID?();
      Proto.ID? disruptsInto30 = nullable19;
      Percent? disruptionSpeedMult30 = nullable17;
      Proto.ID? recoversInto30 = new Proto.ID?();
      Duration recoveryTime30 = new Duration();
      Percent grassGrowthOnTop30 = fifty5;
      TerrainMaterialProto proto30 = new TerrainMaterialProto(goldOre, "Gold ore", orThrow15, hundred16, collapseHeightDiff25, collapseHeightDiff26, graphics54, weathersInto: weathersInto30, disruptsInto: disruptsInto30, disruptionSpeedMult: disruptionSpeedMult30, recoversInto: recoversInto30, recoveryTime: recoveryTime30, grassGrowthOnTop: grassGrowthOnTop30);
      TerrainMaterialProto terrainMaterialProto18 = db30.Add<TerrainMaterialProto>(proto30);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db31 = cDisplayClass80.db;
      Proto.ID goldOreDisrupted = Ids.TerrainMaterials.GoldOreDisrupted;
      LooseProductProto minedProduct16 = terrainMaterialProto18.MinedProduct;
      Percent minedQuantityMult21 = terrainMaterialProto18.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff27 = terrainMaterialProto8.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff28 = terrainMaterialProto8.MaxCollapseHeightDiff;
      Percent fifty6 = Percent.Fifty;
      TerrainMaterialProto.Gfx graphics55 = terrainMaterialProto18.Graphics;
      float? dustiness19 = new float?(0.7f);
      nullable4 = new ColorRgba?(new ColorRgba(9406568, 40));
      float? fullyWetSmoothnessDelta19 = new float?();
      float? fullyWetBrightnessDelta19 = new float?();
      ColorRgba? dustColor19 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers19 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics56 = graphics55.WithReplaced("Assets/Base/Terrain/Textures/GoldOre-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/GoldOre-disrupted-2048-normal-s-ao.png", dustiness19, fullyWetSmoothnessDelta19, fullyWetBrightnessDelta19, dustColor19, detailLayers19);
      Proto.ID? weathersInto31 = new Proto.ID?();
      Proto.ID? disruptsInto31 = new Proto.ID?();
      nullable17 = new Percent?();
      Percent? disruptionSpeedMult31 = nullable17;
      Proto.ID? recoversInto31 = new Proto.ID?();
      Duration recoveryTime31 = new Duration();
      Percent grassGrowthOnTop31 = fifty6;
      TerrainMaterialProto proto31 = new TerrainMaterialProto(goldOreDisrupted, "Gold ore (disrupted)", minedProduct16, minedQuantityMult21, collapseHeightDiff27, collapseHeightDiff28, graphics56, weathersInto: weathersInto31, disruptsInto: disruptsInto31, disruptionSpeedMult: disruptionSpeedMult31, recoversInto: recoversInto31, recoveryTime: recoveryTime31, grassGrowthOnTop: grassGrowthOnTop31);
      db31.Add<TerrainMaterialProto>(proto31);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db32 = cDisplayClass80.db;
      Proto.ID goldOreCrushed = Ids.TerrainMaterials.GoldOreCrushed;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow16 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.GoldOreCrushed);
      Percent minedQuantityMult22 = terrainMaterialProto18.MinedQuantityMult;
      ThicknessTilesF collapseHeightDiff29 = terrainMaterialProto9.MinCollapseHeightDiff;
      ThicknessTilesF collapseHeightDiff30 = terrainMaterialProto9.MaxCollapseHeightDiff;
      TerrainMaterialProto.Gfx graphics57 = terrainMaterialProto18.Graphics;
      float? dustiness20 = new float?(1f);
      nullable4 = new ColorRgba?(new ColorRgba(9406568, 40));
      float? fullyWetSmoothnessDelta20 = new float?();
      float? fullyWetBrightnessDelta20 = new float?();
      ColorRgba? dustColor20 = nullable4;
      ImmutableArray<DetailLayerSpec> detailLayers20 = new ImmutableArray<DetailLayerSpec>();
      TerrainMaterialProto.Gfx graphics58 = graphics57.WithReplaced("Assets/Base/Terrain/Textures/GoldOre-disrupted-2048-albedo-height.png", "Assets/Base/Terrain/Textures/GoldOre-disrupted-2048-normal-s-ao.png", dustiness20, fullyWetSmoothnessDelta20, fullyWetBrightnessDelta20, dustColor20, detailLayers20);
      Proto.ID? weathersInto32 = new Proto.ID?();
      Proto.ID? disruptsInto32 = new Proto.ID?();
      nullable17 = new Percent?();
      Percent? disruptionSpeedMult32 = nullable17;
      Proto.ID? nullable20 = new Proto.ID?();
      Proto.ID? recoversInto32 = nullable20;
      Duration recoveryTime32 = new Duration();
      Percent grassGrowthOnTop32 = new Percent();
      TerrainMaterialProto proto32 = new TerrainMaterialProto(goldOreCrushed, "Gold ore (crushed)", orThrow16, minedQuantityMult22, collapseHeightDiff29, collapseHeightDiff30, graphics58, ignoreInEditor: true, weathersInto: weathersInto32, disruptsInto: disruptsInto32, disruptionSpeedMult: disruptionSpeedMult32, recoversInto: recoversInto32, recoveryTime: recoveryTime32, grassGrowthOnTop: grassGrowthOnTop32);
      db32.Add<TerrainMaterialProto>(proto32);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db33 = cDisplayClass80.db;
      Proto.ID landfill = Ids.TerrainMaterials.Landfill;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow17 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.Waste);
      Percent hundred17 = Percent.Hundred;
      nullable20 = new Proto.ID?(Ids.TerrainMaterials.LandfillOld);
      Duration duration3 = 4.Years();
      ThicknessTilesF minCollapseHeightDiff18 = 0.4.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff18 = 1.5.TilesThick();
      TerrainMaterialProto.Gfx graphics59 = new TerrainMaterialProto.Gfx((ColorRgba) 7820646, (ColorRgba) 7820646, "Assets/Base/Terrain/Textures/Landfill-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Landfill-2048-normal-s-ao.png", 0.5f, 0.1f, -0.4f);
      Proto.ID? weathersInto33 = new Proto.ID?();
      Proto.ID? disruptsInto33 = new Proto.ID?();
      nullable17 = new Percent?();
      Percent? disruptionSpeedMult33 = nullable17;
      Proto.ID? recoversInto33 = nullable20;
      Duration recoveryTime33 = duration3;
      Percent grassGrowthOnTop33 = new Percent();
      TerrainMaterialProto proto33 = new TerrainMaterialProto(landfill, "Landfill", orThrow17, hundred17, minCollapseHeightDiff18, maxCollapseHeightDiff18, graphics59, ignoreInEditor: true, weathersInto: weathersInto33, disruptsInto: disruptsInto33, disruptionSpeedMult: disruptionSpeedMult33, recoversInto: recoversInto33, recoversUnderWater: true, recoveryTime: recoveryTime33, grassGrowthOnTop: grassGrowthOnTop33);
      TerrainMaterialProto terrainMaterialProto19 = db33.Add<TerrainMaterialProto>(proto33);
      terrainMaterialProto19.AddParam((IProtoParam) UnstableTerrainMaterialParam.Instance);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db34 = cDisplayClass80.db;
      Proto.ID landfillOld = Ids.TerrainMaterials.LandfillOld;
      LooseProductProto minedProduct17 = terrainMaterialProto19.MinedProduct;
      Percent minedQuantityMult23 = terrainMaterialProto19.MinedQuantityMult;
      ThicknessTilesF minCollapseHeightDiff19 = terrainMaterialProto19.MinCollapseHeightDiff + 0.2.TilesThick();
      ThicknessTilesF maxCollapseHeightDiff19 = terrainMaterialProto19.MaxCollapseHeightDiff + 0.5.TilesThick();
      TerrainMaterialProto.Gfx graphics60 = terrainMaterialProto19.Graphics.WithReplaced("Assets/Base/Terrain/Textures/Landfill-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Landfill-2048-normal-s-ao.png");
      Proto.ID? weathersInto34 = new Proto.ID?();
      Proto.ID? disruptsInto34 = new Proto.ID?();
      nullable17 = new Percent?();
      Percent? disruptionSpeedMult34 = nullable17;
      Proto.ID? recoversInto34 = new Proto.ID?();
      Duration recoveryTime34 = new Duration();
      Percent grassGrowthOnTop34 = new Percent();
      TerrainMaterialProto proto34 = new TerrainMaterialProto(landfillOld, "Landfill (weathered)", minedProduct17, minedQuantityMult23, minCollapseHeightDiff19, maxCollapseHeightDiff19, graphics60, weathersInto: weathersInto34, disruptsInto: disruptsInto34, disruptionSpeedMult: disruptionSpeedMult34, recoversInto: recoversInto34, recoveryTime: recoveryTime34, grassGrowthOnTop: grassGrowthOnTop34);
      db34.Add<TerrainMaterialProto>(proto34).AddParam((IProtoParam) UnstableTerrainMaterialParam.Instance);
      // ISSUE: reference to a compiler-generated field
      ProtosDb db35 = cDisplayClass80.db;
      Proto.ID uraniumDepleted = Ids.TerrainMaterials.UraniumDepleted;
      // ISSUE: reference to a compiler-generated field
      LooseProductProto orThrow18 = cDisplayClass80.db.GetOrThrow<LooseProductProto>((Proto.ID) Ids.Products.UraniumDepleted);
      Percent hundred18 = Percent.Hundred;
      ThicknessTilesF collapseHeightDiff31 = terrainMaterialProto8.MinCollapseHeightDiff;
      ThicknessTilesF maxCollapseHeightDiff20 = terrainMaterialProto8.MaxCollapseHeightDiff - 0.2.TilesThick();
      TerrainMaterialProto.Gfx graphics61 = new TerrainMaterialProto.Gfx((ColorRgba) 6975060, (ColorRgba) 5922369, "Assets/Base/Terrain/Textures/Uranium-2048-albedo-height.png", "Assets/Base/Terrain/Textures/Uranium-2048-normal-s-ao.png", 0.2f, 0.5f, -0.3f);
      Proto.ID? weathersInto35 = new Proto.ID?();
      Proto.ID? disruptsInto35 = new Proto.ID?();
      nullable17 = new Percent?();
      Percent? disruptionSpeedMult35 = nullable17;
      Proto.ID? recoversInto35 = new Proto.ID?();
      Duration recoveryTime35 = new Duration();
      Percent grassGrowthOnTop35 = new Percent();
      TerrainMaterialProto proto35 = new TerrainMaterialProto(uraniumDepleted, "Depleted uranium", orThrow18, hundred18, collapseHeightDiff31, maxCollapseHeightDiff20, graphics61, weathersInto: weathersInto35, disruptsInto: disruptsInto35, disruptionSpeedMult: disruptionSpeedMult35, recoversInto: recoversInto35, recoveryTime: recoveryTime35, grassGrowthOnTop: grassGrowthOnTop35);
      db35.Add<TerrainMaterialProto>(proto35);
    }

    public TerrainMaterialsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TerrainMaterialsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TerrainMaterialsData.DIRT_MINED_QUANTITY_MULT = 70.Percent();
      TerrainMaterialsData.ROCK_MINED_QUANTITY_MULT = 80.Percent();
      TerrainMaterialsData.COAL_MINED_QUANTITY_MULT = 120.Percent();
      TerrainMaterialsData.BEDROCK_MINED_QUANTITY_MULT = 200.Percent();
      TerrainMaterialsData.DISRUPTION_SPEED_MULT_SAND = 200.Percent();
      TerrainMaterialsData.DISRUPTION_SPEED_MULT_ROCK = 50.Percent();
    }
  }
}
