// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RocketTransporters.RocketTransporterProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.SpaceProgram;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.RocketTransporters
{
  public class RocketTransporterProto : DrivingEntityProto
  {
    public readonly TransportedRocketBaseProto RocketProto;
    public readonly Duration RocketHolderExtensionDuration;
    public readonly RocketTransporterProto.Gfx Graphics;

    public override Type EntityType => typeof (RocketTransporter);

    public RocketTransporterProto(
      DynamicEntityProto.ID id,
      TransportedRocketBaseProto transportedRocketProto,
      RelTile3f entitySize,
      DrivingData drivingData,
      VehiclePathFindingParams pathFindingParams,
      ImmutableArray<ThicknessTilesF> disruptionByDistance,
      Duration rocketHolderExtensionDuration,
      Option<DrivingEntityProto> nextTier,
      RocketTransporterProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, transportedRocketProto.Strings, entitySize, transportedRocketProto.Costs, 0, drivingData, Option<Mafi.Core.Entities.Dynamic.FuelTankProto>.None, pathFindingParams, disruptionByDistance, transportedRocketProto.DurationToBuild, nextTier, (DynamicGroundEntityProto.Gfx) graphics);
      this.RocketProto = transportedRocketProto;
      this.RocketHolderExtensionDuration = rocketHolderExtensionDuration;
      this.Graphics = graphics.CheckNotNull<RocketTransporterProto.Gfx>();
    }

    public new class Gfx : DynamicGroundEntityProto.Gfx
    {
      public readonly string LeftTrackModelName;
      public readonly string RightTrackModelName;
      public readonly RelTile1f SpacingBetweenTracks;
      public readonly RelTile1f TrackTextureLength;

      public Gfx(
        string prefabPath,
        Option<string> customIconPath,
        RelTile2f frontContactPtsOffset,
        RelTile2f rearContactPtsOffset,
        ImmutableArray<DynamicEntityDustParticlesSpec> dustParticles,
        Option<VehicleExhaustParticlesSpec> exhaustParticlesSpec,
        string engineSoundPath,
        string movementSoundPath,
        string leftTrackModelName,
        string rightTrackModelName,
        RelTile1f spacingBetweenTracks,
        RelTile1f trackTextureLength)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, customIconPath, frontContactPtsOffset, rearContactPtsOffset, dustParticles, exhaustParticlesSpec, engineSoundPath, movementSoundPath);
        this.LeftTrackModelName = leftTrackModelName;
        this.RightTrackModelName = rightTrackModelName;
        this.SpacingBetweenTracks = spacingBetweenTracks;
        this.TrackTextureLength = trackTextureLength;
      }
    }
  }
}
