// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.UnstableTerrainMaterialParam
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <summary>
  /// Marked proto is considered landfill and buildings cannot be constructed on top of it.
  /// </summary>
  public sealed class UnstableTerrainMaterialParam : IProtoParam
  {
    public static readonly UnstableTerrainMaterialParam Instance;

    public Type AllowedProtoType => typeof (TerrainMaterialProto);

    public UnstableTerrainMaterialParam()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static UnstableTerrainMaterialParam()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnstableTerrainMaterialParam.Instance = new UnstableTerrainMaterialParam();
    }
  }
}
