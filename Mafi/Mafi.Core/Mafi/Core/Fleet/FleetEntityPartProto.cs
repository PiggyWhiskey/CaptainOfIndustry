// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.FleetEntityPartProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>Adds part to a fleet entity.</summary>
  public abstract class FleetEntityPartProto : Proto, IProtoWithIcon, IProto
  {
    public readonly AssetValue Value;
    public readonly FleetEntityGfx Graphics;
    public readonly UpgradableIntProto ExtraCrew;

    public string IconPath => this.Graphics.IconPath;

    protected FleetEntityPartProto(
      FleetEntityPartProto.ID id,
      Proto.Str strings,
      AssetValue value,
      int extraCrewNeeded,
      FleetEntityGfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings);
      this.Graphics = graphics.CheckNotNull<FleetEntityGfx>();
      this.Value = value.CheckNotDefaultStruct<AssetValue>();
      this.ExtraCrew = new UpgradableIntProto(extraCrewNeeded);
    }

    public virtual void ApplyTo(FleetEntity entity) => entity.CrewNeeded.Apply(this.ExtraCrew);

    public virtual void RemoveFrom(FleetEntity entity) => entity.CrewNeeded.Remove(this.ExtraCrew);

    public virtual void ApplyToStats(FleetEntityStats stats)
    {
      stats.Crew += this.ExtraCrew.BonusValue;
    }

    public FleetEntityPartProto.ID Id => new FleetEntityPartProto.ID(base.Id.Value);

    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    [DebuggerDisplay("{Value,nq}")]
    public new readonly struct ID : 
      IEquatable<FleetEntityPartProto.ID>,
      IComparable<FleetEntityPartProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(FleetEntityPartProto.ID id)
      {
        return new Proto.ID(id.Value);
      }

      public static bool operator ==(Proto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetEntityPartProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetEntityPartProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(FleetEntityPartProto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(FleetEntityPartProto.ID lhs, FleetEntityPartProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is FleetEntityPartProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(FleetEntityPartProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(FleetEntityPartProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(FleetEntityPartProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static FleetEntityPartProto.ID Deserialize(BlobReader reader)
      {
        return new FleetEntityPartProto.ID(reader.ReadString());
      }
    }
  }
}
