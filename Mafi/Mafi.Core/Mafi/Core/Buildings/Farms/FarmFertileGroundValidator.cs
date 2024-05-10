// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmFertileGroundValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Terrain;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class FarmFertileGroundValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    public static readonly ThicknessTilesF MIN_FARMABLE_THICKNESS;
    public static readonly Percent MAX_TILES_WITH_NON_FARMABLE_MATERIAL;
    private readonly TerrainManager m_terrainManager;
    private readonly FarmableManager m_farmableManager;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public FarmFertileGroundValidator(
      TerrainManager terrainManager,
      FarmableManager farmableManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_farmableManager = farmableManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (!(addRequest.Proto is FarmProto))
        return EntityValidationResult.Success;
      Tile2i xy = addRequest.Origin.Xy;
      bool flag = false;
      ThicknessTilesF zero = ThicknessTilesF.Zero;
      ThicknessTilesF thicknessTilesF1 = FarmFertileGroundValidator.MIN_FARMABLE_THICKNESS * addRequest.OccupiedVertices.Length.ScaledByRounded(FarmFertileGroundValidator.MAX_TILES_WITH_NON_FARMABLE_MATERIAL);
      EntityAddRequestData data = new EntityAddRequestData(addRequest.Transform, ignoreForCollisions: addRequest.IgnoreForCollisions.ValueOrNull, recordTileErrors: addRequest.RecordTileErrorsAndMetadata);
      LayoutEntityAddRequest otherAddRequest = (LayoutEntityAddRequest) null;
      if (addRequest.RecordTileErrorsAndMetadata)
      {
        otherAddRequest = LayoutEntityAddRequest.GetPooledInstanceToCreateEntity(addRequest.Proto, data);
        addRequest.CopyTileErrorsTo(otherAddRequest);
      }
      int num1 = 0;
      while (true)
      {
        int num2 = num1;
        ReadOnlyArray<OccupiedTileRelative> occupiedTiles = addRequest.OccupiedTiles;
        int length = occupiedTiles.Length;
        if (num2 < length)
        {
          occupiedTiles = addRequest.OccupiedTiles;
          OccupiedTileRelative occupiedTileRelative = occupiedTiles[num1];
          if (occupiedTileRelative.TileSurface.IsPhantom)
          {
            bool surfaceInWay;
            ThicknessTilesF thicknessTilesF2 = FarmFertileGroundValidator.MIN_FARMABLE_THICKNESS - this.m_farmableManager.GetFarmableThickness(xy + occupiedTileRelative.RelCoord, FarmFertileGroundValidator.MIN_FARMABLE_THICKNESS, out surfaceInWay);
            if (thicknessTilesF2.IsPositive)
            {
              zero += thicknessTilesF2.Min(FarmFertileGroundValidator.MIN_FARMABLE_THICKNESS);
              if (addRequest.RecordTileErrorsAndMetadata)
                otherAddRequest.SetTileError(num1);
              if (surfaceInWay || zero > thicknessTilesF1)
              {
                flag = true;
                if (!addRequest.RecordTileErrorsAndMetadata)
                  break;
              }
            }
          }
          ++num1;
        }
        else
          break;
      }
      if (addRequest.RecordTileErrorsAndMetadata)
      {
        if (flag)
          otherAddRequest.CopyTileErrorsTo(addRequest);
        otherAddRequest.ReturnToPool();
      }
      return !flag ? EntityValidationResult.Success : EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__NotFarmable);
    }

    static FarmFertileGroundValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FarmFertileGroundValidator.MIN_FARMABLE_THICKNESS = ThicknessTilesF.FromFraction(9L, 10L);
      FarmFertileGroundValidator.MAX_TILES_WITH_NON_FARMABLE_MATERIAL = 5.Percent();
    }
  }
}
