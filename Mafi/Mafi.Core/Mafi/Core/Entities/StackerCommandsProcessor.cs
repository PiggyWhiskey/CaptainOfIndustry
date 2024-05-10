// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.StackerCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Commands;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Core.Entities
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class StackerCommandsProcessor : 
    ICommandProcessor<SetDumpHeightOffsetCmd>,
    IAction<SetDumpHeightOffsetCmd>
  {
    private readonly EntitiesManager m_entitiesManager;

    public StackerCommandsProcessor(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
    }

    public void Invoke(SetDumpHeightOffsetCmd offsetCmd)
    {
      Entity entity;
      if (!this.m_entitiesManager.TryGetEntity<Entity>(offsetCmd.EntityId, out entity))
        offsetCmd.SetResultError(string.Format("Unknown entity '{0}'.", (object) offsetCmd.EntityId));
      else if (!(entity is IEntityWithOutputToTerrain withOutputToTerrain))
      {
        offsetCmd.SetResultError(string.Format("Entity does not output to terrain '{0}'.", (object) offsetCmd.EntityId));
      }
      else
      {
        withOutputToTerrain.SetDumpHeightOffset(offsetCmd.Depth);
        offsetCmd.SetResultSuccess();
      }
    }
  }
}
