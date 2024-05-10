// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocStrFormatted
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Localization
{
  /// <summary>
  /// Formatted localized string. This is a convenience wrapper telling you that this string may not be English
  /// and many string mutation operations should not be performed.
  /// 
  /// This should not be saved, since change of language or fix in translation won't propagate here.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct LocStrFormatted : IEquatable<LocStrFormatted>
  {
    public static LocStrFormatted Empty;
    public readonly string Value;

    public static void Serialize(LocStrFormatted value, BlobWriter writer)
    {
      writer.WriteString(value.Value);
    }

    public static LocStrFormatted Deserialize(BlobReader reader)
    {
      return new LocStrFormatted(reader.ReadString());
    }

    public bool IsEmptyOrNull => string.IsNullOrEmpty(this.Value);

    public bool IsNotEmpty => !string.IsNullOrEmpty(this.Value);

    public LocStrFormatted(string value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Value = value;
    }

    public override string ToString() => this.Value;

    public static bool operator ==(LocStrFormatted lhs, LocStrFormatted rhs)
    {
      return lhs.Value == rhs.Value;
    }

    public static bool operator !=(LocStrFormatted lhs, LocStrFormatted rhs)
    {
      return lhs.Value != rhs.Value;
    }

    public bool Equals(LocStrFormatted other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is LocStrFormatted other && this.Equals(other);

    public override int GetHashCode()
    {
      string str = this.Value;
      return str == null ? 0 : str.GetHashCode();
    }

    static LocStrFormatted()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocStrFormatted.Empty = new LocStrFormatted("");
    }
  }
}
