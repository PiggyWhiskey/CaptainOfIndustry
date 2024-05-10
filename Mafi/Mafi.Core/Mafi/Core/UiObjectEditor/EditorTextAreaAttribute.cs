// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UiObjectEditor.EditorTextAreaAttribute
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.UiObjectEditor
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorTextAreaAttribute : Attribute
  {
    public readonly int LinesCount;
    public readonly bool AutoScale;

    /// <summary>Turns text field into text area.</summary>
    /// <param name="linesCount">If autoScale is true, linesHeight is treated as minimum height.</param>
    /// <param name="autoScale">Area will automatically expand if true</param>
    public EditorTextAreaAttribute(int linesCount = 4, bool autoScale = true)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LinesCount = linesCount;
      this.AutoScale = autoScale;
    }
  }
}
