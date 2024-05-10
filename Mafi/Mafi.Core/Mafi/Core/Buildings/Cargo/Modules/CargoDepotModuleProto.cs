// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotModuleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [DebuggerDisplay("CargoDepotModuleProto: {Id}")]
  public class CargoDepotModuleProto : 
    StorageBaseProto,
    IProtoWithUpgrade<CargoDepotModuleProto>,
    IProtoWithUpgrade,
    IProto
  {
    public readonly Electricity ConsumedPowerForCranePerTick;
    public readonly ProductType ProductType;
    public readonly Quantity QuantityPerExchange;
    public readonly Duration DurationPerExchange;
    /// <summary>If false, it has pipe crane animation.</summary>
    public readonly bool HasCraneAnimation;
    /// <summary>
    /// How far from the beginning of the animation is the point where cargo get's transferred to / from the ship.
    /// Inverse from 100 gives import value (import anim. is reversed export anim).
    /// </summary>
    public readonly Percent PercentOfAnimationToDropCargoToShip;
    public readonly CargoDepotModuleProto.Gfx Graphics;

    public override Type EntityType => typeof (CargoDepotModule);

    public UpgradeData<CargoDepotModuleProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public bool HasPipeCraneAnimation => !this.HasCraneAnimation;

    public CargoDepotModuleProto(
      CargoDepotModuleProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      ProductType productType,
      Option<CargoDepotModuleProto> nextTier,
      Quantity capacity,
      Quantity quantityPerExchange,
      Duration durationPerExchange,
      Electricity consumedPowerForCranePerTick,
      bool hasCraneAnimation,
      Percent percentOfAnimationToDropCargoToShip,
      EntityCosts costs,
      CargoDepotModuleProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityProto.ID id1 = (StaticEntityProto.ID) id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      Quantity capacity1 = capacity;
      EntityCosts costs1 = costs;
      CargoDepotModuleProto.Gfx graphics1 = graphics;
      IEnumerable<Tag> tags1 = tags;
      Quantity? transferLimit = new Quantity?();
      Duration? transferLimitDuration = new Duration?();
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, capacity1, costs1, (LayoutEntityProto.Gfx) graphics1, transferLimit, transferLimitDuration, tags2);
      this.QuantityPerExchange = quantityPerExchange.CheckPositive();
      this.DurationPerExchange = durationPerExchange.CheckPositive();
      this.ConsumedPowerForCranePerTick = consumedPowerForCranePerTick;
      this.HasCraneAnimation = hasCraneAnimation;
      this.PercentOfAnimationToDropCargoToShip = percentOfAnimationToDropCargoToShip.CheckPositive();
      this.ProductType = productType;
      this.Upgrade = new UpgradeData<CargoDepotModuleProto>(this, nextTier);
      this.Graphics = graphics;
    }

    public CargoDepotModuleProto.ID Id => new CargoDepotModuleProto.ID(base.Id.Value);

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly CargoDepotModuleProto.Gfx Empty;
      public readonly string CranePrefabPath;

      public Gfx(
        string prefabPath,
        RelTile3f prefabOffset,
        string cranePrefabPath,
        Option<string> customIconPath,
        ColorRgba color,
        ImmutableArray<ToolbarCategoryProto> categories)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        string prefabPath1 = prefabPath;
        RelTile3f prefabOrigin = prefabOffset;
        Option<string> customIconPath1 = customIconPath;
        ColorRgba color1 = color;
        ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(categories);
        LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
        ImmutableArray<ToolbarCategoryProto>? categories1 = nullable;
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, prefabOrigin, customIconPath1, color1, visualizedLayers: visualizedLayers, categories: categories1, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
        this.CranePrefabPath = cranePrefabPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        CargoDepotModuleProto.Gfx.Empty = new CargoDepotModuleProto.Gfx("EMPTY", RelTile3f.Zero, "EMPTY", (Option<string>) "EMPTY", ColorRgba.Black, ImmutableArray<ToolbarCategoryProto>.Empty);
      }
    }

    /// <summary>
    /// Specifies a different than default animation contoller for a crane.
    /// </summary>
    public struct CraneAnimationControllerOverride
    {
      public readonly HeightTilesI Height;
      public readonly string AnimationControllerPath;

      public CraneAnimationControllerOverride(HeightTilesI height, string animationControllerPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Height = height;
        this.AnimationControllerPath = animationControllerPath;
      }
    }

    [ManuallyWrittenSerialization]
    [DebuggerDisplay("{Value,nq}")]
    [DebuggerStepThrough]
    public new readonly struct ID : 
      IEquatable<CargoDepotModuleProto.ID>,
      IComparable<CargoDepotModuleProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Entities.Static.StaticEntityProto.ID" />.
      /// </summary>
      public static implicit operator StaticEntityProto.ID(CargoDepotModuleProto.ID id)
      {
        return new StaticEntityProto.ID(id.Value);
      }

      public static bool operator ==(StaticEntityProto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotModuleProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(StaticEntityProto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotModuleProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Entities.EntityProto.ID" />.
      /// </summary>
      public static implicit operator EntityProto.ID(CargoDepotModuleProto.ID id)
      {
        return new EntityProto.ID(id.Value);
      }

      public static bool operator ==(EntityProto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotModuleProto.ID lhs, EntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityProto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotModuleProto.ID lhs, EntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(CargoDepotModuleProto.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotModuleProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotModuleProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotModuleProto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotModuleProto.ID lhs, CargoDepotModuleProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is CargoDepotModuleProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(CargoDepotModuleProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(CargoDepotModuleProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(CargoDepotModuleProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static CargoDepotModuleProto.ID Deserialize(BlobReader reader)
      {
        return new CargoDepotModuleProto.ID(reader.ReadString());
      }
    }
  }
}
