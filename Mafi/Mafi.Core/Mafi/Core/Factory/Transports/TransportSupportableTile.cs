// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportSupportableTile
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct TransportSupportableTile
  {
    public readonly Tile3i Position;
    public readonly int OccupiedTileIndex;
    public readonly TransportPillarAttachmentType PillarAttachmentType;
    public readonly Rotation90 AttachmentRotation;
    public readonly bool AttachmentFlipY;

    public TransportSupportableTile(
      Tile3i position,
      int occupiedTileIndex,
      TransportPillarAttachmentType pillarAttachmentType,
      Rotation90 attachmentRotation,
      bool attachmentFlipY)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.OccupiedTileIndex = occupiedTileIndex;
      this.PillarAttachmentType = pillarAttachmentType;
      this.AttachmentRotation = attachmentRotation;
      this.AttachmentFlipY = attachmentFlipY;
    }
  }
}
