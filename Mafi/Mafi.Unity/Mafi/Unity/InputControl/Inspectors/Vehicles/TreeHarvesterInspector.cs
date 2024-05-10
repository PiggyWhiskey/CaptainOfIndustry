// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.TreeHarvesterInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.Vehicles;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TreeHarvesterInspector : VehicleInspector<TreeHarvester, TreeHarvesterWindowView>
  {
    private readonly TreeHarvesterWindowView m_view;

    public TreeHarvesterInspector(
      InspectorContext inspectorContext,
      IVehiclesManager vehiclesManager,
      VehiclesAssignerFactory vehiclesAssignerFactory,
      LinesFactory linesFactory,
      VehiclesPathabilityOverlayRenderer navOverlayRenderer,
      UnreachableTerrainDesignationsManager unreachableDesignationsManager,
      TerrainDesignationsRenderer terrainDesignationsRenderer,
      MbBasedEntitiesRenderer entitiesRenderer,
      OrbitalCameraModel orbitalCameraModel,
      TerrainCursor terrainCursor,
      CursorPickingManager pickingManager,
      CursorManager cursorManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext, linesFactory, terrainCursor, pickingManager, cursorManager, entitiesRenderer, navOverlayRenderer, unreachableDesignationsManager, terrainDesignationsRenderer, vehiclesManager);
      VehiclesAssignerView<TruckProto> newView = vehiclesAssignerFactory.CreateNewView<TruckProto>((Func<IEntityAssignedWithVehicles>) (() => (IEntityAssignedWithVehicles) this.SelectedEntity));
      this.m_view = new TreeHarvesterWindowView(this, entitiesRenderer, orbitalCameraModel, newView);
      this.m_view.AddUpdater(this.CreateVehiclesUpdater());
    }

    protected override TreeHarvesterWindowView GetView() => this.m_view;
  }
}
