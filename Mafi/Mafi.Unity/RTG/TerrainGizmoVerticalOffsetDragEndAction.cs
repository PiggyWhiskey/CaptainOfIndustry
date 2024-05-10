// Decompiled with JetBrains decompiler
// Type: RTG.TerrainGizmoVerticalOffsetDragEndAction
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
  public class TerrainGizmoVerticalOffsetDragEndAction : IUndoRedoAction
  {
    private List<TerrainGizmoAffectedObject> _affectedObjects;
    private Terrain _terrain;
    private float[,] _preChangeHeights;
    private float[,] _postChangeHeights;

    public TerrainGizmoVerticalOffsetDragEndAction(
      Terrain terrain,
      float[,] preChangeHeights,
      float[,] postChangeHeights,
      List<TerrainGizmoAffectedObject> affectedObjects)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._terrain = terrain;
      if (preChangeHeights != null)
        this._preChangeHeights = preChangeHeights.Clone() as float[,];
      if (postChangeHeights != null)
        this._postChangeHeights = postChangeHeights.Clone() as float[,];
      this._affectedObjects = new List<TerrainGizmoAffectedObject>((IEnumerable<TerrainGizmoAffectedObject>) affectedObjects);
    }

    public void Execute() => MonoSingleton<RTUndoRedo>.Get.RecordAction((IUndoRedoAction) this);

    public void OnRemovedFromUndoRedoStack()
    {
    }

    public void Redo()
    {
      if ((Object) this._terrain != (Object) null && this._postChangeHeights != null)
        this._terrain.terrainData.SetHeights(0, 0, this._postChangeHeights);
      foreach (TerrainGizmoAffectedObject affectedObject in this._affectedObjects)
      {
        if ((Object) affectedObject.AffectedObject != (Object) null)
          affectedObject.AffectedObject.transform.position = affectedObject.NewPosition;
      }
    }

    public void Undo()
    {
      if ((Object) this._terrain != (Object) null && this._preChangeHeights != null)
        this._terrain.terrainData.SetHeights(0, 0, this._preChangeHeights);
      foreach (TerrainGizmoAffectedObject affectedObject in this._affectedObjects)
      {
        if ((Object) affectedObject.AffectedObject != (Object) null)
          affectedObject.AffectedObject.transform.position = affectedObject.OriginalObjectPos;
      }
    }
  }
}
