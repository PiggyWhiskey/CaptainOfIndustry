// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.LayoutEntityTerrainValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class LayoutEntityTerrainValidator : 
    IEntityAdditionValidator<ILayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly TerrainManager m_terrain;

    public EntityValidatorPriority Priority => EntityValidatorPriority.High;

    public LayoutEntityTerrainValidator(TerrainManager terrain)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrain = terrain;
    }

    EntityValidationResult IEntityAdditionValidator<ILayoutEntityAddRequest>.CanAdd(
      ILayoutEntityAddRequest addRequest)
    {
      Assert.That<ReadOnlyArray<OccupiedTileRelative>>(addRequest.OccupiedTiles).IsNotEmpty<OccupiedTileRelative>();
      Tile3i position = addRequest.Transform.Position;
      ReadOnlyArray<OccupiedTileRelative> occupiedTiles = addRequest.OccupiedTiles;
      for (int index = 0; index < occupiedTiles.Length; ++index)
      {
        if (!isValidTile(position.Xy + occupiedTiles[index].RelCoord, occupiedTiles[index]))
        {
          if (addRequest.RecordTileErrorsAndMetadata)
          {
            addRequest.SetTileError(index);
            for (; index < occupiedTiles.Length; ++index)
            {
              if (!isValidTile(position.Xy + occupiedTiles[index].RelCoord, occupiedTiles[index]))
                addRequest.SetTileError(index);
            }
          }
          string debugMessage = "";
          return EntityValidationResult.CreateErrorFatal((LocStrFormatted) TrCore.AdditionError__OutsideOfMap, debugMessage);
        }
      }
      for (int index = 0; index < occupiedTiles.Length; ++index)
      {
        LocStrFormatted playerMessage;
        string debugMessage;
        if (!this.isValid(occupiedTiles[index], position, out playerMessage, out debugMessage))
        {
          if (addRequest.RecordTileErrorsAndMetadata)
          {
            addRequest.SetTileError(index);
            for (; index < occupiedTiles.Length; ++index)
            {
              if (!this.isValid(occupiedTiles[index], position, out LocStrFormatted _, out string _))
                addRequest.SetTileError(index);
            }
          }
          return EntityValidationResult.CreateError(playerMessage, debugMessage);
        }
      }
      return EntityValidationResult.Success;

      bool isValidTile(Tile2i tileCoord, OccupiedTileRelative occTile)
      {
        if (!this.m_terrain.IsValidCoord(tileCoord))
          return false;
        return !occTile.Constraint.HasAnyConstraints(LayoutTileConstraint.Ground) || !this.m_terrain.IsOffLimits(this.m_terrain.GetTileIndex(tileCoord));
      }
    }

    private bool isValid(
      OccupiedTileRelative occupiedTile,
      Tile3i origin,
      out LocStrFormatted playerMessage,
      out string debugMessage)
    {
      Tile2iIndex tileIndex = this.m_terrain.GetTileIndex(origin.Xy + occupiedTile.RelCoord);
      debugMessage = "";
      if (this.m_terrain.IsBlockingBuildings(tileIndex))
      {
        playerMessage = (LocStrFormatted) TrCore.AdditionError__SomethingInWay;
        return false;
      }
      if (occupiedTile.Constraint.HasAnyConstraints(LayoutTileConstraint.Ground) && this.m_terrain.IsOcean(tileIndex))
      {
        playerMessage = (LocStrFormatted) TrCore.AdditionError__OceanNotAllowed;
        return false;
      }
      if (occupiedTile.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean) && (!this.m_terrain.IsOcean(tileIndex) || this.m_terrain.GetHeight(tileIndex) > StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT))
      {
        playerMessage = (LocStrFormatted) TrCore.AdditionError__NeedsOcean;
        return false;
      }
      playerMessage = LocStrFormatted.Empty;
      return true;
    }
  }
}
