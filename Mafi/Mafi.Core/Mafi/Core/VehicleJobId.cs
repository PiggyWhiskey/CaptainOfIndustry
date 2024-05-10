// Decompiled with JetBrains decompiler
// Type: Mafi.Core.VehicleJobId
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
  [DebuggerDisplay("{Value,nq}")]
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  public readonly struct VehicleJobId : IEquatable<VehicleJobId>, IComparable<VehicleJobId>
  {
    /// <summary>Underlying int value of this Id.</summary>
    public readonly int Value;
    public static VehicleJobId Invalid;

    public VehicleJobId(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Mafi.Assert.That<int>(value).IsNotNegative("Invalid VehicleJobId value");
      this.Value = value;
    }

    public static bool operator ==(VehicleJobId lhs, VehicleJobId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(VehicleJobId lhs, VehicleJobId rhs) => lhs.Value != rhs.Value;

    public override bool Equals(object other)
    {
      return other is VehicleJobId other1 && this.Equals(other1);
    }

    public bool Equals(VehicleJobId other) => this.Value.Equals(other.Value);

    public int CompareTo(VehicleJobId other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value;

    public static void Serialize(VehicleJobId value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.Value);
    }

    public static VehicleJobId Deserialize(BlobReader reader)
    {
      return new VehicleJobId(reader.ReadIntNotNegative());
    }

    public bool IsValid => this.Value > 0;

    static VehicleJobId() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [GenerateSerializer(false, null, 0)]
    public class Factory : IdFactoryBase<VehicleJobId>
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      protected override VehicleJobId GetNextIdInternal(VehicleJobId lastUsed)
      {
        return new VehicleJobId(lastUsed.Value + 1);
      }

      public static void Serialize(VehicleJobId.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<VehicleJobId.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, VehicleJobId.Factory.s_serializeDataDelayedAction);
      }

      public static VehicleJobId.Factory Deserialize(BlobReader reader)
      {
        VehicleJobId.Factory factory;
        if (reader.TryStartClassDeserialization<VehicleJobId.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, VehicleJobId.Factory.s_deserializeDataDelayedAction);
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
        VehicleJobId.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IdFactoryBase<VehicleJobId>) obj).SerializeData(writer));
        VehicleJobId.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IdFactoryBase<VehicleJobId>) obj).DeserializeData(reader));
      }
    }
  }
}
