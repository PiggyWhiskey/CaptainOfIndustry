// Decompiled with JetBrains decompiler
// Type: RTG.GizmoScalerHandle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoScalerHandle
  {
    private int _handleId;
    private List<int> _scaleDragAxisIndices;

    public int HandleId => this._handleId;

    public List<int> ScaleDragAxisIndices
    {
      get => new List<int>((IEnumerable<int>) this._scaleDragAxisIndices);
    }

    public GizmoScalerHandle(int handleId, IEnumerable<int> scaleDragAxisIndices)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._scaleDragAxisIndices = new List<int>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._handleId = handleId;
      this._scaleDragAxisIndices = new List<int>(scaleDragAxisIndices);
    }

    public bool ContainsScaleDragAxisIndex(int scaleDragAxisIndex)
    {
      return this._scaleDragAxisIndices.Contains(scaleDragAxisIndex);
    }
  }
}
