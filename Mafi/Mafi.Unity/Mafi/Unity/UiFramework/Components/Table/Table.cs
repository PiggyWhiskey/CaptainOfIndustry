// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.Table
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  public class Table : IUiElement
  {
    private bool m_columnsBuilt;
    private TextStyle m_headerStyle;
    private Option<Action<int>> m_onItemSelected;
    private Option<Action<int>> m_onItemDoubleClicked;
    private int m_selectedIndex;
    private readonly Lyst<IColumn> m_columns;
    private readonly Lyst<Btn> m_rows;
    private readonly Panel m_mainContainer;
    private readonly StackContainer m_container;
    private readonly Panel m_headers;
    private readonly UiStyle m_style;
    private readonly Panel m_dividers;
    private int m_minWidth;
    private int m_rowHeight;

    public UiBuilder Builder { get; private set; }

    public GameObject GameObject => this.m_mainContainer.GameObject;

    public RectTransform RectTransform => this.m_mainContainer.RectTransform;

    public int RowsCount => this.m_rows.Count;

    public int ColumnsCount => this.m_columns.Count;

    public int SelectedRowIndex => this.m_selectedIndex;

    private BtnStyle m_rowStyleHighlighted
    {
      get
      {
        return new BtnStyle()
        {
          BackgroundClr = new ColorRgba?(this.m_style.Panel.ItemOverlayDark),
          HoveredMaskClr = new ColorRgba?((ColorRgba) 14935011)
        };
      }
    }

    private BtnStyle m_rowStyle
    {
      get
      {
        return new BtnStyle()
        {
          BackgroundClr = new ColorRgba?(this.m_style.Panel.ItemOverlay),
          HoveredMaskClr = new ColorRgba?((ColorRgba) 14935011)
        };
      }
    }

    private BtnStyle m_rowSelectedStyle
    {
      get
      {
        BtnStyle mRowStyle = this.m_rowStyle;
        ref BtnStyle local = ref mRowStyle;
        ColorRgba? nullable = new ColorRgba?((ColorRgba) 1842204);
        TextStyle? text = new TextStyle?();
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable;
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
        ColorRgba? pressedClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = new Offset?();
        return local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      }
    }

    public Table(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_selectedIndex = -1;
      this.m_columns = new Lyst<IColumn>();
      this.m_rows = new Lyst<Btn>();
      this.m_rowHeight = 24;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Builder = builder;
      this.m_style = builder.Style;
      this.m_headerStyle = this.m_style.Statistics.TableHeaderTextStyle;
      this.m_mainContainer = builder.NewPanel(nameof (Table));
      this.m_headers = builder.NewPanel("Headers").SetBackground(new ColorRgba(0, 0)).PutToTopOf<Panel>((IUiElement) this.m_mainContainer, 26f, Offset.Top(6f));
      this.m_container = builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetInnerPadding(Offset.Top(2f)).SetItemSpacing(1f).PutToTopOf<StackContainer>((IUiElement) this.m_mainContainer, 0.0f, Offset.Top(32f));
      this.m_dividers = builder.NewPanel("Dividers container").PutToTopOf<Panel>((IUiElement) this.m_mainContainer, 0.0f, Offset.Top(10f));
    }

    public Mafi.Unity.UiFramework.Components.Table.Table SetOnItemSelected(
      Action<int> onItemSelected)
    {
      this.m_onItemSelected = (Option<Action<int>>) onItemSelected;
      return this;
    }

    public Mafi.Unity.UiFramework.Components.Table.Table SetOnItemDoubleClicked(
      Action<int> onDoubleClick)
    {
      this.m_onItemDoubleClicked = (Option<Action<int>>) onDoubleClick;
      return this;
    }

    public Mafi.Unity.UiFramework.Components.Table.Table UseDarkHeaders()
    {
      this.m_headers.SetBackground(this.m_style.Panel.ItemOverlayDark);
      return this;
    }

    public Mafi.Unity.UiFramework.Components.Table.Table SetHeadersStyle(TextStyle headersStyle)
    {
      Assert.That<bool>(this.m_columnsBuilt).IsFalse("Headers already built!");
      this.m_headerStyle = headersStyle;
      return this;
    }

    public Mafi.Unity.UiFramework.Components.Table.Table SetCustomRowHeight(int rowHeight)
    {
      this.m_rowHeight = rowHeight;
      return this;
    }

    /// <summary>
    /// Don't add any new columns once you added a row or request a min width!
    /// </summary>
    public Mafi.Unity.UiFramework.Components.Table.Table AddColumn(IColumn column)
    {
      Assert.That<bool>(this.m_columnsBuilt).IsFalse("Cannot add columns after a row was addedt!");
      this.m_columns.Add(column);
      return this;
    }

    private void buildHeaders()
    {
      UiStyle style = this.Builder.Style;
      int[] numArray = new int[this.m_columns.Count];
      string[] strArray = new string[this.m_columns.Count];
      int index1 = 0;
      foreach (IColumn column in this.m_columns)
      {
        if (column.MergeWithPrevious)
        {
          numArray[index1 - 1] += column.Width;
        }
        else
        {
          numArray[index1] = column.Width;
          strArray[index1] = column.Title;
          ++index1;
        }
      }
      int leftOffset = 5;
      for (int index2 = 0; index2 < index1; ++index2)
      {
        if (index2 > 0)
        {
          this.Builder.NewPanel("Divider").SetBackground(style.Statistics.TableDividersColor).PutToLeftOf<Panel>((IUiElement) this.m_dividers, 2f, Offset.Left((float) (leftOffset + 6)));
          leftOffset += 12;
        }
        this.Builder.NewTxt("Header " + index2.ToString()).SetText(strArray[index2]).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.m_headerStyle).PutToLeftOf<Txt>((IUiElement) this.m_headers, (float) numArray[index2], Offset.Left((float) leftOffset));
        leftOffset += numArray[index2];
      }
      this.m_minWidth = leftOffset + 6;
    }

    private void handleItemSelected(int index)
    {
      Assert.That<Option<Action<int>>>(this.m_onItemSelected).HasValue<Action<int>>();
      Assert.That<int>(index).IsWithinExcl(0, this.m_rows.Count);
      if (index == this.m_selectedIndex)
        return;
      this.ClearCurrentSelection();
      this.m_selectedIndex = index;
      this.m_rows[this.m_selectedIndex].SetButtonStyle(this.m_rowSelectedStyle);
      this.m_onItemSelected.Value(index);
    }

    public void SelectRow(int rowIndex)
    {
      if (rowIndex < 0 || rowIndex >= this.m_rows.Count)
        this.ClearCurrentSelection();
      if (rowIndex == this.m_selectedIndex)
        return;
      this.ClearCurrentSelection();
      this.m_selectedIndex = rowIndex;
      this.m_rows[this.m_selectedIndex].SetButtonStyle(this.m_rowSelectedStyle);
      this.m_onItemSelected.Value(rowIndex);
    }

    public void ClearCurrentSelection()
    {
      if (this.m_selectedIndex >= 0)
        this.m_rows[this.m_selectedIndex].SetButtonStyle(this.m_rowStyle);
      this.m_selectedIndex = -1;
    }

    /// <summary>All columns must be added before calling this!</summary>
    public void AddRows(int number, bool highlightLast = false)
    {
      for (int index = 0; index < number; ++index)
        this.AddRow(highlight: highlightLast && index + 1 == number);
    }

    /// <summary>All columns must be added before calling this!</summary>
    public void AddRow(Action onClick = null, bool highlight = false, int height = 0)
    {
      if (!this.m_columnsBuilt)
      {
        this.m_columnsBuilt = true;
        this.buildHeaders();
      }
      int size = height > 0 ? height : this.m_rowHeight;
      this.m_columns.ForEach((Action<IColumn>) (x => x.AddRow(highlight)));
      int rowIndex = this.m_rows.Count;
      Btn btn = this.Builder.NewBtn("Row " + rowIndex.ToString()).SetButtonStyle(highlight ? this.m_rowStyleHighlighted : this.m_rowStyle).AddTo<Btn>(this.m_container, (float) rowIndex, (float) size);
      this.m_rows.Add(btn);
      if (onClick != null)
        btn.OnClick(onClick);
      else if (this.m_onItemSelected.HasValue)
        btn.OnClick((Action) (() => this.handleItemSelected(rowIndex)));
      if (this.m_onItemDoubleClicked.HasValue)
        btn.OnDoubleClick((Action) (() => this.m_onItemDoubleClicked.Value(rowIndex)));
      int leftOffset = 5;
      foreach (IColumn column in this.m_columns)
      {
        GameObject gameObject = column.GetGameObject(rowIndex);
        if (!column.MergeWithPrevious && leftOffset > 5)
          leftOffset += 12;
        gameObject.PutToLeftOf(btn.GameObject, (float) column.Width, column.InnerOffset() + Offset.Left((float) leftOffset));
        leftOffset += column.Width;
      }
      this.updateDividersHeight();
    }

    public void ApplyVisibilityMap(BitArray rowsVisibilityMap)
    {
      Assert.That<int>(rowsVisibilityMap.Length).IsEqualTo(this.m_rows.Count);
      this.m_container.StartBatchOperation();
      for (int index = 0; index < this.m_rows.Count; ++index)
        this.m_container.SetItemVisibility((IUiElement) this.m_rows[index], rowsVisibilityMap[index]);
      this.m_container.FinishBatchOperation();
    }

    public void RemoveAllRows()
    {
      this.ClearCurrentSelection();
      while (!this.m_rows.IsEmpty)
      {
        this.m_columns.ForEach((Action<IColumn>) (x => x.RemoveLastRow()));
        this.m_rows.RemoveAt(this.m_rows.Count - 1);
      }
      this.m_container.ClearAndDestroyAll();
      this.updateDividersHeight();
    }

    public void RemoveLastRow()
    {
      this.ClearCurrentSelection();
      if (this.m_rows.IsEmpty)
        return;
      this.m_columns.ForEach((Action<IColumn>) (x => x.RemoveLastRow()));
      this.m_rows.RemoveAt(this.m_rows.Count - 1);
      this.m_container.RemoveAndDestroy((IUiElement) this.m_rows.Last);
      this.updateDividersHeight();
    }

    private void updateDividersHeight()
    {
      this.SetHeight<Mafi.Unity.UiFramework.Components.Table.Table>((float) ((double) this.m_container.GetDynamicHeight() + 26.0 + 6.0));
      this.m_dividers.SetHeight<Panel>(this.GetHeight() - 10f);
    }

    public int GetMinWidth()
    {
      if (!this.m_columnsBuilt)
        this.buildHeaders();
      return this.m_minWidth;
    }

    public Vector2 GetMinDimensions() => new Vector2((float) this.GetMinWidth(), this.GetHeight());
  }
}
