// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.GlobalOutputBuffer
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
  /// Provides products for quick build but does not accept them.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class GlobalOutputBuffer : ProductBuffer
  {
    private readonly ProductStats m_productStats;
    private readonly bool m_autoUpdateCapacity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GlobalOutputBuffer(
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
      productsManager.AssetManager.AddGlobalOutput((IProductBuffer) this, priority, entity.CreateOption<IEntity>());
    }

    protected override void OnQuantityChanged(Quantity diff)
    {
      this.m_productStats.StoredAvailableQuantityChange(diff, this.m_autoUpdateCapacity);
    }

    public override void Destroy()
    {
      Assert.That<Quantity>(this.Quantity).IsZero("Buffer was not cleared before destroy!");
      this.m_productStats.ProductsManager.AssetManager.RemoveGlobalOutput((IProductBuffer) this);
      base.Destroy();
    }

    public static void Serialize(GlobalOutputBuffer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GlobalOutputBuffer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GlobalOutputBuffer.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.m_autoUpdateCapacity);
      ProductStats.Serialize(this.m_productStats, writer);
    }

    public static GlobalOutputBuffer Deserialize(BlobReader reader)
    {
      GlobalOutputBuffer globalOutputBuffer;
      if (reader.TryStartClassDeserialization<GlobalOutputBuffer>(out globalOutputBuffer))
        reader.EnqueueDataDeserialization((object) globalOutputBuffer, GlobalOutputBuffer.s_deserializeDataDelayedAction);
      return globalOutputBuffer;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GlobalOutputBuffer>(this, "m_autoUpdateCapacity", (object) reader.ReadBool());
      reader.SetField<GlobalOutputBuffer>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
    }

    static GlobalOutputBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GlobalOutputBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
      GlobalOutputBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
    }
  }
}
