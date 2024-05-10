// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.IProtoUnlock
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  /// <summary>
  /// Unlocks a prototype that was previously locked to the player.
  /// </summary>
  public interface IProtoUnlock : IUnlockNodeUnit
  {
    /// <summary>Protos to be unlocked.</summary>
    ImmutableArray<IProto> UnlockedProtos { get; }
  }
}
