// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MeshBuilder
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Gfx;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public class MeshBuilder
  {
    [ThreadStatic]
    private static MeshBuilder s_instance;
    private static readonly Lyst<Vector2[]> s_cosSinCircleCache;
    private readonly Lyst<Vector3> m_vertices;
    private readonly Lyst<int> m_indices;
    private readonly Lyst<Vector3> m_normals;
    private readonly Lyst<Vector4> m_tangents;
    private readonly Lyst<Color32> m_colors;
    private readonly Lyst<Vector2> m_texCoords;
    private readonly Lyst<Vector2> m_texCoords2;
    private bool m_transformIsSet;
    private Vector3 m_translation;
    private Quaternion m_rotation;
    private Vector3 m_scale;
    private Matrix4x4 m_transform;
    private readonly List<MeshFilter> m_tempMeshFilters;
    private MeshBuilder.GuiLineBuilder m_guiLineBuilder;

    /// <summary>
    /// Returns global clean instance for mesh construction. It is responsibility of the caller to clear the builder
    /// when he is doe with it! This instance should not be cached or stored and should be used to produce mesh
    /// within small scope. Returned builder is unique per thread. For more complicated mesh building please create
    /// your own instance.
    /// </summary>
    /// <remarks>
    /// Sharing one instance of a builder prevents re-allocation of all the buffers and reduces memory footprint
    /// compared to privately owned builders.
    /// </remarks>
    public static MeshBuilder Instance
    {
      get
      {
        if (MeshBuilder.s_instance == null)
        {
          MeshBuilder.s_instance = new MeshBuilder();
        }
        else
        {
          Assert.That<bool>(MeshBuilder.s_instance.IsEmpty).IsTrue("Taking non-cleared global MeshBuilder instance. This happens when caller did not clear the builder after themselves, or somebody is trying to use builder while it is already being used! Check call stack!");
          MeshBuilder.s_instance.Clear();
        }
        return MeshBuilder.s_instance;
      }
    }

    public IIndexable<Vector3> Vertices => (IIndexable<Vector3>) this.m_vertices;

    public IIndexable<int> Indices => (IIndexable<int>) this.m_indices;

    public IIndexable<Vector3> Normals => (IIndexable<Vector3>) this.m_normals;

    public IIndexable<Color32> Colors => (IIndexable<Color32>) this.m_colors;

    public IIndexable<Vector2> TexCoords => (IIndexable<Vector2>) this.m_texCoords;

    /// <summary>
    /// Current translation applied to all appended vertices. It is reset after clearing or by calling <see cref="M:Mafi.Unity.MeshBuilder.ResetTransform" />.
    /// </summary>
    public Vector3 Translation
    {
      get => this.m_translation;
      set
      {
        this.m_translation = value;
        this.recountTransform();
      }
    }

    /// <summary>
    /// Current rotation applied to all appended vertices and normals. It is reset after clearing or by calling <see cref="M:Mafi.Unity.MeshBuilder.ResetTransform" />.
    /// </summary>
    public Quaternion Rotation
    {
      get => this.m_rotation;
      set
      {
        this.m_rotation = value;
        this.recountTransform();
      }
    }

    /// <summary>
    /// Current scale applied to all appended vertices. It is reset after clearing or by calling <see cref="M:Mafi.Unity.MeshBuilder.ResetTransform" />.
    /// </summary>
    public Vector3 Scale
    {
      get => this.m_scale;
      set
      {
        this.m_scale = value;
        this.recountTransform();
      }
    }

    /// <summary>
    /// Sets current transformation. More effective than setting each property separately. Transform is reset after
    /// clearing or by calling <see cref="M:Mafi.Unity.MeshBuilder.ResetTransform" />.
    /// </summary>
    public void SetTransform(Vector3 translation, Quaternion rotation, float scale)
    {
      Assert.That<float>(scale).IsNotZero();
      this.m_translation = translation;
      this.m_rotation = rotation;
      this.m_scale = Vector3.one * scale;
      this.recountTransform();
    }

    public void SetTransform(Vector3 translation)
    {
      this.m_translation = translation;
      this.m_rotation = Quaternion.identity;
      this.m_scale = Vector3.one;
      this.recountTransform();
    }

    public void SetTransform(Vector3 translation, Quaternion rotation, Vector3 scale)
    {
      this.m_translation = translation;
      this.m_rotation = rotation;
      this.m_scale = scale;
      this.recountTransform();
    }

    private void recountTransform()
    {
      this.m_transform = Matrix4x4.TRS(this.m_translation, this.m_rotation, this.m_scale);
      this.m_transformIsSet = !this.m_transform.isIdentity;
    }

    /// <summary>Resets current transform to identity.</summary>
    public void ResetTransform()
    {
      if (!this.m_transformIsSet)
        return;
      this.m_translation = Vector3.zero;
      this.m_rotation = Quaternion.identity;
      this.m_scale = Vector3.one;
      this.m_transform = Matrix4x4.identity;
      this.m_transformIsSet = false;
    }

    /// <summary>Number of Currently stored vertices.</summary>
    public int VerticesCount => this.m_vertices.Count;

    public int TrianglesCount => this.m_indices.Count / 3;

    /// <summary>
    /// Whether this builder is empty and there are no vertices or indices stored.
    /// </summary>
    public bool IsEmpty
    {
      get => this.m_vertices.IsEmpty && this.m_indices.IsEmpty && !this.IsTransformSet;
    }

    /// <summary>Whether this builder is NOT empty.</summary>
    public bool IsNotEmpty => !this.IsEmpty;

    public bool IsTransformSet => this.m_transformIsSet;

    /// <summary>Clears all stored data. This is O(1) operation.</summary>
    public void Clear()
    {
      this.m_vertices.Clear();
      this.m_indices.Clear();
      this.m_normals.Clear();
      this.m_tangents.Clear();
      this.m_colors.Clear();
      this.m_texCoords.Clear();
      this.m_guiLineBuilder?.Reset();
      this.ResetTransform();
    }

    /// <summary>
    /// Updates mesh of given game object. Creates <see cref="T:UnityEngine.MeshFilter" /> and <see cref="T:UnityEngine.MeshRenderer" /> if
    /// necessary.
    /// </summary>
    public void UpdateGoAndClear(GameObject entityGo, bool omitTextureCoords = false)
    {
      MbUpdatePackage updatePackageAndClear = this.GetMbUpdatePackageAndClear(omitTextureCoords);
      MeshFilter meshFilter = entityGo.GetComponent<MeshFilter>();
      if (!(bool) (UnityEngine.Object) meshFilter)
        meshFilter = entityGo.AddComponent<MeshFilter>();
      if (!(bool) (UnityEngine.Object) entityGo.GetComponent<MeshRenderer>())
        entityGo.AddComponent<MeshRenderer>();
      Mesh mesh = meshFilter.sharedMesh;
      if (!(bool) (UnityEngine.Object) mesh)
      {
        mesh = new Mesh();
        meshFilter.sharedMesh = mesh;
      }
      mesh.Clear();
      mesh.vertices = updatePackageAndClear.Vertices;
      mesh.triangles = updatePackageAndClear.Indices;
      mesh.normals = updatePackageAndClear.Normals.ValueOrNull;
      mesh.tangents = updatePackageAndClear.Tangents.ValueOrNull;
      mesh.colors32 = updatePackageAndClear.Colors.ValueOrNull;
      mesh.uv = updatePackageAndClear.TexCoords.ValueOrNull;
      mesh.RecalculateBounds();
    }

    /// <summary>
    /// Updates given <paramref name="buildableMb" /> with currently stored data and then clears all the data.
    /// </summary>
    public void UpdateMbAndClear(IBuildable buildableMb, bool omitTextureCoords = false, bool omitNormals = false)
    {
      this.GetMbUpdatePackageAndClear(omitTextureCoords, omitNormals).Apply(buildableMb);
    }

    /// <summary>Returns a new mesh from the current builder state.</summary>
    public Mesh GetMeshAndClear(bool omitTextureCoords = false, bool omitNormals = false)
    {
      MbUpdatePackage updatePackageAndClear = this.GetMbUpdatePackageAndClear(omitTextureCoords, omitNormals);
      Mesh meshAndClear = new Mesh();
      meshAndClear.Clear();
      meshAndClear.vertices = updatePackageAndClear.Vertices;
      meshAndClear.triangles = updatePackageAndClear.Indices;
      meshAndClear.normals = updatePackageAndClear.Normals.ValueOrNull;
      meshAndClear.tangents = updatePackageAndClear.Tangents.ValueOrNull;
      meshAndClear.colors32 = updatePackageAndClear.Colors.ValueOrNull;
      meshAndClear.uv = updatePackageAndClear.TexCoords.ValueOrNull;
      meshAndClear.RecalculateBounds();
      return meshAndClear;
    }

    /// <summary>
    /// Returns update package of current builder state and clears all the data.
    /// </summary>
    public MbUpdatePackage GetMbUpdatePackageAndClear(bool omitTextureCoords = false, bool omitNormals = false)
    {
      Assert.That<Lyst<Vector3>>(this.m_vertices).IsNotEmpty<Vector3>("No vertices in the mesh.");
      Assert.That<Lyst<int>>(this.m_indices).IsNotEmpty<int>("No indices in the mesh.");
      Assert.That<int>(this.m_indices.Count % 3).IsZero("Index count is not divisible by 3.");
      Assert.That<bool>(omitNormals && this.m_normals.Count == 0 || !omitNormals && this.m_normals.Count == this.m_vertices.Count).IsTrue("Normals count is not right.");
      Vector3[] arrayAndClear1 = omitNormals ? (Vector3[]) null : this.m_normals.ToArrayAndClear();
      Vector4[] arrayAndClear2 = omitNormals ? (Vector4[]) null : this.m_tangents.ToArrayAndClear();
      Assert.That<bool>(this.m_colors.Count == 0 || this.m_colors.Count == this.m_vertices.Count).IsTrue("There are some colors that will be discarded.");
      Color32[] arrayAndClear3 = this.m_colors.Count == this.m_vertices.Count ? this.m_colors.ToArrayAndClear() : (Color32[]) null;
      Assert.That<bool>(omitTextureCoords || this.m_texCoords.Count == 0 || this.m_texCoords.Count == this.m_vertices.Count).IsTrue("There are some UVs that will be discarded.");
      Vector2[] arrayAndClear4 = this.m_texCoords.Count != this.m_vertices.Count || omitTextureCoords ? (Vector2[]) null : this.m_texCoords.ToArrayAndClear();
      Assert.That<bool>(omitTextureCoords || this.m_texCoords2.Count == 0 || this.m_texCoords2.Count == this.m_vertices.Count).IsTrue("There are some UVs2 that will be discarded.");
      Vector2[] arrayAndClear5 = this.m_texCoords2.Count != this.m_vertices.Count || omitTextureCoords ? (Vector2[]) null : this.m_texCoords2.ToArrayAndClear();
      MbUpdatePackage updatePackageAndClear = new MbUpdatePackage(this.m_vertices.ToArrayAndClear(), this.m_indices.ToArrayAndClear(), (Option<Vector3[]>) arrayAndClear1, (Option<Vector4[]>) arrayAndClear2, (Option<Color32[]>) arrayAndClear3, (Option<Vector2[]>) arrayAndClear4, (Option<Vector2[]>) arrayAndClear5);
      this.Clear();
      return updatePackageAndClear;
    }

    public void AddVertex(Vector3 vertex, Color32 color, Vector2 uv)
    {
      this.m_vertices.Add(vertex);
      this.m_colors.Add(color);
      this.m_texCoords.Add(uv);
    }

    /// <summary>
    /// Adds one sided triangle (clock wise). The vectors are assumed to be in unity space (swapped Y-Z). Adds 3
    /// vertices and 3 indices.
    /// </summary>
    public void AddTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Color32 color)
    {
      this.AddTriangleNoUvs(v0, v1, v2);
      this.m_colors.AddRepeated(color, 3);
      this.m_texCoords.Add(new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1f), new Vector2(1f, 0.0f));
    }

    public void AddTriangleNoUvs(Vector3 v0, Vector3 v1, Vector3 v2)
    {
      int count = this.m_vertices.Count;
      if (this.m_transformIsSet)
      {
        this.m_vertices.Add(this.m_transform.MultiplyPoint3x4(v0), this.m_transform.MultiplyPoint3x4(v1), this.m_transform.MultiplyPoint3x4(v2));
        this.m_normals.AddRepeated(this.m_transform.MultiplyVector(Vector3.Cross(v2 - v1, v0 - v1)).normalized, 3);
      }
      else
      {
        this.m_vertices.Add(v0, v1, v2);
        this.m_normals.AddRepeated(Vector3.Cross(v2 - v1, v0 - v1).normalized, 3);
      }
      this.m_indices.Add(count, count + 1, count + 2);
    }

    public void AddQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
      this.AddQuadVerticesIndices(v0, v1, v2, v3);
      this.m_texCoords.Add(new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0.0f));
    }

    /// <summary>
    /// Adds quad created as two triangles (vertices clock-wise). The quad is expected to be planar (has one normal)!
    /// If not, add two triangles instead. The vectors are assumed to be in unity space (swapped Y-Z). Adds 4
    /// vertices and 6 indices.
    /// </summary>
    public void AddQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color32 color)
    {
      this.AddQuadVerticesIndices(v0, v1, v2, v3);
      this.m_colors.AddRepeated(color, 4);
      this.m_texCoords.Add(new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0.0f));
    }

    public void AddQuad(
      Vector3 v0,
      Color32 c0,
      Vector3 v1,
      Color32 c1,
      Vector3 v2,
      Color32 c2,
      Vector3 v3,
      Color32 c3)
    {
      this.AddQuadVerticesIndices(v0, v1, v2, v3);
      this.m_colors.Add(c0, c1, c2, c3);
      this.m_texCoords.Add(new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0.0f));
    }

    public void AddQuad(
      Vector3 v0,
      Vector2 t0,
      Vector3 v1,
      Vector2 t1,
      Vector3 v2,
      Vector2 t2,
      Vector3 v3,
      Vector2 t3,
      Color32 color)
    {
      this.AddQuadVerticesIndices(v0, v1, v2, v3);
      this.m_colors.AddRepeated(color, 4);
      this.m_texCoords.Add(t0, t1, t2, t3);
    }

    public void AddQuad(
      Vector3 v0,
      Vector2 tex1v0,
      Vector2 tex2v0,
      Vector3 v1,
      Vector2 tex1v1,
      Vector2 tex2v1,
      Vector3 v2,
      Vector2 tex1v2,
      Vector2 tex2v2,
      Vector3 v3,
      Vector2 tex1v3,
      Vector2 tex2v3,
      Color32 color)
    {
      this.AddQuadVerticesIndices(v0, v1, v2, v3);
      this.m_colors.AddRepeated(color, 4);
      this.m_texCoords.Add(tex1v0, tex1v1, tex1v2, tex1v3);
      this.m_texCoords2.Add(tex2v0, tex2v1, tex2v2, tex2v3);
    }

    public void AddQuad(
      Vector3 v0,
      Vector3 v1,
      Vector3 v2,
      Vector3 v3,
      Color32 color,
      Vector2 tex1,
      Vector2 tex2)
    {
      this.AddQuadVerticesIndices(v0, v1, v2, v3);
      this.m_colors.AddRepeated(color, 4);
      this.m_texCoords.Add(tex1, tex1, tex1, tex1);
      this.m_texCoords2.Add(tex2, tex2, tex2, tex2);
    }

    public void AddQuadVerticesIndices(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
      int count = this.m_vertices.Count;
      Vector3 vector = Vector3.Cross(v2 - v1, v0 - v1) + Vector3.Cross(v0 - v3, v2 - v3);
      if (this.m_transformIsSet)
      {
        this.m_vertices.Add(this.m_transform.MultiplyPoint3x4(v0), this.m_transform.MultiplyPoint3x4(v1), this.m_transform.MultiplyPoint3x4(v2), this.m_transform.MultiplyPoint3x4(v3));
        this.m_normals.AddRepeated(this.m_transform.MultiplyVector(vector).normalized, 4);
      }
      else
      {
        this.m_vertices.Add(v0, v1, v2, v3);
        this.m_normals.AddRepeated(vector.normalized, 4);
      }
      this.m_indices.Add(count, count + 1, count + 2);
      this.m_indices.Add(count, count + 2, count + 3);
    }

    /// <summary>
    /// Adds an axis-aligned box at given <paramref name="center" /> and <paramref name="extents" />. Adds 24 vertices
    /// and 36 indices.
    /// </summary>
    public void AddAaBox(Vector3 center, Vector3 extents, Color32 color, BoxFaceMask boxFaceMask = BoxFaceMask.All)
    {
      if ((boxFaceMask & BoxFaceMask.PlusX) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(extents.x, -extents.y, -extents.z), center + new Vector3(extents.x, extents.y, -extents.z), center + new Vector3(extents.x, extents.y, extents.z), center + new Vector3(extents.x, -extents.y, extents.z), color);
      if ((boxFaceMask & BoxFaceMask.MinusX) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(-extents.x, -extents.y, -extents.z), center + new Vector3(-extents.x, -extents.y, extents.z), center + new Vector3(-extents.x, extents.y, extents.z), center + new Vector3(-extents.x, extents.y, -extents.z), color);
      if ((boxFaceMask & BoxFaceMask.PlusY) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(-extents.x, extents.y, -extents.z), center + new Vector3(-extents.x, extents.y, extents.z), center + new Vector3(extents.x, extents.y, extents.z), center + new Vector3(extents.x, extents.y, -extents.z), color);
      if ((boxFaceMask & BoxFaceMask.MinusY) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(-extents.x, -extents.y, -extents.z), center + new Vector3(extents.x, -extents.y, -extents.z), center + new Vector3(extents.x, -extents.y, extents.z), center + new Vector3(-extents.x, -extents.y, extents.z), color);
      if ((boxFaceMask & BoxFaceMask.PlusZ) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(-extents.x, -extents.y, extents.z), center + new Vector3(extents.x, -extents.y, extents.z), center + new Vector3(extents.x, extents.y, extents.z), center + new Vector3(-extents.x, extents.y, extents.z), color);
      if ((boxFaceMask & BoxFaceMask.MinusZ) == BoxFaceMask.None)
        return;
      this.AddQuad(center + new Vector3(-extents.x, -extents.y, -extents.z), center + new Vector3(-extents.x, extents.y, -extents.z), center + new Vector3(extents.x, extents.y, -extents.z), center + new Vector3(extents.x, -extents.y, -extents.z), color);
    }

    public void AddAaWireBox(Vector3 center, Vector3 extents, float wireRadius, Color32 color)
    {
      BoxFaceMask boxFaceMask1 = BoxFaceMask.PlusY | BoxFaceMask.MinusY | BoxFaceMask.PlusZ | BoxFaceMask.MinusZ;
      this.AddAaBox(new Vector3(center.x, center.y - extents.y, center.z - extents.z), new Vector3(extents.x + wireRadius, wireRadius, wireRadius), color, boxFaceMask1);
      this.AddAaBox(new Vector3(center.x, center.y - extents.y, center.z + extents.z), new Vector3(extents.x + wireRadius, wireRadius, wireRadius), color, boxFaceMask1);
      this.AddAaBox(new Vector3(center.x, center.y + extents.y, center.z - extents.z), new Vector3(extents.x + wireRadius, wireRadius, wireRadius), color, boxFaceMask1);
      this.AddAaBox(new Vector3(center.x, center.y + extents.y, center.z + extents.z), new Vector3(extents.x + wireRadius, wireRadius, wireRadius), color, boxFaceMask1);
      BoxFaceMask boxFaceMask2 = BoxFaceMask.PlusX | BoxFaceMask.MinusX | BoxFaceMask.PlusZ | BoxFaceMask.MinusZ;
      this.AddAaBox(new Vector3(center.x - extents.x, center.y, center.z - extents.z), new Vector3(wireRadius, extents.y + wireRadius, wireRadius), color, boxFaceMask2);
      this.AddAaBox(new Vector3(center.x - extents.x, center.y, center.z + extents.z), new Vector3(wireRadius, extents.y + wireRadius, wireRadius), color, boxFaceMask2);
      this.AddAaBox(new Vector3(center.x + extents.x, center.y, center.z - extents.z), new Vector3(wireRadius, extents.y + wireRadius, wireRadius), color, boxFaceMask2);
      this.AddAaBox(new Vector3(center.x + extents.x, center.y, center.z + extents.z), new Vector3(wireRadius, extents.y + wireRadius, wireRadius), color, boxFaceMask2);
      BoxFaceMask boxFaceMask3 = BoxFaceMask.PlusX | BoxFaceMask.MinusX | BoxFaceMask.PlusY | BoxFaceMask.MinusY;
      this.AddAaBox(new Vector3(center.x - extents.x, center.y - extents.y, center.z), new Vector3(wireRadius, wireRadius, extents.z + wireRadius), color, boxFaceMask3);
      this.AddAaBox(new Vector3(center.x - extents.x, center.y + extents.y, center.z), new Vector3(wireRadius, wireRadius, extents.z + wireRadius), color, boxFaceMask3);
      this.AddAaBox(new Vector3(center.x + extents.x, center.y - extents.y, center.z), new Vector3(wireRadius, wireRadius, extents.z + wireRadius), color, boxFaceMask3);
      this.AddAaBox(new Vector3(center.x + extents.x, center.y + extents.y, center.z), new Vector3(wireRadius, wireRadius, extents.z + wireRadius), color, boxFaceMask3);
    }

    /// <summary>
    /// Adds an a box between given points <paramref name="from" /> and <paramref name="to" />. Adds 24 vertices and 36
    /// indices.
    /// </summary>
    public void AddBox(Vector3 from, Vector3 to, float extents, Color32 color)
    {
      Assert.That<float>(extents).IsNotZero();
      Vector3 normalized = (to - from).normalized;
      from -= normalized * extents;
      to += normalized * extents;
      Vector3 rhs = normalized.Orthogonal().normalized * extents;
      Vector3 vector3_1 = Vector3.Cross(normalized, rhs).normalized * extents;
      Vector3 vector3_2 = rhs + vector3_1;
      Vector3 vector3_3 = rhs - vector3_1;
      Vector3 vector3_4 = from + vector3_2;
      Vector3 vector3_5 = from + vector3_3;
      Vector3 vector3_6 = from - vector3_2;
      Vector3 vector3_7 = from - vector3_3;
      Vector3 vector3_8 = to + vector3_2;
      Vector3 vector3_9 = to + vector3_3;
      Vector3 vector3_10 = to - vector3_2;
      Vector3 vector3_11 = to - vector3_3;
      this.AddQuad(vector3_4, vector3_5, vector3_6, vector3_7, color);
      this.AddQuad(vector3_8, vector3_11, vector3_10, vector3_9, color);
      this.AddQuad(vector3_5, vector3_4, vector3_8, vector3_9, color);
      this.AddQuad(vector3_6, vector3_5, vector3_9, vector3_10, color);
      this.AddQuad(vector3_7, vector3_6, vector3_10, vector3_11, color);
      this.AddQuad(vector3_4, vector3_7, vector3_11, vector3_8, color);
    }

    public void AddBoxNoEnds(
      Vector3 from,
      Vector3 to,
      float extents,
      Color32 color,
      Vector2 tex1,
      Vector2 tex2)
    {
      Assert.That<float>(extents).IsNotZero();
      Vector3 normalized = (to - from).normalized;
      from -= normalized * extents;
      to += normalized * extents;
      Vector3 rhs = normalized.Orthogonal().normalized * extents;
      Vector3 vector3_1 = Vector3.Cross(normalized, rhs).normalized * extents;
      Vector3 vector3_2 = rhs + vector3_1;
      Vector3 vector3_3 = rhs - vector3_1;
      Vector3 vector3_4 = from + vector3_2;
      Vector3 vector3_5 = from + vector3_3;
      Vector3 vector3_6 = from - vector3_2;
      Vector3 vector3_7 = from - vector3_3;
      Vector3 vector3_8 = to + vector3_2;
      Vector3 vector3_9 = to + vector3_3;
      Vector3 vector3_10 = to - vector3_2;
      Vector3 vector3_11 = to - vector3_3;
      this.AddQuad(vector3_5, vector3_4, vector3_8, vector3_9, color, tex1, tex2);
      this.AddQuad(vector3_6, vector3_5, vector3_9, vector3_10, color, tex1, tex2);
      this.AddQuad(vector3_7, vector3_6, vector3_10, vector3_11, color, tex1, tex2);
      this.AddQuad(vector3_4, vector3_7, vector3_11, vector3_8, color, tex1, tex2);
    }

    /// <summary>
    /// Adds an a box between given points <paramref name="from" /> and <paramref name="to" />. Adds 24 vertices and 36
    /// indices.
    /// </summary>
    public void AddBox(
      Vector3 from,
      Vector3 to,
      float extents,
      Color32 fromColor,
      Color32 toColor)
    {
      Assert.That<float>(extents).IsNotZero();
      Vector3 normalized = (to - from).normalized;
      from -= normalized * extents;
      to += normalized * extents;
      Vector3 rhs = normalized.Orthogonal().normalized * extents;
      Vector3 vector3_1 = Vector3.Cross(normalized, rhs).normalized * extents;
      Vector3 vector3_2 = rhs + vector3_1;
      Vector3 vector3_3 = rhs - vector3_1;
      Vector3 vector3_4 = from + vector3_2;
      Vector3 vector3_5 = from + vector3_3;
      Vector3 vector3_6 = from - vector3_2;
      Vector3 vector3_7 = from - vector3_3;
      Vector3 vector3_8 = to + vector3_2;
      Vector3 vector3_9 = to + vector3_3;
      Vector3 vector3_10 = to - vector3_2;
      Vector3 vector3_11 = to - vector3_3;
      this.AddQuad(vector3_4, vector3_5, vector3_6, vector3_7, fromColor);
      this.AddQuad(vector3_8, vector3_11, vector3_10, vector3_9, toColor);
      this.AddQuad(vector3_5, fromColor, vector3_4, fromColor, vector3_8, toColor, vector3_9, toColor);
      this.AddQuad(vector3_6, fromColor, vector3_5, fromColor, vector3_9, toColor, vector3_10, toColor);
      this.AddQuad(vector3_7, fromColor, vector3_6, fromColor, vector3_10, toColor, vector3_11, toColor);
      this.AddQuad(vector3_4, fromColor, vector3_7, fromColor, vector3_11, toColor, vector3_8, toColor);
    }

    /// <summary>
    /// Adds an axis-aligned square based pyramid at given <paramref name="center" /> and <paramref name="extents" />.
    /// Adds 20 vertices and 30 indices. The "y" axis is taken to be "up". Radius is the radius of the base.
    /// </summary>
    public void AddAaPyramid(
      Vector3 center,
      Vector3 extents,
      Color32 color,
      BoxFaceMask boxFaceMask = BoxFaceMask.All)
    {
      if ((boxFaceMask & BoxFaceMask.PlusX) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(extents.x, -extents.y, -extents.z), center + new Vector3(0.0f, extents.y, 0.0f), center + new Vector3(0.0f, extents.y, 0.0f), center + new Vector3(extents.x, -extents.y, extents.z), color);
      if ((boxFaceMask & BoxFaceMask.MinusX) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(-extents.x, -extents.y, -extents.z), center + new Vector3(-extents.x, -extents.y, extents.z), center + new Vector3(0.0f, extents.y, 0.0f), center + new Vector3(0.0f, extents.y, 0.0f), color);
      if ((boxFaceMask & BoxFaceMask.MinusY) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(-extents.x, -extents.y, -extents.z), center + new Vector3(extents.x, -extents.y, -extents.z), center + new Vector3(extents.x, -extents.y, extents.z), center + new Vector3(-extents.x, -extents.y, extents.z), color);
      if ((boxFaceMask & BoxFaceMask.PlusZ) != BoxFaceMask.None)
        this.AddQuad(center + new Vector3(-extents.x, -extents.y, extents.z), center + new Vector3(extents.x, -extents.y, extents.z), center + new Vector3(0.0f, extents.y, 0.0f), center + new Vector3(0.0f, extents.y, 0.0f), color);
      if ((boxFaceMask & BoxFaceMask.MinusZ) == BoxFaceMask.None)
        return;
      this.AddQuad(center + new Vector3(-extents.x, -extents.y, -extents.z), center + new Vector3(0.0f, extents.y, 0.0f), center + new Vector3(0.0f, extents.y, 0.0f), center + new Vector3(extents.x, -extents.y, -extents.z), color);
    }

    public void AddMesh(MeshBuilder builder)
    {
      int count = this.m_vertices.Count;
      this.m_indices.EnsureCapacity(this.m_indices.Count + builder.m_indices.Count);
      foreach (int index in builder.m_indices)
        this.m_indices.Add(index + count);
      this.m_vertices.AddRange(builder.m_vertices);
      this.m_normals.AddRange(builder.m_normals);
      this.m_tangents.AddRange(builder.m_tangents);
      this.m_colors.AddRange(builder.m_colors);
      this.m_texCoords.AddRange(builder.m_texCoords);
    }

    /// <summary>Adds Unity mesh to this mesh builder.</summary>
    public void AddMesh(Mesh mesh, bool includeColors = false)
    {
      int count = this.m_vertices.Count;
      foreach (int triangle in mesh.triangles)
        this.m_indices.Add(triangle + count);
      Vector3[] vertices = mesh.vertices;
      Vector3[] array1 = mesh.normals.CheckLength<Vector3>(vertices.Length);
      Vector4[] array2 = mesh.tangents ?? Array.Empty<Vector4>();
      Assert.That<int>(array2.Length).IsEqualTo(array1.Length);
      if (this.m_transformIsSet)
      {
        foreach (Vector3 point in vertices)
          this.m_vertices.Add(this.m_transform.MultiplyPoint3x4(point));
        foreach (Vector3 vector in array1)
          this.m_normals.Add(this.m_transform.MultiplyVector(vector));
        foreach (Vector4 vector in array2)
          this.m_tangents.Add((Vector4) this.m_transform.MultiplyVector((Vector3) vector));
      }
      else
      {
        this.m_vertices.AddRange(vertices);
        this.m_normals.AddRange(array1);
        this.m_tangents.AddRange(array2);
      }
      if (includeColors)
      {
        Color32[] colors32 = mesh.colors32;
        if (colors32.Length == vertices.Length)
          this.m_colors.AddRange(colors32);
        else
          this.m_colors.AddRepeated(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), vertices.Length);
      }
      Vector2[] uv = mesh.uv;
      if (uv.Length == vertices.Length)
        this.m_texCoords.AddRange(uv);
      else
        this.m_texCoords.AddRepeated(Vector2.zero, vertices.Length);
    }

    /// <summary>
    /// Adds all meshes from given Game Object to this mesh builder.
    /// </summary>
    public void AddAllMeshes(GameObject go, bool includeColors = false, bool useGoTransform = false)
    {
      Assert.That<int>(this.m_tempMeshFilters.Count).IsZero();
      if (go.activeInHierarchy)
        go.GetComponentsInChildren<MeshFilter>(this.m_tempMeshFilters);
      else
        this.m_tempMeshFilters.AddRange((IEnumerable<MeshFilter>) go.GetComponentsInChildren<MeshFilter>(true));
      Assert.That<int>(this.m_tempMeshFilters.Count).IsNotZero("Adding mesh with no mesh filters.");
      if (useGoTransform)
        this.SetTransform(go.transform.position, go.transform.rotation, go.transform.localScale.x);
      foreach (MeshFilter tempMeshFilter in this.m_tempMeshFilters)
        this.AddMesh(tempMeshFilter.sharedMesh, includeColors);
      if (useGoTransform)
        this.ResetTransform();
      this.m_tempMeshFilters.Clear();
    }

    /// <summary>
    /// Start extrusion of give <paramref name="crossSection" /> by just adding all vertices of the cross-section at
    /// given coordinate. This should be called once at the start of extrusion. Then call <see cref="M:Mafi.Unity.MeshBuilder.ContinueExtrusion(Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Gfx.CrossSectionVertex}},UnityEngine.Vector3,Mafi.Vector3f,Mafi.Vector3f,System.Single,Mafi.Percent)" />.
    /// </summary>
    public void StartExtrusion(
      ImmutableArray<ImmutableArray<CrossSectionVertex>> crossSection,
      Vector3 origin,
      Vector3f crossSectionPlaneNormal,
      Vector3f up,
      float textureCoordX,
      Percent scale)
    {
      Assert.That<Vector3f>(crossSectionPlaneNormal).IsNormalized("Normal is not normalized.");
      Assert.That<Vector3f>(up).IsNormalized("Up vector is not normalized.");
      Vector3f vector = crossSectionPlaneNormal.Cross(up);
      Vector3 vector3_1 = crossSectionPlaneNormal.ToVector3();
      foreach (ImmutableArray<CrossSectionVertex> immutableArray in crossSection)
      {
        foreach (CrossSectionVertex crossSectionVertex in immutableArray)
        {
          Vector3 point = origin + ((crossSectionVertex.Coord.X * new RelTile3f(vector) + crossSectionVertex.Coord.Y * new RelTile3f(up)) * scale).ToVector3();
          Vector2f normal = crossSectionVertex.Normal;
          Vector3 vector3_2 = (normal.X.Sign() * normal.X * normal.X * vector + normal.Y.Sign() * normal.Y * normal.Y * up).ToVector3();
          if (this.m_transformIsSet)
          {
            this.m_vertices.Add(this.m_transform.MultiplyPoint3x4(point));
            this.m_normals.Add(this.m_transform.MultiplyVector(vector3_2).normalized);
            this.m_tangents.Add((Vector4) this.m_transform.MultiplyVector(vector3_1));
          }
          else
          {
            this.m_vertices.Add(point);
            this.m_normals.Add(vector3_2.normalized);
            this.m_tangents.Add((Vector4) vector3_1);
          }
          this.m_texCoords.Add(new Vector2(textureCoordX, crossSectionVertex.TextureCoordY));
        }
      }
    }

    /// <summary>
    /// Continues extrusion by adding new <paramref name="crossSection" /> and connecting it with the previous one.
    /// This method can be called either after <see cref="M:Mafi.Unity.MeshBuilder.StartExtrusion(Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Gfx.CrossSectionVertex}},UnityEngine.Vector3,Mafi.Vector3f,Mafi.Vector3f,System.Single,Mafi.Percent)" /> or <see cref="M:Mafi.Unity.MeshBuilder.ContinueExtrusion(Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Gfx.CrossSectionVertex}},UnityEngine.Vector3,Mafi.Vector3f,Mafi.Vector3f,System.Single,Mafi.Percent)" />.
    /// </summary>
    public void ContinueExtrusion(
      ImmutableArray<ImmutableArray<CrossSectionVertex>> crossSection,
      Vector3 origin,
      Vector3f crossSectionPlaneNormal,
      Vector3f up,
      float textureCoordX,
      Percent scale)
    {
      Assert.That<bool>(this.IsEmpty).IsFalse("Continuing extrusion of empty builder.");
      int count = this.m_vertices.Count;
      this.StartExtrusion(crossSection, origin, crossSectionPlaneNormal, up, textureCoordX, scale);
      int num1 = count - (this.m_vertices.Count - count);
      int num2 = 0;
      foreach (ImmutableArray<CrossSectionVertex> immutableArray in crossSection)
      {
        Assert.That<int>(immutableArray.Length).IsPositive();
        ++num2;
        int length = immutableArray.Length;
        int num3 = 1;
        while (num3 < length)
        {
          int num4 = num1 + num2;
          int num5 = count + num2;
          this.m_indices.Add(num4 - 1, num5 - 1, num5);
          this.m_indices.Add(num5, num4, num4 - 1);
          ++num3;
          ++num2;
        }
      }
      Assert.That<int>(num1 + num2).IsEqualTo(count);
    }

    public void AddGrid(int cols, int rows, Vector3 origin, Vector2 size, Vector2 uvMult)
    {
      int count = this.m_vertices.Count;
      Vector3 vector3 = (Vector3) new Vector2(size.x / (float) cols, size.y / (float) rows);
      for (int index1 = 0; index1 <= rows; ++index1)
      {
        for (int index2 = 0; index2 <= cols; ++index2)
        {
          this.m_vertices.Add(origin + new Vector3((float) index2 * vector3.x, 0.0f, (float) index1 * vector3.y));
          this.m_texCoords.Add(new Vector2(uvMult.x * (float) index2 / (float) rows, uvMult.y * (float) index1 / (float) cols));
        }
      }
      for (int index3 = 0; index3 < rows; ++index3)
      {
        int num1 = count + index3 * (cols + 1);
        int num2 = count + (index3 + 1) * (cols + 1);
        for (int index4 = 0; index4 < cols; ++index4)
        {
          this.m_indices.Add(index4 + num1 + 1, index4 + num1, index4 + num2 + 1);
          this.m_indices.Add(index4 + num1, index4 + num2, index4 + num2 + 1);
        }
      }
    }

    public void StartLine2D(float lineWidth, Color color)
    {
      if (this.m_guiLineBuilder == null)
        this.m_guiLineBuilder = new MeshBuilder.GuiLineBuilder(this);
      this.m_guiLineBuilder.Initialize(lineWidth, color);
    }

    public void ContinueLine2D(Vector2 point) => this.m_guiLineBuilder.AddPoint(point);

    /// <summary>
    /// Adds three indices. Given indices are incremented by current vertex count.
    /// </summary>
    private void AddTriangleIndicesRelative(int i1, int i2, int i3)
    {
      int verticesCount = this.VerticesCount;
      this.m_indices.Add(i1 + verticesCount, i2 + verticesCount, i3 + verticesCount);
    }

    public void AddCylinderAlongZ(
      Vector3 origin,
      Vector3 extents,
      Color32 color,
      int segments = 8,
      bool omitBottomLid = false)
    {
      Vector2[] cosSinCircleCache = MeshBuilder.getCosSinCircleCache(segments);
      if (segments <= 2)
      {
        Log.Error(string.Format("Cannot make a cylinder with {0} segments.", (object) segments));
      }
      else
      {
        int count1 = this.m_vertices.Count;
        int num1 = 2 * (segments - 1);
        for (int index = 0; index < segments; ++index)
        {
          Vector2 vector2 = cosSinCircleCache[index];
          float x = vector2.x * extents.x;
          float z = vector2.y * extents.z;
          this.m_vertices.Add(origin + new Vector3(x, extents.y, z));
          this.m_vertices.Add(origin + new Vector3(x, -extents.y, z));
          this.m_normals.Add(new Vector3(x, 0.0f, z), new Vector3(x, 0.0f, z));
          this.m_colors.Add(color, color);
          int num2 = 2 * index;
          this.m_indices.Add(count1 + num2, count1 + num1 + 1, count1 + num1);
          this.m_indices.Add(count1 + num2, count1 + num2 + 1, count1 + num1 + 1);
          num1 = num2;
        }
        Assert.That<int>(this.m_vertices.Count - count1).IsEqualTo(2 * segments);
        int count2 = this.m_vertices.Count;
        for (int index = 0; index < segments; ++index)
        {
          Vector2 vector2 = cosSinCircleCache[index];
          float x = vector2.x * extents.x;
          float z = vector2.y * extents.z;
          this.m_vertices.Add(origin + new Vector3(x, extents.y, z));
          this.m_normals.Add(Vector3.up);
          this.m_colors.Add(color);
          if (index >= 2)
            this.m_indices.Add(count2 + index, count2 + index - 1, count2);
        }
        if (omitBottomLid)
          return;
        int count3 = this.m_vertices.Count;
        for (int index = 0; index < segments; ++index)
        {
          Vector2 vector2 = cosSinCircleCache[index];
          float x = vector2.x * extents.x;
          float z = vector2.y * extents.z;
          this.m_vertices.Add(origin + new Vector3(x, -extents.y, z));
          this.m_normals.Add(Vector3.down);
          this.m_colors.Add(color, color);
          if (index >= 2)
            this.m_indices.Add(count3 + index, count3 + index - 1, count3);
        }
      }
    }

    private static Vector2[] getCosSinCircleCache(int count)
    {
      if (count <= 0)
        return Array.Empty<Vector2>();
      Option<Vector2[]> option = MeshBuilder.s_cosSinCircleCache.AtOrNone<Vector2[]>(count - 1);
      if (option.HasValue)
        return option.Value;
      lock (MeshBuilder.s_cosSinCircleCache)
      {
        if (MeshBuilder.s_cosSinCircleCache.Count < count)
          MeshBuilder.s_cosSinCircleCache.Count = count;
        Vector2[] cosSinCircleCache1 = MeshBuilder.s_cosSinCircleCache[count - 1];
        if (cosSinCircleCache1 != null)
          return cosSinCircleCache1;
        Vector2[] cosSinCircleCache2 = new Vector2[count];
        for (int index = 0; index < count; ++index)
        {
          float radians = 6.28318548f * (float) index / (float) count;
          cosSinCircleCache2[index] = new Vector2(MafiMath.Cos(radians), MafiMath.Sin(radians));
        }
        MeshBuilder.s_cosSinCircleCache[count - 1] = cosSinCircleCache2;
        return cosSinCircleCache2;
      }
    }

    public static Mesh CreateChunkMesh(int tilesPerEdge, float tileSize, bool markNoLongerReadable)
    {
      int num1 = tilesPerEdge + 1;
      Vector3[] inVertices = new Vector3[num1 * num1];
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int num2 = index1 * num1;
        float z = (float) index1 * tileSize;
        for (int index2 = 0; index2 < num1; ++index2)
          inVertices[num2 + index2] = new Vector3((float) index2 * tileSize, 0.0f, z);
      }
      ushort[] indices = new ushort[tilesPerEdge * tilesPerEdge * 6];
      for (int index3 = 0; index3 < tilesPerEdge; ++index3)
      {
        int num3 = index3 * tilesPerEdge * 6;
        int num4 = index3 * num1;
        for (int index4 = 0; index4 < tilesPerEdge; ++index4)
        {
          int index5 = num3 + index4 * 6;
          int num5 = num4 + index4;
          indices[index5] = (ushort) num5;
          indices[index5 + 1] = (ushort) (num5 + num1 + 1);
          indices[index5 + 2] = (ushort) (num5 + 1);
          indices[index5 + 3] = (ushort) num5;
          indices[index5 + 4] = (ushort) (num5 + num1);
          indices[index5 + 5] = (ushort) (num5 + num1 + 1);
        }
      }
      Mesh chunkMesh = new Mesh();
      chunkMesh.SetVertices(inVertices);
      chunkMesh.SetIndices(indices, MeshTopology.Triangles, 0);
      chunkMesh.Optimize();
      chunkMesh.UploadMeshData(markNoLongerReadable);
      return chunkMesh;
    }

    public static Mesh CreateChunkMeshWithSkirt(
      int tilesPerEdge,
      float tileSize,
      float skirtSize,
      bool markNoLongerReadable)
    {
      int num1 = tilesPerEdge + 1;
      int num2 = num1 * num1;
      int num3 = 4 * num1;
      Vector3[] inVertices = new Vector3[num2 + num3];
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int num4 = index1 * num1;
        float z = (float) index1 * tileSize;
        for (int index2 = 0; index2 < num1; ++index2)
          inVertices[num4 + index2] = new Vector3((float) index2 * tileSize, 0.0f, z);
      }
      int num5 = 3 * tilesPerEdge * tilesPerEdge * 2;
      int num6 = 24 * tilesPerEdge;
      ushort[] indices = new ushort[num5 + num6];
      for (int index3 = 0; index3 < tilesPerEdge; ++index3)
      {
        int num7 = index3 * tilesPerEdge * 6;
        int num8 = index3 * num1;
        for (int index4 = 0; index4 < tilesPerEdge; ++index4)
        {
          int index5 = num7 + index4 * 6;
          int num9 = num8 + index4;
          indices[index5] = (ushort) num9;
          indices[index5 + 1] = (ushort) (num9 + num1 + 1);
          indices[index5 + 2] = (ushort) (num9 + 1);
          indices[index5 + 3] = (ushort) num9;
          indices[index5 + 4] = (ushort) (num9 + num1);
          indices[index5 + 5] = (ushort) (num9 + num1 + 1);
        }
      }
      float num10 = (float) tilesPerEdge * tileSize;
      int index6 = num2;
      int index7 = num5;
      int num11 = 0;
      while (num11 < tilesPerEdge)
      {
        inVertices[index6] = new Vector3((float) num11 * tileSize, -skirtSize, 0.0f);
        indices[index7] = (ushort) index6;
        indices[index7 + 1] = (ushort) (num11 + 1);
        indices[index7 + 2] = (ushort) (index6 + 1);
        indices[index7 + 3] = (ushort) index6;
        indices[index7 + 4] = (ushort) num11;
        indices[index7 + 5] = (ushort) (num11 + 1);
        ++num11;
        index7 += 6;
        ++index6;
      }
      Vector3[] vector3Array1 = inVertices;
      int index8 = index6;
      int index9 = index8 + 1;
      Vector3 vector3_1 = new Vector3(num10, -skirtSize, 0.0f);
      vector3Array1[index8] = vector3_1;
      int num12 = num1 - 1;
      int num13 = 0;
      while (num13 < tilesPerEdge)
      {
        inVertices[index9] = new Vector3(num10, -skirtSize, (float) num13 * tileSize);
        indices[index7] = (ushort) index9;
        indices[index7 + 1] = (ushort) (num1 * (num13 + 1) + num12);
        indices[index7 + 2] = (ushort) (index9 + 1);
        indices[index7 + 3] = (ushort) index9;
        indices[index7 + 4] = (ushort) (num1 * num13 + num12);
        indices[index7 + 5] = (ushort) (num1 * (num13 + 1) + num12);
        ++num13;
        index7 += 6;
        ++index9;
      }
      Vector3[] vector3Array2 = inVertices;
      int index10 = index9;
      int index11 = index10 + 1;
      Vector3 vector3_2 = new Vector3(num10, -skirtSize, num10);
      vector3Array2[index10] = vector3_2;
      int num14 = num1 * (num1 - 1);
      int num15 = 0;
      while (num15 < tilesPerEdge)
      {
        inVertices[index11] = new Vector3((float) num15 * tileSize, -skirtSize, num10);
        indices[index7] = (ushort) index11;
        indices[index7 + 1] = (ushort) (index11 + 1);
        indices[index7 + 2] = (ushort) (num14 + num15 + 1);
        indices[index7 + 3] = (ushort) index11;
        indices[index7 + 4] = (ushort) (num14 + num15 + 1);
        indices[index7 + 5] = (ushort) (num14 + num15);
        ++num15;
        index7 += 6;
        ++index11;
      }
      Vector3[] vector3Array3 = inVertices;
      int index12 = index11;
      int index13 = index12 + 1;
      Vector3 vector3_3 = new Vector3(num10, -skirtSize, num10);
      vector3Array3[index12] = vector3_3;
      int num16 = 0;
      while (num16 < tilesPerEdge)
      {
        inVertices[index13] = new Vector3(0.0f, -skirtSize, (float) num16 * tileSize);
        indices[index7] = (ushort) index13;
        indices[index7 + 1] = (ushort) (index13 + 1);
        indices[index7 + 2] = (ushort) (num1 * (num16 + 1));
        indices[index7 + 3] = (ushort) index13;
        indices[index7 + 4] = (ushort) (num1 * (num16 + 1));
        indices[index7 + 5] = (ushort) (num1 * num16);
        ++num16;
        index7 += 6;
        ++index13;
      }
      Vector3[] vector3Array4 = inVertices;
      int index14 = index13;
      int num17 = index14 + 1;
      Vector3 vector3_4 = new Vector3(0.0f, -skirtSize, num10);
      vector3Array4[index14] = vector3_4;
      Mesh chunkMeshWithSkirt = new Mesh();
      chunkMeshWithSkirt.SetVertices(inVertices);
      chunkMeshWithSkirt.SetIndices(indices, MeshTopology.Triangles, 0);
      chunkMeshWithSkirt.Optimize();
      chunkMeshWithSkirt.UploadMeshData(markNoLongerReadable);
      return chunkMeshWithSkirt;
    }

    public MeshBuilder()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_vertices = new Lyst<Vector3>(true);
      this.m_indices = new Lyst<int>(true);
      this.m_normals = new Lyst<Vector3>(true);
      this.m_tangents = new Lyst<Vector4>(true);
      this.m_colors = new Lyst<Color32>(true);
      this.m_texCoords = new Lyst<Vector2>(true);
      this.m_texCoords2 = new Lyst<Vector2>(true);
      this.m_translation = Vector3.zero;
      this.m_rotation = Quaternion.identity;
      this.m_scale = Vector3.one;
      this.m_transform = Matrix4x4.identity;
      this.m_tempMeshFilters = new List<MeshFilter>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static MeshBuilder()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MeshBuilder.s_cosSinCircleCache = new Lyst<Vector2[]>();
    }

    private class GuiLineBuilder
    {
      private readonly MeshBuilder m_meshBuilder;
      /// <summary>
      /// Whether we are in a process of line creation, used to check that all line creations are properly started.
      /// </summary>
      private bool m_buildInProgress;
      private float m_lineHalfWidth;
      private Color m_color;
      private Vector2? m_prevPoint;
      private bool m_lineSegmentAdded;

      public GuiLineBuilder(MeshBuilder meshBuilder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_meshBuilder = meshBuilder;
      }

      public void AddPoint(Vector2 point)
      {
        Assert.That<bool>(this.m_buildInProgress).IsTrue();
        if (!this.m_prevPoint.HasValue)
          this.m_prevPoint = new Vector2?(point);
        Vector2 vector2_1 = this.m_prevPoint.Value;
        Vector2 vector2_2 = (point - vector2_1).normalized.LeftOrthogonalVector() * this.m_lineHalfWidth;
        this.m_meshBuilder.AddVertex((Vector3) (vector2_1 - vector2_2), (Color32) this.m_color, new Vector2(0.0f, 0.0f));
        this.m_meshBuilder.AddVertex((Vector3) (vector2_1 + vector2_2), (Color32) this.m_color, new Vector2(0.0f, 1f));
        this.m_meshBuilder.AddVertex((Vector3) (point - vector2_2), (Color32) this.m_color, new Vector2(1f, 0.0f));
        this.m_meshBuilder.AddVertex((Vector3) (point + vector2_2), (Color32) this.m_color, new Vector2(1f, 1f));
        this.m_meshBuilder.AddTriangleIndicesRelative(-4, -3, -1);
        this.m_meshBuilder.AddTriangleIndicesRelative(-4, -1, -2);
        if (this.m_lineSegmentAdded)
        {
          this.m_meshBuilder.AddTriangleIndicesRelative(-6, -5, -3);
          this.m_meshBuilder.AddTriangleIndicesRelative(-6, -3, -4);
        }
        this.m_lineSegmentAdded = true;
        this.m_prevPoint = new Vector2?(point);
      }

      public void Initialize(float lineWidth, Color color)
      {
        Assert.That<float>(lineWidth).IsPositive();
        this.m_buildInProgress = true;
        this.m_lineHalfWidth = lineWidth / 2f;
        this.m_color = color;
        this.m_prevPoint = new Vector2?();
        this.m_lineSegmentAdded = false;
      }

      public void Reset() => this.m_buildInProgress = false;
    }
  }
}
