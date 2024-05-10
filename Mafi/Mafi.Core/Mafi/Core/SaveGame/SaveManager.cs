// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.SaveManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Console;
using Mafi.Core.Game;
using Mafi.Core.Input;
using Mafi.Core.Mods;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Serialization;
using Mafi.Serialization.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

#nullable disable
namespace Mafi.Core.SaveGame
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class SaveManager : ISaveManager, IDisposable
  {
    private static readonly Duration AUTOSAVE_MIN_INTERVAL;
    public static readonly ImmutableArray<int> AUTOSAVE_OPTIONS_MINUTES;
    public static readonly int AUTOSAVE_OPTIONS_DEFAULT_INDEX;
    public static readonly int AUTOSAVE_DEFAULT_INTERVAL_MINUTES;
    private readonly DependencyResolver m_resolver;
    private readonly ISaveConfig m_config;
    private readonly GameNameConfig m_gameNameConfig;
    private readonly IFileSystemHelper m_fileSystemHelper;
    private readonly IInputScheduler m_inputScheduler;
    private readonly IGameSaveInfoProvider m_gameSaveInfoProvider;
    private readonly SimLoopEvents m_simLoopEvents;
    private readonly GameSaver m_saver;
    private Option<string> m_pendingSavePath;
    private bool m_pendingSaveIsAutosave;
    private long m_lastSaveTime;
    private Option<Action<SaveResult>> m_notifyOnSaveDone;
    private readonly ASyncSaver m_asyncSaver;
    private Duration? m_customAutoSaveInterval;
    private bool m_saveEventDispatched;
    private int m_lastSavedCommandsCount;
    private bool m_wasLastSaveAutoSave;

    public event Action OnSaveStart;

    public event Action OnSaveDone;

    public event Action<LocStrFormatted> OnAutoSaveFail;

    public string GameName => this.m_gameNameConfig.GameName;

    public bool IsSavePending => this.m_pendingSavePath.HasValue;

    public SaveManager(
      DependencyResolver resolver,
      SimLoopEvents simLoopEvents,
      ISaveConfig config,
      GameNameConfig gameNameConfig,
      IFileSystemHelper fileSystemHelper,
      IInputScheduler inputScheduler,
      AllImplementationsOf<IMod> mods,
      AllImplementationsOf<IConfig> configs,
      SpecialSerializerFactories specialSerializers,
      IGameSaveInfoProvider gameSaveInfoProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lastSaveTime = (long) Environment.TickCount;
      this.m_asyncSaver = new ASyncSaver();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_config = config;
      this.m_gameNameConfig = gameNameConfig;
      this.m_resolver = resolver;
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_inputScheduler = inputScheduler;
      this.m_gameSaveInfoProvider = gameSaveInfoProvider;
      this.m_simLoopEvents = simLoopEvents;
      this.m_lastSavedCommandsCount = this.m_inputScheduler.ProcessedCommandsInThisSessionAffectingSave;
      this.m_saver = new GameSaver(mods.Implementations, configs.Implementations.Filter((Predicate<IConfig>) (x => SerializerGenerator.TryGetSerializeMethod(x.GetType(), out MethodInfo _))), specialSerializers.SpecialSerializersForConfigs, specialSerializers.SpecialSerializersForGame, gameSaveInfoProvider);
      simLoopEvents.Update.AddNonSaveable<SaveManager>(this, new Action(this.simUpdate));
      simLoopEvents.Sync.AddNonSaveable<SaveManager>(this, new Action(this.sync));
    }

    [ConsoleCommand(false, false, null, null)]
    private string setAutoSaveInterval(int minutes)
    {
      if (minutes <= 0)
      {
        this.m_customAutoSaveInterval = new Duration?(Duration.Zero);
        return "Auto-save is disabled.";
      }
      this.m_customAutoSaveInterval = new Duration?(minutes.Minutes().Max(SaveManager.AUTOSAVE_MIN_INTERVAL));
      return string.Format("Auto-save interval is set to {0} minutes.", (object) this.m_customAutoSaveInterval.Value.Minutes);
    }

    public void SetCustomAutoSaveInterval(Duration duration)
    {
      this.m_customAutoSaveInterval = new Duration?(duration);
    }

    public void RequestGameSave(string saveName, Action<SaveResult> notifyOnSaveDone = null)
    {
      this.m_notifyOnSaveDone = (Option<Action<SaveResult>>) notifyOnSaveDone;
      this.requestGameSave(saveName, false);
    }

    private void requestGameSave(string saveName, bool isAutosave)
    {
      if (this.m_asyncSaver.Started)
        Log.Warning("Save already in progress!");
      this.m_pendingSavePath = (Option<string>) this.m_fileSystemHelper.GetSaveFilePath(saveName, this.m_gameNameConfig.GameName);
      this.m_pendingSaveIsAutosave = isAutosave;
      this.m_lastSaveTime = (long) Environment.TickCount;
      this.m_lastSavedCommandsCount = this.m_inputScheduler.ProcessedCommandsInThisSessionAffectingSave;
      this.m_wasLastSaveAutoSave = isAutosave;
    }

    public bool CanQuitGameWithoutSave()
    {
      long num = ((long) Environment.TickCount - this.m_lastSaveTime).Abs() / 1000L;
      return !this.m_wasLastSaveAutoSave && !this.hasCommandsCountChanged() && num < (long) 2.Minutes().SecondsFloored;
    }

    public bool IsNonAutosaveInProgress()
    {
      return this.m_pendingSavePath.HasValue && !this.m_wasLastSaveAutoSave;
    }

    /// <summary>
    /// Saves game that owns this class into given stream. This is fully synchronous operation and blocks. Use <see cref="M:Mafi.Core.SaveGame.SaveManager.RequestGameSave(System.String,System.Action{Mafi.Core.SaveGame.SaveResult})" /> for efficient asynchronous saving if blocking is an issue.
    /// </summary>
    public void SaveGame(Stream stream, SaveCompressionType compressionType, bool fromTest = false)
    {
      this.m_simLoopEvents.InvokeBeforeSave(fromTest);
      Action onSaveStart = this.OnSaveStart;
      if (onSaveStart != null)
        onSaveStart();
      this.m_saver.StartSave(this.m_resolver, compressionType);
      this.m_saver.FinishSaveWriteToStream(stream);
      this.m_saver.Reset();
      Action onSaveDone = this.OnSaveDone;
      if (onSaveDone == null)
        return;
      onSaveDone();
    }

    /// <summary>
    /// Saves only the raw data of the game without any headers or types map to given stream.
    /// </summary>
    public void SaveGameRaw(Stream stream)
    {
      BlobWriter blobWriter = new BlobWriter(stream);
      blobWriter.WriteGeneric<DependencyResolver>(this.m_resolver);
      blobWriter.Dispose();
    }

    private void deleteAutoSavesOverLimit()
    {
      string gameName = this.GameName.IsNullOrEmpty() ? (string) null : this.GameName;
      foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<FileInfo>) this.m_fileSystemHelper.GetAllFiles(FileType.GameSave, "*-autosave", gameName)).OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (x => x.LastWriteTime)).Skip<FileInfo>(6))
        fileSystemInfo.Delete();
      foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<FileInfo>) this.m_fileSystemHelper.GetAllFiles(FileType.GameSave, "*-autosave-paused", gameName)).OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (x => x.LastWriteTime)).Skip<FileInfo>(6))
        fileSystemInfo.Delete();
    }

    private void sync()
    {
      if (this.m_asyncSaver.Started)
      {
        if (!this.m_asyncSaver.Finished)
          return;
        this.m_asyncSaver.Reset();
        bool flag = false;
        Option<Exception> other = (Option<Exception>) Option.None;
        if (this.m_pendingSaveIsAutosave)
        {
          try
          {
            this.deleteAutoSavesOverLimit();
          }
          catch (Exception ex)
          {
            flag = true;
            Log.Exception(ex, "Failed to delete older save files");
            other = (Option<Exception>) ex;
          }
        }
        try
        {
          Log.Info("Game saved to '" + this.m_pendingSavePath.Value + "' (size: " + ((float) ((double) new FileInfo(this.m_pendingSavePath.Value).Length / 1024.0 / 1024.0)).RoundToSigDigits(2, false, false, false) + " MB).");
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception when checking file size.");
          other = (Option<Exception>) ex;
        }
        this.m_pendingSavePath = (Option<string>) Option.None;
        if (this.m_notifyOnSaveDone.HasValue)
        {
          this.m_notifyOnSaveDone.Value(new SaveResult()
          {
            Error = this.m_asyncSaver.LastSaveError,
            FilePath = this.m_asyncSaver.LastSavePath,
            Exception = this.m_asyncSaver.LastSaveException.Or(other)
          });
          this.m_notifyOnSaveDone = (Option<Action<SaveResult>>) Option.None;
        }
        if (this.m_wasLastSaveAutoSave && this.m_asyncSaver.LastSaveError.HasValue)
        {
          Action<LocStrFormatted> onAutoSaveFail = this.OnAutoSaveFail;
          if (onAutoSaveFail == null)
            return;
          onAutoSaveFail(this.m_asyncSaver.LastSaveError.Value);
        }
        else
        {
          if (!flag)
            return;
          Action<LocStrFormatted> onAutoSaveFail = this.OnAutoSaveFail;
          if (onAutoSaveFail == null)
            return;
          onAutoSaveFail(new LocStrFormatted("The game was saved but there was a failure when attempting to delete an old auto-save file."));
        }
      }
      else if (this.m_pendingSavePath.HasValue)
      {
        if (!this.m_saveEventDispatched)
        {
          this.m_simLoopEvents.InvokeBeforeSave();
          Action onSaveStart = this.OnSaveStart;
          if (onSaveStart != null)
            onSaveStart();
          this.m_gameSaveInfoProvider.ScheduleScreenshotRendering();
          this.m_saveEventDispatched = true;
        }
        else
        {
          this.m_saveEventDispatched = false;
          this.startGameSave();
          Action onSaveDone = this.OnSaveDone;
          if (onSaveDone != null)
            onSaveDone();
          this.m_gameSaveInfoProvider.NotifySaveDone();
        }
      }
      else
      {
        if (!this.m_simLoopEvents.IsSimPaused || !this.isTimeForAutoSave() || !this.hasCommandsCountChanged())
          return;
        this.requestGameSave(this.m_fileSystemHelper.GetTimestampedFileName("-autosave-paused", FileType.Misc, true), true);
      }
    }

    private bool hasCommandsCountChanged()
    {
      return this.m_inputScheduler.ProcessedCommandsInThisSessionAffectingSave != this.m_lastSavedCommandsCount;
    }

    private void simUpdate()
    {
      if (!this.isTimeForAutoSave())
        return;
      this.requestGameSave(this.m_fileSystemHelper.GetTimestampedFileName("-autosave", FileType.Misc, true), true);
    }

    private bool isTimeForAutoSave()
    {
      Duration? nullable = this.m_customAutoSaveInterval ?? this.m_config.AutoSaveInterval;
      if (!nullable.HasValue || nullable.Value.IsNotPositive)
        return false;
      long num = ((long) Environment.TickCount - this.m_lastSaveTime).Abs() / 1000L;
      Duration duration = nullable.Value;
      duration = duration.Max(SaveManager.AUTOSAVE_MIN_INTERVAL);
      long secondsFloored = (long) duration.SecondsFloored;
      return num > secondsFloored && this.m_pendingSavePath.IsNone;
    }

    private void startGameSave()
    {
      if (this.m_pendingSavePath.IsNone)
      {
        Log.Error("No pending save path");
      }
      else
      {
        try
        {
          this.m_saver.StartSave(this.m_resolver, this.m_config.SaveCompressionType);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Failed to save game.");
          if (this.m_notifyOnSaveDone.HasValue)
          {
            this.m_notifyOnSaveDone.Value(new SaveResult()
            {
              Error = new LocStrFormatted?(TrCore.GameSaveLoad__CannotSaveFile_Crash.Format(this.m_pendingSavePath.Value, ex.Message)),
              FilePath = Option<string>.None,
              Exception = (Option<Exception>) ex
            });
            this.m_notifyOnSaveDone = (Option<Action<SaveResult>>) Option.None;
          }
          this.m_pendingSavePath = Option<string>.None;
          return;
        }
        this.m_asyncSaver.RunAsync((IAsyncSavable) this.m_saver, this.m_pendingSavePath.Value);
      }
    }

    public void Dispose()
    {
      for (int index = 0; index < 200 && this.m_asyncSaver.Started && !this.m_asyncSaver.Finished; ++index)
        Thread.Sleep(10);
      if (!this.m_asyncSaver.Started || this.m_asyncSaver.Finished)
        return;
      Log.Error("Async saver did not finish in time during application exit.");
      this.m_asyncSaver.Reset();
    }

    static SaveManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SaveManager.AUTOSAVE_MIN_INTERVAL = 5.Minutes();
      SaveManager.AUTOSAVE_OPTIONS_MINUTES = ImmutableArray.Create<int>(0, 10, 15, 20, 30, 40, 60);
      SaveManager.AUTOSAVE_OPTIONS_DEFAULT_INDEX = 2;
      SaveManager.AUTOSAVE_DEFAULT_INTERVAL_MINUTES = SaveManager.AUTOSAVE_OPTIONS_MINUTES[SaveManager.AUTOSAVE_OPTIONS_DEFAULT_INDEX];
    }
  }
}
