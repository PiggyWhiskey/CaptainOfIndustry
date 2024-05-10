// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.SimpleTooltipPromise
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
  /// <summary>Simple tooltip that contains text.</summary>
  public class SimpleTooltipPromise : FloatingPanelPromiseBase<SimpleTooltip>
  {
    private LocStrFormatted m_text;

    public LocStrFormatted Text
    {
      get => this.m_text;
      set
      {
        this.m_text = value;
        this.OnTargetValueChanged();
      }
    }

    public SimpleTooltipPromise(UiComponent target, LocStrFormatted text = default (LocStrFormatted))
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(target);
      this.Text = text;
    }

    protected override bool HasDataToShow() => this.Text.IsNotEmpty;

    protected override void PopulateAndShow(SimpleTooltip view) => view.SetDataAndShow(this);

    protected override void ClearTarget(SimpleTooltip view) => view.ClearTargetAndData();
  }
}
