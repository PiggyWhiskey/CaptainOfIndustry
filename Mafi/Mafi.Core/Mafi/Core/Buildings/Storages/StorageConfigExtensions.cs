// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  public static class StorageConfigExtensions
  {
    public static bool? GetAlertWhenAboveEnabled(this EntityConfigData data)
    {
      return data.GetBool("AlertWhenAboveEnabled");
    }

    public static void SetAlertWhenAboveEnabled(this EntityConfigData data, bool value)
    {
      data.SetBool("AlertWhenAboveEnabled", new bool?(value));
    }

    public static Percent? GetAlertWhenAbove(this EntityConfigData data)
    {
      return data.GetPercent("AlertWhenAbove");
    }

    public static void SetAlertWhenAbove(this EntityConfigData data, Percent value)
    {
      data.SetPercent("AlertWhenAbove", new Percent?(value));
    }

    public static bool? GetAlertWhenBelowEnabled(this EntityConfigData data)
    {
      return data.GetBool("AlertWhenBelowEnabled");
    }

    public static void SetAlertWhenBelowEnabled(this EntityConfigData data, bool value)
    {
      data.SetBool("AlertWhenBelowEnabled", new bool?(value));
    }

    public static Percent? GetAlertWhenBelow(this EntityConfigData data)
    {
      return data.GetPercent("AlertWhenBelow");
    }

    public static void SetAlertWhenBelow(this EntityConfigData data, Percent value)
    {
      data.SetPercent("AlertWhenBelow", new Percent?(value));
    }

    public static bool? GetIsEnforcingCustomVehicles(this EntityConfigData data)
    {
      return data.GetBool("IsEnforcingCustomVehicles");
    }

    public static void SetIsEnforcingCustomVehicles(this EntityConfigData data, bool value)
    {
      data.SetBool("IsEnforcingCustomVehicles", new bool?(value));
    }

    public static int? GetStorageImportPriority(this EntityConfigData data)
    {
      return data.GetInt("StorageImportPriority");
    }

    public static void SetStorageImportPriority(this EntityConfigData data, int value)
    {
      data.SetInt("StorageImportPriority", new int?(value));
    }

    public static int? GetStorageExportPriority(this EntityConfigData data)
    {
      return data.GetInt("StorageExportPriority");
    }

    public static void SetStorageExportPriority(this EntityConfigData data, int value)
    {
      data.SetInt("StorageExportPriority", new int?(value));
    }

    public static Percent? GetStorageImportUntilPercent(this EntityConfigData data)
    {
      return data.GetPercent("StorageImportUntilPercent");
    }

    public static void SetStorageImportUntilPercent(this EntityConfigData data, Percent value)
    {
      data.SetPercent("StorageImportUntilPercent", new Percent?(value));
    }

    public static Percent? GetStorageExportFromPercent(this EntityConfigData data)
    {
      return data.GetPercent("StorageExportFromPercent");
    }

    public static void SetStorageExportFromPercent(this EntityConfigData data, Percent value)
    {
      data.SetPercent("StorageExportFromPercent", new Percent?(value));
    }

    public static Option<ProductProto> GetStorageStoredProduct(this EntityConfigData data)
    {
      return data.GetProto<ProductProto>("StorageStoredProduct", false);
    }

    public static void SetStorageStoredProduct(
      this EntityConfigData data,
      Option<ProductProto> value)
    {
      data.SetProto<ProductProto>("StorageStoredProduct", value);
    }
  }
}
