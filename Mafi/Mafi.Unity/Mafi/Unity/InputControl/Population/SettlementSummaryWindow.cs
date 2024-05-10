// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.SettlementSummaryWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class SettlementSummaryWindow : WindowView
  {
    private readonly MessagesCenterController m_messagesCenter;
    private readonly SettlementsManager m_settlementsManager;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly UpointsManager m_upointsManager;
    private BalanceTable<Upoints> m_unityTable;
    private BalanceTable<Percent> m_birthTable;
    private BalanceTable<Percent> m_healthTable;

    internal SettlementSummaryWindow(
      MessagesCenterController messagesCenter,
      SettlementsManager settlementsManager,
      PopsHealthManager popsHealthManager,
      UpointsManager upointsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("SettlementGeneralTab");
      this.m_messagesCenter = messagesCenter;
      this.m_settlementsManager = settlementsManager;
      this.m_popsHealthManager = popsHealthManager;
      this.m_upointsManager = upointsManager;
    }

    protected override void BuildWindowContent()
    {
      int widthAvailable = 700;
      Vector2 size = new Vector2((float) widthAvailable, 600f);
      this.SetTitle((LocStrFormatted) Tr.PopulationOverview__Title);
      this.SetContentSize(size);
      this.PositionSelfToCenter();
      this.MakeMovable();
      UiBuilder builder1 = this.Builder;
      int num1 = widthAvailable;
      Func<Upoints, Upoints, Upoints> sumFunc1 = new Func<Upoints, Upoints, Upoints>(upointsSumFunc);
      Func<Upoints, string> toStringFunc1 = new Func<Upoints, string>(upointsStringFunc);
      Upoints zero1 = Upoints.Zero;
      Func<Upoints, bool> isAboveZero1 = new Func<Upoints, bool>(upointsIsAboveZero);
      Upoints zero2 = zero1;
      int widthAvailable1 = num1;
      this.m_unityTable = new BalanceTable<Upoints>(builder1, sumFunc1, toStringFunc1, isAboveZero1, zero2, widthAvailable1, "Assets/Unity/UserInterface/General/UnitySmall.svg", 44);
      UiBuilder builder2 = this.Builder;
      int num2 = widthAvailable;
      Func<Percent, Percent, Percent> sumFunc2 = new Func<Percent, Percent, Percent>(percentSumFunc);
      Func<Percent, string> toStringFunc2 = new Func<Percent, string>(percentStringFunc);
      Percent zero3 = Percent.Zero;
      Func<Percent, bool> isAboveZero2 = new Func<Percent, bool>(percentIsAboveZero);
      Percent zero4 = zero3;
      int widthAvailable2 = num2;
      this.m_birthTable = new BalanceTable<Percent>(builder2, sumFunc2, toStringFunc2, isAboveZero2, zero4, widthAvailable2, "Assets/Unity/UserInterface/General/PopulationSmall.svg", 48);
      UiBuilder builder3 = this.Builder;
      int num3 = widthAvailable;
      Func<Percent, Percent, Percent> sumFunc3 = new Func<Percent, Percent, Percent>(percentSumFunc);
      Func<Percent, string> toStringFunc3 = new Func<Percent, string>(healthStringFunc);
      Percent zero5 = Percent.Zero;
      Func<Percent, bool> isAboveZero3 = new Func<Percent, bool>(percentIsAboveZero);
      Percent zero6 = zero5;
      int widthAvailable3 = num3;
      this.m_healthTable = new BalanceTable<Percent>(builder3, sumFunc3, toStringFunc3, isAboveZero3, zero6, widthAvailable3, "Assets/Unity/UserInterface/General/Health.svg", 28);
      UiStyle style = this.Builder.Style;
      UpdaterBuilder updaterBuilder1 = UpdaterBuilder.Start();
      StackContainer topOf = this.Builder.NewStackContainer("container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Bottom(10f)).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      topOf.SizeChanged += (Action<IUiElement>) (x => this.SetContentSize((float) widthAvailable, x.GetHeight()));
      TextStyle title = style.Global.Title;
      ref TextStyle local = ref title;
      int? nullable = new int?(14);
      ColorRgba? color1 = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color1, fontStyle, fontSize, isCapitalized);
      Txt parent1 = this.AddSectionTitle(topOf, (LocStrFormatted) Tr.Occupants__Title, new LocStrFormatted?((LocStrFormatted) Tr.Occupants__TooltipForIsland));
      Panel parent2 = this.AddOverlayPanel(topOf);
      SomethingOutOfSomethingView occupantsView = new SomethingOutOfSomethingView((IUiElement) parent2, this.Builder);
      occupantsView.First.SetIcon("Assets/Unity/UserInterface/General/Population.svg");
      occupantsView.Second.SetIcon("Assets/Unity/UserInterface/Toolbar/Settlement.svg");
      occupantsView.PutToLeftOf<SomethingOutOfSomethingView>((IUiElement) parent2, 0.0f, Offset.Left(15f));
      Txt amountOfSettlements = this.Builder.NewTxt("SettlementsCount", (IUiElement) occupantsView).SetText(Tr.NumberOfSettlements.Format(4.ToString(), 4)).SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleRight);
      amountOfSettlements.SetWidth<Txt>(amountOfSettlements.GetPreferedWidth());
      occupantsView.AppendElement((IUiElement) amountOfSettlements);
      float leftOffset1 = (float) ((double) widthAvailable * 2.0 / 5.0);
      float num4 = (float) ((double) widthAvailable * 3.0 / 5.0 / 2.0);
      this.Builder.CreateSectionTitle((IUiElement) parent1, (LocStrFormatted) Tr.UnityCap__Title, new LocStrFormatted?((LocStrFormatted) Tr.UnityCap__Tooltip)).PutTo<Txt>((IUiElement) parent1, Offset.Left(leftOffset1));
      TextWithIcon unityCap = new TextWithIcon(this.Builder).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").PutToLeftOf<TextWithIcon>((IUiElement) occupantsView, 0.0f, Offset.Left(leftOffset1));
      float leftOffset2 = (float) ((double) leftOffset1 + (double) num4 - 50.0);
      this.CreateSectionTitle((IUiElement) parent1, (LocStrFormatted) Tr.CurrentDisease__Title, new LocStrFormatted?((LocStrFormatted) Tr.CurrentDisease__Tooltip)).PutTo<Txt>((IUiElement) parent1, Offset.Left(leftOffset2));
      Txt diseaseName = this.Builder.NewTxt("DiseaseName", (IUiElement) occupantsView).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Builder.Style.Global.TextInc);
      diseaseName.PutToLeftOf<Txt>((IUiElement) occupantsView, 0.0f, Offset.Left(leftOffset2 + 18f));
      Panel diseaseNameDesc = this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?(this.Builder.Style.Global.DangerClr)).PutToLeftMiddleOf<Panel>((IUiElement) diseaseName, 14.Vector2(), Offset.Left(-18f));
      Tooltip diseaseTooltip = this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) diseaseNameDesc);
      updaterBuilder1.Observe<Option<DiseaseProto>>((Func<Option<DiseaseProto>>) (() => this.m_popsHealthManager.CurrentDisease)).Do((Action<Option<DiseaseProto>>) (currentDisease =>
      {
        if (currentDisease.HasValue)
        {
          DiseaseProto diseaseProto = currentDisease.Value;
          if (diseaseProto.Reason.TranslatedString.IsEmpty())
            diseaseName.SetText((LocStrFormatted) diseaseProto.Strings.Name);
          else
            diseaseName.SetText(string.Format("{0} ({1})", (object) diseaseProto.Strings.Name, (object) diseaseProto.Reason));
          diseaseTooltip.SetText((LocStrFormatted) diseaseProto.Strings.DescShort);
        }
        else
          diseaseName.SetText((LocStrFormatted) Tr.CurrentDisease__NoDisease);
        diseaseName.SetWidth<Txt>(diseaseName.GetPreferedWidth());
        diseaseName.SetColor(currentDisease.HasValue ? this.Builder.Style.Global.DangerClr : this.Builder.Style.Global.Text.Color);
        diseaseNameDesc.SetVisibility<Panel>(currentDisease.HasValue);
      }));
      Txt element = this.Builder.AddSectionTitle(topOf, (LocStrFormatted) Tr.PopulationGrowth__Title, new LocStrFormatted?((LocStrFormatted) Tr.PopulationGrowth__Tooltip), new Offset?(Offset.Top(5f)));
      this.m_birthTable.AppendTo<BalanceTable<Percent>>(topOf);
      element.SendToFront<Txt>();
      Txt txt1 = this.Builder.AddSectionTitle(topOf, (LocStrFormatted) Tr.Health, new LocStrFormatted?((LocStrFormatted) Tr.Health__Tooltip), new Offset?(Offset.Top(5f)));
      this.Builder.AddTutorialIconForTitle(txt1, this.m_messagesCenter, IdsCore.Messages.TutorialOnHealth, true);
      this.m_healthTable.AppendTo<BalanceTable<Percent>>(topOf);
      txt1.SendToFront<Txt>();
      Txt txt2 = this.Builder.AddSectionTitle(topOf, (LocStrFormatted) TrCore.Unity, new LocStrFormatted?((LocStrFormatted) TrCore.Unity__Tooltip), new Offset?(Offset.Top(5f)));
      this.Builder.AddTutorialIconForTitle(txt2, this.m_messagesCenter, IdsCore.Messages.TutorialOnPopsAndUnity, true);
      this.m_unityTable.AppendTo<BalanceTable<Upoints>>(topOf);
      txt2.SendToFront<Txt>();
      updaterBuilder1.Observe<int>((Func<int>) (() => this.m_settlementsManager.TotalHousingCapacity)).Observe<int>((Func<int>) (() => this.m_settlementsManager.GetTotalPopulation())).Do((Action<int, int>) ((capacity, occupants) =>
      {
        occupantsView.First.SetPrefixText(occupants.ToString());
        occupantsView.Second.SetPrefixText(capacity.ToString());
        ColorRgba color2 = occupants > capacity ? this.Builder.Style.Global.DangerClr : this.Builder.Style.Global.Text.Color;
        occupantsView.First.SetColor(color2);
        occupantsView.Second.SetColor(color2);
      }));
      updaterBuilder1.Observe<int>((Func<int>) (() => this.m_settlementsManager.SettlementsCount)).Do((Action<int>) (count => amountOfSettlements.SetText(Tr.NumberOfSettlements.Format(count.ToString(), count))));
      updaterBuilder1.Observe<Upoints>((Func<Upoints>) (() => this.m_upointsManager.TotalUnityCap)).Do((Action<Upoints>) (unityCapVal => unityCap.SetPrefixText(unityCapVal.ToString())));
      this.AddUpdater(updaterBuilder1.Build());
      Dict<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry> upointsIncomes = new Dict<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry>();
      Dict<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry> upointsDemands = new Dict<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry>();
      StringBuilder sb = new StringBuilder();
      UpdaterBuilder updaterBuilder2 = UpdaterBuilder.Start();
      updaterBuilder2.Observe<UpointsStats.Entry>((Func<IIndexable<UpointsStats.Entry>>) (() => this.m_upointsManager.Stats.ThisMonthRecords), (ICollectionComparator<UpointsStats.Entry, IIndexable<UpointsStats.Entry>>) CompareFixedOrder<UpointsStats.Entry>.Instance).Do((Action<Lyst<UpointsStats.Entry>>) (entries =>
      {
        this.m_unityTable.StartBatchEdits();
        foreach (SettlementSummaryWindow.UpointsEntry upointsEntry in upointsIncomes.Values)
          upointsEntry.Reset();
        foreach (SettlementSummaryWindow.UpointsEntry upointsEntry in upointsDemands.Values)
          upointsEntry.Reset();
        foreach (UpointsStats.Entry entry in entries)
        {
          if (!entry.Category.IsOneTimeAction)
          {
            if (entry.Max.IsNegative)
              upointsDemands.GetOrAdd<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry>(entry.Category, (Func<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry>) (cat => new SettlementSummaryWindow.UpointsEntry(cat))).Add(entry);
            else
              upointsIncomes.GetOrAdd<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry>(entry.Category, (Func<UpointsCategoryProto, SettlementSummaryWindow.UpointsEntry>) (cat => new SettlementSummaryWindow.UpointsEntry(cat))).Add(entry);
          }
        }
        foreach (SettlementSummaryWindow.UpointsEntry upointsEntry in upointsIncomes.Values)
        {
          if (upointsEntry.Count != 0)
            this.m_unityTable.AddItem(upointsEntry.GetName(), upointsEntry.Exchanged, upointsEntry.Max, upointsEntry.Category.Graphics.IconPath, upointsEntry.GetTooltip(sb), true);
        }
        foreach (SettlementSummaryWindow.UpointsEntry upointsEntry in upointsDemands.Values)
        {
          if (upointsEntry.Count != 0)
            this.m_unityTable.AddItem(upointsEntry.GetName(), upointsEntry.Exchanged, upointsEntry.Max, upointsEntry.Category.Graphics.IconPath, upointsEntry.GetTooltip(sb), false);
        }
        this.m_unityTable.FinishBatchEdits();
      }));
      Dict<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry> healthIncomes = new Dict<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry>();
      Dict<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry> healthDemands = new Dict<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry>();
      updaterBuilder2.Observe<HealthStatistics.Entry>((Func<IIndexable<HealthStatistics.Entry>>) (() => this.m_popsHealthManager.HealthStats.LastMonthRecords), (ICollectionComparator<HealthStatistics.Entry, IIndexable<HealthStatistics.Entry>>) CompareFixedOrder<HealthStatistics.Entry>.Instance).Do((Action<Lyst<HealthStatistics.Entry>>) (entries =>
      {
        this.m_healthTable.StartBatchEdits();
        foreach (SettlementSummaryWindow.HealthPointsEntry healthPointsEntry in healthIncomes.Values)
          healthPointsEntry.Reset();
        foreach (SettlementSummaryWindow.HealthPointsEntry healthPointsEntry in healthDemands.Values)
          healthPointsEntry.Reset();
        foreach (HealthStatistics.Entry entry in entries)
        {
          if (entry.Max.IsNegative)
            healthDemands.GetOrAdd<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry>(entry.Category, (Func<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry>) (cat => new SettlementSummaryWindow.HealthPointsEntry(cat))).Add(entry);
          else
            healthIncomes.GetOrAdd<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry>(entry.Category, (Func<HealthPointsCategoryProto, SettlementSummaryWindow.HealthPointsEntry>) (cat => new SettlementSummaryWindow.HealthPointsEntry(cat))).Add(entry);
        }
        foreach (SettlementSummaryWindow.HealthPointsEntry healthPointsEntry in healthIncomes.Values)
        {
          if (healthPointsEntry.Count != 0)
            this.m_healthTable.AddItem(healthPointsEntry.GetName(), healthPointsEntry.Exchanged, healthPointsEntry.Max, healthPointsEntry.Category.Graphics.IconPath, "", true);
        }
        foreach (SettlementSummaryWindow.HealthPointsEntry healthPointsEntry in healthDemands.Values)
        {
          if (healthPointsEntry.Count != 0)
            this.m_healthTable.AddItem(healthPointsEntry.GetName(), healthPointsEntry.Exchanged, healthPointsEntry.Max, healthPointsEntry.Category.Graphics.IconPath, "", false);
        }
        this.m_healthTable.FinishBatchEdits();
      }));
      Dict<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry> bornIncomes = new Dict<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry>();
      Dict<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry> bornDemands = new Dict<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry>();
      updaterBuilder2.Observe<BirthStatistics.Entry>((Func<IIndexable<BirthStatistics.Entry>>) (() => this.m_popsHealthManager.BirthStats.LastMonthRecords), (ICollectionComparator<BirthStatistics.Entry, IIndexable<BirthStatistics.Entry>>) CompareFixedOrder<BirthStatistics.Entry>.Instance).Do((Action<Lyst<BirthStatistics.Entry>>) (entries =>
      {
        this.m_birthTable.StartBatchEdits();
        foreach (SettlementSummaryWindow.BirthRateEntry birthRateEntry in bornIncomes.Values)
          birthRateEntry.Reset();
        foreach (SettlementSummaryWindow.BirthRateEntry birthRateEntry in bornDemands.Values)
          birthRateEntry.Reset();
        foreach (BirthStatistics.Entry entry in entries)
        {
          if (entry.Max.IsNegative)
            bornDemands.GetOrAdd<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry>(entry.Category, (Func<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry>) (cat => new SettlementSummaryWindow.BirthRateEntry(cat))).Add(entry);
          else
            bornIncomes.GetOrAdd<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry>(entry.Category, (Func<BirthRateCategoryProto, SettlementSummaryWindow.BirthRateEntry>) (cat => new SettlementSummaryWindow.BirthRateEntry(cat))).Add(entry);
        }
        foreach (SettlementSummaryWindow.BirthRateEntry birthRateEntry in bornIncomes.Values)
        {
          if (birthRateEntry.Count != 0)
            this.m_birthTable.AddItem(birthRateEntry.GetName(), birthRateEntry.Exchanged, birthRateEntry.Max, birthRateEntry.Category.Graphics.IconPath, "", true);
        }
        foreach (SettlementSummaryWindow.BirthRateEntry birthRateEntry in bornDemands.Values)
        {
          if (birthRateEntry.Count != 0)
            this.m_birthTable.AddItem(birthRateEntry.GetName(), birthRateEntry.Exchanged, birthRateEntry.Max, birthRateEntry.Category.Graphics.IconPath, "", false);
        }
        this.m_birthTable.FinishBatchEdits();
      }));
      this.AddUpdater(updaterBuilder2.Build());

      static string upointsStringFunc(Upoints unity)
      {
        if (unity.IsNegative)
          return unity.FormatForceDigits();
        return unity.IsPositive ? "+" + unity.FormatForceDigits() : Upoints.Zero.FormatForceDigits();
      }

      static Upoints upointsSumFunc(Upoints first, Upoints second) => first + second;

      static bool upointsIsAboveZero(Upoints value) => value.IsPositive;

      static string healthStringFunc(Percent val)
      {
        if (val.IsNegative)
          return val.ToIntPercentRounded().ToString();
        return val.IsPositive ? string.Format("+{0}", (object) val.ToIntPercentRounded()) : 0.ToString();
      }

      static string percentStringFunc(Percent val)
      {
        if (val.IsNegative)
          return val.ToStringRounded(2);
        return val.IsPositive ? "+" + val.ToStringRounded(2) : Percent.Zero.ToStringRounded(2);
      }

      static Percent percentSumFunc(Percent first, Percent second) => first + second;

      static bool percentIsAboveZero(Percent value) => value.IsPositive;
    }

    private class UpointsEntry
    {
      public readonly UpointsCategoryProto Category;
      public Upoints Exchanged;
      public Upoints Max;
      public int Count;
      public readonly Lyst<UpointsStats.Entry> Entries;

      public UpointsEntry(UpointsCategoryProto category)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Entries = new Lyst<UpointsStats.Entry>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Category = category;
      }

      public void Reset()
      {
        this.Exchanged = Upoints.Zero;
        this.Max = Upoints.Zero;
        this.Count = 0;
        this.Entries.Clear();
      }

      public void Add(UpointsStats.Entry entry)
      {
        ++this.Count;
        this.Exchanged += entry.Exchanged;
        this.Max += entry.Max;
        this.Entries.Add(entry);
      }

      public string GetName()
      {
        return this.Category.HideCount || this.Count <= 1 ? this.Category.Title.TranslatedString : string.Format("{0}x {1}", (object) this.Count, (object) this.Category.Title.TranslatedString);
      }

      public string GetTooltip(StringBuilder sb)
      {
        sb.Clear();
        IEnumerable<IGrouping<string, UpointsStats.Entry>> groupings = this.Entries.GroupBy<UpointsStats.Entry, string>((Func<UpointsStats.Entry, string>) (x => x.Title.TranslatedString));
        bool flag = true;
        foreach (IGrouping<string, UpointsStats.Entry> grouping in groupings)
        {
          int num = 0;
          Upoints zero1 = Upoints.Zero;
          Upoints zero2 = Upoints.Zero;
          foreach (UpointsStats.Entry entry in (IEnumerable<UpointsStats.Entry>) grouping)
          {
            zero1 += entry.Exchanged;
            zero2 += entry.Max;
            ++num;
          }
          if (!flag)
            sb.AppendLine();
          flag = false;
          string str = this.Category.HideCount || num <= 1 ? grouping.Key : string.Format("{0}x {1}", (object) num, (object) grouping.Key);
          sb.Append(str);
          sb.Append(": ");
          sb.Append(zero1.FormatForceDigits());
          if (zero1 != zero2)
          {
            sb.Append(" / ");
            sb.Append(zero2.FormatForceDigits());
          }
        }
        return sb.ToString();
      }
    }

    private class HealthPointsEntry
    {
      public readonly HealthPointsCategoryProto Category;
      public Percent Exchanged;
      public Percent Max;
      public int Count;

      public HealthPointsEntry(HealthPointsCategoryProto category)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Category = category;
      }

      public void Reset()
      {
        this.Exchanged = Percent.Zero;
        this.Max = Percent.Zero;
        this.Count = 0;
      }

      public void Add(HealthStatistics.Entry entry)
      {
        ++this.Count;
        this.Exchanged += entry.Change;
        this.Max += entry.Max;
      }

      public string GetName() => this.Category.Title.TranslatedString;
    }

    private class BirthRateEntry
    {
      public readonly BirthRateCategoryProto Category;
      public Percent Exchanged;
      public Percent Max;
      public int Count;

      public BirthRateEntry(BirthRateCategoryProto category)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Category = category;
      }

      public void Reset()
      {
        this.Exchanged = Percent.Zero;
        this.Max = Percent.Zero;
        this.Count = 0;
      }

      public void Add(BirthStatistics.Entry entry)
      {
        ++this.Count;
        this.Exchanged += entry.Change;
        this.Max += entry.Max;
      }

      public string GetName() => this.Category.Title.TranslatedString;
    }
  }
}
