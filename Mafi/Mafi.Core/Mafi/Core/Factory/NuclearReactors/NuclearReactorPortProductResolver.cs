// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.NuclearReactors.NuclearReactorPortProductResolver
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
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Factory.NuclearReactors
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class NuclearReactorPortProductResolver : PortProductResolverBase<NuclearReactor>
  {
    public NuclearReactorPortProductResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      NuclearReactor entity,
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
      NuclearReactorProto nuclearReactorProto = (NuclearReactorProto) proto;
      if (portSpec.Type == IoPortType.Input)
      {
        if ((int) portSpec.Name == (int) nuclearReactorProto.CoolantInPort)
          return ImmutableArray.Create<ProductProto>(nuclearReactorProto.CoolantIn);
        if (nuclearReactorProto.WaterInPorts.Contains<char>(portSpec.Name))
          return ImmutableArray.Create<ProductProto>(nuclearReactorProto.WaterInPerPowerLevel.Product);
        if ((int) nuclearReactorProto.FuelInPort == (int) portSpec.Name)
          return nuclearReactorProto.FuelPairs.Select<ProductProto>((Func<NuclearReactorProto.FuelData, ProductProto>) (x => x.FuelInProto)).ToImmutableArray<ProductProto>();
        if (nuclearReactorProto.Enrichment.HasValue && (int) nuclearReactorProto.Enrichment.Value.InPort == (int) portSpec.Name)
          return ImmutableArray.Create<ProductProto>(nuclearReactorProto.Enrichment.Value.InputProduct);
      }
      else if (portSpec.Type == IoPortType.Output)
      {
        if ((int) portSpec.Name == (int) nuclearReactorProto.CoolantOutPort)
          return ImmutableArray.Create<ProductProto>(nuclearReactorProto.CoolantOut);
        if (nuclearReactorProto.SteamOutPorts.Contains<char>(portSpec.Name))
          return ImmutableArray.Create<ProductProto>(nuclearReactorProto.SteamOutPerPowerLevel.Product);
        if ((int) nuclearReactorProto.FuelOutPort == (int) portSpec.Name)
          return nuclearReactorProto.FuelPairs.Select<ProductProto>((Func<NuclearReactorProto.FuelData, ProductProto>) (x => x.SpentFuelOutProto)).ToImmutableArray<ProductProto>();
        if (nuclearReactorProto.Enrichment.HasValue && (int) nuclearReactorProto.Enrichment.Value.OutPort == (int) portSpec.Name)
          return ImmutableArray.Create<ProductProto>(nuclearReactorProto.Enrichment.Value.OutputProduct);
      }
      Log.Error(string.Format("Unhandled port {0}.", (object) portSpec.Name));
      return ImmutableArray<ProductProto>.Empty;
    }
  }
}
