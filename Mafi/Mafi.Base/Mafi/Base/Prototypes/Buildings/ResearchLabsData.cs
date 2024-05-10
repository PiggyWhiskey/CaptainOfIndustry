// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ResearchLabsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Buildings.ResearchLab;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Mods;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class ResearchLabsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      string[] strArray1 = new string[15]
      {
        "                     [3][3][3][3]            ",
        "                  [3][3][3][3][3][3][3]      ",
        "            [3][3][3][3][3][3][3][3][3][3][3]",
        "      [3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "      [3][3][3][3][3][3][3][3][3][3][3][3][3]",
        "            [3][3][3][3][3][3][3][3][3][3][3]",
        "                  [3][3][3][3][3][3][3]      ",
        "                     [3][3][3][3]            "
      };
      string[] strArray2 = new string[15]
      {
        "                        [3][3][3][3]               ",
        "                     [3][3][3][3][3][3][3]         ",
        "               [3][3][3][3][3][3][3][3][3][3][3]   ",
        "         [3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "A#>[3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]>~X",
        "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "         [3][3][3][3][3][3][3][3][3][3][3][3][3]   ",
        "               [3][3][3][3][3][3][3][3][3][3][3]   ",
        "                     [3][3][3][3][3][3][3]         ",
        "                        [3][3][3][3]               "
      };
      string[] strArray3 = new string[15]
      {
        "                        [4][4][4][4]               ",
        "                     [4][4][4][4][4][4][4]         ",
        "               [4][4][4][4][4][4][4][4][4][4][4]   ",
        "         [4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "A#>[4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]>~X",
        "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "         [4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
        "               [4][4][4][4][4][4][4][4][4][4][4]   ",
        "                     [4][4][4][4][4][4][4]         ",
        "                        [4][4][4][4]               "
      };
      string[] strArray4 = new string[15]
      {
        "                        [5][5][5][5]               ",
        "                     [5][5][5][5][5][5][5]         ",
        "               [5][5][5][5][5][5][5][5][5][5][5]   ",
        "         [5][5][5][5][5][5][5][5][5][5][5][5][5]   ",
        "   [5][5][5][5][5][5][5][5][5][6][6][8][8][5][5]   ",
        "   [5][5][5][5][5][5][5][5][5][6][6][8][8][5][5]   ",
        "   [5][5][5][5][5][9][9][9][9][6][6][5][5][5][5]   ",
        "A#>[5][5][5][5][5][9][9][9][9][6][6][5][5][5][5]>~X",
        "   [5][5][5][5][5][9][9][9][9][6][6][5][5][5][5]   ",
        "   [5][5][5][5][5][5][5][5][5][6][6][5][5][5][5]   ",
        "   [5][5][5][5][5][5][5][5][5][6][6][5][5][5][5]   ",
        "         [5][5][5][5][5][5][5][5][5][5][5][5][5]   ",
        "               [5][5][5][5][5][5][5][5][5][5][5]   ",
        "                     [5][5][5][8][5][5][5]         ",
        "                        [5][5][5][5]               "
      };
      string[] strArray5 = new string[15]
      {
        "                        [6][6][6][6]               ",
        "                     [8][8][8][6][6][6][6]         ",
        "               [6][6][8][8][8][6][6][6][6][7][7]   ",
        "         [7][7][7][7][8][8][8][6][6][6][6][7][7]   ",
        "   [7][7][9][9][9][7][7][7][6][6][6][6][6][6][6]   ",
        "   [9][9][9][9][9][9][7][7][6][6][6][6][6][6][6]   ",
        "   [9][9][9][9][9][9][9][7][6][6][6][6][6][6][6]   ",
        "A#>[9][9][9][9][9][9][9][7][6][6][6][6][6][6][6]>~X",
        "   [9][9][9][9][9][9][9][7][6][6][6][6][6][6][6]   ",
        "   [9][9][9][9][9][9][7][7][6][6][6][6][6][6][6]   ",
        "   [7][7][9][9][9][7][7][7][6][6][6][6][6][6][6]   ",
        "         [7][7][7][7][7][7][6][6][6][6][6][6][6]   ",
        "               [6][6][6][6][6][6][6][6][6][6][6]   ",
        "                     [6][6][6][8][6][6][6]         ",
        "                        [6][6][6][6]               "
      };
      ProtosDb prototypesDb = registrator.PrototypesDb;
      LocStr desc = Loc.Str(Ids.Buildings.ResearchLab2.ToString() + "__desc", "Provides access to more advanced technologies. Important: this lab has to be provided with lab equipment on continuous basis in order to work. The more labs you have, the faster you research. Lab can return consumed products in form of recyclables, if recycling technology is unlocked. ", "description of a research lab");
      UpointsStatsCategoryProto orThrow = registrator.PrototypesDb.GetOrThrow<UpointsStatsCategoryProto>(IdsCore.UpointsStatsCategories.IslandBuilding);
      UpointsCategoryProto upointsCategory = registrator.PrototypesDb.Add<UpointsCategoryProto>(new UpointsCategoryProto((Proto.ID) Ids.Buildings.ResearchLab1, "Assets/Unity/UserInterface/Toolbar/Research.svg", (Option<UpointsStatsCategoryProto>) orThrow));
      ResearchLabProto nextTier1 = registrator.ResearchLabProtoBuilder.Start("Research lab V", Ids.Buildings.ResearchLab5, 5).Description(desc).SetCost(Costs.Buildings.ResearchLab5).SetResearchSpeed(15.Seconds(), 1.6.ToFix32()).SetConsumedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.LabEquipment4).WithQuantity(1)).SetProducedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Recyclables).WithQuantity(8)).SetElectricityConsumed(1000.Kw()).SetComputingConsumed(Computing.FromTFlops(12)).SetUnityMonthlyCost(0.2.Upoints(), upointsCategory).SetLayout(strArray5).SetCategories(Ids.ToolbarCategories.Buildings).SetEmissionIntensity(6).SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()))).SetPrefabPath("Assets/Base/Buildings/ReserachLabs/ResearchLabT4.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      ResearchLabProto nextTier2 = registrator.ResearchLabProtoBuilder.Start("Research lab IV", Ids.Buildings.ResearchLab4, 4).Description(desc).SetCost(Costs.Buildings.ResearchLab4).SetResearchSpeed(15.Seconds(), 1.4.ToFix32()).SetConsumedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.LabEquipment3).WithQuantity(1)).SetProducedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Recyclables).WithQuantity(3)).SetElectricityConsumed(600.Kw()).SetUnityMonthlyCost(0.2.Upoints(), upointsCategory).SetLayout(strArray4).SetCategories(Ids.ToolbarCategories.Buildings).SetEmissionIntensity(5).SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()))).SetNextTier(nextTier1).SetPrefabPath("Assets/Base/Buildings/ReserachLabs/ResearchLabT3.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      ResearchLabProto nextTier3 = registrator.ResearchLabProtoBuilder.Start("Research lab III", Ids.Buildings.ResearchLab3, 3).Description(desc).SetCost(Costs.Buildings.ResearchLab3).SetResearchSpeed(15.Seconds(), 1.2.ToFix32()).SetConsumedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.LabEquipment2).WithQuantity(1)).SetProducedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Recyclables).WithQuantity(2)).SetElectricityConsumed(400.Kw()).SetUnityMonthlyCost(0.2.Upoints(), upointsCategory).SetLayout(strArray3).SetCategories(Ids.ToolbarCategories.Buildings).SetEmissionIntensity(6).SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()))).SetNextTier(nextTier2).SetPrefabPath("Assets/Base/Buildings/ReserachLabs/ResearchLabT2.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      ResearchLabProto nextTier4 = registrator.ResearchLabProtoBuilder.Start("Research lab II", Ids.Buildings.ResearchLab2, 2).Description(desc).SetCost(Costs.Buildings.ResearchLab2).SetResearchSpeed(15.Seconds(), (Fix32) 1).SetConsumedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.LabEquipment).WithQuantity(1)).SetProducedPerDuration(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Recyclables).WithQuantity(1)).SetElectricityConsumed(200.Kw()).SetUnityMonthlyCost(0.25.Upoints(), upointsCategory).SetLayout(strArray2).SetCategories(Ids.ToolbarCategories.Buildings).SetEmissionIntensity(6).SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()))).SetNextTier(nextTier3).SetPrefabPath("Assets/Base/Buildings/ReserachLabs/ResearchLabT1.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.ResearchLabProtoBuilder.Start("Research lab", Ids.Buildings.ResearchLab1, 1).Description("Provides research. The more labs you have, the faster you research.").SetCost(Costs.Buildings.ResearchLab).SetResearchSpeed(15.Seconds(), 0.8.ToFix32()).SetElectricityConsumed(40.Kw()).SetUnityMonthlyCost(0.25.Upoints(), upointsCategory).SetLayout(strArray1).SetCategories(Ids.ToolbarCategories.Buildings).SetNextTier(nextTier4).SetPrefabPath("Assets/Base/Buildings/ReserachLabs/ResearchLabT0.prefab").BuildAndAdd();
    }

    public ResearchLabsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
