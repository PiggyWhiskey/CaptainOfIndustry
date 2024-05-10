// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.MbBasedEntitiesRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Unity.Entities.Static;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>Default renderer that renders entities using MBs.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class MbBasedEntitiesRenderer : DelayedEntitiesRenderer
  {
    private readonly EntityMbFactory m_entityMbFactory;
    private readonly EntitiesIconRenderer m_entitiesIconRenderer;
    private readonly AssetsDb m_assetsDb;
    private readonly ObjectHighlighter m_highlighter;
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
    /// <summary>
    /// Maps <see cref="T:UnityEngine.GameObject" /> to <see cref="T:Mafi.Core.Entities.IEntity" />.
    /// </summary>
    private readonly Dict<GameObject, IRenderedEntity> m_goToEntity;
    private readonly List<IDestroyableEntityMb> m_destroyableMbsTmp;
    private readonly ImmutableArray<IEntityMbUpdater> m_updaters;
    private readonly ImmutableArray<IEntityMbInitializer> m_initializers;
    private readonly Lyst<MbBasedEntitiesRenderer.CollapsingEntity> m_collapsingEntities;
    private readonly Dict<ulong, ObjectHighlightSpec> m_activeHighlights;
    private ulong m_lastUsedHighlightId;
    private readonly Material m_blueprintMaterialShared;

    public override int Priority => int.MaxValue;

    public IReadOnlyDictionary<IRenderedEntity, EntityMb> RenderedEntities
    {
      get => (IReadOnlyDictionary<IRenderedEntity, EntityMb>) this.m_entities;
    }

    public MbBasedEntitiesRenderer(
      EntitiesRenderingManager entitiesRenderingManager,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      EntityMbFactory entityMbFactory,
      AllImplementationsOf<IEntityMbUpdater> updaters,
      AllImplementationsOf<IEntityMbInitializer> initializers,
      AssetsDb assetsDb,
      ObjectHighlighter highlighter,
      EntitiesIconRenderer entitiesIconRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_parentGo = new GameObject("Entities");
      this.m_entities = new Dict<IRenderedEntity, EntityMb>();
      this.m_entitiesToRenderUpdate = new Lyst<IEntityMbWithRenderUpdate>();
      this.m_entitiesToSyncUpdate = new Lyst<IEntityMbWithSyncUpdate>();
      this.m_entitiesToSimEndUpdate = new Lyst<IEntityMbWithSimUpdateEnd>();
      this.m_goToEntity = new Dict<GameObject, IRenderedEntity>();
      this.m_destroyableMbsTmp = new List<IDestroyableEntityMb>();
      this.m_collapsingEntities = new Lyst<MbBasedEntitiesRenderer.CollapsingEntity>();
      this.m_activeHighlights = new Dict<ulong, ObjectHighlightSpec>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityMbFactory = entityMbFactory.CheckNotNull<EntityMbFactory>();
      this.m_assetsDb = assetsDb;
      this.m_highlighter = highlighter;
      this.m_entitiesIconRenderer = entitiesIconRenderer;
      this.m_updaters = updaters.Implementations;
      this.m_initializers = initializers.Implementations;
      this.m_blueprintMaterialShared = assetsDb.GetSharedMaterial("Assets/Core/Materials/BuildingBlueprint.mat");
      entitiesRenderingManager.RegisterRenderer((IEntitiesRenderer) this);
      gameLoopEvents.Terminate.AddNonSaveable<MbBasedEntitiesRenderer>(this, new Action(this.onTerminated));
      gameLoopEvents.RenderUpdate.AddNonSaveable<MbBasedEntitiesRenderer>(this, new Action<GameTime>(this.renderUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<MbBasedEntitiesRenderer>(this, new Action(this.updateSimEnd));
    }

    public override bool CanRenderEntity(EntityProto proto) => true;

    public override void AddEntityOnSync(IRenderedEntity entity, GameTime time)
    {
      if (entity == null)
        Log.Warning("Adding null entity in `AddEntityOnSync`, ignoring.");
      else if (entity.IsDestroyed)
      {
        Log.Warning(string.Format("Adding destroyed entity {0} in `AddEntityOnSync`, ignoring.", (object) entity));
      }
      else
      {
        EntityMb mbFor = this.m_entityMbFactory.CreateMbFor<IRenderedEntity>(entity);
        if (!(bool) (UnityEngine.Object) mbFor)
        {
          Log.Warning(string.Format("Failed to create entity MB for '{0}'.", (object) entity));
        }
        else
        {
          this.m_entities.AddAndAssertNew(entity, mbFor);
          this.m_goToEntity.AddAndAssertNew(mbFor.gameObject, entity);
          foreach (IEntityMbInitializer initializer in this.m_initializers)
            initializer.InitIfNeeded(mbFor);
          if (!mbFor.IsInitialized)
          {
            Assert.Fail(string.Format("Adding non-initialized mb for '{0}' {1}.", (object) entity, (object) mbFor.GetType()));
          }
          else
          {
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
            this.m_entitiesIconRenderer.UpdateVisualsImmediate(entity);
          }
        }
      }
    }

    public override void UpdateEntityOnSync(IRenderedEntity entity, GameTime time)
    {
      this.m_entitiesIconRenderer.UpdateVisualsImmediate(entity);
      EntityMb entityMb;
      if (!this.m_entities.TryGetValue(entity, out entityMb))
        Log.Error(string.Format("Failed to update entity '{0}', its MB was not found.", (object) entity));
      else if (entity is StaticEntity staticEntity)
        this.updateBlueprintVisual(staticEntity, entityMb);
      else
        Log.Warning(string.Format("Updating of entity '{0}' not supported.", (object) entity));
    }

    public override void RemoveEntityOnSync(
      IRenderedEntity entity,
      GameTime time,
      EntityRemoveReason reason)
    {
      this.m_entitiesIconRenderer.UpdateVisualsImmediate(entity);
      EntityMb entityMb;
      if (!this.m_entities.TryRemove(entity, out entityMb))
      {
        Log.Warning(string.Format("Trying to remove non-existent entity '{0}' from renderer, ignoring.", (object) entity.Id));
      }
      else
      {
        if (entityMb is IEntityMbWithRenderUpdate withRenderUpdate)
          this.m_entitiesToRenderUpdate.Remove(withRenderUpdate);
        if (entityMb is IEntityMbWithSyncUpdate mbWithSyncUpdate)
          this.m_entitiesToSyncUpdate.Remove(mbWithSyncUpdate);
        if (entityMb is IEntityMbWithSimUpdateEnd withSimUpdateEnd)
          this.m_entitiesToSimEndUpdate.Remove(withSimUpdateEnd);
        foreach (IEntityMbUpdater updater in this.m_updaters)
          updater.RemoveOnSyncIfNeeded(entityMb);
        this.m_goToEntity.RemoveAndAssert(entityMb.gameObject);
        if (reason == EntityRemoveReason.Collapse)
          this.m_collapsingEntities.Add(new MbBasedEntitiesRenderer.CollapsingEntity(this, entity, entityMb));
        else
          this.destroyMbs(entityMb);
      }
    }

    public override bool TryGetPickableEntityAs<T>(GameObject pickedGo, out T entity)
    {
      IRenderedEntity renderedEntity;
      if (this.m_goToEntity.TryGetValue(pickedGo, out renderedEntity))
      {
        entity = renderedEntity as T;
        return (object) entity != null;
      }
      entity = default (T);
      return false;
    }

    private void updateBlueprintVisual(StaticEntity staticEntity, EntityMb entityMb)
    {
      if (entityMb is StaticEntityMb staticEntityMb)
      {
        ColorRgba color;
        if (InstancedChunkBasedLayoutEntitiesRenderer.GetBlueprintColor((IStaticEntity) staticEntity, out color))
          staticEntityMb.EnsureBlueprintMaterial(this.m_blueprintMaterialShared, color);
        else
          staticEntityMb.EnsureDefaultMaterial();
      }
      else
        Log.Error(string.Format("Entity '{0}' does not have {1}.", (object) staticEntity, (object) "StaticEntityMb"));
    }

    public override ulong AddHighlight(IRenderedEntity entity, ColorRgba color)
    {
      EntityMb entityMb;
      if (!this.TryGetMbFor(entity, out entityMb))
        return 0;
      ObjectHighlightSpec highlightSpec = new ObjectHighlightSpec(entityMb.gameObject, color);
      this.m_highlighter.Highlight(highlightSpec);
      ++this.m_lastUsedHighlightId;
      this.m_activeHighlights.Add(this.m_lastUsedHighlightId, highlightSpec);
      return this.m_lastUsedHighlightId;
    }

    public override void RemoveHighlight(ulong highlightId)
    {
      ObjectHighlightSpec highlightSpec;
      if (!this.m_activeHighlights.TryRemove(highlightId, out highlightSpec))
        return;
      this.m_highlighter.RemoveHighlight(highlightSpec);
      if (this.m_activeHighlights.Count != 0)
        return;
      this.m_lastUsedHighlightId = 0UL;
    }

    public override void SyncUpdate(GameTime time)
    {
      base.SyncUpdate(time);
      foreach (IEntityMbWithSyncUpdate mbWithSyncUpdate in this.m_entitiesToSyncUpdate)
      {
        if (mbWithSyncUpdate != null)
        {
          if (!mbWithSyncUpdate.IsDestroyed)
          {
            try
            {
              mbWithSyncUpdate.SyncUpdate(time);
              continue;
            }
            catch (Exception ex)
            {
              Log.Exception(ex, "Exception thrown in entity syncUpdate.");
              continue;
            }
          }
        }
        Log.Error(string.Format("Destroyed entity '{0}' present in `m_entitiesToSyncUpdate`.", (object) mbWithSyncUpdate));
      }
      foreach (IEntityMbUpdater updater in this.m_updaters)
        updater.SyncUpdate(time);
      if (!this.m_collapsingEntities.IsNotEmpty)
        return;
      foreach (MbBasedEntitiesRenderer.CollapsingEntity collapsingEntity in this.m_collapsingEntities)
      {
        collapsingEntity.SyncUpdate(time);
        if (collapsingEntity.IsDoneCollapsing)
          this.destroyMbs(collapsingEntity.EntityMb);
      }
      this.m_collapsingEntities.RemoveWhere((Predicate<MbBasedEntitiesRenderer.CollapsingEntity>) (x => x.IsDoneCollapsing));
    }

    private void renderUpdate(GameTime time)
    {
      foreach (IEntityMbWithRenderUpdate withRenderUpdate in this.m_entitiesToRenderUpdate)
      {
        if (withRenderUpdate == null || withRenderUpdate.IsDestroyed)
          Log.Error("Destroyed entity '{entity}' present in `m_entitiesToRenderUpdate`.");
        else
          withRenderUpdate.RenderUpdate(time);
      }
      foreach (IEntityMbUpdater updater in this.m_updaters)
        updater.RenderUpdate(time);
      if (!this.m_collapsingEntities.IsNotEmpty)
        return;
      foreach (MbBasedEntitiesRenderer.CollapsingEntity collapsingEntity in this.m_collapsingEntities)
        collapsingEntity.RenderUpdate(time);
    }

    private void updateSimEnd()
    {
      foreach (IEntityMbWithSimUpdateEnd withSimUpdateEnd in this.m_entitiesToSimEndUpdate)
      {
        Assert.That<bool>(withSimUpdateEnd.IsDestroyed).IsFalse();
        withSimUpdateEnd.SimUpdateEnd();
      }
    }

    public Option<EntityMb> GetMbFor(IRenderedEntity entity)
    {
      return this.m_entities.Get<IRenderedEntity, EntityMb>(entity);
    }

    public bool TryGetMbFor(IRenderedEntity entity, out EntityMb entityMb)
    {
      return this.m_entities.TryGetValue(entity, out entityMb);
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

    private void onTerminated()
    {
      foreach (EntityMb entityMb in this.m_entities.Values)
        this.destroyMbs(entityMb);
    }

    public class CollapsingEntity
    {
      public const float PARTICLE_MAX_VELOCITY = 8f;
      public const int PARTICLES_PER_VERTEX_PER_SECOND = 2;
      public readonly EntityMb EntityMb;
      private readonly Transform m_transform;
      private readonly Vector3 m_originalPosition;
      private readonly float m_maxHeightTraveled;
      private readonly float m_maxExtraDurationSec;
      private readonly Option<ParticleSystem> m_particleSystem;
      private readonly ImmutableArray<Vector3> m_particleEmitPositions;
      private float m_deltaHeight;
      private float m_deltaHeightNext;
      private float m_speedPerSecond;
      private float m_extraDurationSec;

      public bool IsDoneCollapsing
      {
        get
        {
          return (double) this.m_extraDurationSec > (double) this.m_maxExtraDurationSec && (double) this.m_deltaHeight > (double) this.m_maxHeightTraveled;
        }
      }

      public CollapsingEntity(
        MbBasedEntitiesRenderer patentRenderer,
        IRenderedEntity renderedEntity,
        EntityMb entityMb)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.EntityMb = entityMb;
        this.m_transform = entityMb.gameObject.transform;
        this.m_originalPosition = this.m_transform.position;
        this.m_deltaHeight = this.m_deltaHeightNext = 0.0f;
        GameObject prefab;
        if (patentRenderer.m_assetsDb.TryGetClonedPrefab("Assets/Base/Buildings/Rendering/BuildingCollapseParticleSystem.prefab", out prefab))
        {
          ParticleSystem component = prefab.GetComponent<ParticleSystem>();
          if ((bool) (UnityEngine.Object) component)
          {
            this.m_particleSystem = (Option<ParticleSystem>) component;
            component.Stop();
            component.transform.SetParent(entityMb.transform, false);
          }
        }
        if (renderedEntity is IStaticEntity staticEntity)
        {
          this.m_maxHeightTraveled = staticEntity.OccupiedTiles.Max<ThicknessTilesI>((Func<OccupiedTileRelative, ThicknessTilesI>) (x => x.ToHeightRelExcl)).ToUnityUnits() + 10f;
          Vector3 origin = staticEntity.CenterTile.ToCornerVector3();
          if (!this.m_particleSystem.HasValue)
            return;
          this.m_particleEmitPositions = staticEntity.OccupiedVertices.Map<Vector3>((Func<OccupiedVertexRelative, Vector3>) (x => origin + x.RelCoord.ExtendZ(x.FromHeightRel.Value + 1).ToVector3()));
          ParticleSystem.MainModule main = this.m_particleSystem.Value.main;
          this.m_maxExtraDurationSec = main.startLifetime.constantMax;
          main.maxParticles = 200 + (int) ((double) (this.m_particleEmitPositions.Length * 2) * (double) main.startLifetime.constantMax);
        }
        else
        {
          this.m_maxHeightTraveled = 20f;
          this.m_maxExtraDurationSec = 0.0f;
          Log.Warning("Something other than static entity is collapsing.");
        }
      }

      public void SyncUpdate(GameTime time)
      {
        if (time.IsGamePaused)
          return;
        if ((double) this.m_deltaHeight < (double) this.m_maxHeightTraveled)
          this.m_speedPerSecond += 0.14f;
        else
          this.m_extraDurationSec += 0.1f;
        this.m_deltaHeight = this.m_deltaHeightNext;
        this.m_deltaHeightNext += this.m_speedPerSecond;
      }

      public void RenderUpdate(GameTime time)
      {
        if (this.m_particleSystem.HasValue)
          this.m_particleSystem.Value.main.simulationSpeed = (float) time.GameSpeedMult;
        if (time.IsGamePaused)
          return;
        float num1 = this.m_deltaHeight.Lerp(this.m_deltaHeightNext, time.AbsoluteT);
        float num2 = 0.4f * (num1 / 4f).Min(1f);
        this.m_transform.position = this.m_originalPosition + new Vector3(num2 * (UnityEngine.Random.value - 0.5f), num2 * (UnityEngine.Random.value - 0.5f) - num1, num2 * (UnityEngine.Random.value - 0.5f));
        if (!this.m_particleSystem.HasValue || (double) this.m_deltaHeight >= (double) this.m_maxHeightTraveled)
          return;
        float num3 = (float) ((double) time.DeltaT * (double) time.GameSpeedMult * 2.0);
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        foreach (Vector3 particleEmitPosition in this.m_particleEmitPositions)
        {
          if ((double) UnityEngine.Random.value <= (double) num3)
          {
            emitParams.position = particleEmitPosition + new Vector3((float) (((double) UnityEngine.Random.value - 0.5) * 2.0), 0.0f, (float) (((double) UnityEngine.Random.value - 0.5) * 2.0));
            Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
            onUnitSphere.y = onUnitSphere.y.Abs();
            emitParams.velocity = onUnitSphere * 8f;
            this.m_particleSystem.Value.Emit(emitParams, 1);
          }
        }
      }
    }
  }
}
