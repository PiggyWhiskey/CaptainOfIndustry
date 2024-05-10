// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Sorters.SorterCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Sorters
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class SorterCommandsProcessor : 
    ICommandProcessor<SorterToggleProductCmd>,
    IAction<SorterToggleProductCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public SorterCommandsProcessor(EntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(SorterToggleProductCmd cmd)
    {
      Option<Sorter> entityOrLog = this.m_entitiesManager.GetEntityOrLog<Sorter>(cmd.SorterId);
      Option<ProductProto> orLog = this.m_protosDb.GetOrLog<ProductProto>((Proto.ID) cmd.ProductId);
      if (entityOrLog.IsNone || orLog.IsNone)
      {
        cmd.SetResultError("Failed to find sorter or product.");
      }
      else
      {
        entityOrLog.Value.ToggleFilteredProduct(orLog.Value);
        cmd.SetResultSuccess();
      }
    }
  }
}
