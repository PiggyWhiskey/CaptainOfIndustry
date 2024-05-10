// Decompiled with JetBrains decompiler
// Type: RTG.GizmoThinQuad2DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoThinQuad2DBorderController : GizmoQuad2DBorderController
  {
    public GizmoThinQuad2DBorderController(GizmoQuad2DBorderControllerData data)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(data);
    }

    public override void UpdateHandles()
    {
      this._data.TargetHandle.Set2DShapeVisible(this._data.BorderQuadIndex, this._data.Border.IsVisible);
    }

    public override void UpdateEpsilons()
    {
      this._data.BorderQuad.WireEps = this._data.PlaneSlider.Settings.BorderLineHoverEps;
    }

    public override void UpdateTransforms()
    {
      QuadShape2D targetQuad = this._data.TargetQuad;
      QuadShape2D borderQuad = this._data.BorderQuad;
      borderQuad.Center = targetQuad.Center;
      borderQuad.RotationDegrees = targetQuad.RotationDegrees;
      borderQuad.Size = targetQuad.Size;
    }
  }
}
