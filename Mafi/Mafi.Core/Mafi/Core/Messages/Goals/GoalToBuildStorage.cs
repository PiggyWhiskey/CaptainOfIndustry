// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalToBuildStorage
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalToBuildStorage : Goal
  {
    private readonly GoalToBuildStorage.Proto m_goalProto;
    private readonly EntitiesManager m_entitiesManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalToBuildStorage(GoalToBuildStorage.Proto goalProto, EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((GoalProto) goalProto);
      this.m_goalProto = goalProto;
      this.m_entitiesManager = entitiesManager;
      this.Title = this.m_goalProto.Title.Value;
    }

    protected override bool UpdateInternal()
    {
      return this.m_entitiesManager.GetFirstEntityOfType<Storage>((Predicate<Storage>) (x =>
      {
        if (!x.IsConstructed || !(x.Prototype.Id == this.m_goalProto.StorageId) || !x.StoredProduct.HasValue || !(x.StoredProduct.Value.Id == this.m_goalProto.ProductStoredId) || this.m_goalProto.RequireImportSlider && !x.ImportUntilPercent.IsPositive || this.m_goalProto.RequireExportSlider && !(x.ExportFromPercent < Percent.Hundred))
          return false;
        return !this.m_goalProto.RequireLogisticsInputDisabled || x.IsLogisticsInputDisabled;
      })).HasValue;
    }

    protected override void UpdateTitleOnLoad() => this.Title = this.m_goalProto.Title.Value;

    public static void Serialize(GoalToBuildStorage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalToBuildStorage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalToBuildStorage.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<GoalToBuildStorage.Proto>(this.m_goalProto);
    }

    public static GoalToBuildStorage Deserialize(BlobReader reader)
    {
      GoalToBuildStorage goalToBuildStorage;
      if (reader.TryStartClassDeserialization<GoalToBuildStorage>(out goalToBuildStorage))
        reader.EnqueueDataDeserialization((object) goalToBuildStorage, GoalToBuildStorage.s_deserializeDataDelayedAction);
      return goalToBuildStorage;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalToBuildStorage>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalToBuildStorage>(this, "m_goalProto", (object) reader.ReadGenericAs<GoalToBuildStorage.Proto>());
    }

    static GoalToBuildStorage()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalToBuildStorage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Goal) obj).SerializeData(writer));
      GoalToBuildStorage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Goal) obj).DeserializeData(reader));
    }

    public class Proto : GoalProto
    {
      public static readonly LocStr2 TITLE_BUILD_STORAGE;
      public readonly LocStrFormatted Title;
      public readonly StaticEntityProto.ID StorageId;
      public readonly ProductProto.ID ProductStoredId;
      public readonly bool RequireImportSlider;
      public readonly bool RequireExportSlider;
      public readonly bool RequireLogisticsInputDisabled;

      public override Type Implementation => typeof (GoalToBuildStorage);

      public Proto(
        string id,
        LocStrFormatted title,
        StaticEntityProto.ID storageId,
        ProductProto.ID productStoredId,
        bool requireImportSlider = false,
        bool requireExportSlider = false,
        bool requireLogisticsInputDisabled = false,
        int lockedByIndex = -1,
        LocStrFormatted? tip = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(id, new Mafi.Core.Prototypes.Proto.ID?(), lockedByIndex, tip);
        this.Title = title;
        this.StorageId = storageId;
        this.ProductStoredId = productStoredId;
        this.RequireImportSlider = requireImportSlider;
        this.RequireExportSlider = requireExportSlider;
        this.RequireLogisticsInputDisabled = requireLogisticsInputDisabled;
      }

      static Proto()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        GoalToBuildStorage.Proto.TITLE_BUILD_STORAGE = Loc.Str2("Goal__BuildStorage", "Build {0} and assign it {1}", "goal text, {0} - storage, {1} - product name");
      }
    }
  }
}
