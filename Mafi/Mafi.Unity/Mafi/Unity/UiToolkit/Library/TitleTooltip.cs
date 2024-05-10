// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TitleTooltip
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.FloatingPanel;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class TitleTooltip : FloatingColumn
  {
    private readonly Label m_label;
    private Option<TitleTooltipPromise> m_data;

    public TitleTooltip()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IFloatingPositionPolicy) new TitleTooltipPositionPolicy());
      this.AlignItemsCenter<TitleTooltip>();
      this.Add((UiComponent) (this.m_label = new Label().Background<Label>(new ColorRgba?(Theme.TooltipBackground)).FontSize<Label>(12).FontBold<Label>().AlignTextCenter<Label>().Padding<Label>(1.pt())), (UiComponent) new Img("Assets/Unity/UserInterface/General/ArrowDownFull.svg").Color<Img>(new ColorRgba?(Theme.TooltipBackground)).Size<Img>(20.px(), 10.px()));
    }

    public void SetDataAndShow(TitleTooltipPromise data)
    {
      this.m_data = (Option<TitleTooltipPromise>) data;
      this.m_label.Text<Label>(data.Text);
      this.SetTarget(data.Target);
    }

    public void ClearTargetAndData()
    {
      this.m_data = (Option<TitleTooltipPromise>) Option.None;
      this.ClearTarget();
    }

    protected override void OnTargetLost() => this.m_data.ValueOrNull?.OnTargetLost();
  }
}
