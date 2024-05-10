// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.BuildStorageAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class BuildStorageAction : IScriptedAiPlayerAction
  {
    private readonly StaticEntityProto.ID m_storageProtoId;
    private readonly ProductProto.ID m_productProtoId;
    private readonly Option<string> m_name;
    private readonly PlacementSpec m_placement;
    private readonly Percent? m_importUntil;
    private readonly Percent? m_exportFrom;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (BuildStorageAction.Core);

    public string Description
    {
      get
      {
        return string.Format("Build `{0}` for `{1}`.", (object) this.m_storageProtoId, (object) this.m_productProtoId);
      }
    }

    public BuildStorageAction(
      StaticEntityProto.ID storageProtoId,
      ProductProto.ID productProtoId,
      string name = null,
      PlacementSpec placement = null,
      Percent? importUntil = null,
      Percent? exportFrom = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_storageProtoId = storageProtoId;
      this.m_productProtoId = productProtoId;
      this.m_name = (Option<string>) name;
      this.m_placement = placement ?? new PlacementSpec();
      this.m_importUntil = importUntil;
      this.m_exportFrom = exportFrom;
    }

    public static void Serialize(BuildStorageAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BuildStorageAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BuildStorageAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteNullableStruct<Percent>(this.m_exportFrom);
      writer.WriteNullableStruct<Percent>(this.m_importUntil);
      Option<string>.Serialize(this.m_name, writer);
      PlacementSpec.Serialize(this.m_placement, writer);
      ProductProto.ID.Serialize(this.m_productProtoId, writer);
      StaticEntityProto.ID.Serialize(this.m_storageProtoId, writer);
    }

    public static BuildStorageAction Deserialize(BlobReader reader)
    {
      BuildStorageAction buildStorageAction;
      if (reader.TryStartClassDeserialization<BuildStorageAction>(out buildStorageAction))
        reader.EnqueueDataDeserialization((object) buildStorageAction, BuildStorageAction.s_deserializeDataDelayedAction);
      return buildStorageAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<BuildStorageAction>(this, "m_exportFrom", (object) reader.ReadNullableStruct<Percent>());
      reader.SetField<BuildStorageAction>(this, "m_importUntil", (object) reader.ReadNullableStruct<Percent>());
      reader.SetField<BuildStorageAction>(this, "m_name", (object) Option<string>.Deserialize(reader));
      reader.SetField<BuildStorageAction>(this, "m_placement", (object) PlacementSpec.Deserialize(reader));
      reader.SetField<BuildStorageAction>(this, "m_productProtoId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<BuildStorageAction>(this, "m_storageProtoId", (object) StaticEntityProto.ID.Deserialize(reader));
    }

    static BuildStorageAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BuildStorageAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BuildStorageAction) obj).SerializeData(writer));
      BuildStorageAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BuildStorageAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly BuildStorageAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private Option<CreateStaticEntityCmd> m_createStorageCmd;
      private Option<StorageSetProductCmd> m_assignProductCmd;
      private Option<StorageSetSliderStepCmd> m_setSliderCmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(BuildStorageAction action, InputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (this.m_createStorageCmd.IsNone)
        {
          this.m_createStorageCmd = (Option<CreateStaticEntityCmd>) this.m_inputScheduler.ScheduleInputCmd<CreateStaticEntityCmd>(new CreateStaticEntityCmd(this.m_action.m_storageProtoId, new TileTransform(player.GetNextBuildingPosition(this.m_action.m_storageProtoId, this.m_action.m_placement), this.m_action.m_placement.Rotation)));
          return false;
        }
        if (!this.m_createStorageCmd.Value.IsProcessed)
          return false;
        if (!this.m_createStorageCmd.Value.Result.IsValid)
        {
          Log.Error(string.Format("Failed to build `{0}`: {1}", (object) this.m_action.m_storageProtoId, (object) this.m_createStorageCmd.Value.ErrorMessage));
          return true;
        }
        if (this.m_assignProductCmd.IsNone)
        {
          if (this.m_action.m_name.HasValue)
            player.RegisterNamedEntity(this.m_createStorageCmd.Value.Result, this.m_action.m_name.Value);
          this.m_assignProductCmd = (Option<StorageSetProductCmd>) this.m_inputScheduler.ScheduleInputCmd<StorageSetProductCmd>(new StorageSetProductCmd(this.m_createStorageCmd.Value.Result, this.m_action.m_productProtoId));
          return false;
        }
        if (!this.m_assignProductCmd.Value.IsProcessed)
          return false;
        if (!this.m_assignProductCmd.Value.Result)
        {
          Log.Error(string.Format("Failed to assign product {0} to storage: ", (object) this.m_action.m_productProtoId) + this.m_assignProductCmd.Value.ErrorMessage);
          return true;
        }
        if (this.m_action.m_importUntil.HasValue || this.m_action.m_exportFrom.HasValue)
        {
          if (this.m_setSliderCmd.IsNone)
          {
            this.m_setSliderCmd = (Option<StorageSetSliderStepCmd>) this.m_inputScheduler.ScheduleInputCmd<StorageSetSliderStepCmd>(new StorageSetSliderStepCmd(this.m_createStorageCmd.Value.Result, this.m_action.m_importUntil, this.m_action.m_exportFrom));
            return false;
          }
          if (!this.m_setSliderCmd.Value.IsProcessed)
            return false;
          if (!this.m_setSliderCmd.Value.Result)
          {
            Log.Error("Failed to set storage i/o sliders: " + this.m_setSliderCmd.Value.ErrorMessage);
            return true;
          }
        }
        return true;
      }

      public static void Serialize(BuildStorageAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<BuildStorageAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, BuildStorageAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        BuildStorageAction.Serialize(this.m_action, writer);
        Option<StorageSetProductCmd>.Serialize(this.m_assignProductCmd, writer);
        Option<CreateStaticEntityCmd>.Serialize(this.m_createStorageCmd, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
        Option<StorageSetSliderStepCmd>.Serialize(this.m_setSliderCmd, writer);
      }

      public static BuildStorageAction.Core Deserialize(BlobReader reader)
      {
        BuildStorageAction.Core core;
        if (reader.TryStartClassDeserialization<BuildStorageAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, BuildStorageAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<BuildStorageAction.Core>(this, "m_action", (object) BuildStorageAction.Deserialize(reader));
        this.m_assignProductCmd = Option<StorageSetProductCmd>.Deserialize(reader);
        this.m_createStorageCmd = Option<CreateStaticEntityCmd>.Deserialize(reader);
        reader.SetField<BuildStorageAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
        this.m_setSliderCmd = Option<StorageSetSliderStepCmd>.Deserialize(reader);
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        BuildStorageAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BuildStorageAction.Core) obj).SerializeData(writer));
        BuildStorageAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BuildStorageAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
