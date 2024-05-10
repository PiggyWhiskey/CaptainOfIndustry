// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ThermalStorages.ThermalStoragePortProductResolver
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings.ThermalStorages
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ThermalStoragePortProductResolver : PortProductResolverBase<ThermalStorage>
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly Lyst<ProductProto> m_cache;

    public ThermalStoragePortProductResolver(ProtosDb protosDb, UnlockedProtosDb unlockedProtosDb)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_cache = new Lyst<ProductProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
      this.m_unlockedProtosDb = unlockedProtosDb;
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      ThermalStorage entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      return this.getPortProduct(entity.Prototype, port.Spec, entity.AssignedProduct);
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      return this.getPortProduct((ThermalStorageProto) proto, portSpec, new ThermalStorageProto.ProductData?());
    }

    private ImmutableArray<ProductProto> getPortProduct(
      ThermalStorageProto p,
      PortSpec portSpec,
      ThermalStorageProto.ProductData? data)
    {
      if (portSpec.Type == IoPortType.Input)
      {
        if ((int) portSpec.Name == (int) p.ProductToChargePort)
          return ImmutableArray.Create<ProductProto>(p.ProductToCharge);
        return data.HasValue ? ImmutableArray.Create<ProductProto>(data.Value.Product) : getAllSupportedProducts();
      }
      if (portSpec.Type == IoPortType.Output)
      {
        if ((int) portSpec.Name == (int) p.DepletedProductPort)
          return ImmutableArray.Create<ProductProto>(p.DepletedProduct);
        return data.HasValue ? ImmutableArray.Create<ProductProto>(data.Value.Product) : getAllSupportedProducts();
      }
      Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
      return ImmutableArray<ProductProto>.Empty;

      ImmutableArray<ProductProto> getAllSupportedProducts()
      {
        this.m_cache.Clear();
        foreach (ThermalStorageProto.ProductData supportedProduct in p.SupportedProducts)
        {
          if (this.m_unlockedProtosDb.IsUnlocked((Proto) supportedProduct.Product))
            this.m_cache.Add(supportedProduct.Product);
        }
        return this.m_cache.ToImmutableArray();
      }
    }
  }
}
