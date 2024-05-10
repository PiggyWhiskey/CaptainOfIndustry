// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.GlobalUiStyle
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
  /// <summary>Global style that is heavily reused by other styles.</summary>
  public class GlobalUiStyle
  {
    public virtual int DefaultPanelFontSize => 12;

    public virtual ColorRgba DefaultPanelTextColor => (ColorRgba) 14935011;

    public virtual ColorRgba DefaultPanelDisabledTextColor => (ColorRgba) 12566463;

    public virtual ColorRgba DarkBorderColor => (ColorRgba) 1513239;

    public virtual ColorRgba ControlsBgColor => (ColorRgba) 2565927;

    public virtual ColorRgba GreenForDark => (ColorRgba) 4371254;

    public virtual ColorRgba BlueForDark => (ColorRgba) 7065085;

    public virtual ColorRgba DangerBtnClr => (ColorRgba) 12724525;

    public virtual ColorRgba DangerClr => (ColorRgba) 16476527;

    public virtual ColorRgba OrangeText => (ColorRgba) 16750848;

    internal virtual string OrangeTextStr => "#FF9900FF";

    public virtual ColorRgba UpointsTextColorForDark => (ColorRgba) 12554239;

    public virtual BorderStyle DefaultDarkBorder => new BorderStyle(this.DarkBorderColor);

    public virtual ColorRgba PanelsBg => (ColorRgba) 2105376;

    public virtual TextStyle ErrorTooltipText
    {
      get
      {
        TextStyle text = this.Text;
        ref TextStyle local = ref text;
        ColorRgba? color = new ColorRgba?((ColorRgba) 16777215);
        int? nullable = new int?(14);
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual ColorRgba ErrorTooltipBg => (ColorRgba) 9961472;

    public virtual ColorRgba FloatingPopupBg => new ColorRgba(2829099);

    public virtual BtnStyle GeneralBtn
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle((ColorRgba) 14925422, fontStyle: new FontStyle?(FontStyle.Bold), isCapitalized: true));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 9671571);
        Offset? nullable2 = new Offset?(Offset.All(6f));
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 3815994);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 8553090);
        ColorRgba? nullable5 = new ColorRgba?((ColorRgba) 6511954);
        ColorRgba? nullable6 = new ColorRgba?((ColorRgba) 4144959);
        ColorRgba? nullable7 = new ColorRgba?((ColorRgba) 8548424);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = nullable3;
        ColorRgba? hoveredMaskClr = nullable4;
        ColorRgba? pressedMaskClr = nullable5;
        ColorRgba? disabledMaskClr = nullable6;
        ColorRgba? foregroundClrWhenDisabled = nullable7;
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = nullable2;
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, true, height: 28, sidePaddings: 10, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle GeneralBtnActive
    {
      get
      {
        BtnStyle generalBtn = this.GeneralBtn;
        ref BtnStyle local = ref generalBtn;
        BorderStyle? nullable = new BorderStyle?(new BorderStyle(this.GeneralBtn.Text.Color));
        TextStyle? text = new TextStyle?();
        BorderStyle? border = nullable;
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
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

    public virtual BtnStyle GeneralBtnToToggle
    {
      get
      {
        BtnStyle generalBtn = this.GeneralBtn;
        ref BtnStyle local = ref generalBtn;
        TextStyle? text = new TextStyle?(this.GeneralBtn.Text.Extend(new ColorRgba?((ColorRgba) 14277081)));
        ColorRgba? nullable = new ColorRgba?((ColorRgba) 6710886);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
        ColorRgba? pressedClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = nullable;
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = new Offset?();
        return local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      }
    }

    public virtual BtnStyle UpointsBtn
    {
      get
      {
        BtnStyle generalBtn = this.GeneralBtn;
        ref BtnStyle local = ref generalBtn;
        TextStyle? text = new TextStyle?(this.GeneralBtn.Text.Extend(new ColorRgba?(this.UpointsTextColorForDark)));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 9196444);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 7816071);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 7825802);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = nullable1;
        ColorRgba? pressedClr = nullable2;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = nullable3;
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = new Offset?();
        return local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      }
    }

    public virtual BtnStyle UpointsBtnActive
    {
      get
      {
        BtnStyle upointsBtn = this.UpointsBtn;
        ref BtnStyle local = ref upointsBtn;
        BorderStyle? nullable = new BorderStyle?(new BorderStyle(this.UpointsTextColorForDark));
        TextStyle? text = new TextStyle?();
        BorderStyle? border = nullable;
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
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

    public virtual BtnStyle DangerBtn
    {
      get
      {
        BtnStyle generalBtn = this.GeneralBtn;
        ref BtnStyle local = ref generalBtn;
        TextStyle? text = new TextStyle?(this.GeneralBtn.Text.Extend(new ColorRgba?(this.DangerClr)));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 9325642);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 6833470);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 9654354);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = nullable1;
        ColorRgba? pressedClr = nullable2;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = nullable3;
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = new Offset?();
        return local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      }
    }

    public virtual BtnStyle DangerBtnActive
    {
      get
      {
        BtnStyle dangerBtn = this.DangerBtn;
        ref BtnStyle local = ref dangerBtn;
        BorderStyle? nullable = new BorderStyle?(new BorderStyle(this.DangerBtn.Text.Color));
        TextStyle? text = new TextStyle?();
        BorderStyle? border = nullable;
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
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

    public virtual BtnStyle ImageBtn
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle(ColorRgba.White));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 13816530);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 10987431);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredMaskClr = nullable1;
        ColorRgba? pressedMaskClr = nullable2;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle PrimaryBtn
    {
      get
      {
        FontStyle? fontStyle = new FontStyle?(FontStyle.Bold);
        TextStyle? text = new TextStyle?(new TextStyle(ColorRgba.Black, fontStyle: fontStyle, isCapitalized: true));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 16762368);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 13750737);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 16777215);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 12040119);
        ColorRgba? nullable5 = new ColorRgba?((ColorRgba) 11246694);
        Offset? nullable6 = new Offset?(Offset.All(6f));
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = nullable2;
        ColorRgba? hoveredMaskClr = nullable3;
        ColorRgba? pressedMaskClr = nullable4;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = nullable5;
        Offset? iconPadding = nullable6;
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, true, height: 28, sidePaddings: 10, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle MinusPrimaryBtn
    {
      get
      {
        BtnStyle primaryBtn = this.PrimaryBtn;
        ref BtnStyle local = ref primaryBtn;
        TextStyle? text = new TextStyle?(this.PrimaryBtn.Text.Extend(new ColorRgba?(ColorRgba.White)));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 11225931);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 13619151);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 16777215);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 13158600);
        ColorRgba? nullable5 = new ColorRgba?((ColorRgba) 7490379);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = nullable2;
        ColorRgba? hoveredClr = nullable3;
        ColorRgba? pressedClr = nullable4;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = nullable5;
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = new Offset?();
        return local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      }
    }

    public virtual BtnStyle ListMenuBtn
    {
      get
      {
        TextStyle? text = new TextStyle?(this.TextControls);
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 3026478);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 14803425);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 10987431);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, sidePaddings: 10, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle ListMenuBtnSelected
    {
      get
      {
        BtnStyle listMenuBtn = this.ListMenuBtn;
        ref BtnStyle local1 = ref listMenuBtn;
        ref TextStyle local2 = ref this.ListMenuBtn.Text;
        FontStyle? nullable1 = new FontStyle?(FontStyle.Bold);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = nullable1;
        int? fontSize = new int?();
        bool? isCapitalized = new bool?();
        TextStyle? text = new TextStyle?(local2.Extend(color, fontStyle, fontSize, isCapitalized));
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 0);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
        ColorRgba? pressedClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = nullable2;
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = new Offset?();
        return local1.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      }
    }

    internal virtual BtnStyle ListMenuBtnDarker
    {
      get
      {
        BtnStyle listMenuBtn = this.ListMenuBtn;
        ref BtnStyle local = ref listMenuBtn;
        ColorRgba? nullable = new ColorRgba?((ColorRgba) 2565927);
        TextStyle? text = new TextStyle?();
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable;
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
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

    public virtual BtnStyle ListMenuGroupBtn
    {
      get
      {
        TextStyle? text = new TextStyle?(this.TextControls);
        ColorRgba? nullable1 = new ColorRgba?(this.ControlsBgColor);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 14803425);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 10987431);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, sidePaddings: 10, iconPadding: iconPadding);
      }
    }

    internal virtual BtnStyle IconBtnWhite
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle(ColorRgba.White));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 12632256);
        ColorRgba? nullable2 = new ColorRgba?(ColorRgba.White);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    internal virtual BtnStyle IconBtnOrange
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle(this.OrangeText));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 12632256);
        ColorRgba? nullable2 = new ColorRgba?(ColorRgba.White);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    internal virtual BtnStyle IconBtnOrange2
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle(ColorRgba.White));
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 14790665);
        ColorRgba? nullable2 = new ColorRgba?(ColorRgba.White);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    internal virtual BtnStyle IconBtnUpointsDanger
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle(ColorRgba.White));
        ColorRgba? nullable1 = new ColorRgba?(this.UpointsTextColorForDark);
        ColorRgba? nullable2 = new ColorRgba?(this.DangerClr);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 14237253);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    public virtual TextStyle Text => new TextStyle(this.DefaultPanelTextColor);

    public virtual TextStyle TextBold
    {
      get => new TextStyle(this.DefaultPanelTextColor, fontStyle: new FontStyle?(FontStyle.Bold));
    }

    public virtual TextStyle TextInc => new TextStyle(this.DefaultPanelTextColor, 13);

    public virtual TextStyle TextIncBold
    {
      get => new TextStyle(this.DefaultPanelTextColor, 13, new FontStyle?(FontStyle.Bold));
    }

    public virtual TextStyle TextMediumBold
    {
      get => new TextStyle(this.DefaultPanelTextColor, 14, new FontStyle?(FontStyle.Bold));
    }

    public virtual TextStyle TextMedium => new TextStyle(this.DefaultPanelTextColor, 14);

    public virtual TextStyle TextBig => new TextStyle(this.DefaultPanelTextColor, 16);

    public virtual TextStyle TextBigBold
    {
      get => new TextStyle(this.DefaultPanelTextColor, 16, new FontStyle?(FontStyle.Bold));
    }

    public virtual TextStyle TextControls => new TextStyle(this.DefaultPanelTextColor, 14);

    public virtual TextStyle TextControlsBold
    {
      get => new TextStyle(this.DefaultPanelTextColor, 14, new FontStyle?(FontStyle.Bold));
    }

    public virtual TextStyle BoldText
    {
      get => new TextStyle(this.DefaultPanelTextColor, fontStyle: new FontStyle?(FontStyle.Bold));
    }

    public virtual TextStyle Title
    {
      get
      {
        TextStyle text = this.Text;
        ref TextStyle local = ref text;
        FontStyle? nullable1 = new FontStyle?(FontStyle.Bold);
        bool? nullable2 = new bool?(true);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = nullable1;
        int? fontSize = new int?();
        bool? isCapitalized = nullable2;
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual TextStyle TitleBig
    {
      get
      {
        TextStyle text = this.Text;
        ref TextStyle local = ref text;
        int? nullable1 = new int?(14);
        FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
        bool? nullable3 = new bool?(true);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = nullable2;
        int? fontSize = nullable1;
        bool? isCapitalized = nullable3;
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual TextStyle HeaderTextStyle
    {
      get
      {
        TextStyle text = this.Text;
        ref TextStyle local = ref text;
        FontStyle? nullable1 = new FontStyle?(FontStyle.Bold);
        int? nullable2 = new int?(13);
        bool? nullable3 = new bool?(true);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = nullable1;
        int? fontSize = nullable2;
        bool? isCapitalized = nullable3;
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    /// <summary>Light text field for dark background.</summary>
    public virtual TxtFieldStyle LightTxtFieldStyle
    {
      get
      {
        return new TxtFieldStyle(new TextStyle(this.DefaultPanelTextColor, 14, new FontStyle?(FontStyle.Bold)), new TextStyle(this.DefaultPanelTextColor, 14, new FontStyle?(FontStyle.Italic)));
      }
    }

    public virtual BtnStyle ToggleBtnOn
    {
      get
      {
        TextStyle? text = new TextStyle?(this.Title);
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 2697513);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 5203794);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 4283973);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 5203794);
        ColorRgba? nullable5 = new ColorRgba?(ColorRgba.White);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable5;
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = nullable4;
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle ToggleBtnOff
    {
      get
      {
        TextStyle? text = new TextStyle?(this.Title);
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 2697513);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 11748421);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 11091776);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 11748421);
        ColorRgba? nullable5 = new ColorRgba?(ColorRgba.White);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable5;
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = nullable4;
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle ToggleBtnAuto
    {
      get
      {
        TextStyle? text = new TextStyle?(this.Title);
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 2697513);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 8745804);
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 10126937);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 8745804);
        ColorRgba? nullable5 = new ColorRgba?(ColorRgba.White);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable5;
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = nullable4;
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      }
    }

    public GlobalUiStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
