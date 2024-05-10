// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.IWorkersManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Population
{
  public interface IWorkersManager
  {
    /// <summary>
    /// Number of available workers. Can be negative if there is not enough workers. Good for UI.
    /// 
    /// Only updated on sim step end.
    /// </summary>
    int AmountOfFreeWorkersOrMissing { get; }

    int NumberOfWorkersWithheld { get; }

    /// <summary>Only fired once per sim step on sim end.</summary>
    IEvent<int> WorkersAmountChanged { get; }

    bool CanWork(IEntityWithWorkers entity, bool doNotNotify = false);

    void ReturnWorkersVoluntarily(IEntityWithWorkers entity);
  }
}
