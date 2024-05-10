// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.LooseProductParam
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  /// <summary>
  /// Specifies what terrain material appears when a loose product is dumped. We cannot have this info in the attribute
  /// since IDs are not constants.
  /// </summary>
  public sealed class LooseProductParam : IProtoParam
  {
    public readonly Proto.ID DumpAs;

    public Type AllowedProtoType => typeof (LooseProductProto);

    public LooseProductParam(Proto.ID dumpAs)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DumpAs = dumpAs;
    }
  }
}
