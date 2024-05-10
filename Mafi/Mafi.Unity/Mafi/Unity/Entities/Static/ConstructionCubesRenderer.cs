// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.ConstructionCubesRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ConstructionCubesRenderer : IDisposable
  {
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly Set<IStaticEntity> m_entitiesToUpdate;
    private readonly Set<IStaticEntity> m_entitiesToResetAnimations;
    private readonly Dict<IStaticEntity, ConstructionCubesRenderer.CubesForEntityData> m_cubesForEntities;
    private readonly Lyst<ConstructionCubesRenderer.CubesForEntityData> m_dataToRemoveTemp;
    private readonly Option<ConstructionCubesRenderer.CubesRenderingChunk>[] m_chunks;
    private readonly Mesh[] m_cubeMeshSharedLods;
    private readonly Material m_cubeMaterialInstancedRenderingShared;
    private readonly Material m_cubeMaterialOriginalShared;

    public ConstructionCubesRenderer(
      EntitiesManager entitiesManager,
      ConstructionManager constructionManager,
      IGameLoopEvents gameLoopEvents,
      ChunkBasedRenderingManager visibleChunksRenderer,
      AssetsDb assetsDb,
      ReloadAfterAssetUpdateManager reloadManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_entitiesToUpdate = new Set<IStaticEntity>();
      this.m_entitiesToResetAnimations = new Set<IStaticEntity>();
      this.m_cubesForEntities = new Dict<IStaticEntity, ConstructionCubesRenderer.CubesForEntityData>();
      this.m_dataToRemoveTemp = new Lyst<ConstructionCubesRenderer.CubesForEntityData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ConstructionCubesRenderer owner = this;
      Assert.That<int>(visibleChunksRenderer.ChunksCountTotal).IsLess((int) ushort.MaxValue, "Encoding of chunks will overflow.");
      this.m_chunksRenderer = visibleChunksRenderer;
      this.m_reloadManager = reloadManager;
      this.m_chunks = new Option<ConstructionCubesRenderer.CubesRenderingChunk>[visibleChunksRenderer.ChunksCountTotal];
      this.m_cubeMaterialInstancedRenderingShared = assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/ConstructionCubeInstanced.mat");
      KeyValuePair<Mesh, Material> materialFromAsset = assetsDb.GetMeshAndMaterialFromAsset("Assets/Base/Buildings/Rendering/ConstructionCube.prefab");
      this.m_cubeMaterialOriginalShared = materialFromAsset.Value;
      this.m_cubeMeshSharedLods = new Mesh[7];
      for (int index = 0; index < 7; ++index)
        this.m_cubeMeshSharedLods[index] = materialFromAsset.Key;
      gameLoopEvents.SyncUpdate.AddNonSaveable<ConstructionCubesRenderer>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RegisterRendererInitState((object) this, (Action) (() =>
      {
        foreach (IStaticEntity staticEntity in entitiesManager.GetAllEntitiesOfType<IStaticEntity>())
        {
          if (!staticEntity.AreConstructionCubesDisabled && ConstructionCubesRenderer.CubesForEntityData.NeedsCubesData(staticEntity))
            owner.registerNewEntity(staticEntity).InitializeCubesNoAnimation();
        }
        entitiesManager.StaticEntityAdded.AddNonSaveable<ConstructionCubesRenderer>(owner, (Action<IStaticEntity>) (entity =>
        {
          if (entity.AreConstructionCubesDisabled)
            return;
          owner.m_entitiesToUpdate.Add(entity);
        }));
        constructionManager.EntityConstructionStateChanged.AddNonSaveable<ConstructionCubesRenderer>(owner, (Action<IStaticEntity, ConstructionState>) ((entity, _) =>
        {
          if (entity.AreConstructionCubesDisabled)
            return;
          owner.m_entitiesToUpdate.Add(entity);
        }));
        constructionManager.EntityPauseStateChanged.AddNonSaveable<ConstructionCubesRenderer>(owner, (Action<IStaticEntity, bool>) ((entity, _) =>
        {
          if (entity.AreConstructionCubesDisabled)
            return;
          owner.m_entitiesToUpdate.Add(entity);
        }));
        constructionManager.OnResetConstructionAnimationState.AddNonSaveable<ConstructionCubesRenderer>(owner, (Action<IStaticEntity>) (entity => owner.m_entitiesToResetAnimations.Add(entity)));
      }));
    }

    public void Dispose()
    {
      for (int index = 0; index < this.m_chunks.Length; ++index)
      {
        this.m_chunks[index].ValueOrNull?.Dispose();
        this.m_chunks[index] = Option<ConstructionCubesRenderer.CubesRenderingChunk>.None;
      }
    }

    private void syncUpdate(GameTime time)
    {
      if (this.m_entitiesToUpdate.IsNotEmpty)
      {
        foreach (IStaticEntity staticEntity in this.m_entitiesToUpdate)
        {
          ConstructionCubesRenderer.CubesForEntityData cubesForEntityData;
          if (this.m_cubesForEntities.TryGetValue(staticEntity, out cubesForEntityData))
            cubesForEntityData.UpdateState();
          else if (ConstructionCubesRenderer.CubesForEntityData.NeedsCubesData(staticEntity))
            this.registerNewEntity(staticEntity);
        }
        this.m_entitiesToUpdate.Clear();
      }
      if (this.m_entitiesToResetAnimations.IsNotEmpty)
      {
        foreach (IStaticEntity toResetAnimation in this.m_entitiesToResetAnimations)
        {
          ConstructionCubesRenderer.CubesForEntityData cubesForEntityData;
          if (this.m_cubesForEntities.TryGetValue(toResetAnimation, out cubesForEntityData))
            cubesForEntityData.ResetAnimations();
        }
        this.m_entitiesToResetAnimations.Clear();
      }
      if (!this.m_cubesForEntities.IsNotEmpty)
        return;
      this.m_dataToRemoveTemp.Clear();
      foreach (ConstructionCubesRenderer.CubesForEntityData cubesForEntityData in this.m_cubesForEntities.Values)
      {
        if (cubesForEntityData.SyncUpdate(time))
          this.m_dataToRemoveTemp.Add(cubesForEntityData);
      }
      foreach (ConstructionCubesRenderer.CubesForEntityData cubesForEntityData in this.m_dataToRemoveTemp)
      {
        this.m_cubesForEntities.RemoveAndAssert(cubesForEntityData.Entity);
        cubesForEntityData.Destroy();
      }
      this.m_dataToRemoveTemp.Clear();
    }

    private ConstructionCubesRenderer.CubesForEntityData registerNewEntity(
      IStaticEntity staticEntity)
    {
      ConstructionCubesRenderer.CubesForEntityData cubesForEntityData = new ConstructionCubesRenderer.CubesForEntityData(this, staticEntity);
      this.m_cubesForEntities.AddAndAssertNew(staticEntity, cubesForEntityData);
      return cubesForEntityData;
    }

    private ConstructionCubesRenderer.CubesRenderingChunk getOrCreateChunk(Chunk256Index index)
    {
      ConstructionCubesRenderer.CubesRenderingChunk newChunk = this.m_chunks[(int) index.Value].ValueOrNull;
      if (newChunk == null)
      {
        this.m_chunks[(int) index.Value] = (Option<ConstructionCubesRenderer.CubesRenderingChunk>) (newChunk = new ConstructionCubesRenderer.CubesRenderingChunk(this.m_chunksRenderer.ExtendChunkCoord(index), this));
        this.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
      }
      return newChunk;
    }

    private uint addCube(ConstructionCubesRenderer.CubeInstanceData data)
    {
      Chunk256Index chunkIndex = this.m_chunksRenderer.GetChunkIndex(data.Position.Xy);
      ConstructionCubesRenderer.CubesRenderingChunk chunk = this.getOrCreateChunk(chunkIndex);
      if (chunk.InstancesCount < (int) ushort.MaxValue)
        return (uint) chunk.AddInstance(data) | (uint) chunkIndex.Value << 16;
      Log.Error("Too many construction cubes on a chunk!");
      return uint.MaxValue;
    }

    private void updateCube(uint id, ConstructionCubesRenderer.CubeInstanceData data)
    {
      if (id == uint.MaxValue)
        return;
      uint index = id >> 16 & (uint) ushort.MaxValue;
      Option<ConstructionCubesRenderer.CubesRenderingChunk>[] chunks = this.m_chunks;
      if (index >= (uint) chunks.Length)
      {
        Log.Error(string.Format("Invalid chunk index {0} for array of length {1}", (object) index, (object) chunks.Length));
      }
      else
      {
        ConstructionCubesRenderer.CubesRenderingChunk valueOrNull = chunks[(int) index].ValueOrNull;
        if (valueOrNull == null)
          Log.Error(string.Format("Failed to update construction cube instance, invalid chunk coord: {0}", (object) index));
        else
          valueOrNull.UpdateInstance((ushort) id, data);
      }
    }

    private void updateCubeColor(uint id, ColorRgba newColor)
    {
      if (id == uint.MaxValue)
        return;
      uint index = id >> 16 & (uint) ushort.MaxValue;
      Option<ConstructionCubesRenderer.CubesRenderingChunk>[] chunks = this.m_chunks;
      if (index >= (uint) chunks.Length)
      {
        Log.Error(string.Format("Invalid chunk index {0} for array of length {1}", (object) index, (object) chunks.Length));
      }
      else
      {
        ConstructionCubesRenderer.CubesRenderingChunk valueOrNull = chunks[(int) index].ValueOrNull;
        if (valueOrNull == null)
        {
          Log.Error(string.Format("Failed to update construction cube instance at index {0}, no chunk", (object) index));
        }
        else
        {
          ushort id1 = (ushort) id;
          valueOrNull.UpdateInstance(id1, valueOrNull.GetInstanceData(id1).WithNewColor(newColor));
        }
      }
    }

    private void removeCube(uint id)
    {
      if (id == uint.MaxValue)
        return;
      uint index = id >> 16 & (uint) ushort.MaxValue;
      Option<ConstructionCubesRenderer.CubesRenderingChunk>[] chunks = this.m_chunks;
      if (index >= (uint) chunks.Length)
      {
        Log.Error(string.Format("Invalid chunk index {0} for array of length {1}", (object) index, (object) chunks.Length));
      }
      else
      {
        ConstructionCubesRenderer.CubesRenderingChunk valueOrNull = chunks[(int) index].ValueOrNull;
        if (valueOrNull == null)
          Log.Error(string.Format("Failed to remove construction cube instance, invalid chunk coord: {0}", (object) index));
        else
          valueOrNull.RemoveInstance((ushort) id);
      }
    }

    [ExpectedStructSize(32)]
    private readonly struct CubeInstanceData
    {
      public readonly Tile3i Position;
      /// <summary>
      /// Time (based on <see cref="P:Mafi.Core.GameTime.TimeSinceLoadMs" />) at which the transition started.
      /// </summary>
      public readonly int TransitionStartTime;
      public readonly int TransitionEndTime;
      public readonly int TransitionHeightTiles;
      public readonly uint ColorRgb;
      public readonly uint Data;

      public CubeInstanceData(
        Tile3i position,
        int transitionStartTime,
        int transitionEndTime,
        int transitionHeightTiles,
        uint colorRgb,
        uint data)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.TransitionStartTime = transitionStartTime;
        this.TransitionEndTime = transitionEndTime;
        this.TransitionHeightTiles = transitionHeightTiles;
        this.ColorRgb = colorRgb;
        this.Data = data;
      }

      public CubeInstanceData(
        ConstrCubeSpec cubeSpec,
        SimStep transitionStartStep,
        SimStep transitionEndStep,
        ColorRgba color,
        int cubeIndex,
        bool isAppearing)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = cubeSpec.Position.ExtendHeight(cubeSpec.Height);
        this.TransitionStartTime = transitionStartStep.Value;
        this.TransitionEndTime = transitionEndStep.Value;
        this.TransitionHeightTiles = (int) cubeSpec.TransitionHeightTiles;
        this.ColorRgb = color.Rgba;
        this.Data = (uint) ((int) cubeSpec.ScaleX << 24 | (int) cubeSpec.ScaleY << 16 | (int) cubeSpec.ScaleZ << 8 | cubeIndex & 3 | (isAppearing ? 0 : 8));
      }

      public ConstructionCubesRenderer.CubeInstanceData WithNewColor(ColorRgba color)
      {
        return new ConstructionCubesRenderer.CubeInstanceData(this.Position, this.TransitionStartTime, this.TransitionEndTime, this.TransitionHeightTiles, color.Rgba, this.Data);
      }

      public override string ToString()
      {
        return string.Format("{0} t{1}-{2} h{3} 0x{4:X8}", (object) this.Position, (object) this.TransitionStartTime, (object) this.TransitionEndTime, (object) this.TransitionHeightTiles, (object) this.Data);
      }
    }

    private sealed class CubesForEntityData
    {
      private static readonly ColorRgba CUBE_COLOR_PAUSED;
      private static readonly ColorRgba CUBE_COLOR_CONSTRUCTION;
      private static readonly ColorRgba CUBE_COLOR_DECONSTRUCTION;
      public readonly IStaticEntity Entity;
      private readonly ImmutableArray<ConstrCubeSpec> m_cubesSpec;
      private readonly int m_totalCubesVolume;
      private readonly ConstructionCubesRenderer m_parentRenderer;
      private PooledArray<uint> m_displayedCubesData;
      private int m_currentCubesVolume;
      private int m_syncedCubesVolume;
      private int m_displayedCubesCount;
      private int m_disappearingCubesCount;
      private SimStep m_lastDisappearedCubeAnimEndTime;
      private ColorRgba m_color;

      public CubesForEntityData(ConstructionCubesRenderer parentRenderer, IStaticEntity entity)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_parentRenderer = parentRenderer;
        this.Entity = entity;
        this.m_cubesSpec = entity.GetConstructionCubesSpec(out this.m_totalCubesVolume);
        this.m_displayedCubesData = PooledArray<uint>.GetPooled(this.m_cubesSpec.Length);
        this.m_color = this.getCubesColor();
      }

      public void Destroy()
      {
        if (!this.m_displayedCubesData.IsValid)
        {
          Log.Warning(string.Format("Cubes for entity {0} is already disposed.", (object) this.Entity));
        }
        else
        {
          this.hideAllCubes();
          this.m_displayedCubesData.ReturnToPool();
          this.m_displayedCubesData = new PooledArray<uint>();
        }
      }

      private void hideAllCubes()
      {
        if (!this.m_displayedCubesData.IsValid)
        {
          Log.Error(string.Format("Cubes for entity {0} is already disposed.", (object) this.Entity));
        }
        else
        {
          int num = this.m_displayedCubesCount + this.m_disappearingCubesCount;
          if (num > this.m_displayedCubesData.Length)
          {
            Log.Error(string.Format("Too many cubes, displayed: {0}, disappearing: ", (object) this.m_displayedCubesCount) + string.Format("{0}, but data length is only: {1}", (object) this.m_disappearingCubesCount, (object) this.m_displayedCubesData.Length));
            num = this.m_displayedCubesData.Length;
          }
          for (int i = num - 1; i >= 0; --i)
            this.m_parentRenderer.removeCube(this.m_displayedCubesData[i]);
          this.m_displayedCubesCount = 0;
          this.m_currentCubesVolume = 0;
          this.m_syncedCubesVolume = 0;
          this.m_disappearingCubesCount = 0;
        }
      }

      public static bool NeedsCubesData(IStaticEntity entity)
      {
        if (entity.ConstructionProgress.IsNone)
          return false;
        switch (entity.ConstructionState)
        {
          case ConstructionState.NotInitialized:
          case ConstructionState.NotStarted:
          case ConstructionState.Constructed:
          case ConstructionState.PreparingUpgrade:
          case ConstructionState.PendingDeconstruction:
          case ConstructionState.Deconstructed:
            return false;
          case ConstructionState.InConstruction:
          case ConstructionState.BeingUpgraded:
          case ConstructionState.InDeconstruction:
            return !entity.ConstructionProgress.Value.IsPaused || entity.ConstructionProgress.Value.Progress.IsPositive;
          default:
            Log.Warning(string.Format("Unhandled construction state: {0}", (object) entity.ConstructionState));
            return false;
        }
      }

      public void UpdateState() => this.updateCubesColor();

      public void ResetAnimations()
      {
        this.hideAllCubes();
        if (this.Entity.IsDestroyed || !this.Entity.ConstructionProgress.HasValue)
          return;
        this.InitializeCubesNoAnimation();
      }

      public void InitializeCubesNoAnimation()
      {
        Assert.That<bool>(this.Entity.IsDestroyed).IsFalse();
        Assert.That<int>(this.m_displayedCubesCount).IsZero();
        Assert.That<int>(this.m_currentCubesVolume).IsZero();
        int targetCubesVolume = this.getTargetCubesVolume((IConstructionProgress) this.Entity.ConstructionProgress.Value);
        for (int index = 0; index < this.m_cubesSpec.Length; ++index)
        {
          ConstrCubeSpec cubeSpec = this.m_cubesSpec[index];
          if (targetCubesVolume < (int) cubeSpec.Volume)
            break;
          ++this.m_displayedCubesCount;
          this.m_currentCubesVolume += (int) cubeSpec.Volume;
          targetCubesVolume -= (int) cubeSpec.Volume;
          ConstructionCubesRenderer.CubeInstanceData data = new ConstructionCubesRenderer.CubeInstanceData(cubeSpec, new SimStep(0), new SimStep(1), this.m_color, index, true);
          this.m_displayedCubesData[index] = this.m_parentRenderer.addCube(data);
        }
      }

      /// <summary>
      /// Returns whether this instance should be removed and destroyed.
      /// </summary>
      public bool SyncUpdate(GameTime time)
      {
        if (this.m_disappearingCubesCount > 0 && this.m_lastDisappearedCubeAnimEndTime.Value < time.SimStepsSinceLoad)
        {
          for (int index = this.m_disappearingCubesCount - 1; index >= 0; --index)
            this.m_parentRenderer.removeCube(this.m_displayedCubesData[this.m_displayedCubesCount + index]);
          this.m_disappearingCubesCount = 0;
        }
        if (!ConstructionCubesRenderer.CubesForEntityData.NeedsCubesData(this.Entity))
        {
          this.ensureCubesCount(0, new SimStep(time.SimStepsSinceLoad));
          return this.m_disappearingCubesCount <= 0;
        }
        this.ensureCubesCount(this.getTargetCubesVolume((IConstructionProgress) this.Entity.ConstructionProgress.Value), new SimStep(time.SimStepsSinceLoad));
        return false;
      }

      private int getTargetCubesVolume(IConstructionProgress progress)
      {
        int currentSteps = progress.CurrentSteps;
        if (currentSteps <= 0)
          return 0;
        if (progress.IsUpgrade)
        {
          int num1 = progress.MaxSteps - progress.ExtraSteps;
          if (currentSteps < num1)
            return 0;
          int num2 = progress.MaxSteps - ConstructionManager.EXTRA_CONSTRUCTION_DURATION.Ticks;
          return currentSteps >= num2 ? this.m_totalCubesVolume : (int) ((long) (currentSteps - num1) * (long) this.m_totalCubesVolume / (long) (num2 - num1));
        }
        int num = progress.MaxSteps - progress.ExtraSteps;
        if (num <= 0)
          return 0;
        return currentSteps > num ? this.m_totalCubesVolume : (int) ((long) this.m_totalCubesVolume * (long) currentSteps / (long) num);
      }

      private void ensureCubesCount(int targetVolume, SimStep currentStep)
      {
        if (targetVolume == this.m_syncedCubesVolume)
          return;
        this.m_syncedCubesVolume = targetVolume;
        if (targetVolume >= this.m_currentCubesVolume)
        {
          int num = targetVolume - this.m_currentCubesVolume;
          for (int displayedCubesCount = this.m_displayedCubesCount; displayedCubesCount < this.m_displayedCubesData.Length; ++displayedCubesCount)
          {
            ConstrCubeSpec cubeSpec = this.m_cubesSpec[displayedCubesCount];
            if (num < (int) cubeSpec.Volume)
              break;
            this.m_currentCubesVolume += (int) cubeSpec.Volume;
            num -= (int) cubeSpec.Volume;
            ConstructionCubesRenderer.CubeInstanceData data = new ConstructionCubesRenderer.CubeInstanceData(cubeSpec, currentStep, getTransitionEndStep((int) cubeSpec.TransitionHeightTiles), this.m_color, displayedCubesCount, true);
            if (this.m_disappearingCubesCount > 0)
            {
              this.m_parentRenderer.updateCube(this.m_displayedCubesData[displayedCubesCount], data);
              --this.m_disappearingCubesCount;
            }
            else
              this.m_displayedCubesData[displayedCubesCount] = this.m_parentRenderer.addCube(data);
            ++this.m_displayedCubesCount;
          }
        }
        else
        {
          int num = this.m_currentCubesVolume - targetVolume;
          for (int index = this.m_displayedCubesCount - 1; index >= 0; --index)
          {
            ConstrCubeSpec cubeSpec = this.m_cubesSpec[index];
            if (num < (int) cubeSpec.Volume)
              break;
            --this.m_displayedCubesCount;
            this.m_currentCubesVolume -= (int) cubeSpec.Volume;
            num -= (int) cubeSpec.Volume;
            ConstructionCubesRenderer.CubeInstanceData data = new ConstructionCubesRenderer.CubeInstanceData(cubeSpec, currentStep, getTransitionEndStep((int) cubeSpec.TransitionHeightTiles), this.m_color, index, false);
            this.m_parentRenderer.updateCube(this.m_displayedCubesData[index], data);
            this.m_lastDisappearedCubeAnimEndTime = this.m_lastDisappearedCubeAnimEndTime.Max(new SimStep(data.TransitionEndTime));
          }
        }

        SimStep getTransitionEndStep(int transitionHeightTiles)
        {
          return new SimStep(currentStep.Value + 5 + 5 * transitionHeightTiles.FastSqrtSmallInt().ToIntFloored());
        }
      }

      private ColorRgba getCubesColor()
      {
        if (this.Entity.IsDestroyed)
          return ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_DECONSTRUCTION;
        if (this.Entity.ConstructionProgress.IsNone)
          return ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_CONSTRUCTION;
        if (this.Entity.ConstructionProgress.Value.IsPaused)
          return ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_PAUSED;
        return !this.Entity.ConstructionProgress.Value.IsDeconstruction ? ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_CONSTRUCTION : ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_DECONSTRUCTION;
      }

      private void updateCubesColor()
      {
        ColorRgba cubesColor = this.getCubesColor();
        if (this.m_color == cubesColor)
          return;
        this.m_color = cubesColor;
        int num = this.m_displayedCubesCount + this.m_disappearingCubesCount;
        if (num > this.m_displayedCubesData.Length)
        {
          Log.Error(string.Format("Too many cubes, displayed: {0}, disappearing: ", (object) this.m_displayedCubesCount) + string.Format("{0}, but data length is only: {1}", (object) this.m_disappearingCubesCount, (object) this.m_displayedCubesData.Length));
          num = this.m_displayedCubesData.Length;
        }
        for (int i = 0; i < num; ++i)
          this.m_parentRenderer.updateCubeColor(this.m_displayedCubesData[i], cubesColor);
      }

      static CubesForEntityData()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_PAUSED = (ColorRgba) 10066329;
        ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_CONSTRUCTION = (ColorRgba) 7769020;
        ConstructionCubesRenderer.CubesForEntityData.CUBE_COLOR_DECONSTRUCTION = (ColorRgba) 10911358;
      }
    }

    private sealed class CubesRenderingChunk : IRenderedChunk, IRenderedChunksBase, IDisposable
    {
      private readonly ConstructionCubesRenderer m_parentRenderer;
      private readonly InstancedMeshesRenderer<ConstructionCubesRenderer.CubeInstanceData> m_renderer;
      private IRenderedChunksParent m_chunkParent;
      private Bounds m_bounds;
      private float m_minHeight;
      private float m_maxHeight;

      public string Name => "Construction cubes";

      public int InstancesCount => this.m_renderer.InstancesCount;

      public Vector2 Origin { get; }

      public Chunk256AndIndex CoordAndIndex { get; }

      public bool TrackStoppedRendering => false;

      public float MaxModelDeviationFromChunkBounds => 0.0f;

      public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

      public CubesRenderingChunk(
        Chunk256AndIndex coordAndIndex,
        ConstructionCubesRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.CoordAndIndex = coordAndIndex;
        this.Origin = coordAndIndex.OriginTile2i.ToVector2();
        this.m_parentRenderer = parentRenderer;
        this.m_renderer = new InstancedMeshesRenderer<ConstructionCubesRenderer.CubeInstanceData>(parentRenderer.m_cubeMeshSharedLods, InstancingUtils.InstantiateMaterialAndCopyTextures(parentRenderer.m_cubeMaterialInstancedRenderingShared, parentRenderer.m_cubeMaterialOriginalShared));
        parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_renderer);
      }

      public void Dispose()
      {
        this.m_parentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<ConstructionCubesRenderer.CubeInstanceData>>(this.m_renderer);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
      {
        info.Add(new RenderedInstancesInfo(this.Name, this.m_renderer.InstancesCount, this.m_renderer.IndicesCountForLod0));
      }

      public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
      {
        return this.m_renderer.Render(this.m_bounds, lod);
      }

      public void Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

      public void NotifyWasNotRendered()
      {
      }

      public ushort AddInstance(ConstructionCubesRenderer.CubeInstanceData data)
      {
        ushort num = this.m_renderer.AddInstance(data);
        float unityUnits = (float) data.Position.Height.ToUnityUnits();
        if (this.m_renderer.InstancesCount == 1)
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
        return num;
      }

      public ConstructionCubesRenderer.CubeInstanceData GetInstanceData(ushort id)
      {
        return this.m_renderer.GetData(id);
      }

      public void UpdateInstance(ushort id, ConstructionCubesRenderer.CubeInstanceData data)
      {
        this.m_renderer.UpdateInstance(id, data);
      }

      public void RemoveInstance(ushort id) => this.m_renderer.RemoveInstance(id);
    }
  }
}
