// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.IUnityConsumingEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Population
{
  public interface IUnityConsumingEntity : IEntityWithGeneralPriority, IEntity, IIsSafeAsHashKey
  {
    /// <summary>Unity consumed at this moment.</summary>
    Upoints MonthlyUnityConsumed { get; }

    /// <summary>
    /// Max possible Unity that can be consumed but is not consumed now.
    /// </summary>
    Upoints MaxMonthlyUnityConsumed { get; }

    Proto.ID UpointsCategoryId { get; }

    Option<Mafi.Core.Population.UnityConsumer> UnityConsumer { get; }
  }
}
