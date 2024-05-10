// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.LoadSave.SaveFileUtils
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.SaveGame;
using Mafi.Serialization;
using System;
using System.IO;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.LoadSave
{
  internal static class SaveFileUtils
  {
    public static bool TryLoadSaveInfo(
      this IFileSystemHelper fsHelper,
      string saveName,
      string gameName,
      out GameSaveInfo gameInfo,
      out ImmutableArray<ModInfoRaw> modsInfo,
      out int saveVersion,
      out Option<LoadFailInfo> error)
    {
      gameInfo = GameSaveInfo.Empty;
      error = Option<LoadFailInfo>.None;
      saveVersion = 0;
      modsInfo = ImmutableArray<ModInfoRaw>.Empty;
      try
      {
        string saveFilePath = fsHelper.GetSaveFilePath(saveName, gameName);
        FileInfo fileInfo = new FileInfo(saveFilePath);
        if (!fileInfo.Exists)
        {
          error = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.FileAccessIssue);
          return false;
        }
        if (fileInfo.Length <= 32L)
        {
          error = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Unknown);
          return false;
        }
        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Open))
        {
          GameLoader gameLoader = new GameLoader();
          gameInfo = gameLoader.TryLoadSaveInfo_MayThrow((Stream) fileStream, out modsInfo, out saveVersion);
          if (!SaveVersion.IsCompatibleSaveVersion(saveVersion))
            error = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Version, new int?(saveVersion));
        }
      }
      catch (CorruptedSaveException ex)
      {
        if (!SaveVersion.IsCompatibleSaveVersion(saveVersion))
          error = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Version, new int?(saveVersion));
        else
          error = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.FileCorrupted, messageForPlayer: ex.MessageForPlayer);
      }
      catch
      {
        error = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Unknown);
      }
      return !error.HasValue;
    }

    public static LoadedModsStatus GetModsStatus(this IMain main, ImmutableArray<ModInfoRaw> mods)
    {
      Lyst<SaveModStatus> lyst = new Lyst<SaveModStatus>();
      Set<System.Type> set = new Set<System.Type>();
      ImmutableArray<ModData> immutableArray = main.Mods.FilterCoreAndDlcMods();
      foreach (ModInfoRaw mod in mods)
      {
        if (!GameBuilder.IsOptionalDlc(mod))
        {
          System.Type modType = ModHelper.GetModTypeMaybe(mod.TypeStr).ValueOrNull;
          if (modType == (System.Type) null)
            lyst.Add(new SaveModStatus(mod, true));
          else if (!immutableArray.Any((Func<ModData, bool>) (x => x.ModType == modType)))
          {
            ModData data = main.Mods.FirstOrDefault((Func<ModData, bool>) (x => x.ModType == modType));
            lyst.Add(data != null ? new SaveModStatus(data, true) : new SaveModStatus(mod, true));
            set.Add(modType);
          }
        }
      }
      foreach (ModData filterThirdPartyMod in main.Mods.FilterThirdPartyMods())
      {
        if (!set.Contains(filterThirdPartyMod.ModType))
          lyst.Add(new SaveModStatus(filterThirdPartyMod, false));
      }
      return new LoadedModsStatus(lyst.ToImmutableArray());
    }

    public static Texture2D CreateTextureFromSaveInfo(this GameSaveInfo info)
    {
      Texture2D tex = new Texture2D(info.ThumbnailImageSize.X, info.ThumbnailImageSize.Y, TextureFormat.RGB24, true);
      tex.LoadImage(info.ThumbnailImageBytes, true).AssertTrue("Failed to load thumbnail.");
      return tex;
    }
  }
}
