// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.InstancedChunkBasedLayoutEntitiesRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Unity.Entities.Emissions;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  /// <summary>
  /// Renders static entities using chung-based instanced rendering.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class InstancedChunkBasedLayoutEntitiesRenderer : DelayedEntitiesRenderer, IDisposable
  {
    public static readonly ColorRgba NO_BLUEPRINT_COLOR;
    public static readonly ColorRgba BLUEPRINT_UNDECIDABLE_COLOR;
    public static readonly ColorRgba BLUEPRINT_PAUSED_COLOR;
    public static readonly ColorRgba BLUEPRINT_CONSTRUCTION_COLOR;
    public static readonly ColorRgba BLUEPRINT_DECONSTRUCTION_COLOR;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly AssetsDb m_assetsDb;
    private readonly ObjectHighlighter m_highlighter;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly EntitiesIconRenderer m_entitiesIconRenderer;
    private readonly EntityMbFactory m_entityMbFactory;
    private readonly EntitiesAnimationsUpdater m_entitiesAnimationsUpdater;
    private readonly SimLoopEvents m_simLoopEvents;
    private readonly Material m_normalMaterialForInstancedRenderingShared;
    private readonly Material m_reflectedMaterialForInstancedRenderingShared;
    private readonly Material m_blueprintMaterialForInstancedRenderingShared;
    private readonly Material m_blueprintReflectedMaterialForInstancedRenderingShared;
    private readonly Material m_highlightMaterialForInstancedRenderingShared;
    private readonly Material m_collapseMaterialForInstancedRenderingShared;
    private readonly Material m_collapseReflectedMaterialForInstancedRenderingShared;
    private readonly Material m_normalMaterialForAnimatedInstancedRenderingShared;
    private readonly Material m_reflectedMaterialForAnimatedInstancedRenderingShared;
    private readonly Material m_blueprintMaterialForAnimatedInstancedRenderingShared;
    private readonly Material m_blueprintReflectedMaterialForAnimatedInstancedRenderingShared;
    private readonly Material m_highlightMaterialForAnimatedInstancedRenderingShared;
    private readonly Material m_collapseMaterialForAnimatedInstancedRenderingShared;
    private readonly Material m_collapseReflectedMaterialForAnimatedInstancedRenderingShared;
    private readonly Lyst<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData> m_entityRenderingData;
    /// <summary>
    /// Parent GO that has all the entities to avoid mess in the scene root.
    /// </summary>
    private readonly GameObject m_parentGo;
    /// <summary>
    /// Maps <see cref="T:Mafi.Core.Entities.IRenderedEntity" /> to its <see cref="T:Mafi.Unity.Entities.EntityMb" /> that is responsible for control of the entity. Keep
    /// in mind that entity's <see cref="T:UnityEngine.GameObject" /> might have more <see cref="T:UnityEngine.MonoBehaviour" /> attached.
    /// </summary>
    private readonly Dict<IRenderedEntity, EntityMb> m_entities;
    private readonly Lyst<IEntityMbWithRenderUpdate> m_entitiesToRenderUpdate;
    private readonly Lyst<IEntityMbWithSyncUpdate> m_entitiesToSyncUpdate;
    private readonly Lyst<IEntityMbWithSimUpdateEnd> m_entitiesToSimEndUpdate;
    private readonly Lyst<IAnimatedEntity> m_entitiesToAnimate;
    private readonly Lyst<IEntityWithEmission> m_entitiesToUpdateEmission;
    private readonly Lyst<Machine> m_entitiesToUpdateMachineEmission;
    /// <summary>
    /// Maps <see cref="T:UnityEngine.GameObject" /> to <see cref="T:Mafi.Core.Entities.IEntity" />.
    /// </summary>
    private readonly Dict<GameObject, IRenderedEntity> m_goToEntity;
    private readonly List<IDestroyableEntityMb> m_destroyableMbsTmp;
    private readonly ImmutableArray<IEntityMbUpdater> m_updaters;
    private readonly ImmutableArray<IEntityMbInitializer> m_initializers;
    private bool m_animatedMbsChanged;
    private bool m_emittingMbsChanged;
    private bool m_emittingMachineMbsChanged;
    private readonly Material m_mbBlueprintMaterialShared;
    private LystStruct<InstancedChunkBasedLayoutEntitiesRenderer.AnimationUpdateData> m_animationsUpdateDataTmp;
    private InstancedChunkBasedLayoutEntitiesRenderer.AnimationUpdateData[] m_animationsDataCache;
    private LystStruct<InstancedChunkBasedLayoutEntitiesRenderer.EmissionUpdateData> m_emissionsUpdateDataTmp;
    private InstancedChunkBasedLayoutEntitiesRenderer.EmissionUpdateData[] m_emissionsDataCache;
    private LystStruct<InstancedChunkBasedLayoutEntitiesRenderer.MachineEmissionUpdateData> m_machineEmissionsUpdateDataTmp;
    private InstancedChunkBasedLayoutEntitiesRenderer.MachineEmissionUpdateData[] m_machineEmissionsDataCache;

    public override int Priority => 100;

    public IReadOnlyDictionary<IRenderedEntity, EntityMb> RenderedEntities
    {
      get => (IReadOnlyDictionary<IRenderedEntity, EntityMb>) this.m_entities;
    }

    public InstancedChunkBasedLayoutEntitiesRenderer(
      EntitiesRenderingManager entitiesRenderingManager,
      ChunkBasedRenderingManager visibleChunksRenderer,
      AssetsDb assetsDb,
      ObjectHighlighter highlighter,
      ReloadAfterAssetUpdateManager reloadManager,
      EntitiesIconRenderer entitiesIconRenderer,
      EntityMbFactory entityMbFactory,
      EntitiesAnimationsUpdater entitiesAnimationsUpdater,
      AllImplementationsOf<IEntityMbUpdater> updaters,
      AllImplementationsOf<IEntityMbInitializer> initializers,
      IGameLoopEvents gameLoopEvents,
      SimLoopEvents simLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_entityRenderingData = new Lyst<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData>()
      {
        (InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData) null
      };
      this.m_parentGo = new GameObject("Instanced Entities");
      this.m_entities = new Dict<IRenderedEntity, EntityMb>();
      this.m_entitiesToRenderUpdate = new Lyst<IEntityMbWithRenderUpdate>();
      this.m_entitiesToSyncUpdate = new Lyst<IEntityMbWithSyncUpdate>();
      this.m_entitiesToSimEndUpdate = new Lyst<IEntityMbWithSimUpdateEnd>();
      this.m_entitiesToAnimate = new Lyst<IAnimatedEntity>();
      this.m_entitiesToUpdateEmission = new Lyst<IEntityWithEmission>();
      this.m_entitiesToUpdateMachineEmission = new Lyst<Machine>();
      this.m_goToEntity = new Dict<GameObject, IRenderedEntity>();
      this.m_destroyableMbsTmp = new List<IDestroyableEntityMb>();
      this.m_animationsDataCache = Array.Empty<InstancedChunkBasedLayoutEntitiesRenderer.AnimationUpdateData>();
      this.m_emissionsDataCache = Array.Empty<InstancedChunkBasedLayoutEntitiesRenderer.EmissionUpdateData>();
      this.m_machineEmissionsDataCache = Array.Empty<InstancedChunkBasedLayoutEntitiesRenderer.MachineEmissionUpdateData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_chunksRenderer = visibleChunksRenderer;
      this.m_assetsDb = assetsDb;
      this.m_highlighter = highlighter;
      this.m_reloadManager = reloadManager;
      this.m_entitiesIconRenderer = entitiesIconRenderer;
      this.m_entityMbFactory = entityMbFactory;
      this.m_entitiesAnimationsUpdater = entitiesAnimationsUpdater;
      this.m_simLoopEvents = simLoopEvents;
      this.m_updaters = updaters.Implementations;
      this.m_initializers = initializers.Implementations;
      this.m_mbBlueprintMaterialShared = assetsDb.GetSharedMaterial("Assets/Core/Materials/BuildingBlueprint.mat");
      this.m_normalMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityInstanced.mat");
      this.m_reflectedMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityReflectedInstanced.mat");
      this.m_blueprintMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityBlueprintInstanced.mat");
      this.m_blueprintReflectedMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityBlueprintReflectedInstanced.mat");
      this.m_highlightMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityHighlightInstanced.mat");
      this.m_collapseMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityCollapseInstanced.mat");
      this.m_collapseReflectedMaterialForInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityCollapseReflectedInstanced.mat");
      this.m_normalMaterialForAnimatedInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityAnimatedInstanced.mat");
      this.m_reflectedMaterialForAnimatedInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityAnimatedReflectedInstanced.mat");
      this.m_blueprintMaterialForAnimatedInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityAnimatedBlueprintInstanced.mat");
      this.m_blueprintReflectedMaterialForAnimatedInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityAnimatedBlueprintReflectedInstanced.mat");
      this.m_highlightMaterialForAnimatedInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityHighlightAnimatedInstanced.mat");
      this.m_collapseMaterialForAnimatedInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityAnimatedCollapseInstanced.mat");
      this.m_collapseReflectedMaterialForAnimatedInstancedRenderingShared = this.m_assetsDb.GetSharedMaterial("Assets/Base/Buildings/Rendering/LayoutEntityAnimatedCollapseReflectedInstanced.mat");
      entitiesRenderingManager.RegisterRenderer((IEntitiesRenderer) this);
      gameLoopEvents.RenderUpdate.AddNonSaveable<InstancedChunkBasedLayoutEntitiesRenderer>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void Dispose()
    {
      for (int index = 1; index < this.m_entityRenderingData.Count; ++index)
        this.m_entityRenderingData[index].Dispose();
    }

    public override bool CanRenderEntity(EntityProto proto)
    {
      return proto is LayoutEntityProto layoutEntityProto && (layoutEntityProto.Graphics.UseInstancedRendering || layoutEntityProto.Graphics.UseSemiInstancedRendering);
    }

    private ushort getOrCreateEntityRenderingData(ILayoutEntity entity)
    {
      uint entityRenderingData = entity.Prototype.Graphics.InstancedRendererIndex;
      if (entityRenderingData == 0U)
      {
        entityRenderingData = (uint) this.m_entityRenderingData.Count;
        if (entityRenderingData > (uint) ushort.MaxValue)
        {
          Log.Error(string.Format("Too many rendered protos, only {0} supported.", (object) ushort.MaxValue));
          return ushort.MaxValue;
        }
        LayoutEntityProto.Gfx graphics = entity.Prototype.Graphics;
        graphics.InstancedRendererIndex = entityRenderingData;
        ImmutableArrayBuilder<ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>>> immutableArrayBuilder = new ImmutableArrayBuilder<ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>>>(7);
        for (int i = 0; i < 7; ++i)
          immutableArrayBuilder[i] = (ImmutableArray<KeyValuePair<string, IReadOnlySet<string>>>) ImmutableArray.Empty;
        float result1 = 0.0f;
        if (graphics.UseSemiInstancedRendering && entity is IAnimatedEntity animatedEntity && animatedEntity.AnimationParams.IsNotEmpty)
        {
          for (int index1 = 0; index1 < 7; ++index1)
          {
            Lyst<KeyValuePair<string, IReadOnlySet<string>>> list = new Lyst<KeyValuePair<string, IReadOnlySet<string>>>();
            TextAsset result2;
            if (!this.m_assetsDb.TryGetSharedAsset<TextAsset>(graphics.AnimationGameObjectsPathForLod(index1), out result2, true))
            {
              if (index1 == 0)
              {
                Log.Error(string.Format("Animation data not found for '{0}'.", (object) entity));
              }
              else
              {
                foreach (KeyValuePair<string, IReadOnlySet<string>> keyValuePair in immutableArrayBuilder[index1 - 1])
                  list.AddAndAssertNew<string, IReadOnlySet<string>>(keyValuePair.Key, (IReadOnlySet<string>) new Set<string>());
              }
            }
            else
            {
              Lyst<string> lines = result2.text.SplitToLines();
              int index2 = 0;
              while (index2 < lines.Count)
              {
                string key = lines[index2];
                if (key.Length != 0)
                {
                  int index3 = index2 + 1 + 1;
                  if (!float.TryParse(lines[index3], NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
                    Log.Error(string.Format("Parsing animation metadata for '{0}' gone wrong. ", (object) entity) + string.Format("Expecting float for animation length. Got '{0}' on line '{1}'", (object) lines[index3], (object) index3));
                  index2 = index3 + 1;
                  AnimationParams first = animatedEntity.AnimationParams.First;
                  switch (first)
                  {
                    case LoopAnimationParams loopAnimationParams:
                      result1 /= loopAnimationParams.Speed.ToFloat();
                      break;
                    case AnimationWithPauseParams animationWithPauseParams:
                      result1 /= animationWithPauseParams.BaseSpeed.ToFloat();
                      break;
                    case RepeatableAnimationParams repeatableAnimationParams:
                      if (repeatableAnimationParams.CustomSpeed.HasValue)
                      {
                        result1 /= repeatableAnimationParams.CustomSpeed.Value.ToFloat();
                        break;
                      }
                      break;
                    default:
                      Log.Error("Unknown animation params " + first.GetType().Name);
                      break;
                  }
                  Set<string> set = new Set<string>();
                  for (; index2 < lines.Count; ++index2)
                  {
                    string str = lines[index2];
                    if (str.Length == 0)
                    {
                      ++index2;
                      break;
                    }
                    set.Add(str);
                  }
                  list.AddAndAssertNew<string, IReadOnlySet<string>>(key, (IReadOnlySet<string>) set);
                }
                else
                  break;
              }
            }
            immutableArrayBuilder[index1] = list.ToImmutableArrayAndClear();
          }
        }
        graphics.AnimatedGameObjects = immutableArrayBuilder.GetImmutableArrayAndClear();
        graphics.AnimationLength = result1;
        this.m_entityRenderingData.Add(new InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData(this, entity.Prototype));
      }
      return (ushort) entityRenderingData;
    }

    private void renderUpdate(GameTime time)
    {
      foreach (IEntityMbWithRenderUpdate withRenderUpdate in this.m_entitiesToRenderUpdate)
        withRenderUpdate.RenderUpdate(time);
      if (time.IsGamePaused)
        return;
      foreach (InstancedChunkBasedLayoutEntitiesRenderer.MachineEmissionUpdateData emissionUpdateData in this.m_machineEmissionsDataCache)
        emissionUpdateData.Data.UpdateMachineEmission(emissionUpdateData.LayoutEntity, emissionUpdateData.InstanceId, emissionUpdateData.Controllers);
    }

    public override void SyncUpdate(GameTime time)
    {
      base.SyncUpdate(time);
      foreach (IEntityMbWithSyncUpdate mbWithSyncUpdate in this.m_entitiesToSyncUpdate)
        mbWithSyncUpdate.SyncUpdate(time);
      if (this.m_animatedMbsChanged)
      {
        this.m_animatedMbsChanged = false;
        this.m_animationsUpdateDataTmp.Clear();
        foreach (IAnimatedEntity key in this.m_entitiesToAnimate)
        {
          if (!(key is ILayoutEntity layoutEntity))
          {
            Log.Error("Unexpected non-layout entity.");
          }
          else
          {
            EntitiesAnimationsUpdater.EntityAnimationsData entityAnimationsData;
            if (!this.m_entitiesAnimationsUpdater.AnimationsData.TryGetValue((IEntity) key, out entityAnimationsData))
            {
              Log.WarningOnce("Animation data not found.");
            }
            else
            {
              ulong num = layoutEntity.RendererData & 72057594037927935UL;
              if (num == 0UL)
              {
                Log.Error("Entity removed?");
              }
              else
              {
                uint index = (uint) (num >> 40);
                ulong instanceId = num & 1099511627775UL;
                InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData data = this.m_entityRenderingData[index];
                foreach (EntitiesAnimationsUpdater.AnimationData animation in entityAnimationsData.Animations)
                  this.m_animationsUpdateDataTmp.Add(new InstancedChunkBasedLayoutEntitiesRenderer.AnimationUpdateData(layoutEntity, instanceId, data, animation));
              }
            }
          }
        }
        this.m_animationsDataCache = this.m_animationsUpdateDataTmp.ToArray();
        this.m_animationsUpdateDataTmp.Clear();
      }
      if (this.m_emittingMbsChanged)
      {
        this.m_emittingMbsChanged = false;
        this.m_emissionsUpdateDataTmp.Clear();
        foreach (IEntityWithEmission entityWithEmission in this.m_entitiesToUpdateEmission)
        {
          if (!(entityWithEmission is ILayoutEntity layoutEntity))
          {
            Log.Error("Entity with emission that isn't a LayoutEntity");
          }
          else
          {
            ulong num = layoutEntity.RendererData & 72057594037927935UL;
            if (num == 0UL)
            {
              Log.Error("Entity removed?");
            }
            else
            {
              uint index = (uint) (num >> 40);
              ulong instanceId = num & 1099511627775UL;
              InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData data = this.m_entityRenderingData[index];
              this.m_emissionsUpdateDataTmp.Add(new InstancedChunkBasedLayoutEntitiesRenderer.EmissionUpdateData(layoutEntity, instanceId, data));
            }
          }
        }
        this.m_emissionsDataCache = this.m_emissionsUpdateDataTmp.ToArray();
        this.m_emissionsUpdateDataTmp.Clear();
      }
      if (this.m_emittingMachineMbsChanged)
      {
        this.m_emittingMachineMbsChanged = false;
        this.m_machineEmissionsUpdateDataTmp.Clear();
        foreach (Machine key in this.m_entitiesToUpdateMachineEmission)
        {
          ILayoutEntity layoutEntity = (ILayoutEntity) key;
          if (layoutEntity == null)
          {
            Log.Error("Machine with emission that isn't a LayoutEntity");
          }
          else
          {
            ulong num = layoutEntity.RendererData & 72057594037927935UL;
            if (num == 0UL)
            {
              Log.Error("Entity removed?");
            }
            else
            {
              uint index = (uint) (num >> 40);
              ulong instanceId = num & 1099511627775UL;
              InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData data = this.m_entityRenderingData[index];
              ImmutableArray<MachinesEmissionsController> controllers = (ImmutableArray<MachinesEmissionsController>) (ImmutableArray.EmptyArray) null;
              foreach (IEntityMbUpdater updater in this.m_updaters)
              {
                if (updater is MachinesEmissionsUpdater emissionsUpdater && !emissionsUpdater.Controllers.TryGetValue((IEntity) key, out controllers))
                  Log.Error(string.Format("Machine emission controllers not found for entity '{0}'", (object) key));
              }
              if (controllers == (ImmutableArray<MachinesEmissionsController>) (ImmutableArray.EmptyArray) null)
                Log.Error(string.Format("Machine emission controllers not found for entity '{0}'", (object) key));
              else
                this.m_machineEmissionsUpdateDataTmp.Add(new InstancedChunkBasedLayoutEntitiesRenderer.MachineEmissionUpdateData(layoutEntity, instanceId, data, controllers));
            }
          }
        }
        this.m_machineEmissionsDataCache = this.m_machineEmissionsUpdateDataTmp.ToArray();
        this.m_machineEmissionsUpdateDataTmp.Clear();
      }
      if (time.IsGamePaused)
        return;
      foreach (InstancedChunkBasedLayoutEntitiesRenderer.AnimationUpdateData animationUpdateData in this.m_animationsDataCache)
        animationUpdateData.Data.UpdateAnimation(animationUpdateData.LayoutEntity, animationUpdateData.InstanceId, animationUpdateData.AnimationData, time);
      foreach (InstancedChunkBasedLayoutEntitiesRenderer.EmissionUpdateData emissionUpdateData in this.m_emissionsDataCache)
        emissionUpdateData.Data.UpdateEmission(emissionUpdateData.LayoutEntity, emissionUpdateData.InstanceId, (emissionUpdateData.LayoutEntity as IEntityWithEmission).EmissionIntensity.GetValueOrDefault(1f));
      foreach (InstancedChunkBasedLayoutEntitiesRenderer.MachineEmissionUpdateData emissionUpdateData in this.m_machineEmissionsDataCache)
        emissionUpdateData.Data.UpdateMachineEmission(emissionUpdateData.LayoutEntity, emissionUpdateData.InstanceId, emissionUpdateData.Controllers);
    }

    public override void AddEntityOnSync(IRenderedEntity entity, GameTime time)
    {
      if (!(entity is ILayoutEntity entity1))
        Log.Error("Expected LayoutEntity");
      else if (entity.IsDestroyed)
      {
        Log.Warning(string.Format("Adding destroyed entity {0} in `AddEntityOnSync`, ignoring.", (object) entity));
      }
      else
      {
        LayoutEntityProto.Gfx graphics = entity1.Prototype.Graphics;
        int entityRenderingData1 = (int) this.getOrCreateEntityRenderingData(entity1);
        if (entity1.Prototype.Graphics.UseSemiInstancedRendering)
        {
          EntityMb mbFor = this.m_entityMbFactory.CreateMbFor<IRenderedEntity>(entity);
          if (!(bool) (UnityEngine.Object) mbFor)
          {
            Log.Warning(string.Format("Failed to create entity MB for '{0}'.", (object) entity));
            return;
          }
          this.m_entities.AddAndAssertNew(entity, mbFor);
          foreach (SkinnedMeshRenderer componentsInChild in mbFor.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
          {
            if (!graphics.SemiInstancedRenderingExcludedObjects.Contains(componentsInChild.gameObject.name))
              UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild);
          }
          foreach (MeshRenderer componentsInChild in mbFor.gameObject.GetComponentsInChildren<MeshRenderer>())
          {
            if (!graphics.SemiInstancedRenderingExcludedObjects.Contains(componentsInChild.gameObject.name))
              UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild);
          }
          foreach (MeshFilter componentsInChild in mbFor.gameObject.GetComponentsInChildren<MeshFilter>())
          {
            if (!graphics.SemiInstancedRenderingExcludedObjects.Contains(componentsInChild.gameObject.name))
              UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild);
          }
          this.m_goToEntity.AddAndAssertNew(mbFor.gameObject, entity);
          foreach (IEntityMbInitializer initializer in this.m_initializers)
            initializer.InitIfNeeded(mbFor);
          if (!mbFor.IsInitialized)
          {
            Assert.Fail(string.Format("Adding non-initialized mb for '{0}' {1}.", (object) entity, (object) mbFor.GetType()));
            return;
          }
          if (mbFor is IEntityMbWithRenderUpdate withRenderUpdate && (!(withRenderUpdate is IEntityMbWithRenderUpdateMaybe renderUpdateMaybe) || renderUpdateMaybe.NeedsRenderUpdate))
            this.m_entitiesToRenderUpdate.Add(withRenderUpdate);
          if (mbFor is IEntityMbWithSyncUpdate mbWithSyncUpdate && (!(mbWithSyncUpdate is IEntityMbWithSyncUpdateMaybe withSyncUpdateMaybe) || withSyncUpdateMaybe.NeedsSyncUpdate))
            this.m_entitiesToSyncUpdate.Add(mbWithSyncUpdate);
          if (mbFor is IEntityMbWithSimUpdateEnd withSimUpdateEnd)
            this.m_entitiesToSimEndUpdate.Add(withSimUpdateEnd);
          foreach (IEntityMbUpdater updater in this.m_updaters)
            updater.AddOnSyncIfNeeded(mbFor);
          if (entity is StaticEntity staticEntity)
            this.updateBlueprintVisual(staticEntity, mbFor);
          if (entity is IAnimatedEntity animatedEntity && entity1.Prototype is IProtoWithAnimation && animatedEntity.AnimationParams.IsNotEmpty && graphics.AnimatedGameObjects.First.IsNotEmpty)
          {
            this.m_animatedMbsChanged = true;
            this.m_entitiesToAnimate.Add(animatedEntity);
          }
          if (entity is IEntityWithEmission entityWithEmission && entityWithEmission.EmissionIntensity.HasValue)
          {
            this.m_emittingMbsChanged = true;
            this.m_entitiesToUpdateEmission.Add(entityWithEmission);
          }
          if (entity is Machine machine && machine.Prototype.Graphics.EmissionsParams.IsNotEmpty)
          {
            this.m_emittingMachineMbsChanged = true;
            this.m_entitiesToUpdateMachineEmission.Add(machine);
          }
        }
        this.m_entitiesIconRenderer.UpdateVisualsImmediate(entity);
        if ((entity.RendererData & 72057594037927935UL) != 0UL)
        {
          Log.Error("Entity already added? Removing first.");
          this.RemoveEntityOnSync(entity, time, EntityRemoveReason.Remove);
        }
        ushort entityRenderingData2 = this.getOrCreateEntityRenderingData(entity1);
        ulong num = this.m_entityRenderingData[(int) entityRenderingData2].AddInstance(entity1, time);
        entity.RendererData = (ulong) ((long) entity.RendererData & -72057594037927936L | (long) num | (long) entityRenderingData2 << 40);
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
      if (!(entity is ILayoutEntity entity1))
      {
        Log.Error("Expected LayoutEntity");
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
          EntityMb entityMb;
          if (this.m_entities.TryRemove(entity, out entityMb))
          {
            if (entityMb is IEntityMbWithRenderUpdate withRenderUpdate)
              this.m_entitiesToRenderUpdate.Remove(withRenderUpdate);
            if (entityMb is IEntityMbWithSyncUpdate mbWithSyncUpdate)
              this.m_entitiesToSyncUpdate.Remove(mbWithSyncUpdate);
            if (entityMb is IEntityMbWithSimUpdateEnd withSimUpdateEnd)
              this.m_entitiesToSimEndUpdate.Remove(withSimUpdateEnd);
            foreach (IEntityMbUpdater updater in this.m_updaters)
              updater.RemoveOnSyncIfNeeded(entityMb);
            if (entity is IAnimatedEntity animatedEntity && this.m_entitiesToAnimate.Remove(animatedEntity))
              this.m_animatedMbsChanged = true;
            if (entity is IEntityWithEmission entityWithEmission && entityWithEmission.EmissionIntensity.HasValue)
            {
              this.m_entitiesToUpdateEmission.Remove(entityWithEmission);
              this.m_emittingMbsChanged = true;
            }
            if (entity is Machine machine && machine.Prototype.Graphics.EmissionsParams.IsNotEmpty)
            {
              this.m_emittingMachineMbsChanged = true;
              this.m_entitiesToUpdateMachineEmission.Remove(machine);
            }
            this.m_goToEntity.RemoveAndAssert(entityMb.gameObject);
            this.destroyMbs(entityMb);
          }
          uint index = (uint) (num1 >> 40);
          ulong num2 = num1 & 1099511627775UL;
          entity.RendererData &= 18374686479671623680UL;
          if ((long) index >= (long) this.m_entityRenderingData.Count)
          {
            Log.Error(string.Format("Invalid entity data index '{0}'", (object) index));
          }
          else
          {
            InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData protoRenderingData = this.m_entityRenderingData[index];
            bool flag = protoRenderingData.IsBlueprint(entity1, num2);
            if (reason == EntityRemoveReason.Collapse && !flag)
              protoRenderingData.AddCollapsingInstance(entity1, new SimStep(time.SimStepsSinceLoad), num2);
            protoRenderingData.RemoveInstance(entity1, num2, out bool _);
          }
        }
      }
    }

    private void updateBlueprintVisual(StaticEntity staticEntity, EntityMb entityMb)
    {
      if (entityMb is StaticEntityMb staticEntityMb)
      {
        ColorRgba color;
        if (InstancedChunkBasedLayoutEntitiesRenderer.GetBlueprintColor((IStaticEntity) staticEntity, out color))
          staticEntityMb.EnsureBlueprintMaterial(this.m_mbBlueprintMaterialShared, color);
        else
          staticEntityMb.EnsureDefaultMaterial();
      }
      else
        Log.Error(string.Format("Entity '{0}' does not have {1}.", (object) staticEntity, (object) "StaticEntityMb"));
    }

    public override bool TryGetPickableEntityAs<T>(GameObject pickedGo, out T entity)
    {
      IRenderedEntity renderedEntity;
      if (this.m_goToEntity.TryGetValue(pickedGo, out renderedEntity))
      {
        entity = renderedEntity as T;
        return (object) entity != null;
      }
      StaticEntityColliderMb component;
      if (pickedGo.TryGetComponent<StaticEntityColliderMb>(out component))
      {
        entity = component.StaticEntity as T;
        return (object) entity != null;
      }
      entity = default (T);
      return false;
    }

    public override ulong AddHighlight(IRenderedEntity entity, ColorRgba color)
    {
      if (!(entity is ILayoutEntity entity1))
      {
        Log.Error("Expected LayoutEntity");
        return 0;
      }
      ushort entityRenderingData = this.getOrCreateEntityRenderingData(entity1);
      return (ulong) this.m_entityRenderingData[(int) entityRenderingData].AddHighlight(entity1, color, this.m_simLoopEvents.StepsSinceLoad.Value) | (ulong) entityRenderingData << 32;
    }

    public override void RemoveHighlight(ulong highlightId)
    {
      this.m_entityRenderingData[(int) (highlightId >> 32)].RemoveHighlight((uint) highlightId);
    }

    public static bool GetBlueprintColor(IStaticEntity entity, out ColorRgba color)
    {
      if (entity.ConstructionState == ConstructionState.NotInitialized)
      {
        color = InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_UNDECIDABLE_COLOR;
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
        color = InstancedChunkBasedLayoutEntitiesRenderer.NO_BLUEPRINT_COLOR;
        return false;
      }
      if (valueOrNull.IsNearlyFinished)
      {
        color = InstancedChunkBasedLayoutEntitiesRenderer.NO_BLUEPRINT_COLOR;
        return false;
      }
      color = !valueOrNull.IsPaused ? (!valueOrNull.IsDeconstruction ? InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_CONSTRUCTION_COLOR : InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_DECONSTRUCTION_COLOR) : InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_PAUSED_COLOR;
      return true;
    }

    private static bool isTrueBlueprint(IStaticEntity entity)
    {
      if (entity.ConstructionState == ConstructionState.NotInitialized)
        return true;
      bool flag1 = entity.ConstructionProgress.ValueOrNull == null;
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
      return !flag1;
    }

    private void destroyMbs(EntityMb entityMb)
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

    static InstancedChunkBasedLayoutEntitiesRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      InstancedChunkBasedLayoutEntitiesRenderer.NO_BLUEPRINT_COLOR = ColorRgba.White;
      InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_UNDECIDABLE_COLOR = (ColorRgba) 2274140800U;
      InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_PAUSED_COLOR = (ColorRgba) 2576980352U;
      InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_CONSTRUCTION_COLOR = (ColorRgba) 2005458304U;
      InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_DECONSTRUCTION_COLOR = (ColorRgba) 3212343424U;
    }

    private readonly struct AnimationUpdateData
    {
      public readonly ILayoutEntity LayoutEntity;
      public readonly ulong InstanceId;
      public readonly InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData Data;
      public readonly EntitiesAnimationsUpdater.AnimationData AnimationData;

      public AnimationUpdateData(
        ILayoutEntity layoutEntity,
        ulong instanceId,
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData data,
        EntitiesAnimationsUpdater.AnimationData animationData)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.LayoutEntity = layoutEntity;
        this.InstanceId = instanceId;
        this.Data = data;
        this.AnimationData = animationData;
      }
    }

    private readonly struct EmissionUpdateData
    {
      public readonly ILayoutEntity LayoutEntity;
      public readonly ulong InstanceId;
      public readonly InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData Data;

      public EmissionUpdateData(
        ILayoutEntity layoutEntity,
        ulong instanceId,
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData data)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.LayoutEntity = layoutEntity;
        this.InstanceId = instanceId;
        this.Data = data;
      }
    }

    private readonly struct MachineEmissionUpdateData
    {
      public readonly ILayoutEntity LayoutEntity;
      public readonly ulong InstanceId;
      public readonly InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData Data;
      public readonly ImmutableArray<MachinesEmissionsController> Controllers;

      public MachineEmissionUpdateData(
        ILayoutEntity layoutEntity,
        ulong instanceId,
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData data,
        ImmutableArray<MachinesEmissionsController> controllers)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.LayoutEntity = layoutEntity;
        this.InstanceId = instanceId;
        this.Data = data;
        this.Controllers = controllers;
      }
    }

    /// <summary>
    /// Handles rendering of a set of entities based on its proto. Each entity proto has a separate instance.
    /// </summary>
    private class EntityProtoRenderingData : IDisposable
    {
      private static readonly int VERTEX_XY_ANIMATION_TEX_SHADER_ID;
      private static readonly int VERTEX_Z_ANIMATION_TEX_SHADER_ID;
      private static readonly int NORMAL_ANIMATION_TEX_SHADER_ID;
      private static readonly int TANGENT_ANIMATION_TEX_SHADER_ID;
      private static readonly int ANIMATION_LENGTH_SHADER_ID;
      public readonly LayoutEntityProto LayoutEntityProto;
      private readonly InstancedChunkBasedLayoutEntitiesRenderer m_parentRenderer;
      private readonly IReadOnlyDictionary<Material, Mesh[]> m_sharedStaticLods;
      private readonly IReadOnlyDictionary<Material, Mesh[]> m_sharedMovingLods;
      private readonly IReadOnlyDictionary<Material, Texture2D[]> m_animationVertexXyTextures;
      private readonly IReadOnlyDictionary<Material, Texture2D[]> m_animationVertexZTextures;
      private readonly IReadOnlyDictionary<Material, Texture2D[]> m_animationNormalTextures;
      private readonly IReadOnlyDictionary<Material, Texture2D[]> m_animationTangentTextures;
      private readonly GameObject m_colliderGo;
      private readonly float m_maxSize;
      private readonly bool m_hasLODs;
      private readonly Option<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk>[] m_chunks;
      private readonly Dict<ILayoutEntity, StaticEntityColliderMb> m_colliders;
      private readonly Lyst<StaticEntityColliderMb> m_colliderGosPool;
      private readonly InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk m_alwaysRenderChunk;

      public EntityProtoRenderingData(
        InstancedChunkBasedLayoutEntitiesRenderer parentRenderer,
        LayoutEntityProto proto)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_colliders = new Dict<ILayoutEntity, StaticEntityColliderMb>();
        this.m_colliderGosPool = new Lyst<StaticEntityColliderMb>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_parentRenderer = parentRenderer;
        this.LayoutEntityProto = proto;
        this.m_chunks = new Option<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk>[this.m_parentRenderer.m_chunksRenderer.ChunksCountTotal];
        this.m_colliderGo = proto.Graphics.UseSemiInstancedRendering ? (GameObject) null : new GameObject("Colliders for " + this.LayoutEntityProto.Id.Value);
        string error;
        if (!InstancingUtils.TryGetMeshLodsAndMaterialsFromPrefab(this.m_parentRenderer.m_assetsDb, proto.Graphics.PrefabPath, proto.Graphics.AnimatedGameObjects, proto.Graphics.SemiInstancedRenderingExcludedObjects, true, out this.m_sharedStaticLods, ref this.m_colliderGo, out error))
        {
          Log.Error(string.Format("Failed to load prefab for layout entity '{0}': {1}", (object) proto.Id, (object) error));
          this.m_sharedStaticLods = (IReadOnlyDictionary<Material, Mesh[]>) new Dict<Material, Mesh[]>();
        }
        if (proto.Graphics.AnimatedGameObjects.First.IsNotEmpty)
        {
          if (!InstancingUtils.TryGetMeshLodsAndMaterialsFromPrefab(this.m_parentRenderer.m_assetsDb, proto.Graphics.PrefabPath, proto.Graphics.AnimatedGameObjects, proto.Graphics.SemiInstancedRenderingExcludedObjects, false, out this.m_sharedMovingLods, ref this.m_colliderGo, out error))
          {
            Log.Error(string.Format("Failed to load prefab for layout entity '{0}': {1}", (object) proto.Id, (object) error));
            this.m_sharedMovingLods = (IReadOnlyDictionary<Material, Mesh[]>) new Dict<Material, Mesh[]>();
          }
          int num = 16384.Min(SystemInfo.maxTextureSize);
          Dict<Material, Texture2D[]> dict1 = new Dict<Material, Texture2D[]>();
          Dict<Material, Texture2D[]> dict2 = new Dict<Material, Texture2D[]>();
          Dict<Material, Texture2D[]> dict3 = new Dict<Material, Texture2D[]>();
          Dict<Material, Texture2D[]> dict4 = new Dict<Material, Texture2D[]>();
          foreach (Material key in this.m_sharedMovingLods.Keys)
          {
            Texture2D[] texture2DArray1 = new Texture2D[7];
            Texture2D[] texture2DArray2 = new Texture2D[7];
            Texture2D[] texture2DArray3 = new Texture2D[7];
            Texture2D[] texture2DArray4 = new Texture2D[7];
            for (int lod = 0; lod < this.m_sharedMovingLods[key].Length; ++lod)
            {
              if ((UnityEngine.Object) this.m_sharedMovingLods[key][lod] == (UnityEngine.Object) null || this.m_sharedMovingLods[key][lod].vertices.Length == 0)
              {
                texture2DArray1[lod] = (Texture2D) null;
                texture2DArray2[lod] = (Texture2D) null;
                texture2DArray3[lod] = (Texture2D) null;
              }
              else if (lod > 0 && (UnityEngine.Object) this.m_sharedMovingLods[key][lod] == (UnityEngine.Object) this.m_sharedMovingLods[key][lod - 1])
              {
                texture2DArray1[lod] = texture2DArray1[lod - 1];
                texture2DArray2[lod] = texture2DArray2[lod - 1];
                texture2DArray3[lod] = texture2DArray3[lod - 1];
              }
              else
              {
                Texture2D result1;
                if (!this.m_parentRenderer.m_assetsDb.TryGetSharedAsset<Texture2D>(proto.Graphics.AnimationTextureVerticesPathForMaterialAndLod(key.name, lod, "Xy"), out result1))
                {
                  Log.Error(string.Format("No vertex texture found for LOD{0} of animated material {1} for '{2}'", (object) lod, (object) key, (object) proto.Id));
                  break;
                }
                if (result1.width > num || result1.height > num)
                  Log.Error(string.Format("Animation textures too large for system max texture size of `{0}`", (object) num));
                Texture2D result2;
                if (!this.m_parentRenderer.m_assetsDb.TryGetSharedAsset<Texture2D>(proto.Graphics.AnimationTextureVerticesPathForMaterialAndLod(key.name, lod, "Z"), out result2))
                {
                  Log.Error(string.Format("No vertex texture found for LOD{0} of animated material {1} for '{2}'", (object) lod, (object) key, (object) proto.Id));
                  break;
                }
                if (result1.width > num || result1.height > num)
                  Log.Error(string.Format("Animation textures too large for system max texture size of `{0}`", (object) num));
                texture2DArray1[lod] = result1;
                texture2DArray2[lod] = result2;
                Texture2D result3;
                this.m_parentRenderer.m_assetsDb.TryGetSharedAsset<Texture2D>(proto.Graphics.AnimationTextureNormalsPathForMaterialAndLod(key.name, lod), out result3);
                if (result3.width > num || result3.height > num)
                  Log.Error(string.Format("Animation textures too large for system max texture size of `{0}`", (object) num));
                texture2DArray3[lod] = result3;
                Texture2D result4;
                this.m_parentRenderer.m_assetsDb.TryGetSharedAsset<Texture2D>(proto.Graphics.AnimationTextureTangentsPathForMaterialAndLod(key.name, lod), out result4);
                if (result4.width > num || result4.height > num)
                  Log.Error(string.Format("Animation textures too large for system max texture size of `{0}`", (object) num));
                texture2DArray4[lod] = result4;
              }
            }
            dict1[key] = texture2DArray1;
            dict2[key] = texture2DArray2;
            dict3[key] = texture2DArray3;
            dict4[key] = texture2DArray4;
          }
          this.m_animationVertexXyTextures = (IReadOnlyDictionary<Material, Texture2D[]>) dict1;
          this.m_animationVertexZTextures = (IReadOnlyDictionary<Material, Texture2D[]>) dict2;
          this.m_animationNormalTextures = (IReadOnlyDictionary<Material, Texture2D[]>) dict3;
          this.m_animationTangentTextures = (IReadOnlyDictionary<Material, Texture2D[]>) dict4;
        }
        else
          this.m_sharedMovingLods = (IReadOnlyDictionary<Material, Mesh[]>) new Dict<Material, Mesh[]>();
        if (!proto.Graphics.UseSemiInstancedRendering)
          this.m_colliderGo.SetActive(false);
        this.m_alwaysRenderChunk = new InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk(this);
        this.m_parentRenderer.m_chunksRenderer.RegisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this.m_alwaysRenderChunk);
        Assert.That<int>(this.m_sharedStaticLods.Count).IsNotZero();
        Bounds bounds1 = new Bounds();
        foreach (Mesh[] meshArray in this.m_sharedStaticLods.Values)
        {
          foreach (Mesh mesh in meshArray)
          {
            if (!((UnityEngine.Object) mesh == (UnityEngine.Object) null))
            {
              if ((double) bounds1.size.sqrMagnitude == 0.0)
              {
                Bounds bounds2 = mesh.bounds;
                Vector3 center = bounds2.center;
                bounds2 = mesh.bounds;
                Vector3 size = bounds2.size;
                bounds1 = new Bounds(center, size);
              }
              else
                bounds1.Encapsulate(mesh.bounds);
            }
          }
        }
        if (proto.Graphics.AnimatedGameObjects.First.IsNotEmpty)
        {
          Assert.That<int>(this.m_sharedMovingLods.Count).IsNotZero(string.Format("'{0}' has animated game objects but m_sharedMovingLods is empty", (object) proto.Id));
          foreach (Mesh[] meshArray in this.m_sharedMovingLods.Values)
          {
            foreach (Mesh mesh in meshArray)
            {
              if (!((UnityEngine.Object) mesh == (UnityEngine.Object) null))
              {
                if ((double) bounds1.size.sqrMagnitude == 0.0)
                {
                  Bounds bounds3 = mesh.bounds;
                  Vector3 center = bounds3.center;
                  bounds3 = mesh.bounds;
                  Vector3 size = bounds3.size;
                  bounds1 = new Bounds(center, size);
                }
                else
                  bounds1.Encapsulate(mesh.bounds);
              }
            }
          }
        }
        this.m_maxSize = 2f * bounds1.size.x.Max(bounds1.size.y).Max(bounds1.size.z);
      }

      public void Dispose()
      {
        foreach (Option<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk> chunk in this.m_chunks)
          chunk.ValueOrNull?.Dispose();
        this.m_alwaysRenderChunk.Dispose();
        foreach (Component component in this.m_colliderGosPool)
          component.gameObject.Destroy();
        this.m_colliderGosPool.Clear();
      }

      private InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk getOrCreateChunk(
        ILayoutEntity entity)
      {
        return this.getOrCreateChunk(this.m_parentRenderer.m_chunksRenderer.GetChunkIndex(entity.CenterTile.Xy));
      }

      private InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk getOrCreateChunk(
        Chunk256Index index)
      {
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk newChunk = this.m_chunks[(int) index.Value].ValueOrNull;
        if (newChunk == null)
        {
          this.m_chunks[(int) index.Value] = (Option<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk>) (newChunk = new InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk(this.m_parentRenderer.m_chunksRenderer.ExtendChunkCoord(index), this));
          this.m_parentRenderer.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
        }
        return newChunk;
      }

      public ulong AddInstance(ILayoutEntity entity, GameTime time)
      {
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk chunk = this.getOrCreateChunk(entity);
        uint num = chunk.AddInstance(entity, time.SimStepsSinceLoad);
        this.addColliderFor(entity);
        return (ulong) chunk.CoordAndIndex.ChunkIndex.Value << 24 | (ulong) num;
      }

      public bool IsBlueprint(ILayoutEntity entity, ulong id)
      {
        return this.getOrCreateChunk(new Chunk256Index((ushort) ((uint) (id >> 24) & (uint) ushort.MaxValue))).IsBlueprint((uint) id & 16777215U);
      }

      public void RemoveInstance(ILayoutEntity entity, ulong id, out bool wasBlueprint)
      {
        StaticEntityColliderMb entityColliderMb;
        if (!entity.Prototype.Graphics.UseSemiInstancedRendering && this.m_colliders.TryRemove(entity, out entityColliderMb))
        {
          entityColliderMb.gameObject.SetActive(false);
          this.m_colliderGosPool.Add(entityColliderMb);
        }
        this.getOrCreateChunk(new Chunk256Index((ushort) ((uint) (id >> 24) & (uint) ushort.MaxValue))).RemoveInstance((uint) id & 16777215U, out wasBlueprint);
      }

      public uint AddHighlight(ILayoutEntity entity, ColorRgba color, int simStepsSinceLoad)
      {
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.StandardChunk chunk = this.getOrCreateChunk(entity);
        return (uint) chunk.AddHighlight(entity, color, simStepsSinceLoad) | (uint) chunk.CoordAndIndex.ChunkIndex.Value << 16;
      }

      public void RemoveHighlight(uint id)
      {
        ushort instanceId = (ushort) id;
        this.getOrCreateChunk(new Chunk256Index((ushort) (id >> 16 & (uint) ushort.MaxValue))).RemoveHighlight(instanceId);
      }

      public bool TryRemoveHighlight(ILayoutEntity entity)
      {
        return this.getOrCreateChunk(entity).TryRemoveHighlight(entity);
      }

      public void AddCollapsingInstance(
        ILayoutEntity entity,
        SimStep currentStepSinceLoad,
        ulong currentId)
      {
        this.m_alwaysRenderChunk.AddCollapsingInstance(entity, currentStepSinceLoad, currentId);
      }

      private void addColliderFor(ILayoutEntity entity)
      {
        if (entity.Prototype.Graphics.UseSemiInstancedRendering)
          return;
        StaticEntityColliderMb entityColliderMb;
        if (this.m_colliderGosPool.IsNotEmpty)
        {
          entityColliderMb = this.m_colliderGosPool.PopLast();
          entityColliderMb.gameObject.SetActive(true);
        }
        else
        {
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_colliderGo);
          gameObject.SetActive(true);
          entityColliderMb = gameObject.AddComponent<StaticEntityColliderMb>();
        }
        entityColliderMb.Initialize((IStaticEntity) entity);
        StaticEntityMb.GetTransformData(entity).Apply(entityColliderMb.transform);
        this.m_colliders.AddAndAssertNew(entity, entityColliderMb);
      }

      public void UpdateAnimation(
        ILayoutEntity entity,
        ulong id,
        EntitiesAnimationsUpdater.AnimationData animationData,
        GameTime time)
      {
        this.getOrCreateChunk(new Chunk256Index((ushort) ((uint) (id >> 24) & (uint) ushort.MaxValue))).UpdateAnimation(entity, (ushort) (id & (ulong) ushort.MaxValue), animationData, time);
      }

      public void UpdateEmission(ILayoutEntity entity, ulong id, float intensity)
      {
        this.getOrCreateChunk(new Chunk256Index((ushort) ((uint) (id >> 24) & (uint) ushort.MaxValue))).UpdateEmission(entity, (ushort) (id & (ulong) ushort.MaxValue), intensity);
      }

      public void UpdateMachineEmission(
        ILayoutEntity entity,
        ulong id,
        ImmutableArray<MachinesEmissionsController> controllers)
      {
        this.getOrCreateChunk(new Chunk256Index((ushort) ((uint) (id >> 24) & (uint) ushort.MaxValue))).UpdateMachineEmission(entity, (ushort) (id & (ulong) ushort.MaxValue), controllers);
      }

      private void setupAnimatedMaterial(
        Material nonSharedMaterial,
        Material sourceOfTextures,
        int lod)
      {
        nonSharedMaterial.SetTexture(InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.VERTEX_XY_ANIMATION_TEX_SHADER_ID, (Texture) this.m_animationVertexXyTextures[sourceOfTextures][lod]);
        nonSharedMaterial.SetTexture(InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.VERTEX_Z_ANIMATION_TEX_SHADER_ID, (Texture) this.m_animationVertexZTextures[sourceOfTextures][lod]);
        nonSharedMaterial.SetTexture(InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.NORMAL_ANIMATION_TEX_SHADER_ID, (Texture) this.m_animationNormalTextures[sourceOfTextures][lod]);
        nonSharedMaterial.SetTexture(InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.TANGENT_ANIMATION_TEX_SHADER_ID, (Texture) this.m_animationTangentTextures[sourceOfTextures][lod]);
        nonSharedMaterial.SetFloat(InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.ANIMATION_LENGTH_SHADER_ID, this.LayoutEntityProto.Graphics.AnimationLength);
      }

      static EntityProtoRenderingData()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.VERTEX_XY_ANIMATION_TEX_SHADER_ID = Shader.PropertyToID("_VertexAnimationXy");
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.VERTEX_Z_ANIMATION_TEX_SHADER_ID = Shader.PropertyToID("_VertexAnimationZ");
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.NORMAL_ANIMATION_TEX_SHADER_ID = Shader.PropertyToID("_NormalAnimation");
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.TANGENT_ANIMATION_TEX_SHADER_ID = Shader.PropertyToID("_TangentAnimation");
        InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.ANIMATION_LENGTH_SHADER_ID = Shader.PropertyToID("_AnimationLength");
      }

      private sealed class StandardChunk : IRenderedChunk, IRenderedChunksBase, IDisposable
      {
        private readonly InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData m_parentData;
        private readonly int m_maxLod;
        private readonly int m_maxMovingLod;
        private IRenderedChunksParent m_chunkParent;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> m_normalChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> m_reflectedChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> m_normalBlueprintChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> m_reflectedBlueprintChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> m_highlightsChunks;
        private bool m_isRegisteredForHighlight;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> m_normalMovingChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> m_reflectedMovingChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> m_normalBlueprintMovingChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> m_reflectedBlueprintMovingChunks;
        private LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> m_highlightsMovingChunks;
        private bool m_isMovingRegisteredForHighlight;
        private readonly Lyst<KeyValuePair<ILayoutEntity, ushort>> m_entityToHighlightInstanceId;
        private int m_instancesCount;
        private Bounds m_bounds;
        private float m_minHeight;
        private float m_maxHeight;

        public string Name => this.m_parentData.LayoutEntityProto.Id.Value;

        public Vector2 Origin { get; }

        public Chunk256AndIndex CoordAndIndex { get; }

        public bool TrackStoppedRendering => false;

        public float MaxModelDeviationFromChunkBounds => this.m_parentData.m_maxSize;

        public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

        public int InstancesCount => this.m_instancesCount;

        public StandardChunk(
          Chunk256AndIndex coordAndIndex,
          InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData parentData)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.m_entityToHighlightInstanceId = new Lyst<KeyValuePair<ILayoutEntity, ushort>>();
          // ISSUE: explicit constructor call
          base.\u002Ector();
          this.CoordAndIndex = coordAndIndex;
          this.Origin = this.CoordAndIndex.OriginTile2i.ToVector2();
          this.m_parentData = parentData;
          this.m_maxLod = this.m_parentData.LayoutEntityProto.Graphics.MaxRenderedLod.Clamp(0, 6);
          this.m_maxMovingLod = this.m_parentData.m_hasLODs ? 0 : this.m_parentData.LayoutEntityProto.Graphics.MaxRenderedLod.Clamp(0, 6);
        }

        public void Dispose()
        {
          ReloadAfterAssetUpdateManager reloadManager = this.m_parentData.m_parentRenderer.m_reloadManager;
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> normalChunk in this.m_normalChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>(normalChunk);
          this.m_normalChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> reflectedChunk in this.m_reflectedChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>(reflectedChunk);
          this.m_reflectedChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> normalBlueprintChunk in this.m_normalBlueprintChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>(normalBlueprintChunk);
          this.m_normalBlueprintChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> reflectedBlueprintChunk in this.m_reflectedBlueprintChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>(reflectedBlueprintChunk);
          this.m_reflectedBlueprintChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> normalMovingChunk in this.m_normalMovingChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>>(normalMovingChunk);
          this.m_normalMovingChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> reflectedMovingChunk in this.m_reflectedMovingChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>>(reflectedMovingChunk);
          this.m_reflectedMovingChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> blueprintMovingChunk in this.m_normalBlueprintMovingChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>>(blueprintMovingChunk);
          this.m_normalBlueprintMovingChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> blueprintMovingChunk in this.m_reflectedBlueprintMovingChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>>(blueprintMovingChunk);
          this.m_reflectedBlueprintMovingChunks.Clear();
          if (!this.m_highlightsChunks.IsNotEmpty && !this.m_highlightsMovingChunks.IsNotEmpty)
            return;
          if (this.m_isRegisteredForHighlight)
          {
            this.m_isRegisteredForHighlight = false;
            foreach (ICustomHighlightsRenderer highlightsChunk in this.m_highlightsChunks)
              this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer(highlightsChunk);
          }
          if (this.m_isMovingRegisteredForHighlight)
          {
            this.m_isMovingRegisteredForHighlight = false;
            foreach (ICustomHighlightsRenderer highlightsMovingChunk in this.m_highlightsMovingChunks)
              this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer(highlightsMovingChunk);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> highlightsChunk in this.m_highlightsChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>(highlightsChunk);
          this.m_highlightsChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> highlightsMovingChunk in this.m_highlightsMovingChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>>(highlightsMovingChunk);
          this.m_highlightsMovingChunks.Clear();
        }

        public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
        {
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> normalChunk in this.m_normalChunks)
            info.Add(new RenderedInstancesInfo(this.Name, normalChunk.InstancesCount, normalChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> reflectedChunk in this.m_reflectedChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (reflected)", reflectedChunk.InstancesCount, reflectedChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> normalBlueprintChunk in this.m_normalBlueprintChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (blueprint)", normalBlueprintChunk.InstancesCount, normalBlueprintChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> reflectedBlueprintChunk in this.m_reflectedBlueprintChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (blueprint, refl)", reflectedBlueprintChunk.InstancesCount, reflectedBlueprintChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> highlightsChunk in this.m_highlightsChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (highlight)", highlightsChunk.InstancesCount, highlightsChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> normalMovingChunk in this.m_normalMovingChunks)
            info.Add(new RenderedInstancesInfo(this.Name, normalMovingChunk.InstancesCount, normalMovingChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> reflectedMovingChunk in this.m_reflectedMovingChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (reflected)", reflectedMovingChunk.InstancesCount, reflectedMovingChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> blueprintMovingChunk in this.m_normalBlueprintMovingChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (blueprint)", blueprintMovingChunk.InstancesCount, blueprintMovingChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> blueprintMovingChunk in this.m_reflectedBlueprintMovingChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (blueprint, refl)", blueprintMovingChunk.InstancesCount, blueprintMovingChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> highlightsMovingChunk in this.m_highlightsMovingChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (highlight)", highlightsMovingChunk.InstancesCount, highlightsMovingChunk.IndicesCountForLod0));
        }

        public uint AddInstance(ILayoutEntity entity, int simStepsSinceLoad)
        {
          ColorRgba color;
          bool blueprintColor = InstancedChunkBasedLayoutEntitiesRenderer.GetBlueprintColor((IStaticEntity) entity, out color);
          byte chunkTypeIndex1;
          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> local1 = ref this.getOrCreateStaticChunkRef(entity.Transform.IsReflected, blueprintColor, out chunkTypeIndex1);
          InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData data1 = this.packInstanceData(entity, color);
          bool boundsChanged = false;
          recalcBounds(data1.Position);
          ushort num = local1[0].AddInstance(data1);
          for (int index = 1; index < local1.Count; ++index)
          {
            InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData data2 = this.packInstanceData(entity, color);
            if ((int) local1[index].AddInstance(data2) != (int) num)
              Log.Error("Chunks instanced ID incorrect!");
            recalcBounds(data2.Position);
          }
          if (this.m_parentData.LayoutEntityProto.Graphics.AnimatedGameObjects.First.IsNotEmpty)
          {
            byte chunkTypeIndex2;
            ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> local2 = ref this.getOrCreateMovingChunkRef(entity.Transform.IsReflected, blueprintColor, out chunkTypeIndex2);
            Assert.That<byte>(chunkTypeIndex2).IsEqualTo<byte>(chunkTypeIndex1);
            for (int index = 0; index < local2.Count; ++index)
            {
              InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData data3 = this.packAnimatedInstanceData(entity, color, simStepsSinceLoad);
              if ((int) local2[index].AddInstance(data3) != (int) num)
                Log.Error("Chunks instanced ID incorrect!");
              recalcBounds(data3.Position);
            }
          }
          if (boundsChanged)
          {
            this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
            this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
          }
          ++this.m_instancesCount;
          return (uint) num | (uint) chunkTypeIndex1 << 16;

          void recalcBounds(Vector3 position)
          {
            float y = position.y;
            if ((double) this.m_minHeight == 0.0 && (double) this.m_maxHeight == 0.0)
            {
              this.m_minHeight = this.m_maxHeight = y;
              boundsChanged = true;
            }
            else if ((double) y < (double) this.m_minHeight)
            {
              this.m_minHeight = y;
              boundsChanged = true;
            }
            else
            {
              if ((double) y <= (double) this.m_maxHeight)
                return;
              this.m_maxHeight = y;
              boundsChanged = true;
            }
          }
        }

        public bool IsBlueprint(uint id) => id >> 16 >= 2U;

        public void RemoveInstance(uint id, out bool wasBlueprint)
        {
          uint chunkTypeIndex = id >> 16;
          bool fail = false;
          wasBlueprint = chunkTypeIndex >= 2U;
          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> local1 = ref getChunkRef();
          if (fail)
          {
            Log.Error(string.Format("Failed to remove entity, invalid chunk type index {0}.", (object) chunkTypeIndex));
          }
          else
          {
            ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> local2 = ref getMovingChunkRef();
            if (fail || local1.IsEmpty && local2.IsEmpty)
            {
              Log.Error(string.Format("Failed to remove entity, invalid chunk type index {0}.", (object) chunkTypeIndex));
            }
            else
            {
              ushort index = (ushort) id;
              foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> instancedMeshesRenderer in local1)
                instancedMeshesRenderer.RemoveInstance(index);
              foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> instancedMeshesRenderer in local2)
                instancedMeshesRenderer.RemoveInstance(index);
              --this.m_instancesCount;
              if (this.m_instancesCount >= 0)
                return;
              this.m_instancesCount = ((IEnumerable<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>) this.m_normalChunks.GetBackingArray()).Sum<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>((Func<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>, int>) (c => c.InstancesCount)) + ((IEnumerable<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>) this.m_reflectedChunks.GetBackingArray()).Sum<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>((Func<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>, int>) (c => c.InstancesCount)) + ((IEnumerable<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>) this.m_normalBlueprintChunks.GetBackingArray()).Sum<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>((Func<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>, int>) (c => c.InstancesCount)) + ((IEnumerable<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>) this.m_reflectedBlueprintChunks.GetBackingArray()).Sum<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>>((Func<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>, int>) (c => c.InstancesCount));
              Log.Error(string.Format("Instanced count went negative, recomputed to: {0}", (object) this.m_instancesCount));
            }
          }

          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> getChunkRef()
          {
            switch (chunkTypeIndex)
            {
              case 0:
                return ref this.m_normalChunks;
              case 1:
                return ref this.m_reflectedChunks;
              case 2:
                return ref this.m_normalBlueprintChunks;
              case 3:
                return ref this.m_reflectedBlueprintChunks;
              default:
                fail = true;
                return ref this.m_normalChunks;
            }
          }

          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> getMovingChunkRef()
          {
            switch (chunkTypeIndex)
            {
              case 0:
                return ref this.m_normalMovingChunks;
              case 1:
                return ref this.m_reflectedMovingChunks;
              case 2:
                return ref this.m_normalBlueprintMovingChunks;
              case 3:
                return ref this.m_reflectedBlueprintMovingChunks;
              default:
                fail = true;
                return ref this.m_normalMovingChunks;
            }
          }
        }

        public ushort AddHighlight(
          ILayoutEntity layoutEntity,
          ColorRgba color,
          int simStepsSinceLoad)
        {
          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> local1 = ref this.m_highlightsChunks;
          if (local1.IsEmpty)
          {
            foreach (Mesh[] sharedMeshLods in this.m_parentData.m_sharedStaticLods.Values)
            {
              InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>(sharedMeshLods, UnityEngine.Object.Instantiate<Material>(this.m_parentData.m_parentRenderer.m_highlightMaterialForInstancedRenderingShared), shadowCastingMode: ShadowCastingMode.Off);
              local1.Add(instance);
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
          }
          InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData data1 = this.packInstanceData(layoutEntity, color);
          if (local1.IsEmpty)
          {
            Log.Error("Chunks unexpectedly empty");
            return 0;
          }
          ushort num = local1[0].AddInstance(data1);
          for (int index = 1; index < local1.Count; ++index)
          {
            InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData data2 = this.packInstanceData(layoutEntity, color);
            if ((int) local1[index].AddInstance(data2) != (int) num)
              Log.Error("Chunks instanced ID incorrect!");
          }
          if (this.m_parentData.LayoutEntityProto.Graphics.AnimatedGameObjects.First.IsNotEmpty)
          {
            ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> local2 = ref this.m_highlightsMovingChunks;
            if (local2.IsEmpty)
            {
              foreach (KeyValuePair<Material, Mesh[]> sharedMovingLod in (IEnumerable<KeyValuePair<Material, Mesh[]>>) this.m_parentData.m_sharedMovingLods)
              {
                Material key = sharedMovingLod.Key;
                Material[] nonSharedMaterials = new Material[7];
                for (int lod = 0; lod < 7; ++lod)
                {
                  Material nonSharedMaterial = UnityEngine.Object.Instantiate<Material>(this.m_parentData.m_parentRenderer.m_highlightMaterialForAnimatedInstancedRenderingShared);
                  this.m_parentData.setupAnimatedMaterial(nonSharedMaterial, key, lod);
                  nonSharedMaterials[lod] = nonSharedMaterial;
                }
                InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>(sharedMovingLod.Value, nonSharedMaterials, shadowCastingMode: ShadowCastingMode.Off);
                local2.Add(instance);
                this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
              }
            }
            for (int index = 0; index < local2.Count; ++index)
            {
              InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData data3 = this.packAnimatedInstanceData(layoutEntity, color, simStepsSinceLoad);
              if ((int) local2[index].AddInstance(data3) != (int) num)
                Log.Error("Chunks instanced ID incorrect!");
            }
          }
          this.m_entityToHighlightInstanceId.Add(new KeyValuePair<ILayoutEntity, ushort>(layoutEntity, num));
          return num;
        }

        public void RemoveHighlight(ushort instanceId)
        {
          if (this.m_highlightsChunks.IsEmpty)
          {
            Log.Error("Failed to remove highlight, chunk never created.");
          }
          else
          {
            foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> highlightsChunk in this.m_highlightsChunks)
              highlightsChunk.RemoveInstance(instanceId);
            foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> highlightsMovingChunk in this.m_highlightsMovingChunks)
              highlightsMovingChunk.RemoveInstance(instanceId);
            KeyValuePair<ILayoutEntity, ushort> keyValuePair1 = new KeyValuePair<ILayoutEntity, ushort>();
            foreach (KeyValuePair<ILayoutEntity, ushort> keyValuePair2 in this.m_entityToHighlightInstanceId)
            {
              if ((int) keyValuePair2.Value == (int) instanceId)
              {
                keyValuePair1 = keyValuePair2;
                break;
              }
            }
            if (keyValuePair1.Key != null)
              this.m_entityToHighlightInstanceId.Remove<ILayoutEntity, ushort>(keyValuePair1.Key);
            else
              Log.Error("Not found entity with highlight on removal.");
          }
        }

        public bool TryRemoveHighlight(ILayoutEntity entity)
        {
          ushort instanceId;
          if (!this.m_entityToHighlightInstanceId.TryGetValue<ILayoutEntity, ushort>(entity, out instanceId))
            return false;
          this.RemoveHighlight(instanceId);
          return true;
        }

        public void UpdateAnimation(
          ILayoutEntity entity,
          ushort instanceId,
          EntitiesAnimationsUpdater.AnimationData animationData,
          GameTime time)
        {
          if (InstancedChunkBasedLayoutEntitiesRenderer.isTrueBlueprint((IStaticEntity) entity))
            return;
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> instancedMeshesRenderer in this.getOrCreateMovingChunkRef(entity.Transform.IsReflected, false, out byte _))
          {
            if (instancedMeshesRenderer.GetDataRef(instanceId).UpdateAnimationIfNeeded(this.m_parentData.LayoutEntityProto.Graphics.AnimationLength, animationData, time))
              instancedMeshesRenderer.NotifyDataUpdated(instanceId);
          }
          ushort index;
          if (!this.m_isMovingRegisteredForHighlight || !this.m_entityToHighlightInstanceId.TryGetValue<ILayoutEntity, ushort>(entity, out index))
            return;
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> highlightsMovingChunk in this.m_highlightsMovingChunks)
          {
            if (highlightsMovingChunk.GetDataRef(index).UpdateAnimationIfNeeded(this.m_parentData.LayoutEntityProto.Graphics.AnimationLength, animationData, time))
            {
              highlightsMovingChunk.NotifyDataUpdated(instanceId);
              EntityMb entityMb;
              if (!this.m_parentData.m_parentRenderer.m_entities.TryGetValue((IRenderedEntity) entity, out entityMb))
                Log.Error(string.Format("No MB found in highlight animation update for '{0}'", (object) entity));
              else
                this.m_parentData.m_parentRenderer.m_highlighter.SetHasChanged(entityMb.gameObject);
            }
          }
        }

        public InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData GetAnimatedInstanceData(
          ILayoutEntity entity,
          ushort instanceId)
        {
          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> local = ref this.getOrCreateMovingChunkRef(entity.Transform.IsReflected, false, out byte _);
          if (!local.IsEmpty)
            return local[0].GetData(instanceId);
          Log.Error("No moving chunks?");
          return new InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData();
        }

        public void UpdateEmission(
          ILayoutEntity entity,
          ushort instanceId,
          float emissionIntensity)
        {
          if (InstancedChunkBasedLayoutEntitiesRenderer.isTrueBlueprint((IStaticEntity) entity))
            return;
          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> local1 = ref this.getOrCreateStaticChunkRef(entity.Transform.IsReflected, false, out byte _);
          for (int index = 0; index < local1.Count; ++index)
          {
            if (local1[index].GetDataRef(instanceId).UpdateEmissionIfNeeded(emissionIntensity))
              local1[index].NotifyDataUpdated(instanceId);
          }
          if (!this.m_parentData.LayoutEntityProto.Graphics.AnimatedGameObjects.First.IsNotEmpty)
            return;
          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> local2 = ref this.getOrCreateMovingChunkRef(entity.Transform.IsReflected, false, out byte _);
          for (int index = 0; index < local2.Count; ++index)
          {
            if (local2[index].GetDataRef(instanceId).UpdateEmissionIfNeeded(emissionIntensity))
              local2[index].NotifyDataUpdated(instanceId);
          }
        }

        public void UpdateMachineEmission(
          ILayoutEntity entity,
          ushort instanceId,
          ImmutableArray<MachinesEmissionsController> controllers)
        {
          if (InstancedChunkBasedLayoutEntitiesRenderer.isTrueBlueprint((IStaticEntity) entity))
            return;
          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> local1 = ref this.getOrCreateStaticChunkRef(entity.Transform.IsReflected, false, out byte _);
          if (local1.IsNotEmpty && (int) instanceId >= local1[0].InstancesCount)
          {
            Log.Warning("Trying to updated machine emission but instance not found.");
          }
          else
          {
            for (int index = 0; index < local1.Count; ++index)
            {
              ref InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData local2 = ref local1[index].GetDataRef(instanceId);
              foreach (MachinesEmissionsController controller in controllers)
              {
                foreach (UnityEngine.Object material in local1[index].Materials)
                {
                  if (material.name == controller.EmissionParams.MaterialName && local2.UpdateEmissionIfNeeded(controller.RenderIntensity.Max(0.0f)))
                    local1[index].NotifyDataUpdated(instanceId);
                }
              }
            }
            if (!this.m_parentData.LayoutEntityProto.Graphics.AnimatedGameObjects.First.IsNotEmpty)
              return;
            ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> local3 = ref this.getOrCreateMovingChunkRef(entity.Transform.IsReflected, false, out byte _);
            if (local3.IsNotEmpty && (int) instanceId >= local3[0].InstancesCount)
            {
              Log.Warning("Trying to updated machine emission but instance not found.");
            }
            else
            {
              for (int index = 0; index < local3.Count; ++index)
              {
                ref InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData local4 = ref local3[index].GetDataRef(instanceId);
                foreach (MachinesEmissionsController controller in controllers)
                {
                  foreach (UnityEngine.Object material in local3[index].Materials)
                  {
                    if (material.name == controller.EmissionParams.MaterialName && local4.UpdateEmissionIfNeeded(controller.RenderIntensity.Max(0.0f)))
                      local3[index].NotifyDataUpdated(instanceId);
                  }
                }
              }
            }
          }
        }

        private InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData packInstanceData(
          ILayoutEntity entity,
          ColorRgba color)
        {
          return new InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData((entity.Prototype.Layout.GetModelOrigin(entity.Transform) - new RelTile3f(entity.Transform.TransformMatrix.Transform(entity.Prototype.Graphics.PrefabOrigin.Xy.Vector2f).ExtendZ(entity.Prototype.Graphics.PrefabOrigin.Z))).ToVector3(), entity.Transform.Rotation, entity.Transform.IsReflected, color, entity is IEntityWithEmission entityWithEmission ? entityWithEmission.EmissionIntensity.GetValueOrDefault(1f) : 0.0f);
        }

        private InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData packAnimatedInstanceData(
          ILayoutEntity entity,
          ColorRgba color,
          int simStepsSinceLoad)
        {
          Tile3f tile = entity.Prototype.Layout.GetModelOrigin(entity.Transform) - new RelTile3f(entity.Transform.TransformMatrix.Transform(entity.Prototype.Graphics.PrefabOrigin.Xy.Vector2f).ExtendZ(entity.Prototype.Graphics.PrefabOrigin.Z));
          float emissionIntensity = entity is IEntityWithEmission entityWithEmission ? entityWithEmission.EmissionIntensity.GetValueOrDefault(1f) : 0.0f;
          EntitiesAnimationsUpdater.EntityAnimationsData entityAnimationsData;
          float animationStartTime;
          float invAnimationLength;
          if (!this.m_parentData.m_parentRenderer.m_entitiesAnimationsUpdater.AnimationsData.TryGetValue((IEntity) entity, out entityAnimationsData))
          {
            Log.Error(string.Format("Animations data for '{0}' not found.", (object) entity));
            animationStartTime = 0.0f;
            invAnimationLength = 0.0f;
          }
          else
          {
            animationStartTime = (float) ((double) simStepsSinceLoad / 10.0 - (double) entityAnimationsData.Animations.First.AnimationState.GetState().TimeMs.ToFloat() / 1000.0);
            invAnimationLength = 1000f / entityAnimationsData.Animations.First.AnimationLengthMs;
          }
          return new InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData(tile.ToVector3(), entity.Transform.Rotation, entity.Transform.IsReflected, color, animationStartTime, invAnimationLength, true, simStepsSinceLoad, emissionIntensity);
        }

        private ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> getOrCreateStaticChunkRef(
          bool isReflected,
          bool isBlueprint,
          out byte chunkTypeIndex)
        {
          if (isBlueprint)
          {
            if (isReflected)
            {
              chunkTypeIndex = (byte) 3;
              return ref getOrCreateRef(ref this.m_reflectedBlueprintChunks);
            }
            chunkTypeIndex = (byte) 2;
            return ref getOrCreateRef(ref this.m_normalBlueprintChunks);
          }
          if (isReflected)
          {
            chunkTypeIndex = (byte) 1;
            return ref getOrCreateRef(ref this.m_reflectedChunks);
          }
          chunkTypeIndex = (byte) 0;
          return ref getOrCreateRef(ref this.m_normalChunks);

          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> getOrCreateRef(
            ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>> chunksRef)
          {
            if (chunksRef.IsNotEmpty)
              return ref chunksRef;
            Material materialToInstantiate = isBlueprint ? (isReflected ? this.m_parentData.m_parentRenderer.m_blueprintReflectedMaterialForInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_blueprintMaterialForInstancedRenderingShared) : (isReflected ? this.m_parentData.m_parentRenderer.m_reflectedMaterialForInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_normalMaterialForInstancedRenderingShared);
            foreach (KeyValuePair<Material, Mesh[]> sharedStaticLod in (IEnumerable<KeyValuePair<Material, Mesh[]>>) this.m_parentData.m_sharedStaticLods)
            {
              Material nonSharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(materialToInstantiate, sharedStaticLod.Key);
              InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData>(sharedStaticLod.Value, nonSharedMaterial, shadowCastingMode: isBlueprint ? ShadowCastingMode.Off : ShadowCastingMode.On);
              chunksRef.Add(instance);
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
            return ref chunksRef;
          }
        }

        private ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> getOrCreateMovingChunkRef(
          bool isReflected,
          bool isBlueprint,
          out byte chunkTypeIndex)
        {
          if (isBlueprint)
          {
            if (isReflected)
            {
              chunkTypeIndex = (byte) 3;
              return ref getOrCreateRef(ref this.m_reflectedBlueprintMovingChunks);
            }
            chunkTypeIndex = (byte) 2;
            return ref getOrCreateRef(ref this.m_normalBlueprintMovingChunks);
          }
          if (isReflected)
          {
            chunkTypeIndex = (byte) 1;
            return ref getOrCreateRef(ref this.m_reflectedMovingChunks);
          }
          chunkTypeIndex = (byte) 0;
          return ref getOrCreateRef(ref this.m_normalMovingChunks);

          ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> getOrCreateRef(
            ref LystStruct<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>> chunksRef)
          {
            if (chunksRef.IsNotEmpty)
              return ref chunksRef;
            Material materialToInstantiate = isBlueprint ? (isReflected ? this.m_parentData.m_parentRenderer.m_blueprintReflectedMaterialForAnimatedInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_blueprintMaterialForAnimatedInstancedRenderingShared) : (isReflected ? this.m_parentData.m_parentRenderer.m_reflectedMaterialForAnimatedInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_normalMaterialForAnimatedInstancedRenderingShared);
            foreach (KeyValuePair<Material, Mesh[]> sharedMovingLod in (IEnumerable<KeyValuePair<Material, Mesh[]>>) this.m_parentData.m_sharedMovingLods)
            {
              Material key = sharedMovingLod.Key;
              Material[] nonSharedMaterials = new Material[7];
              for (int lod = 0; lod < 7; ++lod)
              {
                Material nonSharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(materialToInstantiate, key);
                this.m_parentData.setupAnimatedMaterial(nonSharedMaterial, key, lod);
                nonSharedMaterials[lod] = nonSharedMaterial;
              }
              InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData>(sharedMovingLod.Value, nonSharedMaterials, shadowCastingMode: isBlueprint ? ShadowCastingMode.Off : ShadowCastingMode.On);
              chunksRef.Add(instance);
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
            return ref chunksRef;
          }
        }

        public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
        {
          if (lod > this.m_maxLod)
          {
            if (this.m_isRegisteredForHighlight)
            {
              this.m_isRegisteredForHighlight = false;
              if (this.m_highlightsChunks.IsNotEmpty)
              {
                foreach (ICustomHighlightsRenderer highlightsChunk in this.m_highlightsChunks)
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer(highlightsChunk);
              }
            }
            if (this.m_isMovingRegisteredForHighlight)
            {
              this.m_isMovingRegisteredForHighlight = false;
              if (this.m_highlightsMovingChunks.IsNotEmpty)
              {
                foreach (ICustomHighlightsRenderer highlightsMovingChunk in this.m_highlightsMovingChunks)
                  this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer(highlightsMovingChunk);
              }
            }
            return new RenderStats();
          }
          RenderStats renderStats = new RenderStats();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> normalChunk in this.m_normalChunks)
          {
            if ((UnityEngine.Object) normalChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += normalChunk.Render(this.m_bounds, lod);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> reflectedChunk in this.m_reflectedChunks)
          {
            if ((UnityEngine.Object) reflectedChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += reflectedChunk.Render(this.m_bounds, lod);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> normalBlueprintChunk in this.m_normalBlueprintChunks)
          {
            if ((UnityEngine.Object) normalBlueprintChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += normalBlueprintChunk.Render(this.m_bounds, lod);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> reflectedBlueprintChunk in this.m_reflectedBlueprintChunks)
          {
            if ((UnityEngine.Object) reflectedBlueprintChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += reflectedBlueprintChunk.Render(this.m_bounds, lod);
          }
          if (this.m_highlightsChunks.IsNotEmpty)
          {
            foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityInstanceData> highlightsChunk in this.m_highlightsChunks)
            {
              if (highlightsChunk.InstancesCount > 0)
              {
                if (!this.m_isRegisteredForHighlight)
                {
                  this.m_isRegisteredForHighlight = true;
                  this.m_parentData.m_parentRenderer.m_highlighter.AddCustomHighlightsRenderer((ICustomHighlightsRenderer) highlightsChunk);
                }
                renderStats += new RenderStats(1, highlightsChunk.InstancesCount, highlightsChunk.RenderedInstancesCount, highlightsChunk.RenderedInstancesCount * highlightsChunk.IndicesCountForLod0);
              }
              else if (this.m_isRegisteredForHighlight)
              {
                this.m_isRegisteredForHighlight = false;
                this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) highlightsChunk);
              }
            }
          }
          if (lod > this.m_maxMovingLod)
            return renderStats;
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> normalMovingChunk in this.m_normalMovingChunks)
          {
            if ((UnityEngine.Object) normalMovingChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += normalMovingChunk.Render(this.m_bounds, lod);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> reflectedMovingChunk in this.m_reflectedMovingChunks)
          {
            if ((UnityEngine.Object) reflectedMovingChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += reflectedMovingChunk.Render(this.m_bounds, lod);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> blueprintMovingChunk in this.m_normalBlueprintMovingChunks)
          {
            if ((UnityEngine.Object) blueprintMovingChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += blueprintMovingChunk.Render(this.m_bounds, lod);
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> blueprintMovingChunk in this.m_reflectedBlueprintMovingChunks)
          {
            if ((UnityEngine.Object) blueprintMovingChunk.Materials[lod] != (UnityEngine.Object) null)
              renderStats += blueprintMovingChunk.Render(this.m_bounds, lod);
          }
          if (this.m_highlightsMovingChunks.IsNotEmpty)
          {
            foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData> highlightsMovingChunk in this.m_highlightsMovingChunks)
            {
              if (highlightsMovingChunk.InstancesCount > 0)
              {
                if (!this.m_isMovingRegisteredForHighlight)
                {
                  this.m_isMovingRegisteredForHighlight = true;
                  this.m_parentData.m_parentRenderer.m_highlighter.AddCustomHighlightsRenderer((ICustomHighlightsRenderer) highlightsMovingChunk);
                }
                renderStats += new RenderStats(1, highlightsMovingChunk.InstancesCount, highlightsMovingChunk.RenderedInstancesCount, highlightsMovingChunk.RenderedInstancesCount * highlightsMovingChunk.IndicesCountForLod0);
              }
              else if (this.m_isMovingRegisteredForHighlight)
              {
                this.m_isMovingRegisteredForHighlight = false;
                this.m_parentData.m_parentRenderer.m_highlighter.RemoveCustomHighlightsRenderer((ICustomHighlightsRenderer) highlightsMovingChunk);
              }
            }
          }
          return renderStats;
        }

        public void Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

        public void NotifyWasNotRendered()
        {
        }
      }

      private sealed class AlwaysRenderChunk : 
        IRenderedChunkAlwaysVisible,
        IRenderedChunksBase,
        IDisposable
      {
        private readonly InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData m_parentData;
        private Lyst<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>> m_collapsingChunks;
        private Lyst<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>> m_reflectedCollapsingChunks;
        private Lyst<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>> m_collapsingAnimatedChunks;
        private Lyst<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>> m_reflectedCollapsingAnimatedChunks;
        private readonly Queueue<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>> m_collapsingInstances;
        private readonly Queueue<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<LayoutEntityAnimatedCollapseInstanceData>> m_collapsingAnimatedInstances;

        public string Name { get; }

        public AlwaysRenderChunk(
          InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData parentData)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.m_collapsingChunks = new Lyst<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>>();
          this.m_reflectedCollapsingChunks = new Lyst<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>>();
          this.m_collapsingAnimatedChunks = new Lyst<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>>();
          this.m_reflectedCollapsingAnimatedChunks = new Lyst<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>>();
          this.m_collapsingInstances = new Queueue<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>>();
          this.m_collapsingAnimatedInstances = new Queueue<InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<LayoutEntityAnimatedCollapseInstanceData>>();
          // ISSUE: explicit constructor call
          base.\u002Ector();
          this.m_parentData = parentData;
          this.Name = string.Format("{0} immediate", (object) this.m_parentData.LayoutEntityProto.Id);
        }

        public void Dispose()
        {
          ReloadAfterAssetUpdateManager reloadManager = this.m_parentData.m_parentRenderer.m_reloadManager;
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> collapsingChunk in this.m_collapsingChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>>(collapsingChunk);
          this.m_collapsingChunks.Clear();
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> reflectedCollapsingChunk in this.m_reflectedCollapsingChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>>(reflectedCollapsingChunk);
          this.m_reflectedCollapsingChunks.Clear();
          foreach (InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> collapsingAnimatedChunk in this.m_collapsingAnimatedChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>>(collapsingAnimatedChunk);
          this.m_collapsingAnimatedChunks.Clear();
          foreach (InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> collapsingAnimatedChunk in this.m_reflectedCollapsingAnimatedChunks)
            reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>>(collapsingAnimatedChunk);
          this.m_reflectedCollapsingAnimatedChunks.Clear();
        }

        public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
        {
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> collapsingChunk in this.m_collapsingChunks)
            info.Add(new RenderedInstancesInfo(this.Name, collapsingChunk.InstancesCount, collapsingChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> reflectedCollapsingChunk in this.m_reflectedCollapsingChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (reflected)", reflectedCollapsingChunk.InstancesCount, reflectedCollapsingChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> collapsingAnimatedChunk in this.m_collapsingAnimatedChunks)
            info.Add(new RenderedInstancesInfo(this.Name, collapsingAnimatedChunk.InstancesCount, collapsingAnimatedChunk.IndicesCountForLod0));
          foreach (InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> collapsingAnimatedChunk in this.m_reflectedCollapsingAnimatedChunks)
            info.Add(new RenderedInstancesInfo(this.Name + " (reflected)", collapsingAnimatedChunk.InstancesCount, collapsingAnimatedChunk.IndicesCountForLod0));
        }

        public void AddCollapsingInstance(
          ILayoutEntity entity,
          SimStep currentStepSinceLoad,
          ulong origId)
        {
          bool isReflected = entity.Transform.IsReflected;
          Tile3f tile = entity.Prototype.Layout.GetModelOrigin(entity.Transform) - new RelTile3f(entity.Transform.TransformMatrix.Transform(entity.Prototype.Graphics.PrefabOrigin.Xy.Vector2f).ExtendZ(entity.Prototype.Graphics.PrefabOrigin.Z));
          int num1 = 5 + (entity.Prototype.Layout.LayoutSize.Z * 5 / 10).FastSqrtSmallInt().ToIntFloored() * 10;
          int transitionEndTime = currentStepSinceLoad.Value + num1;
          GameObject prefab;
          ParticleSystem component;
          ImmutableArray<Vector3> particleEmitPositions;
          int num2;
          if (this.m_parentData.m_parentRenderer.m_assetsDb.TryGetClonedPrefab("Assets/Base/Buildings/Rendering/BuildingCollapseParticleSystem.prefab", out prefab) && prefab.TryGetComponent<ParticleSystem>(out component))
          {
            Vector3 origin = entity.CenterTile.ToCornerVector3();
            particleEmitPositions = entity.OccupiedVertices.Map<Vector3>((Func<OccupiedVertexRelative, Vector3>) (x => origin + x.RelCoord.ExtendZ(x.FromHeightRel.Value + 1).ToVector3()));
            component.Stop();
            ParticleSystem.MainModule main = component.main;
            num2 = main.startLifetime.constantMax.CeilToInt() * 10;
            main.maxParticles = 200 + (int) ((double) (particleEmitPositions.Length * 2) * (double) main.startLifetime.constantMax);
          }
          else
          {
            Log.Error(string.Format("Failed to create collapse particle system for: {0}", (object) entity));
            num2 = 0;
            component = (ParticleSystem) null;
            particleEmitPositions = ImmutableArray<Vector3>.Empty;
          }
          Lyst<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>> lyst1 = isReflected ? getOrCreateStatic(ref this.m_reflectedCollapsingChunks) : getOrCreateStatic(ref this.m_collapsingChunks);
          InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData data1 = new InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData(tile.ToVector3(), entity.Transform.Rotation, entity.Transform.IsReflected, currentStepSinceLoad.Value, transitionEndTime, entity.Prototype.Layout.LayoutSize.Z);
          ushort instanceId = lyst1[0].AddInstance(data1);
          for (int index = 1; index < lyst1.Count; ++index)
          {
            if ((int) lyst1[index].AddInstance(data1) != (int) instanceId)
              Log.Error("Renderers instanced ID incorrect!");
          }
          foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> renderer in lyst1)
            this.m_collapsingInstances.Enqueue(new InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>(renderer, instanceId, new SimStep(transitionEndTime), new SimStep(transitionEndTime + num2), component, particleEmitPositions));
          Lyst<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>> lyst2 = isReflected ? getOrCreateMoving(ref this.m_reflectedCollapsingAnimatedChunks) : getOrCreateMoving(ref this.m_collapsingAnimatedChunks);
          InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityAnimatedInstanceData animatedInstanceData = this.m_parentData.getOrCreateChunk(new Chunk256Index((ushort) ((uint) (origId >> 24) & (uint) ushort.MaxValue))).GetAnimatedInstanceData(entity, (ushort) (origId & (ulong) ushort.MaxValue));
          LayoutEntityAnimatedCollapseInstanceData data2 = new LayoutEntityAnimatedCollapseInstanceData(tile.ToVector3(), entity.Transform.Rotation, entity.Transform.IsReflected, currentStepSinceLoad.Value, transitionEndTime, entity.Prototype.Layout.LayoutSize.Z, animatedInstanceData.AnimationStartTime, animatedInstanceData.AnimationPausedAtTime, animatedInstanceData.InvAnimationLength, currentStepSinceLoad.Value);
          for (int index = 0; index < lyst2.Count; ++index)
          {
            if ((int) lyst2[index].AddInstance(data2) != (int) instanceId)
              Log.Error("Renderers instanced ID incorrect!");
          }
          foreach (InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> renderer in lyst2)
            this.m_collapsingAnimatedInstances.Enqueue(new InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<LayoutEntityAnimatedCollapseInstanceData>(renderer, instanceId, new SimStep(transitionEndTime), new SimStep(transitionEndTime + num2), component, particleEmitPositions));

          Lyst<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>> getOrCreateStatic(
            ref Lyst<InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>> chunks)
          {
            if (chunks.IsNotEmpty)
              return chunks;
            Material materialToInstantiate = isReflected ? this.m_parentData.m_parentRenderer.m_collapseReflectedMaterialForInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_collapseMaterialForInstancedRenderingShared;
            foreach (KeyValuePair<Material, Mesh[]> sharedStaticLod in (IEnumerable<KeyValuePair<Material, Mesh[]>>) this.m_parentData.m_sharedStaticLods)
            {
              Material sourceOfTextures = InstancingUtils.InstantiateMaterialAndCopyTextures(materialToInstantiate, sharedStaticLod.Key);
              InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> instance = new InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>(sharedStaticLod.Value, InstancingUtils.InstantiateMaterialAndCopyTextures(materialToInstantiate, sourceOfTextures));
              chunks.Add(instance);
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
            return chunks;
          }

          Lyst<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>> getOrCreateMoving(
            ref Lyst<InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>> chunks)
          {
            if (chunks.IsNotEmpty)
              return chunks;
            Material materialToInstantiate = isReflected ? this.m_parentData.m_parentRenderer.m_collapseReflectedMaterialForAnimatedInstancedRenderingShared : this.m_parentData.m_parentRenderer.m_collapseMaterialForAnimatedInstancedRenderingShared;
            foreach (KeyValuePair<Material, Mesh[]> sharedMovingLod in (IEnumerable<KeyValuePair<Material, Mesh[]>>) this.m_parentData.m_sharedMovingLods)
            {
              Material key = sharedMovingLod.Key;
              Material[] nonSharedMaterials = new Material[7];
              for (int lod = 0; lod < 7; ++lod)
              {
                Material nonSharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(materialToInstantiate, key);
                this.m_parentData.setupAnimatedMaterial(nonSharedMaterial, key, lod);
                nonSharedMaterials[lod] = nonSharedMaterial;
              }
              InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> instance = new InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData>(sharedMovingLod.Value, nonSharedMaterials);
              chunks.Add(instance);
              this.m_parentData.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) instance);
            }
            return chunks;
          }
        }

        public RenderStats RenderAlwaysVisible(GameTime time, Bounds bounds)
        {
          RenderStats renderStats = new RenderStats();
          if (this.m_collapsingInstances.IsNotEmpty)
          {
            while (this.m_collapsingInstances.IsNotEmpty && this.m_collapsingInstances.Peek().ParticlesFinishedAt.Value < time.SimStepsSinceLoad)
            {
              InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> collapseData = this.m_collapsingInstances.Dequeue();
              collapseData.Particles.gameObject.Destroy();
              collapseData.Renderer.RemoveInstance(collapseData.InstanceId);
            }
            foreach (InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> collapsingInstance in this.m_collapsingInstances)
              InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.updateParticles<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData>(collapsingInstance, time);
            foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> collapsingChunk in this.m_collapsingChunks)
              renderStats += collapsingChunk.Render(bounds, 0);
            foreach (InstancedMeshesRenderer<InstancedChunkBasedLayoutEntitiesRenderer.LayoutEntityCollapseInstanceData> reflectedCollapsingChunk in this.m_reflectedCollapsingChunks)
              renderStats += reflectedCollapsingChunk.Render(bounds, 0);
          }
          if (this.m_collapsingAnimatedInstances.IsNotEmpty)
          {
            while (this.m_collapsingAnimatedInstances.IsNotEmpty && this.m_collapsingAnimatedInstances.Peek().ParticlesFinishedAt.Value < time.SimStepsSinceLoad)
            {
              InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<LayoutEntityAnimatedCollapseInstanceData> collapseData = this.m_collapsingAnimatedInstances.Dequeue();
              collapseData.Particles.gameObject.Destroy();
              collapseData.Renderer.RemoveInstance(collapseData.InstanceId);
            }
            foreach (InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<LayoutEntityAnimatedCollapseInstanceData> animatedInstance in this.m_collapsingAnimatedInstances)
              InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.updateParticles<LayoutEntityAnimatedCollapseInstanceData>(animatedInstance, time);
            foreach (InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> collapsingAnimatedChunk in this.m_collapsingAnimatedChunks)
              renderStats += collapsingAnimatedChunk.Render(bounds, 0);
            foreach (InstancedMeshesRenderer<LayoutEntityAnimatedCollapseInstanceData> collapsingAnimatedChunk in this.m_reflectedCollapsingAnimatedChunks)
              renderStats += collapsingAnimatedChunk.Render(bounds, 0);
          }
          return renderStats;
        }

        private static void updateParticles<T>(
          InstancedChunkBasedLayoutEntitiesRenderer.EntityProtoRenderingData.AlwaysRenderChunk.CollapseData<T> data,
          GameTime time)
          where T : struct
        {
          ParticleSystem particles = data.Particles;
          if (!(bool) (UnityEngine.Object) particles)
            return;
          particles.main.simulationSpeed = (float) time.GameSpeedMult;
          if (time.IsGamePaused || data.CollapseFinishedAt.Value <= time.SimStepsSinceLoad)
            return;
          float num = (float) ((double) time.FrameTimeSec * (double) time.GameSpeedMult * 2.0);
          ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
          for (int index = 0; index < data.ParticleEmitPositions.Length; ++index)
          {
            if ((double) UnityEngine.Random.value <= (double) num)
            {
              emitParams.position = data.ParticleEmitPositions[index] + new Vector3((float) (((double) UnityEngine.Random.value - 0.5) * 2.0), 0.0f, (float) (((double) UnityEngine.Random.value - 0.5) * 2.0));
              Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
              onUnitSphere.y = onUnitSphere.y.Abs();
              emitParams.velocity = onUnitSphere * 8f;
              particles.Emit(emitParams, 1);
            }
          }
        }

        private readonly struct CollapseData<T> where T : struct
        {
          public readonly InstancedMeshesRenderer<T> Renderer;
          public readonly ushort InstanceId;
          public readonly SimStep CollapseFinishedAt;
          public readonly SimStep ParticlesFinishedAt;
          public readonly ParticleSystem Particles;
          public readonly ImmutableArray<Vector3> ParticleEmitPositions;

          public CollapseData(
            InstancedMeshesRenderer<T> renderer,
            ushort instanceId,
            SimStep collapseFinishedAt,
            SimStep particlesFinishedAt,
            ParticleSystem particles,
            ImmutableArray<Vector3> particleEmitPositions)
          {
            xxhJUtQyC9HnIshc6H.OukgcisAbr();
            this.InstanceId = instanceId;
            this.CollapseFinishedAt = collapseFinishedAt;
            this.ParticlesFinishedAt = particlesFinishedAt;
            this.Particles = particles;
            this.ParticleEmitPositions = particleEmitPositions;
            this.Renderer = renderer;
          }
        }
      }
    }

    [ExpectedStructSize(20)]
    internal struct LayoutEntityInstanceData
    {
      public readonly Vector3 Position;
      public readonly uint Data;
      public float EmissionIntensity;

      public LayoutEntityInstanceData(
        Vector3 position,
        Rotation90 rotation,
        bool isReflected,
        ColorRgba color,
        float emissionIntensity)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.Data = (uint) ((int) color.Rgba & -16 | rotation.AngleIndex | (isReflected ? 4 : 0));
        this.EmissionIntensity = emissionIntensity;
      }

      public bool UpdateEmissionIfNeeded(float intensity)
      {
        if (intensity.IsNear(this.EmissionIntensity))
          return false;
        this.EmissionIntensity = Mathf.Pow(intensity, 2.2f);
        return true;
      }
    }

    [ExpectedStructSize(32)]
    internal struct LayoutEntityAnimatedInstanceData
    {
      public readonly Vector3 Position;
      public readonly uint Data;
      public float AnimationStartTime;
      public float AnimationPausedAtTime;
      public float InvAnimationLength;
      public float EmissionIntensity;

      public LayoutEntityAnimatedInstanceData(
        Vector3 position,
        Rotation90 rotation,
        bool isReflected,
        ColorRgba color,
        float animationStartTime,
        float invAnimationLength,
        bool isPaused,
        int simStepsSinceLoad,
        float emissionIntensity)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.AnimationStartTime = animationStartTime;
        this.AnimationPausedAtTime = isPaused ? (float) simStepsSinceLoad / 10f : -1f;
        this.InvAnimationLength = invAnimationLength;
        this.Data = (uint) ((int) color.Rgba & -16 | rotation.AngleIndex | (isReflected ? 4 : 0));
        this.EmissionIntensity = emissionIntensity;
      }

      public bool UpdateAnimationIfNeeded(
        float baseAnimationLength,
        EntitiesAnimationsUpdater.AnimationData animationData,
        GameTime time)
      {
        Mafi.Core.Entities.AnimationState state = animationData.AnimationState.GetState();
        float num1 = 0.1f;
        float num2 = 1000f * num1;
        float num3;
        if (state.UseSpeed)
        {
          num3 = animationData.Speed;
        }
        else
        {
          float num4 = (double) this.AnimationPausedAtTime < 0.0 ? (float) ((((double) time.SimStepsSinceLoad - (double) time.GameSpeedMult) * (double) num1 - (double) this.AnimationStartTime) * (double) this.InvAnimationLength % 1.0) : (float) (((double) this.AnimationPausedAtTime - (double) this.AnimationStartTime) * (double) this.InvAnimationLength % 1.0);
          float num5 = (state.TimeMs.ToFloat() / animationData.AnimationLengthMs).Min(1f);
          float self = num5 - num4;
          if (animationData.AnimationLoops)
          {
            if ((double) self > 0.5)
              self = (float) -(1.0 - (double) self);
            else if ((double) self < -0.5)
              self = 1f + self;
          }
          if ((double) self.Abs() > 0.5)
          {
            float num6 = (num5 - num2 / animationData.AnimationLengthMs).Max(0.0f);
            float num7 = num5 - num6;
            Assert.That<float>(num7).IsNotNegative();
            this.AnimationStartTime = ((float) time.SimStepsSinceLoad - (float) time.GameSpeedMult) * num1;
            float num8 = (num7 * animationData.AnimationLengthMs / num2).Max(0.0f);
            if (time.GameSpeedMult != 0)
              num8 /= (float) time.GameSpeedMult;
            this.InvAnimationLength = num8 / baseAnimationLength;
            this.AnimationPausedAtTime = (double) num8 > 0.0 ? -1f : ((float) time.SimStepsSinceLoad - (float) time.GameSpeedMult) * num1;
            return true;
          }
          num3 = (double) self <= 0.0 ? 0.0f : self * animationData.AnimationLengthMs / num2;
        }
        if ((double) num3 == 0.0)
        {
          if ((double) this.AnimationPausedAtTime >= 0.0)
            return false;
          this.AnimationPausedAtTime = ((float) time.SimStepsSinceLoad - (float) time.GameSpeedMult) * num1;
          return true;
        }
        float num9 = num3 * (animationData.IsPlayingBackwards ? -1f : 1f);
        float num10 = this.InvAnimationLength * baseAnimationLength;
        if (time.GameSpeedMult != 0)
          num9 /= (float) time.GameSpeedMult;
        if ((double) this.AnimationPausedAtTime < 0.0 && (double) num10 != 0.0 && (double) Mathf.Abs(num10 - num9) < 0.0099999997764825821 && (double) Mathf.Abs((float) ((double) num9 / (double) num10 - 1.0)) < 0.0099999997764825821)
          return false;
        float num11 = num9 / baseAnimationLength;
        float num12 = ((float) time.SimStepsSinceLoad - (float) time.GameSpeedMult) * num1;
        this.AnimationStartTime = (double) this.AnimationPausedAtTime < 0.0 ? num12 - (num12 - this.AnimationStartTime) * this.InvAnimationLength / num11 : num12 - (this.AnimationPausedAtTime - this.AnimationStartTime) * this.InvAnimationLength / num11;
        this.InvAnimationLength = num11;
        this.AnimationPausedAtTime = -1f;
        return true;
      }

      public bool UpdateEmissionIfNeeded(float intensity)
      {
        if (intensity.IsNear(this.EmissionIntensity))
          return false;
        this.EmissionIntensity = Mathf.Pow(intensity, 2.2f);
        return true;
      }
    }

    [ExpectedStructSize(32)]
    private readonly struct LayoutEntityCollapseInstanceData
    {
      public readonly Vector3 Position;
      public readonly uint Data;
      public readonly int TransitionStartTime;
      public readonly int TransitionEndTime;
      public readonly int TransitionHeightTiles;
      public readonly float EmissionIntensity;

      public LayoutEntityCollapseInstanceData(
        Vector3 position,
        Rotation90 rotation,
        bool isReflected,
        int transitionStartTime,
        int transitionEndTime,
        int transitionHeightTiles)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.Data = (uint) (-16 | rotation.AngleIndex | (isReflected ? 4 : 0));
        this.TransitionStartTime = transitionStartTime;
        this.TransitionEndTime = transitionEndTime;
        this.TransitionHeightTiles = transitionHeightTiles;
        this.EmissionIntensity = 0.0f;
      }
    }
  }
}
