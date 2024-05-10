﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RotatingCabinDriverProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public class RotatingCabinDriverProto
  {
    public readonly AngleDegrees1f MaxSpeedPerTick;
    public readonly AngleDegrees1f MaxAccelerationPerTick;
    public readonly AngleDegrees1f MaxBrakingPerTick;
    public readonly Fix32 BrakingConservativeness;

    public RotatingCabinDriverProto(
      AngleDegrees1f maxSpeedPerTick,
      AngleDegrees1f maxAccelerationPerTick,
      AngleDegrees1f maxBrakingPerTick,
      Fix32 brakingConservativeness)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaxSpeedPerTick = maxSpeedPerTick;
      this.MaxAccelerationPerTick = maxAccelerationPerTick;
      this.MaxBrakingPerTick = maxBrakingPerTick;
      this.BrakingConservativeness = brakingConservativeness;
    }
  }
}
