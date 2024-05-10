// Decompiled with JetBrains decompiler
// Type: Mafi.Base.EntityCostsBuilderExtensions
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base
{
  public static class EntityCostsBuilderExtensions
  {
    [MustUseReturnValue]
    public static EntityCostsTpl.Builder CP(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.ConstructionParts);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder CP2(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.ConstructionParts2);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder CP3(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.ConstructionParts3);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder CP4(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.ConstructionParts4);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Concrete(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.ConcreteSlab);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Copper(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.Copper);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Iron(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.Iron);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Steel(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.Steel);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Glass(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.Glass);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Electronics(this EntityCostsTpl.Builder builder, int count)
    {
      return builder.Product(count, Ids.Products.Electronics);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Electronics2(
      this EntityCostsTpl.Builder builder,
      int count)
    {
      return builder.Product(count, Ids.Products.Electronics2);
    }

    [MustUseReturnValue]
    public static EntityCostsTpl.Builder Electronics3(
      this EntityCostsTpl.Builder builder,
      int count)
    {
      return builder.Product(count, Ids.Products.Electronics3);
    }

    /// <summary>Monthly maintenance for buildings.</summary>
    [MustUseReturnValue]
    public static EntityCostsTpl.Builder MaintenanceT1SuperEarly(
      this EntityCostsTpl.Builder builder,
      int count)
    {
      return builder.Maintenance((Fix32) count, Ids.Products.MaintenanceT1, new Percent?(260.Percent()));
    }

    /// <summary>Monthly maintenance for buildings.</summary>
    [MustUseReturnValue]
    public static EntityCostsTpl.Builder MaintenanceT1Early(
      this EntityCostsTpl.Builder builder,
      int count)
    {
      return builder.Maintenance((Fix32) count, Ids.Products.MaintenanceT1, new Percent?(180.Percent()));
    }

    /// <summary>Monthly maintenance for buildings.</summary>
    [MustUseReturnValue]
    public static EntityCostsTpl.Builder MaintenanceT1(
      this EntityCostsTpl.Builder builder,
      Fix32 count)
    {
      return builder.Maintenance(count, Ids.Products.MaintenanceT1);
    }

    /// <summary>Monthly maintenance for buildings.</summary>
    [MustUseReturnValue]
    public static EntityCostsTpl.Builder MaintenanceT2(
      this EntityCostsTpl.Builder builder,
      int count)
    {
      return builder.Maintenance((Fix32) count, Ids.Products.MaintenanceT2);
    }

    /// <summary>Monthly maintenance for buildings.</summary>
    [MustUseReturnValue]
    public static EntityCostsTpl.Builder MaintenanceT2(
      this EntityCostsTpl.Builder builder,
      Fix32 count)
    {
      return builder.Maintenance(count, Ids.Products.MaintenanceT2);
    }

    /// <summary>Monthly maintenance for buildings.</summary>
    [MustUseReturnValue]
    public static EntityCostsTpl.Builder MaintenanceT3(
      this EntityCostsTpl.Builder builder,
      int count)
    {
      return builder.Maintenance((Fix32) count, Ids.Products.MaintenanceT3);
    }
  }
}
