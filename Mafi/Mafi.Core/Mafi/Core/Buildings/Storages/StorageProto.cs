// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [DebuggerDisplay("StorageProto: {Id}")]
  public class StorageProto : 
    StorageBaseProto,
    IProtoWithUpgrade<StorageProto>,
    IProtoWithUpgrade,
    IProto
  {
    public readonly Electricity PowerConsumedForProductsExchange;
    /// <summary>
    /// Filter for <see cref="P:Mafi.Core.Buildings.Storages.StorageProto.StorableProducts" />.
    /// Do not filter product type, that is redundant work as it is already done for you via <see cref="P:Mafi.Core.Buildings.Storages.StorageProto.ProductType" />
    /// </summary>
    private readonly Func<ProductProto, bool> m_productsFilter;

    public override Type EntityType => typeof (Storage);

    /// <summary>
    /// Products that can be stored in the storage. Serves mainly for the players so they can choose from some list
    /// what to assign.
    /// </summary>
    public IReadOnlySet<ProductProto> StorableProducts { get; private set; }

    public UpgradeData<StorageProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    /// <summary>
    /// Type of product supported. If null, this supports all product types.
    /// However always check <see cref="M:Mafi.Core.Buildings.Storages.StorageProto.IsProductSupported(Mafi.Core.Products.ProductProto)" /> for addition filtering.
    /// </summary>
    public Mafi.Core.Products.ProductType? ProductType { get; }

    public StorageProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      Func<ProductProto, bool> productsFilter,
      Mafi.Core.Products.ProductType? productType,
      Quantity capacity,
      EntityCosts costs,
      Quantity transferLimit,
      Duration transferLimitDuration,
      Electricity powerConsumedForProductsExchange,
      Option<StorageProto> nextTier,
      LayoutEntityProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, capacity, costs, graphics, new Quantity?(transferLimit), new Duration?(transferLimitDuration), tags);
      this.Upgrade = new UpgradeData<StorageProto>(this, nextTier);
      this.ProductType = productType;
      this.PowerConsumedForProductsExchange = powerConsumedForProductsExchange;
      this.m_productsFilter = productsFilter.CheckNotNull<Func<ProductProto, bool>>();
    }

    public bool IsProductSupported(ProductProto product)
    {
      return (!this.ProductType.HasValue || this.ProductType.Value == product.Type) && this.m_productsFilter(product);
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      this.StorableProducts = (IReadOnlySet<ProductProto>) new Set<ProductProto>(protosDb.Filter<ProductProto>(new Func<ProductProto, bool>(this.IsProductSupported)));
    }
  }
}
