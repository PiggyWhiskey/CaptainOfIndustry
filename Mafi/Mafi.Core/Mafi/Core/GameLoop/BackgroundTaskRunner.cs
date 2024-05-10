// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameLoop.BackgroundTaskRunner
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Mafi.Core.GameLoop
{
  /// <summary>
  /// This class takes <see cref="T:Mafi.Core.GameLoop.IBackgroundTask" /> and allows running it in a background thread. This class handles
  /// all the synchronization.
  /// </summary>
  public class BackgroundTaskRunner
  {
    private readonly IBackgroundTask m_taskCore;
    private bool m_taskRunningInBgThread;
    private volatile bool m_terminate;
    private readonly Thread m_thread;
    private readonly Action m_newThreadStaticSetup;
    private readonly AutoResetEvent m_startAutoResetEvent;
    private readonly ManualResetEvent m_doneManualResetEvent;
    private readonly Stopwatch m_workStopwatch;
    private readonly Stopwatch m_waitingStopwatch;

    /// <summary>Whether the background thread is currently working.</summary>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Whether last <see cref="M:Mafi.Core.GameLoop.BackgroundTaskRunner.WaitForFinishWork" /> call waited for the work thread.
    /// </summary>
    public bool WasOverTime { get; private set; }

    /// <summary>Duration of the wait for the task.</summary>
    public TimeSpan LastOvertimeDuration { get; private set; }

    /// <summary>Duration of the last work cycle.</summary>
    public TimeSpan LastWorkDuration { get; private set; }

    public BackgroundTaskRunner(string name, IBackgroundTask taskCore, Action newThreadStaticSetup)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_startAutoResetEvent = new AutoResetEvent(false);
      this.m_doneManualResetEvent = new ManualResetEvent(true);
      this.m_workStopwatch = new Stopwatch();
      this.m_waitingStopwatch = new Stopwatch();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_newThreadStaticSetup = newThreadStaticSetup;
      this.m_taskCore = taskCore.CheckNotNull<IBackgroundTask>();
      this.m_thread = new Thread(new ThreadStart(this.threadMain))
      {
        Name = name.CheckNotNull<string>()
      };
      this.m_thread.Start();
    }

    /// <summary>
    /// Performs the task either in sync or in background thread.
    /// </summary>
    public void PerformWork(bool inBackgroundThread)
    {
      this.m_taskRunningInBgThread = inBackgroundThread;
      if (this.m_taskRunningInBgThread)
      {
        this.m_doneManualResetEvent.Reset();
        this.m_startAutoResetEvent.Set();
      }
      else
      {
        this.m_workStopwatch.Restart();
        this.m_taskCore.PerformWork();
        this.m_workStopwatch.Stop();
      }
    }

    public void Sync() => this.m_taskCore.PerformSync();

    /// <summary>
    /// Blocks current thread until background thread is finished. Does nothing if the work was performed in sync.
    /// Returns whether the work was overtime (calling thread had to wait for the thread);
    /// </summary>
    public void WaitForFinishWork()
    {
      if (!this.m_taskRunningInBgThread)
      {
        this.WasOverTime = false;
        this.LastOvertimeDuration = TimeSpan.Zero;
        this.LastWorkDuration = this.m_workStopwatch.Elapsed;
      }
      else
      {
        this.WasOverTime = this.IsRunning;
        this.m_waitingStopwatch.Restart();
        this.m_doneManualResetEvent.WaitOne();
        this.m_waitingStopwatch.Stop();
        Mafi.Assert.That<bool>(this.IsRunning).IsFalse();
        this.LastOvertimeDuration = this.m_waitingStopwatch.Elapsed;
        this.LastWorkDuration = this.m_workStopwatch.Elapsed;
      }
    }

    /// <summary>
    /// Terminates the worker thread and blocks until the thread is done.
    /// </summary>
    public void Terminate()
    {
      this.m_taskCore.Terminated();
      this.m_terminate = true;
      this.m_startAutoResetEvent.Set();
      this.m_thread.Join(1000);
      if (!this.m_thread.IsAlive)
        return;
      Mafi.Log.Error("Sim thread failed to terminate in 1s, aborting.");
      this.m_thread.Abort();
    }

    /// <summary>Main method of the background thread.</summary>
    private void threadMain()
    {
      Action threadStaticSetup = this.m_newThreadStaticSetup;
      if (threadStaticSetup != null)
        threadStaticSetup();
      while (true)
      {
        try
        {
          this.m_startAutoResetEvent.WaitOne();
          if (this.m_terminate)
            break;
          this.IsRunning = true;
          this.m_workStopwatch.Restart();
          this.m_taskCore.PerformWork();
        }
        catch (ThreadAbortException ex)
        {
          break;
        }
        catch (Exception ex)
        {
          Mafi.Log.Exception(ex, "Exception on the '" + ThreadUtils.ThreadNameFast + "' thread! ");
        }
        finally
        {
          this.m_workStopwatch.Stop();
          this.IsRunning = false;
          this.m_doneManualResetEvent.Set();
        }
      }
    }
  }
}
