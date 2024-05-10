// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IIoPortsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Ports.Io
{
  public interface IIoPortsManager
  {
    bool TryGetPortAt(
      Tile3i position,
      Direction903d direction,
      IoPortShapeProto portShape,
      out IoPort port);

    bool TryGetPortAt(Tile3i position, Direction903d direction, out IoPort port);

    bool IsAnyPortFacingTo(Tile3i tile);

    void DisconnectAndRemove(IoPort port);

    void Disconnect(IoPort port);

    void AddPortAndTryConnect(IoPort port);

    void OnPortConnectionChanged(IoPort port, IoPort otherPort);
  }
}
