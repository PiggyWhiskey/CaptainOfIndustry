// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.BuildableCanvasMb
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
  /// <summary>
  /// Mono Behavior that allows creation of a mesh for UI with <see cref="T:Mafi.Unity.MeshBuilder" />.
  /// </summary>
  [RequireComponent(typeof (RectTransform), typeof (CanvasRenderer))]
  public class BuildableCanvasMb : MonoBehaviour, IBuildable
  {
    private readonly Mafi.Lazy<Mesh> m_mesh;

    public void SetSharedMesh(
      Vector3[] vertices,
      int[] indices,
      Vector3[] normals = null,
      Vector4[] tangents = null,
      Color32[] colors = null,
      Vector2[] uvs = null,
      Vector2[] uvs2 = null,
      bool recalculateNormals = false)
    {
      Mesh mesh = this.m_mesh.Value;
      BuildableMb.SetupMesh(mesh, vertices, indices, normals, tangents, colors, uvs, uvs2, recalculateNormals);
      this.GetComponent<CanvasRenderer>().SetMesh(mesh);
    }

    public void ClearMesh() => this.m_mesh.Value.Clear();

    public BuildableCanvasMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_mesh = new Mafi.Lazy<Mesh>((Func<Mesh>) (() => new Mesh()));
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
