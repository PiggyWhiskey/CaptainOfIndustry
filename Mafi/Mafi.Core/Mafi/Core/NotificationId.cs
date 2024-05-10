// Decompiled with JetBrains decompiler
// Type: Mafi.Core.NotificationId
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
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("{Value,nq}")]
  public readonly struct NotificationId : IEquatable<NotificationId>, IComparable<NotificationId>
  {
    public static readonly NotificationId Invalid;
    /// <summary>Underlying uint value of this Id.</summary>
    public readonly uint Value;

    public bool IsValid => this.Value > 0U;

    public NotificationId(uint value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public static bool operator ==(NotificationId lhs, NotificationId rhs)
    {
      return (int) lhs.Value == (int) rhs.Value;
    }

    public static bool operator !=(NotificationId lhs, NotificationId rhs)
    {
      return (int) lhs.Value != (int) rhs.Value;
    }

    public override bool Equals(object other)
    {
      return other is NotificationId other1 && this.Equals(other1);
    }

    public bool Equals(NotificationId other) => this.Value.Equals(other.Value);

    public int CompareTo(NotificationId other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value.GetHashCode();

    public static void Serialize(NotificationId value, BlobWriter writer)
    {
      writer.WriteUInt(value.Value);
    }

    public static NotificationId Deserialize(BlobReader reader)
    {
      return new NotificationId(reader.ReadUInt());
    }

    static NotificationId() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
  }
}
