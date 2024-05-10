// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.EntityInspectorBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
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
  /// <summary>Handles pausing, and title changes.</summary>
  public abstract class EntityInspectorBase<T> : ItemDetailWindowView where T : IEntity
  {
    private readonly InspectorContext m_inspectorContext;
    private Option<BufferView> m_maintenanceBuffer;

    protected abstract T Entity { get; }

    protected EntityInspectorBase(IEntityInspector inspector)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(typeof (T).Name + "Inspector");
      this.m_inspectorContext = inspector.Context;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      if (typeof (T).IsAssignableTo(typeof (IEntityWithCustomTitle)))
        this.AddTitleRenameButton((Func<IEntityWithCustomTitle>) (() => (IEntityWithCustomTitle) (object) this.Entity), this.m_inspectorContext.InputScheduler);
      Tooltip tooltip = this.AddHelpButton();
      updaterBuilder.Observe<EntityProto>((Func<EntityProto>) (() => this.Entity.Prototype)).Do((Action<EntityProto>) (proto => tooltip.SetText((LocStrFormatted) proto.Strings.DescShort)));
      this.AddEnableToggleButton((Action<bool>) (isNotPaused =>
      {
        Assert.That<bool>(isNotPaused).IsNotEqualTo<bool>(this.Entity.IsNotPaused(), "Paused state mismatch.");
        this.m_inspectorContext.InputScheduler.ScheduleInputCmd<ToggleEnabledCmd>(new ToggleEnabledCmd((IEntity) this.Entity));
      }), updaterBuilder, (Func<bool>) (() => !this.Entity.IsPaused));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.CanBePaused)).Do(new Action<bool>(((ItemDetailWindowView) this).SetEnableButtonVisibility));
      updaterBuilder.Observe<string>((Func<string>) (() => this.Entity.GetTitle())).Observe<bool>((Func<bool>) (() => this.Entity.IsPaused)).Observe<bool>((Func<bool>) (() => this.Entity is IMaintainedEntity entity && entity.Maintenance.Status.IsBroken)).Do((Action<string, bool, bool>) ((title, isPaused, isBroken) =>
      {
        this.SetTitle(title, isPaused, isBroken);
        this.m_headerText.SetWidth<Txt>(this.m_headerText.GetPreferedWidth().Min(this.m_headerHolder.GetWidth() - 22f));
      }));
      if (typeof (T).IsAssignableTo(typeof (IEntityWithWorkers)))
        this.AddWorkersPanel(updaterBuilder, (Func<IEntityWithWorkers>) (() => (IEntityWithWorkers) (object) this.Entity));
      if (typeof (T).IsAssignableTo(typeof (IElectricityConsumingEntity)))
        this.AddElectricityInfoPanel(updaterBuilder, (Func<IElectricityConsumingEntity>) (() => (IElectricityConsumingEntity) (object) this.Entity));
      if (typeof (T).IsAssignableTo(typeof (IComputingConsumingEntity)))
        this.AddComputingInfoPanel(updaterBuilder, (Func<IComputingConsumingEntity>) (() => (IComputingConsumingEntity) (object) this.Entity));
      if (typeof (T).IsAssignableTo(typeof (IEntityWithGeneralPriority)))
        this.AddGeneralPriorityPanel(this.m_inspectorContext, (Func<IEntityWithGeneralPriority>) (() => (IEntityWithGeneralPriority) (object) this.Entity));
      this.SetupAlertsIndicator(updaterBuilder, this.m_inspectorContext, (Func<IEntity>) (() => (IEntity) this.Entity));
      this.AddUpdater(updaterBuilder.Build());
    }

    protected override void AddCustomItemsEnd(StackContainer itemContainer)
    {
      base.AddCustomItemsEnd(itemContainer);
      if (!typeof (T).IsAssignableTo(typeof (IMaintainedEntity)))
        return;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_maintenanceBuffer = (Option<BufferView>) this.AddMaintenancePanel(updaterBuilder, (Func<IMaintainedEntity>) (() => (IMaintainedEntity) (object) this.Entity), (Action<IMaintainedEntity>) (e => this.m_inspectorContext.InputScheduler.ScheduleInputCmd<QuickRepairEntityCmd>(new QuickRepairEntityCmd(e.Id)))).Second;
      this.AddUpdater(updaterBuilder.Build());
    }

    protected override void SetWidth(float width)
    {
      base.SetWidth(width);
      if (!this.m_maintenanceBuffer.HasValue)
        return;
      this.m_maintenanceBuffer.Value.UpdateSize();
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.InputUpdateForRenaming();
    }
  }
}
