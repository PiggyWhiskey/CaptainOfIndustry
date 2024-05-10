// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.EntityCostsTpl
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Maintenance;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public class EntityCostsTpl
  {
    private readonly ImmutableArray<KeyValuePair<ProductProto.ID, Quantity>> m_productQuantities;
    private readonly int m_workers;
    private readonly int m_defaultPriority;
    private readonly EntityCostsTpl.MaintenanceCostsTpl m_maintenance;

    public EntityCostsTpl(
      Lyst<KeyValuePair<ProductProto.ID, Quantity>> productQuantities,
      int workers,
      int defaultPriority,
      EntityCostsTpl.MaintenanceCostsTpl maintenance)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_productQuantities = productQuantities.ToImmutableArray();
      this.m_workers = workers;
      this.m_defaultPriority = defaultPriority;
      this.m_maintenance = maintenance;
    }

    public EntityCosts MapToEntityCosts(ProtoRegistrator registrator, bool isQuickBuildDisabled = false)
    {
      ProtosDb db = registrator.PrototypesDb;
      AssetValue price = new AssetValue(new SmallImmutableArray<ProductQuantity>(this.m_productQuantities.Select<ProductQuantity>((Func<KeyValuePair<ProductProto.ID, Quantity>, ProductQuantity>) (x => new ProductQuantity(db.GetOrThrow<ProductProto>((Proto.ID) x.Key), x.Value))).ToImmutableArray<ProductQuantity>()));
      int workers1 = this.m_workers;
      MaintenanceCosts? nullable = new MaintenanceCosts?(this.m_maintenance.Product.HasValue ? new MaintenanceCosts(db.GetOrThrow<VirtualProductProto>((Proto.ID) this.m_maintenance.Product.Value), this.m_maintenance.Quantity, this.m_maintenance.InitialMaintenanceBoost) : new MaintenanceCosts(VirtualProductProto.Phantom, Quantity.Zero.AsPartial));
      int defaultPriority = this.m_defaultPriority;
      int workers2 = workers1;
      MaintenanceCosts? maintenance = nullable;
      int num = isQuickBuildDisabled ? 1 : 0;
      return new EntityCosts(price, defaultPriority, workers2, maintenance, num != 0);
    }

    public class Builder
    {
      private readonly Lyst<KeyValuePair<ProductProto.ID, Quantity>> m_productQuantities;
      private int m_workers;
      private int m_defaultPriority;
      private ProductProto.ID? m_maintenanceProduct;
      private PartialQuantity m_maintenance;
      private Percent? m_initialMaintenanceBoost;

      [MustUseReturnValue]
      public EntityCostsTpl.Builder Product(int quantity, ProductProto.ID productId)
      {
        Assert.That<bool>(this.m_productQuantities.ContainsKey<ProductProto.ID, Quantity>(productId)).IsFalse<ProductProto.ID>("Duplicate product {0}.", productId);
        this.m_productQuantities.Add(Make.Kvp<ProductProto.ID, Quantity>(productId, new Quantity(quantity)));
        return this;
      }

      [MustUseReturnValue]
      public EntityCostsTpl.Builder ReplaceProduct(int quantity, ProductProto.ID productId)
      {
        Assert.That<bool>(this.m_productQuantities.ContainsKey<ProductProto.ID, Quantity>(productId)).IsTrue();
        this.m_productQuantities.Remove<ProductProto.ID, Quantity>(productId);
        this.m_productQuantities.Add(Make.Kvp<ProductProto.ID, Quantity>(productId, new Quantity(quantity)));
        return this;
      }

      [MustUseReturnValue]
      public EntityCostsTpl.Builder Workers(int workers)
      {
        this.m_workers = workers;
        return this;
      }

      [MustUseReturnValue]
      public EntityCostsTpl.Builder Priority(int defaultPriority)
      {
        this.m_defaultPriority = defaultPriority;
        return this;
      }

      [MustUseReturnValue]
      public EntityCostsTpl.Builder UPoints(int unity)
      {
        return this.Product(unity.Upoints().GetQuantityRounded().Value, IdsCore.Products.Upoints);
      }

      /// <summary>Sets monthly maintenance.</summary>
      [MustUseReturnValue]
      public EntityCostsTpl.Builder Maintenance(
        Fix32 maintenance,
        ProductProto.ID productId,
        Percent? initialMaintenanceBoost = null)
      {
        this.m_maintenance = new PartialQuantity(maintenance);
        this.m_maintenanceProduct = new ProductProto.ID?(productId);
        this.m_initialMaintenanceBoost = initialMaintenanceBoost;
        return this;
      }

      public static implicit operator EntityCostsTpl(EntityCostsTpl.Builder builder)
      {
        builder.m_productQuantities.RemoveAll((Predicate<KeyValuePair<ProductProto.ID, Quantity>>) (x => x.Value.IsNotPositive));
        return new EntityCostsTpl(builder.m_productQuantities, builder.m_workers, builder.m_defaultPriority, new EntityCostsTpl.MaintenanceCostsTpl(builder.m_maintenanceProduct, builder.m_maintenance, builder.m_initialMaintenanceBoost));
      }

      public Builder()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_productQuantities = new Lyst<KeyValuePair<ProductProto.ID, Quantity>>();
        this.m_defaultPriority = 9;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    public struct MaintenanceCostsTpl
    {
      public readonly ProductProto.ID? Product;
      public readonly PartialQuantity Quantity;
      public readonly Percent? InitialMaintenanceBoost;

      public MaintenanceCostsTpl(
        ProductProto.ID? product,
        PartialQuantity quantity,
        Percent? initialMaintenanceBoost)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Product = product;
        this.Quantity = quantity;
        this.InitialMaintenanceBoost = initialMaintenanceBoost;
      }
    }
  }
}
