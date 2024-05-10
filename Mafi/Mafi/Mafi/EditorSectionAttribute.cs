// Decompiled with JetBrains decompiler
// Type: Mafi.EditorSectionAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
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
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Label = label ?? string.Empty;
      this.Tooltip = tooltip ?? string.Empty;
      this.IsHeader = header;
      this.CollapsedByDefault = collapsedByDefault;
    }
  }
}
