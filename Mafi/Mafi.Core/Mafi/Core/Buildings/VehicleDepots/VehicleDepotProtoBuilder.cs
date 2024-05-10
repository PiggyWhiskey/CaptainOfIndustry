// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.VehicleDepots.VehicleDepotProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.VehicleDepots
{
  public sealed class VehicleDepotProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public VehicleDepotProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    public VehicleDepotProtoBuilder.State Start(string name, StaticEntityProto.ID labId)
    {
      return new VehicleDepotProtoBuilder.State(this, labId, name);
    }

    public class State : LayoutEntityBuilderState<VehicleDepotProtoBuilder.State>
    {
      private readonly StaticEntityProto.ID m_id;
      private Duration? m_spawnInterval;
      private Duration? m_doorOpenCloseDuration;
      private RelTile2f? m_spawnTile;
      private RelTile2f? m_spawnDriveTile;
      private RelTile2f? m_despawnTile;
      private RelTile2f? m_despawnDriveTile;
      private Option<string> m_soundPath;
      private Electricity m_consumedPowerPerTick;
      private Option<VehicleDepotProto> m_nextTier;

      public State(VehicleDepotProtoBuilder builder, StaticEntityProto.ID id, string name)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_consumedPowerPerTick = Electricity.Zero;
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, id, name);
        this.m_id = id;
      }

      public VehicleDepotProtoBuilder.State SetNextTier(VehicleDepotProto nextTier)
      {
        this.m_nextTier = (Option<VehicleDepotProto>) nextTier;
        return this;
      }

      public VehicleDepotProtoBuilder.State SetSpawnInterval(Duration spawnInterval)
      {
        this.m_spawnInterval = new Duration?(spawnInterval);
        return this;
      }

      public VehicleDepotProtoBuilder.State SetDoorOpenCloseDuration(Duration doorOpenCloseDuration)
      {
        this.m_doorOpenCloseDuration = new Duration?(doorOpenCloseDuration);
        return this;
      }

      public VehicleDepotProtoBuilder.State SetSpawnTiles(
        RelTile2f spawnTile,
        RelTile2f spawnDriveTile)
      {
        this.m_spawnTile = new RelTile2f?(spawnTile);
        this.m_spawnDriveTile = new RelTile2f?(spawnDriveTile);
        return this;
      }

      public VehicleDepotProtoBuilder.State SetDespawnTiles(
        RelTile2f despawnTile,
        RelTile2f despawnDriveTile)
      {
        this.m_despawnTile = new RelTile2f?(despawnTile);
        this.m_despawnDriveTile = new RelTile2f?(despawnDriveTile);
        return this;
      }

      public VehicleDepotProtoBuilder.State SetConsumedPower(Electricity consumedPowerPerTick)
      {
        this.m_consumedPowerPerTick = consumedPowerPerTick;
        return this;
      }

      public VehicleDepotProtoBuilder.State SetSoundPath(string soundPath)
      {
        this.m_soundPath = (Option<string>) soundPath;
        return this;
      }

      public VehicleDepotProto BuildAndAdd()
      {
        string prefabPath = this.PrefabPath;
        Option<string> customIconPath1 = this.CustomIconPath;
        Option<string> soundPath = this.m_soundPath;
        ImmutableArray<ToolbarCategoryProto> categoriesOrThrow = this.GetCategoriesOrThrow();
        Option<string> customIconPath2 = customIconPath1;
        VehicleDepotBaseProto.Gfx graphics = new VehicleDepotBaseProto.Gfx(prefabPath, soundPath, categoriesOrThrow, customIconPath2);
        return this.AddToDb<VehicleDepotProto>(new VehicleDepotProto(this.m_id, this.Strings, this.LayoutOrThrow, this.Costs, this.m_consumedPowerPerTick, Computing.Zero, this.ValueOrThrow<Duration>(this.m_spawnInterval, "Spawn interval"), this.ValueOrThrow<Duration>(this.m_doorOpenCloseDuration, "Door open/close duration"), this.ValueOrThrow<RelTile2f>(this.m_spawnTile, "Spawn tile"), this.ValueOrThrow<RelTile2f>(this.m_spawnDriveTile, "Spawn drive tile"), this.ValueOrThrow<RelTile2f>(this.m_despawnTile, "Despawn tile"), this.ValueOrThrow<RelTile2f>(this.m_despawnDriveTile, "Despawn drive tile"), this.m_nextTier, graphics));
      }
    }
  }
}
