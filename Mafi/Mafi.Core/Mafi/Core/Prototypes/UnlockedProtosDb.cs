// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.UnlockedProtosDb
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.GameLoop;
using Mafi.Core.Research;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Prototypes
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class UnlockedProtosDb
  {
    private readonly Event m_onUnlockedSetChanged;
    [OnlyForSaveCompatibility(null)]
    private readonly Set<Proto> m_unlockedProtos;
    private readonly ProtosDb m_protosDb;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Called when the set of unlocked protos gets changed. Called on sim thread.
    /// </summary>
    public IEvent OnUnlockedSetChanged => (IEvent) this.m_onUnlockedSetChanged;

    public UnlockedProtosDb(
      ProtosDb protosDb,
      IUnlockedProtosConfig config,
      INewGameCreatedEvents gameLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onUnlockedSetChanged = new Event();
      this.m_unlockedProtos = new Set<Proto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      UnlockedProtosDb unlockedProtosDb = this;
      gameLoopEvents.RegisterNewGameCreated((object) this, (Action) (() =>
      {
        unlockedProtosDb.m_unlockedProtos.AddRange(protosDb.All<Proto>().Where<Proto>((Func<Proto, bool>) (x => !x.IsNotAvailable)));
        if (config.ShouldUnlockAllProtosOnInit)
        {
          unlockedProtosDb.m_onUnlockedSetChanged?.Invoke();
        }
        else
        {
          ResearchManager.LockProtosFromResearchTree(unlockedProtosDb.m_unlockedProtos, protosDb);
          foreach (Proto proto in protosDb.All<TechnologyProto>())
            unlockedProtosDb.m_unlockedProtos.Remove(proto);
          unlockedProtosDb.m_unlockedProtos.RemoveRange((IEnumerable<Proto>) protosDb.ProtosLockedOnInit);
          unlockedProtosDb.m_onUnlockedSetChanged?.Invoke();
        }
      }));
    }

    [InitAfterLoad(InitPriority.High)]
    private void initAfterLoad()
    {
      int num = this.m_unlockedProtos.RemoveWhere((Predicate<Proto>) (x => x.IsPhantom));
      if (num <= 0)
        return;
      Log.Warning(string.Format("Removed {0} phantom protos after load.", (object) num));
    }

    public void OnProtoAvailabilityChanged(Proto proto)
    {
      if (!proto.IsAvailable || !this.m_unlockedProtos.Contains(proto))
        return;
      this.m_onUnlockedSetChanged?.Invoke();
    }

    private UnlockedProtosDb(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onUnlockedSetChanged = new Event();
      this.m_unlockedProtos = new Set<Proto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_unlockedProtos.AddRange(protosDb.All<Proto>());
    }

    public static UnlockedProtosDb CreateAllUnlocked(ProtosDb protosDb)
    {
      return new UnlockedProtosDb(protosDb);
    }

    public IEnumerable<T> AllUnlocked<T>() where T : IProto
    {
      return this.m_unlockedProtos.OfType<T>().Where<T>((Func<T, bool>) (x => !x.IsNotAvailable));
    }

    public IEnumerable<T> FilterUnlocked<T>(Func<T, bool> predicate) where T : IProto
    {
      return this.m_unlockedProtos.OfType<T>().Where<T>(predicate);
    }

    public IEnumerable<T> FilterUnlocked<T>(IEnumerable<T> candidates) where T : IProto
    {
      return candidates.Where<T>((Func<T, bool>) (x => x.IsAvailable && this.m_unlockedProtos.OfType<T>().Contains<T>(x)));
    }

    public bool AnyUnlocked<T>() where T : Proto
    {
      foreach (Proto unlockedProto in this.m_unlockedProtos)
      {
        if (unlockedProto.IsAvailable && unlockedProto is T)
          return true;
      }
      return false;
    }

    public bool IsUnlocked(Proto proto)
    {
      return proto.IsAvailable && this.m_unlockedProtos.Contains(proto);
    }

    public bool IsUnlocked(IProto proto)
    {
      return proto.IsAvailable && ((IEnumerable<IProto>) this.m_unlockedProtos).Contains<IProto>(proto);
    }

    public bool IsLocked(IProto proto)
    {
      return proto.IsNotAvailable || !((IEnumerable<IProto>) this.m_unlockedProtos).Contains<IProto>(proto);
    }

    public bool IsLockedButAvailable(IProto proto)
    {
      return proto.IsAvailable && !((IEnumerable<IProto>) this.m_unlockedProtos).Contains<IProto>(proto);
    }

    /// <summary>
    /// Marks the given protos as locked. Does not matter if a proto was already locked. Make sure to batch all your
    /// changes to a single call. Otherwise you will trigger multiple callbacks which will lead to unnecessary UI
    /// redraws.
    /// </summary>
    public void Lock(IEnumerable<Proto> protos)
    {
      bool flag = false;
      foreach (Proto proto in protos)
        flag |= this.m_unlockedProtos.Remove(proto);
      if (!flag)
        return;
      this.m_onUnlockedSetChanged.Invoke();
    }

    public void Lock(Proto proto)
    {
      if (!this.m_unlockedProtos.Remove(proto))
        return;
      this.m_onUnlockedSetChanged.Invoke();
    }

    /// <summary>
    /// Marks the given protos as unlocked. Does not matter if a proto was already unlocked. Make sure to batch all
    /// your changes to a single call. Otherwise you will trigger multiple callbacks which will lead to unnecessary
    /// UI redraws.
    /// </summary>
    public void Unlock(ImmutableArray<IProto> protos)
    {
      bool flag = false;
      foreach (IProto proto1 in protos)
      {
        Proto proto2 = proto1 as Proto;
        if ((object) proto2 != null)
          flag |= this.m_unlockedProtos.Add(proto2);
      }
      if (!flag)
        return;
      this.m_onUnlockedSetChanged.Invoke();
    }

    public void Unlock(Proto proto)
    {
      if (!this.m_unlockedProtos.Add(proto))
        return;
      this.m_onUnlockedSetChanged.Invoke();
    }

    public static void Serialize(UnlockedProtosDb value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnlockedProtosDb>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnlockedProtosDb.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Event.Serialize(this.m_onUnlockedSetChanged, writer);
      Set<Proto>.Serialize(this.m_unlockedProtos, writer);
    }

    public static UnlockedProtosDb Deserialize(BlobReader reader)
    {
      UnlockedProtosDb unlockedProtosDb;
      if (reader.TryStartClassDeserialization<UnlockedProtosDb>(out unlockedProtosDb))
        reader.EnqueueDataDeserialization((object) unlockedProtosDb, UnlockedProtosDb.s_deserializeDataDelayedAction);
      return unlockedProtosDb;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UnlockedProtosDb>(this, "m_onUnlockedSetChanged", (object) Event.Deserialize(reader));
      reader.RegisterResolvedMember<UnlockedProtosDb>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<UnlockedProtosDb>(this, "m_unlockedProtos", (object) Set<Proto>.Deserialize(reader));
      reader.RegisterInitAfterLoad<UnlockedProtosDb>(this, "initAfterLoad", InitPriority.High);
    }

    static UnlockedProtosDb()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnlockedProtosDb.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UnlockedProtosDb) obj).SerializeData(writer));
      UnlockedProtosDb.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UnlockedProtosDb) obj).DeserializeData(reader));
    }
  }
}
