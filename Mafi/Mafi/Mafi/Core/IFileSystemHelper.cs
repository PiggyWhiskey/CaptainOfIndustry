// Decompiled with JetBrains decompiler
// Type: Mafi.Core.IFileSystemHelper
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.IO;

#nullable disable
namespace Mafi.Core
{
  public interface IFileSystemHelper
  {
    /// <summary>Full path to the game data dir in app data.</summary>
    string GameDataRootDirPath { get; }

    /// <summary>
    /// Path to legacy work directory in documents. Do not use
    /// </summary>
    string GameDataRootDirPathLegacy { get; }

    /// <summary>Full path to working directory.</summary>
    string WorkDirPath { get; }

    /// <summary>
    /// Returns cleaned file name with an extension based on <paramref name="fileType" />.
    /// </summary>
    string GetFileName(string fileName, FileType fileType);

    /// <summary>
    /// Returns extension of given <paramref name="fileType" /> (with the leading period).
    /// </summary>
    string GetExtension(FileType fileType);

    /// <summary>Returns name of directory.</summary>
    string GetDirName(FileType fileType);

    /// <summary>
    /// Returns canonical full path of given directory. Optionally ensures its existence.
    /// </summary>
    string GetDirPath(FileType fileType, bool ensureExists, string subDir = null);

    /// <summary>
    /// Returns all files in requested directory. Pattern is applied before extension.
    /// </summary>
    FileInfo[] GetAllFiles(FileType fileType, string pattern = "*", string subDir = null);

    string AnonymizePath(string path);
  }
}
