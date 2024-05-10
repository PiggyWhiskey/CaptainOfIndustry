// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.VehicleLimitIncreaseUnlocker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Vehicles;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class VehicleLimitIncreaseUnlocker : UnitUnlockerBase<VehicleLimitIncreaseUnlock>
  {
    private readonly IVehiclesManager m_vehiclesManager;

    public VehicleLimitIncreaseUnlocker(IVehiclesManager vehiclesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_vehiclesManager = vehiclesManager;
    }

    public override void Unlock(IIndexable<VehicleLimitIncreaseUnlock> units)
    {
      foreach (VehicleLimitIncreaseUnlock unit in units)
        this.m_vehiclesManager.IncreaseVehicleLimit(unit.LimitIncrease);
    }
  }
}
