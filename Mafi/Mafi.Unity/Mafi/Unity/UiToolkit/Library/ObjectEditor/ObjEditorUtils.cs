// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorUtils
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Reflection;
using System.Text.RegularExpressions;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  public static class ObjEditorUtils
  {
    private static readonly Regex SPLIT_REGEX;

    public static string ProcessCamelCase(string input)
    {
      input = input.Replace('_', ' ');
      return ObjEditorUtils.SPLIT_REGEX.Replace(input, " $1").Trim();
    }

    public static Type GetMemberType(MemberInfo member)
    {
      Type propertyType = (member as PropertyInfo)?.PropertyType;
      return (object) propertyType != null ? propertyType : ((FieldInfo) member).FieldType;
    }

    public static object GetValue(MemberInfo member, object targetObject)
    {
      PropertyInfo propertyInfo = member as PropertyInfo;
      return (object) propertyInfo != null ? propertyInfo.GetValue(targetObject) : ((FieldInfo) member).GetValue(targetObject);
    }

    public static void SetValue(MemberInfo member, object targetObject, object value)
    {
      PropertyInfo propertyInfo = member as PropertyInfo;
      if ((object) propertyInfo != null)
        propertyInfo.SetValue(targetObject, value);
      else
        (member as FieldInfo)?.SetValue(targetObject, value);
    }

    static ObjEditorUtils()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ObjEditorUtils.SPLIT_REGEX = new Regex("([A-Z])", RegexOptions.Compiled);
    }
  }
}
