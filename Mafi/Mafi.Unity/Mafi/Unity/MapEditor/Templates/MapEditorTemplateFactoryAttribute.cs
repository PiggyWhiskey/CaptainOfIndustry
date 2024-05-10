// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.MapEditorTemplateFactoryAttribute
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  /// <summary>
  /// Marks classes that implement <see cref="T:Mafi.Unity.MapEditor.Templates.ITerrainFeatureTemplate" /> for registration in the map editor.
  /// </summary>
  /// <remarks>
  /// This attribute is not strictly necessary, but it helps with discoverability since the
  /// <see cref="T:Mafi.Unity.MapEditor.Templates.ITerrainFeatureTemplate" /> is often hidden in base classes, and it is not immediately clear
  /// what these classes do since they seem to be not used.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Class)]
  internal sealed class MapEditorTemplateFactoryAttribute : Attribute
  {
    public MapEditorTemplateFactoryAttribute()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
