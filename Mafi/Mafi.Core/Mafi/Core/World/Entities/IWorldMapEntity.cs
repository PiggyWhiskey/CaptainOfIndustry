// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.IWorldMapEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public interface IWorldMapEntity : IEntity, IIsSafeAsHashKey
  {
    bool IsOwnedByPlayer { get; }

    WorldMapLocation Location { get; }
  }
}
