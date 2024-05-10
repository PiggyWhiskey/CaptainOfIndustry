// Decompiled with JetBrains decompiler
// Type: Mafi.Core.FileSystemHelperExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Globalization;
using System.IO;

#nullable disable
namespace Mafi.Core
{
  public static class FileSystemHelperExtensions
  {
    /// <summary>Returns whether given file exists.</summary>
    public static bool FileExists(
      this IFileSystemHelper fsHelper,
      string fileName,
      FileType fileType,
      bool noExtension = false,
      string subDir = null)
    {
      return File.Exists(fsHelper.GetFilePath(fileName, fileType, noExtension, subDir));
    }

    /// <summary>
    /// Returns a file name starting with current timestamp and given suffix.
    /// </summary>
    public static string GetTimestampedFileName(
      this IFileSystemHelper fsHelper,
      string suffix,
      FileType fileType,
      bool doNotIncludeSeconds = false)
    {
      return DateTime.Now.ToString(doNotIncludeSeconds ? "yy-MM-dd_HH-mm" : "yy-MM-dd_HH-mm-ss_ffff", (IFormatProvider) CultureInfo.InvariantCulture) + suffix + fsHelper.GetExtension(fileType);
    }

    /// <summary>
    /// Returns clean and canonical a file path starting with current timestamp and given suffix.
    /// </summary>
    public static string GetTimestampedFilePath(
      this IFileSystemHelper fsHelper,
      string suffix,
      FileType fileType,
      bool noExtension = false,
      string subDir = null,
      bool doNotIncludeSeconds = false)
    {
      return fsHelper.GetFilePath(fsHelper.GetTimestampedFileName(suffix, FileType.Misc, doNotIncludeSeconds), fileType, noExtension, subDir);
    }

    /// <summary>
    /// Returns clean and canonical path of given file with an extension based on <paramref name="fileType" />.
    /// </summary>
    public static string GetFilePath(
      this IFileSystemHelper fsHelper,
      string fileName,
      FileType fileType,
      bool noExtension = false,
      string subDir = null)
    {
      return Path.Combine(fsHelper.GetDirPath(fileType, true, subDir), fsHelper.GetFileName(fileName, noExtension ? FileType.Misc : fileType));
    }

    /// <summary>Returns full clean path for given save name.</summary>
    public static string GetSaveFilePath(
      this IFileSystemHelper fsHelper,
      string saveName,
      string gameName,
      bool noExtension = false)
    {
      return fsHelper.GetFilePath(saveName, FileType.GameSave, noExtension, gameName);
    }

    public static ImmutableArray<SaveFileGroup> GetAllSavesGroupedAndSorted(
      this IFileSystemHelper fsHelper)
    {
      string extension = fsHelper.GetExtension(FileType.GameSave);
      string searchPattern = "*" + extension;
      Lyst<SaveFileGroup> groups = new Lyst<SaveFileGroup>();
      Lyst<SaveFileInfo> cache = new Lyst<SaveFileInfo>();
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(fsHelper.GetDirPath(FileType.GameSave, false));
        if (!directoryInfo.Exists)
          return (ImmutableArray<SaveFileGroup>) ImmutableArray.Empty;
        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
          addGroupIfNotEmpty(directory.GetFiles(searchPattern), directory.Name);
        addGroupIfNotEmpty(directoryInfo.GetFiles(searchPattern), "");
        groups.Sort((Comparison<SaveFileGroup>) ((a, b) => b.Saves.First.WriteTimestamp.CompareTo(a.Saves.First.WriteTimestamp)));
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to list all save files.");
      }
      return groups.ToImmutableArray();

      void addGroupIfNotEmpty(FileInfo[] files, string gameName)
      {
        foreach (FileInfo file in files)
        {
          string name = file.Name;
          if (name.EndsWith(extension))
            name = name.Substring(0, name.Length - extension.Length);
          cache.Add(new SaveFileInfo(name, gameName, file.LastWriteTime, file.Length));
        }
        if (!cache.IsNotEmpty)
          return;
        cache.Sort((Comparison<SaveFileInfo>) ((a, b) => b.WriteTimestamp.CompareTo(a.WriteTimestamp)));
        groups.Add(new SaveFileGroup(gameName, cache.ToImmutableArrayAndClear()));
      }
    }

    public static SaveFileInfo? GetLatestSaveFile(this IFileSystemHelper fsHelper)
    {
      foreach (SaveFileGroup saveFileGroup in fsHelper.GetAllSavesGroupedAndSorted())
      {
        if (saveFileGroup.Saves.IsNotEmpty)
          return new SaveFileInfo?(saveFileGroup.Saves.First);
      }
      return new SaveFileInfo?();
    }

    /// <summary>
    /// Requests to delete the given save file. Returns true if delete succeeded.
    /// </summary>
    public static bool DeleteSaveFile(
      this IFileSystemHelper fsHelper,
      string name,
      string gameName)
    {
      string saveFilePath = fsHelper.GetSaveFilePath(name, gameName);
      try
      {
        if (!File.Exists(saveFilePath))
          return false;
        File.Delete(saveFilePath);
        return !File.Exists(saveFilePath);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to delete save file.");
        return false;
      }
    }

    public static bool IsWipMap(this IFileSystemHelper fsHelper, string path)
    {
      return path.EndsWith(".autosave.map") || path.EndsWith(".wip.map");
    }

    public static string GetDlcRootPath(
      this IFileSystemHelper fsHelper,
      string dlcName,
      bool ensureExists,
      string subDir = null)
    {
      string path = subDir == null ? Path.Combine(fsHelper.WorkDirPath, "DLCs", dlcName) : Path.Combine(fsHelper.WorkDirPath, "DLCs", dlcName, subDir);
      if (ensureExists)
        Directory.CreateDirectory(path);
      return path;
    }
  }
}
