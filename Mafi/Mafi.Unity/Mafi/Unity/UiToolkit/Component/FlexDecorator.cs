// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.FlexDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class FlexDecorator : IFlexDecorator
  {
    private static readonly FlexDecorator s_instance;
    private VisualElement m_element;

    public static FlexDecorator GetSharedInstance(VisualElement element)
    {
      FlexDecorator.s_instance.m_element = element;
      return FlexDecorator.s_instance;
    }

    public void Direction(LayoutDirection direction)
    {
      this.m_element.style.display = (StyleEnum<DisplayStyle>) DisplayStyle.Flex;
      this.m_element.style.flexDirection = (StyleEnum<FlexDirection>) direction.ToUnity();
    }

    public void AlignItems(Align align)
    {
      this.m_element.style.alignItems = (StyleEnum<UnityEngine.UIElements.Align>) align.ToUnity();
    }

    public void AlignGridContent(Align alignContent)
    {
      this.m_element.style.alignContent = (StyleEnum<UnityEngine.UIElements.Align>) alignContent.ToUnity();
    }

    public void JustifyItems(Justify justify)
    {
      this.m_element.style.justifyContent = (StyleEnum<UnityEngine.UIElements.Justify>) justify.ToUnity();
    }

    public void Wrap(bool wrap = true)
    {
      this.m_element.style.flexWrap = (StyleEnum<UnityEngine.UIElements.Wrap>) (wrap ? UnityEngine.UIElements.Wrap.Wrap : UnityEngine.UIElements.Wrap.NoWrap);
    }

    public void SetGap<T>(T component, Px? mainAxis, Px? crossAxis) where T : IFlexComponent, UiComponent
    {
      IGapHandler gapHandler = component.GapHandler.ValueOrNull;
      if (gapHandler == null)
      {
        gapHandler = !mainAxis.HasValue || !crossAxis.HasValue ? (IGapHandler) new SimpleGapHandler((UiComponent) component, this.m_element.resolvedStyle.flexDirection) : (IGapHandler) new GridGapHandler((UiComponent) component);
        component.GapHandler = gapHandler.SomeOption<IGapHandler>();
      }
      if (mainAxis.HasValue)
      {
        if (component.GetIsRow())
          gapHandler.SetRowGap(mainAxis.Value);
        else
          gapHandler.SetColumnGap(mainAxis.Value);
      }
      if (!crossAxis.HasValue)
        return;
      if (component.GetIsRow())
        gapHandler.SetColumnGap(crossAxis.Value);
      else
        gapHandler.SetRowGap(crossAxis.Value);
    }

    public void SetReversedDirection(IFlexComponent component)
    {
      FlexDirection direction = !component.GetIsRow() ? FlexDirection.ColumnReverse : FlexDirection.RowReverse;
      this.m_element.style.flexDirection = (StyleEnum<FlexDirection>) direction;
      component.GapHandler.ValueOrNull?.OnDirectionChanged(direction);
    }

    public void SetOverflow(Overflow overflow)
    {
      this.m_element.style.overflow = (StyleEnum<Overflow>) overflow;
    }

    public FlexDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FlexDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FlexDecorator.s_instance = new FlexDecorator();
    }
  }
}
