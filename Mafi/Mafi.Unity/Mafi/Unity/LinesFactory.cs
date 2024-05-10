// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.LinesFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class LinesFactory
  {
    public readonly Material DefaultSharedMaterial;
    private readonly MbPool<LineMb> m_linesPool;

    public LinesFactory(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DefaultSharedMaterial = assetsDb.GetSharedMaterial("Assets/Core/Materials/Line.mat");
      this.m_linesPool = new MbPool<LineMb>("Lines pool", 32, (Func<MbPool<LineMb>, LineMb>) (pool =>
      {
        LineMb lineMb = new GameObject("Line").AddComponent<LineMb>();
        lineMb.Initialize(pool, this.DefaultSharedMaterial);
        return lineMb;
      }), (Action<LineMb>) (mb => { }));
    }

    public LineMb CreateEmptyLine(string name = "line", GameObject parentGo = null)
    {
      LineMb emptyLine = new GameObject(name).AddComponent<LineMb>();
      emptyLine.Initialize(this.DefaultSharedMaterial);
      if ((UnityEngine.Object) parentGo != (UnityEngine.Object) null)
        emptyLine.transform.SetParent(parentGo.transform, true);
      return emptyLine;
    }

    public LineMb CreateLine(
      Vector3 from,
      Vector3 to,
      float width,
      Color color,
      Material sharedMaterial,
      Layer layer = Layer.Custom14TerrainOverlays)
    {
      LineMb line = new GameObject("Line").AddComponent<LineMb>();
      line.gameObject.layer = layer.ToId();
      line.Initialize(sharedMaterial);
      Vector3[] andInit = ArrayPool<Vector3>.GetAndInit(from, to);
      line.SetPoints(andInit);
      andInit.ReturnToPool<Vector3>();
      line.SetColor(color);
      line.SetWidth(width);
      return line;
    }

    public LineMb CreateEmptyLinePooled() => this.m_linesPool.GetInstance();

    public LineMb CreateLinePooled(
      Vector3 from,
      Vector3 to,
      float width,
      Color color,
      Layer layer = Layer.Custom14TerrainOverlays)
    {
      Vector3[] andInit = ArrayPool<Vector3>.GetAndInit(from, to);
      LineMb linePooled = this.CreateLinePooled(andInit, width, color);
      linePooled.gameObject.layer = layer.ToId();
      andInit.ReturnToPool<Vector3>();
      return linePooled;
    }

    public LineMb CreateLinePooled(Vector3[] positions, float width, Color color, Layer layer = Layer.Custom14TerrainOverlays)
    {
      LineMb instance = this.m_linesPool.GetInstance();
      instance.SetPoints(positions);
      instance.SetColor(color);
      instance.SetWidth(width);
      instance.gameObject.layer = layer.ToId();
      return instance;
    }
  }
}
