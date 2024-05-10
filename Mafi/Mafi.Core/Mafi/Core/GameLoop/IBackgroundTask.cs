// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameLoop.IBackgroundTask
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.GameLoop
{
  /// <summary>
  /// This class represents a task that will be ran on a separate thread. All the threading and synchronization is
  /// handled by the <see cref="T:Mafi.Core.GameLoop.BackgroundTaskRunner" />.
  /// WARNING: Correct implementation of this class is not trivial!
  /// </summary>
  public interface IBackgroundTask
  {
    /// <summary>
    /// Called by the <see cref="M:Mafi.Core.GameLoop.BackgroundTaskRunner.Sync" /> when the work of this task was finished and it is in
    /// sync with its parent thread.
    /// </summary>
    void PerformSync();

    /// <summary>
    /// This method performs the main work. Note that no threads should be created here as this method is already
    /// invoked on its own thread!
    /// </summary>
    void PerformWork();

    /// <summary>Called when the parent loop is bing terminated.</summary>
    void Terminated();
  }
}
