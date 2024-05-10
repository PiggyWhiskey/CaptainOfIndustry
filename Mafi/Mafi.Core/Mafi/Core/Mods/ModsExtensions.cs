// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.ModsExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;

#nullable disable
namespace Mafi.Core.Mods
{
  public static class ModsExtensions
  {
    public static ImmutableArray<ModData> FilterCoreAndDlcMods(this ImmutableArray<ModData> allMods)
    {
      return allMods.Filter((Predicate<ModData>) (x => x.Group == ModGroup.Core || x.Group == ModGroup.Dlc));
    }

    public static ImmutableArray<ModData> FilterCoreMods(this ImmutableArray<ModData> allMods)
    {
      return allMods.Filter((Predicate<ModData>) (x => x.Group == ModGroup.Core));
    }

    public static ImmutableArray<ModData> FilterDlcAndThirdPartyMods(
      this ImmutableArray<ModData> allMods)
    {
      return allMods.Filter((Predicate<ModData>) (x => x.Group == ModGroup.Dlc || x.Group == ModGroup.ThirdParty));
    }

    public static ImmutableArray<ModData> FilterThirdPartyMods(this ImmutableArray<ModData> allMods)
    {
      return allMods.Filter((Predicate<ModData>) (x => x.Group == ModGroup.ThirdParty));
    }

    public static ImmutableArray<ModData> FilterAllLoadedMods(this ImmutableArray<ModData> allMods)
    {
      return allMods.Filter((Predicate<ModData>) (x => x.IsFullyLoaded));
    }

    /// <summary>Concat + safe verification to avoid duplicities</summary>
    public static ImmutableArray<ModData> ConcatSafe(
      this ImmutableArray<ModData> first,
      ImmutableArray<ModData> second)
    {
      Lyst<ModData> merged = new Lyst<ModData>();
      foreach (ModData data in first)
        add(data);
      foreach (ModData data in second)
        add(data);
      return merged.ToImmutableArray();

      void add(ModData data)
      {
        if (merged.Contains(data) || merged.Any<ModData>((Predicate<ModData>) (x => x.ModType == data.ModType)))
          Log.Error("Mod " + data.Name + " was already added!");
        else
          merged.Add(data);
      }
    }
  }
}
