// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Slider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Slider : Row, IComponentWithLabel
  {
    private readonly Label m_label;
    private readonly Row m_input;
    private readonly Label m_valueLabel;
    private readonly UiComponent m_fill;
    private readonly UiComponent m_thumbShadow;
    private readonly UiComponent m_thumb;
    private Action<float, float> m_onChange;
    private float m_priorValue;
    private float m_value;

    public float Min { get; private set; }

    public float Max { get; private set; }

    public Slider()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMax\u003Ek__BackingField = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector(2.pt());
      UiComponent[] uiComponentArray = new UiComponent[3]
      {
        (UiComponent) (this.m_label = new Label()),
        null,
        null
      };
      Row component = new Row();
      component.Add<Row>((Action<Row>) (c => c.Class<Row>(Cls.slider).RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.onMouseDown), TrickleDown.NoTrickleDown).RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.onKeyDown)).SetFocusable(true)));
      component.Add(new UiComponent().Class<UiComponent>(Cls.slider_track));
      component.Add(this.m_fill = new UiComponent().Class<UiComponent>(Cls.slider_fill));
      component.Add(this.m_thumbShadow = new UiComponent().Class<UiComponent>(Cls.slider_thumbShadow));
      component.Add(this.m_thumb = new UiComponent().Class<UiComponent>(Cls.slider_thumb));
      Row row = component;
      this.m_input = component;
      uiComponentArray[1] = (UiComponent) row;
      uiComponentArray[2] = (UiComponent) (this.m_valueLabel = new Label().Width<Label>(new Px?(10.pt())));
      this.Add(uiComponentArray);
    }

    public Slider Range(float min, float max)
    {
      this.Min = min;
      this.Max = max;
      this.Value(this.m_value);
      return this;
    }

    public Slider Value(float value, bool notify = false)
    {
      this.m_value = value = Math.Clamp(value, this.Min, this.Max);
      Percent width = ((int) Mathf.Round((float) (((double) this.m_value - (double) this.Min) / ((double) this.Max - (double) this.Min) * 100.0))).Percent();
      this.m_fill.Width<UiComponent>(width);
      UiComponent thumbShadow = this.m_thumbShadow;
      Percent? nullable = new Percent?(width);
      Percent? top1 = new Percent?();
      Percent? right1 = new Percent?();
      Percent? bottom1 = new Percent?();
      Percent? left1 = nullable;
      thumbShadow.AbsolutePosition<UiComponent>(top1, right1, bottom1, left1);
      UiComponent thumb = this.m_thumb;
      nullable = new Percent?(width);
      Percent? top2 = new Percent?();
      Percent? right2 = new Percent?();
      Percent? bottom2 = new Percent?();
      Percent? left2 = nullable;
      thumb.AbsolutePosition<UiComponent>(top2, right2, bottom2, left2);
      this.m_valueLabel.Text<Label>(width.ToStringRounded().AsLoc());
      if (notify)
      {
        Action<float, float> onChange = this.m_onChange;
        if (onChange != null)
          onChange(value, this.m_priorValue);
        this.m_priorValue = value;
      }
      return this;
    }

    public Slider OnValueChanged(Action<float, float> onChange)
    {
      this.m_onChange = onChange;
      return this;
    }

    private void onMouseDown(MouseDownEvent evt)
    {
      this.m_priorValue = this.m_value;
      this.Schedule.Execute(new Action(this.checkForMouseUp));
    }

    private void onKeyDown(KeyDownEvent evt)
    {
      if (evt.keyCode != KeyCode.LeftArrow && evt.keyCode != KeyCode.RightArrow)
        return;
      float num = (float) (((double) this.Max - (double) this.Min) * 0.019999999552965164);
      if (evt.shiftKey)
        num *= 5f;
      if (evt.keyCode == KeyCode.LeftArrow)
        num *= -1f;
      this.Value(this.m_value + num, true);
    }

    private void checkForMouseUp()
    {
      if (UnityEngine.Input.GetKey(KeyCode.Mouse0))
      {
        this.Value((float) ((double) this.m_input.RootElement.WorldToLocal(UiScaleHelper.MousePosition).x / (double) this.m_input.WorldBound.width * ((double) this.Max - (double) this.Min)) + this.Min);
        this.Schedule.Execute(new Action(this.checkForMouseUp));
      }
      else
      {
        Action<float, float> onChange = this.m_onChange;
        if (onChange != null)
          onChange(this.m_value, this.m_priorValue);
        this.m_priorValue = this.m_value;
      }
    }

    void IComponentWithLabel.SetLabel(LocStrFormatted text) => this.m_label.Text<Label>(text);

    void IComponentWithLabel.SetLabelWidth(Percent width) => this.m_label.Width<Label>(width);

    void IComponentWithLabel.SetLabelWidth(Px width) => this.m_label.Width<Label>(new Px?(width));
  }
}
