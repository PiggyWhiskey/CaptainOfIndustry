// Decompiled with JetBrains decompiler
// Type: RTG.TerrainGizmoHorizontalOffsetDragEndAction
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
  public class TerrainGizmoHorizontalOffsetDragEndAction : IUndoRedoAction
  {
    private List<LocalTransformSnapshot> _preChangeSnapshots;
    private List<LocalTransformSnapshot> _postChangeSnapshots;
    private Vector3 _preChangeGizmoPos;
    private Vector3 _postChangeGizmoPos;
    private TerrainGizmo _terrainGizmo;

    public TerrainGizmoHorizontalOffsetDragEndAction(
      TerrainGizmo terrainGizmo,
      Vector3 preChangeGizmoPos,
      List<LocalTransformSnapshot> preChangeSnapshots,
      List<LocalTransformSnapshot> postChangeSnapshots)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._preChangeSnapshots = new List<LocalTransformSnapshot>();
      this._postChangeSnapshots = new List<LocalTransformSnapshot>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._preChangeSnapshots = new List<LocalTransformSnapshot>((IEnumerable<LocalTransformSnapshot>) preChangeSnapshots);
      this._postChangeSnapshots = new List<LocalTransformSnapshot>((IEnumerable<LocalTransformSnapshot>) postChangeSnapshots);
      this._preChangeGizmoPos = preChangeGizmoPos;
      this._postChangeGizmoPos = terrainGizmo.Gizmo.Transform.Position3D;
      this._terrainGizmo = terrainGizmo;
    }

    public void Execute() => MonoSingleton<RTUndoRedo>.Get.RecordAction((IUndoRedoAction) this);

    public void OnRemovedFromUndoRedoStack()
    {
    }

    public void Redo()
    {
      this._terrainGizmo.Gizmo.Transform.Position3D = this._postChangeGizmoPos;
      foreach (LocalTransformSnapshot postChangeSnapshot in this._postChangeSnapshots)
        postChangeSnapshot.Apply();
    }

    public void Undo()
    {
      this._terrainGizmo.Gizmo.Transform.Position3D = this._preChangeGizmoPos;
      foreach (LocalTransformSnapshot preChangeSnapshot in this._preChangeSnapshots)
        preChangeSnapshot.Apply();
    }
  }
}
