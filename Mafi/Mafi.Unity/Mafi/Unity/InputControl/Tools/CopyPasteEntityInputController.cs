// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.CopyPasteEntityInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CopyPasteEntityInputController : CopyEntityInputControllerBase
  {
    private static readonly ColorRgba HOVER_HIGHLIGHT;
    private readonly CopyPasteEntityInputController.CopyPasteToolbox m_toolbox;

    public CopyPasteEntityInputController(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      ToolbarController toolbarController,
      IUnityInputMgr inputManager,
      IInputScheduler inputScheduler,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      NewInstanceOf<StaticEntityMassPlacer> entityPlacer,
      AreaSelectionToolFactory areaSelectionToolFactory,
      NewInstanceOf<FloatingPricePopup> pricePopup,
      NewInstanceOf<EntityHighlighter> highlighter,
      NewInstanceOf<TransportTrajectoryHighlighter> transportTrajectoryHighlighter,
      EntitiesManager entitiesManager,
      EntitiesCloneConfigHelper configCloneHelper,
      TransportBuildController transportBuildController,
      TerrainManager terrainManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, builder, unlockedProtosDb, shortcutsManager, inputManager, inputScheduler, cursorPickingManager, cursorManager, entityPlacer, areaSelectionToolFactory, pricePopup, highlighter, transportTrajectoryHighlighter, entitiesManager, configCloneHelper, transportBuildController, (Mafi.Unity.InputControl.Toolbar.Toolbox) new CopyPasteEntityInputController.CopyPasteToolbox(toolbarController, builder), new Proto.ID?(IdsCore.Technology.CopyTool), terrainManager);
      this.m_toolbox = (CopyPasteEntityInputController.CopyPasteToolbox) this.Toolbox;
      this.InitializeUi(new CursorStyle?(builder.Style.Cursors.Copy), builder.Audio.EntitySelect, CopyPasteEntityInputController.HOVER_HIGHLIGHT, CopyPasteEntityInputController.HOVER_HIGHLIGHT);
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(TrCore.CopyTool.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Copy.svg", 10f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleInstaCopyTool), IdsCore.Messages.TutorialOnCopyTool);
      this.InputManager.RegisterGlobalShortcut((Func<ShortcutsManager, KeyBindings>) (map => map.ToggleCopyTool), (IUnityInputController) this);
    }

    protected override bool Matches(IStaticEntity entity, bool isAreaSelection, bool isLeftCLick)
    {
      switch (entity)
      {
        case ILayoutEntity layoutEntity:
          if (layoutEntity.Prototype.CloningDisabled || !isAreaSelection && entity is MiniZipper)
            return false;
          break;
        case TransportPillar _:
          return false;
      }
      return true;
    }

    public override void Activate()
    {
      this.SetInstaActionDisabled(this.ShortcutsManager.IsOn(this.ShortcutsManager.ToggleCopyTool));
      base.Activate();
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_toolbox.PrimaryActionBtn.SetIsOn(!this.EntityPlacer.IsActive && this.ShortcutsManager.IsPrimaryActionOn);
      this.m_toolbox.PrimaryActionBtn.SetEnabled(!this.EntityPlacer.IsActive);
      return base.InputUpdate(inputScheduler);
    }

    static CopyPasteEntityInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CopyPasteEntityInputController.HOVER_HIGHLIGHT = new ColorRgba(239622);
    }

    private class CopyPasteToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;

      public CopyPasteToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("Copy", "Assets/Unity/UserInterface/Toolbar/Copy.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.CopyTool__Tooltip);
        this.AddToToolbar();
      }
    }
  }
}
