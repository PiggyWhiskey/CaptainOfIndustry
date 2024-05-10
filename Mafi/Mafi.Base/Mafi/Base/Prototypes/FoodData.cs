// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.FoodData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal class FoodData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      FoodCategoryProto foodCategoryProto1 = prototypesDb.Add<FoodCategoryProto>(new FoodCategoryProto(Ids.FoodCategories.Carbs, Proto.CreateStr(Ids.FoodCategories.Carbs, "Carbs"), true));
      FoodCategoryProto foodCategoryProto2 = prototypesDb.Add<FoodCategoryProto>(new FoodCategoryProto(Ids.FoodCategories.Protein, Proto.CreateStr(Ids.FoodCategories.Protein, "Protein"), true));
      FoodCategoryProto foodCategoryProto3 = prototypesDb.Add<FoodCategoryProto>(new FoodCategoryProto(Ids.FoodCategories.Vitamins, Proto.CreateStr(Ids.FoodCategories.Vitamins, "Vitamins"), true));
      FoodCategoryProto foodCategoryProto4 = prototypesDb.Add<FoodCategoryProto>(new FoodCategoryProto(Ids.FoodCategories.Treats, Proto.CreateStr(Ids.FoodCategories.Treats, "Treats"), false));
      ProtosDb protosDb1 = prototypesDb;
      Proto.ID potato = Ids.FoodTypes.Potato;
      ProductProto orThrow1 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Potato);
      Fix32 fix32_1 = 4.7.ToFix32();
      FoodCategoryProto foodCategory1 = foodCategoryProto1;
      Fix32 consumedPerHundredPopsPerMonth1 = fix32_1;
      Upoints upointsWhenProvided1 = 0.15.Upoints();
      FoodProto proto1 = new FoodProto(potato, orThrow1, foodCategory1, consumedPerHundredPopsPerMonth1, upointsWhenProvided1);
      protosDb1.Add<FoodProto>(proto1);
      ProtosDb protosDb2 = prototypesDb;
      Proto.ID corn = Ids.FoodTypes.Corn;
      ProductProto orThrow2 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Corn);
      Fix32 fix32_2 = 3.3.ToFix32();
      FoodCategoryProto foodCategory2 = foodCategoryProto1;
      Fix32 consumedPerHundredPopsPerMonth2 = fix32_2;
      Upoints upointsWhenProvided2 = 0.15.Upoints();
      FoodProto proto2 = new FoodProto(corn, orThrow2, foodCategory2, consumedPerHundredPopsPerMonth2, upointsWhenProvided2);
      protosDb2.Add<FoodProto>(proto2);
      ProtosDb protosDb3 = prototypesDb;
      Proto.ID bread = Ids.FoodTypes.Bread;
      ProductProto orThrow3 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Bread);
      Fix32 fix32_3 = 2.2.ToFix32();
      FoodCategoryProto foodCategory3 = foodCategoryProto1;
      Fix32 consumedPerHundredPopsPerMonth3 = fix32_3;
      Upoints upointsWhenProvided3 = 0.3.Upoints();
      FoodProto proto3 = new FoodProto(bread, orThrow3, foodCategory3, consumedPerHundredPopsPerMonth3, upointsWhenProvided3);
      protosDb3.Add<FoodProto>(proto3);
      ProtosDb protosDb4 = prototypesDb;
      Proto.ID meat = Ids.FoodTypes.Meat;
      ProductProto orThrow4 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Meat);
      Fix32 fix32_4 = 3.ToFix32();
      FoodCategoryProto foodCategory4 = foodCategoryProto2;
      Fix32 consumedPerHundredPopsPerMonth4 = fix32_4;
      Upoints upointsWhenProvided4 = 0.4.Upoints();
      FoodProto proto4 = new FoodProto(meat, orThrow4, foodCategory4, consumedPerHundredPopsPerMonth4, upointsWhenProvided4);
      protosDb4.Add<FoodProto>(proto4);
      ProtosDb protosDb5 = prototypesDb;
      Proto.ID eggs = Ids.FoodTypes.Eggs;
      ProductProto orThrow5 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Eggs);
      Fix32 fix32_5 = 3.3.ToFix32();
      FoodCategoryProto foodCategory5 = foodCategoryProto2;
      Fix32 consumedPerHundredPopsPerMonth5 = fix32_5;
      Upoints upointsWhenProvided5 = 0.3.Upoints();
      FoodProto proto5 = new FoodProto(eggs, orThrow5, foodCategory5, consumedPerHundredPopsPerMonth5, upointsWhenProvided5);
      protosDb5.Add<FoodProto>(proto5);
      ProtosDb protosDb6 = prototypesDb;
      Proto.ID tofu = Ids.FoodTypes.Tofu;
      ProductProto orThrow6 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Tofu);
      Fix32 fix32_6 = 2.ToFix32();
      FoodCategoryProto foodCategory6 = foodCategoryProto2;
      Fix32 consumedPerHundredPopsPerMonth6 = fix32_6;
      Upoints upointsWhenProvided6 = 0.3.Upoints();
      FoodProto proto6 = new FoodProto(tofu, orThrow6, foodCategory6, consumedPerHundredPopsPerMonth6, upointsWhenProvided6);
      protosDb6.Add<FoodProto>(proto6);
      ProtosDb protosDb7 = prototypesDb;
      Proto.ID sausage = Ids.FoodTypes.Sausage;
      ProductProto orThrow7 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Sausage);
      Fix32 fix32_7 = 3.7.ToFix32();
      FoodCategoryProto foodCategory7 = foodCategoryProto2;
      Fix32 consumedPerHundredPopsPerMonth7 = fix32_7;
      Upoints upointsWhenProvided7 = 0.1.Upoints();
      FoodProto proto7 = new FoodProto(sausage, orThrow7, foodCategory7, consumedPerHundredPopsPerMonth7, upointsWhenProvided7);
      protosDb7.Add<FoodProto>(proto7);
      ProtosDb protosDb8 = prototypesDb;
      Proto.ID vegetables = Ids.FoodTypes.Vegetables;
      ProductProto orThrow8 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Vegetables);
      Fix32 fix32_8 = 4.7.ToFix32();
      FoodCategoryProto foodCategory8 = foodCategoryProto3;
      Fix32 consumedPerHundredPopsPerMonth8 = fix32_8;
      Upoints upointsWhenProvided8 = 0.2.Upoints();
      FoodProto proto8 = new FoodProto(vegetables, orThrow8, foodCategory8, consumedPerHundredPopsPerMonth8, upointsWhenProvided8);
      protosDb8.Add<FoodProto>(proto8);
      ProtosDb protosDb9 = prototypesDb;
      Proto.ID fruits = Ids.FoodTypes.Fruits;
      ProductProto orThrow9 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Fruit);
      Fix32 fix32_9 = 3.5.ToFix32();
      FoodCategoryProto foodCategory9 = foodCategoryProto3;
      Fix32 consumedPerHundredPopsPerMonth9 = fix32_9;
      Upoints upointsWhenProvided9 = 0.3.Upoints();
      FoodProto proto9 = new FoodProto(fruits, orThrow9, foodCategory9, consumedPerHundredPopsPerMonth9, upointsWhenProvided9);
      protosDb9.Add<FoodProto>(proto9);
      ProtosDb protosDb10 = prototypesDb;
      Proto.ID snack = Ids.FoodTypes.Snack;
      ProductProto orThrow10 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Snack);
      Fix32 fix32_10 = 5.8.ToFix32();
      FoodCategoryProto foodCategory10 = foodCategoryProto4;
      Fix32 consumedPerHundredPopsPerMonth10 = fix32_10;
      Upoints upointsWhenProvided10 = 0.25.Upoints();
      FoodProto proto10 = new FoodProto(snack, orThrow10, foodCategory10, consumedPerHundredPopsPerMonth10, upointsWhenProvided10);
      protosDb10.Add<FoodProto>(proto10);
      ProtosDb protosDb11 = prototypesDb;
      Proto.ID cake = Ids.FoodTypes.Cake;
      ProductProto orThrow11 = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Cake);
      Fix32 fix32_11 = 2.8.ToFix32();
      FoodCategoryProto foodCategory11 = foodCategoryProto4;
      Fix32 consumedPerHundredPopsPerMonth11 = fix32_11;
      Upoints upointsWhenProvided11 = 0.55.Upoints();
      FoodProto proto11 = new FoodProto(cake, orThrow11, foodCategory11, consumedPerHundredPopsPerMonth11, upointsWhenProvided11);
      protosDb11.Add<FoodProto>(proto11);
    }

    public FoodData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
