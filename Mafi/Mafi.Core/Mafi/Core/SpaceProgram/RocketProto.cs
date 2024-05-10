// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.RocketProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.SpaceProgram
{
  public class RocketProto : TransportedRocketBaseProto
  {
    public readonly Quantity CargoCapacity;
    public readonly ProductQuantity LaunchFuel;
    public readonly int LaunchExp;
    private readonly Fix32 m_midExp;
    private readonly Fix32 m_expScale;
    public readonly RelTile1f AccelerationPerTick;
    public readonly Duration TotalFlightTime;
    /// <summary>
    /// Height offset of the rocket so that the rocket bottom is 2 tiles above the ground.
    /// </summary>
    public readonly RelTile1f GroundOffset;
    public readonly RocketProto.Gfx Graphics;

    public override Type EntityType => typeof (RocketEntity);

    public RocketProto(
      EntityProto.ID id,
      Proto.Str strings,
      EntityCosts costs,
      Duration durationToBuild,
      Quantity cargoCapacity,
      ProductQuantity launchFuel,
      int launchExp,
      Fix32 midExp,
      Fix32 expScale,
      RelTile1f accelerationPerTick,
      Duration totalFlightTime,
      RelTile1f groundOffset,
      RocketProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, costs, durationToBuild, (EntityProto.Gfx) graphics);
      this.CargoCapacity = cargoCapacity;
      this.LaunchFuel = launchFuel;
      this.LaunchExp = launchExp;
      this.m_midExp = midExp;
      this.m_expScale = expScale;
      this.AccelerationPerTick = accelerationPerTick;
      this.TotalFlightTime = totalFlightTime;
      this.Graphics = graphics;
      this.GroundOffset = groundOffset;
    }

    public Percent GetLaunchSuccessChance(int currentExp)
    {
      return Percent.FromRatio((Fix32) 1, (Fix32) 1 + Fix32.Exp(-((Fix32) currentExp - this.m_midExp) / this.m_expScale));
    }

    public new class Gfx : EntityProto.Gfx
    {
      public readonly string PrefabPath;
      public readonly string SoundPrefabPath;

      public Gfx(string prefabPath, string soundPrefabPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(ColorRgba.Empty);
        this.PrefabPath = prefabPath;
        this.SoundPrefabPath = soundPrefabPath;
      }
    }
  }
}
