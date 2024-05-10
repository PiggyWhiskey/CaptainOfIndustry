// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.ASyncSaver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;
using System;
using System.ComponentModel;
using System.IO;

#nullable disable
namespace Mafi.Core.SaveGame
{
  /// <summary>Helper for running async part of the save flow.</summary>
  public class ASyncSaver
  {
    public Option<string> LastSavePath;
    public LocStrFormatted? LastSaveError;
    public Option<Exception> LastSaveException;
    private IAsyncSavable m_saver;
    private string m_filePath;
    private readonly BackgroundWorker m_backgroundWorker;

    public bool Started { get; private set; }

    public bool Finished { get; private set; }

    public event Action OnSaveFinished;

    public ASyncSaver()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_backgroundWorker = new BackgroundWorker();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_backgroundWorker.DoWork += new DoWorkEventHandler(this.doWork);
      this.m_backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
      {
        this.Finished = true;
        Action onSaveFinished = this.OnSaveFinished;
        if (onSaveFinished == null)
          return;
        onSaveFinished();
      });
    }

    /// <summary>
    /// This will run <see cref="M:Mafi.Core.SaveGame.IAsyncSavable.FinishSaveWriteToStream(System.IO.Stream)" /> in a background thread and then it will
    /// write the final stream to the local file identified by the given path.
    /// </summary>
    public void RunAsync(IAsyncSavable saver, string filePath)
    {
      Assert.That<bool>(this.Started).IsFalse();
      Assert.That<bool>(this.m_backgroundWorker.IsBusy).IsFalse();
      this.Reset();
      this.m_saver = saver;
      this.m_filePath = filePath;
      this.LastSaveError = new LocStrFormatted?();
      this.LastSaveException = (Option<Exception>) Option.None;
      this.LastSavePath = (Option<string>) this.m_filePath;
      this.Finished = false;
      this.Started = true;
      this.m_backgroundWorker.RunWorkerAsync();
    }

    public void RunInSyncAndReset(IAsyncSavable saver, string filePath)
    {
      Assert.That<bool>(this.Started).IsFalse();
      Assert.That<bool>(this.m_backgroundWorker.IsBusy).IsFalse();
      this.Reset();
      this.m_saver = saver;
      this.m_filePath = filePath;
      this.LastSaveError = new LocStrFormatted?();
      this.LastSaveException = (Option<Exception>) Option.None;
      this.LastSavePath = (Option<string>) this.m_filePath;
      this.Finished = false;
      this.Started = true;
      this.doWork((object) null, (DoWorkEventArgs) null);
      this.Finished = true;
      Action onSaveFinished = this.OnSaveFinished;
      if (onSaveFinished != null)
        onSaveFinished();
      this.Reset();
    }

    public void Reset()
    {
      if (this.m_backgroundWorker.IsBusy)
        this.m_backgroundWorker.CancelAsync();
      this.m_saver = (IAsyncSavable) null;
      this.m_filePath = (string) null;
      this.Started = false;
      this.Finished = false;
    }

    private void doWork(object _, DoWorkEventArgs __)
    {
      string str1 = (string) null;
      string filePath = this.m_filePath;
      IAsyncSavable saver = this.m_saver;
      try
      {
        str1 = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(str1))
        {
          Log.Warning("Directory for save file does not exist: " + filePath);
          Directory.CreateDirectory(str1);
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to ensure save dir '" + str1 + "'.");
        this.LastSaveError = new LocStrFormatted?(TrCore.GameSaveLoad__CannotSaveFile.Format(filePath, ex.Message));
        this.LastSaveException = (Option<Exception>) ex;
        saver.Reset();
        return;
      }
      bool flag = false;
      string str2 = (string) null;
      SaveWriteStatus saveWriteStatus = SaveWriteStatus.Unknown;
      try
      {
        str2 = Path.Combine(str1, Path.GetFileNameWithoutExtension(filePath) + ".tmp" + Path.GetExtension(filePath));
        if (File.Exists(str2))
        {
          Log.Warning("Save tmp file already exists, deleting.");
          try
          {
            deleteFile(str2);
            saveWriteStatus = SaveWriteStatus.OldTmpDeleted;
          }
          catch (Exception ex)
          {
            Log.Warning("Failed to delete tmp save file, continuing.");
            saveWriteStatus = SaveWriteStatus.OldTmpFailToDelete;
          }
        }
        else
          saveWriteStatus = SaveWriteStatus.NoOldTmp;
        using (FileStream fileStream = openWrite(str2))
          saver.FinishSaveWriteToStream((Stream) fileStream);
        saveWriteStatus = SaveWriteStatus.TmpFileWritten;
        Option<Exception> exception;
        SaveChecksumValidationResults validationResults;
        using (FileStream fileStream = openRead(str2))
          validationResults = GameLoader.ValidateChecksumSafe((Stream) fileStream, out SaveHeader _, out exception);
        saveWriteStatus = SaveWriteStatus.ChecksumComputed;
        if (validationResults != SaveChecksumValidationResults.Success)
        {
          Log.Error("Failed to verify checksum of written save file: " + string.Format("{0} ({1}) {2}", (object) validationResults, (object) str2, (object) exception.ValueOrNull));
          this.LastSaveError = new LocStrFormatted?(TrCore.GameSaveLoad__ChecksumFail.Format(validationResults.ToString()));
        }
        else
        {
          saveWriteStatus = SaveWriteStatus.ChecksumVerified;
          flag = true;
          if (File.Exists(filePath))
          {
            deleteFile(filePath);
            saveWriteStatus = SaveWriteStatus.OldSaveDeleted;
          }
          else
            saveWriteStatus = SaveWriteStatus.NoOldSave;
          moveFile(str2, filePath);
        }
      }
      catch (Exception ex)
      {
        if (flag)
        {
          Log.Exception(ex, string.Format("Exception during save overwrite or rename (last success: {0}): {1}", (object) saveWriteStatus, (object) filePath));
          this.LastSaveError = new LocStrFormatted?(TrCore.GameSaveLoad__SaveNotFinishedButSaved.Format(str2, ex.Message));
          this.LastSaveException = (Option<Exception>) ex;
        }
        else
        {
          Log.Exception(ex, string.Format("Failed to write save file (last success: {0}): {1}", (object) saveWriteStatus, (object) filePath));
          this.LastSaveError = new LocStrFormatted?(TrCore.GameSaveLoad__CannotSaveFile.Format(filePath, ex.Message));
          this.LastSaveException = (Option<Exception>) ex;
          try
          {
            if (!(ex is IOException) || str2 == null || !File.Exists(str2))
              return;
            deleteFile(str2);
            Log.Warning("Removed a partially written temp save file " + str2);
          }
          catch
          {
          }
        }
      }
      finally
      {
        saver.Reset();
      }

      static void deleteFile(string deleteFilePath)
      {
        try
        {
          File.Delete(deleteFilePath);
        }
        catch (IOException ex)
        {
          if (ex.HResult != 0)
            throw;
          else
            Log.Warning(string.Format("DeleteFile failed with result {0} and {1}", (object) ex.HResult, (object) ex.Message));
        }
      }

      static void moveFile(string source, string dest)
      {
        try
        {
          File.Move(source, dest);
        }
        catch (IOException ex)
        {
          if (ex.HResult != 0)
            throw;
          else
            Log.Warning(string.Format("MoveFile failed with result {0} and {1}", (object) ex.HResult, (object) ex.Message));
        }
      }

      static FileStream openWrite(string path)
      {
        FileStream fileStream = (FileStream) null;
        try
        {
          fileStream = File.OpenWrite(path);
        }
        catch (IOException ex)
        {
          if (ex.HResult != 0 || fileStream == null)
            throw;
          else
            Log.Warning(string.Format("OpenWrite failed with result {0} and {1}", (object) ex.HResult, (object) ex.Message));
        }
        return fileStream;
      }

      static FileStream openRead(string path)
      {
        FileStream fileStream = (FileStream) null;
        try
        {
          fileStream = File.OpenRead(path);
        }
        catch (IOException ex)
        {
          if (ex.HResult != 0 || fileStream == null)
            throw;
          else
            Log.Warning(string.Format("OpenRead failed with result {0} and {1}", (object) ex.HResult, (object) ex.Message));
        }
        return fileStream;
      }
    }
  }
}
