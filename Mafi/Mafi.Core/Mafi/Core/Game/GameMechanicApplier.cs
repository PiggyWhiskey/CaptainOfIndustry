// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.GameMechanicApplier
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Game
{
  public class GameMechanicApplier
  {
    private readonly Action<GameDifficultyConfig> m_apply;
    public ImmutableArray<GameDifficultyPreset> RecommendedFor;
    private readonly Lyst<GameMechanicApplier> m_dependencies;
    private readonly Lyst<GameMechanicApplier> m_conflicts;

    public LocStrFormatted Title { get; }

    public ImmutableArray<LocStrFormatted> Items { get; }

    public IIndexable<GameMechanicApplier> Dependencies
    {
      get => (IIndexable<GameMechanicApplier>) this.m_dependencies;
    }

    public IIndexable<GameMechanicApplier> Conflicts
    {
      get => (IIndexable<GameMechanicApplier>) this.m_conflicts;
    }

    public string IconPath { get; private set; }

    public GameMechanicApplier(
      LocStrFormatted title,
      ImmutableArray<LocStrFormatted> items,
      Action<GameDifficultyConfig> apply,
      GameDifficultyPreset[] recommendedFor = null,
      string iconPath = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_dependencies = new Lyst<GameMechanicApplier>();
      this.m_conflicts = new Lyst<GameMechanicApplier>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Title = title;
      this.Items = items;
      this.m_apply = apply;
      this.RecommendedFor = recommendedFor != null ? ((ICollection<GameDifficultyPreset>) recommendedFor).ToImmutableArray<GameDifficultyPreset>() : (ImmutableArray<GameDifficultyPreset>) ImmutableArray.Empty;
      this.IconPath = iconPath;
    }

    public GameMechanicApplier AddDependency(GameMechanicApplier other)
    {
      this.m_dependencies.AddIfNotPresent(other);
      return this;
    }

    public GameMechanicApplier AddConflict(GameMechanicApplier other)
    {
      this.m_conflicts.AddIfNotPresent(other);
      return this;
    }

    public void ApplyTo(GameDifficultyConfig config) => this.m_apply(config);
  }
}
