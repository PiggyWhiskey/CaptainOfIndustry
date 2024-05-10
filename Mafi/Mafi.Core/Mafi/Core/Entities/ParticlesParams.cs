// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.ParticlesParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Recipes;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Parameters for particles effects. Currently used for machines only.
  /// </summary>
  /// <remarks>
  /// If you add particle to a prefab but don't put this configuration for it it won't be played unless it is a child
  /// of a particle system that has such configuration.
  /// </remarks>
  public class ParticlesParams
  {
    /// <summary>
    /// GameObject id of a particle system for which this configuration is.
    /// </summary>
    public readonly string SystemId;
    /// <summary>
    /// Delay from the point when machine starts to work before playing the particles.
    /// </summary>
    public readonly Duration Delay;
    /// <summary>Duration of the particle system play time.</summary>
    public readonly Duration Duration;
    /// <summary>
    /// Whether the alpha color should be updated to reflect the utilization.
    /// </summary>
    public readonly bool UseUtilizationOnAlpha;
    /// <summary>
    /// All recipes supported by this particle. If none than all recipes are supported.
    /// </summary>
    public readonly Option<Func<RecipeProto, bool>> SupportedRecipesSelector;
    public readonly Option<Func<RecipeProto, ColorRgba>> ColorSelector;

    /// <summary>
    /// Particles will be played in loop all the time. They will be stopped only when the machine is stopped.
    /// </summary>
    public static ParticlesParams Loop(
      string systemId,
      bool useUtilizationOnAlpha = false,
      Func<RecipeProto, bool> recipesSelector = null,
      Func<RecipeProto, ColorRgba> colorSelector = null)
    {
      return new ParticlesParams(systemId, Duration.Zero, Duration.Zero, useUtilizationOnAlpha, (Option<Func<RecipeProto, bool>>) recipesSelector, (Option<Func<RecipeProto, ColorRgba>>) colorSelector);
    }

    /// <summary>Played once per machine run (one run over recipe).</summary>
    public static ParticlesParams Timed(
      string systemId,
      Duration delay,
      Duration duration,
      bool useUtilizationOnAlpha = false,
      Func<RecipeProto, bool> recipesSelector = null,
      Func<RecipeProto, ColorRgba> colorSelector = null)
    {
      return new ParticlesParams(systemId, delay, duration, useUtilizationOnAlpha, (Option<Func<RecipeProto, bool>>) recipesSelector, (Option<Func<RecipeProto, ColorRgba>>) colorSelector);
    }

    private ParticlesParams(
      string id,
      Duration delay,
      Duration duration,
      bool useUtilizationOnAlpha,
      Option<Func<RecipeProto, bool>> recipesSelector,
      Option<Func<RecipeProto, ColorRgba>> colorSelector)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SystemId = id.CheckNotNullOrEmpty();
      this.Delay = delay.CheckNotNegative();
      this.Duration = duration.CheckNotNegative();
      this.UseUtilizationOnAlpha = useUtilizationOnAlpha;
      this.SupportedRecipesSelector = recipesSelector;
      this.ColorSelector = colorSelector;
    }
  }
}
