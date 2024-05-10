// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.ThermalStorages.ThermalStorageCommandsProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings.ThermalStorages
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ThermalStorageCommandsProcessor : 
    ICommandProcessor<ThermalStorageSetProductCmd>,
    IAction<ThermalStorageSetProductCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public ThermalStorageCommandsProcessor(EntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(ThermalStorageSetProductCmd cmd)
    {
      Option<ThermalStorage> entityOrLog = this.m_entitiesManager.GetEntityOrLog<ThermalStorage>(cmd.StorageId);
      Option<ProductProto> orLog = this.m_protosDb.GetOrLog<ProductProto>((Proto.ID) cmd.ProductId);
      if (entityOrLog.IsNone || orLog.IsNone)
      {
        cmd.SetResultError("Failed to find storage or product.");
      }
      else
      {
        entityOrLog.Value.AssignProduct(orLog.Value);
        cmd.SetResultSuccess();
      }
    }
  }
}
