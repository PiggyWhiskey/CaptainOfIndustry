// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadConnectionDirection
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Mafi.Core.Roads
{
  /// <summary>A key that is used to match connecting roads.</summary>
  [ExpectedStructSize(8)]
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct RoadConnectionDirection : IEquatable<RoadConnectionDirection>
  {
    [FieldOffset(0)]
    public readonly Tile3iSlim Position;
    [FieldOffset(6)]
    public readonly RoadDirectionCanonical Direction;
    [FieldOffset(7)]
    public readonly RoadConnectionType Type;
    [FieldOffset(0)]
    private readonly ulong RawData;

    public RoadConnectionDirection(
      Tile3iSlim position,
      RoadDirectionCanonical direction,
      RoadConnectionType type)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawData = 0UL;
      this.Position = position;
      this.Direction = direction;
      this.Type = type;
    }

    public bool Equals(RoadConnectionDirection other)
    {
      return (long) this.RawData == (long) other.RawData;
    }

    public override bool Equals(object obj)
    {
      return obj is RoadConnectionDirection other && this.Equals(other);
    }

    public override int GetHashCode() => this.RawData.GetHashCode();
  }
}
