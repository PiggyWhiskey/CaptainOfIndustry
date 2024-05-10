// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.AreaTool.AreaSelectionToolFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.GameLoop;
using Mafi.Core.Terrain;
using Mafi.Unity.Camera;
using Mafi.Unity.Terrain;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.AreaTool
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class AreaSelectionToolFactory
  {
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly TerrainCursor m_terrainCursor;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly TerrainRectSelection m_terrainOutlineRenderer;
    private readonly CameraController m_cameraController;

    public AreaSelectionToolFactory(
      IGameLoopEvents gameLoopEvents,
      TerrainCursor terrainCursor,
      ShortcutsManager shortcutsManager,
      TerrainRectSelection terrainOutlineRenderer,
      CameraController cameraController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_terrainCursor = terrainCursor;
      this.m_shortcutsManager = shortcutsManager;
      this.m_terrainOutlineRenderer = terrainOutlineRenderer;
      this.m_cameraController = cameraController;
    }

    public AreaSelectionTool CreateInstance(
      Action<RectangleTerrainArea2i, bool> onSelectionChangedSync,
      Action<RectangleTerrainArea2i, bool> onSelectionDone,
      Action onSelfDeactivated,
      Action onEmptyRightClick)
    {
      CameraController cameraController = this.m_cameraController;
      IGameLoopEvents gameLoopEvents = this.m_gameLoopEvents;
      ShortcutsManager shortcutsManager = this.m_shortcutsManager;
      TerrainCursor terrainCursor = this.m_terrainCursor;
      TerrainRectSelection terrainOutlineRenderer = this.m_terrainOutlineRenderer;
      Action<RectangleTerrainArea2i, bool> onSelectionChangedSync1 = onSelectionChangedSync;
      Action<RectangleTerrainArea2i, bool> onSelectionDone1 = onSelectionDone;
      Option<Action> option = (Option<Action>) onSelfDeactivated;
      Action onEmptyRightClick1 = onEmptyRightClick;
      Option<Action> onSelfDeactivated1 = option;
      return new AreaSelectionTool(cameraController, gameLoopEvents, shortcutsManager, terrainCursor, terrainOutlineRenderer, onSelectionChangedSync1, onSelectionDone1, onEmptyRightClick1, onSelfDeactivated1);
    }
  }
}
