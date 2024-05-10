// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.IModWithMaps
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Mods
{
  public interface IModWithMaps : IMod
  {
    /// <summary>
    /// Called when "New game" dialog opens. It is advised to store some info about the loaded data so that queries
    /// from <see cref="M:Mafi.Core.Mods.IModWithMaps.TryGetMapData(Mafi.Core.Terrain.Generation.IWorldRegionMapPreviewData,Mafi.Core.IFileSystemHelper,Mafi.Core.Prototypes.ProtosDb,Mafi.Core.Terrain.Generation.IWorldRegionMapAdditionalData@,Mafi.Core.Terrain.Generation.WorldRegionMapFactoryConfig@)" /> can be correctly processed.
    /// </summary>
    /// <remarks>
    /// If the map preview generation takes long (more than a few ms), it is recommended to implement this method
    /// using enumerator pattern and yield return previews are they are being generated. This will prevent the game
    /// lagging. TODO: Implement this.
    /// </remarks>
    IEnumerable<IWorldRegionMapPreviewData> GetMapPreviews(
      IFileSystemHelper fsHelper,
      ProtosDb protosDb,
      bool includeWip);

    /// <summary>Called when a map from this mod is selected.</summary>
    bool TryGetMapData(
      IWorldRegionMapPreviewData preview,
      IFileSystemHelper fsHelper,
      ProtosDb protosDb,
      out IWorldRegionMapAdditionalData fullData,
      out WorldRegionMapFactoryConfig factoryConfig);

    /// <summary>
    /// Called when new game creation is done or cancelled. This should clear any temporary data that were needed
    /// by <see cref="M:Mafi.Core.Mods.IModWithMaps.GetMapPreviews(Mafi.Core.IFileSystemHelper,Mafi.Core.Prototypes.ProtosDb,System.Boolean)" /> or <see cref="M:Mafi.Core.Mods.IModWithMaps.TryGetMapData(Mafi.Core.Terrain.Generation.IWorldRegionMapPreviewData,Mafi.Core.IFileSystemHelper,Mafi.Core.Prototypes.ProtosDb,Mafi.Core.Terrain.Generation.IWorldRegionMapAdditionalData@,Mafi.Core.Terrain.Generation.WorldRegionMapFactoryConfig@)" />.
    /// </summary>
    void ClearMapData();
  }
}
