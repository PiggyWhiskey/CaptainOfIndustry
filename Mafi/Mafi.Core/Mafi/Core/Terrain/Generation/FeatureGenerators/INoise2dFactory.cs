// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.FeatureGenerators.INoise2dFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Random.Noise;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation.FeatureGenerators
{
  public interface INoise2dFactory
  {
    bool TryCreateNoise(
      IResolver resolver,
      IReadOnlyDictionary<string, object> extraArgs,
      out INoise2D result,
      out string error);
  }
}
