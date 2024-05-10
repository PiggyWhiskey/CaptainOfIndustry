// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameLoop.INeedsSimUpdatesForInit
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.GameLoop
{
  /// <summary>
  /// Marks class that need some updates after new game is created to so in-game initialization. The class has to
  /// register itself using <see cref="M:Mafi.Core.GameLoop.GameLoopEvents.RegisterInitSimUpdate(Mafi.Core.GameLoop.INeedsSimUpdatesForInit)" />.
  /// </summary>
  [NotGlobalDependency]
  public interface INeedsSimUpdatesForInit
  {
    /// <summary>
    /// Whether derived class needs more sim updates in order to finish its initialization.
    /// </summary>
    bool NeedsMoreSimUpdates { get; }

    /// <summary>
    /// Whether we've failed initialization and should kick back to main menu.
    /// </summary>
    bool FailedInit { get; }

    /// <summary>Message to describe how we failed to initialize.</summary>
    string FailedInitMessage { get; }
  }
}
