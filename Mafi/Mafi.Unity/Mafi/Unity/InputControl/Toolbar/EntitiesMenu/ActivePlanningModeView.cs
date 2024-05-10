// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.ActivePlanningModeView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.InputControl.Tools;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class ActivePlanningModeView : IUiElement
  {
    private Panel m_container;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly PlanningModeInputController m_planningModeController;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ActivePlanningModeView(
      ShortcutsManager shortcutsManager,
      PlanningModeInputController planningModeController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.m_planningModeController = planningModeController;
    }

    public void BuildUi(UiBuilder builder)
    {
      EntitiesMenuUiStyle entitiesMenu = builder.Style.EntitiesMenu;
      this.m_container = builder.NewPanel("Container");
      builder.NewPanel("TopBorder").SetBackground((ColorRgba) 5001098).PutToBottomOf<Panel>((IUiElement) this, (float) (entitiesMenu.TopBorderSize + 2));
      string niceString1 = this.m_shortcutsManager.TogglePlanningMode.ToNiceString();
      Panel panel1 = builder.NewPanel("Container").OnClick((Action) (() => this.m_planningModeController.DeactivateSelf())).SetBackground((ColorRgba) 5001098);
      Panel panel2 = builder.NewPanel("CenterHolder");
      IconContainer leftOf = builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/Toolbar/Planning.svg", (ColorRgba) 16777215).PutToLeftOf<IconContainer>((IUiElement) panel2, 18f);
      Txt txt = builder.NewTxt("Text").SetText(string.Format("{0} [{1}]", (object) Tr.PlanningModeActive__Title, (object) niceString1)).SetTextStyle(builder.Style.Global.Title.Extend(new ColorRgba?((ColorRgba) 16777215))).SetAlignment(TextAnchor.MiddleCenter);
      txt.PutToRightOf<Txt>((IUiElement) panel2, txt.GetPreferedWidth());
      builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) panel1).SetText((LocStrFormatted) Tr.PlanningModeActive__Tooltip);
      panel2.PutToCenterOf<Panel>((IUiElement) panel1, (float) ((double) leftOf.GetWidth() + (double) txt.GetWidth() + 5.0));
      panel1.PutToCenterTopOf<Panel>((IUiElement) this, new Vector2(panel2.GetWidth() + 60f, 28f));
      this.SetHeight<ActivePlanningModeView>(panel1.GetHeight());
      this.m_shortcutsManager.OnKeyBindingsChanged.AddNonSaveable<ActivePlanningModeView>(this, (Action) (() =>
      {
        string niceString2 = this.m_shortcutsManager.TogglePlanningMode.ToNiceString();
        txt.SetText(string.Format("{0} [{1}]", (object) Tr.PlanningModeActive__Title, (object) niceString2));
      }));
    }
  }
}
