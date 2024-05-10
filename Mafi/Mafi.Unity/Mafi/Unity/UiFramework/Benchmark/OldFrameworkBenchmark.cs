// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Benchmark.OldFrameworkBenchmark
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Benchmark
{
  public class OldFrameworkBenchmark : WindowView
  {
    public int LabelsCnt;
    public int RectsCnt;
    private Vector2 m_windowSize;
    private readonly int m_topBoxesCount;
    private readonly int m_nestingLevel;
    private readonly bool m_drawBorders;
    private readonly bool m_addLabels;
    private readonly Lyst<Txt> m_labels;
    private readonly Lyst<Panel> m_rects;
    private Panel m_firstBox;
    private int m_shuffleTextIteration;
    private int m_shuffleColorsIteration;

    public OldFrameworkBenchmark(
      Vector2 windowSize,
      int topBoxesCount,
      int nestingLevel,
      bool drawBorders,
      bool addLabels)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_labels = new Lyst<Txt>();
      this.m_rects = new Lyst<Panel>();
      // ISSUE: explicit constructor call
      base.\u002Ector("Bench");
      this.m_windowSize = windowSize;
      this.m_topBoxesCount = topBoxesCount;
      this.m_nestingLevel = nestingLevel;
      this.m_drawBorders = drawBorders;
      this.m_addLabels = addLabels;
    }

    protected override void BuildWindowContent()
    {
      UiBuilder builder = this.Builder;
      int width = (int) this.m_windowSize.x;
      this.SetContentSize((float) width, this.m_windowSize.y);
      this.PositionSelfToCenter();
      ScrollableContainer scrollableContainer = builder.NewScrollableContainer("ScrollableContainer", (IUiElement) this.GetContentPanel()).AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel());
      Panel innerContainer = builder.NewPanel("InnerContainer");
      scrollableContainer.AddItemTop((IUiElement) innerContainer);
      scrollableContainer.SetContentToScroll((IUiElement) innerContainer);
      StackContainer column = this.Builder.NewStackContainer("Column", (IUiElement) innerContainer).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutToTopOf<StackContainer>((IUiElement) innerContainer, 0.0f);
      StackContainer leftTopOf = this.Builder.NewStackContainer("Btns").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutToLeftTopOf<StackContainer>((IUiElement) this, new Vector2(180f, 0.0f), Offset.Left(-200f));
      this.Builder.NewBtnPrimary("Btn").SetText("Shuffle text").OnClick(new Action(this.shuffleLabels)).AppendTo<Btn>(leftTopOf, new float?(25f));
      this.Builder.NewBtnPrimary("Btn").SetText("Shuffle colors").OnClick(new Action(this.shuffleColors)).AppendTo<Btn>(leftTopOf, new float?(25f));
      this.Builder.NewBtnPrimary("Btn").SetText("(Un)hide 1. box").OnClick((Action) (() => this.m_firstBox.SetVisibility<Panel>(!this.m_firstBox.IsVisible()))).AppendTo<Btn>(leftTopOf, new float?(25f));
      this.Builder.NewBtnPrimary("Btn").SetText("(Un)hide content").OnClick((Action) (() => scrollableContainer.SetVisibility<ScrollableContainer>(!scrollableContainer.IsVisible()))).AppendTo<Btn>(leftTopOf, new float?(25f));
      this.Builder.NewBtnPrimary("Btn").SetText("Resize window").OnClick((Action) (() =>
      {
        this.m_windowSize -= new Vector2(50f, 50f);
        this.SetContentSize(this.m_windowSize.x, this.m_windowSize.y);
      })).AppendTo<Btn>(leftTopOf, new float?(25f));
      bool mimicReParentingBug = false;
      benchmark(this.m_topBoxesCount, this.m_nestingLevel);

      void benchmark(int topBoxesCount, int nestingLevel)
      {
        int boxSize = nestingLevel * 120;
        int num1 = ((float) width / (float) boxSize).RoundToInt();
        int num2 = topBoxesCount / num1;
        for (int index1 = 0; index1 < num2; ++index1)
        {
          StackContainer stackContainer = this.Builder.NewStackContainer("Row", mimicReParentingBug ? (IUiElement) null : (IUiElement) column).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned);
          if (!mimicReParentingBug)
            stackContainer.AppendTo<StackContainer>(column, new float?((float) boxSize));
          for (int index2 = 0; index2 < num1; ++index2)
          {
            IUiElement boxesInto = createBoxesInto((IUiElement) stackContainer, 0, nestingLevel, (float) boxSize);
            stackContainer.Append(boxesInto, new float?((float) boxSize));
          }
          if (mimicReParentingBug)
            stackContainer.AppendTo<StackContainer>(column, new float?((float) boxSize));
        }
        innerContainer.SetHeight<Panel>(column.GetDynamicHeight());
      }

      IUiElement createBoxesInto(
        IUiElement parent,
        int nestLevel,
        int maxNestLevel,
        float boxSize)
      {
        float num = (float) (((double) boxSize - 10.0) / 2.0);
        Panel parent1 = builder.NewPanel("Row", parent).PutTo<Panel>(parent, Offset.All(5f));
        Panel leftOf = builder.NewPanel("Column1", (IUiElement) parent1).PutToLeftOf<Panel>((IUiElement) parent1, num);
        Panel rightOf = builder.NewPanel("Column2", (IUiElement) parent1).PutToRightOf<Panel>((IUiElement) parent1, num);
        Panel topOf1 = builder.NewPanel("Row1", (IUiElement) leftOf).SetBackground(ColorRgba.Red.SetA((byte) 80)).PutToTopOf<Panel>((IUiElement) leftOf, num);
        Panel bottomOf1 = builder.NewPanel("Row2", (IUiElement) leftOf).SetBackground(ColorRgba.Green.SetA((byte) 80)).PutToBottomOf<Panel>((IUiElement) leftOf, num);
        Panel topOf2 = builder.NewPanel("Row3", (IUiElement) rightOf).SetBackground(ColorRgba.Blue.SetA((byte) 80)).PutToTopOf<Panel>((IUiElement) rightOf, num);
        Panel bottomOf2 = builder.NewPanel("Row4", (IUiElement) rightOf).SetBackground(ColorRgba.Orange.SetA((byte) 80)).PutToBottomOf<Panel>((IUiElement) rightOf, num);
        if (this.m_drawBorders)
        {
          topOf1.SetBorderStyle(new BorderStyle(ColorRgba.Black));
          bottomOf1.SetBorderStyle(new BorderStyle(ColorRgba.Black));
          topOf2.SetBorderStyle(new BorderStyle(ColorRgba.Black));
          bottomOf2.SetBorderStyle(new BorderStyle(ColorRgba.Black));
        }
        if (this.m_firstBox == null)
          this.m_firstBox = topOf1;
        this.RectsCnt += 7;
        ++nestLevel;
        if (nestLevel < maxNestLevel)
        {
          createBoxesInto((IUiElement) topOf1, nestLevel, maxNestLevel, num);
          createBoxesInto((IUiElement) bottomOf1, nestLevel, maxNestLevel, num);
          createBoxesInto((IUiElement) topOf2, nestLevel, maxNestLevel, num);
          createBoxesInto((IUiElement) bottomOf2, nestLevel, maxNestLevel, num);
        }
        else
        {
          if (this.m_addLabels)
          {
            createLabel("A", (IUiElement) topOf1);
            createLabel("C", (IUiElement) topOf2);
          }
          this.m_rects.Add(bottomOf2);
        }
        return (IUiElement) parent1;
      }

      Txt createLabel(string text, IUiElement element)
      {
        Txt label = builder.NewTxt("Txt").SetText(text).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(builder.Style.Global.Text).SetColor(ColorRgba.Black).PutTo<Txt>(element, Offset.All(2f));
        ++this.LabelsCnt;
        if (this.LabelsCnt % 7 == 0)
          this.m_labels.Add(label);
        return label;
      }
    }

    private void shuffleLabels()
    {
      ++this.m_shuffleTextIteration;
      string text = this.m_shuffleTextIteration % 2 == 0 ? "X" : "XY";
      foreach (Txt label in this.m_labels)
        label.SetText(text);
    }

    private void shuffleColors()
    {
      ++this.m_shuffleColorsIteration;
      ColorRgba color = this.m_shuffleColorsIteration % 2 == 0 ? ColorRgba.Magenta : ColorRgba.Gold;
      foreach (Panel rect in this.m_rects)
        rect.SetBackground(color);
    }
  }
}
