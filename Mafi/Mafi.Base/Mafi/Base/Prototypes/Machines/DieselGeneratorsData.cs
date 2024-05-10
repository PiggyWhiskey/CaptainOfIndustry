// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.DieselGeneratorsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Machines.PowerGenerators;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class DieselGeneratorsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProductProto orThrow1 = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Electricity);
      ProductProto orThrow2 = (ProductProto) registrator.PrototypesDb.GetOrThrow<FluidProductProto>((Proto.ID) Ids.Products.Diesel);
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      StaticEntityProto.ID dieselGenerator = (StaticEntityProto.ID) Ids.Machines.DieselGenerator;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Machines.DieselGenerator, "Diesel generator", "Burns diesel to create electricity.");
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow("[3][3][2][2]", "[3][3][2][2]", "[2][2][2][2]", "F@^         ");
      EntityCosts entityCosts1 = Costs.Machines.DieselGenerator.MapToEntityCosts(registrator);
      ProductProto productProto1 = orThrow1;
      Electricity outputElectricity1 = 800.Kw();
      ProductQuantity inputProduct1 = orThrow2.WithQuantity(1);
      ProductQuantity? outputProduct1 = new ProductQuantity?(registrator.PrototypesDb.GetOrThrow<VirtualProductProto>((Proto.ID) IdsCore.Products.PollutedAir).WithQuantity(1));
      ProductProto electricityProto1 = productProto1;
      Duration duration1 = 20.Seconds();
      ImmutableArray<AnimationParams> animationParams1 = ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop());
      ElectricityGeneratorFromProductProto.Gfx graphics1 = new ElectricityGeneratorFromProductProto.Gfx("Assets/Base/Machines/PowerPlant/CombustionEngine.prefab", ImmutableArray.Create<ParticlesParams>(ParticlesParams.Loop("Smoke")), (Option<string>) "Assets/Base/Machines/PowerPlant/CombustionEngine/CombustionEngine_Sound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true);
      ElectricityGeneratorFromProductProto proto1 = new ElectricityGeneratorFromProductProto(dieselGenerator, str1, layoutOrThrow1, entityCosts1, outputElectricity1, 10, inputProduct1, outputProduct1, electricityProto1, 20, duration1, DestroyReason.UsedAsFuel, animationParams1, graphics1);
      prototypesDb1.Add<ElectricityGeneratorFromProductProto>(proto1);
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      StaticEntityProto.ID dieselGeneratorT2 = (StaticEntityProto.ID) Ids.Machines.DieselGeneratorT2;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Machines.DieselGeneratorT2, "Diesel generator II", "More powerful generator. Can also serve as a power backup.");
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow("[3][3][3][3][4][4][4][4]", "[3][3][3][3][4][4][4][4]", "[3][3][3][3][4][4][4][4]", "F@^               v@S   ");
      EntityCosts entityCosts2 = Costs.Machines.DieselGeneratorT2.MapToEntityCosts(registrator);
      ProductProto productProto2 = orThrow1;
      Electricity outputElectricity2 = 5000.Kw();
      ProductQuantity inputProduct2 = orThrow2.WithQuantity(6);
      ProductQuantity? outputProduct2 = new ProductQuantity?(registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Exhaust).WithQuantity(8));
      ProductProto electricityProto2 = productProto2;
      Duration duration2 = 20.Seconds();
      ImmutableArray<AnimationParams> animationParams2 = ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(150.Percent())));
      ElectricityGeneratorFromProductProto.Gfx graphics2 = new ElectricityGeneratorFromProductProto.Gfx("Assets/Base/Machines/PowerPlant/CombustionEngineT2.prefab", (ImmutableArray<ParticlesParams>) ImmutableArray.Empty, (Option<string>) "Assets/Base/Machines/PowerPlant/CombustionEngine/CombustionEngine_Sound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true);
      ElectricityGeneratorFromProductProto proto2 = new ElectricityGeneratorFromProductProto(dieselGeneratorT2, str2, layoutOrThrow2, entityCosts2, outputElectricity2, 10, inputProduct2, outputProduct2, electricityProto2, 4, duration2, DestroyReason.UsedAsFuel, animationParams2, graphics2);
      prototypesDb2.Add<ElectricityGeneratorFromProductProto>(proto2);
    }

    public DieselGeneratorsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
