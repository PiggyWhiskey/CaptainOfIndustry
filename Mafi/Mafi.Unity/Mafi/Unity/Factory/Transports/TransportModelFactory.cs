// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.TransportModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Gfx;
using Mafi.Curves;
using Mafi.Numerics;
using Mafi.Unity.InstancedRendering;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  /// <summary>Procedural generation of transport models.</summary>
  /// <remarks>
  /// This code is used for both the instanced and non-instanced paths. They diverge after cross sections
  /// are created. Transport models are generated on the fly in both instanced and non-instanced, with
  /// the hope that this will help future-proof against trajectories we may not support at the time of
  /// writing.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportModelFactory
  {
    private static readonly float COLLIDER_RADIUS;
    private readonly AssetsDb m_assetsDb;
    private readonly MeshBuilder m_builderStatic;
    private readonly MeshBuilder m_builderMoving;

    public TransportModelFactory(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_builderStatic = new MeshBuilder();
      this.m_builderMoving = new MeshBuilder();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
    }

    /// <summary>
    /// Creates transport model with given pivots and start/end directions.
    /// </summary>
    public Option<GameObject> CreateModel(
      TransportProto proto,
      ImmutableArray<Tile3i> pivots,
      out string error,
      RelTile3i? startDir = null,
      RelTile3i? endDir = null,
      bool allowDenormalizedStartEndDirections = false,
      bool noColliders = false,
      bool singleMesh = false,
      bool noFlowIndicators = false)
    {
      TransportTrajectory trajectory;
      return TransportTrajectory.TryCreateFromPivots(proto, pivots, startDir, endDir, out trajectory, out error, allowDenormalizedStartEndDirections) ? (Option<GameObject>) this.CreateModel(trajectory, noColliders, singleMesh, noFlowIndicators) : Option<GameObject>.None;
    }

    /// <summary>Creates transport model based on given trajectory.</summary>
    public GameObject CreateModel(
      TransportTrajectory trajectory,
      bool noColliders = false,
      bool singleMesh = false,
      bool noFlowIndicators = false,
      Material customSharedMaterial = null,
      Vector3? customOrigin = null)
    {
      Assert.That<bool>(this.m_builderStatic.IsEmpty).IsTrue("Non-empty static mesh builder.");
      Assert.That<bool>(this.m_builderMoving.IsEmpty).IsTrue("Non-empty moving mesh builder.");
      TransportProto transportProto = trajectory.TransportProto;
      if (!noFlowIndicators)
      {
        if (singleMesh)
        {
          Assert.Fail("Flow indicators are not supported for single mesh.");
          noFlowIndicators = true;
        }
        if (transportProto.Graphics.FlowIndicator.IsNone)
          noFlowIndicators = true;
      }
      GameObject model = new GameObject(transportProto.Id.Value);
      Vector3 vector3 = customOrigin ?? trajectory.Pivots[trajectory.Pivots.Length / 2].ToGroundCenterVector3();
      model.transform.localPosition = vector3;
      ImmutableArray<TransportFlowIndicatorPose> immutableArray = noFlowIndicators ? ImmutableArray<TransportFlowIndicatorPose>.Empty : trajectory.FlowIndicatorsPoses;
      ImmutableArray<TransportModelFactory.CrossSection> crossSections = TransportModelFactory.createCrossSections(trajectory.Curve, transportProto, -vector3, immutableArray);
      TransportModelFactory.createMesh(transportProto, crossSections, this.m_builderStatic, this.m_builderMoving);
      if (singleMesh)
      {
        this.m_builderStatic.AddMesh(this.m_builderMoving);
        this.m_builderMoving.Clear();
        this.m_builderStatic.UpdateGoAndClear(model);
        model.GetComponent<MeshRenderer>().sharedMaterial = customSharedMaterial ?? this.m_assetsDb.GetSharedMaterial(transportProto.Graphics.MaterialPath);
      }
      else
      {
        if (this.m_builderMoving.IsNotEmpty)
        {
          GameObject entityGo = new GameObject("moving");
          entityGo.transform.SetParent(model.transform, false);
          this.m_builderMoving.UpdateGoAndClear(entityGo);
          entityGo.GetComponent<MeshRenderer>().sharedMaterial = customSharedMaterial ?? this.m_assetsDb.GetSharedMaterial(transportProto.Graphics.MaterialPath);
        }
        if (this.m_builderStatic.IsNotEmpty)
        {
          this.m_builderStatic.UpdateGoAndClear(model);
          model.GetComponent<MeshRenderer>().sharedMaterial = customSharedMaterial ?? this.m_assetsDb.GetSharedMaterial(transportProto.Graphics.MaterialPath);
        }
        if (!noFlowIndicators)
          this.addFlowIndicators(immutableArray, transportProto, -vector3, model);
      }
      if (!noColliders)
        TransportModelFactory.AddColliders(model, trajectory.Pivots, -vector3);
      return model;
    }

    /// <summary>
    /// Adds any unseen meshes to our list of meshes, and gives a list of sections contained within
    /// this transport.
    /// </summary>
    /// <remarks>
    /// Dealing with "twist" of up vector on vertical pipes is a pain. If we wanted to simplify this
    /// code we could have an explicit graphical discontinuity on these sections which could then
    /// be used to hide anything but "normal" twist.
    /// </remarks>
    public void GetTransportMeshData(
      TransportTrajectory trajectory,
      Lyst<KeyValuePair<long, Mesh>> staticMeshes,
      Lyst<KeyValuePair<long, Mesh>> movingMeshes,
      Lyst<TransportRenderingSection> renderingSections,
      Lyst<TransportConnectorPose> connectorPoses)
    {
      Assert.That<bool>(this.m_builderStatic.IsEmpty).IsTrue("Non-empty static mesh builder.");
      Assert.That<bool>(this.m_builderMoving.IsEmpty).IsTrue("Non-empty moving mesh builder.");
      TransportProto transportProto = trajectory.TransportProto;
      Vector3 origin1 = new Vector3(0.0f, 0.0f, 0.0f);
      ImmutableArray<TransportFlowIndicatorPose> flowIndicators = transportProto.Graphics.FlowIndicator.IsNone ? ImmutableArray<TransportFlowIndicatorPose>.Empty : trajectory.FlowIndicatorsPoses;
      ImmutableArray<TransportModelFactory.CrossSection> crossSections = TransportModelFactory.createCrossSections(trajectory.Curve, transportProto, origin1, flowIndicators);
      Assert.That<int>(crossSections.Length).IsGreaterOrEqual(2);
      TransportModelFactory.CrossSection crossSection1 = crossSections[0];
      int startI = 0;
      for (int index1 = 1; index1 < crossSections.Length; ++index1)
      {
        TransportModelFactory.CrossSection crossSection2 = crossSections[index1];
        if (!crossSection2.EndOfSection)
        {
          if (crossSection2.Start)
          {
            crossSection1 = crossSection2;
            startI = index1;
          }
        }
        else
        {
          Vector3f vector3f1 = crossSection1.Direction;
          Vector3f normalized1 = vector3f1.Normalized;
          vector3f1 = crossSection2.Direction;
          Vector3f normalized2 = vector3f1.Normalized;
          vector3f1 = crossSection1.Up;
          Vector3f normalized3 = vector3f1.Normalized;
          int shift = 5;
          bool flag1 = false;
          bool flag2 = false;
          long key;
          Fix32 fix32;
          Vector2f xy;
          if (index1 - startI == 1)
            key = 0L;
          else if (normalized1.Z.IsNearZero() && normalized2.Z.IsNearZero())
          {
            flag1 = true;
            key = normalized1.Cross(normalized2).Z > 0 ? 1L : 2L;
          }
          else
          {
            if (normalized1.Z.IsNearZero() || normalized2.Z.IsNearZero())
            {
              fix32 = normalized1.Z + normalized2.Z;
              fix32 = fix32.Abs();
              if (!fix32.IsNear(Fix32.One))
              {
                flag2 = true;
                xy = normalized1.Xy;
                if (xy.IsParallelTo(normalized2.Xy))
                {
                  key = !normalized1.Z.IsNearZero() ? (normalized1.Z < 0 ? 5L : 6L) : (normalized2.Z < 0 ? 3L : 4L);
                  goto label_16;
                }
                else
                {
                  xy = normalized1.Xy;
                  long num = 7L + (long) (xy.PseudoCross(normalized2.Xy) > 0);
                  key = !normalized1.Z.IsNearZero() ? num + (normalized1.Z < 0 ? 4L : 6L) : num + (normalized2.Z < 0 ? 0L : 2L);
                  goto label_16;
                }
              }
            }
            fix32 = normalized1.Z + normalized2.Z;
            fix32 = fix32.Abs();
            if (fix32.IsNear(Fix32.One))
            {
              flag1 = true;
              key = normalized1.Z.IsNear((Fix32) 1) || normalized2.Z.IsNear((Fix32) 1) ? (normalized1.Z == 0 ? 15L : 16L) : (normalized1.Z == 0 ? 17L : 18L);
            }
            else
              key = 31L | vecToKeyComponent(normalized1) | vecToKeyComponent(normalized2) | vecToKeyComponent(normalized3);
          }
label_16:
          Assert.That<int>(1 << shift).IsGreaterOrEqual(31);
          Assert.That<int>(shift).IsLessOrEqual(63);
          bool flag3 = index1 == startI + 1;
          ImmutableArray<ImmutableArray<CrossSectionVertex>> crossSectionParts1 = transportProto.Graphics.CrossSection.StaticCrossSectionParts;
          ImmutableArray<ImmutableArray<CrossSectionVertex>> crossSectionParts2 = transportProto.Graphics.CrossSection.MovingCrossSectionParts;
          bool isNotEmpty1 = crossSectionParts1.IsNotEmpty;
          short staticMeshIdx = -1;
          for (short index2 = 0; (int) index2 < staticMeshes.Count; ++index2)
          {
            if (staticMeshes[(int) index2].Key == key)
            {
              staticMeshIdx = index2;
              break;
            }
          }
          bool isNotEmpty2 = crossSectionParts2.IsNotEmpty;
          short movingMeshIdx = -1;
          for (short index3 = 0; (int) index3 < movingMeshes.Count; ++index3)
          {
            if (movingMeshes[(int) index3].Key == key)
            {
              movingMeshIdx = index3;
              break;
            }
          }
          if (isNotEmpty1 && staticMeshIdx < (short) 0 || isNotEmpty2 && movingMeshIdx < (short) 0)
          {
            if (flag3)
            {
              Assert.That<Vector3f>(normalized1 - normalized2).IsNear(Vector3f.Zero, Fix32.EpsilonNear);
              if (isNotEmpty1 && staticMeshIdx < (short) 0)
              {
                this.m_builderStatic.StartExtrusion(crossSectionParts1, new Vector3(-0.5f, 0.0f, 0.0f), new Vector3f((Fix32) 1, (Fix32) 0, (Fix32) 0), new Vector3f((Fix32) 0, (Fix32) 0, (Fix32) 1), 0.0f, Percent.Hundred);
                this.m_builderStatic.ContinueExtrusion(crossSectionParts1, new Vector3(0.5f, 0.0f, 0.0f), new Vector3f((Fix32) 1, (Fix32) 0, (Fix32) 0), new Vector3f((Fix32) 0, (Fix32) 0, (Fix32) 1), 1f, Percent.Hundred);
              }
              if (isNotEmpty2 && movingMeshIdx < (short) 0)
              {
                this.m_builderMoving.StartExtrusion(crossSectionParts2, new Vector3(-0.5f, 0.0f, 0.0f), new Vector3f((Fix32) 1, (Fix32) 0, (Fix32) 0), new Vector3f((Fix32) 0, (Fix32) 0, (Fix32) 1), 0.0f, Percent.Hundred);
                this.m_builderMoving.ContinueExtrusion(crossSectionParts2, new Vector3(0.5f, 0.0f, 0.0f), new Vector3f((Fix32) 1, (Fix32) 0, (Fix32) 0), new Vector3f((Fix32) 0, (Fix32) 0, (Fix32) 1), 1f, Percent.Hundred);
              }
            }
            else
            {
              vector3f1 = crossSection1.Direction;
              if (vector3f1.Dot(crossSection2.Direction).IsNearZero())
              {
                vector3f1 = crossSection1.Direction;
                vector3f1 = vector3f1.Cross(crossSection2.Direction);
                vector3f1 = vector3f1.AbsValue;
                fix32 = vector3f1.MaxComponent();
                if (fix32.IsNear(Fix32.One))
                {
                  bool flag4 = true;
                  Quaternion quaternion1 = new Quaternion();
                  Quaternion quaternion2 = new Quaternion();
                  if (crossSection1.Direction.Z.IsNearZero())
                  {
                    quaternion1.SetFromToRotation(crossSection1.Direction.ToVector3(), new Vector3(1f, 0.0f, 0.0f));
                    quaternion2.SetFromToRotation(quaternion1 * crossSection1.Direction.ToVector3(), new Vector3(0.0f, 1f, 0.0f));
                  }
                  else
                  {
                    Assert.That<Fix32>(crossSection2.Direction.Z).IsNear((Fix32) 0, Fix32.EpsilonNear);
                    quaternion1.SetFromToRotation(crossSection2.Direction.ToVector3(), new Vector3(1f, 0.0f, 0.0f));
                    quaternion2.SetFromToRotation(quaternion1 * crossSection2.Direction.ToVector3(), new Vector3(0.0f, 1f, 0.0f));
                  }
                  float num1 = crossSection1.TotalDistance.Value.ToFloat();
                  float num2 = crossSection2.TotalDistance.Value.ToFloat() - num1;
                  Vector3 vector3 = (crossSection1.Position + crossSection2.Position) / 2f;
                  foreach (TransportModelFactory.CrossSection crossSection3 in crossSections.Slice(startI, index1 - startI + 1))
                  {
                    Vector3 origin2;
                    Vector3f vector3f2;
                    Vector3f up;
                    if (flag1)
                    {
                      origin2 = quaternion1 * (crossSection3.Position - vector3);
                      vector3f2 = (quaternion1 * crossSection3.Direction.ToVector3()).ToVector3f();
                      up = !crossSection1.Direction.Z.IsNearZero() || !crossSection2.Direction.Z.IsNearZero() ? (quaternion2 * vector3f2.ToVector3()).ToVector3f() : Vector3f.UnitZ;
                    }
                    else
                    {
                      origin2 = crossSection3.Position - vector3;
                      vector3f2 = crossSection3.Direction;
                      up = crossSection3.Up;
                    }
                    float textureCoordX = (crossSection3.TotalDistance.Value.ToFloat() - num1) / num2;
                    if (isNotEmpty1 && staticMeshIdx < (short) 0)
                    {
                      if (flag4)
                        this.m_builderStatic.StartExtrusion(crossSectionParts1, origin2, vector3f2, up, textureCoordX, Percent.Hundred);
                      else
                        this.m_builderStatic.ContinueExtrusion(crossSectionParts1, origin2, vector3f2, up, textureCoordX, Percent.Hundred);
                    }
                    if (isNotEmpty2 && movingMeshIdx < (short) 0)
                    {
                      if (flag4)
                        this.m_builderMoving.StartExtrusion(crossSectionParts2, origin2, vector3f2, up, textureCoordX, Percent.Hundred);
                      else
                        this.m_builderMoving.ContinueExtrusion(crossSectionParts2, origin2, vector3f2, up, textureCoordX, Percent.Hundred);
                    }
                    flag4 = false;
                  }
                  goto label_70;
                }
              }
              bool flag5 = true;
              Quaternion quaternion = new Quaternion();
              if (flag2)
              {
                if (crossSection1.Direction.Z.IsNearZero())
                {
                  quaternion.SetFromToRotation(crossSection1.Direction.ToVector3(), new Vector3(1f, 0.0f, 0.0f));
                }
                else
                {
                  Assert.That<Fix32>(crossSection2.Direction.Z).IsNear((Fix32) 0, Fix32.EpsilonNear);
                  quaternion.SetFromToRotation(crossSection2.Direction.ToVector3(), new Vector3(1f, 0.0f, 0.0f));
                }
              }
              float num3 = crossSection1.TotalDistance.Value.ToFloat();
              float num4 = crossSection2.TotalDistance.Value.ToFloat() - num3;
              Vector3 vector3_1 = (crossSection1.Position + crossSection2.Position) / 2f;
              foreach (TransportModelFactory.CrossSection crossSection4 in crossSections.Slice(startI, index1 - startI + 1))
              {
                Vector3 origin3;
                Vector3f crossSectionPlaneNormal;
                Vector3f up;
                if (flag2)
                {
                  origin3 = quaternion * (crossSection4.Position - vector3_1);
                  crossSectionPlaneNormal = (quaternion * crossSection4.Direction.ToVector3()).ToVector3f();
                  up = (quaternion * crossSection4.Up.ToVector3()).ToVector3f();
                }
                else
                {
                  origin3 = crossSection4.Position - vector3_1;
                  crossSectionPlaneNormal = crossSection4.Direction;
                  up = crossSection4.Up;
                }
                float textureCoordX = (crossSection4.TotalDistance.Value.ToFloat() - num3) / num4;
                if (isNotEmpty1 && staticMeshIdx < (short) 0)
                {
                  if (flag5)
                    this.m_builderStatic.StartExtrusion(crossSectionParts1, origin3, crossSectionPlaneNormal, up, textureCoordX, Percent.Hundred);
                  else
                    this.m_builderStatic.ContinueExtrusion(crossSectionParts1, origin3, crossSectionPlaneNormal, up, textureCoordX, Percent.Hundred);
                }
                if (isNotEmpty2 && movingMeshIdx < (short) 0)
                {
                  if (flag5)
                    this.m_builderMoving.StartExtrusion(crossSectionParts2, origin3, crossSectionPlaneNormal, up, textureCoordX, Percent.Hundred);
                  else
                    this.m_builderMoving.ContinueExtrusion(crossSectionParts2, origin3, crossSectionPlaneNormal, up, textureCoordX, Percent.Hundred);
                }
                flag5 = false;
              }
            }
label_70:
            if (isNotEmpty1 && staticMeshIdx == (short) -1)
            {
              Mesh meshAndClear = this.m_builderStatic.GetMeshAndClear();
              staticMeshes.Add(new KeyValuePair<long, Mesh>(key, meshAndClear));
              staticMeshIdx = (short) (staticMeshes.Count - 1);
            }
            if (isNotEmpty2 && movingMeshIdx == (short) -1)
            {
              Mesh meshAndClear = this.m_builderMoving.GetMeshAndClear();
              movingMeshes.Add(new KeyValuePair<long, Mesh>(key, meshAndClear));
              movingMeshIdx = (short) (movingMeshes.Count - 1);
            }
          }
          float straightScale;
          float pitch;
          float yaw;
          float texOffsetY;
          if (flag3)
          {
            fix32 = crossSection2.TotalDistance.Value - crossSection1.TotalDistance.Value;
            straightScale = fix32.ToFloat() * 2f;
            pitch = -Mathf.Asin(normalized1.Z.ToFloat());
            xy = normalized1.Xy;
            if (xy.IsNotZero)
            {
              xy = normalized1.Xy;
              Vector2f normalized4 = xy.Normalized;
              yaw = !normalized4.X.IsNear((Fix32) 1) ? (!normalized4.X.IsNear((Fix32) -1) ? (!normalized4.Y.IsNear((Fix32) 1) ? -1.57079637f : 1.57079637f) : 3.14159274f) : 0.0f;
              texOffsetY = 0.0f;
            }
            else
            {
              yaw = 0.0f;
              texOffsetY = !crossSection1.Up.X.IsNear((Fix32) -1) ? (!crossSection1.Up.X.IsNear((Fix32) 1) ? (!crossSection1.Up.Y.IsNear((Fix32) 1) ? 0.75f : 0.25f) : 0.5f) : 0.0f;
            }
          }
          else
          {
            straightScale = 1f;
            pitch = 0.0f;
            texOffsetY = 0.0f;
            if (flag1 | flag2)
            {
              Vector2f normalized5;
              if (normalized1.Z.IsNearZero())
              {
                xy = normalized1.Xy;
                normalized5 = xy.Normalized;
              }
              else
              {
                xy = normalized2.Xy;
                normalized5 = xy.Normalized;
              }
              yaw = !normalized5.X.IsNear((Fix32) 1) ? (!normalized5.X.IsNear((Fix32) -1) ? (!normalized5.Y.IsNear((Fix32) 1) ? -1.57079637f : 1.57079637f) : -3.14159274f) : 0.0f;
            }
            else
              yaw = 0.0f;
          }
          if (flag3 && normalized1.Z == 0 && normalized2.Z == 0 && crossSection1.Position.ToTile3f().Tile2i.ChunkCoord2i != crossSection2.Position.ToTile3f().Tile2i.ChunkCoord2i)
          {
            Bounds bounds = ChunkBasedRenderingManager.GetChunkBounds(ChunkBasedRenderingManager.GetChunkOrigin(crossSection1.Position.ToTile3f().Tile2i).ToVector2(), 0.0f, crossSection1.Position.y - 100f, crossSection1.Position.y + 100f);
            Vector3 vector3_2 = crossSection2.Position - crossSection1.Position;
            float magnitude = vector3_2.magnitude;
            float num5 = transportProto.Graphics.TransportUvLength.Value.ToFloat();
            Vector3 v = crossSection1.Position;
            Vector3 vector3_3 = bounds.ClosestPoint(crossSection2.Position);
            float num6 = crossSection1.TotalDistance.Value.ToFloat();
            float num7 = crossSection2.TotalDistance.Value.ToFloat() - crossSection1.TotalDistance.Value.ToFloat();
            int num8 = 0;
            while (true)
            {
              ++num8;
              if (num8 < 65536)
              {
                if (vector3_3 != v)
                {
                  vector3_2 = vector3_3 - v;
                  float num9 = vector3_2.magnitude / magnitude;
                  renderingSections.Add(new TransportRenderingSection(staticMeshIdx, movingMeshIdx, (v + vector3_3) / 2f, pitch, yaw, texOffsetY, straightScale * num9, num6 / num5, (num6 + num9 * num7) / num5));
                  if (!(crossSection2.Position == vector3_3))
                  {
                    num6 += num9 * num7;
                    v = vector3_3;
                  }
                  else
                    goto label_93;
                }
                vector3f1 = v.ToVector3f();
                Vector2 vector2_1 = vector3f1.Xy.ToVector2();
                vector3f1 = crossSection1.Direction;
                Vector2 vector2_2 = vector3f1.Xy.ToVector2();
                Bounds chunkBounds = ChunkBasedRenderingManager.GetChunkBounds(ChunkBasedRenderingManager.GetChunkOrigin((vector2_1 + vector2_2).ToTile2f().Tile2i).ToVector2(), 0.0f, crossSection1.Position.y - 100f, crossSection1.Position.y + 100f);
                Assert.That<Vector3>(chunkBounds.center).IsNotEqualTo<Vector3>(bounds.center);
                bounds = chunkBounds;
                vector3_3 = bounds.ClosestPoint(crossSection2.Position);
              }
              else
                break;
            }
            Log.Error("While loop overflow!");
          }
          else
            renderingSections.Add(new TransportRenderingSection(staticMeshIdx, movingMeshIdx, (crossSection1.Position + crossSection2.Position) / 2f, pitch, yaw, texOffsetY, straightScale, crossSection1.TotalDistance.Value.ToFloat() / transportProto.Graphics.TransportUvLength.Value.ToFloat(), crossSection2.TotalDistance.Value.ToFloat() / transportProto.Graphics.TransportUvLength.Value.ToFloat()));
label_93:
          if (!flag3 && transportProto.Graphics.VerticalConnectorPrefabPath.HasValue)
          {
            if (normalized1.Z.IsNearZero())
            {
              fix32 = normalized2.Z.Abs();
              if (fix32.IsNear(Fix32.One))
              {
                connectorPoses.Add(new TransportConnectorPose(crossSection2.Position.ToTile3f()));
                goto label_100;
              }
            }
            if (normalized2.Z.IsNearZero())
            {
              fix32 = normalized1.Z.Abs();
              if (fix32.IsNear(Fix32.One))
                connectorPoses.Add(new TransportConnectorPose(crossSection1.Position.ToTile3f()));
            }
          }
label_100:
          crossSection1 = crossSection2;
          startI = index1;

          long vecToKeyComponent(Vector3f val)
          {
            return valToKeyComponent(val.X) | valToKeyComponent(val.Y) | valToKeyComponent(val.Z);
          }

          long valToKeyComponent(Fix32 val)
          {
            Assert.That<Fix32>(val.Abs()).IsLessOrEqual((Fix32) 1);
            long keyComponent = (long) ((val + (Fix32) 1) * 8).ToIntRounded() << shift;
            shift += 5;
            return keyComponent;
          }
        }
      }
    }

    /// <summary>
    /// Adds colliders to given transport GO based on its pivots.
    /// </summary>
    public static void AddColliders(
      GameObject transportGo,
      ImmutableArray<Tile3i> pivots,
      Vector3 origin)
    {
      if (pivots.Length == 1)
      {
        SphereCollider sphereCollider = transportGo.AddComponent<SphereCollider>();
        sphereCollider.center = origin + pivots.First.ToCenterVector3();
        sphereCollider.radius = TransportModelFactory.COLLIDER_RADIUS;
      }
      else
      {
        for (int index1 = 1; index1 < pivots.Length; ++index1)
        {
          Vector3 vector3_1 = pivots[index1 - 1].ToCenterVector3();
          Vector3 centerVector3 = pivots[index1].ToCenterVector3();
          Vector3 vector3_2 = centerVector3 - vector3_1;
          int num1 = (double) vector3_2.x != 0.0 ? 0 : 2;
          float num2 = new Vector2(vector3_2.x, vector3_2.z).magnitude + TransportModelFactory.COLLIDER_RADIUS * 2f;
          int num3 = (pivots[index1 - 1].Z - pivots[index1].Z).Abs();
          if (num3 == 0)
          {
            CapsuleCollider capsuleCollider = transportGo.AddComponent<CapsuleCollider>();
            capsuleCollider.center = origin + (vector3_1 + centerVector3) / 2f;
            capsuleCollider.radius = TransportModelFactory.COLLIDER_RADIUS;
            capsuleCollider.height = num2;
            capsuleCollider.direction = num1;
          }
          else
          {
            int num4 = ((float) num3 * 1.4f).CeilToInt();
            Vector3 vector3_3 = vector3_2 / (float) num4;
            float num5 = num2 / (float) num4;
            for (int index2 = 0; index2 < num4; ++index2)
            {
              Vector3 vector3_4 = vector3_1 + vector3_3;
              CapsuleCollider capsuleCollider = transportGo.AddComponent<CapsuleCollider>();
              capsuleCollider.center = origin + (vector3_1 + vector3_4) / 2f;
              capsuleCollider.radius = TransportModelFactory.COLLIDER_RADIUS;
              capsuleCollider.height = num5;
              capsuleCollider.direction = num1;
              vector3_1 = vector3_4;
            }
          }
        }
      }
    }

    /// <summary>Creates transport mesh for given curve.</summary>
    private static void createMesh(
      TransportProto proto,
      ImmutableArray<TransportModelFactory.CrossSection> crossSections,
      MeshBuilder builderStatic,
      MeshBuilder builderMoving)
    {
      Assert.That<bool>(builderStatic.IsEmpty).IsTrue();
      Assert.That<bool>(builderMoving.IsEmpty).IsTrue();
      ImmutableArray<ImmutableArray<CrossSectionVertex>> crossSectionParts1 = proto.Graphics.CrossSection.StaticCrossSectionParts;
      ImmutableArray<ImmutableArray<CrossSectionVertex>> crossSectionParts2 = proto.Graphics.CrossSection.MovingCrossSectionParts;
      Percent crossSectionScale = proto.Graphics.CrossSectionScale;
      foreach (TransportModelFactory.CrossSection crossSection in crossSections)
      {
        float textureCoordX = crossSection.TotalDistance.Value.ToFloat() / proto.Graphics.TransportUvLength.Value.ToFloat();
        if (crossSectionParts1.IsNotEmpty)
        {
          if (crossSection.Start)
            builderStatic.StartExtrusion(crossSectionParts1, crossSection.Position, crossSection.Direction, crossSection.Up, textureCoordX, crossSectionScale);
          else
            builderStatic.ContinueExtrusion(crossSectionParts1, crossSection.Position, crossSection.Direction, crossSection.Up, textureCoordX, crossSectionScale);
        }
        if (crossSectionParts2.IsNotEmpty)
        {
          if (crossSection.Start)
            builderMoving.StartExtrusion(crossSectionParts2, crossSection.Position, crossSection.Direction, crossSection.Up, textureCoordX, crossSectionScale);
          else
            builderMoving.ContinueExtrusion(crossSectionParts2, crossSection.Position, crossSection.Direction, crossSection.Up, textureCoordX, crossSectionScale);
        }
      }
    }

    /// <summary>Creates transport mesh for given curve.</summary>
    private static ImmutableArray<TransportModelFactory.CrossSection> createCrossSections(
      CubicBezierCurve3f curve,
      TransportProto proto,
      Vector3 origin,
      ImmutableArray<TransportFlowIndicatorPose> flowIndicators)
    {
      TransportProto.Gfx.FlowIndicatorSpec valueOrNull = proto.Graphics.FlowIndicator.ValueOrNull;
      RelTile1f relTile1f1 = valueOrNull != null ? valueOrNull.SkipTransportLength : RelTile1f.Zero;
      bool flag1 = flowIndicators.IsNotEmpty && relTile1f1.IsPositive;
      int index = 0;
      Vector3f normalized1 = curve.SampleSegmentDerivative(0, Percent.Zero).Normalized;
      Vector3f vector3f1 = normalized1.Normalized;
      Fix32 fix32 = vector3f1.Z.Abs();
      Vector3f normalized2;
      if (fix32.IsNear(Fix32.One, Fix32.Epsilon))
      {
        vector3f1 = normalized1.Cross(Vector3f.UnitX);
        normalized2 = vector3f1.Normalized;
      }
      else
      {
        vector3f1 = normalized1.Cross(Vector3f.UnitZ);
        normalized2 = vector3f1.Normalized;
      }
      vector3f1 = normalized2.Cross(normalized1);
      Vector3f normalized3 = vector3f1.Normalized;
      Quaternion quaternion = new Quaternion();
      Tile3f tile1 = new Tile3f(curve[0]);
      RelTile1f totalDistance1 = RelTile1f.Zero;
      Vector3f v1 = normalized1;
      Vector3f v2 = normalized3;
      Lyst<TransportModelFactory.CrossSection> lyst = new Lyst<TransportModelFactory.CrossSection>();
      lyst.Add(new TransportModelFactory.CrossSection(tile1.ToVector3() + origin, normalized1, normalized3, totalDistance1, true, false));
      for (int segmentIndex = 0; segmentIndex < curve.SegmentsCount; ++segmentIndex)
      {
        bool flag2 = curve.IsStraightSegment(segmentIndex);
        if (!flag2 || segmentIndex + 1 >= curve.SegmentsCount || !curve.IsStraightSegment(segmentIndex + 1))
        {
          int denominator = flag2 ? 1 : proto.Graphics.SamplesPerCurvedSegment;
          for (int numerator = 1; numerator <= denominator; ++numerator)
          {
            Percent t = Percent.FromRatio(numerator, denominator);
            vector3f1 = curve.SampleSegmentDerivative(segmentIndex, t);
            Vector3f normalized4 = vector3f1.Normalized;
            Vector3f normalized5;
            if (proto.ZStepLength.IsZero)
            {
              quaternion.SetFromToRotation(v1.ToVector3(), normalized4.ToVector3());
              quaternion = quaternion.normalized;
              vector3f1 = (quaternion * v2.ToVector3()).ToVector3f();
              normalized5 = vector3f1.Normalized;
            }
            else
            {
              vector3f1 = normalized4.Cross(Vector3f.UnitZ);
              vector3f1 = vector3f1.Normalized.Cross(normalized4);
              normalized5 = vector3f1.Normalized;
            }
            Tile3f tile2 = new Tile3f(curve.SampleSegment64(segmentIndex, t));
            RelTile3f relTile3f;
            if (flag1 & flag2 && index < flowIndicators.Length)
            {
              Line3f line3f = new Line3f(tile1.Vector3f, tile2.Vector3f);
              do
              {
                Vector3f vector3f2 = flowIndicators[index].Position.Vector3f;
                fix32 = line3f.DistanceLineSegmentToPt(vector3f2);
                if (fix32.ToFloat().IsNear(0.0f, 0.05f))
                {
                  Tile3f tile3 = new Tile3f(vector3f2 - normalized4 * (relTile1f1.Value / 2));
                  Tile3f tile4 = new Tile3f(vector3f2 + normalized4 * (relTile1f1.Value / 2));
                  RelTile1f relTile1f2 = totalDistance1;
                  relTile3f = tile3 - tile1;
                  RelTile1f relTile1f3 = new RelTile1f(relTile3f.Length);
                  RelTile1f totalDistance2 = relTile1f2 + relTile1f3;
                  lyst.Add(new TransportModelFactory.CrossSection(tile3.ToVector3() + origin, normalized4, normalized5, totalDistance2, false, true));
                  totalDistance1 = totalDistance2 + relTile1f1;
                  lyst.Add(new TransportModelFactory.CrossSection(tile4.ToVector3() + origin, normalized4, normalized5, totalDistance1, true, false));
                  tile1 = tile4;
                  ++index;
                }
                else
                  break;
              }
              while (index < flowIndicators.Length);
            }
            RelTile1f relTile1f4 = totalDistance1;
            relTile3f = tile2 - tile1;
            RelTile1f relTile1f5 = new RelTile1f(relTile3f.Length);
            totalDistance1 = relTile1f4 + relTile1f5;
            lyst.Add(new TransportModelFactory.CrossSection(tile2.ToVector3() + origin, normalized4, normalized5, totalDistance1, false, numerator == denominator));
            tile1 = tile2;
            v1 = normalized4;
            v2 = normalized5;
          }
        }
      }
      return lyst.ToImmutableArray();
    }

    /// <summary>
    /// Adds flow indicators MBs to given transport GO based on the trajectory information.
    /// </summary>
    private void addFlowIndicators(
      ImmutableArray<TransportFlowIndicatorPose> indicatorPoses,
      TransportProto proto,
      Vector3 origin,
      GameObject transportGo)
    {
      if (proto.Graphics.FlowIndicator.IsNone)
      {
        Assert.Fail(string.Format("No flow indicators for transport: {0}.", (object) proto.Id));
      }
      else
      {
        TransportProto.Gfx.FlowIndicatorSpec indicatorProto = proto.Graphics.FlowIndicator.Value;
        GameObject prefab;
        if (!this.m_assetsDb.TryGetSharedPrefab(indicatorProto.FramePrefabPath, out prefab))
        {
          Assert.Fail(string.Format("No flow indicator prefab for transport: {0}.", (object) proto.Id));
        }
        else
        {
          int indicatorIndex = 0;
          foreach (TransportFlowIndicatorPose indicatorPose in indicatorPoses)
          {
            GameObject gameObject = Object.Instantiate<GameObject>(prefab);
            gameObject.AddComponent<TransportFlowIndicatorMb>().Initialize(indicatorIndex, indicatorProto, indicatorPose.SegmentIndex);
            gameObject.transform.localPosition = origin + indicatorPose.Position.ToVector3();
            gameObject.transform.localRotation = indicatorPose.Rotation.ToQuaternion().ToUnityQuaternion();
            gameObject.transform.SetParent(transportGo.transform, false);
            gameObject.SetActive(true);
            ++indicatorIndex;
          }
        }
      }
    }

    static TransportModelFactory()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TransportModelFactory.COLLIDER_RADIUS = new RelTile1f(0.4.ToFix32()).ToUnityUnits();
    }

    private readonly struct CrossSection
    {
      public readonly Vector3 Position;
      public readonly Vector3f Direction;
      public readonly Vector3f Up;
      public readonly RelTile1f TotalDistance;
      public readonly bool Start;
      public readonly bool EndOfSection;

      public CrossSection(
        Vector3 position,
        Vector3f direction,
        Vector3f up,
        RelTile1f totalDistance,
        bool start,
        bool endOfSection)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.Direction = direction;
        this.Up = up;
        this.TotalDistance = totalDistance;
        this.Start = start;
        this.EndOfSection = endOfSection;
      }
    }
  }
}
