// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.StaticEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public abstract class StaticEntityProto : EntityProto, IStaticEntityProto, IEntityProto, IProto
  {
    public readonly Duration ConstructionDurationPerProduct;
    /// <summary>
    /// If specified, only allowed vehicle goals are in this height range relative to entity origin (+- threshold).
    /// </summary>
    public readonly ThicknessIRange? VehicleGoalHeightAllowedRange;
    public readonly bool DoNotStartConstructionAutomatically;
    public readonly StaticEntityProto.Gfx Graphics;

    public StaticEntityProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityCosts costs,
      Duration constructionDurationPerProduct,
      ThicknessIRange? vehicleGoalHeightAllowedRange,
      StaticEntityProto.Gfx graphics,
      bool doNotStartConstructionAutomatically = false,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((EntityProto.ID) id, strings, costs, (EntityProto.Gfx) graphics, tags);
      this.VehicleGoalHeightAllowedRange = vehicleGoalHeightAllowedRange;
      this.ConstructionDurationPerProduct = constructionDurationPerProduct.CheckPositive();
      this.Graphics = graphics.CheckNotNull<StaticEntityProto.Gfx>();
      this.DoNotStartConstructionAutomatically = doNotStartConstructionAutomatically;
    }

    public StaticEntityProto.ID Id => new StaticEntityProto.ID(base.Id.Value);

    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    [DebuggerDisplay("{Value,nq}")]
    public new readonly struct ID : 
      IEquatable<StaticEntityProto.ID>,
      IComparable<StaticEntityProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Entities.EntityProto.ID" />.
      /// </summary>
      public static implicit operator EntityProto.ID(StaticEntityProto.ID id)
      {
        return new EntityProto.ID(id.Value);
      }

      public static bool operator ==(EntityProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(StaticEntityProto.ID lhs, EntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(StaticEntityProto.ID lhs, EntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(StaticEntityProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, StaticEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(StaticEntityProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, StaticEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(StaticEntityProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(StaticEntityProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(StaticEntityProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is StaticEntityProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(StaticEntityProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(StaticEntityProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(StaticEntityProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static StaticEntityProto.ID Deserialize(BlobReader reader)
      {
        return new StaticEntityProto.ID(reader.ReadString());
      }
    }

    public new class Gfx : EntityProto.Gfx
    {
      public static readonly StaticEntityProto.Gfx Empty;
      /// <summary>
      /// Whether to not display blocked ports icon for this entity.
      /// </summary>
      public readonly bool HideBlockedPortsIcon;

      protected Gfx(ColorRgba color, bool hideBlockedPortsIcon)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(color);
        this.HideBlockedPortsIcon = hideBlockedPortsIcon;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        StaticEntityProto.Gfx.Empty = new StaticEntityProto.Gfx(ColorRgba.Empty, false);
      }
    }
  }
}
