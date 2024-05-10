// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.IFlexDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public interface IFlexDecorator
  {
    void Direction(LayoutDirection direction);

    void AlignItems(Align align);

    void AlignGridContent(Align alignContent);

    void JustifyItems(Justify justify);

    void Wrap(bool wrap = true);

    void SetGap<T>(T component, Px? mainAxis, Px? crossAxis) where T : IFlexComponent, UiComponent;

    void SetReversedDirection(IFlexComponent component);

    void SetOverflow(Overflow overflow);
  }
}
