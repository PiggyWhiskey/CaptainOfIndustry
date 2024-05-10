// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IEntityWithPosition
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities
{
  public interface IEntityWithPosition : IRenderedEntity, IEntity, IIsSafeAsHashKey
  {
    /// <summary>
    /// It is cheaper to return only 2D position for some entities such as vehicles.
    /// </summary>
    Tile2f Position2f { get; }

    Tile3f Position3f { get; }
  }
}
