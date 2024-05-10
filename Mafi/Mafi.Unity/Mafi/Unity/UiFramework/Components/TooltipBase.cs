// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.TooltipBase
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public abstract class TooltipBase
  {
    protected const int PADDING = 10;
    protected readonly PanelWithShadow Container;
    private readonly Transform m_tooltipsContainer;
    protected readonly UiBuilder Builder;
    protected readonly Vector3[] m_corners;
    private float m_extraOffsetFromBottom;

    public TooltipBase(UiBuilder builder, string name = "Tooltip")
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_corners = new Vector3[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_tooltipsContainer = builder.GameOverlaySuperCanvas.GameObject.transform.Find("TooltipsContainer");
      if ((Object) this.m_tooltipsContainer == (Object) null)
        this.m_tooltipsContainer = (Transform) builder.NewPanel("TooltipsContainer").PutToLeftBottomOf<Panel>((IUiElement) builder.GameOverlaySuperCanvas, 0.Vector2()).RectTransform;
      this.Builder = builder;
      this.Container = builder.NewPanelWithShadow(name).AddShadowRightBottom().SetBackground(ColorRgba.Black);
      this.Container.RectTransform.SetParent(this.m_tooltipsContainer);
      this.Container.RectTransform.localScale = Vector3.one;
      this.Container.Hide<PanelWithShadow>();
    }

    protected void PositionSelf(IUiElement parent, float width, float height, bool alignToCenter = false)
    {
      this.m_tooltipsContainer.SetAsLastSibling();
      parent.RectTransform.GetWorldCorners(this.m_corners);
      Vector3 corner = this.m_corners[1];
      float num1 = height + 10f;
      float num2 = width + 10f;
      float leftOffset = 5f;
      float num3 = corner.y / this.Builder.MainCanvas.ScaleFactor + num1 + this.m_extraOffsetFromBottom;
      float num4 = corner.x / this.Builder.MainCanvas.ScaleFactor + num2 + leftOffset;
      if ((double) num4 >= (double) this.Builder.MainCanvas.GetWidth())
        leftOffset -= (float) ((double) num4 - (double) this.Builder.MainCanvas.GetWidth() + 5.0);
      if (!alignToCenter)
      {
        if ((double) num3 > (double) this.Builder.MainCanvas.GetHeight())
          this.Container.PutToLeftBottomOf<PanelWithShadow>(parent, new Vector2(width, height), Offset.BottomLeft(-num1, leftOffset));
        else
          this.Container.PutToLeftTopOf<PanelWithShadow>(parent, new Vector2(width, height), Offset.TopLeft(-num1 - this.m_extraOffsetFromBottom, leftOffset));
      }
      else if ((double) num3 > (double) this.Builder.MainCanvas.GetHeight())
        this.Container.PutToCenterBottomOf<PanelWithShadow>(parent, new Vector2(width, height), Offset.BottomLeft(-num1, 0.0f));
      else
        this.Container.PutToCenterTopOf<PanelWithShadow>(parent, new Vector2(width, height), Offset.TopLeft(-num1 - this.m_extraOffsetFromBottom, 0.0f));
      this.Container.RectTransform.SetParent(this.m_tooltipsContainer, true);
      this.Container.Show<PanelWithShadow>();
    }

    protected void onParentMouseLeave()
    {
      if (this.Container == null || this.Container.GameObject.IsNullOrDestroyed())
        return;
      this.Container.Hide<PanelWithShadow>();
    }

    public void SetExtraOffsetFromBottom(float offset) => this.m_extraOffsetFromBottom = offset;
  }
}
