// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainAreaRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public class TerrainAreaRenderer
  {
    private static readonly int COLOR_PROP_ID;
    private static readonly int STRIPES_PARAMS_PROP_ID;
    private readonly GameObject m_go;
    private readonly BuildableMb m_buildableMb;
    private readonly Material m_material;
    private bool m_wasDestroyed;

    public bool IsShown => this.m_go.activeSelf;

    public TerrainAreaRenderer(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_go = new GameObject("TerrainArea");
      this.m_go.layer = Layer.Custom14TerrainOverlays.ToId();
      this.m_buildableMb = this.m_go.AddComponent<BuildableMb>();
      this.m_material = assetsDb.GetClonedMaterial("Assets/Base/Ships/DockingAreaStripes.mat");
      this.m_go.GetComponent<MeshRenderer>().sharedMaterial = this.m_material;
      this.Hide();
    }

    public void ShowArea(
      RectangleTerrainArea2i area,
      Color color,
      HeightTilesF height,
      float stripesScale = 0.1f,
      float stripesAngle = 45f)
    {
      MeshBuilder instance = MeshBuilder.Instance;
      MeshBuilder meshBuilder = instance;
      Tile2f cornerTile2f = area.Origin.CornerTile2f;
      Vector3 vector3_1 = cornerTile2f.ExtendHeight(height).ToVector3();
      cornerTile2f = area.PlusYCoordExcl.CornerTile2f;
      Vector3 vector3_2 = cornerTile2f.ExtendHeight(height).ToVector3();
      cornerTile2f = area.PlusXyCoordExcl.CornerTile2f;
      Vector3 vector3_3 = cornerTile2f.ExtendHeight(height).ToVector3();
      cornerTile2f = area.PlusXCoordExcl.CornerTile2f;
      Vector3 vector3_4 = cornerTile2f.ExtendHeight(height).ToVector3();
      meshBuilder.AddQuad(vector3_1, vector3_2, vector3_3, vector3_4);
      instance.UpdateMbAndClear((IBuildable) this.m_buildableMb, true);
      this.m_material.SetColor(TerrainAreaRenderer.COLOR_PROP_ID, color);
      this.m_material.SetVector(TerrainAreaRenderer.STRIPES_PARAMS_PROP_ID, new Vector4(stripesScale, stripesAngle, 0.0f, 0.0f));
      this.Show();
    }

    public void ShowAreas(
      IEnumerable<RectangleTerrainArea2i> areas,
      Color color,
      HeightTilesF height,
      float stripesScale = 0.1f,
      float stripesAngle = 45f)
    {
      MeshBuilder instance = MeshBuilder.Instance;
      foreach (RectangleTerrainArea2i area in areas)
      {
        MeshBuilder meshBuilder = instance;
        Tile2f cornerTile2f = area.Origin.CornerTile2f;
        Vector3 vector3_1 = cornerTile2f.ExtendHeight(height).ToVector3();
        cornerTile2f = area.PlusYCoordExcl.CornerTile2f;
        Vector3 vector3_2 = cornerTile2f.ExtendHeight(height).ToVector3();
        cornerTile2f = area.PlusXyCoordExcl.CornerTile2f;
        Vector3 vector3_3 = cornerTile2f.ExtendHeight(height).ToVector3();
        cornerTile2f = area.PlusXCoordExcl.CornerTile2f;
        Vector3 vector3_4 = cornerTile2f.ExtendHeight(height).ToVector3();
        meshBuilder.AddQuad(vector3_1, vector3_2, vector3_3, vector3_4);
      }
      instance.UpdateMbAndClear((IBuildable) this.m_buildableMb, true);
      this.m_material.SetColor(TerrainAreaRenderer.COLOR_PROP_ID, color);
      this.m_material.SetVector(TerrainAreaRenderer.STRIPES_PARAMS_PROP_ID, new Vector4(stripesScale, stripesAngle, 0.0f, 0.0f));
      this.Show();
    }

    public void Show()
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_go.SetActive(true);
    }

    public void Hide()
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_go.SetActive(false);
    }

    public void Destroy()
    {
      Assert.That<bool>(this.m_wasDestroyed).IsFalse();
      this.m_wasDestroyed = true;
      this.m_go.Destroy();
    }

    static TerrainAreaRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TerrainAreaRenderer.COLOR_PROP_ID = Shader.PropertyToID("_Color");
      TerrainAreaRenderer.STRIPES_PARAMS_PROP_ID = Shader.PropertyToID("_StripesParams");
    }
  }
}
