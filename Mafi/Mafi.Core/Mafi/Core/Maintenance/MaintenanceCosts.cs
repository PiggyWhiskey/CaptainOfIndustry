// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.MaintenanceCosts
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct MaintenanceCosts : IEquatable<MaintenanceCosts>
  {
    public readonly VirtualProductProto Product;
    public readonly PartialQuantity MaintenancePerMonth;
    public readonly PartialQuantity MaxMaintenancePerMonth;
    public readonly Percent InitialMaintenanceBoost;

    public MaintenanceCosts(
      VirtualProductProto product,
      PartialQuantity maintenancePerMonth,
      Percent? initialMaintenanceBoost = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new MaintenanceCosts(product, maintenancePerMonth, maintenancePerMonth, initialMaintenanceBoost);
    }

    public MaintenanceCosts(
      VirtualProductProto product,
      Quantity maintenancePerMonth,
      Percent? initialMaintenanceBoost = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this = new MaintenanceCosts(product, maintenancePerMonth.AsPartial, maintenancePerMonth.AsPartial, initialMaintenanceBoost);
    }

    [LoadCtor]
    public MaintenanceCosts(
      VirtualProductProto product,
      PartialQuantity maintenancePerMonth,
      PartialQuantity maxMaintenancePerMonth,
      Percent? initialMaintenanceBoost = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Product = product;
      this.MaintenancePerMonth = maintenancePerMonth.CheckNotNegative();
      this.MaxMaintenancePerMonth = maxMaintenancePerMonth.CheckNotNegative();
      this.InitialMaintenanceBoost = initialMaintenanceBoost ?? Percent.Zero;
    }

    public bool Equals(MaintenanceCosts other)
    {
      return object.Equals((object) this.Product, (object) other.Product) && this.MaintenancePerMonth.Equals(other.MaintenancePerMonth);
    }

    public override bool Equals(object obj) => obj is MaintenanceCosts other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<VirtualProductProto, PartialQuantity>(this.Product, this.MaintenancePerMonth);
    }

    public static void Serialize(MaintenanceCosts value, BlobWriter writer)
    {
      writer.WriteGeneric<VirtualProductProto>(value.Product);
      PartialQuantity.Serialize(value.MaintenancePerMonth, writer);
      PartialQuantity.Serialize(value.MaxMaintenancePerMonth, writer);
      Percent.Serialize(value.InitialMaintenanceBoost, writer);
    }

    public static MaintenanceCosts Deserialize(BlobReader reader)
    {
      return new MaintenanceCosts(reader.ReadGenericAs<VirtualProductProto>(), PartialQuantity.Deserialize(reader), PartialQuantity.Deserialize(reader), new Percent?(Percent.Deserialize(reader)));
    }
  }
}
