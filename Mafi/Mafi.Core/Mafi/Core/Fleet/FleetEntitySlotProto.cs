// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntitySlotProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Fleet
{
  public class FleetEntitySlotProto : Proto
  {
    public readonly FleetEntitySlotProto.SlotType TypeOfSlot;
    public readonly ImmutableArray<FleetEntityPartProto> EligibleItems;
    public readonly FleetEntitySlotProto.Gfx Graphics;

    public FleetEntitySlotProto(
      Proto.ID id,
      Proto.Str strings,
      FleetEntitySlotProto.SlotType typeOfSlot,
      ImmutableArray<FleetEntityPartProto> eligibleItems,
      FleetEntitySlotProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.TypeOfSlot = typeOfSlot;
      this.EligibleItems = eligibleItems.CheckNotEmpty<FleetEntityPartProto>();
      this.Graphics = graphics;
    }

    public enum SlotType
    {
      None,
      Weapons,
      Engines,
      HullUpgrades,
      Radars,
      FuelTankUpgrades,
    }

    public new class Gfx
    {
      public static readonly FleetEntitySlotProto.Gfx Empty;
      public readonly Option<string> GoToShowIfEmpty;

      public Gfx(Option<string> goToShowIfEmpty)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.GoToShowIfEmpty = goToShowIfEmpty;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        FleetEntitySlotProto.Gfx.Empty = new FleetEntitySlotProto.Gfx((Option<string>) Option.None);
      }
    }
  }
}
