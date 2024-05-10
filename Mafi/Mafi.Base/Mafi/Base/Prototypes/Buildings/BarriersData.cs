// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.BarriersData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class BarriersData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ImmutableArray<ToolbarCategoryProto> categoriesProtos = registrator.GetCategoriesProtos(Ids.ToolbarCategories.BuildingsForVehicles);
      LocStr descShort = Loc.Str(Ids.Buildings.BarrierStraight1.ToString() + "__desc", "Barrier that blocks vehicle access.", "description of barrier wall");
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID barrierStraight1 = Ids.Buildings.BarrierStraight1;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.BarrierStraight1, "Barrier (straight)", descShort);
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow("[1]");
      EntityCosts entityCosts1 = Costs.Buildings.Barrier1.MapToEntityCosts(registrator);
      ImmutableArray<ToolbarCategoryProto>? categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/Barrier/BarrierStraight.prefab", categories: categories, useInstancedRendering: true);
      BarrierProto proto1 = new BarrierProto(barrierStraight1, str1, layoutOrThrow1, entityCosts1, graphics1);
      protosDb1.Add<BarrierProto>(proto1);
      ProtosDb protosDb2 = prototypesDb;
      StaticEntityProto.ID barrierCorner = Ids.Buildings.BarrierCorner;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.BarrierCorner, "Barrier (corner)", descShort);
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow("[1]");
      EntityCosts entityCosts2 = Costs.Buildings.Barrier1.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/Barrier/BarrierCorner.prefab", categories: categories, useInstancedRendering: true);
      BarrierProto proto2 = new BarrierProto(barrierCorner, str2, layoutOrThrow2, entityCosts2, graphics2);
      protosDb2.Add<BarrierProto>(proto2);
      ProtosDb protosDb3 = prototypesDb;
      StaticEntityProto.ID barrierCross = Ids.Buildings.BarrierCross;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Buildings.BarrierCross, "Barrier (cross)", descShort);
      EntityLayout layoutOrThrow3 = registrator.LayoutParser.ParseLayoutOrThrow("[1]");
      EntityCosts entityCosts3 = Costs.Buildings.Barrier1.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics3 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/Barrier/BarrierCross.prefab", categories: categories, useInstancedRendering: true);
      BarrierProto proto3 = new BarrierProto(barrierCross, str3, layoutOrThrow3, entityCosts3, graphics3);
      protosDb3.Add<BarrierProto>(proto3);
      ProtosDb protosDb4 = prototypesDb;
      StaticEntityProto.ID barrierTee = Ids.Buildings.BarrierTee;
      Proto.Str str4 = Proto.CreateStr((Proto.ID) Ids.Buildings.BarrierTee, "Barrier (tee)", descShort);
      EntityLayout layoutOrThrow4 = registrator.LayoutParser.ParseLayoutOrThrow("[1]");
      EntityCosts entityCosts4 = Costs.Buildings.Barrier1.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics4 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/Barrier/BarrierTee.prefab", categories: categories, useInstancedRendering: true);
      BarrierProto proto4 = new BarrierProto(barrierTee, str4, layoutOrThrow4, entityCosts4, graphics4);
      protosDb4.Add<BarrierProto>(proto4);
      ProtosDb protosDb5 = prototypesDb;
      StaticEntityProto.ID barrierEnding = Ids.Buildings.BarrierEnding;
      Proto.Str str5 = Proto.CreateStr((Proto.ID) Ids.Buildings.BarrierEnding, "Barrier (ending)", descShort);
      EntityLayout layoutOrThrow5 = registrator.LayoutParser.ParseLayoutOrThrow("[1]");
      EntityCosts entityCosts5 = Costs.Buildings.Barrier1.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(categoriesProtos);
      LayoutEntityProto.Gfx graphics5 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/Barrier/BarrierEnd.prefab", categories: categories, useInstancedRendering: true);
      BarrierProto proto5 = new BarrierProto(barrierEnding, str5, layoutOrThrow5, entityCosts5, graphics5);
      protosDb5.Add<BarrierProto>(proto5);
    }

    public BarriersData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
