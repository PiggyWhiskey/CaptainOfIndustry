// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Gfx.CrossSectionVertex
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Gfx
{
  public struct CrossSectionVertex
  {
    /// <summary>Coord in X-Z plane relative to the transport curve.</summary>
    public readonly RelTile2f Coord;
    /// <summary>Normal of this vertex.</summary>
    public readonly Vector2f Normal;
    /// <summary>Y texture coordinate. X is computed during extrusion.</summary>
    public readonly float TextureCoordY;

    /// <summary>
    /// Creates new cross section vertex. Given normal does not need to be normalized.
    /// </summary>
    public CrossSectionVertex(RelTile2f coord, Vector2f normal, float textureCoordY)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Coord = coord;
      this.Normal = normal.Normalized;
      this.TextureCoordY = textureCoordY.CheckWithinIncl(-1f, 1f);
    }
  }
}
