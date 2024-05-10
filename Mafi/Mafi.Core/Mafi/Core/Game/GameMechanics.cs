// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameMechanics
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Game
{
  /// <summary>These are just difficulty presets, not a game logic.</summary>
  public static class GameMechanics
  {
    public static readonly LocStr GameMechanic__Casual;
    public static readonly LocStr GameMechanic__Realism;
    public static readonly LocStr GameMechanic__Challenges;
    public static readonly GameMechanicApplier Casual;
    public static readonly GameMechanicApplier ResourcesBoost;
    public static readonly GameMechanicApplier OreSorting;
    public static readonly GameMechanicApplier Realism;
    public static readonly GameMechanicApplier RealismPlus;
    public static readonly GameMechanicApplier ReducedWorldMines;

    private static LocStrFormatted title(string memberName, string enTitle)
    {
      return (LocStrFormatted) Loc.Str("Mechanic_" + memberName + "__Title", enTitle, "title of a game mechanic");
    }

    private static LocStrFormatted item(string memberName, string itemName, string enItem)
    {
      return (LocStrFormatted) Loc.Str("Mechanic_" + memberName + "__" + itemName, enItem, "item of a game mechanic");
    }

    static GameMechanics()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameMechanics.GameMechanic__Casual = Loc.Str(nameof (GameMechanic__Casual), "Getting started", "Title for group of related game mechanics in the new game flow");
      GameMechanics.GameMechanic__Realism = Loc.Str(nameof (GameMechanic__Realism), nameof (Realism), "Title for group of related game mechanics in the new game flow");
      GameMechanics.GameMechanic__Challenges = Loc.Str(nameof (GameMechanic__Challenges), "Challenges", "Title for the challenge group of game mechanics in the new game flow");
      GameMechanics.Casual = new GameMechanicApplier(GameMechanics.title(nameof (Casual), nameof (Casual)), ImmutableArray.Create<LocStrFormatted>(GameMechanics.item(nameof (Casual), "LogisticsPower", "Belts and storage units don’t require electricity"), GameMechanics.item(nameof (Casual), "WorldMines", "World mines produce less resources when Unity is low, rather than stopping entirely"), GameMechanics.item(nameof (Casual), "InfiniteMines", "Impact from power and computing outages is reduced")), (Action<GameDifficultyConfig>) (config =>
      {
        GameDifficultyConfig.PowerSettingInfo.SetValue(config, GameDifficultyConfig.LogisticsPowerSetting.DoNotConsume);
        GameDifficultyConfig.WorldMinesNoUnityInfo.SetValue(config, GameDifficultyConfig.WorldMinesNoUnitySetting.SlowDown);
        GameDifficultyConfig.PowerLowInfo.SetValue(config, GameDifficultyConfig.PowerLowSetting.SlowDown);
        GameDifficultyConfig.ComputingLowInfo.SetValue(config, GameDifficultyConfig.ComputingLowSetting.SlowDown);
      }), new GameDifficultyPreset[1], "Assets/Unity/UserInterface/MainMenu/NewGame/ConveyorNoPower.svg");
      GameMechanics.ResourcesBoost = new GameMechanicApplier(GameMechanics.title(nameof (ResourcesBoost), "Resources boost"), ImmutableArray.Create<LocStrFormatted>(GameMechanics.item(nameof (ResourcesBoost), "InfiniteMines", "Infinite world mines"), GameMechanics.item(nameof (ResourcesBoost), "Refund", "Full deconstruction refund"), GameMechanics.item(nameof (ResourcesBoost), "ExtraMaterials", "Extra starting resources")), (Action<GameDifficultyConfig>) (config =>
      {
        GameDifficultyConfig.WorldMinesReservesInfo.SetValue(config, Percent.MaxValue);
        GameDifficultyConfig.DeconstructionRefundInfo.SetValue(config, GameDifficultyConfig.DeconstructionRefundSetting.Full);
        GameDifficultyConfig.ExtraStartingMaterialInfo.SetValue(config, 40.Percent());
      }), new GameDifficultyPreset[1], "Assets/Unity/UserInterface/MainMenu/NewGame/AmpleResources.svg");
      GameMechanics.OreSorting = new GameMechanicApplier(GameMechanics.title(nameof (OreSorting), "Ore sorting"), ImmutableArray.Create<LocStrFormatted>(GameMechanics.item(nameof (OreSorting), nameof (OreSorting), "Mined mixed materials in trucks must be processed at a dedicated sorting facility")), (Action<GameDifficultyConfig>) (config => GameDifficultyConfig.OreSortingInfo.SetValue(config, GameDifficultyConfig.OreSortingSetting.Enabled)), new GameDifficultyPreset[2]
      {
        GameDifficultyPreset.Normal,
        GameDifficultyPreset.Hard
      }, "Assets/Unity/UserInterface/MainMenu/NewGame/OreSorting.svg");
      GameMechanics.Realism = new GameMechanicApplier(GameMechanics.title(nameof (Realism), nameof (Realism)), ImmutableArray.Create<LocStrFormatted>(GameMechanics.item(nameof (Realism), "Starvation", "People die from starvation"), GameMechanics.item(nameof (Realism), "VehiclesFuel", "Vehicles can't move without fuel"), GameMechanics.item(nameof (Realism), "ShipsFuel", "Ships can't move without fuel and can't use Unity as a remedy"), GameMechanics.item(nameof (Realism), "Pumps", "Pumps stop working if groundwater is depleted")), (Action<GameDifficultyConfig>) (config =>
      {
        GameDifficultyConfig.StarvationInfo.SetValue(config, GameDifficultyConfig.StarvationSetting.Death);
        GameDifficultyConfig.VehiclesNoFuelInfo.SetValue(config, GameDifficultyConfig.VehiclesNoFuelSetting.Stop);
        GameDifficultyConfig.ShipsNoFuelInfo.SetValue(config, GameDifficultyConfig.ShipNoFuelSetting.StopWorking);
        GameDifficultyConfig.GroundwaterPumpLowInfo.SetValue(config, GameDifficultyConfig.GroundwaterPumpLowSetting.StopWorking);
      }), new GameDifficultyPreset[2]
      {
        GameDifficultyPreset.Normal,
        GameDifficultyPreset.Hard
      }, "Assets/Unity/UserInterface/MainMenu/NewGame/OutOfGas.svg").AddConflict(GameMechanics.Casual);
      GameMechanics.RealismPlus = new GameMechanicApplier(GameMechanics.title(nameof (RealismPlus), "Realism++"), ImmutableArray.Create<LocStrFormatted>(GameMechanics.item(nameof (RealismPlus), "BrokenConsumers", "Machines and other maintenance consumers stop working when broken"), GameMechanics.item(nameof (RealismPlus), "LogisticsPower", "Belts and storage units require power to function and stop working without it"), GameMechanics.item(nameof (RealismPlus), "QuickRepair", "Quick repair action is not available")), (Action<GameDifficultyConfig>) (config =>
      {
        GameDifficultyConfig.ConsumerBrokenInfo.SetValue(config, GameDifficultyConfig.ConsumerBrokenSetting.Stop);
        GameDifficultyConfig.PowerSettingInfo.SetValue(config, GameDifficultyConfig.LogisticsPowerSetting.ConsumeAlways);
        GameDifficultyConfig.QuickRepairInfo.SetValue(config, GameDifficultyConfig.QuickRepairSetting.Disabled);
      }), new GameDifficultyPreset[1]
      {
        GameDifficultyPreset.Hard
      }, "Assets/Unity/UserInterface/MainMenu/NewGame/BrokenTruck.svg").AddDependency(GameMechanics.Realism);
      GameMechanics.ReducedWorldMines = new GameMechanicApplier(GameMechanics.title(nameof (ReducedWorldMines), "Reduced world mines"), ImmutableArray.Create<LocStrFormatted>(GameMechanics.item(nameof (ReducedWorldMines), "WorldMines", "World mines have reduced deposits")), (Action<GameDifficultyConfig>) (config => GameDifficultyConfig.WorldMinesReservesInfo.SetValue(config, -40.Percent())), new GameDifficultyPreset[1]
      {
        GameDifficultyPreset.Hard
      }, "Assets/Unity/UserInterface/MainMenu/NewGame/ScarceResources.svg").AddConflict(GameMechanics.ResourcesBoost);
    }
  }
}
