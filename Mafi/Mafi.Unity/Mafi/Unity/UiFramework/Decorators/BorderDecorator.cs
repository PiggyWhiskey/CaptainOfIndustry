// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Decorators.BorderDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Decorators
{
  internal class BorderDecorator
  {
    public static Option<GameObject> DecorateWithBorders(
      UiBuilder builder,
      Option<GameObject> borderGo,
      GameObject gameObject,
      BorderStyle borderStyle)
    {
      if ((double) borderStyle.Thickness <= 0.0)
      {
        if (borderGo.HasValue)
          borderGo.Value.SetActive(false);
        return borderGo;
      }
      Sprite sharedSprite = builder.AssetsDb.GetSharedSprite(builder.Style.Icons.Border);
      if (borderStyle.Thickness.IsNear(2f))
        sharedSprite = builder.AssetsDb.GetSharedSprite(builder.Style.Icons.Border2T);
      GameObject objectToPlace = borderGo.ValueOrNull;
      if ((Object) objectToPlace == (Object) null)
      {
        objectToPlace = builder.GetClonedGo("Border", gameObject);
        objectToPlace.AddComponent<Image>();
      }
      Image component = objectToPlace.GetComponent<Image>();
      component.color = borderStyle.Color.ToColor();
      component.sprite = sharedSprite;
      component.raycastTarget = false;
      component.fillCenter = false;
      component.type = Image.Type.Sliced;
      objectToPlace.PutTo(gameObject);
      objectToPlace.SetActive(true);
      return (Option<GameObject>) objectToPlace;
    }

    public BorderDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
