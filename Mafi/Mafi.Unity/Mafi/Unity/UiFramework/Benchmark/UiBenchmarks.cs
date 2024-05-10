// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Benchmark.UiBenchmarks
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Diagnostics;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Benchmark
{
  public class UiBenchmarks
  {
    private UiBuilder m_builder;
    private Vector2 m_windowSize;
    private int m_topItemsCount;
    private int m_nestingLevel;

    public void SetUp(UiBuilder builder)
    {
      this.m_builder = builder;
      addSideButton("Legacy", 300f, (Action) (() => this.runLegacyUi()));
      addSideButton("Legacy (no borders)", 340f, (Action) (() => this.runLegacyUi(false)));
      addSideButton("Legacy (no text)", 380f, (Action) (() => this.runLegacyUi(addLabels: false)));
      addSideButton("New (no css)", 420f, (Action) (() => this.runNewUi(false)));
      addSideButton("New (css)", 460f, (Action) (() => this.runNewUi(true)));
      addSideButton("New (css, no borders)", 500f, (Action) (() => this.runNewUi(true, false)));
      addSideButton("New (css, no text)", 540f, (Action) (() => this.runNewUi(true, addLabels: false)));

      Btn addSideButton(string title, float bottomOffset, Action action)
      {
        return builder.NewBtn("button", (IUiElement) builder.MainCanvas).SetButtonStyle(builder.Style.MainMenu.SideMainMenuButton).SetText(title).OnClick(action).TextBestFitEnabled().PutToRightBottomOf<Btn>((IUiElement) builder.MainCanvas, new Vector2(180f, 36f), Offset.Bottom(bottomOffset) + Offset.Right(30f));
      }
    }

    private void runLegacyUi(bool drawBorders = true, bool addLabels = true)
    {
      OldFrameworkBenchmark bench = new OldFrameworkBenchmark(this.m_windowSize, this.m_topItemsCount, this.m_nestingLevel, drawBorders, addLabels);
      bench.SetOnCloseButtonClickAction((Action) (() => bench.GameObject.Destroy()));
      Stopwatch stopwatch = Stopwatch.StartNew();
      stopwatch.Start();
      bench.BuildUi(this.m_builder);
      bench.Show();
      stopwatch.Stop();
      Mafi.Log.Error(string.Format("Built hierarchy in {0} ms (Panels: {1}x, Labels: {2})x", (object) stopwatch.ElapsedMilliseconds, (object) bench.RectsCnt, (object) bench.LabelsCnt));
    }

    private void runNewUi(bool useCss, bool drawBorders = true, bool addLabels = true)
    {
      NewToolkitBenchmark toolkitBenchmark = new NewToolkitBenchmark(this.m_builder, this.m_windowSize, this.m_topItemsCount, this.m_nestingLevel, drawBorders, addLabels);
      Stopwatch stopwatch = Stopwatch.StartNew();
      stopwatch.Start();
      toolkitBenchmark.CreateHierarchy(useCss);
      stopwatch.Stop();
      Mafi.Log.Error(string.Format("Built hierarchy in {0} ms (Panels: {1}x, Labels: {2})x", (object) stopwatch.ElapsedMilliseconds, (object) toolkitBenchmark.RectsCnt, (object) toolkitBenchmark.LabelsCnt));
    }

    public UiBenchmarks()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_windowSize = new Vector2(1600f, 1200f);
      this.m_topItemsCount = 60;
      this.m_nestingLevel = 4;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
