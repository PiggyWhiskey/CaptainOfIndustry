// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.FillImg
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  /// <summary>
  /// A image that scales to fit its parent container, maintaining aspect ratio and supporting a shadow decoration.
  /// </summary>
  public class FillImg : UiComponent, IComponentWithImage
  {
    private UiComponent m_background;
    private Img m_image;
    private (int Width, int Height) m_intrinsicSize;
    private (float Width, float Height) m_containerSize;

    public FillImg()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new VisualElement());
      this.Fill<FillImg>();
      Img img;
      this.Add(this.m_background = new UiComponent()
      {
        (UiComponent) Outer.ShadowAll
      }.AbsolutePositionCenterMiddle<UiComponent>(), (UiComponent) (img = new Img().AbsolutePositionCenterMiddle<Img>().BorderRadius<Img>(5.px()).OverflowHidden<Img>()));
      this.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.handleGeometryChanged));
      this.m_image = img;
    }

    private void handleGeometryChanged(GeometryChangedEvent evt)
    {
      Rect newRect = evt.newRect;
      double width = (double) newRect.width;
      newRect = evt.newRect;
      double height = (double) newRect.height;
      this.m_containerSize = ((float) width, (float) height);
      this.updateBounds();
    }

    private void updateBounds()
    {
      float width1 = (float) this.m_intrinsicSize.Width;
      float height1 = (float) this.m_intrinsicSize.Height;
      if ((double) width1 == 0.0 || (double) height1 == 0.0)
      {
        this.VisibleForRender<FillImg>(false);
      }
      else
      {
        this.VisibleForRender<FillImg>(true);
        float num = Mathf.Min(this.m_containerSize.Width / width1, this.m_containerSize.Height / height1);
        Px width2 = (width1 * num).px();
        Px height2 = (height1 * num).px();
        this.m_image.Size<Img>(width2, height2);
        this.m_background.Size<UiComponent>(width2, height2);
      }
    }

    internal override VisualElement GetChildrenContainer()
    {
      return this.m_image?.GetChildrenContainer() ?? base.GetChildrenContainer();
    }

    protected override void SetColorInternal(ColorRgba? color) => this.m_image.Color<Img>(color);

    void IComponentWithImage.SetImage(Option<Texture2D> texture)
    {
      this.m_image.Image<Img>(texture);
      this.m_intrinsicSize = texture.HasValue ? (texture.Value.width, texture.Value.height) : (0, 0);
      this.updateBounds();
    }

    void IComponentWithImage.SetImageScaleMode(ScaleMode mode)
    {
      Log.Error("FillImg.SetImageScaleMode() is not supported! Use Img if you want to adjust the scale mode.");
    }
  }
}
