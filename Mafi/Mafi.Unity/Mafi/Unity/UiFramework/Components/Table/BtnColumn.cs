// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.BtnColumn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  /// <summary>
  /// Column that contains button. Only button, nothing else.
  /// </summary>
  public class BtnColumn : Column<BtnColumn.Data, Btn>
  {
    public BtnColumn(Mafi.Unity.UiFramework.Components.Table.Table view, int index, string title, int width, bool mergeWithPrevious)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(view, index, title, width, mergeWithPrevious);
    }

    public override Offset InnerOffset() => Offset.TopBottom(4f);

    protected override Btn CreateView(UiBuilder builder, bool highlighted)
    {
      return builder.NewBtn("Column " + this.Index.ToString()).SetButtonStyle(builder.Style.Global.PrimaryBtn).SetText("");
    }

    protected override GameObject ResolveGameObject(Btn view) => view.GameObject;

    protected override BtnColumn.Data DefaultValue() => new BtnColumn.Data();

    protected override BtnColumn.Data UpdateCell(
      int rowIndex,
      Btn view,
      BtnColumn.Data oldValue,
      BtnColumn.Data newValue)
    {
      view.SetText(newValue.Text);
      view.OnClick(newValue.Action);
      return newValue;
    }

    public struct Data
    {
      public string Text;
      public Action Action;

      public Data(string text, Action action)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Text = text.CheckNotNull<string>();
        this.Action = action.CheckNotNull<Action>();
      }
    }
  }
}
