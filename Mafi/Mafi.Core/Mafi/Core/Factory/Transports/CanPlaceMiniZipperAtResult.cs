// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.CanPlaceMiniZipperAtResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Zippers;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// A successful result of cutting-out transport and placing mini-zipper in the cot-out tile.
  /// </summary>
  public readonly struct CanPlaceMiniZipperAtResult
  {
    public readonly CanCutOutTransportAtResult CutOutResult;
    public readonly MiniZipperProto ZipperProto;

    public CanPlaceMiniZipperAtResult(
      CanCutOutTransportAtResult cutOutResult,
      MiniZipperProto zipperProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.CutOutResult = cutOutResult;
      this.ZipperProto = zipperProto;
    }
  }
}
