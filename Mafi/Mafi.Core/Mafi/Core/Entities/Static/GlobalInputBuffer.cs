// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.GlobalInputBuffer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Accepts products from quick remove but does not provide them.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class GlobalInputBuffer : ProductBuffer
  {
    private readonly ProductStats m_productStats;
    private readonly bool m_autoUpdateCapacity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GlobalInputBuffer(
      Quantity capacity,
      ProductProto product,
      IProductsManager productsManager,
      int priority,
      IEntity entity = null,
      bool disableCapacityAutoScale = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(capacity, product);
      this.m_autoUpdateCapacity = !disableCapacityAutoScale;
      this.m_productStats = productsManager.GetStatsFor(product);
      this.m_productStats.ProductsManager.AssetManager.AddGlobalInput((IProductBuffer) this, priority, entity.CreateOption<IEntity>());
    }

    protected override void OnQuantityChanged(Quantity diff)
    {
      this.m_productStats.StoredUnavailableQuantityChange(diff, this.m_autoUpdateCapacity);
    }

    public override void Destroy()
    {
      Assert.That<Quantity>(this.Quantity).IsZero("Buffer was not cleared before destroy!");
      this.m_productStats.ProductsManager.AssetManager.RemoveGlobalInput((IProductBuffer) this);
      base.Destroy();
    }

    public static void Serialize(GlobalInputBuffer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GlobalInputBuffer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GlobalInputBuffer.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.m_autoUpdateCapacity);
      ProductStats.Serialize(this.m_productStats, writer);
    }

    public static GlobalInputBuffer Deserialize(BlobReader reader)
    {
      GlobalInputBuffer globalInputBuffer;
      if (reader.TryStartClassDeserialization<GlobalInputBuffer>(out globalInputBuffer))
        reader.EnqueueDataDeserialization((object) globalInputBuffer, GlobalInputBuffer.s_deserializeDataDelayedAction);
      return globalInputBuffer;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GlobalInputBuffer>(this, "m_autoUpdateCapacity", (object) reader.ReadBool());
      reader.SetField<GlobalInputBuffer>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
    }

    static GlobalInputBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GlobalInputBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
      GlobalInputBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
    }
  }
}
