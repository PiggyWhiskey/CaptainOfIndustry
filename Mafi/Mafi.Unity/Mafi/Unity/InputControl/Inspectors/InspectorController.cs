// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.InspectorController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class InspectorController : IUnityInputController
  {
    private readonly IoPortsRenderer m_portsRenderer;
    private readonly CursorPickingManager m_picker;
    private readonly DependencyResolver m_resolver;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly Cursoor m_entityHoveredCursor;
    private readonly AudioSource m_inspectorClickSound;
    /// <summary>
    /// Whether we should prevent showing inspector cursor when an inspectable entity is hovered.
    /// </summary>
    private bool m_isHoverSuppressed;
    private bool m_isCursorVisible;
    private HighlightId? m_lastHighlightRequest;

    public ControllerConfig Config
    {
      get
      {
        return ControllerConfig.InspectorWindow with
        {
          DeactivateOnNonUiClick = this.DeactivateOnNonUiClick
        };
      }
    }

    public Option<IEntityInspector> ActiveInspector { get; private set; }

    public virtual bool DeactivateOnNonUiClick
    {
      get => this.ActiveInspector.IsNone || this.ActiveInspector.Value.DeactivateOnNonUiClick;
    }

    public InspectorController(
      AudioDb audioDb,
      CursorPickingManager picker,
      IGameLoopEvents gameLoop,
      DependencyResolver resolver,
      CursorManager cursorManager,
      IoPortsRenderer portsRenderer,
      UiStyle style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_portsRenderer = portsRenderer;
      this.m_inspectorClickSound = audioDb.GetClonedAudio("Assets/Unity/UserInterface/Audio/InspectorClick.prefab", AudioChannel.UserInterface);
      this.m_picker = picker.CheckNotNull<CursorPickingManager>();
      this.m_resolver = resolver.CheckNotNull<DependencyResolver>();
      this.m_gameLoop = gameLoop.CheckNotNull<IGameLoopEvents>();
      this.m_entityHoveredCursor = cursorManager.RegisterCursor(style.Cursors.InspectorHover);
    }

    public bool TryActivate(IUnityInputMgr inputManager)
    {
      IRenderedEntity entity;
      return this.m_picker.TryPickEntityPreferVehicle<IRenderedEntity>(out entity) && this.TryActivateFor(inputManager, entity);
    }

    public bool TryActivateFor(IUnityInputMgr inputManager, IRenderedEntity entity)
    {
      if (this.ActiveInspector.HasValue)
        inputManager.DeactivateController((IUnityInputController) this);
      this.ActiveInspector = this.m_resolver.TryInvokeFactoryHierarchy<IEntityInspector>((object) entity);
      Option<IEntityInspector> activeInspector = this.ActiveInspector;
      if (activeInspector.IsNone)
        return false;
      activeInspector = this.ActiveInspector;
      IUnityInputController valueOrNull = activeInspector.As<IUnityInputController>().ValueOrNull;
      if (valueOrNull != null)
      {
        this.ActiveInspector = (Option<IEntityInspector>) Option.None;
        inputManager.ActivateNewController(valueOrNull);
        this.m_entityHoveredCursor.Hide();
        return false;
      }
      if (entity is IStaticEntity entity1)
      {
        if (this.m_lastHighlightRequest.HasValue)
          this.m_portsRenderer.ClearPortsHighlight(this.m_lastHighlightRequest.Value);
        this.m_lastHighlightRequest = new HighlightId?(this.m_portsRenderer.HighlightAllPortsOf(entity1));
      }
      return true;
    }

    public void UpdateCursor()
    {
      if (this.m_isHoverSuppressed)
        return;
      if (EventSystem.current.IsPointerOverGameObject())
        this.hideCursor();
      else if (this.m_picker.TryPickEntity<IRenderedEntity>(out IRenderedEntity _))
      {
        if (this.m_isCursorVisible)
          return;
        this.m_isCursorVisible = true;
        this.m_entityHoveredCursor.ShowTemporary();
      }
      else
        this.hideCursor();
    }

    private void hideCursor()
    {
      if (!this.m_isCursorVisible)
        return;
      this.m_isCursorVisible = false;
      this.m_entityHoveredCursor.HideTemporary();
    }

    public void SetHoverCursorSuppression(bool isSuppressed)
    {
      this.m_isHoverSuppressed = isSuppressed;
    }

    public void Activate()
    {
      Assert.That<Option<IEntityInspector>>(this.ActiveInspector).HasValue<IEntityInspector>();
      this.m_inspectorClickSound.Play();
      this.ActiveInspector.Value.Activate();
      this.hideCursor();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<InspectorController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<InspectorController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void Deactivate()
    {
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<InspectorController>(this, new Action<GameTime>(this.renderUpdate));
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<InspectorController>(this, new Action<GameTime>(this.syncUpdate));
      if (this.ActiveInspector.HasValue)
        this.ActiveInspector.Value.Deactivate();
      if (this.m_lastHighlightRequest.HasValue)
      {
        this.m_portsRenderer.ClearPortsHighlight(this.m_lastHighlightRequest.Value);
        this.m_lastHighlightRequest = new HighlightId?();
      }
      this.ActiveInspector = Option<IEntityInspector>.None;
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.ActiveInspector.HasValue && this.ActiveInspector.Value.InputUpdate(inputScheduler))
        return true;
      this.UpdateCursor();
      return false;
    }

    private void syncUpdate(GameTime gameTime)
    {
      if (!this.ActiveInspector.HasValue)
        return;
      this.ActiveInspector.Value.SyncUpdate(gameTime);
    }

    private void renderUpdate(GameTime gameTime)
    {
      if (!this.ActiveInspector.HasValue)
        return;
      this.ActiveInspector.Value.RenderUpdate(gameTime);
    }
  }
}
