// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.ParticleErosionPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.UiState;
using Mafi.Core.Utils;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ParticleErosionPostProcessor : 
    CustomTerrainPostProcessorV2,
    IEditableTerrainFeature,
    ITerrainFeatureBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly RelTile1f MAX_PARTICLE_TRAVEL_DISTANCE;
    [DoNotSave(0, null)]
    private Fix32[] m_interactionRates;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<int> m_visitedTilesTmp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Stak<int> m_toExpandTmp;
    [DoNotSaveCreateNewOnLoad("new Lyst<Pair<int, HeightTilesF>>(canOmitClearing: true)", 0)]
    private readonly Lyst<Pair<int, HeightTilesF>> m_newHeightsTmp;
    public readonly ParticleErosionPostProcessor.Configuration ConfigMutable;
    [DoNotSave(0, null)]
    private Option<Task> m_backgroundTask;
    [DoNotSave(0, null)]
    private Option<CancellationTokenSource> m_cancellationTokenSource;

    public static void Serialize(ParticleErosionPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ParticleErosionPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ParticleErosionPostProcessor.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ParticleErosionPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
    }

    public static ParticleErosionPostProcessor Deserialize(BlobReader reader)
    {
      ParticleErosionPostProcessor erosionPostProcessor;
      if (reader.TryStartClassDeserialization<ParticleErosionPostProcessor>(out erosionPostProcessor))
        reader.EnqueueDataDeserialization((object) erosionPostProcessor, ParticleErosionPostProcessor.s_deserializeDataDelayedAction);
      return erosionPostProcessor;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ParticleErosionPostProcessor>(this, "ConfigMutable", (object) ParticleErosionPostProcessor.Configuration.Deserialize(reader));
      reader.SetField<ParticleErosionPostProcessor>(this, "m_newHeightsTmp", (object) new Lyst<Pair<int, HeightTilesF>>(true));
      reader.SetField<ParticleErosionPostProcessor>(this, "m_toExpandTmp", (object) new Stak<int>());
      reader.SetField<ParticleErosionPostProcessor>(this, "m_visitedTilesTmp", (object) new Set<int>());
    }

    public override string Name => "Particle erosion";

    public override int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 100;

    public override bool IsUnique => true;

    public override bool IsImportable => false;

    public bool Is2D => true;

    public bool CanRotate => false;

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors)
    {
      for (int index = 0; index < this.ConfigMutable.PassesConfigs.Count; ++index)
      {
        ParticleErosionPostProcessor.ParticleErosionConfig passesConfig = this.ConfigMutable.PassesConfigs[index];
        int maxInfluenceSteps = passesConfig.GetMaxInfluenceSteps();
        int maxParticleSteps = passesConfig.MaxParticleSteps;
        if (maxParticleSteps > maxInfluenceSteps)
        {
          errors.Add(string.Format("Particle erosion in pass {0} is configured with {1} steps but only {2} ", (object) (index + 1), (object) maxParticleSteps, (object) maxInfluenceSteps) + "can be performed to not affect more than one chunk around it. Lower max steps or step distance.");
          return false;
        }
        if (passesConfig.ExtendedGradWeight.IsNotPositive && passesConfig.ExtendedGradStep.IsPositive)
          errors.Add("When 'Extended Grad Weight' is zero, 'Extended Grad Step' must be also set to zero.");
      }
      return true;
    }

    public HandleData? GetHandleData() => new HandleData?();

    public override RectangleTerrainArea2i? GetBoundingBox() => new RectangleTerrainArea2i?();

    public override bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      return true;
    }

    public override void Reset() => this.EnsureAllBackgroundOperationsAreCancelled();

    public override void ClearCaches()
    {
    }

    public override void TranslateBy(RelTile3f delta)
    {
      this.ConfigMutable.TranslateRegionsBy(delta.Xy);
    }

    public override void RotateBy(AngleDegrees1f rotation)
    {
    }

    public override IEnumerator<Percent> ProcessCustomSchedule(
      Chunk64Area areaChunks,
      TerrainGenerationContext terrainData,
      ParallelOptions parallelOptions,
      int pass,
      int reportProgressFrequencyMs)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new ParticleErosionPostProcessor.\u003CProcessCustomSchedule\u003Ed__39(0)
      {
        \u003C\u003E4__this = this,
        areaChunks = areaChunks,
        terrainData = terrainData,
        parallelOptions = parallelOptions,
        reportProgressFrequencyMs = reportProgressFrequencyMs
      };
    }

    public override void EnsureAllBackgroundOperationsAreCancelled()
    {
      CancellationTokenSource valueOrNull = this.m_cancellationTokenSource.ValueOrNull;
      valueOrNull?.Cancel();
      try
      {
        if (valueOrNull != null)
          this.m_backgroundTask.ValueOrNull?.Wait(this.m_cancellationTokenSource.Value.Token);
        else
          this.m_backgroundTask.ValueOrNull?.Wait();
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Exception was thrown when waiting on background operation completion.");
      }
      if (valueOrNull != null)
        this.m_cancellationTokenSource = Option<CancellationTokenSource>.None;
      valueOrNull?.Dispose();
      this.m_backgroundTask = Option<Task>.None;
    }

    private void simulateErosionStageInParallel(
      ParticleErosionPostProcessor.Schedule[] schedule,
      ParticleErosionPostProcessor.ParticleErosionConfig config,
      TerrainGenerationContext terrainData,
      ParallelOptions parallelOptions)
    {
      bool[] finishedIndices = new bool[schedule.Length];
      int scheduleIndex = -1;
      int num1 = 1;
      if (parallelOptions.MaxDegreeOfParallelism == 1)
        num1 = 4;
      int maxDepsWaitIters = 5 * num1 * 1000 / 10;
      int completedChunks = 0;
      TerrainMaterialSlimId[] ignoredMaterials = ((IEnumerable<TerrainMaterialSlimId>) this.ConfigMutable.IgnoredMaterials.Select<TerrainMaterialSlimId>((Func<TerrainMaterialProto, TerrainMaterialSlimId>) (x => x.SlimId))).Distinct<TerrainMaterialSlimId, TerrainMaterialSlimId>((Func<TerrainMaterialSlimId, TerrainMaterialSlimId>) (x => x)).ToArray<TerrainMaterialSlimId>();
      Exception exception = (Exception) null;
      Parallel.For(0, schedule.Length, parallelOptions, (Action<int>) (_ =>
      {
        try
        {
          ParticleErosionPostProcessor.Schedule schedule1 = schedule[Interlocked.Increment(ref scheduleIndex)];
          bool flag = false;
          int num2 = completedChunks;
          for (int index = 0; index < maxDepsWaitIters; ++index)
          {
            if (exception != null)
              return;
            flag = true;
            foreach (int mustBeProcessedId in schedule1.MustBeProcessedIds)
            {
              if (!finishedIndices[mustBeProcessedId])
              {
                flag = false;
                break;
              }
            }
            if (!flag)
            {
              Thread.Sleep(10);
              if (num2 != completedChunks)
              {
                num2 = completedChunks;
                index = 0;
              }
            }
            else
              break;
          }
          if (!flag)
          {
            Log.Error("Failed to simulate erosion on chunk, deps not processed in time.");
            exception = new Exception("Timeout waiting on deps.");
          }
          else
          {
            this.simulateErosionOnChunk(schedule1.Chunk, config, terrainData.DataOrigin, ref terrainData.Data, ignoredMaterials);
            finishedIndices[schedule1.Index] = true;
            Interlocked.Increment(ref completedChunks);
          }
        }
        catch (OperationCanceledException ex)
        {
          exception = (Exception) ex;
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception was thrown during erosion simulation.");
          exception = ex;
        }
      }));
      if (exception != null)
      {
        if (!(exception is OperationCanceledException))
          throw new FatalGameException("Failed to simulate erosion", exception);
      }
      else
        Assert.That<int>(completedChunks).IsEqualTo(schedule.Length);
    }

    private void simulateErosionOnChunk(
      Chunk2i chunk,
      ParticleErosionPostProcessor.ParticleErosionConfig config,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      TerrainMaterialSlimId[] ignoredMaterials)
    {
      int intFloored = config.ParticlesPerTile.MultByUnchecked(4096).ToIntFloored();
      IRandom random = (IRandom) new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, (ulong) chunk.GetHashCode(), config.Seed);
      random.Jump();
      LystStruct<Polygon2fFast> lystStruct = new LystStruct<Polygon2fFast>();
      foreach (SuppressErosionRegion suppressErosionRegion in this.ConfigMutable.SuppressErosionRegions)
      {
        Vector2f min;
        Vector2f max;
        suppressErosionRegion.Polygon.ComputeBounds(out min, out max);
        RectangleTerrainArea2i rectangleTerrainArea2i = chunk.Area;
        rectangleTerrainArea2i = rectangleTerrainArea2i.Intersect(new RectangleTerrainArea2i(new Tile2i(min.FlooredVector2i), new RelTile2i((max - min).CeiledVector2i)));
        Polygon2fFast polygon;
        if (rectangleTerrainArea2i.AreaTiles > 0 && suppressErosionRegion.Polygon.TryGetFastPolygon(out polygon, out string _))
          lystStruct.Add(polygon);
      }
      for (int index = 0; index < intFloored; ++index)
      {
        Vector2f vector2f = random.NextTileInChunk2f(chunk).Vector2f;
        if (lystStruct.IsNotEmpty)
        {
          bool flag = false;
          foreach (Polygon2fFast polygon2fFast in lystStruct)
          {
            if (polygon2fFast.Contains(vector2f))
            {
              flag = true;
              break;
            }
          }
          if (flag)
            continue;
        }
        Tile2f pos = new Tile2f(vector2f.X - (Fix32) dataOrigin.X, vector2f.Y - (Fix32) dataOrigin.Y);
        int tileIndex = dataRef.GetTileIndex(pos.Tile2i);
        if (ignoredMaterials.Length != 0)
        {
          TileMaterialLayers materialLayer = dataRef.MaterialLayers[tileIndex];
          bool flag = false;
          foreach (TerrainMaterialSlimId ignoredMaterial in ignoredMaterials)
          {
            if (materialLayer.First.SlimId == ignoredMaterial)
            {
              flag = true;
              break;
            }
            if (materialLayer.First.Thickness < ThicknessTilesF.One && materialLayer.Second.SlimId == ignoredMaterial)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            continue;
        }
        ParticleErosionPostProcessor.simulateParticle(pos, new Tile2iIndex(tileIndex), ref dataRef, this.m_interactionRates, config);
      }
    }

    private void simulateErosionFullyRandomized(
      TerrainManager terrain,
      ParticleErosionPostProcessor.Configuration masterConfig)
    {
      IRandom random = (IRandom) new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 0UL, 1UL);
      Tile2f maxValueExcl = new Tile2f((Fix32) terrain.TerrainWidth, (Fix32) terrain.TerrainHeight);
      foreach (ParticleErosionPostProcessor.ParticleErosionConfig passesConfig in masterConfig.PassesConfigs)
      {
        this.recomputeInteractionRate(passesConfig);
        int intFloored = passesConfig.ParticlesPerTile.MultByUnchecked(terrain.TerrainArea.AreaTiles).ToIntFloored();
        random.SeedFast((ulong) terrain.TerrainSize.GetHashCode(), passesConfig.Seed);
        for (int index = 0; index < intFloored; ++index)
        {
          Tile2f pos = random.NextTile2f(Tile2f.Zero, maxValueExcl);
          Tile2iIndex tileIndex = terrain.GetTileIndex(pos.Tile2i);
          ParticleErosionPostProcessor.simulateParticle(pos, tileIndex, ref terrain.GetMutableTerrainDataRef(), this.m_interactionRates, passesConfig);
        }
      }
    }

    public void SimulateParticleTraced(
      Tile2f startPosition,
      ParticleErosionPostProcessor.ParticleErosionConfig config,
      TerrainManager terrainManager)
    {
      this.recomputeInteractionRate(config);
      ParticleErosionPostProcessor.simulateParticle(startPosition, terrainManager.GetTileIndex(startPosition.Tile2i), ref terrainManager.GetMutableTerrainDataRef(), this.m_interactionRates, config);
    }

    private void recomputeInteractionRate(
      ParticleErosionPostProcessor.ParticleErosionConfig config)
    {
      int maxInfluenceSteps = config.GetMaxInfluenceSteps();
      int length = config.MaxParticleSteps;
      if (length > maxInfluenceSteps)
      {
        Log.Warning(string.Format("Particle erosion is configured with {0} but only {1} can be performed no not affect ", (object) length, (object) maxInfluenceSteps) + "more than one chunk, clamping.");
        length = maxInfluenceSteps;
      }
      this.m_interactionRates = new Fix32[length];
      for (int index1 = 0; index1 < length; ++index1)
      {
        Fix32[] interactionRates = this.m_interactionRates;
        int index2 = index1;
        Fix32 fix32_1 = Fix32.Tau.HalfFast * (index1 + 1).ToFix32();
        fix32_1 = fix32_1.DivByPositiveUncheckedUnrounded((Fix32) (length + 1));
        fix32_1 = fix32_1.Sin();
        fix32_1 = fix32_1.Pow(config.TerrainInteractionLifetimeExponent);
        Fix32 fix32_2 = fix32_1.MultByUnchecked(config.TerrainInteractionMult);
        interactionRates[index2] = fix32_2;
      }
    }

    private static void simulateParticle(
      Tile2f pos,
      Tile2iIndex tileIndex,
      ref TerrainManager.TerrainData dataRef,
      Fix32[] interactionRates,
      ParticleErosionPostProcessor.ParticleErosionConfig config)
    {
      ushort[] flags = dataRef.Flags;
      if (((int) flags[tileIndex.Value] & 1) != 0)
        return;
      HeightTilesF[] heights = dataRef.Heights;
      if (heights[tileIndex.Value] < config.LowestProcessedHeightUnderOcean && ((int) flags[tileIndex.Value] & 4) != 0)
        return;
      int num1 = config.ExtendedGradStep.Value;
      int num2 = dataRef.Width - config.ExtendedGradStep.Value;
      int num3 = num1;
      int num4 = dataRef.Height - config.ExtendedGradStep.Value;
      if (pos.X < num1 || pos.X >= num2 || pos.Y < num3 || pos.Y >= num4)
        return;
      Tile2iIndex tileIndex1 = tileIndex;
      Chunk2i maxValue = Chunk2i.MaxValue;
      int terrainStride = dataRef.Width;
      int relX1Y1 = terrainStride + 1;
      int extIndexDeltaX = config.ExtendedGradStep.Value;
      int extIndexDeltaY = config.ExtendedGradStep.Value * terrainStride;
      Fix32 gradientToVelocityMult = -config.GradientToVelocityMult;
      Vector2f gradientAtPoint = getGradientAtPoint(pos, tileIndex1);
      if (gradientAtPoint.IsNearZero())
        return;
      Vector2f v = Vector2f.Zero;
      ThicknessTilesF zero = ThicknessTilesF.Zero;
      HeightTilesF heightTilesF = dataRef.GetHeightInterpolated(pos);
      Tile2f pos1 = pos;
      Tile2iIndex tileIndex2 = tileIndex1;
      Fix32 fix32_1 = Fix32.Zero;
      Fix32 fix32_2 = Fix32.One - config.MomentumFactor;
      Fix32 fix32_3 = Fix32.One - config.Friction;
      foreach (Fix32 interactionRate in interactionRates)
      {
        gradientAtPoint = getGradientAtPoint(pos, tileIndex1);
        Vector2f velocity = computeVelocity(gradientAtPoint);
        v = v.MultByUnchecked(config.MomentumFactor) + velocity.MultByUnchecked(fix32_2);
        v = v.MultByUnchecked(fix32_3);
        Fix32 rhs = v.Length;
        if (rhs - fix32_1 > config.MaxSpeedDelta)
        {
          Fix32 maxSpeedDelta = config.MaxSpeedDelta;
          rhs = fix32_1 + maxSpeedDelta;
        }
        if (rhs < config.MinSpeed && (rhs <= Fix32.EpsilonNear || gradientAtPoint.X < config.MinGradient && gradientAtPoint.Y < config.MinGradient))
          break;
        pos += new RelTile2f(v.MultByUnchecked(config.StepLength.Value.DivByPositiveUncheckedUnrounded(rhs)));
        if (pos.X < num1 || pos.X >= num2 || pos.Y < num3 || pos.Y >= num4)
          break;
        tileIndex1 = new Tile2iIndex(dataRef.GetTileIndex(pos.Tile2i));
        if (((int) flags[tileIndex1.Value] & 1) != 0)
          break;
        HeightTilesF heightInterpolated = dataRef.GetHeightInterpolated(pos);
        if (heightInterpolated > heightTilesF && zero.IsPositive)
        {
          ThicknessTilesF halfFast = zero.HalfFast;
          updateHeight(pos1, tileIndex2, halfFast);
          zero -= halfFast;
          rhs = rhs.HalfFast;
          v = v.HalfFast;
        }
        else
        {
          ThicknessTilesF deltaHeight = new ThicknessTilesF(config.ErodeSpeedThreshold - rhs + zero.Value.MultByUnchecked(config.AccumulatedMaterialErodeThresholdMult)).MultByUnchecked(interactionRate);
          if (deltaHeight.IsPositive)
          {
            if (zero.IsPositive)
            {
              if (deltaHeight > zero)
                deltaHeight = zero;
              updateHeight(pos1, tileIndex2, deltaHeight);
              zero -= deltaHeight;
            }
          }
          else
          {
            if (-deltaHeight.Value > config.MaxErosionPerStep)
              deltaHeight = new ThicknessTilesF(-config.MaxErosionPerStep);
            updateHeight(pos1, tileIndex2, deltaHeight);
            zero -= deltaHeight;
          }
        }
        if (rhs < config.SpeedSmoothingThreshold)
          gaussianBlur3x3(tileIndex2.Value);
        pos1 = pos;
        tileIndex2 = tileIndex1;
        heightTilesF = heightInterpolated;
        fix32_1 = rhs;
      }

      Vector2f getExtendedGradientAtPoint(int i)
      {
        return new Vector2f((heights[i + extIndexDeltaX] - heights[i - extIndexDeltaX]).Value, (heights[i + extIndexDeltaY] - heights[i - extIndexDeltaY]).Value).MultByUnchecked(config.ExtendedGradWeight);
      }

      Vector2f getGradientAtPoint(Tile2f pos, Tile2iIndex tileIndex)
      {
        HeightTilesF height1 = heights[tileIndex.Value];
        HeightTilesF height2 = heights[tileIndex.Value + 1];
        HeightTilesF height3 = heights[tileIndex.Value + terrainStride];
        HeightTilesF height4 = heights[tileIndex.Value + relX1Y1];
        Vector2f gradientAtPoint;
        ref Vector2f local = ref gradientAtPoint;
        ThicknessTilesF thicknessTilesF = height2 - height1;
        Fix32 x = thicknessTilesF.Value.Lerp((height4 - height3).Value, pos.Y.FractionalPartNonNegative);
        thicknessTilesF = height3 - height1;
        Fix32 y = thicknessTilesF.Value.Lerp((height4 - height2).Value, pos.X.FractionalPartNonNegative);
        local = new Vector2f(x, y);
        if (config.ExtendedGradStep.IsPositive && ((int) flags[tileIndex.Value] & 2) == 0)
          gradientAtPoint += getExtendedGradientAtPoint(tileIndex.Value);
        return gradientAtPoint;
      }

      Vector2f computeVelocity(Vector2f gradient)
      {
        return gradient.MultByUnchecked(gradientToVelocityMult);
      }

      void gaussianBlur3x3(int i)
      {
        int index1 = i - terrainStride;
        int index2 = i + terrainStride;
        Fix32 fix32 = heights[i].Value.Times4Fast + (heights[index1].Value + heights[i - 1].Value + heights[i + 1].Value + heights[index2].Value).Times2Fast + heights[index1 - 1].Value + heights[index1 + 1].Value + heights[index2 - 1].Value + heights[index2 + 1].Value;
        heights[i] = new HeightTilesF((fix32 + Fix32.FromRaw(64)).DivBy16Fast);
      }

      void updateHeight(Tile2f pos, Tile2iIndex tileIndex, ThicknessTilesF deltaHeight)
      {
        Fix32 fractionalPartNonNegative1 = pos.X.FractionalPartNonNegative;
        Fix32 fractionalPartNonNegative2 = pos.Y.FractionalPartNonNegative;
        Fix32 fix32_1 = (Fix32.One - fractionalPartNonNegative1).MultByUnchecked(Fix32.One - fractionalPartNonNegative2);
        Fix32 fix32_2 = fractionalPartNonNegative1.MultByUnchecked(Fix32.One - fractionalPartNonNegative2);
        Fix32 fix32_3 = fractionalPartNonNegative1.MultByUnchecked(fractionalPartNonNegative2);
        Fix32 fix32_4 = (Fix32.One - fractionalPartNonNegative1).MultByUnchecked(fractionalPartNonNegative2);
        heights[tileIndex.Value] += deltaHeight.MultByUnchecked(fix32_1);
        heights[tileIndex.Value + 1] += deltaHeight.MultByUnchecked(fix32_2);
        heights[tileIndex.Value + terrainStride] += deltaHeight.MultByUnchecked(fix32_4);
        heights[tileIndex.Value + relX1Y1] += deltaHeight.MultByUnchecked(fix32_3);
      }
    }

    public bool TryFillHoleAt(Tile2i tile, int maxHoleRadius, TerrainManager terrain)
    {
      HeightTilesF heightTilesF = HeightTilesF.MaxValue;
      for (int radius = 1; radius <= maxHoleRadius && heightTilesF == HeightTilesF.MaxValue; ++radius)
      {
        foreach (RelTile2i all8Neighbor in RelTile2i.All8Neighbors)
          heightTilesF = heightTilesF.Min(tryFindHoleBoundaryTile(tile, radius, all8Neighbor));
      }
      if (heightTilesF == HeightTilesF.MaxValue)
        return false;
      int num1 = (2 * maxHoleRadius).Squared();
      Tile2iIndex tileIndex = terrain.GetTileIndex(tile);
      Stak<int> toExpandTmp = this.m_toExpandTmp;
      toExpandTmp.Clear();
      toExpandTmp.Push(tileIndex.Value);
      Set<int> visitedTilesTmp = this.m_visitedTilesTmp;
      visitedTilesTmp.Clear();
      visitedTilesTmp.Add(tileIndex.Value);
      HeightTilesF[] heights = terrain.GetMutableTerrainDataRef().Heights;
      while (toExpandTmp.IsNotEmpty)
      {
        int num2 = toExpandTmp.Pop();
        foreach (Tile2iAndIndexRel eightNeighborsDelta in terrain.EightNeighborsDeltas)
        {
          int index = num2 + eightNeighborsDelta.IndexDelta;
          if (heights[index] <= heightTilesF && visitedTilesTmp.Add(index))
          {
            toExpandTmp.Push(index);
            if (toExpandTmp.Count > num1)
              return false;
          }
        }
      }
      foreach (int index in visitedTilesTmp)
        heights[index] = heightTilesF;
      Lyst<Pair<int, HeightTilesF>> newHeightsTmp = this.m_newHeightsTmp;
      newHeightsTmp.Clear();
      newHeightsTmp.EnsureCapacity(visitedTilesTmp.Count);
      int terrainWidth = terrain.TerrainWidth;
      foreach (int num3 in visitedTilesTmp)
        newHeightsTmp.Add(Pair.Create<int, HeightTilesF>(num3, ParticleErosionPostProcessor.gaussianBlur5x5(num3, heights, terrainWidth)));
      foreach (Pair<int, HeightTilesF> pair in newHeightsTmp)
        heights[pair.First] = pair.Second;
      return true;

      HeightTilesF tryFindHoleBoundaryTile(Tile2i t, int radius, RelTile2i direction)
      {
        HeightTilesF height = terrain.GetHeight(t + radius * direction);
        return !(height > terrain.GetHeight(t + (radius + 1) * direction)) ? HeightTilesF.MaxValue : height;
      }
    }

    private void fillAllHoles(TerrainManager terrain, int maxHoleRadius)
    {
      HeightTilesF[] heights = terrain.GetMutableTerrainDataRef().Heights;
      int terrainWidth = terrain.TerrainWidth;
      int y = 1;
      int num1 = terrain.TerrainHeight - 1;
      int num2 = terrainWidth;
      while (y < num1)
      {
        int x = 1;
        for (int index1 = terrain.TerrainWidth - 1; x < index1; ++x)
        {
          if (!terrain.IsOcean(new Tile2iIndex(num2 + x)))
          {
            int index2 = num2 + x;
            HeightTilesF heightTilesF = heights[index2];
            if (heightTilesF < heights[index2 - 1] && heightTilesF < heights[index2 + 1] && heightTilesF < heights[index2 - terrainWidth] && heightTilesF < heights[index2 + terrainWidth])
              this.TryFillHoleAt(new Tile2i(x, y), maxHoleRadius, terrain);
          }
        }
        ++y;
        num2 += terrainWidth;
      }
    }

    private static HeightTilesF gaussianBlur5x5(int i, HeightTilesF[] heights, int stride)
    {
      int index1 = i - stride - stride;
      Fix32 fix32_1 = heights[index1 - 1].Value + heights[index1].Value.Times2Fast + heights[index1 + 1].Value;
      int index2 = i - stride;
      Fix32 fix32_2 = fix32_1 + (heights[index2 - 2].Value + heights[index2 - 1].Value.Times4Fast + heights[index2].Value.Times5Fast + heights[index2 + 1].Value.Times4Fast + heights[index2 + 2].Value) + (heights[i - 2].Value.Times2Fast + heights[i - 1].Value.Times5Fast + heights[i].Value.Times12Fast + heights[i + 1].Value.Times5Fast + heights[i + 2].Value.Times2Fast);
      int index3 = i + stride;
      Fix32 fix32_3 = fix32_2 + (heights[index3 - 2].Value + heights[index3 - 1].Value.Times4Fast + heights[index3].Value.Times5Fast + heights[index3 + 1].Value.Times4Fast + heights[index3 + 2].Value);
      int index4 = i + stride + stride;
      return new HeightTilesF((fix32_3 + (heights[index4 - 1].Value + heights[index4].Value.Times2Fast + heights[index4 + 1].Value) + Fix32.FromRaw(16)).DivBy64Fast);
    }

    private static ParticleErosionPostProcessor.Schedule[] computeProcessingSchedule(
      Chunk64Area area)
    {
      ParticleErosionPostProcessor.Schedule[] processingSchedule = new ParticleErosionPostProcessor.Schedule[area.Size.ProductInt];
      int[] processed = new int[area.Size.ProductInt];
      int stride = area.Size.X;
      for (int index = 0; index < processed.Length; ++index)
        processed[index] = -1;
      Lyst<int> depsTmp = new Lyst<int>(true);
      int index1 = 0;
      for (int index2 = 0; index2 < 3; ++index2)
      {
        for (int index3 = 0; index3 < 3; ++index3)
        {
          for (int index4 = index2; index4 < area.Size.Y; index4 += 3)
          {
            for (int index5 = index3; index5 < area.Size.X; index5 += 3)
            {
              int index6 = index5 + index4 * stride;
              ImmutableArray<int> mustBeProcessedIds = collectDepsAt(index5, index4);
              processingSchedule[index1] = new ParticleErosionPostProcessor.Schedule(index6, area.Origin.AddX(index5).AddY(index4), mustBeProcessedIds);
              ++index1;
              processed[index6] = index6;
            }
          }
        }
      }
      return processingSchedule;

      ImmutableArray<int> collectDepsAt(int baseX, int baseY)
      {
        depsTmp.Clear();
        for (int index1 = -2; index1 <= 2; ++index1)
        {
          int num1 = baseY + index1;
          if (num1 >= 0)
          {
            if (num1 < area.Size.Y)
            {
              for (int index2 = -2; index2 <= 2; ++index2)
              {
                int num2 = baseX + index2;
                if (num2 >= 0)
                {
                  if (num2 < area.Size.X)
                  {
                    int index3 = num2 + num1 * stride;
                    if (processed[index3] >= 0)
                      depsTmp.Add(processed[index3]);
                  }
                  else
                    break;
                }
              }
            }
            else
              break;
          }
        }
        return depsTmp.ToImmutableArray();
      }
    }

    public ParticleErosionPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_visitedTilesTmp = new Set<int>();
      this.m_toExpandTmp = new Stak<int>();
      this.m_newHeightsTmp = new Lyst<Pair<int, HeightTilesF>>(true);
      this.ConfigMutable = new ParticleErosionPostProcessor.Configuration();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ParticleErosionPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ParticleErosionPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CustomTerrainPostProcessorV2) obj).SerializeData(writer));
      ParticleErosionPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CustomTerrainPostProcessorV2) obj).DeserializeData(reader));
      ParticleErosionPostProcessor.MAX_PARTICLE_TRAVEL_DISTANCE = TerrainChunk.Size.RelTile1f - 2.0.Tiles();
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : IConfig, ITerrainFeatureConfigWithInit, ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [NewInSaveVersion(147, null, "new()", null, null)]
      [EditorCollection(true, true)]
      [EditorEnforceOrder(67)]
      public readonly Lyst<SuppressErosionRegion> SuppressErosionRegions;
      [DoNotSave(0, null)]
      [EditorSection("Ignored materials", null, false, true)]
      [EditorEnforceOrder(73)]
      public Action AddIgnoredMaterial;
      [EditorEnforceOrder(78)]
      [NewInSaveVersion(151, null, "new()", null, null)]
      [EditorCollection(false, true)]
      public readonly Lyst<TerrainMaterialProto> IgnoredMaterials;
      [EditorCollection(true, true)]
      [EditorEnforceOrder(88)]
      public readonly Lyst<ParticleErosionPostProcessor.ParticleErosionConfig> PassesConfigs;
      [DoNotSave(0, null)]
      [EditorIgnore]
      public bool ForceSerialProcessingUpdateAfterEachParticle;

      public static void Serialize(
        ParticleErosionPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ParticleErosionPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ParticleErosionPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Lyst<TerrainMaterialProto>.Serialize(this.IgnoredMaterials, writer);
        Lyst<ParticleErosionPostProcessor.ParticleErosionConfig>.Serialize(this.PassesConfigs, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Lyst<SuppressErosionRegion>.Serialize(this.SuppressErosionRegions, writer);
      }

      public static ParticleErosionPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        ParticleErosionPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<ParticleErosionPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, ParticleErosionPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<ParticleErosionPostProcessor.Configuration>(this, "IgnoredMaterials", reader.LoadedSaveVersion >= 151 ? (object) Lyst<TerrainMaterialProto>.Deserialize(reader) : (object) new Lyst<TerrainMaterialProto>());
        reader.SetField<ParticleErosionPostProcessor.Configuration>(this, "PassesConfigs", (object) Lyst<ParticleErosionPostProcessor.ParticleErosionConfig>.Deserialize(reader));
        this.SortingPriorityAdjustment = reader.ReadInt();
        reader.SetField<ParticleErosionPostProcessor.Configuration>(this, "SuppressErosionRegions", reader.LoadedSaveVersion >= 147 ? (object) Lyst<SuppressErosionRegion>.Deserialize(reader) : (object) new Lyst<SuppressErosionRegion>());
      }

      [EditorEnforceOrder(62)]
      [EditorSection("Suppression regions", null, false, true)]
      [DoNotSave(0, null)]
      public Action AddSuppressionRegion { get; private set; }

      [DoNotSave(0, null)]
      [EditorSection("Erosion passes config", null, false, true)]
      [EditorEnforceOrder(83)]
      public Action AddNewPass { get; private set; }

      [EditorEnforceOrder(92)]
      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.SuppressErosionRegions = new Lyst<SuppressErosionRegion>();
        this.IgnoredMaterials = new Lyst<TerrainMaterialProto>();
        this.PassesConfigs = new Lyst<ParticleErosionPostProcessor.ParticleErosionConfig>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PassesConfigs.Add(new ParticleErosionPostProcessor.ParticleErosionConfig()
        {
          Name = "Low-momentum",
          TerrainInteractionMult = 1.5.ToFix32(),
          StepLength = 0.55.Tiles(),
          MinSpeed = 0.05.ToFix32(),
          MomentumFactor = 0.2.ToFix32(),
          GradientToVelocityMult = 0.5.ToFix32(),
          AccumulatedMaterialErodeThresholdMult = 0.5.ToFix32(),
          ExtendedGradStep = RelTile1i.Zero,
          ExtendedGradWeight = Fix32.Zero
        });
        this.PassesConfigs.Add(new ParticleErosionPostProcessor.ParticleErosionConfig());
      }

      public void InitializeInMapEditor(IResolver resolver)
      {
        this.AddNewPass = (Action) (() => this.PassesConfigs.Add(new ParticleErosionPostProcessor.ParticleErosionConfig()));
        this.AddSuppressionRegion = (Action) (() =>
        {
          Vector2f vector2f = resolver.Resolve<UiCameraState>().PivotPosition.Vector2f;
          SuppressErosionRegion suppressErosionRegion = new SuppressErosionRegion()
          {
            Polygon = new Polygon2fMutable()
          };
          suppressErosionRegion.Polygon.Initialize((IEnumerable<Vector2f>) new Vector2f[4]
          {
            vector2f + new Vector2f((Fix32) -50, (Fix32) -50),
            vector2f + new Vector2f((Fix32) 50, (Fix32) -50),
            vector2f + new Vector2f((Fix32) 50, (Fix32) 50),
            vector2f + new Vector2f((Fix32) -50, (Fix32) 50)
          });
          this.SuppressErosionRegions.Add(suppressErosionRegion);
        });
        ProtosDb protosDb = resolver.Resolve<ProtosDb>();
        this.AddIgnoredMaterial = (Action) (() => this.IgnoredMaterials.Add(protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Sand)));
      }

      public void TranslateRegionsBy(RelTile2f delta)
      {
        foreach (SuppressErosionRegion suppressErosionRegion in this.SuppressErosionRegions)
          suppressErosionRegion.Polygon.TranslateBy(delta.Vector2f);
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        ParticleErosionPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ParticleErosionPostProcessor.Configuration) obj).SerializeData(writer));
        ParticleErosionPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ParticleErosionPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public class ParticleErosionConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      /// <summary>
      /// Terrain height under the ocean that is processed for erosion.
      /// </summary>
      public HeightTilesF LowestProcessedHeightUnderOcean;
      /// <summary>Random seed that decides particles spawn positions.</summary>
      public ulong Seed;
      /// <summary>
      /// How many particles per tile to simulate using this config.
      /// </summary>
      [EditorRange(0.1, 2.0)]
      public Fix32 ParticlesPerTile;
      /// <summary>
      /// Minimum particle lifetime, to give it some time to gain some speed.
      /// </summary>
      [EditorRange(0.0, 10.0)]
      public int MinParticleSteps;
      /// <summary>Max particle lifetime.</summary>
      [EditorRange(20.0, 100.0)]
      public int MaxParticleSteps;
      /// <summary>
      /// Step length for the particle. Turns out that a constant step makes things
      /// </summary>
      [EditorRange(0.2, 2.0)]
      public RelTile1f StepLength;
      /// <summary>Multiplier for terrain interaction.</summary>
      [EditorRange(0.2, 4.0)]
      public Fix32 TerrainInteractionMult;
      /// <summary>
      /// Exponent applied to a sine function to reduce particle effect at start and end of its lifetime.
      /// </summary>
      [EditorRange(0.1, 1.0)]
      public Fix32 TerrainInteractionLifetimeExponent;
      /// <summary>
      /// Speed threshold at which particle will start to erode material.
      /// </summary>
      [EditorRange(0.1, 1.0)]
      public Fix32 ErodeSpeedThreshold;
      /// <summary>
      /// The erode threshold is increased by the amount of material that a particle carries, multiplied by this value.
      /// </summary>
      [EditorRange(0.1, 2.0)]
      public Fix32 AccumulatedMaterialErodeThresholdMult;
      /// <summary>
      /// Particle simulation will be stopped if its speed goes below this value.
      /// </summary>
      [EditorRange(0.025, 0.5)]
      public Fix32 MinSpeed;
      /// <summary>Minimum gradient for early particle termination.</summary>
      [EditorRange(0.05, 0.5)]
      public Fix32 MinGradient;
      /// <summary>
      /// How much of the previous speed is preserved each step.
      /// </summary>
      [EditorRange(0.0, 1.0)]
      public Fix32 MomentumFactor;
      /// <summary>
      /// Multiplier that converts gradient to velocity. Note that velocity is only affecting erosion, not step size.
      /// </summary>
      [EditorRange(0.0, 1.0)]
      public Fix32 GradientToVelocityMult;
      /// <summary>Distance at which are extended gradients taken.</summary>
      [EditorRange(0.0, 5.0)]
      public RelTile1i ExtendedGradStep;
      /// <summary>
      /// Weight of the extended gradients. The normal gradient has weight 1.
      /// </summary>
      [EditorRange(0.0, 4.0)]
      public Fix32 ExtendedGradWeight;
      /// <summary>Percentage of speed removed at each step.</summary>
      [EditorRange(0.0, 0.25)]
      public Fix32 Friction;
      /// <summary>Maximum terrain thickness eroded per step.</summary>
      [EditorRange(0.2, 2.0)]
      public Fix32 MaxErosionPerStep;
      /// <summary>Limits a maximum speed difference per step.</summary>
      [EditorRange(0.05, 1.0)]
      public Fix32 MaxSpeedDelta;
      /// <summary>
      /// At which speed a gaussian smoothing should be applied.
      /// </summary>
      [EditorRange(0.0, 4.0)]
      public Fix32 SpeedSmoothingThreshold;

      public static void Serialize(
        ParticleErosionPostProcessor.ParticleErosionConfig value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ParticleErosionPostProcessor.ParticleErosionConfig>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ParticleErosionPostProcessor.ParticleErosionConfig.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Fix32.Serialize(this.AccumulatedMaterialErodeThresholdMult, writer);
        Fix32.Serialize(this.ErodeSpeedThreshold, writer);
        RelTile1i.Serialize(this.ExtendedGradStep, writer);
        Fix32.Serialize(this.ExtendedGradWeight, writer);
        Fix32.Serialize(this.Friction, writer);
        Fix32.Serialize(this.GradientToVelocityMult, writer);
        HeightTilesF.Serialize(this.LowestProcessedHeightUnderOcean, writer);
        Fix32.Serialize(this.MaxErosionPerStep, writer);
        writer.WriteInt(this.MaxParticleSteps);
        Fix32.Serialize(this.MaxSpeedDelta, writer);
        Fix32.Serialize(this.MinGradient, writer);
        writer.WriteInt(this.MinParticleSteps);
        Fix32.Serialize(this.MinSpeed, writer);
        Fix32.Serialize(this.MomentumFactor, writer);
        writer.WriteString(this.Name);
        Fix32.Serialize(this.ParticlesPerTile, writer);
        writer.WriteULong(this.Seed);
        Fix32.Serialize(this.SpeedSmoothingThreshold, writer);
        RelTile1f.Serialize(this.StepLength, writer);
        Fix32.Serialize(this.TerrainInteractionLifetimeExponent, writer);
        Fix32.Serialize(this.TerrainInteractionMult, writer);
      }

      public static ParticleErosionPostProcessor.ParticleErosionConfig Deserialize(BlobReader reader)
      {
        ParticleErosionPostProcessor.ParticleErosionConfig particleErosionConfig;
        if (reader.TryStartClassDeserialization<ParticleErosionPostProcessor.ParticleErosionConfig>(out particleErosionConfig))
          reader.EnqueueDataDeserialization((object) particleErosionConfig, ParticleErosionPostProcessor.ParticleErosionConfig.s_deserializeDataDelayedAction);
        return particleErosionConfig;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.AccumulatedMaterialErodeThresholdMult = Fix32.Deserialize(reader);
        this.ErodeSpeedThreshold = Fix32.Deserialize(reader);
        this.ExtendedGradStep = RelTile1i.Deserialize(reader);
        this.ExtendedGradWeight = Fix32.Deserialize(reader);
        this.Friction = Fix32.Deserialize(reader);
        this.GradientToVelocityMult = Fix32.Deserialize(reader);
        this.LowestProcessedHeightUnderOcean = HeightTilesF.Deserialize(reader);
        this.MaxErosionPerStep = Fix32.Deserialize(reader);
        this.MaxParticleSteps = reader.ReadInt();
        this.MaxSpeedDelta = Fix32.Deserialize(reader);
        this.MinGradient = Fix32.Deserialize(reader);
        this.MinParticleSteps = reader.ReadInt();
        this.MinSpeed = Fix32.Deserialize(reader);
        this.MomentumFactor = Fix32.Deserialize(reader);
        this.Name = reader.ReadString();
        this.ParticlesPerTile = Fix32.Deserialize(reader);
        this.Seed = reader.ReadULong();
        this.SpeedSmoothingThreshold = Fix32.Deserialize(reader);
        this.StepLength = RelTile1f.Deserialize(reader);
        this.TerrainInteractionLifetimeExponent = Fix32.Deserialize(reader);
        this.TerrainInteractionMult = Fix32.Deserialize(reader);
      }

      public string Name { get; set; }

      /// <summary>
      /// Returns the maximum number of influence in steps that the particle may have.
      /// </summary>
      public int GetMaxInfluenceSteps()
      {
        return ((ParticleErosionPostProcessor.MAX_PARTICLE_TRAVEL_DISTANCE - this.ExtendedGradStep.RelTile1f - RelTile1f.One).Value / this.StepLength.Value).ToIntFloored();
      }

      public ParticleErosionConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CName\u003Ek__BackingField = "Default";
        this.LowestProcessedHeightUnderOcean = -1.0.TilesHigh();
        this.Seed = 3296516565UL;
        this.ParticlesPerTile = Fix32.One;
        this.MinParticleSteps = 5;
        this.MaxParticleSteps = 75;
        this.StepLength = 0.75.Tiles();
        this.TerrainInteractionMult = 1.ToFix32();
        this.TerrainInteractionLifetimeExponent = 0.2.ToFix32();
        this.ErodeSpeedThreshold = 0.2.ToFix32();
        this.AccumulatedMaterialErodeThresholdMult = 1.ToFix32();
        this.MinSpeed = 0.1.ToFix32();
        this.MinGradient = 0.1.ToFix32();
        this.MomentumFactor = 0.9.ToFix32();
        this.GradientToVelocityMult = 0.2.ToFix32();
        this.ExtendedGradStep = 3.Tiles();
        this.ExtendedGradWeight = 2.ToFix32();
        this.Friction = 0.05.ToFix32();
        this.MaxErosionPerStep = (Fix32) 1;
        this.MaxSpeedDelta = 0.2.ToFix32();
        this.SpeedSmoothingThreshold = 1.0.ToFix32();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static ParticleErosionConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        ParticleErosionPostProcessor.ParticleErosionConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ParticleErosionPostProcessor.ParticleErosionConfig) obj).SerializeData(writer));
        ParticleErosionPostProcessor.ParticleErosionConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ParticleErosionPostProcessor.ParticleErosionConfig) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private readonly struct Schedule
    {
      public readonly int Index;
      public readonly Chunk2i Chunk;
      public readonly ImmutableArray<int> MustBeProcessedIds;

      public static void Serialize(ParticleErosionPostProcessor.Schedule value, BlobWriter writer)
      {
        writer.WriteInt(value.Index);
        Chunk2i.Serialize(value.Chunk, writer);
        ImmutableArray<int>.Serialize(value.MustBeProcessedIds, writer);
      }

      public static ParticleErosionPostProcessor.Schedule Deserialize(BlobReader reader)
      {
        return new ParticleErosionPostProcessor.Schedule(reader.ReadInt(), Chunk2i.Deserialize(reader), ImmutableArray<int>.Deserialize(reader));
      }

      public Schedule(int index, Chunk2i chunk, ImmutableArray<int> mustBeProcessedIds)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Index = index;
        this.Chunk = chunk;
        this.MustBeProcessedIds = mustBeProcessedIds;
      }
    }

    public readonly struct ParticleInfo
    {
      public readonly Tile3f Position;
      public readonly Vector2f Velocity;
      public readonly ThicknessTilesF DeltaHeight;
      public readonly ThicknessTilesF MaterialAmount;
      public readonly ParticleErosionPostProcessor.TerminationReason TerminationReason;

      public ParticleInfo(
        Tile3f position,
        Vector2f velocity,
        ThicknessTilesF deltaHeight,
        ThicknessTilesF materialAmount,
        ParticleErosionPostProcessor.TerminationReason terminationReason)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Position = position;
        this.Velocity = velocity;
        this.DeltaHeight = deltaHeight;
        this.MaterialAmount = materialAmount;
        this.TerminationReason = terminationReason;
      }

      public ParticleErosionPostProcessor.ParticleInfo SetTermReason(
        ParticleErosionPostProcessor.TerminationReason terminationReason)
      {
        return new ParticleErosionPostProcessor.ParticleInfo(this.Position, this.Velocity, this.DeltaHeight, this.MaterialAmount, terminationReason);
      }
    }

    public enum TerminationReason
    {
      None,
      ZeroGradient,
      OutOfBounds,
      LowVelocity,
      GoingUp,
      MaxStepsReached,
    }
  }
}
