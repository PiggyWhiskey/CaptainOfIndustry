// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.MouseCursorMessage
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Unity.Camera;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  /// <summary>
  /// Displays message (and optionally and icon) next to mouse cursor and keeps it there until disabled.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class MouseCursorMessage : IUnityUi
  {
    private static readonly Vector3 MSG_OFFSET;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly CameraController m_cameraController;
    private Panel m_container;
    private Txt m_text;
    private Option<string> m_currentText;

    public bool IsActive { get; private set; }

    public MouseCursorMessage(IGameLoopEvents gameLoopEvents, CameraController cameraController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_cameraController = cameraController;
    }

    public void ShowMessage(string message)
    {
      if (!this.IsActive)
        this.m_gameLoopEvents.InputUpdate.AddNonSaveable<MouseCursorMessage>(this, new Action<GameTime>(this.inputUpdate));
      this.IsActive = true;
      this.setText(message);
      this.updatePositionAndVisibility();
      this.m_container.Show<Panel>();
    }

    public void HideMessage()
    {
      if (this.IsActive)
        this.m_gameLoopEvents.InputUpdate.RemoveNonSaveable<MouseCursorMessage>(this, new Action<GameTime>(this.inputUpdate));
      this.IsActive = false;
      this.m_container.Hide<Panel>();
    }

    private void inputUpdate(GameTime time) => this.updatePositionAndVisibility();

    private void updatePositionAndVisibility()
    {
      bool flag = this.m_container.IsVisible();
      bool visibility = this.IsActive && !this.m_cameraController.IsInFreeLookMode && !EventSystem.current.IsPointerOverGameObject();
      if (flag != visibility)
        this.m_container.SetVisibility<Panel>(visibility);
      if (!visibility)
        return;
      this.m_container.SetPosition<Panel>(Input.mousePosition + MouseCursorMessage.MSG_OFFSET - new Vector3(0.0f, this.m_container.GetHeight(), 0.0f));
    }

    public void RegisterUi(UiBuilder builder)
    {
      UiStyle style = builder.Style;
      this.m_container = builder.NewPanel("CursorMessage").SetBackground(builder.AssetsDb.GetSharedSprite(style.Icons.WhiteBgRedBorder), new ColorRgba?(builder.Style.Global.ErrorTooltipBg)).PutToLeftBottomOf<Panel>((IUiElement) builder.MainCanvas, new Vector2(0.0f, 30f));
      builder.NewIconContainer("Warning").SetIcon("Assets/Unity/UserInterface/General/Warning128.png").PutToLeftTopOf<IconContainer>((IUiElement) this.m_container, 18.Vector2(), Offset.TopLeft(10f, 10f));
      this.m_text = builder.NewTxt("Message").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(builder.Style.Global.ErrorTooltipText).PutTo<Txt>((IUiElement) this.m_container, Offset.Left(38f));
      this.m_container.Hide<Panel>();
    }

    private void setText(string text)
    {
      if (text == this.m_currentText)
        return;
      this.m_text.SetText(text);
      this.m_container.SetWidth<Panel>(((float) ((double) this.m_text.GetPreferedWidth() + 40.0 + 28.0)).Min(400f));
      this.m_currentText = (Option<string>) text;
      float preferedHeight = this.m_text.GetPreferedHeight(this.m_container.GetWidth());
      this.m_text.SetHeight<Txt>(preferedHeight);
      this.m_container.SetHeight<Panel>((float) ((double) preferedHeight + 8.0 - 3.0 + 18.0));
    }

    static MouseCursorMessage()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MouseCursorMessage.MSG_OFFSET = new Vector3(16f, -16f, 0.0f);
    }
  }
}
