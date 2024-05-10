// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ProtoWithReservedOceanValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Terrain;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ProtoWithReservedOceanValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly StaticEntityOceanReservationManager m_oceanAreaManager;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public ProtoWithReservedOceanValidator(
      EntitiesManager entitiesManager,
      StaticEntityOceanReservationManager oceanAreaManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_oceanAreaManager = oceanAreaManager;
    }

    public static void GetStripesScaleAndAngle(
      int setIndex,
      out float stripesScale,
      out float stripesAngle)
    {
      stripesScale = (float) (0.079999998211860657 + 0.019999999552965164 * (double) setIndex);
      stripesAngle = (float) (25 + 67 * setIndex);
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (!(addRequest.Proto is IProtoWithReservedOcean proto))
        return EntityValidationResult.Success;
      Tile3i origin = addRequest.Origin;
      if (!(origin.Height < proto.MinGroundHeight))
      {
        origin = addRequest.Origin;
        if (!(origin.Height > proto.MaxGroundHeight))
        {
          ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets = proto.ReservedOceanAreasSets;
          if (reservedOceanAreasSets.IsEmpty)
            return EntityValidationResult.Success;
          bool flag1 = false;
          int num1 = int.MaxValue;
          EntityId id = new EntityId();
          int num2 = 0;
          while (true)
          {
            int num3 = num2;
            reservedOceanAreasSets = proto.ReservedOceanAreasSets;
            int length = reservedOceanAreasSets.Length;
            if (num3 < length)
            {
              reservedOceanAreasSets = proto.ReservedOceanAreasSets;
              ImmutableArray<RectangleTerrainArea2iRelative> immutableArray = reservedOceanAreasSets[num2];
              bool flag2 = true;
              int num4 = 0;
              EntityId entityId = new EntityId();
              foreach (RectangleTerrainArea2iRelative area1 in immutableArray)
              {
                RectangleTerrainArea2i area2 = addRequest.Proto.Layout.Transform(area1, addRequest.Transform);
                if (addRequest.RecordTileErrorsAndMetadata)
                {
                  Lyst<Tile2i> errorTilesStorage = addRequest.GetAdditionalErrorTilesStorage();
                  int blockedTilesCount;
                  EntityId blockingEntityId;
                  if (!this.m_oceanAreaManager.IsAreaValid(area2, errorTilesStorage, out blockedTilesCount, out blockingEntityId))
                  {
                    flag2 = false;
                    num4 += blockedTilesCount;
                    if (entityId == new EntityId())
                    {
                      entityId = blockingEntityId;
                      break;
                    }
                    break;
                  }
                }
                else if (!this.m_oceanAreaManager.IsAreaValid(area2))
                {
                  flag2 = false;
                  break;
                }
              }
              if (!flag2 && num4 < num1)
              {
                num1 = num4;
                id = entityId;
              }
              flag1 |= flag2;
              if (addRequest.RecordTileErrorsAndMetadata)
              {
                foreach (RectangleTerrainArea2iRelative area3 in immutableArray)
                {
                  RectangleTerrainArea2i area4 = addRequest.Proto.Layout.Transform(area3, addRequest.Transform);
                  float stripesScale;
                  float stripesAngle;
                  ProtoWithReservedOceanValidator.GetStripesScaleAndAngle(num2, out stripesScale, out stripesAngle);
                  addRequest.AddMetadata((IAddRequestMetadata) new OceanAreaValidationMetadata(area4, (flag2 ? ColorRgba.Green : ColorRgba.Red).SetA((byte) 28), stripesScale, stripesAngle));
                }
              }
              else if (flag2)
                break;
              ++num2;
            }
            else
              break;
          }
          if (flag1)
            return EntityValidationResult.Success;
          if (id.IsValid)
          {
            IEntity entity;
            if (this.m_entitiesManager.TryGetEntity<IEntity>(id, out entity))
              return EntityValidationResult.CreateError(TrCore.AdditionError__OceanBlockedBy.Format(entity.Prototype.Strings.Name));
            Log.Warning(string.Format("Blocked by unknown entity {0}.", (object) id));
          }
          return EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__OceanBlocked);
        }
      }
      return EntityValidationResult.CreateError(TrCore.AdditionError__InvalidHeight.Format(proto.MinGroundHeight.Value.ToStringCached(), proto.MaxGroundHeight.Value.ToStringCached()));
    }
  }
}
