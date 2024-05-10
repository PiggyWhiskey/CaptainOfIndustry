// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.Settlements.SettlementWasteModuleWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings.Settlements
{
  internal class SettlementWasteModuleWindowView : StaticEntityInspectorBase<SettlementWasteModule>
  {
    private readonly SettlementWasteModuleInspector m_controller;

    protected override SettlementWasteModule Entity => this.m_controller.SelectedEntity;

    public SettlementWasteModuleWindowView(SettlementWasteModuleInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<SettlementWasteModuleInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      this.AddStorageLogisticsPanel(updaterBuilder, (Func<IEntityWithSimpleLogisticsControl>) (() => (IEntityWithSimpleLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Collected);
      BufferView buffer = this.Builder.NewBufferView((IUiElement) itemContainer).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Prototype.ProductAccepted)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.StoredQuantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => buffer.UpdateState(product, capacity, quantity)));
      updaterBuilder.Observe<SettlementWasteModule.State>((Func<SettlementWasteModule.State>) (() => this.Entity.CurrentState)).Do((Action<SettlementWasteModule.State>) (state =>
      {
        switch (state)
        {
          case SettlementWasteModule.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case SettlementWasteModule.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case SettlementWasteModule.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case SettlementWasteModule.State.FullOutput:
            statusInfo.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Critical);
            break;
        }
      }));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
