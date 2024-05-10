// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Decorators.BackgroundDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Decorators
{
  internal class BackgroundDecorator
  {
    public static void DecorateWithBgImage(
      GameObject gameObject,
      Sprite sprite,
      UiBuilder builder = null,
      ColorRgba? color = null,
      bool isTiled = false)
    {
      Image image = gameObject.GetComponent<Image>();
      if ((Object) image == (Object) null)
        image = gameObject.AddComponent<Image>();
      image.color = color.HasValue ? color.GetValueOrDefault().ToColor() : Color.white;
      image.sprite = sprite;
      if (isTiled)
        image.type = Image.Type.Tiled;
      if (!(sprite.border != Vector4.zero))
        return;
      if (isTiled)
        Log.Error("Background set to tiled but sprite is sliced. Ignoring tiling.");
      image.type = Image.Type.Sliced;
    }

    public BackgroundDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
