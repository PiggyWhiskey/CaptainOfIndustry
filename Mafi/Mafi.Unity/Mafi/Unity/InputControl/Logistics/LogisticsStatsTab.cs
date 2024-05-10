// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Logistics.LogisticsStatsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Logistics
{
  internal class LogisticsStatsTab : Tab, IDynamicSizeElement, IUiElement
  {
    private readonly VehicleJobStatsManager m_jobStatsManager;
    private JobStatsView m_jobStatsView;

    public event Action<IUiElement> SizeChanged;

    internal LogisticsStatsTab(VehicleJobStatsManager jobStatsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("StatsTab");
      this.m_jobStatsManager = jobStatsManager;
    }

    protected override void BuildUi()
    {
      UpdaterBuilder updater = UpdaterBuilder.Start();
      StackContainer topOf = this.Builder.NewStackContainer("container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Bottom(10f)).PutToTopOf<StackContainer>((IUiElement) this, 0.0f);
      topOf.SizeChanged += (Action<IUiElement>) (x =>
      {
        this.SetHeight<LogisticsStatsTab>(x.GetHeight());
        Action<IUiElement> sizeChanged = this.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) this);
      });
      this.Builder.AddSectionTitle(topOf, (LocStrFormatted) Tr.TrucksStats__Title);
      this.Builder.AddSwitch(topOf, Tr.TrucksStats__OptionGeneral.TranslatedString, (Action<bool>) (x => this.m_jobStatsView.IncludeGeneralJobs = !this.m_jobStatsView.IncludeGeneralJobs), updater, (Func<bool>) (() => this.m_jobStatsView.IncludeGeneralJobs), Tr.TrucksStats__OptionGeneralTooltip.TranslatedString);
      this.Builder.AddSwitch(topOf, Tr.TrucksStats__OptionMining.TranslatedString, (Action<bool>) (x => this.m_jobStatsView.IncludeMiningJobs = !this.m_jobStatsView.IncludeMiningJobs), updater, (Func<bool>) (() => this.m_jobStatsView.IncludeMiningJobs), Tr.TrucksStats__OptionMiningTooltip.TranslatedString);
      this.Builder.AddSwitch(topOf, Tr.TrucksStats__OptionRefueling.TranslatedString, (Action<bool>) (x => this.m_jobStatsView.IncludeRefuelingJobs = !this.m_jobStatsView.IncludeRefuelingJobs), updater, (Func<bool>) (() => this.m_jobStatsView.IncludeRefuelingJobs), Tr.TrucksStats__OptionRefuelingTooltip.TranslatedString);
      this.m_jobStatsView = new JobStatsView((IUiElement) topOf, this.Builder, this.m_jobStatsManager);
      this.m_jobStatsView.AppendTo<JobStatsView>(topOf, new float?(0.0f));
      Dropdwn rightTopOf = this.Builder.NewDropdown("Dropdown", (IUiElement) this.m_jobStatsView).AddOptions(((IEnumerable<int>) JobStatsView.MonthRanges).Select<int, string>((Func<int, string>) (x => Tr.StatsRange__Months.Format(x.ToString(), x).Value)).ToList<string>()).PutToRightTopOf<Dropdwn>((IUiElement) this.m_jobStatsView, new Vector2(200f, 28f), Offset.Top(-33f) + Offset.Right(10f));
      rightTopOf.SetValueWithoutNotify(Array.IndexOf<int>(JobStatsView.MonthRanges, this.m_jobStatsView.NumberOfMonthsToShow));
      rightTopOf.OnValueChange((Action<int>) (i => this.m_jobStatsView.NumberOfMonthsToShow = JobStatsView.MonthRanges[i]));
      this.AddUpdater(updater.Build());
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      this.m_jobStatsView.RenderUpdate(gameTime);
      base.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      this.m_jobStatsView.SyncUpdate(gameTime);
      base.SyncUpdate(gameTime);
    }
  }
}
