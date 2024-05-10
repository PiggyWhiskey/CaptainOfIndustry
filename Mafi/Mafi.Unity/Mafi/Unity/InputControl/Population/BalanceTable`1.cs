// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.BalanceTable`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  internal class BalanceTable<T> : IDynamicSizeElement, IUiElement
  {
    private readonly Panel m_container;
    private readonly StackContainer m_positivesContainer;
    private readonly StackContainer m_negativesContainer;
    private readonly ViewsCacheHomogeneous<BalanceInfoView> m_unityInfoViewsCache;
    private readonly BalanceInfoView m_positiveTotal;
    private readonly BalanceInfoView m_negativeTotal;
    private readonly BalanceInfoView m_total;
    private readonly UiBuilder m_builder;
    private readonly Func<T, T, T> m_sumFunc;
    private readonly Func<T, string> m_toStringFunc;
    private readonly Func<T, bool> m_isAboveZero;
    private readonly T m_zero;
    private T m_positivesSum;
    private T m_positivesMaxSum;
    private T m_negativesSum;
    private T m_negativesMaxSum;
    private readonly int m_widthAvailable;
    private readonly Panel m_positiveEmpty;
    private readonly Panel m_negativeEmpty;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public BalanceTable(
      UiBuilder builder,
      Func<T, T, T> sumFunc,
      Func<T, string> toStringFunc,
      Func<T, bool> isAboveZero,
      T zero,
      int widthAvailable,
      string entryIcon,
      int textWidth)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      BalanceTable<T> balanceTable = this;
      this.m_builder = builder;
      this.m_sumFunc = sumFunc;
      this.m_toStringFunc = toStringFunc;
      this.m_isAboveZero = isAboveZero;
      this.m_zero = zero;
      this.m_positivesSum = this.m_zero;
      this.m_negativesSum = this.m_zero;
      this.m_widthAvailable = widthAvailable;
      this.m_unityInfoViewsCache = new ViewsCacheHomogeneous<BalanceInfoView>((Func<BalanceInfoView>) (() => new BalanceInfoView(builder, entryIcon, textWidth)));
      this.m_total = new BalanceInfoView(builder, entryIcon, textWidth);
      this.m_total.SetTitle("");
      Txt objectToPlace = builder.NewTxt("Total").SetText(Tr.Total.TranslatedString).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(builder.Style.Global.Title);
      float preferedWidth = objectToPlace.GetPreferedWidth();
      objectToPlace.PutToLeftOf<Txt>((IUiElement) this.m_total, preferedWidth, Offset.Left((float) (-(double) preferedWidth - 10.0)));
      this.m_container = builder.NewPanel("Container");
      int columnWidth = widthAvailable / 2 - 5;
      this.m_positivesContainer = builder.NewStackContainer("PositivesContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(2f).PutToLeftTopOf<StackContainer>((IUiElement) this.m_container, new Vector2((float) columnWidth, 0.0f));
      this.m_negativesContainer = builder.NewStackContainer("NegativesContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(2f).PutToLeftTopOf<StackContainer>((IUiElement) this.m_container, new Vector2((float) columnWidth, 0.0f), Offset.Left((float) (columnWidth + 5)));
      this.m_positiveTotal = createSumView(true);
      this.m_negativeTotal = createSumView(false);
      this.m_positiveEmpty = createEmptyView();
      this.m_negativeEmpty = createEmptyView();
      this.m_positivesContainer.SizeChanged += new Action<IUiElement>(containerSizeChanged);
      this.m_negativesContainer.SizeChanged += new Action<IUiElement>(containerSizeChanged);

      BalanceInfoView createSumView(bool isPositive)
      {
        BalanceInfoView objectToPlace = new BalanceInfoView(builder, entryIcon, textWidth);
        objectToPlace.SetTitle("");
        objectToPlace.SetTransparentBg();
        objectToPlace.PutToLeftTopOf<BalanceInfoView>((IUiElement) balanceTable.m_container, new Vector2((float) columnWidth, 35f), Offset.Top(-35f) + Offset.Left(isPositive ? 0.0f : (float) (columnWidth + 5)));
        if (isPositive)
          objectToPlace.SetPositiveClr();
        else
          objectToPlace.SetCriticalClr();
        return objectToPlace;
      }

      Panel createEmptyView()
      {
        Panel emptyView = builder.NewPanel("Empty");
        emptyView.SetBackground(builder.Style.Panel.ItemOverlay);
        emptyView.PutToLeftTopOf<Panel>((IUiElement) balanceTable.m_container, Vector2.zero);
        builder.NewTxt("Empty").PutTo<Txt>((IUiElement) emptyView).SetText("-").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(builder.Style.Global.Text);
        return emptyView;
      }

      void containerSizeChanged(IUiElement element)
      {
        balanceTable.m_container.SetHeight<Panel>(balanceTable.m_positivesContainer.GetDynamicHeight().Max(balanceTable.m_negativesContainer.GetDynamicHeight()));
        Action<IUiElement> sizeChanged = balanceTable.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) balanceTable);
      }
    }

    public void StartBatchEdits()
    {
      this.m_unityInfoViewsCache.ReturnAll();
      this.m_positivesContainer.ClearAll();
      this.m_negativesContainer.ClearAll();
      this.m_positivesContainer.StartBatchOperation();
      this.m_negativesContainer.StartBatchOperation();
      this.m_positivesSum = this.m_zero;
      this.m_positivesMaxSum = this.m_zero;
      this.m_negativesSum = this.m_zero;
      this.m_negativesMaxSum = this.m_zero;
    }

    public void FinishBatchEdits()
    {
      this.m_positiveTotal.SetValue(this.m_toStringFunc(this.m_positivesSum), this.m_toStringFunc(this.m_positivesMaxSum));
      this.m_negativeTotal.SetValue(this.m_toStringFunc(this.m_negativesSum), this.m_toStringFunc(this.m_negativesMaxSum));
      if (this.m_positivesSum.Equals((object) this.m_zero))
        this.m_positiveTotal.SetWarningClr();
      else
        this.m_positiveTotal.SetPositiveClr();
      int num = this.m_positivesContainer.ItemsCount + this.m_negativesContainer.ItemsCount;
      bool visibility = num == 0 || num > 1;
      this.m_positiveTotal.SetVisibility<BalanceInfoView>(this.m_positivesContainer.ItemsCount > 1);
      this.m_positiveEmpty.SetVisibility<Panel>(this.m_positivesContainer.ItemsCount == 0 && this.m_negativesContainer.ItemsCount > 0);
      if (this.m_positiveEmpty.IsVisible())
        this.m_positivesContainer.Append((IUiElement) this.m_positiveEmpty, new float?(30f));
      this.m_negativeTotal.SetVisibility<BalanceInfoView>(this.m_negativesContainer.ItemsCount > 1);
      this.m_negativeEmpty.SetVisibility<Panel>(this.m_negativesContainer.ItemsCount == 0 && this.m_positivesContainer.ItemsCount > 0);
      if (this.m_negativeEmpty.IsVisible())
        this.m_negativesContainer.Append((IUiElement) this.m_negativeEmpty, new float?(30f));
      if (visibility)
      {
        T obj = this.m_sumFunc(this.m_positivesSum, this.m_negativesSum);
        this.m_total.SetValue(this.m_toStringFunc(obj));
        if (this.m_isAboveZero(obj) || obj.Equals((object) this.m_zero))
        {
          this.m_positivesContainer.Append((IUiElement) this.m_total, new Vector2?(new Vector2(200f, 30f)), new ContainerPosition?(ContainerPosition.RightOrBottom));
          if (this.m_isAboveZero(obj))
            this.m_total.SetPositiveClr();
          else if (obj.Equals((object) this.m_zero))
            this.m_total.SetWarningClr();
        }
        else
        {
          this.m_negativesContainer.Append((IUiElement) this.m_total, new Vector2?(new Vector2(200f, 30f)), new ContainerPosition?(ContainerPosition.RightOrBottom));
          this.m_total.SetCriticalClr();
        }
      }
      this.m_total.SetVisibility<BalanceInfoView>(visibility);
      this.m_positivesContainer.FinishBatchOperation();
      this.m_negativesContainer.FinishBatchOperation();
    }

    public void AddItem(
      string title,
      T value,
      T max,
      string iconPath,
      string tooltip,
      bool isPositive)
    {
      BalanceInfoView view = this.m_unityInfoViewsCache.GetView();
      if (value.Equals((object) max))
        view.SetValue(this.m_toStringFunc(value));
      else
        view.SetValue(this.m_toStringFunc(value), this.m_toStringFunc(max));
      view.SetTitle(title);
      view.SetIcon(iconPath);
      view.SetTooltip(tooltip);
      if (isPositive)
      {
        this.m_positivesContainer.Append((IUiElement) view, new float?(30f));
        this.m_positivesSum = this.m_sumFunc(this.m_positivesSum, value);
        this.m_positivesMaxSum = this.m_sumFunc(this.m_positivesMaxSum, max);
        if (value.Equals((object) this.m_zero) || !value.Equals((object) max))
          view.SetWarningClr();
        else
          view.SetPositiveClr();
      }
      else
      {
        this.m_negativesContainer.Append((IUiElement) view, new float?(30f));
        this.m_negativesSum = this.m_sumFunc(this.m_negativesSum, value);
        this.m_negativesMaxSum = this.m_sumFunc(this.m_negativesMaxSum, max);
        view.SetCriticalClr();
      }
    }
  }
}
