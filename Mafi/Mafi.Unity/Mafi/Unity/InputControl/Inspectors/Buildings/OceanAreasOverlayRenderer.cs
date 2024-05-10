// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.OceanAreasOverlayRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Unity.Terrain;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class OceanAreasOverlayRenderer
  {
    private static readonly Color SELECTED_AREA_COLOR;
    private static readonly Color VALID_AREA_COLOR;
    private static readonly Color INVALID_AREA_COLOR;
    public static readonly HeightTilesF OCEAN_AREA_HEIGHT;
    private readonly AssetsDb m_assetsDb;
    private readonly ActivatorState m_activatorState;
    private readonly Dict<IStaticEntityWithReservedOcean, OceanAreasOverlayRenderer.EntityRecord> m_oceanEntitiesRecords;
    private readonly Lyst<KeyValuePair<IStaticEntityWithReservedOcean, OceanAreasOverlayRenderer.EventType>> m_eventsToProcess;
    private bool m_isActivatedAll;

    public OceanAreasOverlayRenderer(
      AssetsDb assetsDb,
      IEntitiesManager entitiesManager,
      IGameLoopEvents gameLoop)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_oceanEntitiesRecords = new Dict<IStaticEntityWithReservedOcean, OceanAreasOverlayRenderer.EntityRecord>();
      this.m_eventsToProcess = new Lyst<KeyValuePair<IStaticEntityWithReservedOcean, OceanAreasOverlayRenderer.EventType>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_activatorState = new ActivatorState(new Action(this.activate), new Action(this.deactivate));
      gameLoop.SyncUpdate.AddNonSaveable<OceanAreasOverlayRenderer>(this, new Action<GameTime>(this.syncUpdate));
      gameLoop.RegisterRendererInitState((object) this, this.initState(entitiesManager));
    }

    private void syncUpdate(GameTime time)
    {
      foreach (KeyValuePair<IStaticEntityWithReservedOcean, OceanAreasOverlayRenderer.EventType> keyValuePair in this.m_eventsToProcess)
      {
        try
        {
          switch (keyValuePair.Value)
          {
            case OceanAreasOverlayRenderer.EventType.Add:
              this.addImmediate(keyValuePair.Key);
              continue;
            case OceanAreasOverlayRenderer.EventType.Remove:
              this.removeImmediate(keyValuePair.Key);
              continue;
            default:
              Log.Error(string.Format("Unhandled event type {0}.", (object) keyValuePair.Value));
              continue;
          }
        }
        catch (Exception ex)
        {
          Log.Exception(ex, string.Format("Failed to process {0} event for '{1}'.", (object) keyValuePair.Value, (object) keyValuePair.Key));
        }
      }
      this.m_eventsToProcess.Clear();
    }

    private IEnumerator<string> initState(IEntitiesManager entitiesManager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new OceanAreasOverlayRenderer.\u003CinitState\u003Ed__12(0)
      {
        \u003C\u003E4__this = this,
        entitiesManager = entitiesManager
      };
    }

    private void addImmediate(IStaticEntityWithReservedOcean entity)
    {
      if (this.m_oceanEntitiesRecords.ContainsKey(entity))
      {
        Log.Warning(string.Format("Entity '{0}' is already tracked for ocean areas.", (object) entity));
      }
      else
      {
        OceanAreasOverlayRenderer.EntityRecord entityRecord = new OceanAreasOverlayRenderer.EntityRecord(entity, this.m_assetsDb);
        this.m_oceanEntitiesRecords.Add(entity, entityRecord);
        if (!this.m_isActivatedAll)
          return;
        entityRecord.ActivatorState.ActivateDirect();
      }
    }

    private void removeImmediate(IStaticEntityWithReservedOcean entity)
    {
      OceanAreasOverlayRenderer.EntityRecord entityRecord;
      if (!this.m_oceanEntitiesRecords.TryRemove(entity, out entityRecord))
        return;
      entityRecord.ActivatorState.Destroy();
    }

    public IActivator CreateActivator() => this.m_activatorState.CreateActivator();

    public IActivator CreateActivatorSingleEntity(IStaticEntityWithReservedOcean entity)
    {
      OceanAreasOverlayRenderer.EntityRecord entityRecord;
      if (this.m_oceanEntitiesRecords.TryGetValue(entity, out entityRecord))
        return entityRecord.ActivatorState.CreateActivator();
      Log.Error("Entity for ocean activation was not found.");
      return this.CreateActivator();
    }

    /// <summary>
    /// Activates for single entity. Every call must be later followed by <see cref="M:Mafi.Unity.InputControl.Inspectors.Buildings.OceanAreasOverlayRenderer.DeactivateForSingleEntity(Mafi.Option{Mafi.Core.Entities.Static.IStaticEntityWithReservedOcean})" />.
    /// 
    /// Note that due to delayed entity addition, areas of newly added entities can be activated only after sync.
    /// 
    /// Example:
    /// <code>
    /// // To activate:
    /// m_oceanActivatedForEntity = m_oceanAreasRenderer.ActivateForSingleEntity(entity);
    /// ...
    /// // To deactivate:
    /// m_oceanActivatedForEntity = m_oceanAreasRenderer.DeactivateForSingleEntity(m_oceanActivatedForEntity);
    /// </code>
    /// </summary>
    public Option<IStaticEntityWithReservedOcean> ActivateForSingleEntity(
      IStaticEntityWithReservedOcean entity)
    {
      OceanAreasOverlayRenderer.EntityRecord entityRecord;
      if (this.m_oceanEntitiesRecords.TryGetValue(entity, out entityRecord))
      {
        entityRecord.ActivatorState.ActivateDirect();
        return entity.SomeOption<IStaticEntityWithReservedOcean>();
      }
      Log.Warning("Entity for ocean activation was not found. Activated too soon?");
      return Option<IStaticEntityWithReservedOcean>.None;
    }

    public Option<IStaticEntityWithReservedOcean> DeactivateForSingleEntity(
      Option<IStaticEntityWithReservedOcean> entity)
    {
      if (entity.IsNone)
        return Option<IStaticEntityWithReservedOcean>.None;
      OceanAreasOverlayRenderer.EntityRecord entityRecord;
      if (this.m_oceanEntitiesRecords.TryGetValue(entity.Value, out entityRecord))
      {
        entityRecord.ActivatorState.DeactivateDirect();
        return Option<IStaticEntityWithReservedOcean>.None;
      }
      Log.Warning("Entity for ocean activation was not found.");
      return Option<IStaticEntityWithReservedOcean>.None;
    }

    private void activate()
    {
      Assert.That<bool>(this.m_isActivatedAll).IsFalse();
      this.m_isActivatedAll = true;
      foreach (OceanAreasOverlayRenderer.EntityRecord entityRecord in this.m_oceanEntitiesRecords.Values)
        entityRecord.ActivatorState.ActivateDirect();
    }

    private void deactivate()
    {
      this.m_isActivatedAll = false;
      foreach (OceanAreasOverlayRenderer.EntityRecord entityRecord in this.m_oceanEntitiesRecords.Values)
        entityRecord.ActivatorState.DeactivateDirect();
    }

    static OceanAreasOverlayRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      OceanAreasOverlayRenderer.SELECTED_AREA_COLOR = Color.green;
      OceanAreasOverlayRenderer.VALID_AREA_COLOR = Color.green / 2f;
      OceanAreasOverlayRenderer.INVALID_AREA_COLOR = Color.red / 2f;
      OceanAreasOverlayRenderer.OCEAN_AREA_HEIGHT = HeightTilesF.One;
    }

    private enum EventType
    {
      Add,
      Remove,
    }

    private class EntityRecord
    {
      public readonly ActivatorState ActivatorState;
      private readonly ReservedOceanAreaState m_oceanAreasState;
      private readonly TerrainAreaRenderer[] m_areaRenderers;

      public EntityRecord(IStaticEntityWithReservedOcean entity, AssetsDb assetsDb)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.ActivatorState = new ActivatorState(new Action(this.activate), new Action(this.deactivate));
        this.m_oceanAreasState = entity.ReservedOceanAreaState;
        this.m_areaRenderers = new TerrainAreaRenderer[this.m_oceanAreasState.AreasSets.Length];
        for (int index = 0; index < this.m_areaRenderers.Length; ++index)
          this.m_areaRenderers[index] = new TerrainAreaRenderer(assetsDb);
      }

      private void activate()
      {
        for (int index = 0; index < this.m_oceanAreasState.AreasSets.Length; ++index)
        {
          float stripesScale;
          float stripesAngle;
          ProtoWithReservedOceanValidator.GetStripesScaleAndAngle(index, out stripesScale, out stripesAngle);
          Color color = (this.m_oceanAreasState.AreasSetsValidity[index] == 0U ? (this.m_oceanAreasState.FirstValidAreasSetIndex == index ? OceanAreasOverlayRenderer.SELECTED_AREA_COLOR : OceanAreasOverlayRenderer.VALID_AREA_COLOR) : OceanAreasOverlayRenderer.INVALID_AREA_COLOR) with
          {
            a = 0.1f
          };
          ThicknessTilesF thicknessTilesF = new ThicknessTilesF((UnityEngine.Random.value * 0.05f).ToFix32());
          this.m_areaRenderers[index].ShowAreas(this.m_oceanAreasState.AreasSets[index].AsEnumerable(), color, OceanAreasOverlayRenderer.OCEAN_AREA_HEIGHT + thicknessTilesF, stripesScale, stripesAngle);
        }
      }

      private void deactivate()
      {
        foreach (TerrainAreaRenderer areaRenderer in this.m_areaRenderers)
          areaRenderer.Hide();
      }
    }
  }
}
