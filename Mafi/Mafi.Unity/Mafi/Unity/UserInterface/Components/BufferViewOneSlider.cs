// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.BufferViewOneSlider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class BufferViewOneSlider : BufferView
  {
    private readonly Action<float> m_sliderChange;
    private int m_sliderSteps;
    private readonly string m_sliderLabel;
    private readonly ColorRgba? m_customColor;
    private readonly bool m_useImportSlider;
    private Slidder m_slider;
    private bool m_updateSlidersInProgress;
    private bool m_sliderVisible;
    private bool m_showSliderForNoProduct;
    private Func<int, string> m_labelProvider;

    public BufferViewOneSlider(
      IUiElement parent,
      UiBuilder builder,
      Action<float> sliderChange,
      int sliderSteps,
      string sliderLabel,
      Action trashAction,
      Action plusBtnAction,
      ColorRgba? customColor = null,
      bool makeSliderAreaTransparent = false,
      bool useImportSlider = true)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_sliderVisible = true;
      // ISSUE: explicit constructor call
      base.\u002Ector(parent, builder, trashAction, plusBtnAction, true);
      this.m_labelProvider = (Func<int, string>) (i => this.m_sliderLabel);
      this.m_sliderChange = sliderChange;
      this.m_sliderSteps = sliderSteps;
      this.m_sliderLabel = sliderLabel;
      this.m_customColor = customColor;
      this.m_useImportSlider = useImportSlider;
      this.buildSliders(builder);
      if (!this.m_customColor.HasValue)
        return;
      ColorRgba areaColor = makeSliderAreaTransparent ? ColorRgba.Empty : this.m_customColor.Value.SetA((byte) 50);
      if (this.m_useImportSlider)
        this.Bar.SetImportAreaColor(this.m_customColor.Value, areaColor);
      else
        this.Bar.SetExportAreaColor(this.m_customColor.Value, areaColor);
    }

    public void SetLabelFunc(Func<int, string> labelProvider)
    {
      this.m_labelProvider = labelProvider;
    }

    private void buildSliders(UiBuilder builder)
    {
      UiStyle style = builder.Style;
      this.m_slider = this.CreateSlider(this.m_sliderSteps, this.m_customColor ?? style.BufferView.ImportSliderColor, style.Icons.ImportSliderHandle, new Action<float>(this.sliderValueChange), this.m_sliderChange, this.m_sliderLabel);
    }

    public override void UpdateState(
      Option<ProductProto> product,
      Quantity capacity,
      Quantity quantity)
    {
      base.UpdateState(product, capacity, quantity);
      this.m_slider.SetVisibility<Slidder>((this.Product.HasValue || this.m_showSliderForNoProduct) && this.m_sliderVisible);
      if (this.m_useImportSlider)
        this.Bar.SetImportAreaVisibility(this.Product.HasValue && this.m_sliderVisible);
      else
        this.Bar.SetExportAreaVisibility(this.Product.HasValue && this.m_sliderVisible);
    }

    public void UpdateSlider(int value)
    {
      Assert.That<int>(value).IsWithinIncl(0, this.m_sliderSteps);
      this.m_updateSlidersInProgress = true;
      this.m_slider.SetValue((float) value);
      this.m_updateSlidersInProgress = false;
      this.updateSlidersInternal(value);
    }

    public void ShowSliderForNoProduct() => this.m_showSliderForNoProduct = true;

    /// <summary>
    /// Note: This won't override that the slider is hidden when no product is set.
    /// </summary>
    public void SetSliderVisibility(bool isVisible)
    {
      this.m_sliderVisible = isVisible;
      this.m_slider.SetVisibility<Slidder>((this.Product.HasValue || this.m_showSliderForNoProduct) && this.m_sliderVisible);
      if (this.m_useImportSlider)
        this.Bar.SetImportAreaVisibility((this.Product.HasValue || this.m_showSliderForNoProduct) && this.m_sliderVisible);
      else
        this.Bar.SetExportAreaVisibility((this.Product.HasValue || this.m_showSliderForNoProduct) && this.m_sliderVisible);
    }

    private void sliderValueChange(float value)
    {
      if (this.m_updateSlidersInProgress)
        return;
      this.updateSlidersInternal(value.RoundToInt());
    }

    private void updateSlidersInternal(int value)
    {
      ColorRgba color = this.m_customColor ?? this.Builder.Style.BufferView.ImportSliderColor;
      if (this.m_useImportSlider && value == 0 || !this.m_useImportSlider && value == this.m_sliderSteps)
        color = color.SetA((byte) 80);
      this.m_slider.Label.Value.SetColor(color);
      if (this.m_useImportSlider)
        this.Bar.SetImportArea(Percent.FromRatio(value, this.m_sliderSteps));
      else
        this.Bar.SetExportArea(Percent.FromRatio(value, this.m_sliderSteps));
      this.m_label.SetText(this.m_labelProvider(value));
      float leftOffset = 0.0f;
      if (this.m_useImportSlider)
        leftOffset = -((this.m_label.GetPreferedSize().x - 30f).Max(0.0f) / (float) this.m_sliderSteps) * (float) value;
      this.m_label.PutToLeftTopOf<Txt>((IUiElement) this.m_sliderHandle, this.m_label.GetPreferedSize(), Offset.Left(leftOffset));
    }

    public void SetMaxSteps(int sliderSteps)
    {
      this.m_sliderSteps = sliderSteps;
      this.m_slider.SetValuesRange(0.0f, (float) sliderSteps);
    }
  }
}
