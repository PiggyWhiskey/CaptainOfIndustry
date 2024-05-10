// Decompiled with JetBrains decompiler
// Type: Mafi.DebugGameRenderer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using System;

#nullable disable
namespace Mafi
{
  public static class DebugGameRenderer
  {
    public const int DEFAULT_PX_PER_TILE = 9;
    internal static int s_imagesSet;
    public static readonly DebugGameMapDrawing EMPTY_DEBUG_GAME_MAP_DRAWING;
    private static DependencyResolver s_dependencyResolverStatic;
    [ThreadStatic]
    private static DependencyResolver s_dependencyResolverThreadStatic;

    /// <summary>
    /// Number of images saved, even when it is disabled. This is to detect forgotten renders in tests.
    /// </summary>
    public static int ImagesSaved => DebugGameRenderer.s_imagesSet;

    public static bool IsEnabled { get; private set; }

    public static bool IsDisabled => !DebugGameRenderer.IsEnabled;

    public static void SetEnabled(bool enabled) => DebugGameRenderer.IsEnabled = enabled;

    public static DependencyResolver DependencyResolver
    {
      get
      {
        return DebugGameRenderer.s_dependencyResolverThreadStatic ?? DebugGameRenderer.s_dependencyResolverStatic;
      }
    }

    public static void SetDependencyResolver(DependencyResolver dependencyResolver)
    {
      DebugGameRenderer.s_dependencyResolverThreadStatic = dependencyResolver;
      DebugGameRenderer.s_dependencyResolverStatic = dependencyResolver;
    }

    public static RectangleTerrainArea2i GetTerrainSize(TerrainManager terrainManager = null)
    {
      return (terrainManager ?? DebugGameRenderer.DependencyResolver.Resolve<TerrainManager>()).TerrainArea;
    }

    public static void DrawAndSaveGameImage(
      string name,
      Tile2i? from = null,
      RelTile2i? size = null,
      int pixelsPerTile = 9,
      bool forceEnable = false,
      PathFindingEntity drawNavOverlayFor = null,
      bool drawTerrainHeights = false,
      TerrainManager terrainManager = null)
    {
      if (DebugGameRenderer.IsDisabled && !forceEnable)
        return;
      if (!from.HasValue)
      {
        RectangleTerrainArea2i terrainSize = DebugGameRenderer.GetTerrainSize(terrainManager);
        from = new Tile2i?(terrainSize.Origin);
        size = new RelTile2i?(terrainSize.Size);
      }
      else if (!size.HasValue)
        size = new RelTile2i?(DebugGameRenderer.GetTerrainSize(terrainManager).Size - from.Value.RelTile2i);
      DebugGameRenderer.DrawGameImage(from.Value, size.Value, pixelsPerTile, forceEnable, drawNavOverlayFor, drawTerrainHeights, terrainManager).SaveMapAsTga(name);
    }

    public static DebugGameMapDrawing DrawGameImage(
      int pixelsPerTile = 9,
      bool forceEnable = false,
      PathFindingEntity drawNavOverlayFor = null,
      bool drawTerrainHeights = false,
      TerrainManager terrainManager = null)
    {
      RectangleTerrainArea2i terrainSize = DebugGameRenderer.GetTerrainSize(terrainManager);
      return DebugGameRenderer.DrawGameImage(terrainSize.Origin, terrainSize.Size, pixelsPerTile, forceEnable, drawNavOverlayFor, drawTerrainHeights, terrainManager);
    }

    public static DebugGameMapDrawing DrawGameImage(
      Tile2i from,
      Tile2i to,
      int padding = 0,
      int pixelsPerTile = 9,
      bool forceEnable = false,
      PathFindingEntity drawNavOverlayFor = null,
      bool drawTerrainHeights = false,
      TerrainManager terrainManager = null)
    {
      Tile2i from1 = (from.Min(to) - padding).Max(Tile2i.Zero);
      TerrainManager terrainManager1 = terrainManager ?? DebugGameRenderer.DependencyResolver?.Resolve<TerrainManager>();
      Tile2i rhs = terrainManager1 != null ? terrainManager1.TerrainArea.PlusXyCoordExcl : Tile2i.MaxValue;
      Tile2i tile2i = (from.Max(to) + padding).Min(rhs);
      return DebugGameRenderer.DrawGameImage(from1, tile2i - from1, pixelsPerTile, forceEnable, drawNavOverlayFor, drawTerrainHeights, terrainManager);
    }

    public static DebugGameMapDrawing DrawGameImage(
      Tile2i from,
      RelTile2i size,
      int pixelsPerTile = 9,
      bool forceEnable = false,
      PathFindingEntity drawNavOverlayFor = null,
      bool drawTerrainHeights = false,
      TerrainManager terrainManager = null)
    {
      if (DebugGameRenderer.DependencyResolver == null)
        return DebugGameRenderer.StartMapDrawing(from, size, pixelsPerTile, forceEnable).DrawTilesTicks().DrawTerrain(terrainManager);
      DebugGameMapDrawing debugGameMapDrawing = DebugGameRenderer.StartMapDrawing(from, size, pixelsPerTile, forceEnable).DrawTilesTicks().DrawTerrain(terrainManager).DrawTrees().DrawProps();
      if (DebugGameRendererConfig.DrawTileHeights.GetValueOrDefault(drawTerrainHeights))
        debugGameMapDrawing.DrawAllTileHeights(terrainManager);
      if (drawNavOverlayFor != null)
        debugGameMapDrawing.DrawPathabilityOverlayFor(drawNavOverlayFor);
      debugGameMapDrawing.DrawAllDesignations().DrawAllStaticEntities().DrawAllPorts().DrawAllDynamicEntities();
      return debugGameMapDrawing;
    }

    public static DebugGameMapDrawing StartMapDrawing(
      Tile2i from,
      RelTile2i size,
      int pixelsPerTile = 9,
      bool forceEnable = false)
    {
      return DebugGameRenderer.IsDisabled && !forceEnable ? DebugGameRenderer.EMPTY_DEBUG_GAME_MAP_DRAWING : new DebugGameMapDrawing(from, size, pixelsPerTile, DebugGameRenderer.DependencyResolver);
    }

    public static void DrawAndSaveGameImagePeriodically(string name, Duration interval)
    {
      SimLoopEvents sle = DebugGameRenderer.DependencyResolver.Resolve<SimLoopEvents>();
      sle.UpdateEndForUi.AddNonSaveable<DependencyResolver>(DebugGameRenderer.DependencyResolver, (Action) (() =>
      {
        if (sle.CurrentStep.Value % interval.Ticks != 0)
          return;
        DebugGameRenderer.DrawAndSaveGameImage(string.Format("{0}_{1}", (object) name, (object) (sle.CurrentStep.Value / interval.Ticks)), forceEnable: true);
      }));
    }

    static DebugGameRenderer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DebugGameRenderer.EMPTY_DEBUG_GAME_MAP_DRAWING = new DebugGameMapDrawing();
    }
  }
}
