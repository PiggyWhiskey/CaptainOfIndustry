// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.BufferViewTwoSliders
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
  public class BufferViewTwoSliders : BufferView
  {
    private readonly Action<float> m_importSliderChange;
    private readonly Action<float> m_exportSliderChange;
    private readonly string m_importSliderLabel;
    private readonly string m_exportSliderLabel;
    private readonly int m_sliderSteps;
    private Slidder m_importSlider;
    private Slidder m_exportSlider;
    private bool m_updateSlidersInProgress;
    private Quantity m_capacity;

    public BufferViewTwoSliders(
      IUiElement parent,
      UiBuilder builder,
      Action<float> importSliderChange,
      Action<float> exportSliderChange,
      string importSliderLabel,
      string exportSliderLabel,
      int sliderSteps,
      Action trashAction,
      Action plusBtnAction)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(parent, builder, trashAction, plusBtnAction, true);
      this.m_importSliderChange = importSliderChange.CheckNotNull<Action<float>>();
      this.m_exportSliderChange = exportSliderChange.CheckNotNull<Action<float>>();
      this.m_importSliderLabel = importSliderLabel.CheckNotNull<string>();
      this.m_exportSliderLabel = exportSliderLabel.CheckNotNull<string>();
      this.m_sliderSteps = sliderSteps;
      this.buildSliders(builder);
    }

    private void buildSliders(UiBuilder builder)
    {
      UiStyle style = builder.Style;
      this.m_importSlider = this.CreateSlider(this.m_sliderSteps, style.BufferView.ImportSliderColor, style.Icons.ImportSliderHandle, new Action<float>(this.importSliderValueChange), this.m_importSliderChange, this.m_importSliderLabel, true);
      this.m_exportSlider = this.CreateSlider(this.m_sliderSteps, style.BufferView.ExportSliderColor, style.Icons.ExportSliderHandle, new Action<float>(this.exportSliderValueChange), this.m_exportSliderChange, this.m_exportSliderLabel);
    }

    public override void UpdateState(
      Option<ProductProto> product,
      Quantity capacity,
      Quantity quantity)
    {
      this.m_capacity = capacity;
      base.UpdateState(product, capacity, quantity);
      if (product.HasValue)
      {
        this.updateSlidersInternal(this.m_importSlider.Value.RoundToInt(), this.m_exportSlider.Value.RoundToInt());
      }
      else
      {
        this.m_importSlider.Hide<Slidder>();
        this.m_exportSlider.Hide<Slidder>();
      }
    }

    public void UpdateSliders(Percent import, Percent export)
    {
      Assert.That<Percent>(import).IsWithin0To100PercIncl();
      Assert.That<Percent>(export).IsWithin0To100PercIncl();
      this.UpdateSliders(import.Apply(this.m_sliderSteps), export.Apply(this.m_sliderSteps));
    }

    public void UpdateSliders(int importValue, int exportValue)
    {
      Assert.That<bool>(importValue == 0 && exportValue == 0 || importValue == this.m_sliderSteps && exportValue == this.m_sliderSteps || importValue < exportValue).IsTrue();
      Assert.That<int>(importValue).IsWithinIncl(0, this.m_sliderSteps);
      Assert.That<int>(exportValue).IsWithinIncl(0, this.m_sliderSteps);
      this.m_updateSlidersInProgress = true;
      this.m_importSlider.SetValue((float) importValue);
      this.m_exportSlider.SetValue((float) exportValue);
      this.m_updateSlidersInProgress = false;
      if (this.Product.HasValue)
      {
        this.updateSlidersInternal(this.m_importSlider.Value.RoundToInt(), this.m_exportSlider.Value.RoundToInt());
      }
      else
      {
        this.m_importSlider.Hide<Slidder>();
        this.m_exportSlider.Hide<Slidder>();
      }
    }

    public void SetSlidersEnabled(bool enabled)
    {
      this.m_importSlider.SetEnabled(enabled);
      this.m_exportSlider.SetEnabled(enabled);
    }

    private void importSliderValueChange(float value)
    {
      if (this.m_updateSlidersInProgress)
        return;
      int importValue = value.RoundToInt();
      int exportValue = this.m_exportSlider.Value.RoundToInt();
      if (importValue >= exportValue && (importValue != this.m_sliderSteps || exportValue != this.m_sliderSteps))
        this.m_importSlider.SetValue(value - 1f);
      else
        this.updateSlidersInternal(importValue, exportValue);
    }

    private void exportSliderValueChange(float value)
    {
      if (this.m_updateSlidersInProgress)
        return;
      int exportValue = value.RoundToInt();
      int importValue = this.m_importSlider.Value.RoundToInt();
      if (exportValue <= importValue && (importValue != 0 || exportValue != 0))
        this.m_exportSlider.SetValue(value + 1f);
      else
        this.updateSlidersInternal(importValue, exportValue);
    }

    private void updateSlidersInternal(int importValue, int exportValue)
    {
      if (importValue == 0 && exportValue == 0)
      {
        this.m_importSlider.Hide<Slidder>();
        this.m_exportSlider.Show<Slidder>();
      }
      else if (importValue == this.m_sliderSteps && exportValue == this.m_sliderSteps)
      {
        this.m_importSlider.Show<Slidder>();
        this.m_exportSlider.Hide<Slidder>();
      }
      else
      {
        this.m_importSlider.Show<Slidder>();
        this.m_exportSlider.Show<Slidder>();
      }
      ColorRgba color1 = this.Builder.Style.BufferView.ImportSliderColor;
      Txt element1 = this.m_importSlider.Label.Value;
      if (importValue == 0)
      {
        color1 = color1.SetA((byte) 80);
        element1.SetText(this.m_importSliderLabel);
      }
      else
      {
        Quantity quantity = this.m_capacity.ScaledBy(Percent.FromRatio(importValue, this.m_sliderSteps));
        element1.SetText(string.Format("[{0}] {1}", (object) quantity, (object) this.m_importSliderLabel));
      }
      element1.SetWidth<Txt>(element1.GetPreferedWidth());
      element1.SetColor(color1);
      ColorRgba color2 = this.Builder.Style.BufferView.ExportSliderColor;
      Txt element2 = this.m_exportSlider.Label.Value;
      if (exportValue == this.m_sliderSteps)
      {
        color2 = color2.SetA((byte) 80);
        element2.SetText(this.m_exportSliderLabel);
      }
      else
      {
        Quantity quantity = this.m_capacity.ScaledBy(Percent.FromRatio(exportValue, this.m_sliderSteps));
        element2.SetText(string.Format("{0} [{1}]", (object) this.m_exportSliderLabel, (object) quantity));
      }
      element2.SetWidth<Txt>(element2.GetPreferedWidth());
      element2.SetColor(color2);
      this.Bar.SetImportArea(Percent.FromRatio(importValue, this.m_sliderSteps));
      this.Bar.SetExportArea(Percent.FromRatio(exportValue, this.m_sliderSteps));
    }
  }
}
