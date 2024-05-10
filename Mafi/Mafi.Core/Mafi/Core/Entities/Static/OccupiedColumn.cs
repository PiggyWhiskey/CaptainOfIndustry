// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.OccupiedColumn
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public readonly struct OccupiedColumn
  {
    public readonly int From;
    public readonly int ToExcl;
    public readonly int Scale;
    public readonly int IdForXySorting;

    public OccupiedColumn(int from, int toExcl, int scale, int idForXySorting)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.From = from;
      this.ToExcl = toExcl;
      this.Scale = scale;
      this.IdForXySorting = idForXySorting;
    }
  }
}
