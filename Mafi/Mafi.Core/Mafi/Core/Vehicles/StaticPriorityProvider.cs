// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.StaticPriorityProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class StaticPriorityProvider : IInputBufferPriorityProvider, IOutputBufferPriorityProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly StaticPriorityProvider Ignore;
    public static readonly StaticPriorityProvider LowestNoQuantityPreference;
    private readonly BufferStrategy m_strategy;

    public static void Serialize(StaticPriorityProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StaticPriorityProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StaticPriorityProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      BufferStrategy.Serialize(this.m_strategy, writer);
    }

    public static StaticPriorityProvider Deserialize(BlobReader reader)
    {
      StaticPriorityProvider priorityProvider;
      if (reader.TryStartClassDeserialization<StaticPriorityProvider>(out priorityProvider))
        reader.EnqueueDataDeserialization((object) priorityProvider, StaticPriorityProvider.s_deserializeDataDelayedAction);
      return priorityProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<StaticPriorityProvider>(this, "m_strategy", (object) BufferStrategy.Deserialize(reader));
    }

    public StaticPriorityProvider(BufferStrategy strategy)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_strategy = strategy;
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      return this.m_strategy;
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request) => this.m_strategy;

    static StaticPriorityProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StaticPriorityProvider) obj).SerializeData(writer));
      StaticPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StaticPriorityProvider) obj).DeserializeData(reader));
      StaticPriorityProvider.Ignore = new StaticPriorityProvider(BufferStrategy.Ignore);
      StaticPriorityProvider.LowestNoQuantityPreference = new StaticPriorityProvider(BufferStrategy.NoQuantityPreference(15));
    }
  }
}
