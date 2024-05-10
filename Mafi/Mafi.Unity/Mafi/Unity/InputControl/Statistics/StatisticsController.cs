// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatisticsController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.GameLoop;
using Mafi.Core.Products;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class StatisticsController : 
    BaseWindowController<StatisticsView>,
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private readonly IUnityInputMgr m_inputManager;
    private readonly IProductsManager m_productsManager;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public StatisticsController(
      IUnityInputMgr inputManager,
      IProductsManager productsManager,
      StatisticsView statisticsView,
      IGameLoopEvents gameLoop,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inputManager, gameLoop, builder, statisticsView);
      this.m_inputManager = inputManager;
      this.m_productsManager = productsManager;
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      controller.AddMainMenuButton(Tr.Statistics.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Stats.svg", 410f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleStats));
    }

    public void OpenAndShowProducts()
    {
      this.Window.OpenProductsTab();
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void OpenAndShowProductStats(ProductProto product)
    {
      this.Window.OpenProductsTab(this.m_productsManager.GetStatsFor(product));
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void OpenAndShowFoodStats()
    {
      this.Window.OpenAndShowFoodStats();
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }
  }
}
