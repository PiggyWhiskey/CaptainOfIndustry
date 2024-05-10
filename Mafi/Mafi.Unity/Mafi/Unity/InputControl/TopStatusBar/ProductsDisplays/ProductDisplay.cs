// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays.ProductDisplay
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays
{
  internal class ProductDisplay : IUiElementWithUpdater, IUiElement
  {
    public const int HEIGHT = 30;
    public const float WIDTH = 90f;
    private readonly ProductDisplayPanel m_owner;
    private readonly UiBuilder m_builder;
    private readonly Panel m_container;
    private readonly ProductStats m_stats;
    private readonly Panel[] m_bars;
    private readonly Lyst<QuantityLarge> m_lastKnownData;
    private QuantityLarge m_thisMonthQuantity;
    private QuantityLarge m_maxValue;
    private int m_thisMonthBarIndex;
    private bool m_needsToUpdateBars;

    public IUiUpdater Updater { get; }

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ProductProto Product { get; set; }

    public ProductDisplay(
      IUiElement parent,
      ProductDisplayPanel owner,
      ProductProto product,
      UiBuilder builder,
      IProductsManager productsManager,
      Action<ProductProto> onRightClick,
      Action onLeftCLick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lastKnownData = new Lyst<QuantityLarge>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductDisplay productDisplay = this;
      this.m_owner = owner;
      this.m_builder = builder;
      this.Product = product;
      this.m_stats = productsManager.GetStatsFor(product);
      this.m_container = builder.NewPanel(nameof (ProductDisplay), parent).SetBackground(builder.Style.Global.PanelsBg).SetWidth<Panel>(90f).SetHeight<Panel>(30f);
      Panel leftBottomOf1 = builder.NewPanel("ProductIcon", (IUiElement) this.m_container).SetBackground(product.Graphics.IconPath).OnClick(onLeftCLick).OnRightClick((Action) (() => onRightClick(productDisplay.Product))).PutToLeftBottomOf<Panel>((IUiElement) this.m_container, new Vector2(24f, 24f));
      TitleTooltip titleTooltip = new TitleTooltip(this.m_builder);
      titleTooltip.SetMaxWidthOverflow(120);
      titleTooltip.SetText((LocStrFormatted) product.Strings.Name);
      titleTooltip.AttachTo<Panel>((IUiElementWithHover<Panel>) leftBottomOf1);
      this.m_bars = new Panel[6];
      float leftOffset = 30f;
      int x = 3;
      int num1 = 2;
      for (int index = 0; index < 6; ++index)
      {
        Panel leftBottomOf2 = builder.NewPanel("Bar", (IUiElement) this.m_container).SetBackground((ColorRgba) 9539985).PutToLeftBottomOf<Panel>((IUiElement) this.m_container, new Vector2((float) x, 0.0f), Offset.Left(leftOffset) + Offset.Bottom(4f));
        this.m_bars[index] = leftBottomOf2;
        leftOffset += (float) (x + num1);
      }
      Txt amount = builder.NewTxt("Amount", (IUiElement) this.m_container).SetText("0").AllowHorizontalOverflow().AllowVerticalOverflow().SetTextStyle(new TextStyle()
      {
        FontStyle = FontStyle.Bold,
        FontSize = 14,
        Color = new ColorRgba(14277081)
      }).SetAlignment(TextAnchor.LowerRight).PutToLeftOf<Txt>((IUiElement) this.m_container, 60f, Offset.Bottom(4f) + Offset.Left(30f));
      SimpleProgressBar storageState = new SimpleProgressBar((IUiElement) this.m_container, builder).SetColor((ColorRgba) 13948116).SetBackgroundColor(new ColorRgba(7763574)).PutToLeftBottomOf<SimpleProgressBar>((IUiElement) this.m_container, new Vector2(60f, 2f), Offset.Left(30f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<QuantityLarge>((Func<QuantityLarge>) (() => productDisplay.m_stats.StorageCapacity)).Observe<QuantityLarge>((Func<QuantityLarge>) (() => productDisplay.m_stats.StoredQuantityTotal)).Do((Action<QuantityLarge, QuantityLarge>) ((capacity, quantity) =>
      {
        amount.SetText(IntegerSiSuffixFormatter.FormatNumber(quantity.Value));
        if (quantity > closure_0.m_maxValue)
        {
          float num2 = (float) closure_0.m_maxValue.Value / (float) quantity.Value;
          for (int index = 0; index < closure_0.m_thisMonthBarIndex; ++index)
          {
            Panel bar = closure_0.m_bars[index];
            bar.SetHeight<Panel>(bar.GetHeight() * num2);
          }
          closure_0.m_maxValue = quantity;
        }
        Panel bar1 = closure_0.m_bars[closure_0.m_thisMonthBarIndex];
        if (closure_0.m_maxValue.IsPositive)
          bar1.SetHeight<Panel>(((float) quantity.Value * 22f / (float) closure_0.m_maxValue.Value).Clamp(2f, 22f));
        else
          bar1.SetHeight<Panel>(2f);
        if (capacity.IsPositive)
          storageState.SetProgress((float) quantity.Value / (float) capacity.Max(quantity).Value);
        else if (quantity.IsPositive)
          storageState.SetProgress(Percent.Hundred);
        else
          storageState.SetProgress(Percent.Zero);
      }));
      this.Updater = updaterBuilder.Build();
      this.m_container.SetupDragDrop(new Action(this.onBeginDrag), new Action(this.onDrag), new Action(this.onEndDrag));
    }

    private void onBeginDrag() => this.m_owner.OnDragStart(this);

    private void onDrag()
    {
      this.m_owner.OnDragMove((Vector2) (this.m_container.RectTransform.position - new Vector3(0.0f, this.m_container.GetHeight() / 2f)));
    }

    private void onEndDrag() => this.m_owner.OnDragDone();

    public void RefreshDataNow()
    {
      this.OnNewMonth();
      this.updateBars();
    }

    public void OnNewMonth()
    {
      this.m_lastKnownData.Clear();
      this.m_stats.StoredQuantityTotalStats.GetLastNMonths(5, this.m_lastKnownData);
      this.m_lastKnownData.Reverse();
      this.m_thisMonthQuantity = this.m_stats.StoredAvailableQuantity;
      this.m_needsToUpdateBars = true;
    }

    public void RenderUpdate()
    {
      if (!this.m_needsToUpdateBars)
        return;
      this.updateBars();
    }

    private void updateBars()
    {
      this.m_needsToUpdateBars = false;
      Lyst<QuantityLarge> lastKnownData = this.m_lastKnownData;
      int num = this.m_lastKnownData.Count.Min(5);
      bool flag1 = false;
      bool flag2 = false;
      QuantityLarge quantityLarge1;
      if (num >= 1)
      {
        quantityLarge1 = lastKnownData[0];
        for (int index = 1; index < num; ++index)
          quantityLarge1 = quantityLarge1.Max(lastKnownData[index]);
        QuantityLarge quantityLarge2 = lastKnownData[0];
        QuantityLarge thisMonthQuantity = this.m_thisMonthQuantity;
        flag2 = quantityLarge2 > thisMonthQuantity && (double) thisMonthQuantity.Value / (double) quantityLarge2.Value < 0.85;
        flag1 = quantityLarge2 < thisMonthQuantity && (double) thisMonthQuantity.Value / (double) quantityLarge2.Value > 1.15;
        if (!flag2 && !flag1 && lastKnownData.Count > 1)
        {
          bool flag3 = true;
          for (int index = 1; index < lastKnownData.Count; ++index)
            flag3 &= lastKnownData[index - 1] > lastKnownData[index];
          flag2 = flag3;
        }
      }
      else
        quantityLarge1 = QuantityLarge.Zero;
      this.m_thisMonthBarIndex = num;
      this.m_maxValue = quantityLarge1.Max(this.m_thisMonthQuantity);
      ColorRgba color = !flag1 ? (!flag2 ? (ColorRgba) 9539985 : this.m_builder.Style.Global.DangerClr) : (ColorRgba) 4109125;
      for (int index = 0; index < num + 1; ++index)
      {
        Panel bar = this.m_bars[index];
        QuantityLarge quantityLarge3 = index == num ? this.m_thisMonthQuantity : lastKnownData[index];
        bar.SetBackground(color);
        if (this.m_maxValue.IsNotPositive)
          bar.SetHeight<Panel>(2f);
        else
          bar.SetHeight<Panel>(((float) quantityLarge3.Value * 22f / (float) this.m_maxValue.Value).Clamp(2f, 22f));
      }
      for (int index = num + 1; index < 6; ++index)
        this.m_bars[index].SetHeight<Panel>(0.0f);
    }
  }
}
