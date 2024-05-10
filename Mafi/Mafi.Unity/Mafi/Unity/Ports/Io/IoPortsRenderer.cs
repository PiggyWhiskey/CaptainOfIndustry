// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ports.Io.IoPortsRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ports.Io
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class IoPortsRenderer : IDisposable
  {
    public static readonly ColorRgba HIGHLIGHT_OK;
    public static readonly ColorRgba HIGHLIGHT_ERR;
    private static readonly ColorRgba IN_PORT_COLOR;
    private static readonly ColorRgba OUT_PORT_COLOR;
    private static readonly ColorRgba INOUT_PORT_COLOR;
    public static readonly MultiIconSpec ICON_PORT_CAN_CONNECT;
    public static readonly MultiIconSpec ICON_PORT_CAN_NOT_CONNECT;
    public static readonly MultiIconSpec ICON_PORT_BLOCKED;
    private static readonly Vector3[] PORT_ARROW_OFFSETS;
    private static readonly Vector3[] COLLIDER_OFFSETS;
    public static readonly Vector3[] PORT_ICON_OFFSETS;
    private readonly Option<IoPortsRenderer.PortsChunkStandard>[] m_portsChunks;
    private readonly IoPortsRenderer.PortsChunkImmediate m_immediatePortsChunk;
    private HighlightId m_currentHighlightRequestId;
    private IoPortsRenderer.HighlightRequest m_currentHighlightRequest;
    private readonly Stak<IoPortsRenderer.HighlightRequest> m_highlightRequests;
    private readonly Dict<IoPortsRenderer.PortsChunkStandard, IoPortsRenderer.HighlightComputationRequest> m_chunkHighlightRequests;
    private readonly Lyst<IoPortsRenderer.HighlightComputationRequest> m_highlightsToComputeOnSim;
    private readonly Lyst<IoPortsRenderer.HighlightComputationRequest> m_highlightsComputedOnSim;
    private readonly ObjectHighlighter m_highlighter;
    private readonly PortProductsResolver m_portProductsResolver;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly IconProvider m_iconProvider;
    private readonly IoPortsManager m_portsManager;
    private readonly ITransportsPredicates m_transportsPredicates;
    private readonly ChunkBasedRenderingManager m_chunksRenderingManager;
    private readonly ConstructionManager m_constructionManager;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly ImmutableArray<IoPortShapeProto> m_renderIndexToProto;
    private readonly ImmutableArray<KeyValuePair<Mesh[], Material>> m_portsMeshMatData;
    private readonly KeyValuePair<Mesh[], Material> m_arrowsMeshMatData;
    private readonly Dict<GameObject, IoPort> m_colliderGoToPort;
    private Lyst<KeyValuePair<IoPort, IoPortsRenderer.EventType>> m_newEvents;
    private Lyst<KeyValuePair<IoPort, IoPortsRenderer.EventType>> m_eventsToProcess;
    private HighlightId m_lastUsedHighlightRequestId;
    private readonly Material m_materialForPortsInstancedRenderingShared;
    private readonly Material m_materialForArrowsInstancedRenderingShared;
    private readonly Material m_materialForPortHighlightsInstancedRenderingShared;
    private readonly Lyst<GameObject> m_portColliderGosPool;

    [Conditional("PRINT_TIMINGS")]
    private static void startTiming()
    {
    }

    [Conditional("PRINT_TIMINGS")]
    private static void stopTiming<T0>(string name, T0 arg0)
    {
    }

    public IoPortsRenderer(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      IoPortsManager portsManager,
      AssetsDb assetsDb,
      ObjectHighlighter highlighter,
      PortProductsResolver portProductResolvers,
      TerrainOccupancyManager occupancyManager,
      IconProvider iconProvider,
      ITransportsPredicates transportsPredicates,
      ChunkBasedRenderingManager visibleChunksRenderer,
      ProtosDb protosDb,
      ConstructionManager constructionManager,
      ReloadAfterAssetUpdateManager reloadManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_highlightRequests = new Stak<IoPortsRenderer.HighlightRequest>();
      this.m_chunkHighlightRequests = new Dict<IoPortsRenderer.PortsChunkStandard, IoPortsRenderer.HighlightComputationRequest>();
      this.m_highlightsToComputeOnSim = new Lyst<IoPortsRenderer.HighlightComputationRequest>();
      this.m_highlightsComputedOnSim = new Lyst<IoPortsRenderer.HighlightComputationRequest>();
      this.m_colliderGoToPort = new Dict<GameObject, IoPort>();
      this.m_newEvents = new Lyst<KeyValuePair<IoPort, IoPortsRenderer.EventType>>();
      this.m_eventsToProcess = new Lyst<KeyValuePair<IoPort, IoPortsRenderer.EventType>>();
      this.m_lastUsedHighlightRequestId = HighlightId.Empty;
      this.m_portColliderGosPool = new Lyst<GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_highlighter = highlighter;
      this.m_portProductsResolver = portProductResolvers;
      this.m_occupancyManager = occupancyManager;
      this.m_iconProvider = iconProvider;
      this.m_portsManager = portsManager;
      this.m_transportsPredicates = transportsPredicates;
      this.m_chunksRenderingManager = visibleChunksRenderer;
      this.m_constructionManager = constructionManager;
      this.m_reloadManager = reloadManager;
      this.m_portsChunks = new Option<IoPortsRenderer.PortsChunkStandard>[this.m_chunksRenderingManager.ChunksCountTotal];
      this.m_materialForPortsInstancedRenderingShared = assetsDb.GetSharedMaterial("Assets/Base/Transports/IoPortInstanced.mat");
      this.m_materialForArrowsInstancedRenderingShared = assetsDb.GetSharedMaterial("Assets/Base/Transports/Arrows/IoPortArrowInstanced.mat");
      this.m_materialForPortHighlightsInstancedRenderingShared = assetsDb.GetSharedMaterial("Assets/Base/Transports/IoPortHighlightInstanced.mat");
      IEnumerable<IGrouping<string, KeyValuePair<IoPortShapeProto, bool>>> groupings = protosDb.All<IoPortShapeProto>().SelectMany<IoPortShapeProto, KeyValuePair<KeyValuePair<IoPortShapeProto, bool>, string>>((Func<IoPortShapeProto, IEnumerable<KeyValuePair<KeyValuePair<IoPortShapeProto, bool>, string>>>) (x => (IEnumerable<KeyValuePair<KeyValuePair<IoPortShapeProto, bool>, string>>) new KeyValuePair<KeyValuePair<IoPortShapeProto, bool>, string>[2]
      {
        Make.Kvp<KeyValuePair<IoPortShapeProto, bool>, string>(Make.Kvp<IoPortShapeProto, bool>(x, false), x.Graphics.ConnectedPortPrefabPath),
        Make.Kvp<KeyValuePair<IoPortShapeProto, bool>, string>(Make.Kvp<IoPortShapeProto, bool>(x, true), x.Graphics.DisconnectedPortPrefabPath)
      })).GroupBy<KeyValuePair<KeyValuePair<IoPortShapeProto, bool>, string>, string, KeyValuePair<IoPortShapeProto, bool>>((Func<KeyValuePair<KeyValuePair<IoPortShapeProto, bool>, string>, string>) (x => x.Value), (Func<KeyValuePair<KeyValuePair<IoPortShapeProto, bool>, string>, KeyValuePair<IoPortShapeProto, bool>>) (x => x.Key));
      int num = 0;
      Lyst<KeyValuePair<string, string>> list = new Lyst<KeyValuePair<string, string>>();
      Lyst<IoPortShapeProto> lyst = new Lyst<IoPortShapeProto>();
      foreach (IGrouping<string, KeyValuePair<IoPortShapeProto, bool>> grouping in groupings)
      {
        bool flag = true;
        foreach (KeyValuePair<IoPortShapeProto, bool> keyValuePair in (IEnumerable<KeyValuePair<IoPortShapeProto, bool>>) grouping)
        {
          IoPortShapeProto.Gfx graphics = keyValuePair.Key.Graphics;
          if (keyValuePair.Value)
          {
            if (flag)
            {
              flag = false;
              lyst.Add(keyValuePair.Key);
              list.Add<string, string>(graphics.DisconnectedPortPrefabPath, graphics.DisconnectedPortPrefabPathLod3);
            }
            graphics.RendererIndexDisconnected = num;
          }
          else
          {
            if (flag)
            {
              flag = false;
              lyst.Add(keyValuePair.Key);
              list.Add<string, string>(graphics.ConnectedPortPrefabPath, graphics.ConnectedPortPrefabPathLod3);
            }
            graphics.RendererIndexConnected = num;
          }
        }
        ++num;
      }
      Mafi.Assert.That<int>(num).IsEqualTo(lyst.Count);
      Mafi.Assert.That<int>(num).IsEqualTo(list.Count);
      this.m_renderIndexToProto = lyst.ToImmutableArray();
      this.m_portsMeshMatData = list.ToImmutableArray<KeyValuePair<Mesh[], Material>>((Func<KeyValuePair<string, string>, KeyValuePair<Mesh[], Material>>) (x =>
      {
        KeyValuePair<Mesh, Material> materialFromAsset = assetsDb.GetMeshAndMaterialFromAsset(x.Key);
        Mesh[] key = new Mesh[7];
        key[0] = materialFromAsset.Key;
        for (int index = 1; index < 3; ++index)
          key[index] = key[index - 1];
        key[3] = assetsDb.GetMeshFromAsset(x.Value);
        for (int index = 4; index < key.Length; ++index)
          key[index] = key[index - 1];
        return Make.Kvp<Mesh[], Material>(key, materialFromAsset.Value);
      }));
      KeyValuePair<Mesh, Material> materialFromAsset1 = assetsDb.GetMeshAndMaterialFromAsset("Assets/Base/Transports/Arrows/PortArrow.prefab");
      Mesh[] key1 = new Mesh[7];
      key1[0] = materialFromAsset1.Key;
      for (int index = 1; index < key1.Length; ++index)
        key1[index] = key1[index - 1];
      this.m_arrowsMeshMatData = Make.Kvp<Mesh[], Material>(key1, materialFromAsset1.Value);
      this.m_immediatePortsChunk = new IoPortsRenderer.PortsChunkImmediate(this);
      this.m_chunksRenderingManager.RegisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this.m_immediatePortsChunk);
      gameLoopEvents.SyncUpdate.AddNonSaveable<IoPortsRenderer>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.RenderUpdate.AddNonSaveable<IoPortsRenderer>(this, new Action<GameTime>(this.renderUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<IoPortsRenderer>(this, new Action(this.simUpdate));
    }

    private void initState()
    {
      foreach (IoPort port in this.m_portsManager.Ports)
      {
        IoPortsRenderer.PortsChunkStandard portsChunk = this.getOrCreatePortsChunk(port);
        if (shouldBeVisible(port))
          portsChunk.AddPortInstance(port);
        else
          portsChunk.AddPortInstanceKeepHidden(port);
      }
      this.m_portsManager.PortAdded += (Action<IoPort>) (p => this.m_newEvents.Add(Make.Kvp<IoPort, IoPortsRenderer.EventType>(p, shouldBeVisible(p) ? IoPortsRenderer.EventType.Add : IoPortsRenderer.EventType.AddKeepHidden)));
      this.m_portsManager.PortRemoved += (Action<IoPort>) (p => this.m_newEvents.Add(Make.Kvp<IoPort, IoPortsRenderer.EventType>(p, IoPortsRenderer.EventType.Remove)));
      this.m_portsManager.PortConnectionChanged.AddNonSaveable<IoPortsRenderer>(this, (Action<IoPort, IoPort>) ((p, other) => this.m_newEvents.Add(Make.Kvp<IoPort, IoPortsRenderer.EventType>(p, shouldBeVisible(p) ? IoPortsRenderer.EventType.Show : IoPortsRenderer.EventType.Hide))));
      // ISSUE: method pointer
      this.m_constructionManager.EntityConstructed.AddNonSaveable<IoPortsRenderer>(this, new Action<IStaticEntity>((object) this, __methodptr(\u003CinitState\u003Eg__updateAllPorts\u007C50_4)));
      this.m_constructionManager.EntityConstructionStateChanged.AddNonSaveable<IoPortsRenderer>(this, (Action<IStaticEntity, ConstructionState>) ((e, _) => updateAllPorts(e)));
      // ISSUE: method pointer
      this.m_constructionManager.EntityConstructionNearlyFinished.AddNonSaveable<IoPortsRenderer>(this, new Action<IStaticEntity>((object) this, __methodptr(\u003CinitState\u003Eg__updateAllPorts\u007C50_4)));
      this.m_constructionManager.EntityPauseStateChanged.AddNonSaveable<IoPortsRenderer>(this, (Action<IStaticEntity, bool>) ((e, _) => updateAllPorts(e)));

      static bool shouldBeVisible(IoPort port)
      {
        if (port.IsNotConnected)
          return true;
        return port.OwnerEntity is Mafi.Core.Factory.Transports.Transport ? port.ConnectedPort.Value.OwnerEntity is Mafi.Core.Factory.Transports.Transport && port.ShapePrototype.Graphics.ShowWhenTwoTransportsConnect && port.Id.Value < port.ConnectedPort.Value.Id.Value : port.ConnectedPort.Value.OwnerEntity is Mafi.Core.Factory.Transports.Transport || port.Id.Value < port.ConnectedPort.Value.Id.Value;
      }

      void updateAllPorts(IStaticEntity e)
      {
        if (e.IsDestroyed || !(e is IEntityWithPorts entityWithPorts))
          return;
        foreach (IoPort port in entityWithPorts.Ports)
        {
          if (shouldBeVisible(port))
          {
            this.m_newEvents.Add(Make.Kvp<IoPort, IoPortsRenderer.EventType>(port, IoPortsRenderer.EventType.Hide));
            this.m_newEvents.Add(Make.Kvp<IoPort, IoPortsRenderer.EventType>(port, IoPortsRenderer.EventType.Show));
          }
        }
      }
    }

    public void Dispose()
    {
      foreach (Option<IoPortsRenderer.PortsChunkStandard> portsChunk in this.m_portsChunks)
        portsChunk.ValueOrNull?.Dispose();
      this.m_immediatePortsChunk.Dispose();
      this.m_chunkHighlightRequests.Clear();
      foreach (GameObject key in this.m_colliderGoToPort.Keys)
        key.Destroy();
      this.m_colliderGoToPort.Clear();
      foreach (GameObject go in this.m_portColliderGosPool)
        go.Destroy();
      this.m_portColliderGosPool.Clear();
    }

    private void syncUpdate(GameTime gameTime)
    {
      Mafi.Assert.That<Lyst<KeyValuePair<IoPort, IoPortsRenderer.EventType>>>(this.m_eventsToProcess).IsEmpty<KeyValuePair<IoPort, IoPortsRenderer.EventType>>("Some events were not processed");
      Swap.Them<Lyst<KeyValuePair<IoPort, IoPortsRenderer.EventType>>>(ref this.m_newEvents, ref this.m_eventsToProcess);
      this.m_newEvents.Clear();
      if (this.m_highlightsComputedOnSim.IsNotEmpty)
      {
        foreach (IoPortsRenderer.HighlightComputationRequest computationRequest in this.m_highlightsComputedOnSim)
          computationRequest.SyncResultsFromSim();
        this.m_highlightsComputedOnSim.Clear();
      }
      if (!this.m_chunkHighlightRequests.IsNotEmpty)
        return;
      if (this.m_currentHighlightRequestId == HighlightId.Empty)
      {
        foreach (IoPortsRenderer.HighlightComputationRequest computationRequest in this.m_chunkHighlightRequests.Values)
          computationRequest.Clear();
        this.m_chunkHighlightRequests.Clear();
        this.m_highlightsComputedOnSim.Clear();
        this.m_highlightsToComputeOnSim.Clear();
      }
      else
      {
        Mafi.Assert.That<Lyst<IoPortsRenderer.HighlightComputationRequest>>(this.m_highlightsToComputeOnSim).IsEmpty<IoPortsRenderer.HighlightComputationRequest>();
        this.m_highlightsToComputeOnSim.Clear();
        float num = float.PositiveInfinity;
        IoPortsRenderer.HighlightComputationRequest computationRequest1 = (IoPortsRenderer.HighlightComputationRequest) null;
        foreach (IoPortsRenderer.HighlightComputationRequest computationRequest2 in this.m_chunkHighlightRequests.Values)
        {
          if ((double) computationRequest2.DistanceToCamera <= 512.0)
            this.m_highlightsToComputeOnSim.Add(computationRequest2);
          else if ((double) computationRequest2.DistanceToCamera < (double) num)
          {
            num = computationRequest2.DistanceToCamera;
            computationRequest1 = computationRequest2;
          }
        }
        if (computationRequest1 != null)
          this.m_highlightsToComputeOnSim.Add(computationRequest1);
        foreach (IoPortsRenderer.HighlightComputationRequest computationRequest3 in this.m_highlightsToComputeOnSim)
        {
          this.m_chunkHighlightRequests.Remove(computationRequest3.ParentChunk);
          computationRequest3.SyncDataForSim();
        }
      }
    }

    private void renderUpdate(GameTime time)
    {
      if (!this.m_eventsToProcess.IsNotEmpty)
        return;
      foreach (KeyValuePair<IoPort, IoPortsRenderer.EventType> keyValuePair in this.m_eventsToProcess)
      {
        IoPortsRenderer.PortsChunkStandard portsChunk = this.getOrCreatePortsChunk(keyValuePair.Key);
        switch (keyValuePair.Value)
        {
          case IoPortsRenderer.EventType.Add:
            portsChunk.AddPortInstance(keyValuePair.Key);
            continue;
          case IoPortsRenderer.EventType.AddKeepHidden:
            portsChunk.AddPortInstanceKeepHidden(keyValuePair.Key);
            continue;
          case IoPortsRenderer.EventType.Remove:
            portsChunk.RemovePortInstance(keyValuePair.Key);
            continue;
          case IoPortsRenderer.EventType.Show:
            portsChunk.ShowPort(keyValuePair.Key);
            continue;
          case IoPortsRenderer.EventType.Hide:
            portsChunk.HidePort(keyValuePair.Key);
            continue;
          default:
            Mafi.Log.Error(string.Format("Unhandled event type: {0}", (object) keyValuePair.Value));
            continue;
        }
      }
      this.m_eventsToProcess.Clear();
    }

    private void simUpdate()
    {
      if (!this.m_highlightsToComputeOnSim.IsNotEmpty)
        return;
      foreach (IoPortsRenderer.HighlightComputationRequest computationRequest in this.m_highlightsToComputeOnSim)
      {
        computationRequest.PerformComputationOnSim();
        this.m_highlightsComputedOnSim.Add(computationRequest);
      }
      this.m_highlightsToComputeOnSim.Clear();
    }

    private IoPortsRenderer.PortsChunkStandard getOrCreatePortsChunk(IoPort port)
    {
      Chunk256Index chunkIndex = this.m_chunksRenderingManager.GetChunkIndex(port.Position.Xy);
      IoPortsRenderer.PortsChunkStandard newChunk = this.m_portsChunks[(int) chunkIndex.Value].ValueOrNull;
      if (newChunk == null)
      {
        this.m_portsChunks[(int) chunkIndex.Value] = (Option<IoPortsRenderer.PortsChunkStandard>) (newChunk = new IoPortsRenderer.PortsChunkStandard(this.m_chunksRenderingManager.ExtendChunkCoord(chunkIndex), this));
        this.m_chunksRenderingManager.RegisterChunk((IRenderedChunk) newChunk);
      }
      return newChunk;
    }

    /// <summary>
    /// Returns port for given game object of an arrow. Every arrow belongs to one port.
    /// </summary>
    public Option<IoPort> GetPortForArrow<T>(GameObject arrowGo)
    {
      return this.m_colliderGoToPort.Get<GameObject, IoPort>(arrowGo);
    }

    public uint ShowPortImmediate(IoPortVisual visual)
    {
      return this.m_immediatePortsChunk.ShowPortVisual(visual);
    }

    public void HidePortImmediate(uint id) => this.m_immediatePortsChunk.HidePortVisual(id);

    public void UpdatePortImmediate(
      uint id,
      Tile3i position,
      Direction903d direction,
      ColorRgba blueprintColor)
    {
      this.m_immediatePortsChunk.UpdateVisual(id, position, direction, blueprintColor);
    }

    /// <summary>
    /// Highlights ports that match the given predicate. This also shows arrows and icons next to them.
    /// Multiple highlight requests can be overlapping but only the latest one will be active.
    /// 
    /// IMPORTANT: The given predicate is cached and invoked on sim thread. Ensure that all captured variables will
    /// be valid for the entire duration of the highlight.
    /// </summary>
    public HighlightId HighlightPorts(Predicate<IoPort> highlightPredicate, bool withoutColliders = false)
    {
      this.m_lastUsedHighlightRequestId = new HighlightId(this.m_lastUsedHighlightRequestId.Value + 1);
      IoPortsRenderer.HighlightRequest highlightRequest = new IoPortsRenderer.HighlightRequest(this.m_lastUsedHighlightRequestId, highlightPredicate, withoutColliders);
      this.m_highlightRequests.Push(highlightRequest);
      this.m_currentHighlightRequest = highlightRequest;
      this.m_currentHighlightRequestId = highlightRequest.RequestId;
      return highlightRequest.RequestId;
    }

    public bool IsHighlightPending() => this.m_chunkHighlightRequests.IsNotEmpty;

    public void ClearPortsHighlight(HighlightId id)
    {
      int index = this.m_highlightRequests.IndexOf<IoPortsRenderer.HighlightRequest, int>(id.Value, (Func<IoPortsRenderer.HighlightRequest, int>) (x => x.RequestId.Value));
      if (index < 0)
      {
        Mafi.Log.Warning(string.Format("Cannot clear highlight {0}, it does not exist.", (object) id.Value));
      }
      else
      {
        this.m_highlightRequests.RemoveAt(index);
        if (index != 0)
          return;
        if (this.m_highlightRequests.IsEmpty)
        {
          this.m_currentHighlightRequest = new IoPortsRenderer.HighlightRequest();
          this.m_currentHighlightRequestId = HighlightId.Empty;
        }
        else
        {
          this.m_currentHighlightRequest = this.m_highlightRequests.Peek();
          this.m_currentHighlightRequestId = this.m_currentHighlightRequest.RequestId;
        }
      }
    }

    /// <summary>
    /// Important: Given predicate is invoked later on sim thread. Ensure that all captured variables are gonna
    /// be valid.
    /// </summary>
    public HighlightId HighlightAllPortsOf(
      IStaticEntity entity,
      Predicate<IoPort> predicate = null,
      bool withoutColliders = false)
    {
      return this.HighlightPorts(predicate != null ? (Predicate<IoPort>) (port => port.OwnerEntity == entity && predicate(port)) : (Predicate<IoPort>) (port => port.OwnerEntity == entity), withoutColliders);
    }

    public PortHighlightData HighlightPortVisual(
      uint shownPortId,
      PortHighlightSpec newHighlightSpec)
    {
      return this.m_immediatePortsChunk.HighlightPortVisual(shownPortId, newHighlightSpec);
    }

    /// <summary>Updates port highlight position and direction.</summary>
    public void UpdatePortVisualHighlight(
      PortHighlightData highlightData,
      Tile3i position,
      Direction903d direction)
    {
      this.m_immediatePortsChunk.UpdateHighlight(highlightData, position.ToGroundCenterVector3(), direction);
    }

    public void RemovePortVisualHighlight(PortHighlightData shownHighlightData)
    {
      this.m_immediatePortsChunk.RemoveHighlight(shownHighlightData);
    }

    public void PauseHighlightsFor(IoPort port, bool keepCollider = false)
    {
      this.getOrCreatePortsChunk(port).PauseHighlightsFor(port, keepCollider);
    }

    public void RestoreHighlightsFor(IoPort port)
    {
      this.getOrCreatePortsChunk(port).RestoreHighlightsFor(port);
    }

    private GameObject showColliderFor(IoPort port)
    {
      GameObject key;
      if (this.m_portColliderGosPool.IsNotEmpty)
      {
        key = this.m_portColliderGosPool.PopLast();
        key.SetActive(true);
      }
      else
      {
        key = new GameObject("PortCollider");
        CapsuleCollider capsuleCollider = key.AddComponent<CapsuleCollider>();
        capsuleCollider.center = Vector3.zero;
        capsuleCollider.height = 0.0f;
        capsuleCollider.radius = 1f;
      }
      key.transform.position = port.Position.ToCenterVector3() + IoPortsRenderer.COLLIDER_OFFSETS[port.Direction.DirectionIndex];
      this.m_colliderGoToPort.AddAndAssertNew(key, port);
      return key;
    }

    private void clearCollider(GameObject colliderGo)
    {
      colliderGo.SetActive(false);
      this.m_portColliderGosPool.Add(colliderGo);
      this.m_colliderGoToPort.RemoveAndAssert(colliderGo);
    }

    static IoPortsRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      IoPortsRenderer.HIGHLIGHT_OK = new ColorRgba(0, 192, 0, 192);
      IoPortsRenderer.HIGHLIGHT_ERR = new ColorRgba(192, 0, 0, 192);
      IoPortsRenderer.IN_PORT_COLOR = (ColorRgba) 65280;
      IoPortsRenderer.OUT_PORT_COLOR = (ColorRgba) 16711680;
      IoPortsRenderer.INOUT_PORT_COLOR = (ColorRgba) 16776960;
      IoPortsRenderer.ICON_PORT_CAN_CONNECT = new MultiIconSpec(ImmutableArray.Create<string>("Assets/Core/Ports/PortConnected128.png"), IoPortsRenderer.HIGHLIGHT_OK.SetA(byte.MaxValue));
      IoPortsRenderer.ICON_PORT_CAN_NOT_CONNECT = new MultiIconSpec(ImmutableArray.Create<string>("Assets/Core/Ports/PortCantConnect128.png"), IoPortsRenderer.HIGHLIGHT_ERR.SetA(byte.MaxValue));
      IoPortsRenderer.ICON_PORT_BLOCKED = new MultiIconSpec(ImmutableArray.Create<string>("Assets/Core/Ports/PortBlocked128.png"), IoPortsRenderer.HIGHLIGHT_ERR.SetA(byte.MaxValue));
      IoPortsRenderer.PORT_ARROW_OFFSETS = new Vector3[6]
      {
        new RelTile3f(0.9.ToFix32(), (Fix32) 0, (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, 0.9.ToFix32(), (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, (Fix32) 0, 0.9.ToFix32()).ToVector3(),
        new RelTile3f(-0.9.ToFix32(), (Fix32) 0, (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, -0.9.ToFix32(), (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, (Fix32) 0, -0.9.ToFix32()).ToVector3()
      };
      IoPortsRenderer.COLLIDER_OFFSETS = new Vector3[6]
      {
        new RelTile3f(0.7.ToFix32(), (Fix32) 0, (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, 0.7.ToFix32(), (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, (Fix32) 0, 0.7.ToFix32()).ToVector3(),
        new RelTile3f(-0.7.ToFix32(), (Fix32) 0, (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, -0.7.ToFix32(), (Fix32) 0).ToVector3(),
        new RelTile3f((Fix32) 0, (Fix32) 0, -0.7.ToFix32()).ToVector3()
      };
      IoPortsRenderer.PORT_ICON_OFFSETS = new Vector3[6]
      {
        new RelTile3f(Fix32.Half, (Fix32) 0, 1.5.ToFix32()).ToVector3(),
        new RelTile3f((Fix32) 0, Fix32.Half, 1.5.ToFix32()).ToVector3(),
        new RelTile3f((Fix32) 0, (Fix32) 0, 2.ToFix32()).ToVector3(),
        new RelTile3f(-Fix32.Half, (Fix32) 0, 1.5.ToFix32()).ToVector3(),
        new RelTile3f((Fix32) 0, -Fix32.Half, 1.5.ToFix32()).ToVector3(),
        new RelTile3f((Fix32) 0, (Fix32) 0, -2.ToFix32()).ToVector3()
      };
    }

    /// <summary>Infrastructure for instanced rendering.</summary>
    private class PortsChunkBase : IDisposable
    {
      protected int InstancesCount;
      private int m_highlightInstancesCount;
      private int m_registeredForHighlightingCount;
      public readonly IoPortsRenderer IoPortsRenderer;
      private readonly Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>[] m_portModelsChunks;
      private readonly Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>[] m_highlightModelsChunks;
      private readonly bool[] m_isRegisteredForHighlighting;
      private readonly InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> m_arrowsChunk;
      private readonly Transform m_iconsParentTransform;

      public PortsChunkBase(IoPortsRenderer ioPortsRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IoPortsRenderer = ioPortsRenderer;
        this.m_portModelsChunks = new Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>[ioPortsRenderer.m_portsMeshMatData.Length];
        this.m_highlightModelsChunks = new Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>[ioPortsRenderer.m_portsMeshMatData.Length];
        this.m_isRegisteredForHighlighting = new bool[this.m_highlightModelsChunks.Length];
        KeyValuePair<Mesh[], Material> arrowsMeshMatData = ioPortsRenderer.m_arrowsMeshMatData;
        Mesh[] key = arrowsMeshMatData.Key;
        Material instancedRenderingShared = ioPortsRenderer.m_materialForArrowsInstancedRenderingShared;
        arrowsMeshMatData = ioPortsRenderer.m_arrowsMeshMatData;
        Material sourceOfTextures = arrowsMeshMatData.Value;
        Material nonSharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(instancedRenderingShared, sourceOfTextures);
        this.m_arrowsChunk = new InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>(key, nonSharedMaterial, layer: Layer.Custom13Icons);
        ioPortsRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_arrowsChunk);
      }

      public void Dispose()
      {
        for (int index = 0; index < this.m_portModelsChunks.Length; ++index)
        {
          this.IoPortsRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>(this.m_portModelsChunks[index].ValueOrNull);
          this.m_portModelsChunks[index] = Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>.None;
        }
        this.ClearAllHighlights();
        for (int index = 0; index < this.m_highlightModelsChunks.Length; ++index)
        {
          this.IoPortsRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>(this.m_highlightModelsChunks[index].ValueOrNull);
          this.m_highlightModelsChunks[index] = Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>.None;
        }
        this.ClearAllArrows();
        this.IoPortsRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>(this.m_arrowsChunk);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
      {
        for (int index = 0; index < this.m_portModelsChunks.Length; ++index)
        {
          InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> valueOrNull = this.m_portModelsChunks[index].ValueOrNull;
          if (valueOrNull != null)
          {
            IoPortShapeProto ioPortShapeProto = this.IoPortsRenderer.m_renderIndexToProto[index];
            info.Add(new RenderedInstancesInfo(string.Format("Ports ({0})", (object) ioPortShapeProto.Id), valueOrNull.InstancesCount, valueOrNull.IndicesCountForLod0));
          }
        }
        for (int index = 0; index < this.m_highlightModelsChunks.Length; ++index)
        {
          InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> valueOrNull = this.m_highlightModelsChunks[index].ValueOrNull;
          if (valueOrNull != null)
          {
            IoPortShapeProto ioPortShapeProto = this.IoPortsRenderer.m_renderIndexToProto[index];
            info.Add(new RenderedInstancesInfo(string.Format("Port highlights ({0})", (object) ioPortShapeProto.Id), valueOrNull.InstancesCount, valueOrNull.IndicesCountForLod0));
          }
        }
      }

      /// <summary>
      /// Shows port and returns instance ID. Returned ID is never zero (zero is considered "invalid" id).
      /// </summary>
      public uint ShowPortVisual(IoPortVisual visual)
      {
        int index = visual.IsEndPort ? visual.ShapeProto.Graphics.RendererIndexDisconnected : visual.ShapeProto.Graphics.RendererIndexConnected;
        InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> instance = this.m_portModelsChunks[index].ValueOrNull;
        if (instance == null)
        {
          KeyValuePair<Mesh[], Material> keyValuePair = this.IoPortsRenderer.m_portsMeshMatData[index];
          instance = new InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>(keyValuePair.Key, InstancingUtils.InstantiateMaterialAndCopyTextures(this.IoPortsRenderer.m_materialForPortsInstancedRenderingShared, keyValuePair.Value));
          this.IoPortsRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
          this.m_portModelsChunks[index] = (Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>) instance;
        }
        ushort instanceId = instance.AddInstance(new IoPortsRenderer.PortsChunkBase.PortInstanceData(visual.Position.ToGroundCenterVector3(), visual.Direction, visual.BlueprintColor));
        ++this.InstancesCount;
        return IoPortsRenderer.PortsChunkBase.packInstanceIdAndPortModelIndex(instanceId, index);
      }

      private static uint packInstanceIdAndPortModelIndex(ushort instanceId, int portModelIndex)
      {
        return (uint) ((int) instanceId << 16 | portModelIndex + 1);
      }

      protected bool TryUnpackInstanceIdAndPortModelIndex(
        uint packed,
        out ushort instanceId,
        out int portModelIndex)
      {
        instanceId = (ushort) (packed >> 16);
        portModelIndex = ((int) packed & (int) ushort.MaxValue) - 1;
        return (long) (uint) portModelIndex < (long) this.m_portModelsChunks.Length;
      }

      protected bool TryUnpackVisualId(
        uint visualId,
        out InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> chunk,
        out ushort instanceId,
        out int portModelIndex)
      {
        if (!this.TryUnpackInstanceIdAndPortModelIndex(visualId, out instanceId, out portModelIndex))
        {
          Mafi.Log.Error(string.Format("Invalid ID {0}, port model index is {1}.", (object) visualId, (object) portModelIndex));
          chunk = (InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>) null;
          return false;
        }
        chunk = this.m_portModelsChunks[portModelIndex].ValueOrNull;
        if (chunk != null)
          return true;
        Mafi.Log.Error(string.Format("Failed to updated rendered port ID {0}, its chunk was not found.", (object) visualId));
        return false;
      }

      /// <summary>
      /// Hides port visual based on ID obtained from <see cref="M:Mafi.Unity.Ports.Io.IoPortsRenderer.PortsChunkBase.ShowPortVisual(Mafi.Unity.Ports.Io.IoPortVisual)" />.
      /// </summary>
      public void HidePortVisual(uint visualId)
      {
        InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> chunk;
        ushort instanceId;
        int portModelIndex;
        if (!this.TryUnpackVisualId(visualId, out chunk, out instanceId, out portModelIndex))
        {
          Mafi.Log.Error(string.Format("Invalid ID {0}, port model index is {1}.", (object) visualId, (object) portModelIndex));
        }
        else
        {
          chunk.RemoveInstance(instanceId);
          --this.InstancesCount;
        }
      }

      protected RenderStats RenderPorts(Bounds bounds, int lodLevel)
      {
        if (this.InstancesCount <= 0)
          return new RenderStats();
        RenderStats renderStats = new RenderStats();
        foreach (Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>> portModelsChunk in this.m_portModelsChunks)
        {
          if (portModelsChunk.HasValue && portModelsChunk.Value.InstancesCount > 0)
            renderStats += portModelsChunk.Value.Render(bounds, lodLevel);
        }
        return renderStats;
      }

      protected RenderStats RenderPortsHighlights()
      {
        if (this.m_highlightInstancesCount <= 0 && this.m_registeredForHighlightingCount <= 0)
          return new RenderStats();
        RenderStats renderStats = new RenderStats();
        for (int index = 0; index < this.m_highlightModelsChunks.Length; ++index)
        {
          InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> valueOrNull = this.m_highlightModelsChunks[index].ValueOrNull;
          if (valueOrNull != null)
          {
            if (valueOrNull.InstancesCount > 0)
            {
              if (!this.m_isRegisteredForHighlighting[index])
              {
                this.m_isRegisteredForHighlighting[index] = true;
                ++this.m_registeredForHighlightingCount;
                this.IoPortsRenderer.m_highlighter.AddCustomHighlightsRenderer((ICustomHighlightsRenderer) valueOrNull);
              }
              renderStats += new RenderStats(1, valueOrNull.InstancesCount, valueOrNull.RenderedInstancesCount, valueOrNull.RenderedInstancesCount * valueOrNull.IndicesCountForLod0);
            }
            else if (this.m_isRegisteredForHighlighting[index])
            {
              this.m_isRegisteredForHighlighting[index] = false;
              --this.m_registeredForHighlightingCount;
              this.IoPortsRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) valueOrNull);
            }
          }
        }
        return renderStats;
      }

      protected RenderStats RenderArrows(Bounds bounds, int lodLevel)
      {
        return this.m_arrowsChunk.InstancesCount <= 0 ? new RenderStats() : this.m_arrowsChunk.Render(bounds, lodLevel);
      }

      protected void ClearAllArrows() => this.m_arrowsChunk.Clear();

      protected void ClearAllHighlights()
      {
        for (int index = 0; index < this.m_highlightModelsChunks.Length; ++index)
        {
          InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> valueOrNull = this.m_highlightModelsChunks[index].ValueOrNull;
          if (valueOrNull != null)
          {
            valueOrNull.Clear();
            if (this.m_isRegisteredForHighlighting[index])
            {
              this.m_isRegisteredForHighlighting[index] = false;
              --this.m_registeredForHighlightingCount;
              this.IoPortsRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) valueOrNull);
            }
          }
        }
      }

      protected PortHighlightData ShowHighlight(
        PortHighlightSpec highlightSpec,
        Vector3 portPosition,
        Direction903d direction,
        int portModelIndex,
        Option<IoPort> showColliderFor)
      {
        int num1 = 0;
        int num2 = 0;
        if (highlightSpec.ArrowSpec.HasValue)
        {
          Vector3 position = portPosition + IoPortsRenderer.PORT_ARROW_OFFSETS[direction.DirectionIndex];
          switch (highlightSpec.ArrowSpec.Value.Type)
          {
            case IoPortType.Any:
              num1 = 1 + (int) this.m_arrowsChunk.AddInstance(new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, direction, IoPortsRenderer.INOUT_PORT_COLOR));
              num2 = 1 + (int) this.m_arrowsChunk.AddInstance(new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, -direction, IoPortsRenderer.INOUT_PORT_COLOR));
              break;
            case IoPortType.Input:
              num1 = 1 + (int) this.m_arrowsChunk.AddInstance(new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, direction, IoPortsRenderer.IN_PORT_COLOR));
              break;
            case IoPortType.Output:
              num1 = 1 + (int) this.m_arrowsChunk.AddInstance(new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, -direction, IoPortsRenderer.OUT_PORT_COLOR));
              break;
            default:
              Mafi.Log.Warning("Unknown arrow type.");
              goto case IoPortType.Any;
          }
        }
        Option<GameObject> option1 = new Option<GameObject>();
        if (highlightSpec.ActivatePortCollider)
        {
          if (showColliderFor.HasValue)
            option1 = (Option<GameObject>) this.IoPortsRenderer.showColliderFor(showColliderFor.Value);
          else
            Mafi.Log.Warning("Failed to show collider, no port was given.");
        }
        Option<GameObject> option2 = new Option<GameObject>();
        if (highlightSpec.IconSpec.HasValue)
          option2 = (Option<GameObject>) this.IoPortsRenderer.m_iconProvider.GetMultiIconPooled(highlightSpec.IconSpec.Value, this.m_iconsParentTransform, portPosition + IoPortsRenderer.PORT_ICON_OFFSETS[direction.DirectionIndex]);
        uint num3 = 0;
        if (highlightSpec.HighlightColor.HasValue)
        {
          InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> instance = this.m_highlightModelsChunks[portModelIndex].ValueOrNull;
          if (instance == null)
          {
            instance = new InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>(this.IoPortsRenderer.m_portsMeshMatData[portModelIndex].Key, UnityEngine.Object.Instantiate<Material>(this.IoPortsRenderer.m_materialForPortHighlightsInstancedRenderingShared));
            this.IoPortsRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            this.m_highlightModelsChunks[portModelIndex] = (Option<InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData>>) instance;
          }
          num3 = IoPortsRenderer.PortsChunkBase.packInstanceIdAndPortModelIndex(instance.AddInstance(new IoPortsRenderer.PortsChunkBase.PortInstanceData(portPosition, direction, highlightSpec.HighlightColor.Value)), portModelIndex);
          ++this.m_highlightInstancesCount;
        }
        Option<GameObject> iconGo = option2;
        Option<GameObject> colliderGo = option1;
        ref readonly ArrowSpec? local = ref highlightSpec.ArrowSpec;
        int portType = local.HasValue ? (int) local.GetValueOrDefault().Type : 1;
        int arrowId1 = num1;
        int arrowId2 = num2;
        int highlightInstanceId = (int) num3;
        return new PortHighlightData(iconGo, colliderGo, (IoPortType) portType, arrowId1, arrowId2, (uint) highlightInstanceId);
      }

      public void UpdateHighlight(
        PortHighlightData existingHighlight,
        Vector3 portPosition,
        Direction903d direction)
      {
        Vector3 position = portPosition + IoPortsRenderer.PORT_ARROW_OFFSETS[direction.DirectionIndex];
        switch (existingHighlight.PortType)
        {
          case IoPortType.Any:
            if (existingHighlight.ArrowId1 > 0)
              this.m_arrowsChunk.UpdateInstance((ushort) (existingHighlight.ArrowId1 - 1), new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, direction, IoPortsRenderer.INOUT_PORT_COLOR));
            if (existingHighlight.ArrowId2 > 0)
            {
              this.m_arrowsChunk.UpdateInstance((ushort) (existingHighlight.ArrowId2 - 1), new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, -direction, IoPortsRenderer.INOUT_PORT_COLOR));
              break;
            }
            break;
          case IoPortType.Input:
            if (existingHighlight.ArrowId1 > 0)
            {
              this.m_arrowsChunk.UpdateInstance((ushort) (existingHighlight.ArrowId1 - 1), new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, direction, IoPortsRenderer.IN_PORT_COLOR));
              break;
            }
            break;
          case IoPortType.Output:
            if (existingHighlight.ArrowId1 > 0)
            {
              this.m_arrowsChunk.UpdateInstance((ushort) (existingHighlight.ArrowId1 - 1), new IoPortsRenderer.PortsChunkBase.PortInstanceData(position, -direction, IoPortsRenderer.OUT_PORT_COLOR));
              break;
            }
            break;
          default:
            Mafi.Log.Warning("Unknown arrow type.");
            goto case IoPortType.Any;
        }
        if (existingHighlight.IconGo.HasValue)
        {
          Vector3 vector3 = portPosition + IoPortsRenderer.PORT_ICON_OFFSETS[direction.DirectionIndex];
          existingHighlight.IconGo.Value.transform.localPosition = vector3;
        }
        if (existingHighlight.HighlightInstanceId <= 0U)
          return;
        ushort instanceId;
        int portModelIndex;
        if (!this.TryUnpackInstanceIdAndPortModelIndex(existingHighlight.HighlightInstanceId, out instanceId, out portModelIndex))
        {
          Mafi.Log.Error(string.Format("Invalid instance highlight ID {0},", (object) existingHighlight.HighlightInstanceId) + string.Format(" data index is {0}.", (object) portModelIndex));
        }
        else
        {
          InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> valueOrNull = this.m_highlightModelsChunks[portModelIndex].ValueOrNull;
          if (valueOrNull == null)
          {
            Mafi.Log.Warning("No highlight found for updating.");
          }
          else
          {
            IoPortsRenderer.PortsChunkBase.PortInstanceData data = valueOrNull.GetData(instanceId);
            valueOrNull.UpdateInstance(instanceId, new IoPortsRenderer.PortsChunkBase.PortInstanceData(portPosition, direction, new ColorRgba(data.Data & 4294967274U)));
          }
        }
      }

      public void RemoveHighlight(PortHighlightData hlData)
      {
        if (hlData.ArrowId1 > 0)
          this.m_arrowsChunk.RemoveInstance((ushort) (hlData.ArrowId1 - 1));
        if (hlData.ArrowId2 > 0)
          this.m_arrowsChunk.RemoveInstance((ushort) (hlData.ArrowId2 - 1));
        if (hlData.IconGo.HasValue)
        {
          GameObject iconGo = hlData.IconGo.Value;
          this.IoPortsRenderer.m_iconProvider.ReturnIconToPool(ref iconGo);
        }
        if (hlData.ColliderGo.HasValue)
          this.IoPortsRenderer.clearCollider(hlData.ColliderGo.Value);
        if (hlData.HighlightInstanceId <= 0U)
          return;
        ushort instanceId;
        int portModelIndex;
        if (!this.TryUnpackInstanceIdAndPortModelIndex(hlData.HighlightInstanceId, out instanceId, out portModelIndex))
        {
          Mafi.Log.Error(string.Format("Invalid instance highlight ID {0},", (object) hlData.HighlightInstanceId) + string.Format(" port model index is {0}.", (object) portModelIndex));
        }
        else
        {
          InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> valueOrNull = this.m_highlightModelsChunks[portModelIndex].ValueOrNull;
          if (valueOrNull == null)
          {
            Mafi.Log.Warning("No highlight found for updating.");
          }
          else
          {
            valueOrNull.RemoveInstance(instanceId);
            --this.m_highlightInstancesCount;
          }
        }
      }

      [ExpectedStructSize(16)]
      protected readonly struct PortInstanceData
      {
        public readonly Vector3 Position;
        public readonly uint Data;

        public PortInstanceData(Vector3 position, uint data)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Position = position;
          this.Data = data;
        }

        public PortInstanceData(Vector3 position, Direction903d direction, ColorRgba color)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Position = position;
          this.Data = (uint) ((int) color.Rgba & -8 | direction.DirectionIndex);
        }
      }
    }

    /// <summary>Chunk of rendered ports.</summary>
    private sealed class PortsChunkStandard : 
      IoPortsRenderer.PortsChunkBase,
      IRenderedChunk,
      IRenderedChunksBase
    {
      private readonly Set<IoPort> m_ports;
      private readonly Dict<IoPort, PortHighlightData> m_displayedPortHighlights;
      private readonly IoPortsRenderer.HighlightComputationRequest m_highlightComputationRequest;
      private HighlightId m_requestedHighlightId;
      private IRenderedChunksParent m_chunkParent;
      private bool m_highlightsActive;
      private int m_currentHighlightVersion;
      private Bounds m_bounds;
      private float m_minHeight;
      private float m_maxHeight;

      public string Name => "IoPorts";

      public Chunk256AndIndex CoordAndIndex { get; }

      public Vector2 Origin { get; }

      public bool TrackStoppedRendering => true;

      public float MaxModelDeviationFromChunkBounds => 2f;

      public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

      public PortsChunkStandard(Chunk256AndIndex coordAndIndex, IoPortsRenderer ioPortsRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_ports = new Set<IoPort>();
        this.m_displayedPortHighlights = new Dict<IoPort, PortHighlightData>();
        // ISSUE: explicit constructor call
        base.\u002Ector(ioPortsRenderer);
        this.CoordAndIndex = coordAndIndex;
        this.Origin = coordAndIndex.ChunkCoord.OriginTile2i.ToVector2();
        this.m_highlightComputationRequest = new IoPortsRenderer.HighlightComputationRequest(this);
      }

      public void Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

      public void AddPortInstance(IoPort port)
      {
        this.AddPortInstanceKeepHidden(port);
        this.ShowPort(port);
      }

      public void AddPortInstanceKeepHidden(IoPort port)
      {
        this.m_ports.AddAndAssertNew(port);
        this.m_requestedHighlightId = new HighlightId(-1);
      }

      public void RemovePortInstance(IoPort port)
      {
        this.HidePort(port);
        this.m_ports.RemoveAndAssert(port);
        PortHighlightData hlData;
        if (!this.m_displayedPortHighlights.TryRemove(port, out hlData))
          return;
        this.RemoveHighlight(hlData);
      }

      public void ShowPort(IoPort port)
      {
        if (port.RendererId != 0U)
          return;
        ColorRgba color;
        InstancedChunkBasedLayoutEntitiesRenderer.GetBlueprintColor((IStaticEntity) port.OwnerEntity, out color);
        uint num = this.ShowPortVisual(new IoPortVisual(port.Position, port.Direction, port.ShapePrototype, port.IsEndPort, color));
        port.RendererId = num;
        float unityUnits = (float) port.Position.Height.ToUnityUnits();
        if (this.InstancesCount == 1)
        {
          this.m_minHeight = this.m_maxHeight = unityUnits;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        else if ((double) unityUnits < (double) this.m_minHeight)
        {
          this.m_minHeight = unityUnits;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        else
        {
          if ((double) unityUnits <= (double) this.m_maxHeight)
            return;
          this.m_maxHeight = unityUnits;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
      }

      public void HidePort(IoPort port)
      {
        if (port.RendererId == 0U)
          return;
        this.HidePortVisual(port.RendererId);
        port.RendererId = 0U;
      }

      public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
      {
        if (lod > 5)
        {
          if (this.m_highlightsActive)
            this.clearHighlights();
          return new RenderStats();
        }
        RenderStats renderStats = new RenderStats();
        if (lod <= 3)
        {
          if (this.IoPortsRenderer.m_currentHighlightRequestId != this.m_requestedHighlightId)
          {
            this.m_requestedHighlightId = this.IoPortsRenderer.m_currentHighlightRequestId;
            if (this.IoPortsRenderer.m_currentHighlightRequestId == HighlightId.Empty)
            {
              this.m_highlightComputationRequest.Clear();
            }
            else
            {
              this.m_highlightComputationRequest.RequestComputation(this.IoPortsRenderer.m_currentHighlightRequest, cameraDistance, (IEnumerable<IoPort>) this.m_ports);
              this.IoPortsRenderer.m_chunkHighlightRequests[this] = this.m_highlightComputationRequest;
            }
          }
          if (this.m_currentHighlightVersion != this.m_highlightComputationRequest.ResultHighlightVersion)
          {
            this.clearHighlights();
            this.m_currentHighlightVersion = this.m_highlightComputationRequest.ResultHighlightVersion;
            if (this.IoPortsRenderer.m_currentHighlightRequestId != HighlightId.Empty)
            {
              this.m_highlightsActive = true;
              foreach (KeyValuePair<IoPort, PortHighlightSpec> highlightResult in this.m_highlightComputationRequest.HighlightResults)
                this.showAndRegisterHighlightForPort(highlightResult.Key, highlightResult.Value);
            }
          }
          renderStats += this.RenderArrows(this.m_bounds, lod) + this.RenderPortsHighlights();
        }
        else if (this.m_highlightsActive)
          this.clearHighlights();
        return renderStats + this.RenderPorts(this.m_bounds, lod);
      }

      public void NotifyWasNotRendered() => this.clearHighlights();

      private void showAndRegisterHighlightForPort(IoPort port, PortHighlightSpec spec)
      {
        if (port.OwnerEntity.IsDestroyed)
          return;
        bool exists;
        ref PortHighlightData local = ref this.m_displayedPortHighlights.GetRefValue(port, out exists);
        if (exists)
          this.RemoveHighlight(local);
        int portModelIndex = port.IsEndPort ? port.ShapePrototype.Graphics.RendererIndexDisconnected : port.ShapePrototype.Graphics.RendererIndexConnected;
        local = this.ShowHighlight(spec, port.Position.ToGroundCenterVector3(), port.Direction, portModelIndex, (Option<IoPort>) port);
      }

      private void clearHighlights()
      {
        this.m_highlightsActive = false;
        this.m_currentHighlightVersion = 0;
        this.ClearAllArrows();
        this.ClearAllHighlights();
        foreach (PortHighlightData portHighlightData in this.m_displayedPortHighlights.Values)
        {
          if (portHighlightData.IconGo.HasValue)
          {
            GameObject iconGo = portHighlightData.IconGo.Value;
            this.IoPortsRenderer.m_iconProvider.ReturnIconToPool(ref iconGo);
          }
          if (portHighlightData.ColliderGo.HasValue)
            this.IoPortsRenderer.clearCollider(portHighlightData.ColliderGo.Value);
        }
        this.m_displayedPortHighlights.Clear();
      }

      public void PauseHighlightsFor(IoPort port, bool keepCollider)
      {
        if (!this.m_highlightsActive)
          return;
        if (keepCollider)
        {
          this.showAndRegisterHighlightForPort(port, new PortHighlightSpec(activatePortCollider: true));
        }
        else
        {
          PortHighlightData hlData;
          if (!this.m_displayedPortHighlights.TryRemove(port, out hlData))
            return;
          this.RemoveHighlight(hlData);
        }
      }

      public void RestoreHighlightsFor(IoPort port)
      {
        if (!this.m_highlightsActive)
          return;
        foreach (KeyValuePair<IoPort, PortHighlightSpec> highlightResult in this.m_highlightComputationRequest.HighlightResults)
        {
          if (highlightResult.Key == port)
          {
            this.showAndRegisterHighlightForPort(highlightResult.Key, highlightResult.Value);
            break;
          }
        }
      }
    }

    /// <summary>All immediate ports (there is only a single chunk).</summary>
    private sealed class PortsChunkImmediate : 
      IoPortsRenderer.PortsChunkBase,
      IRenderedChunkAlwaysVisible,
      IRenderedChunksBase
    {
      public string Name => "IoPorts immediate";

      public PortsChunkImmediate(IoPortsRenderer ioPortsRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(ioPortsRenderer);
      }

      /// <summary>Updates existing visual.</summary>
      public void UpdateVisual(
        uint visualId,
        Tile3i position,
        Direction903d rotation,
        ColorRgba blueprintColor)
      {
        InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> chunk;
        ushort instanceId;
        if (!this.TryUnpackVisualId(visualId, out chunk, out instanceId, out int _))
          return;
        chunk.UpdateInstance(instanceId, new IoPortsRenderer.PortsChunkBase.PortInstanceData(position.ToGroundCenterVector3(), rotation, blueprintColor));
      }

      public PortHighlightData HighlightPortVisual(uint visualId, PortHighlightSpec highlightSpec)
      {
        InstancedMeshesRenderer<IoPortsRenderer.PortsChunkBase.PortInstanceData> chunk;
        ushort instanceId;
        int portModelIndex;
        if (!this.TryUnpackVisualId(visualId, out chunk, out instanceId, out portModelIndex))
          return new PortHighlightData();
        IoPortsRenderer.PortsChunkBase.PortInstanceData data = chunk.GetData(instanceId);
        return this.ShowHighlight(highlightSpec, data.Position, new Direction903d((int) data.Data & 7), portModelIndex, Option<IoPort>.None);
      }

      public RenderStats RenderAlwaysVisible(GameTime time, Bounds bounds)
      {
        return this.RenderArrows(bounds, 0) + this.RenderPorts(bounds, 0) + this.RenderPortsHighlights();
      }
    }

    /// <summary>
    /// Handles synchronization of highlight requests. Note that we may have one pending request and one processed
    /// request at the same time, so we need to keep more fields to be able to distinguish.
    /// </summary>
    private class HighlightComputationRequest
    {
      public readonly IoPortsRenderer.PortsChunkStandard ParentChunk;
      private IoPortsRenderer.HighlightRequest m_requestOnMain;
      private IoPortsRenderer.HighlightRequest m_requestOnSim;
      private Lyst<IoPort> m_requestedPortsOnMain;
      private Lyst<IoPort> m_requestedPortsOnSim;
      private int m_requestVersionOnMain;
      private int m_requestVersionOnSim;
      private Lyst<KeyValuePair<IoPort, PortHighlightSpec>> m_highlightResultsOnMain;
      private Lyst<KeyValuePair<IoPort, PortHighlightSpec>> m_highlightResultsOnSim;
      private int m_resultVersionOnMain;
      private int m_resultVersionOnSim;

      public float DistanceToCamera { get; private set; }

      public int ResultHighlightVersion => this.m_resultVersionOnMain;

      public IIndexable<KeyValuePair<IoPort, PortHighlightSpec>> HighlightResults
      {
        get => (IIndexable<KeyValuePair<IoPort, PortHighlightSpec>>) this.m_highlightResultsOnMain;
      }

      public HighlightComputationRequest(IoPortsRenderer.PortsChunkStandard parentChunk)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_requestedPortsOnMain = new Lyst<IoPort>();
        this.m_requestedPortsOnSim = new Lyst<IoPort>();
        this.m_highlightResultsOnMain = new Lyst<KeyValuePair<IoPort, PortHighlightSpec>>();
        this.m_highlightResultsOnSim = new Lyst<KeyValuePair<IoPort, PortHighlightSpec>>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.ParentChunk = parentChunk;
      }

      public void RequestComputation(
        IoPortsRenderer.HighlightRequest request,
        float distanceToCamera,
        IEnumerable<IoPort> ports)
      {
        this.DistanceToCamera = distanceToCamera;
        this.m_requestedPortsOnMain.Clear();
        this.m_requestedPortsOnMain.AddRange(ports);
        ++this.m_requestVersionOnMain;
        this.m_requestOnMain = request;
      }

      public void SyncDataForSim()
      {
        Swap.Them<Lyst<IoPort>>(ref this.m_requestedPortsOnMain, ref this.m_requestedPortsOnSim);
        Swap.Them<IoPortsRenderer.HighlightRequest>(ref this.m_requestOnMain, ref this.m_requestOnSim);
        this.m_requestVersionOnSim = this.m_requestVersionOnMain;
      }

      public void SyncResultsFromSim()
      {
        Swap.Them<Lyst<KeyValuePair<IoPort, PortHighlightSpec>>>(ref this.m_highlightResultsOnMain, ref this.m_highlightResultsOnSim);
        this.m_resultVersionOnMain = this.m_resultVersionOnSim;
      }

      public void PerformComputationOnSim()
      {
        Mafi.Assert.That<int>(this.m_resultVersionOnSim).IsNotEqualTo(this.m_requestVersionOnSim, "Already computed?");
        Predicate<IoPort> predicate = this.m_requestOnSim.Predicate;
        if (predicate == null)
          return;
        this.m_highlightResultsOnSim.Clear();
        this.m_resultVersionOnSim = this.m_requestVersionOnSim;
        IoPortsRenderer ioPortsRenderer = this.ParentChunk.IoPortsRenderer;
        IoPortsManager portsManager = ioPortsRenderer.m_portsManager;
        TerrainOccupancyManager occupancyManager = ioPortsRenderer.m_occupancyManager;
        Predicate<EntityId> pillarsPredicate = ioPortsRenderer.m_transportsPredicates.IgnorePillarsPredicate;
        PortProductsResolver productsResolver = ioPortsRenderer.m_portProductsResolver;
        foreach (IoPort port in this.m_requestedPortsOnSim)
        {
          if (predicate(port) && !port.OwnerEntity.IsDestroyed)
          {
            Option<IoPort> option = portsManager[port.ExpectedConnectedPortCoord, -port.Direction];
            MultiIconSpec? nullable;
            if (!port.IsConnected && option.HasValue)
            {
              Mafi.Assert.That<bool>(option.Value.IsConnected).IsFalse();
              Lyst<KeyValuePair<IoPort, PortHighlightSpec>> highlightResultsOnSim = this.m_highlightResultsOnSim;
              IoPort key = port;
              ColorRgba? highlightColor = new ColorRgba?(IoPortsRenderer.HIGHLIGHT_ERR);
              nullable = new MultiIconSpec?(IoPortsRenderer.ICON_PORT_CAN_NOT_CONNECT);
              ArrowSpec? arrowSpec = new ArrowSpec?();
              MultiIconSpec? iconSpec = nullable;
              PortHighlightSpec portHighlightSpec = new PortHighlightSpec(highlightColor, arrowSpec, iconSpec);
              highlightResultsOnSim.Add<IoPort, PortHighlightSpec>(key, portHighlightSpec);
            }
            else if (!port.IsConnected && occupancyManager.IsOccupiedAt(port.ExpectedConnectedPortCoord, pillarsPredicate))
            {
              if (!port.OwnerEntity.Prototype.Graphics.HideBlockedPortsIcon)
              {
                Lyst<KeyValuePair<IoPort, PortHighlightSpec>> highlightResultsOnSim = this.m_highlightResultsOnSim;
                IoPort key = port;
                ColorRgba? highlightColor = new ColorRgba?(IoPortsRenderer.HIGHLIGHT_ERR);
                nullable = new MultiIconSpec?(IoPortsRenderer.ICON_PORT_BLOCKED);
                ArrowSpec? arrowSpec = new ArrowSpec?();
                MultiIconSpec? iconSpec = nullable;
                PortHighlightSpec portHighlightSpec = new PortHighlightSpec(highlightColor, arrowSpec, iconSpec);
                highlightResultsOnSim.Add<IoPort, PortHighlightSpec>(key, portHighlightSpec);
              }
            }
            else
            {
              ImmutableArray<ProductProto> portProducts = productsResolver.GetPortProducts(port, fallbackToUnlockedIfNoRecipesAssigned: true);
              Lyst<KeyValuePair<IoPort, PortHighlightSpec>> highlightResultsOnSim = this.m_highlightResultsOnSim;
              IoPort key = port;
              ColorRgba? highlightColor = new ColorRgba?(IoPortsRenderer.HIGHLIGHT_OK);
              ArrowSpec? arrowSpec = port.IsConnected ? new ArrowSpec?() : new ArrowSpec?(new ArrowSpec(port.Type));
              MultiIconSpec? iconSpec;
              if (!portProducts.IsNotEmpty)
              {
                nullable = new MultiIconSpec?();
                iconSpec = nullable;
              }
              else
                iconSpec = new MultiIconSpec?(new MultiIconSpec(portProducts.Map<string>((Func<ProductProto, string>) (x => x.Graphics.IconPath))));
              int num = !this.m_requestOnSim.WithoutColliders ? 1 : 0;
              PortHighlightSpec portHighlightSpec = new PortHighlightSpec(highlightColor, arrowSpec, iconSpec, num != 0);
              highlightResultsOnSim.Add<IoPort, PortHighlightSpec>(key, portHighlightSpec);
            }
          }
        }
      }

      public void Clear()
      {
        this.m_requestedPortsOnMain.Clear();
        this.m_highlightResultsOnMain.Clear();
        this.m_requestOnMain = new IoPortsRenderer.HighlightRequest();
        this.m_requestOnSim = new IoPortsRenderer.HighlightRequest();
        this.m_requestVersionOnMain = 0;
        this.m_requestVersionOnSim = 0;
        this.m_resultVersionOnMain = 0;
        this.m_resultVersionOnSim = 0;
      }
    }

    private enum EventType
    {
      Add,
      AddKeepHidden,
      Remove,
      Show,
      Hide,
    }

    private readonly struct HighlightRequest
    {
      public readonly HighlightId RequestId;
      public readonly Predicate<IoPort> Predicate;
      public readonly bool WithoutColliders;

      public HighlightRequest(
        HighlightId requestId,
        Predicate<IoPort> predicate,
        bool withoutColliders)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.RequestId = requestId;
        this.Predicate = predicate;
        this.WithoutColliders = withoutColliders;
      }
    }
  }
}
