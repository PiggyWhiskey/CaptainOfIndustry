// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Slidder
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class Slidder : IUiElement
  {
    private readonly Slider m_slider;
    private readonly UiBuilder m_builder;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public float Value => this.m_slider.value;

    public Option<Txt> Label { get; private set; }

    public Slidder(UiBuilder builder, string name)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.GameObject = new GameObject(name);
      this.RectTransform = this.GameObject.AddComponent<RectTransform>();
      this.GameObject.AddComponent<CanvasRenderer>();
      this.m_slider = this.GameObject.AddComponent<Slider>();
      this.m_slider.navigation = new Navigation()
      {
        mode = Navigation.Mode.None
      };
    }

    /// <summary>
    /// Builds the default simple slider. Don't combine this with <see cref="M:Mafi.Unity.UiFramework.Components.Slidder.SetCustomHandle(Mafi.Unity.UiFramework.IUiElement,System.Single)" />.
    /// </summary>
    public Slidder SimpleSlider(SliderStyle style)
    {
      Sprite sharedSprite = this.m_builder.AssetsDb.GetSharedSprite(style.BgSprite);
      this.m_builder.NewPanel("Background").SetBackground(sharedSprite, new ColorRgba?(style.BgColor)).PutTo<Panel>((IUiElement) this, Offset.TopBottom(5f));
      Panel panel = this.m_builder.NewPanel("Fill").SetBackground(sharedSprite, new ColorRgba?(style.FillColor)).PutTo<Panel>((IUiElement) this, Offset.TopBottom(5f));
      this.m_slider.handleRect = this.m_builder.NewPanel("Slider handle").SetBackground(sharedSprite, new ColorRgba?(style.HandleColor)).PutToLeftTopOf<Panel>((IUiElement) this, new Vector2((float) style.HandleWidth, 0.0f), Offset.Left((float) (-style.HandleWidth / 2))).RectTransform;
      this.m_slider.fillRect = panel.RectTransform;
      return this;
    }

    public Slidder SetCustomHandle(IUiElement handle, float width)
    {
      handle.PutToLeftTopOf<IUiElement>((IUiElement) this, new Vector2(width, 0.0f));
      this.m_slider.handleRect = handle.RectTransform;
      return this;
    }

    public Slidder SetCustomLabel(Txt label)
    {
      this.Label = (Option<Txt>) label;
      return this;
    }

    public Slidder WholeNumbersOnly()
    {
      this.m_slider.wholeNumbers = true;
      return this;
    }

    public Slidder SetValuesRange(float minValue, float maxValue)
    {
      this.m_slider.minValue = minValue;
      this.m_slider.maxValue = maxValue;
      return this;
    }

    public Slidder SetMaxValue(float maxValue)
    {
      this.m_slider.maxValue = maxValue;
      return this;
    }

    public Slidder SetValue(float value)
    {
      this.m_slider.value = value;
      return this;
    }

    public Slidder SetEnabled(bool enabled)
    {
      this.m_slider.interactable = enabled;
      return this;
    }

    public Slidder OnValueChange(Action<float> onChangeAction, Action<float> onCommitAction)
    {
      Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
      sliderEvent.AddListener((UnityAction<float>) (x => onChangeAction(x)));
      this.m_slider.onValueChanged = sliderEvent;
      this.m_slider.gameObject.AddComponent<SliderUpListener>().OnPointerUpAction = Option<Action>.Some((Action) (() => onCommitAction(this.m_slider.value)));
      return this;
    }
  }
}
