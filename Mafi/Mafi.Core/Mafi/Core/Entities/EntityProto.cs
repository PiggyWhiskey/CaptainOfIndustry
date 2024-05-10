// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Maintenance;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Entities
{
  public abstract class EntityProto : Proto, IEntityProto, IProto, IProtoWithPropertiesUpdate
  {
    private AssetValue? m_originalPrice;
    public readonly EntityProto.Gfx Graphics;

    public abstract Type EntityType { get; }

    /// <summary>
    /// Costs for the entity - price, maintenance, etc.
    /// 
    /// IMPORTANT: Do not cache the price as it can change during the runtime.
    /// </summary>
    public EntityCosts Costs { get; private set; }

    protected EntityProto(
      EntityProto.ID id,
      Proto.Str strings,
      EntityCosts costs,
      EntityProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings, tags);
      Mafi.Assert.That<Type>(this.EntityType).IsAssignableTo<IEntity>("Invalid entity type.");
      Mafi.Assert.That<bool>(this.Costs.Maintenance.MaintenancePerMonth.IsZero || this.EntityType.IsAssignableTo<IMaintainedEntity>()).IsTrue<string>("Non-maintained entity `{0}` has non-zero maintenance cost. Did you forget to implement `IMaintainedEntity`?", this.EntityType.Name);
      this.Costs = costs;
      this.Graphics = graphics ?? EntityProto.Gfx.Empty;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      protosDb.TrackProperty((IProtoWithPropertiesUpdate) this, IdsCore.PropertyIds.ConstructionCostsMultiplier.Value);
    }

    public virtual void OnPropertyUpdated(IProperty property)
    {
      Percent percent;
      if (!property.TryGetValueAs<Percent>(IdsCore.PropertyIds.ConstructionCostsMultiplier, out percent) || percent.IsNearHundred && !this.m_originalPrice.HasValue)
        return;
      this.m_originalPrice.GetValueOrDefault();
      if (!this.m_originalPrice.HasValue)
        this.m_originalPrice = new AssetValue?(this.Costs.Price);
      this.Costs = new EntityCosts(this.m_originalPrice.Value.ScaledBy(percent), this.Costs.DefaultPriority, this.Costs.Workers, new MaintenanceCosts?(this.Costs.Maintenance), this.Costs.IsQuickBuildDisabled);
    }

    public EntityProto.ID Id => new EntityProto.ID(base.Id.Value);

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : IEquatable<EntityProto.ID>, IComparable<EntityProto.ID>
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
      public static implicit operator Proto.ID(EntityProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, EntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(EntityProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, EntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(EntityProto.ID lhs, EntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityProto.ID lhs, EntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is EntityProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(EntityProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(EntityProto.ID other) => string.CompareOrdinal(this.Value, other.Value);

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(EntityProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static EntityProto.ID Deserialize(BlobReader reader)
      {
        return new EntityProto.ID(reader.ReadString());
      }
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly EntityProto.Gfx Empty;
      /// <summary>
      /// Optional color of this model to apply on colorizable materials.
      /// </summary>
      public readonly ColorRgba Color;
      /// <summary>
      /// This field is solely used by EntitiesRenderingManager and should not be touched.
      /// </summary>
      public int RendererIndex;

      protected Gfx(ColorRgba color)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Color = color;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        EntityProto.Gfx.Empty = new EntityProto.Gfx(ColorRgba.Empty);
      }
    }
  }
}
