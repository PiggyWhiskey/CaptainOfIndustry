// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.IIslandMapGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Map
{
  public interface IIslandMapGenerator
  {
    /// <summary>Name of the generator.</summary>
    string Name { get; }

    /// <summary>
    /// Returns an enumerator for time-sliced map generation. Enumerator returns string that describes what was
    /// performed in that step.
    /// </summary>
    IEnumerator<string> GenerateIslandMapTimeSliced();

    /// <summary>
    /// Returns generated map and clears any temporary data used during generation.
    /// This method should be called only after an iterator from <see cref="M:Mafi.Core.Map.IIslandMapGenerator.GenerateIslandMapTimeSliced" /> finished.
    /// </summary>
    IslandMap GetMapAndClear();
  }
}
