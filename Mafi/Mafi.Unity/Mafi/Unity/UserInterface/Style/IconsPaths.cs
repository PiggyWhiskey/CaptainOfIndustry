// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.IconsPaths
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  public class IconsPaths
  {
    public virtual string Transform => "Assets/Unity/UserInterface/General/Transform128.png";

    public virtual string Plus => "Assets/Unity/UserInterface/General/Plus.svg";

    public virtual string Enable => "Assets/Unity/UserInterface/General/EnableMachine128.png";

    public virtual string ArrowUp => "Assets/Unity/UserInterface/General/UpArrow128.png";

    public virtual string ArrowDown => "Assets/Unity/UserInterface/General/DownArrow128.png";

    public virtual string Alert => "Assets/Unity/UserInterface/General/Bell128.png";

    public virtual string Show => "Assets/Unity/UserInterface/General/Show128.png";

    public virtual string Hide => "Assets/Unity/UserInterface/General/Hide128.png";

    public virtual string Warning => "Assets/Unity/UserInterface/General/Warning128.png";

    public virtual string Pause => "Assets/Unity/UserInterface/Toolbar/Pause128.png";

    public virtual string Play => "Assets/Unity/UserInterface/Toolbar/Play128.png";

    public virtual string FastForward => "Assets/Unity/UserInterface/Toolbar/FastForward128.png";

    public virtual string SuperFastForward
    {
      get => "Assets/Unity/UserInterface/Toolbar/FastFastForward128.png";
    }

    public virtual string ImportSliderHandle
    {
      get => "Assets/Unity/UserInterface/General/ImportSlider128.png";
    }

    public virtual string ExportSliderHandle
    {
      get => "Assets/Unity/UserInterface/General/ExportSlider128.png";
    }

    public virtual string GradientToLeft
    {
      get => "Assets/Unity/UserInterface/General/HorizontalGradientToLeft48.png";
    }

    public virtual string GradientToRight
    {
      get => "Assets/Unity/UserInterface/General/HorizontalGradientToRight48.png";
    }

    public virtual string ArrowToLeft => this.ResArrow;

    public virtual string ArrowToRight => "Assets/Unity/UserInterface/General/ArrowLeft128.png";

    public virtual SlicedSpriteStyle Border
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/Border64.png", 2);
    }

    public virtual SlicedSpriteStyle Border2T
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/Border2T64.png", 2);
    }

    public virtual SlicedSpriteStyle BorderGradient
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/BorderGradient64.png", 9);
    }

    public virtual SlicedSpriteStyle BorderGradientV2
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/BorderGradient64-v2.png", 6);
    }

    public virtual SlicedSpriteStyle BtnShadow
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/BtnShadow64.png", 7);
    }

    public virtual SlicedSpriteStyle Footer
    {
      get
      {
        return new SlicedSpriteStyle("Assets/Unity/UserInterface/General/Footer64.png", new Vector4(9f, 8f, 9f, 0.0f));
      }
    }

    public virtual string Lock => "Assets/Unity/UserInterface/General/Locked128.png";

    public virtual string Tick => "Assets/Unity/UserInterface/General/Tick128.png";

    public virtual string Working => "Assets/Unity/UserInterface/General/Working128.png";

    public virtual string Empty => "Assets/Unity/UserInterface/General/Empty128.png";

    public virtual string ResLineH => "Assets/Unity/UserInterface/General/ResLineH32.png";

    public virtual string ResLineV => "Assets/Unity/UserInterface/General/ResLineV32.png";

    public virtual string ResArrow => "Assets/Unity/UserInterface/General/ResArrow128.png";

    public virtual string ResCornerTR => "Assets/Unity/UserInterface/General/ResCornerTR32.png";

    public virtual string ResCornerRB => "Assets/Unity/UserInterface/General/ResCornerRB32.png";

    public virtual string ResCornerRT => "Assets/Unity/UserInterface/General/ResCornerRT32.png";

    public virtual string ResCornerBR => "Assets/Unity/UserInterface/General/ResCornerBR32.png";

    public virtual SlicedSpriteStyle WhiteBgBlueBorder
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/WhiteBlueBg32.png", 5);
    }

    public virtual SlicedSpriteStyle WhiteBgGreenBorder
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/WhiteGreenBg32.png", 5);
    }

    public virtual SlicedSpriteStyle WhiteBgRedBorder
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/WhiteRedBg32.png", 5);
    }

    public virtual SlicedSpriteStyle WhiteBgGrayBorder
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/WhiteGrayBg32.png", 6);
    }

    public virtual SlicedSpriteStyle WhiteBgBlackBorder
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/WhiteBlackBg32.png", 6);
    }

    public virtual SlicedSpriteStyle GrayBgWhiteBorder
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/GrayWhiteBg.png", 3);
    }

    public virtual SlicedSpriteStyle WhiteBorderAliased
    {
      get => new SlicedSpriteStyle("Assets/Unity/UserInterface/General/WhiteBorderAliased.png", 3);
    }

    public IconsPaths()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
