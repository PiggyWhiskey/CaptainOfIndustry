// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TitleTooltipPromise
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.FloatingPanel;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  /// <summary>
  /// Tooltip that is aligned to the center of an element to show title of it.
  /// </summary>
  public class TitleTooltipPromise : FloatingPanelPromiseBase<TitleTooltip>
  {
    public LocStrFormatted Text { get; set; }

    public TitleTooltipPromise(UiComponent target, LocStrFormatted text = default (LocStrFormatted))
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(target);
      this.Text = text;
    }

    protected override bool HasDataToShow() => this.Text.IsNotEmpty;

    protected override void PopulateAndShow(TitleTooltip view) => view.SetDataAndShow(this);

    protected override void ClearTarget(TitleTooltip view) => view.ClearTargetAndData();
  }
}
