// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.BuildStaticEntityAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class BuildStaticEntityAction : IScriptedAiPlayerAction
  {
    private readonly StaticEntityProto.ID m_protoId;
    private readonly Option<string> m_name;
    private readonly PlacementSpec m_placementSpec;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (BuildStaticEntityAction.Core);

    public string Description => string.Format("Build `{0}`", (object) this.m_protoId);

    public BuildStaticEntityAction(
      StaticEntityProto.ID protoId,
      string name = null,
      PlacementSpec placement = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protoId = protoId;
      this.m_name = (Option<string>) name;
      this.m_placementSpec = placement ?? new PlacementSpec();
    }

    public static void Serialize(BuildStaticEntityAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BuildStaticEntityAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BuildStaticEntityAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<string>.Serialize(this.m_name, writer);
      PlacementSpec.Serialize(this.m_placementSpec, writer);
      StaticEntityProto.ID.Serialize(this.m_protoId, writer);
    }

    public static BuildStaticEntityAction Deserialize(BlobReader reader)
    {
      BuildStaticEntityAction staticEntityAction;
      if (reader.TryStartClassDeserialization<BuildStaticEntityAction>(out staticEntityAction))
        reader.EnqueueDataDeserialization((object) staticEntityAction, BuildStaticEntityAction.s_deserializeDataDelayedAction);
      return staticEntityAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<BuildStaticEntityAction>(this, "m_name", (object) Option<string>.Deserialize(reader));
      reader.SetField<BuildStaticEntityAction>(this, "m_placementSpec", (object) PlacementSpec.Deserialize(reader));
      reader.SetField<BuildStaticEntityAction>(this, "m_protoId", (object) StaticEntityProto.ID.Deserialize(reader));
    }

    static BuildStaticEntityAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BuildStaticEntityAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BuildStaticEntityAction) obj).SerializeData(writer));
      BuildStaticEntityAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BuildStaticEntityAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly BuildStaticEntityAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private Option<CreateStaticEntityCmd> m_cmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(BuildStaticEntityAction action, InputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (this.m_cmd.IsNone)
        {
          this.m_cmd = (Option<CreateStaticEntityCmd>) this.m_inputScheduler.ScheduleInputCmd<CreateStaticEntityCmd>(new CreateStaticEntityCmd(this.m_action.m_protoId, new TileTransform(player.GetNextBuildingPosition(this.m_action.m_protoId, this.m_action.m_placementSpec), this.m_action.m_placementSpec.Rotation)));
          return false;
        }
        if (!this.m_cmd.Value.IsProcessed)
          return false;
        if (!this.m_cmd.Value.Result.IsValid)
        {
          Log.Error(string.Format("Failed to build `{0}`.", (object) this.m_action.m_protoId));
          return true;
        }
        if (this.m_action.m_name.HasValue)
          player.RegisterNamedEntity(this.m_cmd.Value.Result, this.m_action.m_name.Value);
        return true;
      }

      public static void Serialize(BuildStaticEntityAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<BuildStaticEntityAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, BuildStaticEntityAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        BuildStaticEntityAction.Serialize(this.m_action, writer);
        Option<CreateStaticEntityCmd>.Serialize(this.m_cmd, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
      }

      public static BuildStaticEntityAction.Core Deserialize(BlobReader reader)
      {
        BuildStaticEntityAction.Core core;
        if (reader.TryStartClassDeserialization<BuildStaticEntityAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, BuildStaticEntityAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<BuildStaticEntityAction.Core>(this, "m_action", (object) BuildStaticEntityAction.Deserialize(reader));
        this.m_cmd = Option<CreateStaticEntityCmd>.Deserialize(reader);
        reader.SetField<BuildStaticEntityAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        BuildStaticEntityAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((BuildStaticEntityAction.Core) obj).SerializeData(writer));
        BuildStaticEntityAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((BuildStaticEntityAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
