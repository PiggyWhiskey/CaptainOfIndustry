// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToDiscoverWorldMine
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToDiscoverWorldMine : Goal
  {
    private readonly GoalToDiscoverWorldMine.Proto m_goalProto;
    private readonly WorldMapManager m_mapManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToDiscoverWorldMine(
      GoalToDiscoverWorldMine.Proto goalProto,
      WorldMapManager mapManager)
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
      return this.m_mapManager.Map.Locations.Any<WorldMapLocation>((Func<WorldMapLocation, bool>) (x =>
      {
        if (!x.Entity.HasValue || !(x.Entity.Value is WorldMapMine worldMapMine2) || !((Mafi.Core.Prototypes.Proto) worldMapMine2.Prototype.ProducedProductPerStep.Product == (Mafi.Core.Prototypes.Proto) this.m_goalProto.ProductToMine))
          return false;
        return !this.m_goalProto.RequireRepaired || worldMapMine2.IsRepaired;
      }));
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToDiscoverWorldMine value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToDiscoverWorldMine>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToDiscoverWorldMine.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<GoalToDiscoverWorldMine.Proto>(this.m_goalProto);
      WorldMapManager.Serialize(this.m_mapManager, writer);
    }

    public static GoalToDiscoverWorldMine Deserialize(BlobReader reader)
    {
      GoalToDiscoverWorldMine discoverWorldMine;
      if (reader.TryStartClassDeserialization<GoalToDiscoverWorldMine>(out discoverWorldMine))
        reader.EnqueueDataDeserialization((object) discoverWorldMine, GoalToDiscoverWorldMine.s_deserializeDataDelayedAction);
      return discoverWorldMine;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToDiscoverWorldMine>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToDiscoverWorldMine.Proto>());
      reader.SetField<GoalToDiscoverWorldMine>(this, "m_mapManager", (object) WorldMapManager.Deserialize(reader));
    }

    static GoalToDiscoverWorldMine()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToDiscoverWorldMine.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToDiscoverWorldMine.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public readonly LocStrFormatted Title;
      public readonly ProductProto ProductToMine;
      public readonly bool RequireRepaired;

      public override Type Implementation => typeof (GoalToDiscoverWorldMine);

      public Proto(
        string id,
        ProductProto productToMine,
        LocStrFormatted title,
        bool requireRepaired = false,
        Mafi.Core.Prototypes.Proto.ID? tutorial = null,
        int lockedByIndex = -1,
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
        this.Title = title;
        this.ProductToMine = productToMine;
        this.RequireRepaired = requireRepaired;
      }
    }
  }
}
