// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.ProtosDb
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Mods;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Prototypes
{
  [SerializeAsGlobalDep]
  public class ProtosDb : IProtosDbFriend
  {
    private Option<IMod> m_activeMod;
    private readonly Set<Proto> m_protosLockedOnInit;
    private readonly Dict<Proto.ID, Proto> m_protoById;
    private readonly Dict<Type, Set<Proto>> m_protoByType;
    [DoNotSave(0, null)]
    private readonly Set<string> m_propertyIdsToTrack;

    /// <summary>Indicator of locked DB for further edits.</summary>
    internal bool IsReadonly { get; private set; }

    public IReadOnlySet<Proto> ProtosLockedOnInit
    {
      get => (IReadOnlySet<Proto>) this.m_protosLockedOnInit;
    }

    /// <summary>
    /// Ids of properties used by protos.
    /// Protos should be notified on each change of these properties.
    /// </summary>
    public IReadOnlySet<string> PropertyIdsToTrack
    {
      get => (IReadOnlySet<string>) this.m_propertyIdsToTrack;
    }

    internal void SetActiveMod(IMod mod)
    {
      if (this.IsReadonly)
        throw new InvalidOperationException("Failed to set active mod '" + mod.Name + "': DB is LOCKED at this stage.");
      this.m_activeMod = Option.Some<IMod>(mod);
    }

    /// <summary>Creates new DB for prototypes.</summary>
    /// <param name="activeMod">
    /// Optionally, active mod is set to this parameter. This is for external layout too that cannot set active mod.
    /// </param>
    public ProtosDb(IMod activeMod = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_protosLockedOnInit = new Set<Proto>();
      this.m_protoById = new Dict<Proto.ID, Proto>();
      this.m_protoByType = new Dict<Type, Set<Proto>>();
      this.m_propertyIdsToTrack = new Set<string>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_activeMod = Option<IMod>.Create(activeMod);
    }

    /// <summary>
    /// Makes the DB readonly and initializes all registered protos.
    /// </summary>
    void IProtosDbFriend.LockAndInitializeProtos()
    {
      if (this.IsReadonly)
      {
        Log.Error("ProtosDB is already initialized and locked.");
      }
      else
      {
        this.IsReadonly = true;
        this.m_activeMod = Option<IMod>.None;
        foreach (IProtoInternalFriend protoInternalFriend in (IEnumerable<Proto>) this.m_protoById.Values.OrderBy<Proto, Proto.ID>((Func<Proto, Proto.ID>) (x => x.Id)))
          protoInternalFriend.InitializeInternal(this);
      }
    }

    /// <summary>
    /// The proto is required just so we can enforce that it implements IProtoWithPropertiesUpdate
    /// </summary>
    public void TrackProperty(IProtoWithPropertiesUpdate proto, string propertyId)
    {
      this.m_propertyIdsToTrack.Add(propertyId);
    }

    public T Add<T>(T proto, bool lockOnInit = false) where T : Proto
    {
      if (this.IsReadonly)
        throw new InvalidOperationException(string.Format("Failed to add '{0}': DB is LOCKED at this stage.", (object) proto));
      if (proto.IsPhantom)
        throw new InvalidOperationException(string.Format("Addition of phantom protos to DB is forbidden: '{0}'.", (object) proto));
      if (this.m_activeMod.HasValue)
        proto.SetMod(this.m_activeMod.Value);
      Proto proto1;
      if (this.m_protoById.TryGetValue(proto.Id, out proto1))
        throw new InvalidOperationException(string.Format("Failed to add prototype '{0}' ({1}): ", (object) proto, (object) proto.GetType().Name) + string.Format("ID already exists in DB! Existing proto '{0}' ({1}) ", (object) proto1, (object) proto1.GetType().Name) + "was added by '" + proto1.Mod.Name + "' mod.");
      this.m_protoById.Add(proto.Id, (Proto) proto);
      Type type;
      for (type = proto.GetType(); type != typeof (Proto); type = type.BaseType)
        this.addAsType(type, (Proto) proto);
      this.addAsType(type, (Proto) proto);
      if (lockOnInit)
        this.m_protosLockedOnInit.Add((Proto) proto);
      return proto;
    }

    public Option<Proto> TryRemove(Proto.ID id)
    {
      if (this.IsReadonly)
        throw new InvalidOperationException(string.Format("Failed to remove '{0}': DB is LOCKED at this stage.", (object) id));
      Proto x;
      if (!this.m_protoById.TryGetValue(id, out x))
        return Option<Proto>.None;
      if (!this.m_protoById.Remove(x.Id))
        throw new InvalidOperationException(string.Format("Failed to remove prototype '{0}': ID not in DB!", (object) x));
      Type type;
      for (type = x.GetType(); type != typeof (Proto); type = type.BaseType)
        this.removeAsType(type, x);
      this.removeAsType(type, x);
      return (Option<Proto>) x;
    }

    public Proto RemoveOrThrow(Proto.ID id)
    {
      Option<Proto> option = this.TryRemove(id);
      return !option.IsNone ? option.Value : throw new KeyNotFoundException(string.Format("Failed to remove prototype '{0}': ID not in DB!", (object) id));
    }

    public Option<Proto> Get(Proto.ID id) => this.m_protoById.Get<Proto.ID, Proto>(id);

    public Option<T> Get<T>(Proto.ID id) where T : Proto
    {
      return this.m_protoById.Get<Proto.ID, Proto>(id).As<T>();
    }

    /// <summary>
    /// Tries to get the required proto and logs error message if the proto was not found.
    /// </summary>
    public Option<T> GetOrLog<T>(Proto.ID id) where T : Proto
    {
      Proto proto;
      if (!this.m_protoById.TryGetValue(id, out proto))
      {
        Log.Error(string.Format("Proto '{0}' was not found.", (object) id));
        return Option<T>.None;
      }
      if (proto is T orLog)
        return (Option<T>) orLog;
      Log.Error(string.Format("Proto '{0}' was found but couldn't be cast to {1}.", (object) id, (object) typeof (T).Name));
      return Option<T>.None;
    }

    public bool TryGetProto<T>(Proto.ID id, out T proto) where T : Proto
    {
      Proto proto1;
      if (this.m_protoById.TryGetValue(id, out proto1))
      {
        if (proto1 is T obj)
        {
          proto = obj;
          return true;
        }
        Log.Warning(string.Format("TryGetProto: Proto with ID '{0}' was found but it did not match ", (object) id) + "requested type '" + typeof (T).Name + "' since it is '" + proto1.GetType().Name + "'.");
      }
      proto = default (T);
      return false;
    }

    public bool TryFindProtoIgnoreCase<T>(string id, out T proto) where T : Proto
    {
      Proto proto1;
      if (this.m_protoById.TryGetValue(new Proto.ID(id), out proto1))
      {
        if (proto1 is T obj)
        {
          proto = obj;
          return true;
        }
        Log.Warning("TryFindProtoIgnoreCase: Proto with ID '" + id + "' was found but it did not match requested type '" + typeof (T).Name + "' since it is '" + proto1.GetType().Name + "'.");
        proto = default (T);
        return false;
      }
      foreach (KeyValuePair<Proto.ID, Proto> keyValuePair in this.m_protoById)
      {
        if (keyValuePair.Key.Value.Equals(id, StringComparison.OrdinalIgnoreCase))
        {
          if (keyValuePair.Value is T obj)
          {
            proto = obj;
            return true;
          }
          Log.Warning("TryFindProtoIgnoreCase: Proto with ID '" + id + "' was found but it did not match requested type '" + typeof (T).Name + "' since it is '" + keyValuePair.Value.GetType().Name + "'.");
          proto = default (T);
          return false;
        }
      }
      proto = default (T);
      return false;
    }

    /// <summary>
    /// Returns proto or throws <see cref="T:Mafi.Core.ProtoBuilderException" />. This should be called only during the game
    /// initialization. During the game call <see cref="M:Mafi.Core.Prototypes.ProtosDb.Get(Mafi.Core.Prototypes.Proto.ID)" /> instead!
    /// </summary>
    public T GetOrThrow<T>(Proto.ID id) where T : Proto
    {
      Proto proto;
      if (!this.m_protoById.TryGetValue(id, out proto))
        throw new ProtoBuilderException(string.Format("Proto '{0}' was not found.", (object) id));
      return proto is T obj ? obj : throw new ProtoBuilderException(string.Format("Proto '{0}' was found but couldn't be cast to {1}.", (object) id, (object) typeof (T).Name));
    }

    public IEnumerable<T> All<T>() where T : Proto
    {
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(typeof (T));
      return option.IsNone ? Enumerable.Empty<T>() : option.Value.Cast<T>();
    }

    public T AnyOrDefault<T>() where T : Proto
    {
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(typeof (T));
      return option.IsNone || option.Value.IsEmpty ? default (T) : option.Value.First<Proto>() as T;
    }

    internal IEnumerable<Proto> All(Type protoType)
    {
      Assert.That<Type>(protoType).IsAssignableTo<Proto>();
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(protoType);
      return !option.HasValue ? Enumerable.Empty<Proto>() : (IEnumerable<Proto>) option.Value;
    }

    internal IEnumerable<Proto> All() => (IEnumerable<Proto>) this.m_protoById.Values;

    public IEnumerable<T> Filter<T>(Func<T, bool> predicate) where T : Proto
    {
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(typeof (T));
      return option.IsNone ? Enumerable.Empty<T>() : option.Value.OfType<T>().Where<T>(predicate);
    }

    /// <summary>
    /// This can be called only during the game initialization. Don't call this during the game!
    /// </summary>
    public T FilterExactlyOneOrThrow<T>(Func<T, bool> predicate, string exceptionMessage) where T : Proto
    {
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(typeof (T));
      if (option.IsNone)
        throw new InvalidOperationException(string.Format("Failed to find matching proto ({0}): {1}.", (object) typeof (T), (object) exceptionMessage));
      Lyst<T> lyst = option.Value.OfType<T>().Where<T>(predicate).ToLyst<T>();
      return lyst.Count == 1 ? lyst[0] : throw new InvalidOperationException(string.Format("Expected one proto ({0}) but {1} found: '{2}'.", (object) typeof (T), (object) lyst.Count, (object) exceptionMessage));
    }

    public Option<T> First<T>() where T : Proto
    {
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(typeof (T));
      return option.IsNone ? (Option<T>) Option.None : (Option<T>) (T) option.Value.FirstOrDefault<Proto>();
    }

    public Option<T> First<T>(Proto.ID id) where T : Proto
    {
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(typeof (T));
      return option.IsNone ? (Option<T>) Option.None : (Option<T>) (T) option.Value.FirstOrDefault<Proto>((Func<Proto, bool>) (x => x.Id == id));
    }

    public Option<T> First<T>(Func<T, bool> predicate) where T : Proto
    {
      Option<Set<Proto>> option = this.m_protoByType.Get<Type, Set<Proto>>(typeof (T));
      return option.IsNone ? (Option<T>) Option.None : (Option<T>) (T) option.Value.FirstOrDefault<Proto>((Func<Proto, bool>) (x => predicate((T) x)));
    }

    private void addAsType(Type type, Proto x)
    {
      Set<Proto> set;
      if (!this.m_protoByType.TryGetValue(type, out set))
      {
        set = new Set<Proto>();
        this.m_protoByType.Add(type, set);
      }
      set.Add(x);
    }

    private void removeAsType(Type type, Proto x)
    {
      Set<Proto> set;
      if (!this.m_protoByType.TryGetValue(type, out set))
        return;
      Assert.That<bool>(set.Remove(x)).IsTrue();
      if (set.Count != 0)
        return;
      Assert.That<bool>(this.m_protoByType.Remove(type)).IsTrue();
    }
  }
}
