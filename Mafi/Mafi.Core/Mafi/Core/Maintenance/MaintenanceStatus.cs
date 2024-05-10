// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.MaintenanceStatus
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct MaintenanceStatus : IEquatable<MaintenanceStatus>
  {
    /// <summary>Whether the entity is currently broken.</summary>
    public readonly bool IsBroken;
    /// <summary>
    /// Current amount of maintenance points. Note that this may be more than <see cref="F:Mafi.Core.Maintenance.MaintenanceStatus.MaintenancePointsMax" />.
    /// </summary>
    public readonly PartialQuantity MaintenancePointsCurrent;
    /// <summary>Max amount of stored maintenance points.</summary>
    public readonly PartialQuantity MaintenancePointsMax;
    /// <summary>
    /// Chance to breakdown. On average, this is the amount of time this entity is broken compared to running.
    /// </summary>
    public readonly Percent CurrentBreakdownChance;
    /// <summary>
    /// When broken, this is duration in days until fixed.
    /// When not broken, this is accumulator for breakdown duration.
    /// </summary>
    public readonly Fix32 BrokenDurationDays;

    /// <summary>
    /// Amount of missing points to full buffer. If the buffer has more than max, this value is zero.
    /// </summary>
    public PartialQuantity MissingPointsToFull
    {
      get
      {
        return !(this.MaintenancePointsCurrent < this.MaintenancePointsMax) ? PartialQuantity.Zero : this.MaintenancePointsMax - this.MaintenancePointsCurrent;
      }
    }

    public MaintenanceStatus(
      bool isBroken,
      PartialQuantity maintenancePointsCurrent,
      PartialQuantity maintenancePointsMax,
      Percent currentBreakdownChance,
      Fix32 brokenDurationDays)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.IsBroken = isBroken;
      this.MaintenancePointsCurrent = maintenancePointsCurrent;
      this.MaintenancePointsMax = maintenancePointsMax;
      this.CurrentBreakdownChance = currentBreakdownChance;
      this.BrokenDurationDays = brokenDurationDays;
    }

    public bool Equals(MaintenanceStatus other)
    {
      return this.IsBroken == other.IsBroken && this.MaintenancePointsCurrent == other.MaintenancePointsCurrent && this.MaintenancePointsMax == other.MaintenancePointsMax && this.CurrentBreakdownChance == other.CurrentBreakdownChance && this.BrokenDurationDays == other.BrokenDurationDays;
    }

    public override bool Equals(object obj) => obj is MaintenanceStatus other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<bool, PartialQuantity, PartialQuantity, Percent, Fix32>(this.IsBroken, this.MaintenancePointsCurrent, this.MaintenancePointsMax, this.CurrentBreakdownChance, this.BrokenDurationDays);
    }

    public static void Serialize(MaintenanceStatus value, BlobWriter writer)
    {
      writer.WriteBool(value.IsBroken);
      PartialQuantity.Serialize(value.MaintenancePointsCurrent, writer);
      PartialQuantity.Serialize(value.MaintenancePointsMax, writer);
      Percent.Serialize(value.CurrentBreakdownChance, writer);
      Fix32.Serialize(value.BrokenDurationDays, writer);
    }

    public static MaintenanceStatus Deserialize(BlobReader reader)
    {
      return new MaintenanceStatus(reader.ReadBool(), PartialQuantity.Deserialize(reader), PartialQuantity.Deserialize(reader), Percent.Deserialize(reader), Fix32.Deserialize(reader));
    }
  }
}
