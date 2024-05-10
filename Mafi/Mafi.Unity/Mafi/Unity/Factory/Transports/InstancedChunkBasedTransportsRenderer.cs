// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.InstancedChunkBasedTransportsRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Simulation;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  /// <summary>
  /// Renders transports using chunk-based instanced rendering.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class InstancedChunkBasedTransportsRenderer : DelayedEntitiesRenderer, IDisposable
  {
    public static readonly ColorRgba NO_BLUEPRINT_COLOR;
    public static readonly ColorRgba BLUEPRINT_UNDECIDABLE_COLOR;
    public static readonly ColorRgba BLUEPRINT_PAUSED_COLOR;
    public static readonly ColorRgba BLUEPRINT_CONSTRUCTION_COLOR;
    public static readonly ColorRgba BLUEPRINT_DECONSTRUCTION_COLOR;
    private readonly TransportModelFactory m_modelFactory;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly AssetsDb m_assetsDb;
    private readonly ObjectHighlighter m_highlighter;
    private readonly EntityMbFactory m_entityMbFactory;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly EntitiesIconRenderer m_entitiesIconRenderer;
    /// <summary>
    /// Parent GO that has all the entities to avoid mess in the scene root.
    /// </summary>
    private readonly GameObject m_parentGo;
    private readonly Dict<Mafi.Core.Factory.Transports.Transport, TransportMb> m_transportMbs;
    private readonly Dict<GameObject, Mafi.Core.Factory.Transports.Transport> m_goToTransport;
    private bool m_transportMbsChanged;
    private LystStruct<InstancedChunkBasedTransportsRenderer.TransportUpdateData> m_transportUpdateDataTmp1;
    private LystStruct<InstancedChunkBasedTransportsRenderer.TransportUpdateData> m_transportUpdateDataTmp2;
    private LystStruct<InstancedChunkBasedTransportsRenderer.TransportUpdateData> m_transportUpdateDataTmp3;
    private InstancedChunkBasedTransportsRenderer.TransportUpdateData[] m_transportsWithMovementCache;
    private InstancedChunkBasedTransportsRenderer.TransportUpdateData[] m_transportsWithFlowIndicatorsCache;
    private InstancedChunkBasedTransportsRenderer.TransportUpdateData[] m_transportsWithPipeColorsCache;
    private int[] m_transportsMovedStepsCache;
    private BitMap m_transportsFlowUpdateCache;
    private BitMap m_pipeColorUpdateCache;
    private readonly Material m_normalMaterialForInstancedRenderingShared;
    private readonly Material m_blueprintMaterialForInstancedRenderingShared;
    private readonly Material m_highlightMaterialForInstancedRenderingShared;
    private readonly Material m_connectorMaterialForInstancedRenderingShared;
    private readonly Material m_blueprintConnectorMaterialForInstancedRenderingShared;
    private readonly Material m_staticFrameFlowIndicatorMaterialForInstancedRenderingShared;
    private readonly Material m_blueprintStaticFrameFlowIndicatorMaterialForInstancedRenderingShared;
    private readonly Material m_staticGlassFlowIndicatorMaterialForInstancedRenderingShared;
    private readonly Material m_dynamicFlowIndicatorMaterialForInstancedRenderingShared;
    private readonly Material m_highlightMaterialForFlowIndicatorInstancedRenderingShared;
    private readonly Lyst<InstancedChunkBasedTransportsRenderer.TransportRenderingData> m_transportRenderingData;
    private ArrayDataStorage<Lyst<TransportHighlightInfo>> m_activeHighlightInfo;
    private readonly Dict<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<TransportRenderingSection, uint>>> m_transportSections;
    private readonly Dict<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<TransportConnectorPose, uint>>> m_transportConnectors;
    private readonly Dict<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<TransportFlowIndicatorPose, uint>>> m_transportFlowIndicators;
    private readonly List<IDestroyableEntityMb> m_destroyableMbsTmp;

    public override int Priority => 100;

    public InstancedChunkBasedTransportsRenderer(
      EntitiesRenderingManager entitiesRenderingManager,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      TransportModelFactory modelFactory,
      ChunkBasedRenderingManager visibleChunksRenderer,
      AssetsDb assetsDb,
      ObjectHighlighter highlighter,
      EntityMbFactory entityMbFactory,
      ReloadAfterAssetUpdateManager reloadManager,
      EntitiesIconRenderer entitiesIconRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_parentGo = new GameObject("Transports");
      this.m_transportMbs = new Dict<Mafi.Core.Factory.Transports.Transport, TransportMb>();
      this.m_goToTransport = new Dict<GameObject, Mafi.Core.Factory.Transports.Transport>();
      this.m_transportsWithMovementCache = Array.Empty<InstancedChunkBasedTransportsRenderer.TransportUpdateData>();
      this.m_transportsWithFlowIndicatorsCache = Array.Empty<InstancedChunkBasedTransportsRenderer.TransportUpdateData>();
      this.m_transportsWithPipeColorsCache = Array.Empty<InstancedChunkBasedTransportsRenderer.TransportUpdateData>();
      this.m_transportsMovedStepsCache = Array.Empty<int>();
      this.m_transportRenderingData = new Lyst<InstancedChunkBasedTransportsRenderer.TransportRenderingData>()
      {
        (InstancedChunkBasedTransportsRenderer.TransportRenderingData) null
      };
      this.m_transportSections = new Dict<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<TransportRenderingSection, uint>>>();
      this.m_transportConnectors = new Dict<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<TransportConnectorPose, uint>>>();
      this.m_transportFlowIndicators = new Dict<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<TransportFlowIndicatorPose, uint>>>();
      this.m_destroyableMbsTmp = new List<IDestroyableEntityMb>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_chunksRenderer = visibleChunksRenderer;
      this.m_assetsDb = assetsDb;
      this.m_highlighter = highlighter;
      this.m_entityMbFactory = entityMbFactory;
      this.m_reloadManager = reloadManager;
      this.m_entitiesIconRenderer = entitiesIconRenderer;
      this.m_normalMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportInstanced.mat");
      this.m_blueprintMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportInstancedBlueprint.mat");
      this.m_highlightMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportHighlightInstanced.mat");
      this.m_connectorMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportConnectorInstanced.mat");
      this.m_blueprintConnectorMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportConnectorInstancedBlueprint.mat");
      this.m_staticFrameFlowIndicatorMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportFlowIndicatorFrameInstanced.mat");
      this.m_blueprintStaticFrameFlowIndicatorMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportFlowIndicatorFrameBlueprintInstanced.mat");
      this.m_staticGlassFlowIndicatorMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportFlowIndicatorGlassInstanced.mat");
      this.m_dynamicFlowIndicatorMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportFlowIndicatorFlowInstanced.mat");
      this.m_highlightMaterialForFlowIndicatorInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Transports/TransportFlowIndicatorHighlightFrameInstanced.mat");
      entitiesRenderingManager.RegisterRenderer((IEntitiesRenderer) this);
      gameLoopEvents.Terminate.AddNonSaveable<InstancedChunkBasedTransportsRenderer>(this, new Action(this.onTerminated));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<InstancedChunkBasedTransportsRenderer>(this, new Action(this.updateSimEnd));
    }

    public void Dispose()
    {
      for (int index = 1; index < this.m_transportRenderingData.Count; ++index)
        this.m_transportRenderingData[index].Dispose();
      this.m_transportMbs.Clear();
      this.m_goToTransport.Clear();
      this.m_transportSections.Clear();
      this.m_transportFlowIndicators.Clear();
      this.m_transportsWithMovementCache = Array.Empty<InstancedChunkBasedTransportsRenderer.TransportUpdateData>();
      this.m_transportsWithFlowIndicatorsCache = Array.Empty<InstancedChunkBasedTransportsRenderer.TransportUpdateData>();
    }

    public override bool CanRenderEntity(EntityProto proto)
    {
      return proto is TransportProto transportProto && transportProto.Graphics.UseInstancedRendering;
    }

    private ushort getOrCreateEntityRenderingData(TransportProto transportProto)
    {
      if (transportProto.Graphics.InstancedRenderingData.IsNone)
      {
        Log.Error(string.Format("No instanced rendering data on {0}", (object) transportProto));
        return 0;
      }
      ushort entityRenderingData = transportProto.Graphics.InstancedRenderingData.Value.InstancedRendererIndex;
      if (entityRenderingData == (ushort) 0)
      {
        if (this.m_transportRenderingData.Count >= (int) ushort.MaxValue)
        {
          Log.Error(string.Format("Too many rendered protos, only {0} supported.", (object) ushort.MaxValue));
          return ushort.MaxValue;
        }
        entityRenderingData = (ushort) this.m_transportRenderingData.Count;
        transportProto.Graphics.InstancedRenderingData.Value.InstancedRendererIndex = entityRenderingData;
        this.m_transportRenderingData.Add(new InstancedChunkBasedTransportsRenderer.TransportRenderingData(this, this.m_modelFactory, transportProto));
      }
      return entityRenderingData;
    }

    public override void AddEntityOnSync(IRenderedEntity entity, GameTime time)
    {
      if (!(entity is Mafi.Core.Factory.Transports.Transport transport))
      {
        Log.Error("Expected Transport");
      }
      else
      {
        if ((entity.RendererData & 72057594037927935UL) != 0UL)
        {
          Log.Error("Entity already added? Removing first.");
          this.RemoveEntityOnSync(entity, time, EntityRemoveReason.Remove);
        }
        ushort entityRenderingData = this.getOrCreateEntityRenderingData(transport.Prototype);
        this.m_transportRenderingData[(int) entityRenderingData].AddTransport(transport);
        if (transport.IsDestroyed)
        {
          Log.Warning(string.Format("Adding destroyed entity {0} in `AddEntityOnSync`, ignoring.", (object) transport));
        }
        else
        {
          TransportMb mbFor = this.m_entityMbFactory.CreateMbFor<Mafi.Core.Factory.Transports.Transport>(transport) as TransportMb;
          if ((UnityEngine.Object) mbFor == (UnityEngine.Object) null)
          {
            Log.Warning(string.Format("Failed to create entity MB for '{0}'.", (object) transport));
          }
          else
          {
            this.m_transportMbs.AddAndAssertNew(transport, mbFor);
            this.m_goToTransport.AddAndAssertNew(mbFor.gameObject, transport);
            this.m_transportMbsChanged = true;
            if (!mbFor.IsInitialized)
            {
              Assert.Fail(string.Format("Adding non-initialized mb for '{0}' {1}.", (object) transport, (object) mbFor.GetType()));
            }
            else
            {
              uint num = 1;
              entity.RendererData = (ulong) ((long) entity.RendererData & -72057594037927936L | (long) num | (long) entityRenderingData << 40);
            }
          }
        }
      }
    }

    public override void UpdateEntityOnSync(IRenderedEntity entity, GameTime time)
    {
      this.RemoveEntityOnSync(entity, time, EntityRemoveReason.Remove);
      this.AddEntityOnSync(entity, time);
    }

    public override void RemoveEntityOnSync(
      IRenderedEntity entity,
      GameTime time,
      EntityRemoveReason reason)
    {
      this.m_entitiesIconRenderer.UpdateVisualsImmediate(entity);
      if (!(entity is Mafi.Core.Factory.Transports.Transport transport))
      {
        Log.Error("Expected Transport");
      }
      else
      {
        ulong num1 = entity.RendererData & 72057594037927935UL;
        if (num1 == 0UL)
        {
          Log.Error("Entity already removed?");
        }
        else
        {
          uint index = (uint) (num1 >> 40);
          entity.RendererData &= 18374686479671623680UL;
          if ((long) index >= (long) this.m_transportRenderingData.Count)
          {
            Log.Error(string.Format("Invalid entity data index '{0}'", (object) index));
          }
          else
          {
            bool wasBlueprint;
            this.m_transportRenderingData[index].RemoveTransport(transport, out wasBlueprint);
            TransportMb transportMb;
            if (!this.m_transportMbs.TryRemove(transport, out transportMb))
            {
              Log.Warning(string.Format("Trying to remove non-existent transport '{0}' from renderer, ignoring.", (object) transport.Id));
            }
            else
            {
              this.m_goToTransport.RemoveAndAssert(transportMb.gameObject);
              this.m_transportMbsChanged = true;
              if (reason == EntityRemoveReason.Collapse)
              {
                int num2 = wasBlueprint ? 1 : 0;
              }
              transportMb.gameObject.Destroy();
            }
          }
        }
      }
    }

    public override bool TryGetPickableEntityAs<T>(GameObject pickedGo, out T entity)
    {
      Mafi.Core.Factory.Transports.Transport transport;
      if (this.m_goToTransport.TryGetValue(pickedGo, out transport))
      {
        entity = transport as T;
        return (object) entity != null;
      }
      entity = default (T);
      return false;
    }

    public override ulong AddHighlight(IRenderedEntity entity, ColorRgba color)
    {
      if (!(entity is Mafi.Core.Factory.Transports.Transport transport))
      {
        Log.Error("Expected transport");
        return 0;
      }
      ushort entityRenderingData = this.getOrCreateEntityRenderingData(transport.Prototype);
      return (ulong) this.m_transportRenderingData[(int) entityRenderingData].AddHighlight(transport, color) | (ulong) entityRenderingData << 32;
    }

    public override void RemoveHighlight(ulong highlightId)
    {
      this.m_transportRenderingData[(int) (highlightId >> 32)].RemoveHighlight((ushort) highlightId);
    }

    public static bool GetBlueprintColor(IStaticEntity entity, out ColorRgba color)
    {
      if (entity.ConstructionState == ConstructionState.NotInitialized)
      {
        color = InstancedChunkBasedTransportsRenderer.BLUEPRINT_UNDECIDABLE_COLOR;
        return true;
      }
      IEntityConstructionProgress valueOrNull = entity.ConstructionProgress.ValueOrNull;
      bool flag1 = valueOrNull == null;
      if (!flag1)
      {
        bool flag2;
        switch (entity.ConstructionState)
        {
          case ConstructionState.PreparingUpgrade:
          case ConstructionState.BeingUpgraded:
          case ConstructionState.PendingDeconstruction:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = flag2;
      }
      if (flag1)
      {
        color = InstancedChunkBasedTransportsRenderer.NO_BLUEPRINT_COLOR;
        return false;
      }
      color = !valueOrNull.IsPaused ? (!valueOrNull.IsDeconstruction ? InstancedChunkBasedTransportsRenderer.BLUEPRINT_CONSTRUCTION_COLOR : InstancedChunkBasedTransportsRenderer.BLUEPRINT_DECONSTRUCTION_COLOR) : InstancedChunkBasedTransportsRenderer.BLUEPRINT_PAUSED_COLOR;
      return true;
    }

    public override void SyncUpdate(GameTime time)
    {
      base.SyncUpdate(time);
      if (this.m_transportMbsChanged)
      {
        this.m_transportMbsChanged = false;
        this.m_transportUpdateDataTmp1.Clear();
        this.m_transportUpdateDataTmp2.Clear();
        foreach (KeyValuePair<Mafi.Core.Factory.Transports.Transport, TransportMb> transportMb in this.m_transportMbs)
        {
          if (transportMb.Key.IsDestroyed)
            Log.WarningOnce(string.Format("Trying to render destroyed transport: {0}", (object) transportMb.Key));
          else if (!(bool) (UnityEngine.Object) transportMb.Value)
          {
            Log.WarningOnce(string.Format("Trying to transport with destroyed MB: {0}", (object) transportMb.Key));
          }
          else
          {
            if (transportMb.Key.Prototype.Graphics.CrossSection.MovingCrossSectionParts.IsNotEmpty)
            {
              ushort entityRenderingData = this.getOrCreateEntityRenderingData(transportMb.Key.Prototype);
              if (entityRenderingData < ushort.MaxValue)
                this.m_transportUpdateDataTmp1.Add(new InstancedChunkBasedTransportsRenderer.TransportUpdateData(transportMb.Key, transportMb.Value, this.m_transportRenderingData[(int) entityRenderingData]));
            }
            if (transportMb.Value.FlowIndicatorStates.IsNotEmpty)
            {
              ushort entityRenderingData = this.getOrCreateEntityRenderingData(transportMb.Key.Prototype);
              if (entityRenderingData < ushort.MaxValue)
                this.m_transportUpdateDataTmp2.Add(new InstancedChunkBasedTransportsRenderer.TransportUpdateData(transportMb.Key, transportMb.Value, this.m_transportRenderingData[(int) entityRenderingData]));
            }
            if (transportMb.Value.ColorIsFromProducts)
            {
              ushort entityRenderingData = this.getOrCreateEntityRenderingData(transportMb.Key.Prototype);
              if (entityRenderingData < ushort.MaxValue)
                this.m_transportUpdateDataTmp3.Add(new InstancedChunkBasedTransportsRenderer.TransportUpdateData(transportMb.Key, transportMb.Value, this.m_transportRenderingData[(int) entityRenderingData]));
            }
          }
        }
        this.m_transportsWithMovementCache = this.m_transportUpdateDataTmp1.ToArray();
        this.m_transportsWithFlowIndicatorsCache = this.m_transportUpdateDataTmp2.ToArray();
        this.m_transportsWithPipeColorsCache = this.m_transportUpdateDataTmp3.ToArray();
        Array.Resize<int>(ref this.m_transportsMovedStepsCache, this.m_transportsWithMovementCache.Length);
        Array.Clear((Array) this.m_transportsMovedStepsCache, 0, this.m_transportsMovedStepsCache.Length);
        this.m_transportsFlowUpdateCache = new BitMap(this.m_transportsWithFlowIndicatorsCache.Length);
        this.m_pipeColorUpdateCache = new BitMap(this.m_transportsWithPipeColorsCache.Length);
        this.m_transportUpdateDataTmp1.Clear();
        this.m_transportUpdateDataTmp2.Clear();
        this.m_transportUpdateDataTmp3.Clear();
      }
      if (!time.IsGamePaused)
      {
        for (int index = 0; index < this.m_transportsWithMovementCache.Length; ++index)
        {
          InstancedChunkBasedTransportsRenderer.TransportUpdateData transportUpdateData = this.m_transportsWithMovementCache[index];
          transportUpdateData.Mb.SyncTextureOffsets();
          bool forceUpdate = false;
          if ((double) transportUpdateData.Mb.MovedOffset == 0.0)
          {
            if (this.m_transportsMovedStepsCache[index] != transportUpdateData.Transport.MovedStepsTotal)
            {
              this.m_transportsMovedStepsCache[index] = transportUpdateData.Transport.MovedStepsTotal;
              forceUpdate = true;
            }
            else
              continue;
          }
          transportUpdateData.RenderingData.UpdateMovement(transportUpdateData.Transport, transportUpdateData.Mb.MovementTextureOffsetFrom, transportUpdateData.Mb.MovementTextureOffsetTo, forceUpdate);
        }
      }
      for (int index = 0; index < this.m_transportsWithPipeColorsCache.Length; ++index)
      {
        InstancedChunkBasedTransportsRenderer.TransportUpdateData transportUpdateData = this.m_transportsWithPipeColorsCache[index];
        if (transportUpdateData.Mb.ArePipeColorsDirty)
          this.m_pipeColorUpdateCache.ClearBit(index);
        else if (!this.m_pipeColorUpdateCache.IsSet(index))
          this.m_pipeColorUpdateCache.SetBit(index);
        else
          continue;
        transportUpdateData.RenderingData.UpdatePipeColors(transportUpdateData.Transport, transportUpdateData.Mb.PipeColor, transportUpdateData.Mb.PipeAccentColor);
      }
      for (int index = 0; index < this.m_transportsWithFlowIndicatorsCache.Length; ++index)
      {
        InstancedChunkBasedTransportsRenderer.TransportUpdateData transportUpdateData = this.m_transportsWithFlowIndicatorsCache[index];
        if (transportUpdateData.Mb.AreFlowIndicatorsDirty)
          this.m_transportsFlowUpdateCache.ClearBit(index);
        else if (!this.m_transportsFlowUpdateCache.IsSet(index))
          this.m_transportsFlowUpdateCache.SetBit(index);
        else
          continue;
        transportUpdateData.RenderingData.UpdateFlowIndicators(time, transportUpdateData.Transport, transportUpdateData.Mb.FlowIndicatorStates);
      }
    }

    private void updateSimEnd()
    {
      foreach (IEntityMbWithSimUpdateEnd withSimUpdateEnd in this.m_transportMbs.Values)
        withSimUpdateEnd.SimUpdateEnd();
    }

    private void destroyMbs(TransportMb entityMb)
    {
      Assert.That<bool>(entityMb.IsDestroyed).IsFalse();
      this.m_destroyableMbsTmp.Clear();
      entityMb.gameObject.GetComponentsInChildren<IDestroyableEntityMb>(true, this.m_destroyableMbsTmp);
      foreach (IDestroyableEntityMb destroyableEntityMb in this.m_destroyableMbsTmp)
        destroyableEntityMb.Destroy();
      this.m_destroyableMbsTmp.Clear();
      Assert.That<bool>(entityMb.IsDestroyed).IsTrue();
      entityMb.gameObject.Destroy();
    }

    private void onTerminated()
    {
      foreach (TransportMb entityMb in this.m_transportMbs.Values)
        this.destroyMbs(entityMb);
    }

    static InstancedChunkBasedTransportsRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      InstancedChunkBasedTransportsRenderer.NO_BLUEPRINT_COLOR = InstancedChunkBasedLayoutEntitiesRenderer.NO_BLUEPRINT_COLOR;
      InstancedChunkBasedTransportsRenderer.BLUEPRINT_UNDECIDABLE_COLOR = InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_UNDECIDABLE_COLOR;
      InstancedChunkBasedTransportsRenderer.BLUEPRINT_PAUSED_COLOR = (ColorRgba) 8947848;
      InstancedChunkBasedTransportsRenderer.BLUEPRINT_CONSTRUCTION_COLOR = (ColorRgba) 7833787;
      InstancedChunkBasedTransportsRenderer.BLUEPRINT_DECONSTRUCTION_COLOR = InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_DECONSTRUCTION_COLOR;
    }

    private readonly struct TransportUpdateData
    {
      public readonly Mafi.Core.Factory.Transports.Transport Transport;
      public readonly TransportMb Mb;
      public readonly InstancedChunkBasedTransportsRenderer.TransportRenderingData RenderingData;

      public TransportUpdateData(
        Mafi.Core.Factory.Transports.Transport transport,
        TransportMb mb,
        InstancedChunkBasedTransportsRenderer.TransportRenderingData renderingData)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Transport = transport;
        this.Mb = mb;
        this.RenderingData = renderingData;
      }
    }

    /// <summary>
    /// Handles rendering of a set of transports based on their proto. Each transport proto has a separate instance.
    /// </summary>
    private class TransportRenderingData : IDisposable
    {
      public readonly TransportProto TransportProto;
      private readonly InstancedChunkBasedTransportsRenderer m_parentRenderer;
      private readonly TransportModelFactory m_modelFactory;
      private readonly Lyst<Mesh[]> m_sharedStaticMeshesLods;
      private readonly Lyst<Mesh[]> m_sharedMovingMeshesLods;
      private Mesh[] m_sharedConnectorMeshesLods;
      private readonly Mesh[] m_sharedStaticFlowFrameMeshesLods;
      private readonly Mesh[] m_sharedStaticFlowGlassMeshesLods;
      private readonly Mesh[] m_sharedDynamicFlowMeshesLods;
      private readonly Lyst<KeyValuePair<long, Mesh>> m_staticMeshes;
      private readonly Lyst<KeyValuePair<long, Mesh>> m_movingMeshes;
      private readonly Material m_sharedMaterialOriginal;
      private readonly Material m_sharedMaterialConnector;
      private readonly Material m_sharedMaterialFlowIndicatorFrame;
      private readonly Material m_sharedMaterialFlowIndicatorGlass;
      private readonly Material m_sharedMaterialFlowIndicatorFlow;
      private float m_maxSize;
      private readonly Option<InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk>[] m_chunks;
      [ThreadStatic]
      private static readonly Lyst<TransportRenderingSection> s_newTransportSectionsTmp;
      [ThreadStatic]
      private static readonly Lyst<TransportConnectorPose> s_newConnectorsTmp;

      public TransportRenderingData(
        InstancedChunkBasedTransportsRenderer parentRenderer,
        TransportModelFactory modelFactory,
        TransportProto proto)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_sharedStaticMeshesLods = new Lyst<Mesh[]>();
        this.m_sharedMovingMeshesLods = new Lyst<Mesh[]>();
        this.m_staticMeshes = new Lyst<KeyValuePair<long, Mesh>>();
        this.m_movingMeshes = new Lyst<KeyValuePair<long, Mesh>>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_parentRenderer = parentRenderer;
        this.m_modelFactory = modelFactory;
        this.TransportProto = proto;
        this.m_chunks = new Option<InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk>[this.m_parentRenderer.m_chunksRenderer.ChunksCountTotal];
        this.m_sharedMaterialOriginal = this.m_parentRenderer.m_assetsDb.GetSharedMaterial(proto.Graphics.MaterialPath);
        string error1;
        if (proto.Graphics.VerticalConnectorPrefabPath.HasValue && !InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_parentRenderer.m_assetsDb, proto.Graphics.VerticalConnectorPrefabPath.Value, out this.m_sharedConnectorMeshesLods, out this.m_sharedMaterialConnector, out error1))
        {
          Log.Error(string.Format("Failed to load prefab for connector '{0}': {1}", (object) proto.Id, (object) error1));
          this.m_sharedConnectorMeshesLods = new Mesh[7];
          for (int index = 0; index < this.m_sharedConnectorMeshesLods.Length; ++index)
            this.m_sharedConnectorMeshesLods[index] = this.m_parentRenderer.m_assetsDb.DefaultMesh;
        }
        if (proto.Graphics.FlowIndicator.HasValue)
        {
          string error2;
          if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_parentRenderer.m_assetsDb, proto.Graphics.FlowIndicator.Value.FramePrefabPath, out this.m_sharedStaticFlowFrameMeshesLods, out this.m_sharedMaterialFlowIndicatorFrame, out error2))
          {
            Log.Error(string.Format("Failed to load prefab for flow indicator '{0}': {1}", (object) proto.Id, (object) error2));
            this.m_sharedStaticFlowFrameMeshesLods = new Mesh[7];
            for (int index = 0; index < this.m_sharedStaticFlowFrameMeshesLods.Length; ++index)
              this.m_sharedStaticFlowFrameMeshesLods[index] = this.m_parentRenderer.m_assetsDb.DefaultMesh;
          }
          if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_parentRenderer.m_assetsDb, proto.Graphics.FlowIndicator.Value.GlassPrefabPath, out this.m_sharedStaticFlowGlassMeshesLods, out this.m_sharedMaterialFlowIndicatorGlass, out error2))
          {
            Log.Error(string.Format("Failed to load prefab for flow indicator '{0}': {1}", (object) proto.Id, (object) error2));
            this.m_sharedStaticFlowGlassMeshesLods = new Mesh[7];
            for (int index = 0; index < this.m_sharedStaticFlowGlassMeshesLods.Length; ++index)
              this.m_sharedStaticFlowGlassMeshesLods[index] = this.m_parentRenderer.m_assetsDb.DefaultMesh;
          }
          if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_parentRenderer.m_assetsDb, proto.Graphics.FlowIndicator.Value.FlowPrefabPath, out this.m_sharedDynamicFlowMeshesLods, out this.m_sharedMaterialFlowIndicatorFlow, out error2))
          {
            Log.Error(string.Format("Failed to load prefab for flow indicator '{0}': {1}", (object) proto.Id, (object) error2));
            this.m_sharedDynamicFlowMeshesLods = new Mesh[7];
            for (int index = 0; index < this.m_sharedDynamicFlowMeshesLods.Length; ++index)
              this.m_sharedDynamicFlowMeshesLods[index] = this.m_parentRenderer.m_assetsDb.DefaultMesh;
          }
        }
        this.m_maxSize = 0.0f;
      }

      public void Dispose()
      {
        foreach (Option<InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk> chunk in this.m_chunks)
          chunk.ValueOrNull?.Dispose();
      }

      private InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk getOrCreateChunk(
        TransportRenderingSection section)
      {
        return this.getOrCreateChunk(this.m_parentRenderer.m_chunksRenderer.GetChunkIndex(section.Position.ToTile3f().Tile2i));
      }

      private InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk getOrCreateChunk(
        TransportFlowIndicatorPose flowIndicatorPose)
      {
        return this.getOrCreateChunk(this.m_parentRenderer.m_chunksRenderer.GetChunkIndex(flowIndicatorPose.Position.Tile2i));
      }

      private InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk getOrCreateChunk(
        TransportConnectorPose connectorPose)
      {
        return this.getOrCreateChunk(this.m_parentRenderer.m_chunksRenderer.GetChunkIndex(connectorPose.Position.Tile2i));
      }

      private InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk getOrCreateChunk(
        Chunk256Index index)
      {
        InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk newChunk = this.m_chunks[(int) index.Value].ValueOrNull;
        if (newChunk == null)
        {
          this.m_chunks[(int) index.Value] = (Option<InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk>) (newChunk = new InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk(this.m_parentRenderer.m_chunksRenderer.ExtendChunkCoord(index), this));
          this.m_parentRenderer.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
        }
        return newChunk;
      }

      public void AddTransport(Mafi.Core.Factory.Transports.Transport transport)
      {
        Assert.That<Lyst<KeyValuePair<long, Mesh>>>(this.m_staticMeshes).IsNotNull<Lyst<KeyValuePair<long, Mesh>>>();
        Assert.That<Lyst<KeyValuePair<long, Mesh>>>(this.m_movingMeshes).IsNotNull<Lyst<KeyValuePair<long, Mesh>>>();
        InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newTransportSectionsTmp.Clear();
        InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp.Clear();
        this.m_modelFactory.GetTransportMeshData(transport.Trajectory, this.m_staticMeshes, this.m_movingMeshes, InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newTransportSectionsTmp, InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp);
        KeyValuePair<long, Mesh> keyValuePair;
        int count;
        Bounds bounds;
        while (this.m_sharedStaticMeshesLods.Count < this.m_staticMeshes.Count)
        {
          Mesh[] meshArray1 = new Mesh[7];
          for (int index1 = 0; index1 < meshArray1.Length; ++index1)
          {
            Mesh[] meshArray2 = meshArray1;
            int index2 = index1;
            keyValuePair = this.m_staticMeshes[this.m_sharedStaticMeshesLods.Count];
            Mesh mesh = keyValuePair.Value;
            meshArray2[index2] = mesh;
          }
          Mesh mesh1 = meshArray1[0];
          string name = mesh1.name;
          count = this.m_sharedStaticMeshesLods.Count;
          string str = count.ToString();
          mesh1.name = name + str;
          keyValuePair = this.m_staticMeshes[this.m_sharedStaticMeshesLods.Count];
          bounds = keyValuePair.Value.bounds;
          Vector3 size = bounds.size;
          this.m_maxSize = this.m_maxSize.Max(2f * size.x.Max(size.y).Max(size.z));
          this.m_sharedStaticMeshesLods.Add(meshArray1);
        }
        while (this.m_sharedMovingMeshesLods.Count < this.m_movingMeshes.Count)
        {
          Mesh[] meshArray3 = new Mesh[7];
          for (int index3 = 0; index3 < meshArray3.Length; ++index3)
          {
            Mesh[] meshArray4 = meshArray3;
            int index4 = index3;
            keyValuePair = this.m_movingMeshes[this.m_sharedMovingMeshesLods.Count];
            Mesh mesh = keyValuePair.Value;
            meshArray4[index4] = mesh;
          }
          Mesh mesh2 = meshArray3[0];
          string name = mesh2.name;
          count = this.m_sharedMovingMeshesLods.Count;
          string str = count.ToString();
          mesh2.name = name + str;
          keyValuePair = this.m_movingMeshes[this.m_sharedMovingMeshesLods.Count];
          bounds = keyValuePair.Value.bounds;
          Vector3 size = bounds.size;
          this.m_maxSize = this.m_maxSize.Max(2f * size.x.Max(size.y).Max(size.z));
          this.m_sharedMovingMeshesLods.Add(meshArray3);
        }
        ImmutableArrayBuilder<Pair<TransportRenderingSection, uint>> immutableArrayBuilder1 = new ImmutableArrayBuilder<Pair<TransportRenderingSection, uint>>(InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newTransportSectionsTmp.Count);
        for (int index = 0; index < InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newTransportSectionsTmp.Count; ++index)
        {
          TransportRenderingSection renderingSection = InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newTransportSectionsTmp[index];
          uint second = this.getOrCreateChunk(renderingSection).AddInstances(transport, renderingSection);
          immutableArrayBuilder1[index] = Pair.Create<TransportRenderingSection, uint>(renderingSection, second);
        }
        if (InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp.Count > 0)
        {
          ImmutableArrayBuilder<Pair<TransportConnectorPose, uint>> immutableArrayBuilder2 = new ImmutableArrayBuilder<Pair<TransportConnectorPose, uint>>(InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp.Count);
          for (int index = 0; index < InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp.Count; ++index)
          {
            TransportConnectorPose transportConnectorPose = InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp[index];
            uint second = this.getOrCreateChunk(transportConnectorPose).AddConnector(transport, transportConnectorPose);
            immutableArrayBuilder2[index] = Pair.Create<TransportConnectorPose, uint>(transportConnectorPose, second);
          }
          if (!this.m_parentRenderer.m_transportConnectors.TryAdd(transport, immutableArrayBuilder2.GetImmutableArrayAndClear()))
            Log.Warning("Adding already existing transport connectors. Overwriting.");
        }
        if (!this.m_parentRenderer.m_transportSections.TryAdd(transport, immutableArrayBuilder1.GetImmutableArrayAndClear()))
          Log.Warning("Adding already existing transport. Overwriting.");
        InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newTransportSectionsTmp.Clear();
        InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp.Clear();
        ImmutableArray<TransportFlowIndicatorPose> immutableArray = this.TransportProto.Graphics.FlowIndicator.IsNone ? ImmutableArray<TransportFlowIndicatorPose>.Empty : transport.Trajectory.FlowIndicatorsPoses;
        ImmutableArrayBuilder<Pair<TransportFlowIndicatorPose, uint>> immutableArrayBuilder3 = new ImmutableArrayBuilder<Pair<TransportFlowIndicatorPose, uint>>(immutableArray.Length);
        for (int index = 0; index < immutableArray.Length; ++index)
        {
          TransportFlowIndicatorPose flowIndicatorPose = immutableArray[index];
          uint second = this.getOrCreateChunk(flowIndicatorPose).AddFlowIndicator(transport, flowIndicatorPose);
          immutableArrayBuilder3[index] = Pair.Create<TransportFlowIndicatorPose, uint>(flowIndicatorPose, second);
        }
        if (this.m_parentRenderer.m_transportFlowIndicators.TryAdd(transport, immutableArrayBuilder3.GetImmutableArrayAndClear()))
          return;
        Log.Warning("Adding already existing flow indicators. Overwriting.");
      }

      public void UpdateInstance(Mafi.Core.Factory.Transports.Transport transport, ulong instanceId)
      {
        throw new NotImplementedException();
      }

      public void RemoveTransport(Mafi.Core.Factory.Transports.Transport transport, out bool wasBlueprint)
      {
        wasBlueprint = false;
        ImmutableArray<Pair<TransportRenderingSection, uint>> immutableArray1;
        if (!this.m_parentRenderer.m_transportSections.TryGetValue(transport, out immutableArray1))
        {
          Log.Warning("Transport sections data not found.");
        }
        else
        {
          foreach (Pair<TransportRenderingSection, uint> pair in immutableArray1)
            this.getOrCreateChunk(pair.First).RemoveInstances(pair.Second, pair.First);
          this.m_parentRenderer.m_transportSections.Remove(transport);
          ImmutableArray<Pair<TransportConnectorPose, uint>> immutableArray2;
          if (this.m_parentRenderer.m_transportConnectors.TryGetValue(transport, out immutableArray2))
          {
            foreach (Pair<TransportConnectorPose, uint> pair in immutableArray2)
              this.getOrCreateChunk(pair.First).RemoveConnector(pair.Second);
            this.m_parentRenderer.m_transportConnectors.Remove(transport);
          }
          ImmutableArray<Pair<TransportFlowIndicatorPose, uint>> immutableArray3;
          if (!this.m_parentRenderer.m_transportFlowIndicators.TryGetValue(transport, out immutableArray3))
            return;
          foreach (Pair<TransportFlowIndicatorPose, uint> pair in immutableArray3)
            this.getOrCreateChunk(pair.First).RemoveFlowIndicator(pair.Second);
          this.m_parentRenderer.m_transportFlowIndicators.Remove(transport);
        }
      }

      public uint AddHighlight(Mafi.Core.Factory.Transports.Transport transport, ColorRgba color)
      {
        ImmutableArray<Pair<TransportRenderingSection, uint>> immutableArray1;
        if (!this.m_parentRenderer.m_transportSections.TryGetValue(transport, out immutableArray1))
        {
          Log.Warning("Transport sections data not found.");
          return 0;
        }
        Lyst<TransportHighlightInfo> lyst = new Lyst<TransportHighlightInfo>();
        foreach (Pair<TransportRenderingSection, uint> pair in immutableArray1)
        {
          InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk chunk = this.getOrCreateChunk(pair.First);
          uint num = chunk.AddHighlight(transport, pair.First, color);
          if (num >= 65536U)
          {
            if (num == uint.MaxValue)
              Log.Error(string.Format("Invalid instance id {0}. No highlight added?", (object) num));
            else
              Log.Error(string.Format("Invalid instance id {0}. Must be less than 1 << 16.", (object) num));
          }
          else
          {
            uint instanceId = num | (uint) chunk.CoordAndIndex.ChunkIndex.Value << 16;
            TransportHighlightInfo transportHighlightInfo = new TransportHighlightInfo(pair.First.StaticMeshIdx, pair.First.MovingMeshIdx, false, instanceId);
            lyst.Add(transportHighlightInfo);
          }
        }
        ImmutableArray<Pair<TransportFlowIndicatorPose, uint>> immutableArray2;
        if (!this.m_parentRenderer.m_transportFlowIndicators.TryGetValue(transport, out immutableArray2))
        {
          Log.Warning("Transport flow indicator data not found.");
          return 0;
        }
        foreach (Pair<TransportFlowIndicatorPose, uint> pair in immutableArray2)
        {
          InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk chunk = this.getOrCreateChunk(pair.First);
          TransportHighlightInfo transportHighlightInfo = new TransportHighlightInfo((short) -1, (short) -1, true, chunk.AddFlowIndicatorHighlight(pair.First, color) | (uint) chunk.CoordAndIndex.ChunkIndex.Value << 16);
          lyst.Add(transportHighlightInfo);
        }
        return (uint) this.m_parentRenderer.m_activeHighlightInfo.Add(lyst);
      }

      public void RemoveHighlight(ushort highlightGroup)
      {
        foreach (TransportHighlightInfo instanceInfo in (IIndexable<TransportHighlightInfo>) this.m_parentRenderer.m_activeHighlightInfo[(int) highlightGroup])
        {
          ushort instanceId = (ushort) instanceInfo.InstanceId;
          this.getOrCreateChunk(new Chunk256Index((ushort) (instanceInfo.InstanceId >> 16 & (uint) ushort.MaxValue))).RemoveHighlight(instanceInfo, (uint) instanceId);
        }
        this.m_parentRenderer.m_activeHighlightInfo.Remove((int) highlightGroup);
      }

      public void AddCollapsingInstance(Mafi.Core.Factory.Transports.Transport entity, SimStep currentStepSinceLoad)
      {
      }

      public void UpdateMovement(
        Mafi.Core.Factory.Transports.Transport transport,
        float offsetFrom,
        float offsetTo,
        bool forceUpdate)
      {
        if (InstancedChunkBasedTransportsRenderer.GetBlueprintColor((IStaticEntity) transport, out ColorRgba _))
          return;
        ImmutableArray<Pair<TransportRenderingSection, uint>> immutableArray;
        if (!this.m_parentRenderer.m_transportSections.TryGetValue(transport, out immutableArray))
        {
          Log.WarningOnce("Transport sections data not found.");
        }
        else
        {
          foreach (Pair<TransportRenderingSection, uint> pair in immutableArray)
          {
            ushort second = (ushort) pair.Second;
            this.getOrCreateChunk(pair.First).UpdateMovement(second, pair.First, offsetFrom, offsetTo, forceUpdate);
          }
        }
      }

      public void UpdatePipeColors(Mafi.Core.Factory.Transports.Transport transport, ColorRgba color, ColorRgba accentColor)
      {
        if (InstancedChunkBasedTransportsRenderer.GetBlueprintColor((IStaticEntity) transport, out ColorRgba _))
          return;
        ImmutableArray<Pair<TransportRenderingSection, uint>> immutableArray1;
        if (!this.m_parentRenderer.m_transportSections.TryGetValue(transport, out immutableArray1))
        {
          Log.WarningOnce("Transport sections data not found.");
        }
        else
        {
          foreach (Pair<TransportRenderingSection, uint> pair in immutableArray1)
          {
            ushort second = (ushort) pair.Second;
            this.getOrCreateChunk(pair.First).UpdatePipeColor(second, pair.First, color, accentColor);
          }
          ImmutableArray<Pair<TransportFlowIndicatorPose, uint>> immutableArray2;
          if (!this.m_parentRenderer.m_transportFlowIndicators.TryGetValue(transport, out immutableArray2))
          {
            Log.WarningOnce("Transport flow indicator data not found.");
          }
          else
          {
            for (int index = 0; index < immutableArray2.Length; ++index)
              this.getOrCreateChunk(immutableArray2[index].First).UpdateFlowIndicatorColor((ushort) immutableArray2[index].Second, color);
          }
        }
      }

      public void UpdateFlowIndicators(
        GameTime time,
        Mafi.Core.Factory.Transports.Transport transport,
        ReadOnlyArray<InstancedFluidIndicatorState> flowStates)
      {
        if (InstancedChunkBasedTransportsRenderer.GetBlueprintColor((IStaticEntity) transport, out ColorRgba _))
          return;
        ImmutableArray<Pair<TransportFlowIndicatorPose, uint>> immutableArray;
        if (!this.m_parentRenderer.m_transportFlowIndicators.TryGetValue(transport, out immutableArray))
        {
          Log.WarningOnce("Transport flow indicator data not found.");
        }
        else
        {
          for (int index = 0; index < immutableArray.Length; ++index)
          {
            InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk chunk = this.getOrCreateChunk(immutableArray[index].First);
            ushort second = (ushort) immutableArray[index].Second;
            chunk.UpdateFlowIndicator(time, second, flowStates[index]);
          }
        }
      }

      static TransportRenderingData()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newTransportSectionsTmp = new Lyst<TransportRenderingSection>();
        InstancedChunkBasedTransportsRenderer.TransportRenderingData.s_newConnectorsTmp = new Lyst<TransportConnectorPose>();
      }

      private sealed class StandardChunk : IRenderedChunk, IRenderedChunksBase, IDisposable
      {
        public static readonly int MAX_STATIC_MESH_LOD;
        public static readonly int MAX_FLOW_INDICATOR_LOD;
        public static readonly int MAX_MOVEMENT_UPDATES_LOD;
        private readonly InstancedChunkBasedTransportsRenderer.TransportRenderingData m_parentData;
        private readonly int m_maxLod;
        private int m_lastRenderedLod;
        private IRenderedChunksParent m_chunkParent;
        private readonly Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> m_staticChunks;
        private readonly Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> m_movingChunks;
        private readonly Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> m_blueprintStaticChunks;
        private readonly Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> m_blueprintMovingChunks;
        private readonly Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> m_highlightStaticChunks;
        private readonly Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> m_highlightMovingChunks;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>> m_connectorChunk;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>> m_connectorBlueprintChunk;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>> m_staticFlowFrameChunk;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>> m_staticFlowGlassChunk;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>> m_staticFlowFrameBlueprintChunk;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>> m_staticFlowGlassBlueprintChunk;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorDynamicInstanceData>> m_dynamicFlowChunk;
        private Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>> m_staticFlowFrameHighlightChunk;
        private readonly Dict<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>, bool> m_registeredForHighlight;
        private readonly Dict<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>, bool> m_registeredFlowIndicatorForHighlight;
        private static readonly int FLOW_INDICATOR_OFFSET_SCALE_SHADER_ID;
        private Bounds m_bounds;
        private float m_minHeight;
        private float m_maxHeight;

        public string Name => this.m_parentData.TransportProto.Id.Value;

        public Vector2 Origin { get; }

        public Chunk256AndIndex CoordAndIndex { get; }

        public int InstancesCount { get; private set; }

        public bool TrackStoppedRendering => false;

        public float MaxModelDeviationFromChunkBounds => this.m_parentData.m_maxSize;

        public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

        public StandardChunk(
          Chunk256AndIndex coordAndIndex,
          InstancedChunkBasedTransportsRenderer.TransportRenderingData parentData)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.m_staticChunks = new Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>();
          this.m_movingChunks = new Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>();
          this.m_blueprintStaticChunks = new Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>();
          this.m_blueprintMovingChunks = new Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>();
          this.m_highlightStaticChunks = new Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>();
          this.m_highlightMovingChunks = new Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>();
          this.m_registeredForHighlight = new Dict<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>, bool>();
          this.m_registeredFlowIndicatorForHighlight = new Dict<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>, bool>();
          // ISSUE: explicit constructor call
          base.\u002Ector();
          this.CoordAndIndex = coordAndIndex;
          this.Origin = this.CoordAndIndex.OriginTile2i.ToVector2();
          this.m_parentData = parentData;
          this.m_maxLod = this.m_parentData.TransportProto.Graphics.MaxRenderedLod.Clamp(0, 6);
        }

        public void Dispose()
        {
          ReloadAfterAssetUpdateManager reloadManager = this.m_parentData.m_parentRenderer.m_reloadManager;
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instance in this.m_staticChunks.Values)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>(instance);
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instance in this.m_movingChunks.Values)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>(instance);
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instance in this.m_blueprintStaticChunks.Values)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>(instance);
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instance in this.m_blueprintMovingChunks.Values)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>(instance);
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_highlightStaticChunks.Values)
          {
            if (this.m_registeredForHighlight[instancedMeshesRenderer])
            {
              this.m_registeredForHighlight[instancedMeshesRenderer] = false;
              this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) instancedMeshesRenderer);
            }
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>(instancedMeshesRenderer);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_highlightMovingChunks.Values)
          {
            if (this.m_registeredForHighlight[instancedMeshesRenderer])
            {
              this.m_registeredForHighlight[instancedMeshesRenderer] = false;
              this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) instancedMeshesRenderer);
            }
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>>(instancedMeshesRenderer);
          }
          if (this.m_connectorChunk.HasValue)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>>(this.m_connectorChunk.Value);
          if (this.m_connectorBlueprintChunk.HasValue)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>>(this.m_connectorBlueprintChunk.Value);
          if (this.m_staticFlowFrameChunk.HasValue)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>(this.m_staticFlowFrameChunk.Value);
          if (this.m_staticFlowGlassChunk.HasValue)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>(this.m_staticFlowGlassChunk.Value);
          if (this.m_staticFlowFrameBlueprintChunk.HasValue)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>(this.m_staticFlowFrameBlueprintChunk.Value);
          if (this.m_staticFlowGlassBlueprintChunk.HasValue)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>(this.m_staticFlowGlassBlueprintChunk.Value);
          if (this.m_dynamicFlowChunk.HasValue)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorDynamicInstanceData>>(this.m_dynamicFlowChunk.Value);
          if (!this.m_staticFlowFrameHighlightChunk.HasValue)
            return;
          if (this.m_registeredFlowIndicatorForHighlight[this.m_staticFlowFrameHighlightChunk.Value])
          {
            this.m_registeredFlowIndicatorForHighlight[this.m_staticFlowFrameHighlightChunk.Value] = false;
            this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) this.m_staticFlowFrameHighlightChunk.Value);
          }
          reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>(this.m_staticFlowFrameHighlightChunk.Value);
        }

        public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
        {
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_staticChunks.Values)
            info.Add(new RenderedInstancesInfo(this.Name + "(static) " + instancedMeshesRenderer.MeshName, instancedMeshesRenderer.InstancesCount, instancedMeshesRenderer.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_movingChunks.Values)
            info.Add(new RenderedInstancesInfo(this.Name + "(moving) " + instancedMeshesRenderer.MeshName, instancedMeshesRenderer.InstancesCount, instancedMeshesRenderer.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_blueprintStaticChunks.Values)
            info.Add(new RenderedInstancesInfo(this.Name + "(blueprint static) " + instancedMeshesRenderer.MeshName, instancedMeshesRenderer.InstancesCount, instancedMeshesRenderer.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_blueprintMovingChunks.Values)
            info.Add(new RenderedInstancesInfo(this.Name + "(blueprint moving) " + instancedMeshesRenderer.MeshName, instancedMeshesRenderer.InstancesCount, instancedMeshesRenderer.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_highlightStaticChunks.Values)
            info.Add(new RenderedInstancesInfo(this.Name + "(highlight static) " + instancedMeshesRenderer.MeshName, instancedMeshesRenderer.InstancesCount, instancedMeshesRenderer.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_highlightMovingChunks.Values)
            info.Add(new RenderedInstancesInfo(this.Name + "(highlight moving) " + instancedMeshesRenderer.MeshName, instancedMeshesRenderer.InstancesCount, instancedMeshesRenderer.IndicesCountForLod0));
          if (this.m_staticFlowFrameChunk.HasValue)
            info.Add(new RenderedInstancesInfo(this.Name + " (flow frame)", this.m_staticFlowFrameChunk.Value.InstancesCount, this.m_staticFlowFrameChunk.Value.IndicesCountForLod0));
          if (this.m_staticFlowGlassChunk.HasValue)
            info.Add(new RenderedInstancesInfo(this.Name + " (flow glass)", this.m_staticFlowGlassChunk.Value.InstancesCount, this.m_staticFlowGlassChunk.Value.IndicesCountForLod0));
          if (this.m_staticFlowFrameBlueprintChunk.HasValue)
            info.Add(new RenderedInstancesInfo(this.Name + " (flow frame blueprint)", this.m_staticFlowFrameBlueprintChunk.Value.InstancesCount, this.m_staticFlowFrameBlueprintChunk.Value.IndicesCountForLod0));
          if (this.m_staticFlowGlassBlueprintChunk.HasValue)
            info.Add(new RenderedInstancesInfo(this.Name + " (flow frame glass blueprint)", this.m_staticFlowGlassBlueprintChunk.Value.InstancesCount, this.m_staticFlowGlassBlueprintChunk.Value.IndicesCountForLod0));
          if (this.m_dynamicFlowChunk.HasValue)
            info.Add(new RenderedInstancesInfo(this.Name + " (dynamic flow)", this.m_dynamicFlowChunk.Value.InstancesCount, this.m_dynamicFlowChunk.Value.IndicesCountForLod0));
          if (!this.m_staticFlowFrameHighlightChunk.HasValue)
            return;
          info.Add(new RenderedInstancesInfo(this.Name + " (flow frame highlight)", this.m_staticFlowFrameHighlightChunk.Value.InstancesCount, this.m_staticFlowFrameHighlightChunk.Value.IndicesCountForLod0));
        }

        public uint AddInstances(Mafi.Core.Factory.Transports.Transport transport, TransportRenderingSection section)
        {
          ColorRgba color;
          bool blueprintColor = InstancedChunkBasedTransportsRenderer.GetBlueprintColor((IStaticEntity) transport, out color);
          uint num = uint.MaxValue;
          if (section.StaticMeshIdx >= (short) 0)
          {
            InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData data = new InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData(section, transport.Prototype.Graphics.UvShiftY, transport.Prototype.Graphics.CrossSectionScale.ToFloat(), transport.Prototype.Graphics.CrossSectionRadius, color, new ColorRgba());
            Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> dict = blueprintColor ? this.m_blueprintStaticChunks : this.m_staticChunks;
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instance;
            if (!dict.TryGetValue(section.StaticMeshIdx, out instance))
            {
              Material nonSharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(blueprintColor ? this.m_parentData.m_parentRenderer.m_blueprintMaterialForInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_normalMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialOriginal);
              if (transport.Prototype.Graphics.UsePerProductColoring)
                nonSharedMaterial.EnableKeyword("ENABLE_COLORING");
              instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>(this.m_parentData.m_sharedStaticMeshesLods[(int) section.StaticMeshIdx], nonSharedMaterial);
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
              dict.Add(section.StaticMeshIdx, instance);
            }
            num = (uint) instance.AddInstance(data);
          }
          if (section.MovingMeshIdx >= (short) 0)
          {
            InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData data = new InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData(section, transport.Prototype.Graphics.UvShiftY, transport.Prototype.Graphics.CrossSectionScale.ToFloat(), transport.Prototype.Graphics.CrossSectionRadius, color, new ColorRgba());
            Dict<short, InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>> dict = blueprintColor ? this.m_blueprintMovingChunks : this.m_movingChunks;
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instance;
            if (!dict.TryGetValue(section.MovingMeshIdx, out instance))
            {
              Material nonSharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(blueprintColor ? this.m_parentData.m_parentRenderer.m_blueprintMaterialForInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_normalMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialOriginal);
              if (transport.Prototype.Graphics.UsePerProductColoring)
                nonSharedMaterial.EnableKeyword("ENABLE_COLORING");
              instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>(this.m_parentData.m_sharedMovingMeshesLods[(int) section.MovingMeshIdx], nonSharedMaterial);
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
              dict.Add(section.MovingMeshIdx, instance);
            }
            if (num == uint.MaxValue)
              num = (uint) instance.AddInstance(data);
            else if ((int) instance.AddInstance(data) != (int) num)
              Log.Warning("Moving and static instance IDs aren't equal.");
          }
          float y = section.Position.y;
          if (this.InstancesCount == 0)
          {
            this.m_minHeight = this.m_maxHeight = y;
            this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
            this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
          }
          else if ((double) y < (double) this.m_minHeight)
          {
            this.m_minHeight = y;
            this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
            this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
          }
          else if ((double) y > (double) this.m_maxHeight)
          {
            this.m_maxHeight = y;
            this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
            this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
          }
          ++this.InstancesCount;
          return num | (uint) blueprintColor << 16;
        }

        public void RemoveInstances(uint instanceId, TransportRenderingSection sectionData)
        {
          bool flag = (instanceId & 65536U) > 0U;
          ushort index = (ushort) instanceId;
          if (sectionData.StaticMeshIdx >= (short) 0)
          {
            if (flag)
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
              if (!this.m_blueprintStaticChunks.TryGetValue(sectionData.StaticMeshIdx, out instancedMeshesRenderer))
                Log.Warning("Static chunk not found when removing.");
              else
                instancedMeshesRenderer.RemoveInstance(index);
            }
            else
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
              if (!this.m_staticChunks.TryGetValue(sectionData.StaticMeshIdx, out instancedMeshesRenderer))
                Log.Warning("Static chunk not found when removing.");
              else
                instancedMeshesRenderer.RemoveInstance(index);
            }
          }
          if (sectionData.MovingMeshIdx >= (short) 0)
          {
            if (flag)
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
              if (!this.m_blueprintMovingChunks.TryGetValue(sectionData.MovingMeshIdx, out instancedMeshesRenderer))
                Log.Warning("moving chunk not found when removing.");
              else
                instancedMeshesRenderer.RemoveInstance(index);
            }
            else
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
              if (!this.m_movingChunks.TryGetValue(sectionData.MovingMeshIdx, out instancedMeshesRenderer))
                Log.Warning("moving chunk not found when removing.");
              else
                instancedMeshesRenderer.RemoveInstance(index);
            }
          }
          --this.InstancesCount;
          if (this.InstancesCount >= 0)
            return;
          this.InstancesCount = 0;
          Log.Error(string.Format("Instanced count went negative, set to: {0}", (object) this.InstancesCount));
        }

        public uint AddHighlight(
          Mafi.Core.Factory.Transports.Transport transport,
          TransportRenderingSection section,
          ColorRgba color)
        {
          InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData data = new InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData(section, 0.0f, transport.Prototype.Graphics.CrossSectionScale.ToFloat(), transport.Prototype.Graphics.CrossSectionRadius, color, new ColorRgba());
          int num = -1;
          if (section.StaticMeshIdx >= (short) 0)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
            if (!this.m_highlightStaticChunks.TryGetValue(section.StaticMeshIdx, out instancedMeshesRenderer))
            {
              if ((int) section.StaticMeshIdx >= this.m_parentData.m_sharedStaticMeshesLods.Count)
              {
                Log.Error(string.Format("StaticMeshIdx: {0} greater than meshes count {1}.", (object) section.StaticMeshIdx, (object) this.m_parentData.m_sharedStaticMeshesLods.Count));
              }
              else
              {
                Material nonSharedMaterial = UnityEngine.Object.Instantiate<Material>(this.m_parentData.m_parentRenderer.m_highlightMaterialForInstancedRenderingShared);
                instancedMeshesRenderer = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>(this.m_parentData.m_sharedStaticMeshesLods[(int) section.StaticMeshIdx], nonSharedMaterial, shadowCastingMode: ShadowCastingMode.Off);
                this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instancedMeshesRenderer);
                this.m_highlightStaticChunks.AddAndAssertNew(section.StaticMeshIdx, instancedMeshesRenderer);
                this.m_registeredForHighlight.AddAndAssertNew(instancedMeshesRenderer, false);
              }
            }
            if (instancedMeshesRenderer != null)
              num = (int) instancedMeshesRenderer.AddInstance(data);
          }
          if (section.MovingMeshIdx >= (short) 0)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
            if (!this.m_highlightMovingChunks.TryGetValue(section.MovingMeshIdx, out instancedMeshesRenderer))
            {
              if ((int) section.MovingMeshIdx >= this.m_parentData.m_sharedMovingMeshesLods.Count)
              {
                Log.Error(string.Format("MovingMeshIdx: {0} greater than meshes count {1}.", (object) section.MovingMeshIdx, (object) this.m_parentData.m_sharedMovingMeshesLods.Count));
              }
              else
              {
                Material nonSharedMaterial = UnityEngine.Object.Instantiate<Material>(this.m_parentData.m_parentRenderer.m_highlightMaterialForInstancedRenderingShared);
                instancedMeshesRenderer = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData>(this.m_parentData.m_sharedMovingMeshesLods[(int) section.MovingMeshIdx], nonSharedMaterial, shadowCastingMode: ShadowCastingMode.Off);
                this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instancedMeshesRenderer);
                this.m_highlightMovingChunks.AddAndAssertNew(section.MovingMeshIdx, instancedMeshesRenderer);
                this.m_registeredForHighlight.AddAndAssertNew(instancedMeshesRenderer, false);
              }
            }
            if (instancedMeshesRenderer != null)
            {
              if (num == -1)
                num = (int) instancedMeshesRenderer.AddInstance(data);
              else if ((int) instancedMeshesRenderer.AddInstance(data) != num)
                Log.Warning("Moving and static instance IDs aren't equal.");
            }
          }
          return num >= 0 ? (uint) num : uint.MaxValue;
        }

        public uint AddFlowIndicatorHighlight(TransportFlowIndicatorPose pose, ColorRgba color)
        {
          if (this.m_staticFlowFrameHighlightChunk.IsNone)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>(this.m_parentData.m_sharedStaticFlowFrameMeshesLods, UnityEngine.Object.Instantiate<Material>(this.m_parentData.m_parentRenderer.m_highlightMaterialForFlowIndicatorInstancedRenderingShared), shadowCastingMode: ShadowCastingMode.Off);
            this.m_staticFlowFrameHighlightChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>) instance;
            this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            this.m_registeredFlowIndicatorForHighlight.AddAndAssertNew(this.m_staticFlowFrameHighlightChunk.Value, false);
          }
          return (uint) this.m_staticFlowFrameHighlightChunk.Value.AddInstance(new InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData(pose, color));
        }

        public void RemoveHighlight(TransportHighlightInfo instanceInfo, uint instanceId)
        {
          if (instanceInfo.StaticMeshIdx >= (short) 0)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
            if (!this.m_highlightStaticChunks.TryGetValue(instanceInfo.StaticMeshIdx, out instancedMeshesRenderer))
              Log.Warning("Static chunk not found when removing.");
            else
              instancedMeshesRenderer.RemoveInstance((ushort) instanceId);
          }
          if (instanceInfo.MovingMeshIdx >= (short) 0)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
            if (!this.m_highlightMovingChunks.TryGetValue(instanceInfo.MovingMeshIdx, out instancedMeshesRenderer))
              Log.Warning("Moving chunk not found when removing.");
            else
              instancedMeshesRenderer.RemoveInstance((ushort) instanceId);
          }
          if (!instanceInfo.IsFlowIndicator)
            return;
          if (!this.m_staticFlowFrameHighlightChunk.HasValue)
            Log.Warning("Flow indicator highlight chunk not found when removing.");
          else
            this.m_staticFlowFrameHighlightChunk.Value.RemoveInstance((ushort) instanceId);
        }

        public uint AddFlowIndicator(Mafi.Core.Factory.Transports.Transport transport, TransportFlowIndicatorPose flowIndicator)
        {
          ColorRgba color;
          bool blueprintColor = InstancedChunkBasedTransportsRenderer.GetBlueprintColor((IStaticEntity) transport, out color);
          InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData data = new InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData(flowIndicator, color);
          if (blueprintColor)
          {
            if (this.m_staticFlowFrameBlueprintChunk.IsNone)
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>(this.m_parentData.m_sharedStaticFlowFrameMeshesLods, InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_parentData.m_parentRenderer.m_blueprintStaticFrameFlowIndicatorMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialFlowIndicatorFrame));
              this.m_staticFlowFrameBlueprintChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>) instance;
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
            if (this.m_staticFlowGlassBlueprintChunk.IsNone)
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>(this.m_parentData.m_sharedStaticFlowGlassMeshesLods, InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_parentData.m_parentRenderer.m_blueprintStaticFrameFlowIndicatorMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialFlowIndicatorGlass));
              this.m_staticFlowGlassBlueprintChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>) instance;
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
            uint num = (uint) this.m_staticFlowFrameBlueprintChunk.Value.AddInstance(data);
            if ((int) this.m_staticFlowGlassBlueprintChunk.Value.AddInstance(data) != (int) num)
              Log.Warning("Glass and frame instance ids aren't equal");
            return num | 65536U;
          }
          if (this.m_staticFlowFrameChunk.IsNone)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>(this.m_parentData.m_sharedStaticFlowFrameMeshesLods, InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_parentData.m_parentRenderer.m_staticFrameFlowIndicatorMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialFlowIndicatorFrame));
            this.m_staticFlowFrameChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>) instance;
            this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
          }
          if (this.m_staticFlowGlassChunk.IsNone)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>(this.m_parentData.m_sharedStaticFlowGlassMeshesLods, InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_parentData.m_parentRenderer.m_staticGlassFlowIndicatorMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialFlowIndicatorGlass));
            this.m_staticFlowGlassChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData>>) instance;
            this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
          }
          if (this.m_dynamicFlowChunk.IsNone)
          {
            Material nonSharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_parentData.m_parentRenderer.m_dynamicFlowIndicatorMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialFlowIndicatorFlow);
            TransportProto.Gfx graphics = this.m_parentData.TransportProto.Graphics;
            if (graphics.FlowIndicator.HasValue)
              nonSharedMaterial.SetVector(InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.FLOW_INDICATOR_OFFSET_SCALE_SHADER_ID, new Vector4(0.0f, 0.0f, graphics.FlowIndicator.Value.Parameters.DetailsScale, graphics.FlowIndicator.Value.Parameters.StillMovementScale));
            else
              Log.Warning("Flow indicator doesn't have graphics params.");
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorDynamicInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorDynamicInstanceData>(this.m_parentData.m_sharedDynamicFlowMeshesLods, nonSharedMaterial);
            this.m_dynamicFlowChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorDynamicInstanceData>>) instance;
            this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
          }
          uint num1 = (uint) this.m_staticFlowFrameChunk.Value.AddInstance(data);
          if ((int) this.m_staticFlowGlassChunk.Value.AddInstance(data) != (int) num1)
            Log.Warning("Glass and frame instance ids aren't equal");
          if ((int) this.m_dynamicFlowChunk.Value.AddInstance(new InstancedChunkBasedTransportsRenderer.FlowIndicatorDynamicInstanceData(flowIndicator)) != (int) num1)
            Log.Warning("Flow and frame instance ids aren't equal");
          return num1;
        }

        public void RemoveFlowIndicator(uint instanceId)
        {
          bool flag = (instanceId & 65536U) > 0U;
          ushort index = (ushort) instanceId;
          if (flag)
          {
            if (this.m_staticFlowFrameBlueprintChunk.HasValue)
              this.m_staticFlowFrameBlueprintChunk.Value.RemoveInstance(index);
            else
              Log.Error("Failed to remove flow indicator, no chunk.");
            if (this.m_staticFlowGlassBlueprintChunk.HasValue)
              this.m_staticFlowGlassBlueprintChunk.Value.RemoveInstance(index);
            else
              Log.Error("Failed to remove flow indicator, no chunk.");
          }
          else
          {
            if (this.m_staticFlowFrameChunk.HasValue)
              this.m_staticFlowFrameChunk.Value.RemoveInstance(index);
            else
              Log.Error("Failed to remove flow indicator, no chunk.");
            if (this.m_staticFlowGlassChunk.HasValue)
              this.m_staticFlowGlassChunk.Value.RemoveInstance(index);
            else
              Log.Error("Failed to remove flow indicator, no chunk.");
            if (this.m_dynamicFlowChunk.HasValue)
              this.m_dynamicFlowChunk.Value.RemoveInstance(index);
            else
              Log.Error("Failed to remove flow indicator, no chunk.");
          }
        }

        public uint AddConnector(Mafi.Core.Factory.Transports.Transport transport, TransportConnectorPose connector)
        {
          ColorRgba color;
          bool blueprintColor = InstancedChunkBasedTransportsRenderer.GetBlueprintColor((IStaticEntity) transport, out color);
          InstancedChunkBasedTransportsRenderer.ConnectorInstanceData data = new InstancedChunkBasedTransportsRenderer.ConnectorInstanceData(connector, color, transport.Prototype);
          if (blueprintColor)
          {
            if (this.m_connectorBlueprintChunk.IsNone)
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>(this.m_parentData.m_sharedConnectorMeshesLods, InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_parentData.m_parentRenderer.m_blueprintConnectorMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialConnector));
              this.m_connectorBlueprintChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>>) instance;
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
            return (uint) this.m_connectorBlueprintChunk.Value.AddInstance(data) | 65536U;
          }
          if (this.m_connectorChunk.IsNone)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>(this.m_parentData.m_sharedConnectorMeshesLods, InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_parentData.m_parentRenderer.m_connectorMaterialForInstancedRenderingShared, this.m_parentData.m_sharedMaterialConnector));
            this.m_connectorChunk = (Option<InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.ConnectorInstanceData>>) instance;
            this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
          }
          return (uint) this.m_connectorChunk.Value.AddInstance(data);
        }

        public void RemoveConnector(uint instanceId)
        {
          bool flag = (instanceId & 65536U) > 0U;
          ushort index = (ushort) instanceId;
          if (flag)
          {
            if (this.m_connectorBlueprintChunk.HasValue)
              this.m_connectorBlueprintChunk.Value.RemoveInstance(index);
            else
              Log.Error("Failed to remove flow indicator, no chunk.");
          }
          else if (this.m_connectorChunk.HasValue)
            this.m_connectorChunk.Value.RemoveInstance(index);
          else
            Log.Error("Failed to remove flow indicator, no chunk.");
        }

        public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
        {
          this.m_lastRenderedLod = lod;
          if (lod > this.m_maxLod)
          {
            foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> key in this.m_highlightStaticChunks.Values)
            {
              bool flag;
              if (this.m_registeredForHighlight.TryGetValue(key, out flag))
              {
                if (flag)
                {
                  this.m_registeredForHighlight[key] = false;
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
            }
            foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> key in this.m_highlightMovingChunks.Values)
            {
              bool flag;
              if (this.m_registeredForHighlight.TryGetValue(key, out flag))
              {
                if (flag)
                {
                  this.m_registeredForHighlight[key] = false;
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
            }
            if (this.m_staticFlowFrameHighlightChunk.HasValue)
            {
              InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData> key = this.m_staticFlowFrameHighlightChunk.Value;
              bool flag;
              if (this.m_registeredFlowIndicatorForHighlight.TryGetValue(key, out flag))
              {
                if (flag)
                {
                  this.m_registeredFlowIndicatorForHighlight[key] = false;
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
            }
            return new RenderStats();
          }
          RenderStats renderStats = new RenderStats();
          if (lod <= InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.MAX_STATIC_MESH_LOD || this.m_movingChunks.Values.Count == 0)
          {
            foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_staticChunks.Values)
              renderStats += instancedMeshesRenderer.Render(this.m_bounds, lod);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_movingChunks.Values)
            renderStats += instancedMeshesRenderer.Render(this.m_bounds, lod);
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_blueprintStaticChunks.Values)
            renderStats += instancedMeshesRenderer.Render(this.m_bounds, lod);
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer in this.m_blueprintMovingChunks.Values)
            renderStats += instancedMeshesRenderer.Render(this.m_bounds, lod);
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> key in this.m_highlightStaticChunks.Values)
          {
            if (key.InstancesCount > 0)
            {
              bool flag;
              if (this.m_registeredForHighlight.TryGetValue(key, out flag))
              {
                if (!flag)
                {
                  this.m_registeredForHighlight[key] = true;
                  this.m_parentData.m_parentRenderer.m_highlighter.AddCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
              renderStats += new RenderStats(1, key.InstancesCount, key.RenderedInstancesCount, key.RenderedInstancesCount * key.IndicesCountForLod0);
            }
            else
            {
              bool flag;
              if (this.m_registeredForHighlight.TryGetValue(key, out flag))
              {
                if (flag)
                {
                  this.m_registeredForHighlight[key] = false;
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
            }
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> key in this.m_highlightMovingChunks.Values)
          {
            if (key.InstancesCount > 0)
            {
              bool flag;
              if (this.m_registeredForHighlight.TryGetValue(key, out flag))
              {
                if (!flag)
                {
                  this.m_registeredForHighlight[key] = true;
                  this.m_parentData.m_parentRenderer.m_highlighter.AddCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
              renderStats += new RenderStats(1, key.InstancesCount, key.RenderedInstancesCount, key.RenderedInstancesCount * key.IndicesCountForLod0);
            }
            else
            {
              bool flag;
              if (this.m_registeredForHighlight.TryGetValue(key, out flag))
              {
                if (flag)
                {
                  this.m_registeredForHighlight[key] = false;
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
            }
          }
          if (lod > InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.MAX_FLOW_INDICATOR_LOD)
            return renderStats;
          if (this.m_connectorChunk.HasValue)
            renderStats += this.m_connectorChunk.Value.Render(this.m_bounds, lod);
          if (this.m_connectorBlueprintChunk.HasValue)
            renderStats += this.m_connectorBlueprintChunk.Value.Render(this.m_bounds, lod);
          if (this.m_staticFlowFrameHighlightChunk.HasValue)
          {
            InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.FlowIndicatorStaticInstanceData> key = this.m_staticFlowFrameHighlightChunk.Value;
            if (key.InstancesCount > 0)
            {
              bool flag;
              if (this.m_registeredFlowIndicatorForHighlight.TryGetValue(key, out flag))
              {
                if (!flag)
                {
                  this.m_registeredFlowIndicatorForHighlight[key] = true;
                  this.m_parentData.m_parentRenderer.m_highlighter.AddCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
              renderStats += new RenderStats(1, key.InstancesCount, key.RenderedInstancesCount, key.RenderedInstancesCount * key.IndicesCountForLod0);
            }
            else
            {
              bool flag;
              if (this.m_registeredFlowIndicatorForHighlight.TryGetValue(key, out flag))
              {
                if (flag)
                {
                  this.m_registeredFlowIndicatorForHighlight[key] = false;
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) key);
                }
              }
              else
                Log.WarningOnce("Can't find highlight chunk registration status");
            }
          }
          if (this.m_staticFlowFrameChunk.HasValue)
            renderStats += this.m_staticFlowFrameChunk.Value.Render(this.m_bounds, lod);
          if (this.m_staticFlowFrameBlueprintChunk.HasValue)
            renderStats += this.m_staticFlowFrameBlueprintChunk.Value.Render(this.m_bounds, lod);
          if (this.m_staticFlowGlassBlueprintChunk.HasValue)
            renderStats += this.m_staticFlowGlassBlueprintChunk.Value.Render(this.m_bounds, lod);
          if (this.m_staticFlowGlassChunk.HasValue)
            renderStats += this.m_staticFlowGlassChunk.Value.Render(this.m_bounds, lod);
          if (this.m_dynamicFlowChunk.HasValue)
            renderStats += this.m_dynamicFlowChunk.Value.Render(this.m_bounds, lod);
          return renderStats;
        }

        public void UpdateMovement(
          ushort instanceIndex,
          TransportRenderingSection section,
          float offsetFrom,
          float offsetTo,
          bool forceUpdate)
        {
          if (this.m_lastRenderedLod >= InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.MAX_MOVEMENT_UPDATES_LOD && !forceUpdate)
            return;
          InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
          if (!this.m_movingChunks.TryGetValue(section.MovingMeshIdx, out instancedMeshesRenderer))
          {
            Log.WarningOnce("Trying to update movement for section with no chunk.");
          }
          else
          {
            ref InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData local = ref instancedMeshesRenderer.GetDataRef(instanceIndex);
            if ((double) local.MovementOffsetFrom == (double) offsetFrom && (double) local.MovementOffsetTo == (double) offsetTo)
              return;
            local.MovementOffsetFrom = offsetFrom;
            local.MovementOffsetTo = offsetTo;
            instancedMeshesRenderer.NotifyDataUpdated(instanceIndex);
          }
        }

        public void UpdatePipeColor(
          ushort instanceIndex,
          TransportRenderingSection section,
          ColorRgba color,
          ColorRgba accentColor)
        {
          InstancedMeshesRenderer<InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData> instancedMeshesRenderer;
          if (!this.m_staticChunks.TryGetValue(section.StaticMeshIdx, out instancedMeshesRenderer))
          {
            Log.WarningOnce("Trying to update movement for section with no chunk.");
          }
          else
          {
            ref InstancedChunkBasedTransportsRenderer.TransportSectionInstanceData local = ref instancedMeshesRenderer.GetDataRef(instanceIndex);
            if (!((ColorRgba) local.Color != color) && !((ColorRgba) local.AccentColor != accentColor))
              return;
            local.Color = color.Rgba;
            local.AccentColor = accentColor.Rgba;
            instancedMeshesRenderer.NotifyDataUpdated(instanceIndex);
          }
        }

        public void UpdateFlowIndicatorColor(ushort instanceIndex, ColorRgba color)
        {
          if (this.m_staticFlowFrameChunk.IsNone)
          {
            Log.WarningOnce("Trying to update flow indicator with no chunk.");
          }
          else
          {
            this.m_staticFlowFrameChunk.Value.GetDataRef(instanceIndex).UpdateColor(color.Rgba);
            this.m_staticFlowFrameChunk.Value.NotifyDataUpdated(instanceIndex);
          }
        }

        public void UpdateFlowIndicator(
          GameTime time,
          ushort instanceIndex,
          InstancedFluidIndicatorState fluidIndicatorState)
        {
          if (this.m_lastRenderedLod >= InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.MAX_MOVEMENT_UPDATES_LOD)
            return;
          if (this.m_dynamicFlowChunk.IsNone)
          {
            Log.WarningOnce("Trying to update flow indicator with no chunk.");
          }
          else
          {
            fluidIndicatorState.Update(time);
            ref InstancedChunkBasedTransportsRenderer.FlowIndicatorDynamicInstanceData local = ref this.m_dynamicFlowChunk.Value.GetDataRef(instanceIndex);
            float texOffset = fluidIndicatorState.TexOffset;
            ColorRgba currentColor = fluidIndicatorState.CurrentColor;
            if (!local.ShouldUpdate(texOffset, currentColor))
              return;
            local.PrevColor = local.TargetColor;
            local.TargetColor = currentColor.Rgba;
            local.PrevTexOffset = local.TargetTexOffset;
            local.TargetTexOffset = texOffset;
            this.m_dynamicFlowChunk.Value.NotifyDataUpdated(instanceIndex);
          }
        }

        public void Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

        public void NotifyWasNotRendered()
        {
        }

        static StandardChunk()
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.MAX_STATIC_MESH_LOD = 4;
          InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.MAX_FLOW_INDICATOR_LOD = 3;
          InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.MAX_MOVEMENT_UPDATES_LOD = 3;
          InstancedChunkBasedTransportsRenderer.TransportRenderingData.StandardChunk.FLOW_INDICATOR_OFFSET_SCALE_SHADER_ID = Shader.PropertyToID("_OffsetAndScale");
        }
      }
    }

    [ExpectedStructSize(60)]
    public struct TransportSectionInstanceData
    {
      public readonly Vector3 StartPosition;
      public readonly float Pitch;
      public readonly float Scale;
      public readonly float StartUv;
      public readonly float EndUv;
      public float MovementOffsetFrom;
      public float MovementOffsetTo;
      public uint Color;
      public uint AccentColor;
      public readonly float UvShiftY;
      public readonly float CrossSectionScale;
      public readonly float CrossSectionRadius;
      public readonly uint PackedData;

      public TransportSectionInstanceData(
        TransportRenderingSection section,
        float uvShiftY,
        float crossSectionScale,
        float crossSectionRadius,
        ColorRgba color,
        ColorRgba accentColor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.MovementOffsetFrom = 0.0f;
        this.MovementOffsetTo = 0.0f;
        this.StartPosition = section.Position;
        this.Pitch = section.Pitch;
        this.UvShiftY = uvShiftY;
        this.CrossSectionScale = crossSectionScale;
        this.CrossSectionRadius = crossSectionRadius;
        float yaw = section.Yaw;
        if ((double) yaw < 0.0)
          yaw += 6.28318548f;
        ushort num1 = (ushort) (((float) (4.0 * (double) yaw / 6.2831854820251465)).RoundToInt() & 3);
        Assert.That<float>(section.TexOffsetY).IsNotNegative();
        ushort num2 = (ushort) ((4f * section.TexOffsetY).RoundToInt() & 3);
        this.PackedData = (uint) num1 << 2 | (uint) num2;
        this.Scale = section.StraightScale;
        this.StartUv = section.StartUv;
        this.EndUv = section.EndUv;
        this.Color = color.Rgba;
        this.AccentColor = accentColor.Rgba;
      }

      public TransportSectionInstanceData(
        Vector3 startPosition,
        float pitch,
        float scale,
        float startUv,
        float endUv,
        float movementOffsetFrom,
        float movementOffsetTo,
        uint color,
        float uvShiftY,
        float crossSectionScale,
        float crossSectionRadius,
        uint packedData)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.AccentColor = 0U;
        this.StartPosition = startPosition;
        this.Pitch = pitch;
        this.Scale = scale;
        this.StartUv = startUv;
        this.EndUv = endUv;
        this.MovementOffsetFrom = movementOffsetFrom;
        this.MovementOffsetTo = movementOffsetTo;
        this.Color = color;
        this.UvShiftY = uvShiftY;
        this.CrossSectionScale = crossSectionScale;
        this.CrossSectionRadius = crossSectionRadius;
        this.PackedData = packedData;
      }
    }

    [ExpectedStructSize(20)]
    public readonly struct ConnectorInstanceData
    {
      public readonly Vector3 Position;
      public readonly uint Color;
      public readonly float CrossSectionScale;

      public ConnectorInstanceData(
        TransportConnectorPose connectorPose,
        ColorRgba color,
        TransportProto proto)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = connectorPose.Position.ToVector3();
        this.Color = color.Rgba;
        this.CrossSectionScale = 2f * proto.Graphics.CrossSectionScale.ToFloat() * proto.Graphics.CrossSectionRadius;
      }
    }

    [ExpectedStructSize(16)]
    public struct FlowIndicatorStaticInstanceData
    {
      public readonly Vector3 Position;
      public uint Data;

      public FlowIndicatorStaticInstanceData(
        TransportFlowIndicatorPose flowIndicatorPose,
        ColorRgba color)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = flowIndicatorPose.Position.ToVector3();
        ushort num1 = (ushort) (((float) (4.0 * (double) flowIndicatorPose.Rotation.Yaw.Raw / (double) ushort.MaxValue)).RoundToInt() % 4);
        ushort num2 = (ushort) (((float) (4.0 * (double) flowIndicatorPose.Rotation.Pitch.Raw / (double) ushort.MaxValue)).RoundToInt() % 4);
        this.Data = (uint) ((int) color.Rgba & -16 | (int) num2 << 2) | (uint) num1;
      }

      public void UpdateColor(uint newColor)
      {
        this.Data = (uint) ((int) newColor & -16 | (int) this.Data & 15);
      }
    }

    [ExpectedStructSize(32)]
    internal struct FlowIndicatorDynamicInstanceData
    {
      public readonly Vector3 Position;
      public readonly uint PackedYawPitch;
      public uint PrevColor;
      public uint TargetColor;
      public float PrevTexOffset;
      public float TargetTexOffset;

      public FlowIndicatorDynamicInstanceData(TransportFlowIndicatorPose flowIndicatorPose)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = flowIndicatorPose.Position.ToVector3();
        ushort num = (ushort) (((float) (4.0 * (double) flowIndicatorPose.Rotation.Yaw.Raw / (double) ushort.MaxValue)).RoundToInt() % 4);
        this.PackedYawPitch = (uint) (ushort) (((float) (4.0 * (double) flowIndicatorPose.Rotation.Pitch.Raw / (double) ushort.MaxValue)).RoundToInt() % 4) << 2 | (uint) num;
        this.PrevColor = 0U;
        this.TargetColor = 0U;
        this.PrevTexOffset = 0.0f;
        this.TargetTexOffset = 0.0f;
      }

      public readonly bool ShouldUpdate(float newTexOffset, ColorRgba newColor)
      {
        return (double) this.PrevTexOffset != (double) this.TargetTexOffset || (int) this.TargetColor != (int) this.PrevColor || (double) newTexOffset != (double) this.TargetTexOffset || (int) newColor.Rgba != (int) this.TargetColor;
      }
    }
  }
}
