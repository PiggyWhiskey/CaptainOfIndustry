// Decompiled with JetBrains decompiler
// Type: Mafi.EditorLabelAttribute
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
  /// Allows to configure the label of any existing member of a class.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorLabelAttribute : Attribute
  {
    public readonly string Label;
    public readonly string Tooltip;
    public readonly bool IsHeader;
    /// <summary>
    /// If true, label will be colored as an error. Works only for readonly displays.
    /// </summary>
    public readonly bool IsError;

    public EditorLabelAttribute(string label = null, string tooltip = null, bool header = false, bool isError = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Label = label;
      this.Tooltip = tooltip ?? string.Empty;
      this.IsHeader = header;
      this.IsError = isError;
    }
  }
}
