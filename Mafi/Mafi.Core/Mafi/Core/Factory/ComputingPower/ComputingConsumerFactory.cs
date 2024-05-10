// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ComputingPower.ComputingConsumerFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Notifications;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.ComputingPower
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ComputingConsumerFactory : IComputingConsumerFactory
  {
    private readonly ComputingManager m_computingManager;
    private readonly INotificationsManager m_notificationsManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ComputingConsumerFactory(
      ComputingManager computingManager,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_computingManager = computingManager;
      this.m_notificationsManager = notificationsManager;
    }

    public IComputingConsumer CreateConsumer(IComputingConsumingEntity entity)
    {
      return (IComputingConsumer) new ComputingConsumer(entity, this.m_computingManager, this.m_notificationsManager);
    }

    public static void Serialize(ComputingConsumerFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ComputingConsumerFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ComputingConsumerFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ComputingManager.Serialize(this.m_computingManager, writer);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
    }

    public static ComputingConsumerFactory Deserialize(BlobReader reader)
    {
      ComputingConsumerFactory computingConsumerFactory;
      if (reader.TryStartClassDeserialization<ComputingConsumerFactory>(out computingConsumerFactory))
        reader.EnqueueDataDeserialization((object) computingConsumerFactory, ComputingConsumerFactory.s_deserializeDataDelayedAction);
      return computingConsumerFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ComputingConsumerFactory>(this, "m_computingManager", (object) ComputingManager.Deserialize(reader));
      reader.SetField<ComputingConsumerFactory>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
    }

    static ComputingConsumerFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ComputingConsumerFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ComputingConsumerFactory) obj).SerializeData(writer));
      ComputingConsumerFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ComputingConsumerFactory) obj).DeserializeData(reader));
    }
  }
}
