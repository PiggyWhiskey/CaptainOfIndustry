// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.IEntityWithPorts
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Ports.Io;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Ports
{
  public interface IEntityWithPorts : 
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    ImmutableArray<IoPort> Ports { get; }

    void OnPortConnectionChanged(IoPort ourPort, IoPort otherPort);

    /// <summary>Returns what was not received.</summary>
    Quantity ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort);
  }
}
