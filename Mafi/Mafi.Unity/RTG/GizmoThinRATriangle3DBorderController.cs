// Decompiled with JetBrains decompiler
// Type: RTG.GizmoThinRATriangle3DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoThinRATriangle3DBorderController : GizmoRATriangle3DBorderController
  {
    public GizmoThinRATriangle3DBorderController(
      GizmoRATriangle3DBorderControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderTriangleIndex, this._data.Border.IsVisible);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.BorderTriangle.WireEps = zoomFactor * this._data.PlaneSlider.Settings.BorderLineHoverEps;
      this._data.BorderTriangle.ExtrudeEps = this._data.BorderTriangle.WireEps;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      RightAngTriangle3D targetTriangle = this._data.TargetTriangle;
      RightAngTriangle3D borderTriangle = this._data.BorderTriangle;
      borderTriangle.Rotation = targetTriangle.Rotation;
      borderTriangle.RightAngleCorner = targetTriangle.RightAngleCorner;
      borderTriangle.XLength = targetTriangle.XLength;
      borderTriangle.XLengthSign = targetTriangle.XLengthSign;
      borderTriangle.YLength = targetTriangle.YLength;
      borderTriangle.YLengthSign = targetTriangle.YLengthSign;
    }
  }
}
