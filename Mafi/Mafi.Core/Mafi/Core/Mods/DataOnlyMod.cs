// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.DataOnlyMod
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Mods
{
  /// <summary>
  /// Mod that can only register game data using proto registrator. No mutation of dependencies or save/load state
  /// is possible.
  /// </summary>
  public abstract class DataOnlyMod : IMod
  {
    public abstract string Name { get; }

    public abstract int Version { get; }

    public bool IsUiOnly => false;

    public virtual Option<IConfig> ModConfig { get; set; }

    public abstract void RegisterPrototypes(ProtoRegistrator registrator);

    public void RegisterDependencies(
      DependencyResolverBuilder depBuilder,
      ProtosDb protosDb,
      bool gameWasLoaded)
    {
    }

    public virtual void EarlyInit(DependencyResolver resolver)
    {
    }

    public void Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
    }

    protected DataOnlyMod()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
