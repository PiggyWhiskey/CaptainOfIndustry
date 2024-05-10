// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.BaseWindowController`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Unity.InputControl;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public class BaseWindowController<T> : IUnityInputController where T : WindowView
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly UiBuilder m_builder;
    private readonly T m_window;

    protected T Window
    {
      get
      {
        this.m_window.BuildIfNeeded(this.m_builder, (IUiElement) null);
        return this.m_window;
      }
    }

    public virtual ControllerConfig Config { get; }

    public BaseWindowController(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      UiBuilder builder,
      T window,
      ControllerConfig? config = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      BaseWindowController<T> controller = this;
      this.m_gameLoop = gameLoop;
      this.m_builder = builder;
      this.m_window = window;
      this.m_window.SetOnCloseButtonClickAction((Action) (() => inputManager.DeactivateController((IUnityInputController) controller)));
      this.Config = config ?? ControllerConfig.Window;
    }

    public virtual void Activate()
    {
      this.m_gameLoop.SyncUpdate.AddNonSaveable<BaseWindowController<T>>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<BaseWindowController<T>>(this, new Action<GameTime>(this.renderUpdate));
      this.m_window.BuildAndShow(this.m_builder, (IUiElement) null);
    }

    public virtual void Deactivate()
    {
      this.m_window.Hide();
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<BaseWindowController<T>>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<BaseWindowController<T>>(this, new Action<GameTime>(this.renderUpdate));
    }

    public virtual bool InputUpdate(IInputScheduler inputScheduler) => false;

    protected void ForceBuildWindow()
    {
      this.m_window.BuildIfNeeded(this.m_builder, (IUiElement) null);
    }

    private void renderUpdate(GameTime gameTime) => this.m_window.RenderUpdate(gameTime);

    private void syncUpdate(GameTime gameTime) => this.m_window.SyncUpdate(gameTime);
  }
}
