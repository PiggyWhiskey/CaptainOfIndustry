// Decompiled with JetBrains decompiler
// Type: RTG.GizmoThinQuad3DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoThinQuad3DBorderController : GizmoQuad3DBorderController
  {
    public GizmoThinQuad3DBorderController(GizmoQuad3DBorderControllerData data)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(data);
    }

    public override void UpdateHandles()
    {
      GizmoHandle targetHandle = this._data.TargetHandle;
      targetHandle.Set3DShapeVisible(this._data.BorderQuadIndex, this._data.Border.IsVisible);
      targetHandle.Set3DShapeVisible(this._data.TopBoxIndex, false);
      targetHandle.Set3DShapeVisible(this._data.RightBoxIndex, false);
      targetHandle.Set3DShapeVisible(this._data.BottomBoxIndex, false);
      targetHandle.Set3DShapeVisible(this._data.LeftBoxIndex, false);
      targetHandle.Set3DShapeVisible(this._data.TopLeftBoxIndex, false);
      targetHandle.Set3DShapeVisible(this._data.TopRightBoxIndex, false);
      targetHandle.Set3DShapeVisible(this._data.BottomRightBoxIndex, false);
      targetHandle.Set3DShapeVisible(this._data.BottomLeftBoxIndex, false);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      QuadShape3D borderQuad = this._data.BorderQuad;
      borderQuad.WireEps = this._data.PlaneSlider.Settings.BorderLineHoverEps * zoomFactor;
      borderQuad.ExtrudeEps = borderQuad.WireEps;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      QuadShape3D targetQuad = this._data.TargetQuad;
      QuadShape3D borderQuad = this._data.BorderQuad;
      borderQuad.Center = targetQuad.Center;
      borderQuad.Rotation = targetQuad.Rotation;
      borderQuad.Size = targetQuad.Size;
    }
  }
}
