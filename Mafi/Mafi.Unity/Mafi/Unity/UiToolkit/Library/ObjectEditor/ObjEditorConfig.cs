// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorConfig
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Reflection;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  public static class ObjEditorConfig
  {
    public static bool ShouldIgnoreMember(MemberInfo member) => member.Name == "XyPacked";

    public static string GetUnitsSuffix(Type type)
    {
      if (type.IsAssignableTo(typeof (Percent)))
        return "%";
      if (type.IsAssignableTo(typeof (AngleSlim)))
        return "degrees";
      return type.Name.Contains("Tile1") || type.Name.Contains("Tile2") || type.Name.Contains("Tile3") || type.Name.Contains("HeightTiles") ? "tiles" : string.Empty;
    }
  }
}
