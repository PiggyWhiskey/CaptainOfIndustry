// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.IRegistrationMod`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Mods
{
  /// <summary>
  /// This interface marks a mod as registration service for another group of mods. This can effectively extend mod
  /// registrations system for new capabilities.
  /// 
  /// This mechanism is used by the game itself to extend Unity UI without Core knowing about any UI.
  /// 
  /// To use this interface, a mod has to define new interface that extends <see cref="T:Mafi.Core.Mods.IMod" />, for example
  /// <c>IMySuperMod</c> and then define new methods on this interface. All mods implementing the new
  /// <c>IMySuperMod</c> have to implement all new methods too.
  /// 
  /// Now, an existing mod can derive <see cref="T:Mafi.Core.Mods.IRegistrationMod`1" /> with <c>TMod</c> set to the
  /// <c>IMySuperMod</c>. This mod has now authority to register all instances of <c>IMySuperMod</c> with <see cref="M:Mafi.Core.Mods.IRegistrationMod`1.Register(Mafi.Collections.ImmutableCollections.ImmutableArray{`0},Mafi.Core.Mods.RegistrationContext)" /> function.
  /// </summary>
  public interface IRegistrationMod<TMod> : IMod where TMod : IMod
  {
    /// <summary>
    /// Given an array of all mods of requested type, any kind of registration can be performed. This always happens
    /// after all prototypes and dependencies are registered.
    /// </summary>
    void Register(ImmutableArray<TMod> mods, RegistrationContext context);
  }
}
