// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TabButton
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class TabButton : ButtonRow
  {
    public readonly LocStrFormatted Title;
    public readonly string IconAsset;
    private readonly Label m_label;

    public TabButton(LocStrFormatted title, string iconAsset)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(outer: Outer.PanelTab);
      this.Title = title;
      this.IconAsset = iconAsset;
      this.Gap<TabButton>(new Px?(6.px())).Class<TabButton>(Cls.body);
      this.Add(iconAsset != null ? (UiComponent) new Icon(iconAsset).Small() : (UiComponent) null);
      this.Add((UiComponent) (this.m_label = new Label(title)));
    }

    public bool IsOverflowing => (double) this.m_label.LocalBound.height > 28.0;
  }
}
