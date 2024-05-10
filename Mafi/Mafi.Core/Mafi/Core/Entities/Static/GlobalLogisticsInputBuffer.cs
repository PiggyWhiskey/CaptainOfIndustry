// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.GlobalLogisticsInputBuffer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Storages;
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
  public class GlobalLogisticsInputBuffer : LogisticsBuffer
  {
    private readonly ProductStats m_productStats;
    private readonly bool m_autoUpdateCapacity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GlobalLogisticsInputBuffer(
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
      productsManager.AssetManager.AddGlobalInput((IProductBuffer) this, priority, entity.CreateOption<IEntity>());
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

    public static void Serialize(GlobalLogisticsInputBuffer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GlobalLogisticsInputBuffer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GlobalLogisticsInputBuffer.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.m_autoUpdateCapacity);
      ProductStats.Serialize(this.m_productStats, writer);
    }

    public static GlobalLogisticsInputBuffer Deserialize(BlobReader reader)
    {
      GlobalLogisticsInputBuffer logisticsInputBuffer;
      if (reader.TryStartClassDeserialization<GlobalLogisticsInputBuffer>(out logisticsInputBuffer))
        reader.EnqueueDataDeserialization((object) logisticsInputBuffer, GlobalLogisticsInputBuffer.s_deserializeDataDelayedAction);
      return logisticsInputBuffer;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GlobalLogisticsInputBuffer>(this, "m_autoUpdateCapacity", (object) reader.ReadBool());
      reader.SetField<GlobalLogisticsInputBuffer>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
    }

    static GlobalLogisticsInputBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GlobalLogisticsInputBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
      GlobalLogisticsInputBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
    }
  }
}
