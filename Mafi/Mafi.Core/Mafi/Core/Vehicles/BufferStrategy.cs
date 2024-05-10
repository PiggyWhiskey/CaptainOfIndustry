// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.BufferStrategy
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public struct BufferStrategy
  {
    public static readonly BufferStrategy Ignore;
    public readonly int Priority;
    public readonly int PriorityForRefueling;
    /// <summary>
    /// Optimal quantity is used by entities to inform logistics on what quantities are considered optimal when
    /// delivered / removed to / from them. For instance in case of a machine that takes 8 quantity per recipe
    /// in does not make sense to rush to give it 1 quantity. The same example is when finishing construction,
    /// if there is 5 parts left, 4 will not cut it anyway.
    /// 
    /// Optimal quantity can be even null which is equivalent of MaxQuantity. By this the entity state that it
    /// does not care about specific quantity. This is for storages. Their priority is not to be full or empty,
    /// their priority is to make logistics run smoothly. This would obviously backfire for entities
    /// that need all the products to finish, like for instance construction.
    /// 
    /// When pairing two buffers we apply the following rules:
    /// 1) Take minimal from both optimals =&gt; at least on buffer's optimal should be satisfied
    /// 2) If optimal is not set it gets set to MaxQuantity
    /// 3) Clamp optimal with max truck capacity (this means entities don't need to worry with overshooting truck cap)
    /// 4) IMPORTANT: Balance iff quantity to exchange is at least as big as optimal
    /// =&gt; So if there is less to exchange then is optimal, balancing won't happen.
    /// 
    /// This has several nice implications.
    /// 1) Entities don't need to care much about truck sizes. They can help logistics by using nice sizes of buffers
    /// and optimal quantities but it will not affect their correctness.
    /// 2) Balancing is not performed unless it provides value for at least one party or it utilizes full truck
    /// 3) Priority implementors don't need to decide their prios / optimal quantities based on available quantities
    ///    as this is decided for them.
    /// 
    /// There is one tricky case that could go wrong:
    /// If we return optimal quantity greater than what we can exchange we might never be able to finish. Or in fact
    /// we are at mercy of other buffer to hopefully have optimal quantity close to what we can exchange. That is why
    /// optimal = null is not recommended for things that need exact amount of products to finish.
    /// 
    /// There is a cheat to force things to always happen which is Optimal = 1. We use it in very special cases like
    /// deconstruction where if we wouldn't do it, things would end up in shipyard. We also use it when clearing
    /// storage but it might not be that necessary.
    /// 
    /// </summary>
    public readonly Quantity? OptimalQuantity;

    public static void Serialize(BufferStrategy value, BlobWriter writer)
    {
      writer.WriteInt(value.Priority);
      writer.WriteInt(value.PriorityForRefueling);
      writer.WriteNullableStruct<Quantity>(value.OptimalQuantity);
    }

    public static BufferStrategy Deserialize(BlobReader reader)
    {
      return new BufferStrategy(reader.ReadInt(), reader.ReadInt(), reader.ReadNullableStruct<Quantity>());
    }

    public static BufferStrategy FullFillAtAnyCost(int priority)
    {
      return new BufferStrategy(priority, new Quantity?(Quantity.One));
    }

    public static BufferStrategy NoQuantityPreference(int priority)
    {
      return new BufferStrategy(priority, new Quantity?());
    }

    /// <summary>
    /// Strategy where the entity is fine with logistics trying to utilize full trucks.
    /// 
    /// Strongly not recommended for situation where entity needs to get / remove all the products.
    /// </summary>
    public static BufferStrategy LowestNoQuantityPreference()
    {
      return new BufferStrategy(15, new Quantity?());
    }

    public BufferStrategy(int priority, Quantity? optimalQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Priority = priority;
      this.PriorityForRefueling = 15;
      this.OptimalQuantity = optimalQuantity;
    }

    [LoadCtor]
    public BufferStrategy(int priority, int priorityForRefueling, Quantity? optimalQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Priority = priority;
      this.PriorityForRefueling = priorityForRefueling;
      this.OptimalQuantity = optimalQuantity;
    }

    static BufferStrategy()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BufferStrategy.Ignore = new BufferStrategy(16, new Quantity?(Quantity.Zero));
    }
  }
}
