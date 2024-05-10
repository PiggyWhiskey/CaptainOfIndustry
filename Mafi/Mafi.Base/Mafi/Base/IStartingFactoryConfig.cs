// Decompiled with JetBrains decompiler
// Type: Mafi.Base.IStartingFactoryConfig
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base
{
  public interface IStartingFactoryConfig
  {
    int InitialTrucks { get; }

    int InitialExcavators { get; }

    int InitialTreeHarvesters { get; }

    ProductProto.ID StartingFoodProto { get; }

    ImmutableArray<KeyValuePair<ProductProto.ID, Quantity>> InitialProducts { get; }

    ImmutableArray<KeyValuePair<ProductProto.ID, Quantity>> ExtraInitialProductsIfGoalsSkipped { get; }
  }
}
