// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.GameDateView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class GameDateView : IStatusBarItem, IUiElement, IDynamicSizeElement
  {
    private readonly ICalendar m_calendar;
    private readonly UiBuilder m_builder;
    private float m_maxWidth;
    private GameDate m_currentDate;
    private bool m_dateChanged;
    private Txt m_dateLabel;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_dateLabel.GameObject;

    public RectTransform RectTransform => this.m_dateLabel.RectTransform;

    public GameDateView(ICalendar calendar, IGameLoopEvents gameLoop, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_calendar = calendar.CheckNotNull<ICalendar>();
      this.m_builder = builder;
      gameLoop.SyncUpdate.AddNonSaveable<GameDateView>(this, new Action<GameTime>(this.syncUpdate));
      gameLoop.RegisterRendererInitState((object) this, new Action(this.initState));
    }

    private void initState()
    {
      this.m_calendar.NewDay.AddNonSaveable<GameDateView>(this, new Action(this.newDay));
      this.newDay();
    }

    private void newDay()
    {
      this.m_currentDate = this.m_calendar.CurrentDate;
      this.m_dateChanged = true;
    }

    private void syncUpdate(GameTime time)
    {
      if (!this.m_dateChanged)
        return;
      this.m_dateChanged = false;
      this.m_dateLabel.SetText(this.m_currentDate.FormatLong());
      float num = this.m_dateLabel.GetPreferedWidth() + 10f;
      if ((double) num <= (double) this.m_maxWidth)
        return;
      this.m_maxWidth = num;
      this.m_dateLabel.SetWidth<Txt>(this.m_maxWidth);
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      UiStyle style = this.m_builder.Style;
      this.m_dateLabel = this.m_builder.NewTxt("GameDate").SetText("").SetAlignment(TextAnchor.MiddleRight).AllowHorizontalOverflow().SetHeight<Txt>(30f).SetWidth<Txt>(100f).SetTextStyle(style.Global.TextBig);
      statusBar.AddElementToRight((IUiElement) this, 50f, true);
    }
  }
}
