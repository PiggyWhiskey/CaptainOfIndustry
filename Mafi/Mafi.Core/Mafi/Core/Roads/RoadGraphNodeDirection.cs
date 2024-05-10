// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadGraphNodeDirection
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
  [ExpectedStructSize(1)]
  public readonly struct RoadGraphNodeDirection : IEquatable<RoadGraphNodeDirection>
  {
    /// <summary>
    /// 0b00_0011 is set to 11 when X is -1, 01 for X=1 and 00 for X=0.
    /// 0b00_1100 as above but for Y.
    /// 0b11_0000 as above but for Z.
    /// </summary>
    public readonly byte RawData;

    public RelTile3i DirectionSigns
    {
      get
      {
        return new RelTile3i(((int) this.RawData & 3) << 30 >> 30, ((int) this.RawData & 12) << 28 >> 30, ((int) this.RawData & 48) << 26 >> 30);
      }
    }

    public RoadGraphNodeDirection(RelTile3i directionSigns)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawData = (byte) (directionSigns.X & 3 | (directionSigns.Y & 3) << 2 | (directionSigns.Z & 3) << 4);
    }

    public bool Equals(RoadGraphNodeDirection other) => (int) this.RawData == (int) other.RawData;

    public override bool Equals(object obj)
    {
      return obj is RoadGraphNodeDirection other && this.Equals(other);
    }

    public override int GetHashCode() => (int) this.RawData;

    public override string ToString() => this.DirectionSigns.ToString();
  }
}
