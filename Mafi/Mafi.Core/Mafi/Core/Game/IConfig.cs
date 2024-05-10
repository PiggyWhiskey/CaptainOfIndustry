// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.IConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Game
{
  /// <summary>
  /// Classes marked with this interface are global configs that are used to configure the game.
  /// 
  /// Configs marked with <c>[GlobalDependency]</c> attribute will be instantiated automatically (if not provided)
  /// via resolver and may take other configs in the constructor. However, no other dependencies are allowed.
  /// 
  /// Configs with <c>Serialize</c> method (or the ones marked with <c>[GenerateSerializer]</c> attribute) will be saved,
  /// and will be persisted. Serializable configs marked with <c>[GlobalDependency]</c> will loaded from the save file.
  /// 
  /// Configs that are not serializable or not global dependency will be only present in the first game launch and then
  /// will be discarded (for example map generation configs that are no longer needed when the map is generated).
  /// </summary>
  /// <remarks>
  /// Note that configs are handled in a special way, different from other global dependencies. See
  /// <see cref="T:Mafi.Core.Game.GameBuilder" /> for more details.
  /// </remarks>
  [MultiDependency]
  public interface IConfig
  {
  }
}
