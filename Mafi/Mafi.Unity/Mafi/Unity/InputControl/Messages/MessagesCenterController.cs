// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.MessagesCenterController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Messages;
using Mafi.Core.Prototypes;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl.TopStatusBar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class MessagesCenterController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar,
    IUnityUi
  {
    private readonly IUnityInputMgr m_inputManager;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IInputScheduler m_inputScheduler;
    private MessagesCenterView m_view;
    private readonly GameSpeedController m_speedController;
    private readonly UiBuilder m_builder;
    private bool m_wasPaused;
    private IconContainer m_newItemsIcon;

    public event Action<IToolbarItemController> VisibilityChanged;

    public ControllerConfig Config => ControllerConfig.MessageCenter;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public MessagesCenterController(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      IInputScheduler inputScheduler,
      GameSpeedController speedController,
      UiBuilder builder,
      NewInstanceOf<MessagesCenterView> view)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputManager = inputManager;
      this.m_gameLoop = gameLoop;
      this.m_inputScheduler = inputScheduler;
      this.m_speedController = speedController;
      this.m_builder = builder;
      this.m_view = view.Instance;
      this.m_view.SetOnCloseButtonClickAction((Action) (() => this.m_inputManager.DeactivateController((IUnityInputController) this)));
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      ToggleBtn parent = controller.AddMainMenuButtonAndReturn(Tr.MessageCenter__Title.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Tutorials.svg", 405f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleTutorials));
      this.m_newItemsIcon = this.m_builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Circle.svg", (ColorRgba) 16756491).PutToRightTopOf<IconContainer>((IUiElement) parent, 7.Vector2(), Offset.Right(0.0f) + Offset.Top(4f)).Hide<IconContainer>();
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_view.BuildUi(builder);
      this.m_view.InitializeMessages();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<MessagesCenterController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<MessagesCenterController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void SetMessagesReadStatus(bool allRead)
    {
      this.m_newItemsIcon.SetVisibility<IconContainer>(!allRead);
    }

    public void ForceOpen()
    {
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void ForceOpenMessageIfNotRead(MessageProto messageProto)
    {
      this.m_view.ForceOpenMessageIfNotRead(messageProto);
    }

    public void ShowMessage(Message message)
    {
      this.m_view.OpenMessage(message);
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void ShowMessage(Proto.ID messageId)
    {
      if (!this.m_view.OpenMessage(messageId))
        return;
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void Activate()
    {
      this.m_wasPaused = this.m_speedController.IsPaused;
      this.m_speedController.RequestPause();
      this.m_view.Show();
    }

    public void Deactivate()
    {
      if (!this.m_wasPaused)
        this.m_speedController.RequestResume();
      this.m_view.Hide();
    }

    public bool InputUpdate(IInputScheduler inputScheduler) => false;

    private void syncUpdate(GameTime gameTime) => this.m_view.SyncUpdate(gameTime);

    private void renderUpdate(GameTime gameTime) => this.m_view.RenderUpdate(gameTime);
  }
}
