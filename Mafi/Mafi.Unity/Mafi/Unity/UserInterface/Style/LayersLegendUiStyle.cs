// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.LayersLegendUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  /// <summary>Styles for resources legend.</summary>
  public class LayersLegendUiStyle : BaseUiStyle
  {
    public LayersLegendUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual int Width => 220;

    public virtual int ItemHeight => 32;

    public virtual BtnStyle ItemBtnStyle
    {
      get
      {
        return new BtnStyle(backgroundClr: new ColorRgba?((ColorRgba) 16777215), normalMaskClr: new ColorRgba?(ColorRgba.Black.SetA((byte) 0)), hoveredMaskClr: new ColorRgba?(ColorRgba.Black.SetA((byte) 75)), pressedMaskClr: new ColorRgba?(ColorRgba.Black.SetA((byte) 100)));
      }
    }

    /// <summary>Size of the icon for each of the items.</summary>
    public virtual Vector2 IconSize => new Vector2(26f, 26f);

    /// <summary>
    /// Size of box that displays the color of the resource. Border sizes included!
    /// </summary>
    public virtual Vector2 ColorBoxSize => new Vector2(20f, 20f);

    /// <summary>Color box border style.</summary>
    public virtual BorderStyle ColorBoxBorderStyle => new BorderStyle(ColorRgba.Black, 2f);

    public virtual TextStyle ShowLabelTextStyle
    {
      get
      {
        TextStyle text = this.Global.Text;
        ref TextStyle local = ref text;
        ColorRgba? color = new ColorRgba?(this.Global.DefaultPanelDisabledTextColor);
        int? nullable1 = new int?(11);
        bool? nullable2 = new bool?(true);
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable1;
        bool? isCapitalized = nullable2;
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }

    public virtual TextStyle HideLabelTextStyle
    {
      get
      {
        return this.Global.Text.Extend(new ColorRgba?(this.Global.DefaultPanelTextColor), new FontStyle?(FontStyle.Bold), new int?(11), new bool?(true));
      }
    }

    /// <summary>Icon that is displayed when the resource is hidden.</summary>
    public virtual IconStyle ShowIconStyle
    {
      get
      {
        return new IconStyle(this.Icons.Show, new ColorRgba?(this.Global.DefaultPanelDisabledTextColor), new Vector2?(new Vector2(20f, 20f)));
      }
    }

    /// <summary>Icon that is displayed when the resource is visible.</summary>
    public virtual IconStyle HideIconStyle
    {
      get
      {
        return new IconStyle(this.Icons.Hide, new ColorRgba?(this.Global.DefaultPanelTextColor), new Vector2?(new Vector2(20f, 20f)));
      }
    }
  }
}
