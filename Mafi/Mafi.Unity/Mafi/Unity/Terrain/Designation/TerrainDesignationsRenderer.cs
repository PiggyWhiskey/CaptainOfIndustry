// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.Designation.TerrainDesignationsRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain.Designation
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TerrainDesignationsRenderer : IDisposable
  {
    private readonly TerrainDesignationsManager m_terrainDesignationsManager;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly TerrainManager m_terrainManager;
    private readonly TerrainSurfaceTextureManager m_terrainSurfaceTextureManager;
    private readonly IActivator m_terrainGridLinesActivator;
    private readonly Mesh m_designationsMeshShared;
    private readonly Material m_designationsMaterialShared;
    private readonly ActivatorState m_activatorState;
    private readonly Dict<Chunk2i, TerrainDesignationsRenderer.TerrainDesignationsChunk> m_terrainDesignationChunks;
    private readonly Lyst<TerrainDesignationsRenderer.TerrainDesignationsChunk> m_designationChunksSimOnly;
    private bool m_chunksNeedSync;
    private readonly Lyst<KeyValuePair<TerrainDesignation, bool>> m_toProcess;
    private readonly Lyst<KeyValuePair<Tile2i, bool>> m_showRemovalsToProcess;

    public bool IsActive { get; private set; }

    protected TerrainDesignationsRenderer(
      TerrainDesignationsManager terrainDesignationsManager,
      TerrainRenderer terrainRenderer,
      TerrainManager terrainManager,
      TerrainSurfaceTextureManager terrainSurfaceTextureManager,
      AssetsDb db,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_terrainDesignationChunks = new Dict<Chunk2i, TerrainDesignationsRenderer.TerrainDesignationsChunk>();
      this.m_designationChunksSimOnly = new Lyst<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
      this.m_toProcess = new Lyst<KeyValuePair<TerrainDesignation, bool>>();
      this.m_showRemovalsToProcess = new Lyst<KeyValuePair<Tile2i, bool>>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      TerrainDesignationsRenderer designationsRenderer = this;
      this.m_terrainDesignationsManager = terrainDesignationsManager;
      this.m_terrainRenderer = terrainRenderer;
      this.m_terrainManager = terrainManager;
      this.m_terrainSurfaceTextureManager = terrainSurfaceTextureManager;
      this.m_activatorState = new ActivatorState(new Action(this.activate), new Action(this.deactivate));
      GameObject sharedPrefabOrThrow = db.GetSharedPrefabOrThrow("Assets/Unity/TerrainDesignations/TerrainDesignation.prefab");
      this.m_designationsMeshShared = sharedPrefabOrThrow.GetComponent<MeshFilter>().sharedMesh;
      this.m_designationsMaterialShared = sharedPrefabOrThrow.GetComponent<MeshRenderer>().sharedMaterial;
      this.m_terrainGridLinesActivator = terrainRenderer.CreateGridLinesActivator();
      gameLoopEvents.RegisterRendererInitState((object) this, (Action) (() =>
      {
        foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>) terrainDesignationsManager.Designations)
          designationsRenderer.getOrCreateChunk(designation.ChunkCoord).AddOrUpdateDesignation(designation);
      }));
      gameLoopEvents.SyncUpdate.AddNonSaveable<TerrainDesignationsRenderer>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<TerrainDesignationsRenderer>(this, new Action<GameTime>(this.renderUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<TerrainDesignationsRenderer>(this, new Action(this.simEndUpdate));
      terrainDesignationsManager.DesignationAdded.AddNonSaveable<TerrainDesignationsRenderer>(this, (Action<TerrainDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<TerrainDesignation, bool>(d, true))));
      terrainDesignationsManager.DesignationUpdated.AddNonSaveable<TerrainDesignationsRenderer>(this, (Action<TerrainDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<TerrainDesignation, bool>(d, true))));
      terrainDesignationsManager.DesignationRemoved.AddNonSaveable<TerrainDesignationsRenderer>(this, (Action<TerrainDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<TerrainDesignation, bool>(d, false))));
      terrainDesignationsManager.DesignationFulfilledChanged.AddNonSaveable<TerrainDesignationsRenderer>(this, (Action<TerrainDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<TerrainDesignation, bool>(d, true))));
      terrainDesignationsManager.DesignationManagedTowersChanged.AddNonSaveable<TerrainDesignationsRenderer>(this, (Action<TerrainDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<TerrainDesignation, bool>(d, true))));
      terrainDesignationsManager.DesignationReachabilityChanged.AddNonSaveable<TerrainDesignationsRenderer>(this, (Action<TerrainDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<TerrainDesignation, bool>(d, true))));
    }

    public void Dispose()
    {
      foreach (TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk in this.m_terrainDesignationChunks.Values)
        designationsChunk.Dispose();
    }

    private void syncUpdate(GameTime gameTime)
    {
      if (this.m_toProcess.IsNotEmpty)
      {
        foreach (KeyValuePair<TerrainDesignation, bool> keyValuePair in this.m_toProcess)
        {
          TerrainDesignationsRenderer.TerrainDesignationsChunk chunk = this.getOrCreateChunk(keyValuePair.Key.ChunkCoord);
          if (keyValuePair.Value)
            chunk.AddOrUpdateDesignation(keyValuePair.Key);
          else
            chunk.RemoveDesignation(keyValuePair.Key);
        }
        this.m_toProcess.Clear();
      }
      if (this.m_showRemovalsToProcess.IsNotEmpty)
      {
        foreach (KeyValuePair<Tile2i, bool> keyValuePair in this.m_showRemovalsToProcess)
        {
          TerrainDesignationsRenderer.TerrainDesignationsChunk chunk = this.getOrCreateChunk(keyValuePair.Key.ChunkCoord2i);
          if (keyValuePair.Value)
            chunk.ShowRemoval(keyValuePair.Key);
          else
            chunk.HideRemoval(keyValuePair.Key);
        }
        this.m_showRemovalsToProcess.Clear();
      }
      if (this.m_chunksNeedSync)
      {
        this.m_chunksNeedSync = false;
        this.m_designationChunksSimOnly.Clear();
        this.m_designationChunksSimOnly.AddRange((IEnumerable<TerrainDesignationsRenderer.TerrainDesignationsChunk>) this.m_terrainDesignationChunks.Values);
      }
      foreach (TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk in this.m_designationChunksSimOnly)
        designationsChunk.SyncUpdate();
    }

    private void simEndUpdate()
    {
      if (!this.IsActive)
        return;
      foreach (TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk in this.m_designationChunksSimOnly)
        designationsChunk.PrepareForSimUpdate();
      foreach (TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk in this.m_designationChunksSimOnly)
        designationsChunk.ProcessUpdatesOnSim();
    }

    private void renderUpdate(GameTime time)
    {
      if (!this.IsActive)
        return;
      foreach (TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk in this.m_terrainDesignationChunks.Values)
        designationsChunk.RenderDesignations();
    }

    private TerrainDesignationsRenderer.TerrainDesignationsChunk getOrCreateChunk(Chunk2i coord)
    {
      TerrainDesignationsRenderer.TerrainDesignationsChunk chunk1;
      if (this.m_terrainDesignationChunks.TryGetValue(coord, out chunk1))
        return chunk1;
      TerrainDesignationsRenderer.TerrainDesignationsChunk chunk2 = new TerrainDesignationsRenderer.TerrainDesignationsChunk(coord, this);
      this.m_terrainDesignationChunks.Add(coord, chunk2);
      this.m_chunksNeedSync = true;
      chunk2.ConnectToNbrs();
      return chunk2;
    }

    public void AddOrUpdatePreviewDesignation(TerrainDesignationProto proto, DesignationData data)
    {
      this.getOrCreateChunk(data.ChunkCoord).AddOrUpdatePreview(proto, data);
    }

    public void RemovePreviewDesignation(Tile2i originCoord)
    {
      this.getOrCreateChunk(originCoord.ChunkCoord2i).RemovePreview(DesignationData.GetWithinChunkRelIndex(originCoord));
    }

    public void ShowRemoval(Tile2i originTile)
    {
      this.m_showRemovalsToProcess.Add(Make.Kvp<Tile2i, bool>(originTile, true));
    }

    public void HideRemoval(Tile2i originTile)
    {
      this.m_showRemovalsToProcess.Add(Make.Kvp<Tile2i, bool>(originTile, false));
    }

    public void ShowRemovalImmediate(Tile2i originTile)
    {
      this.getOrCreateChunk(originTile.ChunkCoord2i).ShowRemoval(originTile);
    }

    public void HideRemovalImmediate(Tile2i originTile)
    {
      this.getOrCreateChunk(originTile.ChunkCoord2i).HideRemoval(originTile);
    }

    public IActivator CreateActivator() => this.m_activatorState.CreateActivator();

    public IActivator CreateActivatorCombinedWithTerrainGrid()
    {
      return this.CreateActivator().Combine(this.m_terrainGridLinesActivator);
    }

    private void activate()
    {
      if (this.IsActive)
        return;
      this.IsActive = true;
    }

    private void deactivate()
    {
      if (!this.IsActive)
        return;
      this.IsActive = false;
      this.m_chunksNeedSync |= this.m_terrainDesignationChunks.RemoveValues((Predicate<TerrainDesignationsRenderer.TerrainDesignationsChunk>) (x => x.IsEmpty), (Action<KeyValuePair<Chunk2i, TerrainDesignationsRenderer.TerrainDesignationsChunk>>) (pair => pair.Value.Dispose())) > 0;
      foreach (TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk in this.m_terrainDesignationChunks.Values)
        designationsChunk.Deactivate();
    }

    private sealed class TerrainDesignationsChunk : IDisposable
    {
      private static readonly int REMOVAL_AREA_SHADER_ID;
      private static readonly int INSTANCE_DATA_SHADER_ID;
      private readonly Chunk2i m_chunkCoord;
      private readonly TerrainDesignationsRenderer m_parentRenderer;
      private Option<TerrainDesignationsRenderer.TerrainDesignationsChunk> m_plusXNbr;
      private Option<TerrainDesignationsRenderer.TerrainDesignationsChunk> m_plusYNbr;
      private Option<TerrainDesignationsRenderer.TerrainDesignationsChunk> m_minusXNbr;
      private Option<TerrainDesignationsRenderer.TerrainDesignationsChunk> m_minusYNbr;
      /// <summary>
      /// Converts designation rel ID to data index. This serves as O(1) lookup (way faster than dict).
      /// </summary>
      private readonly short[] m_dataIndexLookup;
      private readonly uint[] m_drawArgs;
      private ComputeBuffer m_drawArgsBuffer;
      private Material m_material;
      private bool m_isHeightTextureSet;
      private int m_designationsToRenderCount;
      private TerrainDesignation[] m_designations;
      private TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData[] m_renderingData;
      private bool m_renderingDataNeedsCopyToBuffer;
      private ComputeBuffer m_designationsDataBuffer;
      private readonly Dict<int, TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData> m_previews;
      private readonly Lyst<TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData> m_renderingPreviewsData;
      private bool m_previewsChanged;
      private bool m_waitForNextSync;
      private readonly Set<int> m_removePreviews;
      private float m_minHeight;
      private float m_maxHeight;
      private readonly Vector3 m_chunkCenter;
      private readonly Vector3 m_chunkSize;
      private Bounds m_bounds;
      private bool m_boundsNeedUpdate;
      private bool m_simDataNeedSync;
      private bool m_needsDataUpdateOnSim;
      private int m_validDesignationsCountOnSim;
      private readonly BitMap m_needsUpdateBitmapOnSim;
      private TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData[] m_renderingDataOnSim;
      private float m_minHeightOnSim;
      private float m_maxHeightOnSim;

      public bool IsEmpty => this.m_validDesignationsCountOnSim <= 0;

      public TerrainDesignationsChunk(
        Chunk2i chunkCoord,
        TerrainDesignationsRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_dataIndexLookup = new short[256];
        this.m_drawArgs = new uint[5];
        this.m_designations = new TerrainDesignation[8];
        this.m_renderingData = new TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData[8];
        this.m_previews = new Dict<int, TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData>();
        this.m_renderingPreviewsData = new Lyst<TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData>(true);
        this.m_removePreviews = new Set<int>();
        this.m_minHeight = (float) int.MaxValue;
        this.m_maxHeight = (float) int.MinValue;
        this.m_needsUpdateBitmapOnSim = new BitMap(512);
        this.m_renderingDataOnSim = new TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData[8];
        this.m_minHeightOnSim = (float) int.MaxValue;
        this.m_maxHeightOnSim = (float) int.MinValue;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_chunkCoord = chunkCoord;
        this.m_parentRenderer = parentRenderer;
        this.m_chunkSize = TerrainChunk.Size2i.ExtendZ(0).ToVector3();
        this.m_chunkCenter = chunkCoord.Tile2i.ExtendZ(0).ToCornerVector3() + this.m_chunkSize / 2f;
        this.waitForNextSync();
        for (int index = 0; index < this.m_dataIndexLookup.Length; ++index)
          this.m_dataIndexLookup[index] = (short) -1;
      }

      private bool tryInitialize()
      {
        if (!this.m_isHeightTextureSet)
        {
          this.m_material = UnityEngine.Object.Instantiate<Material>(this.m_parentRenderer.m_designationsMaterialShared);
          this.m_material.SetTexture("_HeightTex", (Texture) this.m_parentRenderer.m_terrainRenderer.HeightTexture);
          this.m_material.SetVector("_TerrainInvSize", new Vector4(1f / (float) (this.m_parentRenderer.m_terrainManager.TerrainWidth * 2), 1f / (float) (this.m_parentRenderer.m_terrainManager.TerrainHeight * 2)));
          this.m_material.SetColor("_defaultColorAbove", new ColorRgba(10534976, 176).ToColor());
          this.m_material.SetColor("_defaultColorBelow", new ColorRgba(16756800, 176).ToColor());
          this.m_material.SetColor("_defaultColorReadyAbove", new ColorRgba(6340672, 176).ToColor());
          this.m_material.SetColor("_defaultColorReadyBelow", new ColorRgba(16764992, 176).ToColor());
          this.m_material.SetColor("_defaultColorFulfilled", new ColorRgba(10405952, 64).ToColor());
          this.m_material.SetTexture("_SurfaceAlbedoTex", (Texture) this.m_parentRenderer.m_terrainSurfaceTextureManager.DiffuseSurfacesArray);
          this.m_material.SetTexture("_SurfaceNormalsTex", (Texture) this.m_parentRenderer.m_terrainSurfaceTextureManager.NormalSurfacesArray);
          this.m_material.SetTexture("_SurfaceSmoothMetalsTex", (Texture) this.m_parentRenderer.m_terrainSurfaceTextureManager.SmoothMetalSurfaceArray);
          this.m_isHeightTextureSet = true;
        }
        this.m_drawArgs[0] = this.m_parentRenderer.m_designationsMeshShared.GetIndexCount(0);
        this.m_drawArgs[2] = this.m_parentRenderer.m_designationsMeshShared.GetIndexStart(0);
        this.m_drawArgs[3] = this.m_parentRenderer.m_designationsMeshShared.GetBaseVertex(0);
        this.m_drawArgsBuffer = new ComputeBuffer(this.m_drawArgs.Length, 4, ComputeBufferType.DrawIndirect);
        return true;
      }

      private void waitForNextSync()
      {
        this.m_waitForNextSync = true;
        this.m_needsDataUpdateOnSim = true;
      }

      public void AddOrUpdateDesignation(TerrainDesignation designation)
      {
        int withinChunkRelIndex = designation.WithinChunkRelIndex;
        this.m_needsUpdateBitmapOnSim.SetBit(withinChunkRelIndex);
        int designationsCountOnSim = (int) this.m_dataIndexLookup[withinChunkRelIndex];
        if (designationsCountOnSim < 0)
        {
          if (this.m_renderingData.Length <= this.m_validDesignationsCountOnSim)
          {
            int newSize = this.m_renderingData.Length * 2;
            Array.Resize<TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData>(ref this.m_renderingData, newSize);
            Array.Resize<TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData>(ref this.m_renderingDataOnSim, newSize);
            Array.Resize<TerrainDesignation>(ref this.m_designations, newSize);
          }
          designationsCountOnSim = this.m_validDesignationsCountOnSim;
          ++this.m_validDesignationsCountOnSim;
          this.m_dataIndexLookup[withinChunkRelIndex] = (short) designationsCountOnSim;
          Assert.That<int>(this.m_validDesignationsCountOnSim).IsLessOrEqual(256);
        }
        this.m_designations[designationsCountOnSim] = designation;
        this.m_needsDataUpdateOnSim = true;
      }

      public void RemoveDesignation(TerrainDesignation designation)
      {
        int index1 = (int) this.m_dataIndexLookup[designation.WithinChunkRelIndex];
        if (index1 < 0)
        {
          Log.Error("Failed to remove designation from rendering.");
        }
        else
        {
          this.m_dataIndexLookup[designation.WithinChunkRelIndex] = (short) -1;
          if (this.m_validDesignationsCountOnSim <= 0)
          {
            Log.Error("Failed to remove designation from rendering, no designations displayed.");
          }
          else
          {
            this.markNeighborsForForcedUpdate(designation.WithinChunkRelCoord);
            int index2 = this.m_validDesignationsCountOnSim - 1;
            if (index1 != index2)
            {
              TerrainDesignation designation1 = this.m_designations[index2];
              this.m_designations[index1] = designation1;
              this.m_renderingDataOnSim[index1] = this.m_renderingDataOnSim[index2];
              this.m_dataIndexLookup[designation1.WithinChunkRelIndex] = (short) index1;
            }
            --this.m_validDesignationsCountOnSim;
            this.m_needsDataUpdateOnSim = true;
          }
        }
      }

      public void AddOrUpdatePreview(TerrainDesignationProto proto, DesignationData data)
      {
        float unityUnits = (float) data.OriginTargetHeight.ToUnityUnits();
        this.updateMinMaxHeight(unityUnits, ref this.m_minHeight, ref this.m_maxHeight);
        uint packedData = 0;
        ColorRgba c = proto.Graphics.ColorCanBeFulfilled;
        if (proto.Id.Value == "ForestryDesignator")
        {
          packedData |= 16U;
          if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          {
            this.tryInitialize();
            for (int y = 0; y <= 4; ++y)
            {
              for (int x = 0; x <= 4; ++x)
              {
                RelTile2i relTile2i = new RelTile2i(x, y);
                bool flag = x == 4 || y == 4;
                Tile2i tile = data.OriginTile + relTile2i;
                if (!proto.IsFulfilledFn((ITerrainDesignationsManager) this.m_parentRenderer.m_terrainDesignationsManager, this.m_parentRenderer.m_terrainManager.ExtendTileIndex(tile), data.CenterTargetHeight, flag))
                {
                  c = proto.Graphics.ColorCanNotBeFulfilled;
                  y = 5;
                  break;
                }
              }
            }
          }
        }
        else if (proto.Id.Value == "LevelDesignator")
          packedData |= 32U;
        this.m_previews[data.WithinChunkRelIndex] = new TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData(data.OriginTile.ToVector2(), data.CenterTargetHeight.ToUnityUnits(), packedData, new Vector4(unityUnits, (float) data.PlusXTargetHeight.ToUnityUnits(), (float) data.PlusXyTargetHeight.ToUnityUnits(), (float) data.PlusYTargetHeight.ToUnityUnits()), c.ToColor());
        this.m_previewsChanged = true;
      }

      public void RemovePreview(int withinChunkRelIndex)
      {
        this.m_previews.Remove(withinChunkRelIndex);
        this.m_previewsChanged = true;
      }

      public void ShowRemoval(Tile2i originTile)
      {
        int withinChunkRelIndex = DesignationData.GetWithinChunkRelIndex(originTile);
        this.m_removePreviews.Add(withinChunkRelIndex);
        this.m_needsUpdateBitmapOnSim.SetBit(withinChunkRelIndex);
        this.m_needsDataUpdateOnSim = true;
      }

      public void HideRemoval(Tile2i originTile)
      {
        int withinChunkRelIndex = DesignationData.GetWithinChunkRelIndex(originTile);
        this.m_removePreviews.Remove(withinChunkRelIndex);
        this.m_needsUpdateBitmapOnSim.SetBit(withinChunkRelIndex);
        this.m_needsDataUpdateOnSim = true;
      }

      public void SetRemovalArea(RectangleTerrainArea2i area)
      {
        if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          this.tryInitialize();
        if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          Log.Error("Failed to initialize material?");
        else
          this.m_material.SetVector(TerrainDesignationsRenderer.TerrainDesignationsChunk.REMOVAL_AREA_SHADER_ID, new Vector4((float) area.Origin.X, (float) area.Origin.Y, (float) area.PlusXyCoordExcl.X, (float) area.PlusXyCoordExcl.Y));
      }

      public void ClearRemovalArea()
      {
        if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          return;
        this.m_material.SetVector(TerrainDesignationsRenderer.TerrainDesignationsChunk.REMOVAL_AREA_SHADER_ID, new Vector4(-1f, -1f, -1f, -1f));
      }

      public void SyncUpdate()
      {
        if (this.m_simDataNeedSync)
        {
          this.m_simDataNeedSync = false;
          Array.Copy((Array) this.m_renderingDataOnSim, (Array) this.m_renderingData, this.m_validDesignationsCountOnSim);
          this.m_renderingDataNeedsCopyToBuffer = true;
          this.m_designationsToRenderCount = this.m_validDesignationsCountOnSim;
          this.m_waitForNextSync = false;
        }
        if (!this.m_boundsNeedUpdate)
          return;
        this.m_boundsNeedUpdate = false;
        if ((double) this.m_minHeightOnSim < (double) this.m_minHeight)
          this.m_minHeight = this.m_minHeightOnSim;
        if ((double) this.m_maxHeightOnSim > (double) this.m_maxHeight)
          this.m_maxHeight = this.m_maxHeightOnSim;
        Vector3 chunkCenter = this.m_chunkCenter;
        Vector3 chunkSize = this.m_chunkSize;
        float num1 = this.m_minHeight.Min(chunkCenter.y - chunkSize.y * 0.5f);
        float num2 = this.m_maxHeight.Max(chunkCenter.y + chunkSize.y * 0.5f);
        chunkCenter.y = (float) (((double) num1 + (double) num2) / 2.0);
        chunkSize.y = num2 - num1;
        this.m_bounds = new Bounds(chunkCenter, chunkSize);
      }

      private void markNeighborsForForcedUpdate(Vector2i relCoord)
      {
        if (relCoord.X < 15)
          markForForcedUpdate(this, DesignationData.GetWithinChunkRelIndex(relCoord.IncrementX));
        else if (this.m_plusXNbr.HasValue)
          markForForcedUpdate(this.m_plusXNbr.Value, DesignationData.GetWithinChunkRelIndex(relCoord.SetX(0)));
        if (relCoord.Y < 15)
          markForForcedUpdate(this, DesignationData.GetWithinChunkRelIndex(relCoord.IncrementY));
        else if (this.m_plusYNbr.HasValue)
          markForForcedUpdate(this.m_plusYNbr.Value, DesignationData.GetWithinChunkRelIndex(relCoord.SetY(0)));
        if (relCoord.X > 0)
          markForForcedUpdate(this, DesignationData.GetWithinChunkRelIndex(relCoord.DecrementX));
        else if (this.m_minusXNbr.HasValue)
          markForForcedUpdate(this.m_minusXNbr.Value, DesignationData.GetWithinChunkRelIndex(relCoord.SetX(15)));
        if (relCoord.Y > 0)
        {
          markForForcedUpdate(this, DesignationData.GetWithinChunkRelIndex(relCoord.DecrementY));
        }
        else
        {
          if (!this.m_minusYNbr.HasValue)
            return;
          markForForcedUpdate(this.m_minusYNbr.Value, DesignationData.GetWithinChunkRelIndex(relCoord.SetY(15)));
        }

        static void markForForcedUpdate(
          TerrainDesignationsRenderer.TerrainDesignationsChunk chunk,
          int key)
        {
          if (chunk.m_dataIndexLookup[key] < (short) 0 || !chunk.m_needsUpdateBitmapOnSim.IsNotSet(key))
            return;
          chunk.m_needsUpdateBitmapOnSim.SetBit(key);
          chunk.m_needsUpdateBitmapOnSim.SetBit(key + 256);
          chunk.m_needsDataUpdateOnSim = true;
        }
      }

      public void PrepareForSimUpdate()
      {
        if (!this.m_needsDataUpdateOnSim)
          return;
        for (int index = 0; index < this.m_validDesignationsCountOnSim; ++index)
        {
          TerrainDesignation designation = this.m_designations[index];
          Assert.That<TerrainDesignation>(designation).IsNotNull<TerrainDesignation>();
          if (!this.m_needsUpdateBitmapOnSim.IsNotSet(designation.WithinChunkRelIndex) && !this.m_needsUpdateBitmapOnSim.IsSet(designation.WithinChunkRelIndex + 256))
            this.markNeighborsForForcedUpdate(designation.WithinChunkRelCoord);
        }
      }

      public void ProcessUpdatesOnSim()
      {
        if (!this.m_needsDataUpdateOnSim)
          return;
        this.m_needsDataUpdateOnSim = false;
        for (int index = 0; index < this.m_validDesignationsCountOnSim; ++index)
        {
          TerrainDesignation designation = this.m_designations[index];
          Assert.That<TerrainDesignation>(designation).IsNotNull<TerrainDesignation>();
          if (this.m_needsUpdateBitmapOnSim.IsSet(designation.WithinChunkRelIndex))
            this.updateDataAt(designation, index);
        }
        this.m_needsUpdateBitmapOnSim.ClearAllBits();
        this.m_simDataNeedSync = true;
      }

      private void updateDataAt(TerrainDesignation td, int index)
      {
        DesignationData data = td.Data;
        uint packedData = (uint) ((td.DisplayWarningTowards(NeighborCoord.MinusY) ? 1 : 0) | (td.DisplayWarningTowards(NeighborCoord.PlusX) ? 2 : 0) | (td.DisplayWarningTowards(NeighborCoord.PlusY) ? 4 : 0) | (td.DisplayWarningTowards(NeighborCoord.MinusX) ? 8 : 0));
        if (td.IsForestry)
          packedData |= 16U;
        ColorRgba c;
        if (this.m_removePreviews.Contains(td.WithinChunkRelIndex))
        {
          c = td.Prototype.Graphics.ColorRemove;
        }
        else
        {
          if (td.Prototype.Id == IdsCore.TerrainDesignators.LevelDesignator)
            packedData |= 32U;
          if (td.IsReadyToBeFulfilled)
            packedData |= 64U;
          packedData |= td.TilesFulfilledBitmap << 7;
          c = !td.IsForestry ? (td.IsFulfilled ? td.Prototype.Graphics.ColorIsFulfilled : (td.IsReadyToBeFulfilled ? td.Prototype.Graphics.ColorCanBeFulfilled : td.Prototype.Graphics.ColorCanNotBeFulfilled)) : (!td.IsFulfilled || td.ManagedByTowers.IsEmpty() ? (td.IsFulfilled ? td.Prototype.Graphics.ColorCanBeFulfilled : td.Prototype.Graphics.ColorCanNotBeFulfilled) : td.Prototype.Graphics.ColorIsFulfilled);
        }
        float unityUnits = (float) data.OriginTargetHeight.ToUnityUnits();
        this.updateMinMaxHeight(unityUnits, ref this.m_minHeightOnSim, ref this.m_maxHeightOnSim);
        this.m_renderingDataOnSim[index] = new TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData(data.OriginTile.ToVector2(), data.CenterTargetHeight.ToUnityUnits(), packedData, new Vector4(unityUnits, (float) data.PlusXTargetHeight.ToUnityUnits(), (float) data.PlusXyTargetHeight.ToUnityUnits(), (float) data.PlusYTargetHeight.ToUnityUnits()), c.ToColor());
      }

      private void updateMinMaxHeight(float heightAtOrigin, ref float min, ref float max)
      {
        if ((double) heightAtOrigin - 2.0 < (double) min)
        {
          min = heightAtOrigin - 2f;
          this.m_boundsNeedUpdate = true;
        }
        if ((double) heightAtOrigin + 2.0 <= (double) max)
          return;
        max = heightAtOrigin + 2f;
        this.m_boundsNeedUpdate = true;
      }

      public void RenderDesignations()
      {
        if (this.m_drawArgsBuffer == null && !this.tryInitialize())
          return;
        Assert.That<Material>(this.m_material).IsNotNull<Material>();
        if (this.m_waitForNextSync)
          return;
        bool flag = false;
        if (this.m_previewsChanged)
        {
          this.m_previewsChanged = false;
          this.m_renderingPreviewsData.Clear();
          foreach (TerrainDesignationsRenderer.TerrainDesignationsChunk.TerrainDesignationInstanceData designationInstanceData in this.m_previews.Values)
            this.m_renderingPreviewsData.Add(designationInstanceData);
          flag = !this.m_renderingDataNeedsCopyToBuffer;
          this.m_renderingDataNeedsCopyToBuffer = true;
        }
        if (this.m_renderingDataNeedsCopyToBuffer)
        {
          this.m_renderingDataNeedsCopyToBuffer = false;
          int num = this.m_designationsToRenderCount + this.m_renderingPreviewsData.Count;
          if (this.m_designationsDataBuffer == null || this.m_designationsDataBuffer.count < num)
          {
            this.m_designationsDataBuffer?.Dispose();
            this.m_designationsDataBuffer = new ComputeBuffer(num + 8, 48);
            this.m_material.SetBuffer(TerrainDesignationsRenderer.TerrainDesignationsChunk.INSTANCE_DATA_SHADER_ID, this.m_designationsDataBuffer);
            flag = false;
          }
          if (this.m_designationsToRenderCount > 0 && !flag)
            this.m_designationsDataBuffer.SetData((Array) this.m_renderingData, 0, 0, this.m_designationsToRenderCount);
          if (this.m_renderingPreviewsData.Count > 0)
            this.m_designationsDataBuffer.SetData((Array) this.m_renderingPreviewsData.GetBackingArray(), 0, this.m_designationsToRenderCount, this.m_renderingPreviewsData.Count);
          if ((long) this.m_drawArgs[1] != (long) num)
          {
            this.m_drawArgs[1] = (uint) num;
            this.m_drawArgsBuffer.SetData((Array) this.m_drawArgs);
          }
        }
        if (this.m_drawArgs[1] <= 0U)
          return;
        Graphics.DrawMeshInstancedIndirect(this.m_parentRenderer.m_designationsMeshShared, 0, this.m_material, this.m_bounds, this.m_drawArgsBuffer, 0, (MaterialPropertyBlock) null, ShadowCastingMode.Off, false, Layer.Custom14TerrainOverlays.ToId(), (Camera) null, LightProbeUsage.Off, (LightProbeProxyVolume) null);
      }

      public void ConnectToNbrs()
      {
        Dict<Chunk2i, TerrainDesignationsRenderer.TerrainDesignationsChunk> designationChunks = this.m_parentRenderer.m_terrainDesignationChunks;
        TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk1;
        if (designationChunks.TryGetValue(this.m_chunkCoord.IncrementX, out designationsChunk1))
        {
          Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(this.m_plusXNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
          Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(designationsChunk1.m_minusXNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
          this.m_plusXNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) designationsChunk1;
          designationsChunk1.m_minusXNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) this;
        }
        TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk2;
        if (designationChunks.TryGetValue(this.m_chunkCoord.IncrementY, out designationsChunk2))
        {
          Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(this.m_plusYNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
          Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(designationsChunk2.m_minusYNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
          this.m_plusYNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) designationsChunk2;
          designationsChunk2.m_minusYNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) this;
        }
        TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk3;
        if (designationChunks.TryGetValue(this.m_chunkCoord.DecrementX, out designationsChunk3))
        {
          Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(this.m_minusXNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
          Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(designationsChunk3.m_plusXNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
          this.m_minusXNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) designationsChunk3;
          designationsChunk3.m_plusXNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) this;
        }
        TerrainDesignationsRenderer.TerrainDesignationsChunk designationsChunk4;
        if (!designationChunks.TryGetValue(this.m_chunkCoord.DecrementY, out designationsChunk4))
          return;
        Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(this.m_minusYNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
        Assert.That<Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>>(designationsChunk4.m_plusYNbr).IsNone<TerrainDesignationsRenderer.TerrainDesignationsChunk>();
        this.m_minusYNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) designationsChunk4;
        designationsChunk4.m_plusYNbr = (Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>) this;
      }

      public void Deactivate()
      {
        this.waitForNextSync();
        this.m_boundsNeedUpdate = true;
      }

      public void Dispose()
      {
        this.m_drawArgsBuffer?.Dispose();
        this.m_drawArgsBuffer = (ComputeBuffer) null;
        this.m_designationsDataBuffer?.Dispose();
        this.m_designationsDataBuffer = (ComputeBuffer) null;
        if (this.m_plusXNbr.HasValue)
        {
          this.m_plusXNbr.Value.m_minusXNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
          this.m_plusXNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
        }
        if (this.m_plusYNbr.HasValue)
        {
          this.m_plusYNbr.Value.m_minusYNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
          this.m_plusYNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
        }
        if (this.m_minusXNbr.HasValue)
        {
          this.m_minusXNbr.Value.m_plusXNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
          this.m_minusXNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
        }
        if (this.m_minusYNbr.HasValue)
        {
          this.m_minusYNbr.Value.m_plusYNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
          this.m_minusYNbr = Option<TerrainDesignationsRenderer.TerrainDesignationsChunk>.None;
        }
        this.m_isHeightTextureSet = false;
      }

      static TerrainDesignationsChunk()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        TerrainDesignationsRenderer.TerrainDesignationsChunk.REMOVAL_AREA_SHADER_ID = Shader.PropertyToID("_RemovalArea");
        TerrainDesignationsRenderer.TerrainDesignationsChunk.INSTANCE_DATA_SHADER_ID = Shader.PropertyToID("_InstanceData");
      }

      /// <summary>
      /// Per-instance data that is passed to GPU. Layout of this struct must match the `InstanceData` struct
      /// in the `TerrainDesignationInstanced` shader.
      /// </summary>
      [ExpectedStructSize(48)]
      private readonly struct TerrainDesignationInstanceData
      {
        /// <summary>
        /// XY position of the origin and depth at the center in the z channel.
        /// </summary>
        public readonly Vector2 Position;
        /// <summary>Height at</summary>
        public readonly float HeightAtCenter;
        /// <summary>
        /// Packed data. Includes warnings per edge.
        /// 1st bit is -Y edge.
        /// 2nd bit is +X edge.
        /// 3rd bit is +Y edge.
        /// 4th bit is -Y edge.
        /// 5th bit marks us as forestry.
        /// 6th bit is "self-compute colour", which also means "is mixed mining/dumping".
        /// 7th bit is "ready to be fulfilled".
        /// 8th to 32nd bits (inclusive) are a map of non-fulfilled tiles.
        /// </summary>
        public readonly uint PackedData;
        /// <summary>Depth at the four corners: origin, +X, +XY, +Y.</summary>
        public readonly Vector4 HeightAtCorners;
        /// <summary>Color and depth at the center in the alpha channel.</summary>
        public readonly Color Color;

        public TerrainDesignationInstanceData(
          Vector2 position,
          float heightAtCenter,
          uint packedData,
          Vector4 heightAtCorners,
          Color color)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Position = position;
          this.HeightAtCenter = heightAtCenter;
          this.PackedData = packedData;
          this.HeightAtCorners = heightAtCorners;
          this.Color = color.linear;
        }
      }
    }
  }
}
