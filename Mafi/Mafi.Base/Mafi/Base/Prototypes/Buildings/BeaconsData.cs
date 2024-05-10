// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.BeaconsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Beacons;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class BeaconsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProductProto diesel = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Diesel);
      ProductProto copper = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Copper);
      ProductProto rubber = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Rubber);
      ProductProto ironScrap = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.IronScrap);
      registrator.PrototypesDb.Add<BeaconScheduleProto>(new BeaconScheduleProto(new Proto.ID("BeaconSchedule"), new Func<int, Option<RefugeesReward>>(generateReward)));
      UpointsStatsCategoryProto orThrow = registrator.PrototypesDb.GetOrThrow<UpointsStatsCategoryProto>(IdsCore.UpointsStatsCategories.IslandBuilding);
      UpointsCategoryProto upointsCategory = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.Buildings.Beacon, "EMPTY", (Option<UpointsStatsCategoryProto>) orThrow));
      registrator.BeaconProtoBuilder.Start("Beacon", Ids.Buildings.Beacon).Description("Strong light helps other refugees to find your island and join you. This can help you to get more workers and some extra starting loot.").SetCost(Costs.Buildings.Beacon).SetElectricityConsumed(80.Kw()).SetTier(1).SetUnityMonthlyCost(1.Upoints(), upointsCategory).SetLayout(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("[0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 3 * h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "[7![7![7![7![7!", "[7![7![7![7![7!", "[7![7![7![7![7!", "[7![7![7![7![7!", "[7![7![7![7![7!").SetCategories(Ids.ToolbarCategories.Buildings).SetPrefabPath("Assets/Base/Buildings/Beacon.prefab").BuildAndAdd();

      Option<RefugeesReward> generateReward(int index)
      {
        ImmutableArray<ImmutableArray<ProductQuantity>> immutableArray;
        int num;
        if (index < 10)
        {
          immutableArray = ImmutableArray.Create<ImmutableArray<ProductQuantity>>(ImmutableArray.Create<ProductQuantity>(new ProductQuantity(copper, new Quantity(40 + 2 * index)), new ProductQuantity(rubber, new Quantity(25 + index)), new ProductQuantity(ironScrap, new Quantity(28 + index)), new ProductQuantity(diesel, new Quantity(46 + 2 * index))));
          num = 14;
        }
        else
        {
          immutableArray = ImmutableArray.Create<ImmutableArray<ProductQuantity>>(ImmutableArray.Create<ProductQuantity>(new ProductQuantity(copper, new Quantity(20)), new ProductQuantity(rubber, new Quantity(5)), new ProductQuantity(diesel, new Quantity(20))));
          num = 18;
        }
        Duration duration;
        if (index >= 2)
        {
          Fix32 fix32 = index.ToFix32();
          fix32 = fix32.Sqrt() * 2.4.ToFix32();
          duration = (fix32.ToIntCeiled() + 1).Months();
        }
        else
          duration = (3 + index).Months();
        int amountOfRefugees = num;
        ImmutableArray<ImmutableArray<ProductQuantity>> possibleRewards = immutableArray;
        return (Option<RefugeesReward>) new RefugeesReward(duration, amountOfRefugees, 1, possibleRewards);
      }
    }

    public BeaconsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
