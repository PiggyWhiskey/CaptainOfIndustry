// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellPumpAdditionValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using System;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class WellPumpAdditionValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly IVirtualResourceManager m_virtualResourceManager;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public WellPumpAdditionValidator(IVirtualResourceManager virtualResourceManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_virtualResourceManager = virtualResourceManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (!(addRequest.Proto is WellPumpProto proto))
        return EntityValidationResult.Success;
      ImmutableArray<IVirtualTerrainResource> immutableArray = this.m_virtualResourceManager.RetrieveResourcesAt(proto.MinedProduct.Product, addRequest.Transform.Position.Tile2i);
      return immutableArray.IsEmpty || immutableArray.All((Func<IVirtualTerrainResource, bool>) (x => x.Quantity.IsZero && x.Product.IsResourceFinal)) ? EntityValidationResult.CreateError(TrCore.AdditionError__NoDeposit.Format(proto.MinedProduct.Product.Strings.Name.TranslatedString).Value) : EntityValidationResult.Success;
    }
  }
}
