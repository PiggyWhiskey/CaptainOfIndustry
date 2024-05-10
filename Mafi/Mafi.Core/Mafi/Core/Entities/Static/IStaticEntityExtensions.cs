// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IStaticEntityExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Static.Layout;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public static class IStaticEntityExtensions
  {
    /// <summary>
    /// This performs linear search. If entity has means to do this more efficiently, override this method!
    /// </summary>
    public static bool HasOccTileWithAnyConstraintAt(
      this IStaticEntity staticEntity,
      Tile2i tileCoord,
      LayoutTileConstraint constraint,
      out int testedTilesCount)
    {
      RelTile2i relTile2i = tileCoord - staticEntity.CenterTile.Xy;
      testedTilesCount = 0;
      foreach (OccupiedTileRelative occupiedTile in staticEntity.OccupiedTiles)
      {
        if (occupiedTile.RelCoord == relTile2i)
        {
          ++testedTilesCount;
          if (occupiedTile.Constraint.HasAnyConstraints(constraint))
            return true;
        }
      }
      return false;
    }
  }
}
