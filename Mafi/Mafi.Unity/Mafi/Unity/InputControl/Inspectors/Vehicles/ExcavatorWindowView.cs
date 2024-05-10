// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.ExcavatorWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.Commands;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  internal class ExcavatorWindowView : VehicleWindowView<Excavator, ExcavatorWindowView>
  {
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly ImmutableArray<TruckProto> m_allTrucks;
    private readonly ExcavatorInspector m_controller;
    private SingleProductFilterEditor<ProductProto> m_filterView;

    protected override Excavator Entity => this.m_controller.SelectedEntity;

    public ExcavatorWindowView(
      ExcavatorInspector controller,
      MbBasedEntitiesRenderer entitiesRenderer,
      OrbitalCameraModel orbitalCameraModel,
      ProtosDb protosDb,
      UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((VehicleInspector<Excavator, ExcavatorWindowView>) controller, orbitalCameraModel, entitiesRenderer);
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_controller = controller.CheckNotNull<ExcavatorInspector>();
      this.m_allTrucks = this.m_protosDb.All<TruckProto>().ToImmutableArray<TruckProto>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.EntityStatus);
      Panel parent1 = this.AddOverlayPanel(this.ItemsContainer, 70);
      Txt stateLabel = this.Builder.NewTxt("Label", (IUiElement) parent1).SetTextStyle(this.Style.Panel.Text).SetAlignment(TextAnchor.UpperLeft).PutToLeftTopOf<Txt>((IUiElement) parent1, new Vector2(200f, 40f), Offset.Left(20f) + Offset.Top(10f));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoTitle);
      BufferWithMultipleProductsView buffer = new BufferWithMultipleProductsView((IUiElement) itemContainer, this.Builder);
      itemContainer.Append((IUiElement) buffer, new float?(buffer.GetHeight()));
      this.m_filterView = new SingleProductFilterEditor<ProductProto>(this.Builder, (ItemDetailWindowView) this, new Action<ProductProto>(productRemoved), new Action<ProductProto>(productAdded), new Func<IEnumerable<ProductProto>>(this.m_protosDb.All<LooseProductProto>().Where<LooseProductProto>((Func<LooseProductProto, bool>) (x => x.CanBeOnTerrain)).Cast<ProductProto>().ToImmutableArray<ProductProto>().AsEnumerable), (Func<Option<ProductProto>>) (() => this.Entity.PrioritizedProduct.As<ProductProto>()));
      this.m_filterView.PutToRightOf<SingleProductFilterEditor<ProductProto>>((IUiElement) parent1, 140f);
      this.CreateSectionTitle((IUiElement) this.m_filterView, (LocStrFormatted) Tr.MiningPriority__Title, new LocStrFormatted?((LocStrFormatted) Tr.MiningPriority__Tooltip)).PutToTopOf<Txt>((IUiElement) this.m_filterView, 25f, Offset.Top(-27f) + Offset.Left(-10f));
      updaterBuilder.Observe<Option<LooseProductProto>>((Func<Option<LooseProductProto>>) (() => this.Entity.PrioritizedProduct)).Do((Action<Option<LooseProductProto>>) (product => this.m_filterView.UpdateFilteredProduct(product.As<ProductProto>())));
      this.AddVehicleFuelStatus(itemContainer, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      Panel parent2 = this.AddOverlayPanel(this.ItemsContainer, 45, Offset.Top(10f));
      Txt txt = this.Builder.NewTitle("title").SetText(string.Format("{0}:", (object) Tr.SupportedTrucks__Title)).SetAlignment(TextAnchor.MiddleLeft);
      txt.PutToLeftOf<Txt>((IUiElement) parent2, txt.GetPreferedWidth(), Offset.Left(10f));
      StackContainer truckStack = this.Builder.NewStackContainer("Trucks").SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(10f).PutToLeftOf<StackContainer>((IUiElement) parent2, 0.0f, Offset.Left(txt.GetWidth() + 25f));
      Lyst<KeyValuePair<TruckProto, IconContainer>> truckIcons = new Lyst<KeyValuePair<TruckProto, IconContainer>>();
      foreach (TruckProto allTruck in this.m_allTrucks)
      {
        IconContainer element = this.Builder.NewIconContainer("Icon").SetIcon(allTruck.Graphics.IconPath);
        element.Hide<IconContainer>();
        truckStack.Append((IUiElement) element, new float?(45f));
        truckIcons.Add(new KeyValuePair<TruckProto, IconContainer>(allTruck, element));
      }
      bool unlockedToggle = false;
      this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += (Action) (() => unlockedToggle = !unlockedToggle);
      updaterBuilder.Observe<ExcavatorProto>((Func<ExcavatorProto>) (() => this.Entity.Prototype)).Observe<bool>((Func<bool>) (() => unlockedToggle)).Do((Action<ExcavatorProto, bool>) ((proto, _) =>
      {
        truckStack.StartBatchOperation();
        foreach (KeyValuePair<TruckProto, IconContainer> keyValuePair in truckIcons)
        {
          TruckProto key = keyValuePair.Key;
          bool isVisible = this.m_unlockedProtosDb.IsUnlocked((IProto) key) && proto.IsTruckSupported(key);
          truckStack.SetItemVisibility((IUiElement) keyValuePair.Value, isVisible);
        }
        truckStack.FinishBatchOperation();
      }));
      this.AddAssignedToPanel(this.m_controller.Context, itemContainer, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      this.AddVehicleButtons(updaterBuilder);
      Lyst<ProductQuantity> productsCache = new Lyst<ProductQuantity>();
      updaterBuilder.Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        productsCache.Clear();
        this.Entity.Cargo.GetCargoProducts(productsCache);
        return (IIndexable<ProductQuantity>) productsCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Prototype.Capacity)).Do((Action<Lyst<ProductQuantity>, Quantity>) ((cargo, capacity) => buffer.SetProducts(cargo, capacity, false)));
      updaterBuilder.Observe<LocStrFormatted>((Func<LocStrFormatted>) (() => this.Entity.CurrentJobInfo)).Observe<ExcavatorState>((Func<ExcavatorState>) (() => this.Entity.State)).Observe<DrivingState>((Func<DrivingState>) (() => this.Entity.DrivingState)).Do((Action<LocStrFormatted, ExcavatorState, DrivingState>) ((jobInfo, state, drivingState) =>
      {
        if (!string.IsNullOrEmpty(jobInfo.Value))
          stateLabel.SetText(string.Format("{0} ({1}, {2})", (object) jobInfo.Value, (object) state, (object) drivingState));
        else
          stateLabel.SetText(string.Format("No job ({0}, {1})", (object) state, (object) drivingState));
      }));
      this.AddUpdater(updaterBuilder.Build());

      void productRemoved(ProductProto proto)
      {
        this.m_controller.InputScheduler.ScheduleInputCmd<ExcavatorTogglePreferredProductCmd>(new ExcavatorTogglePreferredProductCmd(this.Entity.Id, new ProductProto.ID?()));
      }

      void productAdded(ProductProto proto)
      {
        this.m_controller.InputScheduler.ScheduleInputCmd<ExcavatorTogglePreferredProductCmd>(new ExcavatorTogglePreferredProductCmd(this.Entity.Id, new ProductProto.ID?(proto.Id)));
      }
    }
  }
}
