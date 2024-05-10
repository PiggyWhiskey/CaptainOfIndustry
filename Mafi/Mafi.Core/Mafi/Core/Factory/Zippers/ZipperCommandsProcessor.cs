// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.ZipperCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ZipperCommandsProcessor : 
    ICommandProcessor<ZipperSetPriorityPortsCmd>,
    IAction<ZipperSetPriorityPortsCmd>,
    ICommandProcessor<ZipperSetForceEvenInputsCmd>,
    IAction<ZipperSetForceEvenInputsCmd>,
    ICommandProcessor<ZipperSetForceEvenOutputsCmd>,
    IAction<ZipperSetForceEvenOutputsCmd>
  {
    private readonly EntitiesManager m_entitiesManager;

    public ZipperCommandsProcessor(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
    }

    public void Invoke(ZipperSetPriorityPortsCmd cmd)
    {
      Zipper entity;
      if (!this.m_entitiesManager.TryGetEntity<Zipper>(cmd.ZipperId, out entity))
        cmd.SetResultError(string.Format("Zipper entity '{0}' was not found.", (object) cmd.ZipperId));
      else if (entity.TrySetPortPriorityForPort(cmd.PortName, cmd.IsPrioritized))
        cmd.SetResultSuccess();
      else
        cmd.SetResultError("Failed to find port to prioritize.");
    }

    public void Invoke(ZipperSetForceEvenInputsCmd cmd)
    {
      Zipper entity;
      if (!this.m_entitiesManager.TryGetEntity<Zipper>(cmd.ZipperId, out entity))
      {
        cmd.SetResultError(string.Format("Zipper entity '{0}' was not found.", (object) cmd.ZipperId));
      }
      else
      {
        entity.SetForceEvenInputs(cmd.ForceEvenInputs);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(ZipperSetForceEvenOutputsCmd cmd)
    {
      Zipper entity;
      if (!this.m_entitiesManager.TryGetEntity<Zipper>(cmd.ZipperId, out entity))
      {
        cmd.SetResultError(string.Format("Zipper entity '{0}' was not found.", (object) cmd.ZipperId));
      }
      else
      {
        entity.SetForceEvenOutputs(cmd.ForceEvenOutputs);
        cmd.SetResultSuccess();
      }
    }
  }
}
