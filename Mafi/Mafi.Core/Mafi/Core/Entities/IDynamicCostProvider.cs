﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IDynamicCostProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Economy;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Provides added cost for entity that are calculated on fly and can change.
  /// Example: unity for housing.
  /// </summary>
  [MultiDependency]
  public interface IDynamicCostProvider
  {
    Type ManagedProtoType { get; }

    AssetValue GetDynamicCost();
  }
}
