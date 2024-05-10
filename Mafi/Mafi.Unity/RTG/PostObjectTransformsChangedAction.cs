// Decompiled with JetBrains decompiler
// Type: RTG.PostObjectTransformsChangedAction
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class PostObjectTransformsChangedAction : IUndoRedoAction
  {
    private List<LocalTransformSnapshot> _preChangeTransformSnapshots;
    private List<LocalTransformSnapshot> _postChangeTransformSnapshots;

    public PostObjectTransformsChangedAction(
      List<LocalTransformSnapshot> preChangeTransformSnapshots,
      List<LocalTransformSnapshot> postChangeTransformSnapshots)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._preChangeTransformSnapshots = new List<LocalTransformSnapshot>();
      this._postChangeTransformSnapshots = new List<LocalTransformSnapshot>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._preChangeTransformSnapshots = new List<LocalTransformSnapshot>((IEnumerable<LocalTransformSnapshot>) preChangeTransformSnapshots);
      this._postChangeTransformSnapshots = new List<LocalTransformSnapshot>((IEnumerable<LocalTransformSnapshot>) postChangeTransformSnapshots);
    }

    public void Execute() => MonoSingleton<RTUndoRedo>.Get.RecordAction((IUndoRedoAction) this);

    public void Undo()
    {
      foreach (LocalTransformSnapshot transformSnapshot in this._preChangeTransformSnapshots)
        transformSnapshot.Apply();
    }

    public void Redo()
    {
      foreach (LocalTransformSnapshot transformSnapshot in this._postChangeTransformSnapshots)
        transformSnapshot.Apply();
    }

    public void OnRemovedFromUndoRedoStack()
    {
    }
  }
}
