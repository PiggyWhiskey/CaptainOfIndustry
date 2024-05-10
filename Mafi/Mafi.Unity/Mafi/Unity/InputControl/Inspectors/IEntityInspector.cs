// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.IEntityInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  /// <summary>
  /// Non-generic common interface for all entity inspectors.
  /// </summary>
  public interface IEntityInspector
  {
    InspectorContext Context { get; }

    bool DeactivateOnNonUiClick { get; }

    /// <summary>
    /// Called when this input controller is activated by the player. Invoked on the main thread.
    /// </summary>
    void Activate();

    /// <summary>
    /// Called when this input controller is deactivated by the player. Invoked on the main thread.
    /// </summary>
    void Deactivate();

    /// <summary>Called for an active controller.</summary>
    void SyncUpdate(GameTime gameTime);

    /// <summary>Called for an active controller.</summary>
    void RenderUpdate(GameTime gameTime);

    /// <summary>Input update for this inspector.</summary>
    bool InputUpdate(IInputScheduler inputScheduler);
  }
}
