// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Storages.StoragesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Storages
{
  internal class StoragesData : IModData
  {
    private static bool productFilter(ProductProto x) => x.IsStorable && x.Radioactivity == 0;

    public void RegisterData(ProtoRegistrator registrator)
    {
      string comment = "description for storage";
      Loc.Str("StoragePowerConsumptionSuffix", "Consumes power when sending or receiving products via its ports.", "appended at the end of a description to explain that a storage consumes power");
      string str1 = "";
      LocStr1 locStr1_1 = Loc.Str1("StorageSolidFormattedBase__desc", "Stores up to {0} units of a solid product.", comment);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      StorageProto nextTierId1 = registrator.StorageProtoBuilder.Start("Unit storage IV", Ids.Buildings.StorageUnitT4).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.StorageUnitT4.Value + "__desc", locStr1_1.Format(4320.ToString()).ToString() + str1)).SetCost(Costs.Buildings.StorageUnitT4).SetCapacity(4320).SetTransferLimit(10, 3.Ticks()).SetPowerConsumedForProductsExchange(24.Kw()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("   [8][8][8][8][8][8][8][8][8][8]   ", "A#>[8][8][8][8][8][8][8][8][8][8]>#X", "   [8][8][8][8][8][8][8][8][8][8]   ", "B#>[8][8][8][8][8][8][8][8][8][8]>#Y", "   [8][8][8][8][8][8][8][8][8][8]   ", "   [8][8][8][8][8][8][8][8][8][8]   ", "C#>[8][8][8][8][8][8][8][8][8][8]>#Z", "   [8][8][8][8][8][8][8][8][8][8]   ", "D#>[8][8][8][8][8][8][8][8][8][8]>#W", "   [8][8][8][8][8][8][8][8][8][8]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/UnitT4.prefab").BuildUnitAndAdd(new UnitStorageRackData[3]
      {
        new UnitStorageRackData(12, 4, -9f),
        new UnitStorageRackData(12, 4, -3f),
        new UnitStorageRackData(12, 4, 3f)
      }, "Assets/Base/Buildings/Storages/UnitT4_rack.prefab");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      registrator.StorageProtoBuilder.Start("Unit storage III", Ids.Buildings.StorageUnitT3).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.StorageUnitT3.Value + "__desc", locStr1_1.Format(2160.ToString()).ToString() + str1)).SetCost(Costs.Buildings.StorageUnitT3).SetNextTier(nextTierId1).SetCapacity(2160).SetTransferLimit(7, 3.Ticks()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetPowerConsumedForProductsExchange(16.Kw()).SetLayout("   [6][6][6][6][6][6][6][6][6][6]   ", "A#>[6][6][6][6][6][6][6][6][6][6]>#X", "   [6][6][6][6][6][6][6][6][6][6]   ", "B#>[6][6][6][6][6][6][6][6][6][6]>#Y", "   [6][6][6][6][6][6][6][6][6][6]   ", "   [6][6][6][6][6][6][6][6][6][6]   ", "C#>[6][6][6][6][6][6][6][6][6][6]>#Z", "   [6][6][6][6][6][6][6][6][6][6]   ", "D#>[6][6][6][6][6][6][6][6][6][6]>#W", "   [6][6][6][6][6][6][6][6][6][6]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/UnitT3.prefab").BuildUnitAndAdd(new UnitStorageRackData[3]
      {
        new UnitStorageRackData(6, 4, -9f),
        new UnitStorageRackData(6, 4, -3f),
        new UnitStorageRackData(6, 4, 3f)
      }, "Assets/Base/Buildings/Storages/UnitT3_rack.prefab");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      StorageProto nextTierId2 = registrator.StorageProtoBuilder.Start("Unit storage II", Ids.Buildings.StorageUnitT2).Description(LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.StorageUnitT2.Value + "__desc", locStr1_1.Format(360.ToString()).ToString() + str1)).SetCost(Costs.Buildings.StorageUnitT2).SetCapacity(360).SetTransferLimit(5, 4.Ticks()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetPowerConsumedForProductsExchange(8.Kw()).SetLayout("   [5][5][5][5][5]   ", "A#>[5][5][5][5][5]>#X", "   [5][5][5][5][5]   ", "B#>[5][5][5][5][5]>#Y", "   [5][5][5][5][5]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/UnitT2.prefab").BuildUnitAndAdd(new UnitStorageRackData[1]
      {
        new UnitStorageRackData(6, 6, -4f)
      }, "Assets/Base/Buildings/Storages/UnitT2_rack.prefab");
      StorageProtoBuilder.State state1 = registrator.StorageProtoBuilder.Start("Unit storage", Ids.Buildings.StorageUnit);
      string id1 = Ids.Buildings.StorageUnit.Value + "__desc";
      ref LocStr1 local1 = ref locStr1_1;
      int num = 180;
      string str2 = num.ToString();
      LocStrFormatted locStrFormatted = local1.Format(str2);
      string enUs1 = locStrFormatted.ToString() + str1;
      LocStr alreadyLocalizedStr1 = LocalizationManager.CreateAlreadyLocalizedStr(id1, enUs1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      state1.Description(alreadyLocalizedStr1).SetCost(Costs.Buildings.StorageUnit).SetNextTier(nextTierId2).SetCapacity(180).SetTransferLimit(3, 5.Ticks()).SetPowerConsumedForProductsExchange(4.Kw()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("   [4][4][4][4][4]   ", "A#>[4][4][4][4][4]>#X", "   [4][4][4][4][4]   ", "B#>[4][4][4][4][4]>#Y", "   [4][4][4][4][4]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/UnitT1.prefab").BuildUnitAndAdd(new UnitStorageRackData[1]
      {
        new UnitStorageRackData(3, 6, -4f)
      }, "Assets/Base/Buildings/Storages/UnitT1_rack.prefab");
      LocStr1 locStr1_2 = Loc.Str1("StorageLooseFormattedBase__desc", "Stores up to {0} units of a loose product.", comment);
      StorageProtoBuilder.State state2 = registrator.StorageProtoBuilder.Start("Loose storage IV", Ids.Buildings.StorageLooseT4);
      string id2 = Ids.Buildings.StorageLooseT4.Value + "__desc";
      ref LocStr1 local2 = ref locStr1_2;
      num = 4320;
      string str3 = num.ToString();
      locStrFormatted = local2.Format(str3);
      string enUs2 = locStrFormatted.ToString() + str1;
      LocStr alreadyLocalizedStr2 = LocalizationManager.CreateAlreadyLocalizedStr(id2, enUs2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      StorageProtoBuilder.State state3 = state2.Description(alreadyLocalizedStr2).SetCost(Costs.Buildings.StorageLooseT4).SetCapacity(4320).SetTransferLimit(6, 2.Ticks()).SetPowerConsumedForProductsExchange(24.Kw()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter)));
      CustomLayoutToken[] customTokens = new CustomLayoutToken[1]
      {
        new CustomLayoutToken("[0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h + 1;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      };
      Proto.ID? hardenedFloorSurfaceId = new Proto.ID?();
      int? nullable1 = new int?();
      int? customCollapseVerticesThreshold = nullable1;
      ThicknessIRange? customPlacementRange = new ThicknessIRange?();
      Option<IEnumerable<KeyValuePair<char, int>>> customPortHeights = new Option<IEnumerable<KeyValuePair<char, int>>>();
      EntityLayoutParams layoutParams = new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) customTokens, hardenedFloorSurfaceId: hardenedFloorSurfaceId, customCollapseVerticesThreshold: customCollapseVerticesThreshold, customPlacementRange: customPlacementRange, customPortHeights: customPortHeights);
      string[] strArray = new string[10]
      {
        "      [9][9][9][9][9][9][9][9]      ",
        "A~>[9][9][9][9][9][9][9][9][9][9]>~X",
        "   [9][9][9][9][9][9][9][9][9][9]   ",
        "B~>[9][9][9][9][9][9][9][9][9][9]>~Y",
        "   [9![9![9![9![9][9][9][9][9][9]   ",
        "   [9![9![9![9![9][9][9][9][9][9]   ",
        "C~>[9][9][9][9][9][9][9][9][9][9]>~Z",
        "   [9][9][9][9][9][9][9][9][9][9]   ",
        "D~>[9][9][9][9][9][9][9][9][9][9]>~W",
        "      [9][9][9][9][9][9][9][9]      "
      };
      StorageProto nextTierId3 = state3.SetLayout(layoutParams, strArray).SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/LooseT4.prefab").SetPileGfxParams("Pile_Soft", "Pile_Soft", new LoosePileTextureParams(0.2f)).BuildAsLooseAndAdd();
      StorageProtoBuilder.State state4 = registrator.StorageProtoBuilder.Start("Loose storage III", Ids.Buildings.StorageLooseT3);
      string id3 = Ids.Buildings.StorageLooseT3.Value + "__desc";
      ref LocStr1 local3 = ref locStr1_2;
      num = 2160;
      string str4 = num.ToString();
      locStrFormatted = local3.Format(str4);
      string enUs3 = locStrFormatted.ToString() + str1;
      LocStr alreadyLocalizedStr3 = LocalizationManager.CreateAlreadyLocalizedStr(id3, enUs3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      state4.Description(alreadyLocalizedStr3).SetCost(Costs.Buildings.StorageLooseT3).SetNextTier(nextTierId3).SetCapacity(2160).SetTransferLimit(7, 3.Ticks()).SetPowerConsumedForProductsExchange(16.Kw()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("      [6][6][6][6][6][6][6][6]      ", "A~>[6][6][6][6][6][6][6][6][6][6]>~X", "   [6][6][6][6][6][6][6][6][6][6]   ", "B~>[6][6][6][6][6][6][6][6][6][6]>~Y", "   [7][7][7][7][6][6][6][6][6][6]   ", "   [7][7][7][7][6][6][6][6][6][6]   ", "C~>[6][6][6][6][6][6][6][6][6][6]>~Z", "   [6][6][6][6][6][6][6][6][6][6]   ", "D~>[6][6][6][6][6][6][6][6][6][6]>~W", "      [6][6][6][6][6][6][6][6]      ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/LooseT3.prefab").SetPileGfxParams("Pile_Soft", "Pile_Soft", new LoosePileTextureParams(0.2f)).BuildAsLooseAndAdd();
      StorageProtoBuilder.State state5 = registrator.StorageProtoBuilder.Start("Loose storage II", Ids.Buildings.StorageLooseT2);
      string id4 = Ids.Buildings.StorageLooseT2.Value + "__desc";
      ref LocStr1 local4 = ref locStr1_2;
      num = 360;
      string str5 = num.ToString();
      locStrFormatted = local4.Format(str5);
      string enUs4 = locStrFormatted.ToString() + str1;
      LocStr alreadyLocalizedStr4 = LocalizationManager.CreateAlreadyLocalizedStr(id4, enUs4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      StorageProto nextTierId4 = state5.Description(alreadyLocalizedStr4).SetCost(Costs.Buildings.StorageLooseT2).SetCapacity(360).SetTransferLimit(5, 4.Ticks()).SetPowerConsumedForProductsExchange(8.Kw()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("   [6][6][6][6][6]   ", "A~>[6][6][6][6][6]>~X", "   [6][6][6][6][6]   ", "B~>[6][6][6][6][6]>~Y", "   [6][6][6][6][6]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/LooseT2.prefab").SetPileGfxParams("Pile_Soft", "Pile_Soft", new LoosePileTextureParams(0.3f)).BuildAsLooseAndAdd();
      StorageProtoBuilder.State state6 = registrator.StorageProtoBuilder.Start("Loose storage", Ids.Buildings.StorageLoose);
      string id5 = Ids.Buildings.StorageLoose.Value + "__desc";
      ref LocStr1 local5 = ref locStr1_2;
      num = 180;
      string str6 = num.ToString();
      locStrFormatted = local5.Format(str6);
      string enUs5 = locStrFormatted.ToString() + str1;
      LocStr alreadyLocalizedStr5 = LocalizationManager.CreateAlreadyLocalizedStr(id5, enUs5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      state6.Description(alreadyLocalizedStr5).SetCost(Costs.Buildings.StorageLoose).SetNextTier(nextTierId4).SetCapacity(180).SetTransferLimit(3, 5.Ticks()).SetPowerConsumedForProductsExchange(4.Kw()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("   [5][5][5][5][5]   ", "A~>[5][5][5][5][5]>~X", "   [5][5][5][5][5]   ", "B~>[5][5][5][5][5]>~Y", "   [5][5][5][5][5]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/LooseT1.prefab").SetPileGfxParams("Pile_Soft", "Pile_Soft", new LoosePileTextureParams(1.4f)).BuildAsLooseAndAdd();
      LocStr1 locStr1_3 = Loc.Str1("StorageFluidFormattedBase__desc", "Stores up to {0} units of a liquid or gas product.", comment);
      StorageProtoBuilder.State state7 = registrator.StorageProtoBuilder.Start("Fluid storage IV", Ids.Buildings.StorageFluidT4);
      string id6 = Ids.Buildings.StorageFluidT4.Value + "__desc";
      ref LocStr1 local6 = ref locStr1_3;
      num = 4320;
      string str7 = num.ToString();
      string enUs6 = local6.Format(str7).Value;
      LocStr alreadyLocalizedStr6 = LocalizationManager.CreateAlreadyLocalizedStr(id6, enUs6);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      StorageProto nextTierId5 = state7.Description(alreadyLocalizedStr6).SetCost(Costs.Buildings.StorageFluidT4).SetCapacity(4320).SetTransferLimit(4, 1.Ticks()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("      [9][9][9][9][9][9][9][9]      ", "A@>[9][9][9][9][9][9][9][9][9][9]>@X", "   [9][9][9][9][9][9][9][9][9][9]   ", "B@>[9][9][9][9][9][9][9][9][9][9]>@Y", "   [9][9][9][9][9][9][9][9][9][9]   ", "   [9][9][9][9][9][9][9][9][9][9]   ", "C@>[9][9][9][9][9][9][9][9][9][9]>@Z", "   [9][9][9][9][9][9][9][9][9][9]   ", "D@>[9][9][9][9][9][9][9][9][9][9]>@W", "      [9][9][9][9][9][9][9][9]      ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/GasT4.prefab").SetFluidIndicatorGfxParams("part3/icon", "part3/liquid", new FluidIndicatorGfxParams(1f, 2.6f, 2f)).BuildAsFluidAndAdd();
      StorageProtoBuilder.State state8 = registrator.StorageProtoBuilder.Start("Fluid storage III", Ids.Buildings.StorageFluidT3);
      string id7 = Ids.Buildings.StorageFluidT3.Value + "__desc";
      ref LocStr1 local7 = ref locStr1_3;
      num = 2160;
      string str8 = num.ToString();
      string enUs7 = local7.Format(str8).Value;
      LocStr alreadyLocalizedStr7 = LocalizationManager.CreateAlreadyLocalizedStr(id7, enUs7);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      state8.Description(alreadyLocalizedStr7).SetCost(Costs.Buildings.StorageFluidT3).SetNextTier(nextTierId5).SetCapacity(2160).SetTransferLimit(4, 1.Ticks()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("      [6][6][6][6][6][6][6][6]      ", "A@>[6][6][6][6][6][6][6][6][6][6]>@X", "   [6][6][6][6][6][6][6][6][6][6]   ", "B@>[6][6][6][6][6][6][6][6][6][6]>@Y", "   [6][6][6][6][6][6][6][6][6][6]   ", "   [6][6][6][6][6][6][6][6][6][6]   ", "C@>[6][6][6][6][6][6][6][6][6][6]>@Z", "   [6][6][6][6][6][6][6][6][6][6]   ", "D@>[6][6][6][6][6][6][6][6][6][6]>@W", "      [6][6][6][6][6][6][6][6]      ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/GasT3.prefab").SetFluidIndicatorGfxParams("part2/icon", "part2/liquid", new FluidIndicatorGfxParams(1f, 2.6f, 2f)).BuildAsFluidAndAdd();
      StorageProtoBuilder.State state9 = registrator.StorageProtoBuilder.Start("Fluid storage II", Ids.Buildings.StorageFluidT2);
      string id8 = Ids.Buildings.StorageFluidT2.Value + "__desc";
      ref LocStr1 local8 = ref locStr1_3;
      num = 360;
      string str9 = num.ToString();
      string enUs8 = local8.Format(str9).Value;
      LocStr alreadyLocalizedStr8 = LocalizationManager.CreateAlreadyLocalizedStr(id8, enUs8);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      StorageProto nextTierId6 = state9.Description(alreadyLocalizedStr8).SetCost(Costs.Buildings.StorageFluidT2).SetCapacity(360).SetTransferLimit(4, 2.Ticks()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("   [5][5][5][5][5]   ", "A@>[5][5][5][5][5]>@X", "   [5][5][5][5][5]   ", "B@>[5][5][5][5][5]>@Y", "   [5][5][5][5][5]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/GasT2.prefab").SetFluidIndicatorGfxParams("part3/icon", "part3/liquid", new FluidIndicatorGfxParams(1f, 1.3f, 2f)).BuildAsFluidAndAdd();
      StorageProtoBuilder.State state10 = registrator.StorageProtoBuilder.Start("Fluid storage", Ids.Buildings.StorageFluid);
      string id9 = Ids.Buildings.StorageFluid.Value + "__desc";
      ref LocStr1 local9 = ref locStr1_3;
      num = 180;
      string str10 = num.ToString();
      string enUs9 = local9.Format(str10).Value;
      LocStr alreadyLocalizedStr9 = LocalizationManager.CreateAlreadyLocalizedStr(id9, enUs9);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      state10.Description(alreadyLocalizedStr9).SetCost(Costs.Buildings.StorageFluid).SetNextTier(nextTierId6).SetCapacity(180).SetTransferLimit(2, 2.Ticks()).SetProductsFilter(StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter ?? (StoragesData.\u003C\u003EO.\u003C0\u003E__productFilter = new Func<ProductProto, bool>(StoragesData.productFilter))).SetLayout("   [4][4][4][4][4]   ", "A@>[4][4][4][4][4]>@X", "   [4][4][4][4][4]   ", "B@>[4][4][4][4][4]>@Y", "   [4][4][4][4][4]   ").SetCategories(Ids.ToolbarCategories.Storages).SetPrefabPath("Assets/Base/Buildings/Storages/GasT1.prefab").SetFluidIndicatorGfxParams("part1/icon", "part1/liquid", new FluidIndicatorGfxParams(1f, 1.3f, 2f)).BuildAsFluidAndAdd();
      ProtosDb prototypesDb = registrator.PrototypesDb;
      StaticEntityProto.ID nuclearWasteStorage = Ids.Buildings.NuclearWasteStorage;
      Proto.Str str11 = Proto.CreateStr((Proto.ID) Ids.Buildings.NuclearWasteStorage, "Radioactive waste storage", "A special underground storage facility that can safely manage any radioactive waste without causing any danger to the island’s population. Leaving a legacy for the next generations to come.");
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[2]
      {
        new CustomLayoutToken("-0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h, 4, LayoutTileConstraint.Ground, new int?(-h)))),
        new CustomLayoutToken("-0|", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h, 6, LayoutTileConstraint.Ground, new int?(-h))))
      }), "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]", "   [4][4][4][4][4][4][4][4][4]-3]-3]-3]-3]-3]-3][4]", "   [4]-4]-4]-4]-4]-4][4][4][4]-3]-3]-3]-3]-3]-3][4]", "   [4]-4]-4]-4]-4]-4][4][4][4][4][4][4][4]-3]-3][4]", "   [4]-4]-4]-4]-4]-4][4][4][4][4][4][4][4]-3]-3][4]", "   [4]-4]-4]-4]-4]-4][4][4][4][4][4][4][4]-3]-3][4]", "   [4]-4]-4]-4]-4]-4][4][4][4][4][4][4][4]-3]-3][4]", "   [4]-4]-4]-4]-4]-4][4][4][4][4][4]-3]-3]-3]-3][4]", "   [4][6][6][6][6][6][6][4][4][4][4]-3]-3]-3]-3][4]", "   [4][6][6][6][6][6][6][4][4][4][4]-3]-3]-3]-3][4]", "   [4][6][6][6][6][6][6][4][4][4][4][4][4][4][4][4]", "A#>[4][6][6]-3|-3|-3|-3|-3]-2]-2][4][4][4][4][4][4]", "   [4][6][6]-3|-3|-3|-3|-3]-2]-2][4][4][4][4][4][4]", "X#<[4][6][6]-3|-3|-3|-3|-3]-2]-2][4][4][4][4][4][4]", "   [4][6][6][6][6][6][6][4][4][4][4][4][4][4][4][4]", "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]");
      Func<ProductProto, bool> productsFilter = new Func<ProductProto, bool>(radioactiveProductFilter);
      ProductType? productType = new ProductType?(CountableProductProto.ProductType);
      Quantity capacity = 1600.Quantity();
      Quantity retiredWasteCapacity = 180.Quantity();
      EntityCosts entityCosts = Costs.Buildings.NuclearWasteStorage.MapToEntityCosts(registrator);
      Option<StorageProto> none = (Option<StorageProto>) Option.None;
      Electricity electricity = 120.Kw();
      nullable1 = new int?(5);
      LayoutEntityProto.Gfx graphics = new LayoutEntityProto.Gfx("Assets/Base/Buildings/WasteStorage.prefab", customIconPath: Option<string>.None, categories: new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Storages)));
      int? emissionIntensity = nullable1;
      Electricity powerConsumedForProductsExchange = electricity;
      NuclearWasteStorageProto proto = new NuclearWasteStorageProto(nuclearWasteStorage, str11, layoutOrThrow, productsFilter, productType, capacity, retiredWasteCapacity, entityCosts, none, graphics, emissionIntensity, powerConsumedForProductsExchange);
      prototypesDb.Add<NuclearWasteStorageProto>(proto);

      static bool radioactiveProductFilter(ProductProto x) => x.IsStorable && x.Radioactivity > 0;
    }

    public StoragesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
