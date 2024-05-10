// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.NonComparingSyncer`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Syncers
{
  public sealed class NonComparingSyncer<T> : ISyncer<T>, ISyncer
  {
    private T m_value;
    private readonly Func<T> m_provider;

    public bool HasChanged => false;

    public NonComparingSyncer(Func<T> provider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_provider = provider.CheckNotNull<Func<T>>();
    }

    void ISyncer.SyncUpdate(bool invalidate) => this.m_value = this.m_provider();

    public T GetValueAndReset() => this.m_value;
  }
}
