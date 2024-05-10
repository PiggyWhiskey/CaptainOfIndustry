// Decompiled with JetBrains decompiler
// Type: RTG.PostObjectSpawnAction
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class PostObjectSpawnAction : IUndoRedoAction
  {
    private bool _cleanupOnRemovedFromStack;
    private List<GameObject> _spawnedParents;

    public PostObjectSpawnAction(List<GameObject> spawnedParents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._spawnedParents = new List<GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._spawnedParents = new List<GameObject>((IEnumerable<GameObject>) spawnedParents);
    }

    public void Execute() => MonoSingleton<RTUndoRedo>.Get.RecordAction((IUndoRedoAction) this);

    public void Undo()
    {
      if (this._spawnedParents == null)
        return;
      foreach (GameObject spawnedParent in this._spawnedParents)
        spawnedParent.SetActive(false);
      this._cleanupOnRemovedFromStack = true;
    }

    public void Redo()
    {
      if (this._spawnedParents == null)
        return;
      foreach (GameObject spawnedParent in this._spawnedParents)
        spawnedParent.SetActive(true);
      this._cleanupOnRemovedFromStack = false;
    }

    public void OnRemovedFromUndoRedoStack()
    {
      if (!this._cleanupOnRemovedFromStack || this._spawnedParents.Count == 0)
        return;
      foreach (Object spawnedParent in this._spawnedParents)
        Object.Destroy(spawnedParent);
      this._spawnedParents.Clear();
    }
  }
}
