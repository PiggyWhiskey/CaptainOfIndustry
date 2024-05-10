// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportPillarsRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportPillarsRenderer : IEntitiesRenderer, IDisposable
  {
    [ThreadStatic]
    private static Lyst<uint> s_usedMeshesTmp;
    private readonly TransportsConstructionHelper m_transportsConstructionHelper;
    private readonly AssetsDb m_assetsDb;
    private readonly Material m_materialForInstancingShared;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly Dict<TransportPillar, bool> m_pillarsToChangeOnSim;
    private Lyst<KeyValuePair<TransportPillar, PillarVisualsSpec>> m_pillarsToAddOnSim;
    private Lyst<KeyValuePair<TransportPillar, PillarVisualsSpec>> m_pillarsToAddOnMain;
    private Lyst<TransportPillar> m_pillarsToRemoveOnSim;
    private Lyst<TransportPillar> m_pillarsToRemoveOnMain;
    private readonly Option<TransportPillarsRenderer.PillarsChunk>[] m_pillarsChunks;
    private readonly TransportPillarsRenderer.PillarsChunkImmediate m_pillarsChunkImmediate;
    private readonly TransportPillarsRenderer.PillarMeshMat m_pillarBaseMatMesh;
    private readonly TransportPillarsRenderer.PillarMeshMat m_pillarCornerBeamsMatMesh;
    private readonly TransportPillarsRenderer.PillarMeshMat m_pillarXBracesMatMesh;
    private readonly TransportPillarsRenderer.PillarMeshMat m_pillarBeamsWithXBracesMatMesh;
    private readonly Dict<string, TransportPillarsRenderer.PillarMeshMat> m_attachmentMatMeshes;

    public int Priority => 10;

    public TransportPillarsRenderer(
      EntitiesRenderingManager entitiesRenderingManager,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      TransportsConstructionHelper transportsConstructionHelper,
      AssetsDb assetsDb,
      ProtosDb protosDb,
      ChunkBasedRenderingManager visibleChunksRenderer,
      ReloadAfterAssetUpdateManager reloadManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_pillarsToChangeOnSim = new Dict<TransportPillar, bool>();
      this.m_pillarsToAddOnSim = new Lyst<KeyValuePair<TransportPillar, PillarVisualsSpec>>();
      this.m_pillarsToAddOnMain = new Lyst<KeyValuePair<TransportPillar, PillarVisualsSpec>>();
      this.m_pillarsToRemoveOnSim = new Lyst<TransportPillar>();
      this.m_pillarsToRemoveOnMain = new Lyst<TransportPillar>();
      this.m_attachmentMatMeshes = new Dict<string, TransportPillarsRenderer.PillarMeshMat>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_transportsConstructionHelper = transportsConstructionHelper;
      this.m_assetsDb = assetsDb;
      this.m_chunksRenderer = visibleChunksRenderer;
      this.m_reloadManager = reloadManager;
      this.m_pillarsChunks = new Option<TransportPillarsRenderer.PillarsChunk>[visibleChunksRenderer.ChunksCountTotal];
      this.m_materialForInstancingShared = assetsDb.GetSharedMaterial("Assets/Base/Transports/Pillars/TransportPillar.mat");
      TransportPillarProto proto;
      if (protosDb.TryGetProto<TransportPillarProto>((Proto.ID) IdsCore.Transports.Pillar, out proto))
      {
        this.tryGetMatMesh(proto.Graphics.CornerBasePrefabPath, out this.m_pillarBaseMatMesh).AssertTrue();
        this.tryGetMatMesh(proto.Graphics.CornerBeamsPrefabPath, out this.m_pillarCornerBeamsMatMesh).AssertTrue();
        this.tryGetMatMesh(proto.Graphics.SideFillPlusXPrefabPath, out this.m_pillarXBracesMatMesh).AssertTrue();
        this.tryGetMatMesh(proto.Graphics.BaseWithSideFillsPrefabPath, out this.m_pillarBeamsWithXBracesMatMesh).AssertTrue();
      }
      else
        Log.Error(string.Format("Pillar proto '{0}' was not found!", (object) IdsCore.Transports.Pillar));
      this.m_pillarsChunkImmediate = new TransportPillarsRenderer.PillarsChunkImmediate(this);
      this.m_chunksRenderer.RegisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this.m_pillarsChunkImmediate);
      entitiesRenderingManager.RegisterRenderer((IEntitiesRenderer) this);
      gameLoopEvents.RenderUpdate.AddNonSaveable<TransportPillarsRenderer>(this, new Action<GameTime>(this.renderUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<TransportPillarsRenderer>(this, new Action(this.simUpdateEnd));
    }

    public void Dispose()
    {
      foreach (Option<TransportPillarsRenderer.PillarsChunk> pillarsChunk in this.m_pillarsChunks)
        pillarsChunk.ValueOrNull?.Dispose();
      this.m_attachmentMatMeshes.Clear();
      this.m_pillarsChunkImmediate.Dispose();
    }

    public bool CanRenderEntity(EntityProto proto) => proto is TransportPillarProto;

    public void AddEntityOnSim(IRenderedEntity entity)
    {
      if (entity is TransportPillar key)
        this.m_pillarsToChangeOnSim[key] = true;
      else
        Log.Error(string.Format("Adding invalid entity {0}.", (object) entity));
    }

    public void UpdateEntityOnSim(IRenderedEntity entity)
    {
      if (entity is TransportPillar key)
        this.m_pillarsToChangeOnSim[key] = true;
      else
        Log.Error(string.Format("Updating invalid entity {0}.", (object) entity));
    }

    public void RemoveEntityOnSim(IRenderedEntity entity, EntityRemoveReason reason)
    {
      if (entity is TransportPillar key)
        this.m_pillarsToChangeOnSim[key] = false;
      else
        Log.Error(string.Format("Removing invalid entity {0}.", (object) entity));
    }

    public bool TryGetPickableEntityAs<T>(GameObject pickedGo, out T entity) where T : class, IRenderedEntity
    {
      entity = default (T);
      return false;
    }

    public ulong AddHighlight(IRenderedEntity entity, ColorRgba color)
    {
      Log.Warning("Highlighting of pillars is currently not supported.");
      return 0;
    }

    public void RemoveHighlight(ulong highlightId)
    {
      Log.Warning("Highlighting of pillars is currently not supported.");
    }

    private void simUpdateEnd()
    {
      foreach (KeyValuePair<TransportPillar, bool> keyValuePair in this.m_pillarsToChangeOnSim)
      {
        if (keyValuePair.Value)
          this.m_pillarsToAddOnSim.Add<TransportPillar, PillarVisualsSpec>(keyValuePair.Key, this.m_transportsConstructionHelper.ComputePillarVisuals(keyValuePair.Key.CenterTile, keyValuePair.Key.Height));
        else
          this.m_pillarsToRemoveOnSim.Add(keyValuePair.Key);
      }
      this.m_pillarsToChangeOnSim.Clear();
    }

    public void HandlePreSyncRemove(GameTime time)
    {
    }

    public void SyncUpdate(GameTime time)
    {
      Swap.Them<Lyst<KeyValuePair<TransportPillar, PillarVisualsSpec>>>(ref this.m_pillarsToAddOnSim, ref this.m_pillarsToAddOnMain);
      Swap.Them<Lyst<TransportPillar>>(ref this.m_pillarsToRemoveOnSim, ref this.m_pillarsToRemoveOnMain);
    }

    private void renderUpdate(GameTime time)
    {
      if (this.m_pillarsToRemoveOnMain.IsNotEmpty)
      {
        foreach (TransportPillar pillar in this.m_pillarsToRemoveOnMain)
          removeRenderedPillar(pillar);
        this.m_pillarsToRemoveOnMain.Clear();
      }
      if (!this.m_pillarsToAddOnMain.IsNotEmpty)
        return;
      foreach (KeyValuePair<TransportPillar, PillarVisualsSpec> keyValuePair in this.m_pillarsToAddOnMain)
      {
        TransportPillarsRenderer.PillarsChunk chunk = this.getOrCreateChunk(keyValuePair.Key);
        if (keyValuePair.Key.RendererData.IsValid)
          removeRenderedPillar(keyValuePair.Key);
        PooledArray<uint> partsIds = chunk.AddPillarParts(keyValuePair.Value);
        keyValuePair.Key.RendererData = new TransportPillarRendererData(chunk.CoordAndIndex.ChunkIndex, partsIds);
      }
      this.m_pillarsToAddOnMain.Clear();

      void removeRenderedPillar(TransportPillar pillar)
      {
        if (!pillar.RendererData.IsValid)
          return;
        this.getOrCreateChunk(pillar.RendererData.ChunkIndex).RemovePillarParts(pillar.RendererData.PartsIds);
        pillar.RendererData.PartsIds.ReturnToPool();
        pillar.RendererData = new TransportPillarRendererData();
      }
    }

    private TransportPillarsRenderer.PillarsChunk getOrCreateChunk(TransportPillar pillar)
    {
      return this.getOrCreateChunk(this.m_chunksRenderer.GetChunkIndex(pillar.CenterTile.Xy));
    }

    private TransportPillarsRenderer.PillarsChunk getOrCreateChunk(Chunk256Index index)
    {
      TransportPillarsRenderer.PillarsChunk newChunk = this.m_pillarsChunks[(int) index.Value].ValueOrNull;
      if (newChunk == null)
      {
        this.m_pillarsChunks[(int) index.Value] = (Option<TransportPillarsRenderer.PillarsChunk>) (newChunk = new TransportPillarsRenderer.PillarsChunk(this.m_chunksRenderer.ExtendChunkCoord(index), this));
        this.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
      }
      return newChunk;
    }

    public RenderedPillarData AddPillarVisualImmediate(PillarVisualsSpec visual)
    {
      return new RenderedPillarData(this.m_pillarsChunkImmediate.AddPillarParts(visual));
    }

    public PooledArray<RenderedPillarData> AddPillarVisualImmediate(
      ImmutableArray<PillarVisualsSpec> visualsArray)
    {
      PooledArray<RenderedPillarData> pooled = PooledArray<RenderedPillarData>.GetPooled(visualsArray.Length);
      for (int index = 0; index < visualsArray.Length; ++index)
        pooled[index] = this.AddPillarVisualImmediate(visualsArray[index]);
      return pooled;
    }

    /// <summary>
    /// Note that this returns data to pool. It is callers responsibility to not reuse the given data.
    /// </summary>
    public void RemovePillarVisualImmediate(ref RenderedPillarData data)
    {
      this.m_pillarsChunkImmediate.RemovePillarParts(data.ModelsDta);
      data.ModelsDta.ReturnToPool();
      data = new RenderedPillarData();
    }

    /// <summary>
    /// Note that this returns data to pool. It is callers responsibility to not reuse the given array.
    /// </summary>
    public void RemovePillarVisualImmediate(ref PooledArray<RenderedPillarData> dataArray)
    {
      foreach (RenderedPillarData backing in dataArray.BackingArray)
      {
        this.m_pillarsChunkImmediate.RemovePillarParts(backing.ModelsDta);
        backing.ModelsDta.ReturnToPool();
      }
      dataArray.ReturnToPool();
      dataArray = new PooledArray<RenderedPillarData>();
    }

    private bool tryGetMatMesh(
      string prefabPath,
      out TransportPillarsRenderer.PillarMeshMat meshAndMaterial)
    {
      if (this.m_attachmentMatMeshes.TryGetValue(prefabPath, out meshAndMaterial))
        return true;
      Mesh[] meshes;
      Material sharedMaterial;
      string error;
      if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_assetsDb, prefabPath, out meshes, out sharedMaterial, out error))
      {
        Log.Error("Failed to load pillar prefab '" + prefabPath + "': " + error);
        return false;
      }
      meshAndMaterial = new TransportPillarsRenderer.PillarMeshMat(meshes, sharedMaterial);
      this.m_attachmentMatMeshes.Add(prefabPath, meshAndMaterial);
      return true;
    }

    private readonly struct PillarMeshMat
    {
      public readonly Mesh[] SharedMeshLods;
      public readonly Material SharedMaterial;

      public PillarMeshMat(Mesh[] sharedMeshLods, Material sharedMaterial)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.SharedMeshLods = sharedMeshLods;
        this.SharedMaterial = sharedMaterial;
      }
    }

    private class PillarsChunkBase : IDisposable
    {
      protected readonly TransportPillarsRenderer ParentRenderer;
      private readonly Dict<Pair<TransportProto, TransportPillarAttachmentType>, int> m_attachmentRendererIndices;
      private LystStruct<InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData>> m_renderers;

      public int InstancesCount { get; private set; }

      public PillarsChunkBase(TransportPillarsRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_attachmentRendererIndices = new Dict<Pair<TransportProto, TransportPillarAttachmentType>, int>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.ParentRenderer = parentRenderer;
        Material instancingShared = parentRenderer.m_materialForInstancingShared;
        this.m_renderers.Add(new InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData>(parentRenderer.m_pillarBaseMatMesh.SharedMeshLods, InstancingUtils.InstantiateMaterialAndCopyTextures(instancingShared, parentRenderer.m_pillarBaseMatMesh.SharedMaterial)));
        this.m_renderers.Add(new InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData>(parentRenderer.m_pillarCornerBeamsMatMesh.SharedMeshLods, InstancingUtils.InstantiateMaterialAndCopyTextures(instancingShared, parentRenderer.m_pillarCornerBeamsMatMesh.SharedMaterial)));
        this.m_renderers.Add(new InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData>(parentRenderer.m_pillarXBracesMatMesh.SharedMeshLods, InstancingUtils.InstantiateMaterialAndCopyTextures(instancingShared, parentRenderer.m_pillarXBracesMatMesh.SharedMaterial)));
        this.m_renderers.Add(new InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData>(parentRenderer.m_pillarBeamsWithXBracesMatMesh.SharedMeshLods, InstancingUtils.InstantiateMaterialAndCopyTextures(instancingShared, parentRenderer.m_pillarBeamsWithXBracesMatMesh.SharedMaterial)));
        foreach (InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData> renderer in this.m_renderers)
          parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) renderer);
      }

      public void Dispose()
      {
        foreach (InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData> renderer in this.m_renderers)
          this.ParentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData>>(renderer);
        this.m_renderers.Clear();
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
      {
        info.Add(new RenderedInstancesInfo("Transport pillars (base)", this.m_renderers[0].InstancesCount, this.m_renderers[0].IndicesCountForLod0));
        info.Add(new RenderedInstancesInfo("Transport pillars (beams)", this.m_renderers[1].InstancesCount, this.m_renderers[1].IndicesCountForLod0));
        info.Add(new RenderedInstancesInfo("Transport pillars (x-braces)", this.m_renderers[2].InstancesCount, this.m_renderers[2].IndicesCountForLod0));
        info.Add(new RenderedInstancesInfo("Transport pillars (beams+braces combo)", this.m_renderers[3].InstancesCount, this.m_renderers[3].IndicesCountForLod0));
      }

      protected RenderStats RenderPillars(Bounds bounds, int lod)
      {
        if (lod > 5)
          return new RenderStats();
        RenderStats renderStats = new RenderStats();
        if (lod < 2)
          renderStats += this.m_renderers[0].Render(bounds, lod);
        for (int index = 1; index < this.m_renderers.Count; ++index)
        {
          InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData> renderer = this.m_renderers[index];
          renderStats += renderer.Render(bounds, lod);
        }
        return renderStats;
      }

      protected PooledArray<uint> AddPillarParts(PillarVisualsSpec spec)
      {
        Lyst<uint> usedMeshes = TransportPillarsRenderer.s_usedMeshesTmp;
        if (usedMeshes == null)
        {
          usedMeshes = new Lyst<uint>(true);
          TransportPillarsRenderer.s_usedMeshesTmp = usedMeshes;
        }
        usedMeshes.Clear();
        Vector3 groundCenterVector3 = spec.BasePosition.ToGroundCenterVector3();
        ColorRgba color = spec.IsConstructed ? ColorRgba.White : InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_UNDECIDABLE_COLOR;
        addInstance(0, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(groundCenterVector3, color, Rotation90.Deg0));
        for (int i = 0; i < spec.Layers.Length; ++i)
        {
          Vector3 position = groundCenterVector3 + new Vector3(0.0f, (float) (i * 2), 0.0f);
          PillarLayerSpec layer = spec.Layers[i];
          if (layer.AttachedTransport.HasValue && layer.AttachmentType != TransportPillarAttachmentType.NoAttachment)
          {
            int rendererIndex;
            if (this.tryGetAttachmentRendererIndex(layer.AttachedTransport.Value, layer.AttachmentType, out rendererIndex))
              addInstance(rendererIndex, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(position, color, layer.AttachmentRotation, layer.AttachmentFlipY));
            else
              Log.Warning(string.Format("Failed to find attachment mesh for '{0}' ", (object) layer.AttachmentType) + string.Format("of transport{0}", (object) layer.AttachedTransport.Value.Id));
          }
          if (layer.HasBeamsAndAllBraces)
          {
            addInstance(3, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(position, color, Rotation90.Deg0));
          }
          else
          {
            if (layer.HasBeams)
              addInstance(1, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(position, color, Rotation90.Deg0));
            if (layer.HasAnyFill)
            {
              if (layer.HasFillPlusX)
                addInstance(2, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(position, color, Rotation90.Deg0));
              if (layer.HasFillPlusY)
                addInstance(2, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(position, color, Rotation90.Deg90));
              if (layer.HasFillMinusX)
                addInstance(2, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(position, color, Rotation90.Deg180));
              if (layer.HasFillMinusY)
                addInstance(2, new TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData(position, color, Rotation90.Deg270));
            }
          }
        }
        this.InstancesCount += usedMeshes.Count;
        return usedMeshes.ToPooledArrayAndClear();

        void addInstance(
          int meshIndex,
          TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData data)
        {
          ushort instanceId = this.m_renderers[meshIndex].AddInstance(data);
          usedMeshes.Add(TransportPillarsRenderer.PillarsChunkBase.packRendererAndInstanceIds(meshIndex, instanceId));
        }
      }

      public void RemovePillarParts(PooledArray<uint> parts)
      {
        foreach (uint backing in parts.BackingArray)
        {
          int rendererIndex;
          ushort instanceId;
          TransportPillarsRenderer.PillarsChunkBase.unpackRendererAndInstanceIds(backing, out rendererIndex, out instanceId);
          this.m_renderers[rendererIndex].RemoveInstance(instanceId);
        }
        this.InstancesCount -= parts.Length;
        Assert.That<int>(this.InstancesCount).IsNotNegative();
      }

      private static uint packRendererAndInstanceIds(int rendererIndex, ushort instanceId)
      {
        return (uint) (rendererIndex << 16) | (uint) instanceId;
      }

      private static void unpackRendererAndInstanceIds(
        uint packedValue,
        out int rendererIndex,
        out ushort instanceId)
      {
        rendererIndex = (int) (packedValue >> 16);
        instanceId = (ushort) (packedValue & (uint) ushort.MaxValue);
      }

      private bool tryGetAttachmentRendererIndex(
        TransportProto transportProto,
        TransportPillarAttachmentType attachmentType,
        out int rendererIndex)
      {
        Pair<TransportProto, TransportPillarAttachmentType> key = Pair.Create<TransportProto, TransportPillarAttachmentType>(transportProto, attachmentType);
        if (this.m_attachmentRendererIndices.TryGetValue(key, out rendererIndex))
          return true;
        string prefabPath;
        TransportPillarsRenderer.PillarMeshMat meshAndMaterial;
        if (!transportProto.Graphics.PillarAttachments.TryGetValue(attachmentType, out prefabPath) || !this.ParentRenderer.tryGetMatMesh(prefabPath, out meshAndMaterial))
          return false;
        rendererIndex = this.m_renderers.Count;
        InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData> instance = new InstancedMeshesRenderer<TransportPillarsRenderer.PillarsChunkBase.PillarPartInstanceData>(meshAndMaterial.SharedMeshLods, InstancingUtils.InstantiateMaterialAndCopyTextures(this.ParentRenderer.m_materialForInstancingShared, meshAndMaterial.SharedMaterial));
        this.m_renderers.Add(instance);
        this.ParentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
        this.m_attachmentRendererIndices.Add(key, rendererIndex);
        return true;
      }

      /// <summary>
      /// Per-instance data that is passed to GPU. Layout of this struct must match the `InstanceData` struct
      /// in the `TerrainDesignationInstanced` shader.
      /// </summary>
      [ExpectedStructSize(16)]
      private readonly struct PillarPartInstanceData
      {
        public readonly Vector3 Position;
        public readonly uint RotationAndFlip;

        public PillarPartInstanceData(Vector3 position, ColorRgba color, Rotation90 rotation)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Position = position;
          this.RotationAndFlip = (uint) ((int) color.Rgba & -8 | rotation.AngleIndex);
        }

        public PillarPartInstanceData(
          Vector3 position,
          ColorRgba color,
          Rotation90 rotation,
          bool flipY)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Position = position;
          this.RotationAndFlip = (uint) ((int) color.Rgba & -8 | (flipY ? 4 : 0) | rotation.AngleIndex);
        }
      }
    }

    private sealed class PillarsChunk : 
      TransportPillarsRenderer.PillarsChunkBase,
      IRenderedChunk,
      IRenderedChunksBase
    {
      private static readonly float MAX_DEVIATION;
      private IRenderedChunksParent m_chunkParent;
      private Bounds m_bounds;
      private float m_minHeight;
      private float m_maxHeight;

      public string Name => "Pillars";

      public Vector2 Origin { get; }

      public Chunk256AndIndex CoordAndIndex { get; }

      public bool TrackStoppedRendering => false;

      public float MaxModelDeviationFromChunkBounds
      {
        get => TransportPillarsRenderer.PillarsChunk.MAX_DEVIATION;
      }

      public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

      public PillarsChunk(Chunk256AndIndex coordAndIndex, TransportPillarsRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(parentRenderer);
        this.CoordAndIndex = coordAndIndex;
        this.Origin = coordAndIndex.OriginTile2i.ToVector2();
      }

      public new PooledArray<uint> AddPillarParts(PillarVisualsSpec spec)
      {
        float unityUnits = (float) (spec.BasePosition.Height + new ThicknessTilesI(spec.Layers.Length / 2)).ToUnityUnits();
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
        else if ((double) unityUnits > (double) this.m_maxHeight)
        {
          this.m_maxHeight = unityUnits;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        return base.AddPillarParts(spec);
      }

      public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
      {
        return this.RenderPillars(this.m_bounds, lod);
      }

      public void Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

      public void NotifyWasNotRendered()
      {
      }

      static PillarsChunk()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        TransportPillarsRenderer.PillarsChunk.MAX_DEVIATION = TransportPillarProto.MAX_PILLAR_HEIGHT.ToUnityUnits() / 2f;
      }
    }

    private sealed class PillarsChunkImmediate : 
      TransportPillarsRenderer.PillarsChunkBase,
      IRenderedChunkAlwaysVisible,
      IRenderedChunksBase
    {
      public string Name => "Pillars immediate";

      public PillarsChunkImmediate(TransportPillarsRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(parentRenderer);
      }

      public new PooledArray<uint> AddPillarParts(PillarVisualsSpec spec)
      {
        return base.AddPillarParts(spec);
      }

      public RenderStats RenderAlwaysVisible(GameTime time, Bounds bounds)
      {
        return this.RenderPillars(bounds, 0);
      }
    }
  }
}
