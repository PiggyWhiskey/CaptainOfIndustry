// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentDecorated`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Library;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class UiComponentDecorated<T> : UiComponent where T : VisualElement
  {
    /// <summary>
    /// If there is no shadow the ButtonElement == "outer element" and there is no overhead.
    /// </summary>
    protected readonly T InnerElement;
    protected Option<Outer> OuterDecor;
    protected readonly Option<Inner> InnerDecor;

    public UiComponentDecorated(T element, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(outer != null ? new VisualElement() : (VisualElement) element);
      this.InnerElement = outer != null ? element : (T) this.Element;
      this.OuterDecor = (Option<Outer>) outer;
      this.InnerDecor = (Option<Inner>) inner;
      if (outer != null)
      {
        this.Element.Add(outer.ElementToPlace);
        this.Element.Add((VisualElement) this.InnerElement);
        this.RelativePosition<UiComponentDecorated<T>>();
        this.WrapClass(outer.WrapClassName.ValueOrNull);
        this.Name<UiComponentDecorated<T>>("Wrapper");
      }
      if (!this.InnerDecor.HasValue)
        return;
      this.InnerElement.Add(this.InnerDecor.Value.ElementToPlace);
    }

    protected override void OnChildInserted(UiComponent child)
    {
      base.OnChildInserted(child);
      if (!this.InnerDecor.HasValue)
        return;
      this.InnerDecor.Value.BringToFront();
    }

    protected void SetOuterDecor(Option<Outer> outer)
    {
      if (this.OuterDecor.HasValue)
        this.WrapClass(this.OuterDecor.Value.WrapClassName.ValueOrNull, false);
      this.OuterDecor = outer;
      if (!this.OuterDecor.HasValue)
        return;
      this.WrapClass(this.OuterDecor.Value.WrapClassName.ValueOrNull);
    }

    public override void Clear()
    {
      base.Clear();
      if (!this.InnerDecor.HasValue)
        return;
      this.GetChildrenContainer().Add(this.InnerDecor.Value.ElementToPlace);
    }

    internal override VisualElement GetChildrenContainer() => (VisualElement) this.InnerElement;

    public override IClassDecorator GetClassDecorator()
    {
      return (IClassDecorator) ClassDecorator.GetSharedInstance((VisualElement) this.InnerElement);
    }

    public override IPaddingDecorator GetPaddingDecorator()
    {
      return (IPaddingDecorator) PaddingDecorator.GetSharedInstance((VisualElement) this.InnerElement);
    }

    public override IFontDecorator GetFontDecorator()
    {
      return (IFontDecorator) FontDecorator.GetSharedInstance((VisualElement) this.InnerElement);
    }

    public override IBorderDecorator GetBorderDecorator()
    {
      return (IBorderDecorator) BorderDecorator.GetSharedInstance((VisualElement) this.InnerElement);
    }

    public override IBackgroundDecorator GetBackgroundDecorator()
    {
      return (IBackgroundDecorator) BackgroundDecorator.GetSharedInstance((VisualElement) this.InnerElement);
    }

    protected override void SetColorInternal(ColorRgba? color)
    {
      IStyle style = this.InnerElement.style;
      Color? nullable = color.HasValue ? new Color?(color.GetValueOrDefault().ToColor()) : new Color?();
      StyleColor styleColor = nullable.HasValue ? (StyleColor) nullable.GetValueOrDefault() : new StyleColor(StyleKeyword.Null);
      style.color = styleColor;
    }

    public UiComponentDecorated<T> WrapClass(string cls, bool add = true)
    {
      if (cls != null)
        this.Element.EnableInClassList(cls, add);
      if (this.OuterDecor.HasValue)
      {
        this.InnerElement.style.flexGrow = (StyleFloat) 1f;
        this.InnerElement.style.flexShrink = (StyleFloat) 1f;
      }
      return this;
    }

    public override void SetFocusable(bool isFocusable)
    {
      this.InnerElement.focusable = isFocusable;
    }

    public override void Focus() => this.InnerElement.Focus();

    public override UiComponent RegisterCallback<TEventType>(
      EventCallback<TEventType> callback,
      TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
    {
      bool flag;
      switch (callback)
      {
        case KeyUpEvent _:
        case MouseDownEvent _:
        case ClickEvent _:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
        this.InnerElement.RegisterCallback<TEventType>(callback, useTrickleDown);
      return base.RegisterCallback<TEventType>(callback, useTrickleDown);
    }
  }
}
