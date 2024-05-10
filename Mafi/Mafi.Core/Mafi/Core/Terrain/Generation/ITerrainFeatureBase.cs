// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainFeatureBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainFeatureBase
  {
    /// <summary>
    /// Can be changed in editor and can help with map organization.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Mainly for editor usage to identify the same features between different map snapshots.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Disabled features will be ignored by map generator but will be still present in the map save.
    /// </summary>
    bool IsDisabled { get; set; }

    /// <summary>
    /// Whether this feature type can be in the map only once. This is based on the actual feature <see cref="T:System.Type" />,
    /// base classes are not considered.
    /// </summary>
    bool IsUnique { get; }

    /// <summary>
    /// Whether map import should include this feature. Generally, global or unique features should not be importable.
    /// </summary>
    bool IsImportable { get; }

    /// <summary>
    /// Affected area. If null is returned, the entire map is affected.
    /// </summary>
    RectangleTerrainArea2i? GetBoundingBox();

    /// <summary>
    /// Called before the map is generated. Any global pre-computation can be done here.
    /// Features can also register custom classes that can store extra data. This is currently used for trees
    /// and props. If false is returned, this feature will not be part of the map generation.
    /// </summary>
    bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg);

    /// <summary>
    /// Frees all resources after map generation is done. This does not clear caches.
    /// </summary>
    void Reset();

    /// <summary>Clears any caches.</summary>
    void ClearCaches();

    /// <summary>Moves the feature by the given amount.</summary>
    void TranslateBy(RelTile3f delta);

    /// <summary>
    /// Rotates the feature around a vertical axis by the given amount.
    /// </summary>
    void RotateBy(AngleDegrees1f rotation);
  }
}
