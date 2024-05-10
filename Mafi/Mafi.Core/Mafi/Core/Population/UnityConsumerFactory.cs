// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.UnityConsumerFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Notifications;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class UnityConsumerFactory : IUnityConsumerFactory
  {
    private readonly ICalendar m_calendar;
    private readonly INotificationsManager m_notificationsManager;
    private readonly IUpointsManager m_upointsManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public UnityConsumerFactory(
      IUpointsManager upointsManager,
      ICalendar calendar,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_upointsManager = upointsManager;
      this.m_calendar = calendar;
      this.m_notificationsManager = notificationsManager;
    }

    public UnityConsumer CreateConsumer(IUnityConsumingEntity entity)
    {
      return new UnityConsumer(this.m_upointsManager, this.m_calendar, this.m_notificationsManager, entity);
    }

    public static void Serialize(UnityConsumerFactory value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnityConsumerFactory>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnityConsumerFactory.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
    }

    public static UnityConsumerFactory Deserialize(BlobReader reader)
    {
      UnityConsumerFactory unityConsumerFactory;
      if (reader.TryStartClassDeserialization<UnityConsumerFactory>(out unityConsumerFactory))
        reader.EnqueueDataDeserialization((object) unityConsumerFactory, UnityConsumerFactory.s_deserializeDataDelayedAction);
      return unityConsumerFactory;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<UnityConsumerFactory>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<UnityConsumerFactory>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<UnityConsumerFactory>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
    }

    static UnityConsumerFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnityConsumerFactory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UnityConsumerFactory) obj).SerializeData(writer));
      UnityConsumerFactory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UnityConsumerFactory) obj).DeserializeData(reader));
    }
  }
}
