// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.IToolbarItemController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar
{
  public interface IToolbarItemController : IUnityInputController, IToolbarItemRegistrar
  {
    /// <summary>
    /// Invoked when <see cref="P:Mafi.Unity.InputControl.Toolbar.IToolbarItemController.IsVisible" /> changes. The event can be invoked from any thread.
    /// </summary>
    event Action<IToolbarItemController> VisibilityChanged;

    /// <summary>
    /// Whether the controller can be activated and should be displayed in the menu.
    /// </summary>
    bool IsVisible { get; }

    bool DeactivateShortcutsIfNotVisible { get; }
  }
}
