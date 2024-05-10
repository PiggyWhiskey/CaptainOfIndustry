// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IResourceGeneratorFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [MultiDependency]
  public interface IResourceGeneratorFactory
  {
    /// <summary>
    /// Name of the factory for easier identification in development.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Priority for the generator. Generators will be called in ascending order (low numbers first). Order of
    /// generators with the same <see cref="P:Mafi.Core.Terrain.Generation.IResourceGeneratorFactory.Priority" /> is undefined.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Whether to generate this resource close to starting location.
    /// </summary>
    bool GenerateNearStartLocation { get; }

    bool AllowOnStartingCell { get; }
  }
}
