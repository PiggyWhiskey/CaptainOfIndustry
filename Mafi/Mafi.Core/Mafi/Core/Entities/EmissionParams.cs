// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EmissionParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Parameters for emissions. Currently used for machines only.
  /// </summary>
  public class EmissionParams
  {
    /// <summary>
    /// GameObject ids to apply emissions on.
    /// Note: Only used in non-instanced mode.
    /// </summary>
    public readonly ImmutableArray<string> GameObjectsIds;
    /// <summary>
    /// Material to apply emissions on.
    /// Note: Only used in instanced mode.
    /// </summary>
    public readonly string MaterialName;
    /// <summary>
    /// Delay from the point when machine starts to work before starting the emission.
    /// </summary>
    public readonly Duration Delay;
    /// <summary>Duration of the emission.</summary>
    public readonly Duration Duration;
    /// <summary>Intensity of the emission.</summary>
    public readonly float Intensity;
    /// <summary>When turning on, how big increments to use per frame.</summary>
    public readonly float DiffToOn;
    /// <summary>
    /// When turning off, how big decrements to use per frame. Keep positive!
    /// </summary>
    public readonly float DiffToOff;
    /// <summary>Color of emission.</summary>
    public readonly ColorRgba Color;

    /// <summary>Emission enabled all the time the machine runs.</summary>
    public static EmissionParams AllTime(
      ImmutableArray<string> gameObjectsIds,
      string materialName,
      float intensity,
      float diffToOn = 1f,
      float diffToOff = 1f,
      ColorRgba? color = null)
    {
      return new EmissionParams(gameObjectsIds, materialName, Duration.Zero, Duration.Zero, intensity, diffToOn, diffToOff, color);
    }

    /// <summary>Played once per machine run (one run over recipe).</summary>
    public static EmissionParams Timed(
      ImmutableArray<string> gameObjectsIds,
      string materialName,
      Duration delay,
      Duration duration,
      float intensity,
      float diffToOn = 1f,
      float diffToOff = 1f,
      ColorRgba? color = null)
    {
      return new EmissionParams(gameObjectsIds, materialName, delay, duration, intensity, diffToOn, diffToOff, color);
    }

    private EmissionParams(
      ImmutableArray<string> gameObjectsIds,
      string materialName,
      Duration delay,
      Duration duration,
      float intensity,
      float diffToOn,
      float diffToOff,
      ColorRgba? color)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GameObjectsIds = gameObjectsIds;
      this.MaterialName = materialName;
      this.Delay = delay.CheckNotNegative();
      this.Duration = duration.CheckNotNegative();
      this.Intensity = intensity;
      this.DiffToOn = diffToOn.CheckPositive();
      this.DiffToOff = diffToOff.CheckPositive();
      this.Color = color ?? ColorRgba.White;
    }
  }
}
