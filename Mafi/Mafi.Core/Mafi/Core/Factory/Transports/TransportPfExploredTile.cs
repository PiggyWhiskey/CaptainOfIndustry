// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPfExploredTile
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct TransportPfExploredTile
  {
    public readonly Tile3i Position;
    public readonly Tile3i ParentPosition;
    public readonly bool IsProcessed;
    public readonly byte PathLengthSteps;

    public TransportPfExploredTile(
      Tile3i position,
      Tile3i parentPosition,
      bool isProcessed,
      byte pathLengthSteps)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.ParentPosition = parentPosition;
      this.IsProcessed = isProcessed;
      this.PathLengthSteps = pathLengthSteps;
    }
  }
}
