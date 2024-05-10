// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.CropsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  /// <remarks>
  /// Note for water consumption: <b>Crops should consume MORE THAN 0.2 and LESS THAN 2 water per day.</b>
  /// Farms accumulate 2 quantity of water per rainy day (4 per heavy rain). If crop takes more than 2, soil water level will be
  /// decreasing on farm during rain. Farm without crop has 0.2 water/day evaporation, greenhouses are half
  /// of that at 0.1 per day.
  /// 
  /// This also means that for each 1 month of rain the farm will get 60 water.
  /// 
  /// Note for fertility consumption: <b>Crops should consume LESS THAN 0.4% fertility per day.</b> Farms regenerate
  /// 0.6% of missing fertility per day. This means that if farm is all the way at 0 fertility, it will
  /// regenerate 0.6% fertility per day. Given that crops without rotation will add 50% fertility penalty,
  /// no crop should have higher fertility consumption than 0.4%, otherwise they will drain farm all the way to zero
  /// fertility.
  /// </remarks>
  internal class CropsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb db = registrator.PrototypesDb;
      Assert.That<bool>(db.Add<CropProto>(new CropProto(Ids.Crops.NoCrop, Proto.CreateStr(Ids.Crops.NoCrop, "No crop", translationComment: "title of an item in the crop rotation schedule where actually no crop gets planted but instead the field is left to rest for few months"), ProductQuantity.None, PartialQuantity.Zero, Percent.Zero, Percent.Zero, 3.Months(), new Duration?(3.Months()), new CropProto.Gfx("Assets/Base/Products/Icons/LandRest.svg", "Assets/Base/Buildings/Farms/Crops/Potato.prefab", 0.2f, 1f, 1f, 1f))).IsEmptyCrop).IsTrue();
      db.Add<CropProto>(new CropProto(Ids.Crops.GreenManure, Proto.CreateStr(Ids.Crops.GreenManure, "Green manure"), ProductQuantity.None, 0.9.Quantity(), -0.12.Percent(), 0.Percent(), 2.Months(), new Duration?(), new CropProto.Gfx("Assets/Base/Buildings/Farms/GreenManure128.png", "Assets/Base/Buildings/Farms/Crops/GreenManure.prefab", 0.1f, 1f, 1f, 1f)));
      ProtosDb protosDb1 = db;
      Proto.ID potato = Ids.Crops.Potato;
      Proto.Str str1 = Proto.CreateStr(Ids.Crops.Potato, "Potato");
      ProductQuantity productProduced1 = produced(58, Ids.Products.Potato);
      PartialQuantity consumedWaterPerDay1 = 1.2.Quantity();
      Percent consumedFertilityPerDay1 = 0.35.Percent();
      Percent minFertilityToStartGrowth1 = 0.Percent();
      Duration growthDuration1 = 3.Months();
      Duration? surviveWithNoWaterDuration1 = new Duration?(3.Months());
      bool? nullable = new bool?(true);
      CropProto.Gfx graphics1 = new CropProto.Gfx("Assets/Base/Products/Icons/Potato128.png", "Assets/Base/Buildings/Farms/Crops/Potato.prefab", 0.2f, 1f, 1f, 1f);
      bool? requiresGreenhouse1 = new bool?(false);
      bool? plantByDefault1 = nullable;
      CropProto proto1 = new CropProto(potato, str1, productProduced1, consumedWaterPerDay1, consumedFertilityPerDay1, minFertilityToStartGrowth1, growthDuration1, surviveWithNoWaterDuration1, graphics1, requiresGreenhouse1, plantByDefault1);
      protosDb1.Add<CropProto>(proto1);
      db.Add<CropProto>(new CropProto(Ids.Crops.Corn, Proto.CreateStr(Ids.Crops.Corn, "Corn"), produced(66, Ids.Products.Corn), 1.33.Quantity(), 0.4.Percent(), 0.Percent(), 4.Months(), new Duration?(1.Months()), new CropProto.Gfx("Assets/Base/Products/Icons/Corn.svg", "Assets/Base/Buildings/Farms/Crops/Corn.prefab", 0.1f, 1f, 1f, 1f)));
      db.Add<CropProto>(new CropProto(Ids.Crops.Wheat, Proto.CreateStr(Ids.Crops.Wheat, "Wheat"), produced(58, Ids.Products.Wheat), 1.06.Quantity(), 0.35.Percent(), 0.Percent(), 6.Months(), new Duration?(2.Months()), new CropProto.Gfx("Assets/Base/Products/Icons/Wheat.svg", "Assets/Base/Buildings/Farms/Crops/Wheat.prefab", 0.1f, 1f, 1f, 1f)));
      ProtosDb protosDb2 = db;
      Proto.ID treeSapling = Ids.Crops.TreeSapling;
      Proto.Str str2 = Proto.CreateStr(Ids.Crops.TreeSapling, "Tree sapling");
      ProductQuantity productProduced2 = produced(60, Ids.Products.TreeSapling);
      PartialQuantity consumedWaterPerDay2 = 0.9.Quantity();
      Percent consumedFertilityPerDay2 = 0.2.Percent();
      Percent minFertilityToStartGrowth2 = 0.Percent();
      Duration growthDuration2 = 12.Months();
      Duration? surviveWithNoWaterDuration2 = new Duration?(10.Months());
      nullable = new bool?(false);
      CropProto.Gfx graphics2 = new CropProto.Gfx("Assets/Base/Products/Icons/TreeSapling.svg", "Assets/Base/Buildings/Farms/Crops/TreeSapling.prefab", 0.1f, 1f, 1f, 1f);
      bool? requiresGreenhouse2 = nullable;
      bool? plantByDefault2 = new bool?(false);
      CropProto proto2 = new CropProto(treeSapling, str2, productProduced2, consumedWaterPerDay2, consumedFertilityPerDay2, minFertilityToStartGrowth2, growthDuration2, surviveWithNoWaterDuration2, graphics2, requiresGreenhouse2, plantByDefault2);
      protosDb2.Add<CropProto>(proto2);
      db.Add<CropProto>(new CropProto(Ids.Crops.Soybeans, Proto.CreateStr(Ids.Crops.Soybeans, "Soybeans"), produced(22, Ids.Products.Soybean), 1.2.Quantity(), 0.5.Percent(), 0.Percent(), 4.Months(), new Duration?(1.Months()), new CropProto.Gfx("Assets/Base/Products/Icons/Soybean.svg", "Assets/Base/Buildings/Farms/Crops/Soy.prefab", 0.1f, 1f, 1f, 1f)));
      ProtosDb protosDb3 = db;
      Proto.ID sugarCane = Ids.Crops.SugarCane;
      Proto.Str str3 = Proto.CreateStr(Ids.Crops.SugarCane, "Sugar cane");
      ProductQuantity productProduced3 = produced(176, Ids.Products.SugarCane);
      PartialQuantity consumedWaterPerDay3 = 1.6.Quantity();
      Percent consumedFertilityPerDay3 = 0.5.Percent();
      Percent minFertilityToStartGrowth3 = 0.Percent();
      Duration growthDuration3 = 9.Months();
      Duration? surviveWithNoWaterDuration3 = new Duration?(3.Months());
      nullable = new bool?(true);
      CropProto.Gfx graphics3 = new CropProto.Gfx("Assets/Base/Products/Icons/SugarCane.svg", "Assets/Base/Buildings/Farms/Crops/Sugar.prefab", 0.1f, 1f, 1f, 1f);
      bool? requiresGreenhouse3 = nullable;
      bool? plantByDefault3 = new bool?(false);
      CropProto proto3 = new CropProto(sugarCane, str3, productProduced3, consumedWaterPerDay3, consumedFertilityPerDay3, minFertilityToStartGrowth3, growthDuration3, surviveWithNoWaterDuration3, graphics3, requiresGreenhouse3, plantByDefault3);
      protosDb3.Add<CropProto>(proto3);
      db.Add<CropProto>(new CropProto(Ids.Crops.Vegetables, Proto.CreateStr(Ids.Crops.Vegetables, "Vegetables"), produced(60, Ids.Products.Vegetables), 1.07.Quantity(), 0.35.Percent(), 0.Percent(), 4.Months(), new Duration?(3.Months()), new CropProto.Gfx("Assets/Base/Products/Icons/Vegetables.svg", "Assets/Base/Buildings/Farms/Crops/Veggie.prefab", 0.1f, 1f, 1f, 1f)));
      ProtosDb protosDb4 = db;
      Proto.ID fruits = Ids.Crops.Fruits;
      Proto.Str str4 = Proto.CreateStr(Ids.Crops.Fruits, "Fruits");
      ProductQuantity productProduced4 = produced(80, Ids.Products.Fruit);
      PartialQuantity consumedWaterPerDay4 = 1.33.Quantity();
      Percent consumedFertilityPerDay4 = 0.3.Percent();
      Percent minFertilityToStartGrowth4 = 0.Percent();
      Duration growthDuration4 = 8.Months();
      Duration? surviveWithNoWaterDuration4 = new Duration?(3.Months());
      nullable = new bool?(true);
      CropProto.Gfx graphics4 = new CropProto.Gfx("Assets/Base/Products/Icons/Fruits.svg", "Assets/Base/Buildings/Farms/Crops/Fruits.prefab", 0.1f, 1f, 1f, 1f);
      bool? requiresGreenhouse4 = nullable;
      bool? plantByDefault4 = new bool?(false);
      CropProto proto4 = new CropProto(fruits, str4, productProduced4, consumedWaterPerDay4, consumedFertilityPerDay4, minFertilityToStartGrowth4, growthDuration4, surviveWithNoWaterDuration4, graphics4, requiresGreenhouse4, plantByDefault4);
      protosDb4.Add<CropProto>(proto4);
      db.Add<CropProto>(new CropProto(Ids.Crops.Canola, Proto.CreateStr(Ids.Crops.Canola, "Canola"), produced(26, Ids.Products.Canola), 0.93.Quantity(), 0.3.Percent(), 0.Percent(), 3.Months(), new Duration?(1.Months()), new CropProto.Gfx("Assets/Base/Products/Icons/Canola.svg", "Assets/Base/Buildings/Farms/Crops/Canola.prefab", 0.1f, 1f, 1f, 1f)));
      ProtosDb protosDb5 = db;
      Proto.ID poppy = Ids.Crops.Poppy;
      Proto.Str str5 = Proto.CreateStr(Ids.Crops.Poppy, "Poppy");
      ProductQuantity productProduced5 = produced(20, Ids.Products.Poppy);
      PartialQuantity consumedWaterPerDay5 = 0.9.Quantity();
      Percent consumedFertilityPerDay5 = 0.3.Percent();
      Percent minFertilityToStartGrowth5 = 0.Percent();
      Duration growthDuration5 = 4.Months();
      Duration? surviveWithNoWaterDuration5 = new Duration?(1.Months());
      nullable = new bool?(true);
      CropProto.Gfx graphics5 = new CropProto.Gfx("Assets/Base/Products/Icons/Poppy.svg", "Assets/Base/Buildings/Farms/Crops/Poppy.prefab", 0.1f, 1f, 1f, 1f);
      bool? requiresGreenhouse5 = nullable;
      bool? plantByDefault5 = new bool?(false);
      CropProto proto5 = new CropProto(poppy, str5, productProduced5, consumedWaterPerDay5, consumedFertilityPerDay5, minFertilityToStartGrowth5, growthDuration5, surviveWithNoWaterDuration5, graphics5, requiresGreenhouse5, plantByDefault5);
      protosDb5.Add<CropProto>(proto5);
      ProtosDb protosDb6 = db;
      Proto.ID flowers = Ids.Crops.Flowers;
      Proto.Str str6 = Proto.CreateStr(Ids.Crops.Flowers, "Flowers");
      ProductQuantity productProduced6 = produced(24, Ids.Products.Flowers);
      PartialQuantity consumedWaterPerDay6 = 1.5.Quantity();
      Percent consumedFertilityPerDay6 = 0.5.Percent();
      Percent minFertilityToStartGrowth6 = 0.Percent();
      Duration growthDuration6 = 4.Months();
      Duration? surviveWithNoWaterDuration6 = new Duration?(1.Months());
      nullable = new bool?(true);
      CropProto.Gfx graphics6 = new CropProto.Gfx("Assets/Base/Products/Icons/Flowers.svg", "Assets/Base/Buildings/Farms/Crops/Flowers.prefab", 0.1f, 1f, 1f, 1f);
      bool? requiresGreenhouse6 = nullable;
      bool? plantByDefault6 = new bool?(false);
      CropProto proto6 = new CropProto(flowers, str6, productProduced6, consumedWaterPerDay6, consumedFertilityPerDay6, minFertilityToStartGrowth6, growthDuration6, surviveWithNoWaterDuration6, graphics6, requiresGreenhouse6, plantByDefault6);
      protosDb6.Add<CropProto>(proto6).SetAvailability(false);

      ProductQuantity produced(int quantity, ProductProto.ID productId)
      {
        return db.GetOrThrow<ProductProto>((Proto.ID) productId).WithQuantity(quantity);
      }
    }

    public CropsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
