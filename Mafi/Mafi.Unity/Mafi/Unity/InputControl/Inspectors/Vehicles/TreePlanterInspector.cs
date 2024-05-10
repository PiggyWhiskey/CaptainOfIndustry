// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.TreePlanterInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.Vehicles;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TreePlanterInspector : VehicleInspector<TreePlanter, TreePlanterWindowView>
  {
    private readonly DependencyResolver m_resolver;

    public TreePlanterInspector(
      InspectorContext inspectorContext,
      IVehiclesManager vehiclesManager,
      LinesFactory linesFactory,
      VehiclesPathabilityOverlayRenderer navOverlayRenderer,
      TerrainCursor terrainCursor,
      CursorPickingManager pickingManager,
      CursorManager cursorManager,
      DependencyResolver resolver,
      UnreachableTerrainDesignationsManager unreachableDesignationsManager,
      TerrainDesignationsRenderer terrainDesignationsRenderer,
      MbBasedEntitiesRenderer entitiesRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext, linesFactory, terrainCursor, pickingManager, cursorManager, entitiesRenderer, navOverlayRenderer, unreachableDesignationsManager, terrainDesignationsRenderer, vehiclesManager);
      this.m_resolver = resolver;
    }

    protected override TreePlanterWindowView GetView()
    {
      return this.m_resolver.Instantiate<TreePlanterWindowView>(new object[1]
      {
        (object) this
      });
    }
  }
}
