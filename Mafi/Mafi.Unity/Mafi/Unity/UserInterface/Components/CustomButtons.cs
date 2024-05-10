// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.CustomButtons
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public static class CustomButtons
  {
    public static Btn NewBtnGeneral(this UiBuilder builder, string name, IUiElement parent = null)
    {
      return builder.NewBtn(name, parent).SetButtonStyle(builder.Style.Global.GeneralBtn);
    }

    internal static Btn NewBtnGeneralBig(this UiBuilder builder, string name, IUiElement parent = null)
    {
      Btn btn = builder.NewBtn(name, parent);
      BtnStyle generalBtn = builder.Style.Global.GeneralBtn;
      ref BtnStyle local1 = ref generalBtn;
      ref TextStyle local2 = ref builder.Style.Global.GeneralBtn.Text;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle? text = new TextStyle?(local2.Extend(color, fontStyle, fontSize, isCapitalized));
      BorderStyle? border = new BorderStyle?();
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
      BtnStyle buttonStyle = local1.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      return btn.SetButtonStyle(buttonStyle);
    }

    internal static Btn NewBtnGeneralWide(this UiBuilder builder, string name, IUiElement parent = null)
    {
      Btn btn = builder.NewBtn(name, parent);
      BtnStyle generalBtn = builder.Style.Global.GeneralBtn;
      ref BtnStyle local1 = ref generalBtn;
      int? nullable1 = new int?(20);
      ref TextStyle local2 = ref builder.Style.Global.GeneralBtn.Text;
      int? nullable2 = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable2;
      bool? isCapitalized = new bool?();
      TextStyle? text = new TextStyle?(local2.Extend(color, fontStyle, fontSize, isCapitalized));
      BorderStyle? border = new BorderStyle?();
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
      int? sidePaddings = nullable1;
      Offset? iconPadding = new Offset?();
      BtnStyle buttonStyle = local1.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      return btn.SetButtonStyle(buttonStyle);
    }

    public static Btn NewReturnBtn(this UiBuilder builder, IUiElement parent = null)
    {
      Btn element = builder.NewBtnGeneral("GoBack", parent).SetText(Tr.GoBack.TranslatedString).EnableDynamicSize().SetIcon("Assets/Unity/UserInterface/General/Return.svg", 18.Vector2());
      element.SetSize<Btn>(new Vector2(element.GetOptimalWidth() + 10f, 40f));
      return element;
    }

    public static Btn NewBtnPrimary(this UiBuilder builder, string name, IUiElement parent = null)
    {
      return builder.NewBtn(name, parent).SetButtonStyle(builder.Style.Global.PrimaryBtn);
    }

    internal static Btn NewBtnPrimaryBig(this UiBuilder builder, string name, IUiElement parent = null)
    {
      Btn btn = builder.NewBtn(name, parent);
      BtnStyle primaryBtn = builder.Style.Global.PrimaryBtn;
      ref BtnStyle local1 = ref primaryBtn;
      ref TextStyle local2 = ref builder.Style.Global.PrimaryBtn.Text;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle? text = new TextStyle?(local2.Extend(color, fontStyle, fontSize, isCapitalized));
      BorderStyle? border = new BorderStyle?();
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
      BtnStyle buttonStyle = local1.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      return btn.SetButtonStyle(buttonStyle);
    }

    internal static Btn NewBtnPrimaryWide(this UiBuilder builder, string name, IUiElement parent = null)
    {
      Btn btn = builder.NewBtn(name, parent).SetText(name);
      BtnStyle primaryBtn = builder.Style.Global.PrimaryBtn;
      ref BtnStyle local1 = ref primaryBtn;
      int? nullable1 = new int?(20);
      ref TextStyle local2 = ref builder.Style.Global.PrimaryBtn.Text;
      int? nullable2 = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable2;
      bool? isCapitalized = new bool?();
      TextStyle? text = new TextStyle?(local2.Extend(color, fontStyle, fontSize, isCapitalized));
      BorderStyle? border = new BorderStyle?();
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
      int? sidePaddings = nullable1;
      Offset? iconPadding = new Offset?();
      BtnStyle buttonStyle = local1.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      return btn.SetButtonStyle(buttonStyle);
    }

    public static Btn NewBtnUpoints(this UiBuilder builder, string name, IUiElement parent = null)
    {
      return builder.NewBtn(name, parent).SetButtonStyle(builder.Style.Global.UpointsBtn);
    }

    public static Btn NewBtnDanger(this UiBuilder builder, string name, IUiElement parent = null)
    {
      return builder.NewBtn(name, parent).SetButtonStyle(builder.Style.Global.DangerBtn);
    }

    internal static Btn NewBtnDangerBig(this UiBuilder builder, string name, IUiElement parent = null)
    {
      Btn btn = builder.NewBtn(name, parent).SetText(name);
      BtnStyle dangerBtn = builder.Style.Global.DangerBtn;
      ref BtnStyle local1 = ref dangerBtn;
      ref TextStyle local2 = ref builder.Style.Global.DangerBtn.Text;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle? text = new TextStyle?(local2.Extend(color, fontStyle, fontSize, isCapitalized));
      BorderStyle? border = new BorderStyle?();
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
      BtnStyle buttonStyle = local1.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      return btn.SetButtonStyle(buttonStyle);
    }
  }
}
