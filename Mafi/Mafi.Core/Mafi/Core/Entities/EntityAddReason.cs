// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntityAddReason
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities
{
  public enum EntityAddReason
  {
    /// <summary>
    /// New entity that is added to the game and is being paid for.
    /// </summary>
    New,
    /// <summary>
    /// Entity that is added just because it was moved. See <see cref="!:IMovableEntity" />.
    /// </summary>
    Move,
  }
}
