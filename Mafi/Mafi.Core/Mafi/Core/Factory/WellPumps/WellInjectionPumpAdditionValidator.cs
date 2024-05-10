// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellInjectionPumpAdditionValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Resources;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class WellInjectionPumpAdditionValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly IVirtualResourceManager m_virtualResourceManager;
    private readonly TerrainManager m_terrainManager;
    private readonly Lyst<ProductResource> m_resultCache;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public WellInjectionPumpAdditionValidator(
      IVirtualResourceManager virtualResourceManager,
      TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_resultCache = new Lyst<ProductResource>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_virtualResourceManager = virtualResourceManager;
      this.m_terrainManager = terrainManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (!(addRequest.Proto is WellInjectionPumpProto proto))
        return EntityValidationResult.Success;
      ImmutableArray<IVirtualTerrainResource> immutableArray = this.m_virtualResourceManager.RetrieveAllResourcesAt(addRequest.Transform.Position.Tile2i);
      if (immutableArray.IsNotEmpty)
        return EntityValidationResult.CreateError(TrCore.AdditionError__HasDeposit.Format(immutableArray.First.Product.Strings.Name.TranslatedString).Value);
      TerrainTile terrainTile = this.m_terrainManager[addRequest.Transform.Position.Tile2i];
      this.m_resultCache.Clear();
      HybridSet<LooseProductProto> products = HybridSet<LooseProductProto>.From(proto.TerrainProductRequired);
      terrainTile.GetResourceDetails(products, this.m_resultCache);
      if (this.m_resultCache.IsEmpty)
        return EntityValidationResult.CreateError(TrCore.AdditionError__NoDeposit.Format(proto.TerrainProductRequired.Strings.Name.TranslatedString).Value);
      if (this.m_resultCache.First.Height.FlooredThicknessTilesI.Value >= 6)
        return EntityValidationResult.Success;
      ref readonly LocStr2 local = ref TrCore.AdditionError__ThinDeposit;
      int num = 6;
      string str1 = num.ToString();
      num = this.m_resultCache.First.Height.FlooredThicknessTilesI.Value;
      string str2 = num.ToString();
      return EntityValidationResult.CreateError(local.Format(str1, str2).Value);
    }
  }
}
