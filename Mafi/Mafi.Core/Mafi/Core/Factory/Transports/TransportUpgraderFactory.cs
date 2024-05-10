// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportUpgraderFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class TransportUpgraderFactory : ITransportUpgraderFactory
  {
    private readonly INotificationsManager m_notificationsManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly UpgradeCostResolver m_upgradeCostResolver;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public EntityId.Factory EntityIdFactory { get; private set; }

    public TransportUpgraderFactory(
      INotificationsManager notificationsManager,
      EntityId.Factory idFactory,
      UnlockedProtosDb unlockedProtosDb,
      UpgradeCostResolver upgradeCostResolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityIdFactory = idFactory;
      this.m_notificationsManager = notificationsManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_upgradeCostResolver = upgradeCostResolver;
    }

    public IUpgrader CreateInstance(Transport entity)
    {
      return (IUpgrader) new TransportUpgrader(this.m_notificationsManager, this.m_unlockedProtosDb, this.m_upgradeCostResolver, entity);
    }

    public static void Serialize(TransportUpgraderFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TransportUpgraderFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TransportUpgraderFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      EntityId.Factory.Serialize(this.EntityIdFactory, writer);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
    }

    public static TransportUpgraderFactory Deserialize(BlobReader reader)
    {
      TransportUpgraderFactory transportUpgraderFactory;
      if (reader.TryStartClassDeserialization<TransportUpgraderFactory>(out transportUpgraderFactory))
        reader.EnqueueDataDeserialization((object) transportUpgraderFactory, TransportUpgraderFactory.s_deserializeDataDelayedAction);
      return transportUpgraderFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.EntityIdFactory = EntityId.Factory.Deserialize(reader);
      reader.SetField<TransportUpgraderFactory>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<TransportUpgraderFactory>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.RegisterResolvedMember<TransportUpgraderFactory>(this, "m_upgradeCostResolver", typeof (UpgradeCostResolver), true);
    }

    static TransportUpgraderFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportUpgraderFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TransportUpgraderFactory) obj).SerializeData(writer));
      TransportUpgraderFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TransportUpgraderFactory) obj).DeserializeData(reader));
    }
  }
}
