// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.IUnityInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public interface IUnityInputController
  {
    ControllerConfig Config { get; }

    /// <summary>
    /// Called when this input controller is activated by the player. Invoked on the main thread.
    /// </summary>
    void Activate();

    /// <summary>
    /// Called when this input controller is deactivated by the player. Invoked on the main thread.
    /// </summary>
    void Deactivate();

    /// <summary>
    /// Called every frame when the controller is active. Invoked on the main thread. Returns whether input was
    /// processed and no other controllers should be updated.
    /// </summary>
    bool InputUpdate(IInputScheduler inputScheduler);
  }
}
