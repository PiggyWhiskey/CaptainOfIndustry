// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapCargoShipWreckProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public class WorldMapCargoShipWreckProto : WorldMapEntityProto
  {
    public readonly AssetValue CostToRepair;

    public override Type EntityType => typeof (WorldMapCargoShipWreck);

    public WorldMapCargoShipWreckProto(
      EntityProto.ID id,
      Proto.Str strings,
      AssetValue costToRepair,
      WorldMapEntityProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, EntityCosts.None, graphics, tags);
      this.CostToRepair = costToRepair;
    }
  }
}
