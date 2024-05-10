// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToResearchNode
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Research;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToResearchNode : Goal
  {
    private readonly GoalToResearchNode.Proto m_goalProto;
    private readonly ResearchManager m_researchManager;
    private Percent m_currentProgress;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToResearchNode(GoalToResearchNode.Proto goalProto, ResearchManager researchManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_researchManager = researchManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      ResearchNode node;
      if (!this.m_researchManager.TryGetResearchNode(this.m_goalProto.NodeToResearch.Id, out node))
      {
        Log.Error(string.Format("Failed to find proto for '{0}'", (object) this.m_goalProto.NodeToResearch.Id));
        return true;
      }
      if (node.State == ResearchNodeState.Researched)
      {
        this.m_currentProgress = Percent.Hundred;
        this.updateTitle();
        return true;
      }
      if (this.m_currentProgress != node.ProgressInPerc)
      {
        this.m_currentProgress = node.ProgressInPerc;
        this.updateTitle();
      }
      return false;
    }

    private void updateTitle()
    {
      this.Title = this.m_goalProto.Title.Format(string.Format("<bc>{0}</bc>", (object) this.m_goalProto.NodeToResearch.Strings.Name)).Value + " (" + this.m_currentProgress.ToStringRounded() + ")";
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToResearchNode value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToResearchNode>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToResearchNode.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.m_currentProgress, writer);
      writer.WriteGeneric<GoalToResearchNode.Proto>(this.m_goalProto);
      ResearchManager.Serialize(this.m_researchManager, writer);
    }

    public static GoalToResearchNode Deserialize(BlobReader reader)
    {
      GoalToResearchNode goalToResearchNode;
      if (reader.TryStartClassDeserialization<GoalToResearchNode>(out goalToResearchNode))
        reader.EnqueueDataDeserialization((object) goalToResearchNode, GoalToResearchNode.s_deserializeDataDelayedAction);
      return goalToResearchNode;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentProgress = Percent.Deserialize(reader);
      reader.SetField<GoalToResearchNode>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToResearchNode.Proto>());
      reader.SetField<GoalToResearchNode>(this, "m_researchManager", (object) ResearchManager.Deserialize(reader));
    }

    static GoalToResearchNode()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToResearchNode.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToResearchNode.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      private static readonly LocStr1 TITLE_RESEARCH;
      public readonly ResearchNodeProto NodeToResearch;
      /// <summary>{0} replaced with tech to research</summary>
      public readonly LocStr1 Title;

      public override Type Implementation => typeof (GoalToResearchNode);

      public Proto(
        string id,
        ResearchNodeProto nodeToResearch,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex, tip, tutorialUnlock);
        this.NodeToResearch = nodeToResearch;
        this.Title = GoalToResearchNode.Proto.TITLE_RESEARCH;
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToResearchNode.Proto.TITLE_RESEARCH = Loc.Str1("Goal__Research", "Research {0}", "text for a goal, {0} replaced with title of a node to research");
      }
    }
  }
}
