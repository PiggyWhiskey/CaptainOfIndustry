// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.ProductWithIconUiStyle
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
  /// <summary>Styles for vertical product with icon.</summary>
  /// <code>
  ///   PRODUCT NAME
  ///     |   |
  ///     |   |
  ///     ------
  ///    QUANTITY
  /// </code>
  public class ProductWithIconUiStyle : BaseUiStyle
  {
    public ProductWithIconUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    public virtual Vector2 Size => new Vector2(70f, 70f);

    public virtual float QuantityLineHeight => 30f;

    public virtual ColorRgba OverlayTextBackground => new ColorRgba(3815994);

    public virtual TextStyle QuantityText
    {
      get
      {
        TextStyle text = this.Global.Text;
        ref TextStyle local = ref text;
        int? nullable1 = new int?(13);
        FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = nullable2;
        int? fontSize = nullable1;
        bool? isCapitalized = new bool?();
        return local.Extend(color, fontStyle, fontSize, isCapitalized);
      }
    }
  }
}
