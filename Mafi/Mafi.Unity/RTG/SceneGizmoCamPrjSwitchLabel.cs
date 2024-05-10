// Decompiled with JetBrains decompiler
// Type: RTG.SceneGizmoCamPrjSwitchLabel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SceneGizmoCamPrjSwitchLabel
  {
    private SceneGizmo _sceneGizmo;
    private GizmoHandle _handle;
    private QuadShape2D _labelQuad;

    public GizmoHandle Handle => this._handle;

    public int Id => this._handle.Id;

    public SceneGizmoCamPrjSwitchLabel(SceneGizmo sceneGizmo)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._labelQuad = new QuadShape2D();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sceneGizmo = sceneGizmo;
      this._handle = this._sceneGizmo.Gizmo.CreateHandle(GizmoHandleId.SceneGizmoCamPrjSwitchLabel);
      this._handle.Add2DShape((Shape2D) this._labelQuad);
      sceneGizmo.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      sceneGizmo.Gizmo.PreHandlePicked += new GizmoPreHandlePickedHandler(this.OnGizmoHandlePicked);
    }

    public void OnGUI()
    {
      SceneGizmoLookAndFeel lookAndFeel = this._sceneGizmo.LookAndFeel;
      Camera camera = this._sceneGizmo.SceneGizmoCamera.Camera;
      if (!lookAndFeel.IsCamPrjSwitchLabelVisible)
        return;
      if ((Object) this._sceneGizmo.SceneGizmoCamera.SceneCamera != (Object) MonoSingleton<RTFocusCamera>.Get.TargetCamera || !MonoSingleton<RTFocusCamera>.Get.IsDoingProjectionSwitch)
      {
        Texture2D texture2D = camera.orthographic ? lookAndFeel.CamOrthoModeLabelTexture : lookAndFeel.CamPerspModeLabelTexture;
        GUIEx.PushColor(lookAndFeel.CamPrjSwitchLabelTint);
        Rect position = RectEx.FromTexture2D(texture2D).PlaceBelowCenterHrz(camera.pixelRect).InvertScreenY();
        position.center = new Vector2(position.center.x, (float) (Screen.height - 1) - this._labelQuad.Center.y);
        GUI.DrawTexture(position, (Texture) texture2D);
        GUIEx.PopColor();
      }
      else
      {
        Texture2D modeLabelTexture1 = lookAndFeel.CamOrthoModeLabelTexture;
        Texture2D modeLabelTexture2 = lookAndFeel.CamPerspModeLabelTexture;
        if (MonoSingleton<RTFocusCamera>.Get.PrjSwitchTransitionType == CameraPrjSwitchTransition.Type.ToPerspective)
        {
          modeLabelTexture1 = lookAndFeel.CamPerspModeLabelTexture;
          modeLabelTexture2 = lookAndFeel.CamOrthoModeLabelTexture;
        }
        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0.0f, lookAndFeel.CamPrjSwitchLabelTint.a, 1f, 0.0f);
        float newAlpha1 = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, lookAndFeel.CamPrjSwitchLabelTint.a).Evaluate(MonoSingleton<RTFocusCamera>.Get.PrjSwitchProgress);
        float newAlpha2 = animationCurve.Evaluate(MonoSingleton<RTFocusCamera>.Get.PrjSwitchProgress);
        GUIEx.PushColor(lookAndFeel.CamPrjSwitchLabelTint.KeepAllButAlpha(newAlpha2));
        Rect position1 = RectEx.FromTexture2D(modeLabelTexture2).PlaceBelowCenterHrz(camera.pixelRect).InvertScreenY();
        position1.center = new Vector2(position1.center.x, (float) (Screen.height - 1) - this._labelQuad.Center.y);
        GUI.DrawTexture(position1, (Texture) modeLabelTexture2);
        GUIEx.PopColor();
        GUIEx.PushColor(lookAndFeel.CamPrjSwitchLabelTint.KeepAllButAlpha(newAlpha1));
        Rect position2 = RectEx.FromTexture2D(modeLabelTexture1).PlaceBelowCenterHrz(camera.pixelRect).InvertScreenY();
        position2.center = new Vector2(position2.center.x, (float) (Screen.height - 1) - this._labelQuad.Center.y);
        GUI.DrawTexture(position2, (Texture) modeLabelTexture1);
        GUIEx.PopColor();
      }
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      this._handle.Is2DVisible = this._sceneGizmo.LookAndFeel.IsCamPrjSwitchLabelVisible;
      this.UpdateTransform();
    }

    private void UpdateTransform()
    {
      SceneGizmoLookAndFeel lookAndFeel = this._sceneGizmo.LookAndFeel;
      Rect rect = (RectEx.FromTexture2D(this._sceneGizmo.SceneGizmoCamera.Camera.orthographic ? lookAndFeel.CamOrthoModeLabelTexture : lookAndFeel.CamPerspModeLabelTexture) with
      {
        size = lookAndFeel.CalculateMaxPrjSwitchLabelRectSize()
      }).PlaceBelowCenterHrz(this._sceneGizmo.SceneGizmoCamera.Camera.pixelRect);
      this._labelQuad.Center = rect.center;
      this._labelQuad.Size = rect.size;
    }

    private void OnGizmoHandlePicked(Gizmo gizmo, int handleId)
    {
      if (handleId != this._handle.Id)
        return;
      MonoSingleton<RTFocusCamera>.Get.PerformProjectionSwitch();
    }
  }
}
