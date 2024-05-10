// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToReachRefugees
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Settlements;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToReachRefugees : Goal
  {
    private readonly GoalToReachRefugees.Proto m_goalProto;
    private readonly SettlementsManager m_settlementsManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToReachRefugees(
      GoalToReachRefugees.Proto goalProto,
      SettlementsManager settlementsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_settlementsManager = settlementsManager;
      this.Title = GoalToReachRefugees.Proto.Title.TranslatedString;
    }

    protected override bool UpdateInternal()
    {
      return this.m_settlementsManager.NewPopsFromAdoptions.Lifetime.IsPositive;
    }

    protected override void UpdateTitleOnLoad()
    {
      this.Title = GoalToReachRefugees.Proto.Title.TranslatedString;
    }

    public static void Serialize(GoalToReachRefugees value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToReachRefugees>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToReachRefugees.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<GoalToReachRefugees.Proto>(this.m_goalProto);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
    }

    public static GoalToReachRefugees Deserialize(BlobReader reader)
    {
      GoalToReachRefugees goalToReachRefugees;
      if (reader.TryStartClassDeserialization<GoalToReachRefugees>(out goalToReachRefugees))
        reader.EnqueueDataDeserialization((object) goalToReachRefugees, GoalToReachRefugees.s_deserializeDataDelayedAction);
      return goalToReachRefugees;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToReachRefugees>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToReachRefugees.Proto>());
      reader.SetField<GoalToReachRefugees>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
    }

    static GoalToReachRefugees()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToReachRefugees.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToReachRefugees.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public static readonly LocStr Title;

      public override Type Implementation => typeof (GoalToReachRefugees);

      public Proto(string id, int lockedByIndex = -1)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex);
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToReachRefugees.Proto.Title = Loc.Str("Goal__WaitForRefugees", "Wait for the first refugees to arrive", "text for a goal");
      }
    }
  }
}
