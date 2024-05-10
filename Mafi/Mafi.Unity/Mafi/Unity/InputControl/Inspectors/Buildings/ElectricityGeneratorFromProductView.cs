// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ElectricityGeneratorFromProductView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Machines.PowerGenerators;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class ElectricityGeneratorFromProductView : 
    StaticEntityInspectorBase<ElectricityGeneratorFromProduct>
  {
    private readonly ElectricityGeneratorFromProductInspector m_controller;
    private readonly VirtualProductProto m_electricityProto;

    protected override ElectricityGeneratorFromProduct Entity => this.m_controller.SelectedEntity;

    public ElectricityGeneratorFromProductView(
      ElectricityGeneratorFromProductInspector controller,
      VirtualProductProto electricityProto)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller;
      this.m_electricityProto = electricityProto;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddMaxElectricityOutputPanel(updaterBuilder, (Func<Electricity>) (() => this.Entity.Prototype.OutputElectricity));
      base.AddCustomItems(itemContainer);
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.Builder.DurationNormalizer.AttachPer60ToggleToTitle(this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Recipes), this.Builder, updaterBuilder);
      SingleRecipeObserver objectToPlace = new SingleRecipeObserver((IUiElement) this.ItemsContainer, this.Builder, (Option<RecipesBookController>) this.m_controller.Context.RecipesBookController, (Func<Option<IRecipeForUi>>) (() => (Option<IRecipeForUi>) (IRecipeForUi) this.Entity.Prototype));
      objectToPlace.AppendTo<SingleRecipeObserver>(this.ItemsContainer, new float?(this.Style.RecipeDetail.Height), Offset.Top(5f));
      this.AddUpdater(objectToPlace.Updater);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle, new LocStrFormatted?((LocStrFormatted) Tr.PowerGenerator__AutoScalingTooltip));
      BufferView outBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<Electricity>((Func<Electricity>) (() => this.Entity.Prototype.OutputElectricity)).Observe<Electricity>((Func<Electricity>) (() => this.Entity.ElectricityGenerator.GeneratedThisTick)).Do((Action<Electricity, Electricity>) ((capacity, quantity) => outBuffer.UpdateState((ProductProto) this.m_electricityProto, capacity.Quantity, quantity.Quantity)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Requires);
      BufferView consumption = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Prototype.InputProduct.Product)).Observe<Percent>((Func<Percent>) (() => this.Entity.CurrentProductLeft)).Observe<Percent>((Func<Percent>) (() => this.Entity.CurrentFuelUsage)).Do((Action<ProductProto, Percent, Percent>) ((product, percent, utilization) =>
      {
        LocStrFormatted text = Tr.PowerGenerator__Utilization.Format(utilization.ToStringRounded());
        consumption.UpdateState((Option<ProductProto>) product, percent, text);
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FuelTank_Title);
      BufferView bufferView = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.Prototype.InputProduct.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.InputBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.InputBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(bufferView.UpdateState));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle);
      BufferView outputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => this.Entity.Prototype.OutputProduct)).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBufferReadOnly valueOrNull = this.Entity.OutputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Capacity;
      })).Observe<Quantity>((Func<Quantity>) (() =>
      {
        IProductBufferReadOnly valueOrNull = this.Entity.OutputBuffer.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.Quantity;
      })).Do((Action<ProductQuantity, Quantity, Quantity>) ((output, capacity, quantity) =>
      {
        itemContainer.SetItemVisibility((IUiElement) outputBuffer, output.IsNotEmpty && output.Product.Type != VirtualProductProto.ProductType);
        if (!output.IsNotEmpty)
          return;
        outputBuffer.UpdateState(output.Product, capacity, quantity);
      }));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
