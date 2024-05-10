// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainResource
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Map;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainResource
  {
    /// <summary>
    /// Name for human identification and debugging. Not displayed to the player.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Approximate center position of this resource generator.
    /// </summary>
    Tile3i Position { get; }

    /// <summary>
    /// Approximate radius of this resource. A circle at <see cref="P:Mafi.Core.Terrain.Generation.ITerrainResource.Position" /> with this radius should contain the
    /// entire resource.
    /// </summary>
    RelTile1i MaxRadius { get; }

    /// <summary>
    /// Priority of generation. Generators will be called in ascending order (low numbers first). Note that this
    /// number is just a sorting hint. Changing this after the initial sort will have no effect.
    /// </summary>
    int Priority { get; }

    /// <summary>Color of the resource. This is for UI.</summary>
    ColorRgba ResourceColor { get; }

    /// <summary>Called once at the end of map construction.</summary>
    void Initialize(IslandMap map, bool isEditorPreview = false);
  }
}
