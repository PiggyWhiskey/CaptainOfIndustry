// Decompiled with JetBrains decompiler
// Type: RTG.SceneOverlapFilter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SceneOverlapFilter
  {
    private List<GameObjectType> _allowedObjectTypes;
    private List<GameObject> _ignoreObjects;
    private int _layerMask;

    public List<GameObjectType> AllowedObjectTypes => this._allowedObjectTypes;

    public List<GameObject> IgnoreObjects => this._ignoreObjects;

    public int LayerMask
    {
      get => this._layerMask;
      set => this._layerMask = value;
    }

    public void FilterOverlaps(List<GameObject> gameObjects)
    {
      gameObjects.RemoveAll((Predicate<GameObject>) (item => !this.AllowedObjectTypes.Contains(item.GetGameObjectType()) || this.IgnoreObjects.Contains(item) || !LayerEx.IsLayerBitSet(this._layerMask, item.layer)));
    }

    public SceneOverlapFilter()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._allowedObjectTypes = new List<GameObjectType>();
      this._ignoreObjects = new List<GameObject>();
      this._layerMask = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
