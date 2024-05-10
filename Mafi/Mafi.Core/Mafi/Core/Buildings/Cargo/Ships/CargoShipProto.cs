// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.CargoShipProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships
{
  public class CargoShipProto : EntityProto, IProtoWithIcon, IProto
  {
    /// <summary>Maximum number of modules the ship can have.</summary>
    public readonly int MaximumModulesCount;
    public readonly ImmutableArray<CargoShipModuleProto> AvailableModules;
    public readonly ImmutableArray<CargoShipProto.FuelData> AvailableFuels;
    public readonly Duration DockTransitionDuration;
    public RelTile2f DockOffset;
    public readonly CargoShipProto.Gfx Graphics;

    public override Type EntityType => typeof (CargoShip);

    public string IconPath => this.Graphics.IconPath;

    public CargoShipProto(
      EntityProto.ID id,
      Proto.Str strings,
      EntityCosts entityCosts,
      int maximumModulesCount,
      Duration dockTransitionDuration,
      RelTile2f dockOffset,
      ImmutableArray<CargoShipModuleProto> availableModules,
      ImmutableArray<CargoShipProto.FuelData> availableFuels,
      CargoShipProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, entityCosts, (EntityProto.Gfx) graphics, tags);
      Assert.That<int>(maximumModulesCount).IsPositive();
      if (availableFuels.IsEmpty)
        throw new ProtoInitException("Ship needs to have at least one fuel type assigned!");
      this.MaximumModulesCount = maximumModulesCount.CheckNotNegative();
      this.DockOffset = dockOffset;
      this.DockTransitionDuration = dockTransitionDuration;
      this.AvailableModules = availableModules;
      this.AvailableFuels = availableFuels;
      this.Graphics = graphics.CheckNotNull<CargoShipProto.Gfx>();
    }

    public CargoShipProto.Gfx GetGraphicsFor(ProductProto fuelProto)
    {
      return this.AvailableFuels.FirstOrDefault((Func<CargoShipProto.FuelData, bool>) (x => (Proto) x.FuelProto == (Proto) fuelProto))?.CustomGraphics.ValueOrNull ?? this.Graphics;
    }

    public class FuelData
    {
      public readonly ProductProto FuelProto;
      public readonly Quantity FuelPerJourneyBase;
      public readonly Quantity FuelPerJourneyPerModule;
      public readonly Option<Proto> LockingProto;
      /// <summary>
      /// Replacing this fuel with any of these fuels does not incur any extra cost.
      /// </summary>
      public readonly ImmutableArray<ProductProto> CompatibleFuels;
      /// <summary>Percent of fuel that ends up as pollution.</summary>
      public readonly Percent PollutionPercent;
      public readonly AssetValue Cost;
      /// <summary>Overrides default graphics if set.</summary>
      public readonly Option<CargoShipProto.Gfx> CustomGraphics;

      public FuelData(
        ProductProto fuelProto,
        Quantity fuelPerJourneyBase,
        Quantity fuelPerJourneyPerModule,
        Option<Proto> lockingProto,
        ImmutableArray<ProductProto> compatibleFuels,
        AssetValue cost,
        Percent pollutionPercent,
        Option<CargoShipProto.Gfx> graphics)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.FuelProto = fuelProto;
        this.FuelPerJourneyBase = fuelPerJourneyBase;
        this.FuelPerJourneyPerModule = fuelPerJourneyPerModule;
        this.LockingProto = lockingProto;
        this.CompatibleFuels = compatibleFuels;
        this.PollutionPercent = pollutionPercent;
        this.Cost = cost;
        this.CustomGraphics = graphics;
      }
    }

    public new class Gfx : EntityProto.Gfx
    {
      public static readonly CargoShipProto.Gfx EMPTY;
      public readonly string FrontPrefabPath;
      public readonly string BackPrefabPath;
      public readonly string EmptyModulePrefabPath;
      public readonly string EngineSoundPath;
      public readonly string ArrivalSoundPath;
      public readonly string DepartureSoundPath;
      /// <summary>
      /// Ship's box collider size for a ship with zero modules, with just front and back. Actual ship's box
      /// collider is based on this size and extended in the X axis by the length of all modules combined.
      /// </summary>
      public readonly RelTile3f BasicBoxColliderSize;
      /// <summary>Space available for each cargo module.</summary>
      public readonly RelTile1i ModuleSlotLength;

      /// <summary>Icon asset path to be used in UI.</summary>
      public string IconPath { get; private set; }

      public Gfx(
        string frontPrefabPath,
        string backPrefabPath,
        string emptyModulePrefabPath,
        string engineSoundPath,
        string arrivalSoundPath,
        string departureSoundPath,
        RelTile1i moduleSlotLength,
        RelTile3f basicBoxColliderSize,
        string iconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty);
        this.FrontPrefabPath = frontPrefabPath;
        this.BackPrefabPath = backPrefabPath;
        this.EmptyModulePrefabPath = emptyModulePrefabPath;
        this.EngineSoundPath = engineSoundPath;
        this.ArrivalSoundPath = arrivalSoundPath;
        this.DepartureSoundPath = departureSoundPath;
        this.ModuleSlotLength = moduleSlotLength;
        this.BasicBoxColliderSize = basicBoxColliderSize;
        this.IconPath = iconPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        CargoShipProto.Gfx.EMPTY = new CargoShipProto.Gfx(nameof (EMPTY), nameof (EMPTY), nameof (EMPTY), nameof (EMPTY), nameof (EMPTY), nameof (EMPTY), RelTile1i.Zero, RelTile3f.Zero, nameof (EMPTY));
      }
    }
  }
}
