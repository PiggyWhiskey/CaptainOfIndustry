// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IWorldRegionMapFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface IWorldRegionMapFactory
  {
    /// <summary>
    /// Returns an enumerator for time-sliced world region generation. Enumerator returns progress.
    /// </summary>
    IEnumerator<Percent> GenerateIslandMapTimeSliced(Option<object> mapConfig);

    /// <summary>
    /// Returns generated map and clears any temporary data used during generation.
    /// This method should be called only after an iterator from <see cref="M:Mafi.Core.Terrain.Generation.IWorldRegionMapFactory.GenerateIslandMapTimeSliced(Mafi.Option{System.Object})" /> finished.
    /// </summary>
    IWorldRegionMap GetMapAndClear(out IWorldRegionMapPreviewData previewData);
  }
}
