// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Entity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Population;
using Mafi.Core.Ports.Io;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  public abstract class Entity : 
    IEntityFriend,
    IEntity,
    IIsSafeAsHashKey,
    IEquatable<Entity>,
    IEventOwner
  {
    private readonly EntityId m_id;
    private Option<LystMutableDuringIter<IEntityObserver>> m_observers;

    /// <summary>Unique ID of this entity.</summary>
    public EntityId Id => this.m_id;

    public virtual LocStrFormatted DefaultTitle => (LocStrFormatted) this.Prototype.Strings.Name;

    public EntityProto Prototype { get; protected set; }

    public EntityContext Context { get; private set; }

    public bool IsDestroyed { get; private set; }

    public abstract bool CanBePaused { get; }

    protected virtual bool IsEnabledNow => this.IsNotPaused;

    public bool IsEnabled { get; private set; }

    public bool IsNotEnabled => !this.IsEnabled;

    /// <summary>
    /// Whether entity was paused by player. This halts any operation and construction.
    /// </summary>
    public bool IsPaused { get; private set; }

    public bool IsNotPaused => !this.IsPaused;

    protected Entity(EntityId id, EntityProto prototype, EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_id = id;
      this.Prototype = prototype;
      this.Context = context;
    }

    void IEntityFriend.AddedToWorld(EntityAddReason reason)
    {
      this.OnAddedToWorld(reason);
      this.UpdateIsEnabled();
    }

    void IEntityFriend.NotifyAfterUpgrade(IEntityProto oldProto, IEntityProto newProto)
    {
      this.OnUpgradeDone(oldProto, newProto);
      if (this.m_observers.IsNone)
        return;
      foreach (IEntityObserver entityObserver in this.m_observers.Value)
      {
        if (entityObserver is IEntityObserverForUpgrade observerForUpgrade)
          observerForUpgrade.OnEntityUpgraded((IEntity) this, oldProto);
      }
    }

    protected virtual void OnUpgradeDone(IEntityProto oldProto, IEntityProto newProto)
    {
    }

    public void AddObserver(IEntityObserver observer)
    {
      if (this.m_observers.IsNone)
        this.m_observers = new LystMutableDuringIter<IEntityObserver>().SomeOption<LystMutableDuringIter<IEntityObserver>>();
      if (this.m_observers.Value.Contains<IEntityObserver>(observer))
        Log.Error("Observer already registered");
      else
        this.m_observers.Value.Add(observer);
    }

    public void RemoveObserver(IEntityObserver observer)
    {
      this.m_observers.ValueOrNull?.Remove(observer);
    }

    protected virtual void OnAddedToWorld(EntityAddReason reason)
    {
    }

    public virtual void SetPaused(bool isPaused)
    {
      if (this.IsPaused == isPaused)
        return;
      this.IsPaused = isPaused;
      ((IEntitiesManagerInternal) this.Context.EntitiesManager).NotifyEntityPauseStateChanged((IEntity) this, isPaused);
      this.UpdateIsEnabled();
    }

    public void UpdateIsEnabled()
    {
      bool isEnabledNow = this.IsEnabledNow;
      if (this.IsEnabled == isEnabledNow)
        return;
      this.IsEnabled = isEnabledNow;
      this.OnEnabledChanged();
      if (this.m_observers.HasValue)
      {
        foreach (IEntityObserver entityObserver in this.m_observers.Value)
        {
          if (entityObserver is IEntityObserverForEnabled observerForEnabled)
            observerForEnabled.OnEnabledChange((IEntity) this, this.IsEnabled);
        }
      }
      ((IEntitiesManagerInternal) this.Context.EntitiesManager).NotifyEntityEnabledChanged((IEntity) this, this.IsEnabled);
    }

    protected virtual void OnEnabledChanged()
    {
    }

    public void UpdateIsBroken()
    {
      this.UpdateIsEnabled();
      this.OnIsBrokenChanged();
    }

    protected virtual void OnIsBrokenChanged()
    {
    }

    public void UpdateProperties() => this.OnPropertiesChanged();

    protected virtual void OnPropertiesChanged()
    {
    }

    protected void NotifyOnPortsConnectionChanged(IoPort ourPort, IoPort otherPort)
    {
      if (this.m_observers.IsNone)
        return;
      foreach (IEntityObserver entityObserver in this.m_observers.Value)
      {
        if (entityObserver is IEntityObserverForPorts observerForPorts)
          observerForPorts.OnConnectionChanged(ourPort, otherPort);
      }
    }

    protected void NotifyOnGeneralPriorityChange()
    {
      if (this.m_observers.IsNone)
        return;
      foreach (IEntityObserver entityObserver in this.m_observers.Value)
      {
        if (entityObserver is IEntityObserverForPriority observerForPriority)
          observerForPriority.OnGeneralPriorityChange((IEntity) this);
      }
    }

    /// <summary>
    /// Destroys this entity. This also removes it from <see cref="T:Mafi.Core.Entities.EntitiesManager" /> if it is managed.
    /// </summary>
    protected virtual void OnDestroy()
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse<EntityId, EntityProto.ID>("Entity {0} '{1}' was destroyed more than once.", this.Id, this.Prototype.Id);
      this.IsDestroyed = true;
    }

    void IEntityFriend.Destroy()
    {
      this.OnDestroy();
      if (this.m_observers.HasValue)
      {
        foreach (IEntityObserver entityObserver in this.m_observers.Value)
          entityObserver.OnEntityDestroy((IEntity) this);
        if (this.m_observers.Value.IsNotEmpty<IEntityObserver>())
        {
          Log.Error("Some observers did not un-register, e.g. " + this.m_observers.Value.First<IEntityObserver>().GetType().Name);
          this.m_observers.Value.Clear();
        }
      }
      Assert.That<bool>(this.IsDestroyed).IsTrue<Entity>("Entity '{0}' was not be properly destroyed. Did you forget to call `base.OnDestroy()`?", this);
    }

    public override string ToString()
    {
      return string.Format("{0} #{1}{2}", (object) this.GetType().Name, (object) this.Id, this.IsDestroyed ? (object) " (destroyed)" : (object) "");
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      if (!(obj is Entity entity) || !this.Id.Equals(entity.Id))
        return false;
      Log.Error(string.Format("Entity '{0}' has two different instances with the same ID: {1}", (object) this.GetType().Name, (object) this.Id));
      return true;
    }

    public bool Equals(Entity other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (!this.Id.Equals(other.Id))
        return false;
      Log.Error(string.Format("Entity '{0}' has two different instances with the same ID: {1}", (object) this.GetType().Name, (object) this.Id));
      return true;
    }

    public override int GetHashCode() => this.Id.GetHashCode();

    protected static bool HasWorkers(IEntityWithWorkers entity)
    {
      return entity.HasWorkersCached || entity.Context.WorkersManager.CanWork(entity);
    }

    protected static bool IsMissingWorkers(IEntityWithWorkers entity)
    {
      return !entity.HasWorkersCached && !entity.Context.WorkersManager.CanWork(entity);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsDestroyed);
      writer.WriteBool(this.IsEnabled);
      writer.WriteBool(this.IsPaused);
      EntityId.Serialize(this.m_id, writer);
      Option<LystMutableDuringIter<IEntityObserver>>.Serialize(this.m_observers, writer);
      writer.WriteGeneric<EntityProto>(this.Prototype);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<Entity>(this, "Context", typeof (EntityContext), false);
      this.IsDestroyed = reader.ReadBool();
      this.IsEnabled = reader.ReadBool();
      this.IsPaused = reader.ReadBool();
      reader.SetField<Entity>(this, "m_id", (object) EntityId.Deserialize(reader));
      this.m_observers = Option<LystMutableDuringIter<IEntityObserver>>.Deserialize(reader);
      this.Prototype = reader.ReadGenericAs<EntityProto>();
    }
  }
}
