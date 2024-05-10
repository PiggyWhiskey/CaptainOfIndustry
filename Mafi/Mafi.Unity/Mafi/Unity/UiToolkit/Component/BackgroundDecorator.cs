// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.BackgroundDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class BackgroundDecorator : IBackgroundDecorator
  {
    private static readonly BackgroundDecorator s_instance;
    private VisualElement m_element;

    public static BackgroundDecorator GetSharedInstance(VisualElement element)
    {
      BackgroundDecorator.s_instance.m_element = element;
      return BackgroundDecorator.s_instance;
    }

    public void SetBackground(ColorRgba? background)
    {
      if (background.HasValue)
        this.m_element.style.backgroundColor = (StyleColor) background.Value.ToColor();
      else
        this.m_element.style.backgroundColor = new StyleColor(StyleKeyword.Null);
    }

    public void SetBackground(
      IComponentWithBackground component,
      [CanBeNull] string imageAssetPath,
      ColorRgba tintColor = default (ColorRgba))
    {
      VisualElement element = this.m_element;
      if (imageAssetPath == null)
      {
        element.style.backgroundImage = new StyleBackground(StyleKeyword.Null);
      }
      else
      {
        if (tintColor.IsNotEmpty)
          element.style.unityBackgroundImageTintColor = (StyleColor) tintColor.ToColor();
        element.style.backgroundSize = (StyleBackgroundSize) new BackgroundSize(BackgroundSizeType.Contain);
        component.RunWithBuilder((Action<UiBuilder>) (builder => element.style.backgroundImage = new StyleBackground(builder.AssetsDb.GetSharedTexture(imageAssetPath))));
      }
    }

    public void SetBackgroundScale(BackgroundSizeType type)
    {
      this.m_element.style.backgroundSize = (StyleBackgroundSize) new BackgroundSize(type);
    }

    public BackgroundDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static BackgroundDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BackgroundDecorator.s_instance = new BackgroundDecorator();
    }
  }
}
