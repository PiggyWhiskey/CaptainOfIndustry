// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ElectricityGeneratorFromMechPowerView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Machines.PowerGenerators;
using Mafi.Core;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class ElectricityGeneratorFromMechPowerView : 
    StaticEntityInspectorBase<ElectricityGeneratorFromMechPower>
  {
    private readonly ElectricityGeneratorFromMechPowerInspector m_controller;
    private readonly VirtualProductProto m_electricityProto;
    private readonly VirtualProductProto m_mechPowerProto;
    private StatusPanel m_statusInfo;

    protected override ElectricityGeneratorFromMechPower Entity => this.m_controller.SelectedEntity;

    public ElectricityGeneratorFromMechPowerView(
      ElectricityGeneratorFromMechPowerInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller;
      this.m_electricityProto = controller.Context.ProtosDb.GetOrThrow<VirtualProductProto>((Proto.ID) IdsCore.Products.Electricity);
      this.m_mechPowerProto = controller.Context.ShaftManager.MechPowerProto;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddMaxElectricityOutputPanel(updaterBuilder, (Func<Electricity>) (() => this.Entity.Prototype.OutputElectricity));
      base.AddCustomItems(itemContainer);
      SingleRecipeObserver objectToPlace = new SingleRecipeObserver((IUiElement) this.ItemsContainer, this.Builder, (Option<RecipesBookController>) this.m_controller.Context.RecipesBookController, (Func<Option<IRecipeForUi>>) (() => (Option<IRecipeForUi>) (IRecipeForUi) this.Entity.Prototype));
      objectToPlace.AppendTo<SingleRecipeObserver>(this.ItemsContainer, new float?(this.Style.RecipeDetail.Height), Offset.Top(10f));
      this.AddUpdater(objectToPlace.Updater);
      this.m_statusInfo = this.AddStatusInfoPanel();
      updaterBuilder.Observe<MechPowerGeneratorFromProduct.State>((Func<MechPowerGeneratorFromProduct.State>) (() => this.Entity.CurrentState)).Do((Action<MechPowerGeneratorFromProduct.State>) (state => MechPowerGeneratorFromProductView.UpdateStatusInfo(state, this.m_statusInfo)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle, new LocStrFormatted?((LocStrFormatted) Tr.PowerGenerator__AutoScalingTooltip));
      BufferView outBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<Electricity>((Func<Electricity>) (() => this.Entity.Prototype.OutputElectricity)).Observe<Electricity>((Func<Electricity>) (() => this.Entity.ElectricityGenerator.GeneratedThisTick)).Do((Action<Electricity, Electricity>) ((capacity, quantity) => outBuffer.UpdateState((ProductProto) this.m_electricityProto, capacity.Quantity, quantity.Quantity)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Requires);
      BufferView inBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<MechPower>((Func<MechPower>) (() => this.Entity.Prototype.InputMechPower)).Observe<MechPower>((Func<MechPower>) (() => this.Entity.UsedMechPowerThisTick)).Do((Action<MechPower, MechPower>) ((capacity, quantity) => inBuffer.UpdateState((ProductProto) this.m_mechPowerProto, capacity.Quantity, quantity.Quantity)));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
