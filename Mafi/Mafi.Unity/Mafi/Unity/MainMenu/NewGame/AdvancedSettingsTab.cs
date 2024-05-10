// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.AdvancedSettingsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.World.Loans;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class AdvancedSettingsTab : Column, ITab
  {
    private static readonly Percent LABEL_WIDTH;
    private static readonly Px SECTION_GAP;
    private static readonly Px ICON_OFFSET;
    private readonly Option<GameDifficultyApplier> m_difficultyApplier;
    private readonly Option<NewGameConfigForUi> m_newGameDifficultyConfig;

    public GameDifficultyConfig Config { get; private set; }

    public AdvancedSettingsTab(GameDifficultyApplier applier)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_difficultyApplier = (Option<GameDifficultyApplier>) applier;
      this.Config = applier.DifficultyConfig.Clone();
    }

    public AdvancedSettingsTab(NewGameConfigForUi newGameDifficulty)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_newGameDifficultyConfig = (Option<NewGameConfigForUi>) newGameDifficulty;
      this.Config = this.m_newGameDifficultyConfig.Value.DifficultyConfig;
    }

    public void ResetConfig()
    {
      if (this.m_difficultyApplier.IsNone)
      {
        Log.Error("No applier set!");
      }
      else
      {
        this.Config = this.m_difficultyApplier.Value.DifficultyConfig.Clone();
        ((ITab) this).Activate();
      }
    }

    void ITab.Activate()
    {
      if (this.m_newGameDifficultyConfig.HasValue)
        this.Config = this.m_newGameDifficultyConfig.Value.DifficultyConfig;
      this.AlignItemsStretch<AdvancedSettingsTab>();
      this.Padding<AdvancedSettingsTab>(1.pt());
      UiComponent[] uiComponentArray = new UiComponent[1];
      Row component = new Row(6.pt());
      component.Add<Row>((Action<Row>) (c => c.AlignItemsStretch<Row>().Fill<Row>()));
      component.Add(column(new UiComponent[3]
      {
        section((LocStrFormatted) Tr.Resources, new UiComponent[3]
        {
          setting<Percent>(GameDifficultyConfig.ExtraStartingMaterialInfo),
          setting<Percent>(GameDifficultyConfig.ResourceMiningDiffInfo),
          setting<Percent>(GameDifficultyConfig.WorldMinesReservesInfo)
        }),
        section((LocStrFormatted) Tr.Mechanics, new UiComponent[1]
        {
          setting<GameDifficultyConfig.OreSortingSetting>(GameDifficultyConfig.OreSortingInfo)
        }),
        section((LocStrFormatted) Tr.Costs, new UiComponent[6]
        {
          setting<Percent>(GameDifficultyConfig.ConstructionCostsDiffInfo),
          setting<GameDifficultyConfig.DeconstructionRefundSetting>(GameDifficultyConfig.DeconstructionRefundInfo),
          setting<Percent>(GameDifficultyConfig.ResearchCostDiffInfo),
          setting<Percent>(GameDifficultyConfig.MaintenanceDiffInfo),
          setting<Percent>(GameDifficultyConfig.FuelConsumptionDiffInfo),
          setting<Percent>(GameDifficultyConfig.SettlementConsumptionDiffInfo)
        })
      }));
      component.Add(column(new UiComponent[2]
      {
        section((LocStrFormatted) Tr.FailureOutages, new UiComponent[7]
        {
          setting<GameDifficultyConfig.VehiclesNoFuelSetting>(GameDifficultyConfig.VehiclesNoFuelInfo),
          setting<GameDifficultyConfig.ShipNoFuelSetting>(GameDifficultyConfig.ShipsNoFuelInfo),
          setting<GameDifficultyConfig.GroundwaterPumpLowSetting>(GameDifficultyConfig.GroundwaterPumpLowInfo),
          setting<GameDifficultyConfig.WorldMinesNoUnitySetting>(GameDifficultyConfig.WorldMinesNoUnityInfo),
          setting<GameDifficultyConfig.PowerLowSetting>(GameDifficultyConfig.PowerLowInfo),
          setting<GameDifficultyConfig.ComputingLowSetting>(GameDifficultyConfig.ComputingLowInfo),
          setting<GameDifficultyConfig.ConsumerBrokenSetting>(GameDifficultyConfig.ConsumerBrokenInfo)
        }),
        section((LocStrFormatted) Tr.Population, new UiComponent[4]
        {
          setting<GameDifficultyConfig.StarvationSetting>(GameDifficultyConfig.StarvationInfo),
          setting<Percent>(GameDifficultyConfig.DiseaseMortalityDiffInfo),
          setting<Percent>(GameDifficultyConfig.BaseHealthDiffInfo),
          setting<Percent>(GameDifficultyConfig.PollutionDiffInfo)
        })
      }));
      component.Add(column(new UiComponent[4]
      {
        section((LocStrFormatted) Tr.Power, new UiComponent[2]
        {
          setting<GameDifficultyConfig.LogisticsPowerSetting>(GameDifficultyConfig.PowerSettingInfo),
          setting<Percent>(GameDifficultyConfig.SolarPowerDiffInfo)
        }),
        section((LocStrFormatted) Tr.Economy, new UiComponent[2]
        {
          setting<Percent>(GameDifficultyConfig.ExtraContractsProfitInfo),
          setting<LoansDifficulty>(GameDifficultyConfig.LoansDifficultyInfo)
        }),
        section((LocStrFormatted) Tr.Nature, new UiComponent[4]
        {
          setting<GameDifficultyConfig.WeatherDifficultySetting>(GameDifficultyConfig.WeatherDifficultyInfo),
          setting<Percent>(GameDifficultyConfig.RainYieldDiffInfo),
          setting<Percent>(GameDifficultyConfig.FarmYieldInfo),
          setting<Percent>(GameDifficultyConfig.TreesGrowthInfo)
        }),
        section((LocStrFormatted) TrCore.Unity, new UiComponent[3]
        {
          setting<Percent>(GameDifficultyConfig.UnityProductionDiffInfo),
          setting<GameDifficultyConfig.QuickRepairSetting>(GameDifficultyConfig.QuickRepairInfo),
          setting<Percent>(GameDifficultyConfig.QuickActionsCostInfo)
        })
      }));
      uiComponentArray[0] = (UiComponent) component;
      this.SetChildren(uiComponentArray);

      static UiComponent column(UiComponent[] children)
      {
        Column component = new Column(AdvancedSettingsTab.SECTION_GAP);
        component.Add<Column>((Action<Column>) (c => c.Fill<Column>().AlignItemsStretch<Column>().Width<Column>(33.Percent())));
        component.Add((IEnumerable<UiComponent>) children);
        return (UiComponent) component;
      }

      static UiComponent section(LocStrFormatted title, UiComponent[] children)
      {
        Column component = new Column(2.pt());
        component.Add<Column>((Action<Column>) (c => c.PaddingLeft<Column>(4.pt()).AlignItemsStretch<Column>()));
        component.Add((UiComponent) new Title(title).MarginLeft<Title>(-4.pt()));
        component.Add((IEnumerable<UiComponent>) children);
        return (UiComponent) component;
      }

      UiComponent setting<T>(DiffSettingInfo<T> setting)
      {
        RelGameDate lockedTimeRemaining = this.getLockedTimeRemaining((IDiffSettingInfo) setting);
        Dropdown<T> component1 = new Dropdown<T>();
        Icon changed = (Icon) null;
        component1.Label<Dropdown<T>>(setting.Title).Tooltip<Dropdown<T>>(new LocStrFormatted?(setting.Tooltip)).LabelWidth<Dropdown<T>>(AdvancedSettingsTab.LABEL_WIDTH).SetOptionViewFactory((Func<T, int, bool, UiComponent>) ((opt, idx, _) =>
        {
          PropDifficultyRating difficultyRating = setting.GetDifficultyRating(opt);
          ColorRgba colorRgba1 = Theme.Text;
          ColorRgba colorRgba2;
          switch (difficultyRating)
          {
            case PropDifficultyRating.Easy:
              colorRgba2 = Theme.PositiveColor;
              break;
            case PropDifficultyRating.Standard:
label_4:
              return (UiComponent) new Label(setting.CreateLabel(opt).AsLoc()).Color<Label>(new ColorRgba?(colorRgba1));
            default:
              colorRgba2 = Theme.NegativeColor;
              break;
          }
          colorRgba1 = colorRgba2;
          goto label_4;
        })).SetOptions((IEnumerable<T>) setting.Options).SetValue(setting.GetValue(this.Config)).OnValueChanged((Action<T, int>) ((opt, idx) =>
        {
          setting.SetValue(this.Config, opt);
          updateChanged();
        })).Enabled<Dropdown<T>>(lockedTimeRemaining.IsNotPositive);
        if (lockedTimeRemaining.IsPositive)
        {
          Dropdown<T> dropdown = component1;
          Icon component2 = new Icon("Assets/Unity/UserInterface/General/Locked128.png").Tooltip<Icon>(new LocStrFormatted?(Tr.LockedFor__Tooltip.Format(lockedTimeRemaining.FormatYearsOrMonthsLong()))).Small();
          Px? nullable = new Px?(AdvancedSettingsTab.ICON_OFFSET);
          Px? top = new Px?();
          Px? right = new Px?();
          Px? bottom = new Px?();
          Px? left = nullable;
          Icon child = component2.AbsolutePosition<Icon>(top, right, bottom, left);
          dropdown.Add((UiComponent) child);
        }
        else if (this.m_difficultyApplier.HasValue)
        {
          changed = new Icon("Assets/Unity/UserInterface/General/Rename.svg");
          changed.Small().Color<Icon>(new ColorRgba?(Theme.PrimaryColor));
          Dropdown<T> dropdown = component1;
          Icon component3 = changed;
          Px? nullable = new Px?(AdvancedSettingsTab.ICON_OFFSET);
          Px? top = new Px?();
          Px? right = new Px?();
          Px? bottom = new Px?();
          Px? left = nullable;
          Icon child = component3.AbsolutePosition<Icon>(top, right, bottom, left);
          dropdown.Add((UiComponent) child);
          updateChanged();
        }
        return (UiComponent) component1;

        void updateChanged()
        {
          if (this.m_difficultyApplier.IsNone)
            return;
          changed.Visible<Icon>(!setting.AreSame(this.Config, this.m_difficultyApplier.Value.DifficultyConfig));
        }
      }
    }

    void ITab.Deactivate()
    {
    }

    private RelGameDate getLockedTimeRemaining(IDiffSettingInfo setting)
    {
      if (this.m_difficultyApplier.IsNone)
        return RelGameDate.Zero;
      GameDate? lastChangeFor = this.m_difficultyApplier.Value.ChangeLog.GetLastChangeFor(setting);
      return !lastChangeFor.HasValue ? RelGameDate.Zero : GameDifficultyApplier.DifficultyChangeTimeout - (this.m_difficultyApplier.Value.CurrentDate - lastChangeFor.Value);
    }

    static AdvancedSettingsTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AdvancedSettingsTab.LABEL_WIDTH = 55.Percent();
      AdvancedSettingsTab.SECTION_GAP = 4.pt();
      AdvancedSettingsTab.ICON_OFFSET = -22.px();
    }
  }
}
