// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.StorageInspectorGeneric`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class StorageInspectorGeneric<T> : EntityInspector<T, StorageWindowView<T>> where T : Storage
  {
    private readonly StorageWindowView<T> m_windowView;

    public StorageInspectorGeneric(
      InspectorContext inspectorContext,
      BuildingsAssigner buildingsAssigner,
      UnlockedProtosDb unlockedProtosDb,
      VehiclesAssignerFactory vehiclesAssignerFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.SetBuildingsAssigner(buildingsAssigner);
      VehiclesAssignerView<TruckProto> newView = vehiclesAssignerFactory.CreateNewView<TruckProto>((Func<IEntityAssignedWithVehicles>) (() => (IEntityAssignedWithVehicles) this.SelectedEntity));
      this.m_windowView = new StorageWindowView<T>(this, unlockedProtosDb, newView);
      this.m_windowView.AddUpdater(this.CreateVehiclesUpdater());
    }

    protected override StorageWindowView<T> GetView() => this.m_windowView;

    public void SetProductToStore(ProductProto product)
    {
      this.InputScheduler.ScheduleInputCmd<StorageSetProductCmd>(new StorageSetProductCmd((Storage) this.SelectedEntity, product));
    }

    public void RemoveStoredProduct()
    {
      this.InputScheduler.ScheduleInputCmd<StorageClearProductCmd>(new StorageClearProductCmd((Storage) this.SelectedEntity));
    }

    public void SetImportSlider(int step) => this.updateSlider(new int?(step), new int?());

    public void SetExportSlider(int step) => this.updateSlider(new int?(), new int?(step));

    private void updateSlider(int? importStep, int? exportStep)
    {
      Assert.That<bool>(importStep.HasValue || exportStep.HasValue).IsTrue();
      this.InputScheduler.ScheduleInputCmd<StorageSetSliderStepCmd>(new StorageSetSliderStepCmd((Storage) this.SelectedEntity, importStep, exportStep));
    }
  }
}
