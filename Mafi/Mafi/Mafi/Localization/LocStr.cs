// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocStr
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
  /// Localized string without any arguments. Serializable. Can be implicitly converted to <see cref="T:Mafi.Localization.LocStrFormatted" />.
  /// </summary>
  [ManuallyWrittenSerialization]
  public readonly struct LocStr : IEquatable<LocStr>
  {
    public static LocStr Empty;
    public readonly string Id;
    public readonly string TranslatedString;

    internal LocStr(string id, string translatedString)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Id = id;
      this.TranslatedString = translatedString;
    }

    /// <summary>
    /// Implicit conversion to LocStrFmt. This is very cheap operation without any allocations.
    /// </summary>
    public LocStrFormatted AsFormatted => new LocStrFormatted(this.TranslatedString);

    /// <summary>
    /// Implicit conversion to LocStrFmt. This is very cheap operation without any allocations.
    /// </summary>
    public static implicit operator LocStrFormatted(LocStr locStr)
    {
      return new LocStrFormatted(locStr.TranslatedString);
    }

    public override string ToString() => this.TranslatedString;

    public static bool operator ==(LocStr lhs, LocStr rhs) => lhs.Id == rhs.Id;

    public static bool operator !=(LocStr lhs, LocStr rhs) => lhs.Id != rhs.Id;

    public bool Equals(LocStr other) => this == other;

    public override bool Equals(object obj) => obj is LocStr other && this.Equals(other);

    public override int GetHashCode()
    {
      string id = this.Id;
      return id == null ? 0 : id.GetHashCode();
    }

    public static void Serialize(LocStr value, BlobWriter writer) => writer.WriteString(value.Id);

    public static LocStr Deserialize(BlobReader reader)
    {
      return LocalizationManager.LoadLocalizedString0(reader.ReadString());
    }

    static LocStr()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocStr.Empty = Loc.Str("EmptyString", "", "HIDE (this must remain empty)");
    }
  }
}
