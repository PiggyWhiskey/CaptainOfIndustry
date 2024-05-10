// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UiObjectEditor.EditorButtonAttribute
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
  /// Note: For addition config you can also attach EditorInfoAttribute
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorButtonAttribute : Attribute
  {
    /// <summary>Override text of the button.</summary>
    public readonly Option<string> ButtonText;
    public readonly string ButtonTooltip;
    /// <summary>
    /// Whether the button should be styles as an important one.
    /// </summary>
    public readonly bool IsPrimary;
    public readonly ObjEditorIcon Icon;

    public EditorButtonAttribute(
      string buttonText = null,
      string buttonTooltip = null,
      bool isPrimary = false,
      ObjEditorIcon icon = ObjEditorIcon.None)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ButtonText = (Option<string>) buttonText;
      this.ButtonTooltip = buttonTooltip;
      this.IsPrimary = isPrimary;
      this.Icon = icon;
    }
  }
}
