// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.BuildTransportBetweenAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.PathFinding;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  internal class BuildTransportBetweenAction : IScriptedAiPlayerAction
  {
    private readonly string m_fromEntityName;
    private readonly string m_toEntityName;
    private readonly ProductProto.ID m_productId;
    private readonly char? m_fromPortName;
    private readonly char? m_toPortName;
    private readonly Option<string> m_name;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (BuildTransportBetweenAction.Core);

    public string Description
    {
      get
      {
        return string.Format("Build transport for `{0}` from `{1}` ", (object) this.m_productId, (object) this.m_fromEntityName) + "to " + this.m_toEntityName + ")";
      }
    }

    public BuildTransportBetweenAction(
      string fromEntityName,
      string toEntityName,
      ProductProto.ID productId,
      char? fromPortName = null,
      char? toPortName = null,
      string name = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_fromEntityName = fromEntityName;
      this.m_toEntityName = toEntityName;
      this.m_productId = productId;
      this.m_fromPortName = fromPortName;
      this.m_toPortName = toPortName;
      this.m_name = (Option<string>) name;
    }

    public static void Serialize(BuildTransportBetweenAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BuildTransportBetweenAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BuildTransportBetweenAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.m_fromEntityName);
      writer.WriteNullableStruct<char>(this.m_fromPortName);
      Option<string>.Serialize(this.m_name, writer);
      ProductProto.ID.Serialize(this.m_productId, writer);
      writer.WriteString(this.m_toEntityName);
      writer.WriteNullableStruct<char>(this.m_toPortName);
    }

    public static BuildTransportBetweenAction Deserialize(BlobReader reader)
    {
      BuildTransportBetweenAction transportBetweenAction;
      if (reader.TryStartClassDeserialization<BuildTransportBetweenAction>(out transportBetweenAction))
        reader.EnqueueDataDeserialization((object) transportBetweenAction, BuildTransportBetweenAction.s_deserializeDataDelayedAction);
      return transportBetweenAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<BuildTransportBetweenAction>(this, "m_fromEntityName", (object) reader.ReadString());
      reader.SetField<BuildTransportBetweenAction>(this, "m_fromPortName", (object) reader.ReadNullableStruct<char>());
      reader.SetField<BuildTransportBetweenAction>(this, "m_name", (object) Option<string>.Deserialize(reader));
      reader.SetField<BuildTransportBetweenAction>(this, "m_productId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<BuildTransportBetweenAction>(this, "m_toEntityName", (object) reader.ReadString());
      reader.SetField<BuildTransportBetweenAction>(this, "m_toPortName", (object) reader.ReadNullableStruct<char>());
    }

    static BuildTransportBetweenAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BuildTransportBetweenAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BuildTransportBetweenAction) obj).SerializeData(writer));
      BuildTransportBetweenAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BuildTransportBetweenAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly BuildTransportBetweenAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private readonly ITransportPathFinder m_transportPathFinder;
      private readonly ProtosDb m_protosDb;
      private readonly PortProductsResolver m_portProductsResolver;
      private readonly UnlockedProtosDb m_unlockedProtosDb;
      private readonly TransportsManager m_transportsManager;
      private Option<BuildTransportCmd> m_cmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(
        BuildTransportBetweenAction action,
        InputScheduler inputScheduler,
        ITransportPathFinder transportPathFinder,
        ProtosDb protosDb,
        PortProductsResolver portProductsResolver,
        UnlockedProtosDb unlockedProtosDb,
        TransportsManager transportsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
        this.m_transportPathFinder = transportPathFinder;
        this.m_protosDb = protosDb;
        this.m_portProductsResolver = portProductsResolver;
        this.m_unlockedProtosDb = unlockedProtosDb;
        this.m_transportsManager = transportsManager;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (this.m_cmd.IsNone)
        {
          IEntityWithPorts entity1;
          if (!player.TryGetNamedEntity<IEntityWithPorts>(this.m_action.m_fromEntityName, out entity1))
          {
            Log.Error("Failed to find entity `" + this.m_action.m_fromEntityName + "`.");
            return true;
          }
          IEntityWithPorts entity2;
          if (!player.TryGetNamedEntity<IEntityWithPorts>(this.m_action.m_toEntityName, out entity2))
          {
            Log.Error("Failed to find entity `" + this.m_action.m_toEntityName + "`.");
            return true;
          }
          ProductProto proto;
          if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) this.m_action.m_productId, out proto))
          {
            Log.Error(string.Format("Failed to find product `{0}`.", (object) this.m_action.m_productId));
            return true;
          }
          IoPort fountPort1;
          if (!this.m_portProductsResolver.TryGetFirstUnconnectedPortFor(entity1, IoPortType.Output, proto, this.m_action.m_fromPortName, out fountPort1))
          {
            Log.Error(string.Format("Failed to find valid output port for `{0}` on `{1}`. ", (object) this.m_action.m_productId, (object) entity1) + "Try to specify port name.");
            return true;
          }
          IoPort fountPort2;
          if (!this.m_portProductsResolver.TryGetFirstUnconnectedPortFor(entity2, IoPortType.Input, proto, this.m_action.m_toPortName, out fountPort2))
          {
            Log.Error(string.Format("Failed to find valid input port for `{0}` on `{1}`. ", (object) this.m_action.m_productId, (object) entity2) + "Try to specify port name.");
            return true;
          }
          TransportProto transportProto;
          if (!this.tryGetTransportProtoFor(fountPort1, fountPort2, out transportProto))
            return true;
          int iterations = 1000;
          Tile3i start = fountPort1.ExpectedConnectedPortCoord;
          Tile3i connectedPortCoord = fountPort2.ExpectedConnectedPortCoord;
          PathFinderResult pathFinderResult = PathFinderResult.Unknown;
          ImmutableArray<Tile3i> immutableArray = ImmutableArray<Tile3i>.Empty;
          Direction903d? nullable = new Direction903d?(-fountPort1.Direction);
          for (int index = 0; index < 5; ++index)
          {
            TransportPathFinderOptions options = new TransportPathFinderOptions(bannedStartDirections: nullable.HasValue ? ImmutableArray.Create<Direction903d>(nullable.Value) : ImmutableArray<Direction903d>.Empty, flags: TransportPathFinderFlags.BanTilesInFrontOfPorts);
            this.m_transportPathFinder.InitPathFinding(transportProto, start, connectedPortCoord, options);
            ImmutableArray<Tile3i> outPivots;
            pathFinderResult = this.m_transportPathFinder.ContinuePathFinding(ref iterations, out outPivots);
            immutableArray = immutableArray.IsEmpty ? outPivots : immutableArray.Concat(outPivots, 1, outPivots.Length - 1);
            if (pathFinderResult == PathFinderResult.PathFound && !(outPivots.Last == fountPort2.ExpectedConnectedPortCoord))
            {
              start = outPivots.Last;
              nullable = outPivots.Length == 1 ? new Direction903d?() : new Direction903d?((outPivots.PreLast - outPivots.Last).ToDirection903d());
            }
            else
              break;
          }
          try
          {
            if (pathFinderResult != PathFinderResult.PathFound)
            {
              Log.Error("Failed to find transport between `" + this.m_action.m_fromEntityName + "` " + string.Format("and `{0}` in {1} iterations.", (object) this.m_action.m_toEntityName, (object) 1000));
              return true;
            }
            LocStrFormatted error;
            if (!this.m_transportsManager.CanBuildOrJoinTransport(transportProto, immutableArray, Set<Tile2i>.Empty, new Direction903d?(-fountPort1.Direction), new Direction903d?(-fountPort2.Direction), true, out CanBuildTransportResult _, out error, out Option<IStaticEntity> _))
            {
              Log.Error("Transport between `" + this.m_action.m_fromEntityName + "` " + string.Format("and `{0}` won't be possible to build: {1}", (object) this.m_action.m_toEntityName, (object) error));
              return true;
            }
          }
          catch
          {
            Tile2i xy1 = (pathFinderResult == PathFinderResult.PathFound ? immutableArray.First : fountPort1.ExpectedConnectedPortCoord).Xy;
            Tile2i xy2 = (pathFinderResult == PathFinderResult.PathFound ? immutableArray.Last : fountPort2.ExpectedConnectedPortCoord).Xy;
            DebugGameMapDrawing debugGameMapDrawing = DebugGameRenderer.DrawGameImage(xy1.Min(xy2) - new RelTile2i(10, 10), (xy2 - xy1).AbsValue + new RelTile2i(20, 20), forceEnable: true).HighlightTiles((IEnumerable<Tile2i>) new Tile2i[1]
            {
              xy1
            }, ColorRgba.Cyan.SetA((byte) 128)).HighlightTiles((IEnumerable<Tile2i>) new Tile2i[1]
            {
              xy2
            }, ColorRgba.Green.SetA((byte) 128)).DrawString(xy1.CenterTile2f, "start", ColorRgba.Blue.SetA((byte) 128), centered: true).DrawString(xy2.CenterTile2f, "goal", ColorRgba.Green.SetA((byte) 128), centered: true);
            (pathFinderResult == PathFinderResult.PathFound ? debugGameMapDrawing.DrawLine(immutableArray.Select<Tile2f>((Func<Tile3i, Tile2f>) (x => x.Xy.CenterTile2f)), ColorRgba.Red.SetA((byte) 128)) : debugGameMapDrawing.DrawLine(xy1.CenterTile2f, xy2.CenterTile2f, ColorRgba.Red.SetA((byte) 128))).SaveMapAsTga("FailedTransport");
            throw;
          }
          this.m_cmd = (Option<BuildTransportCmd>) this.m_inputScheduler.ScheduleInputCmd<BuildTransportCmd>(new BuildTransportCmd(transportProto.Id, immutableArray, ImmutableArray<Tile2i>.Empty, new Direction903d?(-fountPort1.Direction), new Direction903d?(-fountPort2.Direction), false, false));
          return false;
        }
        if (!this.m_cmd.Value.IsProcessed)
          return false;
        if (!this.m_cmd.Value.Result.IsValid)
        {
          Log.Error("Failed to build transport between `" + this.m_action.m_fromEntityName + "` and `" + this.m_action.m_toEntityName + "`.");
          return true;
        }
        if (this.m_action.m_name.HasValue)
          player.RegisterNamedEntity(this.m_cmd.Value.Result, this.m_action.m_name.Value);
        return true;
      }

      private bool tryGetTransportProtoFor(
        IoPort fromPort,
        IoPort toPort,
        out TransportProto transportProto)
      {
        if ((Proto) fromPort.ShapePrototype != (Proto) toPort.ShapePrototype)
        {
          Log.Error(string.Format("Ports are incompatible: {0}, {1}", (object) fromPort.ShapePrototype, (object) toPort.ShapePrototype));
          transportProto = (TransportProto) null;
          return false;
        }
        transportProto = this.m_protosDb.All<TransportProto>().Where<TransportProto>(new Func<TransportProto, bool>(this.m_unlockedProtosDb.IsUnlocked)).FirstOrDefault<TransportProto>((Func<TransportProto, bool>) (tp => (Proto) tp.PortsShape == (Proto) fromPort.ShapePrototype));
        if (!((Proto) transportProto == (Proto) null))
          return true;
        Log.Error(string.Format("TransportProto for port shape {0} not found ", (object) fromPort.ShapePrototype) + "(searching only unlocked).");
        return false;
      }

      public static void Serialize(BuildTransportBetweenAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<BuildTransportBetweenAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, BuildTransportBetweenAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        BuildTransportBetweenAction.Serialize(this.m_action, writer);
        Option<BuildTransportCmd>.Serialize(this.m_cmd, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
        TransportsManager.Serialize(this.m_transportsManager, writer);
        UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      }

      public static BuildTransportBetweenAction.Core Deserialize(BlobReader reader)
      {
        BuildTransportBetweenAction.Core core;
        if (reader.TryStartClassDeserialization<BuildTransportBetweenAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, BuildTransportBetweenAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<BuildTransportBetweenAction.Core>(this, "m_action", (object) BuildTransportBetweenAction.Deserialize(reader));
        this.m_cmd = Option<BuildTransportCmd>.Deserialize(reader);
        reader.SetField<BuildTransportBetweenAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
        reader.RegisterResolvedMember<BuildTransportBetweenAction.Core>(this, "m_portProductsResolver", typeof (PortProductsResolver), true);
        reader.RegisterResolvedMember<BuildTransportBetweenAction.Core>(this, "m_protosDb", typeof (ProtosDb), true);
        reader.RegisterResolvedMember<BuildTransportBetweenAction.Core>(this, "m_transportPathFinder", typeof (ITransportPathFinder), true);
        reader.SetField<BuildTransportBetweenAction.Core>(this, "m_transportsManager", (object) TransportsManager.Deserialize(reader));
        reader.SetField<BuildTransportBetweenAction.Core>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        BuildTransportBetweenAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BuildTransportBetweenAction.Core) obj).SerializeData(writer));
        BuildTransportBetweenAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BuildTransportBetweenAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
