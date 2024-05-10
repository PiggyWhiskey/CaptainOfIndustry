// Decompiled with JetBrains decompiler
// Type: Mafi.Core.FileSystemHelper
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Mafi.Core
{
  public class FileSystemHelper : IFileSystemHelper
  {
    public const string ASSET_BUNDLES_DIR_NAME = "AssetBundles";
    public const string DLCS_DIR_NAME = "DLCs";
    internal const string DOCUMENTS_ROOT_DIR_NAME = "Captain of Industry";
    public const string MAPS_DIR_NAME = "Maps";
    public const string MAP_NAME_SUFFIX = ".map";
    public const string MAP_WIP_NAME_SUFFIX = ".wip";
    public const string MAP_AUTOSAVE_NAME_SUFFIX = ".autosave";
    private static readonly string APP_DATA_DIR_PATH;
    internal static readonly string GAME_DATA_ROOT_DIR_PATH;
    private static readonly string GAME_DATA_ROOT_DIR_PATH_LEGACY;
    private static readonly string WORK_DIR_PATH;
    private static readonly IReadOnlyDictionary<FileType, string> DIR_NAMES;
    private static readonly IReadOnlyDictionary<FileType, string> EXTENSIONS;
    private static readonly Regex MATCH_INVALID_CHARS;
    /// <summary>
    /// From: https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file?redirectedfrom=MSDN
    /// </summary>
    private static readonly Set<string> DISALLOWED_NAMES;

    public static bool ContainsInvalidFileNameCharacters(string fileName)
    {
      return FileSystemHelper.MATCH_INVALID_CHARS.IsMatch(fileName);
    }

    public static string RemoveInvalidFileNameCharacters(string fileName)
    {
      return FileSystemHelper.MATCH_INVALID_CHARS.Replace(fileName, "_");
    }

    public string GameDataRootDirPath { get; private set; }

    public string GameDataRootDirPathLegacy => FileSystemHelper.GAME_DATA_ROOT_DIR_PATH_LEGACY;

    public string WorkDirPath => FileSystemHelper.WORK_DIR_PATH;

    public FileSystemHelper()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(FileSystemHelper.GAME_DATA_ROOT_DIR_PATH);
    }

    public FileSystemHelper(string rootPath)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GameDataRootDirPath = rootPath;
    }

    public string GetFileName(string fileName, FileType fileType)
    {
      return FileSystemHelper.RemoveInvalidFileNameCharacters(fileName.Trim()) + this.GetExtension(fileType);
    }

    public string GetDirName(FileType fileType)
    {
      string dirName;
      if (FileSystemHelper.DIR_NAMES.TryGetValue(fileType, out dirName))
        return dirName;
      Assert.Fail(string.Format("Unknown dir type requested: {0}", (object) fileType));
      return "Unknown";
    }

    public string GetExtension(FileType fileType)
    {
      string extension;
      if (FileSystemHelper.EXTENSIONS.TryGetValue(fileType, out extension))
        return extension;
      Assert.Fail(string.Format("Unknown file type requested: {0}", (object) fileType));
      return "";
    }

    public string GetDirPath(FileType fileType, bool ensureExists, string subDir = null)
    {
      string str = Path.Combine(this.GameDataRootDirPath, this.GetDirName(fileType));
      if (subDir != null)
        str = Path.Combine(str, FileSystemHelper.RemoveInvalidFileNameCharacters(subDir));
      string fullPath = Path.GetFullPath(str);
      Assert.That<bool>(fullPath.StartsWith(this.GameDataRootDirPath)).IsTrue();
      if (ensureExists && !Directory.Exists(fullPath))
      {
        Log.Info(string.Format("Created a new dir for file type '{0}': {1}", (object) fileType, (object) fullPath));
        Directory.CreateDirectory(fullPath);
      }
      return fullPath;
    }

    public FileInfo[] GetAllFiles(FileType fileType, string pattern = "*", string subDir = null)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirPath(fileType, false, subDir));
        return directoryInfo.Exists ? directoryInfo.GetFiles(pattern + this.GetExtension(fileType)) : Array.Empty<FileInfo>();
      }
      catch (Exception ex)
      {
        Log.Exception(ex, string.Format("Failed to list files of type {0}.", (object) fileType));
        return Array.Empty<FileInfo>();
      }
    }

    public string AnonymizePath(string path)
    {
      path = Path.GetFullPath(path);
      path = path.Replace(FileSystemHelper.APP_DATA_DIR_PATH, "%APPDATA%");
      return path;
    }

    public static bool IsDirectoryNameAllowed(string name)
    {
      return !name.IsNullOrEmpty() && name.IndexOf('.') == -1 && name.IndexOf('~') == -1 && name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && !FileSystemHelper.DISALLOWED_NAMES.Contains(name.Trim().ToUpperInvariant());
    }

    static FileSystemHelper()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      FileSystemHelper.APP_DATA_DIR_PATH = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
      FileSystemHelper.GAME_DATA_ROOT_DIR_PATH = Path.GetFullPath(Path.Combine(FileSystemHelper.APP_DATA_DIR_PATH, "Captain of Industry"));
      FileSystemHelper.GAME_DATA_ROOT_DIR_PATH_LEGACY = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Captain of Industry"));
      FileSystemHelper.WORK_DIR_PATH = Path.GetFullPath(".");
      FileSystemHelper.DIR_NAMES = (IReadOnlyDictionary<FileType, string>) new Dict<FileType, string>()
      {
        {
          FileType.Misc,
          "Misc"
        },
        {
          FileType.GameSave,
          "Saves"
        },
        {
          FileType.Replay,
          "Replays"
        },
        {
          FileType.Map,
          "Maps"
        },
        {
          FileType.Screenshot,
          "Screenshots"
        },
        {
          FileType.Log,
          "Logs"
        },
        {
          FileType.Debug,
          "Debug"
        },
        {
          FileType.Console,
          "Console"
        },
        {
          FileType.Mod,
          "Mods"
        },
        {
          FileType.Blueprints,
          "Blueprints"
        },
        {
          FileType.AssetsOverrides,
          "AssetsOverrides"
        },
        {
          FileType.TerrainDataCache,
          "TerrainDataCache"
        }
      };
      FileSystemHelper.EXTENSIONS = (IReadOnlyDictionary<FileType, string>) new Dict<FileType, string>()
      {
        {
          FileType.Misc,
          ""
        },
        {
          FileType.GameSave,
          ".save"
        },
        {
          FileType.Replay,
          ".replay"
        },
        {
          FileType.Map,
          ".map"
        },
        {
          FileType.Screenshot,
          ".png"
        },
        {
          FileType.Log,
          ".log"
        },
        {
          FileType.Debug,
          ""
        },
        {
          FileType.Console,
          ".txt"
        },
        {
          FileType.Mod,
          ""
        },
        {
          FileType.Blueprints,
          ""
        },
        {
          FileType.AssetsOverrides,
          ""
        },
        {
          FileType.TerrainDataCache,
          ".cache"
        }
      };
      FileSystemHelper.MATCH_INVALID_CHARS = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]+", RegexOptions.Compiled);
      FileSystemHelper.DISALLOWED_NAMES = new Set<string>()
      {
        "CON",
        "PRN",
        "AUX",
        "NUL",
        "COM0",
        "COM1",
        "COM2",
        "COM3",
        "COM4",
        "COM5",
        "COM6",
        "COM7",
        "COM8",
        "COM9",
        "COM\u00B9",
        "COM\u00B2",
        "COM\u00B3",
        "LPT0",
        "LPT1",
        "LPT2",
        "LPT3",
        "LPT4",
        "LPT5",
        "LPT6",
        "LPT7",
        "LPT8",
        "LPT9",
        "LPT\u00B9",
        "LPT\u00B2",
        "LPT\u00B3"
      };
    }
  }
}
