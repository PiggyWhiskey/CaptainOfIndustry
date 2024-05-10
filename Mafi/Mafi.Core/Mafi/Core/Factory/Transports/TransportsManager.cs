// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using jOgQY3RGtH5fd9qQao;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Notifications;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class TransportsManager : ITransportsPredicates, IPillarsChecker
  {
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<Transport> m_transports;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<EntityId> m_transportsAndPillarsIds;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<EntityId> m_transportsElevatedAndMiniZippersIds;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<Tile2i, TransportPillar> m_pillars;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<EntityId> m_pillarIds;
    private readonly EntitiesManager m_entitiesManager;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly TerrainManager m_terrain;
    private readonly IAssetTransactionManager m_assetTransactions;
    private readonly TransportsConstructionHelper m_constructionHelper;
    private readonly TransportsBuilder m_transportsBuilder;
    private readonly TransportPillarsBuilder m_pillarsBuilder;
    private readonly ProtosDb m_protosDb;
    private readonly IResolver m_resolver;
    private readonly IIoPortsManager m_portsManager;
    private readonly IConstructionManager m_constructionManager;
    private readonly IUpgradesManager m_upgradesManager;
    private readonly EntityCollapseHelper m_collapseHelper;
    public readonly INotificationsManager NotificationsManager;
    private uint m_seqNumber;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Queueue<Transport> m_transportsToCheckForMerge;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Queueue<MiniZipper> m_miniZippersToCheckForRemoval;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Queueue<TransportPillar> m_pillarsToCheck;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Queueue<KeyValuePair<Transport, Tile3i>> m_transportsSupportCheck;
    [DoNotSaveCreateNewOnLoad("new Lyst<Tile3i>(canOmitClearing: true)", 0)]
    private readonly Lyst<Tile3i> m_tilesTmp;
    [DoNotSaveCreateNewOnLoad("new()", 0)]
    private readonly Lyst<TransportPillar> m_pillarsTmp;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IReadOnlySet<Transport> Transports => (IReadOnlySet<Transport>) this.m_transports;

    public IReadOnlyDictionary<Tile2i, TransportPillar> Pillars
    {
      get => (IReadOnlyDictionary<Tile2i, TransportPillar>) this.m_pillars;
    }

    public TransportPillarProto PillarProto => this.m_pillarsBuilder.PillarProto;

    public IProductsManager ProductsManager { get; private set; }

    [DoNotSave(0, null)]
    public Predicate<EntityId> IgnoreTransportsElevatedAndMiniZippersPredicate { get; private set; }

    [DoNotSave(0, null)]
    public Predicate<EntityId> IgnorePillarsPredicate { get; private set; }

    [DoNotSave(0, null)]
    public Predicate<EntityId> IgnoreTransportsAndPillars { get; private set; }

    public TransportsManager(
      EntitiesManager entitiesManager,
      TerrainOccupancyManager occupancyManager,
      TerrainManager terrain,
      IAssetTransactionManager assetTransactions,
      TransportsConstructionHelper constructionHelper,
      TransportsBuilder transportsBuilder,
      TransportPillarsBuilder pillarsBuilder,
      ISimLoopEvents simLoopEvents,
      ProtosDb protosDb,
      IProductsManager productsManager,
      DependencyResolver resolver,
      IIoPortsManager portsManager,
      IConstructionManager constructionManager,
      IUpgradesManager upgradesManager,
      EntityCollapseHelper collapseHelper,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_transports = new Set<Transport>();
      this.m_transportsAndPillarsIds = new Set<EntityId>();
      this.m_transportsElevatedAndMiniZippersIds = new Set<EntityId>();
      this.m_pillars = new Dict<Tile2i, TransportPillar>();
      this.m_pillarIds = new Set<EntityId>();
      this.m_transportsToCheckForMerge = new Queueue<Transport>();
      this.m_miniZippersToCheckForRemoval = new Queueue<MiniZipper>();
      this.m_pillarsToCheck = new Queueue<TransportPillar>();
      this.m_transportsSupportCheck = new Queueue<KeyValuePair<Transport, Tile3i>>();
      this.m_tilesTmp = new Lyst<Tile3i>(true);
      this.m_pillarsTmp = new Lyst<TransportPillar>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_occupancyManager = occupancyManager;
      this.m_terrain = terrain;
      this.m_assetTransactions = assetTransactions;
      this.m_constructionHelper = constructionHelper;
      this.m_transportsBuilder = transportsBuilder;
      this.m_pillarsBuilder = pillarsBuilder;
      this.m_protosDb = protosDb;
      this.ProductsManager = productsManager;
      this.m_resolver = (IResolver) resolver;
      this.m_portsManager = portsManager;
      this.m_constructionManager = constructionManager;
      this.m_upgradesManager = upgradesManager;
      this.m_collapseHelper = collapseHelper;
      this.NotificationsManager = notificationsManager;
      simLoopEvents.UpdateAfterCmdProc.Add<TransportsManager>(this, new Action(this.checkTransportJoinsAndZippers));
      simLoopEvents.UpdateEnd.Add<TransportsManager>(this, new Action(this.checkTransportJoinsAndZippers));
      entitiesManager.StaticEntityAdded.Add<TransportsManager>(this, new Action<IStaticEntity>(this.entityAdded));
      entitiesManager.StaticEntityRemoved.Add<TransportsManager>(this, new Action<IStaticEntity>(this.entityRemoved));
      entitiesManager.OnUpgradeJustPerformed.Add<TransportsManager>(this, new Action<IUpgradableEntity, IEntityProto>(this.checkForMergesAndZipperChangesEntity));
      constructionManager.EntityConstructed.Add<TransportsManager>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      this.initialize();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initialize() => OBqe2IUAeSpOmlOQ4O.TyOaFSuuHy(1, new object[0], (object) this);

    private void reconstructEntityIds()
    {
      foreach (IEntity entity1 in this.m_entitiesManager.Entities)
      {
        if (entity1 is IStaticEntity entity2)
          this.entityAdded(entity2);
      }
    }

    [InitAfterLoad(InitPriority.Low)]
    private void validate()
    {
      this.checkPillarsState();
      this.clearIncompatibleTransportedProducts();
    }

    private void checkPillarsState()
    {
      foreach (TransportPillar transportPillar in this.m_pillars.Values.ToArray<TransportPillar>())
      {
        TransportPillar pillar = transportPillar;
        Tile3i centerTile1;
        if (pillar.IsConstructed)
        {
          bool flag = false;
          for (int addedZ = 0; addedZ < pillar.Height.Value; ++addedZ)
          {
            TerrainOccupancyManager occupancyManager1 = this.m_occupancyManager;
            centerTile1 = pillar.CenterTile;
            Tile3i position1 = centerTile1.AddZ(addedZ);
            Transport transport;
            ref Transport local1 = ref transport;
            if (occupancyManager1.TryGetOccupyingEntityAt<Transport>(position1, out local1))
            {
              flag = true;
              break;
            }
            TerrainOccupancyManager occupancyManager2 = this.m_occupancyManager;
            centerTile1 = pillar.CenterTile;
            Tile3i position2 = centerTile1.AddZ(addedZ);
            LayoutEntity layoutEntity;
            ref LayoutEntity local2 = ref layoutEntity;
            if (occupancyManager2.TryGetOccupyingEntityAt<LayoutEntity>(position2, out local2) && layoutEntity.Prototype is ILayoutEntityProtoWithElevation prototype && prototype.CanBeElevated && (layoutEntity.OccupiedTiles.FindOrDefault((Predicate<OccupiedTileRelative>) (t =>
            {
              Tile3i centerTile2 = layoutEntity.CenterTile;
              Tile2i tile2i = centerTile2.Xy + t.RelCoord;
              centerTile2 = pillar.CenterTile;
              Tile2i xy = centerTile2.Xy;
              return tile2i == xy;
            })).Constraint & LayoutTileConstraint.UsingPillar) != LayoutTileConstraint.None)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            Log.Error(string.Format("Found pillar at {0} with no supported transports, removing.", (object) pillar.CenterTile));
            this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) pillar, EntityRemoveReason.Remove);
          }
        }
        else
        {
          for (int addedZ = 0; addedZ < pillar.Height.Value; ++addedZ)
          {
            TerrainOccupancyManager occupancyManager3 = this.m_occupancyManager;
            centerTile1 = pillar.CenterTile;
            Tile3i position3 = centerTile1.AddZ(addedZ);
            Transport transport;
            ref Transport local3 = ref transport;
            if (occupancyManager3.TryGetOccupyingEntityAt<Transport>(position3, out local3) && transport.IsConstructed)
            {
              pillar.MakeFullyConstructed(true, true);
              break;
            }
            TerrainOccupancyManager occupancyManager4 = this.m_occupancyManager;
            centerTile1 = pillar.CenterTile;
            Tile3i position4 = centerTile1.AddZ(addedZ);
            LayoutEntity layoutEntity;
            ref LayoutEntity local4 = ref layoutEntity;
            if (occupancyManager4.TryGetOccupyingEntityAt<LayoutEntity>(position4, out local4) && layoutEntity.Prototype is ILayoutEntityProtoWithElevation prototype && prototype.CanBeElevated && layoutEntity.IsConstructed)
            {
              pillar.MakeFullyConstructed(true, true);
              break;
            }
          }
        }
      }
    }

    private void clearIncompatibleTransportedProducts()
    {
      ImmutableArray<ProductProto> managedProtos = this.ProductsManager.SlimIdManager.ManagedProtos;
      foreach (Transport transport in this.m_transports)
      {
        if (!transport.TransportedProducts.IsEmpty<TransportedProductMutable>())
        {
          ProductSlimId slimId = transport.TransportedProducts.First<TransportedProductMutable>().SlimId;
          bool flag = transport.Prototype.PortsShape.AllowedProductType == managedProtos[(int) slimId.Value].Type;
          if (flag)
          {
            foreach (TransportedProductMutable transportedProduct in transport.TransportedProducts)
            {
              if (!(transportedProduct.SlimId == slimId))
              {
                if (transport.Prototype.PortsShape.AllowedProductType == managedProtos[(int) transportedProduct.SlimId.Value].Type)
                {
                  slimId = transportedProduct.SlimId;
                }
                else
                {
                  flag = false;
                  break;
                }
              }
            }
          }
          if (!flag)
          {
            AssetValue assetValue = transport.ClearAndReturnTransportedProducts();
            Log.Error(string.Format("Found invalid products on transport '{0}' (shape '{1}'): ", (object) transport, (object) transport.Prototype.PortsShape) + assetValue.ToString());
          }
        }
      }
    }

    public byte GetNextSeqNumber()
    {
      ++this.m_seqNumber;
      return (byte) this.m_seqNumber;
    }

    private void checkTransportJoinsAndZippers()
    {
      while (this.m_transportsToCheckForMerge.IsNotEmpty)
        this.mergeWithConnectedTransportsIfPossible(this.m_transportsToCheckForMerge.Dequeue());
      while (this.m_miniZippersToCheckForRemoval.IsNotEmpty)
        this.removeZipperIfNeeded(this.m_miniZippersToCheckForRemoval.Dequeue());
      while (this.m_pillarsToCheck.IsNotEmpty)
        this.checkPillarAndReplaceIfNeeded(this.m_pillarsToCheck.Dequeue());
      while (this.m_transportsSupportCheck.IsNotEmpty)
      {
        KeyValuePair<Transport, Tile3i> keyValuePair = this.m_transportsSupportCheck.Dequeue();
        this.checkTransportSupportedAt(keyValuePair.Key, keyValuePair.Value);
      }
    }

    private void checkForMergesAndZipperChangesEntity(IEntity entity, IEntityProto previousProto)
    {
      if (!(entity is Transport transport))
        return;
      this.checkForMergesAndZipperChanges(transport);
    }

    private void onEntityConstructed(IStaticEntity e)
    {
      if (!(e is IEntityWithPorts entityWithPorts))
        return;
      if (entityWithPorts is Transport transport)
      {
        this.checkForMergesAndZipperChanges(transport);
        this.m_pillarsTmp.Clear();
        this.FindAttachedPillars(transport, this.m_pillarsTmp);
        foreach (StaticEntity staticEntity in this.m_pillarsTmp)
          staticEntity.MakeFullyConstructed();
        this.m_pillarsTmp.Clear();
      }
      if (entityWithPorts.Prototype is ILayoutEntityProtoWithElevation prototype && prototype.CanBeElevated)
      {
        if (entityWithPorts is LayoutEntity layoutEntity)
        {
          this.m_pillarsTmp.Clear();
          this.FindAttachedPillars(layoutEntity, this.m_pillarsTmp);
          foreach (StaticEntity staticEntity in this.m_pillarsTmp)
            staticEntity.MakeFullyConstructed();
          this.m_pillarsTmp.Clear();
        }
        else
          Log.Error(string.Format("ILayoutEntityProtoWithElevation '{0}' not LayoutEntity {1}", (object) entityWithPorts.Prototype, (object) entityWithPorts));
      }
      if (!entityWithPorts.Ports.IsNotEmpty || entityWithPorts is MiniZipper)
        return;
      foreach (IoPort port in entityWithPorts.Ports)
      {
        if (port.ConnectedPort.HasValue && port.ConnectedPort.Value.OwnerEntity is MiniZipper ownerEntity)
          ownerEntity.StartConstructionIfNotStarted();
      }
    }

    private void checkForMergesAndZipperChanges(Transport transport)
    {
      this.m_transportsToCheckForMerge.Enqueue(transport);
      if (transport.StartInputPort.IsConnected && transport.StartInputPort.ConnectedPort.Value.OwnerEntity is MiniZipper ownerEntity1)
        this.m_miniZippersToCheckForRemoval.Enqueue(ownerEntity1);
      if (!transport.EndOutputPort.IsConnected || !(transport.EndOutputPort.ConnectedPort.Value.OwnerEntity is MiniZipper ownerEntity2))
        return;
      this.m_miniZippersToCheckForRemoval.Enqueue(ownerEntity2);
    }

    private void registerNewEntity(IStaticEntity entity)
    {
      switch (entity)
      {
        case Transport transport:
          this.m_transports.AddAndAssertNew(transport);
          this.m_transportsAndPillarsIds.AddAndAssertNew(transport.Id);
          this.m_transportsElevatedAndMiniZippersIds.AddAndAssertNew(transport.Id);
          break;
        case TransportPillar transportPillar:
          this.m_pillars.AddAndAssertNew(transportPillar.CenterTile.Xy, transportPillar);
          this.m_pillarIds.AddAndAssertNew(transportPillar.Id);
          this.m_transportsAndPillarsIds.AddAndAssertNew(transportPillar.Id);
          break;
        case MiniZipper miniZipper:
          this.m_transportsElevatedAndMiniZippersIds.AddAndAssertNew(miniZipper.Id);
          break;
        default:
          if (!(entity.Prototype is ILayoutEntityProtoWithElevation prototype) || !prototype.CanBeElevated)
            break;
          this.m_transportsElevatedAndMiniZippersIds.AddAndAssertNew(entity.Id);
          break;
      }
    }

    private void entityAdded(IStaticEntity entity)
    {
      this.registerNewEntity(entity);
      if (entity is Transport transport)
        this.checkAffectedPillars(transport);
      if (!(entity.Prototype is ILayoutEntityProtoWithElevation prototype) || !prototype.CanBeElevated)
        return;
      if (entity is LayoutEntity layoutEntity)
        this.checkAffectedPillars(layoutEntity);
      else
        Log.Error(string.Format("ILayoutEntityProtoWithElevation '{0}' not LayoutEntity {1}", (object) entity.Prototype, (object) entity));
    }

    private void entityRemoved(IStaticEntity entity)
    {
      switch (entity)
      {
        case Transport transport:
          this.m_transports.RemoveAndAssert(transport);
          this.m_transportsAndPillarsIds.RemoveAndAssert(transport.Id);
          this.m_transportsElevatedAndMiniZippersIds.RemoveAndAssert(transport.Id);
          MiniZipper entity1;
          if (this.m_occupancyManager.TryGetOccupyingEntityAt<MiniZipper>(transport.StartInputPort.ExpectedConnectedPortCoord, out entity1))
            this.m_miniZippersToCheckForRemoval.Enqueue(entity1);
          MiniZipper entity2;
          if (this.m_occupancyManager.TryGetOccupyingEntityAt<MiniZipper>(transport.EndOutputPort.ExpectedConnectedPortCoord, out entity2))
            this.m_miniZippersToCheckForRemoval.Enqueue(entity2);
          this.checkAffectedPillars(transport);
          break;
        case TransportPillar transportPillar:
          this.m_pillars.RemoveAndAssert(transportPillar.CenterTile.Xy);
          this.m_pillarIds.RemoveAndAssert(transportPillar.Id);
          this.m_transportsAndPillarsIds.RemoveAndAssert(transportPillar.Id);
          OccupiedTileRelative occupiedTile = transportPillar.OccupiedTiles[0];
          Tile3i tile3i = transportPillar.CenterTile.AddY(occupiedTile.FromHeightRel.Value);
          int addedZ = 0;
          for (int index = occupiedTile.VerticalSize.Value; addedZ < index; ++addedZ)
          {
            Tile3i position = tile3i.AddZ(addedZ);
            Transport entity3;
            if (this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(position, out entity3))
              this.m_transportsSupportCheck.Enqueue(Make.Kvp<Transport, Tile3i>(entity3, position));
          }
          break;
        case MiniZipper miniZipper:
          this.m_transportsElevatedAndMiniZippersIds.RemoveAndAssert(miniZipper.Id);
          break;
        default:
          if (entity.Prototype is ILayoutEntityProtoWithElevation prototype && prototype.CanBeElevated)
          {
            if (entity is LayoutEntity layoutEntity)
            {
              this.m_transportsElevatedAndMiniZippersIds.RemoveAndAssert(layoutEntity.Id);
              this.checkAffectedPillars(layoutEntity);
            }
            else
              Log.Error(string.Format("ILayoutEntityProtoWithElevation '{0}' not LayoutEntity {1}", (object) entity.Prototype, (object) entity));
          }
          if (!(entity is IEntityWithPorts entityWithPorts))
            break;
          foreach (IoPort port in entityWithPorts.Ports)
          {
            MiniZipper entity4;
            if (this.m_occupancyManager.TryGetOccupyingEntityAt<MiniZipper>(port.ExpectedConnectedPortCoord, out entity4))
              this.m_miniZippersToCheckForRemoval.Enqueue(entity4);
          }
          break;
      }
    }

    public void FindAttachedPillars(Transport transport, Lyst<TransportPillar> outPillars)
    {
      foreach (TransportSupportableTile supportableTile in transport.Trajectory.SupportableTiles)
      {
        TransportPillar transportPillar;
        if (this.m_pillars.TryGetValue(supportableTile.Position.Xy, out transportPillar) && transportPillar.TopTileHeight >= supportableTile.Position.Height)
          outPillars.Add(transportPillar);
      }
    }

    public void FindAttachedPillars(LayoutEntity layoutEntity, Lyst<TransportPillar> outPillars)
    {
      Tile2i xy = layoutEntity.CenterTile.Xy;
      foreach (OccupiedTileRelative occupiedTile in layoutEntity.OccupiedTiles)
      {
        TransportPillar transportPillar;
        if (this.m_pillars.TryGetValue(xy + occupiedTile.RelCoord, out transportPillar) && transportPillar.TopTileHeight >= layoutEntity.CenterTile.Height)
          outPillars.Add(transportPillar);
      }
    }

    private void checkAffectedPillars(Transport transport)
    {
      ImmutableArray<TransportSupportableTile> supportableTiles = transport.Trajectory.SupportableTiles;
      for (int index = 0; index < supportableTiles.Length; ++index)
      {
        TransportPillar transportPillar;
        if (this.m_pillars.TryGetValue(supportableTiles[index].Position.Xy, out transportPillar))
          this.m_pillarsToCheck.Enqueue(transportPillar);
      }
    }

    private void checkAffectedPillars(LayoutEntity layoutEntity)
    {
      Tile2i xy = layoutEntity.CenterTile.Xy;
      foreach (OccupiedTileRelative occupiedTile in layoutEntity.OccupiedTiles)
      {
        TransportPillar transportPillar;
        if (this.m_pillars.TryGetValue(xy + occupiedTile.RelCoord, out transportPillar))
          this.m_pillarsToCheck.Enqueue(transportPillar);
      }
    }

    private void mergeWithConnectedTransportsIfPossible(Transport transport)
    {
      if (transport.IsDestroyed || transport.ConstructionState != ConstructionState.Constructed)
        return;
      Transport mergedTransport1;
      if (transport.StartInputPort.IsConnected && transport.StartInputPort.ConnectedPort.Value.OwnerEntity is Transport ownerEntity1 && this.tryMergeTransports(ownerEntity1, transport, ReadOnlyArraySlice<Tile3i>.Empty, true, out mergedTransport1))
      {
        this.m_transportsToCheckForMerge.Enqueue(mergedTransport1);
      }
      else
      {
        Transport mergedTransport2;
        if (!transport.EndOutputPort.IsConnected || !(transport.EndOutputPort.ConnectedPort.Value.OwnerEntity is Transport ownerEntity) || !this.tryMergeTransports(transport, ownerEntity, ReadOnlyArraySlice<Tile3i>.Empty, true, out mergedTransport2))
          return;
        this.m_transportsToCheckForMerge.Enqueue(mergedTransport2);
      }
    }

    /// <summary>
    /// Tries to merge the given transports to one. The first transport's end point must be directed at the first
    /// connecting pivot, and the second transport's start point must be directed at the last connecting pivot.
    /// If no connecting pivots are given, both transports should point at each other. Both transport must be
    /// constructed.
    /// </summary>
    private bool tryMergeTransports(
      Transport firstT,
      Transport secondT,
      ReadOnlyArraySlice<Tile3i> connectingPivots,
      bool mergeEvenIfTooLong,
      out Transport mergedTransport)
    {
      mergedTransport = (Transport) null;
      if ((Proto) firstT.Prototype != (Proto) secondT.Prototype || firstT == secondT || firstT.IsDestroyed || secondT.IsDestroyed || firstT.ConstructionState != secondT.ConstructionState)
        return false;
      ConstructionState constructionState = firstT.ConstructionState;
      switch (constructionState)
      {
        case ConstructionState.InConstruction:
          if (firstT.ConstructionProgress.Value.CurrentSteps != 0 || secondT.ConstructionProgress.Value.CurrentSteps != 0)
            return false;
          goto case ConstructionState.Constructed;
        case ConstructionState.Constructed:
          if (!mergeEvenIfTooLong && firstT.Trajectory.Waypoints.Length + secondT.Trajectory.Waypoints.Length > 16383)
            return false;
          ImmutableArray<Tile3i> immutableArray;
          if (connectingPivots.IsEmpty)
          {
            if (firstT.EndPosition + firstT.EndDirection.ToTileDirection() != secondT.StartPosition || secondT.StartPosition + secondT.StartDirection.ToTileDirection() != firstT.EndPosition)
            {
              Log.Error("Trying to merge transports that are not next to each other.");
              return false;
            }
            immutableArray = firstT.Trajectory.Pivots.Concat(secondT.Trajectory.Pivots);
          }
          else
          {
            if (firstT.EndPosition + firstT.EndDirection.ToTileDirection() != connectingPivots.First || secondT.StartPosition + secondT.StartDirection.ToTileDirection() != connectingPivots.Last)
            {
              Log.Error("Trying to merge transports that are not connected to given connecting pivots.");
              return false;
            }
            Lyst<Tile3i> lyst = new Lyst<Tile3i>(firstT.Trajectory.Pivots.Length + connectingPivots.Length + secondT.Trajectory.Pivots.Length, true);
            lyst.AddRange(firstT.Trajectory.Pivots);
            lyst.AddRange(connectingPivots);
            lyst.AddRange(secondT.Trajectory.Pivots);
            immutableArray = lyst.ToImmutableArrayAndClear();
          }
          TransportTrajectory trajectory;
          LocStrFormatted error;
          if (!this.TryCreateTrajectory(firstT.Prototype, immutableArray.AsSlice, firstT.StartDirection, secondT.EndDirection, false, out trajectory, out bool _, out error))
          {
            Log.Error("Failed to merge transports due to invalid trajectory. " + error.ToString());
            return false;
          }
          if (!mergeEvenIfTooLong && trajectory.Waypoints.Length > 16383)
            return false;
          AssetValue transportValue = firstT.Value + secondT.Value;
          mergedTransport = this.m_transportsBuilder.FromTrajectory(trajectory, transportValue, constructionState == ConstructionState.Constructed ? AssetValue.Empty : transportValue);
          mergedTransport.TransferProductsSharedStart(firstT);
          mergedTransport.TransferProductsSharedEnd(secondT);
          AssetValue assetValue = firstT.ClearAndReturnTransportedProducts() + secondT.ClearAndReturnTransportedProducts();
          if (assetValue.IsNotEmpty)
          {
            Log.Error("Some products were not transferred on merge.");
            this.ProductsManager.ProductDestroyed(assetValue, DestroyReason.Cleared);
          }
          this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) firstT, EntityRemoveReason.Remove);
          this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) secondT, EntityRemoveReason.Remove);
          this.m_entitiesManager.AddEntityNoChecks((IEntity) mergedTransport);
          if (constructionState == ConstructionState.Constructed)
            mergedTransport.MakeFullyConstructed(true, true);
          return true;
        default:
          return false;
      }
    }

    private static bool checkNotDestructing(IStaticEntity entity)
    {
      bool flag;
      switch (entity.ConstructionState)
      {
        case ConstructionState.PendingDeconstruction:
        case ConstructionState.InDeconstruction:
        case ConstructionState.Deconstructed:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return !flag;
    }

    private static bool checkNotAlreadyConstructing(IStaticEntity entity)
    {
      return !entity.ConstructionProgress.HasValue || entity.ConstructionProgress.Value.CurrentSteps <= 0 && !entity.ConstructionProgress.Value.GetAssetValueOfBuffers().IsNotEmpty;
    }

    /// <summary>
    /// Removes the given mini-zipper if any of the following conditions are met:
    /// 1) Mini-zipper has no ports connected.
    /// 2) Mini-zipper has only 1 port connected, and the connected port is a transport, and the transport can be
    ///    extended to replace that mini-zipper. If any of the previous conditions are not met, mini-zipper stays.
    /// 3) Mini-zipper has exactly 2 connected transports and they can be connected through the removed mini-zipper.
    ///    If ports are not two transports, or the transports cannot be connected (due to direction for example),
    ///    the mini-zipper stays.
    /// 4) If mini-zipper has connected 1 transport and 1 non-transport, and the transport can be extended to
    ///    connect to the non-transport port.
    /// </summary>
    private void removeZipperIfNeeded(MiniZipper miniZipper)
    {
      if (miniZipper.IsDestroyed)
        return;
      int num = miniZipper.Ports.Count((Func<IoPort, bool>) (x => x.IsConnected));
      if (num > 2)
        return;
      if (num <= 1)
      {
        this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) miniZipper, EntityRemoveReason.Remove);
      }
      else
      {
        bool flag1 = false;
        bool flag2 = false;
        foreach (IoPort port in miniZipper.Ports)
        {
          if (port.IsConnected)
          {
            if (port.ConnectedPort.Value.Type != IoPortType.Output)
              flag1 = true;
            else if (port.ConnectedPort.Value.Type != IoPortType.Input)
              flag2 = true;
          }
        }
        if (!flag1 || !flag2)
        {
          Assert.That<int>(num).IsEqualTo(2);
        }
        else
        {
          IoPort ioPort1 = (IoPort) null;
          IoPort ioPort2 = (IoPort) null;
          foreach (IoPort ioPort3 in miniZipper.Ports.Where((Func<IoPort, bool>) (x => x.IsConnected)))
          {
            if (ioPort1 == null)
            {
              ioPort1 = ioPort3;
            }
            else
            {
              ioPort2 = ioPort3;
              break;
            }
          }
          Assert.That<IoPort>(ioPort1).IsNotNull<IoPort>();
          Assert.That<IoPort>(ioPort2).IsNotNull<IoPort>();
          Transport ownerEntity1 = ioPort1?.ConnectedPort.Value.OwnerEntity as Transport;
          Transport ownerEntity2 = ioPort2?.ConnectedPort.Value.OwnerEntity as Transport;
          if (ownerEntity1 != null && ownerEntity2 != null && TransportsManager.checkNotDestructing((IStaticEntity) ownerEntity1) && TransportsManager.checkNotDestructing((IStaticEntity) ownerEntity2))
          {
            if (ownerEntity1 == ownerEntity2 || (Proto) ownerEntity1.Prototype != (Proto) ownerEntity2.Prototype || ioPort1.ConnectedPort.Value.Type == ioPort2.ConnectedPort.Value.Type)
              return;
            Transport mergedTransport;
            if (ioPort1.ConnectedPort.Value.Type == IoPortType.Output)
              this.tryMergeTransports(ownerEntity1, ownerEntity2, new Tile3i[1]
              {
                miniZipper.CenterTile
              }.AsSlice<Tile3i>(), false, out mergedTransport);
            else
              this.tryMergeTransports(ownerEntity2, ownerEntity1, new Tile3i[1]
              {
                miniZipper.CenterTile
              }.AsSlice<Tile3i>(), false, out mergedTransport);
          }
          else if (ownerEntity1 != null)
          {
            tryExtendTransportIntoMiniZipper(ownerEntity1, ioPort1, ioPort2);
          }
          else
          {
            if (ownerEntity2 == null)
              return;
            tryExtendTransportIntoMiniZipper(ownerEntity2, ioPort2, ioPort1);
          }
        }
      }

      void tryExtendTransportIntoMiniZipper(
        Transport transport,
        IoPort zipperPort,
        IoPort otherPort)
      {
        if (transport.ConstructionState != ConstructionState.Constructed)
          return;
        bool flag = transport.Trajectory.Pivots.First == zipperPort.ExpectedConnectedPortCoord;
        ImmutableArray<Tile3i> pivots;
        Direction903d direction903d1;
        Direction903d direction903d2;
        if (flag)
        {
          pivots = transport.Trajectory.Pivots.Insert(0, miniZipper.CenterTile);
          direction903d1 = otherPort.Direction;
          direction903d2 = transport.EndDirection;
        }
        else
        {
          pivots = transport.Trajectory.Pivots.Add(miniZipper.CenterTile);
          direction903d1 = transport.StartDirection;
          direction903d2 = otherPort.Direction;
        }
        CanBuildTransportResult result;
        if (!this.m_constructionHelper.CanBuildOrJoinTransport(transport.Prototype, pivots, Set<Tile2i>.Empty, new Direction903d?(direction903d1), new Direction903d?(direction903d2), true, out result, out LocStrFormatted _, ignoreForJoining: (Predicate<Transport>) (t => true)))
          return;
        if (result.PivotsWereReversed)
        {
          Log.Error("Extending transports into miniZipper tried to reverse transport.");
        }
        else
        {
          ConstructionState constructionState = transport.ConstructionState;
          Percent constrProgress;
          AssetValue availableMaterials;
          if (transport.ConstructionProgress.HasValue)
          {
            constrProgress = transport.ConstructionProgress.Value.Progress;
            availableMaterials = this.m_constructionManager.CancelConstructionAndReturnBuffers((IStaticEntity) transport);
          }
          else
          {
            constrProgress = Percent.Zero;
            availableMaterials = AssetValue.Empty;
          }
          this.m_constructionManager.ResetConstructionAnimationState((IStaticEntity) transport);
          this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) miniZipper, EntityRemoveReason.Remove);
          this.m_entitiesManager.RemoveEntityNoChecks((IEntity) transport);
          Option<Transport> transport1;
          this.buildOrJoinTransport(result, out transport1, out Option<MiniZipper> _, out Option<MiniZipper> _);
          if (transport1.HasValue)
          {
            if (flag)
              transport1.Value.TransferProductsSharedEnd(transport);
            else
              transport1.Value.TransferProductsSharedStart(transport);
            availableMaterials = this.updateConstructionStateOfNewTransport(constructionState, transport1.Value, availableMaterials, constrProgress);
          }
          else
            Log.Error("Failed to build transport after mini-zipper extension.");
          this.ProductsManager.ProductDestroyed(availableMaterials, DestroyReason.Wasted);
          AssetValue assetValue = transport.ClearAndReturnTransportedProducts();
          if (assetValue.IsNotEmpty)
          {
            Log.Error("Some products were not transferred on mini-zipper extension.");
            this.ProductsManager.ProductDestroyed(assetValue, DestroyReason.Cleared);
          }
          ((IEntityFriend) transport).Destroy();
        }
      }
    }

    /// <summary>
    /// Checks if pillar has transport at its top. If not, shortens or extends the pillar, or removes it
    /// when no transports are being supported anymore.
    /// </summary>
    private void checkPillarAndReplaceIfNeeded(TransportPillar pillar)
    {
      if (pillar.IsDestroyed)
        return;
      ThicknessTilesI height = ThicknessTilesI.Zero;
      EntityId id = pillar.Id;
      Predicate<EntityId> ignoredIds = (Predicate<EntityId>) (x => x == id);
      bool flag = false;
      for (int addedZ = TransportPillarProto.MAX_PILLAR_HEIGHT.Value - 1; addedZ >= 0; --addedZ)
      {
        Tile3i position = pillar.CenterTile.AddZ(addedZ);
        Transport entity1;
        if (this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(position, out entity1, ignoredIds))
        {
          if (entity1.Trajectory.SupportableTiles.Any((Func<TransportSupportableTile, bool>) (x => x.Position == position)) && (entity1.Prototype.NeedsPillarsAtGround || !TransportHelper.IsAtGroundWithNoNeedForPillarBelow(this.m_terrain[position.Xy], position.Height)))
            flag |= entity1.IsConstructed;
          else
            continue;
        }
        else
        {
          LayoutEntity entity2;
          if (this.m_occupancyManager.TryGetOccupyingEntityAt<LayoutEntity>(position, out entity2, ignoredIds) && entity2.Prototype is ILayoutEntityProtoWithElevation prototype)
          {
            flag |= entity2.IsConstructed;
            if (!prototype.CanPillarsPassThrough)
            {
              height = new ThicknessTilesI(addedZ + 1);
              continue;
            }
          }
          else
            continue;
        }
        height = new ThicknessTilesI(addedZ + 1);
        break;
      }
      this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) pillar, EntityRemoveReason.Remove);
      if (!height.IsPositive)
        return;
      TransportPillar transportPillar = this.m_pillarsBuilder.Create(pillar.CenterTile, height);
      this.m_entitiesManager.AddEntityNoChecks((IEntity) transportPillar);
      if (!flag)
        return;
      transportPillar.MakeFullyConstructed(true, true);
    }

    private void checkTransportSupportedAt(Transport transport, Tile3i position)
    {
      if (transport.IsDestroyed)
        return;
      bool positionIsUnsupported;
      int unsupportedStartIndex;
      int unsupportedEndIndex;
      this.m_constructionHelper.FindUnsupportedRegionAround(transport.Trajectory, position, out positionIsUnsupported, out unsupportedStartIndex, out unsupportedEndIndex);
      if (!positionIsUnsupported)
        return;
      int num1 = unsupportedEndIndex - unsupportedStartIndex + 1;
      Assert.That<int>(num1).IsPositive();
      ImmutableArray<TransportSupportableTile> supportableTiles;
      int index;
      int num2;
      if (unsupportedStartIndex == 0)
      {
        int num3 = unsupportedEndIndex;
        supportableTiles = transport.Trajectory.SupportableTiles;
        int num4 = supportableTiles.Length - 1;
        if (num3 >= num4)
        {
          this.m_collapseHelper.TryDestroyEntityAndAddRubble((IStaticEntity) transport);
          return;
        }
        if (num1 <= transport.Prototype.MaxPillarSupportRadius.Value)
          return;
        index = unsupportedStartIndex;
        num2 = unsupportedEndIndex - transport.Prototype.MaxPillarSupportRadius.Value;
      }
      else
      {
        int num5 = unsupportedEndIndex;
        supportableTiles = transport.Trajectory.SupportableTiles;
        int num6 = supportableTiles.Length - 1;
        if (num5 >= num6)
        {
          if (num1 <= transport.Prototype.MaxPillarSupportRadius.Value)
            return;
          index = unsupportedStartIndex + transport.Prototype.MaxPillarSupportRadius.Value;
          num2 = unsupportedEndIndex;
        }
        else
        {
          if (num1 <= 2 * transport.Prototype.MaxPillarSupportRadius.Value)
            return;
          index = unsupportedStartIndex + transport.Prototype.MaxPillarSupportRadius.Value;
          num2 = unsupportedEndIndex - transport.Prototype.MaxPillarSupportRadius.Value;
        }
      }
      Assert.That<int>(index).IsLessOrEqual(num2);
      supportableTiles = transport.Trajectory.SupportableTiles;
      Tile3i position1 = supportableTiles[index].Position;
      supportableTiles = transport.Trajectory.SupportableTiles;
      Tile3i position2 = supportableTiles[num2].Position;
      LocStrFormatted error;
      if (this.TryCollapseSubTransport(transport, position1, position2, out Option<Transport> _, out Option<Transport> _, out error))
        return;
      Log.Error("Failed to collapse transport: " + error.ToString());
    }

    /// <summary>
    /// Tries to create a transport trajectory from given pivots and optional start/end directions and returns
    /// <c>true</c> if it was successfully created.
    /// 
    /// If start/end directions are not provided, they will "snap" to a first matching neighboring port, or
    /// they will be chosen based on transport direction so that they are valid.
    /// 
    /// This also verifies that transport has a correct direction and in case of miss match (e.g. start connected
    /// to input port) the order of pivots will be reversed. If both connected ports are input (or output),
    /// this will fail.
    /// 
    /// Note that <paramref name="trajectory" /> may be set even when <c>false</c> is returned. This "invalid"
    /// trajectory can be still shown to user along with error message.
    /// </summary>
    public bool TryCreateTrajectorySnapToPorts(
      TransportProto proto,
      ReadOnlyArraySlice<Tile3i> pivots,
      Direction903d? startDirection,
      Direction903d? endDirection,
      bool allowReversing,
      bool disablePortSnapping,
      out TransportTrajectory trajectory,
      out bool wasReversed,
      out LocStrFormatted error)
    {
      RelTile3i resolvedStartDir;
      RelTile3i resolvedEndDir;
      if (disablePortSnapping)
        this.ComputeTrajDirections(pivots, startDirection, endDirection, out resolvedStartDir, out resolvedEndDir);
      else
        this.ComputeTrajDirectionsSnapToPorts(proto, pivots, startDirection, endDirection, out resolvedStartDir, out resolvedEndDir);
      TransportTrajectory trajectory1;
      bool trajectory2 = this.TryCreateTrajectory(proto, pivots, resolvedStartDir, resolvedEndDir, allowReversing, out trajectory1, out wasReversed, out error);
      trajectory = trajectory1;
      return trajectory2;
    }

    public bool TryCreateTrajectory(
      TransportProto proto,
      ReadOnlyArraySlice<Tile3i> pivots,
      Direction903d startDirection,
      Direction903d endDirection,
      bool allowReversing,
      out TransportTrajectory trajectory,
      out bool wasReversed,
      out LocStrFormatted error)
    {
      return this.TryCreateTrajectory(proto, pivots, startDirection.ToTileDirection(), endDirection.ToTileDirection(), allowReversing, out trajectory, out wasReversed, out error);
    }

    public bool TryCreateTrajectory(
      TransportProto proto,
      ReadOnlyArraySlice<Tile3i> pivots,
      RelTile3i startDirection,
      RelTile3i endDirection,
      bool allowReversing,
      out TransportTrajectory trajectory,
      out bool wasReversed,
      out LocStrFormatted error)
    {
      wasReversed = false;
      if (pivots.IsEmpty)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransport;
        trajectory = (TransportTrajectory) null;
        return false;
      }
      if (allowReversing)
      {
        IoPortType? portTypeFor1 = getPortTypeFor(pivots.First, startDirection);
        IoPortType? portTypeFor2 = getPortTypeFor(pivots.Last, endDirection);
        IoPortType? nullable;
        bool flag1;
        if (portTypeFor1.HasValue)
        {
          nullable = portTypeFor1;
          IoPortType ioPortType1 = IoPortType.Any;
          if (!(nullable.GetValueOrDefault() == ioPortType1 & nullable.HasValue))
          {
            nullable = portTypeFor1;
            IoPortType ioPortType2 = IoPortType.Input;
            flag1 = nullable.GetValueOrDefault() == ioPortType2 & nullable.HasValue;
            if (portTypeFor2.HasValue)
            {
              nullable = portTypeFor2;
              IoPortType ioPortType3 = IoPortType.Any;
              if (!(nullable.GetValueOrDefault() == ioPortType3 & nullable.HasValue))
              {
                nullable = portTypeFor2;
                IoPortType ioPortType4 = IoPortType.Output;
                bool flag2 = nullable.GetValueOrDefault() == ioPortType4 & nullable.HasValue;
                if (flag1 != flag2)
                {
                  flag1 = false;
                  goto label_13;
                }
                else
                  goto label_13;
              }
              else
                goto label_13;
            }
            else
              goto label_13;
          }
        }
        if (portTypeFor2.HasValue)
        {
          nullable = portTypeFor2;
          IoPortType ioPortType = IoPortType.Any;
          if (!(nullable.GetValueOrDefault() == ioPortType & nullable.HasValue))
          {
            flag1 = portTypeFor2.Value == IoPortType.Output;
            goto label_13;
          }
        }
        flag1 = false;
label_13:
        if (flag1)
        {
          pivots = pivots.Reversed();
          Swap.Them<RelTile3i>(ref startDirection, ref endDirection);
          wasReversed = true;
        }
      }
      this.m_tilesTmp.Clear();
      this.m_tilesTmp.AddRange(pivots);
      TransportTrajectory.OptimizePivots(this.m_tilesTmp);
      ImmutableArray<Tile3i> immutableArrayAndClear = this.m_tilesTmp.ToImmutableArrayAndClear();
      if (!TransportTrajectory.TryCreateFromPivots(proto, immutableArrayAndClear, new RelTile3i?(startDirection), new RelTile3i?(endDirection), out trajectory, out string _))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__InvalidTransport;
        return false;
      }
      error = LocStrFormatted.Empty;
      return true;

      IoPortType? getPortTypeFor(Tile3i position, RelTile3i direction)
      {
        if (direction.IsZero)
          return new IoPortType?();
        IoPort port;
        return this.m_portsManager.TryGetPortAt(position + direction, -direction.ToDirection903d(), proto.PortsShape, out port) && !port.IsConnected ? new IoPortType?(port.Type) : new IoPortType?();
      }
    }

    /// <summary>
    /// Tests whether a mini-zipper is necessary to be placed based on the surrounding ports.
    /// </summary>
    public bool IsMiniZipperPlacementNeeded(
      TransportProto proto,
      Tile3i position,
      out MiniZipperAtResult? result)
    {
      if (!this.m_portsManager.IsAnyPortFacingTo(position) || this.m_occupancyManager.IsOccupiedAt(position))
      {
        result = new MiniZipperAtResult?();
        return false;
      }
      int num1 = 0;
      int num2 = 0;
      foreach (Direction903d allSixDirection in Direction903d.AllSixDirections)
      {
        IoPort port;
        if (this.m_portsManager.TryGetPortAt(position + new RelTile3i(allSixDirection.DirectionVector), -allSixDirection, proto.PortsShape, out port) && TransportsManager.checkNotDestructing((IStaticEntity) port.OwnerEntity))
        {
          if (port.Type == IoPortType.Input)
            ++num1;
          else if (port.Type == IoPortType.Output)
          {
            ++num2;
          }
          else
          {
            result = new MiniZipperAtResult?();
            return false;
          }
        }
      }
      if (num1 >= 2 || num2 >= 2)
      {
        Option<MiniZipperProto> option = this.m_protosDb.Get<MiniZipperProto>((Proto.ID) IdsCore.Transports.GetMiniZipperIdFor(proto.PortsShape.Id));
        if (option.HasValue)
        {
          result = new MiniZipperAtResult?(new MiniZipperAtResult(option.Value, position));
          return true;
        }
      }
      result = new MiniZipperAtResult?();
      return false;
    }

    /// <summary>
    /// Port snapping used in <see cref="M:Mafi.Core.Factory.Transports.TransportsManager.TryCreateTrajectorySnapToPorts(Mafi.Core.Factory.Transports.TransportProto,Mafi.Collections.ReadonlyCollections.ReadOnlyArraySlice{Mafi.Tile3i},System.Nullable{Mafi.Direction903d},System.Nullable{Mafi.Direction903d},System.Boolean,System.Boolean,Mafi.Core.Factory.Transports.TransportTrajectory@,System.Boolean@,Mafi.Localization.LocStrFormatted@)" />.
    /// </summary>
    public void ComputeTrajDirectionsSnapToPorts(
      TransportProto proto,
      ReadOnlyArraySlice<Tile3i> pivots,
      Direction903d? startDirection,
      Direction903d? endDirection,
      out RelTile3i resolvedStartDir,
      out RelTile3i resolvedEndDir,
      bool disablePortSnapping = false)
    {
      if (pivots.IsNotEmpty && !disablePortSnapping)
      {
        IoPortType selectedType = IoPortType.Any;
        Lyst<Direction903d> forbiddenDirections = new Lyst<Direction903d>(true);
        RelTile3i relTile3i;
        Direction90 direction90;
        if (pivots.Length == 1)
        {
          if (endDirection.HasValue)
            forbiddenDirections.Add(endDirection.Value);
        }
        else
        {
          relTile3i = pivots.Second - pivots.First;
          RelTile2i xy = relTile3i.Xy;
          if (xy.IsZero)
          {
            Lyst<Direction903d> lyst = forbiddenDirections;
            relTile3i = pivots.Second - pivots.First;
            Direction903d direction903d = relTile3i.ToDirection903d();
            lyst.Add(direction903d);
          }
          else
          {
            Lyst<Direction903d> lyst = forbiddenDirections;
            direction90 = xy.ToDirection90();
            Direction903d as3d1 = direction90.As3d;
            lyst.Add(as3d1);
            if (pivots.Second.Z - pivots.First.Z != 0)
            {
              direction90 = xy.ToDirection90();
              direction90 = direction90.RotatedPlus90;
              Direction903d as3d2 = direction90.As3d;
              Direction903d? nullable = startDirection;
              Direction903d direction903d1 = as3d2;
              if ((nullable.HasValue ? (nullable.GetValueOrDefault() != direction903d1 ? 1 : 0) : 1) != 0)
                forbiddenDirections.Add(as3d2);
              direction90 = xy.ToDirection90();
              direction90 = direction90.RotatedMinus90;
              Direction903d as3d3 = direction90.As3d;
              nullable = startDirection;
              Direction903d direction903d2 = as3d3;
              if ((nullable.HasValue ? (nullable.GetValueOrDefault() != direction903d2 ? 1 : 0) : 1) != 0)
                forbiddenDirections.Add(as3d3);
            }
          }
        }
        if (!startDirection.HasValue || forbiddenDirections.Contains(startDirection.Value))
        {
          startDirection = this.getPortDirectionAt(proto, pivots.First, forbiddenDirections, IoPortType.Output, out selectedType);
        }
        else
        {
          IoPort port;
          if (this.m_portsManager.TryGetPortAt(pivots.First + startDirection.Value.ToTileDirection(), -startDirection.Value, proto.PortsShape, out port))
            selectedType = port.Type;
        }
        if (!endDirection.HasValue)
        {
          forbiddenDirections.Clear();
          if (pivots.Length == 1)
          {
            if (startDirection.HasValue)
              forbiddenDirections.Add(startDirection.Value);
          }
          else
          {
            relTile3i = pivots.PreLast - pivots.Last;
            RelTile2i xy = relTile3i.Xy;
            if (xy.IsZero)
            {
              Lyst<Direction903d> lyst = forbiddenDirections;
              relTile3i = pivots.PreLast - pivots.Last;
              Direction903d direction903d = relTile3i.ToDirection903d();
              lyst.Add(direction903d);
            }
            else
            {
              Lyst<Direction903d> lyst1 = forbiddenDirections;
              direction90 = xy.ToDirection90();
              Direction903d as3d4 = direction90.As3d;
              lyst1.Add(as3d4);
              if (pivots.PreLast.Z - pivots.Last.Z != 0)
              {
                Lyst<Direction903d> lyst2 = forbiddenDirections;
                direction90 = xy.ToDirection90();
                direction90 = direction90.RotatedPlus90;
                Direction903d as3d5 = direction90.As3d;
                lyst2.Add(as3d5);
                Lyst<Direction903d> lyst3 = forbiddenDirections;
                direction90 = xy.ToDirection90();
                direction90 = direction90.RotatedMinus90;
                Direction903d as3d6 = direction90.As3d;
                lyst3.Add(as3d6);
              }
            }
          }
          IoPortType portPreference = selectedType == IoPortType.Input ? IoPortType.Output : IoPortType.Input;
          endDirection = this.getPortDirectionAt(proto, pivots.Last, forbiddenDirections, portPreference, out IoPortType _);
        }
      }
      this.ComputeTrajDirections(pivots, startDirection, endDirection, out resolvedStartDir, out resolvedEndDir);
    }

    public void ComputeTrajDirections(
      ReadOnlyArraySlice<Tile3i> pivots,
      Direction903d? startDirection,
      Direction903d? endDirection,
      out RelTile3i resolvedStartDir,
      out RelTile3i resolvedEndDir)
    {
      TransportTrajectory.ComputeStartAndEndDirections(pivots, startDirection.HasValue ? new RelTile3i?(startDirection.GetValueOrDefault().ToTileDirection()) : new RelTile3i?(), endDirection.HasValue ? new RelTile3i?(endDirection.GetValueOrDefault().ToTileDirection()) : new RelTile3i?(), out resolvedStartDir, out resolvedEndDir);
    }

    private Direction903d? getPortDirectionAt(
      TransportProto proto,
      Tile3i position,
      Lyst<Direction903d> forbiddenDirections,
      IoPortType portPreference,
      out IoPortType selectedType)
    {
      selectedType = IoPortType.Any;
      if (!this.m_portsManager.IsAnyPortFacingTo(position))
        return new Direction903d?();
      Direction903d? portDirectionAt = new Direction903d?();
      foreach (Direction903d allSixDirection in Direction903d.AllSixDirections)
      {
        bool flag = false;
        foreach (Direction903d forbiddenDirection in forbiddenDirections)
        {
          if (forbiddenDirection == allSixDirection)
          {
            flag = true;
            break;
          }
        }
        IoPort port;
        if (!flag && this.m_portsManager.TryGetPortAt(position + allSixDirection.ToTileDirection(), -allSixDirection, proto.PortsShape, out port) && TransportsManager.checkNotDestructing((IStaticEntity) port.OwnerEntity))
        {
          portDirectionAt = new Direction903d?(allSixDirection);
          selectedType = port.Type;
          if (port.Type == portPreference)
            break;
        }
      }
      return portDirectionAt;
    }

    /// <summary>
    /// Verifies whether a transport constructed from given parameters is buildable. This also considers joining
    /// a transport with mini-zippers or changing direction of existing transports.
    /// 
    /// If a transport is not buildable, the <paramref name="result" /> still may have some useful data on what was
    /// attempted (like a trajectory) that can be displayed to the player.
    /// 
    /// If start/end directions are not provided, they will "snap" to a first matching neighboring port, or
    /// they will be chosen based on transport direction so that they are valid.
    /// 
    /// This also verifies that transport has a correct direction and in case of miss match (e.g. start connected
    /// to input port) the order of pivots will be reversed. If both connected ports are input (or output),
    /// this will fail.
    /// 
    /// Note that resulting transport pivots may be different (shorter and/or reversed) than provided.
    /// </summary>
    public bool CanBuildOrJoinTransport(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      IReadOnlySet<Tile2i> pillarHints,
      Direction903d? startDirection,
      Direction903d? endDirection,
      bool disablePortSnapping,
      out CanBuildTransportResult result,
      out LocStrFormatted error,
      out Option<IStaticEntity> blockingEntity,
      bool ignorePillars = false,
      bool disallowMiniZipAtStart = false,
      bool disallowMiniZipAtEnd = false,
      bool skipExtraPillarsForBetterVisuals = false)
    {
      blockingEntity = Option<IStaticEntity>.None;
      if (!this.m_constructionHelper.CanBuildOrJoinTransport(proto, pivots, pillarHints, startDirection, endDirection, disablePortSnapping, out result, out error, ignorePillars, disallowMiniZipAtStart: disallowMiniZipAtStart, disallowMiniZipAtEnd: disallowMiniZipAtEnd, skipExtraPillarsForBetterVisuals: skipExtraPillarsForBetterVisuals) || result.NewTrajectory.HasValue && !this.canBuildTransport(result.NewTrajectory.Value, this.IgnorePillarsPredicate, out error, out blockingEntity))
        return false;
      error = LocStrFormatted.Empty;
      return true;
    }

    private bool canBuildTransport(
      TransportTrajectory trajectory,
      Predicate<EntityId> ignoreForCollisions,
      out LocStrFormatted error,
      out Option<IStaticEntity> blockingEntity)
    {
      blockingEntity = Option<IStaticEntity>.None;
      foreach (OccupiedTileRange occupiedTile in trajectory.OccupiedTiles)
      {
        if (this.m_terrain.IsBlockingBuildings(this.m_terrain.GetTileIndex(occupiedTile.Position)))
        {
          error = (LocStrFormatted) TrCore.AdditionError__SomethingInWay;
          return false;
        }
      }
      TerrainOccupancyManager occupancyManager1 = this.m_occupancyManager;
      ImmutableArray<OccupiedTileRange> occupiedTiles = trajectory.OccupiedTiles;
      ReadOnlyArraySlice<OccupiedTileRange> asSlice1 = occupiedTiles.AsSlice;
      EntityId id;
      ref EntityId local = ref id;
      Predicate<EntityId> ignoredIds = ignoreForCollisions;
      if (!occupancyManager1.CanAdd(asSlice1, out local, ignoredIds))
      {
        IStaticEntity entity;
        this.m_entitiesManager.TryGetEntity<IStaticEntity>(id, out entity);
        blockingEntity = entity.CreateOption<IStaticEntity>();
        error = TrCore.TrAdditionError__Blocked.Format(entity.GetTitle());
        return false;
      }
      TerrainOccupancyManager occupancyManager2 = this.m_occupancyManager;
      occupiedTiles = trajectory.OccupiedTiles;
      ReadOnlyArraySlice<OccupiedTileRange> asSlice2 = occupiedTiles.AsSlice;
      if (occupancyManager2.AreTilesSelfColliding(asSlice2))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__SelfColliding;
        return false;
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    /// <summary>
    /// Tries to build a transport supported by pillars on given tiles.
    /// 
    /// This verifies that:
    /// 1) Transport itself can be built based on occupancy. The given trajectory is assumed valid.
    ///    Use <see cref="M:Mafi.Core.Factory.Transports.TransportsManager.TryCreateTrajectorySnapToPorts(Mafi.Core.Factory.Transports.TransportProto,Mafi.Collections.ReadonlyCollections.ReadOnlyArraySlice{Mafi.Tile3i},System.Nullable{Mafi.Direction903d},System.Nullable{Mafi.Direction903d},System.Boolean,System.Boolean,Mafi.Core.Factory.Transports.TransportTrajectory@,System.Boolean@,Mafi.Localization.LocStrFormatted@)" /> or <see cref="M:Mafi.Core.Factory.Transports.TransportTrajectory.TryCreateFromPivots(Mafi.Core.Factory.Transports.TransportProto,Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Tile3i},System.Nullable{Mafi.RelTile3i},System.Nullable{Mafi.RelTile3i},Mafi.Core.Factory.Transports.TransportTrajectory@,System.String@,System.Boolean,System.Boolean)" />
    ///    to create one.
    /// 2) Transporting has no self-collisions (this is not guaranteed by <see cref="T:Mafi.Core.Factory.Transports.TransportTrajectory" />).
    /// 3) Transport is adequately supported on given 2D tiles by existing or new pillars.
    /// 
    /// It is OK to have extra or invalid pillars in the pillars array, but they will be discarded. If transport
    /// is still valid without them, it will be built.
    /// </summary>
    public bool TryBuildOrJoinTransport(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      Direction903d startDirection,
      Direction903d endDirection,
      out Option<Transport> transport,
      out LocStrFormatted error)
    {
      return this.TryBuildOrJoinTransport(proto, pivots, Set<Tile2i>.Empty, new Direction903d?(startDirection), new Direction903d?(endDirection), true, out transport, out Option<MiniZipper> _, out Option<MiniZipper> _, out error, out CanBuildTransportResult _, out Option<IStaticEntity> _);
    }

    /// <summary>Allows port snapping.</summary>
    public bool TryBuildOrJoinTransport(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      Direction903d? startDirection,
      Direction903d? endDirection,
      bool disablePortSnapping,
      out Option<Transport> transport,
      out LocStrFormatted error)
    {
      return this.TryBuildOrJoinTransport(proto, pivots, Set<Tile2i>.Empty, startDirection, endDirection, disablePortSnapping, out transport, out Option<MiniZipper> _, out Option<MiniZipper> _, out error, out CanBuildTransportResult _, out Option<IStaticEntity> _);
    }

    /// <summary>Exposes more info about the build attempt.</summary>
    public bool TryBuildOrJoinTransport(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      IReadOnlySet<Tile2i> pillarHints,
      Direction903d? startDirection,
      Direction903d? endDirection,
      bool disablePortSnapping,
      out Option<Transport> transport,
      out Option<MiniZipper> miniZipperAtStart,
      out Option<MiniZipper> miniZipperAtEnd,
      out LocStrFormatted error,
      out CanBuildTransportResult buildResult,
      out Option<IStaticEntity> blockingEntity,
      bool skipExtraPillarsForBetterVisuals = false)
    {
      if (this.CanBuildOrJoinTransport(proto, pivots, pillarHints, startDirection, endDirection, disablePortSnapping, out buildResult, out error, out blockingEntity, skipExtraPillarsForBetterVisuals: skipExtraPillarsForBetterVisuals))
      {
        this.buildOrJoinTransport(buildResult, out transport, out miniZipperAtStart, out miniZipperAtEnd);
        return true;
      }
      transport = Option<Transport>.None;
      miniZipperAtStart = Option<MiniZipper>.None;
      miniZipperAtEnd = Option<MiniZipper>.None;
      return false;
    }

    private void buildOrJoinTransport(
      CanBuildTransportResult buildResult,
      out Option<Transport> transport,
      out Option<MiniZipper> mzAtStart,
      out Option<MiniZipper> mzAtEnd)
    {
      if (buildResult.NewTrajectory.HasValue)
      {
        Transport transport1 = this.m_transportsBuilder.FromTrajectory(buildResult.NewTrajectory.Value, buildResult.NewTransportValue, buildResult.NewTransportValue);
        this.m_entitiesManager.AddEntityNoChecks((IEntity) transport1);
        transport = (Option<Transport>) transport1;
        foreach (Tile3i supportedTile in buildResult.SupportedTiles)
          this.BuildOrExtendPillarNoChecks(supportedTile.Xy, supportedTile.Height, true);
      }
      else
        transport = Option<Transport>.None;
      processChangeDirection(buildResult.ChangeDirectionNearStart);
      processChangeDirection(buildResult.ChangeDirectionNearEnd);
      if (buildResult.MiniZipperAtStart.HasValue)
      {
        Assert.That<CanPlaceMiniZipperAtResult?>(buildResult.MiniZipJoinResultAtStart).IsNone<CanPlaceMiniZipperAtResult>();
        MiniZipperAtResult miniZipperAtResult = buildResult.MiniZipperAtStart.Value;
        mzAtStart = this.buildMiniZipperAt(miniZipperAtResult.ZipperProto, miniZipperAtResult.Position, AssetValue.Empty);
      }
      else
        mzAtStart = processMiniZipJoin(buildResult.MiniZipJoinResultAtStart);
      if (buildResult.MiniZipperAtEnd.HasValue)
      {
        Assert.That<CanPlaceMiniZipperAtResult?>(buildResult.MiniZipJoinResultAtEnd).IsNone<CanPlaceMiniZipperAtResult>();
        MiniZipperAtResult miniZipperAtResult = buildResult.MiniZipperAtEnd.Value;
        mzAtEnd = this.buildMiniZipperAt(miniZipperAtResult.ZipperProto, miniZipperAtResult.Position, AssetValue.Empty);
      }
      else
        mzAtEnd = processMiniZipJoin(buildResult.MiniZipJoinResultAtEnd);

      static void processChangeDirection(CanChangeDirectionResult? result)
      {
        string error;
        if (!result.HasValue || result.Value.Transport.TryChangeDirection(result.Value.NewDirection, result.Value.ChangeAtStart, out error))
          return;
        Log.Error("Failed to change transport direction. " + error);
      }

      Option<MiniZipper> processMiniZipJoin(CanPlaceMiniZipperAtResult? result)
      {
        if (!result.HasValue)
          return Option<MiniZipper>.None;
        AssetValue cutOutProducts;
        this.cutOutTransport(result.Value.CutOutResult.ExtendToFullCutOutResult(), false, out Option<Transport> _, out Option<Transport> _, out Option<Transport> _, out cutOutProducts);
        return this.buildMiniZipperAt(result.Value.ZipperProto, result.Value.CutOutResult.CutOutPosition, cutOutProducts);
      }
    }

    internal Transport CreateAndAddTransportNoChecks(
      TransportTrajectory trajectory,
      bool addPillars)
    {
      AssetValue priceFor = trajectory.TransportProto.GetPriceFor(trajectory.Pivots);
      Transport transportNoChecks = this.m_transportsBuilder.FromTrajectory(trajectory, priceFor, priceFor);
      this.m_entitiesManager.AddEntityNoChecks((IEntity) transportNoChecks);
      ImmutableArray<Tile3i> supportedTiles;
      if (addPillars && this.m_constructionHelper.TryFindPillarsForTrajectory(trajectory, Set<Tile2i>.Empty, out supportedTiles, out int _, out int _))
      {
        foreach (Tile3i tile3i in supportedTiles)
          this.BuildOrExtendPillarNoChecks(tile3i.Xy, tile3i.Height);
      }
      return transportNoChecks;
    }

    internal TransportPillar BuildOrReplacePillarNoChecks(Tile3i origin, ThicknessTilesI height)
    {
      TransportPillar transportPillar1;
      if (this.m_pillars.TryGetValue(origin.Xy, out transportPillar1))
        this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) transportPillar1, EntityRemoveReason.Remove);
      TransportPillar transportPillar2 = this.m_pillarsBuilder.Create(origin, height);
      this.m_entitiesManager.AddEntityNoChecks((IEntity) transportPillar2);
      return transportPillar2;
    }

    public bool CanBuildMiniZipperAt(
      MiniZipperProto proto,
      Tile3i position,
      out CanPlaceMiniZipperAtResult? result,
      out LocStrFormatted error)
    {
      if (this.m_terrain.IsOffLimitsOrInvalid(position.Tile2i))
      {
        error = (LocStrFormatted) TrCore.AdditionError__OutsideOfMap;
        result = new CanPlaceMiniZipperAtResult?();
        return false;
      }
      if (this.m_terrain.IsOcean(this.m_terrain.GetTileIndex(position.Tile2i)) && position.Z < 1)
      {
        error = (LocStrFormatted) TrCore.AdditionError__OceanNotAllowed;
        result = new CanPlaceMiniZipperAtResult?();
        return false;
      }
      IStaticEntity entity;
      if (!this.m_occupancyManager.TryGetOccupyingEntityAt<IStaticEntity>(position, out entity, this.IgnorePillarsPredicate))
      {
        error = LocStrFormatted.Empty;
        result = new CanPlaceMiniZipperAtResult?();
        return true;
      }
      if (!(entity is Transport transport))
      {
        error = TrCore.TrAdditionError__Blocked.Format(entity.GetTitle());
        result = new CanPlaceMiniZipperAtResult?();
        return false;
      }
      TransportsConstructionHelper constructionHelper = this.m_constructionHelper;
      Tile3i miniZipperPosition = position;
      CanPlaceMiniZipperAtResult miniZipperAtResult;
      ref CanPlaceMiniZipperAtResult local1 = ref miniZipperAtResult;
      ref LocStrFormatted local2 = ref error;
      Direction903d? bannedMiniZipperConnection = new Direction903d?();
      if (!constructionHelper.CanPlaceMiniZipperAt(transport, miniZipperPosition, out local1, out local2, bannedMiniZipperConnection))
      {
        result = new CanPlaceMiniZipperAtResult?();
        return false;
      }
      if ((Proto) miniZipperAtResult.ZipperProto != (Proto) proto)
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__TypesNoMatch;
        result = new CanPlaceMiniZipperAtResult?();
        return false;
      }
      result = new CanPlaceMiniZipperAtResult?(miniZipperAtResult);
      return true;
    }

    public void CutOutTransportForMiniZipper(CanPlaceMiniZipperAtResult result)
    {
      AssetValue cutOutProducts;
      this.cutOutTransport(result.CutOutResult.ExtendToFullCutOutResult(), false, out Option<Transport> _, out Option<Transport> _, out Option<Transport> _, out cutOutProducts);
      this.ProductsManager.ProductDestroyed(cutOutProducts, DestroyReason.Cleared);
    }

    private Option<MiniZipper> buildMiniZipperAt(
      MiniZipperProto proto,
      Tile3i position,
      AssetValue initialProducts)
    {
      Option<MiniZipper> option = this.m_resolver.TryInvokeFactoryHierarchy<StaticEntity>((object) proto, (object) new TileTransform(position, Rotation90.Deg0)).As<MiniZipper>();
      if (option.IsNone)
      {
        Log.Error("Failed to crete mini zipper!");
        return Option<MiniZipper>.None;
      }
      MiniZipper miniZipper = option.Value;
      Assert.That<Quantity>(miniZipper.TotalQuantityInBuffers).IsZero();
      if (initialProducts.IsNotEmpty)
      {
        foreach (ProductQuantity product in initialProducts.Products)
          miniZipper.PushProductsToBuffer(product);
      }
      this.m_entitiesManager.AddEntityNoChecks((IEntity) miniZipper);
      return option;
    }

    internal bool TryCutOutTransport(
      Transport transport,
      Tile3i cutOutFrom,
      Tile3i cutOutTo,
      bool expandCutOverRampsInsteadOfFailing,
      bool createCutOutTrajectory,
      out Option<Transport> startSubTransport,
      out Option<TransportTrajectory> cutOutSubTrajectory,
      out Option<Transport> endSubTransport,
      out AssetValue cutOutProducts,
      out LocStrFormatted error)
    {
      startSubTransport = Option<Transport>.None;
      cutOutSubTrajectory = Option<TransportTrajectory>.None;
      endSubTransport = Option<Transport>.None;
      cutOutProducts = AssetValue.Empty;
      if (!TransportsManager.checkNotDestructing((IStaticEntity) transport))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__BeingDestroyed;
        return false;
      }
      if (!TransportsManager.checkNotAlreadyConstructing((IStaticEntity) transport))
      {
        error = (LocStrFormatted) TrCore.TrCutError__ConstructionAlreadyStarted;
        return false;
      }
      CanCutOutTransportResult result;
      if (!TransportsConstructionHelper.CanCutOutTransport(transport, cutOutFrom, cutOutTo, expandCutOverRampsInsteadOfFailing, createCutOutTrajectory, out result, out error))
        return false;
      cutOutSubTrajectory = result.CutOutSubTransport;
      this.cutOutTransport(result, false, out startSubTransport, out Option<Transport> _, out endSubTransport, out cutOutProducts);
      return true;
    }

    internal bool TryCutOutTransport(
      Transport transport,
      Tile3i cutOutFrom,
      Tile3i cutOutTo,
      bool expandCutOverRampsInsteadOfFailing,
      bool createCutOutTransport,
      out Option<Transport> startSubTransport,
      out Option<Transport> cutOutSubTransport,
      out Option<Transport> endSubTransport,
      out AssetValue cutOutProducts,
      out LocStrFormatted error)
    {
      startSubTransport = Option<Transport>.None;
      cutOutSubTransport = Option<Transport>.None;
      endSubTransport = Option<Transport>.None;
      cutOutProducts = AssetValue.Empty;
      if (!TransportsManager.checkNotDestructing((IStaticEntity) transport))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__BeingDestroyed;
        return false;
      }
      if (!TransportsManager.checkNotAlreadyConstructing((IStaticEntity) transport))
      {
        error = (LocStrFormatted) TrCore.TrCutError__ConstructionAlreadyStarted;
        return false;
      }
      CanCutOutTransportResult result;
      if (!TransportsConstructionHelper.CanCutOutTransport(transport, cutOutFrom, cutOutTo, expandCutOverRampsInsteadOfFailing, createCutOutTransport, out result, out error))
        return false;
      this.cutOutTransport(result, createCutOutTransport, out startSubTransport, out cutOutSubTransport, out endSubTransport, out cutOutProducts);
      return true;
    }

    internal bool TryCutOutTransportAt(
      Transport transport,
      Tile3i cutOutPosition,
      out Option<Transport> startSubTransport,
      out Option<Transport> endSubTransport,
      out AssetValue cutOutProducts,
      out LocStrFormatted error)
    {
      startSubTransport = Option<Transport>.None;
      endSubTransport = Option<Transport>.None;
      cutOutProducts = AssetValue.Empty;
      if (!TransportsManager.checkNotDestructing((IStaticEntity) transport))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__BeingDestroyed;
        return false;
      }
      CanCutOutTransportAtResult result;
      if (!TransportsConstructionHelper.CanCutOutTransportAt(transport, cutOutPosition, out result, out error))
        return false;
      this.cutOutTransport(result.ExtendToFullCutOutResult(), false, out startSubTransport, out Option<Transport> _, out endSubTransport, out cutOutProducts);
      return true;
    }

    private void cutOutTransport(
      CanCutOutTransportResult canCutResult,
      bool createCutOutTransport,
      out Option<Transport> startSubTransport,
      out Option<Transport> cutOutSubTransport,
      out Option<Transport> endSubTransport,
      out AssetValue cutOutProducts)
    {
      Transport replacedTransport = canCutResult.ReplacedTransport;
      if (replacedTransport.IsDestroyed)
      {
        startSubTransport = Option<Transport>.None;
        cutOutSubTransport = Option<Transport>.None;
        endSubTransport = Option<Transport>.None;
        cutOutProducts = AssetValue.Empty;
        Log.Error("Trying to cut out destroyed transport.");
      }
      else
      {
        ConstructionState constructionState = replacedTransport.ConstructionState;
        int count1 = replacedTransport.TransportedProducts.Count;
        Percent constrProgress;
        bool flag1;
        AssetValue availableMaterials;
        bool flag2;
        if (replacedTransport.ConstructionProgress.HasValue)
        {
          constrProgress = replacedTransport.ConstructionProgress.Value.Progress;
          flag1 = replacedTransport.ConstructionProgress.Value.IsPaused;
          availableMaterials = this.m_constructionManager.CancelConstructionAndReturnBuffers((IStaticEntity) replacedTransport);
          flag2 = replacedTransport.IsProductsRemovalInProgress;
        }
        else
        {
          constrProgress = Percent.Zero;
          availableMaterials = AssetValue.Empty;
          flag1 = false;
          flag2 = false;
        }
        if (canCutResult.StartSubTransport.HasValue)
        {
          TransportTrajectory trajectory = canCutResult.StartSubTransport.Value;
          AssetValue priceFor = trajectory.TransportProto.GetPriceFor(trajectory.Pivots);
          startSubTransport = (Option<Transport>) this.m_transportsBuilder.FromTrajectory(trajectory, priceFor, priceFor);
          startSubTransport.Value.TransferProductsSharedStart(replacedTransport);
        }
        else
          startSubTransport = Option<Transport>.None;
        if (canCutResult.EndSubTransport.HasValue)
        {
          TransportTrajectory trajectory = canCutResult.EndSubTransport.Value;
          AssetValue priceFor = trajectory.TransportProto.GetPriceFor(trajectory.Pivots);
          endSubTransport = (Option<Transport>) this.m_transportsBuilder.FromTrajectory(trajectory, priceFor, priceFor);
          endSubTransport.Value.TransferProductsSharedEnd(replacedTransport);
        }
        else
          endSubTransport = Option<Transport>.None;
        if (canCutResult.CutOutSubTransport.HasValue & createCutOutTransport)
        {
          TransportTrajectory trajectory = canCutResult.CutOutSubTransport.Value;
          AssetValue cutOutValue = canCutResult.ComputeCutOutValue();
          cutOutSubTransport = (Option<Transport>) this.m_transportsBuilder.FromTrajectory(trajectory, cutOutValue, cutOutValue);
          RelTile1f srcProductsDistFromStartBias = startSubTransport.HasValue ? -startSubTransport.Value.Trajectory.TrajectoryLength : RelTile1f.Zero;
          cutOutSubTransport.Value.TransferProductsSharedStart(replacedTransport, srcProductsDistFromStartBias);
        }
        else
          cutOutSubTransport = Option<Transport>.None;
        Transport valueOrNull1 = startSubTransport.ValueOrNull;
        int count2 = valueOrNull1 != null ? valueOrNull1.TransportedProducts.Count : 0;
        Transport valueOrNull2 = cutOutSubTransport.ValueOrNull;
        int count3 = valueOrNull2 != null ? valueOrNull2.TransportedProducts.Count : 0;
        int num = count2 + count3;
        Transport valueOrNull3 = endSubTransport.ValueOrNull;
        int count4 = valueOrNull3 != null ? valueOrNull3.TransportedProducts.Count : 0;
        Assert.That<int>(num + count4 + replacedTransport.TransportedProducts.Count).IsEqualTo(count1, "Error in products transfer.");
        Assert.That<bool>(replacedTransport.TransportedProducts.Count <= 1 || cutOutSubTransport.IsNone).IsTrue<int>("There is {0} transported products remaining on the original transport despite cut-out transport being constructed.", replacedTransport.TransportedProducts.Count);
        cutOutProducts = replacedTransport.ClearAndReturnTransportedProducts();
        this.m_constructionManager.ResetConstructionAnimationState((IStaticEntity) replacedTransport);
        this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) replacedTransport, EntityRemoveReason.Remove);
        if (startSubTransport.HasValue)
        {
          Transport transport = startSubTransport.Value;
          this.m_entitiesManager.AddEntityNoChecks((IEntity) transport);
          availableMaterials = this.updateConstructionStateOfNewTransport(constructionState, transport, availableMaterials, constrProgress);
          if (flag1 && !transport.ConstructionProgress.Value.IsPaused)
            this.m_constructionManager.TrySetConstructionPause((IStaticEntity) transport, true).AssertTrue();
          if (flag2)
            transport.RequestProductsRemoval();
          transport.SetPaused(replacedTransport.IsPaused);
        }
        if (endSubTransport.HasValue)
        {
          Transport transport = endSubTransport.Value;
          this.m_entitiesManager.AddEntityNoChecks((IEntity) transport);
          availableMaterials = this.updateConstructionStateOfNewTransport(constructionState, transport, availableMaterials, constrProgress);
          if (flag1 && !transport.ConstructionProgress.Value.IsPaused)
            this.m_constructionManager.TrySetConstructionPause((IStaticEntity) transport, true).AssertTrue();
          if (flag2)
            transport.RequestProductsRemoval();
          transport.SetPaused(replacedTransport.IsPaused);
        }
        if (cutOutSubTransport.HasValue)
        {
          Transport newTransport = cutOutSubTransport.Value;
          this.m_entitiesManager.AddEntityNoChecks((IEntity) newTransport);
          availableMaterials = this.updateConstructionStateOfNewTransport(constructionState, newTransport, availableMaterials, constrProgress);
          newTransport.SetPaused(replacedTransport.IsPaused);
        }
        this.ProductsManager.ProductDestroyed(availableMaterials, DestroyReason.Wasted);
      }
    }

    public bool TryCollapseSubTransport(
      Transport transport,
      Tile3i from,
      Tile3i to,
      out Option<Transport> startSubTransport,
      out Option<Transport> endSubTransport,
      out LocStrFormatted error)
    {
      CanCutOutTransportResult result;
      if (!TransportsConstructionHelper.CanCutOutTransport(transport, from, to, true, true, out result, out error))
      {
        startSubTransport = Option<Transport>.None;
        endSubTransport = Option<Transport>.None;
        return false;
      }
      Option<Transport> cutOutSubTransport;
      AssetValue cutOutProducts;
      this.cutOutTransport(result, true, out startSubTransport, out cutOutSubTransport, out endSubTransport, out cutOutProducts);
      this.ProductsManager.ProductDestroyed(cutOutProducts, DestroyReason.Cleared);
      if (cutOutSubTransport.HasValue)
      {
        this.m_collapseHelper.TryDestroyEntityAndAddRubble((IStaticEntity) cutOutSubTransport.Value).AssertTrue("Transport collapse failed");
        return true;
      }
      Log.Error("No cut out transport?");
      return false;
    }

    public bool TryDeconstructSubTransport(
      Transport transport,
      Tile3i from,
      Tile3i to,
      out Option<Transport> startSubTransport,
      out Option<Transport> deconstructedSubTransport,
      out Option<Transport> endSubTransport,
      out LocStrFormatted error)
    {
      if (!TransportsManager.checkNotDestructing((IStaticEntity) transport))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__BeingDestroyed;
        startSubTransport = Option<Transport>.None;
        deconstructedSubTransport = Option<Transport>.None;
        endSubTransport = Option<Transport>.None;
        return false;
      }
      CanCutOutTransportResult result;
      if (!TransportsConstructionHelper.CanCutOutTransport(transport, from, to, true, true, out result, out error))
      {
        startSubTransport = Option<Transport>.None;
        deconstructedSubTransport = Option<Transport>.None;
        endSubTransport = Option<Transport>.None;
        return false;
      }
      AssetValue cutOutProducts;
      this.cutOutTransport(result, true, out startSubTransport, out deconstructedSubTransport, out endSubTransport, out cutOutProducts);
      this.ProductsManager.ProductDestroyed(cutOutProducts, DestroyReason.Cleared);
      if (deconstructedSubTransport.HasValue)
      {
        this.m_constructionManager.StartDeconstruction((IStaticEntity) deconstructedSubTransport.Value);
        bool flag;
        switch (deconstructedSubTransport.Value.ConstructionState)
        {
          case ConstructionState.InDeconstruction:
          case ConstructionState.Deconstructed:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        Assert.That<bool>(flag).IsTrue();
      }
      else
        Log.Error("No cut-out transport after cut.");
      return true;
    }

    private AssetValue updateConstructionStateOfNewTransport(
      ConstructionState constrState,
      Transport newTransport,
      AssetValue availableMaterials,
      Percent constrProgress)
    {
      switch (constrState)
      {
        case ConstructionState.InConstruction:
          Assert.That<Option<IEntityConstructionProgress>>(newTransport.ConstructionProgress).HasValue<IEntityConstructionProgress>();
          availableMaterials = newTransport.Context.ConstructionManager.FillConstructionBuffersWith((IStaticEntity) newTransport, availableMaterials, new Percent?(constrProgress));
          this.m_constructionManager.ResetConstructionAnimationState((IStaticEntity) newTransport);
          break;
        case ConstructionState.Constructed:
          newTransport.MakeFullyConstructed(true, true);
          break;
        case ConstructionState.PreparingUpgrade:
        case ConstructionState.BeingUpgraded:
          Assert.That<Option<IEntityConstructionProgress>>(newTransport.ConstructionProgress).HasValue<IEntityConstructionProgress>();
          newTransport.MakeFullyConstructed(true, true);
          if (this.m_upgradesManager.TryStartUpgrade((IUpgradableEntity) newTransport))
          {
            availableMaterials = this.m_upgradesManager.FillUpgradeBuffersWith((IUpgradableEntity) newTransport, availableMaterials, new Percent?(constrProgress));
            this.m_constructionManager.ResetConstructionAnimationState((IStaticEntity) newTransport);
            break;
          }
          Log.Error("Failed to start upgrade of transport.");
          break;
        case ConstructionState.InDeconstruction:
          Assert.That<Option<IEntityConstructionProgress>>(newTransport.ConstructionProgress).HasValue<IEntityConstructionProgress>();
          newTransport.MakeFullyConstructed(true, true);
          this.m_constructionManager.StartDeconstruction((IStaticEntity) newTransport, true, createEmptyBuffersWithCapacity: true);
          availableMaterials = this.m_constructionManager.FillConstructionBuffersWith((IStaticEntity) newTransport, availableMaterials, new Percent?(constrProgress));
          this.m_constructionManager.ResetConstructionAnimationState((IStaticEntity) newTransport);
          break;
        default:
          Log.Error(string.Format("Invalid construction state of split transport: {0}", (object) constrState));
          newTransport.MakeFullyConstructed(true, true);
          break;
      }
      return availableMaterials;
    }

    public bool TryReverseTransport(Transport transport, out LocStrFormatted error)
    {
      if (!TransportsManager.checkNotDestructing((IStaticEntity) transport))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__BeingDestroyed;
        return false;
      }
      if (!transport.TryReverse(out string _))
      {
        error = (LocStrFormatted) TrCore.TrAdditionError__CannotReverse;
        return false;
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    internal bool TryBuildPillarAt_TestOnly(Tile2i position, ThicknessTilesI height)
    {
      HeightTilesI pillarBaseHeight = TransportHelper.GetPillarBaseHeight(this.m_terrain[position]);
      return this.TryBuildPillarAt_TestOnly(position, pillarBaseHeight + height - ThicknessTilesI.One);
    }

    internal bool TryBuildPillarAt_TestOnly(Tile2i position, HeightTilesI topTileHeight)
    {
      HeightTilesI baseHeight;
      ThicknessTilesI newHeight;
      if (!this.CanBuildPillarAt(position, topTileHeight, out baseHeight, out newHeight))
        return false;
      this.m_entitiesManager.AddEntityNoChecks((IEntity) this.m_pillarsBuilder.Create(position.ExtendHeight(baseHeight), newHeight));
      return true;
    }

    public bool HasPillarAt(
      Tile2i position,
      HeightTilesI topTileHeight,
      out TransportPillar existingPillar)
    {
      return this.m_pillars.TryGetValue(position, out existingPillar) && !(topTileHeight < existingPillar.CenterTile.Height) && topTileHeight <= existingPillar.TopTileHeight;
    }

    public bool CanBuildPillarAt(
      Tile2i position,
      HeightTilesI topTileHeight,
      out HeightTilesI baseHeight,
      out ThicknessTilesI newHeight)
    {
      TerrainTile tile = this.m_terrain[position];
      if (this.m_terrain.IsBlockingBuildings(tile.DataIndex))
      {
        baseHeight = new HeightTilesI();
        newHeight = new ThicknessTilesI();
        return false;
      }
      baseHeight = TransportHelper.GetPillarBaseHeight(tile);
      newHeight = topTileHeight - baseHeight + ThicknessTilesI.One;
      return !newHeight.IsNotPositive && !(newHeight > TransportPillarProto.MAX_PILLAR_HEIGHT) && !this.m_occupancyManager.TryGetAnyOccupyingEntityInRange(position.ExtendHeight(baseHeight), newHeight, out EntityId _, this.IgnoreTransportsElevatedAndMiniZippersPredicate);
    }

    public bool CanExtendPillarAt(
      Tile2i position,
      HeightTilesI topTileHeight,
      out TransportPillar existingPillar,
      out ThicknessTilesI newHeight)
    {
      newHeight = new ThicknessTilesI();
      if (!this.m_pillars.TryGetValue(position, out existingPillar))
        return false;
      if (topTileHeight < existingPillar.CenterTile.Height)
      {
        Log.Warning("Attempting to extend pivot below its base?");
        return false;
      }
      newHeight = topTileHeight - existingPillar.CenterTile.Height + ThicknessTilesI.One;
      ThicknessTilesI verticalSize = newHeight - existingPillar.Height;
      return verticalSize.IsNotPositive || !(newHeight > TransportPillarProto.MAX_PILLAR_HEIGHT) && !this.m_occupancyManager.TryGetAnyOccupyingEntityInRange(position.ExtendHeight(existingPillar.CenterTile.Height + existingPillar.Height), verticalSize, out EntityId _, this.IgnoreTransportsElevatedAndMiniZippersPredicate);
    }

    public HeightTilesI? GetMaxPillarHeightAt(Tile2i position)
    {
      TransportPillar transportPillar;
      if (this.m_pillars.TryGetValue(position, out transportPillar))
      {
        ThicknessTilesI verticalSize = TransportPillarProto.MAX_PILLAR_HEIGHT - transportPillar.Height;
        if (verticalSize.IsNotPositive)
          return new HeightTilesI?(transportPillar.CenterTile.Height + TransportPillarProto.MAX_PILLAR_HEIGHT);
        EntityId entityId;
        if (!this.m_occupancyManager.TryGetAnyOccupyingEntityInRange(position.ExtendHeight(transportPillar.CenterTile.Height + transportPillar.Height), verticalSize, out entityId, this.IgnoreTransportsElevatedAndMiniZippersPredicate))
          return new HeightTilesI?(transportPillar.CenterTile.Height + TransportPillarProto.MAX_PILLAR_HEIGHT);
        HeightTilesI heightTilesI = transportPillar.CenterTile.Height + transportPillar.Height;
        for (int index = 0; index < verticalSize.Value; ++index)
        {
          if (this.m_occupancyManager.TryGetAnyOccupyingEntityAt(position.ExtendHeight(transportPillar.CenterTile.Height + transportPillar.Height + new ThicknessTilesI(index)), out entityId, this.IgnoreTransportsElevatedAndMiniZippersPredicate))
            return new HeightTilesI?(heightTilesI);
          heightTilesI += ThicknessTilesI.One;
        }
        Log.Warning("All checks passed but single check for the entire pillar range did not?");
        return new HeightTilesI?(heightTilesI);
      }
      TerrainTile tile = this.m_terrain[position];
      if (this.m_terrain.IsBlockingBuildings(tile.DataIndex))
        return new HeightTilesI?();
      HeightTilesI pillarBaseHeight = TransportHelper.GetPillarBaseHeight(tile);
      EntityId entityId1;
      if (!this.m_occupancyManager.TryGetAnyOccupyingEntityInRange(position.ExtendHeight(pillarBaseHeight), TransportPillarProto.MAX_PILLAR_HEIGHT, out entityId1, this.IgnoreTransportsElevatedAndMiniZippersPredicate))
        return new HeightTilesI?(pillarBaseHeight + TransportPillarProto.MAX_PILLAR_HEIGHT);
      HeightTilesI? maxPillarHeightAt = new HeightTilesI?();
      for (int index = 0; index < TransportPillarProto.MAX_PILLAR_HEIGHT.Value; ++index)
      {
        if (this.m_occupancyManager.TryGetAnyOccupyingEntityAt(position.ExtendHeight(pillarBaseHeight + new ThicknessTilesI(index)), out entityId1, this.IgnoreTransportsElevatedAndMiniZippersPredicate))
          return maxPillarHeightAt;
        maxPillarHeightAt = new HeightTilesI?(!maxPillarHeightAt.HasValue ? pillarBaseHeight + ThicknessTilesI.One : maxPillarHeightAt.Value + ThicknessTilesI.One);
      }
      Log.Warning("All checks passed but single check for the entire pillar range did not?");
      return maxPillarHeightAt;
    }

    public bool CanBuildOrExtendPillarAt(Tile2i position, HeightTilesI topTileHeight)
    {
      ThicknessTilesI newHeight;
      return this.CanExtendPillarAt(position, topTileHeight, out TransportPillar _, out newHeight) || this.CanBuildPillarAt(position, topTileHeight, out HeightTilesI _, out newHeight);
    }

    /// <summary>Builds a pillar without any checks.</summary>
    public void BuildOrExtendPillarNoChecks(
      Tile2i position,
      HeightTilesI topHeight,
      bool skipTallEnough = false)
    {
      TransportPillar transportPillar1;
      HeightTilesI height;
      if (this.m_pillars.TryGetValue(position, out transportPillar1))
      {
        if (transportPillar1.TopTileHeight >= topHeight & skipTallEnough)
          return;
        this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) transportPillar1, EntityRemoveReason.Remove);
        height = TransportHelper.GetPillarBaseHeight(this.m_terrain[position]).Min(transportPillar1.CenterTile.Height);
      }
      else
        height = TransportHelper.GetPillarBaseHeight(this.m_terrain[position]);
      TransportPillar transportPillar2 = this.m_pillarsBuilder.Create(position.ExtendHeight(height), topHeight - height + ThicknessTilesI.One);
      this.m_entitiesManager.AddEntityNoChecks((IEntity) transportPillar2);
      if (transportPillar1 == null || !transportPillar1.IsConstructed)
        return;
      transportPillar2.MakeFullyConstructed(true, true);
    }

    internal void ReportTrajectoryChanged(Transport t)
    {
      this.m_entitiesManager.InvokeOnEntityVisualChanged((IEntity) t);
      this.checkForMergesAndZipperChanges(t);
      foreach (TransportSupportableTile supportableTile in t.Trajectory.SupportableTiles)
      {
        TransportPillar transportPillar;
        if (this.m_pillars.TryGetValue(supportableTile.Position.Xy, out transportPillar))
          this.m_pillarsToCheck.Enqueue(transportPillar);
      }
    }

    public static void Serialize(TransportsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TransportsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TransportsManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetTransactions);
      EntityCollapseHelper.Serialize(this.m_collapseHelper, writer);
      writer.WriteGeneric<IConstructionManager>(this.m_constructionManager);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      writer.WriteGeneric<IIoPortsManager>(this.m_portsManager);
      writer.WriteGeneric<IResolver>(this.m_resolver);
      writer.WriteUInt(this.m_seqNumber);
      TerrainManager.Serialize(this.m_terrain, writer);
      writer.WriteGeneric<IUpgradesManager>(this.m_upgradesManager);
      writer.WriteGeneric<INotificationsManager>(this.NotificationsManager);
      writer.WriteGeneric<IProductsManager>(this.ProductsManager);
    }

    public static TransportsManager Deserialize(BlobReader reader)
    {
      TransportsManager transportsManager;
      if (reader.TryStartClassDeserialization<TransportsManager>(out transportsManager))
        reader.EnqueueDataDeserialization((object) transportsManager, TransportsManager.s_deserializeDataDelayedAction);
      return transportsManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<TransportsManager>(this, "m_assetTransactions", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<TransportsManager>(this, "m_collapseHelper", (object) EntityCollapseHelper.Deserialize(reader));
      reader.RegisterResolvedMember<TransportsManager>(this, "m_constructionHelper", typeof (TransportsConstructionHelper), true);
      reader.SetField<TransportsManager>(this, "m_constructionManager", (object) reader.ReadGenericAs<IConstructionManager>());
      reader.SetField<TransportsManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<TransportsManager>(this, "m_miniZippersToCheckForRemoval", (object) new Queueue<MiniZipper>());
      reader.SetField<TransportsManager>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.SetField<TransportsManager>(this, "m_pillarIds", (object) new Set<EntityId>());
      reader.SetField<TransportsManager>(this, "m_pillars", (object) new Dict<Tile2i, TransportPillar>());
      reader.RegisterResolvedMember<TransportsManager>(this, "m_pillarsBuilder", typeof (TransportPillarsBuilder), true);
      reader.SetField<TransportsManager>(this, "m_pillarsTmp", (object) new Lyst<TransportPillar>());
      reader.SetField<TransportsManager>(this, "m_pillarsToCheck", (object) new Queueue<TransportPillar>());
      reader.SetField<TransportsManager>(this, "m_portsManager", (object) reader.ReadGenericAs<IIoPortsManager>());
      reader.RegisterResolvedMember<TransportsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<TransportsManager>(this, "m_resolver", (object) reader.ReadGenericAs<IResolver>());
      this.m_seqNumber = reader.ReadUInt();
      reader.SetField<TransportsManager>(this, "m_terrain", (object) TerrainManager.Deserialize(reader));
      reader.SetField<TransportsManager>(this, "m_tilesTmp", (object) new Lyst<Tile3i>(true));
      reader.SetField<TransportsManager>(this, "m_transports", (object) new Set<Transport>());
      reader.SetField<TransportsManager>(this, "m_transportsAndPillarsIds", (object) new Set<EntityId>());
      reader.RegisterResolvedMember<TransportsManager>(this, "m_transportsBuilder", typeof (TransportsBuilder), true);
      reader.SetField<TransportsManager>(this, "m_transportsElevatedAndMiniZippersIds", (object) new Set<EntityId>());
      reader.SetField<TransportsManager>(this, "m_transportsSupportCheck", (object) new Queueue<KeyValuePair<Transport, Tile3i>>());
      reader.SetField<TransportsManager>(this, "m_transportsToCheckForMerge", (object) new Queueue<Transport>());
      reader.SetField<TransportsManager>(this, "m_upgradesManager", (object) reader.ReadGenericAs<IUpgradesManager>());
      reader.SetField<TransportsManager>(this, "NotificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      this.ProductsManager = reader.ReadGenericAs<IProductsManager>();
      reader.RegisterInitAfterLoad<TransportsManager>(this, "initialize", InitPriority.Normal);
      reader.RegisterInitAfterLoad<TransportsManager>(this, "validate", InitPriority.Low);
    }

    static TransportsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TransportsManager) obj).SerializeData(writer));
      TransportsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TransportsManager) obj).DeserializeData(reader));
    }
  }
}
