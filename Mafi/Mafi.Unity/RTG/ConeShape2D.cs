// Decompiled with JetBrains decompiler
// Type: RTG.ConeShape2D
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
  public class ConeShape2D : Shape2D
  {
    private Vector2 _baseCenter;
    private float _rotationDegrees;
    private float _baseRadius;
    private float _height;

    public Vector2 BaseCenter
    {
      get => this._baseCenter;
      set => this._baseCenter = value;
    }

    public Vector2 BaseLeft
    {
      get => this._baseCenter - this.Right * this._baseRadius;
      set => this._baseCenter = value + this.Right * this._baseRadius;
    }

    public Vector2 BaseRight
    {
      get => this._baseCenter + this.Right * this._baseRadius;
      set => this._baseCenter = value - this.Right * this._baseRadius;
    }

    public Vector2 Tip
    {
      get => this._baseCenter + this.CentralAxis * this._height;
      set => this._baseCenter = value - this.CentralAxis * this._height;
    }

    public float BaseRadius
    {
      get => this._baseRadius;
      set => this._baseRadius = Mathf.Abs(value);
    }

    public float Height
    {
      get => this._height;
      set => this._height = Mathf.Abs(value);
    }

    public float RotationDegrees
    {
      get => this._rotationDegrees;
      set => this._rotationDegrees = value % 360f;
    }

    public Quaternion Rotation => Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward);

    public Vector2 CentralAxis => this.Up;

    public Vector2 Right => (Vector2) (this.Rotation * (Vector3) ConeShape2D.ModelRight);

    public Vector2 Up => (Vector2) (this.Rotation * (Vector3) ConeShape2D.ModelUp);

    public static Vector2 ModelRight => Vector2.right;

    public static Vector2 ModelUp => Vector2.up;

    public static Vector2 ModelBaseCenter => Vector2.zero;

    public override void RenderArea(Camera camera)
    {
      Vector2 baseLeft = this.BaseLeft;
      List<Vector2> points = new List<Vector2>();
      points.Add(this.Tip);
      points.Add(this.BaseRight);
      Camera camera1 = camera;
      GLRenderer.DrawTriangleFan2D(baseLeft, points, camera1);
    }

    public override void RenderBorder(Camera camera)
    {
      GLRenderer.DrawLineLoop2D(new List<Vector2>()
      {
        this.BaseLeft,
        this.Tip,
        this.BaseRight
      }, camera);
    }

    public override bool ContainsPoint(Vector2 point)
    {
      return TriangleMath.Contains2DPoint(point, this.BaseLeft, this.Tip, this.BaseRight);
    }

    public override Rect GetEncapsulatingRect()
    {
      return RectEx.FromPoints((IEnumerable<Vector2>) new List<Vector2>()
      {
        this.BaseLeft,
        this.Tip,
        this.BaseRight
      });
    }

    public ConeShape2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._baseCenter = ConeShape2D.ModelBaseCenter;
      this._baseRadius = 15f;
      this._height = 15f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
