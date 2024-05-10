// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Img
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
  public class Img : UiComponentDecorated<Image>, IComponentWithImage
  {
    public Img(Outer outer = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Image(), outer);
      this.Class<Img>(Cls.img);
    }

    public Img(Texture2D texture, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Image(), outer, inner);
      this.Class<Img>(Cls.img);
      this.Image<Img>((Option<Texture2D>) texture);
    }

    public Img(string texturePath, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Image(), outer, inner);
      this.Class<Img>(Cls.img);
      this.Image<Img>(texturePath);
    }

    protected override void SetColorInternal(ColorRgba? color)
    {
      this.InnerElement.tintColor = color.HasValue ? color.GetValueOrDefault().ToColor() : Color.white;
    }

    void IComponentWithImage.SetImage(Option<Texture2D> texture)
    {
      this.InnerElement.image = (Texture) texture.ValueOrNull;
    }

    void IComponentWithImage.SetImageScaleMode(ScaleMode mode)
    {
      this.InnerElement.scaleMode = mode;
    }
  }
}
