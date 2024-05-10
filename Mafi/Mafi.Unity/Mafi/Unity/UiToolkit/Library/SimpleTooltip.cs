// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.SimpleTooltip
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
  public class SimpleTooltip : FloatingColumn
  {
    private readonly Label m_label;
    private Option<SimpleTooltipPromise> m_data;

    public SimpleTooltip()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IFloatingPositionPolicy) new TooltipPositionPolicy(), Outer.ShadowAll);
      this.Background<SimpleTooltip>(new ColorRgba?(Theme.TooltipBackground)).Padding<SimpleTooltip>(2.pt());
      this.Add((UiComponent) (this.m_label = new Label()));
    }

    public void SetDataAndShow(SimpleTooltipPromise data)
    {
      this.m_data = (Option<SimpleTooltipPromise>) data;
      this.m_label.Text<Label>(data.Text);
      this.SetTarget(data.Target);
    }

    public void ClearTargetAndData()
    {
      this.m_data = (Option<SimpleTooltipPromise>) Option.None;
      this.ClearTarget();
    }

    protected override void OnTargetLost() => this.m_data.ValueOrNull?.OnTargetLost();
  }
}
