// Decompiled with JetBrains decompiler
// Type: Mafi.TypeExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Reflection;

#nullable disable
namespace Mafi
{
  public static class TypeExtensions
  {
    public static bool IsAssignableTo(this Type type, Type toType) => toType.IsAssignableFrom(type);

    public static bool IsAssignableTo<T>(this Type type) => typeof (T).IsAssignableFrom(type);

    public static bool HasAttribute<TAttr>(this Type type, bool inherit = false)
    {
      return type.GetCustomAttributes(typeof (TAttr), inherit).Length != 0;
    }

    public static bool HasAttribute<TAttr>(this MemberInfo memberInfo, bool inherit = false)
    {
      return memberInfo.GetCustomAttributes(typeof (TAttr), inherit).Length != 0;
    }
  }
}
