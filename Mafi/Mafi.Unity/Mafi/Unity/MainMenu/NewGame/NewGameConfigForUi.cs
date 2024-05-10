// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.NewGameConfigForUi
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Mods;
using Mafi.Core.Terrain.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  /// <summary>Tracks persistent settings for a game.</summary>
  public class NewGameConfigForUi
  {
    public readonly Set<ModData> EnabledModTypes;
    public GameDifficultyConfig DifficultyConfig;
    private NewGameConfigForUi.GameNameValidationResult? m_lastPartialValidationResult;
    private NewGameConfigForUi.GameNameValidationResult? m_lastFullValidationResult;
    private readonly IFileSystemHelper m_fsHelper;
    private static readonly Random s_random;
    private static readonly string[] ADJECTIVES;
    private static readonly string[] NOUNS;

    public Option<NewGameMapSelection> Map { get; set; }

    public bool IsSelectedMapCorrupted { get; set; }

    public int PreviewIndex { get; set; }

    public int StartingLocationIndex { get; set; }

    public bool ShowResourcesOnMap { get; set; }

    public int DifficultyIndex { get; set; }

    public string GameName { get; private set; }

    public NewGameConfigForUi.GameNameValidationResult? LastGameNameValidationResult
    {
      get => this.m_lastFullValidationResult ?? this.m_lastPartialValidationResult;
    }

    public string RandomSeed { get; set; }

    public NewGameConfigForUi(IFileSystemHelper fsHelper)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.EnabledModTypes = new Set<ModData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_fsHelper = fsHelper;
      this.SetGameName(NewGameConfigForUi.GetRandomName());
    }

    public ImmutableArray<IConfig> ToConfigs()
    {
      if (this.Map.IsNone)
        throw new InvalidOperationException("No map was selected.");
      if (this.StartingLocationIndex < 0 || this.StartingLocationIndex >= this.Map.Value.AdditionalData.StartingLocations.Length)
      {
        Log.Error("Invalid starting location index.");
        this.StartingLocationIndex = 0;
      }
      if (!this.m_lastPartialValidationResult.HasValue)
        Log.Error("Starting new game with no game name validation result!");
      return ImmutableArray.Create<IConfig>((IConfig) this.Map.Value.FactoryConfig, (IConfig) this.DifficultyConfig, (IConfig) TutorialsConfig.CreateConfig(true), (IConfig) new GameNameConfig(this.GameName), (IConfig) new RandomSeedConfig(this.RandomSeed), (IConfig) new StartingLocationConfig(this.StartingLocationIndex));
    }

    public void SetGameName(string gameName)
    {
      string finalName;
      this.m_lastPartialValidationResult = new NewGameConfigForUi.GameNameValidationResult?(this.validateGameNamePartial(gameName, out finalName));
      this.m_lastFullValidationResult = new NewGameConfigForUi.GameNameValidationResult?();
      this.GameName = finalName;
    }

    public bool CanStartGame()
    {
      try
      {
        string finalName;
        this.m_lastFullValidationResult = new NewGameConfigForUi.GameNameValidationResult?(this.validateGameNameFull(this.GameName, out finalName));
        this.GameName = finalName;
      }
      catch
      {
        this.m_lastFullValidationResult = new NewGameConfigForUi.GameNameValidationResult?(NewGameConfigForUi.GameNameValidationResult.ErrorCannotReadWrite);
      }
      NewGameConfigForUi.GameNameValidationResult? validationResult1 = this.m_lastFullValidationResult;
      NewGameConfigForUi.GameNameValidationResult validationResult2 = NewGameConfigForUi.GameNameValidationResult.ErrorInvalidChars;
      if (!(validationResult1.GetValueOrDefault() == validationResult2 & validationResult1.HasValue))
      {
        NewGameConfigForUi.GameNameValidationResult? validationResult3 = this.m_lastFullValidationResult;
        NewGameConfigForUi.GameNameValidationResult validationResult4 = NewGameConfigForUi.GameNameValidationResult.ErrorCannotCreateDir;
        if (!(validationResult3.GetValueOrDefault() == validationResult4 & validationResult3.HasValue))
        {
          NewGameConfigForUi.GameNameValidationResult? validationResult5 = this.m_lastFullValidationResult;
          NewGameConfigForUi.GameNameValidationResult validationResult6 = NewGameConfigForUi.GameNameValidationResult.ErrorCannotReadWrite;
          return !(validationResult5.GetValueOrDefault() == validationResult6 & validationResult5.HasValue);
        }
      }
      return false;
    }

    private NewGameConfigForUi.GameNameValidationResult validateGameNameFull(
      string name,
      out string finalName)
    {
      NewGameConfigForUi.GameNameValidationResult validationResult = this.validateGameNamePartial(name, out finalName);
      if (validationResult == NewGameConfigForUi.GameNameValidationResult.ErrorInvalidChars)
        return validationResult;
      string dirPath = this.m_fsHelper.GetDirPath(FileType.GameSave, false, finalName);
      if (validationResult != NewGameConfigForUi.GameNameValidationResult.AlreadyExistsButEmpty)
      {
        if (validationResult != NewGameConfigForUi.GameNameValidationResult.AlreadyExists)
        {
          try
          {
            Directory.CreateDirectory(dirPath);
          }
          catch (Exception ex)
          {
            return NewGameConfigForUi.GameNameValidationResult.ErrorCannotCreateDir;
          }
        }
      }
      string path = Path.Combine(dirPath, "readWriteTest.tmp");
      try
      {
        using (FileStream fileStream = File.Create(path))
        {
          fileStream.WriteByte((byte) 0);
          fileStream.Close();
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to create a test file at " + path);
        return NewGameConfigForUi.GameNameValidationResult.ErrorCannotReadWrite;
      }
      try
      {
        File.Delete(path);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to delete a test file at " + path);
        return NewGameConfigForUi.GameNameValidationResult.ErrorCannotReadWrite;
      }
      return NewGameConfigForUi.GameNameValidationResult.Ok;
    }

    private NewGameConfigForUi.GameNameValidationResult validateGameNamePartial(
      string name,
      out string finalName)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        finalName = "";
        return NewGameConfigForUi.GameNameValidationResult.Ok;
      }
      finalName = name.Trim();
      if (!FileSystemHelper.IsDirectoryNameAllowed(finalName))
        return NewGameConfigForUi.GameNameValidationResult.ErrorInvalidChars;
      bool isDirEmpty;
      if (!this.doesDirectoryExist(finalName, out finalName, out isDirEmpty))
        return NewGameConfigForUi.GameNameValidationResult.Ok;
      return !isDirEmpty ? NewGameConfigForUi.GameNameValidationResult.AlreadyExists : NewGameConfigForUi.GameNameValidationResult.AlreadyExistsButEmpty;
    }

    private bool doesDirectoryExist(string gameName, out string finalName, out bool isDirEmpty)
    {
      string dirPath1 = this.m_fsHelper.GetDirPath(FileType.GameSave, false);
      string dirPath2 = this.m_fsHelper.GetDirPath(FileType.GameSave, false, gameName);
      isDirEmpty = false;
      if (Directory.Exists(dirPath2))
      {
        isDirEmpty = Directory.GetFiles(dirPath2).IsEmpty<string>();
        string[] array = ((IEnumerable<DirectoryInfo>) new DirectoryInfo(dirPath1).GetDirectories()).Select<DirectoryInfo, string>((Func<DirectoryInfo, string>) (x => x.Name)).ToArray<string>();
        if (((IEnumerable<string>) array).Contains<string>(gameName))
        {
          finalName = gameName;
          return true;
        }
        string str = ((IEnumerable<string>) array).FirstOrDefault<string>((Func<string, bool>) (x => x.Equals(gameName, StringComparison.InvariantCultureIgnoreCase)));
        if (str == null)
        {
          Log.Error("Cannot find a directory " + gameName + " even though it exists!");
          finalName = gameName;
          return false;
        }
        finalName = str;
        return true;
      }
      finalName = gameName;
      return false;
    }

    public static string GetRandomSeed()
    {
      return new string(Enumerable.Repeat<string>("abcdefghijklmnopqrstuvwxyz0123456789", 12).Select<string, char>((Func<string, char>) (s => s[NewGameConfigForUi.s_random.Next(s.Length - 1)])).ToArray<char>());
    }

    public static string GetRandomName()
    {
      return NewGameConfigForUi.ADJECTIVES[NewGameConfigForUi.s_random.Next(NewGameConfigForUi.ADJECTIVES.Length - 1)] + " " + NewGameConfigForUi.NOUNS[NewGameConfigForUi.s_random.Next(NewGameConfigForUi.NOUNS.Length - 1)];
    }

    static NewGameConfigForUi()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      NewGameConfigForUi.s_random = new Random();
      NewGameConfigForUi.ADJECTIVES = new string[38]
      {
        "Amber",
        "Abandoned",
        "Barren",
        "Captain's",
        "Emergent",
        "Expansive",
        "Flourishing",
        "Fresh",
        "Green",
        "Haunting",
        "Hopeful",
        "Industrial",
        "Last-Stop",
        "Lush",
        "New",
        "Oak",
        "Pirate",
        "Pioneering",
        "Promised",
        "Prosperous",
        "Radiant",
        "Rainy",
        "Reclaimed",
        "Remote",
        "Resourceful",
        "Revitalized",
        "Renewed",
        "Secluded",
        "Second-Chance",
        "Serene",
        "Shipping",
        "Shimmering",
        "Strategic",
        "Sunny",
        "Sunrise",
        "Transport",
        "Untamed",
        "Unyielding"
      };
      NewGameConfigForUi.NOUNS = new string[41]
      {
        "Archipelago",
        "Bastion",
        "Bay",
        "Bend",
        "Border",
        "Colony",
        "Cove",
        "Enclave",
        "Expanse",
        "Factory",
        "Forest",
        "Forge",
        "Foundry",
        "Frontier",
        "Grid",
        "Greens",
        "Harbor",
        "Haven",
        "Industry",
        "Island",
        "Land",
        "Landing",
        "Marina",
        "Meadow",
        "Nook",
        "Oasis",
        "Outpost",
        "Plains",
        "Powerhouse",
        "Quay",
        "Reef",
        "Refinery",
        "Retreat",
        "Sanctuary",
        "Shelter",
        "Shore",
        "Shores",
        "Territory",
        "Vault",
        "Waters",
        "Whales"
      };
    }

    public enum GameNameValidationResult
    {
      ErrorInvalidChars,
      ErrorCannotCreateDir,
      ErrorCannotReadWrite,
      AlreadyExists,
      AlreadyExistsButEmpty,
      Ok,
    }
  }
}
