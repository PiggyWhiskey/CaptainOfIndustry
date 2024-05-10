// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainRectSelection
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  /// <summary>
  /// Highlights a rectangular area on terrain by drawing lines on the border.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TerrainRectSelection
  {
    private readonly Option<TerrainManager> m_terrainManager;
    private readonly Vector3[] m_points;
    private readonly LineMb m_areaOutline;
    private bool m_wasDestroyed;

    public bool IsShown => this.m_areaOutline.gameObject.activeSelf;

    public TerrainRectSelection(LinesFactory lineFactory, Option<TerrainManager> terrainManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_points = new Vector3[5];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_areaOutline = lineFactory.CreateLinePooled(this.m_points, 0.5f, Color.black);
      this.m_areaOutline.Hide();
    }

    /// <returns>
    /// True if the outline was successfully created, false if a phantom tile was hit while creating the outline. If
    /// creation of outline was unsuccessful the outline then stays the same as before.
    /// </returns>
    public void SetArea(RectangleTerrainArea2i area, Color color, HeightTilesF height = default (HeightTilesF))
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      if (area.Size.IsZero)
        this.Hide();
      if (this.m_terrainManager.HasValue)
      {
        area = area.ClampToTerrainBounds(this.m_terrainManager.Value);
        Vector3[] points1 = this.m_points;
        Vector3[] points2 = this.m_points;
        TerrainTile terrainTile = this.m_terrainManager.Value[area.Origin];
        Vector3 vector3_1;
        Vector3 vector3_2 = vector3_1 = terrainTile.CornerTile3f.ToVector3();
        points2[4] = vector3_1;
        Vector3 vector3_3 = vector3_2;
        points1[0] = vector3_3;
        this.m_points[1] = this.m_terrainManager.Value[area.PlusXCoordExcl].CornerTile3f.ToVector3();
        this.m_points[2] = this.m_terrainManager.Value[area.PlusXyCoordExcl].CornerTile3f.ToVector3();
        this.m_points[3] = this.m_terrainManager.Value[area.PlusYCoordExcl].CornerTile3f.ToVector3();
      }
      else
      {
        Vector3[] points3 = this.m_points;
        Vector3[] points4 = this.m_points;
        Tile2f cornerTile2f = area.Origin.CornerTile2f;
        Vector3 vector3_4;
        Vector3 vector3_5 = vector3_4 = cornerTile2f.ExtendHeight(height).ToVector3();
        points4[4] = vector3_4;
        Vector3 vector3_6 = vector3_5;
        points3[0] = vector3_6;
        Vector3[] points5 = this.m_points;
        Tile2i tile2i = area.PlusXCoordExcl;
        cornerTile2f = tile2i.CornerTile2f;
        Vector3 vector3_7 = cornerTile2f.ExtendHeight(height).ToVector3();
        points5[1] = vector3_7;
        Vector3[] points6 = this.m_points;
        tile2i = area.PlusXyCoordExcl;
        cornerTile2f = tile2i.CornerTile2f;
        Vector3 vector3_8 = cornerTile2f.ExtendHeight(height).ToVector3();
        points6[2] = vector3_8;
        Vector3[] points7 = this.m_points;
        cornerTile2f = area.PlusYCoordExcl.CornerTile2f;
        Vector3 vector3_9 = cornerTile2f.ExtendHeight(height).ToVector3();
        points7[3] = vector3_9;
      }
      this.m_areaOutline.SetPoints(this.m_points);
      this.m_areaOutline.SetColor(color);
      this.Show();
    }

    public void SetColor(Color color)
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_areaOutline.SetColor(color);
    }

    public void Show()
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_areaOutline.Show();
    }

    public void Hide()
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_areaOutline.Hide();
    }

    public void SetParent(GameObject parentGo, bool worldPositionStays)
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_areaOutline.transform.SetParent(parentGo.transform, worldPositionStays);
    }

    public void Destroy()
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_wasDestroyed = true;
      this.m_areaOutline.ReturnToPool();
    }
  }
}
