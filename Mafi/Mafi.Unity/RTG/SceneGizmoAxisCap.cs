// Decompiled with JetBrains decompiler
// Type: RTG.SceneGizmoAxisCap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SceneGizmoAxisCap : SceneGizmoCap
  {
    private AxisDescriptor _axisDesc;
    private BoxFace _midAxisBoxFace;
    private GizmoTransform _zoomFactorTransform;
    private ColorRef _color;
    private ColorTransition _colorTransition;
    private Texture2D _labelTexture;

    public SceneGizmoAxisCap(SceneGizmo sceneGizmo, int id, AxisDescriptor gizmoAxisDesc)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._zoomFactorTransform = new GizmoTransform();
      this._color = new ColorRef();
      // ISSUE: explicit constructor call
      base.\u002Ector(sceneGizmo, id);
      this._axisDesc = gizmoAxisDesc;
      this._midAxisBoxFace = this._axisDesc.GetAssociatedBoxFace();
      this._cap.SetZoomFactorTransform(this._zoomFactorTransform);
      if (this._axisDesc.IsPositive)
        this._labelTexture = this._axisDesc.Index != 0 ? (this._axisDesc.Index != 1 ? Singleton<TexturePool>.Get.ZAxisLabel : Singleton<TexturePool>.Get.YAxisLabel) : Singleton<TexturePool>.Get.XAxisLabel;
      this._colorTransition = new ColorTransition(this._color);
      this._cap.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      this._cap.Gizmo.PreHandlePicked += new GizmoPreHandlePickedHandler(this.OnGizmoHandlePicked);
      this._sceneGizmo.LookAndFeel.ConnectAxisCapLookAndFeel(this._cap, this._axisDesc.Index, this._axisDesc.Sign);
    }

    public override void Render(Camera camera)
    {
      SceneGizmoLookAndFeel lookAndFeel = this._sceneGizmo.LookAndFeel;
      RTSceneGizmoCamera sceneGizmoCamera = this._sceneGizmo.SceneGizmoCamera;
      this._cap.Render(camera);
      if (!this._axisDesc.IsPositive)
        return;
      GizmoLabelMaterial get = Singleton<GizmoLabelMaterial>.Get;
      get.SetZWriteEnabled(false);
      get.SetZTestLessEqual();
      get.SetColor(lookAndFeel.AxesLabelTint.KeepAllButAlpha(this._color.Value.a));
      get.SetTexture(this._labelTexture);
      get.SetPass(0);
      Vector3 axis3D = this._sceneGizmo.Gizmo.Transform.GetAxis3D(this._axisDesc);
      Vector3 s = Vector3Ex.FromValue(lookAndFeel.GetAxesLabelWorldSize(sceneGizmoCamera.Camera, this._cap.Position));
      Vector3 position = this._cap.Position + axis3D * (s.x * 0.5f);
      Vector2 screenPoint1 = (Vector2) sceneGizmoCamera.Camera.WorldToScreenPoint(position);
      Vector2 screenPoint2 = (Vector2) sceneGizmoCamera.Camera.WorldToScreenPoint(this._sceneGizmo.SceneGizmoCamera.LookAtPoint);
      Vector2 normalized = (screenPoint1 - screenPoint2).normalized;
      float num = Mathf.Abs(sceneGizmoCamera.Look.AbsDot(axis3D));
      Vector2 vector2 = screenPoint1 + Vector2.Scale(normalized, Vector2Ex.FromValue(lookAndFeel.AxisLabelScreenSize)) * num;
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitQuadXY, Matrix4x4.TRS(sceneGizmoCamera.Camera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, (position - sceneGizmoCamera.WorldPosition).magnitude)), Quaternion.LookRotation(sceneGizmoCamera.Look, sceneGizmoCamera.Up), s));
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      this._sceneGizmo.LookAndFeel.ConnectAxisCapLookAndFeel(this._cap, this._axisDesc.Index, this._axisDesc.Sign);
      this.UpdateColor();
      this.UpdateHoverPermission();
      this.UpdateTransform(gizmo.FocusCamera);
    }

    private void UpdateHoverPermission()
    {
      if (this._colorTransition.IsActive || this._colorTransition.TransitionState == ColorTransition.State.CompleteFadeOut)
        this._cap.SetHoverable(false);
      else
        this._cap.SetHoverable(true);
    }

    private void UpdateColor()
    {
      SceneGizmoLookAndFeel lookAndFeel = this._sceneGizmo.LookAndFeel;
      Color color = lookAndFeel.GetAxisCapColor(this._axisDesc.Index, this._axisDesc.Sign);
      if (this._cap.IsHovered)
        color = lookAndFeel.HoveredColor;
      ColorTransition.State transitionState = this._colorTransition.TransitionState;
      if ((double) this._sceneGizmo.Gizmo.Transform.GetAxis3D(this._axisDesc).AbsDot(this._sceneGizmo.SceneGizmoCamera.Look) > (double) lookAndFeel.AxisCamAlignFadeOutThreshold)
      {
        if (transitionState != ColorTransition.State.CompleteFadeOut && transitionState != ColorTransition.State.FadingOut)
        {
          this._colorTransition.DurationInSeconds = lookAndFeel.AxisCamAlignFadeOutDuration;
          this._colorTransition.FadeOutColor = color.KeepAllButAlpha(lookAndFeel.AxisCamAlignFadeOutAlpha);
          this._colorTransition.BeginFadeOut(true);
        }
      }
      else if (transitionState != ColorTransition.State.FadingIn && transitionState != ColorTransition.State.CompleteFadeIn && transitionState != ColorTransition.State.Ready)
      {
        this._colorTransition.DurationInSeconds = lookAndFeel.AxisCamAlignFadeOutDuration;
        this._colorTransition.FadeInColor = color;
        this._colorTransition.BeginFadeIn(true);
      }
      else
        this._color.Value = color;
      this._colorTransition.Update(Time.deltaTime);
      this._cap.OverrideColor.IsActive = true;
      this._cap.OverrideColor.Color = this._color.Value;
    }

    private void UpdateTransform(Camera camera)
    {
      Vector3 lookAtPoint = this._sceneGizmo.SceneGizmoCamera.LookAtPoint;
      RTSceneGizmoCamera sceneGizmoCamera = this._sceneGizmo.SceneGizmoCamera;
      Vector3 axis3D = this._sceneGizmo.Gizmo.Transform.GetAxis3D(this._axisDesc);
      this._zoomFactorTransform.Position3D = lookAtPoint;
      float zoomFactor = this._cap.GetZoomFactor(camera);
      Vector3 boxSize = this._sceneGizmo.LookAndFeel.MidCapType == GizmoCap3DType.Box ? Vector3Ex.FromValue(this._sceneGizmo.LookAndFeel.MidCapBoxSize * zoomFactor) : Vector3Ex.FromValue(this._sceneGizmo.LookAndFeel.MidCapSphereRadius * 2f * zoomFactor);
      Vector3 sliderEndPt = BoxMath.CalcBoxFaceCenter(lookAtPoint, boxSize, Quaternion.identity, this._midAxisBoxFace);
      this._cap.CapSlider3DInvert(axis3D, sliderEndPt);
    }

    private void OnGizmoHandlePicked(Gizmo gizmo, int handleId)
    {
      if (handleId != this.HandleId)
        return;
      MonoSingleton<RTFocusCamera>.Get.PerformRotationSwitch(Quaternion.LookRotation(-this._sceneGizmo.Gizmo.Transform.GetAxis3D(this._axisDesc), Vector3.up));
    }
  }
}
