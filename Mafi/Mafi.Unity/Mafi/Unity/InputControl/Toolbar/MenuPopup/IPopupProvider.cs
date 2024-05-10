// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.IPopupProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup
{
  /// <summary>
  /// Populates the popup view with item's data. This allows to extend popups with new item types.
  /// </summary>
  [MultiDependency]
  public interface IPopupProvider
  {
    /// <summary>Type of item supported by this provider.</summary>
    Type SupportedType { get; }

    /// <summary>
    /// Populates the given popup view with the given item. The item's type must be assignable to <see cref="P:Mafi.Unity.InputControl.Toolbar.MenuPopup.IPopupProvider.SupportedType" />.
    /// </summary>
    void PopulateView(MenuPopupView view, object item, bool isForResearch);
  }
}
