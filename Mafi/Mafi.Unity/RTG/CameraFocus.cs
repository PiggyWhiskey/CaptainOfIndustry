// Decompiled with JetBrains decompiler
// Type: RTG.CameraFocus
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class CameraFocus
  {
    public static CameraFocus.Data CalculateFocusData(
      Camera camera,
      AABB focusAABB,
      CameraFocusSettings focusSettings)
    {
      float magnitude = focusAABB.Size.magnitude;
      float num = camera.GetFrustumDistanceFromHeight(magnitude) + focusSettings.FocusDistanceAdd;
      if ((double) num < (double) camera.nearClipPlane)
        num += camera.nearClipPlane - num;
      return new CameraFocus.Data(focusAABB.Center - camera.transform.forward * num, focusAABB.Center);
    }

    public class Data
    {
      private Vector3 _cameraWorldPosition;
      private Vector3 _focusPoint;
      private float _focusPointOffset;

      public Vector3 CameraWorldPosition => this._cameraWorldPosition;

      public Vector3 FocusPoint => this._focusPoint;

      public float FocusPointOffset => this._focusPointOffset;

      public Data(Vector3 cameraWorldPosition, Vector3 focusPoint)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this._cameraWorldPosition = cameraWorldPosition;
        this._focusPoint = focusPoint;
        this._focusPointOffset = (cameraWorldPosition - focusPoint).magnitude;
      }
    }
  }
}
