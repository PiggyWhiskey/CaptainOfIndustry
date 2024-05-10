// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.SelectDifficultyData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Localization;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class SelectDifficultyData
  {
    public Func<GameDifficultyConfig> CreateConfig;
    public string ImageAsset;
    public LocStrFormatted Name;
    public LocStrFormatted Description;
    public LocStrFormatted Food;
    public LocStrFormatted Construction;
    public LocStrFormatted Fuel;
    public LocStrFormatted Maintenance;
    public LocStrFormatted Mining;
    public LocStrFormatted Research;
    public LocStrFormatted Growth;
    public LocStrFormatted Rain;
    public LocStrFormatted Contracts;
    public LocStrFormatted Disease;
    public LocStrFormatted Pollution;
    public LocStrFormatted Unity;
    public static readonly ImmutableArray<SelectDifficultyData> All;
    public static readonly SelectDifficultyData Sailor;
    public static readonly SelectDifficultyData Captain;
    public static readonly SelectDifficultyData Admiral;

    public SelectDifficultyData()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static SelectDifficultyData()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SelectDifficultyData.All = ImmutableArray.Create<SelectDifficultyData>(new SelectDifficultyData()
      {
        CreateConfig = (Func<GameDifficultyConfig>) (() => GameDifficultyConfig.Easy()),
        Name = (LocStrFormatted) GameDifficultyConfig.GameDifficulty__EasyTitle,
        Description = (LocStrFormatted) GameDifficultyConfig.GameDifficulty__EasyDescription,
        ImageAsset = "Assets/Unity/UserInterface/MainMenu/NewGame/Hat_Sailor.png",
        Food = (LocStrFormatted) TrCore.DifficultyFood__Easy,
        Construction = (LocStrFormatted) TrCore.DifficultyConstruction__Easy,
        Fuel = (LocStrFormatted) TrCore.DifficultyFuel__Easy,
        Maintenance = (LocStrFormatted) TrCore.DifficultyMaintenance__Easy,
        Mining = (LocStrFormatted) TrCore.DifficultyMining__Easy,
        Research = (LocStrFormatted) TrCore.DifficultyResearch__Easy,
        Growth = (LocStrFormatted) TrCore.DifficultyGrowth__Easy,
        Rain = (LocStrFormatted) TrCore.DifficultyRainfall__Easy,
        Contracts = (LocStrFormatted) TrCore.DifficultyContracts__Easy,
        Disease = (LocStrFormatted) TrCore.DifficultyDisease__Easy,
        Pollution = (LocStrFormatted) TrCore.DifficultyPollution__Easy,
        Unity = (LocStrFormatted) TrCore.DifficultyUnity__Easy
      }, new SelectDifficultyData()
      {
        CreateConfig = (Func<GameDifficultyConfig>) (() => GameDifficultyConfig.Normal()),
        Name = (LocStrFormatted) GameDifficultyConfig.GameDifficulty__NormalTitle,
        Description = (LocStrFormatted) GameDifficultyConfig.GameDifficulty__NormalDescription,
        ImageAsset = "Assets/Unity/UserInterface/MainMenu/NewGame/Hat_Captain.png",
        Food = (LocStrFormatted) TrCore.DifficultyFood__Normal,
        Construction = (LocStrFormatted) TrCore.DifficultyConstruction__Normal,
        Fuel = (LocStrFormatted) TrCore.DifficultyFuel__Normal,
        Maintenance = (LocStrFormatted) TrCore.DifficultyMaintenance__Normal,
        Mining = (LocStrFormatted) TrCore.DifficultyMining__Normal,
        Research = (LocStrFormatted) TrCore.DifficultyResearch__Normal,
        Growth = (LocStrFormatted) TrCore.DifficultyGrowth__Normal,
        Rain = (LocStrFormatted) TrCore.DifficultyRainfall__Normal,
        Contracts = (LocStrFormatted) TrCore.DifficultyContracts__Normal,
        Disease = (LocStrFormatted) TrCore.DifficultyDisease__Normal,
        Pollution = (LocStrFormatted) TrCore.DifficultyPollution__Normal,
        Unity = (LocStrFormatted) TrCore.DifficultyUnity__Normal
      }, new SelectDifficultyData()
      {
        CreateConfig = (Func<GameDifficultyConfig>) (() => GameDifficultyConfig.Hard()),
        Name = (LocStrFormatted) GameDifficultyConfig.GameDifficulty__AdmiralTitle,
        Description = (LocStrFormatted) GameDifficultyConfig.GameDifficulty__AdmiralDescription,
        ImageAsset = "Assets/Unity/UserInterface/MainMenu/NewGame/Hat_Admiral.png",
        Food = (LocStrFormatted) TrCore.DifficultyFood__Hard,
        Construction = (LocStrFormatted) TrCore.DifficultyConstruction__Hard,
        Fuel = (LocStrFormatted) TrCore.DifficultyFuel__Hard,
        Maintenance = (LocStrFormatted) TrCore.DifficultyMaintenance__Hard,
        Mining = (LocStrFormatted) TrCore.DifficultyMining__Hard,
        Research = (LocStrFormatted) TrCore.DifficultyResearch__Hard,
        Growth = (LocStrFormatted) TrCore.DifficultyGrowth__Hard,
        Rain = (LocStrFormatted) TrCore.DifficultyRainfall__Hard,
        Contracts = (LocStrFormatted) TrCore.DifficultyContracts__Hard,
        Disease = (LocStrFormatted) TrCore.DifficultyDisease__Hard,
        Pollution = (LocStrFormatted) TrCore.DifficultyPollution__Hard,
        Unity = (LocStrFormatted) TrCore.DifficultyUnity__Hard
      });
      SelectDifficultyData.Sailor = SelectDifficultyData.All[0];
      SelectDifficultyData.Captain = SelectDifficultyData.All[1];
      SelectDifficultyData.Admiral = SelectDifficultyData.All[2];
    }
  }
}
