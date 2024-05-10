// Decompiled with JetBrains decompiler
// Type: RTG.DuplicateObjectsAction
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
  public class DuplicateObjectsAction : IUndoRedoAction
  {
    private List<GameObject> _rootsToDuplicate;
    private List<GameObject> _duplicateResult;
    private bool _cleanupOnRemovedFromStack;

    public List<GameObject> DuplicateResult
    {
      get => new List<GameObject>((IEnumerable<GameObject>) this._duplicateResult);
    }

    public DuplicateObjectsAction(List<GameObject> rootsToDuplicate)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._duplicateResult = new List<GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._rootsToDuplicate = GameObjectEx.FilterParentsOnly((IEnumerable<GameObject>) rootsToDuplicate);
    }

    public void Execute()
    {
      if (this._rootsToDuplicate.Count == 0)
        return;
      ObjectCloning.Config defaultConfig = ObjectCloning.DefaultConfig;
      foreach (GameObject root in this._rootsToDuplicate)
      {
        Transform transform = root.transform;
        defaultConfig.Layer = root.layer;
        defaultConfig.Parent = transform.parent;
        this._duplicateResult.Add(ObjectCloning.CloneHierarchy(root, defaultConfig));
      }
      MonoSingleton<RTUndoRedo>.Get.RecordAction((IUndoRedoAction) this);
    }

    public void Undo()
    {
      if (this._duplicateResult == null)
        return;
      foreach (GameObject gameObject in this._duplicateResult)
        gameObject.SetActive(false);
      this._cleanupOnRemovedFromStack = true;
    }

    public void Redo()
    {
      if (this._duplicateResult == null)
        return;
      foreach (GameObject gameObject in this._duplicateResult)
        gameObject.SetActive(true);
      this._cleanupOnRemovedFromStack = false;
    }

    public void OnRemovedFromUndoRedoStack()
    {
      if (!this._cleanupOnRemovedFromStack || this._duplicateResult.Count == 0)
        return;
      foreach (Object @object in this._duplicateResult)
        Object.Destroy(@object);
      this._duplicateResult.Clear();
    }
  }
}
