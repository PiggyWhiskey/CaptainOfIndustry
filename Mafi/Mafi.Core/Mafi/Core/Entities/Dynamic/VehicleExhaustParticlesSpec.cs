// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.VehicleExhaustParticlesSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public class VehicleExhaustParticlesSpec
  {
    public readonly ImmutableArray<string> GameObjectPaths;
    public readonly float BaseParticleRate;
    public readonly float ParticlesSpeedRate;
    public readonly float ParticlesAccelerationRate;
    public readonly float BaseEmitSpeed;
    public readonly float VariableEmitSpeed;
    public readonly float MaxRate;
    public readonly float InverseMaxParticleRate;

    public VehicleExhaustParticlesSpec(
      string[] gameObjectPaths,
      float baseParticleRate,
      float particlesSpeedRate,
      float particlesAccelerationRate,
      float baseEmitSpeed,
      float variableEmitSpeed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GameObjectPaths = ((ICollection<string>) gameObjectPaths).ToImmutableArray<string>().CheckNotEmpty<string>();
      this.BaseParticleRate = baseParticleRate.CheckPositive();
      this.ParticlesSpeedRate = particlesSpeedRate.CheckPositive();
      this.ParticlesAccelerationRate = particlesAccelerationRate.CheckPositive();
      this.BaseEmitSpeed = baseEmitSpeed;
      this.VariableEmitSpeed = variableEmitSpeed;
      this.MaxRate = baseParticleRate + this.ParticlesSpeedRate + this.ParticlesAccelerationRate;
      Assert.That<float>(this.MaxRate).IsPositive();
      this.InverseMaxParticleRate = 1f / this.MaxRate;
    }
  }
}
