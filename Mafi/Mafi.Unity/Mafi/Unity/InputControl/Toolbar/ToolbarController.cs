// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.ToolbarController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ToolbarController
  {
    public const int LEFT_OFFSET_OCCUPIED = 102;
    public Panel ToolsAnchor;
    public Panel ToolsWindowAnchor;
    public Panel OverlayAnchor;
    private int m_numberOfActiveMenus;
    private readonly IUnityInputMgr m_inputMgr;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly LazyResolve<MessagesCenterController> m_messageCenter;
    private readonly DependencyResolver m_resolver;
    private UiBuilder m_builder;
    private ToolbarController.ButtonsStrip m_mainButtonsStrip;
    private ToolbarController.ButtonsGrid m_leftButtonsStrip;
    private ToolbarController.ButtonsStrip m_rightButtonsStrip;
    private Panel m_fillerPanel;
    private Option<IUiElement> m_notificationStrip;

    public bool HasBottomMenuOpen => this.m_numberOfActiveMenus > 0;

    public ToolbarController(
      IUnityInputMgr inputMgr,
      IGameLoopEvents gameLoopEvents,
      ShortcutsManager shortcutsManager,
      LazyResolve<MessagesCenterController> messageCenter,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputMgr = inputMgr;
      this.m_shortcutsManager = shortcutsManager;
      this.m_messageCenter = messageCenter;
      this.m_resolver = resolver;
      inputMgr.ControllerActivated += (Action<IUnityInputController>) (controller => this.onOtherControllerToggled(controller, true));
      inputMgr.ControllerDeactivated += (Action<IUnityInputController>) (controller => this.onOtherControllerToggled(controller, false));
      gameLoopEvents.SyncUpdate.AddNonSaveable<ToolbarController>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<ToolbarController>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void onOtherControllerToggled(
      IUnityInputController controllerUntyped,
      bool wasActivated)
    {
      if (!(controllerUntyped is IToolbarItemController controller))
        return;
      this.m_mainButtonsStrip.ControllerToggled(controller);
      this.m_leftButtonsStrip.ControllerToggled(controller);
      this.m_rightButtonsStrip.ControllerToggled(controller);
      if (controller.Config.Group != ControllerGroup.BottomMenu)
        return;
      if (wasActivated)
        ++this.m_numberOfActiveMenus;
      else
        --this.m_numberOfActiveMenus;
      this.updateToolsAnchor();
    }

    private void syncUpdate(GameTime gameTime)
    {
      this.m_mainButtonsStrip.SyncUpdate(gameTime);
      this.m_leftButtonsStrip.SyncUpdate(gameTime);
      this.m_rightButtonsStrip.SyncUpdate(gameTime);
    }

    private void renderUpdate(GameTime gameTime)
    {
      this.m_fillerPanel.SetVisibility<Panel>(this.m_numberOfActiveMenus <= 0 && hasToolboxVisible());

      bool hasToolboxVisible()
      {
        for (int index = 0; index < this.ToolsAnchor.RectTransform.childCount; ++index)
        {
          if (this.ToolsAnchor.RectTransform.GetChild(index).gameObject.activeInHierarchy)
            return true;
        }
        return false;
      }
    }

    public void Build(UiBuilder builder)
    {
      this.m_builder = builder;
      UiStyle style = builder.Style;
      this.m_fillerPanel = this.m_builder.NewPanel("BottomStripBg").SetBackground(this.m_builder.Style.Global.PanelsBg).PutToBottomOf<Panel>((IUiElement) builder.MainCanvas, this.m_builder.Style.Toolbar.MainMenuStripHeight).Hide<Panel>();
      this.m_mainButtonsStrip = new ToolbarController.ButtonsStrip("Toolbar", builder, this.m_inputMgr).PutToCenterBottomOf<ToolbarController.ButtonsStrip>((IUiElement) builder.MainCanvas, new Vector2(0.0f, style.Toolbar.IconsOnlyMenuStripHeight));
      this.m_leftButtonsStrip = new ToolbarController.ButtonsGrid("LeftToolbar", builder, this.m_inputMgr);
      this.m_leftButtonsStrip.PutToLeftBottomOf<ToolbarController.ButtonsGrid>((IUiElement) builder.MainCanvas, new Vector2(this.m_leftButtonsStrip.GetWidth(), 0.0f));
      this.m_rightButtonsStrip = new ToolbarController.ButtonsStrip("RightToolbar", builder, this.m_inputMgr).PutToRightBottomOf<ToolbarController.ButtonsStrip>((IUiElement) builder.MainCanvas, new Vector2(0.0f, style.Toolbar.IconsOnlyMenuStripHeight));
      this.ToolsAnchor = this.m_builder.NewPanel("ToolsAnchor", (IUiElement) builder.MainCanvas);
      this.ToolsWindowAnchor = this.m_builder.NewPanel("ToolsWindowAnchor", (IUiElement) builder.MainCanvas);
      this.OverlayAnchor = this.m_builder.NewPanel("OverlayAnchor", (IUiElement) this.m_leftButtonsStrip);
      this.updateToolsAnchor();
      foreach (IToolbarItemRegistrar implementation in this.m_resolver.ResolveAll<IToolbarItemRegistrar>().Implementations)
        implementation.RegisterIntoToolbar(this);
    }

    public void SetNotificationStripForCentralButtons(IUiElement strip)
    {
      this.m_notificationStrip = strip.SomeOption<IUiElement>();
      strip.PutToBottomOf<IUiElement>((IUiElement) this.m_builder.MainCanvas, strip.GetHeight(), Offset.Bottom(this.m_builder.Style.Toolbar.MainMenuStripHeight) + Offset.Left(102f));
      strip.SendToBack<IUiElement>();
    }

    private void updateToolsAnchor()
    {
      Assert.That<int>(this.m_numberOfActiveMenus).IsNotNegative();
      UiStyle style = this.m_builder.Style;
      this.ToolsAnchor.PutToRightBottomOf<Panel>((IUiElement) this.m_builder.MainCanvas, new Vector2(this.m_builder.MainCanvas.GetWidth() / 2f, 0.0f), Offset.Bottom(this.m_numberOfActiveMenus <= 0 ? style.Toolbar.MainMenuStripHeight : (float) style.EntitiesMenu.MenuHeight + style.Toolbar.MainMenuStripHeight));
      this.ToolsWindowAnchor.PutToLeftBottomOf<Panel>((IUiElement) this.m_builder.MainCanvas, new Vector2(this.m_builder.MainCanvas.GetWidth() / 2f, 0.0f), Offset.Bottom(this.m_numberOfActiveMenus <= 0 ? style.Toolbar.MainMenuStripHeight : (float) style.EntitiesMenu.MenuHeight + style.Toolbar.MainMenuStripHeight));
      this.OverlayAnchor.PutToBottomOf<Panel>((IUiElement) this.m_builder.MainCanvas, 0.0f, Offset.Bottom(this.m_leftButtonsStrip.GetHeight() * 1.2f));
      this.m_leftButtonsStrip.SetExtraBgVisibility(this.m_numberOfActiveMenus > 0);
    }

    internal float GetToolbarHeight() => this.m_leftButtonsStrip.GetHeight();

    /// <summary>
    /// Adds button to the main menu - the middle section of Toolbar.
    /// </summary>
    /// <remarks>
    /// Main menu is automatically organized to sections. Sections are automatically split by dividers. Last two
    /// decimal digits (00-99) of the order specify order with the section and any higher digits specify the section
    /// number. For example order 205 is order 5 in section 2.
    /// </remarks>
    public void AddMainMenuButton(
      string name,
      IToolbarItemController controller,
      string iconAssetPath,
      float order,
      Func<ShortcutsManager, KeyBindings> shortcut = null)
    {
      this.AddMainMenuButtonAndReturn(name, controller, iconAssetPath, order, shortcut);
    }

    internal ToggleBtn AddMainMenuButtonAndReturn(
      string name,
      IToolbarItemController controller,
      string iconAssetPath,
      float order,
      Func<ShortcutsManager, KeyBindings> shortcut = null)
    {
      ToggleBtn btn = this.m_mainButtonsStrip.AddButton(name, controller, iconAssetPath, order, shortcut);
      btn.AddTooltip(LocStrFormatted.Empty);
      updateTooltip();
      btn.SetOnMouseEnterLeaveActions((Action) (() => { }), new Action(updateTooltip));
      return btn;

      void updateTooltip()
      {
        string str = this.resolveShortcut(name, controller, shortcut);
        string text = str.IsNotEmpty() ? "[<bc>" + str + "</bc>] " + name : name;
        btn.OnTooltip.SetText(text);
        btn.OffTooltip.SetText(text);
      }
    }

    /// <summary>
    /// Same as <see cref="M:Mafi.Unity.InputControl.Toolbar.ToolbarController.AddMainMenuButton(System.String,Mafi.Unity.InputControl.Toolbar.IToolbarItemController,System.String,System.Single,System.Func{Mafi.Unity.InputControl.ShortcutsManager,Mafi.Unity.InputControl.KeyBindings})" /> but adds the button to the left menu.
    /// </summary>
    public ToggleBtn AddLeftMenuButton(
      string name,
      IToolbarItemController controller,
      string iconAssetPath,
      float order,
      Func<ShortcutsManager, KeyBindings> shortcut,
      BtnStyle? styleWhenOn = null,
      BtnStyle? styleWhenOff = null,
      LocStrFormatted? tooltip = null)
    {
      ToggleBtn btn = this.m_leftButtonsStrip.AddButton(name, controller, iconAssetPath, order, shortcut, styleWhenOn, styleWhenOff);
      btn.AddTooltip(LocStrFormatted.Empty);
      updateTooltip();
      this.updateToolsAnchor();
      btn.SetOnMouseEnterLeaveActions((Action) (() => { }), new Action(updateTooltip));
      return btn;

      void updateTooltip()
      {
        string str1 = this.resolveShortcut(name, controller, shortcut);
        string text1 = str1.IsNotEmpty() ? "[<bc>" + str1 + "</bc>] " + name : name;
        if (tooltip.HasValue)
        {
          Tooltip onTooltip = btn.OnTooltip;
          string str2 = text1;
          LocStrFormatted locStrFormatted = tooltip.Value;
          string str3 = locStrFormatted.ToString();
          string text2 = str2 + " " + str3;
          onTooltip.SetText(text2);
          Tooltip offTooltip = btn.OffTooltip;
          string str4 = text1;
          locStrFormatted = tooltip.Value;
          string str5 = locStrFormatted.ToString();
          string text3 = str4 + " " + str5;
          offTooltip.SetText(text3);
        }
        else
        {
          btn.OnTooltip.SetText(text1);
          btn.OffTooltip.SetText(text1);
        }
      }
    }

    public ToggleBtn AddLeftMenuButton(
      string name,
      IToolbarItemController controller,
      string iconAssetPath,
      float order,
      Func<ShortcutsManager, KeyBindings> shortcut,
      Proto.ID tutorialId,
      BtnStyle? styleWhenOn = null,
      BtnStyle? styleWhenOff = null)
    {
      ToggleBtn btn = this.m_leftButtonsStrip.AddButton(name, controller, iconAssetPath, order, shortcut, styleWhenOn, styleWhenOff);
      btn.SetOnToggleAction((Action<bool>) (s =>
      {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
          this.m_messageCenter.Value.ShowMessage(tutorialId);
        else
          this.m_inputMgr.ToggleController((IUnityInputController) controller);
      }));
      btn.AddTooltip(LocStrFormatted.Empty);
      string suffix = " (" + Tr.ClickToLearnMore.Format("<b>alt + click</b>").ToString() + ")";
      updateTooltip();
      this.updateToolsAnchor();
      btn.SetOnMouseEnterLeaveActions((Action) (() => { }), new Action(updateTooltip));
      return btn;

      void updateTooltip()
      {
        string str1 = this.resolveShortcut(name, controller, shortcut);
        string str2 = str1.IsNotEmpty() ? "[<bc>" + str1 + "</bc>] " + name : name;
        btn.OnTooltip.SetText(str2 + suffix);
        btn.OffTooltip.SetText(str2 + suffix);
      }
    }

    /// <summary>
    /// Same as <see cref="M:Mafi.Unity.InputControl.Toolbar.ToolbarController.AddMainMenuButton(System.String,Mafi.Unity.InputControl.Toolbar.IToolbarItemController,System.String,System.Single,System.Func{Mafi.Unity.InputControl.ShortcutsManager,Mafi.Unity.InputControl.KeyBindings})" /> but adds the button to the right menu.
    /// </summary>
    public void AddRightMenuButton(
      string name,
      IToolbarItemController controller,
      string iconAssetPath,
      float order)
    {
      this.m_rightButtonsStrip.AddButton(name, controller, iconAssetPath, order);
    }

    public void AddToolbox(IUiElement element, float width)
    {
      element.PutToCenterBottomOf<IUiElement>((IUiElement) this.ToolsAnchor, new Vector2(width, this.m_builder.Style.Toolbar.ToolboxBtnSize), Offset.Left(100f));
    }

    public void AddSearchBox(IUiElement element, float width)
    {
      element.PutToLeftMiddleOf<IUiElement>((IUiElement) this.m_mainButtonsStrip, new Vector2(width, 30f), Offset.Left((float) (-(double) width - 50.0)));
    }

    private string resolveShortcut(
      string name,
      IToolbarItemController controller,
      Func<ShortcutsManager, KeyBindings> shortcut = null)
    {
      string str = "";
      if (shortcut != null)
      {
        try
        {
          str = shortcut(this.m_shortcutsManager).ToNiceString();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception when resolving a shortcut for " + name + " " + controller.GetType().Name + "!");
        }
      }
      return str;
    }

    private class ButtonsStrip : IUiElement
    {
      private readonly UiBuilder m_builder;
      private readonly IUnityInputMgr m_inputMgr;
      private readonly StackContainer m_buttonsContainer;
      private readonly Dict<IToolbarItemController, ToggleBtn> m_buttonsMap;
      /// <summary>List of added dividers for sections.</summary>
      private readonly Lyst<int> m_dividers;
      private readonly Lyst<IToolbarItemController> m_controllersWithChangedVisibility;

      public GameObject GameObject => this.m_buttonsContainer.GameObject;

      public RectTransform RectTransform => this.m_buttonsContainer.RectTransform;

      public ButtonsStrip(string name, UiBuilder builder, IUnityInputMgr inputMgr)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_buttonsMap = new Dict<IToolbarItemController, ToggleBtn>();
        this.m_dividers = new Lyst<int>();
        this.m_controllersWithChangedVisibility = new Lyst<IToolbarItemController>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_builder = builder;
        this.m_inputMgr = inputMgr;
        UiStyle style = builder.Style;
        this.m_buttonsContainer = builder.NewStackContainer(name).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(new Offset(3f, 3f, 3f, 3f)).SetItemSpacing(1f).SetBackground(style.Global.PanelsBg).PutToCenterBottomOf<StackContainer>((IUiElement) builder.MainCanvas, new Vector2(0.0f, style.Toolbar.MainMenuStripHeight));
      }

      private void ensureDivider(int section)
      {
        if (section == 0 || this.m_dividers.Contains(section))
          return;
        this.m_dividers.Add(section);
        if (section < 0)
          ++section;
        Panel parent = this.m_builder.NewPanel("Divider").AddTo<Panel>(this.m_buttonsContainer, (float) (section * 100), 15f);
        this.m_builder.NewPanel("Line").SetBackground("Assets/Unity/UserInterface/General/ToolbarDivider.png", new ColorRgba?((ColorRgba) 6973287), true).PutToLeftMiddleOf<Panel>((IUiElement) parent, new Vector2(5f, 22f), Offset.Left(5f));
      }

      /// <summary>Adds button to the strip.</summary>
      /// <remarks>
      /// Strip is automatically organized to sections. Sections are automatically split by dividers. Last two
      /// decimal digits (00-99) of the order specify order with the section and any higher digits specify the
      /// section number. For example order 205 is order 5 in section 2.
      /// </remarks>
      public ToggleBtn AddButton(
        string name,
        IToolbarItemController controller,
        string iconAssetPath,
        float order,
        Func<ShortcutsManager, KeyBindings> shortcut = null,
        BtnStyle? styleWhenOn = null,
        BtnStyle? styleWhenOff = null)
      {
        this.ensureDivider(order.FloorToInt() / 100);
        ToggleBtn element = this.m_builder.NewToggleBtn(name, (IUiElement) this.m_buttonsContainer).SetButtonStyleWhenOn(styleWhenOn ?? this.m_builder.Style.Toolbar.ButtonOn).SetButtonStyleWhenOff(styleWhenOff ?? this.m_builder.Style.Toolbar.ButtonOff).SetBtnIcon(iconAssetPath).SetOnToggleAction((Action<bool>) (s => this.m_inputMgr.ToggleController((IUnityInputController) controller)));
        element.SetVisibility<ToggleBtn>(controller.IsVisible).AddTo<ToggleBtn>(this.m_buttonsContainer, order, this.m_builder.Style.Toolbar.MainMenuIconHeight);
        this.m_buttonsMap.Add(controller, element);
        controller.VisibilityChanged += new Action<IToolbarItemController>(this.onControllerVisibilityChanged);
        if (shortcut != null)
          this.m_inputMgr.RegisterGlobalShortcut(shortcut, (IUnityInputController) controller);
        return element;
      }

      public void ControllerToggled(IToolbarItemController controller)
      {
        ToggleBtn toggleBtn;
        if (!this.m_buttonsMap.TryGetValue(controller, out toggleBtn))
          return;
        toggleBtn.Toggle();
      }

      public void SyncUpdate(GameTime time)
      {
        if (this.m_controllersWithChangedVisibility.Count <= 0)
          return;
        foreach (IToolbarItemController key in this.m_controllersWithChangedVisibility)
          this.m_buttonsContainer.SetItemVisibility((IUiElement) this.m_buttonsMap[key], key.IsVisible);
        this.m_controllersWithChangedVisibility.Clear();
      }

      private void onControllerVisibilityChanged(IToolbarItemController controller)
      {
        if (this.m_controllersWithChangedVisibility.Contains(controller))
          return;
        this.m_controllersWithChangedVisibility.Add(controller);
      }
    }

    private class ButtonsGrid : IUiElement
    {
      private readonly UiBuilder m_builder;
      private readonly IUnityInputMgr m_inputMgr;
      private readonly Panel m_container;
      private readonly Panel m_extraBg;
      private readonly GridContainer m_buttonsContainer;
      private readonly Dict<IToolbarItemController, ToggleBtn> m_buttonsMap;
      private readonly Lyst<IToolbarItemController> m_controllersWithChangedVisibility;
      private readonly Lyst<KeyValuePair<IUiElement, float>> m_orderPanels;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public ButtonsGrid(string name, UiBuilder builder, IUnityInputMgr inputMgr)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_buttonsMap = new Dict<IToolbarItemController, ToggleBtn>();
        this.m_controllersWithChangedVisibility = new Lyst<IToolbarItemController>();
        this.m_orderPanels = new Lyst<KeyValuePair<IUiElement, float>>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_builder = builder;
        this.m_inputMgr = inputMgr;
        UiStyle style = builder.Style;
        this.m_container = builder.NewPanel("LeftToolbar").SetWidth<Panel>(102f);
        this.m_extraBg = builder.NewPanel("Bg").SetBackground(style.Global.PanelsBg).PutTo<Panel>((IUiElement) this.m_container).Hide<Panel>();
        this.m_buttonsContainer = builder.NewGridContainer(name).SetCellSpacing(2f).SetInnerPadding(new Offset(12f, 3f, 12f, 3f)).SetBackground(style.Global.PanelsBg).SetCellSize(new Vector2(style.Toolbar.MainMenuIconHeight, style.Toolbar.MainMenuIconHeight)).SetDynamicHeightMode(2).PutToBottomOf<GridContainer>((IUiElement) this.m_container, 0.0f);
        this.m_buttonsContainer.SetWidth<GridContainer>(102f);
      }

      public ToggleBtn AddButton(
        string name,
        IToolbarItemController controller,
        string iconAssetPath,
        float order,
        Func<ShortcutsManager, KeyBindings> shortcut,
        BtnStyle? styleWhenOn = null,
        BtnStyle? styleWhenOff = null,
        LocStrFormatted? tooltip = null)
      {
        ToggleBtn toggleBtn = this.m_builder.NewToggleBtn(name, (IUiElement) this.m_buttonsContainer).SetButtonStyleWhenOn(styleWhenOn ?? this.m_builder.Style.Toolbar.ButtonOn).SetButtonStyleWhenOff(styleWhenOff ?? this.m_builder.Style.Toolbar.ButtonOff).SetBtnIcon(iconAssetPath).SetOnToggleAction((Action<bool>) (s => this.m_inputMgr.ToggleController((IUnityInputController) controller)));
        if (tooltip.HasValue)
          toggleBtn.AddTooltip(tooltip.Value);
        toggleBtn.SetVisibility<ToggleBtn>(controller.IsVisible);
        this.m_buttonsMap.Add(controller, toggleBtn);
        controller.VisibilityChanged += new Action<IToolbarItemController>(this.onControllerVisibilityChanged);
        if (shortcut != null)
          this.m_inputMgr.RegisterGlobalShortcut(shortcut, (IUnityInputController) controller);
        this.m_orderPanels.Add(new KeyValuePair<IUiElement, float>((IUiElement) toggleBtn, order));
        this.m_buttonsContainer.ClearAll();
        foreach (KeyValuePair<IUiElement, float> keyValuePair in (IEnumerable<KeyValuePair<IUiElement, float>>) this.m_orderPanels.OrderBy<KeyValuePair<IUiElement, float>, float>((Func<KeyValuePair<IUiElement, float>, float>) (x => x.Value)))
          this.m_buttonsContainer.Append(keyValuePair.Key);
        this.m_container.SetHeight<Panel>(this.m_buttonsContainer.GetRequiredHeight().Max(185f));
        return toggleBtn;
      }

      public void SetExtraBgVisibility(bool isVisible)
      {
        this.m_extraBg.SetVisibility<Panel>(isVisible);
      }

      public void ControllerToggled(IToolbarItemController controller)
      {
        ToggleBtn toggleBtn;
        if (!this.m_buttonsMap.TryGetValue(controller, out toggleBtn))
          return;
        toggleBtn.Toggle();
      }

      public void SyncUpdate(GameTime time)
      {
        if (this.m_controllersWithChangedVisibility.Count <= 0)
          return;
        foreach (IToolbarItemController key in this.m_controllersWithChangedVisibility)
          this.m_buttonsContainer.SetItemVisibility((IUiElement) this.m_buttonsMap[key], key.IsVisible);
        this.m_controllersWithChangedVisibility.Clear();
      }

      private void onControllerVisibilityChanged(IToolbarItemController controller)
      {
        if (this.m_controllersWithChangedVisibility.Contains(controller))
          return;
        this.m_controllersWithChangedVisibility.Add(controller);
      }
    }
  }
}
