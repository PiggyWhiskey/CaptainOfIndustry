// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.NuclearWasteStoragePortProductResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Storages.NuclearWaste;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class NuclearWasteStoragePortProductResolver : 
    PortProductResolverBase<NuclearWasteStorage>
  {
    private readonly Dict<ProductProto, ImmutableArray<ProductProto>> m_arraysCache;

    public NuclearWasteStoragePortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_arraysCache = new Dict<ProductProto, ImmutableArray<ProductProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      NuclearWasteStorage entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      if (entity.StoredProduct.IsNone)
        return ImmutableArray<ProductProto>.Empty;
      ProductProto product = entity.StoredProduct.Value;
      if (port.Type == IoPortType.Output && entity.OutputBuffer.HasValue && !entity.DoNotSendRetiredWasteToOutputPort)
        product = entity.OutputBuffer.Value.Product;
      ImmutableArray<ProductProto> portProduct;
      if (!this.m_arraysCache.TryGetValue(product, out portProduct))
      {
        portProduct = ImmutableArray.Create<ProductProto>(product);
        this.m_arraysCache.Add(product, portProduct);
      }
      return portProduct;
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
