// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.RuinsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class RuinsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProductProto orThrow1 = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.IronScrap);
      ProductProto orThrow2 = registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Bricks);
      registrator.PrototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.Electronics);
      int quantity = 944;
      string description = "Seems like a building that is abandoned for a long time and no longer useful. We can deconstruct it for useful materials which can be processed in our factory.";
      registrator.RuinsProtoBuilder.Start("Abandoned communication station", Ids.Buildings.Ruins).Description(description).SetDeconstructionParams(new AssetValue(orThrow1.WithQuantity(quantity), orThrow2.WithQuantity(600)), 1.Seconds()).SetLayout(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("(12", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: 12, terrainSurfaceHeight: new int?(0))))
      }), "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(12(12(12(12(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(12(12(12(12(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(12(12(12(12(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(12(12(12(12(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(12(12(12(12(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(8)(8)(8)(8)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)", "(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)").SetPrefabPath("Assets/Base/Buildings/Ruins/RadioTower.prefab").SetPrefabOffset(-RelTile3f.UnitX).SetNoCategory().BuildAndAdd();
    }

    public RuinsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
