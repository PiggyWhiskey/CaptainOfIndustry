// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyId`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  [DebuggerDisplay("{Value,nq}")]
  public readonly struct PropertyId<T> : IEquatable<PropertyId<T>>, IComparable<PropertyId<T>>
  {
    /// <summary>Underlying string value of this Id.</summary>
    public readonly string Value;

    public PropertyId(string value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public static bool operator ==(PropertyId<T> lhs, PropertyId<T> rhs)
    {
      return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
    }

    public static bool operator !=(PropertyId<T> lhs, PropertyId<T> rhs)
    {
      return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
    }

    public override bool Equals(object other)
    {
      return other is PropertyId<T> other1 && this.Equals(other1);
    }

    public bool Equals(PropertyId<T> other)
    {
      return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
    }

    public int CompareTo(PropertyId<T> other) => string.CompareOrdinal(this.Value, other.Value);

    public override string ToString() => this.Value ?? string.Empty;

    public override int GetHashCode()
    {
      string str = this.Value;
      return str == null ? 0 : str.GetHashCode();
    }

    public static void Serialize(PropertyId<T> value, BlobWriter writer)
    {
      writer.WriteString(value.Value);
    }

    public static PropertyId<T> Deserialize(BlobReader reader)
    {
      return new PropertyId<T>(reader.ReadString());
    }
  }
}
