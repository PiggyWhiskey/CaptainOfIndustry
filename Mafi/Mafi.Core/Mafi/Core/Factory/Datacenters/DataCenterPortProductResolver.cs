// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Datacenters.DataCenterPortProductResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Datacenters
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class DataCenterPortProductResolver : PortProductResolverBase<DataCenter>
  {
    public DataCenterPortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      DataCenter entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      return this.GetPortProduct((IEntityProto) entity.Prototype, port.Spec);
    }

    public override ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec)
    {
      DataCenterProto dataCenterProto = (DataCenterProto) proto;
      if (dataCenterProto.CoolantInPorts.Contains(portSpec.Name))
        return ImmutableArray.Create<ProductProto>(dataCenterProto.CoolantIn);
      if (dataCenterProto.CoolantOutPorts.Contains(portSpec.Name))
        return ImmutableArray.Create<ProductProto>(dataCenterProto.CoolantOut);
      Log.Error(string.Format("Unhandled port {0}.", (object) portSpec));
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
