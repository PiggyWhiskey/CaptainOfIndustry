// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.RoadsModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Gfx;
using Mafi.Core.Roads;
using Mafi.Curves;
using Mafi.Unity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class RoadsModelFactory : 
    IProtoModelFactory<RoadEntityProto>,
    IFactory<RoadEntityProto, GameObject>,
    IProtoModelFactory<RoadEntranceEntityProto>,
    IFactory<RoadEntranceEntityProto, GameObject>
  {
    private readonly AssetsDb m_assetsDb;
    private readonly Dict<RelTile1f, ImmutableArray<ImmutableArray<CrossSectionVertex>>> m_crossSectionCache;

    public RoadsModelFactory(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_crossSectionCache = new Dict<RelTile1f, ImmutableArray<ImmutableArray<CrossSectionVertex>>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
    }

    public GameObject Create(RoadEntityProto proto)
    {
      GameObject go;
      string error;
      if (!this.TryCreateModel((RoadEntityProtoBase) proto, out go, out error))
      {
        Log.Error("Failed to create road model: " + error);
        go = new GameObject("RoadFail");
      }
      return go;
    }

    public GameObject Create(RoadEntranceEntityProto proto)
    {
      GameObject go;
      string error;
      if (!this.TryCreateModel((RoadEntityProtoBase) proto, out go, out error))
      {
        Log.Error("Failed to create road model: " + error);
        go = new GameObject("RoadFail");
      }
      return go;
    }

    public bool TryCreateModel(
      RoadEntityProtoBase proto,
      out GameObject go,
      out string error,
      bool noColliders = false)
    {
      go = new GameObject(proto.Id.Value);
      MeshBuilder instance = MeshBuilder.Instance;
      foreach (IGrouping<CubicBezierCurve3f, RoadLaneSpec> source in proto.LanesSpecs.AsEnumerable().GroupBy<RoadLaneSpec, CubicBezierCurve3f>((Func<RoadLaneSpec, CubicBezierCurve3f>) (x => x.TrajectoryCurve)))
      {
        RoadLaneSpec[] array = source.ToArray<RoadLaneSpec>();
        if (!((IEnumerable<RoadLaneSpec>) array).Any<RoadLaneSpec>((Func<RoadLaneSpec, bool>) (x => x.IsHidden)))
        {
          MinMaxPair<RelTile1f> minMaxPair = ((IEnumerable<RoadLaneSpec>) array).MinMax<RelTile1f, RoadLaneSpec>((Func<RoadLaneSpec, RelTile1f>) (x => x.TrajectoryOffset));
          RelTile1f roadWidth = minMaxPair.Max - minMaxPair.Min + RoadEntityProto.LANE_WIDTH_OUTER;
          this.createMesh(proto, instance, roadWidth, source.Key, array[0].CustomZCurve);
        }
      }
      instance.UpdateGoAndClear(go);
      go.GetComponent<MeshRenderer>().sharedMaterial = this.m_assetsDb.GetSharedMaterial(proto.Graphics.MaterialPath);
      if (!noColliders)
      {
        Mesh sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
        BoxCollider boxCollider = go.AddComponent<BoxCollider>();
        boxCollider.center = sharedMesh.bounds.center;
        boxCollider.size = sharedMesh.bounds.size;
      }
      error = "";
      return true;
    }

    private void createMesh(
      RoadEntityProtoBase proto,
      MeshBuilder meshBuilder,
      RelTile1f roadWidth,
      CubicBezierCurve3f roadCurve,
      Option<CubicBezierCurve3f> customZCurve)
    {
      CubicBezierCurve3fSampler curveSampler = roadCurve.GetUniformSampler(50);
      CubicBezierCurve3fSampler zCurveSampler = customZCurve.ValueOrNull?.GetUniformSampler(50);
      meshBuilder.SetTransform(-proto.Layout.GetCenter(TileTransform.Identity).ToVector3());
      ImmutableArray<ImmutableArray<CrossSectionVertex>> crossSection;
      if (!this.m_crossSectionCache.TryGetValue(roadWidth, out crossSection))
      {
        crossSection = RoadsModelFactory.createCrossSection(roadWidth);
        this.m_crossSectionCache.Add(roadWidth, crossSection);
      }
      Vector3 origin1 = sampleCurve(Percent.Zero);
      meshBuilder.StartExtrusion(crossSection, origin1, sampleDirection(Percent.Zero), Vector3f.UnitZ, 0.0f, Percent.Hundred);
      float textureCoordX = 0.0f;
      float num = 1f / roadWidth.ToUnityUnits();
      int meshSegmentsCount = proto.Graphics.MeshSegmentsCount;
      for (int index = 0; index < meshSegmentsCount; ++index)
      {
        Percent p = Percent.FromRatio(index + 1, meshSegmentsCount);
        Vector3 origin2 = sampleCurve(p);
        textureCoordX += (origin2 - origin1).magnitude * num;
        meshBuilder.ContinueExtrusion(crossSection, origin2, sampleDirection(p), Vector3f.UnitZ, textureCoordX, Percent.Hundred);
        origin1 = origin2;
      }

      Vector3 sampleCurve(Percent p)
      {
        Vector3 vector3 = new Tile3f(curveSampler.SampleUniform(p)).ToVector3();
        if (zCurveSampler != null)
          vector3.y = (zCurveSampler.SampleUniform(p).Z * 2).ToFloat();
        return vector3;
      }

      Vector3f sampleDirection(Percent p)
      {
        Vector3f normalized1 = curveSampler.SampleDerivativeUniform(p).Normalized;
        if (zCurveSampler != null)
        {
          Vector3f normalized2 = zCurveSampler.SampleDerivativeUniform(p).Normalized;
          normalized1 = normalized1.SetZ(normalized2.Z).Normalized;
        }
        return normalized1;
      }
    }

    private static ImmutableArray<ImmutableArray<CrossSectionVertex>> createCrossSection(
      RelTile1f roadWidth)
    {
      double x = (double) (roadWidth / 2).Value.ToFloat();
      double y1 = 0.05;
      double y2 = -0.25;
      return ImmutableArray.Create<ImmutableArray<CrossSectionVertex>>(ImmutableArray.Create<CrossSectionVertex>(create(-x, y2, -1.0, 0.0, 0.0), create(-x, 0.0, -1.0, 0.0, 0.02), create(-x + 0.1, y1, 0.0, 1.0, 0.04)), ImmutableArray.Create<CrossSectionVertex>(create(-x + 0.1, y1, 0.0, 1.0, 0.04), create(x - 0.1, y1, 0.0, 1.0, 0.96)), ImmutableArray.Create<CrossSectionVertex>(create(x - 0.1, y1, 0.0, 1.0, 0.96), create(x, 0.0, 1.0, 0.0, 0.98), create(x, y2, 1.0, 0.0, 1.0)), ImmutableArray.Create<CrossSectionVertex>(create(x, y2, 0.0, -1.0, 1.0), create(-x, y2, 0.0, -1.0, 0.0)));

      static CrossSectionVertex create(
        double x,
        double y,
        double normalX,
        double normalY,
        double uv)
      {
        return new CrossSectionVertex(new RelTile2f(x.ToFix32(), y.ToFix32()), new Vector2f(normalX.ToFix32(), normalY.ToFix32()), (float) uv);
      }
    }
  }
}
