// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DrivingData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  /// <summary>
  /// Immutable class which stores data for the driving engine.
  /// </summary>
  public sealed class DrivingData
  {
    /// <summary>Maximum forwards speed in discrete units per tick.</summary>
    public readonly RelTile1f MaxForwardsSpeed;
    /// <summary>Maximum backwards speed in discrete units per tick.</summary>
    public readonly RelTile1f MaxBackwardsSpeed;
    /// <summary>
    /// Multiplier of speed at the max steering angle, is in range [0.1, 1].
    /// </summary>
    public readonly Percent SteeringSpeedMult;
    /// <summary>Forward acceleration in discrete units per tick.</summary>
    public readonly RelTile1f Acceleration;
    /// <summary>
    /// Deceleration (breaking) in discrete units per tick. This should be generally more than acceleration.
    /// </summary>
    public readonly RelTile1f Braking;
    /// <summary>
    /// Max steering angle of the wheels. Value is in range [0, 80] degrees.
    /// </summary>
    public readonly AngleDegrees1f MaxSteeringAngle;
    /// <summary>Maximal change of steering angle per tick.</summary>
    public readonly AngleDegrees1f MaxSteeringSpeed;
    /// <summary>
    /// How conservative breaking should be, values around 2.0 are good, higher values causes more conservative
    /// driving.
    /// </summary>
    public readonly Fix32 BrakingConservativness;
    /// <summary>
    /// Distance from vehicle center to steering axle. This determines turning radius. If the offset is zero then the
    /// vehicle can turn in place.
    /// </summary>
    /// <remarks>
    /// <code>
    ///  O==+==O  -
    ///     |     |
    ///     |     | SteeringAxleOffset
    ///     |     |
    ///     #     - (center)
    ///  O==+==O
    ///  O==+==O
    ///  O==+==O
    /// </code>
    /// </remarks>
    public readonly RelTile1f SteeringAxleOffset;
    /// <summary>
    /// Distance from vehicle center to the non-steering axle. This determines turning radius.
    /// </summary>
    /// <remarks>
    /// <code>
    ///  O==+==O
    ///     |
    ///     |
    ///     |
    ///     #     - (center)
    ///  O==+==O  | NonSteeringAxleOffset
    ///  O==+==O  -
    ///  O==+==O
    /// </code>
    /// </remarks>
    public readonly RelTile1f NonSteeringAxleOffset;
    /// <summary>
    /// Whether steering is independent of vehicle speed. For example tracks can turn in pace, wheels cannot.
    /// </summary>
    public readonly bool CanTurnInPlace;

    /// <summary>Distance between steering and non steering axles.</summary>
    public RelTile1f AxlesDistance => this.SteeringAxleOffset + this.NonSteeringAxleOffset;

    public DrivingData(
      RelTile1f maxForwardsSpeed,
      RelTile1f maxBackwardsSpeed,
      Percent steeringSpeedMult,
      RelTile1f acceleration,
      RelTile1f breaking,
      AngleDegrees1f maxSteeringAngle,
      AngleDegrees1f maxSteeringSpeed,
      Fix32 breakingConservativness,
      RelTile1f steeringAxleOffset,
      RelTile1f nonSteeringAxleOffset)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaxForwardsSpeed = maxForwardsSpeed.CheckPositive();
      this.MaxBackwardsSpeed = maxBackwardsSpeed.CheckWithinIncl(0.01.Tiles(), maxForwardsSpeed);
      this.SteeringSpeedMult = steeringSpeedMult.CheckWithinIncl(10.Percent(), 100.Percent());
      this.Acceleration = acceleration.CheckPositive();
      this.Braking = breaking.CheckPositive();
      this.MaxSteeringAngle = maxSteeringAngle.CheckWithinIncl(AngleDegrees1f.Zero, 60.Degrees());
      this.MaxSteeringSpeed = maxSteeringSpeed.CheckPositive();
      this.BrakingConservativness = breakingConservativness.CheckWithinIncl((Fix32) 1, (Fix32) 10);
      this.SteeringAxleOffset = steeringAxleOffset.CheckNotNegative();
      this.NonSteeringAxleOffset = nonSteeringAxleOffset.CheckNotNegative();
      this.CanTurnInPlace = this.AxlesDistance.IsZero;
    }
  }
}
