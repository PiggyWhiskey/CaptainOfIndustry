// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.BuilderBufferOneSliderExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public static class BuilderBufferOneSliderExtensions
  {
    public static BufferViewOneSlider NewBufferWithOneSlider(
      this UiBuilder builder,
      IUiElement parent,
      Action<float> sliderChange,
      int sliderSteps,
      string sliderLabel,
      Action trashAction = null,
      Action plusBtnAction = null,
      ColorRgba? customColor = null,
      bool useImportSlider = true,
      bool makeSliderAreaTransparent = false)
    {
      IUiElement parent1 = parent;
      UiBuilder builder1 = builder;
      Action<float> sliderChange1 = sliderChange.CheckNotNull<Action<float>>();
      int sliderSteps1 = sliderSteps.CheckPositive();
      string sliderLabel1 = sliderLabel;
      Action trashAction1 = trashAction;
      Action plusBtnAction1 = plusBtnAction;
      ColorRgba? customColor1 = customColor;
      bool flag = useImportSlider;
      int num1 = makeSliderAreaTransparent ? 1 : 0;
      int num2 = flag ? 1 : 0;
      return new BufferViewOneSlider(parent1, builder1, sliderChange1, sliderSteps1, sliderLabel1, trashAction1, plusBtnAction1, customColor1, num1 != 0, num2 != 0);
    }
  }
}
