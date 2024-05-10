// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Datacenters.DataCenterProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
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
namespace Mafi.Core.Factory.Datacenters
{
  [DebuggerDisplay("DataCenterProto: {Id}")]
  public class DataCenterProto : LayoutEntityProto
  {
    /// <summary>How many racks this datacenter can fit in.</summary>
    public readonly int RacksCapacity;
    /// <summary>
    /// Coolant per rack necessary for the DataCenter to work.
    /// </summary>
    public readonly ProductProto CoolantIn;
    /// <summary>Reformed coolant produced per rack.</summary>
    public readonly ProductProto CoolantOut;
    public readonly Quantity CoolantCapacity;
    public ImmutableArray<char> CoolantInPorts;
    public ImmutableArray<char> CoolantOutPorts;
    public readonly DataCenterProto.Gfx Graphics;

    public override Type EntityType => typeof (DataCenter);

    public DataCenterProto(
      DataCenterProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      int racksCapacity,
      ProductProto coolantIn,
      ProductProto coolantOut,
      Quantity coolantCapacity,
      ImmutableArray<char> coolantInPorts,
      ImmutableArray<char> coolantOutPorts,
      DataCenterProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityProto.ID id1 = (StaticEntityProto.ID) id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      EntityCosts costs1 = costs;
      DataCenterProto.Gfx graphics1 = graphics;
      IEnumerable<Tag> tags1 = tags;
      Duration? constructionDurationPerProduct = new Duration?();
      Upoints? boostCost = new Upoints?();
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, costs1, (LayoutEntityProto.Gfx) graphics1, constructionDurationPerProduct, boostCost, tags: tags2);
      this.RacksCapacity = racksCapacity.CheckPositive();
      this.CoolantIn = coolantIn;
      this.CoolantOut = coolantOut;
      this.CoolantInPorts = coolantInPorts;
      this.CoolantOutPorts = coolantOutPorts;
      this.CoolantCapacity = coolantCapacity;
      this.Graphics = graphics;
    }

    public DataCenterProto.ID Id => new DataCenterProto.ID(base.Id.Value);

    public new class Gfx : LayoutEntityProto.Gfx
    {
      /// <summary>Positions of racks in the datacenter.</summary>
      public readonly ImmutableArray<DataCenterProto.RackPosition> RackPositions;
      public static readonly DataCenterProto.Gfx Empty;

      public Gfx(
        string prefabPath,
        RelTile3f prefabOffset,
        Option<string> customIconPath,
        ImmutableArray<DataCenterProto.RackPosition> rackPositions,
        ImmutableArray<ToolbarCategoryProto> categories)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, prefabOffset, customIconPath, ColorRgba.Empty, visualizedLayers: new LayoutEntityProto.VisualizedLayers?(LayoutEntityProto.VisualizedLayers.Empty), categories: new ImmutableArray<ToolbarCategoryProto>?(categories));
        this.RackPositions = rackPositions.CheckNotEmpty<DataCenterProto.RackPosition>();
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        DataCenterProto.Gfx.Empty = new DataCenterProto.Gfx("EMPTY", RelTile3f.Zero, (Option<string>) Option.None, ImmutableArray.Create<DataCenterProto.RackPosition>(new DataCenterProto.RackPosition[1]
        {
          DataCenterProto.RackPosition.Empty
        }), (ImmutableArray<ToolbarCategoryProto>) ImmutableArray.Empty);
      }
    }

    /// <summary>
    /// Represents a position and rotation of a rack in a datacenter. The position and rotation are relative to the
    /// datacenter.
    /// </summary>
    public struct RackPosition
    {
      public static readonly DataCenterProto.RackPosition Empty;
      public readonly RelTile2f Position;
      public readonly Rotation90 Rotation;

      public RackPosition(RelTile2f position, Rotation90 rotation)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Position = position;
        this.Rotation = rotation;
      }

      static RackPosition() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
    }

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : IEquatable<DataCenterProto.ID>, IComparable<DataCenterProto.ID>
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
      public static implicit operator StaticEntityProto.ID(DataCenterProto.ID id)
      {
        return new StaticEntityProto.ID(id.Value);
      }

      public static bool operator ==(StaticEntityProto.ID lhs, DataCenterProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(DataCenterProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(StaticEntityProto.ID lhs, DataCenterProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(DataCenterProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Entities.EntityProto.ID" />.
      /// </summary>
      public static implicit operator EntityProto.ID(DataCenterProto.ID id)
      {
        return new EntityProto.ID(id.Value);
      }

      public static bool operator ==(EntityProto.ID lhs, DataCenterProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(DataCenterProto.ID lhs, EntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityProto.ID lhs, DataCenterProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(DataCenterProto.ID lhs, EntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(DataCenterProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, DataCenterProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(DataCenterProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, DataCenterProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(DataCenterProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(DataCenterProto.ID lhs, DataCenterProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(DataCenterProto.ID lhs, DataCenterProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is DataCenterProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(DataCenterProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(DataCenterProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(DataCenterProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static DataCenterProto.ID Deserialize(BlobReader reader)
      {
        return new DataCenterProto.ID(reader.ReadString());
      }
    }
  }
}
