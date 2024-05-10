// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.KbAttribute
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
  public sealed class KbAttribute : Attribute
  {
    public readonly KbCategory Category;
    public readonly LocStrFormatted Title;
    public readonly LocStr? Tooltip;
    public readonly bool IgnoreConflicts;
    public readonly bool IsPrimaryReadonly;
    private readonly string m_id;

    public string PrefsIdPrimary => "KBP_" + this.m_id;

    public string PrefsIdSecondary => "KBS_" + this.m_id;

    private string TranslationId => "Kb_" + this.m_id + "__label";

    private string m_tooltipId => "Kb_" + this.m_id + "__tooltip";

    public string GroupId => this.m_id;

    public KbAttribute(
      KbCategory category,
      string id,
      string label,
      string tooltip = null,
      bool ignoreConflicts = false,
      bool isPrimaryReadonly = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_id = id;
      this.Category = category;
      this.Title = (LocStrFormatted) LocalizationManager.GetLocalizedString0Arg(this.TranslationId, label, "label for a key-binding" + (tooltip == null ? "" : " that does: " + tooltip), ignoreDuplicates: true);
      this.Tooltip = string.IsNullOrEmpty(tooltip) ? new LocStr?() : new LocStr?(LocalizationManager.GetLocalizedString0Arg(this.m_tooltipId, tooltip, "tooltip for a key-binding: " + label, ignoreDuplicates: true));
      this.IgnoreConflicts = ignoreConflicts;
      this.IsPrimaryReadonly = isPrimaryReadonly;
    }

    public KbAttribute(
      KbCategory category,
      string id,
      string labelFormat,
      string formatArg,
      string tooltip,
      bool ignoreConflicts = false,
      bool isPrimaryReadonly = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_id = id;
      this.Category = category;
      this.Title = LocalizationManager.GetLocalizedString1Arg(this.TranslationId, labelFormat, "label for a key-binding" + (tooltip == null ? "" : " that does: " + tooltip), ignoreDuplicates: true).Format(formatArg);
      this.Tooltip = string.IsNullOrEmpty(tooltip) ? new LocStr?() : new LocStr?(LocalizationManager.GetLocalizedString0Arg(this.m_tooltipId, tooltip, "tooltip for a key-binding: " + labelFormat, ignoreDuplicates: true));
      this.IgnoreConflicts = ignoreConflicts;
      this.IsPrimaryReadonly = isPrimaryReadonly;
    }
  }
}
