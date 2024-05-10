// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.LayoutDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class LayoutDecorator : ILayoutDecorator
  {
    private static readonly LayoutDecorator s_instance;
    private VisualElement m_element;
    private IComponentWithLayout m_owner;

    public static LayoutDecorator GetSharedInstance(
      VisualElement element,
      IComponentWithLayout owner)
    {
      LayoutDecorator.s_instance.m_element = element;
      LayoutDecorator.s_instance.m_owner = owner;
      return LayoutDecorator.s_instance;
    }

    public void SetSize(StyleLength? width, StyleLength? height)
    {
      this.m_owner.SetSize(width, height);
    }

    public void SetMinWidth(StyleLength width) => this.m_element.style.minWidth = width;

    public void SetMinHeight(StyleLength height) => this.m_element.style.minHeight = height;

    public void SetMaxSize(StyleLength? width, StyleLength? height)
    {
      if (width.HasValue)
        this.m_element.style.maxWidth = width.Value;
      if (!height.HasValue)
        return;
      this.m_element.style.maxHeight = height.Value;
    }

    public void SetMargin(Px? top = null, Px? right = null, Px? bottom = null, Px? left = null)
    {
      Px? nullable;
      if (top.HasValue)
      {
        IStyle style = this.m_element.style;
        nullable = top;
        Px auto = Px.Auto;
        StyleLength styleLength = (nullable.HasValue ? (nullable.GetValueOrDefault() == auto ? 1 : 0) : 0) != 0 ? (StyleLength) StyleKeyword.Auto : (StyleLength) top.Value.Pixels;
        style.marginTop = styleLength;
      }
      if (right.HasValue)
      {
        IStyle style = this.m_element.style;
        nullable = right;
        Px auto = Px.Auto;
        StyleLength styleLength = (nullable.HasValue ? (nullable.GetValueOrDefault() == auto ? 1 : 0) : 0) != 0 ? (StyleLength) StyleKeyword.Auto : (StyleLength) right.Value.Pixels;
        style.marginRight = styleLength;
      }
      if (bottom.HasValue)
      {
        IStyle style = this.m_element.style;
        nullable = bottom;
        Px auto = Px.Auto;
        StyleLength styleLength = (nullable.HasValue ? (nullable.GetValueOrDefault() == auto ? 1 : 0) : 0) != 0 ? (StyleLength) StyleKeyword.Auto : (StyleLength) bottom.Value.Pixels;
        style.marginBottom = styleLength;
      }
      if (!left.HasValue)
        return;
      IStyle style1 = this.m_element.style;
      nullable = left;
      Px auto1 = Px.Auto;
      StyleLength styleLength1 = (nullable.HasValue ? (nullable.GetValueOrDefault() == auto1 ? 1 : 0) : 0) != 0 ? (StyleLength) StyleKeyword.Auto : (StyleLength) left.Value.Pixels;
      style1.marginLeft = styleLength1;
    }

    public void SetMargin(Percent? top = null, Percent? right = null, Percent? bottom = null, Percent? left = null)
    {
      if (top.HasValue)
        this.m_element.style.marginTop = (StyleLength) new Length((float) top.Value.ToIntPercentRounded(), LengthUnit.Percent);
      if (right.HasValue)
        this.m_element.style.marginRight = (StyleLength) new Length((float) right.Value.ToIntPercentRounded(), LengthUnit.Percent);
      if (bottom.HasValue)
        this.m_element.style.marginBottom = (StyleLength) new Length((float) bottom.Value.ToIntPercentRounded(), LengthUnit.Percent);
      if (!left.HasValue)
        return;
      this.m_element.style.marginLeft = (StyleLength) new Length((float) left.Value.ToIntPercentRounded(), LengthUnit.Percent);
    }

    public void AddMarginLeft(float diff)
    {
      if (this.m_element.style.marginLeft.keyword == StyleKeyword.Auto)
        return;
      this.m_element.style.marginLeft = (StyleLength) (this.m_element.style.marginLeft.value.value + diff);
    }

    public void AddMarginTop(float diff)
    {
      if (this.m_element.style.marginTop.keyword == StyleKeyword.Auto)
        return;
      this.m_element.style.marginTop = (StyleLength) (this.m_element.style.marginTop.value.value + diff);
    }

    public void AddMarginLeftRight(float diff)
    {
      StyleLength styleLength1;
      Length length;
      if (this.m_element.style.marginLeft.keyword != StyleKeyword.Auto)
      {
        IStyle style = this.m_element.style;
        styleLength1 = this.m_element.style.marginLeft;
        length = styleLength1.value;
        StyleLength styleLength2 = (StyleLength) (length.value + diff);
        style.marginLeft = styleLength2;
      }
      styleLength1 = this.m_element.style.marginRight;
      if (styleLength1.keyword == StyleKeyword.Auto)
        return;
      IStyle style1 = this.m_element.style;
      styleLength1 = this.m_element.style.marginRight;
      length = styleLength1.value;
      StyleLength styleLength3 = (StyleLength) (length.value + diff);
      style1.marginRight = styleLength3;
    }

    public void AddMarginTopBottom(float diff)
    {
      StyleLength styleLength1;
      Length length;
      if (this.m_element.style.marginTop.keyword != StyleKeyword.Auto)
      {
        IStyle style = this.m_element.style;
        styleLength1 = this.m_element.style.marginTop;
        length = styleLength1.value;
        StyleLength styleLength2 = (StyleLength) (length.value + diff);
        style.marginTop = styleLength2;
      }
      styleLength1 = this.m_element.style.marginBottom;
      if (styleLength1.keyword == StyleKeyword.Auto)
        return;
      IStyle style1 = this.m_element.style;
      styleLength1 = this.m_element.style.marginBottom;
      length = styleLength1.value;
      StyleLength styleLength3 = (StyleLength) (length.value + diff);
      style1.marginBottom = styleLength3;
    }

    public void Fill()
    {
      this.m_element.style.flexGrow = (StyleFloat) 1f;
      this.m_element.style.flexShrink = (StyleFloat) 1f;
    }

    public void FlexGrow(float flexGrow) => this.m_element.style.flexGrow = (StyleFloat) flexGrow;

    public void FlexShrink(float flexShrink)
    {
      this.m_element.style.flexShrink = (StyleFloat) flexShrink;
    }

    public void RelativePosition()
    {
      this.m_element.style.position = (StyleEnum<Position>) Position.Relative;
    }

    public void SetAbsolutePosition(
      StyleLength? top = null,
      StyleLength? right = null,
      StyleLength? bottom = null,
      StyleLength? left = null)
    {
      this.m_element.style.position = (StyleEnum<Position>) Position.Absolute;
      if (top.HasValue)
        this.m_element.style.top = top.Value;
      if (right.HasValue)
        this.m_element.style.right = right.Value;
      if (bottom.HasValue)
        this.m_element.style.bottom = bottom.Value;
      if (!left.HasValue)
        return;
      this.m_element.style.left = left.Value;
    }

    public void AlignSelf(Align alignSelf)
    {
      this.m_element.style.alignSelf = (StyleEnum<UnityEngine.UIElements.Align>) alignSelf.ToUnity();
    }

    public void Opacity(float value) => this.m_element.style.opacity = (StyleFloat) value;

    public void SetRotate(int? degrees)
    {
      this.m_element.style.rotate = degrees.HasValue ? new StyleRotate(new Rotate((Angle) (float) degrees.Value)) : (StyleRotate) StyleKeyword.Null;
    }

    public void SetScale(float x = 1f, float y = 1f)
    {
      if ((double) x == 1.0 && (double) y == 1.0)
        this.m_element.style.scale = (StyleScale) StyleKeyword.Null;
      else
        this.m_element.style.scale = (StyleScale) new Scale(new Vector2(x, y));
    }

    public void SetTranslate(int x = 0, int y = 0)
    {
      if (x == 1 && y == 1)
        this.m_element.style.translate = (StyleTranslate) StyleKeyword.Null;
      else
        this.m_element.style.translate = new StyleTranslate(new Translate((Length) (float) x, (Length) (float) y));
    }

    public void SetTranslate(Percent x, Percent y)
    {
      this.m_element.style.translate = (StyleTranslate) new Translate(new Length((float) x.ToIntPercentRounded(), LengthUnit.Percent), new Length((float) y.ToIntPercentRounded(), LengthUnit.Percent));
    }

    public void SetTransformOrigin(Percent x, Percent y)
    {
      this.m_element.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(new Length((float) x.ToIntPercentRounded(), LengthUnit.Percent), new Length((float) y.ToIntPercentRounded(), LengthUnit.Percent)));
    }

    public LayoutDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static LayoutDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LayoutDecorator.s_instance = new LayoutDecorator();
    }
  }
}
