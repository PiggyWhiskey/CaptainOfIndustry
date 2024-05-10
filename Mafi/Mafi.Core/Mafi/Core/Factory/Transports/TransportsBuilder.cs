// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportsBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Maintenance;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>Helper class that instantiates transports.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportsBuilder
  {
    private readonly TerrainManager m_terrain;
    private readonly EntityContext m_context;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly EntityId.Factory m_idFactory;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly ITransportUpgraderFactory m_upgraderFactory;
    private readonly IEntityMaintenanceProvidersFactory m_maintenanceProvidersFactory;
    private readonly LazyResolve<TransportsManager> m_transportsManager;

    public TransportsBuilder(
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      EntityId.Factory idFactory,
      LazyResolve<TransportsManager> transportsManager,
      TerrainManager terrain,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      ITransportUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_context = context;
      this.m_simLoopEvents = simLoopEvents;
      this.m_idFactory = idFactory;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_upgraderFactory = upgraderFactory;
      this.m_maintenanceProvidersFactory = maintenanceProvidersFactory;
      this.m_transportsManager = transportsManager.CheckNotNull<LazyResolve<TransportsManager>>();
      this.m_terrain = terrain.CheckNotNull<TerrainManager>();
    }

    public Transport FromTrajectory(
      TransportTrajectory trajectory,
      AssetValue transportValue,
      AssetValue constructionCost)
    {
      Assert.That<AssetValue>(constructionCost).HasAllValuesPositive();
      return new Transport(this.m_idFactory.GetNextId(), trajectory, this.m_context, transportValue, constructionCost, this.m_simLoopEvents, this.m_transportsManager.Value, this.m_vehicleBuffersRegistry, this.m_upgraderFactory, this.m_maintenanceProvidersFactory, this.m_terrain);
    }
  }
}
