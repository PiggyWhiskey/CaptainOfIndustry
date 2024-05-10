// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.FarmPortProductResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class FarmPortProductResolver : PortProductResolverBase<Farm>
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly Dict<FarmPortProductResolver.Key, ImmutableArray<ProductProto>> m_inputPortProductsCache;
    private readonly ImmutableArray<ProductProto> m_fertilizers;
    private readonly ImmutableArray<ProductProto> m_cropProductsForGreenhouse;
    private readonly ImmutableArray<ProductProto> m_cropProductsForNoGreenhouse;

    public FarmPortProductResolver(ProtosDb protosDb, UnlockedProtosDb unlockedProtosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_inputPortProductsCache = new Dict<FarmPortProductResolver.Key, ImmutableArray<ProductProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb);
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_fertilizers = protosDb.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.GetParam<FertilizerProductParam>().HasValue)).ToImmutableArray<ProductProto>();
      this.m_cropProductsForNoGreenhouse = protosDb.All<CropProto>().Where<CropProto>((Func<CropProto, bool>) (x => !x.RequiresGreenhouse)).Select<CropProto, ProductProto>((Func<CropProto, ProductProto>) (x => x.ProductProduced.Product)).Where<ProductProto>((Func<ProductProto, bool>) (x => x.IsNotPhantom)).ToImmutableArray<ProductProto>();
      this.m_cropProductsForGreenhouse = protosDb.All<CropProto>().Select<CropProto, ProductProto>((Func<CropProto, ProductProto>) (x => x.ProductProduced.Product)).Where<ProductProto>((Func<ProductProto, bool>) (x => x.IsNotPhantom)).ToImmutableArray<ProductProto>();
    }

    protected override ImmutableArray<ProductProto> GetPortProduct(
      Farm entity,
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
      if (!(proto is FarmProto farmProto))
        return ImmutableArray<ProductProto>.Empty;
      if (portSpec.Type == IoPortType.Input)
        return this.getOutputPortProduct(farmProto, portSpec.Name);
      if (portSpec.Type == IoPortType.Output)
        return this.getFarmOutput(farmProto, portSpec);
      Log.Warning(string.Format("Unhandled farm port type {0}", (object) portSpec.Type));
      return ImmutableArray<ProductProto>.Empty;
    }

    private ImmutableArray<ProductProto> getFarmOutput(FarmProto farm, PortSpec portSpec)
    {
      return farm.IsGreenhouse ? this.m_cropProductsForGreenhouse.Where((Func<ProductProto, bool>) (x => portSpec.Shape.AllowedProductType == x.Type && this.m_unlockedProtosDb.IsUnlocked((Proto) x))).ToImmutableArray<ProductProto>() : this.m_cropProductsForNoGreenhouse.Where((Func<ProductProto, bool>) (x => portSpec.Shape.AllowedProductType == x.Type && this.m_unlockedProtosDb.IsUnlocked((Proto) x))).ToImmutableArray<ProductProto>();
    }

    private ImmutableArray<ProductProto> getOutputPortProduct(FarmProto proto, char portName)
    {
      FarmPortProductResolver.Key key = new FarmPortProductResolver.Key(proto, portName);
      ImmutableArray<ProductProto> outputPortProduct;
      if (this.m_inputPortProductsCache.TryGetValue(key, out outputPortProduct))
        return outputPortProduct;
      ImmutableArray<ProductProto> fertilizers;
      switch (portName)
      {
        case 'A':
          fertilizers = ImmutableArray.Create<ProductProto>(proto.WaterCollectedPerDay.Product);
          break;
        case 'B':
          fertilizers = this.m_fertilizers;
          break;
        default:
          Log.Warning(string.Format("Unknown farm port {0}", (object) portName));
          return ImmutableArray<ProductProto>.Empty;
      }
      this.m_inputPortProductsCache[key] = fertilizers;
      return fertilizers;
    }

    private struct Key : IEquatable<FarmPortProductResolver.Key>
    {
      public readonly FarmProto Proto;
      public readonly char PortName;

      public Key(FarmProto proto, char portName)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Proto = proto;
        this.PortName = portName;
      }

      public bool Equals(FarmPortProductResolver.Key other)
      {
        return (Proto) this.Proto == (Proto) other.Proto && (int) this.PortName == (int) other.PortName;
      }

      public override bool Equals(object obj)
      {
        return obj is FarmPortProductResolver.Key other && this.Equals(other);
      }

      public override int GetHashCode() => Hash.Combine<FarmProto, char>(this.Proto, this.PortName);
    }
  }
}
