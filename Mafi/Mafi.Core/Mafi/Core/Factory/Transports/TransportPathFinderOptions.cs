// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPathFinderOptions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct TransportPathFinderOptions : IEquatable<TransportPathFinderOptions>
  {
    public readonly HeightTilesI? PreferredHeight;
    public readonly Direction903d? ForcedStartDirection;
    public readonly ImmutableArray<Direction903d> BannedStartDirections;
    public readonly TransportPathFinderFlags Flags;

    public TransportPathFinderOptions(
      HeightTilesI? preferredHeight = null,
      Direction903d? forcedStartDirection = null,
      ImmutableArray<Direction903d> bannedStartDirections = default (ImmutableArray<Direction903d>),
      TransportPathFinderFlags flags = TransportPathFinderFlags.None)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.PreferredHeight = preferredHeight;
      this.ForcedStartDirection = forcedStartDirection;
      this.BannedStartDirections = bannedStartDirections.IsValid ? bannedStartDirections : ImmutableArray<Direction903d>.Empty;
      this.Flags = flags;
    }

    public bool HasFlags(TransportPathFinderFlags flags) => (this.Flags & flags) != 0;

    public static bool operator ==(TransportPathFinderOptions lhs, TransportPathFinderOptions rhs)
    {
      return lhs.Equals(rhs);
    }

    public static bool operator !=(TransportPathFinderOptions lhs, TransportPathFinderOptions rhs)
    {
      return !(lhs == rhs);
    }

    public bool Equals(TransportPathFinderOptions other)
    {
      if (!Nullable.Equals<HeightTilesI>(this.PreferredHeight, other.PreferredHeight) || !Nullable.Equals<Direction903d>(this.ForcedStartDirection, other.ForcedStartDirection) || this.Flags != other.Flags)
        return false;
      if (this.BannedStartDirections.IsEmpty || other.BannedStartDirections.IsEmpty)
        return this.BannedStartDirections.IsEmpty && other.BannedStartDirections.IsEmpty;
      foreach (Direction903d bannedStartDirection in this.BannedStartDirections)
      {
        if (!other.BannedStartDirections.Contains(bannedStartDirection))
          return false;
      }
      foreach (Direction903d bannedStartDirection in other.BannedStartDirections)
      {
        if (!this.BannedStartDirections.Contains(bannedStartDirection))
          return false;
      }
      return true;
    }

    public override bool Equals(object obj)
    {
      return obj is TransportPathFinderOptions other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return Hash.Combine<HeightTilesI?, Direction903d?, ImmutableArray<Direction903d>, TransportPathFinderFlags>(this.PreferredHeight, this.ForcedStartDirection, this.BannedStartDirections, this.Flags);
    }
  }
}
