// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainCircleRenderer
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
  public class TerrainCircleRenderer
  {
    private readonly LineMb m_line;

    public bool IsShown => this.m_line.gameObject.activeSelf;

    public TerrainCircleRenderer(LinesFactory lineFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_line = lineFactory.CreateEmptyLine(nameof (TerrainCircleRenderer));
      this.m_line.SetColor(Color.white);
      this.m_line.SetWidth(0.5f);
    }

    public void Show() => this.m_line.Show();

    public void Hide() => this.m_line.Hide();

    public void SetColor(Color color) => this.m_line.SetColor(color);

    public void SetWidth(float width) => this.m_line.SetWidth(width);

    public void SetCircle(Tile2f centre, RelTile1i radius, HeightTilesF height)
    {
      float unityUnits1 = radius.ToUnityUnits();
      float unityUnits2 = height.ToUnityUnits();
      float unityUnits3 = centre.X.Tiles().ToUnityUnits();
      float unityUnits4 = centre.Y.Tiles().ToUnityUnits();
      int num = radius.Value.Max(16) * 2;
      Vector3[] positions = new Vector3[num + 1];
      for (int index = 0; index <= num; ++index)
      {
        float f = (float) index * 6.28318548f / (float) num;
        positions[index] = new Vector3(unityUnits3 + Mathf.Cos(f) * unityUnits1, unityUnits2, unityUnits4 + Mathf.Sin(f) * unityUnits1);
      }
      this.m_line.SetPoints(positions);
    }

    public void SetCircle(Tile2f centre, RelTile1i radius, TerrainManager terrainManager)
    {
      int num1 = radius.Value;
      int num2 = radius.Value.Max(16);
      Vector3[] positions = new Vector3[num2 + 1];
      for (int index = 0; index <= num2; ++index)
      {
        Fix32 fix32 = num2 / Fix32.Tau;
        Tile2f tile2f = new Tile2f(centre.X + fix32.Cos() * num1, centre.Y + fix32.Sin() * num1);
        positions[index] = tile2f.ExtendZ(getHeightSafe(tile2f.Tile2i)).ToVector3();
      }
      this.m_line.SetPoints(positions);

      Fix32 getHeightSafe(Tile2i tile)
      {
        return !terrainManager.IsValidCoord(tile) ? Fix32.Zero : terrainManager.GetHeightOrOceanSurface(centre.Tile2i).Value;
      }
    }

    public void Destroy() => this.m_line.gameObject.Destroy();
  }
}
