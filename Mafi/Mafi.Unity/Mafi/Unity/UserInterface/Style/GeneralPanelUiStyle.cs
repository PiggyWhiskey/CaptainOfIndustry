// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.GeneralPanelUiStyle
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
  /// <summary>
  /// General panel styles that are shared by multiple different panels.
  /// </summary>
  public class GeneralPanelUiStyle : BaseUiStyle
  {
    private static readonly ColorRgba ResourceNotConumsedClr;

    public GeneralPanelUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual float WindowHeight => 10f;

    public virtual BorderStyle WindowContentBorder => new BorderStyle((ColorRgba) 723723);

    public virtual TextStyle Text => this.Global.Text;

    public virtual TextStyle TextMedium => this.Global.TextMedium;

    public virtual TextStyle SectionTitle => this.Global.Title;

    public virtual TextStyle WindowTitleText
    {
      get
      {
        TextStyle headerTextStyle = this.Global.HeaderTextStyle;
        ref TextStyle local = ref headerTextStyle;
        ColorRgba? color = new ColorRgba?((ColorRgba) 14277081);
        int? nullable = new int?(14);
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual TextStyle HeaderText
    {
      get
      {
        TextStyle headerTextStyle = this.Global.HeaderTextStyle;
        ref TextStyle local = ref headerTextStyle;
        int? nullable = new int?(13);
        ColorRgba? color = new ColorRgba?((ColorRgba) 14277081);
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual BtnStyle HeaderButton
    {
      get
      {
        TextStyle? text = new TextStyle?(new TextStyle(this.Global.DefaultPanelTextColor, fontStyle: new FontStyle?(FontStyle.Bold), isCapitalized: true));
        ColorRgba? nullable1 = new ColorRgba?(this.Global.ControlsBgColor);
        Offset? nullable2 = new Offset?(Offset.All(6f));
        ColorRgba? nullable3 = new ColorRgba?((ColorRgba) 16777215);
        ColorRgba? nullable4 = new ColorRgba?((ColorRgba) 11579311);
        ColorRgba? nullable5 = new ColorRgba?((ColorRgba) 11579311);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = nullable3;
        ColorRgba? hoveredMaskClr = nullable4;
        ColorRgba? pressedMaskClr = nullable5;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = nullable2;
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, sidePaddings: 5, iconPadding: iconPadding);
      }
    }

    public virtual BtnStyle HeaderButtonNegative
    {
      get
      {
        BtnStyle headerButton = this.HeaderButton;
        ref BtnStyle local = ref headerButton;
        ColorRgba? nullable1 = new ColorRgba?(this.Global.DefaultPanelTextColor);
        ColorRgba? nullable2 = new ColorRgba?((ColorRgba) 16528955);
        TextStyle? text = new TextStyle?();
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = nullable1;
        ColorRgba? hoveredClr = nullable2;
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

    public virtual BtnStyle HeaderButtonNegativeOn
    {
      get => this.HeaderButton.ExtendText(new ColorRgba?(this.Global.DangerBtnClr));
    }

    public virtual BtnStyle HeaderCloseButton
    {
      get
      {
        return new BtnStyle()
        {
          Text = this.Global.HeaderTextStyle,
          BackgroundClr = new ColorRgba?(ColorRgba.White),
          NormalMaskClr = new ColorRgba?(this.Global.ControlsBgColor),
          HoveredMaskClr = new ColorRgba?((ColorRgba) 6160384),
          PressedMaskClr = new ColorRgba?((ColorRgba) 4390912)
        };
      }
    }

    public virtual ColorRgba HeaderBackground => this.Global.ControlsBgColor;

    public virtual int HeaderHeight => 28;

    public virtual float HeadeButtonWidth => 28f;

    public virtual ColorRgba Background => new ColorRgba(3815994);

    public virtual BorderStyle Border => this.Global.DefaultDarkBorder;

    public virtual ColorRgba ItemOverlay => (ColorRgba) 3092271;

    public virtual ColorRgba ItemOverlayDark => new ColorRgba(0, 120);

    public virtual ColorRgba PlainIconColor => this.Global.DefaultPanelTextColor;

    public virtual float Padding => 10f;

    public virtual float PaddingCompact => 5f;

    public virtual float Indent => 20f;

    public virtual float LineHeight => 28f;

    public virtual float SectionTitleTopPadding => 5f;

    public virtual float ItemsSpacing => 5f;

    public virtual SliderStyle Slider
    {
      get
      {
        return new SliderStyle()
        {
          BgColor = (ColorRgba) 3487029,
          FillColor = (ColorRgba) 11974326,
          HandleColor = (ColorRgba) 2236962,
          BgSprite = this.Icons.WhiteBgGrayBorder,
          HandleWidth = 18
        };
      }
    }

    public virtual Vector2 TopSquareButtonSize => new Vector2(34f, 34f);

    public virtual BtnStyle InfoBoxDefault
    {
      get
      {
        ColorRgba? nullable1 = new ColorRgba?((ColorRgba) 3684408);
        TextStyle? text = new TextStyle?(new TextStyle()
        {
          FontStyle = FontStyle.Bold,
          FontSize = 14,
          Color = this.Global.DefaultPanelTextColor
        });
        BorderStyle? border = new BorderStyle?(this.Global.DefaultDarkBorder);
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? nullable2 = new ColorRgba?(ColorRgba.White);
        ColorRgba? nullable3 = new ColorRgba?(ColorRgba.White);
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredMaskClr = nullable2;
        ColorRgba? pressedMaskClr = nullable3;
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        return new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, height: 80, iconPadding: iconPadding);
      }
    }

    private BtnStyle InfoBoxDefault_NotConsuming
    {
      get
      {
        return this.InfoBoxDefault.ExtendText(new ColorRgba?(GeneralPanelUiStyle.ResourceNotConumsedClr));
      }
    }

    private BtnStyle InfoBoxDefault_Low
    {
      get => this.InfoBoxDefault.ExtendText(new ColorRgba?(this.Global.DangerClr));
    }

    internal virtual BtnStyle ElectricityInfoBox_Consuming
    {
      get => this.InfoBoxDefault.ExtendText(new ColorRgba?(this.Global.OrangeText));
    }

    internal virtual IconStyle ElectricityInfoBoxIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Electricity.svg", new ColorRgba?(this.Global.OrangeText), new Vector2?(new Vector2(20f, 20f)));
      }
    }

    internal virtual BtnStyle ElectricityInfoBox_NotConsuming => this.InfoBoxDefault_NotConsuming;

    internal virtual IconStyle ElectricityInfoBoxIcon_NotConsuming
    {
      get
      {
        return this.ElectricityInfoBoxIcon.Extend(color: new ColorRgba?(GeneralPanelUiStyle.ResourceNotConumsedClr));
      }
    }

    internal virtual BtnStyle ElectricityInfoBox_Low => this.InfoBoxDefault_Low;

    internal virtual IconStyle ElectricityInfoBoxIcon_Low
    {
      get => this.ElectricityInfoBoxIcon.Extend(color: new ColorRgba?(this.Global.DangerClr));
    }

    internal virtual BtnStyle ElectricityInfoBox_Producing
    {
      get => this.InfoBoxDefault.ExtendText(new ColorRgba?(this.Global.GreenForDark));
    }

    internal virtual IconStyle ElectricityInfoBoxIcon_Producing
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Electricity.svg", new ColorRgba?(this.Global.GreenForDark), new Vector2?(new Vector2(20f, 20f)));
      }
    }

    internal virtual BtnStyle ConsumedUnityInfoBox
    {
      get => this.InfoBoxDefault.ExtendText(new ColorRgba?(this.Global.OrangeText));
    }

    internal virtual IconStyle ConsumedUnityInfoBoxIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/UnitySmall.svg", new ColorRgba?(this.Global.OrangeText), new Vector2?(new Vector2(20f, 20f)));
      }
    }

    internal virtual BtnStyle ConsumedUnityInfoBox_NotConsuming => this.InfoBoxDefault_NotConsuming;

    internal virtual IconStyle ConsumedUnityInfoBoxIcon_NotConsuming
    {
      get
      {
        return this.ConsumedUnityInfoBoxIcon.Extend(color: new ColorRgba?(GeneralPanelUiStyle.ResourceNotConumsedClr));
      }
    }

    internal virtual BtnStyle ConsumedUnityInfoBox_Low => this.InfoBoxDefault_Low;

    internal virtual IconStyle ConsumedUnityInfoBoxIcon_Low
    {
      get => this.ConsumedUnityInfoBoxIcon.Extend(color: new ColorRgba?(this.Global.DangerClr));
    }

    internal virtual BtnStyle WorkersInfoBox
    {
      get => this.InfoBoxDefault.ExtendText(new ColorRgba?(this.Global.BlueForDark));
    }

    internal virtual IconStyle WorkersInfoBoxIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/WorkerSmall.svg", new ColorRgba?(this.Global.BlueForDark), new Vector2?(new Vector2(20f, 20f)));
      }
    }

    internal virtual BtnStyle WorkersInfoBox_NotAssigned
    {
      get
      {
        return this.InfoBoxDefault.ExtendText(new ColorRgba?(GeneralPanelUiStyle.ResourceNotConumsedClr));
      }
    }

    internal virtual IconStyle WorkersInfoBoxIcon_NotAssigned
    {
      get
      {
        return this.WorkersInfoBoxIcon.Extend(color: new ColorRgba?(GeneralPanelUiStyle.ResourceNotConumsedClr));
      }
    }

    public virtual BtnStyle CreatedComputingInfoBox
    {
      get => this.InfoBoxDefault.ExtendText(new ColorRgba?(this.Global.GreenForDark));
    }

    public virtual IconStyle CreatedComputingInfoBoxIcon
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Computing128.png", new ColorRgba?(this.Global.GreenForDark), new Vector2?(new Vector2(20f, 20f)));
      }
    }

    public virtual BtnStyle ComputingInfoBox_Consuming
    {
      get => this.InfoBoxDefault.ExtendText(new ColorRgba?(this.Global.OrangeText));
    }

    public virtual IconStyle ComputingInfoBoxIcon_Consuming
    {
      get
      {
        return new IconStyle("Assets/Unity/UserInterface/General/Computing128.png", new ColorRgba?(this.Global.OrangeText), new Vector2?(new Vector2(20f, 20f)));
      }
    }

    internal virtual BtnStyle ComputingInfoBox_NotConsuming => this.InfoBoxDefault_NotConsuming;

    internal virtual IconStyle ComputingInfoBoxIcon_NotConsuming
    {
      get
      {
        return this.ComputingInfoBoxIcon_Consuming.Extend(color: new ColorRgba?(GeneralPanelUiStyle.ResourceNotConumsedClr));
      }
    }

    internal virtual BtnStyle ComputingInfoBox_Low => this.InfoBoxDefault_Low;

    internal virtual IconStyle ComputingInfoBoxIcon_Low
    {
      get
      {
        return this.ComputingInfoBoxIcon_Consuming.Extend(color: new ColorRgba?(this.Global.DangerClr));
      }
    }

    public virtual ColorRgba StatusCriticalBg => (ColorRgba) 9651259;

    public virtual ColorRgba StatusWarningBg => (ColorRgba) 8746569;

    public virtual ColorRgba StatusOkBg => (ColorRgba) 5203794;

    static GeneralPanelUiStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GeneralPanelUiStyle.ResourceNotConumsedClr = (ColorRgba) 14540253;
    }
  }
}
