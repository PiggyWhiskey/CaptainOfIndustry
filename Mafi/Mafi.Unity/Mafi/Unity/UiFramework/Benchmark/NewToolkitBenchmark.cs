// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Benchmark.NewToolkitBenchmark
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Benchmark
{
  internal class NewToolkitBenchmark
  {
    private readonly UiBuilder m_builder;
    private Vector2 m_windowSize;
    private readonly int m_topBoxesCount;
    private readonly int m_nestingLevel;
    private readonly bool m_drawBorders;
    private readonly bool m_addLabels;
    public int LabelsCnt;
    public int RectsCnt;
    private readonly Lyst<Label> m_labels;
    private readonly Lyst<Row> m_rects;
    private Row m_firstBox;
    private Column m_firstParent;
    private int m_shuffleTextIteration;
    private int m_shuffleColorsIteration;

    private bool m_isFirstRemoved { get; set; }

    private bool m_isAllRemoved { get; set; }

    public NewToolkitBenchmark(
      UiBuilder builder,
      Vector2 windowSize,
      int topBoxesCount,
      int nestingLevel,
      bool drawBorders,
      bool addLabels)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_labels = new Lyst<Label>();
      this.m_rects = new Lyst<Row>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_windowSize = windowSize;
      this.m_topBoxesCount = topBoxesCount;
      this.m_nestingLevel = nestingLevel;
      this.m_drawBorders = drawBorders;
      this.m_addLabels = addLabels;
    }

    public void CreateHierarchy(bool useCss)
    {
      int width = (int) this.m_windowSize.x;
      Mafi.Unity.UiToolkit.Library.Window window1 = new Mafi.Unity.UiToolkit.Library.Window((LocStrFormatted) LocStr.Empty);
      ScrollColumn component1 = new ScrollColumn();
      component1.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Border<ScrollColumn>(1).BorderColor<ScrollColumn>((ColorRgba) 2105376).AlignSelf<ScrollColumn>(Align.Stretch)));
      Column component2 = new Column();
      component2.Add<Column>((Action<Column>) (c => c.Fill<Column>().AlignItemsStart<Column>()));
      Column child1 = component2;
      Column column = component2;
      component1.Add((UiComponent) child1);
      ScrollColumn child2 = component1;
      ScrollColumn scroll = component1;
      window1.Add((UiComponent) child2);
      Mafi.Unity.UiToolkit.Library.Window window = window1;
      window.Size<Mafi.Unity.UiToolkit.Library.Window>(width.px(), this.m_windowSize.y.px());
      window.MinWidth<Mafi.Unity.UiToolkit.Library.Window>(new Px?(500.px()));
      window.MinHeight<Mafi.Unity.UiToolkit.Library.Window>(new Px?(500.px()));
      Column component3 = new Column();
      component3.Add<Column>((Action<Column>) (c =>
      {
        Column component4 = c;
        Px? top = new Px?(50.px());
        Px? nullable = new Px?(50.px());
        Px? right = new Px?();
        Px? bottom = new Px?();
        Px? left = nullable;
        component4.AbsolutePosition<Column>(top, right, bottom, left).Height<Column>(new Px?(245.px())).Width<Column>(new Px?(160.px()));
      }));
      component3.Add((UiComponent) createBtn("Shuffle text", new Action(this.shuffleLabels)));
      component3.Add((UiComponent) createBtn("Shuffle colors", new Action(this.shuffleColors)));
      component3.Add((UiComponent) createBtn("Hide 1. box", (Action) (() => this.m_firstBox.SetVisible(!this.m_firstBox.IsVisible()))));
      component3.Add((UiComponent) createBtn("Remove 1. box", (Action) (() =>
      {
        if (this.m_isFirstRemoved)
          this.m_firstParent.Add((UiComponent) this.m_firstBox);
        else
          this.m_firstBox.RemoveFromHierarchy();
        this.m_isFirstRemoved = !this.m_isFirstRemoved;
      })));
      component3.Add((UiComponent) createBtn("Hide content", (Action) (() => scroll.SetVisible(!scroll.IsVisible()))));
      component3.Add((UiComponent) createBtn("Remove content", (Action) (() =>
      {
        if (this.m_isAllRemoved)
          window.Add((UiComponent) scroll);
        else
          scroll.RemoveFromHierarchy();
        this.m_isAllRemoved = !this.m_isAllRemoved;
      })));
      component3.Add((UiComponent) createBtn("Resize window", (Action) (() =>
      {
        this.m_windowSize -= new Vector2(50f, 50f);
        window.Size<Mafi.Unity.UiToolkit.Library.Window>(this.m_windowSize.x.px(), this.m_windowSize.y.px());
      })));
      Column btns = component3;
      benchmark(this.m_topBoxesCount, this.m_nestingLevel);
      this.m_builder.AddComponent((UiComponent) window);
      this.m_builder.AddComponent((UiComponent) btns);
      window.OnHide((Action) (() => btns.RemoveFromHierarchy()));

      static Button createBtn(string title, Action action)
      {
        ButtonText component = new ButtonText(title.AsLoc(), action);
        component.Add<ButtonText>((Action<ButtonText>) (c => c.Background<ButtonText>(new ColorRgba?(ColorRgba.Black)).Size<ButtonText>(160.px(), 30.px()).MarginTop<ButtonText>(1.px())));
        return (Button) component;
      }

      void benchmark(int topBoxesCount, int nestingLevel)
      {
        int boxSize = nestingLevel * 120;
        int num1 = ((float) width / (float) boxSize).RoundToInt();
        int num2 = topBoxesCount / num1;
        for (int index1 = 0; index1 < num2; ++index1)
        {
          Row component = new Row();
          component.Add<Row>((Action<Row>) (c => c.Height<Row>(new Px?(boxSize.px())).AlignSelf<Row>(Align.Stretch)));
          Row row = component;
          column.Add((UiComponent) row);
          for (int index2 = 0; index2 < num1; ++index2)
            createBoxesInto(row, 0, nestingLevel);
        }
      }

      void createBoxesInto(Row addTo, int nestLevel, int maxNestLevel)
      {
        Row component = new Row();
        component.Add<Row>((Action<Row>) (c =>
        {
          if (useCss)
            c.Class<Row>("bench-row");
          else
            c.Margin<Row>(1.pt()).Fill<Row>().AlignSelf<Row>(Align.Stretch);
        }));
        Column column1 = new Column();
        column1.Add<Column>((Action<Column>) (c =>
        {
          if (useCss)
            c.Class<Column>("bench-col");
          else
            c.Width<Column>(50.Percent()).AlignSelf<Column>(Align.Stretch);
        }));
        Row row1 = new Row();
        row1.Add<Row>((Action<Row>) (c =>
        {
          if (useCss)
            c.Class<Row>(this.m_drawBorders ? "bench-row1-border" : "bench-row1");
          else
            c.Height<Row>(50.Percent()).AlignSelf<Row>(Align.Stretch).Border<Row>(this.m_drawBorders ? 1 : 0).BorderColor<Row>(ColorRgba.Black).Background<Row>(new ColorRgba?(ColorRgba.Red.SetA((byte) 80)));
        }));
        Row addTo1 = row1;
        column1.Add((UiComponent) row1);
        Row row2 = new Row();
        row2.Add<Row>((Action<Row>) (c =>
        {
          if (useCss)
            c.Class<Row>(this.m_drawBorders ? "bench-row2-border" : "bench-row2");
          else
            c.Height<Row>(50.Percent()).AlignSelf<Row>(Align.Stretch).Border<Row>(this.m_drawBorders ? 1 : 0).BorderColor<Row>(ColorRgba.Black).Background<Row>(new ColorRgba?(ColorRgba.Green.SetA((byte) 80)));
        }));
        Row addTo2 = row2;
        column1.Add((UiComponent) row2);
        Column column2 = column1;
        component.Add((UiComponent) column1);
        Column column3 = new Column();
        column3.Add<Column>((Action<Column>) (c =>
        {
          if (useCss)
            c.Class<Column>("bench-col");
          else
            c.Width<Column>(50.Percent()).AlignSelf<Column>(Align.Stretch);
        }));
        Row row3 = new Row();
        row3.Add<Row>((Action<Row>) (c =>
        {
          if (useCss)
            c.Class<Row>(this.m_drawBorders ? "bench-row3-border" : "bench-row3");
          else
            c.Height<Row>(50.Percent()).AlignSelf<Row>(Align.Stretch).Border<Row>(this.m_drawBorders ? 1 : 0).BorderColor<Row>(ColorRgba.Black).Background<Row>(new ColorRgba?(ColorRgba.Blue.SetA((byte) 80)));
        }));
        Row addTo3 = row3;
        column3.Add((UiComponent) row3);
        Row row4 = new Row();
        row4.Add<Row>((Action<Row>) (c =>
        {
          if (useCss)
            c.Class<Row>(this.m_drawBorders ? "bench-row4-border" : "bench-row4");
          else
            c.Height<Row>(50.Percent()).AlignSelf<Row>(Align.Stretch).Border<Row>(this.m_drawBorders ? 1 : 0).BorderColor<Row>(ColorRgba.Black).Background<Row>(new ColorRgba?(ColorRgba.Orange.SetA((byte) 80)));
        }));
        Row addTo4 = row4;
        column3.Add((UiComponent) row4);
        component.Add((UiComponent) column3);
        Row child = component;
        addTo.Add((UiComponent) child);
        this.RectsCnt += 7;
        if (this.m_firstBox == null)
        {
          this.m_firstParent = column2;
          this.m_firstBox = addTo1;
        }
        ++nestLevel;
        if (nestLevel < maxNestLevel)
        {
          createBoxesInto(addTo1, nestLevel, maxNestLevel);
          createBoxesInto(addTo2, nestLevel, maxNestLevel);
          createBoxesInto(addTo3, nestLevel, maxNestLevel);
          createBoxesInto(addTo4, nestLevel, maxNestLevel);
        }
        else
        {
          if (this.m_addLabels)
          {
            addTo1.Add((UiComponent) createLabel("A"));
            addTo3.Add((UiComponent) createLabel("C"));
          }
          this.m_rects.Add(addTo4);
        }
      }

      Label createLabel(string text)
      {
        Label label = !useCss ? new Label(text.AsLoc()).Color<Label>(new ColorRgba?(ColorRgba.Black)).Fill<Label>().FontStyle<Label>(Mafi.Unity.UiToolkit.Component.FontStyle.Bold).FontSize<Label>(12).Padding<Label>(1.px()).Margin<Label>(1.px()) : new Label(text.AsLoc()).Class<Label>("bench-text");
        ++this.LabelsCnt;
        if (this.LabelsCnt % 7 == 0)
          this.m_labels.Add(label);
        return label;
      }
    }

    private void shuffleLabels()
    {
      ++this.m_shuffleTextIteration;
      string str = this.m_shuffleTextIteration % 2 == 0 ? "X" : "XY";
      foreach (Label label in this.m_labels)
        label.Text<Label>(str.AsLoc());
    }

    private void shuffleColors()
    {
      ++this.m_shuffleColorsIteration;
      ColorRgba colorRgba = this.m_shuffleColorsIteration % 2 == 0 ? ColorRgba.Magenta : ColorRgba.Gold;
      foreach (Row rect in this.m_rects)
        rect.Background<Row>(new ColorRgba?(colorRgba));
    }
  }
}
