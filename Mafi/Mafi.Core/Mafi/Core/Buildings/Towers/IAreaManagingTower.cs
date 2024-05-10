// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Towers.IAreaManagingTower
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Buildings.Towers
{
  public interface IAreaManagingTower : IEntity, IIsSafeAsHashKey, IAssignableToFuelStation
  {
    IReadOnlySet<TerrainDesignation> ManagedDesignations { get; }

    RectangleTerrainArea2i Area { get; }

    new IReadOnlySet<FuelStation> AssignedFuelStations { get; }
  }
}
