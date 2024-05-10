// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.World.StaticWorldMap
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Fleet;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Prototypes.World
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class StaticWorldMap : IWorldMapGenerator
  {
    private readonly ProtosDb m_protosDb;
    private readonly RandomProvider m_randomProvider;

    public StaticWorldMap(ProtosDb protosDb, RandomProvider randomProvider)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_randomProvider = randomProvider;
    }

    public Mafi.Core.World.WorldMap CreateWorldMap()
    {
      Mafi.Core.World.WorldMap map = new Mafi.Core.World.WorldMap();
      ProtosDb protosDb = this.m_protosDb;
      WorldMapLocation worldMapLocation1 = new WorldMapLocation("Home island", new Vector2i(1860, 2717));
      map.AddLocation(worldMapLocation1);
      WorldMapLocation worldMapLocation2 = new WorldMapLocation("Location 2", new Vector2i(1688, 2626));
      map.AddLocation(worldMapLocation2);
      WorldMapLocation worldMapLocation3 = new WorldMapLocation("Island", new Vector2i(1846, 2912));
      map.AddLocation(worldMapLocation3);
      WorldMapLocation worldMapLocation4 = new WorldMapLocation("Location 59", new Vector2i(2031, 2611));
      map.AddLocation(worldMapLocation4);
      WorldMapLocation worldMapLocation5 = new WorldMapLocation("Location 3", new Vector2i(1911, 2498));
      map.AddLocation(worldMapLocation5);
      WorldMapLocation worldMapLocation6 = new WorldMapLocation("Location 50", new Vector2i(2095, 2766));
      map.AddLocation(worldMapLocation6);
      WorldMapLocation worldMapLocation7 = new WorldMapLocation("Location 64", new Vector2i(1748, 2483));
      map.AddLocation(worldMapLocation7);
      WorldMapLocation worldMapLocation8 = new WorldMapLocation("Location 52", new Vector2i(1667, 2942));
      map.AddLocation(worldMapLocation8);
      WorldMapLocation worldMapLocation9 = new WorldMapLocation("Location 53", new Vector2i(2189, 2635));
      map.AddLocation(worldMapLocation9);
      WorldMapLocation worldMapLocation10 = new WorldMapLocation("Location 60", new Vector2i(1866, 2326));
      map.AddLocation(worldMapLocation10);
      WorldMapLocation worldMapLocation11 = new WorldMapLocation("Location 63", new Vector2i(1930, 3107));
      map.AddLocation(worldMapLocation11);
      WorldMapLocation worldMapLocation12 = new WorldMapLocation("Location 61", new Vector2i(1589, 2854));
      map.AddLocation(worldMapLocation12);
      WorldMapLocation worldMapLocation13 = new WorldMapLocation("Location 51", new Vector2i(2193, 2948));
      map.AddLocation(worldMapLocation13);
      WorldMapLocation worldMapLocation14 = new WorldMapLocation("Location 4", new Vector2i(1474, 2492));
      map.AddLocation(worldMapLocation14);
      WorldMapLocation worldMapLocation15 = new WorldMapLocation("Location 49", new Vector2i(1651, 2370));
      map.AddLocation(worldMapLocation15);
      WorldMapLocation worldMapLocation16 = new WorldMapLocation("Location 12", new Vector2i(2095, 2252));
      map.AddLocation(worldMapLocation16);
      WorldMapLocation worldMapLocation17 = new WorldMapLocation("Location 49", new Vector2i(2301, 2524));
      map.AddLocation(worldMapLocation17);
      WorldMapLocation worldMapLocation18 = new WorldMapLocation("Location 58", new Vector2i(2126, 2471));
      map.AddLocation(worldMapLocation18);
      WorldMapLocation worldMapLocation19 = new WorldMapLocation("Location 56", new Vector2i(2345, 2728));
      map.AddLocation(worldMapLocation19);
      WorldMapLocation worldMapLocation20 = new WorldMapLocation("Location 62", new Vector2i(2352, 2895));
      map.AddLocation(worldMapLocation20);
      WorldMapLocation worldMapLocation21 = new WorldMapLocation("Location 66", new Vector2i(1681, 2261));
      map.AddLocation(worldMapLocation21);
      WorldMapLocation worldMapLocation22 = new WorldMapLocation("Location 47", new Vector2i(2093, 3110));
      map.AddLocation(worldMapLocation22);
      WorldMapLocation worldMapLocation23 = new WorldMapLocation("Location 27", new Vector2i(2361, 3052));
      map.AddLocation(worldMapLocation23);
      WorldMapLocation worldMapLocation24 = new WorldMapLocation("Location 57", new Vector2i(1276, 2514));
      map.AddLocation(worldMapLocation24);
      WorldMapLocation worldMapLocation25 = new WorldMapLocation("Location 48", new Vector2i(1334, 2346));
      map.AddLocation(worldMapLocation25);
      WorldMapLocation worldMapLocation26 = new WorldMapLocation("Location 65", new Vector2i(1455, 3016));
      map.AddLocation(worldMapLocation26);
      WorldMapLocation worldMapLocation27 = new WorldMapLocation("Location 13", new Vector2i(1334, 2658));
      map.AddLocation(worldMapLocation27);
      WorldMapLocation worldMapLocation28 = new WorldMapLocation("Location 39", new Vector2i(2447, 2420));
      map.AddLocation(worldMapLocation28);
      WorldMapLocation worldMapLocation29 = new WorldMapLocation("Location 7", new Vector2i(1849, 2158));
      map.AddLocation(worldMapLocation29);
      WorldMapLocation worldMapLocation30 = new WorldMapLocation("Location 69", new Vector2i(2361, 2223));
      map.AddLocation(worldMapLocation30);
      WorldMapLocation worldMapLocation31 = new WorldMapLocation("Location 51", new Vector2i(1442, 2803));
      map.AddLocation(worldMapLocation31);
      WorldMapLocation worldMapLocation32 = new WorldMapLocation("Location 28", new Vector2i(2515, 3202));
      map.AddLocation(worldMapLocation32);
      WorldMapLocation worldMapLocation33 = new WorldMapLocation("Location 9", new Vector2i(1416, 2113));
      map.AddLocation(worldMapLocation33);
      WorldMapLocation worldMapLocation34 = new WorldMapLocation("Location 68", new Vector2i(1129, 2645));
      map.AddLocation(worldMapLocation34);
      WorldMapLocation worldMapLocation35 = new WorldMapLocation("Location 54", new Vector2i(2588, 3030));
      map.AddLocation(worldMapLocation35);
      WorldMapLocation worldMapLocation36 = new WorldMapLocation("Location 67", new Vector2i(2590, 2333));
      map.AddLocation(worldMapLocation36);
      WorldMapLocation worldMapLocation37 = new WorldMapLocation("Location 25", new Vector2i(2306, 3312));
      map.AddLocation(worldMapLocation37);
      WorldMapLocation worldMapLocation38 = new WorldMapLocation("Location 40", new Vector2i(2679, 2454));
      map.AddLocation(worldMapLocation38);
      WorldMapLocation worldMapLocation39 = new WorldMapLocation("Location 14", new Vector2i(1063, 2814));
      map.AddLocation(worldMapLocation39);
      WorldMapLocation worldMapLocation40 = new WorldMapLocation("Location 8", new Vector2i(1896, 1966));
      map.AddLocation(worldMapLocation40);
      WorldMapLocation worldMapLocation41 = new WorldMapLocation("Location 26", new Vector2i(2465, 3362));
      map.AddLocation(worldMapLocation41);
      WorldMapLocation worldMapLocation42 = new WorldMapLocation("Location 6", new Vector2i(1587, 2037));
      map.AddLocation(worldMapLocation42);
      WorldMapLocation worldMapLocation43 = new WorldMapLocation("Location 23", new Vector2i(2137, 3273));
      map.AddLocation(worldMapLocation43);
      WorldMapLocation worldMapLocation44 = new WorldMapLocation("Location 24", new Vector2i(2209, 3466));
      map.AddLocation(worldMapLocation44);
      WorldMapLocation worldMapLocation45 = new WorldMapLocation("Location 52", new Vector2i(2749, 3180));
      map.AddLocation(worldMapLocation45);
      WorldMapLocation worldMapLocation46 = new WorldMapLocation("Location 15", new Vector2i(1134, 2979));
      map.AddLocation(worldMapLocation46);
      WorldMapLocation worldMapLocation47 = new WorldMapLocation("Location 50", new Vector2i(2639, 2645));
      map.AddLocation(worldMapLocation47);
      WorldMapLocation worldMapLocation48 = new WorldMapLocation("Location 53", new Vector2i(868, 2730));
      map.AddLocation(worldMapLocation48);
      WorldMapLocation worldMapLocation49 = new WorldMapLocation("Location 55", new Vector2i(2912, 2495));
      map.AddLocation(worldMapLocation49);
      WorldMapLocation worldMapLocation50 = new WorldMapLocation("Location 31", new Vector2i(2087, 3564));
      map.AddLocation(worldMapLocation50);
      WorldMapLocation worldMapLocation51 = new WorldMapLocation("Location 16", new Vector2i(1107, 3126));
      map.AddLocation(worldMapLocation51);
      WorldMapLocation worldMapLocation52 = new WorldMapLocation("Location 21", new Vector2i(1911, 3280));
      map.AddLocation(worldMapLocation52);
      WorldMapLocation worldMapLocation53 = new WorldMapLocation("Location 30", new Vector2i(2692, 3618));
      map.AddLocation(worldMapLocation53);
      WorldMapLocation worldMapLocation54 = new WorldMapLocation("Location 43", new Vector2i(2954, 2748));
      map.AddLocation(worldMapLocation54);
      WorldMapLocation worldMapLocation55 = new WorldMapLocation("Location 41", new Vector2i(3042, 2262));
      map.AddLocation(worldMapLocation55);
      WorldMapLocation worldMapLocation56 = new WorldMapLocation("Location 35", new Vector2i(648, 2942));
      map.AddLocation(worldMapLocation56);
      WorldMapLocation worldMapLocation57 = new WorldMapLocation("Location 22", new Vector2i(1820, 3195));
      map.AddLocation(worldMapLocation57);
      WorldMapLocation worldMapLocation58 = new WorldMapLocation("Location 17", new Vector2i(983, 3192));
      map.AddLocation(worldMapLocation58);
      WorldMapLocation worldMapLocation59 = new WorldMapLocation("Location 29", new Vector2i(2673, 3706));
      map.AddLocation(worldMapLocation59);
      WorldMapLocation worldMapLocation60 = new WorldMapLocation("Location 46", new Vector2i(3141, 2193));
      map.AddLocation(worldMapLocation60);
      WorldMapLocation worldMapLocation61 = new WorldMapLocation("Location 18", new Vector2i(1441, 3149));
      map.AddLocation(worldMapLocation61);
      WorldMapLocation worldMapLocation62 = new WorldMapLocation("Location 42", new Vector2i(3003, 2135));
      map.AddLocation(worldMapLocation62);
      WorldMapLocation worldMapLocation63 = new WorldMapLocation("Location 20", new Vector2i(1627, 3139));
      map.AddLocation(worldMapLocation63);
      WorldMapLocation worldMapLocation64 = new WorldMapLocation("Location 37", new Vector2i(430, 2854));
      map.AddLocation(worldMapLocation64);
      WorldMapLocation worldMapLocation65 = new WorldMapLocation("Location 44", new Vector2i(3263, 2660));
      map.AddLocation(worldMapLocation65);
      WorldMapLocation worldMapLocation66 = new WorldMapLocation("Location 32", new Vector2i(1677, 3767));
      map.AddLocation(worldMapLocation66);
      WorldMapLocation worldMapLocation67 = new WorldMapLocation("Location 47", new Vector2i(3215, 2973));
      map.AddLocation(worldMapLocation67);
      WorldMapLocation worldMapLocation68 = new WorldMapLocation("Location 33", new Vector2i(1582, 3792));
      map.AddLocation(worldMapLocation68);
      WorldMapLocation worldMapLocation69 = new WorldMapLocation("Location 38", new Vector2i(409, 2667));
      map.AddLocation(worldMapLocation69);
      WorldMapLocation worldMapLocation70 = new WorldMapLocation("Location 34", new Vector2i(892, 3598));
      map.AddLocation(worldMapLocation70);
      map.AddConnection(worldMapLocation3, worldMapLocation1);
      map.AddConnection(worldMapLocation2, worldMapLocation1);
      map.AddConnection(worldMapLocation14, worldMapLocation2);
      map.AddConnection(worldMapLocation1, worldMapLocation5);
      map.AddConnection(worldMapLocation29, worldMapLocation16);
      map.AddConnection(worldMapLocation5, worldMapLocation16);
      map.AddConnection(worldMapLocation29, worldMapLocation40);
      map.AddConnection(worldMapLocation42, worldMapLocation29);
      map.AddConnection(worldMapLocation33, worldMapLocation42);
      map.AddConnection(worldMapLocation33, worldMapLocation15);
      map.AddConnection(worldMapLocation27, worldMapLocation14);
      map.AddConnection(worldMapLocation39, worldMapLocation27);
      map.AddConnection(worldMapLocation39, worldMapLocation46);
      map.AddConnection(worldMapLocation51, worldMapLocation46);
      map.AddConnection(worldMapLocation58, worldMapLocation51);
      map.AddConnection(worldMapLocation46, worldMapLocation61);
      map.AddConnection(worldMapLocation61, worldMapLocation63);
      map.AddConnection(worldMapLocation63, worldMapLocation57);
      map.AddConnection(worldMapLocation57, worldMapLocation52);
      map.AddConnection(worldMapLocation52, worldMapLocation43);
      map.AddConnection(worldMapLocation41, worldMapLocation32);
      map.AddConnection(worldMapLocation43, worldMapLocation37);
      map.AddConnection(worldMapLocation37, worldMapLocation41);
      map.AddConnection(worldMapLocation23, worldMapLocation32);
      map.AddConnection(worldMapLocation37, worldMapLocation23);
      map.AddConnection(worldMapLocation44, worldMapLocation37);
      map.AddConnection(worldMapLocation43, worldMapLocation44);
      map.AddConnection(worldMapLocation41, worldMapLocation53);
      map.AddConnection(worldMapLocation59, worldMapLocation53);
      map.AddConnection(worldMapLocation50, worldMapLocation44);
      map.AddConnection(worldMapLocation66, worldMapLocation50);
      map.AddConnection(worldMapLocation68, worldMapLocation66);
      map.AddConnection(worldMapLocation70, worldMapLocation58);
      map.AddConnection(worldMapLocation56, worldMapLocation39);
      map.AddConnection(worldMapLocation64, worldMapLocation56);
      map.AddConnection(worldMapLocation69, worldMapLocation64);
      map.AddConnection(worldMapLocation16, worldMapLocation28);
      map.AddConnection(worldMapLocation28, worldMapLocation38);
      map.AddConnection(worldMapLocation38, worldMapLocation54);
      map.AddConnection(worldMapLocation54, worldMapLocation65);
      map.AddConnection(worldMapLocation38, worldMapLocation55);
      map.AddConnection(worldMapLocation62, worldMapLocation55);
      map.AddConnection(worldMapLocation55, worldMapLocation60);
      map.AddConnection(worldMapLocation54, worldMapLocation67);
      map.AddConnection(worldMapLocation15, worldMapLocation5);
      map.AddConnection(worldMapLocation14, worldMapLocation15);
      map.AddConnection(worldMapLocation1, worldMapLocation6);
      map.AddConnection(worldMapLocation6, worldMapLocation13);
      map.AddConnection(worldMapLocation13, worldMapLocation23);
      map.AddConnection(worldMapLocation8, worldMapLocation3);
      map.AddConnection(worldMapLocation6, worldMapLocation9);
      map.AddConnection(worldMapLocation22, worldMapLocation13);
      map.AddConnection(worldMapLocation25, worldMapLocation14);
      map.AddConnection(worldMapLocation25, worldMapLocation33);
      map.AddConnection(worldMapLocation9, worldMapLocation17);
      map.AddConnection(worldMapLocation17, worldMapLocation28);
      map.AddConnection(worldMapLocation47, worldMapLocation38);
      map.AddConnection(worldMapLocation27, worldMapLocation31);
      map.AddConnection(worldMapLocation23, worldMapLocation35);
      map.AddConnection(worldMapLocation35, worldMapLocation45);
      map.AddConnection(worldMapLocation38, worldMapLocation49);
      map.AddConnection(worldMapLocation39, worldMapLocation48);
      map.AddConnection(worldMapLocation9, worldMapLocation19);
      map.AddConnection(worldMapLocation14, worldMapLocation24);
      map.AddConnection(worldMapLocation9, worldMapLocation18);
      map.AddConnection(worldMapLocation1, worldMapLocation4);
      map.AddConnection(worldMapLocation5, worldMapLocation10);
      map.AddConnection(worldMapLocation2, worldMapLocation12);
      map.AddConnection(worldMapLocation13, worldMapLocation20);
      map.AddConnection(worldMapLocation3, worldMapLocation11);
      map.AddConnection(worldMapLocation2, worldMapLocation7);
      map.AddConnection(worldMapLocation12, worldMapLocation26);
      map.AddConnection(worldMapLocation15, worldMapLocation21);
      map.AddConnection(worldMapLocation28, worldMapLocation36);
      map.AddConnection(worldMapLocation27, worldMapLocation34);
      map.AddConnection(worldMapLocation16, worldMapLocation30);
      map.AddConnection(worldMapLocation30, worldMapLocation36);
      Dict<WorldMapLocation, int> nodes = new Dict<WorldMapLocation, int>();
      nodes.Add(worldMapLocation1, 0);
      nodes.Add(worldMapLocation2, 195);
      nodes.Add(worldMapLocation3, 196);
      nodes.Add(worldMapLocation4, 202);
      nodes.Add(worldMapLocation5, 225);
      nodes.Add(worldMapLocation6, 241);
      nodes.Add(worldMapLocation7, 350);
      nodes.Add(worldMapLocation8, 377);
      nodes.Add(worldMapLocation9, 402);
      nodes.Add(worldMapLocation10, 403);
      nodes.Add(worldMapLocation11, 408);
      nodes.Add(worldMapLocation12, 444);
      nodes.Add(worldMapLocation13, 447);
      nodes.Add(worldMapLocation14, 448);
      nodes.Add(worldMapLocation15, 515);
      nodes.Add(worldMapLocation16, 533);
      nodes.Add(worldMapLocation17, 559);
      nodes.Add(worldMapLocation18, 577);
      nodes.Add(worldMapLocation19, 583);
      nodes.Add(worldMapLocation20, 615);
      nodes.Add(worldMapLocation21, 628);
      nodes.Add(worldMapLocation22, 638);
      nodes.Add(worldMapLocation23, 645);
      nodes.Add(worldMapLocation24, 647);
      nodes.Add(worldMapLocation25, 650);
      nodes.Add(worldMapLocation26, 654);
      nodes.Add(worldMapLocation27, 665);
      nodes.Add(worldMapLocation28, 739);
      nodes.Add(worldMapLocation29, 796);
      nodes.Add(worldMapLocation30, 800);
      nodes.Add(worldMapLocation31, 846);
      nodes.Add(worldMapLocation32, 860);
      nodes.Add(worldMapLocation33, 863);
      nodes.Add(worldMapLocation34, 870);
      nodes.Add(worldMapLocation35, 873);
      nodes.Add(worldMapLocation36, 906);
      nodes.Add(worldMapLocation37, 911);
      nodes.Add(worldMapLocation38, 973);
      nodes.Add(worldMapLocation39, 977);
      nodes.Add(worldMapLocation40, 994);
      nodes.Add(worldMapLocation41, 1027);
      nodes.Add(worldMapLocation42, 1051);
      nodes.Add(worldMapLocation43, 1084);
      nodes.Add(worldMapLocation44, 1093);
      nodes.Add(worldMapLocation45, 1093);
      nodes.Add(worldMapLocation46, 1157);
      nodes.Add(worldMapLocation47, 1168);
      nodes.Add(worldMapLocation48, 1190);
      nodes.Add(worldMapLocation49, 1210);
      nodes.Add(worldMapLocation50, 1249);
      nodes.Add(worldMapLocation51, 1307);
      nodes.Add(worldMapLocation52, 1310);
      nodes.Add(worldMapLocation53, 1370);
      nodes.Add(worldMapLocation54, 1376);
      nodes.Add(worldMapLocation55, 1384);
      nodes.Add(worldMapLocation56, 1412);
      nodes.Add(worldMapLocation57, 1435);
      nodes.Add(worldMapLocation58, 1447);
      nodes.Add(worldMapLocation59, 1460);
      nodes.Add(worldMapLocation60, 1505);
      nodes.Add(worldMapLocation61, 1508);
      nodes.Add(worldMapLocation62, 1517);
      nodes.Add(worldMapLocation63, 1636);
      nodes.Add(worldMapLocation64, 1647);
      nodes.Add(worldMapLocation65, 1697);
      nodes.Add(worldMapLocation66, 1707);
      nodes.Add(worldMapLocation67, 1720);
      nodes.Add(worldMapLocation68, 1805);
      nodes.Add(worldMapLocation69, 1835);
      nodes.Add(worldMapLocation70, 1863);
      int num1 = 765;
      ImmutableArray<int> immutableArray1 = ImmutableArray.Create<int>(195, 196, 202, 225, 241, 350, 377, 402, 403, 408, 444, 447, 448, 515, 533, 559, 577, 583, 615, 628, 638, 645, 647, 650, 654, 665, 739);
      int num2 = 1245;
      ImmutableArray<int> immutableArray2 = ImmutableArray.Create<int>(796, 800, 846, 860, 863, 870, 873, 906, 911, 973, 977, 994, 1027, 1051, 1084, 1093, 1093, 1157, 1168, 1190, 1210);
      int kmEnd = 2125;
      ImmutableArray<int> immutableArray3 = ImmutableArray.Create<int>(1249, 1307, 1310, 1370, 1376, 1384, 1412, 1435, 1447, 1460, 1505, 1508, 1517, 1636, 1647, 1697, 1707, 1720, 1805, 1835, 1863);
      int num3 = immutableArray1[immutableArray1.Length / 2 - 1];
      int num4 = immutableArray2[immutableArray2.Length / 2 - 1];
      int num5 = immutableArray3[immutableArray3.Length / 2 - 1];
      int index = 9;
      map.SetHomeLocation(worldMapLocation1);
      StaticWorldMap.Generator generator = new StaticWorldMap.Generator(this.m_protosDb, this.m_randomProvider.GetSimRandomFor((object) this, nameof (StaticWorldMap)), map, nodes);
      generator.generateEntityNextTo((Proto.ID) Ids.World.OilRigCost1, worldMapLocation1).AddTechnology(protosDb, Ids.Technology.OilDrilling);
      generator.generateEntityNextTo((Proto.ID) Ids.World.Settlement1, worldMapLocation1);
      generator.generateEntityNextTo((Proto.ID) Ids.World.CargoShipWreckCost1, worldMapLocation1).AddTechnology(protosDb, Ids.Technology.CargoShip);
      generator.generateEntity((Proto.ID) Ids.World.Settlement2, immutableArray1[6], immutableArray1[10]);
      generator.generateEntity((Proto.ID) Ids.World.CargoShipWreckCost1, immutableArray1[7], immutableArray1[10]).AddTechnology(protosDb, Ids.Technology.CargoShip);
      generator.generateEntity((Proto.ID) Ids.World.SulfurMine, immutableArray1[9], immutableArray1[13]);
      generator.generateEntity((Proto.ID) Ids.World.OilRigCost2, immutableArray1[8], immutableArray1[15]).AddTechnology(protosDb, Ids.Technology.OilDrilling);
      generator.generateEntity((Proto.ID) Ids.World.SettlementForFuel, immutableArray1[18], num1);
      generator.generateEntity((Proto.ID) Ids.World.CoalMine, immutableArray1[18], num1);
      generator.generateEntity((Proto.ID) Ids.World.QuartzMine, immutableArray1[22], num1);
      generator.generateEntity((Proto.ID) Ids.World.SettlementForShips, immutableArray1[10], num1).AddTechnology(protosDb, Ids.Technology.CargoShip);
      generator.generateEntity((Proto.ID) Ids.World.CargoShipWreckCost2, immutableArray1[10], num1).AddTechnology(protosDb, Ids.Technology.CargoShip);
      generator.generateEntity((Proto.ID) Ids.World.OilRigCost3, num1, immutableArray2[5]);
      generator.generateEntity((Proto.ID) Ids.World.WaterWell, num1, num4);
      generator.generateEntity((Proto.ID) Ids.World.CargoShipWreckCost2, num1, num4).AddTechnology(protosDb, Ids.Technology.CargoShip);
      generator.generateEntity((Proto.ID) Ids.World.Settlement3, num1, num4);
      generator.generateEntity((Proto.ID) Ids.World.UraniumMine, num1, num4).AddTechnology(protosDb, Ids.Technology.NuclearEnergy);
      generator.generateEntity((Proto.ID) Ids.World.Settlement4, num4, num2);
      generator.generateEntity((Proto.ID) Ids.World.CoalMine, num4, num2);
      generator.generateEntity((Proto.ID) Ids.World.QuartzMine, num4, num2);
      generator.generateEntity((Proto.ID) Ids.World.LimestoneMine, num4, num2);
      generator.generateEntity((Proto.ID) Ids.World.RockMine, num2, num5);
      generator.generateEntity((Proto.ID) Ids.World.OilRigCost3, num2, num5);
      generator.generateEntity((Proto.ID) Ids.World.Settlement5, num2, num5);
      generator.generateEntity((Proto.ID) Ids.World.SettlementForUranium, num2, num5);
      generator.generateEntity((Proto.ID) Ids.World.QuartzMine, num5, kmEnd);
      generator.generateEntity((Proto.ID) Ids.World.UraniumMine, num5, kmEnd);
      generator.generateEntity((Proto.ID) Ids.World.LimestoneMine, num5, kmEnd);
      generator.generateProtoUnlock(immutableArray1[10], immutableArray1[18], Ids.Technology.ShipRadar);
      generator.generateProtoUnlock(immutableArray1[24], immutableArray2[6], Ids.Technology.ShipRadarT2);
      generator.generateProtoUnlock(immutableArray1[5], immutableArray1[11], Ids.Technology.WheatSeeds);
      generator.generateProtoUnlock(immutableArray1[6], immutableArray1[14], Ids.Technology.CornSeeds);
      generator.generateProtoUnlock(immutableArray1[14], immutableArray1[20], Ids.Technology.SoybeansSeeds);
      generator.generateProtoUnlock(num1, num4, Ids.Technology.Microchip);
      generator.generateProtoUnlock(num1, immutableArray2[6], Ids.Technology.CanolaSeeds);
      generator.generateProtoUnlock(num1, immutableArray2[8], Ids.Technology.SugarCaneSeeds);
      generator.generateProtoUnlock(num1, num4, Ids.Technology.FruitSeeds);
      generator.generateProtoUnlock(num4, num2, Ids.Technology.PoppySeeds);
      ProductProto orThrow1 = this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.IronScrap);
      ProductProto orThrow2 = this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.CopperScrap);
      ProductProto orThrow3 = this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Rubber);
      ProductProto orThrow4 = this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Copper);
      ProductProto orThrow5 = this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.VehicleParts);
      ProductProto orThrow6 = this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Electronics2);
      ProductProto orThrow7 = this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Electronics3);
      generator.generateLootForEmptyLocations(0, num3, new AssetValue[1]
      {
        new AssetValue(orThrow1.WithQuantity(100), orThrow3.WithQuantity(40), orThrow4.WithQuantity(60), orThrow5.WithQuantity(12))
      });
      generator.generateLootForEmptyLocations(num3, num1, new AssetValue[1]
      {
        new AssetValue(orThrow1.WithQuantity(40), orThrow2.WithQuantity(80), orThrow3.WithQuantity(20), orThrow4.WithQuantity(40))
      });
      generator.generateLootForEmptyLocations(num1, num4, new AssetValue[1]
      {
        new AssetValue(orThrow6.WithQuantity(60), orThrow7.WithQuantity(30))
      });
      generator.generateLootForEmptyLocations(num4, num2, new AssetValue[1]
      {
        new AssetValue(orThrow6.WithQuantity(80), orThrow7.WithQuantity(40))
      });
      generator.generateLootForEmptyLocations(num2, kmEnd, new AssetValue[1]
      {
        new AssetValue(orThrow6.WithQuantity(80), orThrow7.WithQuantity(60))
      });
      generator.upliftEmptyLocationToTreasure(0, num3, 2);
      generator.upliftEmptyLocationToTreasure(num3, num1, 2);
      generator.upliftEmptyLocationToTreasure(num4, num2, 2);
      generator.upliftEmptyLocationToTreasure(num2, num5, 2);
      generator.upliftEmptyLocationToTreasure(num5, kmEnd, 2);
      worldMapLocation1.SetGraphics(this.m_protosDb.GetOrThrow<WorldMapLocationGfxProto>(Ids.Islands.HomeIsland));
      generator.generateEnemies(immutableArray1[index], immutableArray1[11], 0.75f, (Func<BattleFleet>) (() => this.newPirates(this.scout(1))));
      generator.generateEnemies(immutableArray1[12], immutableArray1[14], 0.75f, (Func<BattleFleet>) (() => this.newPirates(this.patrol(gunsT1: 1))));
      generator.generateEnemies(immutableArray1[15], num1, 0.75f, (Func<BattleFleet>) (() => this.newPirates(this.scout(1), this.scout(2))));
      generator.generateEnemies(immutableArray2[0], immutableArray2[3], 0.85f, (Func<BattleFleet>) (() => this.newPirates(this.patrol(gunsT1: 1), this.patrol(1))));
      generator.generateEnemies(immutableArray2[4], immutableArray2[5], 0.85f, (Func<BattleFleet>) (() => this.newPirates(this.cruiser(gunsT1: 3, armorsT1: 1))));
      generator.generateEnemies(immutableArray2[6], immutableArray2[8], 0.85f, (Func<BattleFleet>) (() => this.newPirates(this.cruiser(gunsT1: 3, armorsT1: 1), this.patrol(gunsT1: 2, armorsT1: 1))));
      generator.generateEnemies(immutableArray2[9], immutableArray2[11], 0.85f, (Func<BattleFleet>) (() => this.newPirates(this.cruiser(gunsT1: 2, gunsT2: 1, armorsT1: 1))));
      generator.generateEnemies(immutableArray2[12], num2, 0.85f, (Func<BattleFleet>) (() => this.newPirates(this.cruiser(gunsT1: 1, gunsT2: 2))));
      generator.generateEnemies(immutableArray3[0], num5, 1f, (Func<BattleFleet>) (() => this.newPirates(this.cruiser(gunsT2: 3, armorsT1: 2))));
      generator.generateEnemies(num5, kmEnd, 1f, (Func<BattleFleet>) (() => this.newPirates(this.cruiser(gunsT2: 3, armorsT1: 2), this.patrol(gunsT2: 2, armorsT1: 1))));
      return map;
    }

    private FleetEntity scout(
      int gunsT0 = 0,
      int gunsT1 = 0,
      int gunsT2 = 0,
      int gunsT3 = 0,
      int armorsT1 = 0,
      int armorsT2 = 0)
    {
      return this.pirate(Ids.Fleet.Hulls.Scout, gunsT0, gunsT1, gunsT2, gunsT3, armorsT1, armorsT2);
    }

    private FleetEntity patrol(
      int gunsT0 = 0,
      int gunsT1 = 0,
      int gunsT2 = 0,
      int gunsT3 = 0,
      int armorsT1 = 0,
      int armorsT2 = 0)
    {
      return this.pirate(Ids.Fleet.Hulls.Patrol, gunsT0, gunsT1, gunsT2, gunsT3, armorsT1, armorsT2);
    }

    private FleetEntity cruiser(
      int gunsT0 = 0,
      int gunsT1 = 0,
      int gunsT2 = 0,
      int gunsT3 = 0,
      int armorsT1 = 0,
      int armorsT2 = 0)
    {
      return this.pirate(Ids.Fleet.Hulls.Cruiser, gunsT0, gunsT1, gunsT2, gunsT3, armorsT1, armorsT2);
    }

    private FleetEntity pirate(
      FleetEntityHullProto.ID hullId,
      int gunsT0 = 0,
      int gunsT1 = 0,
      int gunsT2 = 0,
      int gunsT3 = 0,
      int armorsT1 = 0,
      int armorsT2 = 0)
    {
      FleetEntityHullProto orThrow = this.m_protosDb.GetOrThrow<FleetEntityHullProto>((Proto.ID) hullId);
      Lyst<FleetWeaponProto> lyst1 = new Lyst<FleetWeaponProto>();
      lyst1.AddRepeated(this.m_protosDb.GetOrThrow<FleetWeaponProto>((Proto.ID) Ids.Fleet.Weapons.Gun0), gunsT0);
      lyst1.AddRepeated(this.m_protosDb.GetOrThrow<FleetWeaponProto>((Proto.ID) Ids.Fleet.Weapons.Gun1), gunsT1);
      lyst1.AddRepeated(this.m_protosDb.GetOrThrow<FleetWeaponProto>((Proto.ID) Ids.Fleet.Weapons.Gun2), gunsT2);
      lyst1.AddRepeated(this.m_protosDb.GetOrThrow<FleetWeaponProto>((Proto.ID) Ids.Fleet.Weapons.Gun3), gunsT3);
      Lyst<FleetEntityPartProto> lyst2 = new Lyst<FleetEntityPartProto>();
      lyst2.AddRepeated(this.m_protosDb.GetOrThrow<FleetEntityPartProto>((Proto.ID) Ids.Fleet.Armor.ArmorT1), armorsT1);
      lyst2.AddRepeated(this.m_protosDb.GetOrThrow<FleetEntityPartProto>((Proto.ID) Ids.Fleet.Armor.ArmorT2), armorsT2);
      return new FleetEntity(orThrow, lyst1.ToImmutableArray(), lyst2.ToImmutableArray());
    }

    private BattleFleet newPirates(params FleetEntity[] fleets)
    {
      BattleFleet battleFleet = new BattleFleet("Pirates", false);
      foreach (FleetEntity fleet in fleets)
        battleFleet.AddEntity(fleet);
      return battleFleet;
    }

    private class Generator
    {
      private readonly ProtosDb m_protosDb;
      private Dict<WorldMapLocation, int> m_nodes;
      private IRandom m_rng;
      private Mafi.Core.World.WorldMap m_map;

      public Generator(
        ProtosDb protosDb,
        IRandom rng,
        Mafi.Core.World.WorldMap map,
        Dict<WorldMapLocation, int> nodes)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_protosDb = protosDb;
        this.m_nodes = nodes;
        this.m_rng = rng;
        this.m_map = map;
      }

      public WorldMapLocation generateEntity(Proto.ID entityId, int kmStart, int kmEnd)
      {
        Assert.That<int>(kmStart).IsLess(kmEnd);
        WorldMapEntityProto orThrow = this.m_protosDb.GetOrThrow<WorldMapEntityProto>(entityId);
        Lyst<WorldMapLocation> lyst = this.m_nodes.Where<KeyValuePair<WorldMapLocation, int>>((Func<KeyValuePair<WorldMapLocation, int>, bool>) (x => x.Value >= kmStart && x.Value <= kmEnd && x.Value != 0 && x.Key.EntityProto.IsNone)).Select<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>((Func<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>) (x => x.Key)).ToLyst<WorldMapLocation>();
        WorldMapLocation entity = !(orThrow is WorldMapCargoShipWreckProto) ? lyst.SampleRandomWeightedOrDefault<WorldMapLocation>(this.m_rng, (Func<WorldMapLocation, int>) (loc => (20 - 15 * this.m_map.NeighborsOf(loc).Count<WorldMapLocation>((Func<WorldMapLocation, bool>) (x => x.EntityProto.HasValue && !(x.EntityProto.Value is WorldMapCargoShipWreckProto)))).Max(1))) : this.m_rng.GetRandomElementOrDefault<WorldMapLocation>((IIndexable<WorldMapLocation>) lyst);
        entity.SetEntityProto(orThrow);
        return entity;
      }

      public WorldMapLocation generateEntityNextTo(Proto.ID entityId, WorldMapLocation asNeighborOf)
      {
        WorldMapEntityProto orThrow = this.m_protosDb.GetOrThrow<WorldMapEntityProto>(entityId);
        WorldMapLocation elementOrDefault = this.m_rng.GetRandomElementOrDefault<WorldMapLocation>((IIndexable<WorldMapLocation>) this.m_map.NeighborsOf(asNeighborOf).Where<WorldMapLocation>((Func<WorldMapLocation, bool>) (x => x.EntityProto.IsNone)).ToLyst<WorldMapLocation>());
        elementOrDefault.SetEntityProto(orThrow);
        return elementOrDefault;
      }

      public void generateLootForEmptyLocations(
        int kmStart,
        int kmEnd,
        AssetValue[] possibleRewards)
      {
        Assert.That<int>(kmStart).IsLess(kmEnd);
        IEnumerable<WorldMapLocation> worldMapLocations = this.m_nodes.Where<KeyValuePair<WorldMapLocation, int>>((Func<KeyValuePair<WorldMapLocation, int>, bool>) (x =>
        {
          if (x.Value < kmStart || x.Value > kmEnd)
            return false;
          return x.Key.Loot.IsNone || x.Key.Loot.Value.Products.IsEmpty;
        })).Select<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>((Func<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>) (x => x.Key));
        Set<AssetValue> set = new Set<AssetValue>();
        set.AddRange((IEnumerable<AssetValue>) possibleRewards);
        foreach (WorldMapLocation worldMapLocation in worldMapLocations)
        {
          WorldMapLoot worldMapLoot = new WorldMapLoot();
          AssetValue assetValue = set.SampleRandomOrDefault<AssetValue>(this.m_rng);
          foreach (ProductQuantity product in assetValue.Products)
          {
            float num = worldMapLocation.EntityProto.HasValue ? 0.5f : 1f;
            int quantity = this.m_rng.NextInt(((double) product.Quantity.Value * (double) num * 0.8).RoundToInt(), ((double) product.Quantity.Value * (double) num * 1.2).RoundToInt());
            worldMapLoot.Products += new AssetValue(product.Product.WithQuantity(quantity));
          }
          set.Remove(assetValue);
          if (set.IsEmpty<AssetValue>())
            set.AddRange((IEnumerable<AssetValue>) possibleRewards);
          if (worldMapLocation.Loot.IsNone)
          {
            worldMapLocation.Loot = (Option<WorldMapLoot>) worldMapLoot;
          }
          else
          {
            worldMapLocation.Loot.Value.Products = worldMapLoot.Products;
            worldMapLocation.Loot.Value.People = worldMapLoot.People;
          }
        }
      }

      public void upliftEmptyLocationToTreasure(
        int kmStart,
        int kmEnd,
        int amountOfLocationsToUplift)
      {
        Assert.That<int>(kmStart).IsLess(kmEnd);
        Lyst<WorldMapLocation> lyst = this.m_nodes.Where<KeyValuePair<WorldMapLocation, int>>((Func<KeyValuePair<WorldMapLocation, int>, bool>) (x => x.Value >= kmStart && x.Value <= kmEnd && x.Key.EntityProto.IsNone && x.Key.Loot.HasValue && !x.Key.Loot.Value.IsTreasure)).Select<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>((Func<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>) (x => x.Key)).ToLyst<WorldMapLocation>();
        for (int index = 0; index < amountOfLocationsToUplift; ++index)
        {
          WorldMapLocation elementOrDefault = this.m_rng.GetRandomElementOrDefault<WorldMapLocation>((IIndexable<WorldMapLocation>) lyst);
          if (elementOrDefault == null)
          {
            Log.Error("Out of locations to uplift!");
            break;
          }
          lyst.Remove(elementOrDefault);
          WorldMapLoot worldMapLoot = elementOrDefault.Loot.Value;
          worldMapLoot.Products = worldMapLoot.Products.Mul(2);
        }
      }

      public void generateProtoUnlock(int kmStart, int kmEnd, Proto.ID unlockedProtoId)
      {
        Assert.That<int>(kmStart).IsLess(kmEnd);
        WorldMapLocation worldMapLocation = this.m_nodes.Where<KeyValuePair<WorldMapLocation, int>>((Func<KeyValuePair<WorldMapLocation, int>, bool>) (x =>
        {
          if (x.Value < kmStart || x.Value > kmEnd || !x.Key.EntityProto.IsNone)
            return false;
          return x.Key.Loot.IsNone || x.Key.Loot.Value.ProtosToUnlock.IsEmpty;
        })).Select<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>((Func<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>) (x => x.Key)).SampleRandomOrDefault<WorldMapLocation>(this.m_rng);
        if (worldMapLocation == null)
        {
          Log.Warning(string.Format("Failed to find location for unlock of {0}! Relaxing constraints!", (object) unlockedProtoId));
          worldMapLocation = this.m_nodes.Where<KeyValuePair<WorldMapLocation, int>>((Func<KeyValuePair<WorldMapLocation, int>, bool>) (x =>
          {
            if (x.Value < kmStart || x.Value > kmEnd)
              return false;
            return x.Key.Loot.IsNone || x.Key.Loot.Value.ProtosToUnlock.IsEmpty;
          })).Select<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>((Func<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>) (x => x.Key)).SampleRandomOrDefault<WorldMapLocation>(this.m_rng);
        }
        if (worldMapLocation.Loot.IsNone)
          worldMapLocation.Loot = (Option<WorldMapLoot>) new WorldMapLoot();
        Assert.That<ImmutableArray<TechnologyProto>>(worldMapLocation.Loot.Value.ProtosToUnlock).IsEmpty<TechnologyProto>();
        TechnologyProto orThrow = this.m_protosDb.GetOrThrow<TechnologyProto>(unlockedProtoId);
        worldMapLocation.Loot.Value.ProtosToUnlock = ImmutableArray.Create<TechnologyProto>(orThrow);
        worldMapLocation.Loot.Value.IsTreasure = true;
      }

      public void generateEnemies(
        int kmStart,
        int kmEnd,
        float probability,
        Func<BattleFleet> fleetProvider)
      {
        Assert.That<int>(kmStart).IsLess(kmEnd);
        foreach (WorldMapLocation worldMapLocation in this.m_nodes.Where<KeyValuePair<WorldMapLocation, int>>((Func<KeyValuePair<WorldMapLocation, int>, bool>) (x => x.Value >= kmStart && x.Value <= kmEnd)).Select<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>((Func<KeyValuePair<WorldMapLocation, int>, WorldMapLocation>) (x => x.Key)))
        {
          if (!(worldMapLocation.EntityProto.ValueOrNull is WorldMapVillageProto) && !worldMapLocation.Enemy.HasValue)
          {
            bool flag1 = worldMapLocation.EntityProto.ValueOrNull is WorldMapMineProto;
            bool flag2 = worldMapLocation.Loot.HasValue && worldMapLocation.Loot.Value.IsTreasure;
            if (flag1 || flag2 || this.m_rng.TestProbability(probability))
              worldMapLocation.SetEnemy(fleetProvider());
          }
        }
      }
    }
  }
}
