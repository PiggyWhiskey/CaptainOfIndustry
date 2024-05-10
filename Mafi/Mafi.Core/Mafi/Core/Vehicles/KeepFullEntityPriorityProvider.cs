// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.KeepFullEntityPriorityProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class KeepFullEntityPriorityProvider : IInputBufferPriorityProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IEntityWithGeneralPriority m_entity;

    public static void Serialize(KeepFullEntityPriorityProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<KeepFullEntityPriorityProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, KeepFullEntityPriorityProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IEntityWithGeneralPriority>(this.m_entity);
    }

    public static KeepFullEntityPriorityProvider Deserialize(BlobReader reader)
    {
      KeepFullEntityPriorityProvider priorityProvider;
      if (reader.TryStartClassDeserialization<KeepFullEntityPriorityProvider>(out priorityProvider))
        reader.EnqueueDataDeserialization((object) priorityProvider, KeepFullEntityPriorityProvider.s_deserializeDataDelayedAction);
      return priorityProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<KeepFullEntityPriorityProvider>(this, "m_entity", (object) reader.ReadGenericAs<IEntityWithGeneralPriority>());
    }

    public KeepFullEntityPriorityProvider(IEntityWithGeneralPriority entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entity = entity;
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      return new BufferStrategy(this.m_entity.GeneralPriority, new Quantity?(buffer.UsableCapacity - pendingQuantity));
    }

    static KeepFullEntityPriorityProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      KeepFullEntityPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((KeepFullEntityPriorityProvider) obj).SerializeData(writer));
      KeepFullEntityPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((KeepFullEntityPriorityProvider) obj).DeserializeData(reader));
    }
  }
}
