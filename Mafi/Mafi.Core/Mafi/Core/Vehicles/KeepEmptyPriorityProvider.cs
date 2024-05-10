// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.KeepEmptyPriorityProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class KeepEmptyPriorityProvider : IOutputBufferPriorityProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly int m_priority;

    public static void Serialize(KeepEmptyPriorityProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<KeepEmptyPriorityProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, KeepEmptyPriorityProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer) => writer.WriteInt(this.m_priority);

    public static KeepEmptyPriorityProvider Deserialize(BlobReader reader)
    {
      KeepEmptyPriorityProvider priorityProvider;
      if (reader.TryStartClassDeserialization<KeepEmptyPriorityProvider>(out priorityProvider))
        reader.EnqueueDataDeserialization((object) priorityProvider, KeepEmptyPriorityProvider.s_deserializeDataDelayedAction);
      return priorityProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<KeepEmptyPriorityProvider>(this, "m_priority", (object) reader.ReadInt());
    }

    public KeepEmptyPriorityProvider(int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_priority = priority;
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      return new BufferStrategy(this.m_priority, new Quantity?(request.Buffer.Quantity - request.PendingQuantity));
    }

    static KeepEmptyPriorityProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      KeepEmptyPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((KeepEmptyPriorityProvider) obj).SerializeData(writer));
      KeepEmptyPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((KeepEmptyPriorityProvider) obj).DeserializeData(reader));
    }
  }
}
