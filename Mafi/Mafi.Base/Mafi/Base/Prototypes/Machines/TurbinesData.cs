// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.TurbinesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Machines.PowerGenerators;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.MechanicalPower;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class TurbinesData : IModData
  {
    private static readonly Duration RECIPE_DURATION;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto orThrow = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.MechanicalPower);
      string[] strArray1 = new string[5]
      {
        "               ^@X      ",
        "   [3][3][3][3][3][3]   ",
        "P|>[3][3][3][3][3][3]>|Q",
        "   [3][3][3][3][3][3]   ",
        "      A@^               "
      };
      LocStr descShort1 = Loc.Str(Ids.Machines.TurbineSuperPress.ToString() + "__desc", "Uses super pressure steam to create mechanical power.", "description of a super pressure turbine");
      MechPowerGeneratorFromProductProto fromProductProto1 = prototypesDb.Add<MechPowerGeneratorFromProductProto>(new MechPowerGeneratorFromProductProto((StaticEntityProto.ID) Ids.Machines.TurbineSuperPress, Proto.CreateStr((Proto.ID) Ids.Machines.TurbineSuperPress, "Super-pressure turbine", descShort1), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customPortHeights: (Option<IEnumerable<KeyValuePair<char, int>>>) (IEnumerable<KeyValuePair<char, int>>) new Lyst<KeyValuePair<char, int>>()
      {
        Make.Kvp<char, int>('X', 2)
      }), strArray1), Costs.Machines.TurbineSuperPress.MapToEntityCosts(registrator), TurbinesData.RECIPE_DURATION, 8.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamSp)), new ProductQuantity?(8.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamHi))), orThrow, MechPower.FromKw(18000), 0.375.Percent(), 3.Percent(), Option<MechPowerGeneratorFromProductProto>.None, ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()), true)), new MechPowerGeneratorFromProductProto.Gfx("Assets/Base/Machines/PowerPlant/TurbineSP.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/TurbineHP/TurbineHP_Sound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true)));
      fromProductProto1.AddOrReplaceParam((IProtoParam) new ShaftInertiaProtoParam(fromProductProto1.MechPowerOutput, 2.Seconds()));
      string[] strArray2 = new string[5]
      {
        "      A@v               ",
        "   [3][3][3][3][3][3]   ",
        "P|>[3][3][3][3][3][3]>|Q",
        "   [3][3][3][3][3][3]   ",
        "               v@Y      "
      };
      LocStr descShort2 = Loc.Str(Ids.Machines.TurbineHighPress.ToString() + "__desc", "Uses high pressure steam to create mechanical power.", "description of a high pressure turbine");
      MechPowerGeneratorFromProductProto nextTier1 = prototypesDb.Add<MechPowerGeneratorFromProductProto>(new MechPowerGeneratorFromProductProto((StaticEntityProto.ID) Ids.Machines.TurbineHighPressT2, Proto.CreateStr((Proto.ID) Ids.Machines.TurbineHighPressT2, "High-pressure turbine II", descShort2), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customPortHeights: (Option<IEnumerable<KeyValuePair<char, int>>>) (IEnumerable<KeyValuePair<char, int>>) new Lyst<KeyValuePair<char, int>>()
      {
        Make.Kvp<char, int>('A', 2),
        Make.Kvp<char, int>('Y', 2)
      }), strArray2), Costs.Machines.TurbineHighPressT2.MapToEntityCosts(registrator), TurbinesData.RECIPE_DURATION, 8.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamHi)), new ProductQuantity?(8.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamLo))), orThrow, MechPower.FromKw(12000), 0.375.Percent(), 3.Percent(), Option<MechPowerGeneratorFromProductProto>.None, ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()), true)), new MechPowerGeneratorFromProductProto.Gfx("Assets/Base/Machines/PowerPlant/TurbineHP2.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/TurbineHP/TurbineHP_Sound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true)));
      nextTier1.AddOrReplaceParam((IProtoParam) new ShaftInertiaProtoParam(nextTier1.MechPowerOutput, 2.Seconds()));
      MechPowerGeneratorFromProductProto fromProductProto2 = prototypesDb.Add<MechPowerGeneratorFromProductProto>(new MechPowerGeneratorFromProductProto((StaticEntityProto.ID) Ids.Machines.TurbineHighPress, Proto.CreateStr((Proto.ID) Ids.Machines.TurbineHighPress, "High-pressure turbine", descShort2), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customPortHeights: (Option<IEnumerable<KeyValuePair<char, int>>>) (IEnumerable<KeyValuePair<char, int>>) new Lyst<KeyValuePair<char, int>>()
      {
        Make.Kvp<char, int>('A', 2),
        Make.Kvp<char, int>('Y', 2)
      }), strArray2), Costs.Machines.TurbineHighPress.MapToEntityCosts(registrator), TurbinesData.RECIPE_DURATION, 4.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamHi)), new ProductQuantity?(4.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamLo))), orThrow, MechPower.FromKw(6000), 0.375.Percent(), 3.Percent(), (Option<MechPowerGeneratorFromProductProto>) nextTier1, ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()), true)), new MechPowerGeneratorFromProductProto.Gfx("Assets/Base/Machines/PowerPlant/TurbineHP1.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/TurbineHP/TurbineHP_Sound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true)));
      fromProductProto2.AddOrReplaceParam((IProtoParam) new ShaftInertiaProtoParam(fromProductProto2.MechPowerOutput, 2.Seconds()));
      string[] strArray3 = new string[7]
      {
        "            ^@X            ",
        "   [3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3]   ",
        "P|>[3][3][3][3][3][3][3]>|Q",
        "   [3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3]   ",
        "            A@^            "
      };
      LocStr descShort3 = Loc.Str(Ids.Machines.TurbineLowPress.ToString() + "__desc", "Improves power production efficiency by reusing low pressure steam to create mechanical power.", "description of a low pressure turbine");
      MechPowerGeneratorFromProductProto nextTier2 = prototypesDb.Add<MechPowerGeneratorFromProductProto>(new MechPowerGeneratorFromProductProto((StaticEntityProto.ID) Ids.Machines.TurbineLowPressT2, Proto.CreateStr((Proto.ID) Ids.Machines.TurbineLowPressT2, "Low-pressure turbine II", descShort3), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customPortHeights: (Option<IEnumerable<KeyValuePair<char, int>>>) (IEnumerable<KeyValuePair<char, int>>) new Lyst<KeyValuePair<char, int>>()
      {
        Make.Kvp<char, int>('A', 2)
      }), strArray3), Costs.Machines.TurbineLowPressT2.MapToEntityCosts(registrator), TurbinesData.RECIPE_DURATION, 8.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamLo)), new ProductQuantity?(8.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamDepleted))), orThrow, MechPower.FromKw(6000), 0.375.Percent(), 3.Percent(), Option<MechPowerGeneratorFromProductProto>.None, ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()), true)), new MechPowerGeneratorFromProductProto.Gfx("Assets/Base/Machines/PowerPlant/TurbineLP2.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/TurbineLP/TurbineLPSound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true)));
      nextTier2.AddOrReplaceParam((IProtoParam) new ShaftInertiaProtoParam(nextTier2.MechPowerOutput, 2.Seconds()));
      MechPowerGeneratorFromProductProto fromProductProto3 = prototypesDb.Add<MechPowerGeneratorFromProductProto>(new MechPowerGeneratorFromProductProto((StaticEntityProto.ID) Ids.Machines.TurbineLowPress, Proto.CreateStr((Proto.ID) Ids.Machines.TurbineLowPress, "Low-pressure turbine", descShort3), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customPortHeights: (Option<IEnumerable<KeyValuePair<char, int>>>) (IEnumerable<KeyValuePair<char, int>>) new Lyst<KeyValuePair<char, int>>()
      {
        Make.Kvp<char, int>('A', 2)
      }), strArray3), Costs.Machines.TurbineLowPress.MapToEntityCosts(registrator), TurbinesData.RECIPE_DURATION, 4.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamLo)), new ProductQuantity?(4.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.SteamDepleted))), orThrow, MechPower.FromKw(3000), 0.375.Percent(), 3.Percent(), (Option<MechPowerGeneratorFromProductProto>) nextTier2, ImmutableArray.Create<AnimationParams>((AnimationParams) AnimationParams.Loop(new Percent?(50.Percent()), true)), new MechPowerGeneratorFromProductProto.Gfx("Assets/Base/Machines/PowerPlant/TurbineLP1.prefab", (Option<string>) "Assets/Base/Machines/PowerPlant/TurbineLP/TurbineLPSound.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity), useSemiInstancedRendering: true)));
      fromProductProto3.AddOrReplaceParam((IProtoParam) new ShaftInertiaProtoParam(fromProductProto3.MechPowerOutput, 2.Seconds()));
    }

    public TurbinesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TurbinesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TurbinesData.RECIPE_DURATION = 10.Seconds();
    }
  }
}
