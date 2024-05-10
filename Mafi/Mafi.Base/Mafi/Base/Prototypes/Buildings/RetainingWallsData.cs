// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.RetainingWallsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class RetainingWallsData : IModData
  {
    public const int RETAINED_HEIGHT_TILES = 5;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ImmutableArray<ToolbarCategoryProto> categoriesProtos = registrator.GetCategoriesProtos(Ids.ToolbarCategories.BuildingsForVehicles);
      LocStr1 locStr1 = Loc.Str1(Ids.Buildings.RetainingWallStraight1.ToString() + "__desc", "Prevents terrain from collapsing. Walls can be placed below the surface to prevent terrain collapse during mining, or above terrain to aid with dumping operations. The placement elevation is adjustable. Walls will collapse if they hold more than {0} units of height or if they are overfilled.", "description of retaining wall");
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.RetainingWallStraight1.ToString() + "_formatted", locStr1.Format(5.ToString()).Value);
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID retainingWallStraight1 = Ids.Buildings.RetainingWallStraight1;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.RetainingWallStraight1, "Retaining wall (short)", alreadyLocalizedStr);
      EntityLayout layout1 = createLayout(1, 1, new string[3]
      {
        "..",
        "##",
        ".."
      });
      EntityCosts entityCosts1 = Costs.Buildings.RetainingWall1.MapToEntityCosts(registrator);
      ImmutableArray<ToolbarCategoryProto>? categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/RetainingWalls/RetainingWall2m.prefab", categories: categories, useInstancedRendering: true);
      RetainingWallProto proto1 = new RetainingWallProto(retainingWallStraight1, str1, layout1, entityCosts1, graphics1);
      protosDb1.Add<RetainingWallProto>(proto1);
      ProtosDb protosDb2 = prototypesDb;
      StaticEntityProto.ID retainingWallStraight4 = Ids.Buildings.RetainingWallStraight4;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.RetainingWallStraight4, "Retaining wall (long)", alreadyLocalizedStr);
      EntityLayout layout2 = createLayout(4, 3, new string[3]
      {
        ".....",
        "#####",
        "....."
      });
      EntityCosts entityCosts2 = Costs.Buildings.RetainingWall4.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/RetainingWalls/RetainingWall8m.prefab", categories: categories, useInstancedRendering: true);
      RetainingWallProto proto2 = new RetainingWallProto(retainingWallStraight4, str2, layout2, entityCosts2, graphics2);
      protosDb2.Add<RetainingWallProto>(proto2);
      ProtosDb protosDb3 = prototypesDb;
      StaticEntityProto.ID retainingWallCorner = Ids.Buildings.RetainingWallCorner;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Buildings.RetainingWallCorner, "Retaining wall (corner)", alreadyLocalizedStr);
      EntityLayout layout3 = createLayout(2, 0, new string[3]
      {
        ".#.",
        ".##",
        "..."
      });
      EntityCosts entityCosts3 = Costs.Buildings.RetainingWall2.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics3 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/RetainingWalls/RetainingWallCorner.prefab", categories: categories, useInstancedRendering: true);
      RetainingWallProto proto3 = new RetainingWallProto(retainingWallCorner, str3, layout3, entityCosts3, graphics3);
      protosDb3.Add<RetainingWallProto>(proto3);
      ProtosDb protosDb4 = prototypesDb;
      StaticEntityProto.ID retainingWallCross = Ids.Buildings.RetainingWallCross;
      Proto.Str str4 = Proto.CreateStr((Proto.ID) Ids.Buildings.RetainingWallCross, "Retaining wall (cross)", alreadyLocalizedStr);
      EntityLayout layout4 = createLayout(2, 0, new string[3]
      {
        ".#.",
        "###",
        ".#."
      });
      EntityCosts entityCosts4 = Costs.Buildings.RetainingWall2.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics4 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/RetainingWalls/RetainingWallXing.prefab", categories: categories, useInstancedRendering: true);
      RetainingWallProto proto4 = new RetainingWallProto(retainingWallCross, str4, layout4, entityCosts4, graphics4);
      protosDb4.Add<RetainingWallProto>(proto4);
      ProtosDb protosDb5 = prototypesDb;
      StaticEntityProto.ID retainingWallTee = Ids.Buildings.RetainingWallTee;
      Proto.Str str5 = Proto.CreateStr((Proto.ID) Ids.Buildings.RetainingWallTee, "Retaining wall (tee)", alreadyLocalizedStr);
      EntityLayout layout5 = createLayout(2, 0, new string[3]
      {
        ".#.",
        "###",
        "..."
      });
      EntityCosts entityCosts5 = Costs.Buildings.RetainingWall2.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics5 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/RetainingWalls/RetainingWallTee.prefab", categories: categories, useInstancedRendering: true);
      RetainingWallProto proto5 = new RetainingWallProto(retainingWallTee, str5, layout5, entityCosts5, graphics5);
      protosDb5.Add<RetainingWallProto>(proto5);

      EntityLayout createLayout(
        int wallLengthTiles,
        int collapseThreshold,
        string[] retainingVerticesLayout)
      {
        string str = "(W)".RepeatString(wallLengthTiles);
        EntityLayoutParser layoutParser = registrator.LayoutParser;
        CustomLayoutToken[] customTokens = new CustomLayoutToken[1]
        {
          new CustomLayoutToken("(W)", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-6, 1, LayoutTileConstraint.Ground | LayoutTileConstraint.NoRubbleAfterCollapse, minTerrainHeight: new int?(-5), maxTerrainHeight: new int?(0))))
        };
        string[] strArray1 = retainingVerticesLayout;
        int? nullable = new int?(collapseThreshold);
        Proto.ID? hardenedFloorSurfaceId = new Proto.ID?();
        string[] customVertexDataLayout = strArray1;
        int? customCollapseVerticesThreshold = nullable;
        ThicknessIRange? customPlacementRange = new ThicknessIRange?();
        Option<IEnumerable<KeyValuePair<char, int>>> customPortHeights = new Option<IEnumerable<KeyValuePair<char, int>>>();
        EntityLayoutParams layoutParams = new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) customTokens, hardenedFloorSurfaceId: hardenedFloorSurfaceId, customVertexDataLayout: customVertexDataLayout, customVertexTransformFn: (Func<TerrainVertexRel, char, TerrainVertexRel>) ((v, c) => c != '#' ? v : v.WithExtraConstraint(LayoutTileConstraint.DisableTerrainPhysics)), customCollapseVerticesThreshold: customCollapseVerticesThreshold, customPlacementRange: customPlacementRange, customPortHeights: customPortHeights);
        string[] strArray2 = new string[2]{ str, str };
        return layoutParser.ParseLayoutOrThrow(layoutParams, strArray2);
      }
    }

    public RetainingWallsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
