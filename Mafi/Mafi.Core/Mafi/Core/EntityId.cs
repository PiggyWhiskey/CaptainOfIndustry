// Decompiled with JetBrains decompiler
// Type: Mafi.Core.EntityId
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
namespace Mafi.Core
{
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("{Value,nq}")]
  [DebuggerStepThrough]
  public readonly struct EntityId : IEquatable<EntityId>, IComparable<EntityId>
  {
    public static EntityId Invalid;
    /// <summary>Underlying int value of this Id.</summary>
    public readonly int Value;

    public bool IsValid => this.Value > 0;

    public bool IsNotValid => this.Value <= 0;

    public EntityIdOption Some() => new EntityIdOption(this);

    public EntityId(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Mafi.Assert.That<int>(value).IsNotNegative("Invalid EntityId value");
      this.Value = value;
    }

    public static bool operator ==(EntityId lhs, EntityId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(EntityId lhs, EntityId rhs) => lhs.Value != rhs.Value;

    public override bool Equals(object other) => other is EntityId other1 && this.Equals(other1);

    public bool Equals(EntityId other) => this.Value.Equals(other.Value);

    public int CompareTo(EntityId other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value;

    public static void Serialize(EntityId value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.Value);
    }

    public static EntityId Deserialize(BlobReader reader)
    {
      return new EntityId(reader.ReadIntNotNegative());
    }

    static EntityId() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [GenerateSerializer(false, null, 0)]
    public class Factory : IdFactoryBase<EntityId>
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      protected override EntityId GetNextIdInternal(EntityId lastUsed)
      {
        return new EntityId(lastUsed.Value + 1);
      }

      public static void Serialize(EntityId.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<EntityId.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, EntityId.Factory.s_serializeDataDelayedAction);
      }

      public static EntityId.Factory Deserialize(BlobReader reader)
      {
        EntityId.Factory factory;
        if (reader.TryStartClassDeserialization<EntityId.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, EntityId.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      public Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        EntityId.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IdFactoryBase<EntityId>) obj).SerializeData(writer));
        EntityId.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IdFactoryBase<EntityId>) obj).DeserializeData(reader));
      }
    }
  }
}
