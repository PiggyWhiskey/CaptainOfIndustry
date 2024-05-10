// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotificationId
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
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct MessageNotificationId : 
    IEquatable<MessageNotificationId>,
    IComparable<MessageNotificationId>
  {
    /// <summary>Underlying int value of this Id.</summary>
    public readonly int Value;
    public static MessageNotificationId Invalid;

    public MessageNotificationId(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Mafi.Assert.That<int>(value).IsNotNegative("Invalid MessageNotificationId value");
      this.Value = value;
    }

    public static bool operator ==(MessageNotificationId lhs, MessageNotificationId rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(MessageNotificationId lhs, MessageNotificationId rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public override bool Equals(object other)
    {
      return other is MessageNotificationId other1 && this.Equals(other1);
    }

    public bool Equals(MessageNotificationId other) => this.Value.Equals(other.Value);

    public int CompareTo(MessageNotificationId other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value;

    public static void Serialize(MessageNotificationId value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.Value);
    }

    public static MessageNotificationId Deserialize(BlobReader reader)
    {
      return new MessageNotificationId(reader.ReadIntNotNegative());
    }

    public bool IsValid => this.Value > 0;

    static MessageNotificationId() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [GenerateSerializer(false, null, 0)]
    public class Factory : IdFactoryBase<MessageNotificationId>
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      protected override MessageNotificationId GetNextIdInternal(MessageNotificationId lastUsed)
      {
        return new MessageNotificationId(lastUsed.Value + 1);
      }

      public static void Serialize(MessageNotificationId.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<MessageNotificationId.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, MessageNotificationId.Factory.s_serializeDataDelayedAction);
      }

      public static MessageNotificationId.Factory Deserialize(BlobReader reader)
      {
        MessageNotificationId.Factory factory;
        if (reader.TryStartClassDeserialization<MessageNotificationId.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, MessageNotificationId.Factory.s_deserializeDataDelayedAction);
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
        MessageNotificationId.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IdFactoryBase<MessageNotificationId>) obj).SerializeData(writer));
        MessageNotificationId.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IdFactoryBase<MessageNotificationId>) obj).DeserializeData(reader));
      }
    }
  }
}
