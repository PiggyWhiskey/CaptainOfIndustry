// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Data.SurfaceDesignationsData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Terrain.Designation;

#nullable disable
namespace Mafi.Core.Data
{
  internal sealed class SurfaceDesignationsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<SurfaceDesignationProto>(new SurfaceDesignationProto(IdsCore.TerrainDesignators.PlaceSurfaceDesignator, true));
      registrator.PrototypesDb.Add<SurfaceDesignationProto>(new SurfaceDesignationProto(IdsCore.TerrainDesignators.ClearSurfaceDesignator, false));
    }

    public SurfaceDesignationsData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
