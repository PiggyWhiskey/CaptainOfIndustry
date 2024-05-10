// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToExploreWithShip
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToExploreWithShip : Goal
  {
    private readonly GoalToExploreWithShip.Proto m_goalProto;
    private readonly WorldMapManager m_mapManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToExploreWithShip(GoalToExploreWithShip.Proto goalProto, WorldMapManager mapManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_mapManager = mapManager;
      this.Title = goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      return this.m_mapManager.Map.Locations.Count<WorldMapLocation>((Func<WorldMapLocation, bool>) (x => x.State == WorldMapLocationState.Explored && this.m_mapManager.Map.HomeLocation != x)) > 1;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToExploreWithShip value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToExploreWithShip>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToExploreWithShip.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<GoalToExploreWithShip.Proto>(this.m_goalProto);
      WorldMapManager.Serialize(this.m_mapManager, writer);
    }

    public static GoalToExploreWithShip Deserialize(BlobReader reader)
    {
      GoalToExploreWithShip toExploreWithShip;
      if (reader.TryStartClassDeserialization<GoalToExploreWithShip>(out toExploreWithShip))
        reader.EnqueueDataDeserialization((object) toExploreWithShip, GoalToExploreWithShip.s_deserializeDataDelayedAction);
      return toExploreWithShip;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToExploreWithShip>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToExploreWithShip.Proto>());
      reader.SetField<GoalToExploreWithShip>(this, "m_mapManager", (object) WorldMapManager.Deserialize(reader));
    }

    static GoalToExploreWithShip()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToExploreWithShip.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToExploreWithShip.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;

      public override Type Implementation => typeof (GoalToExploreWithShip);

      public Proto(string id, LocStrFormatted title, int lockedByIndex = -1, Mafi.Core.Prototypes.Proto.ID? tutorial = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, tutorial, lockedByIndex);
        this.Title = title;
      }
    }
  }
}
