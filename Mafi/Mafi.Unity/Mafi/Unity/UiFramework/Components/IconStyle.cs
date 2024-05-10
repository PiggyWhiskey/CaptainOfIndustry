// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.IconStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public struct IconStyle
  {
    public readonly Vector2 Size;
    public readonly ColorRgba Color;
    public readonly string AssetPath;
    public readonly bool PreserveAspectRatio;

    public float Width => this.Size.x;

    public float Height => this.Size.y;

    public IconStyle(string path, ColorRgba? color = null, Vector2? size = null, bool preserveAspect = true)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.AssetPath = path.CheckNotNull<string>();
      this.Color = color ?? ColorRgba.White;
      this.Size = size ?? Vector2.zero;
      this.PreserveAspectRatio = preserveAspect;
    }

    [Pure]
    public IconStyle Extend(string path = null, ColorRgba? color = null, Vector2? size = null, bool? preserveAspect = null)
    {
      return new IconStyle(path ?? this.AssetPath, new ColorRgba?(color ?? this.Color), new Vector2?(size ?? this.Size), ((int) preserveAspect ?? (this.PreserveAspectRatio ? 1 : 0)) != 0);
    }
  }
}
