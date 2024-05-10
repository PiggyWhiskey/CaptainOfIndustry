// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.RelGameDateTimer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  /// <summary>Simple timer that counts in game date.</summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class RelGameDateTimer
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(RelGameDateTimer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RelGameDateTimer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RelGameDateTimer.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer) => RelGameDate.Serialize(this.Remaining, writer);

    public static RelGameDateTimer Deserialize(BlobReader reader)
    {
      RelGameDateTimer relGameDateTimer;
      if (reader.TryStartClassDeserialization<RelGameDateTimer>(out relGameDateTimer))
        reader.EnqueueDataDeserialization((object) relGameDateTimer, RelGameDateTimer.s_deserializeDataDelayedAction);
      return relGameDateTimer;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.Remaining = RelGameDate.Deserialize(reader);
    }

    /// <summary>
    /// Remaining date interval left. This number may be negative if timer was decremented too many times.
    /// </summary>
    public RelGameDate Remaining { get; private set; }

    /// <summary>Whether number of remaining time is zero (or less).</summary>
    public bool IsFinished => this.Remaining <= RelGameDate.Zero;

    public bool IsNotFinished => this.Remaining > RelGameDate.Zero;

    /// <summary>Starts the timer.</summary>
    public void Start(RelGameDate relDate) => this.Remaining = relDate.CheckNotNegative();

    /// <summary>
    /// Decrements the timer by one month and returns <c>true</c> if the timer is still not finished.
    /// </summary>
    public bool DecrementOneMonth()
    {
      this.Remaining -= RelGameDate.OneMonth;
      return this.IsNotFinished;
    }

    public RelGameDateTimer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static RelGameDateTimer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RelGameDateTimer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RelGameDateTimer) obj).SerializeData(writer));
      RelGameDateTimer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RelGameDateTimer) obj).DeserializeData(reader));
    }
  }
}
