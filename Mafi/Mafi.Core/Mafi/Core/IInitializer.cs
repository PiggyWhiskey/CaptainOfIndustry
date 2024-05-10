// Decompiled with JetBrains decompiler
// Type: Mafi.Core.IInitializer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core
{
  /// <summary>
  /// Allow to run conditional code based on whether game is being loaded or not. Usage of this reduces the amount of
  /// constructor overloads that were typically needed.
  /// </summary>
  public interface IInitializer
  {
    /// <summary>
    /// Whether the caller is being instantiated during loading process.
    /// </summary>
    bool IsBeingLoaded { get; }

    /// <summary>
    /// The given action gets invoked only when new game is created (not from save file).
    /// NOTE: This should be used only in constructors of root classes.
    /// </summary>
    void DoOnNewGameOnly(Action action);

    /// <summary>
    /// The given action gets invoked only (and immediately) when new game is created (not from save file) or after
    /// the game was fully loaded.
    /// NOTE: This should be used only in constructors of root classes.
    /// </summary>
    void DoOnNewGameOrAfterLoad(Action action);
  }
}
