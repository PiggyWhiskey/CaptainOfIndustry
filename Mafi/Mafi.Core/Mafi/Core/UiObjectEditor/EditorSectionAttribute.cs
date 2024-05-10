// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UiObjectEditor.EditorSectionAttribute
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.UiObjectEditor
{
  /// <summary>
  /// Creates new section in the editor by introducing a standalone title which
  /// will be collapsible.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorSectionAttribute : Attribute
  {
    public readonly string Label;
    public readonly string Tooltip;
    public readonly bool IsHeader;
    public readonly bool CollapsedByDefault;

    public EditorSectionAttribute(
      string label = null,
      string tooltip = null,
      bool header = false,
      bool collapsedByDefault = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Label = label ?? string.Empty;
      this.Tooltip = tooltip ?? string.Empty;
      this.IsHeader = header;
      this.CollapsedByDefault = collapsedByDefault;
    }
  }
}
