// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Blueprints.BlueprintsController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Blueprints;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Blueprints
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class BlueprintsController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private readonly IUnityInputMgr m_inputManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDbForUi;
    private readonly BlueprintsView m_view;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly ToolbarController m_toolbarController;
    private readonly UiBuilder m_builder;
    private readonly StaticEntityMassPlacer m_entityPlacer;
    private readonly TechnologyProto m_blueprintsTech;
    private readonly Lyst<EntityConfigData> m_copiedConfigs;
    private readonly Lyst<TileSurfaceCopyPasteData> m_copiedSurfaces;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible { get; private set; }

    public bool DeactivateShortcutsIfNotVisible => true;

    public ControllerConfig Config
    {
      get => !this.m_entityPlacer.IsActive ? ControllerConfig.Window : ControllerConfig.Tool;
    }

    public BlueprintsController(
      NewInstanceOf<StaticEntityMassPlacer> entityPlacer,
      UnlockedProtosDb unlockedProtosDb,
      UnlockedProtosDbForUi unlockedProtosDbForUi,
      ProtosDb protosDb,
      IUnityInputMgr inputManager,
      BlueprintsView view,
      IGameLoopEvents gameLoop,
      ToolbarController toolbarController,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_copiedConfigs = new Lyst<EntityConfigData>();
      this.m_copiedSurfaces = new Lyst<TileSurfaceCopyPasteData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      BlueprintsController controller = this;
      this.m_entityPlacer = entityPlacer.Instance;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_unlockedProtosDbForUi = unlockedProtosDbForUi;
      this.m_inputManager = inputManager;
      this.m_view = view;
      this.m_gameLoop = gameLoop;
      this.m_toolbarController = toolbarController;
      this.m_builder = builder;
      this.m_view.SetOnCloseButtonClickAction((Action) (() => inputManager.DeactivateController((IUnityInputController) controller)));
      this.m_blueprintsTech = protosDb.GetOrThrow<TechnologyProto>(IdsCore.Technology.Blueprints);
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      this.IsVisible = this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_blueprintsTech);
      this.m_toolbarController.AddMainMenuButton(Tr.Blueprints.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Blueprints.svg", 430f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleBlueprints));
      if (this.IsVisible)
        return;
      this.m_unlockedProtosDb.OnUnlockedSetChanged.AddNonSaveable<BlueprintsController>(this, new Action(this.onProtosUnlocked));
      this.m_inputManager.RemoveGlobalShortcut((IUnityInputController) this);
    }

    private void onProtosUnlocked()
    {
      this.IsVisible = this.m_unlockedProtosDb.IsUnlocked((Proto) this.m_blueprintsTech);
      if (!this.IsVisible)
        return;
      this.m_unlockedProtosDb.OnUnlockedSetChanged.RemoveNonSaveable<BlueprintsController>(this, new Action(this.onProtosUnlocked));
      this.m_inputManager.RegisterGlobalShortcut((Func<ShortcutsManager, KeyBindings>) (m => m.ToggleBlueprints), (IUnityInputController) this);
      Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((IToolbarItemController) this);
    }

    public void Activate()
    {
      this.m_gameLoop.SyncUpdate.AddNonSaveable<BlueprintsController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<BlueprintsController>(this, new Action<GameTime>(this.renderUpdate));
      this.m_view.BuildAndShow(this.m_builder);
    }

    public void Deactivate()
    {
      if (this.m_entityPlacer.IsActive)
        this.m_entityPlacer.Deactivate();
      this.m_view.Hide();
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<BlueprintsController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<BlueprintsController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      return this.m_entityPlacer.IsActive ? this.m_entityPlacer.InputUpdate(inputScheduler) : this.m_view.InputUpdate(inputScheduler);
    }

    private void renderUpdate(GameTime gameTime) => this.m_view.RenderUpdate(gameTime);

    private void syncUpdate(GameTime gameTime) => this.m_view.SyncUpdate(gameTime);

    public void StartBlueprintPlacement(IBlueprint blueprint)
    {
      this.m_copiedConfigs.Clear();
      this.m_copiedSurfaces.Clear();
      foreach (EntityConfigData entityConfigData in blueprint.Items)
      {
        if (!entityConfigData.Prototype.IsNone)
        {
          Proto proto = entityConfigData.Prototype.Value;
          if (this.m_unlockedProtosDbForUi.IsLocked(proto))
          {
            Option<Proto> unlockedDowngradeFor = this.m_unlockedProtosDbForUi.GetNearestUnlockedDowngradeFor((IProto) proto);
            if (unlockedDowngradeFor.HasValue)
              this.m_copiedConfigs.Add(entityConfigData.CreateCopyWithNewProto(unlockedDowngradeFor.Value));
          }
          else
            this.m_copiedConfigs.Add(entityConfigData.CreateCopy());
        }
      }
      foreach (TileSurfaceCopyPasteData surface in blueprint.Surfaces)
      {
        if (surface.SurfaceData.DecalSlimId != TileSurfaceDecalSlimId.PhantomId)
          this.m_copiedSurfaces.Add(surface);
      }
      if (this.m_copiedConfigs.IsEmpty && this.m_copiedSurfaces.IsEmpty)
        return;
      this.m_view.Hide();
      this.m_entityPlacer.Activate((object) this, new Action(this.blueprintPlaced), new Action(this.blueprintPlacementCancelled));
      this.m_entityPlacer.SetEntitiesToClone((IIndexable<EntityConfigData>) this.m_copiedConfigs, (Option<IIndexable<TileSurfaceCopyPasteData>>) (IIndexable<TileSurfaceCopyPasteData>) this.m_copiedSurfaces, StaticEntityMassPlacer.ApplyConfigMode.ApplyIfNotDisabled, true);
    }

    private void blueprintPlaced()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    private void blueprintPlacementCancelled() => this.m_view.BuildAndShow(this.m_builder);
  }
}
