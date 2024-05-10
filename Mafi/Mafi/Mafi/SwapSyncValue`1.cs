// Decompiled with JetBrains decompiler
// Type: Mafi.SwapSyncValue`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  public class SwapSyncValue<T> : ISwapSyncValue<T>, IReadOnlySwapSyncValue<T> where T : new()
  {
    private T m_readValue;
    private T m_writeValue;

    public SwapSyncValue()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_readValue = new T();
      this.m_writeValue = new T();
    }

    public T Value
    {
      get => this.m_readValue;
      set => this.m_writeValue = value;
    }

    public void Swap() => Mafi.Swap.Them<T>(ref this.m_readValue, ref this.m_writeValue);
  }
}
