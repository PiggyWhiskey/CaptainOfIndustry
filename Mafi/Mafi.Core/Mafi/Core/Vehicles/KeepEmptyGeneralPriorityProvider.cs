// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.KeepEmptyGeneralPriorityProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Priorities;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class KeepEmptyGeneralPriorityProvider : IOutputBufferPriorityProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IEntityWithGeneralPriority m_entity;

    public static void Serialize(KeepEmptyGeneralPriorityProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<KeepEmptyGeneralPriorityProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, KeepEmptyGeneralPriorityProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IEntityWithGeneralPriority>(this.m_entity);
    }

    public static KeepEmptyGeneralPriorityProvider Deserialize(BlobReader reader)
    {
      KeepEmptyGeneralPriorityProvider priorityProvider;
      if (reader.TryStartClassDeserialization<KeepEmptyGeneralPriorityProvider>(out priorityProvider))
        reader.EnqueueDataDeserialization((object) priorityProvider, KeepEmptyGeneralPriorityProvider.s_deserializeDataDelayedAction);
      return priorityProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<KeepEmptyGeneralPriorityProvider>(this, "m_entity", (object) reader.ReadGenericAs<IEntityWithGeneralPriority>());
    }

    public KeepEmptyGeneralPriorityProvider(IEntityWithGeneralPriority entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entity = entity;
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      return new BufferStrategy(this.m_entity.GeneralPriority, new Quantity?(request.Buffer.Quantity - request.PendingQuantity));
    }

    static KeepEmptyGeneralPriorityProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      KeepEmptyGeneralPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((KeepEmptyGeneralPriorityProvider) obj).SerializeData(writer));
      KeepEmptyGeneralPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((KeepEmptyGeneralPriorityProvider) obj).DeserializeData(reader));
    }
  }
}
