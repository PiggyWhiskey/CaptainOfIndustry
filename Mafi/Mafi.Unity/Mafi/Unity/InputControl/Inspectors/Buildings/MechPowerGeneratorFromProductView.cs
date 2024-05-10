// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.MechPowerGeneratorFromProductView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Machines.PowerGenerators;
using Mafi.Core;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class MechPowerGeneratorFromProductView : 
    StaticEntityInspectorBase<MechPowerGeneratorFromProduct>
  {
    private readonly MechPowerGeneratorFromProductInspector m_controller;
    private readonly VirtualProductProto m_mechPowerProto;
    private StatusPanel m_statusInfo;
    private SwitchBtn m_balanceSwitch;

    protected override MechPowerGeneratorFromProduct Entity => this.m_controller.SelectedEntity;

    public MechPowerGeneratorFromProductView(MechPowerGeneratorFromProductInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller;
      this.m_mechPowerProto = controller.Context.ShaftManager.MechPowerProto;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.SetupTopButtonsContainer();
      this.m_statusInfo = this.AddStatusInfoPanel();
      updaterBuilder.Observe<MechPowerGeneratorFromProduct.State>((Func<MechPowerGeneratorFromProduct.State>) (() => this.Entity.CurrentState)).Do((Action<MechPowerGeneratorFromProduct.State>) (state => MechPowerGeneratorFromProductView.UpdateStatusInfo(state, this.m_statusInfo)));
      this.Builder.DurationNormalizer.AttachPer60ToggleToTitle(this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Recipes), this.Builder, updaterBuilder);
      SingleRecipeObserver objectToPlace = new SingleRecipeObserver((IUiElement) this.ItemsContainer, this.Builder, (Option<RecipesBookController>) this.m_controller.Context.RecipesBookController, (Func<Option<IRecipeForUi>>) (() => (Option<IRecipeForUi>) (IRecipeForUi) this.Entity.Prototype));
      objectToPlace.AppendTo<SingleRecipeObserver>(this.ItemsContainer, new float?(this.Style.RecipeDetail.Height), Offset.Top(5f));
      this.AddUpdater(objectToPlace.Updater);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.MechPowerGenerator__EfficiencyTitle, new LocStrFormatted?((LocStrFormatted) Tr.MechPowerGenerator__EfficiencyTooltip));
      Panel parent = this.AddOverlayPanel(itemContainer);
      QuantityBar efficiencyBar = new QuantityBar(this.Builder).PutToLeftTopOf<QuantityBar>((IUiElement) parent, new Vector2(160f, 25f), Offset.Top(5f) + Offset.Left(this.Style.Panel.Indent));
      this.m_balanceSwitch = this.Builder.NewSwitchBtn().SetText(Tr.MechPowerGenerator__AutoBalance.TranslatedString).AddTooltip(Tr.MechPowerGenerator__AutoBalanceTooltip.TranslatedString).SetOnToggleAction((Action<bool>) (x => this.m_controller.InputScheduler.ScheduleInputCmd<ToggleMechGeneratorAutoBalanceCmd>(new ToggleMechGeneratorAutoBalanceCmd(this.Entity)))).PutToLeftOf<SwitchBtn>((IUiElement) parent, this.Style.Panel.LineHeight, Offset.Left(efficiencyBar.GetWidth() + 2f * this.Style.Panel.Indent));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.AutoBalance)).Do((Action<bool>) (s => this.m_balanceSwitch.SetIsOn(s)));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.Efficiency)).Do((Action<Percent>) (efficiency => efficiencyBar.UpdateValues(efficiency, new LocStrFormatted(efficiency.ToStringRounded()))));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle);
      BufferView outputBuffer1 = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<MechPower>((Func<MechPower>) (() => this.Entity.Prototype.MechPowerOutput)).Observe<MechPower>((Func<MechPower>) (() => this.Entity.PowerGeneratedLastTick)).Do((Action<MechPower, MechPower>) ((capacity, produced) => outputBuffer1.UpdateState((ProductProto) this.m_mechPowerProto, capacity.Quantity, produced.Quantity)));
      BufferView outputBuffer2 = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight), Offset.Top(5f));
      updaterBuilder.Observe<ProductQuantity?>((Func<ProductQuantity?>) (() => this.Entity.Prototype.ProducedProduct)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.OutputBufferQuantity)).Do((Action<ProductQuantity?, Quantity>) ((producedPq, stored) =>
      {
        itemContainer.SetItemVisibility((IUiElement) outputBuffer2, producedPq.HasValue);
        if (!producedPq.HasValue)
          return;
        outputBuffer2.UpdateState(producedPq.Value.Product, producedPq.Value.Quantity, stored);
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle);
      BufferView inputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.SuperCompactHeight));
      updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => this.Entity.Prototype.ConsumedProduct)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.InputBufferQuantity)).Do((Action<ProductQuantity, Quantity>) ((consumedPq, quantity) => inputBuffer.UpdateState(consumedPq.Product, consumedPq.Quantity, quantity)));
      this.AddUpdater(updaterBuilder.Build());
    }

    internal static void UpdateStatusInfo(
      MechPowerGeneratorFromProduct.State state,
      StatusPanel statusPanel)
    {
      switch (state)
      {
        case MechPowerGeneratorFromProduct.State.Working:
          statusPanel.SetStatusWorking();
          break;
        case MechPowerGeneratorFromProduct.State.Broken:
          statusPanel.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
          break;
        case MechPowerGeneratorFromProduct.State.Paused:
          statusPanel.SetStatusPaused();
          break;
        case MechPowerGeneratorFromProduct.State.NotEnoughWorkers:
          statusPanel.SetStatusNoWorkers();
          break;
        case MechPowerGeneratorFromProduct.State.OutputFull:
          statusPanel.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Critical);
          break;
        case MechPowerGeneratorFromProduct.State.NotEnoughInput:
          statusPanel.SetStatus(Tr.EntityStatus__MissingInput, StatusPanel.State.Critical);
          break;
        case MechPowerGeneratorFromProduct.State.NoShaft:
          statusPanel.SetStatus(Tr.EntityStatus__NoShaft, StatusPanel.State.Critical);
          break;
        default:
          statusPanel.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
          break;
      }
    }
  }
}
