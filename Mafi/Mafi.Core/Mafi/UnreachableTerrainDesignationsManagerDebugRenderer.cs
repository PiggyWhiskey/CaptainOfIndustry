// Decompiled with JetBrains decompiler
// Type: Mafi.UnreachableTerrainDesignationsManagerDebugRenderer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Terrain.Designation;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class UnreachableTerrainDesignationsManagerDebugRenderer
  {
    public static DebugGameMapDrawing DrawUnreachableDesignationsFor(
      this DebugGameMapDrawing drawing,
      Vehicle vehicle)
    {
      UnreachableTerrainDesignationsManager dep;
      if (drawing.IsNotEnabled || drawing.Resolver.IsNone || !drawing.Resolver.Value.TryGetResolvedDependency<UnreachableTerrainDesignationsManager>(out dep))
        return drawing;
      drawing.DrawDesignations((IEnumerable<IDesignation>) dep.GetUnreachableDesignationsFor((IPathFindingVehicle) vehicle), ColorRgba.Red);
      return drawing;
    }
  }
}
