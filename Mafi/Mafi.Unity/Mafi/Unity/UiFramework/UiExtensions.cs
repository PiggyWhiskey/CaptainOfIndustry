// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.UiExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  internal static class UiExtensions
  {
    public static float GetPreferedWidth(this Text text)
    {
      TextGenerationSettings generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);
      return text.cachedTextGenerator.GetPreferredWidth(text.text, generationSettings);
    }

    public static float GetPreferedHeight(this Text text)
    {
      TextGenerationSettings generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);
      return text.cachedTextGenerator.GetPreferredHeight(text.text, generationSettings);
    }

    public static float GetPreferedHeight(this Text text, float width)
    {
      TextGenerationSettings generationSettings = text.GetGenerationSettings(new Vector2(width, 0.0f));
      return text.cachedTextGenerator.GetPreferredHeight(text.text, generationSettings);
    }
  }
}
