// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Surfaces.SurfaceDecalCategoryProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Terrain.Surfaces
{
  [DebuggerDisplay("SurfaceDecalCategoryProto: {Id}")]
  public class SurfaceDecalCategoryProto : Proto
  {
    public readonly float Order;

    public SurfaceDecalCategoryProto(Proto.ID id, Proto.Str strings, float order)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Order = order;
    }
  }
}
