// Decompiled with JetBrains decompiler
// Type: Mafi.Core.IoPortId
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
  public readonly struct IoPortId : IEquatable<IoPortId>, IComparable<IoPortId>
  {
    /// <summary>Underlying int value of this Id.</summary>
    public readonly int Value;
    public static IoPortId Invalid;

    public IoPortId(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Mafi.Assert.That<int>(value).IsNotNegative("Invalid IoPortId value");
      this.Value = value;
    }

    public static bool operator ==(IoPortId lhs, IoPortId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(IoPortId lhs, IoPortId rhs) => lhs.Value != rhs.Value;

    public override bool Equals(object other) => other is IoPortId other1 && this.Equals(other1);

    public bool Equals(IoPortId other) => this.Value.Equals(other.Value);

    public int CompareTo(IoPortId other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value;

    public static void Serialize(IoPortId value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.Value);
    }

    public static IoPortId Deserialize(BlobReader reader)
    {
      return new IoPortId(reader.ReadIntNotNegative());
    }

    public bool IsValid => this.Value > 0;

    static IoPortId() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [GenerateSerializer(false, null, 0)]
    public class Factory : IdFactoryBase<IoPortId>
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      protected override IoPortId GetNextIdInternal(IoPortId lastUsed)
      {
        return new IoPortId(lastUsed.Value + 1);
      }

      public static void Serialize(IoPortId.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<IoPortId.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, IoPortId.Factory.s_serializeDataDelayedAction);
      }

      public static IoPortId.Factory Deserialize(BlobReader reader)
      {
        IoPortId.Factory factory;
        if (reader.TryStartClassDeserialization<IoPortId.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, IoPortId.Factory.s_deserializeDataDelayedAction);
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
        IoPortId.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IdFactoryBase<IoPortId>) obj).SerializeData(writer));
        IoPortId.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IdFactoryBase<IoPortId>) obj).DeserializeData(reader));
      }
    }
  }
}
