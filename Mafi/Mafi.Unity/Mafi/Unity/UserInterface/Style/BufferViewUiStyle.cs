// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.BufferViewUiStyle
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
  /// <summary>
  /// Buffer view styles (used in the storage inspector for instance).
  /// </summary>
  public class BufferViewUiStyle : BaseUiStyle
  {
    public BufferViewUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual float Height => 70f;

    public virtual float CompactHeight => 50f;

    public virtual float SuperCompactHeight => 35f;

    public virtual float HeightWithSlider => 80f;

    public virtual float SliderHandleHeight => 28f;

    public virtual float SliderHandleWidth => 28f;

    public virtual float SliderPointerLineWidth => 3f;

    public virtual float SliderBarOverflow => 4f;

    public virtual float SliderBottomOffset => 4f;

    public virtual ColorRgba ImportSliderColor => this.Global.GreenForDark;

    public virtual ColorRgba ExportSliderColor => this.Global.DangerClr;

    public virtual Vector2 TrashIconSize => new Vector2(32f, 32f);

    public virtual TextStyle LabelTextStyle
    {
      get
      {
        TextStyle text = this.Global.Text;
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
  }
}
