// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.OceanWaterPumpProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  public class OceanWaterPumpProto : 
    MachineProto,
    IProtoWithReservedOcean,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto
  {
    public override Type EntityType => typeof (OceanWaterPump);

    public ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> ReservedOceanAreasSets { get; }

    public HeightTilesI MinGroundHeight { get; }

    public HeightTilesI MaxGroundHeight { get; }

    public OceanWaterPumpProto(
      MachineProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity consumedPowerPerTick,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets,
      HeightTilesI minGroundHeight,
      HeightTilesI maxGroundHeight,
      MachineProto.Gfx graphics,
      int? buffersMultiplier = null,
      bool useAllRecipesAtStartOrAfterUnlock = false,
      Option<MachineProto> nextTier = default (Option<MachineProto>),
      Computing computingConsumed = default (Computing),
      int? emissionWhenRunning = null,
      bool isWasteDisposal = false,
      bool disableLogisticsByDefault = false,
      Upoints? boostCost = null)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, consumedPowerPerTick, computingConsumed, buffersMultiplier, nextTier, useAllRecipesAtStartOrAfterUnlock, animationParams, graphics, emissionWhenRunning, isWasteDisposal, disableLogisticsByDefault, boostCost);
      this.ReservedOceanAreasSets = reservedOceanAreasSets;
      this.MinGroundHeight = minGroundHeight;
      this.MaxGroundHeight = maxGroundHeight;
    }
  }
}
