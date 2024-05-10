// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotModuleConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.World.Contracts;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  public static class CargoDepotModuleConfigExtensions
  {
    public static Option<ContractProto> GetContractAssigned(this EntityConfigData data)
    {
      return data.GetProto<ContractProto>("ContractAssigned", true);
    }

    public static void SetContractAssigned(this EntityConfigData data, Option<ContractProto> value)
    {
      data.SetProto<ContractProto>("ContractAssigned", value);
    }

    public static Option<ProductProto> GetProductAssignedToDepotModule(this EntityConfigData data)
    {
      return data.GetProto<ProductProto>("ProductAssignedToDepotModule", true);
    }

    public static void SetProductAssignedToDepotModule(
      this EntityConfigData data,
      Option<ProductProto> value)
    {
      data.SetProto<ProductProto>("ProductAssignedToDepotModule", value);
    }
  }
}
