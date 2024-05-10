﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.ForestryDesignationController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Designation;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Inspectors.Buildings;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.Terrain;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Mine
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ForestryDesignationController : TerrainDesignationController
  {
    public ForestryDesignationController(
      IUnityInputMgr inputManager,
      TerrainCursor terrainCursor,
      TerrainDesignationsRenderer renderer,
      TowerAreasRenderer towerAreasRenderer,
      TerrainDesignationsManager manager,
      ShortcutsManager shortcutsManager,
      DesignationControllerCursors cursors,
      MouseCursorMessage cursorMessage,
      ToolbarController toolbarController,
      MessagesCenterController messagesCenter,
      IGameLoopEvents gameLoopEvents,
      ProtosDb protosDb,
      TerrainRectSelection terrainOutlineRenderer,
      ITerrainDesignationsManager designationManager,
      UnlockedProtosDbForUi unlockedProtosDb,
      CameraController cameraController,
      OceanAreasOverlayRenderer oceanOverlayRenderer,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inputManager, terrainCursor, renderer, towerAreasRenderer, manager, shortcutsManager, (IAreaToolbox) new ForestryAreaToolbox(toolbarController, builder, messagesCenter), cursors, cursorMessage, toolbarController, gameLoopEvents, terrainOutlineRenderer, designationManager, unlockedProtosDb, protosDb.GetOrThrow<TerrainDesignationProto>(IdsCore.TerrainDesignators.ForestryDesignator), cameraController, oceanOverlayRenderer, builder, builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/ForestryApply.prefab"), (Option<Proto>) Option.None, true, true);
    }

    protected override void RegisterUiToolbar(
      UiBuilder builder,
      ToolbarController toolbarController)
    {
    }
  }
}
