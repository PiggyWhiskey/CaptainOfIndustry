// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.Extensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Syncers
{
  public static class Extensions
  {
    [MustUseReturnValue]
    public static Extensions.BooleanCallBuilder DoIfTrue(
      this TriggerBuilder<bool> syncer,
      Action callback)
    {
      return new Extensions.BooleanCallBuilder(callback, syncer);
    }

    [MustUseReturnValue]
    public static Extensions.OptionCallBuilder<T> DoIfHasValue<T>(
      this TriggerBuilder<Option<T>> syncer,
      Action<T> callback)
      where T : class
    {
      return new Extensions.OptionCallBuilder<T>(callback, syncer);
    }

    public class BooleanCallBuilder
    {
      private readonly Action m_callIfTrue;
      private readonly TriggerBuilder<bool> m_builder;

      internal BooleanCallBuilder(Action callIfTrue, TriggerBuilder<bool> builder)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_callIfTrue = callIfTrue;
        this.m_builder = builder;
      }

      public UpdaterBuilder Done()
      {
        return this.m_builder.Do((Action<bool>) (val =>
        {
          if (!val)
            return;
          this.m_callIfTrue();
        }));
      }

      public UpdaterBuilder DoIfFalse(Action callIfFalse)
      {
        return this.m_builder.Do((Action<bool>) (val =>
        {
          if (val)
            this.m_callIfTrue();
          else
            callIfFalse();
        }));
      }
    }

    public class OptionCallBuilder<T> where T : class
    {
      private readonly Action<T> m_callIfHasValue;
      private readonly TriggerBuilder<Option<T>> m_builder;

      internal OptionCallBuilder(Action<T> callIfHasValue, TriggerBuilder<Option<T>> builder)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_callIfHasValue = callIfHasValue;
        this.m_builder = builder;
      }

      public UpdaterBuilder DoIfNone(Action doIfNone = null)
      {
        return doIfNone != null ? this.m_builder.Do((Action<Option<T>>) (val =>
        {
          if (val.HasValue)
            this.m_callIfHasValue(val.Value);
          else
            doIfNone();
        })) : this.m_builder.Do((Action<Option<T>>) (val =>
        {
          if (!val.HasValue)
            return;
          this.m_callIfHasValue(val.Value);
        }));
      }
    }
  }
}
