// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ThermalStorageView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings.ThermalStorages;
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
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class ThermalStorageView : StaticEntityInspectorBase<ThermalStorage>
  {
    private readonly ThermalStorageInspector m_controller;
    private readonly ProtoPicker<ProductProto> m_protoPicker;
    private Btn m_plusBtn;
    private StorageAlertingPanel<ThermalStorage> m_alertPanel;

    protected override ThermalStorage Entity => this.m_controller.SelectedEntity;

    public ThermalStorageView(ThermalStorageInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_protoPicker = new ProtoPicker<ProductProto>(new Action<ProductProto>(this.setProductToStore));
      this.m_controller = controller;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      StatusPanel status = this.AddStatusInfoPanel();
      updaterBuilder.Observe<ThermalStorage.State>((Func<ThermalStorage.State>) (() => this.Entity.CurrentState)).Do((Action<ThermalStorage.State>) (state =>
      {
        switch (state)
        {
          case ThermalStorage.State.Working:
            status.SetStatusWorking();
            break;
          case ThermalStorage.State.Broken:
            status.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case ThermalStorage.State.Paused:
            status.SetStatusPaused();
            break;
          case ThermalStorage.State.NotEnoughWorkers:
            status.SetStatusNoWorkers();
            break;
          case ThermalStorage.State.NotEnoughPower:
            status.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          default:
            status.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
            break;
        }
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.StoredHeat__Title, new LocStrFormatted?((LocStrFormatted) Tr.StoredHeat__Tooltip));
      int btnSize = 38;
      Panel parent = this.Builder.NewPanel("Bar container").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(60f));
      QuantityBar heatBar = new QuantityBar(this.Builder).SetColor(this.Style.QuantityBar.NegativeBarColor).PutTo<QuantityBar>((IUiElement) parent, Offset.TopBottom(15f) + Offset.Left(80f) + Offset.Right(20f));
      Panel iconContainer = this.Builder.NewPanel("IconContainer", (IUiElement) parent).PutToLeftOf<Panel>((IUiElement) parent, this.Style.ProductWithIcon.Size.x);
      Panel plusBtnHolder = this.Builder.NewPanel("PlusBtnContainer", (IUiElement) iconContainer).PutToCenterMiddleOf<Panel>((IUiElement) iconContainer, btnSize.Vector2());
      Txt callToAction = this.Builder.NewTxt("CallToAction").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Builder.Style.Global.TextInc).SetText(string.Format("({0})", (object) Tr.StoredHeat__NoProductSelected)).PutToLeftOf<Txt>((IUiElement) parent, 250f, Offset.Left(80f));
      this.m_plusBtn = this.Builder.NewBtnPrimary("PlusBtn", (IUiElement) plusBtnHolder).SetIcon(this.Style.Icons.Plus, new Offset?(Offset.All(8f))).OnClick(new Action(this.plusBtnClicked)).PutTo<Btn>((IUiElement) plusBtnHolder);
      updaterBuilder.Observe<int>((Func<int>) (() => this.Entity.HeatStored)).Observe<int>((Func<int>) (() => this.Entity.HeatCapacity)).Do((Action<int, int>) ((stored, capacity) =>
      {
        heatBar.SetVisibility<QuantityBar>(capacity > 0);
        heatBar.UpdateValues(capacity.Quantity(), stored.Quantity());
      }));
      this.m_protoPicker.BuildUi(this.Builder);
      Panel overlay = this.AddOverlay(new Action(((View) this.m_protoPicker).Hide));
      this.m_protoPicker.PutToLeftTopOf<ProtoPicker<ProductProto>>((IUiElement) plusBtnHolder, this.m_protoPicker.GetSize(), Offset.Left(plusBtnHolder.GetWidth() - 1f));
      this.m_protoPicker.OnShowStart += (Action) (() =>
      {
        overlay.Show<Panel>();
        plusBtnHolder.SetParent<Panel>((IUiElement) this);
      });
      this.m_protoPicker.OnHide += (Action) (() =>
      {
        overlay.Hide<Panel>();
        plusBtnHolder.PutToCenterMiddleOf<Panel>((IUiElement) iconContainer, btnSize.Vector2());
      });
      Txt chargingTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ThermalStorage__ChargingRecipeTitle);
      SingleRecipeObserver chargingRecipeObserver = new SingleRecipeObserver((IUiElement) this.ItemsContainer, this.Builder, (Option<RecipesBookController>) this.m_controller.Context.RecipesBookController, (Func<Option<IRecipeForUi>>) (() => this.Entity.GetChargingRecipe()));
      chargingRecipeObserver.AppendTo<SingleRecipeObserver>(this.ItemsContainer, new float?(this.Style.RecipeDetail.Height), Offset.Top(5f));
      this.AddUpdater(chargingRecipeObserver.Updater);
      Txt dischargingTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ThermalStorage__DischargingRecipeTitle);
      SingleRecipeObserver dischargingRecipeObserver = new SingleRecipeObserver((IUiElement) this.ItemsContainer, this.Builder, (Option<RecipesBookController>) this.m_controller.Context.RecipesBookController, (Func<Option<IRecipeForUi>>) (() => this.Entity.GetDischargingRecipeFor()));
      dischargingRecipeObserver.AppendTo<SingleRecipeObserver>(this.ItemsContainer, new float?(this.Style.RecipeDetail.Height), Offset.Top(5f));
      this.AddUpdater(dischargingRecipeObserver.Updater);
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() =>
      {
        ThermalStorageProto.ProductData? assignedProduct = this.Entity.AssignedProduct;
        ref ThermalStorageProto.ProductData? local = ref assignedProduct;
        return !local.HasValue ? (ProductProto) null : local.GetValueOrDefault().Product;
      })).Do((Action<ProductProto>) (assignedProduct =>
      {
        itemContainer.SetItemVisibility((IUiElement) chargingTitle, (Proto) assignedProduct != (Proto) null);
        itemContainer.SetItemVisibility((IUiElement) chargingRecipeObserver, (Proto) assignedProduct != (Proto) null);
        itemContainer.SetItemVisibility((IUiElement) dischargingTitle, (Proto) assignedProduct != (Proto) null);
        itemContainer.SetItemVisibility((IUiElement) dischargingRecipeObserver, (Proto) assignedProduct != (Proto) null);
        this.m_plusBtn.SetIcon((Proto) assignedProduct == (Proto) null ? this.Style.Icons.Plus : "Assets/Unity/UserInterface/General/Rotate128.png", new Offset?(Offset.All(8f)));
        this.m_plusBtn.SetButtonStyle((Proto) assignedProduct == (Proto) null ? this.Style.Global.PrimaryBtn : this.Style.Global.GeneralBtn);
        callToAction.SetVisibility<Txt>((Proto) assignedProduct == (Proto) null);
      }));
      this.m_alertPanel = new StorageAlertingPanel<ThermalStorage>((IUiElement) parent, this.Builder, this.m_controller.InputScheduler, (Func<ThermalStorage>) (() => this.Entity));
      this.m_alertPanel.PutToRightTopOf<StorageAlertingPanel<ThermalStorage>>((IUiElement) parent, this.m_alertPanel.GetSize(), Offset.Right((float) (-(double) this.m_alertPanel.GetWidth() + 1.0)) + Offset.Top(-30f));
      this.AddUpdater(this.m_alertPanel.Updater);
      this.AddUpdater(updaterBuilder.Build());
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_protoPicker.SetVisibleProtos(this.m_controller.Context.UnlockedProtosDbForUi.FilterUnlocked<ProductProto>(this.Entity.Prototype.SupportedProducts.Select<ProductProto>((Func<ThermalStorageProto.ProductData, ProductProto>) (x => x.Product))));
        this.m_protoPicker.Show();
      }
    }

    private void setProductToStore(IProtoWithIconAndName product)
    {
      if (!(product is ProductProto product1))
      {
        Log.Error(string.Format("Trying to store UI product which isn't a ProductProto '{0}'", (object) product));
      }
      else
      {
        this.m_controller.ScheduleInputCmd<ThermalStorageSetProductCmd>(new ThermalStorageSetProductCmd(this.m_controller.SelectedEntity, product1));
        this.m_protoPicker.Hide();
      }
    }
  }
}
