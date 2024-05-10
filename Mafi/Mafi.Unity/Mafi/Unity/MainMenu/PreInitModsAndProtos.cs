// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.PreInitModsAndProtos
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  public class PreInitModsAndProtos
  {
    public PreInitModsAndProtos(ImmutableArray<IMod> loadedMods, ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.LoadedMods = loadedMods;
      this.ProtosDb = protosDb;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public ImmutableArray<IMod> LoadedMods { get; }

    public ProtosDb ProtosDb { get; }
  }
}
