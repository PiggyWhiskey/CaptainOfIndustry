// Decompiled with JetBrains decompiler
// Type: RTG.BoxCollider3DChangedAction
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class BoxCollider3DChangedAction : IUndoRedoAction
  {
    private BoxCollider3DSnapshot _preChangeSnapshot;
    private BoxCollider3DSnapshot _postChangeSnapshot;

    public BoxCollider3DChangedAction(
      BoxCollider3DSnapshot preChangeSnapshot,
      BoxCollider3DSnapshot postChangeSnapshot)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._preChangeSnapshot = new BoxCollider3DSnapshot(preChangeSnapshot);
      this._postChangeSnapshot = new BoxCollider3DSnapshot(postChangeSnapshot);
    }

    public void Execute() => MonoSingleton<RTUndoRedo>.Get.RecordAction((IUndoRedoAction) this);

    public void OnRemovedFromUndoRedoStack()
    {
    }

    public void Redo() => this._postChangeSnapshot.Apply();

    public void Undo() => this._preChangeSnapshot.Apply();
  }
}
