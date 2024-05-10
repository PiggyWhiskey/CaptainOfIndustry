// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.TxtColumn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  /// <summary>Column that displays string.</summary>
  public class TxtColumn : Column<string, Txt>
  {
    private readonly TextAnchor m_textAnchor;
    private readonly TextStyle? m_textStyle;

    public TxtColumn(
      Mafi.Unity.UiFramework.Components.Table.Table view,
      int index,
      string title,
      int width,
      bool mergeWithPrevious,
      TextAnchor alignment,
      TextStyle? textStyle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(view, index, title, width, mergeWithPrevious);
      this.m_textAnchor = alignment;
      this.m_textStyle = textStyle;
    }

    protected override Txt CreateView(UiBuilder builder, bool highlighted)
    {
      TextStyle textStyle = this.m_textStyle ?? builder.Style.Panel.TextMedium;
      return builder.NewTxt("Column " + this.Index.ToString()).SetAlignment(this.m_textAnchor).SetTextStyle(highlighted ? builder.Style.Statistics.HighligtedText : textStyle).SetText(string.Empty);
    }

    protected override GameObject ResolveGameObject(Txt view) => view.GameObject;

    protected override string DefaultValue() => string.Empty;

    protected override string UpdateCell(int rowIndex, Txt view, string oldValue, string newValue)
    {
      if (oldValue != newValue)
        view.SetText(newValue);
      return newValue;
    }
  }
}
