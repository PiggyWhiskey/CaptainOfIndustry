// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportFlow
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct TransportFlow
  {
    /// <summary>No products and no flow.</summary>
    public static readonly TransportFlow Empty;
    private readonly ColorRgba m_color;

    public TransportFlow(bool isFlowing, ColorRgba productColor)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_color = productColor.SetA(isFlowing ? byte.MaxValue : (byte) 254);
    }

    public ColorRgba Color => this.m_color.SetA(byte.MaxValue);

    public bool IsFlowing => this.m_color.A == byte.MaxValue;

    public bool HasProducts => this.m_color.IsNotEmpty;

    static TransportFlow() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
  }
}
