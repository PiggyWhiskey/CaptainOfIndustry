// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.ModHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;
using System.IO;

#nullable disable
namespace Mafi.Core.SaveGame
{
  public static class ModHelper
  {
    public static ImmutableArray<ModInfoRaw> LoadModsFromSaveFile(
      IFileSystemHelper fsHelper,
      SaveFileInfo saveInfo,
      out Option<LoadFailInfo> errorOnFileLoad)
    {
      try
      {
        string saveFilePath = fsHelper.GetSaveFilePath(saveInfo.NameNoExtension, saveInfo.GameName);
        FileInfo fileInfo = new FileInfo(saveFilePath);
        if (!fileInfo.Exists)
        {
          Log.Error("Save file does NOT exist!");
          errorOnFileLoad = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.FileAccessIssue);
          return ImmutableArray<ModInfoRaw>.Empty;
        }
        if (fileInfo.Length <= 32L)
        {
          errorOnFileLoad = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Unknown);
          return ImmutableArray<ModInfoRaw>.Empty;
        }
        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Open))
        {
          int saveVersion;
          ImmutableArray<ModInfoRaw> immutableArray = new GameLoader().TryLoadMods_MayThrow((Stream) fileStream, out saveVersion);
          errorOnFileLoad = SaveVersion.IsCompatibleSaveVersion(saveVersion) ? Option<LoadFailInfo>.None : (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Version, new int?(saveVersion));
          return immutableArray;
        }
      }
      catch (CorruptedSaveException ex)
      {
        errorOnFileLoad = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Unknown, messageForPlayer: ex.MessageForPlayer);
      }
      catch
      {
        errorOnFileLoad = (Option<LoadFailInfo>) new LoadFailInfo(LoadFailInfo.Reason.Unknown);
      }
      return ImmutableArray<ModInfoRaw>.Empty;
    }

    public static Option<Type> GetModTypeMaybe(string modTypeStr)
    {
      Type modTypeMaybe = (Type) null;
      try
      {
        modTypeMaybe = Type.GetType(modTypeStr);
      }
      catch
      {
      }
      return (Option<Type>) modTypeMaybe;
    }
  }
}
