// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public abstract class LayoutEntity : 
    LayoutEntityBase,
    IEntityWithCustomTitle,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithGeneralPriorityFriend,
    IEntityWithGeneralPriority
  {
    [DoNotSaveCreateNewOnLoad(null, 0)]
    protected ImmutableArray<IoPortData> ConnectedOutputPorts;

    public Option<string> CustomTitle { get; set; }

    public int GeneralPriority { get; private set; }

    public virtual bool IsCargoAffectedByGeneralPriority => false;

    public virtual bool IsGeneralPriorityVisible => this.IsPriorityVisibleByDefault();

    void IEntityWithGeneralPriorityFriend.SetGeneralPriorityInternal(int priority)
    {
      if (this.GeneralPriority == priority)
        return;
      this.GeneralPriority = priority;
      this.NotifyOnGeneralPriorityChange();
    }

    public ImmutableArray<IoPort> Ports { get; protected set; }

    protected virtual IoPortType? PortTypeOverride => new IoPortType?();

    protected LayoutEntity(
      EntityId id,
      LayoutEntityProto proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, proto, transform, context);
      this.GeneralPriority = proto.Costs.DefaultPriority;
      this.createPorts();
      this.recreateOutputPortData();
    }

    [InitAfterLoad(InitPriority.Highest)]
    private void initPorts()
    {
      this.recreateOutputPortData();
      this.OnPortsLoadOrChange();
    }

    private void createPorts()
    {
      IEntityWithPorts thisWithPorts = this as IEntityWithPorts;
      if (thisWithPorts != null)
      {
        this.Ports = this.Prototype.Ports.Map<IoPort>((Func<IoPortTemplate, int, IoPort>) ((x, i) => IoPort.CreateFor<IEntityWithPorts>(this.Context.PortIdFactory.GetNextId(), thisWithPorts, this.Prototype.Layout, this.Transform, x, i, this.PortTypeOverride)));
      }
      else
      {
        this.Ports = ImmutableArray<IoPort>.Empty;
        ImmutableArray<IoPort> ports = this.Ports;
        Assertion<bool> actual = Mafi.Assert.That<bool>(ports.IsEmpty);
        LayoutEntityProto prototype = this.Prototype;
        string name = this.GetType().Name;
        ports = this.Ports;
        // ISSUE: variable of a boxed type
        __Boxed<int> length = (ValueType) ports.Length;
        string message = string.Format("{0} ({1}) has '{2}' ports ", (object) prototype, (object) name, (object) length) + "but does not implement 'IEntityWithPorts' interface";
        actual.IsTrue(message);
      }
    }

    private void recreateOutputPortData()
    {
      ImmutableArray<IoPort> immutableArray = this.Ports;
      immutableArray = immutableArray.Filter((Predicate<IoPort>) (x => x.IsConnectedAsOutput));
      this.ConnectedOutputPorts = immutableArray.Map<IoPortData>((Func<IoPort, IoPortData>) (x => new IoPortData(x)));
    }

    /// <summary>
    /// This method is called
    /// - on load
    /// - when we change ports after upgrade
    /// - any port changes connection
    /// Any class that holds IoPortData or IoPort needs to implement this!
    /// IMPORTANT: This is not called from ctor, so any code performed
    /// in this method should be performed in super ctor as well.
    /// </summary>
    protected virtual void OnPortsLoadOrChange()
    {
    }

    public void OnPortConnectionChanged(IoPort ourPort, IoPort otherPort)
    {
      if (ourPort.Type != IoPortType.Input)
        this.recreateOutputPortData();
      this.OnPortsLoadOrChange();
      this.OnPortConnectionChanged(ourPort);
      this.NotifyOnPortsConnectionChanged(ourPort, otherPort);
    }

    protected virtual void OnPortConnectionChanged(IoPort ourPort)
    {
    }

    protected override void OnUpgradeDone(IEntityProto oldProto, IEntityProto newProto)
    {
      base.OnUpgradeDone(oldProto, newProto);
      foreach (IoPort port in this.Ports)
        this.Context.IoPortsManager.DisconnectAndRemove(port);
      this.createPorts();
      foreach (IoPort port in this.Ports)
        this.Context.IoPortsManager.AddPortAndTryConnect(port);
      this.recreateOutputPortData();
      this.OnPortsLoadOrChange();
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    protected void AssertInputPortsOnly()
    {
      Mafi.Assert.That<bool>(this.Ports.All((Func<IoPort, bool>) (x => x.Type == IoPortType.Input))).IsTrue(string.Format("Only input ports are allowed on entity '{0}'.", (object) this.Prototype.Id));
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    protected void AssertOutputPortsOnly()
    {
      Mafi.Assert.That<bool>(this.Ports.All((Func<IoPort, bool>) (x => x.Type == IoPortType.Output))).IsTrue(string.Format("Only output ports are allowed on entity '{0}'.", (object) this.Prototype.Id));
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<string>.Serialize(this.CustomTitle, writer);
      writer.WriteInt(this.GeneralPriority);
      ImmutableArray<IoPort>.Serialize(this.Ports, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ConnectedOutputPorts = new ImmutableArray<IoPortData>();
      this.CustomTitle = Option<string>.Deserialize(reader);
      this.GeneralPriority = reader.ReadInt();
      this.Ports = ImmutableArray<IoPort>.Deserialize(reader);
      reader.RegisterInitAfterLoad<LayoutEntity>(this, "initPorts", InitPriority.Highest);
    }
  }
}
