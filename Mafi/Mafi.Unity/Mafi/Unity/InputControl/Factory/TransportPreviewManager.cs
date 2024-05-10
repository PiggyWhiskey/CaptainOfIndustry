// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.TransportPreviewManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Unity.Entities;
using Mafi.Unity.Factory.Transports;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportPreviewManager
  {
    public readonly TransportModelFactory TransportModelFactory;
    public readonly ObjectHighlighter ObjectHighlighter;
    public readonly EntityHighlighter EntityHighlighter;
    public readonly PortPreviewManager PortPreviewManager;
    public readonly TransportPillarsRenderer PillarsRenderer;
    public readonly LayoutEntityPreviewManager EntityPreviewManager;
    public readonly TransportsManager TransportsManager;
    public readonly TerrainManager TerrainManager;
    public readonly TransportsConstructionHelper TransportsConstructionHelper;
    public readonly TransportPillarProto PillarProto;
    private readonly ObjectPool2<StaticTransportPreview> m_previewPool;
    private readonly Lyst<StaticTransportPreview> m_activePreviews;
    private readonly Lyst<StaticTransportPreview> m_activePreviewsForSim;
    private readonly Lyst<StaticTransportPreview> m_removedPreviews;
    private bool m_updateActivePreviewsForSim;

    public TransportPreviewManager(
      ISimLoopEvents simLoopEvents,
      IGameLoopEvents gameLoopEvents,
      IEntitiesManager entitiesManager,
      TransportModelFactory transportModelFactory,
      LayoutEntityPreviewManager entityPreviewManager,
      ObjectHighlighter objectHighlighter,
      NewInstanceOf<EntityHighlighter> entityHighlighter,
      PortPreviewManager portPreviewManager,
      TransportPillarsRenderer pillarsRenderer,
      TransportsManager transportsManager,
      TransportsConstructionHelper transportsConstructionHelper,
      TransportPillarsBuilder pillarsBuilder,
      TerrainManager terrainManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_activePreviews = new Lyst<StaticTransportPreview>(true);
      this.m_activePreviewsForSim = new Lyst<StaticTransportPreview>(true);
      this.m_removedPreviews = new Lyst<StaticTransportPreview>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TransportModelFactory = transportModelFactory;
      this.ObjectHighlighter = objectHighlighter;
      this.PortPreviewManager = portPreviewManager;
      this.PillarsRenderer = pillarsRenderer;
      this.EntityHighlighter = entityHighlighter.Instance;
      this.EntityPreviewManager = entityPreviewManager;
      this.TransportsManager = transportsManager;
      this.TransportsConstructionHelper = transportsConstructionHelper;
      this.PillarProto = pillarsBuilder.PillarProto;
      this.TerrainManager = terrainManager;
      this.m_previewPool = new ObjectPool2<StaticTransportPreview>(32, (Func<ObjectPool2<StaticTransportPreview>, StaticTransportPreview>) (pool => new StaticTransportPreview(this)));
      simLoopEvents.Sync.AddNonSaveable<TransportPreviewManager>(this, new Action(this.syncUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<TransportPreviewManager>(this, new Action(this.simUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<TransportPreviewManager>(this, new Action<GameTime>(this.renderUpdate));
      entitiesManager.StaticEntityRemoved.AddNonSaveable<TransportPreviewManager>(this, new Action<IStaticEntity>(this.entityRemoved));
    }

    private void syncUpdate()
    {
      if (this.m_activePreviews.IsEmpty)
      {
        this.m_activePreviewsForSim.Clear();
      }
      else
      {
        this.m_removedPreviews.Clear();
        this.m_activePreviews.RemoveWhere((Predicate<StaticTransportPreview>) (x => x.IsDestroyed), this.m_removedPreviews);
        if (this.m_removedPreviews.IsNotEmpty)
        {
          this.m_updateActivePreviewsForSim = true;
          foreach (StaticTransportPreview removedPreview in this.m_removedPreviews)
            this.m_previewPool.ReturnInstanceKeepReference(removedPreview);
          this.m_removedPreviews.Clear();
        }
        if (this.m_updateActivePreviewsForSim)
        {
          this.m_updateActivePreviewsForSim = false;
          this.m_activePreviewsForSim.Clear();
          this.m_activePreviewsForSim.AddRange(this.m_activePreviews);
        }
        foreach (StaticTransportPreview activePreview in this.m_activePreviews)
          activePreview.SyncUpdate();
      }
    }

    private void renderUpdate(GameTime time)
    {
      foreach (StaticTransportPreview activePreview in this.m_activePreviews)
        activePreview.RenderUpdate();
    }

    private void simUpdate()
    {
      foreach (StaticTransportPreview transportPreview in this.m_activePreviewsForSim)
        transportPreview.SimUpdate();
    }

    private void entityRemoved(IStaticEntity entity)
    {
      foreach (StaticTransportPreview transportPreview in this.m_activePreviewsForSim)
        transportPreview.Invalidate();
    }

    public TransportPreviewVisualizer GeTransportPreviewVisualizer()
    {
      return new TransportPreviewVisualizer(this);
    }

    public StaticTransportPreview CreatePreviewPooled(
      TransportTrajectory trajectory,
      ImmutableArray<Tile2i> pillarHints)
    {
      StaticTransportPreview instance = this.m_previewPool.GetInstance();
      Assert.That<bool>(instance.IsDestroyed).IsTrue();
      instance.Initialize(trajectory, pillarHints);
      this.m_activePreviews.Add(instance);
      this.m_updateActivePreviewsForSim = true;
      return instance;
    }

    public ImmutableArray<PillarVisualsSpec> GetPillarsForTraj(
      Option<TransportTrajectory> trajectory,
      IReadOnlySet<Tile2i> pillarHints,
      out bool pillarsFound)
    {
      if (trajectory.IsNone)
      {
        pillarsFound = true;
        return ImmutableArray<PillarVisualsSpec>.Empty;
      }
      ImmutableArray<Tile3i> supportedTiles;
      pillarsFound = this.TransportsConstructionHelper.TryFindPillarsForTrajectory(trajectory.Value, pillarHints ?? Set<Tile2i>.Empty, out supportedTiles, out int _, out int _);
      return !pillarsFound ? ImmutableArray<PillarVisualsSpec>.Empty : this.CreatePillarsVisualSpec(trajectory.Value, supportedTiles);
    }

    public ImmutableArray<PillarVisualsSpec> CreatePillarsVisualSpec(
      TransportTrajectory trajectory,
      ImmutableArray<Tile3i> suppTiles)
    {
      ImmutableArrayBuilder<PillarVisualsSpec> immutableArrayBuilder = new ImmutableArrayBuilder<PillarVisualsSpec>(suppTiles.Length);
      for (int index = 0; index < suppTiles.Length; ++index)
      {
        Tile3i suppTile = suppTiles[index];
        TransportPillar existingPillar;
        ThicknessTilesI newHeight;
        PillarVisualsSpec pillarVisualsSpec;
        if (this.TransportsManager.CanExtendPillarAt(suppTile.Xy, suppTile.Height, out existingPillar, out newHeight))
        {
          pillarVisualsSpec = this.TransportsConstructionHelper.ComputePillarVisuals(existingPillar.CenterTile, newHeight, (Option<TransportTrajectory>) trajectory, true);
        }
        else
        {
          HeightTilesI baseHeight;
          if (this.TransportsManager.CanBuildPillarAt(suppTile.Xy, suppTile.Height, out baseHeight, out newHeight))
          {
            pillarVisualsSpec = this.TransportsConstructionHelper.ComputePillarVisuals(suppTile.SetZ(baseHeight.Value), newHeight, (Option<TransportTrajectory>) trajectory, true);
          }
          else
          {
            Log.Error(string.Format("Pillar could not be built at {0}.", (object) suppTile));
            pillarVisualsSpec = new PillarVisualsSpec(suppTile, PooledArray<PillarLayerSpec>.Empty, false);
          }
        }
        immutableArrayBuilder[index] = pillarVisualsSpec;
      }
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }
  }
}
