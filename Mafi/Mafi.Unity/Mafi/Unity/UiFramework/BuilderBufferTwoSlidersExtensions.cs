// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.BuilderBufferTwoSlidersExtensions
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
  public static class BuilderBufferTwoSlidersExtensions
  {
    public static BufferViewTwoSliders NewBufferWithTwoSliders(
      this UiBuilder builder,
      IUiElement parent,
      Action<float> importSliderChange,
      Action<float> exportSliderChange,
      string importSliderLabel,
      string exportSliderLabel,
      int sliderSteps,
      Action trashAction = null,
      Action plusBtnAction = null)
    {
      return new BufferViewTwoSliders(parent, builder, importSliderChange.CheckNotNull<Action<float>>(), exportSliderChange.CheckNotNull<Action<float>>(), importSliderLabel, exportSliderLabel, sliderSteps.CheckPositive(), trashAction, plusBtnAction);
    }
  }
}
