// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.QueueJobResultRef
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  public class QueueJobResultRef
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(QueueJobResultRef value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<QueueJobResultRef>(value))
        return;
      writer.EnqueueDataSerialization((object) value, QueueJobResultRef.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteNullableStruct<bool>(this.ReachedGoal);
    }

    public static QueueJobResultRef Deserialize(BlobReader reader)
    {
      QueueJobResultRef queueJobResultRef;
      if (reader.TryStartClassDeserialization<QueueJobResultRef>(out queueJobResultRef))
        reader.EnqueueDataDeserialization((object) queueJobResultRef, QueueJobResultRef.s_deserializeDataDelayedAction);
      return queueJobResultRef;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ReachedGoal = reader.ReadNullableStruct<bool>();
    }

    public bool? ReachedGoal { get; private set; }

    public void ReportResult(bool reachedGoal)
    {
      Assert.That<bool?>(this.ReachedGoal).IsNull<bool>("Result already set.");
      this.ReachedGoal = new bool?(reachedGoal);
    }

    public void ClearResult() => this.ReachedGoal = new bool?();

    public QueueJobResultRef()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static QueueJobResultRef()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      QueueJobResultRef.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((QueueJobResultRef) obj).SerializeData(writer));
      QueueJobResultRef.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((QueueJobResultRef) obj).DeserializeData(reader));
    }
  }
}
