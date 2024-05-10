// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.IRocketOwner
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.SpaceProgram
{
  /// <summary>
  /// Enables entity to handle rockets. Note that owning entity is responsible for destroying of
  /// the rocket when itself is destroyed.
  /// </summary>
  public interface IRocketOwner : IRenderedEntity, IEntity, IIsSafeAsHashKey
  {
    Option<RocketEntityBase> AttachedRocketBase { get; }

    Option<RocketEntityBase> DetachRocket();

    bool CanAttachRocket(RocketEntityBase rocket);

    void AttachRocket(RocketEntityBase rocket);
  }
}
