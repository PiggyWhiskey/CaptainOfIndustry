// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.ElectricityConsumerFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Notifications;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ElectricityConsumerFactory : IElectricityConsumerFactory
  {
    private readonly ElectricityManager m_electricityManager;
    private readonly INotificationsManager m_notificationsManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ElectricityConsumerFactory(
      ElectricityManager electricityManager,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_electricityManager = electricityManager;
      this.m_notificationsManager = notificationsManager;
    }

    public IElectricityConsumer CreateConsumer(IElectricityConsumingEntity entity)
    {
      return (IElectricityConsumer) new ElectricityConsumer(entity, this.m_electricityManager, this.m_notificationsManager);
    }

    public static void Serialize(ElectricityConsumerFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ElectricityConsumerFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ElectricityConsumerFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ElectricityManager.Serialize(this.m_electricityManager, writer);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
    }

    public static ElectricityConsumerFactory Deserialize(BlobReader reader)
    {
      ElectricityConsumerFactory electricityConsumerFactory;
      if (reader.TryStartClassDeserialization<ElectricityConsumerFactory>(out electricityConsumerFactory))
        reader.EnqueueDataDeserialization((object) electricityConsumerFactory, ElectricityConsumerFactory.s_deserializeDataDelayedAction);
      return electricityConsumerFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ElectricityConsumerFactory>(this, "m_electricityManager", (object) ElectricityManager.Deserialize(reader));
      reader.SetField<ElectricityConsumerFactory>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
    }

    static ElectricityConsumerFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ElectricityConsumerFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ElectricityConsumerFactory) obj).SerializeData(writer));
      ElectricityConsumerFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ElectricityConsumerFactory) obj).DeserializeData(reader));
    }
  }
}
