// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.TickTimer
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
  /// <summary>
  /// Simple timer that counts game ticks. TODO: Make this a struct.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class TickTimer
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(TickTimer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TickTimer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TickTimer.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Duration.Serialize(this.StartedAtTicks, writer);
      Duration.Serialize(this.Ticks, writer);
    }

    public static TickTimer Deserialize(BlobReader reader)
    {
      TickTimer tickTimer;
      if (reader.TryStartClassDeserialization<TickTimer>(out tickTimer))
        reader.EnqueueDataDeserialization((object) tickTimer, TickTimer.s_deserializeDataDelayedAction);
      return tickTimer;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.StartedAtTicks = Duration.Deserialize(reader);
      this.Ticks = Duration.Deserialize(reader);
    }

    /// <summary>
    /// Number of ticks left. This number may be negative if timer was decremented too many times.
    /// </summary>
    public Duration Ticks { get; private set; }

    public Duration StartedAtTicks { get; private set; }

    /// <summary>Whether number of remaining ticks is zero (or less).</summary>
    public bool IsFinished => this.Ticks.IsNotPositive;

    public bool IsNotFinished => this.Ticks.IsPositive;

    public Percent PercentFinished
    {
      get
      {
        return !this.IsFinished ? Percent.FromRatio((this.StartedAtTicks - this.Ticks).Ticks, this.StartedAtTicks.Ticks) : Percent.Hundred;
      }
    }

    /// <summary>Starts the timer.</summary>
    public void Start(Duration ticks)
    {
      this.StartedAtTicks = this.Ticks = ticks.CheckNotNegative();
    }

    /// <summary>
    /// Decrements the timer and returns <c>true</c> if the timer is still not finished.
    /// </summary>
    public bool Decrement()
    {
      this.Ticks -= Duration.OneTick;
      return this.IsNotFinished;
    }

    public void DecrementOnly() => this.Ticks -= Duration.OneTick;

    /// <summary>
    /// Resets timer to initial state - Ticks are zero and timer is thus finished.
    /// </summary>
    public void Reset() => this.Ticks = this.StartedAtTicks = Duration.Zero;

    public TickTimer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TickTimer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TickTimer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TickTimer) obj).SerializeData(writer));
      TickTimer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TickTimer) obj).DeserializeData(reader));
    }
  }
}
