// Decompiled with JetBrains decompiler
// Type: RTG.GizmoBoxQuad3DBorderController
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
  public class GizmoBoxQuad3DBorderController : GizmoQuad3DBorderController
  {
    public GizmoBoxQuad3DBorderController(GizmoQuad3DBorderControllerData data)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(data);
    }

    public override void UpdateHandles()
    {
      GizmoHandle targetHandle = this._data.TargetHandle;
      if (this._data.Border.IsVisible)
      {
        targetHandle.Set3DShapeVisible(this._data.TopBoxIndex, true);
        targetHandle.Set3DShapeVisible(this._data.RightBoxIndex, true);
        targetHandle.Set3DShapeVisible(this._data.BottomBoxIndex, true);
        targetHandle.Set3DShapeVisible(this._data.LeftBoxIndex, true);
        targetHandle.Set3DShapeVisible(this._data.TopLeftBoxIndex, true);
        targetHandle.Set3DShapeVisible(this._data.TopRightBoxIndex, true);
        targetHandle.Set3DShapeVisible(this._data.BottomRightBoxIndex, true);
        targetHandle.Set3DShapeVisible(this._data.BottomLeftBoxIndex, true);
      }
      else
      {
        targetHandle.Set3DShapeVisible(this._data.TopBoxIndex, false);
        targetHandle.Set3DShapeVisible(this._data.RightBoxIndex, false);
        targetHandle.Set3DShapeVisible(this._data.BottomBoxIndex, false);
        targetHandle.Set3DShapeVisible(this._data.LeftBoxIndex, false);
        targetHandle.Set3DShapeVisible(this._data.TopLeftBoxIndex, false);
        targetHandle.Set3DShapeVisible(this._data.TopRightBoxIndex, false);
        targetHandle.Set3DShapeVisible(this._data.BottomRightBoxIndex, false);
        targetHandle.Set3DShapeVisible(this._data.BottomLeftBoxIndex, false);
      }
      targetHandle.Set3DShapeVisible(this._data.BorderQuadIndex, false);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      Vector3 vector3 = Vector3Ex.FromValue(this._data.PlaneSlider.Settings.BorderBoxHoverEps * zoomFactor);
      this._data.TopLeftBox.SizeEps = vector3;
      this._data.TopRightBox.SizeEps = vector3;
      this._data.BottomRightBox.SizeEps = vector3;
      this._data.BottomLeftBox.SizeEps = vector3;
      this._data.TopBox.SizeEps = vector3;
      this._data.BottomBox.SizeEps = vector3;
      this._data.LeftBox.SizeEps = vector3;
      this._data.RightBox.SizeEps = vector3;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      GizmoPlaneSlider3DLookAndFeel lookAndFeel = this._data.PlaneSlider.LookAndFeel;
      Vector3 right = this._data.TargetQuad.Right;
      Vector3 up = this._data.TargetQuad.Up;
      Vector3 normal = this._data.TargetQuad.Normal;
      float width = this._data.TargetQuad.Width;
      float height = this._data.TargetQuad.Height;
      List<Vector3> cornerPoints = this._data.TargetQuad.GetCornerPoints();
      Vector3 vector3_1 = cornerPoints[0];
      Vector3 vector3_2 = cornerPoints[1];
      Vector3 vector3_3 = cornerPoints[2];
      Vector3 vector3_4 = cornerPoints[3];
      float realBoxHeight = this._data.Border.GetRealBoxHeight(zoomFactor);
      float realBoxDepth = this._data.Border.GetRealBoxDepth(zoomFactor);
      BoxShape3D topLeftBox = this._data.TopLeftBox;
      topLeftBox.AlignHeight(normal);
      topLeftBox.AlignWidth(right);
      topLeftBox.Width = realBoxDepth;
      topLeftBox.Height = realBoxHeight;
      topLeftBox.Depth = realBoxDepth;
      topLeftBox.SetFaceCenter(BoxFace.Left, vector3_1 - up * topLeftBox.GetSizeAlongDirection(up) * 0.5f);
      BoxShape3D topRightBox = this._data.TopRightBox;
      topRightBox.AlignHeight(normal);
      topRightBox.AlignWidth(right);
      topRightBox.Width = realBoxDepth;
      topRightBox.Height = realBoxHeight;
      topRightBox.Depth = realBoxDepth;
      topRightBox.SetFaceCenter(BoxFace.Right, vector3_2 - up * topRightBox.GetSizeAlongDirection(up) * 0.5f);
      BoxShape3D bottomRightBox = this._data.BottomRightBox;
      bottomRightBox.AlignHeight(normal);
      bottomRightBox.AlignWidth(right);
      bottomRightBox.Width = realBoxDepth;
      bottomRightBox.Height = realBoxHeight;
      bottomRightBox.Depth = realBoxDepth;
      bottomRightBox.SetFaceCenter(BoxFace.Right, vector3_3 + up * bottomRightBox.GetSizeAlongDirection(up) * 0.5f);
      BoxShape3D bottomLeftBox = this._data.BottomLeftBox;
      bottomLeftBox.AlignHeight(normal);
      bottomLeftBox.AlignWidth(right);
      bottomLeftBox.Width = realBoxDepth;
      bottomLeftBox.Height = realBoxHeight;
      bottomLeftBox.Depth = realBoxDepth;
      bottomLeftBox.SetFaceCenter(BoxFace.Left, vector3_4 + up * bottomLeftBox.GetSizeAlongDirection(up) * 0.5f);
      BoxShape3D topBox = this._data.TopBox;
      topBox.AlignHeight(normal);
      topBox.AlignWidth(right);
      topBox.Width = width - 2f * topLeftBox.GetSizeAlongDirection(right);
      topBox.Height = realBoxHeight;
      topBox.Depth = realBoxDepth;
      topBox.SetFaceCenter(BoxFace.Left, topLeftBox.GetFaceCenter(BoxFace.Right));
      BoxShape3D rightBox = this._data.RightBox;
      rightBox.AlignHeight(normal);
      rightBox.AlignWidth(up);
      rightBox.Width = height - 2f * topRightBox.GetSizeAlongDirection(up);
      rightBox.Height = realBoxHeight;
      rightBox.Depth = realBoxDepth;
      rightBox.SetFaceCenter(BoxFace.Right, topRightBox.GetFaceCenter(BoxFace.Back));
      BoxShape3D bottomBox = this._data.BottomBox;
      bottomBox.AlignHeight(normal);
      bottomBox.AlignWidth(right);
      bottomBox.Width = width - 2f * bottomRightBox.GetSizeAlongDirection(right);
      bottomBox.Height = realBoxHeight;
      bottomBox.Depth = realBoxDepth;
      bottomBox.SetFaceCenter(BoxFace.Left, bottomLeftBox.GetFaceCenter(BoxFace.Right));
      BoxShape3D leftBox = this._data.LeftBox;
      leftBox.AlignHeight(normal);
      leftBox.AlignWidth(up);
      leftBox.Width = height - 2f * topLeftBox.GetSizeAlongDirection(up);
      leftBox.Height = realBoxHeight;
      leftBox.Depth = realBoxDepth;
      leftBox.SetFaceCenter(BoxFace.Right, topLeftBox.GetFaceCenter(BoxFace.Back));
    }
  }
}
