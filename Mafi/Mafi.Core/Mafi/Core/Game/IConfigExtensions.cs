// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.IConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Game
{
  public static class IConfigExtensions
  {
    public static bool TryGetConfig<T>(this ImmutableArray<IConfig> configs, out T config) where T : IConfig
    {
      foreach (IConfig config1 in configs)
      {
        if (config1 is T obj)
        {
          config = obj;
          return true;
        }
      }
      config = default (T);
      return false;
    }
  }
}
