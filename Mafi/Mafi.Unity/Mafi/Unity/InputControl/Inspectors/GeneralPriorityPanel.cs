// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.GeneralPriorityPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Commands;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
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
  public class GeneralPriorityPanel : BasePriorityPanel
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly Func<IEntityWithGeneralPriority> m_provider;

    public GeneralPriorityPanel(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IEntityWithGeneralPriority> provider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(parent, builder, 14, Tr.Priority.TranslatedString);
      GeneralPriorityPanel generalPriorityPanel = this;
      this.m_inputScheduler = inputScheduler;
      this.m_provider = provider;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() => provider().IsCargoAffectedByGeneralPriority)).Do((Action<bool>) (affectsCargo => generalPriorityPanel.SetTooltip((LocStrFormatted) (affectsCargo ? Tr.PriorityGeneral__TooltipWithCargo : Tr.PriorityGeneral__Tooltip))));
      Panel panel = builder.NewPanel("Options");
      SwitchBtn surplusMode = builder.NewSwitchBtn().SetText((LocStrFormatted) Tr.ConsumeSurplusPower__Toggle).SetOnToggleAction((Action<bool>) (isOn => generalPriorityPanel.m_inputScheduler.ScheduleInputCmd<SetIsElectricitySurplusConsumerCmd>(new SetIsElectricitySurplusConsumerCmd((IEntity) generalPriorityPanel.m_provider(), isOn)))).AddTooltip((LocStrFormatted) Tr.ConsumeSurplusPower__Tooltip).PutTo<SwitchBtn>((IUiElement) panel);
      panel.SetSize<Panel>(new Vector2(surplusMode.GetWidth() + 10f, 25f));
      this.AddOptionsPanel((IUiElement) panel);
      updaterBuilder.Observe<IElectricityConsumingEntity>((Func<IElectricityConsumingEntity>) (() => generalPriorityPanel.m_provider() as IElectricityConsumingEntity)).Do((Action<IElectricityConsumingEntity>) (elConsumingEntity =>
      {
        closure_0.SetOptionBtnVisibility(elConsumingEntity != null && elConsumingEntity.ElectricityConsumer.HasValue && elConsumingEntity.PowerRequired.IsPositive);
        if (elConsumingEntity == null)
          return;
        IElectricityConsumerReadonly valueOrNull = elConsumingEntity.ElectricityConsumer.ValueOrNull;
        bool flag = valueOrNull != null && valueOrNull.IsSurplusConsumer;
        surplusMode.SetIsOn(flag);
        closure_0.SetHasOptionsSet(flag);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    protected override void OnValueChanged(int index)
    {
      this.m_inputScheduler.ScheduleInputCmd<SetGeneralPriorityCmd>(new SetGeneralPriorityCmd(this.m_provider().Id, index));
    }

    protected override int GetCurrentPriority() => this.m_provider().GeneralPriority;

    protected override bool IsPrioritySupported() => this.m_provider().IsGeneralPriorityVisible;
  }
}
