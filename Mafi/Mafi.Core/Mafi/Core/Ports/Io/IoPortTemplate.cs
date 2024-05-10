// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IoPortTemplate
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static.Layout;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  /// <summary>
  /// Template of an IO port that has relative position and rotation.
  /// </summary>
  public class IoPortTemplate
  {
    public readonly PortSpec Spec;
    /// <summary>
    /// Relative coordinate of this port with respect to the layout origin.
    /// </summary>
    public readonly RelTile3i RelativePosition;
    /// <summary>
    /// Relative direction of this port. A port always points outwards from the parent entity towards potentially
    /// connected port.
    /// </summary>
    public readonly Direction90 RelativeDirection;

    public IoPortShapeProto Shape => this.Spec.Shape;

    public IoPortType Type => this.Spec.Type;

    public char Name => this.Spec.Name;

    /// <summary>
    /// Relative position of potentially connected port or transport pivot.
    /// </summary>
    public RelTile3i RelativePositionOfConnectedPort
    {
      get => this.RelativePosition + this.RelativeDirection.ToTileDirection().ExtendZ(0);
    }

    public IoPortTemplate(PortSpec spec, RelTile3i relativePosition, Direction90 relativeDirection)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Spec = spec;
      this.RelativePosition = relativePosition;
      this.RelativeDirection = relativeDirection;
    }

    public Tile3i GetPosition(EntityLayout entityLayout, TileTransform entityTransform)
    {
      return entityLayout.Transform(this.RelativePosition, entityTransform);
    }

    public Direction903d GetDirection(TileTransform entityTransform)
    {
      return entityTransform.Transform(this.RelativeDirection).As3d;
    }

    public Tile3i GetExpectedConnectedPortPosition(
      EntityLayout entityLayout,
      TileTransform entityTransform)
    {
      return this.GetPosition(entityLayout, entityTransform) + new RelTile3i(this.GetDirection(entityTransform).DirectionVector);
    }
  }
}
