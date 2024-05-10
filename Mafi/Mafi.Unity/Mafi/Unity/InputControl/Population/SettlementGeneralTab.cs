// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.SettlementGeneralTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class SettlementGeneralTab : Tab
  {
    private readonly IUnityInputMgr m_inputManager;
    private readonly SettlementSummaryController m_settlementSummaryController;
    private readonly MessagesCenterController m_messagesCenter;
    private Func<Settlement> m_settlementProvider;
    public int WidthAvailable;

    public Settlement Settlement => this.m_settlementProvider();

    internal SettlementGeneralTab(
      IUnityInputMgr inputManager,
      SettlementSummaryController settlementSummaryController,
      MessagesCenterController messagesCenter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("SettlementSummary");
      this.m_inputManager = inputManager;
      this.m_settlementSummaryController = settlementSummaryController;
      this.m_messagesCenter = messagesCenter;
      this.ShowAfterSync = true;
    }

    public void SetSettlementProvider(Func<Settlement> settlementProvider)
    {
      this.m_settlementProvider = settlementProvider;
    }

    protected override void BuildUi()
    {
      StackContainer topOf = this.Builder.NewStackContainer("container", (IUiElement) this).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Bottom(10f)).PutToTopOf<StackContainer>((IUiElement) this, 0.0f);
      topOf.SizeChanged += (Action<IUiElement>) (x => this.SetHeight<SettlementGeneralTab>(x.GetHeight()));
      topOf.StartBatchOperation();
      NeedsList objectToPlace = new NeedsList((IUiElement) topOf, this.Builder, new Func<ImmutableArray<PopNeed>>(allNeeds));
      this.AddUpdater(objectToPlace.Updater);
      UiStyle style = this.Builder.Style;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Txt parent1 = this.Builder.AddSectionTitle(topOf, (LocStrFormatted) Tr.Occupants__Title, new LocStrFormatted?((LocStrFormatted) Tr.Occupants__TooltipForSettlement));
      StackContainer parent2 = this.Builder.NewStackContainer("PopsCountContainer", (IUiElement) topOf).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.LeftToRight).SetInnerPadding(Offset.Left(15f)).SetBackground(style.Panel.ItemOverlay).AppendTo<StackContainer>(topOf, new float?(35f));
      SomethingOutOfSomethingView occupantsView = new SomethingOutOfSomethingView((IUiElement) parent2, this.Builder);
      occupantsView.First.SetIcon("Assets/Unity/UserInterface/General/Population.svg");
      occupantsView.Second.SetIcon("Assets/Unity/UserInterface/Toolbar/Settlement.svg");
      occupantsView.PutToLeftOf<SomethingOutOfSomethingView>((IUiElement) parent2, 0.0f, Offset.Left(15f));
      this.Builder.CreateSectionTitle((IUiElement) parent1, (LocStrFormatted) Tr.SettlementWaste__Title, new LocStrFormatted?((LocStrFormatted) Tr.SettlementWaste__Tooltip)).PutTo<Txt>((IUiElement) parent1, Offset.Left((float) (this.WidthAvailable / 2)));
      TextWithIcon landfillView = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextMediumBold).SetIcon("Assets/Unity/UserInterface/General/Trash128.png").PutToLeftOf<TextWithIcon>((IUiElement) occupantsView, 0.0f, Offset.Left((float) (this.WidthAvailable / 2)));
      TextWithIcon landfillHealthView = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextMediumBold).SetIcon("Assets/Unity/UserInterface/General/Health.svg").SetColor(this.Style.Global.DangerClr).PutToLeftOf<TextWithIcon>((IUiElement) landfillView, 0.0f);
      updaterBuilder.Observe<PartialQuantity>((Func<PartialQuantity>) (() => this.Settlement.LandfillInSettlement)).Observe<Quantity>((Func<Quantity>) (() => this.Settlement.LandfillLimitForCurrentPopulation)).Observe<Percent>((Func<Percent>) (() => this.Settlement.NominalHealthPenaltyFromWasteLastDay)).Do((Action<PartialQuantity, Quantity, Percent>) ((landfillInSettlement, landfillLimit, healthPenalty) =>
      {
        landfillView.SetPrefixText(string.Format("{0} / {1}", (object) landfillInSettlement.IntegerPart, (object) landfillLimit));
        if (healthPenalty.IsPositive)
        {
          landfillHealthView.SetPrefixText(string.Format("-> {0}", (object) -healthPenalty.ToIntPercentRounded()));
          landfillHealthView.PutToLeftOf<TextWithIcon>((IUiElement) landfillView, landfillView.GetWidth(), Offset.Left(landfillView.GetWidth() + 5f));
        }
        landfillHealthView.SetVisibility<TextWithIcon>(healthPenalty.IsPositive);
        landfillView.SetColor(healthPenalty.IsPositive ? this.Style.Global.DangerClr : this.Style.Global.TextMediumBold.Color);
      }));
      this.Builder.AddTutorialIconForTitle(this.Builder.AddSectionTitle(topOf, (LocStrFormatted) Tr.SettlementServices, new LocStrFormatted?((LocStrFormatted) Tr.SettlementServices__Tooltip)), this.m_messagesCenter, IdsCore.Messages.TutorialOnPopsAndUnity, true);
      objectToPlace.AppendTo<NeedsList>(topOf);
      BalanceInfoView generatedTotalView = new BalanceInfoView(this.Builder, "Assets/Unity/UserInterface/General/UnitySmall.svg").AppendTo<BalanceInfoView>(topOf, new float?(30f), Offset.Top(2f));
      generatedTotalView.SetTitle(Tr.Total.TranslatedString);
      this.Builder.NewBtn("OpenSettlementBtn").SetButtonStyle(this.Builder.Style.Global.GeneralBtn).OnClick((Action) (() => this.m_inputManager.ActivateNewController((IUnityInputController) this.m_settlementSummaryController))).SetText((LocStrFormatted) Tr.PopulationOverview__OpenAction).AppendTo<Btn>(topOf, new Vector2?(new Vector2(220f, 30f)), ContainerPosition.MiddleOrCenter, Offset.Top(10f));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Settlement.TotalHousingCapacity)).Observe<int>((Func<int>) (() => this.Settlement.Population)).Do((Action<int, int>) ((capacity, occupants) =>
      {
        occupantsView.First.SetPrefixText(occupants.ToString());
        occupantsView.Second.SetPrefixText(capacity.ToString());
        ColorRgba color = occupants > capacity ? this.Builder.Style.Global.DangerClr : this.Builder.Style.Global.Text.Color;
        occupantsView.First.SetColor(color);
        occupantsView.Second.SetColor(color);
      }));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Settlement.AllNeeds.Sum((Func<PopNeed, int>) (x => x.UnityAfterLastUpdate.Value.RawValue)))).Do((Action<int>) (upointsRawSum =>
      {
        Upoints unity = new Upoints(Fix32.FromRaw(upointsRawSum));
        generatedTotalView.SetUnityDiff(unity);
        if (unity.IsPositive)
          generatedTotalView.SetPositiveClr();
        else
          generatedTotalView.SetWarningClr();
      }));
      this.AddUpdater(updaterBuilder.Build());
      topOf.FinishBatchOperation();

      ImmutableArray<PopNeed> allNeeds() => this.Settlement.AllNeeds;
    }
  }
}
