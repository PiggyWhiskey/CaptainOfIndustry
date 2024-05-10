// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.TradeWindowController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class TradeWindowController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private readonly IUnityInputMgr m_inputManager;
    private readonly TradeWindow m_tradeWindow;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly UiBuilder m_builder;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible { get; private set; }

    public bool DeactivateShortcutsIfNotVisible => true;

    public ControllerConfig Config => ControllerConfig.Window;

    public TradeWindowController(
      IUnityInputMgr inputManager,
      TradeWindow tradeWindow,
      UnlockedProtosDbForUi unlockedProtosDb,
      IGameLoopEvents gameLoop,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      TradeWindowController controller = this;
      this.m_inputManager = inputManager;
      this.m_tradeWindow = tradeWindow;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_gameLoop = gameLoop;
      this.m_builder = builder;
      this.m_tradeWindow.SetOnCloseButtonClickAction((Action) (() => inputManager.DeactivateController((IUnityInputController) controller)));
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      if (this.m_unlockedProtosDb.AnyUnlocked<TradeDockProto>())
      {
        this.IsVisible = true;
      }
      else
      {
        this.IsVisible = false;
        this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.checkDockUnlocked);
      }
      controller.AddMainMenuButton(Tr.TradeTitle.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Trade.svg", 420f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleTradePanel));
    }

    private void checkDockUnlocked()
    {
      if (!this.m_unlockedProtosDb.AnyUnlocked<TradeDockProto>())
        return;
      this.IsVisible = true;
      this.m_unlockedProtosDb.OnUnlockedSetChangedForUi -= new Action(this.checkDockUnlocked);
      Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((IToolbarItemController) this);
    }

    public void OpenAndShowContracts()
    {
      this.m_tradeWindow.BuildAndShow(this.m_builder);
      this.m_tradeWindow.OpenContractsTab();
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void OpenAndShowLoans()
    {
      this.m_tradeWindow.BuildAndShow(this.m_builder);
      this.m_tradeWindow.OpenLoansTab();
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void Activate()
    {
      this.m_gameLoop.SyncUpdate.AddNonSaveable<TradeWindowController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<TradeWindowController>(this, new Action<GameTime>(this.renderUpdate));
      this.m_tradeWindow.BuildAndShow(this.m_builder);
    }

    public void Deactivate()
    {
      this.m_tradeWindow.Hide();
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<TradeWindowController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<TradeWindowController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      return this.m_tradeWindow.InputUpdate(inputScheduler);
    }

    private void renderUpdate(GameTime gameTime) => this.m_tradeWindow.RenderUpdate(gameTime);

    private void syncUpdate(GameTime gameTime) => this.m_tradeWindow.SyncUpdate(gameTime);
  }
}
