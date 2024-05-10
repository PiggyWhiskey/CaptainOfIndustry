// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.QuantityBarUiStyle
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
  /// Quantity bar styles.
  /// TODO: Try to merge this with BufferViewUiStyle
  /// </summary>
  public class QuantityBarUiStyle : BaseUiStyle
  {
    public QuantityBarUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
    }

    /// <summary>Use if having a full buffer is a good thing.</summary>
    public virtual ColorRgba BarColor => (ColorRgba) 8879170;

    /// <summary>
    /// Use if having a full buffer is a bad thing (output buffer).
    /// </summary>
    public virtual ColorRgba NegativeBarColor => (ColorRgba) 10766137;

    /// <summary>
    /// Use if having a full buffer is a great thing (export buffer).
    /// </summary>
    public virtual ColorRgba PositiveBarColor => (ColorRgba) 1203234;

    public virtual ColorRgba BackgroundColor => (ColorRgba) 4408131;

    public virtual TextStyle Text
    {
      get
      {
        return new TextStyle()
        {
          Color = ColorRgba.White,
          FontStyle = FontStyle.Normal,
          FontSize = 14
        };
      }
    }
  }
}
