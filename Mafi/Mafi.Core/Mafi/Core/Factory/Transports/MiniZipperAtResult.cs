// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.MiniZipperAtResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct MiniZipperAtResult
  {
    public readonly MiniZipperProto ZipperProto;
    public readonly Tile3i Position;

    public bool IsValid => (Proto) this.ZipperProto != (Proto) null;

    public MiniZipperAtResult(MiniZipperProto zipperProto, Tile3i position)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ZipperProto = zipperProto;
      this.Position = position;
    }
  }
}
