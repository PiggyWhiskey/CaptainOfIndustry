// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Settlements.SettlementDecorationsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Settlements
{
  public class SettlementDecorationsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow("(4)".RepeatString(28).Repeat<string>(28));
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID settlementPillar = Ids.Buildings.SettlementPillar;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementPillar, "Square with column");
      EntityLayout layout1 = layoutOrThrow;
      EntityCosts entityCosts1 = Costs.Buildings.SettlementPillar.MapToEntityCosts(registrator);
      Option<SettlementDecorationModuleProto> none1 = (Option<SettlementDecorationModuleProto>) Option.None;
      Upoints upointsBonusToNearbyHousing1 = 1.Upoints();
      Option<SettlementDecorationModuleProto> nextTier1 = none1;
      ImmutableArray<ToolbarCategoryProto>? categories1 = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/Decorations/SquarePillar.prefab", categories: categories1);
      SettlementDecorationModuleProto proto1 = new SettlementDecorationModuleProto(settlementPillar, str1, layout1, entityCosts1, upointsBonusToNearbyHousing1, 1, nextTier1, graphics1);
      SettlementDecorationModuleProto decorationModuleProto = protosDb1.Add<SettlementDecorationModuleProto>(proto1);
      ProtosDb protosDb2 = prototypesDb;
      StaticEntityProto.ID settlementSquare1 = Ids.Buildings.SettlementSquare1;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementSquare1, "Square (light)");
      EntityLayout layout2 = layoutOrThrow;
      EntityCosts entityCosts2 = Costs.Buildings.SettlementSquare.MapToEntityCosts(registrator);
      Option<SettlementDecorationModuleProto> none2 = (Option<SettlementDecorationModuleProto>) Option.None;
      Upoints upointsBonusToNearbyHousing2 = 1.Upoints();
      Option<SettlementDecorationModuleProto> nextTier2 = none2;
      categories1 = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/Decorations/SquareBlankLight.prefab", categories: categories1);
      SettlementDecorationModuleProto proto2 = new SettlementDecorationModuleProto(settlementSquare1, str2, layout2, entityCosts2, upointsBonusToNearbyHousing2, 1, nextTier2, graphics2);
      protosDb2.Add<SettlementDecorationModuleProto>(proto2);
      ProtosDb protosDb3 = prototypesDb;
      StaticEntityProto.ID settlementSquare2 = Ids.Buildings.SettlementSquare2;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementSquare2, "Square (dark)");
      EntityLayout layout3 = layoutOrThrow;
      EntityCosts entityCosts3 = Costs.Buildings.SettlementSquare.MapToEntityCosts(registrator);
      Option<SettlementDecorationModuleProto> none3 = (Option<SettlementDecorationModuleProto>) Option.None;
      Upoints upointsBonusToNearbyHousing3 = 1.Upoints();
      Option<SettlementDecorationModuleProto> nextTier3 = none3;
      categories1 = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      LayoutEntityProto.Gfx graphics3 = new LayoutEntityProto.Gfx("Assets/Base/Settlements/Decorations/SquareBlankDark.prefab", categories: categories1);
      SettlementDecorationModuleProto proto3 = new SettlementDecorationModuleProto(settlementSquare2, str3, layout3, entityCosts3, upointsBonusToNearbyHousing3, 1, nextTier3, graphics3);
      protosDb3.Add<SettlementDecorationModuleProto>(proto3);
      ProtosDb protosDb4 = prototypesDb;
      StaticEntityProto.ID settlementFountain = Ids.Buildings.SettlementFountain;
      Proto.Str str4 = Proto.CreateStr((Proto.ID) Ids.Buildings.SettlementFountain, "Square with fountain");
      EntityLayout layout4 = layoutOrThrow;
      EntityCosts entityCosts4 = Costs.Buildings.SettlementFountain.MapToEntityCosts(registrator);
      Option<SettlementDecorationModuleProto> none4 = (Option<SettlementDecorationModuleProto>) Option.None;
      Upoints upointsBonusToNearbyHousing4 = 1.Upoints();
      Option<SettlementDecorationModuleProto> nextTier4 = none4;
      string prefabPath = decorationModuleProto.Graphics.PrefabPath;
      categories1 = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Housing));
      RelTile3f prefabOrigin = new RelTile3f();
      Option<string> customIconPath = new Option<string>();
      ColorRgba color = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
      ImmutableArray<ToolbarCategoryProto>? categories2 = categories1;
      ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
      LayoutEntityProto.Gfx graphics4 = new LayoutEntityProto.Gfx(prefabPath, prefabOrigin, customIconPath, color, visualizedLayers: visualizedLayers, categories: categories2, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
      SettlementDecorationModuleProto proto4 = new SettlementDecorationModuleProto(settlementFountain, str4, layout4, entityCosts4, upointsBonusToNearbyHousing4, 1, nextTier4, graphics4);
      protosDb4.Add<SettlementDecorationModuleProto>(proto4).SetAvailability(false);
    }

    public SettlementDecorationsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
