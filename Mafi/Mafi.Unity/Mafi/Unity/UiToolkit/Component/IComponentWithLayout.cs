// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.IComponentWithLayout
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public interface IComponentWithLayout : IComponentWithClass
  {
    ILayoutDecorator GetLayoutDecorator();

    void SetSize(StyleLength? width, StyleLength? height);
  }
}
