// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays.ProductsDisplaysController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Maintenance;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.InputControl.Statistics;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ProductsDisplaysController : IUnityUi
  {
    private readonly RecipesBookController m_recipesBookController;
    private readonly ProductDisplayPanel m_displayPanel;

    public ProductsDisplaysController(
      IProductsManager productsManager,
      IGameLoopEvents gameLoop,
      ICalendar calendar,
      IInputScheduler inputScheduler,
      PinnedProductsManager pinnedProductsManager,
      MaintenanceManager maintenanceManager,
      StatisticsController statsController,
      StatsMaintenanceController statsMaintenanceController,
      RecipesBookController recipesBook)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductsDisplaysController displaysController = this;
      this.m_recipesBookController = recipesBook;
      this.m_displayPanel = new ProductDisplayPanel(calendar, gameLoop, productsManager, inputScheduler, pinnedProductsManager, maintenanceManager, new Action(onPanelClick), new Action(onMaintenanceClick), new Action<ProductProto>(onProductRightClick));

      void onProductRightClick(ProductProto product)
      {
        displaysController.m_recipesBookController.OpenForProduct(product);
      }

      void onPanelClick() => statsController.OpenAndShowProducts();

      void onMaintenanceClick() => statsMaintenanceController.OpenAndShowMaintenance();
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_displayPanel.BuildUi(builder);
      Panel panel = builder.NewPanel("Container").OnClick(new Action(this.m_recipesBookController.Open)).SetBackground(builder.Style.Global.PanelsBg);
      this.m_displayPanel.AddBottomPanel((IUiElement) panel, 48.Vector2());
      builder.NewBtn("btn").OnClick(new Action(this.m_recipesBookController.Open)).SetButtonStyle(builder.Style.Toolbar.ButtonOff).SetIcon("Assets/Unity/UserInterface/Toolbar/Recipes.svg").PutToCenterMiddleOf<Btn>((IUiElement) panel, 38.Vector2());
    }
  }
}
