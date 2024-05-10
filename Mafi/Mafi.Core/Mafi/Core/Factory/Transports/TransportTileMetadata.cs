// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportTileMetadata
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct TransportTileMetadata
  {
    public readonly Direction903d StartDirection;
    public readonly Direction903d EndDirection;
    public readonly TransportStartEndType StartType;
    public readonly TransportStartEndType EndType;

    /// <summary>Whether the tile is straight or a turn.</summary>
    public bool IsStraight => this.StartDirection.IsParallelTo(this.EndDirection);

    public bool IsStartFlat => this.StartType == TransportStartEndType.Flat;

    public bool IsEndFlat => this.EndType == TransportStartEndType.Flat;

    public TransportTileMetadata(
      Direction903d startDirection,
      TransportStartEndType startType,
      Direction903d endDirection,
      TransportStartEndType endType)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.StartDirection = startDirection;
      this.StartType = startType;
      this.EndDirection = endDirection;
      this.EndType = endType;
    }

    public TransportPillarAttachmentType GetAttachmentType(out Rotation90 rotation, out bool flipY)
    {
      flipY = !this.IsStraight && this.StartDirection.DirectionVector.Z == 0 && this.EndDirection.DirectionVector.Z == 0 && -this.StartDirection.ToHorizontalOrError() + Rotation90.Deg90 != this.EndDirection.ToHorizontalOrError();
      rotation = this.StartDirection.DirectionVector.Z != 0 ? (!this.StartDirection.DirectionVector.Xy.IsZero || this.EndDirection.DirectionVector.Z != 0 ? Rotation90.Deg0 : (-this.EndDirection).ToHorizontalOrError().ToRotation()) : (-this.StartDirection).ToHorizontalOrError().ToRotation();
      switch (this.StartType)
      {
        case TransportStartEndType.RampDown:
          switch (this.EndType)
          {
            case TransportStartEndType.RampDown:
              return TransportPillarAttachmentType.NoAttachment;
            case TransportStartEndType.Flat:
              if (this.IsStraight)
              {
                rotation += Rotation90.Deg180;
                return TransportPillarAttachmentType.FlatToRampDown_Straight;
              }
              rotation += flipY ? Rotation90.Deg90 : -Rotation90.Deg90;
              flipY = !flipY;
              return TransportPillarAttachmentType.FlatToRampDown_Turn;
            case TransportStartEndType.RampUp:
              return !this.IsStraight ? TransportPillarAttachmentType.RampDownToRampUp_Turn : TransportPillarAttachmentType.NoAttachment;
          }
          break;
        case TransportStartEndType.Flat:
          switch (this.EndType)
          {
            case TransportStartEndType.RampDown:
              return !this.IsStraight ? TransportPillarAttachmentType.FlatToRampDown_Turn : TransportPillarAttachmentType.FlatToRampDown_Straight;
            case TransportStartEndType.Flat:
              return !this.IsStraight ? TransportPillarAttachmentType.FlatToFlat_Turn : TransportPillarAttachmentType.FlatToFlat_Straight;
            case TransportStartEndType.RampUp:
              return !this.IsStraight ? TransportPillarAttachmentType.FlatToRampUp_Turn : TransportPillarAttachmentType.FlatToRampUp_Straight;
            case TransportStartEndType.Vertical:
              return this.EndDirection.DirectionVector.Z >= 0 ? TransportPillarAttachmentType.FlatToVertical : TransportPillarAttachmentType.FlatToVertical_Down;
          }
          break;
        case TransportStartEndType.RampUp:
          switch (this.EndType)
          {
            case TransportStartEndType.RampDown:
              if (this.IsStraight)
                return TransportPillarAttachmentType.NoAttachment;
              rotation += flipY ? Rotation90.Deg90 : -Rotation90.Deg90;
              flipY = !flipY;
              return TransportPillarAttachmentType.RampDownToRampUp_Turn;
            case TransportStartEndType.Flat:
              if (this.IsStraight)
              {
                rotation += Rotation90.Deg180;
                return TransportPillarAttachmentType.FlatToRampUp_Straight;
              }
              rotation += flipY ? Rotation90.Deg90 : -Rotation90.Deg90;
              flipY = !flipY;
              return TransportPillarAttachmentType.FlatToRampUp_Turn;
            case TransportStartEndType.RampUp:
              return TransportPillarAttachmentType.NoAttachment;
          }
          break;
        case TransportStartEndType.Vertical:
          switch (this.EndType)
          {
            case TransportStartEndType.Flat:
              return this.StartDirection.DirectionVector.Z >= 0 ? TransportPillarAttachmentType.FlatToVertical : TransportPillarAttachmentType.FlatToVertical_Down;
            case TransportStartEndType.Vertical:
              return TransportPillarAttachmentType.VerticalToVertical;
          }
          break;
        default:
          Log.Error(string.Format("Invalid start type '{0}'.", (object) this.StartType));
          return TransportPillarAttachmentType.NoAttachment;
      }
      Log.Error(string.Format("Invalid end type '{0}'.", (object) this.EndType));
      return TransportPillarAttachmentType.NoAttachment;
    }
  }
}
