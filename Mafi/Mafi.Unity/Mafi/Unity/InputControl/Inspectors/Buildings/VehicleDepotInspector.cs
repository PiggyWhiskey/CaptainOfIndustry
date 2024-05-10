// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.VehicleDepotInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class VehicleDepotInspector : 
    EntityInspector<VehicleDepot, VehicleDepotWindowViewBase<VehicleDepot>>
  {
    private readonly VehicleDepotWindowViewBase<VehicleDepot> m_windowView;

    public VehicleDepotInspector(
      InspectorContext inspectorContext,
      IGameLoopEvents gameLoopEvents,
      IVehiclesManager vehiclesManager,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_windowView = new VehicleDepotWindowViewBase<VehicleDepot>((IEntityInspector<VehicleDepot>) this, protosDb, unlockedProtosDb, vehiclesManager, inspectorContext.AssetsManager, gameLoopEvents);
    }

    protected override VehicleDepotWindowViewBase<VehicleDepot> GetView() => this.m_windowView;
  }
}
