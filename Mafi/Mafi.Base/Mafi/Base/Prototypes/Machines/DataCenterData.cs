// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.DataCenterData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes.Machines.ComputingEntities;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Datacenters;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class DataCenterData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProductProto orThrow = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Server);
      registrator.PrototypesDb.Add<ServerRackProto>(new ServerRackProto(Ids.DataCenters.BasicServerRack, Proto.CreateStr(Ids.DataCenters.BasicServerRack, "Basic rack", "", "title of a server rack in a datacenter (rack is just a set of servers assembled together)."), 40.Kw(), Computing.FromTFlops(2), new ProductQuantity(orThrow, new Quantity(25)), new ProductQuantity(orThrow, new Quantity(25)), new PartialQuantity(0.5.ToFix32()), new PartialQuantity(0.5.ToFix32()), new PartialQuantity(0.58.ToFix32()), new ServerRackProto.Gfx("Assets/Base/Buildings/DataCenter/ServerRack128.png", "Assets/Base/Buildings/DataCenter/Rack.prefab", ImmutableArray.Create<string>("DataCenter_Rack1", "DataCenter_Rack2", "DataCenter_Rack3", "DataCenter_Rack4"))));
      string description = "Datacenter is used to host server racks that you add to it. It is highly scalable and makes computing more affordable. Each added server rack provides computing. It is worth noting that each server rack has its power, cooling, and maintenance demands. However, some people say that all it does is to persist and search in a vast collection of pictures of cats and memes - we were not able to debunk this.";
      registrator.DataCenterProtoBuilder.Start("Data center", Ids.DataCenters.DataCenter).SetCost(Costs.Buildings.Datacenter).Description(description).SetLayout("   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", ">@A[5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]>@B", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", "<@X[5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]<@Y", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]   ").SetPrefabPath("Assets/Base/Buildings/DataCenter/DataCenter.prefab").SetRacksCapacity(48).SetRackPositionsGenerator(new Func<int, DataCenterProto.RackPosition>(rackPositionGenerator)).SetCoolants(Ids.Products.ChilledWater, Ids.Products.Water, 60.Quantity()).SetPortsSpec(ImmutableArray.Create<char>('A', 'B'), ImmutableArray.Create<char>('X', 'Y')).SetCategories(Ids.ToolbarCategories.Machines).BuildAndAdd();
      registrator.PrototypesDb.Add<MainframeProto>(new MainframeProto((StaticEntityProto.ID) Ids.DataCenters.Mainframe, Proto.CreateStr((Proto.ID) Ids.DataCenters.Mainframe, "Mainframe computer", "Provides computing as a resource that can be used on your island. Computing is used in advanced machines such as robotic assemblers or microchip makers. This is early technology with low efficiency."), registrator.LayoutParser.ParseLayoutOrThrow("[5][5][5][5][5][5][5][5]", "[5][5][5][5][5][5][5][5]", "[5][5][5][5][5][5][5][5]", "[5][5][5][5][5][5][5][5]", "[5][5][5][5][5][5][5][5]", "[5][5][5][5][5][5][5][5]", "[5][5][5][5][5][5][5][5]", "[5][5][5][5][5][5][5][5]"), Costs.Buildings.MainframeComputer.MapToEntityCosts(registrator), Computing.FromTFlops(8), 2000.Kw(), new LayoutEntityProto.Gfx("Assets/Base/Buildings/Mainframe.prefab", customIconPath: Option<string>.None, categories: new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Machines)))));

      static DataCenterProto.RackPosition rackPositionGenerator(int rackIndex)
      {
        int num1 = rackIndex / 12;
        int num2 = rackIndex % 12;
        return new DataCenterProto.RackPosition(new RelTile2f((Fix32) (num2 * 2 - 13 + (num2 >= 4 ? 2 : 0) + (num2 >= 8 ? 2 : 0)), (Fix32) (num1 * 4 - 6)), num1 % 2 == 1 ? Rotation90.Deg180 : Rotation90.Deg0);
      }
    }

    public DataCenterData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
