// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainGeneratorV2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>
  /// This class handles the complexity or parallel terrain generation. It has no state and configuration that affects
  /// the resulting map.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public sealed class TerrainGeneratorV2 : ITerrainGeneratorV2
  {
    private readonly TerrainGeneratorV2Config m_config;
    private readonly IResolver m_resolver;
    private readonly ObjectPool<TerrainGeneratorChunkData> m_chunkDataPool;
    private Option<CancellationTokenSource> m_cancellationTokenSource;
    private readonly Queueue<int> m_oceanSeeds;
    private readonly Lyst<TerrainMaterialThicknessSlim> m_layersTmp;
    private Option<Task> m_backgroundTerGenTask;
    private Option<Task> m_backgroundPostProcessTask;
    private Option<CustomTerrainPostProcessorV2> m_runningCustomPostProcessor;
    private Option<ConcurrentDictionary<ITerrainFeatureGenerator, long>> m_perfData;
    /// <summary>
    /// Vertices cannot go lower than -MAP_HEIGHT_CAP, nor higher than MAP_HEIGHT_CAP.
    /// </summary>
    public const int MAP_HEIGHT_CAP = 2048;

    public TerrainGeneratorV2(TerrainGeneratorV2Config config, IResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_chunkDataPool = new ObjectPool<TerrainGeneratorChunkData>(2 * Environment.ProcessorCount, (Func<TerrainGeneratorChunkData>) (() => new TerrainGeneratorChunkData()), (Action<TerrainGeneratorChunkData>) (_ => { }));
      this.m_oceanSeeds = new Queueue<int>();
      this.m_layersTmp = new Lyst<TerrainMaterialThicknessSlim>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_config = config;
      this.m_resolver = resolver;
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      int? nullable1 = commandLineArgs.FirstIndexOf<string>((Predicate<string>) (x => x == "--terrain-gen-threads"));
      if (!nullable1.HasValue)
        return;
      int? nullable2 = nullable1;
      int? nullable3 = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() + 1) : new int?();
      int length = commandLineArgs.Length;
      int result;
      if (!(nullable3.GetValueOrDefault() < length & nullable3.HasValue) || !int.TryParse(commandLineArgs[nullable1.Value + 1], out result))
        return;
      this.m_config.MaxDegreeOfParallelism = (MaxDegreeOfParallelism) result.Clamp(0, 256);
      Log.Info(string.Format("Terrain generation max threads count set to: {0}", (object) (int) this.m_config.MaxDegreeOfParallelism));
    }

    public bool IsSerial()
    {
      return this.m_config.MaxDegreeOfParallelism == MaxDegreeOfParallelism.OneThread;
    }

    public void SetSerial()
    {
      this.m_config.MaxDegreeOfParallelism = MaxDegreeOfParallelism.OneThread;
    }

    public void SetPerfDataCollection(bool isEnabled)
    {
      this.m_perfData = isEnabled ? (Option<ConcurrentDictionary<ITerrainFeatureGenerator, long>>) new ConcurrentDictionary<ITerrainFeatureGenerator, long>() : Option<ConcurrentDictionary<ITerrainFeatureGenerator, long>>.None;
    }

    public void Clear()
    {
      this.m_oceanSeeds.Clear();
      this.m_oceanSeeds.TrimExcess();
      this.m_layersTmp.Clear();
      this.m_layersTmp.TrimExcess();
      this.m_chunkDataPool.Clear();
      this.m_perfData = Option<ConcurrentDictionary<ITerrainFeatureGenerator, long>>.None;
    }

    /// <summary>
    /// Cancels all background operations and waits for their termination.
    /// </summary>
    public void Cancel() => this.ensureAllBackgroundOperationsAreCancelled();

    private void ensureAllBackgroundOperationsAreCancelled()
    {
      CancellationTokenSource valueOrNull = this.m_cancellationTokenSource.ValueOrNull;
      try
      {
        valueOrNull?.Cancel();
        this.m_backgroundTerGenTask.ValueOrNull?.Wait();
        this.m_backgroundPostProcessTask.ValueOrNull?.Wait();
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (AggregateException ex)
      {
        if (!ex.ContainsOperationCanceledExceptionRecursively())
          Log.Exception((Exception) ex, "Exception was thrown when waiting on background operation completion.");
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Exception was thrown when waiting on background operation completion.");
      }
      valueOrNull?.Dispose();
      this.m_backgroundTerGenTask = Option<Task>.None;
      this.m_backgroundPostProcessTask = Option<Task>.None;
      this.m_cancellationTokenSource = Option<CancellationTokenSource>.None;
      this.m_runningCustomPostProcessor.ValueOrNull?.EnsureAllBackgroundOperationsAreCancelled();
    }

    public TerrainGenerationContext CreateContext(
      RelTile2i terrainSize,
      Tile2i dataOrigin,
      TerrainManager.TerrainData data,
      TerrainMaterialProto bedrockMaterial,
      int initialMapCreationSaveVersion)
    {
      return new TerrainGenerationContext(terrainSize, dataOrigin, data, bedrockMaterial, initialMapCreationSaveVersion, this.m_resolver);
    }

    public IEnumerator<Percent> GenerateTerrainTimeSliced(
      TerrainGenerationContext context,
      IIndexable<ITerrainFeatureGenerator> terrainFeatureGenerators,
      IIndexable<ITerrainPostProcessorV2> terrainPostProcessors,
      Func<Tile2i, bool> isOcean)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CGenerateTerrainTimeSliced\u003Ed__19(0)
      {
        \u003C\u003E4__this = this,
        context = context,
        terrainFeatureGenerators = terrainFeatureGenerators,
        terrainPostProcessors = terrainPostProcessors,
        isOcean = isOcean
      };
    }

    public IEnumerator<Percent> ApplyGeneratedData(
      TerrainGenerationContext context,
      Chunk64Area areaToApply,
      TerrainManager terrainManager,
      bool isInMapEditor)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CApplyGeneratedData\u003Ed__20(0)
      {
        \u003C\u003E4__this = this,
        context = context,
        areaToApply = areaToApply,
        terrainManager = terrainManager,
        isInMapEditor = isInMapEditor
      };
    }

    private void applyTerrainDataInPlace(
      TerrainGenerationContext context,
      Chunk64Area areaToApply,
      TerrainManager terrainManager)
    {
      Assert.That<RectangleTerrainArea2i>(areaToApply.Area2i).IsEqualTo<RectangleTerrainArea2i>(terrainManager.TerrainArea, "Regenerating partial area but using terrain manager's data. That won't work well.");
      ref TerrainManager.TerrainData local = ref terrainManager.GetMutableTerrainDataRef();
      Assert.That<RelTile2i>(local.Size).IsEqualTo<RelTile2i>(context.Data.Size);
      Assert.That<ushort[]>(local.Flags).IsEqualTo<ushort[]>(context.Data.Flags);
      uint savedFlagsMask = local.SavedFlagsMask;
      local = context.Data;
      local.ChangedTiles.ClearAllBits();
      local.SavedFlagsMask = savedFlagsMask;
    }

    private void copyTerrainChunk(
      Chunk2i chunk,
      TerrainGenerationContext sourceData,
      TerrainManager terrainManager)
    {
      ref TerrainManager.TerrainData local1 = ref terrainManager.GetMutableTerrainDataRef();
      int width1 = sourceData.Data.Width;
      int width2 = local1.Width;
      HeightTilesF[] heights1 = sourceData.Data.Heights;
      HeightTilesF[] heights2 = local1.Heights;
      TileSurfaceData[] surfaces1 = sourceData.Data.Surfaces;
      TileSurfaceData[] surfaces2 = local1.Surfaces;
      ushort[] flags1 = sourceData.Data.Flags;
      ushort[] flags2 = local1.Flags;
      TileMaterialLayers[] materialLayers1 = sourceData.Data.MaterialLayers;
      TileMaterialLayers[] materialLayers2 = local1.MaterialLayers;
      RelTile2i relTile2i = chunk.Tile2i - sourceData.DataOrigin;
      int num1 = relTile2i.X + width1 * relTile2i.Y;
      int num2 = terrainManager.GetTileIndex(chunk.Tile2i).Value;
      Lyst<TerrainMaterialThicknessSlim> layersTmp = this.m_layersTmp;
      int num3 = 0;
      while (num3 < 64)
      {
        for (int index1 = 0; index1 < 64; ++index1)
        {
          int index2 = num1 + index1;
          int index3 = num2 + index1;
          heights2[index3] = heights1[index2];
          surfaces2[index3] = surfaces1[index2];
          flags2[index3] = flags1[index2];
          layersTmp.Clear();
          sourceData.Data.GetLayersAt(materialLayers1[index2], layersTmp);
          ref TileMaterialLayers local2 = ref materialLayers2[index3];
          local1.ClearAllLayersOf(ref local2);
          local1.InitializeLayers(ref local2, layersTmp.BackingArrayAsSlice);
        }
        ++num3;
        num1 += width1;
        num2 += width2;
      }
    }

    private IEnumerator<Percent> computeOceanTimeSliced(
      TerrainGenerationContext dataWrapped,
      Chunk64Area chunkArea,
      Func<Tile2i, bool> isOcean)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CcomputeOceanTimeSliced\u003Ed__23(0)
      {
        \u003C\u003E4__this = this,
        dataWrapped = dataWrapped,
        chunkArea = chunkArea,
        isOcean = isOcean
      };
    }

    private static bool processCandidateOceanChunk(
      Chunk2i chunk,
      TerrainGenerationContext dataWrapped)
    {
      int tileIndex1 = dataWrapped.GetTileIndex(chunk.Tile2i);
      int width = dataWrapped.Data.Width;
      HeightTilesF[] heights = dataWrapped.Data.Heights;
      int num1 = 0;
      while (num1 < 64)
      {
        for (int index = 0; index < 64; ++index)
        {
          if (heights[tileIndex1 + index] >= OceanTerrainManager.OCEAN_THRESHOLD)
            return false;
        }
        ++num1;
        tileIndex1 += width;
      }
      int tileIndex2 = dataWrapped.GetTileIndex(chunk.Tile2i);
      ushort[] flags = dataWrapped.Data.Flags;
      int num2 = 0;
      while (num2 < 64)
      {
        for (int index = 0; index < 64; ++index)
          flags[tileIndex2 + index] |= (ushort) 4;
        ++num2;
        tileIndex2 += width;
      }
      return true;
    }

    /// <summary>Flood-fills ocean from the given tile.</summary>
    private IEnumerator<Percent> floodFillOceanTimeSliced(
      TerrainGenerationContext dataWrapped,
      Queueue<int> toProcess)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CfloodFillOceanTimeSliced\u003Ed__25(0)
      {
        dataWrapped = dataWrapped,
        toProcess = toProcess
      };
    }

    private static void processDisruptionUnderOcean(
      TerrainManager terrainManager,
      Chunk2i chunk,
      TerrainGenerationContext dataWrapped)
    {
      int tileIndex = dataWrapped.GetTileIndex(chunk.Tile2i);
      int width = dataWrapped.Data.Width;
      ref TerrainManager.TerrainData local1 = ref dataWrapped.Data;
      TileMaterialLayerOverflow[] backingArray = local1.MaterialLayersOverflow.GetBackingArray();
      ushort[] flags = dataWrapped.Data.Flags;
      int num = 0;
      while (num < 64)
      {
        for (int index1 = 0; index1 < 64; ++index1)
        {
          int index2 = tileIndex + index1;
          if (((int) flags[index2] & 4) != 0)
          {
            ref TileMaterialLayers local2 = ref local1.MaterialLayers[index2];
            if (local2.Count != 0)
            {
              processLayer(ref local2.First);
              if (local2.Count != 1)
              {
                processLayer(ref local2.Second);
                if (local2.Count != 2)
                {
                  processLayer(ref local2.Third);
                  if (local2.Count != 3)
                  {
                    processLayer(ref local2.Fourth);
                    if (local2.Count != 4)
                    {
                      int overflowIndex = local2.OverflowIndex;
                      for (int index3 = 4; index3 < local2.Count; ++index3)
                      {
                        ref TileMaterialLayerOverflow local3 = ref backingArray[overflowIndex];
                        processLayer(ref local3.Material);
                        overflowIndex = local3.OverflowIndex;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        ++num;
        tileIndex += width;
      }
      Assert.That<TileMaterialLayerOverflow[]>(local1.MaterialLayersOverflow.GetBackingArray()).IsEqualTo<TileMaterialLayerOverflow[]>(backingArray);

      void processLayer(ref TerrainMaterialThicknessSlim oldLayer)
      {
        TerrainMaterialProto full = oldLayer.SlimId.ToFull(terrainManager);
        if (!full.DisruptedMaterialProto.HasValue || !full.DisruptedMaterialProto.Value.RecoveredMaterialProto.HasValue || full.DisruptedMaterialProto.Value.RecoversUnderWater)
          return;
        TerrainMaterialProto terrainMaterialProto = full.DisruptedMaterialProto.Value;
        oldLayer = oldLayer.WithNewId(terrainMaterialProto.SlimId);
      }
    }

    private IEnumerator<Percent> disruptUnderOceanTimeSliced(
      TerrainGenerationContext dataWrapped,
      Chunk64Area chunkArea)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CdisruptUnderOceanTimeSliced\u003Ed__27(0)
      {
        \u003C\u003E4__this = this,
        dataWrapped = dataWrapped,
        chunkArea = chunkArea
      };
    }

    private static void removeBedrockLayers(
      Chunk2i chunk,
      TerrainGenerationContext dataWrapped,
      object overflowLock)
    {
      int tileIndex = dataWrapped.GetTileIndex(chunk.Tile2i);
      int width = dataWrapped.Data.Width;
      TerrainMaterialSlimId slimId = dataWrapped.BedrockMaterial.SlimId;
      ref TerrainManager.TerrainData local1 = ref dataWrapped.Data;
      TileMaterialLayerOverflow[] backingArray = local1.MaterialLayersOverflow.GetBackingArray();
      int num = 0;
      while (num < 64)
      {
        for (int index1 = 0; index1 < 64; ++index1)
        {
          ref TileMaterialLayers local2 = ref local1.MaterialLayers[tileIndex + index1];
          if (local2.Count > 0)
          {
            bool flag;
            do
            {
              flag = false;
              int count = local2.Count;
              if (count <= 4)
              {
                if (count == 1)
                {
                  if (local2.First.SlimId == slimId)
                  {
                    local2.First = new TerrainMaterialThicknessSlim();
                    local2.Count = 0;
                    break;
                  }
                }
                else if (count == 2)
                {
                  if (local2.Second.SlimId == slimId)
                  {
                    local2.Second = new TerrainMaterialThicknessSlim();
                    local2.Count = 1;
                    flag = true;
                  }
                }
                else if (count == 3)
                {
                  if (local2.Third.SlimId == slimId)
                  {
                    local2.Third = new TerrainMaterialThicknessSlim();
                    local2.Count = 2;
                    flag = true;
                  }
                }
                else if (local2.Fourth.SlimId == slimId)
                {
                  local2.Fourth = new TerrainMaterialThicknessSlim();
                  local2.Count = 3;
                  flag = true;
                }
              }
              else
              {
                int overflowIndex = local2.OverflowIndex;
                ref TileMaterialLayerOverflow local3 = ref backingArray[overflowIndex];
                if (count == 5)
                {
                  if (local3.Material.SlimId == slimId)
                  {
                    lock (overflowLock)
                      local1.RemoveFirstLayerFromOverflow_noChecks(ref local2);
                    --local2.Count;
                    flag = true;
                  }
                }
                else
                {
                  for (int index2 = 5; index2 < count; ++index2)
                  {
                    local3 = ref backingArray[overflowIndex];
                    overflowIndex = local3.OverflowIndex;
                  }
                  if (local3.Material.SlimId == slimId)
                  {
                    lock (overflowLock)
                      local1.RemoveMiddleLayerFromOverflow_noChecks(ref local3);
                    --local2.Count;
                    flag = true;
                  }
                }
              }
            }
            while (flag);
          }
        }
        ++num;
        tileIndex += width;
      }
      Assert.That<TileMaterialLayerOverflow[]>(local1.MaterialLayersOverflow.GetBackingArray()).IsEqualTo<TileMaterialLayerOverflow[]>(backingArray);
    }

    private static void compressLayers(
      Chunk2i chunk,
      TerrainGenerationContext dataWrapped,
      object overflowLock)
    {
      int tileIndex = dataWrapped.GetTileIndex(chunk.Tile2i);
      int width = dataWrapped.Data.Width;
      ref TerrainManager.TerrainData local1 = ref dataWrapped.Data;
      TileMaterialLayerOverflow[] backingArray = local1.MaterialLayersOverflow.GetBackingArray();
      int num = 0;
      while (num < 64)
      {
        for (int index1 = 0; index1 < 64; ++index1)
        {
          int index2 = tileIndex + index1;
          ref TileMaterialLayers local2 = ref local1.MaterialLayers[index2];
          if (local2.Count > 1)
          {
            if (local2.Count > 5)
            {
              int count = local2.Count;
              int overflowIndex = local2.OverflowIndex;
              for (int index3 = 5; index3 < count; ++index3)
              {
                ref TileMaterialLayerOverflow local3 = ref backingArray[overflowIndex];
                if (local3.Material.IsNone)
                {
                  Log.Warning("Encountered layer with phantom material.");
                }
                else
                {
                  TerrainMaterialSlimId slimId = local3.Material.SlimId;
                  ref TileMaterialLayerOverflow local4 = ref backingArray[local3.OverflowIndex];
                  if (local4.Material.SlimId == slimId)
                  {
                    local3.Material += local4.Material.Thickness;
                    lock (overflowLock)
                      local1.RemoveMiddleLayerFromOverflow_noChecks(ref local3);
                    --count;
                    --local2.Count;
                    --index3;
                  }
                  else
                    overflowIndex = local3.OverflowIndex;
                }
              }
            }
            if (local2.Count >= 5)
            {
              ref TileMaterialLayerOverflow local5 = ref backingArray[local2.OverflowIndex];
              if (local2.Fourth.SlimId == local5.Material.SlimId)
              {
                local2.Fourth += local5.Material.Thickness;
                lock (overflowLock)
                {
                  local1.RemoveFirstLayerFromOverflow_noChecks(ref local2);
                  --local2.Count;
                }
              }
            }
            if (local2.Count >= 4 && local2.Third.SlimId == local2.Fourth.SlimId)
            {
              local2.Third += local2.Fourth.Thickness;
              lock (overflowLock)
                local1.RemoveFourthLayer_noChecks(ref local2);
            }
            if (local2.Count >= 3 && local2.Second.SlimId == local2.Third.SlimId)
            {
              local2.Second += local2.Third.Thickness;
              lock (overflowLock)
                local1.RemoveThirdLayer_noChecks(ref local2);
            }
            if (local2.Count >= 2 && local2.First.SlimId == local2.Second.SlimId)
            {
              local2.First += local2.Second.Thickness;
              lock (overflowLock)
                local1.RemoveSecondLayer_noChecks(ref local2);
            }
          }
        }
        ++num;
        tileIndex += width;
      }
      Assert.That<TileMaterialLayerOverflow[]>(local1.MaterialLayersOverflow.GetBackingArray()).IsEqualTo<TileMaterialLayerOverflow[]>(backingArray);
    }

    private static void removeThinLayers(
      Chunk2i chunk,
      TerrainGenerationContext dataWrapped,
      object overflowLock)
    {
      int tileIndex = dataWrapped.GetTileIndex(chunk.Tile2i);
      int width = dataWrapped.Data.Width;
      ref TerrainManager.TerrainData local1 = ref dataWrapped.Data;
      ThicknessTilesF minLayerThickness = TerrainTile.MIN_LAYER_THICKNESS;
      TileMaterialLayerOverflow[] backingArray = local1.MaterialLayersOverflow.GetBackingArray();
      int num1 = 0;
      while (num1 < 64)
      {
        for (int index1 = 0; index1 < 64; ++index1)
        {
          int index2 = tileIndex + index1;
          ref TileMaterialLayers local2 = ref local1.MaterialLayers[index2];
          if (local2.Count > 0)
          {
            if (local2.Count >= 5)
            {
              int count = local2.Count;
              int index3 = local2.OverflowIndex;
              int index4 = -1;
              int num2 = 0;
              int num3 = 4;
              while (num3 < count)
              {
                ref TileMaterialLayerOverflow local3 = ref backingArray[index3];
                int overflowIndex = local3.OverflowIndex;
                if (!(local3.Material.Thickness >= minLayerThickness))
                {
                  lock (overflowLock)
                  {
                    if (num3 - num2 == 4)
                      local1.RemoveFirstLayerFromOverflow_noChecks(ref local2);
                    else
                      local1.RemoveMiddleLayerFromOverflow_noChecks(ref backingArray[index4]);
                  }
                  --local2.Count;
                  ++num2;
                }
                ++num3;
                index4 = index3;
                index3 = overflowIndex;
              }
            }
            if (local2.Count >= 4 && local2.Fourth.Thickness < minLayerThickness)
            {
              lock (overflowLock)
                local1.RemoveFourthLayer_noChecks(ref local2);
            }
            if (local2.Count >= 3 && local2.Third.Thickness < minLayerThickness)
            {
              lock (overflowLock)
                local1.RemoveThirdLayer_noChecks(ref local2);
            }
            if (local2.Count >= 2 && local2.Second.Thickness < minLayerThickness)
            {
              lock (overflowLock)
                local1.RemoveSecondLayer_noChecks(ref local2);
            }
            if (local2.First.Thickness < minLayerThickness)
            {
              lock (overflowLock)
                local1.RemoveFirstLayer_noChecks(ref local2);
            }
          }
        }
        ++num1;
        tileIndex += width;
      }
      Assert.That<TileMaterialLayerOverflow[]>(local1.MaterialLayersOverflow.GetBackingArray()).IsEqualTo<TileMaterialLayerOverflow[]>(backingArray);
    }

    private IEnumerator<Percent> tidyLayersTimeSliced(
      TerrainGenerationContext dataWrapped,
      Chunk64Area chunkArea)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CtidyLayersTimeSliced\u003Ed__31(0)
      {
        \u003C\u003E4__this = this,
        dataWrapped = dataWrapped,
        chunkArea = chunkArea
      };
    }

    public IEnumerator<Percent> GenerateTerrainFeaturesTimeSliced(
      IIndexable<ITerrainFeatureGenerator> generators,
      Chunk64Area areaChunks,
      TerrainGenerationContext context,
      int reportProgressFrequencyMs)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CGenerateTerrainFeaturesTimeSliced\u003Ed__32(0)
      {
        \u003C\u003E4__this = this,
        generators = generators,
        areaChunks = areaChunks,
        context = context,
        reportProgressFrequencyMs = reportProgressFrequencyMs
      };
    }

    /// <summary>
    /// Transfers data from chunk data structure to terrain data safely and efficiently.
    /// </summary>
    private static void writeChunkToTerrain(
      TerrainGeneratorChunkData chunkData,
      TerrainGenerationContext terrainData)
    {
      int width = terrainData.Data.Width;
      RelTile2i relTile2i = chunkData.Area.Origin - terrainData.DataOrigin;
      int num1 = relTile2i.X + width * relTile2i.Y;
      HeightTilesF[] heights1 = terrainData.Data.Heights;
      HeightTilesF[] heights2 = chunkData.Heights;
      TileSurfaceData[] surfaces1 = terrainData.Data.Surfaces;
      TileSurfaceData[] surfaces2 = chunkData.Surfaces;
      int num2 = 0;
      int num3 = 0;
      while (num2 < 64)
      {
        for (int index1 = 0; index1 < 64; ++index1)
        {
          int index2 = num3 + index1;
          int index3 = num1 + index1;
          heights1[index3] = heights2[index2];
          surfaces1[index3] = surfaces2[index2];
        }
        ++num2;
        num3 += 64;
        num1 += width;
      }
      lock (terrainData)
      {
        ref TerrainManager.TerrainData local = ref terrainData.Data;
        LystStruct<TerrainMaterialThicknessSlim>[] materialLayers = chunkData.MaterialLayers;
        int num4 = relTile2i.X + width * relTile2i.Y;
        int num5 = 0;
        int num6 = 0;
        while (num5 < 64)
        {
          for (int index4 = 0; index4 < 64; ++index4)
          {
            int index5 = num6 + index4;
            int index6 = num4 + index4;
            LystStruct<TerrainMaterialThicknessSlim> lystStruct = materialLayers[index5];
            lystStruct.Reverse();
            local.ClearAllLayersOf(ref local.MaterialLayers[index6]);
            local.InitializeLayers(ref local.MaterialLayers[index6], lystStruct.BackingArrayAsSlice);
          }
          ++num5;
          num6 += 64;
          num4 += width;
        }
      }
    }

    public IEnumerator<Percent> PostProcessTerrainTimeSliced(
      IIndexable<ITerrainPostProcessorV2> postProcessors,
      Chunk64Area areaChunks,
      TerrainGenerationContext context,
      int reportProgressFrequencyMs)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CPostProcessTerrainTimeSliced\u003Ed__35(0)
      {
        \u003C\u003E4__this = this,
        postProcessors = postProcessors,
        areaChunks = areaChunks,
        context = context,
        reportProgressFrequencyMs = reportProgressFrequencyMs
      };
    }

    private static ImmutableArray<TerrainGeneratorV2.PostProcessStage> computePostProcessStagesForArea(
      Chunk64Area areaChunks)
    {
      int self = (Environment.ProcessorCount * 2).CeilDiv(areaChunks.Size.X);
      if (self <= 0)
      {
        Log.Warning("Invalid rows per stage, processor count: " + Environment.ProcessorCount.ToString());
        self = 1;
      }
      Lyst<TerrainGeneratorV2.PostProcessStage> lyst = new Lyst<TerrainGeneratorV2.PostProcessStage>();
      Lyst<Chunk2i> chunks = new Lyst<Chunk2i>();
      int rowsCount1 = (self + 1).Min(areaChunks.Size.Y);
      int rowsCount2 = rowsCount1 == areaChunks.Size.Y ? rowsCount1 : self.Min(areaChunks.Size.Y);
      lyst.Add(new TerrainGeneratorV2.PostProcessStage(generateChunksForRows(0, rowsCount1), generateChunksForRows(0, rowsCount2)));
      int rowStart1 = rowsCount1;
      int rowStart2 = rowsCount2;
      while (rowStart1 < areaChunks.Size.Y)
      {
        int rowsCount3 = (areaChunks.Size.Y - rowStart1).Min(self);
        int num = areaChunks.Size.Y - rowStart2;
        if (rowStart1 + rowsCount3 < areaChunks.Size.Y)
          num = num.Min(self);
        lyst.Add(new TerrainGeneratorV2.PostProcessStage(generateChunksForRows(rowStart1, rowsCount3), generateChunksForRows(rowStart2, num)));
        rowStart1 += rowsCount3;
        rowStart2 += num;
      }
      return lyst.ToImmutableArray();

      Chunk2i[] generateChunksForRows(int rowStart, int rowsCount)
      {
        chunks.Clear();
        for (int index1 = 0; index1 < rowsCount; ++index1)
        {
          int y = areaChunks.Origin.Y + rowStart + index1;
          for (int index2 = 0; index2 < areaChunks.Size.X; ++index2)
            chunks.Add(new Chunk2i(areaChunks.Origin.X + index2, y));
        }
        return chunks.ToArray();
      }
    }

    private IEnumerator<Percent> postProcessTerrain(
      ITerrainPostProcessorV2 postProcessor,
      RectangleTerrainArea2i postProcessorBoundingBox,
      Chunk64Area areaChunks,
      TerrainGenerationContext terrainData,
      ImmutableArray<TerrainGeneratorV2.PostProcessStage> stages,
      ParallelOptions parallelOptions,
      int pass,
      int reportProgressFrequencyMs)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainGeneratorV2.\u003CpostProcessTerrain\u003Ed__37(0)
      {
        \u003C\u003E4__this = this,
        postProcessor = postProcessor,
        postProcessorBoundingBox = postProcessorBoundingBox,
        areaChunks = areaChunks,
        terrainData = terrainData,
        stages = stages,
        parallelOptions = parallelOptions,
        pass = pass,
        reportProgressFrequencyMs = reportProgressFrequencyMs
      };
    }

    private readonly struct PostProcessStage
    {
      public readonly Chunk2i[] ChunksToAnalyze;
      public readonly Chunk2i[] ChunksToApply;

      public PostProcessStage(Chunk2i[] chunksToAnalyze, Chunk2i[] chunksToApply)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ChunksToAnalyze = chunksToAnalyze;
        this.ChunksToApply = chunksToApply;
      }
    }
  }
}
