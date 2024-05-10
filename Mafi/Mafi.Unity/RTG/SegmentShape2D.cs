// Decompiled with JetBrains decompiler
// Type: RTG.SegmentShape2D
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
  public class SegmentShape2D : Shape2D
  {
    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private Vector2 _direction;
    private float _length;
    private SegmentEpsilon _epsilon;

    public float Length
    {
      get => this._length;
      set
      {
        this._length = Mathf.Abs(value);
        this._endPoint = this._startPoint + this._direction * this._length;
      }
    }

    public Vector2 StartPoint
    {
      get => this._startPoint;
      set
      {
        this._startPoint = value;
        this._endPoint = this._startPoint + this._direction * this._length;
      }
    }

    public Vector2 EndPoint
    {
      get => this._endPoint;
      set
      {
        this._endPoint = value;
        this._direction = this._endPoint - this._startPoint;
        this._length = this._direction.magnitude;
        this._direction.Normalize();
      }
    }

    public Vector2 Direction
    {
      get => this._direction;
      set
      {
        this._direction = value.normalized;
        this._endPoint = this._startPoint + this._direction * this._length;
      }
    }

    public SegmentEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float RaycastEps
    {
      get => this._epsilon.RaycastEps;
      set => this._epsilon.RaycastEps = value;
    }

    public float PtOnSegmentEps
    {
      get => this._epsilon.PtOnSegmentEps;
      set => this._epsilon.PtOnSegmentEps = value;
    }

    public void SetEndPtFromStart(Vector2 dirDromStart, float offset)
    {
      this.EndPoint = this.StartPoint + dirDromStart * offset;
    }

    public override void RenderBorder(Camera camera)
    {
      GLRenderer.DrawLine2D(this._startPoint, this._endPoint, camera);
    }

    public override void RenderArea(Camera camera)
    {
      GLRenderer.DrawLine2D(this._startPoint, this._endPoint, camera);
    }

    public override bool ContainsPoint(Vector2 point)
    {
      return SegmentMath.Is2DPointOnSegment(point, this._startPoint, this._endPoint, this._epsilon);
    }

    public override Rect GetEncapsulatingRect()
    {
      return RectEx.FromPoints((IEnumerable<Vector2>) new Vector2[2]
      {
        this._startPoint,
        this._endPoint
      });
    }

    public SegmentShape2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._startPoint = Vector2.zero;
      this._endPoint = Vector2.right;
      this._direction = Vector2.right;
      this._length = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
