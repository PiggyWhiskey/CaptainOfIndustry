// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DynamicEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public abstract class DynamicEntityProto : EntityProto, IProtoWithIcon, IProto
  {
    public readonly DynamicEntityProto.Gfx Graphics;
    /// <summary>
    /// Amount of vehicle quota this vehicle costs. Can be zero for vehicles that do not cost any quota.
    /// </summary>
    public readonly int VehicleQuotaCost;

    public string IconPath => this.Graphics.IconPath;

    protected DynamicEntityProto(
      DynamicEntityProto.ID id,
      Proto.Str strings,
      DynamicEntityProto.Gfx graphics,
      EntityCosts entityCosts,
      int vehicleQuotaCost)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((EntityProto.ID) id, strings, entityCosts, (EntityProto.Gfx) graphics);
      this.VehicleQuotaCost = vehicleQuotaCost.CheckNotNegative();
      this.Graphics = graphics.CheckNotNull<DynamicEntityProto.Gfx>();
      this.Graphics.Initialize(this);
    }

    public DynamicEntityProto.ID Id => new DynamicEntityProto.ID(base.Id.Value);

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : 
      IEquatable<DynamicEntityProto.ID>,
      IComparable<DynamicEntityProto.ID>
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
      public static implicit operator EntityProto.ID(DynamicEntityProto.ID id)
      {
        return new EntityProto.ID(id.Value);
      }

      public static bool operator ==(EntityProto.ID lhs, DynamicEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(DynamicEntityProto.ID lhs, EntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityProto.ID lhs, DynamicEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(DynamicEntityProto.ID lhs, EntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(DynamicEntityProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, DynamicEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(DynamicEntityProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, DynamicEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(DynamicEntityProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(DynamicEntityProto.ID lhs, DynamicEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(DynamicEntityProto.ID lhs, DynamicEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is DynamicEntityProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(DynamicEntityProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(DynamicEntityProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(DynamicEntityProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static DynamicEntityProto.ID Deserialize(BlobReader reader)
      {
        return new DynamicEntityProto.ID(reader.ReadString());
      }
    }

    public new class Gfx : EntityProto.Gfx
    {
      /// <summary>
      /// Whether custom icon path was set. Otherwise, icon path is automatically generated.
      /// </summary>
      public readonly bool IconIsCustom;

      /// <summary>Path for icon sprite.</summary>
      /// <remarks>This path is valid only after <see cref="M:Mafi.Core.Entities.Dynamic.DynamicEntityProto.Gfx.Initialize(Mafi.Core.Entities.Dynamic.DynamicEntityProto)" /> was called.</remarks>
      public string IconPath { get; private set; }

      protected Gfx(Option<string> customIconPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty);
        this.IconPath = customIconPath.ValueOrNull;
        this.IconIsCustom = customIconPath.HasValue;
      }

      internal void Initialize(DynamicEntityProto proto)
      {
        Mafi.Assert.That<DynamicEntityProto.Gfx>(proto.Graphics).IsEqualTo<DynamicEntityProto.Gfx>(this);
        if (this.IconIsCustom)
          return;
        this.IconPath = string.Format("{0}/Vehicle/{1}.png", (object) "Assets/Unity/Generated/Icons", (object) proto.Id);
      }
    }
  }
}
