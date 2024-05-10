// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.StatusBarUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  public class StatusBarUiStyle : BaseUiStyle
  {
    private static readonly ColorRgba s_redText;

    public StatusBarUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual TextStyle PausedTextStyle
    {
      get
      {
        return new TextStyle()
        {
          FontSize = 16,
          IsCapitalized = true,
          FontStyle = FontStyle.Bold,
          Color = (ColorRgba) 12303291
        };
      }
    }

    /// <summary>
    /// Inner offset of the pause button (how far is the icon from borders).
    /// </summary>
    public virtual Offset PauseButtonOffset => Offset.LeftRight(6f);

    /// <summary>Dimensions of play and pause icons.</summary>
    public virtual Vector2 PauseIconSize => new Vector2(20f, 20f);

    /// <summary>Spacing between play and pause icon.</summary>
    public virtual float PlayPauseSpacing => 6f;

    public virtual BtnStyle PlayButtonEnabled
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle((ColorRgba) 15048462));
        Offset? nullable = new Offset?(Offset.Zero);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredMaskClr = new ColorRgba?();
        ColorRgba? pressedMaskClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = nullable;
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle PlayButtonDisabled
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle((ColorRgba) 16777215));
        Offset? nullable1 = new Offset?(Offset.Zero);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 13619151);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 12025094);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 15048462);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = nullable2;
        ColorRgba? hoveredMaskClr = nullable4;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = nullable1;
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    public virtual int IconWidth => 22;

    public virtual ColorRgba IconColor => (ColorRgba) 15658734;

    public virtual ColorRgba DisplayBgColor => (ColorRgba) 0;

    public virtual SlicedSpriteStyle QuantityStateBg => this.Icons.WhiteBgGrayBorder;

    public virtual TextStyle QuantityStateText
    {
      get
      {
        return new TextStyle()
        {
          FontStyle = FontStyle.Bold,
          IsCapitalized = true,
          FontSize = 16
        };
      }
    }

    public virtual TextStyle HelperText
    {
      get
      {
        return new TextStyle()
        {
          FontStyle = FontStyle.Bold,
          IsCapitalized = true,
          FontSize = 12,
          Color = (ColorRgba) 11579568
        };
      }
    }

    public virtual TextStyle QuantityChangeText
    {
      get
      {
        return new TextStyle()
        {
          FontStyle = FontStyle.Bold,
          IsCapitalized = true,
          FontSize = 14
        };
      }
    }

    public virtual ColorRgba QuantityStatePositiveColor => (ColorRgba) 16777215;

    public virtual ColorRgba QuantityChangePositiveColor => this.Global.GreenForDark;

    public virtual ColorRgba QuantityNegativeColor => this.Global.DangerClr;

    static StatusBarUiStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      StatusBarUiStyle.s_redText = (ColorRgba) 16528955;
    }
  }
}
