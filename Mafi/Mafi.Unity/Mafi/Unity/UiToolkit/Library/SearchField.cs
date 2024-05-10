// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.SearchField
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class SearchField : TxtField
  {
    private readonly IconClickable m_clearButton;

    public SearchField()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      IconClickable component = new IconClickable("Assets/Unity/UserInterface/General/Clear128.png", new Action(this.handleClear)).Medium();
      Px? top = new Px?(0.px());
      Px? nullable = new Px?(0.px());
      Px? right = new Px?(0.px());
      Px? bottom = nullable;
      Px? left = new Px?();
      this.m_clearButton = component.AbsolutePosition<IconClickable>(top, right, bottom, left).Visible<IconClickable>(false);
      this.SelectAllOnFocus().Placeholder((LocStrFormatted) Tr.Search).OnValueChanged((Action<string>) (v => this.m_clearButton.Visible<IconClickable>(!string.IsNullOrEmpty(v))));
      this.RunWithBuilder((Action<UiBuilder>) (b => this.m_clearButton.Build(b)));
      this.Element.Add(this.m_clearButton.RootElement);
    }

    public override TxtField Text(string text)
    {
      this.m_clearButton.Visible<IconClickable>(!string.IsNullOrEmpty(text));
      return base.Text(text);
    }

    private void handleClear()
    {
      this.Text("");
      this.notifyChanged("");
    }
  }
}
