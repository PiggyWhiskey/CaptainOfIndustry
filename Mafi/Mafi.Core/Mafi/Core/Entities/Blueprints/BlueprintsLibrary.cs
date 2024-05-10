// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Blueprints.BlueprintsLibrary
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Blueprints
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class BlueprintsLibrary
  {
    internal const int LATEST_VERSION = 2;
    internal const int V2_DECALS = 2;
    internal const int V1_FIRST_VERSION = 1;
    private static readonly TimeSpan TIMESPAN_BETWEEN_BACKUPS;
    private BlueprintsFolder m_root;
    public readonly string PathToFile;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly ConfigSerializationContext m_serializationContext;
    private bool m_saveRequested;
    private Option<BackgroundWorker> m_backgroundSaver;
    private bool m_isSerializationDisabled;
    private readonly Set<EntityId> m_idsToKeepCache;

    public static bool CanBeInBlueprint(Proto proto)
    {
      switch (proto)
      {
        case TransportPillarProto _:
          return false;
        case LayoutEntityProto layoutEntityProto:
          return !layoutEntityProto.CannotBeBuiltByPlayer;
        default:
          return true;
      }
    }

    public BlueprintsLibrary.Status LibraryStatus { get; private set; }

    public int NumberOfBackupsAvailable { get; private set; }

    /// <summary>
    /// Touching Root will trigger an async library load. This makes sure
    /// that the library is loaded only when "needed". This is currently triggered
    /// on UI build.
    /// </summary>
    public IBlueprintsFolder Root
    {
      get
      {
        if (this.m_root == null)
        {
          this.m_root = new BlueprintsFolder(nameof (Root));
          this.loadLibrary();
        }
        return (IBlueprintsFolder) this.m_root;
      }
    }

    public BlueprintsLibrary(
      IFileSystemHelper fileSystemHelper,
      ConfigSerializationContext serializationContext)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_idsToKeepCache = new Set<EntityId>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_serializationContext = serializationContext;
      this.LibraryStatus = BlueprintsLibrary.Status.LoadingInProgress;
      this.PathToFile = Path.Combine(fileSystemHelper.GetDirPath(FileType.Blueprints, true), "blueprints.data");
    }

    private void loadLibrary()
    {
      if (this.m_isSerializationDisabled)
        return;
      BackgroundWorker backgroundLoader = new BackgroundWorker();
      backgroundLoader.DoWork += new DoWorkEventHandler(this.loadLibraryAsync);
      backgroundLoader.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) => backgroundLoader.Dispose());
      backgroundLoader.RunWorkerAsync();
    }

    private void loadLibraryAsync(object sender, DoWorkEventArgs args)
    {
      if (!File.Exists(this.PathToFile))
      {
        this.LibraryStatus = BlueprintsLibrary.Status.NoLibraryFound;
      }
      else
      {
        string end;
        try
        {
          using (StreamReader streamReader = new StreamReader(this.PathToFile))
            end = streamReader.ReadToEnd();
        }
        catch (Exception ex)
        {
          Log.Exception(ex);
          this.LibraryStatus = BlueprintsLibrary.Status.LoadFailedNoAccess;
          return;
        }
        try
        {
          string error;
          IBlueprintItem result;
          if (this.TryParseFromString(end, out error, out result))
          {
            switch (result)
            {
              case BlueprintsFolder folderToMergeFrom:
                this.LibraryStatus = BlueprintsLibrary.Status.LoadSuccess;
                this.m_root.MergeAllItemsFrom(folderToMergeFrom);
                break;
              case Blueprint blueprint:
                this.LibraryStatus = BlueprintsLibrary.Status.LoadSuccess;
                this.m_root.TryAdd((IBlueprint) blueprint);
                break;
              default:
                this.LibraryStatus = BlueprintsLibrary.Status.LoadFailedDueToFormat;
                Log.Error("Loaded unknown item from a blueprint: " + result.GetType().Name + ".");
                break;
            }
            updateBackupFilesCount();
            return;
          }
          Log.Warning("Failed to parse the blueprint library: " + error);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to load the blueprint library");
        }
        updateBackupFilesCount();
        this.LibraryStatus = BlueprintsLibrary.Status.LoadFailedDueToFormat;
      }

      void updateBackupFilesCount()
      {
        try
        {
          int num = 0;
          for (int index = 4; index >= 0; --index)
          {
            if (File.Exists(this.getPathToBackup(index)))
              ++num;
          }
          this.NumberOfBackupsAvailable = num;
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to count backups");
        }
      }
    }

    private void saveLibrary()
    {
      if (this.m_isSerializationDisabled)
        return;
      if (this.m_backgroundSaver.HasValue && this.m_backgroundSaver.Value.IsBusy)
        this.m_saveRequested = true;
      else
        this.startSave();
    }

    private void startSave()
    {
      this.LibraryStatus = BlueprintsLibrary.Status.SaveInProgress;
      this.m_saveRequested = false;
      this.m_backgroundSaver = (Option<BackgroundWorker>) new BackgroundWorker();
      this.m_backgroundSaver.Value.DoWork += new DoWorkEventHandler(this.saveLibraryAsync);
      this.m_backgroundSaver.Value.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
      {
        if (!this.m_saveRequested)
          return;
        this.startSave();
      });
      this.m_backgroundSaver.Value.RunWorkerAsync((object) this.m_root.CreateCopyForSave());
    }

    private void saveLibraryAsync(object sender, DoWorkEventArgs args)
    {
      if (!(args.Argument is BlueprintsFolder folder))
      {
        Log.Error("No data to save");
        this.LibraryStatus = BlueprintsLibrary.Status.SaveFailed;
      }
      else
      {
        bool flag = false;
        try
        {
          string str = this.ConvertToString((IBlueprintsFolder) folder);
          if (this.m_saveRequested)
            return;
          try
          {
            this.makeBackupIfNeeded();
          }
          catch (Exception ex)
          {
            flag = true;
            Log.Exception(ex, "Failed to create or move backup files");
          }
          using (StreamWriter streamWriter = new StreamWriter(this.PathToFile, false))
          {
            streamWriter.Write(str);
            streamWriter.Flush();
          }
        }
        catch (Exception ex)
        {
          this.LibraryStatus = BlueprintsLibrary.Status.SaveFailed;
          Log.Exception(ex, "Failed to save the library");
          return;
        }
        this.LibraryStatus = flag ? BlueprintsLibrary.Status.SaveDoneBackupFailed : BlueprintsLibrary.Status.SaveDone;
      }
    }

    private void makeBackupIfNeeded()
    {
      if (!File.Exists(this.PathToFile))
        return;
      string pathToBackup = this.getPathToBackup(0);
      if (File.Exists(pathToBackup))
      {
        if (!(DateTime.Now - File.GetLastWriteTime(pathToBackup) > BlueprintsLibrary.TIMESPAN_BETWEEN_BACKUPS))
          return;
        doBackups();
      }
      else
      {
        if (!(DateTime.Now - File.GetCreationTime(this.PathToFile) > BlueprintsLibrary.TIMESPAN_BETWEEN_BACKUPS))
          return;
        doBackups();
      }

      void doBackups()
      {
        int num = 0;
        for (int index = 4; index >= 0; --index)
        {
          string pathToBackup = this.getPathToBackup(index);
          if (File.Exists(pathToBackup))
          {
            if (index == 4)
            {
              File.Delete(pathToBackup);
            }
            else
            {
              File.Move(pathToBackup, this.getPathToBackup(index + 1));
              ++num;
            }
          }
        }
        if (File.Exists(this.PathToFile))
        {
          File.Move(this.PathToFile, this.getPathToBackup(0));
          ++num;
        }
        this.NumberOfBackupsAvailable = num;
      }
    }

    private string getPathToBackup(int index)
    {
      return Path.Combine(this.m_fileSystemHelper.GetDirPath(FileType.Blueprints, false), "blueprints-backup{0}.data".FormatInvariant((object) index));
    }

    internal void TestOnly_DisableSerialization() => this.m_isSerializationDisabled = true;

    /// <summary>
    /// Creates a blueprint instance without adding it to the library.
    /// </summary>
    public bool TryCreateBlueprint(
      string name,
      ImmutableArray<EntityConfigData> items,
      Lyst<TileSurfaceCopyPasteData> surfaceData,
      out IBlueprint blueprint,
      out string error,
      bool doNotNormalizePositions = false)
    {
      blueprint = (IBlueprint) null;
      if (items.IsEmpty && surfaceData.IsEmpty)
      {
        error = "Cannot create an empty blueprint.";
        return false;
      }
      foreach (EntityConfigData entityConfigData in items)
      {
        if (entityConfigData.Prototype.IsNone)
        {
          error = "Cannot create a blueprint with a missing proto!";
          return false;
        }
        if (!BlueprintsLibrary.CanBeInBlueprint(entityConfigData.Prototype.Value))
        {
          error = string.Format("Proto {0} is not supported in blueprints!", (object) entityConfigData.Prototype.Value.Id);
          return false;
        }
      }
      if (!doNotNormalizePositions)
        this.normalizeBlueprintPositions(items, surfaceData);
      this.RemoveExternalReferences(items);
      error = "";
      blueprint = (IBlueprint) new Blueprint(name, items, surfaceData.ToImmutableArray(), this.m_serializationContext);
      return true;
    }

    public Option<IBlueprint> AddBlueprint(
      IBlueprintsFolder targetFolder,
      ImmutableArray<EntityConfigData> items,
      Lyst<TileSurfaceCopyPasteData> surfaceData)
    {
      string name = TrCore.NewBlueprintTitlePlaceholder.TranslatedString;
      for (int index = 2; index < 30 && !targetFolder.Blueprints.AsEnumerable().All<IBlueprint>((Func<IBlueprint, bool>) (x => x.Name != name)); ++index)
        name = string.Format("{0} #{1}", (object) TrCore.NewBlueprintTitlePlaceholder, (object) index);
      IBlueprint blueprint;
      string error;
      if (!this.TryCreateBlueprint(name, items, surfaceData, out blueprint, out error))
      {
        Log.Error("Failed to create blueprint: " + error);
        return Option<IBlueprint>.None;
      }
      ((BlueprintsFolder) targetFolder).TryAdd(blueprint);
      this.saveLibrary();
      return blueprint.SomeOption<IBlueprint>();
    }

    public bool TryAddBlueprintFromString(
      IBlueprintsFolder targetFolder,
      string code,
      out IBlueprintItem result)
    {
      if (!this.TryParseFromString(code, out string _, out result))
        return false;
      if (result is Blueprint blueprint)
      {
        ((BlueprintsFolder) targetFolder).TryAdd((IBlueprint) blueprint);
        this.saveLibrary();
        return true;
      }
      if (result is BlueprintsFolder newFolder)
      {
        ((BlueprintsFolder) targetFolder).TryAdd((IBlueprintsFolder) newFolder);
        this.saveLibrary();
        return true;
      }
      Log.Error("Unknown type of blueprint");
      result = (IBlueprintItem) null;
      return false;
    }

    public void RenameItem(IBlueprintItem item, string newTitle)
    {
      if (string.IsNullOrEmpty(newTitle) || item.Name == newTitle)
        return;
      ((IBlueprintItemFriend) item).SetName(newTitle);
      this.saveLibrary();
    }

    public void SetDescription(IBlueprintItem item, string newDescription)
    {
      if (newDescription == null)
        newDescription = "";
      ((IBlueprintItemFriend) item).SetDescription(newDescription);
      this.saveLibrary();
    }

    public void DeleteItem(IBlueprintsFolder parentFolder, IBlueprintItem item)
    {
      switch (item)
      {
        case Blueprint blueprint:
          if (!((BlueprintsFolder) parentFolder).Remove((IBlueprint) blueprint))
            break;
          this.saveLibrary();
          break;
        case BlueprintsFolder folder:
          if (!((BlueprintsFolder) parentFolder).Remove((IBlueprintsFolder) folder))
            break;
          this.saveLibrary();
          break;
      }
    }

    public IBlueprintsFolder AddNewFolder(IBlueprintsFolder rootDir)
    {
      string name = TrCore.NewFolderTitlePlaceholder.TranslatedString;
      for (int index = 2; index < 30 && !rootDir.Folders.AsEnumerable().All<IBlueprintsFolder>((Func<IBlueprintsFolder, bool>) (x => x.Name != name)); ++index)
        name = string.Format("{0} #{1}", (object) TrCore.NewFolderTitlePlaceholder, (object) index);
      BlueprintsFolder newFolder = new BlueprintsFolder(name);
      Assert.That<bool>(((BlueprintsFolder) rootDir).TryAdd((IBlueprintsFolder) newFolder)).IsTrue();
      this.saveLibrary();
      return (IBlueprintsFolder) newFolder;
    }

    public bool MoveFolder(IBlueprintsFolder folderToMove, IBlueprintsFolder newParent)
    {
      if (folderToMove == newParent || newParent.ParentFolder == folderToMove || folderToMove == this.Root)
        return false;
      if (folderToMove.ParentFolder.HasValue)
        ((BlueprintsFolder) folderToMove.ParentFolder.Value).Remove(folderToMove);
      ((BlueprintsFolder) folderToMove).SetParentFolder(newParent.CreateOption<IBlueprintsFolder>());
      Assert.That<bool>(((BlueprintsFolder) newParent).TryAdd(folderToMove)).IsTrue();
      this.saveLibrary();
      return true;
    }

    public void MoveBlueprint(
      IBlueprint blueprint,
      IBlueprintsFolder currentParent,
      IBlueprintsFolder newParent)
    {
      if (currentParent == newParent || !((BlueprintsFolder) currentParent).Remove(blueprint))
        return;
      Assert.That<bool>(((BlueprintsFolder) newParent).TryAdd(blueprint)).IsTrue();
      this.saveLibrary();
    }

    /// <summary>
    /// NOTE: newIndex is index in a list that contains all the folders and then all the blueprints.
    /// </summary>
    public void TryReorderItem(IBlueprintItem blueprint, IBlueprintsFolder parent, int newIndex)
    {
      if (!((BlueprintsFolder) parent).TryReorderItem(blueprint, newIndex))
        return;
      this.saveLibrary();
    }

    public string ConvertToString(IBlueprint blueprint)
    {
      return BlueprintsLibrary.convertToString(new Action<BlobWriter>(((Blueprint) blueprint).SerializeForBlueprints), false);
    }

    public string ConvertToString(IBlueprintsFolder folder)
    {
      return BlueprintsLibrary.convertToString(new Action<BlobWriter>(((BlueprintsFolder) folder).SerializeForBlueprints), true);
    }

    internal string ConvertToDebugString(IBlueprint blueprint)
    {
      PrintingStream outputStream = new PrintingStream();
      BlobWriter writer = new BlobWriter((Stream) outputStream);
      writer.WriteIntNotNegative(2);
      ((Blueprint) blueprint).SerializeForBlueprints(writer);
      writer.FinalizeSerialization();
      writer.Dispose();
      return outputStream.ToString();
    }

    private static string convertToString(Action<BlobWriter> writerFunc, bool isFolder)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (GZipStream outputStream = new GZipStream((Stream) memoryStream, CompressionLevel.Optimal, true))
        {
          BlobWriter blobWriter = new BlobWriter((Stream) outputStream);
          blobWriter.WriteIntNotNegative(2);
          writerFunc(blobWriter);
          blobWriter.FinalizeSerialization();
          blobWriter.Dispose();
        }
        string base64String = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
        return string.Format("{0}{1}{2}", (object) (char) (isFolder ? 70 : 66), (object) base64String.Length, (object) ':') + base64String;
      }
    }

    public bool TryParseFromString(string strData, out string error, out IBlueprintItem result)
    {
      return BlueprintsLibrary.TryParseFromString(strData, this.m_serializationContext, out error, out result);
    }

    /// <summary>
    /// Tries to parse a blueprint from the given string and returns either Blueprint or BlueprintFolder.
    /// </summary>
    public static bool TryParseFromString(
      string strData,
      ConfigSerializationContext context,
      out string error,
      out IBlueprintItem result)
    {
      result = (IBlueprintItem) null;
      if (strData == null)
      {
        error = "Input string is null.";
        return false;
      }
      strData = strData.Trim().Trim('"');
      if (strData.Length <= 4)
      {
        error = "String is too short for a valid blueprint.";
        return false;
      }
      bool flag1 = strData[0] == 'F';
      bool flag2 = strData[0] == 'B';
      if (!flag1 && !flag2)
      {
        error = string.Format("Invalid prefix '{0}', expected '{1}' or '{2}'.", (object) strData[0], (object) 'F', (object) 'B');
        return false;
      }
      int num = strData.IndexOf(':', 1);
      if (num < 0)
      {
        error = string.Format("Failed to find header end char '{0}'.", (object) ':');
        return false;
      }
      string s = strData.Substring(1, num - 1);
      int result1;
      if (!int.TryParse(s, out result1))
      {
        error = "Failed to parse checksum integer from '" + s + "'.";
        return false;
      }
      strData = strData.SubstringSafe(num + 1);
      if (strData.Length != result1)
      {
        error = string.Format("Invalid checksum {0}, expected {1}.", (object) result1, (object) strData.Length);
        return false;
      }
      try
      {
        using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(strData)))
        {
          using (GZipStream inputStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
          {
            BlobReader reader = new BlobReader((Stream) inputStream, 168);
            int libraryVersion = reader.ReadIntNotNegative();
            if (libraryVersion > 2)
            {
              error = string.Format("Too high library version {0}, max is {1}.", (object) libraryVersion, (object) 2);
              return false;
            }
            result = !flag1 ? (IBlueprintItem) Blueprint.DeserializeForBlueprints(reader, context, libraryVersion) : (IBlueprintItem) BlueprintsFolder.DeserializeForBlueprints(reader, context, libraryVersion);
            reader.FinalizeLoading((Option<DependencyResolver>) Option.None);
            reader.Destroy();
            error = "";
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        error = ex.ToString();
        return false;
      }
    }

    private void normalizeBlueprintPositions(
      ImmutableArray<EntityConfigData> configs,
      Lyst<TileSurfaceCopyPasteData> surfaces)
    {
      Tile3i tile3i = Tile3i.MaxValue;
      foreach (EntityConfigData config in configs)
      {
        Option<TransportTrajectory> trajectory = config.Trajectory;
        if (trajectory.HasValue)
        {
          foreach (Tile3i pivot in trajectory.Value.Pivots)
            tile3i = tile3i.Min(pivot);
        }
        else
        {
          TileTransform? transform = config.Transform;
          if (transform.HasValue)
            tile3i = tile3i.Min(transform.Value.Position);
        }
      }
      foreach (TileSurfaceCopyPasteData surface in surfaces)
        tile3i = new Tile3i(tile3i.X.Min(surface.Position.X), tile3i.Y.Min(surface.Position.Y), tile3i.Z);
      RelTile3i offset = -new RelTile3i(tile3i.X, tile3i.Y, tile3i.Z);
      foreach (EntityConfigData config in configs)
      {
        Option<TransportTrajectory> trajectory = config.Trajectory;
        if (trajectory.HasValue)
        {
          config.Trajectory = (Option<TransportTrajectory>) trajectory.Value.OffsetBy(offset);
          ImmutableArray<Tile2i>? pillars = config.Pillars;
          if (pillars.HasValue)
            config.Pillars = new ImmutableArray<Tile2i>?(pillars.Value.Map<Tile2i, RelTile2i>(offset.Xy, (Func<Tile2i, RelTile2i, Tile2i>) ((x, o) => x + o)));
        }
        else
        {
          TileTransform? transform = config.Transform;
          if (transform.HasValue)
            config.Transform = new TileTransform?(transform.Value.OffsetBy(offset));
        }
      }
      for (int index = 0; index < surfaces.Count; ++index)
        surfaces[index] = surfaces[index].NormalizePosition(offset.Xy);
    }

    internal void RemoveExternalReferences(ImmutableArray<EntityConfigData> configs)
    {
      this.m_idsToKeepCache.Clear();
      foreach (EntityConfigData config in configs)
      {
        EntityId? originalEntityId = config.OriginalEntityId;
        if (originalEntityId.HasValue)
          this.m_idsToKeepCache.Add(originalEntityId.Value);
      }
      foreach (EntityConfigData config in configs)
        config.FinalizeAsBlueprint(this.m_idsToKeepCache);
    }

    public static bool IsBlueprintHashValid(string strData, out string error)
    {
      if (strData == null)
      {
        error = "Input string is null.";
        return false;
      }
      strData = strData.Trim().Trim('"');
      if (strData.Length <= 4)
      {
        error = "String is too short for a valid blueprint.";
        return false;
      }
      bool flag1 = strData[0] == 'F';
      bool flag2 = strData[0] == 'B';
      if (!flag1 && !flag2)
      {
        error = string.Format("Invalid prefix '{0}', expected '{1}' or '{2}'.", (object) strData[0], (object) 'F', (object) 'B');
        return false;
      }
      int num = strData.IndexOf(':', 1);
      if (num < 0)
      {
        error = string.Format("Failed to find header end char '{0}'.", (object) ':');
        return false;
      }
      string s = strData.Substring(1, num - 1);
      int result;
      if (!int.TryParse(s, out result))
      {
        error = "Failed to parse checksum integer from '" + s + "'.";
        return false;
      }
      strData = strData.SubstringSafe(num + 1);
      if (strData.Length != result)
      {
        error = string.Format("Invalid checksum {0}, expected {1}.", (object) result, (object) strData.Length);
        return false;
      }
      error = "";
      return true;
    }

    static BlueprintsLibrary()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BlueprintsLibrary.TIMESPAN_BETWEEN_BACKUPS = new TimeSpan(0, 12, 0, 0);
    }

    public enum Status
    {
      NoLibraryFound,
      LoadingInProgress,
      LoadFailedDueToFormat,
      LoadFailedNoAccess,
      LoadSuccess,
      SaveInProgress,
      SaveFailed,
      SaveDone,
      SaveDoneBackupFailed,
    }
  }
}
