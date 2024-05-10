// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.ToolbarUiStyle
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
  /// <summary>Styles for toolbar only.</summary>
  public class ToolbarUiStyle : BaseUiStyle
  {
    private static readonly ColorRgba s_redText;

    public ToolbarUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual float MainMenuIconHeight => 38f;

    public virtual float MainMenuStripHeight => this.IconsOnlyMenuStripHeight;

    public virtual float IconsOnlyMenuStripHeight => 44f;

    /// <summary>Style of labels in 3d menu.</summary>
    public virtual TextStyle CaptionsStyle
    {
      get
      {
        return new TextStyle()
        {
          Color = ColorRgba.White,
          FontStyle = FontStyle.Bold,
          FontSize = 12,
          IsCapitalized = true
        };
      }
    }

    public virtual BtnStyle ButtonOff
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle((ColorRgba) 16777215));
        Offset? nullable1 = new Offset?(Offset.All(3f));
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

    /// <summary>Highlighted button (when its panel is active).</summary>
    public virtual BtnStyle ButtonOn
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle((ColorRgba) 15048462));
        Offset? nullable = new Offset?(Offset.All(3f));
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

    /// <summary>Red</summary>
    public virtual BtnStyle ButtonNegative
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle((ColorRgba) 11551298));
        Offset? nullable = new Offset?(Offset.All(3f));
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

    public virtual BtnStyle ButtonDeleteOn => this.ButtonNegative;

    public virtual BtnStyle ButtonDeleteOff
    {
      get
      {
        BtnStyle buttonOff = this.ButtonOff;
        ref BtnStyle local = ref buttonOff;
        ColorRgba? nullable = new ColorRgba?((ColorRgba) 11551298);
        TextStyle? text = new TextStyle?();
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = nullable;
        ColorRgba? pressedClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = new Offset?();
        return local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      }
    }

    public virtual ColorRgba GameMenuOverlayBgColor => new ColorRgba(0, 100);

    public virtual float GameMenuPanelWidth => 260f;

    public virtual float GameMenuButtonsHeight => 40f;

    /// <summary>
    /// Left and right offsets of the buttons from the panel borders.
    /// </summary>
    public virtual Offset GameMenuButtonsOffset => Offset.LeftRight(20f);

    public virtual float ToolboxBtnSize => 50f;

    /// <summary>Toolbox button style.</summary>
    public virtual BtnStyle ToolboxButton
    {
      get
      {
        TextStyle title = this.Global.Title;
        ref TextStyle local = ref title;
        int? nullable1 = new int?(21);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable1;
        bool? isCapitalized = new bool?();
        TextStyle? text = new TextStyle?(local.Extend(color, fontStyle, fontSize, isCapitalized));
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 4408131);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 15395562);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 16777215);
        Offset? nullable5 = new Offset?(Offset.All(5f));
        ColorRgba? nullable6 = new ColorRgba?((ColorRgba) 8553090);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable2;
        ColorRgba? normalMaskClr = nullable3;
        ColorRgba? hoveredMaskClr = new ColorRgba?();
        ColorRgba? pressedMaskClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = nullable4;
        ColorRgba? foregroundClrWhenDisabled = nullable6;
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = nullable5;
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    /// <summary>Toolbox button style that is selected = toggled on.</summary>
    public virtual BtnStyle ToolboxButtonActive
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle((ColorRgba) 16619815));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 4408131);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 15395562);
        Offset? nullable3 = new Offset?(Offset.All(5f));
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = nullable2;
        ColorRgba? hoveredMaskClr = new ColorRgba?();
        ColorRgba? pressedMaskClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = nullable3;
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    /// <summary>Style of text displaying a shorcut key.</summary>
    public virtual TextStyle ToolboxShortcutText
    {
      get
      {
        return new TextStyle()
        {
          Color = (ColorRgba) 16619815,
          FontSize = 18,
          FontStyle = FontStyle.Bold
        };
      }
    }

    public virtual TextStyle ToolboxShortcutTextLong
    {
      get
      {
        TextStyle toolboxShortcutText = this.ToolboxShortcutText;
        ref TextStyle local = ref toolboxShortcutText;
        int? nullable = new int?(12);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    /// <summary>Background under the shortcut text.</summary>
    public virtual ColorRgba ToolboxShortcutBackground => new ColorRgba(3355443);

    public virtual SlicedSpriteStyle MoneyPopupBg => this.Icons.WhiteBgBlueBorder;

    public virtual SlicedSpriteStyle MoneyPopupBgPositive => this.Icons.WhiteBgGreenBorder;

    public virtual SlicedSpriteStyle MoneyPopupBgCannotAfford => this.Icons.WhiteBgRedBorder;

    public virtual TextStyle MoneyPopupText
    {
      get
      {
        return new TextStyle()
        {
          FontSize = 11,
          FontStyle = FontStyle.Bold
        };
      }
    }

    public virtual ColorRgba MoneyPopupTextColor => (ColorRgba) 30640;

    public virtual ColorRgba MoneyPopupTextColorPositive => (ColorRgba) 32768;

    public virtual ColorRgba MoneyPopupTextColorCannotAfford => ToolbarUiStyle.s_redText;

    public virtual int MoneyPopupHeight => 18;

    static ToolbarUiStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ToolbarUiStyle.s_redText = (ColorRgba) 16528955;
    }
  }
}
