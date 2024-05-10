// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.PortProductsResolver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class PortProductsResolver
  {
    private readonly Dict<Type, IPortProductResolverImpl> m_productResolvers;

    public PortProductsResolver(
      AllImplementationsOf<IPortProductResolverImpl> portProductResolvers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_productResolvers = new Dict<Type, IPortProductResolverImpl>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      foreach (IPortProductResolverImpl implementation in portProductResolvers.Implementations)
        this.m_productResolvers.AddAndAssertNew(implementation.ResolvedEntityType, implementation);
    }

    /// <summary>
    /// Returns port products for a concrete port. By default this considers enabled recipes only.
    /// The <paramref name="considerAllUnlockedRecipes" /> can be used consider all unlocked.
    /// This is handy for entities that are in construction or disabled.
    /// </summary>
    public ImmutableArray<ProductProto> GetPortProducts(
      IoPort port,
      bool considerAllUnlockedRecipes = false,
      bool fallbackToUnlockedIfNoRecipesAssigned = false)
    {
      IPortProductResolverImpl resolver;
      return !this.getPortProductResolver(port.OwnerEntity.GetType(), out resolver) ? ImmutableArray<ProductProto>.Empty : resolver.GetPortProduct(port, considerAllUnlockedRecipes, fallbackToUnlockedIfNoRecipesAssigned);
    }

    public ImmutableArray<KeyValuePair<ImmutableArray<ProductProto>, Tile3f>> GetEntityPortProductsClustered(
      IEntityWithPorts entity,
      bool considerAllUnlockedRecipes = false)
    {
      return ((IEnumerable<KeyValuePair<IoPort, ImmutableArray<ProductProto>>>) entity.Ports.MapArray<KeyValuePair<IoPort, ImmutableArray<ProductProto>>>((Func<IoPort, KeyValuePair<IoPort, ImmutableArray<ProductProto>>>) (x => Make.Kvp<IoPort, ImmutableArray<ProductProto>>(x, this.GetPortProducts(x, considerAllUnlockedRecipes))))).GroupBy<KeyValuePair<IoPort, ImmutableArray<ProductProto>>, string>((Func<KeyValuePair<IoPort, ImmutableArray<ProductProto>>, string>) (x => x.Key.Spec.Type.ToString() + x.Value.Select<string>((Func<ProductProto, string>) (p => p.Id.Value)).OrderBy<string, string>((Func<string, string>) (s => s)).JoinStrings("|"))).Select<IGrouping<string, KeyValuePair<IoPort, ImmutableArray<ProductProto>>>, KeyValuePair<ImmutableArray<ProductProto>, Tile3f>>((Func<IGrouping<string, KeyValuePair<IoPort, ImmutableArray<ProductProto>>>, KeyValuePair<ImmutableArray<ProductProto>, Tile3f>>) (g =>
      {
        Lyst<Tile3i> lyst = g.Select<KeyValuePair<IoPort, ImmutableArray<ProductProto>>, Tile3i>((Func<KeyValuePair<IoPort, ImmutableArray<ProductProto>>, Tile3i>) (x => x.Key.Position)).ToLyst<Tile3i>();
        Tile3f zero = Tile3f.Zero;
        foreach (Tile3i tile3i in lyst)
          zero += tile3i.CenterTile3f.RelTile3f;
        Tile3f tile3f = (zero / (Fix32) lyst.Count).AddZ(Fix32.Half);
        return Make.Kvp<ImmutableArray<ProductProto>, Tile3f>(g.First<KeyValuePair<IoPort, ImmutableArray<ProductProto>>>().Value, tile3f);
      })).ToImmutableArray<KeyValuePair<ImmutableArray<ProductProto>, Tile3f>>();
    }

    /// <summary>
    /// Returns port products for given port. This considers all unlocked recipes.
    /// </summary>
    public ImmutableArray<ProductProto> GetPortProducts(IStaticEntityProto proto, PortSpec portSpec)
    {
      IPortProductResolverImpl resolver;
      return !this.getPortProductResolver(proto.EntityType, out resolver) ? ImmutableArray<ProductProto>.Empty : resolver.GetPortProduct((IEntityProto) proto, portSpec);
    }

    /// <summary>
    /// Returns groups of products and their locations relative to the layout.
    /// This considers all unlocked recipes.
    /// </summary>
    public ImmutableArray<KeyValuePair<ImmutableArray<ProductProto>, RelTile3f>> GetLayoutEntityPortProductsClustered(
      LayoutEntityProto proto)
    {
      return ((IEnumerable<KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>>) proto.Ports.MapArray<KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>>((Func<IoPortTemplate, KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>>) (x => Make.Kvp<IoPortTemplate, ImmutableArray<ProductProto>>(x, this.GetPortProducts((IStaticEntityProto) proto, x.Spec))))).GroupBy<KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>, string>((Func<KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>, string>) (x => x.Key.Spec.Type.ToString() + x.Value.Select<string>((Func<ProductProto, string>) (p => p.Id.Value)).OrderBy<string, string>((Func<string, string>) (s => s)).JoinStrings("|"))).Select<IGrouping<string, KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>>, KeyValuePair<ImmutableArray<ProductProto>, RelTile3f>>((Func<IGrouping<string, KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>>, KeyValuePair<ImmutableArray<ProductProto>, RelTile3f>>) (g =>
      {
        RelTile3f zero = RelTile3f.Zero;
        int num = 0;
        foreach (KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>> keyValuePair in (IEnumerable<KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>>) g)
        {
          zero += keyValuePair.Key.RelativePosition.CenterRelTile3f;
          ++num;
        }
        RelTile3f relTile3f = zero / (Fix32) num;
        return Make.Kvp<ImmutableArray<ProductProto>, RelTile3f>(g.First<KeyValuePair<IoPortTemplate, ImmutableArray<ProductProto>>>().Value, relTile3f);
      })).ToImmutableArray<KeyValuePair<ImmutableArray<ProductProto>, RelTile3f>>();
    }

    /// <summary>Finds a product resolver for given entity type.</summary>
    private bool getPortProductResolver(Type entityType, out IPortProductResolverImpl resolver)
    {
      if (this.m_productResolvers.TryGetValue(entityType, out resolver))
        return true;
      foreach (KeyValuePair<Type, IPortProductResolverImpl> productResolver in this.m_productResolvers)
      {
        if (entityType.IsAssignableTo(productResolver.Key))
        {
          this.m_productResolvers.Add(entityType, productResolver.Value);
          resolver = productResolver.Value;
          return true;
        }
      }
      return false;
    }

    public bool TryGetFirstUnconnectedPortFor(
      IEntityWithPorts entity,
      IoPortType type,
      ProductProto product,
      char? name,
      out IoPort fountPort)
    {
      if (name.HasValue)
      {
        fountPort = entity.Ports.FindOrDefault((Predicate<IoPort>) (x =>
        {
          if ((int) x.Name != (int) name.Value || !x.IsNotConnected)
            return false;
          return x.Type == type || x.Type == IoPortType.Any;
        }));
        return fountPort != null;
      }
      fountPort = entity.Ports.FirstOrDefault((Func<IoPort, bool>) (port => !port.IsConnected && port.Type == type && this.GetPortProducts(port, true).Contains(product)));
      return fountPort != null;
    }
  }
}
