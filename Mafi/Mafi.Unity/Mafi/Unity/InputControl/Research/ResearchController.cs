// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Research.ResearchController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Research;
using Mafi.Core.Utils;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Research
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class ResearchController : 
    BaseWindowController<ResearchWindowView>,
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar,
    IUnityUi
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly CameraController m_cameraController;
    private readonly IUnityInputMgr m_inputManager;
    private readonly ResearchManager m_researchManager;
    private readonly ResearchPopupController m_researchPopupController;
    private readonly IInstaBuildManager m_instaBuildManager;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible { get; private set; }

    public bool DeactivateShortcutsIfNotVisible => false;

    /// <summary>
    /// True if there isn't any enabled research lab in the game. This is updated only when the window is opened
    /// since during the time it is opened the player has no option to alter that.
    /// </summary>
    public bool NoLabAvailable => !this.m_researchManager.HasActiveLab;

    internal ResearchController(
      IGameLoopEvents gameLoop,
      CameraController cameraController,
      IUnityInputMgr inputManager,
      ResearchManager researchManager,
      ResearchPopupController researchPopupController,
      IInstaBuildManager instaBuildManager,
      UiBuilder builder,
      NewInstanceOf<ResearchWindowView> researchView)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inputManager, gameLoop, builder, researchView.Instance, new ControllerConfig?(ControllerConfig.WindowFullscreen));
      this.m_gameLoop = gameLoop;
      this.m_cameraController = cameraController;
      this.m_inputManager = inputManager;
      this.m_researchManager = researchManager;
      this.m_researchPopupController = researchPopupController;
      this.m_instaBuildManager = instaBuildManager;
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      if (this.m_researchManager.WasLabEverBuilt || this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        this.IsVisible = true;
      }
      else
      {
        this.IsVisible = false;
        this.m_gameLoop.SyncUpdate.AddNonSaveable<ResearchController>(this, new Action<GameTime>(this.checkLabBuilt));
      }
      controller.AddMainMenuButton(Tr.Research.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Research.svg", 450f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleResearchWindow));
    }

    private void checkLabBuilt(GameTime time)
    {
      if (!this.m_researchManager.WasLabEverBuilt)
        return;
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<ResearchController>(this, new Action<GameTime>(this.checkLabBuilt));
      this.IsVisible = true;
      Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((IToolbarItemController) this);
    }

    public void OpenResearchForRecipe(IRecipeForUi recipe)
    {
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
      this.Window.FindResearchFor(recipe);
    }

    public override void Activate()
    {
      base.Activate();
      this.m_cameraController.IsEnabled = false;
      this.m_researchPopupController.Activate();
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.m_cameraController.IsEnabled = true;
      this.m_researchPopupController.Deactivate();
    }

    public override bool InputUpdate(IInputScheduler inputScheduler) => this.Window.InputUpdate();

    public void RegisterUi(UiBuilder builder) => this.ForceBuildWindow();
  }
}
