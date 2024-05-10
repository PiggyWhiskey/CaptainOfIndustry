// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentWithImageExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentWithImageExtensions
  {
    public static T Image<T>(this T component, string path) where T : IComponentWithImage, UiComponent
    {
      component.RunWithBuilder((Action<UiBuilder>) (bld => ((T) component).SetImage((Option<Texture2D>) bld.AssetsDb.GetSharedTexture(path))));
      return component;
    }

    public static T Image<T>(this T component, Option<Texture2D> texture) where T : IComponentWithImage
    {
      component.SetImage(texture);
      return component;
    }

    /// <summary>Scales and crops the image to fill its bounds completely</summary>
    public static T ImageCover<T>(this T component) where T : IComponentWithImage
    {
      component.SetImageScaleMode(ScaleMode.ScaleAndCrop);
      return component;
    }

    /// <summary>Scales the image to fit its bounds, potentially leaving empty space</summary>
    public static T ImageFit<T>(this T component) where T : IComponentWithImage
    {
      component.SetImageScaleMode(ScaleMode.ScaleToFit);
      return component;
    }
  }
}
