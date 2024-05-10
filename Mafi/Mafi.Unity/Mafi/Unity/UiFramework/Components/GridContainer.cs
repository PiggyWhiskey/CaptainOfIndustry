// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.GridContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  /// <summary>
  /// Container that organizes its items in form of a regular grid. Items are placed from top right.
  /// </summary>
  public class GridContainer : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly Panel m_container;
    private readonly Lyst<GridContainer.Item> m_items;
    private int m_rowsCount;
    private int m_columnsCount;
    private int m_visibleItemsCount;
    private Vector2 m_cellSize;
    private Vector2 m_cellSpacing;
    private GridContainer.SizeMode m_sizeMode;
    private bool m_postponeRecompute;
    private Offset m_containerPadding;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public int VisibleItemsCount => this.m_visibleItemsCount;

    public event Action<IUiElement> SizeChanged;

    public GridContainer(UiBuilder builder, string name, IUiElement parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_items = new Lyst<GridContainer.Item>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.m_container = new Panel(builder, name, parent?.GameObject);
      this.m_cellSize = new Vector2(50f, 50f);
      this.m_sizeMode = GridContainer.SizeMode.DynamicAll;
    }

    /// <summary>
    /// Computes what will be the width of the container if it would have the given amount of columns. This takes
    /// into account all the current paddings and spacings so use it once you provided all of that.
    /// </summary>
    public float ComputeWidthFor(int columnsCount)
    {
      float widthFor = this.m_containerPadding.LeftRightOffset + (float) columnsCount * this.m_cellSize.x;
      if (columnsCount > 1)
        widthFor += (float) (columnsCount - 1) * this.m_cellSpacing.x;
      return widthFor;
    }

    /// <summary>
    /// Computes what will be the height of the container if it would have the given amount of rows. This takes into
    /// account all the current paddings and spacings so use it once you provided all of that.
    /// </summary>
    public float ComputeHeightFor(int rowsCount)
    {
      float heightFor = this.m_containerPadding.TopBottomOffset + (float) rowsCount * this.m_cellSize.y;
      if (rowsCount > 1)
        heightFor += (float) (rowsCount - 1) * this.m_cellSpacing.y;
      return heightFor;
    }

    /// <summary>
    /// Returns the width this container needs under its current size mode.
    /// </summary>
    public float GetRequiredWidth()
    {
      return this.m_sizeMode == GridContainer.SizeMode.DynamicHeight ? this.ComputeWidthFor(this.m_columnsCount) : this.GetWidth();
    }

    /// <summary>
    /// Returns the height this container needs under its current size mode.
    /// </summary>
    public float GetRequiredHeight()
    {
      return this.m_sizeMode == GridContainer.SizeMode.DynamicWidth ? this.ComputeHeightFor(this.m_rowsCount) : this.GetHeight();
    }

    /// <summary>
    /// See <see cref="F:Mafi.Unity.UiFramework.Components.GridContainer.SizeMode.DynamicWidth" />. Set number of rows the contrainer should use. If you don't specify
    /// the number then the container will automatically fit its width.
    /// </summary>
    public GridContainer SetDynamicWidthMode(int rowsCount = -1)
    {
      this.m_sizeMode = GridContainer.SizeMode.DynamicWidth;
      this.m_rowsCount = rowsCount;
      this.recomputeLayout();
      return this;
    }

    /// <summary>
    /// See <see cref="F:Mafi.Unity.UiFramework.Components.GridContainer.SizeMode.DynamicHeight" />. Set number of columns the contrainer should use. If you don't
    /// specify the number then the container will automatically fit its height.
    /// </summary>
    public GridContainer SetDynamicHeightMode(int columnsCount = -1)
    {
      this.m_sizeMode = GridContainer.SizeMode.DynamicHeight;
      this.m_columnsCount = columnsCount;
      this.recomputeLayout();
      return this;
    }

    public GridContainer SetCellSize(Vector2 size)
    {
      this.m_cellSize = size;
      this.recomputeLayout();
      return this;
    }

    /// <summary>Default is zero.</summary>
    public GridContainer SetCellSpacing(Vector2 spacing)
    {
      this.m_cellSpacing = spacing;
      this.recomputeLayout();
      return this;
    }

    /// <summary>Default is zero.</summary>
    public GridContainer SetCellSpacing(float spacing)
    {
      this.m_cellSpacing = new Vector2(spacing, spacing);
      this.recomputeLayout();
      return this;
    }

    /// <summary>Default is zero.</summary>
    public GridContainer SetInnerPadding(Offset offset)
    {
      this.m_containerPadding = offset;
      this.recomputeLayout();
      return this;
    }

    public GridContainer SetBackground(ColorRgba color)
    {
      this.m_container.SetBackground(color);
      return this;
    }

    public void SetItemVisibility(IUiElement element, bool isVisible)
    {
      if (element.IsVisible() == isVisible)
        return;
      if (element.IsVisible())
      {
        element.Hide<IUiElement>();
        --this.m_visibleItemsCount;
        this.recomputeLayout();
      }
      else
      {
        element.Show<IUiElement>();
        ++this.m_visibleItemsCount;
        this.recomputeLayout();
      }
    }

    public void ShowItem(IUiElement element)
    {
      if (element.IsVisible())
        return;
      element.Show<IUiElement>();
      ++this.m_visibleItemsCount;
      this.recomputeLayout();
    }

    public void HideItem(IUiElement element)
    {
      if (!element.IsVisible())
        return;
      element.Hide<IUiElement>();
      --this.m_visibleItemsCount;
      this.recomputeLayout();
    }

    public void HideAllItems()
    {
      this.m_items.ForEach<IUiElement>((Func<GridContainer.Item, IUiElement>) (x => x.Element.Hide<IUiElement>()));
      this.m_visibleItemsCount = 0;
      this.recomputeLayout();
    }

    /// <summary>Adds new item to the end of this container.</summary>
    public void Append(IUiElement element)
    {
      this.m_items.Add(new GridContainer.Item()
      {
        Element = element
      });
      element.SetParent<IUiElement>((IUiElement) this, false);
      this.m_visibleItemsCount += element.IsVisible() ? 1 : 0;
      this.recomputeLayout();
    }

    /// <summary>
    /// Use this before running mass changes as it prevents running layout recomptutation for every single change.
    /// After you are done don't forget to call <see cref="M:Mafi.Unity.UiFramework.Components.GridContainer.FinishBatchOperation" />
    /// </summary>
    public void StartBatchOperation() => this.m_postponeRecompute = true;

    /// <summary>
    /// Request layout recomputation. Use this after <see cref="M:Mafi.Unity.UiFramework.Components.GridContainer.StartBatchOperation" />.
    /// </summary>
    public void FinishBatchOperation()
    {
      this.m_postponeRecompute = false;
      this.recomputeLayout();
    }

    private void recomputeLayout()
    {
      if (this.m_postponeRecompute)
        return;
      Assert.That<int>(this.m_items.Count).IsGreaterOrEqual(this.m_visibleItemsCount);
      int columnsCount = this.m_columnsCount;
      int rowsCount = this.m_rowsCount;
      if (columnsCount < 0)
        columnsCount = ((float) (((double) this.GetWidth() - (double) this.m_containerPadding.LeftRightOffset + (double) this.m_cellSpacing.x) / ((double) this.m_cellSize.x + (double) this.m_cellSpacing.x))).FloorToInt();
      if (rowsCount < 0)
        rowsCount = ((float) (((double) this.GetHeight() - (double) this.m_containerPadding.TopBottomOffset + (double) this.m_cellSpacing.y) / ((double) this.m_cellSize.y + (double) this.m_cellSpacing.y))).FloorToInt();
      if (rowsCount == 0 && columnsCount == 0)
        rowsCount = Mathf.Sqrt((float) this.m_items.Count).CeilToInt();
      if (columnsCount > 0 && rowsCount == 0)
        rowsCount = ((float) this.m_visibleItemsCount / (float) columnsCount).CeilToInt();
      if (rowsCount > 0 && columnsCount == 0)
        columnsCount = ((float) this.m_visibleItemsCount / (float) rowsCount).CeilToInt();
      int index1 = 0;
      int num = 0;
      for (int index2 = 0; index2 < rowsCount && num < this.m_visibleItemsCount; ++index2)
      {
        for (int index3 = 0; index3 < columnsCount && num < this.m_visibleItemsCount; ++index3)
        {
          while (!this.m_items[index1].Element.IsVisible())
            ++index1;
          this.m_items[index1].Element.PutToLeftTopOf<IUiElement>((IUiElement) this, this.m_cellSize, new Offset(0.0f, this.m_containerPadding.TopOffset + (float) index2 * (this.m_cellSize.y + this.m_cellSpacing.y), this.m_containerPadding.LeftOffset + (float) index3 * (this.m_cellSize.x + this.m_cellSpacing.x), 0.0f));
          ++num;
          ++index1;
        }
      }
      if (this.m_sizeMode == GridContainer.SizeMode.DynamicHeight)
        this.SetHeight<GridContainer>(this.ComputeHeightFor(rowsCount));
      else if (this.m_sizeMode == GridContainer.SizeMode.DynamicWidth)
      {
        this.SetWidth<GridContainer>(this.ComputeWidthFor(columnsCount));
      }
      else
      {
        this.SetWidth<GridContainer>(this.ComputeWidthFor(columnsCount));
        this.SetHeight<GridContainer>(this.ComputeHeightFor(rowsCount));
      }
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    public void ClearAllAndDestroy()
    {
      this.m_items.ForEachAndClear((Action<GridContainer.Item>) (item => item.Element.Destroy()));
      this.m_visibleItemsCount = 0;
      this.recomputeLayout();
    }

    public void ClearAll()
    {
      this.m_items.Clear();
      this.m_visibleItemsCount = 0;
      this.recomputeLayout();
    }

    public void ClearAllAndHide()
    {
      this.m_items.ForEachAndClear<IUiElement>((Func<GridContainer.Item, IUiElement>) (x => x.Element.Hide<IUiElement>()));
      this.m_visibleItemsCount = 0;
      this.recomputeLayout();
    }

    private struct Item
    {
      public IUiElement Element;
    }

    public enum SizeMode
    {
      /// <summary>The container grows to both sides automatically.</summary>
      DynamicAll,
      /// <summary>
      /// The container has fixed height determined by the given number of rows.
      /// </summary>
      DynamicWidth,
      /// <summary>
      /// The container has fixed width determined by the given number of columns.
      /// </summary>
      DynamicHeight,
    }
  }
}
