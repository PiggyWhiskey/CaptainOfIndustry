// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Dropdwn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class Dropdwn : IUiElement
  {
    public static int HEIGHT;
    private readonly TMP_Dropdown m_dropdown;
    private readonly UiBuilder m_builder;
    private Image m_image;
    private Txt m_label;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public int Value => this.m_dropdown.value;

    public Txt Label => this.m_label;

    public Dropdwn(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.GameObject = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = this.GameObject.GetComponent<RectTransform>();
      this.m_dropdown = this.GameObject.AddComponent<TMP_Dropdown>();
      Sprite sharedSprite = builder.AssetsDb.GetSharedSprite(builder.Style.Icons.WhiteBgGrayBorder);
      ColorRgba colorRgba = (ColorRgba) 3487029;
      this.m_dropdown.colors = this.m_dropdown.colors with
      {
        normalColor = 11776947.ToColor(),
        highlightedColor = 15066597.ToColor(),
        selectedColor = 11776947.ToColor(),
        fadeDuration = 0.0f
      };
      this.m_image = this.GameObject.AddComponent<Image>();
      this.m_image.sprite = sharedSprite;
      this.m_image.type = Image.Type.Sliced;
      this.m_image.color = 3487029.ToColor();
      this.m_dropdown.targetGraphic = (Graphic) this.m_image;
      this.m_label = builder.NewTxt(nameof (Label), (IUiElement) this).SetTextStyle(builder.Style.Global.TextControls).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) this, Offset.Left(10f) + Offset.Right(30f));
      this.m_dropdown.captionText = this.m_label.GameObject.GetComponent<TMP_Text>();
      builder.NewIconContainer("Arrow", (IUiElement) this).SetIcon("Assets/Unity/UserInterface/General/ArrowDown.svg").SetColor(builder.Style.Global.TextControls.Color).PutToRightMiddleOf<IconContainer>((IUiElement) this, 14.Vector2(), Offset.Right(10f));
      ScrollableContainer scrollableContainer = builder.NewScrollableContainer("Template", (IUiElement) this).AddVerticalScrollbar().DisableHorizontalScroll().PutToBottomOf<ScrollableContainer>((IUiElement) this, 300f, Offset.Bottom(2f)).Hide<ScrollableContainer>();
      scrollableContainer.RectTransform.pivot = new Vector2(0.5f, 1f);
      Image image = scrollableContainer.GameObject.AddComponent<Image>();
      image.sprite = sharedSprite;
      image.type = Image.Type.Sliced;
      image.color = colorRgba.ToColor();
      Panel panel1 = builder.NewPanel("Content", (IUiElement) scrollableContainer.Viewport);
      panel1.PutToTopOf<Panel>((IUiElement) scrollableContainer.Viewport, 28f);
      scrollableContainer.SetContentToScroll((IUiElement) panel1);
      Panel middleOf = builder.NewPanel("Item", (IUiElement) panel1).PutToMiddleOf<Panel>((IUiElement) panel1, 20f, Offset.LeftRight(5f));
      Toggle toggle = middleOf.GameObject.AddComponent<Toggle>();
      ColorBlock colors = toggle.colors with
      {
        normalColor = 14079702.ToColor(),
        highlightedColor = 5723991.ToColor(),
        selectedColor = 16777215.ToColor(),
        fadeDuration = 0.0f
      };
      toggle.colors = colors;
      Panel panel2 = builder.NewPanel("Item Background", (IUiElement) middleOf).SetBackground(colorRgba).PutTo<Panel>((IUiElement) middleOf);
      toggle.targetGraphic = panel2.GameObject.GetComponent<Graphic>();
      IconContainer leftMiddleOf = builder.NewIconContainer("Item Checkmark", (IUiElement) middleOf).SetIcon("Assets/Unity/UserInterface/General/Tick128.png").SetColor(builder.Style.Global.TextControls.Color).PutToLeftMiddleOf<IconContainer>((IUiElement) middleOf, 12.Vector2());
      toggle.graphic = leftMiddleOf.GameObject.GetComponent<Graphic>();
      this.m_dropdown.itemText = builder.NewTxt("Item Label", (IUiElement) middleOf).SetTextStyle(builder.Style.Global.TextControls).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) middleOf, Offset.Left(leftMiddleOf.GetWidth() + 5f)).GameObject.GetComponent<TMP_Text>();
      this.m_dropdown.template = scrollableContainer.RectTransform;
    }

    public Dropdwn SetEnabled(bool isEnabled)
    {
      this.m_dropdown.interactable = isEnabled;
      return this;
    }

    public Dropdwn SetLabelTextColor(ColorRgba color)
    {
      this.m_label.SetColor(color);
      return this;
    }

    public Dropdwn SetBackground(ColorRgba color)
    {
      this.m_image.color = color.ToColor();
      return this;
    }

    public Dropdwn AddOptions(List<string> options)
    {
      this.m_dropdown.AddOptions(options);
      return this;
    }

    public Dropdwn ClearOptions()
    {
      this.m_dropdown.ClearOptions();
      return this;
    }

    public Dropdwn SetValueWithoutNotify(int itemIndex)
    {
      this.m_dropdown.SetValueWithoutNotify(itemIndex);
      return this;
    }

    public Dropdwn OnValueChange(Action<int> onChangeAction)
    {
      TMP_Dropdown.DropdownEvent dropdownEvent = new TMP_Dropdown.DropdownEvent();
      dropdownEvent.AddListener((UnityAction<int>) (x => onChangeAction(x)));
      this.m_dropdown.onValueChanged = dropdownEvent;
      return this;
    }

    public Dropdwn AddTooltip(string tooltip)
    {
      this.m_builder.AddTooltipForElement((IUiElement) this, tooltip);
      return this;
    }

    static Dropdwn()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Dropdwn.HEIGHT = 30;
    }
  }
}
