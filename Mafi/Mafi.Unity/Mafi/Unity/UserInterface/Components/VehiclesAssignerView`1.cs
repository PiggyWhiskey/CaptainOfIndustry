// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.VehiclesAssignerView`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Commands;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components.VehiclesAssigner;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  /// <summary>
  /// Handles assignment of exactly all prototypes of type T via <see cref="T:Mafi.Unity.UserInterface.Components.VehiclesAssigner.VehicleTypeAssignerView" />.
  /// </summary>
  public class VehiclesAssignerView<T> : IUiElement, IDynamicSizeElement where T : DrivingEntityProto
  {
    private readonly Func<IEntityAssignedWithVehicles> m_entityProvider;
    private readonly IInputScheduler m_inputScheduler;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly CameraController m_cameraController;
    private readonly ImmutableArray<T> m_protosToAssign;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private Panel m_container;
    private GridContainer m_gridContainer;
    private Lyst<VehicleTypeAssignerView> m_views;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; private set; }

    public VehiclesAssignerView(
      ImmutableArray<T> protosToAssign,
      UnlockedProtosDb unlockedProtosDb,
      CameraController cameraController,
      IInputScheduler inputScheduler,
      IVehiclesManager vehiclesManager,
      Func<IEntityAssignedWithVehicles> entityProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_views = new Lyst<VehicleTypeAssignerView>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cameraController = cameraController;
      this.m_protosToAssign = protosToAssign;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_inputScheduler = inputScheduler.CheckNotNull<IInputScheduler>();
      this.m_vehiclesManager = vehiclesManager.CheckNotNull<IVehiclesManager>();
      this.m_entityProvider = entityProvider.CheckNotNull<Func<IEntityAssignedWithVehicles>>();
    }

    public VehiclesAssignerView<T> Build(IUiElement parent, UiBuilder builder)
    {
      this.m_container = builder.NewPanel("AssignersContainer").SetBackground(builder.Style.Panel.ItemOverlay).SetHeight<Panel>(60f);
      this.m_gridContainer = builder.NewGridContainer("Grid", parent).SetCellSize(new Vector2(139f, 60f)).SetCellSpacing(15f).SetInnerPadding(Offset.LeftRight(10f)).SetDynamicHeightMode(3).PutToLeftTopOf<GridContainer>((IUiElement) this.m_container, Vector2.zero);
      this.Updater = UpdaterBuilder.Start().Build();
      foreach (T proto in this.m_protosToAssign.OrderBy<float>((Func<T, float>) (p => p.UIOrder)))
      {
        VehicleTypeAssignerView view = new VehicleTypeAssignerView(builder, (DrivingEntityProto) proto, this.m_unlockedProtosDb, this.m_cameraController, this.m_vehiclesManager, this.m_entityProvider, new Action<DrivingEntityProto>(this.addVehicle), new Action<DrivingEntityProto>(this.removeVehicle));
        this.m_gridContainer.Append((IUiElement) view);
        this.m_views.Add(view);
        view.VisibilityChanged += (Action<bool>) (isVisible =>
        {
          this.m_gridContainer.SetItemVisibility((IUiElement) view, isVisible);
          this.m_container.SetHeight<Panel>(this.m_gridContainer.GetRequiredHeight().Max(60f));
          Action<IUiElement> sizeChanged = this.SizeChanged;
          if (sizeChanged == null)
            return;
          sizeChanged((IUiElement) this);
        });
        this.Updater.AddChildUpdater(view.Updater);
      }
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<IEntityAssignedWithVehicles>((Func<IEntityAssignedWithVehicles>) (() => this.m_entityProvider())).Observe<int>((Func<int>) (() => this.m_gridContainer.VisibleItemsCount)).Do((Action<IEntityAssignedWithVehicles, int>) ((_1, _2) =>
      {
        if (this.m_gridContainer.VisibleItemsCount > 0)
          return;
        for (int index = 0; index < this.m_views.Count; ++index)
        {
          VehicleTypeAssignerView view = this.m_views[this.m_views.Count - index - 1];
          if (view.IsHiddenBecauseOwnedIsZero)
          {
            this.m_gridContainer.SetItemVisibility((IUiElement) view, true);
            this.m_container.SetHeight<Panel>(this.m_gridContainer.GetRequiredHeight().Max(60f));
            Action<IUiElement> sizeChanged = this.SizeChanged;
            if (sizeChanged == null)
              break;
            sizeChanged((IUiElement) this);
            break;
          }
        }
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
      return this;
    }

    private void addVehicle(DynamicGroundEntityProto vehicleProto)
    {
      IEntityAssignedWithVehicles entity = this.m_entityProvider();
      bool flag = NotMappedShortcuts.IsBuildMultiple();
      this.m_inputScheduler.ScheduleInputCmd<AssignVehicleTypeToEntityCmd>(new AssignVehicleTypeToEntityCmd((DynamicEntityProto) vehicleProto, entity, flag ? 5 : 1));
    }

    private void removeVehicle(DynamicGroundEntityProto vehicleProto)
    {
      IEntityAssignedWithVehicles entity = this.m_entityProvider();
      bool flag = NotMappedShortcuts.IsBuildMultiple();
      this.m_inputScheduler.ScheduleInputCmd<UnassignVehicleFromEntityCmd>(new UnassignVehicleFromEntityCmd((DynamicEntityProto) vehicleProto, entity, flag ? 5 : 1));
    }
  }
}
