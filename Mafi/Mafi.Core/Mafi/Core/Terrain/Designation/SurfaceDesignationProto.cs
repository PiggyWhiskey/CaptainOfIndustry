// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.SurfaceDesignationProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  public class SurfaceDesignationProto : Proto
  {
    public readonly bool IsPlacing;

    public SurfaceDesignationProto(Proto.ID id, bool isPlacing)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.IsPlacing = isPlacing;
    }
  }
}
