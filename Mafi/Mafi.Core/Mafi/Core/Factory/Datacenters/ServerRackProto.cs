// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Datacenters.ServerRackProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Factory.Datacenters
{
  [DebuggerDisplay("ServerRackProto: {Id}")]
  public class ServerRackProto : Proto
  {
    /// <summary>
    /// Electricity consumed every tick to allow the server to work.
    /// </summary>
    public readonly Electricity ConsumedPowerPerTick;
    /// <summary>Computing created every tick by the server.</summary>
    public readonly Computing CreatedComputingPerTick;
    /// <summary>
    /// Product (and its quantity) that the player has to provide to build the rack.
    /// </summary>
    public readonly ProductQuantity ProductToAddThis;
    /// <summary>
    /// Product (and its quantity) that the player gets when removing this rack.
    /// </summary>
    public readonly ProductQuantity ProductToRemoveThis;
    /// <summary>
    /// Coolant per rack necessary for the DataCenter to work.
    /// </summary>
    public readonly PartialQuantity CoolantInPerMonth;
    /// <summary>Reformed coolant produced per rack.</summary>
    public readonly PartialQuantity CoolantOutPerMonth;
    public readonly PartialQuantity Maintenance;
    /// <summary>
    /// Graphics-only properties that does not affect game simulation and are not needed or accessed by the game
    /// simulation.
    /// </summary>
    public readonly ServerRackProto.Gfx Graphics;

    public ServerRackProto(
      Proto.ID id,
      Proto.Str strings,
      Electricity consumedPowerPerTick,
      Computing createdComputingPerTick,
      ProductQuantity productToAddThis,
      ProductQuantity productToRemoveThis,
      PartialQuantity coolantInPerMonth,
      PartialQuantity coolantOutPerMonth,
      PartialQuantity maintenance,
      ServerRackProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, tags);
      this.ConsumedPowerPerTick = consumedPowerPerTick;
      this.CreatedComputingPerTick = createdComputingPerTick;
      this.ProductToAddThis = productToAddThis;
      this.ProductToRemoveThis = productToRemoveThis;
      this.CoolantInPerMonth = coolantInPerMonth;
      this.CoolantOutPerMonth = coolantOutPerMonth;
      this.Maintenance = maintenance;
      this.Graphics = graphics;
    }

    public new class Gfx : Proto.Gfx
    {
      public readonly string IconPath;
      public readonly string PrefabPath;
      /// <summary>
      /// Paths to subobjects of the rack prefab that represent possible front panels of the rack.
      /// The rack will be changing front panels in order to animate it.
      /// </summary>
      public readonly ImmutableArray<string> FrontPanels;
      public static readonly ServerRackProto.Gfx Empty;

      public Gfx(string iconPath, string prefabPath, ImmutableArray<string> frontPanels)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.IconPath = iconPath.CheckNotNull<string>();
        this.PrefabPath = prefabPath.CheckNotNull<string>();
        this.FrontPanels = frontPanels.CheckNotEmpty<string>();
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ServerRackProto.Gfx.Empty = new ServerRackProto.Gfx("EMPTY", "EMPTY", ImmutableArray.Create<string>(new string[1]
        {
          "EMPTY"
        }));
      }
    }
  }
}
