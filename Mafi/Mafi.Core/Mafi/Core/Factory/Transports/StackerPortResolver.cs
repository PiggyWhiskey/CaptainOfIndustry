// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.StackerPortResolver
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
namespace Mafi.Core.Factory.Transports
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class StackerPortResolver : PortProductResolverBase<Stacker>
  {
    private readonly ImmutableArray<ProductProto> m_allDumpableProducts;

    public StackerPortResolver(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
      this.m_allDumpableProducts = protosDb.All<LooseProductProto>().Where<LooseProductProto>((Func<LooseProductProto, bool>) (x => x.CanBeOnTerrain)).Cast<ProductProto>().ToImmutableArray<ProductProto>();
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      Stacker entity,
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
      return this.m_allDumpableProducts;
    }
  }
}
