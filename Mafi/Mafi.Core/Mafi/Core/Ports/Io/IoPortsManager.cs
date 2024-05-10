// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IoPortsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  /// <summary>Ports manager.</summary>
  /// <remarks>
  /// Ports manager does not implement <see cref="!:IEntityValidator" /> because for layout entities all ports are within
  /// the entity so that they are always safe to add and for other implementations of <see cref="T:Mafi.Core.Entities.Static.StaticEntity" /> there
  /// is no general way how to obtain port locations from proto.
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class IoPortsManager : IIoPortsManager
  {
    /// <summary>All managed ports.</summary>
    private readonly Dict<IoPortKey, IoPort> m_ports;
    private readonly Dict<Tile3i, int> m_portConnTiles;
    private readonly Event<IoPort, IoPort> m_portConnectionChanged;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IReadOnlyDictionary<Tile3i, int> PortConnTiles
    {
      get => (IReadOnlyDictionary<Tile3i, int>) this.m_portConnTiles;
    }

    /// <summary>
    /// Raised when a new port is added. This event is raised on the sim thread.
    /// </summary>
    public event Action<IoPort> PortAdded;

    /// <summary>Raised when a port connection status is changed.</summary>
    public IEvent<IoPort, IoPort> PortConnectionChanged
    {
      get => (IEvent<IoPort, IoPort>) this.m_portConnectionChanged;
    }

    /// <summary>
    /// Raised when a port is removed. This event is raised on the sim thread.
    /// </summary>
    public event Action<IoPort> PortRemoved;

    /// <summary>Returns I/O port on requested coordinate.</summary>
    public Option<IoPort> this[IoPortKey key]
    {
      get
      {
        IoPort ioPort;
        return !this.m_ports.TryGetValue(key, out ioPort) ? Option<IoPort>.None : (Option<IoPort>) ioPort;
      }
    }

    public Option<IoPort> this[Tile3i position, Direction903d direction]
    {
      get => this[new IoPortKey(position, direction)];
    }

    /// <summary>Total number of all managed ports.</summary>
    public int PortsCount => this.m_ports.Count;

    /// <summary>Returns all managed ports.</summary>
    public IEnumerable<IoPort> Ports => (IEnumerable<IoPort>) this.m_ports.Values;

    public IoPortsManager(EntitiesManager manager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_ports = new Dict<IoPortKey, IoPort>();
      this.m_portConnTiles = new Dict<Tile3i, int>();
      this.m_portConnectionChanged = new Event<IoPort, IoPort>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      manager.StaticEntityAdded.Add<IoPortsManager>(this, new Action<IStaticEntity>(this.addAllPortsOf));
      manager.StaticEntityRemoved.Add<IoPortsManager>(this, new Action<IStaticEntity>(this.removeAllPortsOf));
    }

    [InitAfterLoad(InitPriority.Low)]
    private void verifyPortsAfterLoad() => this.ValidateAndFixPorts();

    internal bool ValidateAndFixPorts()
    {
      bool flag = false;
      foreach (IoPort ioPort in this.m_ports.Values)
      {
        if (ioPort.ConnectedPort.HasValue)
        {
          IoPort otherPort = ioPort.ConnectedPort.Value;
          IoPort port;
          if (!this.TryGetPortAt(ioPort.ExpectedConnectedPortCoord, ioPort.ExpectedConnectedPortDirection, out port))
          {
            Log.Error("Following ports are connected manager does not have counterpart port registered, " + string.Format("disconnecting:\n{0}\n{1} (not at {2} ", (object) ioPort, (object) otherPort, (object) ioPort.ExpectedConnectedPortCoord) + string.Format("{0})", (object) ioPort.ExpectedConnectedPortDirection));
            ioPort.Disconnect((IIoPortsManager) this);
            flag = true;
          }
          else if (otherPort != port)
          {
            Log.Error("Following port is connected but it should be attempted to be connected to a different port, reconnecting:\n" + string.Format("{0}\nexpected: {1}\nactual: {2}", (object) ioPort, (object) port, (object) otherPort));
            ioPort.Disconnect((IIoPortsManager) this);
            ioPort.TryConnect(port, (IIoPortsManager) this);
            flag = true;
          }
          else if (!ioPort.IsCompatibleWith(otherPort))
          {
            Log.Error("Following ports are connected but they are not compatible, disconnecting:\n" + string.Format("{0}\n{1}", (object) ioPort, (object) otherPort));
            ioPort.Disconnect((IIoPortsManager) this);
            flag = true;
          }
        }
        else
        {
          IoPort port;
          if (this.TryGetPortAt(ioPort.ExpectedConnectedPortCoord, ioPort.ExpectedConnectedPortDirection, out port) && ioPort.IsCompatibleWith(port))
          {
            Log.Error("Following ports are not connected but they are compatible, connecting:\n" + string.Format("{0}\n{1}", (object) ioPort, (object) port));
            ioPort.TryConnect(port, (IIoPortsManager) this).AssertTrue();
            flag = true;
          }
        }
      }
      return flag;
    }

    public bool TryGetPortAt(Tile3i position, Direction903d direction, out IoPort port)
    {
      return this.m_ports.TryGetValue(new IoPortKey(position, direction), out port);
    }

    public bool TryGetPortAt(
      Tile3i position,
      Direction903d direction,
      IoPortShapeProto portShape,
      out IoPort port)
    {
      return this.m_ports.TryGetValue(new IoPortKey(position, direction), out port) && (Proto) port.ShapePrototype == (Proto) portShape;
    }

    /// <summary>Whether given port can be safely added.</summary>
    public bool CanAdd(IoPort port)
    {
      return !this.m_ports.ContainsKey(new IoPortKey(port.Position, port.Direction));
    }

    public bool IsAnyPortFacingTo(Tile3i tile) => this.m_portConnTiles.ContainsKey(tile);

    private void addPortAndTryConnect(IoPort port)
    {
      Assert.That<bool>(port.IsConnected).IsFalse("Adding already connected port?");
      this.m_ports.AddAndAssertNew(new IoPortKey(port.Position, port.Direction), port);
      this.m_portConnTiles.IncOrInsert1<Tile3i>(port.ExpectedConnectedPortCoord);
      this.onPortAdded(port);
      IoPort otherPort;
      if (!this.m_ports.TryGetValue(new IoPortKey(port.ExpectedConnectedPortCoord, port.ExpectedConnectedPortDirection), out otherPort))
        return;
      port.TryConnect(otherPort, (IIoPortsManager) this);
    }

    /// <summary>Disconnects and removes given port.</summary>
    public void DisconnectAndRemove(IoPort port) => this.disconnectAndRemovePort(port);

    public void Disconnect(IoPort port) => port.Disconnect((IIoPortsManager) this);

    public void AddPortAndTryConnect(IoPort port) => this.addPortAndTryConnect(port);

    /// <summary>Removes all ports of given entity from this map.</summary>
    private void removeAllPortsOf(IStaticEntity entity)
    {
      if (!(entity is IEntityWithPorts entityWithPorts))
        return;
      foreach (IoPort port in entityWithPorts.Ports)
        this.disconnectAndRemovePort(port);
    }

    /// <summary>
    /// Adds all ports of given entity if it is instance of <see cref="T:Mafi.Core.Entities.Static.StaticEntity" />. All added ports are attempted
    /// to connect.
    /// </summary>
    private void addAllPortsOf(IStaticEntity entity)
    {
      if (!(entity is IEntityWithPorts entityWithPorts))
        return;
      foreach (IoPort port in entityWithPorts.Ports)
        this.addPortAndTryConnect(port);
    }

    private void disconnectAndRemovePort(IoPort port)
    {
      if (port.IsConnected)
        port.Disconnect((IIoPortsManager) this);
      this.m_ports.RemoveAndAssert(new IoPortKey(port.Position, port.Direction));
      this.m_portConnTiles.DecAndRemoveAtZero<Tile3i>(port.ExpectedConnectedPortCoord);
      this.onPortRemoved(port);
      port.MarkedAsDestroyed();
    }

    private void onPortAdded(IoPort port)
    {
      Assert.That<Dict<IoPortKey, IoPort>>(this.m_ports).ContainsKeyValue<IoPortKey, IoPort>(new IoPortKey(port.Position, port.Direction), port, "Port added for non-managed port.");
      Action<IoPort> portAdded = this.PortAdded;
      if (portAdded == null)
        return;
      portAdded(port);
    }

    public void OnPortConnectionChanged(IoPort port, IoPort otherPort)
    {
      this.m_portConnectionChanged.Invoke(port, otherPort);
    }

    private void onPortRemoved(IoPort port)
    {
      Assert.That<Dict<IoPortKey, IoPort>>(this.m_ports).NotContainsKey<IoPortKey, IoPort>(new IoPortKey(port.Position, port.Direction), "Port removed for still existing port.");
      Action<IoPort> portRemoved = this.PortRemoved;
      if (portRemoved == null)
        return;
      portRemoved(port);
    }

    /// <summary>
    /// Verifies that all managed ports match ports of entities.
    /// </summary>
    public bool Debug_VerifyPorts(IEntitiesManager entitiesManager, out string err)
    {
      Lyst<IoPort> lyst = entitiesManager.Entities.AsEnumerable().OfType<IEntityWithPorts>().SelectMany<IEntityWithPorts, IoPort>((Func<IEntityWithPorts, IEnumerable<IoPort>>) (x => x.Ports.AsEnumerable())).ToLyst<IoPort>();
      Set<IoPort> set1 = lyst.ToSet<IoPort>();
      if (lyst.Count != set1.Count)
      {
        err = string.Format("There are {0} ports on entities but only ", (object) lyst.Count) + string.Format("{0} of them are distinct!", (object) set1.Count);
        return false;
      }
      Set<IoPort> set2 = this.m_ports.Values.ToSet<IoPort>();
      if (set2.Count != this.m_ports.Values.Count)
      {
        err = string.Format("There are {0} ports managed by ports manager but only ", (object) this.m_ports.Values.Count) + string.Format("{0} of them are distinct!", (object) set2.Count);
        return false;
      }
      set2.ExceptWith((IEnumerable<IoPort>) lyst);
      if (set2.IsNotEmpty)
      {
        err = string.Format("There are {0} ports managed ports manager but not present on entities: ", (object) set2.Count) + set2.Select<IoPort, string>((Func<IoPort, string>) (x => x.ToString())).JoinStrings(", ");
        return false;
      }
      set1.ExceptWith((IEnumerable<IoPort>) this.m_ports.Values);
      if (set1.IsNotEmpty)
      {
        err = string.Format("There are {0} ports on entities that are not managed by the manager: ", (object) set1.Count) + set1.Select<IoPort, string>((Func<IoPort, string>) (x => x.ToString())).JoinStrings(", ");
        return false;
      }
      err = "";
      return true;
    }

    public static void Serialize(IoPortsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IoPortsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IoPortsManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Event<IoPort, IoPort>.Serialize(this.m_portConnectionChanged, writer);
      Dict<Tile3i, int>.Serialize(this.m_portConnTiles, writer);
      Dict<IoPortKey, IoPort>.Serialize(this.m_ports, writer);
    }

    public static IoPortsManager Deserialize(BlobReader reader)
    {
      IoPortsManager ioPortsManager;
      if (reader.TryStartClassDeserialization<IoPortsManager>(out ioPortsManager))
        reader.EnqueueDataDeserialization((object) ioPortsManager, IoPortsManager.s_deserializeDataDelayedAction);
      return ioPortsManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<IoPortsManager>(this, "m_portConnectionChanged", (object) Event<IoPort, IoPort>.Deserialize(reader));
      reader.SetField<IoPortsManager>(this, "m_portConnTiles", (object) Dict<Tile3i, int>.Deserialize(reader));
      reader.SetField<IoPortsManager>(this, "m_ports", (object) Dict<IoPortKey, IoPort>.Deserialize(reader));
      reader.RegisterInitAfterLoad<IoPortsManager>(this, "verifyPortsAfterLoad", InitPriority.Low);
    }

    static IoPortsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IoPortsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IoPortsManager) obj).SerializeData(writer));
      IoPortsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IoPortsManager) obj).DeserializeData(reader));
    }
  }
}
