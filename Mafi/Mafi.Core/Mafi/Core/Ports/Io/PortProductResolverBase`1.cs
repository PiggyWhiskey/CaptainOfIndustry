// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.PortProductResolverBase`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  public abstract class PortProductResolverBase<TResolvedEntity> : IPortProductResolverImpl where TResolvedEntity : IStaticEntity
  {
    private readonly ImmutableArray<ProductProto> m_shaftProduct;

    public Type ResolvedEntityType => typeof (TResolvedEntity);

    protected PortProductResolverBase(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductProto proto;
      if (protosDb.TryGetProto<ProductProto>((Proto.ID) IdsCore.Products.MechanicalPower, out proto))
      {
        this.m_shaftProduct = ImmutableArray.Create<ProductProto>(proto);
      }
      else
      {
        Log.Error("No mechanical power proto defined.");
        this.m_shaftProduct = ImmutableArray<ProductProto>.Empty;
      }
    }

    public ImmutableArray<ProductProto> GetPortProduct(
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned)
    {
      return port.ShapePrototype.Id == IdsCore.Transports.ShaftPortShape ? this.m_shaftProduct : this.GetPortProduct((TResolvedEntity) port.OwnerEntity, port, considerAllUnlockedRecipes, fallbackToUnlockedIfNoRecipesAssigned);
    }

    protected abstract ImmutableArray<ProductProto> GetPortProduct(
      TResolvedEntity entity,
      IoPort port,
      bool considerAllUnlockedRecipes,
      bool fallbackToUnlockedIfNoRecipesAssigned);

    public abstract ImmutableArray<ProductProto> GetPortProduct(
      IEntityProto proto,
      PortSpec portSpec);
  }
}
