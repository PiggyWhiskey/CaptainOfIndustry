// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Styles.SlicedSpriteStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Styles
{
  public class SlicedSpriteStyle
  {
    public readonly string IconAssetPath;
    public Vector4 SliceBorder;

    public SlicedSpriteStyle(string iconAssetPath, int sliceBorder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(iconAssetPath, new Vector4((float) sliceBorder, (float) sliceBorder, (float) sliceBorder, (float) sliceBorder));
    }

    public SlicedSpriteStyle(string iconAssetPath, Vector4 sliceBorder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.IconAssetPath = iconAssetPath.CheckNotNull<string>();
      this.SliceBorder = sliceBorder;
    }
  }
}
