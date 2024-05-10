// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.ScrollableStackContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  /// <summary>
  /// Automatically scrolls its content if it gets over maxHeight
  /// </summary>
  public class ScrollableStackContainer : IUiElement, IDynamicSizeElement
  {
    public StackContainer ItemsContainer;
    public bool IsScrollingNeeded;
    private readonly ScrollableContainer m_scrollableContainer;

    public GameObject GameObject => this.m_scrollableContainer.GameObject;

    public RectTransform RectTransform => this.m_scrollableContainer.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public ScrollableStackContainer(
      UiBuilder builder,
      float maxHeight,
      StackContainer existingContainer = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ScrollableStackContainer scrollableStackContainer = this;
      this.m_scrollableContainer = builder.NewScrollableContainer("ScrollableContainer").AddVerticalScrollbar();
      this.ItemsContainer = existingContainer == null ? builder.NewStackContainer("StackContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(2f) : existingContainer.SetSizeMode(StackContainer.SizeMode.Dynamic);
      this.m_scrollableContainer.AddItemTop((IUiElement) this.ItemsContainer);
      this.ItemsContainer.SizeChanged += (Action<IUiElement>) (_ =>
      {
        float dynamicHeight = scrollableStackContainer.ItemsContainer.GetDynamicHeight();
        scrollableStackContainer.m_scrollableContainer.SetHeight<ScrollableContainer>(dynamicHeight.Min(maxHeight));
        scrollableStackContainer.IsScrollingNeeded = (double) dynamicHeight > (double) maxHeight;
        Action<IUiElement> sizeChanged = scrollableStackContainer.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) scrollableStackContainer);
      });
    }
  }
}
