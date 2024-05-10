// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.NuclearWaste.NuclearWasteStorageCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Storages.NuclearWaste
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class NuclearWasteStorageCommandsProcessor : 
    ICommandProcessor<NuclearWasteStorageToggleOutputPortCmd>,
    IAction<NuclearWasteStorageToggleOutputPortCmd>
  {
    private readonly EntitiesManager m_entitiesManager;
    private readonly ProtosDb m_protosDb;

    public NuclearWasteStorageCommandsProcessor(EntitiesManager entitiesManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(NuclearWasteStorageToggleOutputPortCmd cmd)
    {
      NuclearWasteStorage entity;
      if (!this.m_entitiesManager.TryGetEntity<NuclearWasteStorage>(cmd.StorageId, out entity))
      {
        cmd.SetResultError(string.Format("Storage '{0}' was not found.", (object) cmd.StorageId));
      }
      else
      {
        entity.ToggleDoNotSendRetiredWasteToOutputPort();
        cmd.SetResultSuccess();
      }
    }
  }
}
