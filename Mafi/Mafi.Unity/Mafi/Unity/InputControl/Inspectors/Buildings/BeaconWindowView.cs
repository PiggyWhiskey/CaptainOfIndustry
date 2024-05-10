// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.BeaconWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Beacons;
using Mafi.Core.Population;
using Mafi.Core.Population.Refugees;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class BeaconWindowView : StaticEntityInspectorBase<Beacon>
  {
    private readonly RefugeesManager m_refugeesManager;
    private readonly BeaconInspector m_controller;
    private StatusPanel m_statusInfo;

    protected override Beacon Entity => this.m_controller.SelectedEntity;

    public BeaconWindowView(BeaconInspector controller, RefugeesManager refugeesManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_refugeesManager = refugeesManager;
      this.m_controller = controller.CheckNotNull<BeaconInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddUnityCostPanel(updaterBuilder, (Func<IUnityConsumingEntity>) (() => (IUnityConsumingEntity) this.Entity));
      this.m_statusInfo = this.AddStatusInfoPanel();
      Panel parent = this.Builder.NewPanel("Container").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(60f), Offset.Top(10f));
      Txt info = this.Builder.NewTxt("").SetTextStyle(this.Builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleCenter).PutToTopOf<Txt>((IUiElement) parent, 40f);
      this.Builder.NewTxt("StopInfo").SetText((LocStrFormatted) Tr.Beacon__Notice).SetTextStyle(this.Builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) parent, 40f);
      updaterBuilder.Observe<Option<RefugeesReward>>((Func<Option<RefugeesReward>>) (() => this.m_refugeesManager.NextReward)).Observe<int?>((Func<int?>) (() =>
      {
        Duration? durationLeft = this.m_refugeesManager.DurationLeft;
        ref Duration? local = ref durationLeft;
        return !local.HasValue ? new int?() : new int?(local.GetValueOrDefault().Months.ToIntCeiled());
      })).Observe<BeaconWindowView.BeaconState>((Func<BeaconWindowView.BeaconState>) (() =>
      {
        BeaconWindowView.BeaconState beaconState;
        if (this.Entity.IsPaused)
          beaconState = BeaconWindowView.BeaconState.Paused;
        else if (!((IEntityWithWorkers) this.Entity).HasWorkersCached)
          beaconState = BeaconWindowView.BeaconState.NoWorkers;
        else if (this.Entity.NotEnoughPower)
        {
          beaconState = BeaconWindowView.BeaconState.NoPower;
        }
        else
        {
          UnityConsumer valueOrNull = this.Entity.UnityConsumer.ValueOrNull;
          beaconState = (valueOrNull != null ? (valueOrNull.NotEnoughUnity ? 1 : 0) : 0) == 0 ? BeaconWindowView.BeaconState.Working : BeaconWindowView.BeaconState.NoUnity;
        }
        return beaconState;
      })).Do((Action<Option<RefugeesReward>, int?, BeaconWindowView.BeaconState>) ((nextRewardMaybe, monthsLeft, state) =>
      {
        switch (state)
        {
          case BeaconWindowView.BeaconState.Paused:
            this.m_statusInfo.SetStatusPaused();
            info.SetText("");
            break;
          case BeaconWindowView.BeaconState.NoWorkers:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case BeaconWindowView.BeaconState.NoUnity:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__NoUnity, StatusPanel.State.Critical);
            break;
          case BeaconWindowView.BeaconState.NoPower:
            this.m_statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          default:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Working);
            if (nextRewardMaybe.IsNone)
            {
              info.SetText((LocStrFormatted) Tr.Beacon__NoMoreRefugees);
              break;
            }
            Assert.That<int?>(monthsLeft).HasValue<int>();
            RefugeesReward refugeesReward = nextRewardMaybe.Value;
            int? nullable = monthsLeft;
            int num = 1;
            if (nullable.GetValueOrDefault() <= num & nullable.HasValue || !monthsLeft.HasValue)
              monthsLeft = new int?(1);
            info.SetText(Tr.Beacon__Status.Format(refugeesReward.AmountOfRefugees.ToString(), refugeesReward.AmountOfRefugees).ToString() + " " + TrCore.NumberOfMonths.Format(monthsLeft.ToString(), monthsLeft.Value).ToString());
            break;
        }
      }));
      this.AddUpdater(updaterBuilder.Build());
    }

    private enum BeaconState
    {
      Paused,
      NoWorkers,
      NoUnity,
      NoPower,
      Working,
    }
  }
}
