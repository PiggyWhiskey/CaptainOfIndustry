// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.VehicleDepotMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class VehicleDepotMbFactory : 
    IEntityMbFactory<VehicleDepot>,
    IFactory<VehicleDepot, EntityMb>
  {
    private readonly LayoutEntityModelFactory m_modelFactory;

    public VehicleDepotMbFactory(LayoutEntityModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
    }

    public EntityMb Create(VehicleDepot depot)
    {
      VehicleDepotMb vehicleDepotMb = this.m_modelFactory.Create((ILayoutEntityProto) depot.Prototype).AddComponent<VehicleDepotMb>();
      vehicleDepotMb.Initialize(depot);
      return (EntityMb) vehicleDepotMb;
    }
  }
}
