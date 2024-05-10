// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.RocketAssemblyBuildingInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class RocketAssemblyBuildingInspector : 
    EntityInspector<RocketAssemblyBuilding, VehicleDepotWindowViewBase<RocketAssemblyBuilding>>
  {
    private readonly VehicleDepotWindowViewBase<RocketAssemblyBuilding> m_windowView;

    public RocketAssemblyBuildingInspector(
      InspectorContext inspectorContext,
      IGameLoopEvents gameLoopEvents,
      IVehiclesManager vehiclesManager,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new VehicleDepotWindowViewBase<RocketAssemblyBuilding>((IEntityInspector<RocketAssemblyBuilding>) this, protosDb, unlockedProtosDb, vehiclesManager, inspectorContext.AssetsManager, gameLoopEvents);
    }

    protected override VehicleDepotWindowViewBase<RocketAssemblyBuilding> GetView()
    {
      return this.m_windowView;
    }
  }
}
