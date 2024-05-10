// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TerrainCursorConstHeightMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
  public class TerrainCursorConstHeightMb : MonoBehaviour
  {
    private Mesh m_mesh;
    private Vector3[] m_vertices;

    public void Awake()
    {
      MeshRenderer component = this.GetComponent<MeshRenderer>();
      component.shadowCastingMode = ShadowCastingMode.Off;
      component.receiveShadows = false;
      this.m_vertices = new Vector3[4];
      this.m_mesh = this.GetComponent<MeshFilter>().mesh;
      this.m_mesh.vertices = this.m_vertices;
      this.m_mesh.normals = new Vector3[4]
      {
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up
      };
      this.m_mesh.triangles = new int[6]{ 0, 2, 1, 1, 2, 3 };
    }

    public void UpdateCursor(TilesRectSelection selection, HeightTilesF height)
    {
      Tile2i origin = selection.Origin;
      RelTile2i size = selection.Size;
      float unityUnits = height.ToUnityUnits();
      this.m_vertices[0] = new Vector3((float) origin.X, unityUnits + 0.01f, (float) origin.Y);
      this.m_vertices[1] = new Vector3((float) (origin.X + size.X), unityUnits + 0.01f, (float) origin.Y);
      this.m_vertices[2] = new Vector3((float) origin.X, unityUnits + 0.01f, (float) (origin.Y + size.Y));
      this.m_vertices[3] = new Vector3((float) (origin.X + size.X), unityUnits + 0.01f, (float) (origin.Y + size.Y));
      this.m_mesh.vertices = this.m_vertices;
      this.m_mesh.RecalculateBounds();
    }

    public void Show() => this.gameObject.SetActive(true);

    public void Hide() => this.gameObject.SetActive(false);

    public TerrainCursorConstHeightMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
