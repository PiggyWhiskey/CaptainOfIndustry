// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.IEntityWithBoost
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  public interface IEntityWithBoost : IEntity, IIsSafeAsHashKey
  {
    /// <summary>
    /// Whether the player request boost. That doesn't mean that the boost is active if there isn't enough unity.
    /// </summary>
    bool IsBoostRequested { get; }

    void SetBoosted(bool isBoosted);

    Upoints? BoostCost { get; }
  }
}
