// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportsCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class TransportsCommandsProcessor : 
    ICommandProcessor<BuildTransportCmd>,
    IAction<BuildTransportCmd>,
    ICommandProcessor<ReverseTransportCmd>,
    IAction<ReverseTransportCmd>,
    ICommandProcessor<ClearTransportCmd>,
    IAction<ClearTransportCmd>,
    ICommandProcessor<DeconstructTransportSegmentCmd>,
    IAction<DeconstructTransportSegmentCmd>
  {
    private readonly ProtosDb m_protosDb;
    private readonly TransportsManager m_transportsManager;
    private readonly EntitiesManager m_entitiesManager;
    private readonly EntitiesCloneConfigHelper m_configCloneHelper;
    private readonly ConstructionManager m_constructionManager;

    public TransportsCommandsProcessor(
      ProtosDb protosDb,
      TransportsManager transportsManager,
      EntitiesManager entitiesManager,
      EntitiesCloneConfigHelper configCloneHelper,
      ConstructionManager constructionManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_transportsManager = transportsManager;
      this.m_entitiesManager = entitiesManager;
      this.m_configCloneHelper = configCloneHelper;
      this.m_constructionManager = constructionManager;
    }

    public void Invoke(BuildTransportCmd cmd)
    {
      TransportProto proto;
      if (!this.m_protosDb.TryGetProto<TransportProto>((Proto.ID) cmd.ProtoId, out proto))
      {
        cmd.SetResultError(EntityId.Invalid, string.Format("Unknown transport proto '{0}'.", (object) cmd.ProtoId));
      }
      else
      {
        string error;
        EntityId result;
        if (this.tryBuildTransport(proto, cmd.PivotPositions, cmd.PillarHints, cmd.StartDirection, cmd.EndDirection, cmd.DisablePortSnapping, false, out error, out result))
          cmd.SetResultSuccess(result);
        else
          cmd.SetResultError(EntityId.Invalid, error);
      }
    }

    public bool TryBuildTransportFromConfig(
      EntityConfigData config,
      bool applyConfiguration,
      bool isFree,
      out string error)
    {
      if (!(config.Prototype.ValueOrNull is TransportProto valueOrNull1))
      {
        error = string.Format("Unknown or missing transport proto '{0}'.", (object) config.Prototype.ValueOrNull);
        return false;
      }
      TransportTrajectory valueOrNull2 = config.Trajectory.ValueOrNull;
      if (valueOrNull2 == null)
      {
        error = "Missing trajectory";
        return false;
      }
      ImmutableArray<Tile3i> pivots = valueOrNull2.Pivots;
      ImmutableArray<Tile2i> pillarHints = config.Pillars ?? ImmutableArray<Tile2i>.Empty;
      Direction903d? startDirection = new Direction903d?(valueOrNull2.StartDirection.ToDirection903d());
      Direction903d? endDirection = new Direction903d?(valueOrNull2.EndDirection.ToDirection903d());
      int num = isFree ? 1 : 0;
      ref string local1 = ref error;
      EntityId entityId;
      ref EntityId local2 = ref entityId;
      EntityConfigData configToCopy = applyConfiguration ? config : (EntityConfigData) null;
      return this.tryBuildTransport(valueOrNull1, pivots, pillarHints, startDirection, endDirection, false, num != 0, out local1, out local2, configToCopy, true);
    }

    /// <returns>Returns false if there was an error.</returns>
    private bool tryBuildTransport(
      TransportProto transportProto,
      ImmutableArray<Tile3i> pivots,
      ImmutableArray<Tile2i> pillarHints,
      Direction903d? startDirection,
      Direction903d? endDirection,
      bool disablePortSnapping,
      bool isFree,
      out string error,
      out EntityId result,
      EntityConfigData configToCopy = null,
      bool skipExtraPillarsForBetterVisuals = false)
    {
      Option<Transport> transport;
      Option<MiniZipper> miniZipperAtStart;
      Option<MiniZipper> miniZipperAtEnd;
      LocStrFormatted error1;
      if (!this.m_transportsManager.TryBuildOrJoinTransport(transportProto, pivots, pillarHints.ToReadonlySet(), startDirection, endDirection, disablePortSnapping, out transport, out miniZipperAtStart, out miniZipperAtEnd, out error1, out CanBuildTransportResult _, out Option<IStaticEntity> _, skipExtraPillarsForBetterVisuals))
      {
        error = error1.Value;
        result = EntityId.Invalid;
        return false;
      }
      Transport valueOrNull1 = transport.ValueOrNull;
      EntityId? nullable1;
      if (valueOrNull1 == null)
      {
        MiniZipper valueOrNull2 = miniZipperAtStart.ValueOrNull;
        nullable1 = valueOrNull2 != null ? new EntityId?(valueOrNull2.Id) : miniZipperAtEnd.ValueOrNull?.Id;
      }
      else
      {
        // ISSUE: explicit non-virtual call
        nullable1 = new EntityId?(__nonvirtual (valueOrNull1.Id));
      }
      EntityId? nullable2 = nullable1;
      if (!nullable2.HasValue)
      {
        error = "";
        result = EntityId.Invalid;
        return true;
      }
      result = nullable2.Value;
      if (transport.HasValue && configToCopy != null)
        this.m_configCloneHelper.ApplyConfigTo(configToCopy, (IEntity) transport.Value);
      if (isFree && transport.HasValue)
        transport.Value.MakeFullyConstructed();
      error = "";
      return true;
    }

    public void Invoke(ReverseTransportCmd cmd)
    {
      Transport entity;
      if (!this.m_entitiesManager.TryGetEntity<Transport>(cmd.TransportId, out entity))
      {
        cmd.SetResultError(string.Format("Transport {0} was not found.", (object) cmd.TransportId));
      }
      else
      {
        LocStrFormatted error;
        if (!this.m_transportsManager.TryReverseTransport(entity, out error))
          cmd.SetResultError(error.Value);
        else
          cmd.SetResultSuccess();
      }
    }

    public void Invoke(ClearTransportCmd cmd)
    {
      Transport entity;
      if (!this.m_entitiesManager.TryGetEntity<Transport>(cmd.TransportId, out entity))
      {
        cmd.SetResultError(EntityId.Invalid, string.Format("Transport {0} was not found.", (object) cmd.TransportId));
      }
      else
      {
        if (entity.IsProductsRemovalInProgress)
          entity.CancelProductsRemoval();
        else
          entity.RequestProductsRemoval();
        cmd.SetResultSuccess(entity.Id);
      }
    }

    public void Invoke(DeconstructTransportSegmentCmd cmd)
    {
      Transport entity;
      if (!this.m_entitiesManager.TryGetEntity<Transport>(cmd.TransportId, out entity))
      {
        cmd.SetResultError(string.Format("Transport {0} was not found.", (object) cmd.TransportId));
      }
      else
      {
        Option<Transport> deconstructedSubTransport;
        LocStrFormatted error;
        if (!this.m_transportsManager.TryDeconstructSubTransport(entity, cmd.StartPosition, cmd.EndPosition, out Option<Transport> _, out deconstructedSubTransport, out Option<Transport> _, out error))
        {
          cmd.SetResultError(error.Value);
        }
        else
        {
          if (cmd.QuickRemove && deconstructedSubTransport.HasValue)
            this.m_constructionManager.TryPerformQuickDeliveryOrRemoval((IStaticEntity) deconstructedSubTransport.Value, true);
          cmd.SetResultSuccess();
        }
      }
    }
  }
}
