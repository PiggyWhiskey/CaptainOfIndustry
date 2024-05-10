// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.StretchedImg
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
  /// An image that scales to the parent container's cross axis size (width for column, height for row).
  /// NOTE: Call StretchHeight for row containers or StretchWidth column containers.
  /// </summary>
  public class StretchedImg : UiComponentDecorated<Image>, IComponentWithImage
  {
    private Vector2i m_intrinsicSize;
    private bool m_stretchHeight;

    public StretchedImg(Texture2D texture = null, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Image(), outer, inner);
      this.Class<StretchedImg>(Cls.img);
      this.Image<StretchedImg>(texture.CreateOption<Texture2D>());
    }

    public StretchedImg(string imagePath, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Image(), outer, inner);
      this.Class<StretchedImg>(Cls.img);
      this.Image<StretchedImg>(imagePath);
    }

    void IComponentWithImage.SetImage(Option<Texture2D> texture)
    {
      this.InnerElement.style.backgroundImage = (StyleBackground) texture.ValueOrNull;
      this.m_intrinsicSize = texture.HasValue ? new Vector2i(texture.Value.width, texture.Value.height) : Vector2i.Zero;
      this.updateBounds();
    }

    void IComponentWithImage.SetImageScaleMode(ScaleMode mode)
    {
      this.InnerElement.scaleMode = mode;
      switch (mode)
      {
        case ScaleMode.ScaleToFit:
          this.InnerElement.style.backgroundSize = (StyleBackgroundSize) new BackgroundSize(BackgroundSizeType.Contain);
          break;
        default:
          this.InnerElement.style.backgroundSize = (StyleBackgroundSize) new BackgroundSize(BackgroundSizeType.Cover);
          break;
      }
      this.updateBounds();
    }

    public StretchedImg ClearImage()
    {
      this.InnerElement.style.backgroundImage = (StyleBackground) (Texture2D) null;
      this.m_intrinsicSize = new Vector2i(0, 0);
      this.updateBounds();
      return this;
    }

    protected override void SetColorInternal(ColorRgba? color)
    {
      this.InnerElement.tintColor = color.HasValue ? color.GetValueOrDefault().ToColor() : Color.white;
    }

    private void updateBounds()
    {
      if (this.m_intrinsicSize.X == 0 || this.m_intrinsicSize.Y == 0)
      {
        this.InnerElement.style.paddingTop = (StyleLength) 0.0f;
        this.InnerElement.style.paddingLeft = (StyleLength) 0.0f;
      }
      else
      {
        if (this.m_stretchHeight)
        {
          this.InnerElement.style.paddingTop = (StyleLength) 0.0f;
          this.InnerElement.style.paddingLeft = (StyleLength) new Length((float) ((double) this.m_intrinsicSize.X / (double) this.m_intrinsicSize.Y * 100.0), LengthUnit.Percent);
        }
        else
        {
          this.InnerElement.style.paddingTop = (StyleLength) new Length((float) ((double) this.m_intrinsicSize.Y / (double) this.m_intrinsicSize.X * 100.0), LengthUnit.Percent);
          this.InnerElement.style.paddingLeft = (StyleLength) 0.0f;
        }
        this.InnerElement.style.alignSelf = (StyleEnum<UnityEngine.UIElements.Align>) UnityEngine.UIElements.Align.Stretch;
      }
    }

    public StretchedImg StretchWidth()
    {
      this.m_stretchHeight = false;
      this.updateBounds();
      return this;
    }

    public StretchedImg StretchHeight()
    {
      this.m_stretchHeight = true;
      this.updateBounds();
      return this;
    }
  }
}
