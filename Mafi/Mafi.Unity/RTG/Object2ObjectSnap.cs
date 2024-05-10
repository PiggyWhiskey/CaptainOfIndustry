// Decompiled with JetBrains decompiler
// Type: RTG.Object2ObjectSnap
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
  public static class Object2ObjectSnap
  {
    private static List<GameObject> _nearbyObjectBuffer;
    private static Object2ObjectSnap.Config _defaultConfig;

    public static int MaxSourceObjects => 100;

    static Object2ObjectSnap()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Object2ObjectSnap._nearbyObjectBuffer = new List<GameObject>();
      Object2ObjectSnap._defaultConfig = new Object2ObjectSnap.Config();
      Object2ObjectSnap._defaultConfig.AreaMatchEps = 1E-05f;
      Object2ObjectSnap._defaultConfig.Prefs = Object2ObjectSnap.Prefs.None;
    }

    public static Object2ObjectSnap.Config DefaultConfig => Object2ObjectSnap._defaultConfig;

    public static Object2ObjectSnap.SnapResult Snap(
      List<GameObject> roots,
      Object2ObjectSnap.Config snapConfig)
    {
      float num = float.MaxValue;
      Object2ObjectSnap.SnapResult snapResult1 = new Object2ObjectSnap.SnapResult(Object2ObjectSnap.SnapFailReson.NoDestinationFound);
      foreach (GameObject root in roots)
      {
        Object2ObjectSnap.SnapResult snapResult2 = Object2ObjectSnap.CalculateSnapResult(root, snapConfig);
        if (snapResult2.FailReason == Object2ObjectSnap.SnapFailReson.MaxObjectsExceeded)
          return snapResult2;
        if (snapResult2.FailReason == Object2ObjectSnap.SnapFailReson.None && (double) snapResult2.SnapDistance < (double) num)
        {
          num = snapResult2.SnapDistance;
          snapResult1 = snapResult2;
        }
      }
      if (snapResult1.Success)
        ObjectSnap.Snap(roots, snapResult1.SnapPivot, snapResult1.SnapDestination);
      return snapResult1;
    }

    public static Object2ObjectSnap.SnapResult Snap(
      GameObject root,
      Object2ObjectSnap.Config snapConfig)
    {
      Object2ObjectSnap.SnapResult snapResult = Object2ObjectSnap.CalculateSnapResult(root, snapConfig);
      if (snapResult.Success)
        ObjectSnap.Snap(root, snapResult.SnapPivot, snapResult.SnapDestination);
      return snapResult;
    }

    public static Object2ObjectSnap.SnapResult CalculateSnapResult(
      GameObject root,
      Object2ObjectSnap.Config snapConfig)
    {
      if (snapConfig.IgnoreDestObjects == null)
        snapConfig.IgnoreDestObjects = new List<GameObject>();
      List<GameObject> allChildrenAndSelf = root.GetAllChildrenAndSelf();
      if (allChildrenAndSelf.Count > Object2ObjectSnap.MaxSourceObjects)
        return new Object2ObjectSnap.SnapResult(Object2ObjectSnap.SnapFailReson.MaxObjectsExceeded);
      List<GameObject> objectsInHierarchy1 = root.GetMeshObjectsInHierarchy();
      List<GameObject> objectsInHierarchy2 = root.GetSpriteObjectsInHierarchy();
      if (objectsInHierarchy1.Count == 0 && objectsInHierarchy2.Count == 0)
        return new Object2ObjectSnap.SnapResult(Object2ObjectSnap.SnapFailReson.InvalidSourceObjects);
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
      Vector3 vector3_1 = Vector3.one * snapConfig.SnapRadius * 2f;
      List<BoxFace> allBoxFaces = BoxMath.AllBoxFaces;
      bool flag1 = (snapConfig.Prefs & Object2ObjectSnap.Prefs.TryMatchArea) != 0;
      bool flag2 = false;
      List<Object2ObjectSnap.SnapSortData> snapSortDataList = new List<Object2ObjectSnap.SnapSortData>(10);
      Object2ObjectSnap.SnapSortData snapSortData = new Object2ObjectSnap.SnapSortData();
      foreach (GameObject gameObject1 in allChildrenAndSelf)
      {
        OBB obb = ObjectBounds.CalcWorldOBB(gameObject1, queryConfig);
        obb.Size += vector3_1;
        MonoSingleton<RTScene>.Get.OverlapBox(obb, Object2ObjectSnap._nearbyObjectBuffer);
        Object2ObjectSnap._nearbyObjectBuffer.RemoveAll((Predicate<GameObject>) (item => item.transform.IsChildOf(root.transform) || snapConfig.IgnoreDestObjects.Contains(item) || !LayerEx.IsLayerBitSet(snapConfig.DestinationLayers, item.layer)));
        if (Object2ObjectSnap._nearbyObjectBuffer.Count != 0)
        {
          Object2ObjectSnapData object2ObjectSnapData1 = Singleton<Object2ObjectSnapDataDb>.Get.GetObject2ObjectSnapData(gameObject1);
          if (object2ObjectSnapData1 != null)
          {
            snapSortData.SrcObject = gameObject1;
            foreach (BoxFace boxFace1 in allBoxFaces)
            {
              BoxFaceAreaDesc worldSnapAreaDesc1 = object2ObjectSnapData1.GetWorldSnapAreaDesc(boxFace1);
              List<Vector3> centerAndCornerPoints1 = object2ObjectSnapData1.GetWorldSnapAreaBounds(boxFace1).GetCenterAndCornerPoints();
              snapSortData.SrcSnapFace = boxFace1;
              foreach (GameObject gameObject2 in Object2ObjectSnap._nearbyObjectBuffer)
              {
                Object2ObjectSnapData object2ObjectSnapData2 = Singleton<Object2ObjectSnapDataDb>.Get.GetObject2ObjectSnapData(gameObject2);
                if (object2ObjectSnapData2 != null)
                {
                  snapSortData.DestObject = gameObject2;
                  foreach (BoxFace boxFace2 in allBoxFaces)
                  {
                    snapSortData.DestSnapFace = boxFace2;
                    BoxFaceAreaDesc worldSnapAreaDesc2 = object2ObjectSnapData2.GetWorldSnapAreaDesc(boxFace2);
                    snapSortData.FaceAreasMatch = false;
                    if (flag1 && worldSnapAreaDesc2.AreaType == worldSnapAreaDesc1.AreaType)
                    {
                      snapSortData.FaceAreaDiff = Mathf.Abs(worldSnapAreaDesc2.Area - worldSnapAreaDesc1.Area);
                      if ((double) snapSortData.FaceAreaDiff <= 1000.0)
                        snapSortData.FaceAreasMatch = true;
                    }
                    List<Vector3> centerAndCornerPoints2 = object2ObjectSnapData2.GetWorldSnapAreaBounds(boxFace2).GetCenterAndCornerPoints();
                    foreach (Vector3 vector3_2 in centerAndCornerPoints1)
                    {
                      snapSortData.SnapPivot = vector3_2;
                      foreach (Vector3 vector3_3 in centerAndCornerPoints2)
                      {
                        snapSortData.SnapDistance = (vector3_3 - vector3_2).magnitude;
                        if ((double) snapSortData.SnapDistance < (double) snapConfig.SnapRadius)
                        {
                          snapSortData.SnapDest = vector3_3;
                          snapSortDataList.Add(snapSortData);
                          if (snapSortData.FaceAreasMatch)
                            flag2 = true;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      if (snapSortDataList.Count == 0)
        return new Object2ObjectSnap.SnapResult(Object2ObjectSnap.SnapFailReson.NoDestinationFound);
      if (!flag1 || !flag2)
      {
        snapSortDataList.Sort((Comparison<Object2ObjectSnap.SnapSortData>) ((s0, s1) => s0.SnapDistance.CompareTo(s1.SnapDistance)));
      }
      else
      {
        while (!snapSortDataList[0].FaceAreasMatch)
          snapSortDataList.RemoveAt(0);
        snapSortDataList.Sort((Comparison<Object2ObjectSnap.SnapSortData>) ((s0, s1) => s0.FaceAreaDiff.CompareTo(s1.FaceAreaDiff)));
      }
      return new Object2ObjectSnap.SnapResult(snapSortDataList[0].SnapPivot, snapSortDataList[0].SnapDest, snapSortDataList[0].SnapDistance);
    }

    [Flags]
    public enum Prefs
    {
      None = 0,
      TryMatchArea = 1,
      All = TryMatchArea, // 0x00000001
    }

    public enum SnapFailReson
    {
      None,
      MaxObjectsExceeded,
      InvalidSourceObjects,
      NoDestinationFound,
    }

    public struct SnapResult
    {
      private bool _success;
      private Vector3 _snapPivot;
      private Vector3 _snapDestination;
      private float _snapDistance;
      private Object2ObjectSnap.SnapFailReson _failReason;

      public bool Success => this._success;

      public Vector3 SnapPivot => this._snapPivot;

      public Vector3 SnapDestination => this._snapDestination;

      public float SnapDistance => this._snapDistance;

      public Object2ObjectSnap.SnapFailReson FailReason => this._failReason;

      public SnapResult(Object2ObjectSnap.SnapFailReson failReson)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._success = false;
        this._snapPivot = Vector3.zero;
        this._snapDestination = Vector3.zero;
        this._snapDistance = 0.0f;
        this._failReason = failReson;
      }

      public SnapResult(Vector3 snapPivot, Vector3 snapDestination, float snapDistance)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._success = true;
        this._snapPivot = snapPivot;
        this._snapDestination = snapDestination;
        this._snapDistance = snapDistance;
        this._failReason = Object2ObjectSnap.SnapFailReson.None;
      }
    }

    public struct Config
    {
      private float _areaMatchEps;
      public List<GameObject> IgnoreDestObjects;
      public int DestinationLayers;
      public float SnapRadius;
      public Object2ObjectSnap.Prefs Prefs;

      public float AreaMatchEps
      {
        get => this._areaMatchEps;
        set => this._areaMatchEps = Mathf.Abs(value);
      }
    }

    private struct SnapSortData
    {
      public GameObject SrcObject;
      public GameObject DestObject;
      public BoxFace SrcSnapFace;
      public BoxFace DestSnapFace;
      public bool FaceAreasMatch;
      public float FaceAreaDiff;
      public Vector3 SnapPivot;
      public Vector3 SnapDest;
      public float SnapDistance;
    }
  }
}
