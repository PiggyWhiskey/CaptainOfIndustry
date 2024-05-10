// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.IconButton
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class IconButton : Button, IUiComponent
  {
    private readonly Icon m_icon;

    public IconButton(string assetPath = null, Action onClick = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(onClick, Outer.ShadowAll);
      this.Variant<IconButton>(ButtonVariant.Boxy);
      this.Class<IconButton>(Cls.btn_boxy);
      this.FlexNoShrink<IconButton>();
      this.Add((UiComponent) (this.m_icon = new Icon(assetPath)));
    }

    public IconButton SetIcon(string iconAssetPath)
    {
      this.m_icon.Background<Icon>(iconAssetPath);
      return this;
    }

    void IUiComponent.SetColor(ColorRgba? color) => this.m_icon.Color<Icon>(color);

    public IconButton IconSize(Px width, Px height)
    {
      this.m_icon.Size<Icon>(width, height);
      return this;
    }

    public IconButton Small() => this.Class<IconButton>(Cls.small);

    public IconButton Medium() => this.Class<IconButton>(Cls.medium);

    public IconButton Large() => this.Class<IconButton>(Cls.large);
  }
}
