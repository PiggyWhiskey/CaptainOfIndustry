// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.EntitiesMenuUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  public class EntitiesMenuUiStyle : BaseUiStyle
  {
    public EntitiesMenuUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual ColorRgba BorderBg => (ColorRgba) 3158064;

    public virtual int TopBorderSize => 4;

    public virtual int BottomBorderSize => 6;

    public virtual int ScrollViewSideOffset => 20;

    public virtual ColorRgba MenuBg => (ColorRgba) 3618615;

    public virtual int ItemsSpacing => 10;

    public virtual int SideOverlayWidth => 42;

    public virtual IconStyle SideOverlayLeft
    {
      get
      {
        return new IconStyle(this.Icons.GradientToRight, new ColorRgba?(this.MenuBg), preserveAspect: false);
      }
    }

    public virtual IconStyle SideOverlayArrowLeft
    {
      get
      {
        return new IconStyle(this.Icons.ArrowToRight, new ColorRgba?((ColorRgba) 8946012), new Vector2?(new Vector2(14f, 18f)), false);
      }
    }

    public virtual IconStyle SideOverlayRight
    {
      get => this.SideOverlayLeft.Extend(this.Icons.GradientToLeft);
    }

    public virtual IconStyle SideOverlayArrowRight
    {
      get => this.SideOverlayArrowLeft.Extend(this.Icons.ArrowToLeft);
    }

    /// <summary>Item styles</summary>
    public virtual int ItemHeight => 100 + this.ItemTitleHeight - this.ItemImageBottomOffestHack;

    public virtual int ItemWidth => 100;

    public virtual int MenuHeight => this.ItemHeight + this.TopBorderSize;

    public virtual Vector2 ItemIconSize => new Vector2(80f, 80f);

    public virtual Vector2 ItemIconHoveredSize => new Vector2(100f, 100f);

    public virtual TextStyle ItemTitleStyle
    {
      get
      {
        return new TextStyle()
        {
          Color = (ColorRgba) 16777215,
          FontSize = 11,
          FontStyle = FontStyle.Normal,
          IsCapitalized = true
        };
      }
    }

    public virtual int ItemTitleHeight => 47;

    public virtual ColorRgba ItemTitleHoveredClr => (ColorRgba) 15051109;

    public virtual int ItemImageBottomOffestHack => 10;

    public virtual ColorRgba ItemIconClr => (ColorRgba) 12763842;

    public virtual ColorRgba ItemIconHoveredClr => (ColorRgba) 16777215;
  }
}
