// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.VehiclesReplacementMap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class VehiclesReplacementMap
  {
    private readonly Dict<DrivingEntityProto, Option<DrivingEntityProto>> m_replacementsMap;

    public IEnumerable<DrivingEntityProto> AllVehicles
    {
      get => (IEnumerable<DrivingEntityProto>) this.m_replacementsMap.Keys;
    }

    public VehiclesReplacementMap(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_replacementsMap = new Dict<DrivingEntityProto, Option<DrivingEntityProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      foreach (DrivingEntityProto key in protosDb.All<VehicleDepotProto>().SelectMany<VehicleDepotProto, DrivingEntityProto>((Func<VehicleDepotProto, IEnumerable<DrivingEntityProto>>) (x => x.BuildableEntities.AsEnumerable().OfType<DrivingEntityProto>())).ToSet<DrivingEntityProto>())
        this.m_replacementsMap.Add(key, key.NextTier);
    }

    public Option<DrivingEntityProto> GetReplacementFor(DrivingEntityProto proto)
    {
      return this.m_replacementsMap[proto];
    }

    public void SetReplacementFor(DrivingEntityProto proto, DrivingEntityProto replacement)
    {
      this.m_replacementsMap[proto] = (Option<DrivingEntityProto>) replacement;
    }
  }
}
