// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.VehiclesPathabilityOverlayRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.PathFinding;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Unity.Camera;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class VehiclesPathabilityOverlayRenderer : IDisposable
  {
    private readonly ClearancePathabilityProvider m_clearancePathabilityProvider;
    private readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
    private readonly CameraController m_cameraController;
    private bool m_isOverlayShownOnSim;
    private int m_currentCapabilityClassIndex;
    private RelTile2i m_coordsOffset;
    private int m_currentCapabilityClassIndexOnSim;
    private RelTile2i m_coordsOffsetOnSim;
    private bool m_markAllChanged;
    private bool m_clearAllData;
    private bool m_clearAllChunks;
    private Tile2i m_previousSortOrigin;
    private readonly Dict<Chunk2i, VehiclesPathabilityOverlayRenderer.VizChunk> m_chunks;
    private readonly Lyst<VehiclesPathabilityOverlayRenderer.VizChunk> m_chunksOrdered;
    private readonly Lyst<ClearancePathabilityProvider.DataChunk> m_newDataChunks;
    private readonly VehiclesPathabilityOverlayRenderer.VizChunk.DistanceComparer m_chunksComparer;
    private readonly Mesh m_overlayMeshShared;
    private readonly Material m_overlayMaterialShared;
    private int m_budgetMultiplier;

    public bool IsOverlayShown => this.ShownPfParams.HasValue;

    public Option<VehiclePathFindingParams> ShownPfParams { get; private set; }

    public VehiclesPathabilityOverlayRenderer(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      ClearancePathabilityProvider clearancePathabilityProvider,
      IVehicleSurfaceProvider vehicleSurfaceProvider,
      CameraController cameraController,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_currentCapabilityClassIndex = -1;
      this.m_chunks = new Dict<Chunk2i, VehiclesPathabilityOverlayRenderer.VizChunk>();
      this.m_chunksOrdered = new Lyst<VehiclesPathabilityOverlayRenderer.VizChunk>();
      this.m_newDataChunks = new Lyst<ClearancePathabilityProvider.DataChunk>();
      this.m_chunksComparer = new VehiclesPathabilityOverlayRenderer.VizChunk.DistanceComparer();
      this.m_budgetMultiplier = 1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_clearancePathabilityProvider = clearancePathabilityProvider;
      this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
      this.m_cameraController = cameraController;
      GameObject sharedPrefabOrThrow = assetsDb.GetSharedPrefabOrThrow("Assets/Core/NavOverlay/NavOverlayTile.prefab");
      this.m_overlayMeshShared = sharedPrefabOrThrow.GetComponent<MeshFilter>().sharedMesh;
      this.m_overlayMaterialShared = sharedPrefabOrThrow.GetComponent<MeshRenderer>().sharedMaterial;
      foreach (ClearancePathabilityProvider.DataChunk dataChunk in this.m_clearancePathabilityProvider.Chunks.Values)
        this.chunkCreated(dataChunk);
      clearancePathabilityProvider.OnChunkCreated += new Action<ClearancePathabilityProvider.DataChunk>(this.chunkCreated);
      clearancePathabilityProvider.AllChunksCleared += new Action(this.clearAllChunks);
      gameLoopEvents.SyncUpdate.AddNonSaveable<VehiclesPathabilityOverlayRenderer>(this, new Action<GameTime>(this.sync));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<VehiclesPathabilityOverlayRenderer>(this, new Action(this.simUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<VehiclesPathabilityOverlayRenderer>(this, new Action<GameTime>(this.renderUpdate));
      gameLoopEvents.Terminate.AddNonSaveable<VehiclesPathabilityOverlayRenderer>(this, new Action(this.terminate));
    }

    public void Dispose()
    {
      foreach (VehiclesPathabilityOverlayRenderer.VizChunk vizChunk in this.m_chunks.Values)
        vizChunk.Dispose();
      this.m_chunks.Clear();
    }

    private void sync(GameTime gameTime)
    {
      if (this.m_clearAllChunks)
      {
        this.m_clearAllChunks = false;
        this.m_clearAllData = false;
        this.m_markAllChanged = false;
        foreach (VehiclesPathabilityOverlayRenderer.VizChunk vizChunk in this.m_chunks.Values)
          vizChunk.Dispose();
        this.m_chunks.Clear();
      }
      if (this.m_newDataChunks.IsNotEmpty)
      {
        foreach (ClearancePathabilityProvider.DataChunk newDataChunk in this.m_newDataChunks)
          this.addDataChunk(newDataChunk);
        this.m_newDataChunks.Clear();
      }
      this.m_isOverlayShownOnSim = this.IsOverlayShown;
      if (!this.IsOverlayShown)
        return;
      this.m_chunksComparer.Origin = this.m_cameraController.CameraModel.State.PivotPosition.Tile2i;
      this.m_currentCapabilityClassIndexOnSim = this.m_currentCapabilityClassIndex;
      this.m_coordsOffsetOnSim = this.m_coordsOffset;
      if (this.m_clearAllData)
      {
        this.m_clearAllData = false;
        this.m_markAllChanged = false;
        foreach (VehiclesPathabilityOverlayRenderer.VizChunk vizChunk in this.m_chunks.Values)
        {
          vizChunk.Clear();
          vizChunk.MarkChangedAll();
        }
      }
      else
      {
        if (!this.m_markAllChanged)
          return;
        this.m_markAllChanged = false;
        foreach (VehiclesPathabilityOverlayRenderer.VizChunk vizChunk in this.m_chunks.Values)
          vizChunk.MarkChangedAll();
      }
    }

    public void SetRuntimeBudgetMultiplierForNextTick(int multiplier)
    {
      this.m_budgetMultiplier = multiplier;
    }

    private void simUpdate()
    {
      if (!this.m_isOverlayShownOnSim)
        return;
      if (this.m_previousSortOrigin.DistanceSqrTo(this.m_chunksComparer.Origin) > 1024L)
      {
        this.m_chunksOrdered.Sort((IComparer<VehiclesPathabilityOverlayRenderer.VizChunk>) this.m_chunksComparer);
        this.m_previousSortOrigin = this.m_chunksComparer.Origin;
      }
      int updateBudget = 50 * this.m_budgetMultiplier;
      this.m_budgetMultiplier = 1;
      if (updateBudget <= 0)
        return;
      foreach (VehiclesPathabilityOverlayRenderer.VizChunk vizChunk in this.m_chunksOrdered)
      {
        vizChunk.UpdateData(this.m_currentCapabilityClassIndexOnSim, ref updateBudget);
        if (updateBudget <= 0)
          break;
      }
    }

    private void addDataChunk(ClearancePathabilityProvider.DataChunk dataChunk)
    {
      Chunk2i chunkCoord2i = dataChunk.OriginTile.ChunkCoord2i;
      VehiclesPathabilityOverlayRenderer.VizChunk vizChunk;
      if (!this.m_chunks.TryGetValue(chunkCoord2i, out vizChunk))
      {
        vizChunk = new VehiclesPathabilityOverlayRenderer.VizChunk(chunkCoord2i, this);
        this.m_chunks.Add(chunkCoord2i, vizChunk);
        this.m_chunksOrdered.Add(vizChunk);
      }
      vizChunk.AddChunk(dataChunk);
    }

    private void renderUpdate(GameTime gameTime)
    {
      if (!this.m_isOverlayShownOnSim)
        return;
      foreach (VehiclesPathabilityOverlayRenderer.VizChunk vizChunk in this.m_chunks.Values)
        vizChunk.Render();
    }

    private void chunkCreated(ClearancePathabilityProvider.DataChunk dataChunk)
    {
      this.m_newDataChunks.Add(dataChunk);
    }

    private void clearAllChunks()
    {
      this.m_newDataChunks.Clear();
      this.m_clearAllChunks = true;
    }

    private void chunkUpdated(ClearancePathabilityProvider.DataChunk dataChunk)
    {
      VehiclesPathabilityOverlayRenderer.VizChunk vizChunk;
      if (!this.m_chunks.TryGetValue(dataChunk.OriginTile.ChunkCoord2i, out vizChunk))
        return;
      vizChunk.MarkChanged(dataChunk);
    }

    public void ShowOverlayFor(VehiclePathFindingParams pathFindingParams)
    {
      if (this.ShownPfParams.IsNone)
      {
        this.m_clearancePathabilityProvider.OnChunkUpdated += new Action<ClearancePathabilityProvider.DataChunk>(this.chunkUpdated);
        this.m_markAllChanged = true;
      }
      this.ShownPfParams = (Option<VehiclePathFindingParams>) pathFindingParams;
      int pathabilityClassIndex = this.m_clearancePathabilityProvider.GetPathabilityClassIndex(pathFindingParams.PathabilityQueryMask);
      if (this.m_currentCapabilityClassIndex == pathabilityClassIndex)
        return;
      this.m_currentCapabilityClassIndex = pathabilityClassIndex;
      this.m_coordsOffset = -pathFindingParams.ConvertToCornerTileSpace(Tile2i.Zero).RelTile2i;
      this.m_clearAllData = true;
    }

    public void HideOverlay()
    {
      if (!this.ShownPfParams.HasValue)
        return;
      this.ShownPfParams = Option<VehiclePathFindingParams>.None;
      this.m_clearancePathabilityProvider.OnChunkUpdated -= new Action<ClearancePathabilityProvider.DataChunk>(this.chunkUpdated);
    }

    private void terminate() => this.clearAllChunks();

    private class VizChunk
    {
      private static readonly int INSTANCE_DATA_SHADER_ID;
      [ThreadStatic]
      private static float[] s_vehicleSurfaceTmp;
      private readonly Tile2i m_centerCoord;
      private readonly VehiclesPathabilityOverlayRenderer m_parent;
      private readonly VehiclesPathabilityOverlayRenderer.VehiclePathabilityInstanceData[] m_tilePathabilityData;
      private readonly ClearancePathabilityProvider.DataChunk[] m_dataChunks;
      private ulong m_dataChunkNeedsUpdate;
      private bool m_instanceDataChanged;
      private readonly Material m_tileValidationMaterial;
      private readonly ComputeBuffer m_drawArgsBuffer;
      private readonly ComputeBuffer m_detailsDataBuffer;
      private Bounds m_bounds;

      public VizChunk(Chunk2i chunkCoord, VehiclesPathabilityOverlayRenderer parent)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_tilePathabilityData = new VehiclesPathabilityOverlayRenderer.VehiclePathabilityInstanceData[4096];
        this.m_dataChunks = new ClearancePathabilityProvider.DataChunk[64];
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_centerCoord = chunkCoord.CenterTile2i;
        this.m_parent = parent;
        this.m_bounds = new Bounds(chunkCoord.CenterTile2i.CenterTile2f.ExtendZ((Fix32) 0).ToVector3(), TerrainChunk.Size2i.ExtendZ(1).ToVector3());
        this.m_tileValidationMaterial = UnityEngine.Object.Instantiate<Material>(parent.m_overlayMaterialShared);
        uint[] numArray = ArrayPool<uint>.Get(5);
        numArray[0] = parent.m_overlayMeshShared.GetIndexCount(0);
        numArray[1] = (uint) this.m_tilePathabilityData.Length;
        numArray[2] = parent.m_overlayMeshShared.GetIndexStart(0);
        numArray[3] = parent.m_overlayMeshShared.GetBaseVertex(0);
        this.m_drawArgsBuffer = new ComputeBuffer(numArray.Length, 4, ComputeBufferType.DrawIndirect);
        this.m_drawArgsBuffer.SetData((Array) numArray);
        numArray.ReturnToPool<uint>();
        this.m_detailsDataBuffer = new ComputeBuffer(this.m_tilePathabilityData.Length, 24);
        this.m_tileValidationMaterial.SetBuffer(VehiclesPathabilityOverlayRenderer.VizChunk.INSTANCE_DATA_SHADER_ID, this.m_detailsDataBuffer);
      }

      private static int getDataChunkIndex(ClearancePathabilityProvider.DataChunk dataChunk)
      {
        TileInChunk2i tileInChunkCoord = dataChunk.OriginTile.TileCoord.TileInChunkCoord;
        return (tileInChunkCoord.X >> 3) + 8 * (tileInChunkCoord.Y >> 3);
      }

      public void AddChunk(ClearancePathabilityProvider.DataChunk dataChunk)
      {
        int dataChunkIndex = VehiclesPathabilityOverlayRenderer.VizChunk.getDataChunkIndex(dataChunk);
        Assert.That<ClearancePathabilityProvider.DataChunk>(this.m_dataChunks[dataChunkIndex]).IsNull<ClearancePathabilityProvider.DataChunk>();
        this.m_dataChunks[dataChunkIndex] = dataChunk;
        this.m_dataChunkNeedsUpdate |= (ulong) (1L << dataChunkIndex);
      }

      public void MarkChanged(ClearancePathabilityProvider.DataChunk dataChunk)
      {
        this.m_dataChunkNeedsUpdate |= (ulong) (1L << VehiclesPathabilityOverlayRenderer.VizChunk.getDataChunkIndex(dataChunk));
      }

      public void MarkChangedAll() => this.m_dataChunkNeedsUpdate = ulong.MaxValue;

      public void UpdateData(int capabilityIndex, ref int updateBudget)
      {
        if (this.m_dataChunkNeedsUpdate == 0UL)
          return;
        ulong maxValue = (ulong) byte.MaxValue;
        int num1 = 0;
        while (num1 < 8)
        {
          if (((long) this.m_dataChunkNeedsUpdate & (long) maxValue) != 0L)
          {
            int index = num1 * 8;
            int num2 = 0;
            while (num2 < 8)
            {
              if (((long) this.m_dataChunkNeedsUpdate & 1L << index) != 0L)
              {
                ClearancePathabilityProvider.DataChunk dataChunk = this.m_dataChunks[index];
                if (dataChunk != null)
                {
                  this.updateData(dataChunk, index * 64, capabilityIndex);
                  --updateBudget;
                }
              }
              ++num2;
              ++index;
            }
          }
          ++num1;
          maxValue <<= 8;
        }
        this.m_dataChunkNeedsUpdate = 0UL;
        this.m_instanceDataChanged = true;
      }

      public void Render()
      {
        if (this.m_instanceDataChanged)
        {
          this.m_instanceDataChanged = false;
          this.m_detailsDataBuffer.SetData((Array) this.m_tilePathabilityData, 0, 0, this.m_tilePathabilityData.Length);
        }
        Graphics.DrawMeshInstancedIndirect(this.m_parent.m_overlayMeshShared, 0, this.m_tileValidationMaterial, this.m_bounds, this.m_drawArgsBuffer, 0, (MaterialPropertyBlock) null, ShadowCastingMode.Off, false, Layer.Custom14TerrainOverlays.ToId(), (UnityEngine.Camera) null, LightProbeUsage.Off, (LightProbeProxyVolume) null);
      }

      public void Dispose()
      {
        this.m_drawArgsBuffer.Dispose();
        this.m_detailsDataBuffer.Dispose();
      }

      private void updateData(
        ClearancePathabilityProvider.DataChunk dataChunk,
        int tileDataIndex,
        int capabilityIndex)
      {
        if (dataChunk.IsDirty)
        {
          for (int index = 0; index < 64; ++index)
            this.m_tilePathabilityData[index + tileDataIndex].MarkDirty();
        }
        else
        {
          if (VehiclesPathabilityOverlayRenderer.VizChunk.s_vehicleSurfaceTmp == null)
            VehiclesPathabilityOverlayRenderer.VizChunk.s_vehicleSurfaceTmp = new float[81];
          float[] vehicleSurfaceTmp = VehiclesPathabilityOverlayRenderer.VizChunk.s_vehicleSurfaceTmp;
          IVehicleSurfaceProvider vehicleSurfaceProvider = this.m_parent.m_vehicleSurfaceProvider;
          Tile2i tile2i1 = (Tile2i) (dataChunk.OriginTile.TileCoordSlim + this.m_parent.m_coordsOffsetOnSim);
          for (int y = 0; y <= 8; ++y)
          {
            int num = y * 9;
            for (int x = 0; x <= 8; ++x)
            {
              Tile2i tile2i2 = tile2i1 + new RelTile2i(x, y);
              HeightTilesF vehicleSurfaceAt = vehicleSurfaceProvider.GetVehicleSurfaceAt(tile2i1 + new RelTile2i(x, y), out bool _);
              this.m_bounds.Encapsulate(tile2i2.ExtendHeight(vehicleSurfaceAt.HeightI).CenterTile3f.ToVector3());
              vehicleSurfaceTmp[num + x] = vehicleSurfaceAt.Value.ToFloat();
            }
          }
          PathabilityBitmap noRecomputeDirty = dataChunk.ComputePathabilityBitmapNoRecomputeDirty(capabilityIndex);
          for (int y = 0; y < 8; ++y)
          {
            int num1 = y * 9;
            int num2 = tileDataIndex + y * 8;
            for (int x = 0; x < 8; ++x)
            {
              Vector2 vector2 = (tile2i1 + new RelTile2i(x, y)).ToVector2();
              int index = num1 + x;
              float self = vehicleSurfaceTmp[index];
              float num3 = vehicleSurfaceTmp[index + 1];
              float num4 = vehicleSurfaceTmp[index + 9];
              float num5 = vehicleSurfaceTmp[index + 10];
              float num6 = self.Min(num3).Min(num4).Min(num5);
              this.m_tilePathabilityData[x + num2] = new VehiclesPathabilityOverlayRenderer.VehiclePathabilityInstanceData(new Vector3(vector2.x, 2f * num6, vector2.y), MafiMath.PackTwoSmallPositiveFloats(self - num6, num3 - num6), MafiMath.PackTwoSmallPositiveFloats(num4 - num6, num5 - num6), noRecomputeDirty.IsPathableAt(x, y));
            }
          }
        }
      }

      public void Clear()
      {
        Array.Clear((Array) this.m_tilePathabilityData, 0, this.m_tilePathabilityData.Length);
        this.m_instanceDataChanged = true;
      }

      static VizChunk()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        VehiclesPathabilityOverlayRenderer.VizChunk.INSTANCE_DATA_SHADER_ID = Shader.PropertyToID("_PerInstanceData");
      }

      public class DistanceComparer : IComparer<VehiclesPathabilityOverlayRenderer.VizChunk>
      {
        public Tile2i Origin { get; set; }

        public int Compare(
          VehiclesPathabilityOverlayRenderer.VizChunk x,
          VehiclesPathabilityOverlayRenderer.VizChunk y)
        {
          return x.m_centerCoord.DistanceSqrTo(this.Origin).CompareTo(y.m_centerCoord.DistanceSqrTo(this.Origin));
        }

        public DistanceComparer()
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          // ISSUE: explicit constructor call
          base.\u002Ector();
        }
      }
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the `InstanceData` struct
    /// in the shader.
    /// </summary>
    [ExpectedStructSize(24)]
    private struct VehiclePathabilityInstanceData
    {
      public readonly Vector3 Position;
      public readonly float DeltaHeights12;
      public readonly float DeltaHeights23;
      public float IsNavigable;

      public VehiclePathabilityInstanceData(
        Vector3 position,
        float deltaHeights12,
        float deltaHeights34,
        bool isNavigable)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.DeltaHeights12 = deltaHeights12;
        this.DeltaHeights23 = deltaHeights34;
        this.IsNavigable = isNavigable ? 1f : -1f;
      }

      public void MarkDirty() => this.IsNavigable *= 2f;
    }
  }
}
