// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Beacons.RefugeesReward
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Beacons
{
  [GenerateSerializer(false, null, 0)]
  public class RefugeesReward
  {
    public readonly Duration Duration;
    public readonly int AmountOfRefugees;
    public readonly int MinimalTier;
    public readonly ImmutableArray<ImmutableArray<ProductQuantity>> PossibleRewards;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public RefugeesReward(
      Duration duration,
      int amountOfRefugees,
      int minimalTier,
      ImmutableArray<ImmutableArray<ProductQuantity>> possibleRewards)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Duration = duration;
      this.AmountOfRefugees = amountOfRefugees;
      this.MinimalTier = minimalTier;
      this.PossibleRewards = possibleRewards;
    }

    public static void Serialize(RefugeesReward value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RefugeesReward>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RefugeesReward.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.AmountOfRefugees);
      Duration.Serialize(this.Duration, writer);
      writer.WriteInt(this.MinimalTier);
      ImmutableArray<ImmutableArray<ProductQuantity>>.Serialize(this.PossibleRewards, writer);
    }

    public static RefugeesReward Deserialize(BlobReader reader)
    {
      RefugeesReward refugeesReward;
      if (reader.TryStartClassDeserialization<RefugeesReward>(out refugeesReward))
        reader.EnqueueDataDeserialization((object) refugeesReward, RefugeesReward.s_deserializeDataDelayedAction);
      return refugeesReward;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<RefugeesReward>(this, "AmountOfRefugees", (object) reader.ReadInt());
      reader.SetField<RefugeesReward>(this, "Duration", (object) Duration.Deserialize(reader));
      reader.SetField<RefugeesReward>(this, "MinimalTier", (object) reader.ReadInt());
      reader.SetField<RefugeesReward>(this, "PossibleRewards", (object) ImmutableArray<ImmutableArray<ProductQuantity>>.Deserialize(reader));
    }

    static RefugeesReward()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RefugeesReward.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RefugeesReward) obj).SerializeData(writer));
      RefugeesReward.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RefugeesReward) obj).DeserializeData(reader));
    }
  }
}
