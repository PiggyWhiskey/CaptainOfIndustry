// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.TreesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal class TreesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      CountableProductProto orThrow1 = prototypesDb.GetOrThrow<CountableProductProto>((Proto.ID) Ids.Products.Wood);
      LocStr descShort = Loc.Str("TreesInMenuDescription", "A tree that can be manually positioned without having to use a forestry tower. Planting it requires a tree sapling and a tree planting vehicle.", "description of tree in a build menu");
      ImmutableArray<ToolbarCategoryProto> categoriesProtos = registrator.GetCategoriesProtos(Ids.ToolbarCategories.Landmarks);
      TerrainMaterialProto orThrow2 = prototypesDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.ForestFloor);
      ProtosDb protosDb1 = prototypesDb;
      Proto.ID spruceTree = Ids.Trees.SpruceTree;
      Proto.Str strings1 = new Proto.Str(Loc.Str(Ids.Trees.SpruceTree.ToString() + "__desc", "Spruce tree", ""), descShort);
      Percent baseScaleStdDeviation1 = 10.Percent();
      RelTile1i minForestFloorRadius1 = 0.Tiles();
      RelTile1i maxForestFloorRadius1 = 8.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial1 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow1 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      ImmutableArray<ToolbarCategoryProto>? categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics1 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Conifer/Conifer1-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics1 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Conifer/Conifer1-Color1.prefab", "Assets/Base/Trees/Conifer/Conifer1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Conifer/Conifer1-Color2.prefab", "Assets/Base/Trees/Conifer/Conifer1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Conifer/Conifer1-Color3.prefab", "Assets/Base/Trees/Conifer/Conifer1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Conifer/Conifer2-Color1.prefab", "Assets/Base/Trees/Conifer/Conifer2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Conifer/Conifer2-Color2.prefab", "Assets/Base/Trees/Conifer/Conifer2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Conifer/Conifer2-Color3.prefab", "Assets/Base/Trees/Conifer/Conifer2-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Common/NewConiferLog.prefab", 8.0.Tiles(), "Assets/Base/Icons/Tree.svg");
      TreeProto proto1 = new TreeProto(spruceTree, strings1, baseScaleStdDeviation1, minForestFloorRadius1, maxForestFloorRadius1, 3, false, forestFloorMaterial1, layoutOrThrow1, layoutEntityGraphics1, treeGraphics1);
      TreeProto treeProto1 = protosDb1.Add<TreeProto>(proto1);
      ProtosDb protosDb2 = prototypesDb;
      Proto.ID firTree = Ids.Trees.FirTree;
      Proto.Str strings2 = new Proto.Str(Loc.Str(Ids.Trees.FirTree.ToString() + "__desc", "Fir tree", ""), descShort);
      Percent baseScaleStdDeviation2 = 10.Percent();
      RelTile1i minForestFloorRadius2 = 0.Tiles();
      RelTile1i maxForestFloorRadius2 = 8.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial2 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow2 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics2 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Fir/Fir1-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics2 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Fir/Fir1-Color1.prefab", "Assets/Base/Trees/Fir/Fir1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Fir/Fir1-Color2.prefab", "Assets/Base/Trees/Fir/Fir1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Fir/Fir1-Color3.prefab", "Assets/Base/Trees/Fir/Fir1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Fir/Fir2-Color1.prefab", "Assets/Base/Trees/Fir/Fir2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Fir/Fir2-Color2.prefab", "Assets/Base/Trees/Fir/Fir2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Fir/Fir2-Color3.prefab", "Assets/Base/Trees/Fir/Fir2-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Common/NewConiferLog.prefab", 8.0.Tiles(), "Assets/Base/Icons/Tree.svg");
      TreeProto proto2 = new TreeProto(firTree, strings2, baseScaleStdDeviation2, minForestFloorRadius2, maxForestFloorRadius2, 3, false, forestFloorMaterial2, layoutOrThrow2, layoutEntityGraphics2, treeGraphics2);
      TreeProto treeProto2 = protosDb2.Add<TreeProto>(proto2);
      ProtosDb protosDb3 = prototypesDb;
      Proto.ID birchTree = Ids.Trees.BirchTree;
      Proto.Str strings3 = new Proto.Str(Loc.Str(Ids.Trees.BirchTree.ToString() + "__desc", "Birch tree", ""), descShort);
      Percent baseScaleStdDeviation3 = 10.Percent();
      RelTile1i minForestFloorRadius3 = 0.Tiles();
      RelTile1i maxForestFloorRadius3 = 8.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial3 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow3 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics3 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Birch/Birch1-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics3 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Birch/Birch1-Color1.prefab", "Assets/Base/Trees/Birch/Birch1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Birch/Birch1-Color2.prefab", "Assets/Base/Trees/Birch/Birch1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Birch/Birch2-Color1.prefab", "Assets/Base/Trees/Birch/Birch2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Birch/Birch2-Color2.prefab", "Assets/Base/Trees/Birch/Birch2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Birch/Birch3-Color1.prefab", "Assets/Base/Trees/Birch/Birch3-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Birch/Birch3-Color2.prefab", "Assets/Base/Trees/Birch/Birch3-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Birch/BirchLogCut.prefab", 8.0.Tiles(), "Assets/Base/Icons/DeciduousTree.svg");
      TreeProto proto3 = new TreeProto(birchTree, strings3, baseScaleStdDeviation3, minForestFloorRadius3, maxForestFloorRadius3, 3, false, forestFloorMaterial3, layoutOrThrow3, layoutEntityGraphics3, treeGraphics3);
      TreeProto treeProto3 = protosDb3.Add<TreeProto>(proto3);
      ProtosDb protosDb4 = prototypesDb;
      Proto.ID birchTreeDry = Ids.Trees.BirchTreeDry;
      Proto.Str strings4 = new Proto.Str(Loc.Str(Ids.Trees.BirchTreeDry.ToString() + "__desc", "Birch tree (dry)", ""), descShort);
      Percent baseScaleStdDeviation4 = 10.Percent();
      RelTile1i minForestFloorRadius4 = 0.Tiles();
      RelTile1i maxForestFloorRadius4 = 2.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial4 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow4 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics4 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Birch/Birch4-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics4 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Birch/Birch4-Color1.prefab", "Assets/Base/Trees/Birch/Birch4-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Birch/BirchLogCut.prefab", 8.0.Tiles(), "Assets/Base/Icons/DeciduousTree.svg");
      TreeProto proto4 = new TreeProto(birchTreeDry, strings4, baseScaleStdDeviation4, minForestFloorRadius4, maxForestFloorRadius4, 3, true, forestFloorMaterial4, layoutOrThrow4, layoutEntityGraphics4, treeGraphics4);
      TreeProto treeProto4 = protosDb4.Add<TreeProto>(proto4);
      ProtosDb protosDb5 = prototypesDb;
      Proto.ID mapleTree = Ids.Trees.MapleTree;
      Proto.Str strings5 = new Proto.Str(Loc.Str(Ids.Trees.MapleTree.ToString() + "__desc", "Maple tree", ""), descShort);
      Percent baseScaleStdDeviation5 = 10.Percent();
      RelTile1i minForestFloorRadius5 = 0.Tiles();
      RelTile1i maxForestFloorRadius5 = 9.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial5 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow5 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics5 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Maple/Maple1-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics5 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Maple/Maple1-Color1.prefab", "Assets/Base/Trees/Maple/Maple1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Maple/Maple1-Color2.prefab", "Assets/Base/Trees/Maple/Maple1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Maple/Maple5-Color1.prefab", "Assets/Base/Trees/Maple/Maple5-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Maple/Maple5-Color2.prefab", "Assets/Base/Trees/Maple/Maple5-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Maple/Maple7-Color1.prefab", "Assets/Base/Trees/Maple/Maple7-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Maple/Maple7-Color2.prefab", "Assets/Base/Trees/Maple/Maple7-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Maple/MapleLogCut.prefab", 8.0.Tiles(), "Assets/Base/Icons/DeciduousTree.svg");
      TreeProto proto5 = new TreeProto(mapleTree, strings5, baseScaleStdDeviation5, minForestFloorRadius5, maxForestFloorRadius5, 4, false, forestFloorMaterial5, layoutOrThrow5, layoutEntityGraphics5, treeGraphics5);
      TreeProto treeProto5 = protosDb5.Add<TreeProto>(proto5);
      ProtosDb protosDb6 = prototypesDb;
      Proto.ID mapleTreeDry = Ids.Trees.MapleTreeDry;
      Proto.Str strings6 = new Proto.Str(Loc.Str(Ids.Trees.MapleTreeDry.ToString() + "__desc", "Maple tree (dry)", ""), descShort);
      Percent baseScaleStdDeviation6 = 10.Percent();
      RelTile1i minForestFloorRadius6 = 0.Tiles();
      RelTile1i maxForestFloorRadius6 = 2.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial6 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow6 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics6 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Maple/Maple4-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics6 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Maple/Maple4-Color1.prefab", "Assets/Base/Trees/Maple/Maple4-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Maple/MapleLogCut.prefab", 8.0.Tiles(), "Assets/Base/Icons/DeciduousTree.svg");
      TreeProto proto6 = new TreeProto(mapleTreeDry, strings6, baseScaleStdDeviation6, minForestFloorRadius6, maxForestFloorRadius6, 3, true, forestFloorMaterial6, layoutOrThrow6, layoutEntityGraphics6, treeGraphics6);
      TreeProto treeProto6 = protosDb6.Add<TreeProto>(proto6);
      ProtosDb protosDb7 = prototypesDb;
      Proto.ID oakTree = Ids.Trees.OakTree;
      Proto.Str strings7 = new Proto.Str(Loc.Str(Ids.Trees.OakTree.ToString() + "__desc", "Oak tree", ""), descShort);
      Percent baseScaleStdDeviation7 = 10.Percent();
      RelTile1i minForestFloorRadius7 = 0.Tiles();
      RelTile1i maxForestFloorRadius7 = 10.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial7 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow7 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics7 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Oak/Oak1-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics7 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Oak/Oak1-Color1.prefab", "Assets/Base/Trees/Oak/Oak1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Oak/Oak1-Color2.prefab", "Assets/Base/Trees/Oak/Oak1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Oak/Oak2-Color1.prefab", "Assets/Base/Trees/Oak/Oak2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Oak/Oak2-Color2.prefab", "Assets/Base/Trees/Oak/Oak2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Oak/Oak4-Color1.prefab", "Assets/Base/Trees/Oak/Oak4-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Oak/Oak4-Color2.prefab", "Assets/Base/Trees/Oak/Oak4-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Oak/OakLogCut.prefab", 8.0.Tiles(), "Assets/Base/Icons/DeciduousTree.svg");
      TreeProto proto7 = new TreeProto(oakTree, strings7, baseScaleStdDeviation7, minForestFloorRadius7, maxForestFloorRadius7, 4, false, forestFloorMaterial7, layoutOrThrow7, layoutEntityGraphics7, treeGraphics7);
      TreeProto treeProto7 = protosDb7.Add<TreeProto>(proto7);
      ProtosDb protosDb8 = prototypesDb;
      Proto.ID oakTreeDry = Ids.Trees.OakTreeDry;
      Proto.Str strings8 = new Proto.Str(Loc.Str(Ids.Trees.OakTreeDry.ToString() + "__desc", "Oak tree (dry)", ""), descShort);
      Percent baseScaleStdDeviation8 = 10.Percent();
      RelTile1i minForestFloorRadius8 = 0.Tiles();
      RelTile1i maxForestFloorRadius8 = 2.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial8 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow8 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics8 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Oak/Oak3-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics8 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Oak/Oak3-Color1.prefab", "Assets/Base/Trees/Oak/Oak3-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Oak/OakLogCut.prefab", 8.0.Tiles(), "Assets/Base/Icons/DeciduousTree.svg");
      TreeProto proto8 = new TreeProto(oakTreeDry, strings8, baseScaleStdDeviation8, minForestFloorRadius8, maxForestFloorRadius8, 3, true, forestFloorMaterial8, layoutOrThrow8, layoutEntityGraphics8, treeGraphics8);
      TreeProto treeProto8 = protosDb8.Add<TreeProto>(proto8);
      ProtosDb protosDb9 = prototypesDb;
      Proto.ID palmTree = Ids.Trees.PalmTree;
      Proto.Str strings9 = new Proto.Str(Loc.Str(Ids.Trees.PalmTree.ToString() + "__desc", "Palm tree", ""), descShort);
      Percent baseScaleStdDeviation9 = 10.Percent();
      RelTile1i minForestFloorRadius9 = 0.Tiles();
      RelTile1i maxForestFloorRadius9 = 3.Tiles();
      Option<TerrainMaterialProto> forestFloorMaterial9 = (Option<TerrainMaterialProto>) orThrow2;
      EntityLayout layoutOrThrow9 = new EntityLayoutParser(prototypesDb).ParseLayoutOrThrow(new EntityLayoutParams(enforceEmptySurface: true), "(8)");
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx layoutEntityGraphics9 = new LayoutEntityProto.Gfx("Assets/Base/Trees/Palm/Palm1-Color1.prefab", categories: categories);
      TreeProto.TreeGfx treeGraphics9 = new TreeProto.TreeGfx(ImmutableArray.Create<Pair<string, string>>(Pair.Create<string, string>("Assets/Base/Trees/Palm/Palm1-Color1.prefab", "Assets/Base/Trees/Palm/Palm1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Palm/Palm1-Color2.prefab", "Assets/Base/Trees/Palm/Palm1-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Palm/Palm2-Color1.prefab", "Assets/Base/Trees/Palm/Palm2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Palm/Palm2-Color2.prefab", "Assets/Base/Trees/Palm/Palm2-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Palm/Palm3-Color1.prefab", "Assets/Base/Trees/Palm/Palm3-Cut.prefab"), Pair.Create<string, string>("Assets/Base/Trees/Palm/Palm3-Color2.prefab", "Assets/Base/Trees/Palm/Palm3-Cut.prefab")), (Option<string>) "Assets/Base/Trees/Palm/PalmLogCut.prefab", 8.0.Tiles(), "Assets/Base/Icons/DeciduousTree.svg");
      TreeProto proto9 = new TreeProto(palmTree, strings9, baseScaleStdDeviation9, minForestFloorRadius9, maxForestFloorRadius9, 4, false, forestFloorMaterial9, layoutOrThrow9, layoutEntityGraphics9, treeGraphics9);
      TreeProto treeProto9 = protosDb9.Add<TreeProto>(proto9);
      prototypesDb.Add<ForestProto>(new ForestProto(Ids.Forests.ConiferForest, Proto.CreateStr(Ids.Forests.ConiferForest, "Conifer forest", "HIDE"), orThrow2, ImmutableArray.Create<TreeProto>(treeProto1, treeProto2)));
      prototypesDb.Add<TreePlantingGroupProto>(new TreePlantingGroupProto(Ids.PlantingGroups.ConiferGroup, ImmutableArray.Create<TreeProto>(treeProto1, treeProto2), orThrow1.WithQuantity(20), 68.Months(), 76.Months(), 108.Months(), 144.Months(), "TODO"));
      prototypesDb.Add<TreePlantingGroupProto>(new TreePlantingGroupProto(Ids.PlantingGroups.DeciduousGroup, ImmutableArray.Create<TreeProto>(treeProto3, treeProto5, treeProto7), orThrow1.WithQuantity(20), 68.Months(), 76.Months(), 108.Months(), 144.Months(), "TODO"));
      prototypesDb.Add<TreePlantingGroupProto>(new TreePlantingGroupProto(Ids.PlantingGroups.PalmsGroup, ImmutableArray.Create<TreeProto>(treeProto9), orThrow1.WithQuantity(10), 68.Months(), 76.Months(), 108.Months(), 144.Months(), "TODO"));
      prototypesDb.Add<TreePlantingGroupProto>(new TreePlantingGroupProto(Ids.PlantingGroups.NonPlantableGroup, ImmutableArray.Create<TreeProto>(treeProto4, treeProto6, treeProto8), orThrow1.WithQuantity(10), 68.Months(), 76.Months(), 108.Months(), 144.Months(), "TODO"));
    }

    public TreesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
