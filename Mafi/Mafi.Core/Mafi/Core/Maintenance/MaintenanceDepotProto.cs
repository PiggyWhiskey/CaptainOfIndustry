// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.MaintenanceDepotProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Maintenance
{
  public class MaintenanceDepotProto : MachineProto
  {
    public readonly Quantity MaintenanceBufferExtraCapacity;

    public override Type EntityType => typeof (MaintenanceDepot);

    public MaintenanceDepotProto(
      MachineProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity consumedPowerPerTick,
      Option<MachineProto> nextTier,
      Quantity maintenanceBufferExtraCapacity,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      MachineProto.Gfx graphics,
      bool isWasteDisposal = false,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, consumedPowerPerTick, Computing.Zero, new int?(), nextTier, false, animationParams, graphics, isWasteDisposal: isWasteDisposal, boostCost: new Upoints?(MachineProto.BOOST_COST), tags: tags);
      this.MaintenanceBufferExtraCapacity = maintenanceBufferExtraCapacity;
    }
  }
}
