// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.LoadSave.LoadedModsStatus
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.LoadSave
{
  internal readonly struct LoadedModsStatus
  {
    public readonly ImmutableArray<SaveModStatus> ThirdParty;

    public LoadedModsStatus(ImmutableArray<SaveModStatus> thirdParty)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.ThirdParty = thirdParty;
    }

    public ImmutableArray<ModData> GetSelectedAvailableThirdPartyMods()
    {
      return this.ThirdParty.Filter((Predicate<SaveModStatus>) (x => x.IsAvailable && x.IsSelected)).Map<ModData>((Func<SaveModStatus, ModData>) (x => x.ModData.Value));
    }

    public bool IsAnyUnavailableModSelected()
    {
      return this.ThirdParty.Any((Func<SaveModStatus, bool>) (m => m.IsSelected && !m.IsAvailable));
    }

    public bool HasAnythingToConfigure() => this.ThirdParty.Length > 0;

    public bool HasAnythingSelected()
    {
      return this.ThirdParty.Any((Func<SaveModStatus, bool>) (m => m.IsSelected));
    }
  }
}
