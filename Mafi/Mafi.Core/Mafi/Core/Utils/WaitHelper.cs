// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.WaitHelper
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
  /// Helper class that counts ticks of waiting. It can handle increasing delay of consecutive failures. For usage see
  /// example below.
  /// </summary>
  /// <example>
  /// This is intended use of the WaitHelper class.
  /// <code>
  /// WaitHelper waiter;
  /// 
  /// bool SearchSomething() {
  /// 	if (waiter.WaitOne()) {
  /// 		return false; // Waiting.
  /// 	}
  /// 
  /// 	if (search()) {
  /// 		waiter.ResetExtraWaitTime(); // Resets retries and wait time.
  /// 		return true; // Success.
  /// 	} else if (waiter.StartWait(true)) {
  /// 		return false;
  /// 	} else {
  /// 		waiter.ResetExtraWaitTime();
  /// 		return true; // Max retries was reached, do something about it!
  /// 	}
  /// }
  /// </code>
  /// </example>
  [GenerateSerializer(false, null, 0)]
  public class WaitHelper
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>Increase by 20%.</summary>
    private static readonly Percent PROGRESSIVE_MULT;
    private readonly int m_maxRetries;
    private readonly bool m_progressivelyIncreaseWaitTime;
    /// <summary>Initial number of waiting ticks.</summary>
    private readonly int m_initialWaitTicks;
    private int m_currTicks;
    private int m_maxTicksWait;
    /// <summary>Current count of consecutive retries from last reset.</summary>
    private int m_retriesCount;

    public static void Serialize(WaitHelper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WaitHelper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WaitHelper.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.m_currTicks);
      writer.WriteInt(this.m_initialWaitTicks);
      writer.WriteInt(this.m_maxRetries);
      writer.WriteInt(this.m_maxTicksWait);
      writer.WriteBool(this.m_progressivelyIncreaseWaitTime);
      writer.WriteInt(this.m_retriesCount);
    }

    public static WaitHelper Deserialize(BlobReader reader)
    {
      WaitHelper waitHelper;
      if (reader.TryStartClassDeserialization<WaitHelper>(out waitHelper))
        reader.EnqueueDataDeserialization((object) waitHelper, WaitHelper.s_deserializeDataDelayedAction);
      return waitHelper;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_currTicks = reader.ReadInt();
      reader.SetField<WaitHelper>(this, "m_initialWaitTicks", (object) reader.ReadInt());
      reader.SetField<WaitHelper>(this, "m_maxRetries", (object) reader.ReadInt());
      this.m_maxTicksWait = reader.ReadInt();
      reader.SetField<WaitHelper>(this, "m_progressivelyIncreaseWaitTime", (object) reader.ReadBool());
      this.m_retriesCount = reader.ReadInt();
    }

    public WaitHelper(int maxRetries, bool progressivelyIncreaseWaitTime = true, int initialWaitTicks = 5)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_maxRetries = maxRetries.CheckPositive();
      this.m_progressivelyIncreaseWaitTime = progressivelyIncreaseWaitTime;
      this.m_initialWaitTicks = initialWaitTicks.CheckPositive();
      this.m_maxTicksWait = this.m_initialWaitTicks.CheckGreaterOrEqual(initialWaitTicks);
    }

    /// <summary>
    /// Starts the wait and optionally increases the wait time for the next wait. Also increases retries count.
    /// Returns false if maximum retries was reached.
    /// </summary>
    public bool StartWait(Duration extraWait = default (Duration))
    {
      Assert.That<int>(this.m_retriesCount).IsLess(this.m_maxRetries, "Forgot to restart?");
      this.m_currTicks = this.m_maxTicksWait + extraWait.Ticks;
      if (this.m_progressivelyIncreaseWaitTime)
        this.m_maxTicksWait = WaitHelper.PROGRESSIVE_MULT.ApplyCeiled(this.m_maxTicksWait).Min(23);
      return this.m_retriesCount++ < this.m_maxRetries;
    }

    /// <summary>
    /// Increases internal ticks count and returns if the caller should keep waiting. Returns true if the caller
    /// should wait more. It is intended that this statement is in a if branch that has return statement.
    /// </summary>
    public bool WaitOne() => this.m_currTicks-- > 0;

    /// <summary>
    /// Resets retries count and optionally extra wait time that was added by <see cref="M:Mafi.Core.Utils.WaitHelper.StartWait(Mafi.Duration)" />.
    /// </summary>
    public void Reset()
    {
      this.m_retriesCount = 0;
      this.m_maxTicksWait = this.m_initialWaitTicks;
    }

    public override string ToString()
    {
      return this.m_currTicks > 0 ? string.Format("Waiting {0} (max {1})", (object) this.m_currTicks, (object) this.m_maxTicksWait) : string.Format("Not waiting (max {0})", (object) this.m_maxTicksWait);
    }

    static WaitHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WaitHelper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WaitHelper) obj).SerializeData(writer));
      WaitHelper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WaitHelper) obj).DeserializeData(reader));
      WaitHelper.PROGRESSIVE_MULT = 120.Percent();
    }
  }
}
