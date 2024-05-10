// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.ILayoutEntityProtoWithElevationValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Terrain;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ILayoutEntityProtoWithElevationValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator,
    IEntityPreAddValidator
  {
    private readonly TerrainManager m_terrainManager;
    private readonly TransportsManager m_transportsManager;
    [CanBeNull]
    private LayoutEntityAddRequest m_lastAddRequest;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public ILayoutEntityProtoWithElevationValidator(
      TerrainManager terrainManager,
      TransportsManager transportsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_transportsManager = transportsManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (!(addRequest.Proto is ILayoutEntityProtoWithElevation proto) || !proto.CanBeElevated)
        return EntityValidationResult.Success;
      Tile2i origin = addRequest.Origin.Xy;
      HeightTilesI originHeight = addRequest.Origin.Height;
      for (int index = 0; index < addRequest.OccupiedTiles.Length; ++index)
      {
        LocStrFormatted err;
        if (!isValid(addRequest.OccupiedTiles[index], out err))
        {
          if (addRequest.RecordTileErrorsAndMetadata)
          {
            addRequest.SetTileError(index);
            for (; index < addRequest.OccupiedTiles.Length; ++index)
            {
              if (!isValid(addRequest.OccupiedTiles[index], out LocStrFormatted _))
                addRequest.SetTileError(index);
            }
          }
          return EntityValidationResult.CreateError(err);
        }
      }
      this.m_lastAddRequest = addRequest;
      return EntityValidationResult.Success;

      bool isValid(OccupiedTileRelative relTile, out LocStrFormatted err)
      {
        Tile2i tile2i = origin + relTile.RelCoord;
        Tile2iIndex tileIndex = this.m_terrainManager.GetTileIndex(tile2i);
        HeightTilesF height = this.m_terrainManager.GetHeight(tileIndex);
        if (this.m_terrainManager.IsOcean(tileIndex) && originHeight < HeightTilesI.One)
        {
          err = (LocStrFormatted) TrCore.AdditionError__OceanNotAllowed;
          return false;
        }
        if (height > originHeight)
        {
          err = (LocStrFormatted) TrCore.AdditionError__TerrainTooHigh;
          return false;
        }
        if (height == originHeight)
        {
          err = new LocStrFormatted();
          return true;
        }
        if (!relTile.Constraint.HasAnyConstraints(LayoutTileConstraint.UsingPillar))
        {
          err = new LocStrFormatted();
          return true;
        }
        if (this.m_transportsManager.CanBuildOrExtendPillarAt(tile2i, originHeight))
        {
          err = new LocStrFormatted();
          return true;
        }
        err = (LocStrFormatted) TrCore.TrAdditionError__NoPillars;
        return false;
      }
    }

    public void PrepareForAdd()
    {
      if (this.m_lastAddRequest == null || !(this.m_lastAddRequest.Proto is ILayoutEntityProtoWithElevation proto) || !proto.CanBeElevated)
      {
        this.m_lastAddRequest = (LayoutEntityAddRequest) null;
      }
      else
      {
        Tile2i xy = this.m_lastAddRequest.Origin.Xy;
        foreach (OccupiedTileRelative occupiedTile in this.m_lastAddRequest.OccupiedTiles)
        {
          Tile2i tile = xy + occupiedTile.RelCoord;
          HeightTilesF height1 = this.m_terrainManager.GetHeight(tile);
          Tile3i origin = this.m_lastAddRequest.Origin;
          HeightTilesI height2 = origin.Height;
          if (!(height1 >= height2) && occupiedTile.Constraint.HasAnyConstraints(LayoutTileConstraint.UsingPillar))
          {
            TransportsManager transportsManager = this.m_transportsManager;
            Tile2i position = tile;
            origin = this.m_lastAddRequest.Origin;
            HeightTilesI height3 = origin.Height;
            transportsManager.BuildOrExtendPillarNoChecks(position, height3, true);
          }
        }
        this.m_lastAddRequest = (LayoutEntityAddRequest) null;
      }
    }
  }
}
