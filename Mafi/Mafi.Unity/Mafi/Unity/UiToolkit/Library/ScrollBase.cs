// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ScrollBase
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class ScrollBase : 
    UiComponent<ScrollView>,
    IFlexComponent,
    IUiComponent,
    IComponentWithPadding
  {
    protected VisualElement m_contentContainer;
    private Option<MethodInfo> m_internalOnScroll;

    Option<IGapHandler> IFlexComponent.GapHandler { get; set; }

    protected ScrollBase(ScrollViewMode mode)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new ScrollView(mode));
      ScrollBase.ApplyThumbStyles(this.Element);
      this.m_contentContainer = this.Element.Q("unity-content-container", (string) null);
    }

    public IFlexDecorator GetFlexDecorator()
    {
      return (IFlexDecorator) FlexDecorator.GetSharedInstance(this.m_contentContainer);
    }

    public bool GetIsRow() => this.Element.mode == ScrollViewMode.Horizontal;

    public ScrollBase ScrollTo(UiComponent child)
    {
      this.Element.ScrollTo(child.RootElement);
      return this;
    }

    public ScrollBase ScrollerAuto()
    {
      if (this.GetIsRow())
        this.Element.horizontalScrollerVisibility = ScrollerVisibility.Auto;
      else
        this.Element.verticalScrollerVisibility = ScrollerVisibility.Auto;
      return this;
    }

    public ScrollBase ScrollerAlwaysVisible()
    {
      if (this.GetIsRow())
        this.Element.horizontalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
      else
        this.Element.verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
      return this;
    }

    /// <summary>
    /// Increases the size of the content area by the size of the scrollbar to pretend
    /// it's not there.
    /// </summary>
    public ScrollBase PreventResizeForScroller()
    {
      if (this.GetIsRow())
        this.m_contentContainer.style.marginBottom = (StyleLength) -17f;
      else
        this.m_contentContainer.style.marginRight = (StyleLength) -17f;
      return this;
    }

    protected override void setIgnoreInputPicking(bool isIgnored, bool recursive)
    {
      base.setIgnoreInputPicking(isIgnored, recursive);
      if (!(isIgnored & recursive))
        return;
      this.Schedule.Execute((Action) (() => UiComponent.setPickingModeRecursively(this.Element.Q((string) null, "unity-slider"), PickingMode.Position)));
    }

    /// <summary>
    /// HACK: Forwards the given scroll event to the underlying ScrollView's implementation for consistent behavior.
    /// 
    /// This allows the scroll event to be captured on another element (such as background container) but be
    /// handled by this scroll container. This is useful for scroll containers with input picking ignored.
    /// </summary>
    public void HandleScroll(WheelEvent evt)
    {
      if (this.m_internalOnScroll.IsNone)
        this.m_internalOnScroll = (Option<MethodInfo>) this.Element.GetType().GetMethod("OnScrollWheel", BindingFlags.Instance | BindingFlags.NonPublic);
      this.m_internalOnScroll.Value.Invoke((object) this.Element, (object[]) new WheelEvent[1]
      {
        evt
      });
    }

    public static void ApplyThumbStyles(ScrollView scrollView)
    {
      scrollView.mouseWheelScrollSize = 1000f;
      VisualElement visualElement = scrollView.Q((string) null, "unity-base-slider__dragger");
      if (visualElement == null || scrollView.Q((string) null, "background") != null)
        return;
      VisualElement child1 = new VisualElement();
      child1.AddToClassList("background");
      visualElement.Add(child1);
      VisualElement child2 = new VisualElement();
      child2.AddToClassList("grab-indicator");
      visualElement.Add(child2);
    }

    IPaddingDecorator IComponentWithPadding.GetPaddingDecorator()
    {
      return (IPaddingDecorator) PaddingDecorator.GetSharedInstance(this.m_contentContainer);
    }
  }
}
