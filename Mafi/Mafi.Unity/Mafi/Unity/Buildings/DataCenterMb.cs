// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.DataCenterMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Datacenters;
using Mafi.Core.Prototypes;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  internal class DataCenterMb : 
    StaticEntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private IRandom m_random;
    private AssetsDb m_assetsDb;
    private DataCenter m_dataCenter;
    private Option<DataCenterMb.RackVisual>[] m_racks;
    private float m_nextBlinkTimeMs;
    /// <summary>
    /// Queue of rack changes. Rack changes are added in the simulation thread, so this may not be touched by the UI
    /// thread. As the UI thread should be invoked more often than the simulation thread, we expect to have at most
    /// one element in this queue under normal conditions.
    /// </summary>
    private Lyst<DataCenterMb.RackChange> m_rackChangesSync;
    private bool m_isWorking;
    /// <summary>
    /// List of rack changes to be processed by <see cref="!:RenderUpdate" />.
    /// </summary>
    private Lyst<DataCenterMb.RackChange> m_rackChanges;

    public void Initialize(DataCenter dataCenter, AssetsDb assetsDb, IRandom random)
    {
      this.Initialize((ILayoutEntity) dataCenter);
      this.m_dataCenter = dataCenter;
      this.m_random = random;
      this.m_assetsDb = assetsDb;
      this.m_racks = new Option<DataCenterMb.RackVisual>[dataCenter.Prototype.RacksCapacity];
      int positionIndex = 0;
      foreach (KeyValuePair<ServerRackProto, int> rackCount in this.m_dataCenter.GetRackCounts())
      {
        ServerRackProto key = rackCount.Key;
        int num = rackCount.Value;
        for (int index = 0; index < num; ++index)
        {
          if (positionIndex >= this.m_racks.Length)
          {
            Log.Error(string.Format("Too many racks in datacenter, max was {0}, ", (object) this.m_racks.Length) + string.Format("trying to add {0}", (object) this.m_dataCenter.GetRackCounts().Sum<KeyValuePair<ServerRackProto, int>>((Func<KeyValuePair<ServerRackProto, int>, int>) (x => x.Value))));
            break;
          }
          this.m_racks[positionIndex] = (Option<DataCenterMb.RackVisual>) this.createRackVisual(key, positionIndex);
          ++positionIndex;
        }
      }
      this.m_dataCenter.RackAdded += new Action<ServerRackProto>(this.rackAdded);
      this.m_dataCenter.RackRemoved += new Action<ServerRackProto>(this.rackRemoved);
    }

    /// <summary>
    /// Called by a datacenter in sim thread after a rack is added. Can touch only m_rackChangesSyncQueue.
    /// </summary>
    private void rackAdded(ServerRackProto rackProto)
    {
      this.m_rackChangesSync.Add(new DataCenterMb.RackChange(rackProto, true));
    }

    /// <summary>
    /// Called by a datacenter in sim thread after a rack is removed. Can touch only m_rackChangesSyncQueue.
    /// </summary>
    private void rackRemoved(ServerRackProto rackProto)
    {
      this.m_rackChangesSync.Add(new DataCenterMb.RackChange(rackProto, false));
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      Swap.Them<Lyst<DataCenterMb.RackChange>>(ref this.m_rackChanges, ref this.m_rackChangesSync);
      this.m_isWorking = this.m_dataCenter.CurrentState == DataCenter.State.Working;
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      foreach (DataCenterMb.RackChange rackChange in this.m_rackChanges)
      {
        if (rackChange.Added)
        {
          int freeSlotIndex = this.getFreeSlotIndex();
          this.m_racks[freeSlotIndex] = (Option<DataCenterMb.RackVisual>) this.createRackVisual(rackChange.RackPrototype, freeSlotIndex);
        }
        else
          this.removeRack(rackChange.RackPrototype);
      }
      this.m_rackChanges.Clear();
      if (this.m_isWorking)
        return;
      this.m_nextBlinkTimeMs -= time.DeltaTimeMs;
      if ((double) this.m_nextBlinkTimeMs >= 0.0)
        return;
      foreach (Option<DataCenterMb.RackVisual> rack in this.m_racks)
      {
        if (rack.HasValue)
          rack.Value.ActivateNextFrontPanel();
      }
      this.m_nextBlinkTimeMs = (float) (300 + this.m_random.NextInt(100));
    }

    private int getFreeSlotIndex()
    {
      for (int freeSlotIndex = 0; freeSlotIndex < this.m_racks.Length; ++freeSlotIndex)
      {
        if (this.m_racks[freeSlotIndex].IsNone)
          return freeSlotIndex;
      }
      Assert.Fail("Free rack slot not found.");
      return 0;
    }

    private void removeRack(ServerRackProto rackProto)
    {
      for (int index = this.m_racks.Length - 1; index >= 0; --index)
      {
        Option<DataCenterMb.RackVisual> rack = this.m_racks[index];
        if (rack.HasValue && (Proto) rack.Value.Prototype == (Proto) rackProto)
        {
          rack.Value.RackGo.Destroy();
          this.m_racks[index] = (Option<DataCenterMb.RackVisual>) Option.None;
          return;
        }
      }
      Assert.Fail("Rack to be removed not found.");
    }

    private DataCenterMb.RackVisual createRackVisual(ServerRackProto rackProto, int positionIndex)
    {
      GameObject clonedPrefabOrEmptyGo = this.m_assetsDb.GetClonedPrefabOrEmptyGo(rackProto.Graphics.PrefabPath);
      clonedPrefabOrEmptyGo.transform.SetParent(this.transform, false);
      DataCenterProto.RackPosition rackPosition = this.m_dataCenter.Prototype.Graphics.RackPositions.Length > positionIndex ? this.m_dataCenter.Prototype.Graphics.RackPositions[positionIndex] : DataCenterProto.RackPosition.Empty;
      clonedPrefabOrEmptyGo.transform.localPosition = rackPosition.Position.Vector2f.ExtendZ((Fix32) 0).ToVector3();
      clonedPrefabOrEmptyGo.transform.rotation = Quaternion.AngleAxis((this.m_dataCenter.Transform.Rotation.Angle + rackPosition.Rotation.Angle).ToUnityAngleDegrees(), Vector3.up);
      return new DataCenterMb.RackVisual(rackProto, clonedPrefabOrEmptyGo, this.m_random);
    }

    public DataCenterMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_rackChangesSync = new Lyst<DataCenterMb.RackChange>();
      this.m_rackChanges = new Lyst<DataCenterMb.RackChange>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private struct RackChange
    {
      public readonly ServerRackProto RackPrototype;
      public readonly bool Added;

      public RackChange(ServerRackProto rackProto, bool added)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.RackPrototype = rackProto.CheckNotNull<ServerRackProto>();
        this.Added = added;
      }
    }

    private class RackVisual
    {
      public readonly ServerRackProto Prototype;
      public readonly GameObject RackGo;
      public readonly ImmutableArray<GameObject> FrontPanelGos;
      private IRandom m_random;
      private int m_activeFrontIndex;

      public RackVisual(ServerRackProto rackProto, GameObject rackGo, IRandom random)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Prototype = rackProto;
        this.RackGo = rackGo;
        this.m_random = random;
        int length = rackProto.Graphics.FrontPanels.Length;
        ImmutableArrayBuilder<GameObject> immutableArrayBuilder = new ImmutableArrayBuilder<GameObject>(length);
        for (int index = 0; index < length; ++index)
        {
          Transform transform = rackGo.transform.Find(rackProto.Graphics.FrontPanels[index]);
          if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
          {
            immutableArrayBuilder[index] = new GameObject("Placeholder for missing front panel.");
            Assert.Fail(string.Format("Rack front panel path invalid: {0}, rack proto: {1}", (object) rackProto.Graphics.FrontPanels[index], (object) rackProto.Strings.Name));
          }
          else
            immutableArrayBuilder[index] = transform.gameObject;
        }
        this.FrontPanelGos = immutableArrayBuilder.GetImmutableArrayAndClear();
        this.m_activeFrontIndex = random.NextInt(this.FrontPanelGos.Length);
        for (int index = 0; index < this.FrontPanelGos.Length; ++index)
        {
          if (index != this.m_activeFrontIndex)
            this.FrontPanelGos[index].SetActive(false);
        }
      }

      public void ActivateNextFrontPanel()
      {
        this.FrontPanelGos[this.m_activeFrontIndex].SetActive(false);
        this.m_activeFrontIndex = this.m_random.NextInt(this.FrontPanelGos.Length);
        this.FrontPanelGos[this.m_activeFrontIndex].SetActive(true);
      }
    }
  }
}
