// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.MovingAverageHelper
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Utils
{
  /// <summary>
  /// Calculates moving average from the data given.
  /// 
  /// The average is calculated from last windowSize points.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class MovingAverageHelper
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly int m_windowSize;
    private readonly Queueue<int> m_data;

    public static void Serialize(MovingAverageHelper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MovingAverageHelper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MovingAverageHelper.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Queueue<int>.Serialize(this.m_data, writer);
      writer.WriteInt(this.m_windowSize);
    }

    public static MovingAverageHelper Deserialize(BlobReader reader)
    {
      MovingAverageHelper movingAverageHelper;
      if (reader.TryStartClassDeserialization<MovingAverageHelper>(out movingAverageHelper))
        reader.EnqueueDataDeserialization((object) movingAverageHelper, MovingAverageHelper.s_deserializeDataDelayedAction);
      return movingAverageHelper;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MovingAverageHelper>(this, "m_data", (object) Queueue<int>.Deserialize(reader));
      reader.SetField<MovingAverageHelper>(this, "m_windowSize", (object) reader.ReadInt());
    }

    public MovingAverageHelper(int windowSize)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_windowSize = windowSize.CheckPositive();
      this.m_data = new Queueue<int>(this.m_windowSize);
    }

    public void AddItem(int value)
    {
      if (this.m_data.Count == this.m_windowSize)
        this.m_data.Dequeue();
      this.m_data.Enqueue(value);
    }

    public int GetAvg()
    {
      if (this.m_data.Count == 0)
      {
        Log.Error("Invalid request for average");
        return 0;
      }
      int num = 0;
      for (int index = 0; index < this.m_data.Count; ++index)
        num += this.m_data[index];
      return num / this.m_data.Count;
    }

    static MovingAverageHelper()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      MovingAverageHelper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MovingAverageHelper) obj).SerializeData(writer));
      MovingAverageHelper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MovingAverageHelper) obj).DeserializeData(reader));
    }
  }
}
