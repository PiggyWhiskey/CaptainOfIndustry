// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.StatusPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  public class StatusPanel : IUiElement, IDynamicSizeElement
  {
    private readonly UiBuilder m_builder;
    private readonly Panel m_panel;
    private readonly Txt m_textView;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_panel.GameObject;

    public RectTransform RectTransform => this.m_panel.RectTransform;

    public StatusPanel(IUiElement parent, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_panel = builder.NewPanel("Status", parent).SetBorderStyle(builder.Style.Global.DefaultDarkBorder);
      Txt txt = builder.NewTxt("Text", (IUiElement) this.m_panel);
      TextStyle title = builder.Style.Global.Title;
      ref TextStyle local = ref title;
      ColorRgba? color = new ColorRgba?((ColorRgba) 15395562);
      int? nullable = new int?(11);
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_textView = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_panel, Offset.LeftRight(2f));
    }

    public void SetStatus(LocStr text, StatusPanel.State state = StatusPanel.State.Ok)
    {
      this.SetStatus(text.TranslatedString, state);
    }

    public void SetStatus(string text, StatusPanel.State state = StatusPanel.State.Ok)
    {
      this.m_textView.SetText(text);
      int num = 120;
      this.SetWidth<StatusPanel>((this.m_textView.GetPreferedWidth() + 14f).Min((float) num));
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged != null)
        sizeChanged((IUiElement) this);
      switch (state)
      {
        case StatusPanel.State.Ok:
          this.m_panel.SetBackground(this.m_builder.Style.Panel.StatusOkBg);
          break;
        case StatusPanel.State.Warning:
          this.m_panel.SetBackground(this.m_builder.Style.Panel.StatusWarningBg);
          break;
        case StatusPanel.State.Critical:
          this.m_panel.SetBackground(this.m_builder.Style.Panel.StatusCriticalBg);
          break;
      }
    }

    public void SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      this.m_panel.SetOnMouseEnterLeaveActions(enterAction, leaveAction);
    }

    public void SetStatusWorking() => this.SetStatus(Tr.EntityStatus__Working);

    public void SetStatusPaused()
    {
      this.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Critical);
    }

    public void SetStatusNoWorkers()
    {
      this.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
    }

    public enum State
    {
      Ok,
      Warning,
      Critical,
    }
  }
}
