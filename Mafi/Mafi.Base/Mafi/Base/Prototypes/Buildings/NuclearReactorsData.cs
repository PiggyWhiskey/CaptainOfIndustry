// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.NuclearReactorsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Machines;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.NuclearReactors;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class NuclearReactorsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      ProductProto orThrow1 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Water);
      ProductProto orThrow2 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamHi);
      ProductProto orThrow3 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamDepleted);
      ProductProto orThrow4 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamSp);
      ProductProto orThrow5 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.UraniumRod);
      ProductProto orThrow6 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SpentFuel);
      ProductProto orThrow7 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.MoxRod);
      ProductProto orThrow8 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SpentMox);
      ProductProto orThrow9 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.CoreFuel);
      ProductProto orThrow10 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.CoreFuelDirty);
      ProductProto orThrow11 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.BlanketFuel);
      ProductProto orThrow12 = prototypesDb1.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.BlanketFuelEnriched);
      string[] strArray = new string[32]
      {
        "      [4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][5][5][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][5][6][7][7][7][7][7][7][7][7][6][5][5]   ",
        "      [4][5][6][7][7][7][7][7][7][7][7][6][5][5]   ",
        "      [4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "      [4][4][6][7][7][7][7][7][7][7][7][5][5][5]   ",
        "   [3][4][4][6][7][7][7][7][7][7][7][7][5][5][5]   ",
        "F#>[3][4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "   [3][4][4][4][7][7][7][7][7][7][7][7][5][5][5]   ",
        "S#<[3][4][4][4][4][4][6][6][6][6][4][4][4][4][4]   ",
        "   [3][4][4][4][4][5][5][5][5][5][5][4][4][4][4][3]",
        "   [3][4][4][4][6][6][6][6][6][6][6][5][4][4][4][3]",
        "   [3][4][4][6][6]-3]-3]-3]-3][6][6][6][5][5][4][3]",
        "   [3][4][5][6]-3]-3]-5]-5]-3]-3]-3][6][6][5][4][3]",
        "   [3][4]-3]-3]-3]-5]-5]-5]-5]-5]-3]-3]-3][5][4][3]",
        "   [3][4]-4]-4]-5]-5]-5]-5]-5]-5]-4]-4]-3][5][4][3]",
        "   [3][4]-4]-4]-5]-5]-5]-5]-5]-5]-4]-4]-3][5][4][3]",
        "   [3][4]-3]-3]-5]-5]-5]-5]-5]-5]-3]-3]-3][5][4][3]",
        "   [3][4][5]-3]-3]-5]-5]-5]-5]-5]-3][6][6][5][4][3]",
        "   [3][4][4][5]-3]-3]-3]-3]-3]-3]-3][6][5][5][4][3]",
        "      [4][4][4][5][6][6][6][6][6][6][5][4][4][4]   ",
        "         [4][4][4][5][5][5][5][5][5][4][4][4]      ",
        "      D@>[4][4][4][4][4][4][4][4][4][4][4][4]W@>   ",
        "         [4][4][4][4][4][4][4][4][4][4][4][4]      ",
        "      A@>[4][4][4][4][4][4][4][4][4][4][4][4]X@>   ",
        "      B@>[4][4][4][4][4][4][4][4][4][4][4][4]Y@>   ",
        "         [4][4][4][4][4][4][4][4][4][4][4][4]      "
      };
      LocStr1 locStr1_1 = Loc.Str1(Ids.Buildings.NuclearReactorT2.Value + "__desc", "Advanced thermal reactor that provides increased throughput. This reactor is also able to utilize MOX fuel. It can also regulate its power level automatically (if computing is provided). This plant can be set up to effectively provide up to {0} MW of electricity when running on full power.", "example usage of {0}: 'provide up to 12 MW of electricity'");
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      StaticEntityProto.ID nuclearReactorT2 = Ids.Buildings.NuclearReactorT2;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.NuclearReactorT2, "Nuclear reactor II", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.NuclearReactorT2.Value + "Formatted", locStr1_1.Format(120.ToString()).Value));
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("-0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h, 6, terrainSurfaceHeight: new int?(-h))))
      }), strArray);
      EntityCosts entityCosts1 = Costs.Buildings.NuclearReactorT2.MapToEntityCosts(registrator);
      ProductQuantity waterInPerStep1 = orThrow1.WithQuantity(16);
      ProductQuantity steamOutPerStep1 = orThrow2.WithQuantity(16);
      Duration duration1 = BoilersData.DURATION;
      ImmutableArray<NuclearReactorProto.FuelData> fuelPairs1 = ImmutableArray.Create<NuclearReactorProto.FuelData>(new NuclearReactorProto.FuelData(orThrow7, orThrow8, 120.Seconds()), new NuclearReactorProto.FuelData(orThrow5, orThrow6, 120.Seconds()));
      ProductProto coolantIn1 = orThrow1;
      ProductProto coolantOut1 = orThrow3;
      Computing computingConsumed1 = Computing.FromTFlops(12);
      Option<NuclearReactorProto.EnrichmentData> none1 = (Option<NuclearReactorProto.EnrichmentData>) Option.None;
      Option<NuclearReactorProto> none2 = Option<NuclearReactorProto>.None;
      Option<string> none3 = Option<string>.None;
      Option<string> fuelIconPath1 = (Option<string>) "Assets/Base/Buildings/NuclearReactors/Shared/MoxReactorFuelIcon.svg";
      Option<string> soundPrefabPath1 = (Option<string>) "Assets/Base/Buildings/NuclearReactors/Shared/ReactorSound.prefab";
      NuclearReactorProto.Gfx graphics1 = new NuclearReactorProto.Gfx("Assets/Base/Buildings/NuclearReactors/NuclearReactorT2.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), soundPrefabPath1, fuelIconPath1, 3.5f, none3);
      NuclearReactorProto proto1 = new NuclearReactorProto(nuclearReactorT2, str1, layoutOrThrow1, entityCosts1, 4, waterInPerStep1, steamOutPerStep1, "AB", "XY", duration1, fuelPairs1, 'F', 'S', coolantIn1, coolantOut1, 'D', 'W', true, false, computingConsumed1, none1, none2, graphics1);
      NuclearReactorProto nuclearReactorProto = prototypesDb2.Add<NuclearReactorProto>(proto1);
      LocStr1 locStr1_2 = Loc.Str1(Ids.Buildings.NuclearReactor.Value + "__desc", "Thermal reactor that maintains nuclear chain reaction from enriched uranium rods. The reaction releases a large amount of energy utilized for steam generation. This plant can be set up to effectively provide up to {0} MW of electricity when running on full power. Beware that spent fuel is radioactive and can harm the population if not stored in a specialized facility.", "example usage of {0}: 'provide up to 12 MW of electricity'");
      ProtosDb prototypesDb3 = registrator.PrototypesDb;
      StaticEntityProto.ID nuclearReactor = Ids.Buildings.NuclearReactor;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.NuclearReactor, "Nuclear reactor", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.NuclearReactor.Value + "Formatted", locStr1_2.Format(90.ToString()).Value));
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("-0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h, 5, terrainSurfaceHeight: new int?(-h))))
      }), strArray);
      EntityCosts entityCosts2 = Costs.Buildings.NuclearReactor.MapToEntityCosts(registrator);
      ProductQuantity waterInPerStep2 = orThrow1.WithQuantity(16);
      ProductQuantity steamOutPerStep2 = orThrow2.WithQuantity(16);
      Duration duration2 = BoilersData.DURATION;
      ImmutableArray<NuclearReactorProto.FuelData> fuelPairs2 = ImmutableArray.Create<NuclearReactorProto.FuelData>(new NuclearReactorProto.FuelData(orThrow5, orThrow6, 120.Seconds()));
      ProductProto coolantIn2 = orThrow1;
      ProductProto coolantOut2 = orThrow3;
      Computing zero = Computing.Zero;
      Option<NuclearReactorProto.EnrichmentData> none4 = (Option<NuclearReactorProto.EnrichmentData>) Option.None;
      Option<NuclearReactorProto> nextTier = (Option<NuclearReactorProto>) nuclearReactorProto;
      Option<string> none5 = Option<string>.None;
      Option<string> fuelIconPath2 = (Option<string>) "Assets/Base/Buildings/NuclearReactors/Shared/MoxReactorFuelIcon.svg";
      Option<string> soundPrefabPath2 = (Option<string>) "Assets/Base/Buildings/NuclearReactors/Shared/ReactorSound.prefab";
      NuclearReactorProto.Gfx graphics2 = new NuclearReactorProto.Gfx("Assets/Base/Buildings/NuclearReactors/NuclearReactorT1.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), soundPrefabPath2, fuelIconPath2, 3.5f, none5);
      NuclearReactorProto proto2 = new NuclearReactorProto(nuclearReactor, str2, layoutOrThrow2, entityCosts2, 3, waterInPerStep2, steamOutPerStep2, "AB", "XY", duration2, fuelPairs2, 'F', 'S', coolantIn2, coolantOut2, 'D', 'W', true, false, zero, none4, nextTier, graphics2);
      prototypesDb3.Add<NuclearReactorProto>(proto2);
      LocStr1 locStr1_3 = Loc.Str1(Ids.Buildings.FastBreederReactor.Value + "__desc", "Fast breeder reactor is a nuclear reactor in which the fission chain reaction is sustained by fast neutrons. This process needs highly enriched fuel and produces large amounts of heat. The core containing enriched fuel is surrounded by a blanket of fissionable material which is bombarded by the fast neutrons and transformed into fissile fuel. This process also allows to burn transuranic isotopes which would normally take thousands of years to decay. This reactor does not use solid fuel rods, instead, its fuel is dissolved in molten salt. It runs under higher operating temperatures to produce super pressurized steam (800 °C). If the core is overheated and no emergency cooling is available, the reactor will automatically shut off by draining its molten fuel out of the reactor, all fuel will be lost, and the reactor will be damaged. This plant can be set up to effectively provide up to {0} MW of electricity when running on full power.", "example usage of {0}: 'provide up to 12 MW of electricity'");
      ProtosDb prototypesDb4 = registrator.PrototypesDb;
      StaticEntityProto.ID fastBreederReactor = Ids.Buildings.FastBreederReactor;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Buildings.FastBreederReactor, "Fast breeder reactor", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.FastBreederReactor.Value + "Formatted", locStr1_3.Format(240.ToString()).Value));
      EntityLayout layoutOrThrow3 = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("[0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h + 4;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "   [3][3][3][3][3][3][8][8][8][8][8][8][8][8][8][8][8][5][5][5][2]", "   [3][3][3][3][3][3][8][8][8][8][8][8][8][8][8][8][8][5][5][5][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][5][5][5][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][5][5][5][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][5][5][5][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "F@>[6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "S@<[6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "Q@>[6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "E@<[6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][6][6][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [6][6][6][6][6][6][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [9![9![9![4][4][4][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [9![9![9![4][4][4][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [2][3][3][4][4][4][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [2][3][3][4][4][4][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [2][3][3][4][4][4][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [2][3][3][4][4][4][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [2][3][3][4][4][4][8][8][8][8][8][8][8][8][8][8][8][6][4][4][2]", "   [2][3][3][4][4][4][7][7][7][7][7][7][7][7][7][7][7][6][4][4][2]", "   [2][3][3][4][4][4][7][7][7][7][7][7][7][7][7][7][7][7][4][4][2]", "   [2][3][3][4][4][4][7][7][7][7][9][9][9][9][9][9][7][7][7][4][2]", "   [2][3][3][4][4][4][7][7][7][9][9][9][9][9][9][9][9][7][7][7][2]", "   [2][3][3][4][4][4][7][7][9][7![7![7![7![7![7![7![7![9][7][7][2]", "   [2][3][3][4][4][4][7][7][9][7![7![7![7![7![7![7![7![9][7][7][2]", "   [2][3][3][4][4][4][7][7][9][7![7![7![7![7![7![7![7![9][7][7][2]", "   [2][3][3][4][4][4][7][7][9][7![7![7![7![7![7![7![7![9][7][7][2]", "   [2][3][3][4][4][4][7][7][9][7![7![7![7![7![7![7![7![9][7][7]   ", "                  [4][7][7][9][9][9][7![7![7![7![9][9][9][7][7]   ", "                     [7][7][7][9][9][9][9][9][9][9][9][7][7][7]   ", "                        [7][7][7][9][9][9][9][9][9][7][7][7]      ", "                        [7][7][7][7][7][7][7][7][7][7][7][7]      ", "                     D@>[4][4][4][7][7][7][7][7][7][4][4][4]W@>   ", "                        [4][4][4][4][4][4][4][4][4][4][4][4]      ", "                     A@>[4][4][4][4][4][4][4][4][4][4][4][4]X@>   ", "                     B@>[4][4][4][4][4][4][4][4][4][4][4][4]Y@>   ", "                        [4][4][4][4][4][4][4][4][4][4][4][4]      ");
      EntityCosts entityCosts3 = Costs.Buildings.NuclearReactorT3.MapToEntityCosts(registrator);
      ProductQuantity waterInPerStep3 = orThrow1.WithQuantity(16);
      ProductQuantity steamOutPerStep3 = orThrow4.WithQuantity(16);
      Duration duration3 = BoilersData.DURATION;
      ImmutableArray<NuclearReactorProto.FuelData> fuelPairs3 = ImmutableArray.Create<NuclearReactorProto.FuelData>(new NuclearReactorProto.FuelData(orThrow9, orThrow10, 60.Seconds()));
      ProductProto coolantIn3 = orThrow1;
      ProductProto coolantOut3 = orThrow3;
      Computing computingConsumed2 = Computing.FromTFlops(18);
      Option<NuclearReactorProto.EnrichmentData> enrichment = (Option<NuclearReactorProto.EnrichmentData>) new NuclearReactorProto.EnrichmentData(orThrow11, 'Q', orThrow12, 'E', new PartialQuantity(1.2.ToFix32()), 40.Quantity(), true);
      Option<NuclearReactorProto> none6 = Option<NuclearReactorProto>.None;
      Option<string> none7 = Option<string>.None;
      Option<string> none8 = Option<string>.None;
      Option<string> soundPrefabPath3 = (Option<string>) "Assets/Base/Buildings/NuclearReactors/Shared/ReactorSound.prefab";
      NuclearReactorProto.Gfx graphics3 = new NuclearReactorProto.Gfx("Assets/Base/Buildings/NuclearReactors/FastReactor.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), soundPrefabPath3, none8, 9f, none7);
      NuclearReactorProto proto3 = new NuclearReactorProto(fastBreederReactor, str3, layoutOrThrow3, entityCosts3, 4, waterInPerStep3, steamOutPerStep3, "AB", "XY", duration3, fuelPairs3, 'F', 'S', coolantIn3, coolantOut3, 'D', 'W', false, true, computingConsumed2, enrichment, none6, graphics3);
      prototypesDb4.Add<NuclearReactorProto>(proto3);
    }

    public NuclearReactorsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
