// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.PillarLayerSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct PillarLayerSpec
  {
    public const uint BEAMS_MASK = 1;
    public const uint FILL_PLUS_X_MASK = 2;
    public const uint FILL_PLUS_Y_MASK = 4;
    public const uint FILL_MINUS_X_MASK = 8;
    public const uint FILL_MINUS_Y_MASK = 16;
    public const uint FLIP_Y_MASK = 32;
    public const uint ALL_FILLS_MASK = 30;
    public const uint BEAMS_AND_ALL_FILLS_MASK = 31;
    public readonly Option<TransportProto> AttachedTransport;
    public readonly TransportPillarAttachmentType AttachmentType;
    public readonly Rotation90 AttachmentRotation;
    public readonly byte Flags;

    public bool HasBeams => ((uint) this.Flags & 1U) > 0U;

    public bool HasBeamsAndAllBraces => ((int) this.Flags & 31) == 31;

    public bool HasAnyFill => ((uint) this.Flags & 30U) > 0U;

    public bool HasFillPlusX => ((uint) this.Flags & 2U) > 0U;

    public bool HasFillPlusY => ((uint) this.Flags & 4U) > 0U;

    public bool HasFillMinusX => ((uint) this.Flags & 8U) > 0U;

    public bool HasFillMinusY => ((uint) this.Flags & 16U) > 0U;

    public bool AttachmentFlipY => ((uint) this.Flags & 32U) > 0U;

    public PillarLayerSpec(
      Option<TransportProto> attachedTransport,
      TransportPillarAttachmentType attachmentType,
      Rotation90 attachmentRotation,
      byte flags)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.AttachedTransport = attachedTransport;
      this.AttachmentType = attachmentType;
      this.AttachmentRotation = attachmentRotation;
      this.Flags = flags;
    }
  }
}
