// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.VirtualBuffersMap
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class VirtualBuffersMap : IVirtualBuffersMap
  {
    public static readonly VirtualBuffersMap Empty;
    private readonly Dict<ProductProto, IVirtualBufferProvider> m_providers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public VirtualBuffersMap(
      AllImplementationsOf<IVirtualBufferProvider> providers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_providers = new Dict<ProductProto, IVirtualBufferProvider>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      foreach (IVirtualBufferProvider implementation in providers.Implementations)
      {
        foreach (ProductProto providedProduct in implementation.ProvidedProducts)
          this.m_providers.AddAndAssertNew(providedProduct, implementation, "Some other virtual buffer provider already registered for this product.");
      }
    }

    public Option<IProductBuffer> GetBuffer(ProductProto proto, IEntity entity)
    {
      IVirtualBufferProvider virtualBufferProvider;
      return this.m_providers.TryGetValue(proto, out virtualBufferProvider) ? virtualBufferProvider.GetBuffer(proto, Option<IEntity>.Create(entity)) : (Option<IProductBuffer>) Option.None;
    }

    public static void Serialize(VirtualBuffersMap value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VirtualBuffersMap>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VirtualBuffersMap.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<ProductProto, IVirtualBufferProvider>.Serialize(this.m_providers, writer);
    }

    public static VirtualBuffersMap Deserialize(BlobReader reader)
    {
      VirtualBuffersMap virtualBuffersMap;
      if (reader.TryStartClassDeserialization<VirtualBuffersMap>(out virtualBuffersMap))
        reader.EnqueueDataDeserialization((object) virtualBuffersMap, VirtualBuffersMap.s_deserializeDataDelayedAction);
      return virtualBuffersMap;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VirtualBuffersMap>(this, "m_providers", (object) Dict<ProductProto, IVirtualBufferProvider>.Deserialize(reader));
    }

    static VirtualBuffersMap()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VirtualBuffersMap.Empty = new VirtualBuffersMap(new AllImplementationsOf<IVirtualBufferProvider>(Array.Empty<IVirtualBufferProvider>()));
      VirtualBuffersMap.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VirtualBuffersMap) obj).SerializeData(writer));
      VirtualBuffersMap.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VirtualBuffersMap) obj).DeserializeData(reader));
    }
  }
}
