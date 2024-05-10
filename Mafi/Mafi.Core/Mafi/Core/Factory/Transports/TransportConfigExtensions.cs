// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public static class TransportConfigExtensions
  {
    public static void GetTransportColor(
      this EntityConfigData data,
      out ColorRgba? color,
      out ColorRgba? accentColor)
    {
      int? nullable1 = data.GetInt("TransportColor");
      color = nullable1.HasValue ? new ColorRgba?(new ColorRgba((uint) nullable1.Value)) : new ColorRgba?();
      int? nullable2 = data.GetInt("TransportAccentColor");
      accentColor = nullable2.HasValue ? new ColorRgba?(new ColorRgba((uint) nullable2.Value)) : new ColorRgba?();
    }

    public static void SetTransportColor(
      this EntityConfigData data,
      ColorRgba color,
      ColorRgba accentColor)
    {
      data.SetInt("TransportColor", new int?((int) color.Rgba));
      data.SetInt("TransportAccentColor", new int?((int) accentColor.Rgba));
    }
  }
}
