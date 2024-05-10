// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.BeaconMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Beacons;
using Mafi.Core.Simulation;
using Mafi.Unity.Entities;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class BeaconMbFactory : IEntityMbFactory<Beacon>, IFactory<Beacon, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;
    private readonly ISimLoopEvents m_simLoopEvents;

    public BeaconMbFactory(ProtoModelFactory modelFactory, ISimLoopEvents simLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_simLoopEvents = simLoopEvents;
    }

    public EntityMb Create(Beacon reactor)
    {
      Assert.That<Beacon>(reactor).IsNotNull<Beacon>();
      BeaconMb beaconMb = this.m_modelFactory.CreateModelFor<BeaconProto>(reactor.Prototype).AddComponent<BeaconMb>();
      beaconMb.Initialize(reactor, this.m_simLoopEvents);
      return (EntityMb) beaconMb;
    }
  }
}
