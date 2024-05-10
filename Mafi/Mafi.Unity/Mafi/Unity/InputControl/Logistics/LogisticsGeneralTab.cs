// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Logistics.LogisticsGeneralTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Commands;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Logistics
{
  internal class LogisticsGeneralTab : Tab, IDynamicSizeElement, IUiElement
  {
    private readonly CameraController m_cameraController;
    private readonly IInputScheduler m_inputScheduler;
    private readonly EntitiesManager m_entitiesManager;
    private readonly VehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly ITerrainDumpingManager m_dumpingManager;
    private readonly IWindowWithInnerWindowsSupport m_parentWindow;

    public event Action<IUiElement> SizeChanged;

    internal LogisticsGeneralTab(
      CameraController cameraController,
      IInputScheduler inputScheduler,
      EntitiesManager entitiesManager,
      VehicleBuffersRegistry vehicleBuffersRegistry,
      ITerrainDumpingManager dumpingManager,
      IWindowWithInnerWindowsSupport parentWindow)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("GeneralTab");
      this.m_cameraController = cameraController;
      this.m_inputScheduler = inputScheduler;
      this.m_entitiesManager = entitiesManager;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_dumpingManager = dumpingManager;
      this.m_parentWindow = parentWindow;
    }

    protected override void BuildUi()
    {
      UpdaterBuilder updater = UpdaterBuilder.Start();
      StackContainer container = this.Builder.NewStackContainer("container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Bottom(10f)).PutToTopOf<StackContainer>((IUiElement) this, 0.0f);
      container.SizeChanged += (Action<IUiElement>) (x =>
      {
        this.SetHeight<LogisticsGeneralTab>(x.GetHeight());
        Action<IUiElement> sizeChanged = this.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) this);
      });
      this.Builder.AddSectionTitle(container, (LocStrFormatted) Tr.Options);
      this.Builder.AddSwitch(container, TrCore.PartialTrucksToggle.TranslatedString, (Action<bool>) (x => this.m_inputScheduler.ScheduleInputCmd<TogglePartialTrucksLoadCmd>(new TogglePartialTrucksLoadCmd())), updater, (Func<bool>) (() => this.m_vehicleBuffersRegistry.AllowPartialTrucks), TrCore.PartialTrucksToggle__Tooltip.TranslatedString);
      this.Builder.AddSectionTitle(container, (LocStrFormatted) Tr.DumpingFilterGlobal__Title, new LocStrFormatted?((LocStrFormatted) Tr.DumpingFilterGlobal__Tooltip));
      ProtosFilterEditor<ProductProto> protosFilterEditor = new ProtosFilterEditor<ProductProto>(this.Builder, this.m_parentWindow, container, (Action<ProductProto>) (p =>
      {
        if (p != null)
        {
          ProductProto productProto = p;
          this.m_inputScheduler.ScheduleInputCmd<RemoveProductToDumpCmd>(new RemoveProductToDumpCmd((Option<MineTower>) Option.None, productProto));
        }
        else
          Log.Error(string.Format("Trying to remove UI product which isn't a ProductProto '{0}'", (object) p));
      }), (Action<ProductProto>) (p =>
      {
        if (p != null)
        {
          ProductProto productProto = p;
          this.m_inputScheduler.ScheduleInputCmd<AddProductToDumpCmd>(new AddProductToDumpCmd((Option<MineTower>) Option.None, productProto));
        }
        else
          Log.Error(string.Format("Trying to remove UI product which isn't a ProductProto '{0}'", (object) p));
      }), new Func<IEnumerable<ProductProto>>(this.m_dumpingManager.AllDumpableProducts.AsEnumerable), (Func<IEnumerable<ProductProto>>) (() => (IEnumerable<ProductProto>) this.m_dumpingManager.ProductsAllowedToDump), 7);
      protosFilterEditor.SetTextToShowWhenEmpty(Tr.DumpingFilter__Empty.TranslatedString);
      updater.Observe<ProductProto>((Func<IReadOnlyCollection<ProductProto>>) (() => (IReadOnlyCollection<ProductProto>) this.m_dumpingManager.ProductsAllowedToDump), (ICollectionComparator<ProductProto, IReadOnlyCollection<ProductProto>>) CompareByCount<ProductProto>.Instance).Do(new Action<Lyst<ProductProto>>(protosFilterEditor.UpdateFilteredProtos));
      LogisticsGeneralTab.VehiclesStatsView totalCountsStats = new LogisticsGeneralTab.VehiclesStatsView(this.m_cameraController, this.Builder, Tr.OwnedVehicles).AppendTo<LogisticsGeneralTab.VehiclesStatsView>(container, new float?(105f), Offset.Top(10f));
      LogisticsGeneralTab.VehiclesStatsView totalAssignableStats = new LogisticsGeneralTab.VehiclesStatsView(this.m_cameraController, this.Builder, Tr.AvailableToAssign).AppendTo<LogisticsGeneralTab.VehiclesStatsView>(container, new float?(105f), Offset.Top(10f));
      LogisticsGeneralTab.VehiclesStatsView miningCountsStats = new LogisticsGeneralTab.VehiclesStatsView(this.m_cameraController, this.Builder, Tr.VehiclesAssignedToMining).AppendTo<LogisticsGeneralTab.VehiclesStatsView>(container, new float?(105f), Offset.Top(10f));
      LogisticsGeneralTab.VehiclesStatsView treeHarvestingCountsStats = new LogisticsGeneralTab.VehiclesStatsView(this.m_cameraController, this.Builder, Tr.VehiclesAssignedToTreeHarvesting).AppendTo<LogisticsGeneralTab.VehiclesStatsView>(container, new float?(105f), Offset.Top(10f));
      LogisticsGeneralTab.VehiclesStatsView otherAssignmentsCountsStats = new LogisticsGeneralTab.VehiclesStatsView(this.m_cameraController, this.Builder, Tr.VehiclesAssignedToBuildings).AppendTo<LogisticsGeneralTab.VehiclesStatsView>(container, new float?(105f), Offset.Top(10f));
      updater.DoOnSyncPeriodically((Action) (() =>
      {
        totalCountsStats.UpdateIdleVehicles();
        totalAssignableStats.UpdateIdleVehicles();
        miningCountsStats.UpdateIdleVehicles();
        treeHarvestingCountsStats.UpdateIdleVehicles();
        otherAssignmentsCountsStats.UpdateIdleVehicles();
      }), new Duration?(Duration.FromTicks(5)));
      updater.Observe<Mafi.Core.Entities.Dynamic.Vehicle>((Func<IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) (() => this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Entities.Dynamic.Vehicle>()), (ICollectionComparator<Mafi.Core.Entities.Dynamic.Vehicle, IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) CompareFixedOrder<Mafi.Core.Entities.Dynamic.Vehicle>.Instance).Do((Action<Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>) (vehicles =>
      {
        totalCountsStats.SetVehicles(vehicles);
        container.SetItemVisibility((IUiElement) totalCountsStats, vehicles.IsNotEmpty);
      }));
      updater.Observe<Mafi.Core.Entities.Dynamic.Vehicle>((Func<IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) (() => this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Entities.Dynamic.Vehicle>((Predicate<Mafi.Core.Entities.Dynamic.Vehicle>) (x => x.CanBeAssigned))), (ICollectionComparator<Mafi.Core.Entities.Dynamic.Vehicle, IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) CompareFixedOrder<Mafi.Core.Entities.Dynamic.Vehicle>.Instance).Do((Action<Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>) (vehicles =>
      {
        totalAssignableStats.SetVehicles(vehicles);
        container.SetItemVisibility((IUiElement) totalAssignableStats, vehicles.IsNotEmpty);
      }));
      updater.Observe<Mafi.Core.Entities.Dynamic.Vehicle>((Func<IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) (() => this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Entities.Dynamic.Vehicle>((Predicate<Mafi.Core.Entities.Dynamic.Vehicle>) (x => x.AssignedTo.ValueOrNull is MineTower))), (ICollectionComparator<Mafi.Core.Entities.Dynamic.Vehicle, IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) CompareFixedOrder<Mafi.Core.Entities.Dynamic.Vehicle>.Instance).Do((Action<Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>) (vehicles =>
      {
        miningCountsStats.SetVehicles(vehicles);
        container.SetItemVisibility((IUiElement) miningCountsStats, vehicles.IsNotEmpty);
      }));
      updater.Observe<Mafi.Core.Entities.Dynamic.Vehicle>((Func<IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) (() => this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Entities.Dynamic.Vehicle>((Predicate<Mafi.Core.Entities.Dynamic.Vehicle>) (x => x is TreeHarvester || x.AssignedTo.ValueOrNull is TreeHarvester))), (ICollectionComparator<Mafi.Core.Entities.Dynamic.Vehicle, IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) CompareFixedOrder<Mafi.Core.Entities.Dynamic.Vehicle>.Instance).Do((Action<Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>) (vehicles =>
      {
        treeHarvestingCountsStats.SetVehicles(vehicles);
        container.SetItemVisibility((IUiElement) treeHarvestingCountsStats, vehicles.IsNotEmpty);
      }));
      updater.Observe<Mafi.Core.Entities.Dynamic.Vehicle>((Func<IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) (() => this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Entities.Dynamic.Vehicle>((Predicate<Mafi.Core.Entities.Dynamic.Vehicle>) (x =>
      {
        bool flag;
        switch (x.AssignedTo.ValueOrNull)
        {
          case FuelStation _:
          case Storage _:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        return flag;
      }))), (ICollectionComparator<Mafi.Core.Entities.Dynamic.Vehicle, IEnumerable<Mafi.Core.Entities.Dynamic.Vehicle>>) CompareFixedOrder<Mafi.Core.Entities.Dynamic.Vehicle>.Instance).Do((Action<Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>) (vehicles =>
      {
        otherAssignmentsCountsStats.SetVehicles(vehicles);
        container.SetItemVisibility((IUiElement) otherAssignmentsCountsStats, vehicles.IsNotEmpty);
      }));
      this.AddUpdater(updater.Build());
    }

    private class VehiclesStatsView : IUiElement, IDynamicSizeElement
    {
      private readonly CameraController m_cameraController;
      private readonly ViewsCacheHomogeneous<LogisticsGeneralTab.VehicleCountView> m_viewsCache;
      private readonly Panel m_container;
      private readonly Panel m_innerContainer;
      private readonly GridContainer m_statsContainer;
      private readonly Txt m_workers;
      private readonly Dict<DynamicGroundEntityProto, int> m_countsCache;
      private readonly Dict<DrivingEntityProto, Lyst<Mafi.Core.Entities.Dynamic.Vehicle>> m_vehiclesCache;
      private readonly Dict<DynamicGroundEntityProto, LogisticsGeneralTab.VehicleCountView> m_viewsMap;
      private readonly Lyst<Mafi.Core.Entities.Dynamic.Vehicle> m_vehicles;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public event Action<IUiElement> SizeChanged;

      public VehiclesStatsView(CameraController cameraController, UiBuilder builder, LocStr title)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_countsCache = new Dict<DynamicGroundEntityProto, int>();
        this.m_vehiclesCache = new Dict<DrivingEntityProto, Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>();
        this.m_viewsMap = new Dict<DynamicGroundEntityProto, LogisticsGeneralTab.VehicleCountView>();
        this.m_vehicles = new Lyst<Mafi.Core.Entities.Dynamic.Vehicle>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        LogisticsGeneralTab.VehiclesStatsView element = this;
        this.m_cameraController = cameraController;
        this.m_viewsCache = new ViewsCacheHomogeneous<LogisticsGeneralTab.VehicleCountView>((Func<LogisticsGeneralTab.VehicleCountView>) (() => new LogisticsGeneralTab.VehicleCountView(builder, new Action<DrivingEntityProto, int>(element.panToVehicle))));
        this.m_container = builder.NewPanel("Container");
        builder.NewTitle(title).PutToTopOf<Txt>((IUiElement) this.m_container, 20f, Offset.Left(10f));
        this.m_workers = builder.NewTitle("Workers").SetAlignment(TextAnchor.MiddleRight).PutToTopOf<Txt>((IUiElement) this.m_container, 20f, Offset.Right(20f));
        this.m_innerContainer = builder.NewPanel("Container").SetBackground(builder.Style.Panel.ItemOverlay).PutTo<Panel>((IUiElement) this.m_container, Offset.Top(25f));
        this.m_statsContainer = builder.NewGridContainer("GridContainer").SetCellSize(80.Vector2()).SetCellSpacing(5f).SetDynamicHeightMode(7).PutToLeftTopOf<GridContainer>((IUiElement) this.m_innerContainer, Vector2.zero, Offset.Left(5f));
        this.m_statsContainer.SizeChanged += (Action<IUiElement>) (e =>
        {
          element.SetHeight<LogisticsGeneralTab.VehiclesStatsView>(element.m_statsContainer.GetRequiredHeight() + 25f);
          Action<IUiElement> sizeChanged = element.SizeChanged;
          if (sizeChanged == null)
            return;
          sizeChanged((IUiElement) element);
        });
      }

      public void SetVehicles(Lyst<Mafi.Core.Entities.Dynamic.Vehicle> vehicles)
      {
        this.m_viewsMap.Clear();
        this.m_vehicles.Clear();
        this.m_vehicles.AddRange(vehicles);
        int quantity = 0;
        foreach (Lyst<Mafi.Core.Entities.Dynamic.Vehicle> lyst in this.m_vehiclesCache.Values)
          lyst.Clear();
        foreach (Mafi.Core.Entities.Dynamic.Vehicle vehicle in vehicles)
        {
          Lyst<Mafi.Core.Entities.Dynamic.Vehicle> lyst;
          if (!this.m_vehiclesCache.TryGetValue(vehicle.Prototype, out lyst))
          {
            lyst = new Lyst<Mafi.Core.Entities.Dynamic.Vehicle>();
            this.m_vehiclesCache[vehicle.Prototype] = lyst;
          }
          lyst.Add(vehicle);
          quantity += vehicle.WorkersAssigned();
        }
        this.m_statsContainer.ClearAll();
        this.m_viewsCache.ReturnAll();
        foreach (KeyValuePair<DrivingEntityProto, Lyst<Mafi.Core.Entities.Dynamic.Vehicle>> keyValuePair in this.m_vehiclesCache)
        {
          if (keyValuePair.Value.Count != 0)
          {
            LogisticsGeneralTab.VehicleCountView view = this.m_viewsCache.GetView();
            view.SetVehicleData(keyValuePair.Key, keyValuePair.Value.Count);
            this.m_viewsMap[(DynamicGroundEntityProto) keyValuePair.Key] = view;
            this.m_statsContainer.Append((IUiElement) view);
          }
        }
        this.m_workers.SetText(string.Format("[ {0} ]", (object) Tr.VehiclesManagement__Drivers.Format(quantity.ToString(), quantity)));
        this.UpdateIdleVehicles();
      }

      public void UpdateIdleVehicles()
      {
        this.m_countsCache.Clear();
        foreach (Mafi.Core.Entities.Dynamic.Vehicle vehicle in this.m_vehicles)
        {
          int num;
          if (!this.m_countsCache.TryGetValue((DynamicGroundEntityProto) vehicle.Prototype, out num))
            num = 0;
          this.m_countsCache[(DynamicGroundEntityProto) vehicle.Prototype] = num + (vehicle.IsIdle ? 1 : 0);
        }
        foreach (KeyValuePair<DynamicGroundEntityProto, LogisticsGeneralTab.VehicleCountView> views in this.m_viewsMap)
        {
          int count;
          if (this.m_countsCache.TryGetValue(views.Key, out count))
            views.Value.SetIdleCount(count);
          else
            views.Value.SetIdleCount(0);
        }
      }

      private void panToVehicle(DrivingEntityProto groupProto, int index)
      {
        Lyst<Mafi.Core.Entities.Dynamic.Vehicle> lyst;
        if (!this.m_vehiclesCache.TryGetValue(groupProto, out lyst) || index < 0 || index >= lyst.Count)
          return;
        this.m_cameraController.PanTo(lyst[index].Position2f);
      }
    }

    private class VehicleCountView : IUiElement
    {
      private readonly Action<DrivingEntityProto, int> m_onClick;
      private readonly Panel m_container;
      private readonly IconContainer m_icon;
      private readonly Txt m_countTxt;
      private readonly Txt m_idleCount;
      private DrivingEntityProto m_proto;
      private int m_count;
      private int m_lastClickIndex;
      private readonly IconContainer m_fuelIcon;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public VehicleCountView(UiBuilder builder, Action<DrivingEntityProto, int> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_onClick = onClick;
        this.m_container = builder.NewPanel("Container").OnClick(new Action(this.onClickInternal));
        this.m_icon = builder.NewIconContainer("Icon").PutTo<IconContainer>((IUiElement) this.m_container, Offset.Bottom(30f));
        this.m_fuelIcon = builder.NewIconContainer("Icon").PutToLeftBottomOf<IconContainer>((IUiElement) this.m_icon, 18.Vector2(), Offset.Bottom(4f));
        Txt txt1 = builder.NewTxt("Count");
        TextStyle text1 = builder.Style.Global.Text;
        ref TextStyle local1 = ref text1;
        int? nullable = new int?(14);
        ColorRgba? color1 = new ColorRgba?();
        FontStyle? fontStyle1 = new FontStyle?();
        int? fontSize1 = nullable;
        bool? isCapitalized1 = new bool?();
        TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
        this.m_countTxt = txt1.SetTextStyle(textStyle1).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) this.m_container, 20f, Offset.Bottom(15f));
        Txt txt2 = builder.NewTxt("Idle");
        TextStyle text2 = builder.Style.Global.Text;
        ref TextStyle local2 = ref text2;
        nullable = new int?(11);
        ColorRgba? color2 = new ColorRgba?();
        FontStyle? fontStyle2 = new FontStyle?();
        int? fontSize2 = nullable;
        bool? isCapitalized2 = new bool?();
        TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
        this.m_idleCount = txt2.SetTextStyle(textStyle2).SetColor(new ColorRgba(15048741)).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) this.m_container, 15f);
      }

      private void onClickInternal()
      {
        if (this.m_count <= 0)
          return;
        this.m_lastClickIndex %= this.m_count;
        this.m_onClick(this.m_proto, this.m_lastClickIndex);
        ++this.m_lastClickIndex;
      }

      public void SetVehicleData(DrivingEntityProto proto, int count)
      {
        this.m_lastClickIndex = 0;
        this.m_count = count;
        this.m_proto = proto;
        this.m_icon.SetIcon(proto.Graphics.IconPath);
        this.m_countTxt.SetText(count.ToString());
        this.m_fuelIcon.SetVisibility<IconContainer>(proto.FuelTankProto.HasValue);
        this.m_fuelIcon.SetIcon(proto.FuelTankProto.ValueOrNull?.Product.IconPath ?? "");
      }

      public void SetIdleCount(int count)
      {
        this.m_idleCount.SetText(count > 0 ? Tr.VehiclesManagement__IdleCount.Format(count.ToString()) : LocStrFormatted.Empty);
      }
    }
  }
}
