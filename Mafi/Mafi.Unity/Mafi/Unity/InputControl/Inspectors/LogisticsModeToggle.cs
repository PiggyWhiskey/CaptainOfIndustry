// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.LogisticsModeToggle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  public class LogisticsModeToggle : IUiElement
  {
    public const int HEIGHT = 35;
    private readonly Panel m_container;
    private readonly Btn m_onBtn;
    private readonly Option<Btn> m_autoBtn;
    private readonly Btn m_offBtn;
    private readonly Panel m_disabledOverlay;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public LogisticsModeToggle(
      IUiElement parent,
      UiBuilder builder,
      bool isInput,
      Action<EntityLogisticsMode> onModeClick,
      bool disabledAutoMode = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_container = builder.NewPanel("LogisticsToggle", parent);
      IconContainer leftMiddleOf = builder.NewIconContainer("Icon").SetIcon(isInput ? "Assets/Unity/UserInterface/General/VehiclesImport.svg" : "Assets/Unity/UserInterface/General/VehiclesExport.svg").AddOutline().PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_container, new Vector2(44f, 25f));
      builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) leftMiddleOf).SetText(isInput ? TrCore.LogisticsControl__InputTooltip.TranslatedString : TrCore.LogisticsControl__OutputTooltip.TranslatedString);
      PanelWithShadow panelWithShadow = builder.NewPanelWithShadow("ToggleShadow", (IUiElement) this.m_container).AddShadowRightBottom();
      StackContainer buttonsContainer = builder.NewStackContainer("ToggleCont", (IUiElement) panelWithShadow).SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.LeftToRight).SetInnerPadding(Offset.All(1f)).SetItemSpacing(1f).SetBackground((ColorRgba) 0).PutToLeftOf<StackContainer>((IUiElement) panelWithShadow, 0.0f);
      LocStrFormatted tooltip1 = (LocStrFormatted) (isInput ? TrCore.LogisticsControl__On_InputTooltip : TrCore.LogisticsControl__On_OutputTooltip);
      LocStrFormatted tooltip2 = (LocStrFormatted) (isInput ? TrCore.LogisticsControl__Off_InputTooltip : TrCore.LogisticsControl__Off_OutputTooltip);
      LocStrFormatted tooltip3 = (LocStrFormatted) (isInput ? TrCore.LogisticsControl__Auto_InputTooltip : TrCore.LogisticsControl__Auto_OutputTooltip);
      this.m_onBtn = addToggleBtn("on", (LocStrFormatted) TrCore.LogisticsControl__On, tooltip1, builder.Style.Global.ToggleBtnOn, (Action) (() => onModeClick(EntityLogisticsMode.On)));
      if (!disabledAutoMode)
        this.m_autoBtn = (Option<Btn>) addToggleBtn("auto", (LocStrFormatted) TrCore.LogisticsControl__Auto, tooltip3, builder.Style.Global.ToggleBtnAuto, (Action) (() => onModeClick(EntityLogisticsMode.Auto)));
      this.m_offBtn = addToggleBtn("off", (LocStrFormatted) TrCore.LogisticsControl__Off, tooltip2, builder.Style.Global.ToggleBtnOff, (Action) (() => onModeClick(EntityLogisticsMode.Off)));
      panelWithShadow.PutToLeftMiddleOf<PanelWithShadow>((IUiElement) this.m_container, new Vector2(buttonsContainer.GetDynamicWidth(), 25f), Offset.Left(leftMiddleOf.GetWidth() + 5f));
      this.m_disabledOverlay = builder.NewPanel("DisabledOverlay").SetBackground(new ColorRgba(4276545, 150)).PutTo<Panel>((IUiElement) buttonsContainer).AddToolTip("Option not available").Hide<Panel>();
      this.m_container.SetSize<Panel>(new Vector2((float) ((double) panelWithShadow.GetWidth() + (double) leftMiddleOf.GetWidth() + 10.0), 35f));

      Btn addToggleBtn(
        string name,
        LocStrFormatted text,
        LocStrFormatted tooltip,
        BtnStyle style,
        Action onClick)
      {
        Btn objectToPlace = builder.NewBtn(name, (IUiElement) buttonsContainer).SetButtonStyle(style).OnClick(onClick).AddToolTip(tooltip.Value).SetText(text);
        return objectToPlace.AppendTo<Btn>(buttonsContainer, new float?((float) objectToPlace.GetOptimalWidth().CeilToInt()));
      }
    }

    public void SetMode(EntityLogisticsMode mode)
    {
      this.m_onBtn.SetEnabled(mode != EntityLogisticsMode.On);
      if (this.m_autoBtn.HasValue)
        this.m_autoBtn.Value.SetEnabled(mode != 0);
      this.m_offBtn.SetEnabled(mode != EntityLogisticsMode.Off);
    }

    public void SetIsEnabled(bool isEnabled)
    {
      this.m_disabledOverlay.SetVisibility<Panel>(!isEnabled);
    }
  }
}
