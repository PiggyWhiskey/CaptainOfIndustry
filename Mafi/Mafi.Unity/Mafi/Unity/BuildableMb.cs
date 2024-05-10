// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.BuildableMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// Mono Behavior that allows creation of the mesh with <see cref="T:Mafi.Unity.MeshBuilder" />.
  /// </summary>
  [RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
  public class BuildableMb : EntityMb, IBuildable
  {
    public int VertexCount => this.GetComponent<MeshFilter>().sharedMesh.vertexCount;

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
      Mesh mesh = this.GetComponent<MeshFilter>().sharedMesh;
      if ((Object) mesh == (Object) null)
      {
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().sharedMesh = mesh;
      }
      if (indices.Length > 65536)
        mesh.indexFormat = IndexFormat.UInt32;
      BuildableMb.SetupMesh(mesh, vertices, indices, normals, tangents, colors, uvs, uvs2, recalculateNormals);
    }

    public static void SetupMesh(
      Mesh mesh,
      Vector3[] vertices,
      int[] indices,
      Vector3[] normals = null,
      Vector4[] tangents = null,
      Color32[] colors = null,
      Vector2[] uvs = null,
      Vector2[] uvs2 = null,
      bool recalculateNormals = false)
    {
      Assert.That<Vector3[]>(vertices).IsNotEmpty<Vector3>();
      Assert.That<int[]>(indices).IsNotEmpty<int>();
      Assert.That<int>(indices.Length % 3).IsZero();
      mesh.Clear();
      mesh.vertices = vertices;
      mesh.triangles = indices;
      if (normals != null && normals.Length != 0)
      {
        Assert.That<Vector3[]>(normals).HasLength<Vector3>(vertices.Length);
        mesh.normals = normals;
      }
      else
        mesh.normals = (Vector3[]) null;
      if (tangents != null && tangents.Length != 0)
      {
        Assert.That<Vector4[]>(tangents).HasLength<Vector4>(vertices.Length);
        mesh.tangents = tangents;
      }
      else
        mesh.tangents = (Vector4[]) null;
      if (colors != null && colors.Length != 0)
      {
        Assert.That<Color32[]>(colors).HasLength<Color32>(vertices.Length);
        mesh.colors32 = colors;
      }
      else
        mesh.colors32 = (Color32[]) null;
      if (uvs != null && uvs.Length != 0)
      {
        Assert.That<Vector2[]>(uvs).HasLength<Vector2>(vertices.Length);
        mesh.uv = uvs;
      }
      else
        mesh.uv = (Vector2[]) null;
      if (uvs2 != null && uvs2.Length != 0)
      {
        Assert.That<Vector2[]>(uvs2).HasLength<Vector2>(vertices.Length);
        mesh.uv2 = uvs2;
      }
      else
        mesh.uv2 = (Vector2[]) null;
      mesh.RecalculateBounds();
      if (!recalculateNormals)
        return;
      mesh.RecalculateNormals();
    }

    public Color32[] SwapColors(Color32[] newColors)
    {
      Mesh sharedMesh = this.GetComponent<MeshFilter>().sharedMesh;
      Assert.That<Color32[]>(newColors).HasLength<Color32>(sharedMesh.vertexCount);
      Color32[] colors32 = sharedMesh.colors32;
      sharedMesh.colors32 = newColors;
      return colors32;
    }

    public void ClearMesh() => this.GetComponent<MeshFilter>().sharedMesh.Clear();

    public void SetSharedMaterial(Material mat)
    {
      this.GetComponent<MeshRenderer>().sharedMaterial = mat;
    }

    public BuildableMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
