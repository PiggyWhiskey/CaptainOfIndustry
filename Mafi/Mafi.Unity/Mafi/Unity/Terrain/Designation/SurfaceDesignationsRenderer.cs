// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.Designation.SurfaceDesignationsRenderer
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
  public class SurfaceDesignationsRenderer
  {
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly TerrainManager m_terrainManager;
    private readonly TerrainSurfaceTextureManager m_terrainSurfaceTextureManager;
    private readonly IActivator m_terrainGridLinesActivator;
    private readonly Mesh m_surfaceDesignationsMeshShared;
    private readonly Material m_surfaceDesignationsMaterialShared;
    private readonly ActivatorState m_activatorState;
    private readonly Lyst<KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>>> m_designationChunks;
    private readonly Lyst<SurfaceDesignationsRenderer.SurfaceDesignationsChunk> m_designationsChunksSimOnly;
    private bool m_chunksNeedSync;
    private readonly Lyst<KeyValuePair<SurfaceDesignation, bool>> m_toProcess;
    private readonly Lyst<KeyValuePair<Tile2i, bool>> m_showRemovalsToProcess;

    public bool IsActive { get; private set; }

    protected SurfaceDesignationsRenderer(
      SurfaceDesignationsManager designationsManager,
      TerrainRenderer terrainRenderer,
      TerrainManager terrainManager,
      TerrainSurfaceTextureManager terrainSurfaceTextureManager,
      AssetsDb db,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_designationChunks = new Lyst<KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>>>();
      this.m_designationsChunksSimOnly = new Lyst<SurfaceDesignationsRenderer.SurfaceDesignationsChunk>();
      this.m_toProcess = new Lyst<KeyValuePair<SurfaceDesignation, bool>>();
      this.m_showRemovalsToProcess = new Lyst<KeyValuePair<Tile2i, bool>>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      SurfaceDesignationsRenderer designationsRenderer = this;
      this.m_terrainRenderer = terrainRenderer;
      this.m_terrainManager = terrainManager;
      this.m_terrainSurfaceTextureManager = terrainSurfaceTextureManager;
      this.m_activatorState = new ActivatorState(new Action(this.activate), new Action(this.deactivate));
      this.m_surfaceDesignationsMeshShared = MeshBuilder.CreateChunkMesh(4, 2f, true);
      this.m_surfaceDesignationsMaterialShared = db.GetSharedMaterial("Assets/Unity/TerrainDesignations/SurfaceDesignation.mat");
      this.m_terrainGridLinesActivator = terrainRenderer.CreateGridLinesActivator();
      gameLoopEvents.RegisterRendererInitState((object) this, (Action) (() =>
      {
        foreach (SurfaceDesignation placingDesignation in (IEnumerable<SurfaceDesignation>) designationsManager.PlacingDesignations)
          designationsRenderer.getOrCreateChunk(placingDesignation).AddOrUpdateDesignation(placingDesignation);
        foreach (SurfaceDesignation clearingDesignation in (IEnumerable<SurfaceDesignation>) designationsManager.ClearingDesignations)
          designationsRenderer.getOrCreateChunk(clearingDesignation).AddOrUpdateDesignation(clearingDesignation);
      }));
      gameLoopEvents.SyncUpdate.AddNonSaveable<SurfaceDesignationsRenderer>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<SurfaceDesignationsRenderer>(this, new Action<GameTime>(this.renderUpdate));
      gameLoopEvents.Terminate.AddNonSaveable<SurfaceDesignationsRenderer>(this, new Action(this.terminate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<SurfaceDesignationsRenderer>(this, new Action(this.simEndUpdate));
      designationsManager.DesignationAdded.AddNonSaveable<SurfaceDesignationsRenderer>(this, (Action<SurfaceDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<SurfaceDesignation, bool>(d, true))));
      designationsManager.DesignationUpdated.AddNonSaveable<SurfaceDesignationsRenderer>(this, (Action<SurfaceDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<SurfaceDesignation, bool>(d, true))));
      designationsManager.DesignationRemoved.AddNonSaveable<SurfaceDesignationsRenderer>(this, (Action<SurfaceDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<SurfaceDesignation, bool>(d, false))));
      designationsManager.DesignationFulfilledChanged.AddNonSaveable<SurfaceDesignationsRenderer>(this, (Action<SurfaceDesignation>) (d => designationsRenderer.m_toProcess.Add(Make.Kvp<SurfaceDesignation, bool>(d, true))));
    }

    private void syncUpdate(GameTime gameTime)
    {
      if (this.m_toProcess.IsNotEmpty)
      {
        foreach (KeyValuePair<SurfaceDesignation, bool> keyValuePair in this.m_toProcess)
        {
          SurfaceDesignationsRenderer.SurfaceDesignationsChunk chunk = this.getOrCreateChunk(keyValuePair.Key);
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
          foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
          {
            SurfaceDesignationsRenderer.SurfaceDesignationsChunk chunk = this.getOrCreateChunk(designationChunk.Key, keyValuePair.Key.ChunkCoord2i);
            if (keyValuePair.Value)
              chunk.ShowRemoval(keyValuePair.Key);
            else
              chunk.HideRemoval(keyValuePair.Key);
          }
        }
        this.m_showRemovalsToProcess.Clear();
      }
      if (this.m_chunksNeedSync)
      {
        this.m_chunksNeedSync = false;
        this.m_designationsChunksSimOnly.Clear();
        foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
          this.m_designationsChunksSimOnly.AddRange((IEnumerable<SurfaceDesignationsRenderer.SurfaceDesignationsChunk>) designationChunk.Value.Values);
      }
      foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in this.m_designationsChunksSimOnly)
        designationsChunk.SyncUpdate();
    }

    private void simEndUpdate()
    {
      if (!this.IsActive)
        return;
      foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in this.m_designationsChunksSimOnly)
        designationsChunk.ProcessUpdatesOnSim();
    }

    private void renderUpdate(GameTime time)
    {
      if (!this.IsActive)
        return;
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
      {
        foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in designationChunk.Value.Values)
          designationsChunk.RenderDesignations();
      }
    }

    private void terminate()
    {
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
      {
        foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in designationChunk.Value.Values)
          designationsChunk.Dispose();
      }
    }

    private SurfaceDesignationsRenderer.SurfaceDesignationsChunk getOrCreateChunk(
      SurfaceDesignationProto proto,
      Chunk2i coord)
    {
      Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk> dict;
      if (!this.m_designationChunks.TryGetValue<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>>(proto, out dict))
      {
        dict = new Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>();
        this.m_designationChunks.Add(new KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>>(proto, dict));
      }
      SurfaceDesignationsRenderer.SurfaceDesignationsChunk chunk1;
      if (dict.TryGetValue(coord, out chunk1))
        return chunk1;
      SurfaceDesignationsRenderer.SurfaceDesignationsChunk chunk2 = new SurfaceDesignationsRenderer.SurfaceDesignationsChunk(coord, this);
      dict.Add(coord, chunk2);
      this.m_chunksNeedSync = true;
      return chunk2;
    }

    private SurfaceDesignationsRenderer.SurfaceDesignationsChunk getOrCreateChunk(
      SurfaceDesignation designation)
    {
      return this.getOrCreateChunk(designation.Prototype, designation.ChunkCoord);
    }

    public void AddOrUpdatePreviewDesignation(
      SurfaceDesignationProto proto,
      SurfaceDesignationData data)
    {
      this.getOrCreateChunk(proto, data.ChunkCoord).AddOrUpdatePreview(data);
    }

    public void RemovePreviewDesignation(Tile2i originCoord)
    {
      int withinChunkRelIndex = DesignationData.GetWithinChunkRelIndex(originCoord);
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
        this.getOrCreateChunk(designationChunk.Key, originCoord.ChunkCoord2i).RemovePreview(withinChunkRelIndex);
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
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
        this.getOrCreateChunk(designationChunk.Key, originTile.ChunkCoord2i).ShowRemoval(originTile);
    }

    public void HideRemovalImmediate(Tile2i originTile)
    {
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
        this.getOrCreateChunk(designationChunk.Key, originTile.ChunkCoord2i).HideRemoval(originTile);
    }

    public void SetRemovalArea(RectangleTerrainArea2i area)
    {
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
      {
        foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in designationChunk.Value.Values)
          designationsChunk.SetRemovalArea(area);
      }
    }

    public void ClearRemovalArea()
    {
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
      {
        foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in designationChunk.Value.Values)
          designationsChunk.ClearRemovalArea();
      }
    }

    public void SetAdditionArea(RectangleTerrainArea2i area)
    {
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
      {
        foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in designationChunk.Value.Values)
          designationsChunk.SetAdditionArea(area);
      }
    }

    public void ClearAdditionArea()
    {
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
      {
        foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in designationChunk.Value.Values)
          designationsChunk.ClearAdditionArea();
      }
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
      foreach (KeyValuePair<SurfaceDesignationProto, Dict<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>> designationChunk in this.m_designationChunks)
      {
        this.m_chunksNeedSync |= designationChunk.Value.RemoveValues((Predicate<SurfaceDesignationsRenderer.SurfaceDesignationsChunk>) (x => x.IsEmpty), (Action<KeyValuePair<Chunk2i, SurfaceDesignationsRenderer.SurfaceDesignationsChunk>>) (pair => pair.Value.Dispose())) > 0;
        foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk designationsChunk in designationChunk.Value.Values)
          designationsChunk.Deactivate();
      }
    }

    private sealed class SurfaceDesignationsChunk : IDisposable
    {
      private static readonly int REMOVAL_AREA_SHADER_ID;
      private static readonly int ADDITION_AREA_SHADER_ID;
      private static readonly int INSTANCE_DATA_SHADER_ID;
      private readonly Chunk2i m_chunkCoord;
      private readonly SurfaceDesignationsRenderer m_parentRenderer;
      /// <summary>
      /// Converts designation rel ID to data index. This serves as O(1) lookup (way faster than dict).
      /// </summary>
      private readonly short[] m_dataIndexLookup;
      private readonly uint[] m_drawArgs;
      private ComputeBuffer m_drawArgsBuffer;
      private Material m_material;
      private bool m_isHeightTextureSet;
      private int m_designationsToRenderCount;
      private SurfaceDesignation[] m_designations;
      private SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData[] m_renderingData;
      private bool m_renderingDataNeedsCopyToBuffer;
      private ComputeBuffer m_designationsDataBuffer;
      private readonly Dict<int, SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData> m_previews;
      private readonly Lyst<SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData> m_renderingPreviewsData;
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
      private SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData[] m_renderingDataOnSim;
      private float m_minHeightOnSim;
      private float m_maxHeightOnSim;

      public bool IsEmpty => this.m_validDesignationsCountOnSim <= 0;

      public SurfaceDesignationsChunk(
        Chunk2i chunkCoord,
        SurfaceDesignationsRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_dataIndexLookup = new short[4096];
        this.m_drawArgs = new uint[5];
        this.m_designations = new SurfaceDesignation[8];
        this.m_renderingData = new SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData[8];
        this.m_previews = new Dict<int, SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData>();
        this.m_renderingPreviewsData = new Lyst<SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData>(true);
        this.m_removePreviews = new Set<int>();
        this.m_minHeight = (float) int.MaxValue;
        this.m_maxHeight = (float) int.MinValue;
        this.m_needsUpdateBitmapOnSim = new BitMap(4096);
        this.m_renderingDataOnSim = new SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData[8];
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
          this.m_material = UnityEngine.Object.Instantiate<Material>(this.m_parentRenderer.m_surfaceDesignationsMaterialShared);
          this.m_material.SetTexture("_HeightTex", (Texture) this.m_parentRenderer.m_terrainRenderer.HeightTexture);
          this.m_material.SetVector("_TerrainInvSize", new Vector4(1f / (float) (this.m_parentRenderer.m_terrainManager.TerrainWidth * 2), 1f / (float) (this.m_parentRenderer.m_terrainManager.TerrainHeight * 2)));
          this.m_material.SetColor("_defaultRemovalColor", new ColorRgba(16728128, 176).ToColor());
          this.m_material.SetTexture("_SurfaceAlbedoTex", (Texture) this.m_parentRenderer.m_terrainSurfaceTextureManager.DiffuseSurfacesArray);
          this.m_material.SetTexture("_SurfaceNormalsTex", (Texture) this.m_parentRenderer.m_terrainSurfaceTextureManager.NormalSurfacesArray);
          this.m_material.SetTexture("_SurfaceSmoothMetalsTex", (Texture) this.m_parentRenderer.m_terrainSurfaceTextureManager.SmoothMetalSurfaceArray);
          this.m_isHeightTextureSet = true;
        }
        this.m_drawArgs[0] = this.m_parentRenderer.m_surfaceDesignationsMeshShared.GetIndexCount(0);
        this.m_drawArgs[2] = this.m_parentRenderer.m_surfaceDesignationsMeshShared.GetIndexStart(0);
        this.m_drawArgs[3] = this.m_parentRenderer.m_surfaceDesignationsMeshShared.GetBaseVertex(0);
        this.m_drawArgsBuffer = new ComputeBuffer(this.m_drawArgs.Length, 4, ComputeBufferType.DrawIndirect);
        return true;
      }

      private void waitForNextSync()
      {
        this.m_waitForNextSync = true;
        this.m_needsDataUpdateOnSim = true;
      }

      public void AddOrUpdateDesignation(SurfaceDesignation designation)
      {
        int withinChunkRelIndex = designation.WithinChunkRelIndex;
        this.m_needsUpdateBitmapOnSim.SetBit(withinChunkRelIndex);
        int designationsCountOnSim = (int) this.m_dataIndexLookup[withinChunkRelIndex];
        if (designationsCountOnSim < 0)
        {
          if (this.m_renderingData.Length <= this.m_validDesignationsCountOnSim)
          {
            int newSize = this.m_renderingData.Length * 2;
            Array.Resize<SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData>(ref this.m_renderingData, newSize);
            Array.Resize<SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData>(ref this.m_renderingDataOnSim, newSize);
            Array.Resize<SurfaceDesignation>(ref this.m_designations, newSize);
          }
          designationsCountOnSim = this.m_validDesignationsCountOnSim;
          ++this.m_validDesignationsCountOnSim;
          this.m_dataIndexLookup[withinChunkRelIndex] = (short) designationsCountOnSim;
          Assert.That<int>(this.m_validDesignationsCountOnSim).IsLessOrEqual(256);
        }
        this.m_designations[designationsCountOnSim] = designation;
        this.m_needsDataUpdateOnSim = true;
      }

      public void RemoveDesignation(SurfaceDesignation designation)
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
            int index2 = this.m_validDesignationsCountOnSim - 1;
            if (index1 != index2)
            {
              SurfaceDesignation designation1 = this.m_designations[index2];
              this.m_designations[index1] = designation1;
              this.m_renderingDataOnSim[index1] = this.m_renderingDataOnSim[index2];
              this.m_dataIndexLookup[designation1.WithinChunkRelIndex] = (short) index1;
            }
            --this.m_validDesignationsCountOnSim;
            this.m_needsDataUpdateOnSim = true;
          }
        }
      }

      public void AddOrUpdatePreview(SurfaceDesignationData data)
      {
        this.updateMinMaxHeight(this.m_parentRenderer.m_terrainManager.GetHeight((Tile2i) data.OriginTile).ToUnityUnits(), ref this.m_minHeight, ref this.m_maxHeight);
        uint packedData = 0;
        uint packedData2 = (uint) ((int) data.UnassignedTilesBitmap | 262144 | 524288);
        this.m_previews[data.WithinChunkRelIndex] = new SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData(data.OriginTile.AsFull.ToVector2(), packedData, packedData2, (ulong) data.SurfaceProtoSlimId.Value);
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
          this.m_material.SetVector(SurfaceDesignationsRenderer.SurfaceDesignationsChunk.REMOVAL_AREA_SHADER_ID, new Vector4((float) area.Origin.X, (float) area.Origin.Y, (float) area.PlusXyCoordExcl.X, (float) area.PlusXyCoordExcl.Y));
      }

      public void ClearRemovalArea()
      {
        if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          return;
        this.m_material.SetVector(SurfaceDesignationsRenderer.SurfaceDesignationsChunk.REMOVAL_AREA_SHADER_ID, new Vector4(-1f, -1f, -1f, -1f));
      }

      public void SetAdditionArea(RectangleTerrainArea2i area)
      {
        if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          this.tryInitialize();
        if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          Log.Error("Failed to initialize material?");
        else
          this.m_material.SetVector(SurfaceDesignationsRenderer.SurfaceDesignationsChunk.ADDITION_AREA_SHADER_ID, new Vector4((float) area.Origin.X, (float) area.Origin.Y, (float) area.PlusXyCoordExcl.X, (float) area.PlusXyCoordExcl.Y));
      }

      public void ClearAdditionArea()
      {
        if ((UnityEngine.Object) this.m_material == (UnityEngine.Object) null)
          return;
        this.m_material.SetVector(SurfaceDesignationsRenderer.SurfaceDesignationsChunk.ADDITION_AREA_SHADER_ID, new Vector4(-1f, -1f, -1f, -1f));
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

      public void ProcessUpdatesOnSim()
      {
        if (!this.m_needsDataUpdateOnSim)
          return;
        this.m_needsDataUpdateOnSim = false;
        for (int index = 0; index < this.m_validDesignationsCountOnSim; ++index)
        {
          SurfaceDesignation designation = this.m_designations[index];
          Assert.That<SurfaceDesignation>(designation).IsNotNull<SurfaceDesignation>();
          if (this.m_needsUpdateBitmapOnSim.IsSet(designation.WithinChunkRelIndex))
            this.updateDataAt(designation, index);
        }
        this.m_needsUpdateBitmapOnSim.ClearAllBits();
        this.m_simDataNeedSync = true;
      }

      private void updateDataAt(SurfaceDesignation td, int index)
      {
        uint packedData = 0;
        if (!this.m_removePreviews.Contains(td.WithinChunkRelIndex))
          packedData |= td.TilesFulfilledBitmap << 7;
        uint packedData2 = td.UnassignedTilesBitmap & (uint) ushort.MaxValue;
        this.updateMinMaxHeight(this.m_parentRenderer.m_terrainManager.GetHeight((Tile2i) td.OriginTile).ToUnityUnits(), ref this.m_minHeightOnSim, ref this.m_maxHeightOnSim);
        this.m_renderingDataOnSim[index] = new SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData(td.OriginTile.AsFull.ToVector2(), packedData, packedData2, td.SurfaceTypeMap);
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
          foreach (SurfaceDesignationsRenderer.SurfaceDesignationsChunk.SurfaceDesignationInstanceData designationInstanceData in this.m_previews.Values)
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
            this.m_designationsDataBuffer = new ComputeBuffer(num + 8, 24);
            this.m_material.SetBuffer(SurfaceDesignationsRenderer.SurfaceDesignationsChunk.INSTANCE_DATA_SHADER_ID, this.m_designationsDataBuffer);
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
        Graphics.DrawMeshInstancedIndirect(this.m_parentRenderer.m_surfaceDesignationsMeshShared, 0, this.m_material, this.m_bounds, this.m_drawArgsBuffer, 0, (MaterialPropertyBlock) null, ShadowCastingMode.Off, false, Layer.Custom14TerrainOverlays.ToId(), (Camera) null, LightProbeUsage.Off, (LightProbeProxyVolume) null);
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
        this.m_isHeightTextureSet = false;
      }

      static SurfaceDesignationsChunk()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        SurfaceDesignationsRenderer.SurfaceDesignationsChunk.REMOVAL_AREA_SHADER_ID = Shader.PropertyToID("_RemovalArea");
        SurfaceDesignationsRenderer.SurfaceDesignationsChunk.ADDITION_AREA_SHADER_ID = Shader.PropertyToID("_AdditionArea");
        SurfaceDesignationsRenderer.SurfaceDesignationsChunk.INSTANCE_DATA_SHADER_ID = Shader.PropertyToID("_InstanceData");
      }

      /// <summary>
      /// Per-instance data that is passed to GPU. Layout of this struct must match the `InstanceData` struct
      /// in the `TerrainDesignationInstanced` shader.
      /// </summary>
      [ExpectedStructSize(24)]
      private readonly struct SurfaceDesignationInstanceData
      {
        /// <summary>
        /// XY position of the origin and depth at the center in the z channel.
        /// </summary>
        public readonly Vector2 Position;
        /// <summary>
        /// Packed data. Includes warnings per edge.
        /// 1st-7th bits are not used.
        /// 8th-32nd bits (inclusive) are a map of non-fulfilled tiles.
        /// </summary>
        public readonly uint PackedData;
        /// <summary>
        /// 1st-16th bits are "does concreting NOT apply to tile at y = (i / 4) * 4, x = i % 4"
        /// 17th-18th bits are rotation
        /// 19th bit is "repeat first textureId"
        /// 20th bit is "is preview"
        /// Remaining bits unused
        /// </summary>
        public readonly uint PackedData2;
        public readonly ulong PerTileTextureIds;

        public SurfaceDesignationInstanceData(
          Vector2 position,
          uint packedData,
          uint packedData2,
          ulong perTileTextureIds)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Position = position;
          this.PackedData = packedData;
          this.PackedData2 = packedData2;
          this.PerTileTextureIds = perTileTextureIds;
        }
      }
    }
  }
}
