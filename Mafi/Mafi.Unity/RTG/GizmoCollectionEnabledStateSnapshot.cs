// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCollectionEnabledStateSnapshot
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCollectionEnabledStateSnapshot
  {
    private Dictionary<Gizmo, bool> _gizmoToState;

    public void Snapshot(IEnumerable<Gizmo> gizmos)
    {
      this._gizmoToState.Clear();
      foreach (Gizmo gizmo in gizmos)
        this._gizmoToState.Add(gizmo, gizmo.IsEnabled);
    }

    public void Apply()
    {
      foreach (KeyValuePair<Gizmo, bool> keyValuePair in this._gizmoToState)
        keyValuePair.Key.SetEnabled(keyValuePair.Value);
      this._gizmoToState.Clear();
    }

    public GizmoCollectionEnabledStateSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._gizmoToState = new Dictionary<Gizmo, bool>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
