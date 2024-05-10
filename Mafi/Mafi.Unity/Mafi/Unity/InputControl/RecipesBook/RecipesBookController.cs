// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.RecipesBook.RecipesBookController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.RecipesBook
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class RecipesBookController : BaseWindowController<RecipesBookView>
  {
    private readonly IUnityInputMgr m_inputManager;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly RecipesBookView m_view;
    private readonly StaticEntityMassPlacer m_entityPlacer;

    public override ControllerConfig Config
    {
      get => !this.m_entityPlacer.IsActive ? base.Config : ControllerConfig.Tool;
    }

    public RecipesBookController(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      IUnityInputMgr unityInputMgr,
      UnlockedProtosDbForUi unlockedProtosDb,
      RecipesBookView recipesView,
      NewInstanceOf<StaticEntityMassPlacer> entityPlacer,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(unityInputMgr, gameLoop, builder, recipesView);
      this.m_inputManager = inputManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_view = recipesView;
      this.m_entityPlacer = entityPlacer.Instance;
      builder.AddDependency<RecipesBookController>(this);
      unityInputMgr.RegisterGlobalShortcut((Func<ShortcutsManager, KeyBindings>) (m => m.ToggleRecipeBook), (IUnityInputController) this);
    }

    public void OpenForProduct(ProductProto product)
    {
      this.Window.SetProduct(product);
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void Open() => this.m_inputManager.ActivateNewController((IUnityInputController) this);

    public void StartProtoBuild(LayoutEntityProto proto)
    {
      if (this.m_unlockedProtosDb.IsLocked((Proto) proto))
        return;
      this.m_entityPlacer.Activate((object) this, new Action(this.entityPlacedOrCancelled), new Action(this.entityPlacedOrCancelled));
      this.m_entityPlacer.SetLayoutEntityToPlace((ILayoutEntityProto) proto);
      this.Window.Hide();
    }

    private void entityPlacedOrCancelled()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      return this.m_entityPlacer.IsActive ? this.m_entityPlacer.InputUpdate(inputScheduler) : base.InputUpdate(inputScheduler);
    }

    public override void Deactivate()
    {
      if (this.m_entityPlacer.IsActive)
        this.m_entityPlacer.Deactivate();
      base.Deactivate();
    }
  }
}
