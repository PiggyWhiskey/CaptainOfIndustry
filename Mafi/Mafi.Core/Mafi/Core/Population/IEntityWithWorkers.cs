// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.IEntityWithWorkers
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Population
{
  public interface IEntityWithWorkers : IEntityWithGeneralPriority, IEntity, IIsSafeAsHashKey
  {
    int WorkersNeeded { get; }

    /// <summary>
    /// This property is used as a cache to avoid checking workers when not necessary.
    /// DO NOT UPDATE IT!
    /// This is used by WorkersManager after workers are checked.
    /// Entities can set this to false (typically on workers required change).
    /// IMPORTANT: Setting this to false will not revoke any workers!
    /// IMPORTANT: Do not save it, it will be restored on load properly.
    /// </summary>
    bool HasWorkersCached { get; set; }
  }
}
