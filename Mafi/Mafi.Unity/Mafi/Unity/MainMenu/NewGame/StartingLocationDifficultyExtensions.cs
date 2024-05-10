// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.StartingLocationDifficultyExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Terrain.Generation;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public static class StartingLocationDifficultyExtensions
  {
    public static ColorRgba ToColor(this StartingLocationDifficulty difficulty)
    {
      switch (difficulty)
      {
        case StartingLocationDifficulty.Medium:
          return Theme.DifficultyMedium;
        case StartingLocationDifficulty.Hard:
          return Theme.DifficultyHard;
        case StartingLocationDifficulty.Insane:
          return Theme.DifficultyInsane;
        default:
          return Theme.DifficultyEasy;
      }
    }

    public static LocStr ToLabel(this StartingLocationDifficulty difficulty)
    {
      switch (difficulty)
      {
        case StartingLocationDifficulty.Medium:
          return TrCore.StartingLocationDifficulty_Medium;
        case StartingLocationDifficulty.Hard:
          return TrCore.StartingLocationDifficulty_Hard;
        case StartingLocationDifficulty.Insane:
          return TrCore.StartingLocationDifficulty_Insane;
        default:
          return TrCore.StartingLocationDifficulty_Easy;
      }
    }

    public static LocStr ToDescription(this StartingLocationDifficulty difficulty)
    {
      switch (difficulty)
      {
        case StartingLocationDifficulty.Medium:
          return TrCore.StartingLocationDifficulty__MediumTooltip;
        case StartingLocationDifficulty.Hard:
          return TrCore.StartingLocationDifficulty__HardTooltip;
        case StartingLocationDifficulty.Insane:
          return TrCore.StartingLocationDifficulty__InsaneTooltip;
        default:
          return TrCore.StartingLocationDifficulty__EasyTooltip;
      }
    }
  }
}
