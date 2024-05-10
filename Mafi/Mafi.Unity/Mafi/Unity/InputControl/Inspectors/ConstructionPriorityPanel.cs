// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.ConstructionPriorityPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Commands;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  public class ConstructionPriorityPanel : BasePriorityPanel
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly Func<Option<IEntityConstructionProgress>> m_progressProvider;

    public ConstructionPriorityPanel(
      IUiElement parent,
      IInputScheduler inputScheduler,
      GlobalPrioritiesManager prioritiesManager,
      UiBuilder builder,
      Func<Option<IEntityConstructionProgress>> progressProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(parent, builder, 14, Tr.Priority.TranslatedString);
      ConstructionPriorityPanel constructionPriorityPanel = this;
      this.m_inputScheduler = inputScheduler;
      this.m_progressProvider = progressProvider;
      this.SetCustomIcon("Assets/Unity/UserInterface/General/Build.svg");
      string lastSeenPriorityType = "";
      int lastSeenPriorityValue = 0;
      Btn makeDefaultBtn = builder.NewBtnGeneral("MakeDefault").SetText((LocStrFormatted) Tr.MakeDefault).OnClick((Action) (() => closure_0.m_inputScheduler.ScheduleInputCmd<SetGlobalPriorityCmd>(new SetGlobalPriorityCmd(lastSeenPriorityType, lastSeenPriorityValue))));
      Tooltip btnTooltip = builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) makeDefaultBtn);
      this.AddButton(makeDefaultBtn);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() =>
      {
        IEntityConstructionProgress valueOrNull = progressProvider().ValueOrNull;
        return valueOrNull != null && valueOrNull.IsDeconstruction;
      })).Observe<bool>((Func<bool>) (() =>
      {
        IEntityConstructionProgress valueOrNull = progressProvider().ValueOrNull;
        return valueOrNull != null && valueOrNull.IsPriority;
      })).Observe<int>(new Func<int>(((BasePriorityPanel) this).GetCurrentPriority)).Observe<int>((Func<int>) (() => prioritiesManager.ConstructionPriority)).Observe<int>((Func<int>) (() => prioritiesManager.DeconstructionPriority)).Do((Action<bool, bool, int, int, int>) ((isDeconstruction, isPrioritized, priority, constrPriority, deconstrPriority) =>
      {
        lastSeenPriorityValue = priority;
        if (isPrioritized)
          makeDefaultBtn.SetEnabled(false);
        else if (isDeconstruction)
        {
          lastSeenPriorityType = "DeconstructionPriority";
          makeDefaultBtn.SetEnabled(priority != deconstrPriority);
        }
        else
        {
          lastSeenPriorityType = "ConstructionPriority";
          makeDefaultBtn.SetEnabled(priority != constrPriority);
        }
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() =>
      {
        IEntityConstructionProgress valueOrNull = progressProvider().ValueOrNull;
        return valueOrNull != null && valueOrNull.IsDeconstruction;
      })).Do((Action<bool>) (isDeconstruction =>
      {
        if (isDeconstruction)
        {
          closure_0.SetTooltip((LocStrFormatted) Tr.Priority__DeconstructionTooltip);
          btnTooltip.SetText((LocStrFormatted) Tr.MakeDefault__DeconstructionTooltip);
        }
        else
        {
          closure_0.SetTooltip((LocStrFormatted) Tr.Priority__ConstructionTooltip);
          btnTooltip.SetText((LocStrFormatted) Tr.MakeDefault__ConstructionTooltip);
        }
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    protected override void OnValueChanged(int index)
    {
      IEntityConstructionProgress valueOrNull = this.m_progressProvider().ValueOrNull;
      if (valueOrNull == null)
        return;
      this.m_inputScheduler.ScheduleInputCmd<SetConstructionPriorityCmd>(new SetConstructionPriorityCmd(valueOrNull.Entity, index));
    }

    protected override int GetCurrentPriority()
    {
      IEntityConstructionProgress valueOrNull = this.m_progressProvider().ValueOrNull;
      return valueOrNull == null ? 0 : valueOrNull.GetPriority();
    }

    protected override bool IsPrioritySupported() => this.m_progressProvider().HasValue;

    protected override bool IsReadonly()
    {
      IEntityConstructionProgress valueOrNull = this.m_progressProvider().ValueOrNull;
      return valueOrNull != null && valueOrNull.IsPriority;
    }
  }
}
