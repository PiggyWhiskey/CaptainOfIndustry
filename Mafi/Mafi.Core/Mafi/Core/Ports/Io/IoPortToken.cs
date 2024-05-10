// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IoPortToken
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  public readonly struct IoPortToken
  {
    public static readonly IoPortToken Invalid;
    /// <summary>Index withing parent array of ports.</summary>
    public readonly byte PortIndex;
    public readonly char Name;

    public IoPortToken(IoPort port)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.PortIndex = port.PortIndex;
      this.Name = port.Name;
    }

    static IoPortToken() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
  }
}
