// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.QuantityBar
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class QuantityBar : IUiElement
  {
    private Percent m_importAreaPerc;
    private Percent m_exportAreaPerc;
    private Percent m_percentFull;
    private readonly UiBuilder m_builder;
    private readonly bool m_hasAreas;
    private readonly Txt m_quantityText;
    private readonly Panel m_barContainer;
    private readonly Panel m_quantityBar;
    private readonly Option<Panel> m_importAreaBorder;
    private readonly Option<Panel> m_importAreaOverlay;
    private readonly Option<Panel> m_exportAreaBorder;
    private readonly Option<Panel> m_exportAreaOverlay;

    public GameObject GameObject => this.m_barContainer.GameObject;

    public RectTransform RectTransform => this.m_barContainer.RectTransform;

    public QuantityBar(UiBuilder builder, bool hasAreas = false, Action clickAction = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_hasAreas = hasAreas;
      UiStyle style = builder.Style;
      this.m_barContainer = builder.NewPanel("Bar").SetBackground(style.QuantityBar.BackgroundColor);
      if (clickAction != null)
        this.m_barContainer.OnClick(clickAction);
      this.m_quantityBar = builder.NewPanel("Quantity bar").SetBackground(style.QuantityBar.BarColor).PutToLeftOf<Panel>((IUiElement) this.m_barContainer, 0.0f);
      if (hasAreas)
      {
        this.m_importAreaOverlay = (Option<Panel>) builder.NewPanel("ImportAreaOverlay").SetBackground(new ColorRgba(1260315, 110)).PutToLeftOf<Panel>((IUiElement) this.m_barContainer, 0.0f).Hide<Panel>();
        this.m_exportAreaOverlay = (Option<Panel>) builder.NewPanel("ExportAreaOverlay").SetBackground(new ColorRgba(7677970, 190)).PutToRightOf<Panel>((IUiElement) this.m_barContainer, 0.0f).Hide<Panel>();
      }
      this.m_quantityText = builder.NewTxt("Quantity text").SetText("0").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(style.QuantityBar.Text).PutTo<Txt>((IUiElement) this.m_barContainer, Offset.Right(5f));
      this.m_barContainer.SetBorderStyle(style.Global.DefaultDarkBorder);
      if (!hasAreas)
        return;
      this.m_importAreaBorder = (Option<Panel>) builder.NewPanel("ImportAreaBorder").SetBorderStyle(new BorderStyle(style.BufferView.ImportSliderColor)).PutToLeftOf<Panel>((IUiElement) this.m_barContainer, 0.0f, Offset.All(-1f)).Hide<Panel>();
      this.m_exportAreaBorder = (Option<Panel>) builder.NewPanel("ExportAreaBorder").SetBorderStyle(new BorderStyle(style.BufferView.ExportSliderColor)).PutToRightOf<Panel>((IUiElement) this.m_barContainer, 0.0f, Offset.All(-1f)).Hide<Panel>();
    }

    public void SetImportAreaColor(ColorRgba borderColor, ColorRgba areaColor)
    {
      this.m_importAreaBorder.Value.SetBorderStyle(new BorderStyle(borderColor));
      this.m_importAreaOverlay.Value.SetBackground(areaColor);
    }

    public void SetExportAreaColor(ColorRgba borderColor, ColorRgba areaColor)
    {
      this.m_exportAreaBorder.Value.SetBorderStyle(new BorderStyle(borderColor));
      this.m_exportAreaOverlay.Value.SetBackground(areaColor);
    }

    public void SetImportArea(Percent percent)
    {
      this.m_importAreaPerc = percent;
      this.m_importAreaBorder.Value.SetWidth<Panel>(percent.Apply(this.GetWidth()));
      this.m_importAreaOverlay.Value.SetWidth<Panel>(percent.Apply(this.GetWidth()));
      this.SetImportAreaVisibility(true);
    }

    public void SetExportArea(Percent percent)
    {
      this.m_exportAreaPerc = percent;
      this.updateExportAreaInternal();
      this.SetExportAreaVisibility(true);
    }

    public void SetImportAreaVisibility(bool isVisible)
    {
      this.m_importAreaBorder.Value.SetVisibility<Panel>(isVisible);
      this.m_importAreaOverlay.Value.SetVisibility<Panel>(isVisible);
    }

    public void SetExportAreaVisibility(bool isVisible)
    {
      this.m_exportAreaBorder.Value.SetVisibility<Panel>(isVisible);
      this.m_exportAreaOverlay.Value.SetVisibility<Panel>(isVisible);
    }

    /// <summary>
    /// Set the slider to use a negative color for displayed quantity.
    /// </summary>
    public void UseNegativeColor()
    {
      this.m_quantityBar.SetBackground(this.m_builder.Style.QuantityBar.NegativeBarColor);
    }

    /// <summary>
    /// Set the slider to use a positive color for displayed quantity.
    /// </summary>
    public void UsePositiveColor()
    {
      this.m_quantityBar.SetBackground(this.m_builder.Style.QuantityBar.PositiveBarColor);
    }

    /// <summary>Set the slider to use a neutral color.</summary>
    public void UseNeutralColor()
    {
      this.m_quantityBar.SetBackground(this.m_builder.Style.QuantityBar.BarColor);
    }

    public void AlignTextToLeft(Offset offset)
    {
      this.m_quantityText.SetAlignment(TextAnchor.MiddleLeft);
      this.m_quantityText.PutTo<Txt>((IUiElement) this.m_barContainer, offset);
    }

    private void updateExportAreaInternal()
    {
      this.m_exportAreaBorder.Value.SetWidth<Panel>((Percent.Hundred - this.m_exportAreaPerc).Apply(this.GetWidth()));
      Percent percent = Percent.Hundred - this.m_percentFull;
      this.m_exportAreaOverlay.Value.PutToRightOf<Panel>((IUiElement) this.m_barContainer, (this.m_percentFull - this.m_exportAreaPerc).Max(Percent.Zero).Apply(this.GetWidth()), Offset.Right(percent.Apply(this.GetWidth())));
    }

    public void UpdateValues(Percent percentFull, LocStrFormatted str)
    {
      this.UpdateValues(percentFull, str.Value);
    }

    public void UpdateValues(Percent percentFull, string str)
    {
      this.m_percentFull = percentFull;
      this.m_quantityText.SetText(str);
      float width = this.GetWidth();
      this.m_quantityBar.SetWidth<Panel>(this.m_percentFull.Clamp0To100().Apply(width));
      if (!this.m_exportAreaBorder.HasValue)
        return;
      this.updateExportAreaInternal();
    }

    public void UpdateValues(Quantity capacity, Quantity quantity)
    {
      Assert.That<Quantity>(capacity).IsNotNegative();
      Percent percentFull = capacity.IsZero ? Percent.Zero : Percent.FromRatio(quantity.Value, capacity.Value);
      this.UpdateValues(percentFull, string.Format("{0} / {1} ({2})", (object) quantity.Value, (object) capacity.Value, (object) percentFull.ToStringRounded()));
    }

    public void UpdateSize()
    {
      float width = this.GetWidth();
      this.m_quantityBar.SetWidth<Panel>(this.m_percentFull.Clamp0To100().Apply(width));
    }

    public void SetEmpty()
    {
      this.m_quantityText.SetText("");
      this.m_quantityBar.SetWidth<Panel>(0.0f);
      this.m_percentFull = Percent.Zero;
      if (this.m_exportAreaBorder.HasValue)
        this.SetExportArea(Percent.Hundred);
      if (!this.m_importAreaBorder.HasValue)
        return;
      this.SetImportArea(Percent.Zero);
    }

    public QuantityBar SetColor(ColorRgba color)
    {
      this.m_quantityBar.SetBackground(color);
      return this;
    }

    public QuantityBar.Marker AddMarker(Percent position, ColorRgba color)
    {
      QuantityBar.Marker marker = new QuantityBar.Marker(this.m_builder.NewPanel("marker"), (IUiElement) this.m_barContainer);
      marker.SetPosition(position);
      marker.SetColor(color);
      return marker;
    }

    public class Marker : IUiElement
    {
      private readonly Panel m_marker;
      private readonly IUiElement m_container;
      private Option<Tooltip> m_tooltip;

      public GameObject GameObject => this.m_marker.GameObject;

      public RectTransform RectTransform => this.m_marker.RectTransform;

      public Marker(Panel marker, IUiElement container)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_marker = marker;
        this.m_container = container;
      }

      public void SetColor(ColorRgba color) => this.m_marker.SetBackground(color);

      public void SetPosition(Percent position)
      {
        this.m_marker.PutToLeftMiddleOf<Panel>(this.m_container, new Vector2(2f, this.m_container.GetHeight()), Offset.LeftRight(this.m_container.GetWidth() * position.ToFloat()));
      }

      public Tooltip AddTooltip(UiBuilder builder, LocStrFormatted text)
      {
        if (this.m_tooltip.IsNone)
        {
          Panel element = builder.NewPanel("TransparentOverlay").SetBackground(ColorRgba.Empty).PutTo<Panel>((IUiElement) this.m_marker, Offset.LeftRight(-10f));
          this.m_tooltip = (Option<Tooltip>) builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) element).SetText(text);
        }
        return this.m_tooltip.Value;
      }
    }
  }
}
