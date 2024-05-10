// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToSetupDumping
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Designation;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToSetupDumping : Goal
  {
    private readonly GoalToSetupDumping.Proto m_goalProto;
    private readonly ITerrainDumpingManager m_dumpingManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToSetupDumping(
      GoalToSetupDumping.Proto goalProto,
      ITerrainDumpingManager dumpingManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_dumpingManager = dumpingManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      return this.m_dumpingManager.HasEligibleDumpingDesignationsFor(this.m_goalProto.LooseProductProto);
    }

    private void updateTitle()
    {
      this.Title = GoalToSetupDumping.Proto.TITLE_DESIGNATE_DUMPING.Format(string.Format("<bc>{0}</bc>", (object) this.m_goalProto.LooseProductProto.Strings.Name)).Value;
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToSetupDumping value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToSetupDumping>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToSetupDumping.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ITerrainDumpingManager>(this.m_dumpingManager);
      writer.WriteGeneric<GoalToSetupDumping.Proto>(this.m_goalProto);
    }

    public static GoalToSetupDumping Deserialize(BlobReader reader)
    {
      GoalToSetupDumping goalToSetupDumping;
      if (reader.TryStartClassDeserialization<GoalToSetupDumping>(out goalToSetupDumping))
        reader.EnqueueDataDeserialization((object) goalToSetupDumping, GoalToSetupDumping.s_deserializeDataDelayedAction);
      return goalToSetupDumping;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToSetupDumping>(this, "m_dumpingManager", (object) reader.ReadGenericAs<ITerrainDumpingManager>());
      reader.SetField<GoalToSetupDumping>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToSetupDumping.Proto>());
    }

    static GoalToSetupDumping()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToSetupDumping.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToSetupDumping.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public static readonly LocStr1 TITLE_DESIGNATE_DUMPING;
      public readonly LooseProductProto LooseProductProto;

      public override Type Implementation => typeof (GoalToSetupDumping);

      public Proto(
        string id,
        LooseProductProto looseProduct,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock,
        int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        string id1 = id;
        Mafi.Core.Prototypes.Proto.ID? tutorial1 = tutorial;
        int lockedByIndex1 = lockedByIndex;
        GoalProto.TutorialUnlockMode tutorialUnlockMode = tutorialUnlock;
        LocStrFormatted? tip = new LocStrFormatted?();
        int tutorialUnlock1 = (int) tutorialUnlockMode;
        // ISSUE: explicit constructor call
        base.\u002Ector(id1, tutorial1, lockedByIndex1, tip, (GoalProto.TutorialUnlockMode) tutorialUnlock1);
        this.LooseProductProto = looseProduct;
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToSetupDumping.Proto.TITLE_DESIGNATE_DUMPING = Loc.Str1("Goal__DesignateDumping", "Designate a dumping zone for {0}", "text for a goal, {0} replaced with name of product (e.g. slag)");
      }
    }
  }
}
