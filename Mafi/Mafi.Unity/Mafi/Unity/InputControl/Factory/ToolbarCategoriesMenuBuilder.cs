// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.ToolbarCategoriesMenuBuilder
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl.Toolbar.EntitiesMenu;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>
  /// Registers itself to the UI initialization and creates toolbar items and menus (represented by <see cref="T:Mafi.Unity.InputControl.Factory.EntitiesMenuController" />) for each existing <see cref="T:Mafi.Core.Entities.Static.Layout.ToolbarCategoryProto" />.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class ToolbarCategoriesMenuBuilder : IToolbarItemRegistrar
  {
    private readonly IUnityInputMgr m_inputMgr;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly ProtosDb m_protosDb;
    private readonly ToolbarController m_toolbarController;
    private readonly EntitiesMenuView m_menuView;
    private readonly DependencyResolver m_resolver;
    private readonly UiBuilder m_builder;
    private readonly ImmutableArray<EntitiesMenuItem> m_allItems;
    private ImmutableArray<EntitiesMenuController> m_allMenus;

    public ToolbarCategoriesMenuBuilder(
      IGameLoopEvents gameLoopEvents,
      IUnityInputMgr inputMgr,
      ShortcutsManager shortcutsManager,
      ProtosDb protosDb,
      ToolbarController toolbarController,
      EntitiesMenuView menuView,
      DependencyResolver resolver,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      gameLoopEvents.InputUpdate.AddNonSaveable<ToolbarCategoriesMenuBuilder>(this, new Action<GameTime>(this.inputUpdate));
      this.m_inputMgr = inputMgr;
      this.m_shortcutsManager = shortcutsManager;
      this.m_protosDb = protosDb;
      this.m_toolbarController = toolbarController;
      this.m_menuView = menuView;
      this.m_resolver = resolver;
      this.m_builder = builder;
      ToolbarCategoryProto orThrow = this.m_protosDb.GetOrThrow<ToolbarCategoryProto>(IdsCore.ToolbarCategories.Surfaces);
      TechnologyProto surfacesTech = this.m_protosDb.GetOrThrow<TechnologyProto>(IdsCore.Technology.CustomSurfaces);
      ImmutableArray<ToolbarCategoryProto> concreteCatList = ImmutableArray.Create<ToolbarCategoryProto>(orThrow);
      Lyst<EntitiesMenuItem> lyst = new Lyst<EntitiesMenuItem>();
      lyst.AddRange(this.m_protosDb.All<TransportProto>().Where<TransportProto>((Func<TransportProto, bool>) (t => t.IsBuildable)).Select<TransportProto, EntitiesMenuItem>((Func<TransportProto, EntitiesMenuItem>) (x => new EntitiesMenuItem((Proto) x, (LocStrFormatted) x.Strings.Name, x.Graphics.IconPath, (ImmutableArray<ToolbarCategoryProto>) ImmutableArray.Empty))));
      lyst.AddRange(this.m_protosDb.All<LayoutEntityProto>().Where<LayoutEntityProto>((Func<LayoutEntityProto, bool>) (x => x.Graphics.Categories.IsNotEmpty)).Select<LayoutEntityProto, EntitiesMenuItem>((Func<LayoutEntityProto, EntitiesMenuItem>) (x => new EntitiesMenuItem((Proto) x, (LocStrFormatted) x.Strings.Name, x.Graphics.IconPath, x.Graphics.Categories))));
      lyst.AddRange(this.m_protosDb.All<TreeProto>().Where<TreeProto>((Func<TreeProto, bool>) (x => x.Graphics.Categories.IsNotEmpty)).Select<TreeProto, EntitiesMenuItem>((Func<TreeProto, EntitiesMenuItem>) (x => new EntitiesMenuItem((Proto) x, (LocStrFormatted) x.Strings.Name, x.Graphics.IconPath, x.Graphics.Categories))));
      lyst.Add((EntitiesMenuItem) new PaintSurfaceDecalsMenuItem((Option<Proto>) (Proto) surfacesTech, (LocStrFormatted) Tr.Decals_Paint, "Assets/Unity/UserInterface/Toolbar/PaintBrush.svg", concreteCatList));
      lyst.Add((EntitiesMenuItem) new TerrainTileSurfaceMenuItem(Option<TerrainTileSurfaceProto>.None, (Option<Proto>) (Proto) surfacesTech, (LocStrFormatted) Tr.ClearSurface__Title, "Assets/Unity/UserInterface/Toolbar/Buldoze128.png", concreteCatList));
      lyst.AddRange((IEnumerable<EntitiesMenuItem>) this.m_protosDb.All<TerrainTileSurfaceProto>().Where<TerrainTileSurfaceProto>((Func<TerrainTileSurfaceProto, bool>) (x => x.CanBePlacedByPlayer)).Select<TerrainTileSurfaceProto, TerrainTileSurfaceMenuItem>((Func<TerrainTileSurfaceProto, TerrainTileSurfaceMenuItem>) (x => new TerrainTileSurfaceMenuItem((Option<TerrainTileSurfaceProto>) x, (Option<Proto>) (Proto) surfacesTech, (LocStrFormatted) x.Strings.Name, x.Graphics.IconPath, concreteCatList))));
      this.m_allItems = lyst.ToImmutableArrayAndClear();
    }

    private void inputUpdate(GameTime time)
    {
      if (!this.m_shortcutsManager.IsUp(this.m_shortcutsManager.Search) || this.m_inputMgr.IsWindowControllerOpen() || this.m_allMenus.IsEmpty)
        return;
      EntitiesMenuController controller = this.m_allMenus.FirstOrDefault((Func<EntitiesMenuController, bool>) (x => x.IsActive));
      if (controller != null)
      {
        this.m_inputMgr.ActivateNewController((IUnityInputController) controller);
        controller.FocusSearchBox();
      }
      else
      {
        this.m_inputMgr.ActivateNewController((IUnityInputController) this.m_allMenus.First);
        this.m_allMenus.First.FocusSearchBox();
      }
    }

    public void OpenMenu(Proto proto)
    {
      if (this.m_allMenus.IsEmpty)
        return;
      this.m_inputMgr.ActivateNewController((IUnityInputController) this.m_allMenus.First);
      this.m_allMenus.First.SelectProto(proto);
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      this.m_menuView.Build(this.m_builder, this.m_allItems);
      Lyst<EntitiesMenuController> lyst = new Lyst<EntitiesMenuController>();
      foreach (ToolbarCategoryProto toolbarCategoryProto in this.m_protosDb.All<ToolbarCategoryProto>())
      {
        ToolbarCategoryProto category = toolbarCategoryProto;
        IEnumerable<EntitiesMenuItem> thisMenuItems = this.m_allItems.Where((Func<EntitiesMenuItem, bool>) (x =>
        {
          if (x.Categories.Contains(category))
            return true;
          return category.ContainsTransports && x.Categories.IsEmpty;
        }));
        EntitiesMenuController entitiesMenuController = this.m_resolver.Instantiate<EntitiesMenuController>();
        entitiesMenuController.BuildUi(this.m_builder, category, thisMenuItems, category.IsTransportBuildAllowed);
        lyst.Add(entitiesMenuController);
      }
      this.m_allMenus = lyst.ToImmutableArray();
    }
  }
}
