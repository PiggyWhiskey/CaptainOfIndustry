// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DynamicEntityDustParticlesSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public class DynamicEntityDustParticlesSpec : DustParticlesSpec
  {
    public readonly float ParticlesPerSpeedMult;
    public readonly RelTile1f MinSpeed;
    public readonly float ParticlesPerDegreeMult;

    public DynamicEntityDustParticlesSpec(
      string prefabPath,
      float dustScale,
      RelTile3f relativePosition,
      float particlesPerSpeedMult,
      RelTile1f minSpeed,
      float particlesPerDegreeMult = 0.0f)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(prefabPath, dustScale, relativePosition);
      this.ParticlesPerSpeedMult = particlesPerSpeedMult;
      this.MinSpeed = minSpeed;
      this.ParticlesPerDegreeMult = particlesPerDegreeMult;
    }
  }
}
