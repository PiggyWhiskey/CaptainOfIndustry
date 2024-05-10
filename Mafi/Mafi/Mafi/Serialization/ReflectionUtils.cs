// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.ReflectionUtils
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Reflection;

#nullable disable
namespace Mafi.Serialization
{
  public static class ReflectionUtils
  {
    /// <summary>
    /// For structs use the other overload. You might need to assign the boxed value back if it was a member
    /// in some class.
    /// Note: Prefer using <see cref="M:Mafi.Serialization.BlobReader.SetField``1(``0,System.String,System.Object)" /> directly for better perf if you can.
    /// </summary>
    public static void SetField<T>(T obj, string fieldName, object value) where T : class
    {
      ReflectionUtils.SetField((object) obj, typeof (T), fieldName, value);
    }

    public static void SetField(object obj, Type type, string fieldName, object value)
    {
      FieldInfo field = type.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (field == (FieldInfo) null)
        Log.Error("Field '" + fieldName + "' on object '" + type.Name + "' does not exist.");
      else
        field.SetValue(obj, value);
    }
  }
}
