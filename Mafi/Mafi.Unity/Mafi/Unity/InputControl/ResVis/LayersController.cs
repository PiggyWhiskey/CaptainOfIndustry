// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ResVis.LayersController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Resources;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.Mine;
using Mafi.Unity.Terrain;
using Mafi.Unity.Trees;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.ResVis
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class LayersController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private readonly ResVisBarsRenderer.Activator m_resBarsRendererActivator;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly IActivator m_treeDesignatorsActivator;
    private readonly IActivator m_terrainGridActivator;
    private readonly ToolbarController m_toolbarController;
    private readonly UiBuilder m_builder;
    private readonly LayersLegendView m_resVisLegendView;

    public event Action<IToolbarItemController> VisibilityChanged;

    public ControllerConfig Config => ControllerConfig.LayersPanel;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public LayersController(
      ResVisBarsRenderer resBarsRenderer,
      TowerAreasRenderer towerAreasRenderer,
      TerrainResourcesProvider resourcesProvider,
      TreeHarvestingDesignatorRenderer treeDesignatorsRenderer,
      TerrainRenderer terrainRenderer,
      IUnityInputMgr inputManager,
      ToolbarController toolbarController,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      LayersController controller = this;
      this.m_resBarsRendererActivator = resBarsRenderer.CreateActivator();
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignators();
      this.m_treeDesignatorsActivator = treeDesignatorsRenderer.CreateActivator();
      this.m_terrainGridActivator = terrainRenderer.CreateGridLinesActivator();
      this.m_toolbarController = toolbarController;
      this.m_builder = builder;
      this.m_resVisLegendView = new LayersLegendView(toolbarController, resourcesProvider, new Action<LooseProductProto, bool>(this.onTerrainMaterialToggle), new Action<VirtualResourceProductProto, bool>(this.onVirtResourceToggle), new Action<bool>(this.onTerrainGridToggle), new Action<bool>(this.onTerrainDesignatorsToggle), new Action<bool>(this.onTreeDesignatorsToggle));
      this.m_resVisLegendView.SetOnCloseButtonClickAction((Action) (() => inputManager.DeactivateController((IUnityInputController) controller)));
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(Tr.Layers.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/General/Layers.svg", 80f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleResVis));
    }

    public void Activate()
    {
      this.m_resVisLegendView.BuildAndShow(this.m_builder);
      this.m_resBarsRendererActivator.ShowExactly(((IEnumerable<ProductProto>) this.m_resVisLegendView.GetVisibleTerrainMaterials()).Concat<ProductProto>(this.m_resVisLegendView.GetVisibleVirtualResourcesStates()));
      if (this.m_resVisLegendView.GetTerrainGridState())
        this.m_terrainGridActivator.Activate();
      if (this.m_resVisLegendView.GetTerrainDesignatorsState())
        this.m_towerAreasAndDesignatorsActivator.Activate();
      if (!this.m_resVisLegendView.GetTreeDesignatorsState())
        return;
      this.m_treeDesignatorsActivator.Activate();
    }

    public void Deactivate()
    {
      this.m_resVisLegendView.Hide();
      this.m_resBarsRendererActivator.HideAll();
      this.m_towerAreasAndDesignatorsActivator.DeactivateIfActive();
      this.m_terrainGridActivator.DeactivateIfActive();
      this.m_treeDesignatorsActivator.DeactivateIfActive();
    }

    public bool InputUpdate(IInputScheduler inputScheduler) => false;

    private void onTerrainMaterialToggle(LooseProductProto product, bool isOn)
    {
      if (isOn)
        this.m_resBarsRendererActivator.Show((ProductProto) product);
      else
        this.m_resBarsRendererActivator.Hide((ProductProto) product);
    }

    private void onTerrainGridToggle(bool isOn)
    {
      if (isOn)
        this.m_terrainGridActivator.Activate();
      else
        this.m_terrainGridActivator.Deactivate();
    }

    private void onTerrainDesignatorsToggle(bool isOn)
    {
      if (isOn)
        this.m_towerAreasAndDesignatorsActivator.Activate();
      else
        this.m_towerAreasAndDesignatorsActivator.Deactivate();
    }

    private void onTreeDesignatorsToggle(bool isOn)
    {
      if (isOn)
        this.m_treeDesignatorsActivator.Activate();
      else
        this.m_treeDesignatorsActivator.Deactivate();
    }

    private void onVirtResourceToggle(VirtualResourceProductProto product, bool isOn)
    {
      if (isOn)
        this.m_resBarsRendererActivator.Show(product.Product);
      else
        this.m_resBarsRendererActivator.Hide(product.Product);
    }
  }
}
