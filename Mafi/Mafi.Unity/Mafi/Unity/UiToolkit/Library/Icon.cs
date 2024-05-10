// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Icon
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
  /// Meant to be colorizable which for some reason does not work well with image.
  /// In css use "-unity-background-image-tint-color"
  /// </summary>
  public class Icon : UiComponent<VisualElement>
  {
    public Icon(string texturePath = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(new VisualElement());
      this.Class<Icon>(Cls.img, Cls.icon);
      if (texturePath == null)
        return;
      this.Background<Icon>(texturePath);
    }

    protected override void SetColorInternal(ColorRgba? color)
    {
      IStyle style = this.Element.style;
      Color? nullable = color.HasValue ? new Color?(color.GetValueOrDefault().ToColor()) : new Color?();
      StyleColor styleColor = nullable.HasValue ? (StyleColor) nullable.GetValueOrDefault() : new StyleColor(StyleKeyword.None);
      style.unityBackgroundImageTintColor = styleColor;
    }

    public Icon Large() => this.Class<Icon>(Cls.large);

    public Icon Medium() => this.Class<Icon>(Cls.medium);

    public Icon Small() => this.Class<Icon>(Cls.small);
  }
}
