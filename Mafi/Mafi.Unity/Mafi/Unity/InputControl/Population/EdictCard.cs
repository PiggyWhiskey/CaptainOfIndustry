// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.EdictCard
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.Population.Edicts;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  internal class EdictCard : IUiElement
  {
    public const int TOP_CARD_WIDTH = 50;
    public const int TOP_CARD_HEIGHT = 30;
    public const int TOTAL_WIDTH = 345;
    public const int TOTAL_HEIGHT = 133;
    public readonly IUiUpdater Updater;
    public readonly Edict Edict;
    private readonly Panel m_container;
    private readonly Btn m_btn;
    private readonly Btn m_topCard;
    private EdictCard.State m_state;
    public bool IsSelected;
    private readonly IconContainer m_tickIcon;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiElement TopCard => (IUiElement) this.m_topCard;

    public EdictCard(
      IUiElement parent,
      UiBuilder builder,
      IInputScheduler inputScheduler,
      UnlockedProtosDbForUi unlockedProtosDb,
      Edict edict,
      int order,
      Action<EdictCard> onHeaderClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      EdictCard edictCard = this;
      this.Edict = edict;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_container = builder.NewPanel("EdictToggle", parent);
      int leftOffset = order * 50;
      this.m_topCard = builder.NewBtn(nameof (TopCard), (IUiElement) this.m_container).PlayErrorSoundWhenDisabled().OnClick((Action) (() => onHeaderClick(edictCard))).SetButtonStyle(new BtnStyle()
      {
        BackgroundClr = new ColorRgba?(builder.Style.Panel.ItemOverlay),
        NormalMaskClr = new ColorRgba?((ColorRgba) 16777215),
        HoveredMaskClr = new ColorRgba?((ColorRgba) 13158600),
        PressedMaskClr = new ColorRgba?((ColorRgba) 16777215),
        Border = new BorderStyle(ColorRgba.Gray)
      }).PutToLeftTopOf<Btn>((IUiElement) this.m_container, new Vector2(50f, 32f), Offset.Left((float) leftOffset));
      Txt txt1 = builder.NewTxt("Title", (IUiElement) this.m_topCard);
      TextStyle title = builder.Style.Global.Title;
      ref TextStyle local = ref title;
      int? nullable = new int?(15);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      Txt parent1 = txt1.SetTextStyle(textStyle).SetText(EdictProto.ROMAN_NUMERALS[order]).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) this.m_topCard, Offset.Left(6f));
      this.m_tickIcon = builder.NewIconContainer("TickIcon", (IUiElement) parent1).SetIcon("Assets/Unity/UserInterface/General/Tick128.png").PutToRightOf<IconContainer>((IUiElement) parent1, 16f, Offset.Right(4f));
      this.m_btn = builder.NewBtn("EdictToggle", (IUiElement) this.m_container).PlayErrorSoundWhenDisabled().OnClick((Action) (() => inputScheduler.ScheduleInputCmd<ToggleEdictEnabledCmd>(new ToggleEdictEnabledCmd(edictCard.Edict.Prototype)))).SetButtonStyle(new BtnStyle()
      {
        BackgroundClr = new ColorRgba?(builder.Style.Panel.ItemOverlay),
        NormalMaskClr = new ColorRgba?((ColorRgba) 16777215),
        HoveredMaskClr = new ColorRgba?((ColorRgba) 13158600),
        PressedMaskClr = new ColorRgba?((ColorRgba) 16777215),
        Border = new BorderStyle(ColorRgba.Gray)
      }).PutTo<Btn>((IUiElement) this.m_container, Offset.Top(30f));
      int coord = 35;
      builder.NewIconContainer("EdictIcon", (IUiElement) this.m_btn).SetIcon(edict.Prototype.Graphics.IconPath).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_btn, coord.Vector2(), Offset.Left(10f));
      StackContainer topOf = builder.NewStackContainer("EdictToggle", (IUiElement) this.m_btn).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutToTopOf<StackContainer>((IUiElement) this.m_btn, 0.0f, Offset.All(5f) + Offset.Left((float) (10 + coord + 5)));
      Panel parent2 = builder.NewPanel("BtnTitleHolder", (IUiElement) topOf).AppendTo<Panel>(topOf, new float?(25f));
      Txt parent3 = builder.NewTxt("title", (IUiElement) parent2).SetText((LocStrFormatted) this.Edict.Prototype.Strings.Name).SetAlignment(TextAnchor.MiddleLeft).EnableRichText().SetTextStyle(builder.Style.Global.Title).PutTo<Txt>((IUiElement) parent2);
      if (edict.Prototype.IsGeneratingUnity)
        parent3.SetColor((ColorRgba) 12287956);
      Upoints upoints = -edict.Prototype.MonthlyUpointsCost;
      TextWithIcon unityPrice = new TextWithIcon(builder, (IUiElement) parent3, 16).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetPrefixText(upoints.IsPositive ? "+" + upoints.Format() : upoints.Format()).PutToLeftOf<TextWithIcon>((IUiElement) parent3, 40f, Offset.Left(parent3.GetPreferedWidth() + 10f));
      Txt txt2 = builder.NewTxt("Info", (IUiElement) topOf).SetAlignment(TextAnchor.UpperLeft).SetTextStyle(builder.Style.Panel.Text).BestFitEnabled(14).AppendTo<Txt>(topOf, new float?(40f));
      this.m_container.SetHeight<Panel>((float) ((double) topOf.GetDynamicHeight() - 2.0 + 10.0 + 30.0));
      this.m_container.SetWidth<Panel>(345f);
      txt2.SetText((LocStrFormatted) this.Edict.Prototype.Strings.DescShort);
      updaterBuilder.Observe<bool>((Func<bool>) (() => edictCard.Edict.IsEnabled)).Observe<Edict.EdictEnableCheckResult>(new Func<Edict.EdictEnableCheckResult>(this.Edict.CanBeEnabled)).Do((Action<bool, Edict.EdictEnableCheckResult>) ((isEnabled, canBeEnabledResult) => edictCard.m_btn.SetEnabled(isEnabled || canBeEnabledResult.CanBeEnabled)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => edictCard.Edict.IsEnabled)).Observe<bool>((Func<bool>) (() => edictCard.Edict.IsActive)).Do((Action<bool, bool>) ((isEnabled, isActive) =>
      {
        if (!isActive)
          unityPrice.SetColor(builder.Style.Global.Text.Color);
        else if (edict.Prototype.MonthlyUpointsCost.IsPositive)
          unityPrice.SetColor(builder.Style.Global.DangerClr);
        else
          unityPrice.SetColor(builder.Style.Global.GreenForDark);
        closure_0.m_state = isEnabled ? (isActive ? EdictCard.State.Active : EdictCard.State.ActiveBlocked) : EdictCard.State.NotActive;
        closure_0.updateColors();
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => unlockedProtosDb.IsUnlocked((IProto) edictCard.Edict.Prototype))).Do((Action<bool>) (isUnlocked =>
      {
        edictCard.m_topCard.SetEnabled(isUnlocked);
        edictCard.m_tickIcon.SetIcon(isUnlocked ? "Assets/Unity/UserInterface/General/Tick128.png" : "Assets/Unity/UserInterface/General/Locked128.png");
      }));
      this.Updater = updaterBuilder.Build();
    }

    public void SetSelected(bool isSelected)
    {
      this.IsSelected = isSelected;
      this.updateColors();
    }

    private void updateColors()
    {
      ColorRgba borderColor = this.GetBorderColor(false);
      this.m_topCard.SetBorderColor(borderColor);
      this.m_btn.SetBorderColor(borderColor);
      this.m_tickIcon.SetColor(this.GetBorderColor(true));
    }

    private ColorRgba GetBorderColor(bool ignoreSelected)
    {
      if (this.m_state == EdictCard.State.Active)
        return (ColorRgba) (this.IsSelected | ignoreSelected ? 4371254 : 4163384);
      return this.m_state == EdictCard.State.ActiveBlocked ? (ColorRgba) (this.IsSelected | ignoreSelected ? 16750848 : 12481798) : (ColorRgba) (this.IsSelected | ignoreSelected ? 10658466 : 5723991);
    }

    public void SetBottomPartVisibility(bool isVisible) => this.m_btn.SetVisibility<Btn>(isVisible);

    private enum State
    {
      NotActive,
      ActiveBlocked,
      Active,
    }
  }
}
