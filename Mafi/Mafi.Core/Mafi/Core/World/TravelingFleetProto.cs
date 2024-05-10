// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.TravelingFleetProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Fleet;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.World
{
  public class TravelingFleetProto : EntityProto
  {
    public readonly Percent StartingHp;
    public readonly Percent MinOperableHp;
    public readonly RelTile2f ShipyardOffset;
    public readonly int CargoAndRefugeesCapacity;
    public readonly Duration DockTransitionDuration;
    public readonly FleetEntityHullProto InitialHullProto;
    public readonly FleetEnginePartProto InitialEngine;
    public readonly FleetBridgePartProto InitialBridge;
    public readonly TravelingFleetProto.Gfx Graphics;

    public override Type EntityType => typeof (TravelingFleet);

    public TravelingFleetProto(
      EntityProto.ID id,
      Proto.Str strings,
      Percent startingHp,
      Percent minOperableHp,
      RelTile2f shipyardOffset,
      int cargoAndRefugeesCapacity,
      Duration dockTransitionDuration,
      FleetEntityHullProto initialHullProto,
      FleetEnginePartProto initialEngine,
      FleetBridgePartProto initialBridge,
      TravelingFleetProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, new EntityCosts(AssetValue.Empty, 0), (EntityProto.Gfx) graphics);
      this.StartingHp = startingHp.CheckPositive();
      this.MinOperableHp = minOperableHp.CheckPositive();
      this.ShipyardOffset = shipyardOffset;
      this.CargoAndRefugeesCapacity = cargoAndRefugeesCapacity.CheckPositive();
      this.DockTransitionDuration = dockTransitionDuration.CheckPositive();
      this.InitialHullProto = initialHullProto.CheckNotNull<FleetEntityHullProto>();
      this.InitialEngine = initialEngine.CheckNotNull<FleetEnginePartProto>();
      this.InitialBridge = initialBridge;
      this.Graphics = graphics;
    }

    public new class Gfx : EntityProto.Gfx
    {
      public static readonly TravelingFleetProto.Gfx EMPTY;
      public readonly string EngineSoundPath;
      public readonly string ArrivalSoundPath;
      public readonly string DepartureSoundPath;

      public Gfx(string engineSoundPath, string arrivalSoundPath, string departureSoundPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty);
        this.EngineSoundPath = engineSoundPath;
        this.ArrivalSoundPath = arrivalSoundPath;
        this.DepartureSoundPath = departureSoundPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TravelingFleetProto.Gfx.EMPTY = new TravelingFleetProto.Gfx(nameof (EMPTY), nameof (EMPTY), nameof (EMPTY));
      }
    }
  }
}
