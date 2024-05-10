// Decompiled with JetBrains decompiler
// Type: Mafi.Core.EntityIdOption
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core
{
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("{Value,nq}")]
  [DebuggerStepThrough]
  public readonly struct EntityIdOption : IEquatable<EntityIdOption>, IComparable<EntityIdOption>
  {
    /// <summary>Underlying EntityId value of this Id.</summary>
    public readonly EntityId Value;

    public static EntityIdOption None => new EntityIdOption();

    public bool HasValue => this.Value.IsValid;

    public EntityId? ToNullable
    {
      get => !this.Value.IsValid ? new EntityId?() : new EntityId?(this.Value);
    }

    public EntityIdOption(EntityId value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public static bool operator ==(EntityIdOption lhs, EntityIdOption rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(EntityIdOption lhs, EntityIdOption rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public override bool Equals(object other)
    {
      return other is EntityIdOption other1 && this.Equals(other1);
    }

    public bool Equals(EntityIdOption other) => this.Value.Equals(other.Value);

    public int CompareTo(EntityIdOption other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value.GetHashCode();

    public static void Serialize(EntityIdOption value, BlobWriter writer)
    {
      EntityId.Serialize(value.Value, writer);
    }

    public static EntityIdOption Deserialize(BlobReader reader)
    {
      return new EntityIdOption(EntityId.Deserialize(reader));
    }
  }
}
