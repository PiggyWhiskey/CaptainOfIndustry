// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntitiesIconRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Gfx;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntitiesIconRenderer
  {
    private readonly AssetsDb m_assetsDb;
    private readonly LazyResolve<MbBasedEntitiesRenderer> m_entitiesRenderer;
    private readonly Material m_baseIconMaterialTemplate;
    private readonly Dict<IconSpec, Material> m_iconMaterials;
    private readonly Dict<IRenderedEntity, Dict<IconSpec, EntitiesIconRenderer.IconRecord>> m_icons;
    private readonly GameObjectPool m_iconsGoPool;
    private Lyst<EntitiesIconRenderer.IconRequest> m_pendingRequests;
    private Lyst<EntitiesIconRenderer.IconRequest> m_delayedPendingRequests;
    private Dict<IconSpec, EntitiesIconRenderer.IconRecord> m_iconsTemp;
    private LystStruct<GameObject> m_temporarilyHiddenGOs;

    public bool AllIconsVisible { get; private set; }

    public EntitiesIconRenderer(
      AssetsDb assetsDb,
      LazyResolve<MbBasedEntitiesRenderer> entitiesRenderer,
      IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: reference to a compiler-generated field
      this.\u003CAllIconsVisible\u003Ek__BackingField = true;
      this.m_iconMaterials = new Dict<IconSpec, Material>();
      this.m_icons = new Dict<IRenderedEntity, Dict<IconSpec, EntitiesIconRenderer.IconRecord>>();
      this.m_pendingRequests = new Lyst<EntitiesIconRenderer.IconRequest>();
      this.m_delayedPendingRequests = new Lyst<EntitiesIconRenderer.IconRequest>();
      this.m_iconsTemp = new Dict<IconSpec, EntitiesIconRenderer.IconRecord>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_entitiesRenderer = entitiesRenderer;
      this.m_baseIconMaterialTemplate = assetsDb.GetSharedPrefabOrThrow("Assets/Core/IconOverlay/IconBillboardFixed.prefab").GetComponent<MeshRenderer>().sharedMaterial;
      this.m_iconsGoPool = new GameObjectPool("Entity icons", 16, (Func<GameObject>) (() =>
      {
        GameObject clonedPrefabOrEmptyGo = assetsDb.GetClonedPrefabOrEmptyGo("Assets/Core/IconOverlay/IconBillboardFixed.prefab");
        clonedPrefabOrEmptyGo.name = "EntityIcon";
        clonedPrefabOrEmptyGo.layer = Layer.Custom13Icons.ToId();
        return clonedPrefabOrEmptyGo;
      }), (Action<GameObject>) (x => { }));
      gameLoopEvents.SyncUpdate.AddNonSaveable<EntitiesIconRenderer>(this, new Action<GameTime>(this.sync));
    }

    private void sync(GameTime time)
    {
      foreach (EntitiesIconRenderer.IconRequest delayedPendingRequest in this.m_delayedPendingRequests)
      {
        if (delayedPendingRequest.Type == EntitiesIconRenderer.RequestType.Add)
          this.addIconImmediate(delayedPendingRequest.IconSpec, delayedPendingRequest.Entity);
        else if (delayedPendingRequest.Type == EntitiesIconRenderer.RequestType.Remove)
        {
          if (delayedPendingRequest.IconSpec.Path == "REMOVE ALL")
          {
            this.removeAllIconsOfImmediate(delayedPendingRequest.Entity);
          }
          else
          {
            this.removeIconImmediate(delayedPendingRequest.IconSpec, delayedPendingRequest.Entity);
            this.UpdateVisualsImmediate(delayedPendingRequest.Entity);
          }
        }
        else if (delayedPendingRequest.Type == EntitiesIconRenderer.RequestType.Show)
          this.SetIconVisibility(delayedPendingRequest.IconSpec, delayedPendingRequest.Entity, true);
        else if (delayedPendingRequest.Type == EntitiesIconRenderer.RequestType.Hide)
          this.SetIconVisibility(delayedPendingRequest.IconSpec, delayedPendingRequest.Entity, false);
      }
      this.m_delayedPendingRequests.Clear();
      Swap.Them<Lyst<EntitiesIconRenderer.IconRequest>>(ref this.m_pendingRequests, ref this.m_delayedPendingRequests);
    }

    public void UpdateVisualsImmediate(IRenderedEntity entity)
    {
      Dict<IconSpec, EntitiesIconRenderer.IconRecord> dict;
      if (!this.m_icons.TryGetValue(entity, out dict))
        return;
      if (entity.IsDestroyed)
      {
        this.removeAllIconsOfImmediate(entity);
      }
      else
      {
        int num = 0;
        foreach (KeyValuePair<IconSpec, EntitiesIconRenderer.IconRecord> keyValuePair in dict)
        {
          GameObject go = keyValuePair.Value.Go;
          if ((bool) (UnityEngine.Object) go)
            this.m_iconsGoPool.ReturnInstance(ref go);
          GameObject iconGo = this.createIconGo(keyValuePair.Key, entity, num++);
          iconGo.SetActive(this.AllIconsVisible);
          EntitiesIconRenderer.IconRecord iconRecord = new EntitiesIconRenderer.IconRecord()
          {
            AmountOfIcons = keyValuePair.Value.AmountOfIcons,
            Go = iconGo
          };
          this.m_iconsTemp.Add(keyValuePair.Key, iconRecord);
        }
        this.m_icons[entity] = this.m_iconsTemp;
        this.m_iconsTemp = dict;
        this.m_iconsTemp.Clear();
      }
    }

    public void AddIcon(IconSpec iconSpec, IRenderedEntity entity)
    {
      this.m_pendingRequests.Add(new EntitiesIconRenderer.IconRequest(iconSpec, entity, EntitiesIconRenderer.RequestType.Add));
    }

    public void RemoveIcon(IconSpec iconSpec, IRenderedEntity entity)
    {
      this.m_pendingRequests.Add(new EntitiesIconRenderer.IconRequest(iconSpec, entity, EntitiesIconRenderer.RequestType.Remove));
    }

    public void ShowIcon(IconSpec iconSpec, IRenderedEntity entity)
    {
      this.m_pendingRequests.Add(new EntitiesIconRenderer.IconRequest(iconSpec, entity, EntitiesIconRenderer.RequestType.Show));
    }

    public void HideIcon(IconSpec iconSpec, IRenderedEntity entity)
    {
      this.m_pendingRequests.Add(new EntitiesIconRenderer.IconRequest(iconSpec, entity, EntitiesIconRenderer.RequestType.Hide));
    }

    private void SetIconVisibility(IconSpec iconSpec, IRenderedEntity entity, bool isVisible)
    {
      Dict<IconSpec, EntitiesIconRenderer.IconRecord> dict;
      EntitiesIconRenderer.IconRecord iconRecord;
      if (!this.m_icons.TryGetValue(entity, out dict) || !dict.TryGetValue(iconSpec, out iconRecord) || !(bool) (UnityEngine.Object) iconRecord.Go)
        return;
      iconRecord.Go.SetActive(isVisible);
      if (isVisible || !this.m_temporarilyHiddenGOs.IsNotEmpty)
        return;
      this.m_temporarilyHiddenGOs.Remove(iconRecord.Go);
    }

    public void SetAllIconsVisibility(bool isVisible)
    {
      if (this.AllIconsVisible == isVisible)
        return;
      this.AllIconsVisible = isVisible;
      if (!isVisible)
      {
        this.m_temporarilyHiddenGOs.Clear();
        foreach (Dict<IconSpec, EntitiesIconRenderer.IconRecord> dict in this.m_icons.Values)
        {
          foreach (EntitiesIconRenderer.IconRecord iconRecord in dict.Values)
          {
            if ((bool) (UnityEngine.Object) iconRecord.Go && iconRecord.Go.activeSelf)
            {
              this.m_temporarilyHiddenGOs.Add(iconRecord.Go);
              iconRecord.Go.SetActive(false);
            }
          }
        }
      }
      else
      {
        foreach (GameObject temporarilyHiddenGo in this.m_temporarilyHiddenGOs)
        {
          if ((bool) (UnityEngine.Object) temporarilyHiddenGo)
            temporarilyHiddenGo.SetActive(true);
        }
        this.m_temporarilyHiddenGOs.Clear();
      }
    }

    private void addIconImmediate(IconSpec iconSpec, IRenderedEntity entity)
    {
      Assert.That<bool>(entity.IsDestroyed).IsFalse<string, IRenderedEntity>("Adding icon {0} for destroyed entity {1}.", iconSpec.Path, entity);
      Dict<IconSpec, EntitiesIconRenderer.IconRecord> dict;
      if (!this.m_icons.TryGetValue(entity, out dict))
      {
        dict = new Dict<IconSpec, EntitiesIconRenderer.IconRecord>();
        this.m_icons.Add(entity, dict);
      }
      EntitiesIconRenderer.IconRecord iconRecord;
      if (!dict.TryGetValue(iconSpec, out iconRecord))
      {
        GameObject iconGo = this.createIconGo(iconSpec, entity, dict.Count);
        iconGo.SetActive(this.AllIconsVisible);
        iconRecord = new EntitiesIconRenderer.IconRecord()
        {
          AmountOfIcons = 1,
          Go = iconGo
        };
      }
      else
        ++iconRecord.AmountOfIcons;
      dict[iconSpec] = iconRecord;
    }

    private GameObject createIconGo(IconSpec iconSpec, IRenderedEntity entity, int iconsCount)
    {
      Vector3 offset = !(entity is Mafi.Core.Factory.Transports.Transport) ? new Vector3(0.0f, (float) (5 + 4 * iconsCount), 0.0f) : new Vector3(0.0f, 2.5f + (float) (4 * iconsCount), 0.0f);
      EntityMb entityMb;
      GameObject iconGo;
      if (this.m_entitiesRenderer.Value.TryGetMbFor(entity, out entityMb))
        iconGo = this.createIcon(iconSpec, entityMb.gameObject, offset);
      else if (entity is IEntityWithPosition entityWithPosition)
      {
        iconGo = this.createIcon(iconSpec, entityWithPosition, offset);
      }
      else
      {
        Log.Error(string.Format("Failed to display icon for entity '{0}' that has no MB and is ", (object) entity) + "not 'IEntityWithPosition'.");
        iconGo = new GameObject();
      }
      return iconGo;
    }

    private void removeIconImmediate(IconSpec iconSpec, IRenderedEntity entity)
    {
      Dict<IconSpec, EntitiesIconRenderer.IconRecord> dict;
      EntitiesIconRenderer.IconRecord iconRecord;
      if (!this.m_icons.TryGetValue(entity, out dict) || !dict.TryGetValue(iconSpec, out iconRecord))
        return;
      if (iconRecord.AmountOfIcons == 1)
      {
        if ((bool) (UnityEngine.Object) iconRecord.Go)
          this.m_iconsGoPool.ReturnInstance(ref iconRecord.Go);
        dict.Remove(iconSpec);
      }
      else
      {
        --iconRecord.AmountOfIcons;
        dict[iconSpec] = iconRecord;
      }
    }

    private void removeAllIconsOfImmediate(IRenderedEntity entity)
    {
      Dict<IconSpec, EntitiesIconRenderer.IconRecord> dict;
      if (!this.m_icons.TryRemove(entity, out dict))
        return;
      foreach (KeyValuePair<IconSpec, EntitiesIconRenderer.IconRecord> keyValuePair in dict)
      {
        GameObject go = keyValuePair.Value.Go;
        if ((bool) (UnityEngine.Object) go)
          this.m_iconsGoPool.ReturnInstance(ref go);
      }
      dict.Clear();
    }

    private GameObject createIcon(IconSpec iconSpec, GameObject parent, Vector3 offset)
    {
      Assert.That<GameObject>(parent).IsValidUnityObject<GameObject>();
      GameObject instance = this.m_iconsGoPool.GetInstance();
      instance.transform.SetParent(parent.transform, false);
      instance.transform.localPosition = offset;
      instance.GetComponent<MeshRenderer>().sharedMaterial = this.getOrCreateMaterial(iconSpec);
      return instance;
    }

    private GameObject createIcon(
      IconSpec iconSpec,
      IEntityWithPosition entityWithPosition,
      Vector3 offset)
    {
      GameObject instance = this.m_iconsGoPool.GetInstance();
      instance.transform.localPosition = entityWithPosition.Position3f.ToVector3() + offset;
      instance.GetComponent<MeshRenderer>().sharedMaterial = this.getOrCreateMaterial(iconSpec);
      return instance;
    }

    private Material getOrCreateMaterial(IconSpec iconSpec)
    {
      Material material1;
      if (this.m_iconMaterials.TryGetValue(iconSpec, out material1))
        return material1;
      Material material2 = new Material(this.m_baseIconMaterialTemplate)
      {
        mainTexture = (Texture) this.m_assetsDb.GetSharedTexture(iconSpec.Path),
        color = (Color) iconSpec.Color.ToColor32()
      };
      this.m_iconMaterials.Add(iconSpec, material2);
      return material2;
    }

    private struct IconRecord
    {
      public int AmountOfIcons;
      public GameObject Go;
    }

    private enum RequestType
    {
      Add,
      Remove,
      Show,
      Hide,
    }

    private readonly struct IconRequest
    {
      public readonly IconSpec IconSpec;
      public readonly IRenderedEntity Entity;
      public readonly EntitiesIconRenderer.RequestType Type;

      public IconRequest(
        IconSpec iconPath,
        IRenderedEntity entity,
        EntitiesIconRenderer.RequestType type)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.IconSpec = iconPath;
        this.Entity = entity;
        this.Type = type;
      }
    }
  }
}
