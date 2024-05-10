// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Levelling.PopulationStatusBarView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.GameLoop;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Population;
using Mafi.Unity.InputControl.TopStatusBar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Levelling
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class PopulationStatusBarView : IStatusBarItem
  {
    public const float TILE_HEIGHT = 90f;
    public const float TILE_WIDTH = 100f;
    public const float WIDTH = 200f;
    public const float HEIGHT = 90f;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly SettlementsManager m_settlementsManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly UpointsManager m_upointsManager;
    private readonly UiBuilder m_builder;
    private readonly ICalendar m_calendar;
    private readonly Action m_onClick;
    private Panel m_container;
    private IUiUpdater m_updater;
    private readonly ProductProto m_unityProto;

    public PopulationStatusBarView(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      SettlementsManager housingManager,
      PopsHealthManager popsHealthManager,
      UpointsManager upointsManager,
      ProtosDb protosDb,
      SettlementSummaryController settlementSummary,
      UiBuilder builder,
      ICalendar calendar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_settlementsManager = housingManager;
      this.m_popsHealthManager = popsHealthManager;
      this.m_upointsManager = upointsManager;
      this.m_builder = builder;
      this.m_calendar = calendar;
      this.m_onClick = (Action) (() => inputManager.ActivateNewController((IUnityInputController) settlementSummary));
      this.m_unityProto = protosDb.Get<ProductProto>((Proto.ID) IdsCore.Products.Upoints).ValueOrThrow("Unity proto not found!");
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_container = this.m_builder.NewPanel(nameof (PopulationStatusBarView));
      StackContainer leftOf = this.m_builder.NewStackContainer("PopTiles").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).PutToLeftOf<StackContainer>((IUiElement) this.m_container, 0.0f, Offset.Top(5f) + Offset.Bottom(5f));
      PopulationStatusBarView.PopInfoTile popHealthTile = new PopulationStatusBarView.PopInfoTile(this.m_builder, "Assets/Unity/UserInterface/General/Health.svg").OnClick(this.m_onClick).AppendTo<PopulationStatusBarView.PopInfoTile>(leftOf, new float?(100f));
      TextWithIcon healthSubText = new TextWithIcon(this.m_builder, 15);
      healthSubText.SetTextStyle(this.m_builder.Style.Global.TextMediumBold).SetIcon("Assets/Unity/UserInterface/General/PopulationSmall.svg");
      popHealthTile.AddCustomSubTextWithIcon(healthSubText);
      PopulationStatusBarView.PopInfoTile unityTile = new PopulationStatusBarView.PopInfoTile(this.m_builder, "Assets/Unity/UserInterface/General/Unity.svg").OnClick(this.m_onClick).AppendTo<PopulationStatusBarView.PopInfoTile>(leftOf, new float?(100f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => this.m_upointsManager.Quantity)).Observe<Upoints>((Func<Upoints>) (() => this.m_upointsManager.PossibleDiffForLastMonth)).Do((Action<Quantity, Upoints>) ((quantity, lastDiff) =>
      {
        unityTile.SetText(this.m_upointsManager.Quantity.Upoints().FormatWithUnitySuffix().Value);
        if (lastDiff.IsNegative)
        {
          if (quantity.IsZero)
            unityTile.SetCriticalColor();
          else
            unityTile.SetStandardColor();
          unityTile.SetSubText(Tr.QuantityPerMonth.Format(lastDiff.Format()).Value, new ColorRgba?(unityTile.CritricalClr));
        }
        else
        {
          unityTile.SetStandardColor();
          unityTile.SetSubText(Tr.QuantityPerMonth.Format("+" + lastDiff.Format()).Value, new ColorRgba?(unityTile.SuccessClr));
        }
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.m_popsHealthManager.HealthStats.HealthLastMonth)).Observe<Percent>((Func<Percent>) (() => this.m_popsHealthManager.BirthStats.BirthRateLastMonth)).Do((Action<Percent, Percent>) ((popHealth, birthRate) =>
      {
        popHealthTile.SetText(popHealth.ToIntPercentRounded().ToString());
        if (popHealth <= PopsHealthManager.MIN_HEALTH - 10.Percent())
          popHealthTile.SetCriticalColor();
        else if (popHealth <= PopsHealthManager.MIN_HEALTH)
          popHealthTile.SetWarningColor();
        else
          popHealthTile.SetStandardColor();
        if (birthRate.IsPositive)
        {
          healthSubText.SetPrefixText(string.Format("+{0}", (object) birthRate));
          healthSubText.SetColor(popHealthTile.SuccessClr);
        }
        else if (birthRate.IsNegative)
        {
          healthSubText.SetPrefixText(string.Format("{0}", (object) birthRate));
          healthSubText.SetColor(birthRate > -0.5.Percent() ? popHealthTile.WarningClr : popHealthTile.CritricalClr);
        }
        healthSubText.SetVisibility<TextWithIcon>(birthRate.IsNotZero);
      }));
      this.m_updater = updaterBuilder.Build();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<PopulationStatusBarView>(this, (Action<GameTime>) (x => this.m_updater.SyncUpdate()));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<PopulationStatusBarView>(this, (Action<GameTime>) (x => this.m_updater.RenderUpdate()));
      statusBar.AddLargeTileToLeft((IUiElement) this.m_container, 200f, 0.0f);
    }

    public class PopInfoTile : IUiElement
    {
      private readonly UiBuilder m_builder;
      private readonly Btn m_container;
      private readonly Txt m_text;
      private readonly Txt m_subText;
      private readonly ColorRgba m_standardClr;
      public readonly ColorRgba SuccessClr;
      public readonly ColorRgba WarningClr;
      public readonly ColorRgba NormalClr;
      private IconContainer m_icon;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public ColorRgba CritricalClr => this.m_builder.Style.Global.DangerClr;

      public PopInfoTile(UiBuilder builder, string iconPath)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.SuccessClr = new ColorRgba(2211126);
        this.WarningClr = new ColorRgba(15176704);
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_builder = builder;
        TextStyle text = builder.Style.Global.Text;
        this.NormalClr = text.Color;
        this.m_standardClr = text.Color;
        this.m_container = builder.NewBtn(nameof (PopulationStatusBarView)).SetButtonStyle(builder.Style.Panel.HeaderButton);
        this.m_icon = builder.NewIconContainer("PopIcon").SetIcon(iconPath).SetColor(text.Color).PutToCenterTopOf<IconContainer>((IUiElement) this.m_container, new Vector2(40f, 40f));
        Txt txt1 = builder.NewTxt("PopTileText");
        ref TextStyle local1 = ref text;
        int? nullable1 = new int?(14);
        FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
        ColorRgba? color1 = new ColorRgba?();
        FontStyle? fontStyle1 = nullable2;
        int? fontSize1 = nullable1;
        bool? isCapitalized1 = new bool?();
        TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
        this.m_text = txt1.SetTextStyle(textStyle1).BestFitEnabled(14).SetAlignment(TextAnchor.MiddleCenter).PutToTopOf<Txt>((IUiElement) this.m_container, 22f, Offset.Top(40f));
        Txt txt2 = builder.NewTxt("PopTileSubText");
        ref TextStyle local2 = ref text;
        nullable1 = new int?(12);
        nullable2 = new FontStyle?(FontStyle.Bold);
        ColorRgba? color2 = new ColorRgba?();
        FontStyle? fontStyle2 = nullable2;
        int? fontSize2 = nullable1;
        bool? isCapitalized2 = new bool?();
        TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
        this.m_subText = txt2.SetTextStyle(textStyle2).BestFitEnabled(12).SetAlignment(TextAnchor.MiddleCenter).PutToTopOf<Txt>((IUiElement) this.m_container, 20f, Offset.Top(60f));
      }

      public IconContainer AddArrowUp(UiBuilder builder)
      {
        return builder.NewIconContainer("ArrowUp").SetIcon(builder.Style.Icons.ArrowUp).SetColor(this.SuccessClr).PutToRightBottomOf<IconContainer>((IUiElement) this.m_icon, new Vector2(20f, 40f), Offset.Right(-14f));
      }

      public IconContainer AddArrowDown(UiBuilder builder)
      {
        return builder.NewIconContainer("ArrowDown").SetIcon(builder.Style.Icons.ArrowDown).SetColor(this.CritricalClr).PutToRightBottomOf<IconContainer>((IUiElement) this.m_icon, new Vector2(20f, 40f), Offset.Right(-14f));
      }

      public void AddCustomSubTextWithIcon(TextWithIcon txt)
      {
        this.m_subText.Hide<Txt>();
        txt.PutToTopOf<TextWithIcon>((IUiElement) this.m_container, 20f, Offset.Top(60f));
      }

      public Tooltip AddTooltip()
      {
        return this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this.m_container).SetText("");
      }

      public PopulationStatusBarView.PopInfoTile OnClick(Action onClick)
      {
        this.m_container.OnClick(onClick);
        return this;
      }

      public void MakeAsSingleText()
      {
        this.m_text.SetAlignment(TextAnchor.UpperCenter).PutToTopOf<Txt>((IUiElement) this.m_container, 40f, Offset.Top(40f) + Offset.LeftRight(5f));
      }

      public void SetText(string text) => this.m_text.SetText(text);

      public void SetText(LocStrFormatted text) => this.m_text.SetText(text);

      public void SetSubText(string text, ColorRgba? color = null)
      {
        this.m_subText.SetText(text);
        if (!color.HasValue)
          return;
        this.m_subText.SetColor(color.Value);
      }

      public void SetWarningColor()
      {
        this.m_icon.SetColor(this.WarningClr);
        this.m_text.SetColor(this.WarningClr);
        this.m_subText.SetColor(this.WarningClr);
      }

      public void SetCriticalColor()
      {
        this.m_icon.SetColor(this.CritricalClr);
        this.m_text.SetColor(this.CritricalClr);
        this.m_subText.SetColor(this.CritricalClr);
      }

      public void SetStandardColor()
      {
        this.m_icon.SetColor(this.m_standardClr);
        this.m_text.SetColor(this.m_standardClr);
        this.m_subText.SetColor(this.m_standardClr);
      }
    }
  }
}
