// Decompiled with JetBrains decompiler
// Type: Mafi.UpointsExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class UpointsExtensions
  {
    public static Mafi.Upoints Upoints(this Fix32 value) => new Mafi.Upoints(value);

    public static Mafi.Upoints Upoints(this int value) => new Mafi.Upoints(value);

    public static Mafi.Upoints Upoints(this double value) => new Mafi.Upoints(value.ToFix32());

    public static Mafi.Upoints Upoints(this float value) => new Mafi.Upoints(value.ToFix32());

    public static Mafi.Upoints Upoints(this Quantity value)
    {
      return new Mafi.Upoints(Fix32.FromRaw(value.Value));
    }
  }
}
