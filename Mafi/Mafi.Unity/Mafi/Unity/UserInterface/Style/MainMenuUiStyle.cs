// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.MainMenuUiStyle
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
  /// <summary>Styles for main menu only.</summary>
  public class MainMenuUiStyle : BaseUiStyle
  {
    public MainMenuUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual int MenuBtnsWidth => 240;

    public virtual ColorRgba MenuBgColor => (ColorRgba) 0;

    public virtual BtnStyle PrimaryMainMenuButton
    {
      get
      {
        return new BtnStyle()
        {
          BackgroundClr = new ColorRgba?(this.Global.ControlsBgColor),
          Border = this.Global.DefaultDarkBorder,
          Text = new TextStyle()
          {
            FontStyle = FontStyle.Bold,
            Color = (ColorRgba) 12297326,
            IsCapitalized = true,
            FontSize = 16
          },
          HoveredMaskClr = new ColorRgba?((ColorRgba) 14803425),
          PressedMaskClr = new ColorRgba?((ColorRgba) 10987431),
          DisabledMaskClr = new ColorRgba?((ColorRgba) 13158600)
        };
      }
    }

    public virtual BtnStyle SideMainMenuButton
    {
      get
      {
        return new BtnStyle()
        {
          BackgroundClr = new ColorRgba?(this.Global.ControlsBgColor),
          Border = this.Global.DefaultDarkBorder,
          Text = new TextStyle()
          {
            FontStyle = FontStyle.Bold,
            Color = (ColorRgba) 12297326,
            IsCapitalized = true
          },
          HoveredMaskClr = new ColorRgba?((ColorRgba) 14803425),
          PressedMaskClr = new ColorRgba?((ColorRgba) 10987431)
        };
      }
    }
  }
}
