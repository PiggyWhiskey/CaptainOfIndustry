// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.InspectorBuilderExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public static class InspectorBuilderExtensions
  {
    public static SwitchBtn AddSwitch(
      this UiBuilder builder,
      StackContainer parent,
      string title,
      Action<bool> action,
      UpdaterBuilder updater,
      Func<bool> provider,
      string tooltip = null)
    {
      SwitchBtn switcher = builder.AddSwitch(parent, title, action, tooltip);
      updater.Observe<bool>(provider).Do((Action<bool>) (s => switcher.SetIsOn(s)));
      return switcher;
    }

    public static SwitchBtn AddSwitch(
      this UiBuilder builder,
      StackContainer parent,
      string title,
      Action<bool> action,
      string tooltip = null)
    {
      SwitchBtn switchBtn = builder.NewSwitchBtn((IUiElement) parent).SetText(title).SetOnToggleAction(action).AppendTo<SwitchBtn>(parent, new float?(builder.Style.Panel.LineHeight), Offset.Left(builder.Style.Panel.Indent));
      if (tooltip != null)
        switchBtn.AddTooltip(tooltip);
      return switchBtn;
    }

    public static Txt AddSectionTitle(
      this UiBuilder builder,
      StackContainer parent,
      LocStrFormatted title,
      LocStrFormatted? tooltip = null,
      Offset? extraOffset = null)
    {
      return builder.AddSectionTitle(parent, title.Value, tooltip?.Value, extraOffset);
    }

    public static Txt AddSectionTitle(
      this UiBuilder builder,
      StackContainer parent,
      string title,
      string tooltip = null,
      Offset? extraOffset = null)
    {
      return builder.CreateSectionTitle((IUiElement) parent, new LocStrFormatted(title), tooltip == null ? new LocStrFormatted?() : new LocStrFormatted?(new LocStrFormatted(tooltip))).AppendTo<Txt>(parent, new float?(builder.Style.Panel.LineHeight), new Offset(0.0f, builder.Style.Panel.SectionTitleTopPadding, builder.Style.Panel.Padding, 0.0f) + (extraOffset ?? Offset.Zero));
    }

    public static Txt CreateSectionTitle(
      this UiBuilder builder,
      LocStrFormatted title,
      LocStrFormatted? tooltip = null)
    {
      Txt title1 = builder.NewTxt("SectionTitle").SetText(title).SetTextStyle(builder.Style.Panel.SectionTitle).SetAlignment(TextAnchor.MiddleLeft);
      if (tooltip.HasValue)
        builder.AddTooltipForTitle(title1, tooltip.Value);
      return title1;
    }

    public static Txt CreateSectionTitle(
      this UiBuilder builder,
      IUiElement parent,
      LocStrFormatted title,
      LocStrFormatted? tooltip = null)
    {
      Txt title1 = builder.NewTxt("SectionTitle", parent).SetText(title).SetTextStyle(builder.Style.Panel.SectionTitle).IncreaseFontForSymbols().SetAlignment(TextAnchor.MiddleLeft);
      if (tooltip.HasValue)
        builder.AddTooltipForTitle(title1, tooltip.Value);
      return title1;
    }

    public static void AddTutorialIconForTitle(
      this UiBuilder builder,
      Txt title,
      MessagesCenterController messagesCenter,
      Proto.ID tutorialId,
      bool hasTooltip)
    {
      builder.NewBtn("Tutorial", (IUiElement) title).SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/Toolbar/Tutorials.svg").OnClick((Action) (() => messagesCenter.ShowMessage(tutorialId))).AddToolTip(Tr.OpenTutorial).PutToLeftMiddleOf<Btn>((IUiElement) title, 18.Vector2(), Offset.Left((float) ((double) title.GetPreferedWidth() + 5.0 + (hasTooltip ? 22.0 : 0.0))));
    }

    internal static Txt CreateSectionTitleBig(
      this UiBuilder builder,
      IUiElement parent,
      LocStrFormatted title,
      LocStrFormatted? tooltip = null)
    {
      Txt title1 = builder.NewTxt("SectionTitle", parent).SetText(title).SetTextStyle(builder.Style.Global.TitleBig).SetAlignment(TextAnchor.MiddleLeft);
      if (tooltip.HasValue)
        builder.AddTooltipForTitle(title1, tooltip.Value);
      return title1;
    }

    public static Panel AddOverlayPanel(
      this UiBuilder builder,
      StackContainer parent,
      int height = 35,
      Offset offset = default (Offset))
    {
      return builder.NewPanel("Panel", (IUiElement) parent).SetBackground(builder.Style.Panel.ItemOverlay).AppendTo<Panel>(parent, new float?((float) height), offset);
    }
  }
}
