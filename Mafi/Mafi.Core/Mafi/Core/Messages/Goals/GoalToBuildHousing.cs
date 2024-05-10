// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToBuildHousing
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Settlements;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToBuildHousing : Goal
  {
    private readonly GoalToBuildHousing.Proto m_goalProto;
    private readonly SettlementsManager m_settlementsManager;
    private int m_currentCount;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToBuildHousing(
      GoalToBuildHousing.Proto goalProto,
      SettlementsManager settlementsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_settlementsManager = settlementsManager;
      this.updateTitle();
    }

    protected override bool UpdateInternal()
    {
      int num = this.m_settlementsManager.Settlements.Max<Settlement>((Func<Settlement, int>) (x => x.HousingModules.Count<SettlementHousingModule>((Func<SettlementHousingModule, bool>) (h => h.IsConstructed))));
      if (this.m_currentCount != num)
      {
        this.m_currentCount = num;
        this.updateTitle();
      }
      return this.m_currentCount >= this.m_goalProto.TargetHousingCount;
    }

    private void updateTitle()
    {
      this.Title = this.m_goalProto.Title.ToString() + string.Format(" ({0} / {1})", (object) this.m_currentCount, (object) this.m_goalProto.TargetHousingCount);
    }

    protected override void UpdateTitleOnLoad() => this.updateTitle();

    public static void Serialize(GoalToBuildHousing value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToBuildHousing>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToBuildHousing.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.m_currentCount);
      writer.WriteGeneric<GoalToBuildHousing.Proto>(this.m_goalProto);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
    }

    public static GoalToBuildHousing Deserialize(BlobReader reader)
    {
      GoalToBuildHousing goalToBuildHousing;
      if (reader.TryStartClassDeserialization<GoalToBuildHousing>(out goalToBuildHousing))
        reader.EnqueueDataDeserialization((object) goalToBuildHousing, GoalToBuildHousing.s_deserializeDataDelayedAction);
      return goalToBuildHousing;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentCount = reader.ReadInt();
      reader.SetField<GoalToBuildHousing>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToBuildHousing.Proto>());
      reader.SetField<GoalToBuildHousing>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
    }

    static GoalToBuildHousing()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToBuildHousing.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToBuildHousing.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      private static readonly LocStr1 TITLE_BUILD_HOUSING;
      public readonly LocStrFormatted Title;
      public readonly int TargetHousingCount;

      public override Type Implementation => typeof (GoalToBuildHousing);

      public Proto(
        string id,
        SettlementHousingModuleProto housingProto,
        int targetHousingCount,
        int lockedByIndex = -1,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        GoalProto.TutorialUnlockMode tutorialUnlock = GoalProto.TutorialUnlockMode.DoNotUnlock)
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
        this.Title = GoalToBuildHousing.Proto.TITLE_BUILD_HOUSING.Format(string.Format("<bc>{0}</bc>", (object) housingProto.Strings.Name));
        this.TargetHousingCount = targetHousingCount;
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToBuildHousing.Proto.TITLE_BUILD_HOUSING = Loc.Str1("Goal__BuildHousing", "Build {0} attached to the existing settlement", "text for a goal, {0} replaced with the name of a housing unit");
      }
    }
  }
}
