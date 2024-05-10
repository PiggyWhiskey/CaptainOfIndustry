// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.MaintenanceDepotsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Maintenance;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class MaintenanceDepotsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      LocStr descShort = Loc.Str(Ids.Buildings.MaintenanceDepotT1.ToString() + "__desc", "Converts products into maintenance which is distributed to all machines, buildings, and vehicles that need it. Important as if there is not enough maintenance, vehicles and machines can break down temporarily.", "description of a maintenance depot");
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      MachineProto.ID maintenanceDepotT3 = Ids.Buildings.MaintenanceDepotT3;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.MaintenanceDepotT3, "Maintenance III depot", descShort);
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow("   [3][4][4][4][4][3]   ", "X~<[3][4][4][4][4][3]<#C", "B#>[3][4][4][4][4][3]<#D", "   [3][4][4][4][4][3]   ", "   [3][4][4][4][4][3]   ", "   [3][4][4][4][4][3]   ", "         [3][3][3][3]   ");
      EntityCosts entityCosts1 = Costs.Buildings.MaintenanceDepotT3.MapToEntityCosts(registrator);
      Electricity consumedPowerPerTick1 = 600.Kw();
      Quantity quantity1 = 480.Quantity();
      Option<MachineProto> none1 = Option<MachineProto>.None;
      Quantity maintenanceBufferExtraCapacity1 = quantity1;
      ImmutableArray<AnimationParams> animationParams1 = ImmutableArray.Create<AnimationParams>((AnimationParams) new LoopAnimationParams(60.Percent()));
      RelTile3f zero1 = RelTile3f.Zero;
      Option<string> none2 = Option<string>.None;
      ImmutableArray<EmissionParams> empty1 = ImmutableArray<EmissionParams>.Empty;
      ImmutableArray<ParticlesParams> empty2 = ImmutableArray<ParticlesParams>.Empty;
      Option<string> machineSoundPrefabPath1 = (Option<string>) "Assets/Base/Buildings/Maintenance/T1/MaintenanceDepotT1Sound.prefab";
      ColorRgba empty3 = ColorRgba.Empty;
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty);
      MachineProto.Gfx graphics1 = new MachineProto.Gfx("Assets/Base/Buildings/Maintenance/MaintenanceDepotT3.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings), zero1, none2, empty2, empty1, machineSoundPrefabPath1, useSemiInstancedRendering: true, color: empty3, visualizedLayers: visualizedLayers);
      MaintenanceDepotProto proto1 = new MaintenanceDepotProto(maintenanceDepotT3, str1, layoutOrThrow1, entityCosts1, consumedPowerPerTick1, none1, maintenanceBufferExtraCapacity1, animationParams1, graphics1);
      MaintenanceDepotProto machine1 = prototypesDb1.Add<MaintenanceDepotProto>(proto1);
      registrator.RecipeProtoBuilder.Start("Maintenance III", Ids.Recipes.MaintenanceT3, (MachineProto) machine1).SetProductsDestroyReason(DestroyReason.Maintenance).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics3).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(60, Ids.Products.MaintenanceT3).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Maintenance III", Ids.Recipes.MaintenanceT3Recycling, (MachineProto) machine1).SetProductsDestroyReason(DestroyReason.Maintenance).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Electronics3).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(60, Ids.Products.MaintenanceT3).AddOutput<RecipeProtoBuilder.State>(5, Ids.Products.Recyclables).BuildAndAdd();
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      MachineProto.ID maintenanceDepotT2 = Ids.Buildings.MaintenanceDepotT2;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.MaintenanceDepotT2, "Maintenance II depot", descShort);
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow("   [3][4][4][4][4][3]   ", "X~<[3][4][4][4][4][3]<#C", "B#>[3][4][4][4][4][3]<#D", "   [3][4][4][4][4][3]   ", "   [3][4][4][4][4][3]   ", "   [3][4][4][4][4][3]   ", "         [3][3][3][3]   ");
      EntityCosts entityCosts2 = Costs.Buildings.MaintenanceDepotT2.MapToEntityCosts(registrator);
      Electricity consumedPowerPerTick2 = 400.Kw();
      Quantity quantity2 = 1280.Quantity();
      Option<MachineProto> none3 = Option<MachineProto>.None;
      Quantity maintenanceBufferExtraCapacity2 = quantity2;
      ImmutableArray<AnimationParams> animationParams2 = ImmutableArray.Create<AnimationParams>((AnimationParams) new LoopAnimationParams(60.Percent()));
      RelTile3f zero2 = RelTile3f.Zero;
      Option<string> none4 = Option<string>.None;
      ImmutableArray<EmissionParams> empty4 = ImmutableArray<EmissionParams>.Empty;
      ImmutableArray<ParticlesParams> empty5 = ImmutableArray<ParticlesParams>.Empty;
      Option<string> machineSoundPrefabPath2 = (Option<string>) "Assets/Base/Buildings/Maintenance/T1/MaintenanceDepotT1Sound.prefab";
      ColorRgba empty6 = ColorRgba.Empty;
      visualizedLayers = new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty);
      MachineProto.Gfx graphics2 = new MachineProto.Gfx("Assets/Base/Buildings/Maintenance/MaintenanceDepotT2.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings), zero2, none4, empty5, empty4, machineSoundPrefabPath2, useSemiInstancedRendering: true, color: empty6, visualizedLayers: visualizedLayers);
      MaintenanceDepotProto proto2 = new MaintenanceDepotProto(maintenanceDepotT2, str2, layoutOrThrow2, entityCosts2, consumedPowerPerTick2, none3, maintenanceBufferExtraCapacity2, animationParams2, graphics2);
      MaintenanceDepotProto machine2 = prototypesDb2.Add<MaintenanceDepotProto>(proto2);
      registrator.RecipeProtoBuilder.Start("Maintenance II", Ids.Recipes.MaintenanceT2, (MachineProto) machine2).SetProductsDestroyReason(DestroyReason.Maintenance).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Electronics2).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(160, Ids.Products.MaintenanceT2).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Maintenance II", Ids.Recipes.MaintenanceT2Recycling, (MachineProto) machine2).SetProductsDestroyReason(DestroyReason.Maintenance).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Electronics2).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(160, Ids.Products.MaintenanceT2).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Recyclables).BuildAndAdd();
      ProtosDb prototypesDb3 = registrator.PrototypesDb;
      MachineProto.ID maintenanceDepotT1 = Ids.Buildings.MaintenanceDepotT1;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Buildings.MaintenanceDepotT1, "Maintenance depot", descShort);
      EntityLayout layoutOrThrow3 = registrator.LayoutParser.ParseLayoutOrThrow("   [3][4][4][4][4][3]   ", "X~<[3][4][4][4][4][3]<#C", "B#>[3][4][4][4][4][3]<#D", "   [3][4][4][4][4][3]   ", "   [3][4][4][4][4][3]   ", "   [3][4][4][4][4][3]   ", "         [3][3][3][3]   ");
      EntityCosts entityCosts3 = Costs.Buildings.MaintenanceDepotT1.MapToEntityCosts(registrator);
      Electricity consumedPowerPerTick3 = 250.Kw();
      Quantity quantity3 = 2880.Quantity();
      Option<MachineProto> none5 = Option<MachineProto>.None;
      Quantity maintenanceBufferExtraCapacity3 = quantity3;
      ImmutableArray<AnimationParams> animationParams3 = ImmutableArray.Create<AnimationParams>((AnimationParams) new LoopAnimationParams(60.Percent()));
      RelTile3f zero3 = RelTile3f.Zero;
      Option<string> none6 = Option<string>.None;
      ImmutableArray<EmissionParams> empty7 = ImmutableArray<EmissionParams>.Empty;
      ImmutableArray<ParticlesParams> empty8 = ImmutableArray<ParticlesParams>.Empty;
      Option<string> machineSoundPrefabPath3 = (Option<string>) "Assets/Base/Buildings/Maintenance/T1/MaintenanceDepotT1Sound.prefab";
      ColorRgba empty9 = ColorRgba.Empty;
      visualizedLayers = new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty);
      MachineProto.Gfx graphics3 = new MachineProto.Gfx("Assets/Base/Buildings/Maintenance/MaintenanceDepotT1.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings), zero3, none6, empty8, empty7, machineSoundPrefabPath3, useSemiInstancedRendering: true, color: empty9, visualizedLayers: visualizedLayers);
      MaintenanceDepotProto proto3 = new MaintenanceDepotProto(maintenanceDepotT1, str3, layoutOrThrow3, entityCosts3, consumedPowerPerTick3, none5, maintenanceBufferExtraCapacity3, animationParams3, graphics3);
      MaintenanceDepotProto machine3 = prototypesDb3.Add<MaintenanceDepotProto>(proto3);
      registrator.RecipeProtoBuilder.Start("Maintenance I", Ids.Recipes.MaintenanceT1, (MachineProto) machine3).SetProductsDestroyReason(DestroyReason.Maintenance).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Electronics).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(160, Ids.Products.MaintenanceT1).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Maintenance I", Ids.Recipes.MaintenanceT1Recycling, (MachineProto) machine3).SetProductsDestroyReason(DestroyReason.Maintenance).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Electronics).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(160, Ids.Products.MaintenanceT1).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Recyclables).BuildAndAdd();
      ProtosDb prototypesDb4 = registrator.PrototypesDb;
      MachineProto.ID maintenanceDepotT0 = Ids.Buildings.MaintenanceDepotT0;
      Proto.Str str4 = Proto.CreateStr((Proto.ID) Ids.Buildings.MaintenanceDepotT0, "Maintenance depot (basic)", descShort);
      EntityLayout layoutOrThrow4 = registrator.LayoutParser.ParseLayoutOrThrow("[3][3][3][3][3][3]   ", "[3][3][3][3][3][3]<#C", "[3][3][3][3][3][3]<#D", "[3][3][3][3][3][3]   ", "[3][3][3][3][3][3]   ", "[3][3][3][3][3][3]   ", "[3][3][3][3][3][3]   ");
      EntityCosts entityCosts4 = Costs.Buildings.MaintenanceDepotT0.MapToEntityCosts(registrator);
      Electricity consumedPowerPerTick4 = 100.Kw();
      Quantity quantity4 = 630.Quantity();
      Option<MachineProto> nextTier = (Option<MachineProto>) (MachineProto) machine3;
      Quantity maintenanceBufferExtraCapacity4 = quantity4;
      ImmutableArray<AnimationParams> empty10 = (ImmutableArray<AnimationParams>) ImmutableArray.Empty;
      RelTile3f zero4 = RelTile3f.Zero;
      Option<string> none7 = Option<string>.None;
      ImmutableArray<EmissionParams> empty11 = ImmutableArray<EmissionParams>.Empty;
      ImmutableArray<ParticlesParams> empty12 = ImmutableArray<ParticlesParams>.Empty;
      Option<string> machineSoundPrefabPath4 = (Option<string>) "Assets/Base/Buildings/Maintenance/T0/MaintenanceDepotT0Sound.prefab";
      ColorRgba empty13 = ColorRgba.Empty;
      visualizedLayers = new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty);
      MachineProto.Gfx graphics4 = new MachineProto.Gfx("Assets/Base/Buildings/Maintenance/MaintenanceDepotT0.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings), zero4, none7, empty12, empty11, machineSoundPrefabPath4, useSemiInstancedRendering: true, color: empty13, visualizedLayers: visualizedLayers);
      MaintenanceDepotProto proto4 = new MaintenanceDepotProto(maintenanceDepotT0, str4, layoutOrThrow4, entityCosts4, consumedPowerPerTick4, nextTier, maintenanceBufferExtraCapacity4, empty10, graphics4);
      MaintenanceDepotProto machine4 = prototypesDb4.Add<MaintenanceDepotProto>(proto4);
      registrator.RecipeProtoBuilder.Start("Maintenance I", Ids.Recipes.MaintenanceT0, (MachineProto) machine4).SetProductsDestroyReason(DestroyReason.Maintenance).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.MechanicalParts).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Electronics).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(35, Ids.Products.MaintenanceT1).BuildAndAdd();
    }

    public MaintenanceDepotsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
