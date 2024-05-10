// Decompiled with JetBrains decompiler
// Type: RTG.Object2ObjectSnapData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class Object2ObjectSnapData
  {
    private GameObject _gameObject;
    private AABB[] _snapAreaBounds;
    private BoxFaceAreaDesc[] _snapAreaDesc;
    private List<Vector3> _vertsBuffer;

    public bool Initialize(GameObject gameObject)
    {
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null || (UnityEngine.Object) this._gameObject != (UnityEngine.Object) null)
        return false;
      Mesh mesh = gameObject.GetMesh();
      Sprite sprite = gameObject.GetSprite();
      if ((UnityEngine.Object) mesh == (UnityEngine.Object) null && (UnityEngine.Object) sprite == (UnityEngine.Object) null)
        return false;
      bool flag = true;
      if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
        flag = false;
      RTMesh rtMesh = (RTMesh) null;
      if (flag)
      {
        Renderer meshRenderer = gameObject.GetMeshRenderer();
        if ((UnityEngine.Object) meshRenderer == (UnityEngine.Object) null || !meshRenderer.enabled)
          flag = false;
        rtMesh = Singleton<RTMeshDb>.Get.GetRTMesh(mesh);
        if (rtMesh == null)
          flag = false;
      }
      if (rtMesh == null && (UnityEngine.Object) sprite == (UnityEngine.Object) null)
        return false;
      List<AABB> aabbList = this.BuildVertOverlapAABBs(gameObject, flag ? (Sprite) null : sprite, flag ? rtMesh : (RTMesh) null);
      if (aabbList.Count == 0)
        return false;
      AABB aabb = flag ? rtMesh.AABB : ObjectBounds.CalcSpriteModelAABB(gameObject);
      List<BoxFace> allBoxFaces = BoxMath.AllBoxFaces;
      this._gameObject = gameObject;
      if (flag)
      {
        foreach (BoxFace index in allBoxFaces)
        {
          AABB modelAABB = aabbList[(int) index];
          rtMesh.OverlapModelVerts(modelAABB, this._vertsBuffer);
          Plane plane = BoxMath.CalcBoxFacePlane(aabb.Center, aabb.Size, Quaternion.identity, index);
          this._snapAreaBounds[(int) index] = new AABB((IEnumerable<Vector3>) plane.ProjectAllPoints(this._vertsBuffer));
          this._snapAreaDesc[(int) index] = BoxMath.GetBoxFaceAreaDesc(this._snapAreaBounds[(int) index].Size, index);
        }
      }
      else
      {
        foreach (BoxFace index in allBoxFaces)
        {
          switch (index)
          {
            case BoxFace.Front:
            case BoxFace.Back:
              this._snapAreaBounds[(int) index] = AABB.GetInvalid();
              this._snapAreaDesc[(int) index] = BoxFaceAreaDesc.GetInvalid();
              continue;
            default:
              AABB collectAABB = aabbList[(int) index];
              List<Vector3> points = ObjectVertexCollect.CollectModelSpriteVerts(sprite, collectAABB);
              List<Vector3> pointCloud = BoxMath.CalcBoxFacePlane(aabb.Center, aabb.Size, Quaternion.identity, index).ProjectAllPoints(points);
              this._snapAreaBounds[(int) index] = new AABB((IEnumerable<Vector3>) pointCloud);
              this._snapAreaDesc[(int) index] = BoxMath.GetBoxFaceAreaDesc(this._snapAreaBounds[(int) index].Size, index);
              continue;
          }
        }
      }
      return true;
    }

    public BoxFaceAreaDesc GetWorldSnapAreaDesc(BoxFace boxFace)
    {
      return BoxMath.GetBoxFaceAreaDesc(Vector3.Scale(this._snapAreaBounds[(int) boxFace].Size, this._gameObject.transform.lossyScale.Abs()), boxFace);
    }

    public List<OBB> GetAllWorldSnapAreaBounds()
    {
      if ((UnityEngine.Object) this._gameObject == (UnityEngine.Object) null)
        return new List<OBB>();
      Transform transform = this._gameObject.transform;
      List<OBB> worldSnapAreaBounds = new List<OBB>(this._snapAreaBounds.Length);
      foreach (AABB snapAreaBound in this._snapAreaBounds)
        worldSnapAreaBounds.Add(new OBB(snapAreaBound, transform));
      return worldSnapAreaBounds;
    }

    public OBB GetWorldSnapAreaBounds(BoxFace boxFace)
    {
      if ((UnityEngine.Object) this._gameObject == (UnityEngine.Object) null)
        return OBB.GetInvalid();
      Transform transform = this._gameObject.transform;
      return new OBB(this._snapAreaBounds[(int) boxFace], transform);
    }

    private List<AABB> BuildVertOverlapAABBs(GameObject gameObject, Sprite sprite, RTMesh rtMesh)
    {
      if ((UnityEngine.Object) sprite == (UnityEngine.Object) null && rtMesh == null)
        return new List<AABB>();
      float num = 0.1f;
      AABB aabb = (UnityEngine.Object) sprite != (UnityEngine.Object) null ? ObjectBounds.CalcSpriteModelAABB(gameObject) : rtMesh.AABB;
      Vector3 size = aabb.Size;
      List<BoxFace> allBoxFaces = BoxMath.AllBoxFaces;
      Vector3[] vector3Array = new Vector3[allBoxFaces.Count];
      vector3Array[2] = new Vector3(0.2f, size.y + 1f / 1000f, size.z + 1f / 1000f);
      vector3Array[3] = new Vector3(0.2f, size.y + 1f / 1000f, size.z + 1f / 1000f);
      vector3Array[4] = new Vector3(size.x + 1f / 1000f, 0.2f, size.z + 1f / 1000f);
      vector3Array[5] = new Vector3(size.x + 1f / 1000f, 0.2f, size.z + 1f / 1000f);
      vector3Array[1] = new Vector3(size.x + 1f / 1000f, size.y + 1f / 1000f, 0.2f);
      vector3Array[0] = new Vector3(size.x + 1f / 1000f, size.y + 1f / 1000f, 0.2f);
      List<AABB> aabbList = new List<AABB>();
      for (int index = 0; index < allBoxFaces.Count; ++index)
      {
        BoxFace boxFace = allBoxFaces[index];
        Vector3 center = BoxMath.CalcBoxFaceCenter(aabb.Center, aabb.Size, Quaternion.identity, boxFace) - BoxMath.CalcBoxFaceNormal(aabb.Center, aabb.Size, Quaternion.identity, boxFace) * num;
        aabbList.Add(new AABB(center, vector3Array[index]));
      }
      return aabbList;
    }

    public Object2ObjectSnapData()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._snapAreaBounds = new AABB[Enum.GetValues(typeof (BoxFace)).Length];
      this._snapAreaDesc = new BoxFaceAreaDesc[Enum.GetValues(typeof (BoxFace)).Length];
      this._vertsBuffer = new List<Vector3>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
