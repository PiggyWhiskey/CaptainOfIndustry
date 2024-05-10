// Decompiled with JetBrains decompiler
// Type: RTG.GizmoScaleGuide
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
  public class GizmoScaleGuide
  {
    private GizmoScaleGuideLookAndFeel _lookAndFeel;
    private GizmoScaleGuideLookAndFeel _sharedLookAndFeel;

    public GizmoScaleGuideLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel != null ? this._sharedLookAndFeel : this._lookAndFeel;
    }

    public GizmoScaleGuideLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set => this._sharedLookAndFeel = value;
    }

    public void Render(IEnumerable<GameObject> gameObjects, Camera camera)
    {
      if (gameObjects == null)
        return;
      GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
      get.ResetValuesToSensibleDefaults();
      foreach (GameObject gameObject in gameObjects)
      {
        Transform transform = gameObject.transform;
        Vector3 position = transform.position;
        Vector3 right = transform.right;
        Vector3 up = transform.up;
        Vector3 forward = transform.forward;
        float num1 = 1f;
        if (this.LookAndFeel.UseZoomFactor)
          num1 = camera.EstimateZoomFactor(position);
        float num2 = this.LookAndFeel.AxisLength * num1;
        Vector3 startPoint1 = position - right * num2;
        Vector3 endPoint1 = position + right * num2;
        get.SetColor(this.LookAndFeel.XAxisColor);
        get.SetPass(0);
        GLRenderer.DrawLine3D(startPoint1, endPoint1);
        Vector3 startPoint2 = position - up * num2;
        Vector3 endPoint2 = position + up * num2;
        get.SetColor(this.LookAndFeel.YAxisColor);
        get.SetPass(0);
        GLRenderer.DrawLine3D(startPoint2, endPoint2);
        Vector3 startPoint3 = position - forward * num2;
        Vector3 endPoint3 = position + forward * num2;
        get.SetColor(this.LookAndFeel.ZAxisColor);
        get.SetPass(0);
        GLRenderer.DrawLine3D(startPoint3, endPoint3);
      }
    }

    public GizmoScaleGuide()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._lookAndFeel = new GizmoScaleGuideLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
