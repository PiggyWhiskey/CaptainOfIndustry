// Decompiled with JetBrains decompiler
// Type: Mafi.Core.ThicknessIRange
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct ThicknessIRange : IEquatable<ThicknessIRange>
  {
    public static readonly ThicknessIRange Zero;
    public readonly ThicknessTilesI From;
    public readonly ThicknessTilesI To;

    public ThicknessTilesI Height => this.To - this.From;

    [LoadCtor]
    public ThicknessIRange(ThicknessTilesI from, ThicknessTilesI to)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.From = from;
      this.To = to;
    }

    public ThicknessIRange(int fromTiles, int toTiles)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.From = new ThicknessTilesI(fromTiles);
      this.To = new ThicknessTilesI(toTiles);
    }

    /// <summary>
    /// Intersects this and the given range by computing max from From, and min from To. If ranges
    /// are not overlapping this will return a range with negative height.
    /// </summary>
    public ThicknessIRange Intersect(ThicknessIRange range)
    {
      return new ThicknessIRange(this.From.Max(range.From), this.To.Min(range.To));
    }

    public ThicknessTilesI ClampToRange(ThicknessTilesI value)
    {
      if (value < this.From)
        return this.From;
      return !(value > this.To) ? value : this.To;
    }

    public ThicknessIRange ShiftBy(ThicknessTilesI delta)
    {
      return new ThicknessIRange(this.From + delta, this.To + delta);
    }

    public override string ToString()
    {
      return string.Format("{0}-{1}", (object) this.From, (object) this.To);
    }

    public static bool operator ==(ThicknessIRange lhs, ThicknessIRange rhs)
    {
      return lhs.From == rhs.From && lhs.To == rhs.To;
    }

    public static bool operator !=(ThicknessIRange lhs, ThicknessIRange rhs)
    {
      return lhs.From != rhs.From || lhs.To != rhs.To;
    }

    public bool Equals(ThicknessIRange other) => this == other;

    public override bool Equals(object obj) => obj is ThicknessIRange other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<ThicknessTilesI, ThicknessTilesI>(this.From, this.To);
    }

    public static void Serialize(ThicknessIRange value, BlobWriter writer)
    {
      ThicknessTilesI.Serialize(value.From, writer);
      ThicknessTilesI.Serialize(value.To, writer);
    }

    public static ThicknessIRange Deserialize(BlobReader reader)
    {
      return new ThicknessIRange(ThicknessTilesI.Deserialize(reader), ThicknessTilesI.Deserialize(reader));
    }

    static ThicknessIRange() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
  }
}
