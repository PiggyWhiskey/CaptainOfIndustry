// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.IDynamicSizeElement
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  /// <summary>
  /// Any ui element that has a dynamic size can implement this interface. When such element is registered into a
  /// container that supports this interface, the container will listen to the changes and resizes itself accordingly.
  /// </summary>
  [NotGlobalDependency]
  public interface IDynamicSizeElement : IUiElement
  {
    event Action<IUiElement> SizeChanged;
  }
}
