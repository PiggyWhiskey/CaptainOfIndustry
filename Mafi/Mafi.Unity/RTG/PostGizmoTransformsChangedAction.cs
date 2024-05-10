// Decompiled with JetBrains decompiler
// Type: RTG.PostGizmoTransformsChangedAction
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class PostGizmoTransformsChangedAction : IUndoRedoAction
  {
    private List<LocalGizmoTransformSnapshot> _preChangeTransformSnapshots;
    private List<LocalGizmoTransformSnapshot> _postChangeTransformSnapshots;

    public PostGizmoTransformsChangedAction(
      List<LocalGizmoTransformSnapshot> preChangeTransformSnapshots,
      List<LocalGizmoTransformSnapshot> postChangeTransformSnapshots)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._preChangeTransformSnapshots = new List<LocalGizmoTransformSnapshot>();
      this._postChangeTransformSnapshots = new List<LocalGizmoTransformSnapshot>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._preChangeTransformSnapshots = new List<LocalGizmoTransformSnapshot>((IEnumerable<LocalGizmoTransformSnapshot>) preChangeTransformSnapshots);
      this._postChangeTransformSnapshots = new List<LocalGizmoTransformSnapshot>((IEnumerable<LocalGizmoTransformSnapshot>) postChangeTransformSnapshots);
    }

    public void Execute() => MonoSingleton<RTUndoRedo>.Get.RecordAction((IUndoRedoAction) this);

    public void Undo()
    {
      foreach (LocalGizmoTransformSnapshot transformSnapshot in this._preChangeTransformSnapshots)
        transformSnapshot.Apply();
    }

    public void Redo()
    {
      foreach (LocalGizmoTransformSnapshot transformSnapshot in this._postChangeTransformSnapshots)
        transformSnapshot.Apply();
    }

    public void OnRemovedFromUndoRedoStack()
    {
    }
  }
}
