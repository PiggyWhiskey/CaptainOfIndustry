// Decompiled with JetBrains decompiler
// Type: RTG.XZGridCell
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
  public class XZGridCell
  {
    private IXZGrid _parentGrid;
    private int _xIndex;
    private int _zIndex;
    private Vector3 _min;
    private Vector3 _max;

    public IXZGrid ParentGrid => this._parentGrid;

    public int XIndex => this._xIndex;

    public int ZIndex => this._zIndex;

    public Vector3 Min => this._min;

    public Vector3 Max => this._max;

    public Vector3 Center => (this._min + this._max) * 0.5f;

    public XZGridCell(int xIndex, int zIndex, Vector3 min, Vector3 max, IXZGrid parentGrid)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._xIndex = xIndex;
      this._zIndex = zIndex;
      this._min = min;
      this._max = max;
      this._parentGrid = parentGrid;
    }

    public static XZGridCell FromPoint(
      Vector3 point,
      float cellSizeX,
      float cellSizeZ,
      IXZGrid parentGrid)
    {
      Matrix4x4 worldMatrix = parentGrid.WorldMatrix;
      Vector3 vector3 = worldMatrix.inverse.MultiplyPoint(point);
      int xIndex = Mathf.FloorToInt(vector3.x / cellSizeX);
      int zIndex = Mathf.FloorToInt(vector3.z / cellSizeZ);
      Vector3 point1 = Vector3.right * (float) xIndex * cellSizeX + Vector3.forward * (float) zIndex * cellSizeZ;
      Vector3 point2 = point1 + Vector3.right * cellSizeX + Vector3.forward * cellSizeZ;
      Vector3 min = worldMatrix.MultiplyPoint(point1);
      Vector3 max = worldMatrix.MultiplyPoint(point2);
      return new XZGridCell(xIndex, zIndex, min, max, parentGrid);
    }

    public List<Vector3> GetCenterAndCorners()
    {
      List<Vector3> centerAndCorners = new List<Vector3>();
      centerAndCorners.Add(this.Center);
      Vector3 vector3 = this._max - this._min;
      centerAndCorners.Add(this._min);
      centerAndCorners.Add(this._min + Vector3.forward * vector3.z);
      centerAndCorners.Add(this._max);
      centerAndCorners.Add(this._min + Vector3.right * vector3.x);
      return centerAndCorners;
    }
  }
}
