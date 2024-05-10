// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.WheelsAnimator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class WheelsAnimator
  {
    /// <summary>Axis to rotate wheels around when steering.</summary>
    private readonly Vector3 m_steeringRotationAxis;
    /// <summary>Axis to rotate wheels around when rolling.</summary>
    private readonly Vector3 m_rollingRotationAxis;
    private readonly Transform[] m_steeringWheelsTransforms;
    private readonly Transform[] m_staticWheelsTransforms;
    /// <summary>
    /// How many degrees to rotate per one unit of speed (in in RelTile1f). Float used for higher precision.
    /// </summary>
    private readonly float m_wheelRotationPerSpeedDeg;
    private Quaternion m_nextSteeringWheelsRotation;
    private Quaternion m_nextStaticWheelsRotation;

    public WheelsAnimator(
      GameObject vehicleGo,
      ImmutableArray<string> steeringWheelsGoPaths,
      ImmutableArray<string> staticWheelsGoPaths,
      Vector3 steeringRotationAxis,
      Vector3 rollingRotationAxis,
      float wheelsDiameterMeters)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_steeringRotationAxis = steeringRotationAxis.CheckNotDefaultStruct<Vector3>();
      this.m_rollingRotationAxis = rollingRotationAxis.CheckNotDefaultStruct<Vector3>();
      this.m_nextSteeringWheelsRotation = this.m_nextStaticWheelsRotation = Quaternion.AngleAxis(0.0f, rollingRotationAxis);
      Transform transform = vehicleGo.transform;
      this.m_steeringWheelsTransforms = new Transform[steeringWheelsGoPaths.Length];
      for (int index = 0; index < this.m_steeringWheelsTransforms.Length; ++index)
        this.m_steeringWheelsTransforms[index] = transform.Find(steeringWheelsGoPaths[index]);
      this.m_staticWheelsTransforms = new Transform[staticWheelsGoPaths.Length];
      for (int index = 0; index < this.m_staticWheelsTransforms.Length; ++index)
        this.m_staticWheelsTransforms[index] = transform.Find(staticWheelsGoPaths[index]);
      this.m_wheelRotationPerSpeedDeg = 360f / (float) ((double) wheelsDiameterMeters * 6.2831854820251465 / 2.0);
    }

    public void Sync(AngleDegrees1f steeringAngle, RelTile1f vehicleSpeed)
    {
      Quaternion quaternion = Quaternion.AngleAxis(steeringAngle.ToUnityAngleDegrees(), this.m_steeringRotationAxis);
      this.m_nextStaticWheelsRotation *= Quaternion.AngleAxis(vehicleSpeed.ToUnityUnits() * this.m_wheelRotationPerSpeedDeg, this.m_rollingRotationAxis);
      this.m_nextSteeringWheelsRotation = quaternion * this.m_nextStaticWheelsRotation;
    }

    public void RenderUpdate(GameTime time)
    {
      if (this.m_steeringWheelsTransforms.IsNotEmpty<Transform>())
      {
        Quaternion quaternion = Quaternion.Slerp(this.m_steeringWheelsTransforms[0].localRotation, this.m_nextSteeringWheelsRotation, time.RelativeT);
        foreach (Transform steeringWheelsTransform in this.m_steeringWheelsTransforms)
          steeringWheelsTransform.localRotation = quaternion;
      }
      if (!this.m_staticWheelsTransforms.IsNotEmpty<Transform>())
        return;
      Quaternion quaternion1 = Quaternion.Slerp(this.m_staticWheelsTransforms[0].localRotation, this.m_nextStaticWheelsRotation, time.RelativeT);
      foreach (Transform staticWheelsTransform in this.m_staticWheelsTransforms)
        staticWheelsTransform.localRotation = quaternion1;
    }
  }
}
