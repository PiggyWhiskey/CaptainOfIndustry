// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.Area2iRenderer
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
  public class Area2iRenderer
  {
    private readonly LineMb m_line;

    public bool IsShown => this.m_line.gameObject.activeSelf;

    public Area2iRenderer(LinesFactory lineFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_line = lineFactory.CreateEmptyLine(nameof (Area2iRenderer));
      this.m_line.SetColor(Color.white);
      this.m_line.SetWidth(0.5f);
      this.m_line.gameObject.layer = Layer.Custom14TerrainOverlays.ToId();
    }

    public void Show() => this.m_line.Show();

    public void Hide() => this.m_line.Hide();

    public void SetColor(Color color) => this.m_line.SetColor(color);

    public void SetWidth(float width) => this.m_line.SetWidth(width);

    public void SetArea(RectangleTerrainArea2i area, HeightTilesF height)
    {
      this.m_line.SetPoints(new Vector3[5]
      {
        area.Origin.CornerTile2f.ExtendZ(height.Value).ToVector3(),
        area.PlusXCoordExcl.CornerTile2f.ExtendZ(height.Value).ToVector3(),
        area.PlusXyCoordExcl.CornerTile2f.ExtendZ(height.Value).ToVector3(),
        area.PlusYCoordExcl.CornerTile2f.ExtendZ(height.Value).ToVector3(),
        area.Origin.CornerTile2f.ExtendZ(height.Value).ToVector3()
      });
    }

    public void SetArea(RectangleTerrainArea2i area, TerrainManager terrainManager)
    {
      this.m_line.SetPoints(new Vector3[5]
      {
        area.Origin.CornerTile2f.ExtendZ(getHeightSafe(area.Origin)).ToVector3(),
        area.PlusXCoordExcl.CornerTile2f.ExtendZ(getHeightSafe(area.PlusXCoordExcl)).ToVector3(),
        area.PlusXyCoordExcl.CornerTile2f.ExtendZ(getHeightSafe(area.PlusXyCoordExcl)).ToVector3(),
        area.PlusYCoordExcl.CornerTile2f.ExtendZ(getHeightSafe(area.PlusYCoordExcl)).ToVector3(),
        area.Origin.CornerTile2f.ExtendZ(getHeightSafe(area.Origin)).ToVector3()
      });

      Fix32 getHeightSafe(Tile2i tile)
      {
        return !terrainManager.IsValidCoord(tile) ? Fix32.Zero : terrainManager.GetHeightOrOceanSurface(tile).Value;
      }
    }

    public void Destroy() => this.m_line.gameObject.Destroy();
  }
}
