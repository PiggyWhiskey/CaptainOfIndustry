// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IoPort
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Lifts;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  /// <summary>
  /// Generic I/O port that handles connection logic and has position in the world.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class IoPort : IEquatable<IoPort>, IIsSafeAsHashKey
  {
    /// <summary>
    /// Max transferred quantity through port per tick. This makes max port throughput 30 / sec which is equal
    /// to four T3 transports.
    /// </summary>
    /// <remarks>This number should not be lower since T3 transports are using stack size of 3.</remarks>
    public static readonly Quantity MAX_TRANSFER_PER_TICK;
    /// <summary>Index withing parent array of ports.</summary>
    public readonly byte PortIndex;
    /// <summary>Unique ID of this port.</summary>
    public readonly IoPortId Id;
    /// <summary>Entity that owns this port.</summary>
    public readonly IEntityWithPorts OwnerEntity;
    public readonly PortSpec Spec;
    /// <summary>
    /// Helper field solely for use by `IoPortsRenderer` to avoid dict lookup. Do not touch this.
    /// </summary>
    [DoNotSave(0, null)]
    public uint RendererId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>Tile position of this port.</summary>
    public Tile3i Position { get; private set; }

    /// <summary>
    /// Direction of this port always pointing outwards from the parent entity - towards potentially connected port.
    /// </summary>
    public Direction903d Direction { get; private set; }

    /// <summary>Shape of this port.</summary>
    public IoPortShapeProto ShapePrototype => this.Spec.Shape;

    /// <summary>Type of this port.</summary>
    public IoPortType Type => this.Spec.Type;

    /// <summary>Name of this port.</summary>
    public char Name => this.Spec.Name;

    /// <summary>
    /// Connected port or <see cref="F:Mafi.Option.None" />.
    /// </summary>
    public Option<IoPort> ConnectedPort { get; private set; }

    public bool IsDestroyed { get; private set; }

    public IoPort(
      IoPortId id,
      IEntityWithPorts ownerEntity,
      PortSpec spec,
      Tile3i position,
      Direction903d direction,
      int portIndex,
      bool isEndPort = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Id = id;
      this.OwnerEntity = ownerEntity.CheckNotNull<IEntityWithPorts>();
      this.Position = position;
      this.Direction = direction;
      this.Spec = spec;
      this.PortIndex = (byte) portIndex;
      this.IsEndPort = isEndPort;
    }

    public static IoPort CreateFor<T>(
      IoPortId id,
      T ownerEntity,
      EntityLayout layout,
      TileTransform transform,
      IoPortTemplate template,
      int portIndex,
      IoPortType? typeOverride = null)
      where T : IEntityWithPorts
    {
      return new IoPort(id, (IEntityWithPorts) ownerEntity, new PortSpec(template.Name, (IoPortType) ((int) typeOverride ?? (int) template.Type), template.Shape, template.Spec.CanOnlyConnectToTransports), template.GetPosition(layout, transform), template.GetDirection(transform), portIndex);
    }

    public static IoPort CreateFor<T>(
      IoPortId id,
      T ownerEntity,
      IoPortTemplate template,
      int portIndex,
      IoPortType? typeOverride = null)
      where T : IEntityWithPorts, ILayoutEntity
    {
      return new IoPort(id, (IEntityWithPorts) ownerEntity, new PortSpec(template.Name, (IoPortType) ((int) typeOverride ?? (int) template.Type), template.Shape, template.Spec.CanOnlyConnectToTransports), template.GetPosition(ownerEntity.Prototype.Layout, ownerEntity.Transform), template.GetDirection(ownerEntity.Transform), portIndex);
    }

    /// <summary>Connected status of this port.</summary>
    public bool IsConnected => this.ConnectedPort.HasValue;

    /// <summary>Whether this port is not connected to any other port.</summary>
    public bool IsNotConnected => !this.ConnectedPort.HasValue;

    /// <summary>
    /// Whether this port is end-port. This should not change for the entire lifetime of a port.
    /// End port is a flag for the 3D model that selects what type of model is used.
    /// Some ports such as conveyors have two types of ports, "end" and "through".
    /// </summary>
    public bool IsEndPort { get; private set; }

    /// <summary>
    /// Returns true if this port is connected to an output port.
    /// </summary>
    public bool IsConnectedAsInput
    {
      get
      {
        if (!this.IsConnected)
          return false;
        return this.Type != IoPortType.Any ? this.Type == IoPortType.Input : this.ConnectedPort.Value.Type == IoPortType.Output;
      }
    }

    /// <summary>
    /// Returns true if this port is connected to an input port.
    /// </summary>
    public bool IsConnectedAsOutput
    {
      get
      {
        if (!this.IsConnected)
          return false;
        return this.Type != IoPortType.Any ? this.Type == IoPortType.Output : this.ConnectedPort.Value.Type == IoPortType.Input;
      }
    }

    /// <summary>
    /// Position of expected connected port (position + direction).
    /// </summary>
    public Tile3i ExpectedConnectedPortCoord => this.Position + this.Direction.ToTileDirection();

    public Direction903d ExpectedConnectedPortDirection => -this.Direction;

    /// <summary>
    /// Whether this port can be connected to the given port. This is relationship is always symmetric:
    /// <c>X.CanConnectTo(Y) == Y.CanConnectTo(X)</c>.
    /// </summary>
    public bool CanConnectTo(IoPort otherPort)
    {
      if (this.IsConnected || otherPort.IsConnected)
        return false;
      Tile3i position = otherPort.Position;
      Direction903d direction = otherPort.Direction;
      IoPortShapeProto shapePrototype = otherPort.ShapePrototype;
      IoPortType type = otherPort.Type;
      bool otherOwnerIsTransport;
      switch (otherPort.OwnerEntity)
      {
        case Transport _:
        case Lift _:
          otherOwnerIsTransport = true;
          break;
        default:
          otherOwnerIsTransport = false;
          break;
      }
      return this.IsCompatibleWith(position, direction, shapePrototype, type, otherOwnerIsTransport, otherPort.Spec.CanOnlyConnectToTransports);
    }

    public bool CanConnectTo(
      Tile3i position,
      Direction903d direction,
      IoPortShapeProto shape,
      IoPortType type,
      bool otherOwnerIsTransport,
      bool otherPortCanOnlyConnectToTransports)
    {
      return !this.IsConnected && this.IsCompatibleWith(position, direction, shape, type, otherOwnerIsTransport, otherPortCanOnlyConnectToTransports);
    }

    public bool IsCompatibleWith(IoPort otherPort)
    {
      Tile3i position = otherPort.Position;
      Direction903d direction = otherPort.Direction;
      IoPortShapeProto shapePrototype = otherPort.ShapePrototype;
      IoPortType type = otherPort.Type;
      bool otherOwnerIsTransport;
      switch (otherPort.OwnerEntity)
      {
        case Transport _:
        case Lift _:
          otherOwnerIsTransport = true;
          break;
        default:
          otherOwnerIsTransport = false;
          break;
      }
      return this.IsCompatibleWith(position, direction, shapePrototype, type, otherOwnerIsTransport, otherPort.Spec.CanOnlyConnectToTransports);
    }

    public bool IsCompatibleWith(
      Tile3i position,
      Direction903d direction,
      IoPortShapeProto shape,
      IoPortType type,
      bool otherOwnerIsTransport,
      bool otherPortCanOnlyConnectToTransports)
    {
      return !((Proto) this.ShapePrototype != (Proto) shape) && this.Type.IsCompatibleWith(type) && (!this.Spec.CanOnlyConnectToTransports || otherOwnerIsTransport) && (!otherPortCanOnlyConnectToTransports || this.OwnerEntity is Transport || this.OwnerEntity is Lift) && !(this.ExpectedConnectedPortCoord != position) && !(position + direction.ToTileDirection() != this.Position);
    }

    /// <summary>Tries to connect to the given port.</summary>
    public bool TryConnect(IoPort otherPort, IIoPortsManager mgr)
    {
      if (!this.CanConnectTo(otherPort))
        return false;
      otherPort.ConnectedPort = (Option<IoPort>) this;
      this.ConnectedPort = (Option<IoPort>) otherPort;
      this.onConnectionChangedInternal(otherPort, mgr);
      otherPort.onConnectionChangedInternal(this, mgr);
      Assert.That<bool>(this.IsConnected).IsTrue();
      Assert.That<bool>(otherPort.IsConnected).IsTrue();
      return true;
    }

    /// <summary>Disconnects any connected port.</summary>
    public void Disconnect(IIoPortsManager mgr)
    {
      if (!this.IsConnected)
        return;
      IoPort otherPort = this.ConnectedPort.Value;
      Assert.That<IoPort>(otherPort.ConnectedPort.Value).IsEqualTo<IoPort>(this);
      otherPort.ConnectedPort = (Option<IoPort>) Option.None;
      this.ConnectedPort = (Option<IoPort>) Option.None;
      this.onConnectionChangedInternal(otherPort, mgr);
      otherPort.onConnectionChangedInternal(this, mgr);
      Assert.That<bool>(this.IsConnected).IsFalse();
      Assert.That<bool>(otherPort.IsConnected).IsFalse();
    }

    private void onConnectionChangedInternal(IoPort otherPort, IIoPortsManager mgr)
    {
      mgr.OnPortConnectionChanged(this, otherPort);
      this.OwnerEntity.OnPortConnectionChanged(this, otherPort);
    }

    public PartialQuantity GetMaxThroughputPerTick()
    {
      if (!this.IsConnected)
        return PartialQuantity.Zero;
      return !(this.ConnectedPort.Value.OwnerEntity is Transport ownerEntity) ? IoPort.MAX_TRANSFER_PER_TICK.AsPartial : ownerEntity.Prototype.ThroughputPerTick;
    }

    public override string ToString()
    {
      return string.Format("{0} #{1} ({2}), type {3}, at {4}, {5}", (object) this.GetType().Name, (object) this.Id, (object) this.Name, (object) this.Type, (object) this.Position, this.IsConnected ? (object) ("connected to #" + this.ConnectedPort.Value.Id.ToString()) : (object) "disconnected");
    }

    public bool Equals(IoPort other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (!this.Id.Equals(other.Id))
        return false;
      Log.Error(string.Format("IO ports that are not equal references have equal IDs: {0}", (object) this.Id));
      return true;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      if (!(obj is IoPort ioPort) || !this.Id.Equals(ioPort.Id))
        return false;
      Log.Error(string.Format("IO ports that are not equal references have equal IDs: {0}", (object) this.Id));
      return true;
    }

    public override int GetHashCode() => this.Id.GetHashCode();

    internal void MarkedAsDestroyed() => this.IsDestroyed = true;

    public static void Serialize(IoPort value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IoPort>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IoPort.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Option<IoPort>.Serialize(this.ConnectedPort, writer);
      Direction903d.Serialize(this.Direction, writer);
      IoPortId.Serialize(this.Id, writer);
      writer.WriteBool(this.IsDestroyed);
      writer.WriteBool(this.IsEndPort);
      writer.WriteGeneric<IEntityWithPorts>(this.OwnerEntity);
      writer.WriteByte(this.PortIndex);
      Tile3i.Serialize(this.Position, writer);
      PortSpec.Serialize(this.Spec, writer);
    }

    public static IoPort Deserialize(BlobReader reader)
    {
      IoPort ioPort;
      if (reader.TryStartClassDeserialization<IoPort>(out ioPort))
        reader.EnqueueDataDeserialization((object) ioPort, IoPort.s_deserializeDataDelayedAction);
      return ioPort;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.ConnectedPort = Option<IoPort>.Deserialize(reader);
      this.Direction = Direction903d.Deserialize(reader);
      reader.SetField<IoPort>(this, "Id", (object) IoPortId.Deserialize(reader));
      this.IsDestroyed = reader.ReadBool();
      this.IsEndPort = reader.ReadBool();
      reader.SetField<IoPort>(this, "OwnerEntity", (object) reader.ReadGenericAs<IEntityWithPorts>());
      reader.SetField<IoPort>(this, "PortIndex", (object) reader.ReadByte());
      this.Position = Tile3i.Deserialize(reader);
      reader.SetField<IoPort>(this, "Spec", (object) PortSpec.Deserialize(reader));
    }

    static IoPort()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IoPort.MAX_TRANSFER_PER_TICK = 3.Quantity();
      IoPort.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IoPort) obj).SerializeData(writer));
      IoPort.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IoPort) obj).DeserializeData(reader));
    }
  }
}
