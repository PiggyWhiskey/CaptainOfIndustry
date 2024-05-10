// Decompiled with JetBrains decompiler
// Type: Mafi.IoPortTypeExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class IoPortTypeExtensions
  {
    public static bool IsCompatibleWith(this IoPortType type, IoPortType otherType)
    {
      return type != otherType;
    }
  }
}
