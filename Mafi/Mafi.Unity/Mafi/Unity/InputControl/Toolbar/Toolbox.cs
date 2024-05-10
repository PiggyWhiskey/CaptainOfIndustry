// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.Toolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar
{
  public abstract class Toolbox
  {
    protected UiBuilder Builder;
    protected readonly ToolbarController Toolbar;
    private StackContainer m_stackingContainer;

    public bool IsHovered { get; private set; }

    protected Toolbox(ToolbarController toolbar, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Toolbar = toolbar;
      this.Builder = builder;
    }

    protected abstract void BuildCustomItems(UiBuilder builder);

    protected void BuildIfNeeded()
    {
      if (this.m_stackingContainer != null)
        return;
      this.m_stackingContainer = this.Builder.NewStackContainer(nameof (Toolbox)).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f).OnMouseEnter((Action) (() => this.IsHovered = true)).OnMouseLeave((Action) (() => this.IsHovered = true)).Hide<StackContainer>();
      this.BuildCustomItems(this.Builder);
      this.m_stackingContainer.Hide<StackContainer>();
    }

    protected Btn AddButton(
      string name,
      string iconAssetPath,
      Action onClick,
      Func<ShortcutsManager, KeyBindings> shortcut,
      LocStrFormatted tooltip = default (LocStrFormatted))
    {
      Assert.That<UiBuilder>(this.Builder).IsNotNull<UiBuilder>("You must build the toolbox before adding items.");
      Btn parent = this.Builder.NewBtn(name, (IUiElement) this.m_stackingContainer).SetButtonStyle(this.Builder.Style.Toolbar.ToolboxButton).SetIcon(iconAssetPath).OnClick(onClick).AppendTo<Btn>(this.m_stackingContainer, new float?(this.Builder.Style.Toolbar.ToolboxBtnSize));
      if (tooltip.IsNotEmpty)
      {
        Tooltip tooltip1 = parent.AddToolTipAndReturn();
        tooltip1.SetText(tooltip.Value);
        tooltip1.SetExtraOffsetFromBottom(30f);
      }
      if (shortcut != null)
        this.buildShortcutHint((IUiElement) parent, shortcut);
      return parent;
    }

    protected Btn AddButton(
      string text,
      Action onClick,
      Func<ShortcutsManager, KeyBindings> shortcut)
    {
      Assert.That<UiBuilder>(this.Builder).IsNotNull<UiBuilder>("You must build the toolbox before adding items.");
      Btn btn = this.Builder.NewBtn(text, (IUiElement) this.m_stackingContainer);
      BtnStyle toolboxButton = this.Builder.Style.Toolbar.ToolboxButton;
      ref BtnStyle local = ref toolboxButton;
      int? nullable = new int?(20);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      BtnStyle buttonStyle = local.ExtendText(color, fontStyle, fontSize, isCapitalized);
      Btn parent = btn.SetButtonStyle(buttonStyle).SetText(text).OnClick(onClick).AppendTo<Btn>(this.m_stackingContainer, new float?(this.Builder.Style.Toolbar.ToolboxBtnSize));
      this.buildShortcutHint((IUiElement) parent, shortcut);
      return parent;
    }

    protected ToggleBtn AddToggleButton(
      string name,
      string iconAssetPath,
      Action<bool> onClick,
      Func<ShortcutsManager, KeyBindings> shortcut,
      LocStrFormatted tooltip = default (LocStrFormatted))
    {
      ToggleBtn parent = this.Builder.NewToggleBtn(name, (IUiElement) this.m_stackingContainer).SetButtonStyleWhenOn(this.Builder.Style.Toolbar.ToolboxButtonActive).SetButtonStyleWhenOff(this.Builder.Style.Toolbar.ToolboxButton).SetBtnIcon(iconAssetPath).SetOnToggleAction(onClick).AppendTo<ToggleBtn>(this.m_stackingContainer, new float?(this.Builder.Style.Toolbar.ToolboxBtnSize));
      if (tooltip.IsNotEmpty)
        parent.AddTooltip(tooltip, 30f);
      this.buildShortcutHint((IUiElement) parent, shortcut);
      return parent;
    }

    protected void SetBtnVisibility(IUiElement btn, bool isVisible)
    {
      this.m_stackingContainer.SetItemVisibility(btn, isVisible);
    }

    private void buildShortcutHint(IUiElement parent, Func<ShortcutsManager, KeyBindings> shortcut)
    {
      Panel centerTopOf = this.Builder.NewPanel("KeyCode").SetBackground(this.Builder.Style.Icons.WhiteBgGrayBorder, new ColorRgba?((ColorRgba) 2500134)).PutToCenterTopOf<Panel>(parent, new Vector2(44f, 38f), Offset.Top(-32f));
      Panel leftTopOf = this.Builder.NewPanel("KeyCode").SetBackground(this.Builder.Style.Icons.WhiteBgGrayBorder, new ColorRgba?((ColorRgba) 3355443)).PutToLeftTopOf<Panel>((IUiElement) centerTopOf, new Vector2(44f, 38f), Offset.Left(-2f) + Offset.Top(-2f));
      KeyBindings keyBindings = shortcut(this.Builder.ShortcutsManager);
      if (keyBindings.IsCode(KeyCode.Mouse1))
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/RightClick128.png", (ColorRgba) 16619815).PutTo<IconContainer>((IUiElement) leftTopOf, Offset.All(4f));
      else if (keyBindings.IsCode(KeyCode.Mouse0))
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/LeftClick128.png", (ColorRgba) 16619815).PutTo<IconContainer>((IUiElement) leftTopOf, Offset.All(4f));
      else
        this.Builder.NewTxt("Text").SetText(keyBindings.ToNiceString()).SetAlignment(TextAnchor.MiddleCenter).BestFitEnabled().SetTextStyle(this.Builder.Style.Toolbar.ToolboxShortcutText).PutTo<Txt>((IUiElement) leftTopOf, Offset.All(4f));
    }

    protected void AddToToolbar()
    {
      this.Toolbar.AddToolbox((IUiElement) this.m_stackingContainer, this.m_stackingContainer.GetWidth());
    }

    public void Show()
    {
      this.BuildIfNeeded();
      this.m_stackingContainer.Show<StackContainer>();
    }

    public void Hide()
    {
      StackContainer stackingContainer = this.m_stackingContainer;
      if (stackingContainer == null)
        return;
      stackingContainer.Hide<StackContainer>();
    }
  }
}
