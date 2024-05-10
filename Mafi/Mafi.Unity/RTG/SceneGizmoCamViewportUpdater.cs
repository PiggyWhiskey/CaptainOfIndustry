// Decompiled with JetBrains decompiler
// Type: RTG.SceneGizmoCamViewportUpdater
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SceneGizmoCamViewportUpdater : ISceneGizmoCamViewportUpdater
  {
    private SceneGizmo _sceneGizmo;

    public SceneGizmoCamViewportUpdater(SceneGizmo sceneGizmo)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sceneGizmo = sceneGizmo;
    }

    public void Update(RTSceneGizmoCamera sceneGizmoCamera)
    {
      SceneGizmoLookAndFeel lookAndFeel = this._sceneGizmo.LookAndFeel;
      Vector2 screenOffset = lookAndFeel.ScreenOffset;
      Rect pixelRect = sceneGizmoCamera.SceneCamera.pixelRect;
      Vector2 switchLabelRectSize = lookAndFeel.CalculateMaxPrjSwitchLabelRectSize();
      bool switchLabelVisible = lookAndFeel.IsCamPrjSwitchLabelVisible;
      float screenSize = lookAndFeel.ScreenSize;
      if (lookAndFeel.ScreenCorner == SceneGizmoScreenCorner.TopRight)
        sceneGizmoCamera.Camera.pixelRect = new Rect(pixelRect.xMax - screenSize + screenOffset.x, pixelRect.yMax - screenSize + screenOffset.y, screenSize, screenSize);
      else if (lookAndFeel.ScreenCorner == SceneGizmoScreenCorner.TopLeft)
        sceneGizmoCamera.Camera.pixelRect = new Rect(pixelRect.xMin + screenOffset.x, pixelRect.yMax - screenSize + screenOffset.y, screenSize, screenSize);
      else if (lookAndFeel.ScreenCorner == SceneGizmoScreenCorner.BottomRight)
        sceneGizmoCamera.Camera.pixelRect = new Rect(pixelRect.xMax - screenSize + screenOffset.x, pixelRect.yMin + (switchLabelVisible ? switchLabelRectSize.y + 1f : 0.0f) + screenOffset.y, screenSize, screenSize);
      else
        sceneGizmoCamera.Camera.pixelRect = new Rect(pixelRect.xMin + screenOffset.x, pixelRect.yMin + (switchLabelVisible ? switchLabelRectSize.y + 1f : 0.0f) + screenOffset.y, screenSize, screenSize);
    }
  }
}
