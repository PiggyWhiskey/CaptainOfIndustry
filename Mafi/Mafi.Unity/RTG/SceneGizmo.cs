// Decompiled with JetBrains decompiler
// Type: RTG.SceneGizmo
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
  [Serializable]
  public class SceneGizmo : GizmoBehaviour, ISceneGizmo
  {
    private SceneGizmoCamPrjSwitchLabel _camPrjSwitchLabel;
    private SceneGizmoMidCap _midAxisHandle;
    private SceneGizmoAxisCap[] _axesHandles;
    private List<SceneGizmoCap> _renderSortedHandles;
    private RTSceneGizmoCamera _sceneGizmoCamera;
    [SerializeField]
    private SceneGizmoLookAndFeel _lookAndFeel;
    [SerializeField]
    private SceneGizmoLookAndFeel _sharedLookAndFeel;

    public RTSceneGizmoCamera SceneGizmoCamera => this._sceneGizmoCamera;

    public Gizmo OwnerGizmo => this.Gizmo;

    public Camera SceneCamera => this._sceneGizmoCamera.SceneCamera;

    public SceneGizmoLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel == null ? this._lookAndFeel : this._sharedLookAndFeel;
    }

    public SceneGizmoLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set => this._sharedLookAndFeel = value;
    }

    public override void OnAttached()
    {
      this._sceneGizmoCamera = MonoSingleton<RTGizmosEngine>.Get.CreateSceneGizmoCamera(MonoSingleton<RTFocusCamera>.Get.TargetCamera, (ISceneGizmoCamViewportUpdater) new SceneGizmoCamViewportUpdater(this));
      this._midAxisHandle = new SceneGizmoMidCap(this);
      this._renderSortedHandles.Add((SceneGizmoCap) this._midAxisHandle);
      AxisDescriptor[] axisDescriptorArray = new AxisDescriptor[6]
      {
        new AxisDescriptor(0, AxisSign.Positive),
        new AxisDescriptor(1, AxisSign.Positive),
        new AxisDescriptor(2, AxisSign.Positive),
        new AxisDescriptor(0, AxisSign.Negative),
        new AxisDescriptor(1, AxisSign.Negative),
        new AxisDescriptor(2, AxisSign.Negative)
      };
      int[] numArray = new int[6]
      {
        GizmoHandleId.SceneGizmoPositiveXAxis,
        GizmoHandleId.SceneGizmoPositiveYAxis,
        GizmoHandleId.SceneGizmoPositiveZAxis,
        GizmoHandleId.SceneGizmoNegativeXAxis,
        GizmoHandleId.SceneGizmoNegativeYAxis,
        GizmoHandleId.SceneGizmoNegativeZAxis
      };
      for (int index = 0; index < axisDescriptorArray.Length; ++index)
      {
        this._axesHandles[index] = new SceneGizmoAxisCap(this, numArray[index], axisDescriptorArray[index]);
        this._renderSortedHandles.Add((SceneGizmoCap) this._axesHandles[index]);
      }
      this._camPrjSwitchLabel = new SceneGizmoCamPrjSwitchLabel(this);
      this.Gizmo.Transform.Position3D = this._sceneGizmoCamera.LookAtPoint;
      this.Gizmo.Transform.Rotation3D = Quaternion.identity;
      this.Gizmo.GenericHoverPriority.MakeHighest();
      this.Gizmo.HoverPriority2D.MakeHighest();
      this.Gizmo.HoverPriority3D.MakeHighest();
    }

    public override void OnGUI() => this._camPrjSwitchLabel.OnGUI();

    public override void OnGizmoRender(Camera camera)
    {
      if ((UnityEngine.Object) camera != (UnityEngine.Object) this._sceneGizmoCamera.Camera)
        return;
      Vector3 cameraPos = this._sceneGizmoCamera.WorldPosition;
      this._renderSortedHandles.Sort((Comparison<SceneGizmoCap>) ((h0, h1) =>
      {
        float sqrMagnitude = (h0.Position - cameraPos).sqrMagnitude;
        return (h1.Position - cameraPos).sqrMagnitude.CompareTo(sqrMagnitude);
      }));
      foreach (SceneGizmoCap renderSortedHandle in this._renderSortedHandles)
        renderSortedHandle.Render(this._sceneGizmoCamera.Camera);
    }

    public SceneGizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._axesHandles = new SceneGizmoAxisCap[6];
      this._renderSortedHandles = new List<SceneGizmoCap>(7);
      this._lookAndFeel = new SceneGizmoLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
