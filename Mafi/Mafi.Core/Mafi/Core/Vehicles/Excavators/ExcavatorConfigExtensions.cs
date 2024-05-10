// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Excavators.ExcavatorConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Vehicles.Excavators
{
  public static class ExcavatorConfigExtensions
  {
    public static Option<LooseProductProto> GetPrioritizedProductToMine(this EntityConfigData data)
    {
      return data.GetProto<LooseProductProto>("PrioritizedProductToMine", true);
    }

    public static void SetPrioritizedProductToMine(
      this EntityConfigData data,
      Option<LooseProductProto> value)
    {
      data.SetProto<LooseProductProto>("PrioritizedProductToMine", value);
    }
  }
}
