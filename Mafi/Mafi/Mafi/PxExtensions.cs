// Decompiled with JetBrains decompiler
// Type: Mafi.PxExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class PxExtensions
  {
    public static Px px(this int value) => new Px((float) value);

    public static Px px(this float value) => new Px(value);

    public static Px pt(this int value) => new Px((float) (4 * value));
  }
}
