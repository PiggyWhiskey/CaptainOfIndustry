// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadDirectionCanonical
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;
using System;

#nullable disable
namespace Mafi.Core.Roads
{
  /// <summary>
  /// Canonical direction. This will convert all vectors to signs and all XY vectors will have Y component non-negative
  /// (and [-1, 0] is banned, it will be converted to [1, 0]). This represents an unique non-directed line in
  /// 45 deg increments.
  /// </summary>
  [ExpectedStructSize(1)]
  public readonly struct RoadDirectionCanonical : IEquatable<RoadDirectionCanonical>
  {
    /// <summary>
    /// 0b00_0011 is set to 11 when X is -1, 01 for X=1 and 00 for X=0.
    /// 0b00_0100 is set when Y is non-zero, not set when it is zero.
    /// 0b11_0000 is set to 11 when Z is -1, 01 for Z=1 and 00 for Z=0.
    /// </summary>
    public readonly byte RawData;

    public RelTile3i DirectionSigns
    {
      get
      {
        return new RelTile3i(((int) this.RawData & 3) << 30 >> 30, ((int) this.RawData & 4) >> 2, ((int) this.RawData & 48) << 26 >> 30);
      }
    }

    public RoadDirectionCanonical(RelTile3i directionSigns)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      int num = directionSigns.X;
      if (directionSigns.Y < 0)
        num = -num;
      this.RawData = (byte) (num & 3 | (directionSigns.Y & 1) << 2 | (directionSigns.Z & 3) << 4);
      if (((int) this.RawData & 15) != 3)
        return;
      this.RawData &= (byte) 253;
    }

    public bool Equals(RoadDirectionCanonical other) => (int) this.RawData == (int) other.RawData;

    public override bool Equals(object obj)
    {
      return obj is RoadDirectionCanonical other && this.Equals(other);
    }

    public override int GetHashCode() => (int) this.RawData;
  }
}
