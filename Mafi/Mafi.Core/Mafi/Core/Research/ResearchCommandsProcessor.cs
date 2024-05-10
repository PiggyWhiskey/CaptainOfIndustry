// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Research
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ResearchCommandsProcessor : 
    ICommandProcessor<ResearchStartCmd>,
    IAction<ResearchStartCmd>,
    ICommandProcessor<ResearchStopCmd>,
    IAction<ResearchStopCmd>,
    ICommandProcessor<ResearchCheatFinishCmd>,
    IAction<ResearchCheatFinishCmd>,
    ICommandProcessor<ResearchQueueDequeueCmd>,
    IAction<ResearchQueueDequeueCmd>
  {
    private readonly ProtosDb m_protosDb;
    private readonly ResearchManager m_researchManager;

    public ResearchCommandsProcessor(ProtosDb protosDb, ResearchManager researchManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_researchManager = researchManager;
    }

    public void Invoke(ResearchStartCmd cmd)
    {
      ResearchNodeProto proto;
      if (!this.m_protosDb.TryGetProto<ResearchNodeProto>((Proto.ID) cmd.NodeId, out proto))
      {
        cmd.SetResultError(string.Format("Research node '{0}' was not found.", (object) cmd.NodeId));
      }
      else
      {
        string errorMessage;
        if (this.m_researchManager.TryStartResearch(proto, out errorMessage))
          cmd.SetResultSuccess();
        else
          cmd.SetResultError(errorMessage);
      }
    }

    public void Invoke(ResearchStopCmd cmd)
    {
      this.m_researchManager.StopResearch();
      cmd.SetResultSuccess();
    }

    public void Invoke(ResearchCheatFinishCmd cmd)
    {
      this.m_researchManager.Cheat_FinishCurrent();
      cmd.SetResultSuccess();
    }

    public void Invoke(ResearchQueueDequeueCmd cmd)
    {
      ResearchNodeProto proto;
      if (!this.m_protosDb.TryGetProto<ResearchNodeProto>((Proto.ID) cmd.NodeId, out proto))
        cmd.SetResultError(string.Format("Research node '{0}' was not found.", (object) cmd.NodeId));
      else if (cmd.IsEnqueue)
      {
        if (this.m_researchManager.TryEnqueueResearch(proto))
          cmd.SetResultSuccess();
        else
          cmd.SetResultError(string.Format("Failed to enqueue research {0}", (object) cmd.NodeId));
      }
      else if (this.m_researchManager.TryDequeueResearch(proto))
        cmd.SetResultSuccess();
      else
        cmd.SetResultError(string.Format("Failed to dequeue research {0}", (object) cmd.NodeId));
    }
  }
}
