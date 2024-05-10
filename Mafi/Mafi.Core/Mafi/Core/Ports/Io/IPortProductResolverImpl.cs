// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IPortProductResolverImpl
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  /// <summary>
  /// Provides a products corresponding to ports for a given type of an entity.
  /// To query port products use <see cref="T:Mafi.Core.Ports.Io.PortProductsResolver" />.
  /// </summary>
  [MultiDependency]
  public interface IPortProductResolverImpl
  {
    /// <summary>Type of entity that owns queried ports.</summary>
    Type ResolvedEntityType { get; }

    /// <summary>
    /// Returns a product to be shown above given port of a given entity.
    /// By default this considers enabled recipes only.
    /// The <paramref name="considerAllUnlockedRecipes" /> can be used consider all unlocked.
    /// </summary>
    ImmutableArray<ProductProto> GetPortProduct(
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned);

    /// <summary>
    /// This can be used in previews where we don't have entity instance yet.
    /// </summary>
    ImmutableArray<ProductProto> GetPortProduct(IEntityProto proto, PortSpec portSpec);
  }
}
