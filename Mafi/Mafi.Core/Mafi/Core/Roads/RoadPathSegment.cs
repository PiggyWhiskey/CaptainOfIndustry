// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadPathSegment
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Roads
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct RoadPathSegment : IEquatable<RoadPathSegment>
  {
    public readonly IRoadGraphEntity Entity;
    public readonly int LaneIndex;

    public RoadPathSegment(IRoadGraphEntity entity, int laneIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Entity = entity;
      this.LaneIndex = laneIndex;
    }

    public static bool operator ==(RoadPathSegment lhs, RoadPathSegment rhs)
    {
      return lhs.Entity == rhs.Entity && lhs.LaneIndex == rhs.LaneIndex;
    }

    public static bool operator !=(RoadPathSegment lhs, RoadPathSegment rhs)
    {
      return lhs.Entity == rhs.Entity && lhs.LaneIndex == rhs.LaneIndex;
    }

    public bool Equals(RoadPathSegment other) => this == other;

    public override bool Equals(object obj) => obj is RoadPathSegment other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<IRoadGraphEntity, int>(this.Entity, this.LaneIndex);
    }

    public override string ToString()
    {
      return string.Format("{0} #{1} lane {2}", (object) this.Entity.RoadProto.Id, (object) this.Entity.Id, (object) this.LaneIndex);
    }

    public static void Serialize(RoadPathSegment value, BlobWriter writer)
    {
      writer.WriteGeneric<IRoadGraphEntity>(value.Entity);
      writer.WriteInt(value.LaneIndex);
    }

    public static RoadPathSegment Deserialize(BlobReader reader)
    {
      return new RoadPathSegment(reader.ReadGenericAs<IRoadGraphEntity>(), reader.ReadInt());
    }
  }
}
