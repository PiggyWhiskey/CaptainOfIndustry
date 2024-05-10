// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.TextOverflowExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class TextOverflowExtensions
  {
    public static void ApplyTo(this TextOverflow overflow, VisualElement element)
    {
      switch (overflow)
      {
        case TextOverflow.Wrap:
          element.style.textOverflow = (StyleEnum<UnityEngine.UIElements.TextOverflow>) StyleKeyword.Null;
          element.style.overflow = (StyleEnum<Overflow>) StyleKeyword.Null;
          element.style.whiteSpace = (StyleEnum<WhiteSpace>) StyleKeyword.Null;
          element.style.unityTextAlign = (StyleEnum<TextAnchor>) StyleKeyword.Null;
          break;
        case TextOverflow.Clip:
          element.style.textOverflow = (StyleEnum<UnityEngine.UIElements.TextOverflow>) UnityEngine.UIElements.TextOverflow.Clip;
          element.style.overflow = (StyleEnum<Overflow>) Overflow.Hidden;
          element.style.whiteSpace = (StyleEnum<WhiteSpace>) WhiteSpace.NoWrap;
          element.style.unityTextAlign = (StyleEnum<TextAnchor>) TextAnchor.MiddleLeft;
          break;
        case TextOverflow.Ellipsis:
          element.style.textOverflow = (StyleEnum<UnityEngine.UIElements.TextOverflow>) UnityEngine.UIElements.TextOverflow.Ellipsis;
          element.style.overflow = (StyleEnum<Overflow>) Overflow.Hidden;
          element.style.whiteSpace = (StyleEnum<WhiteSpace>) WhiteSpace.NoWrap;
          element.style.unityTextAlign = (StyleEnum<TextAnchor>) TextAnchor.MiddleLeft;
          break;
      }
    }
  }
}
