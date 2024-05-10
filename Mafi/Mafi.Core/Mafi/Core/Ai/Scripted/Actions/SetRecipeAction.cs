// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.SetRecipeAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class SetRecipeAction : IScriptedAiPlayerAction
  {
    private readonly string m_entityName;
    private readonly RecipeProto.ID m_recipeId;
    private readonly bool m_isEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (SetRecipeAction.Core);

    public string Description
    {
      get
      {
        return string.Format("Set recipe `{0}` of entity `{1}` to ", (object) this.m_recipeId, (object) this.m_entityName) + (this.m_isEnabled ? "enabled" : "disabled") + ".";
      }
    }

    public SetRecipeAction(string entityName, RecipeProto.ID recipeId, bool isEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityName = entityName;
      this.m_recipeId = recipeId;
      this.m_isEnabled = isEnabled;
    }

    public static void Serialize(SetRecipeAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetRecipeAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetRecipeAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteString(this.m_entityName);
      writer.WriteBool(this.m_isEnabled);
      RecipeProto.ID.Serialize(this.m_recipeId, writer);
    }

    public static SetRecipeAction Deserialize(BlobReader reader)
    {
      SetRecipeAction setRecipeAction;
      if (reader.TryStartClassDeserialization<SetRecipeAction>(out setRecipeAction))
        reader.EnqueueDataDeserialization((object) setRecipeAction, SetRecipeAction.s_deserializeDataDelayedAction);
      return setRecipeAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SetRecipeAction>(this, "m_entityName", (object) reader.ReadString());
      reader.SetField<SetRecipeAction>(this, "m_isEnabled", (object) reader.ReadBool());
      reader.SetField<SetRecipeAction>(this, "m_recipeId", (object) RecipeProto.ID.Deserialize(reader));
    }

    static SetRecipeAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetRecipeAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetRecipeAction) obj).SerializeData(writer));
      SetRecipeAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetRecipeAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly SetRecipeAction m_action;
      private readonly IInputScheduler m_inputScheduler;
      private Option<MachineSetRecipeActiveCmd> m_cmd;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(SetRecipeAction action, IInputScheduler inputScheduler)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        Machine entity;
        if (!player.TryGetNamedEntity<Machine>(this.m_action.m_entityName, out entity))
        {
          Log.Error("Failed to find machine `" + this.m_action.m_entityName + "`.");
          return true;
        }
        if (!this.m_cmd.IsNone)
          return this.m_cmd.Value.IsProcessed;
        this.m_cmd = (Option<MachineSetRecipeActiveCmd>) this.m_inputScheduler.ScheduleInputCmd<MachineSetRecipeActiveCmd>(new MachineSetRecipeActiveCmd(entity.Id, this.m_action.m_recipeId, this.m_action.m_isEnabled));
        return false;
      }

      public static void Serialize(SetRecipeAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<SetRecipeAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, SetRecipeAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        SetRecipeAction.Serialize(this.m_action, writer);
        Option<MachineSetRecipeActiveCmd>.Serialize(this.m_cmd, writer);
        writer.WriteGeneric<IInputScheduler>(this.m_inputScheduler);
      }

      public static SetRecipeAction.Core Deserialize(BlobReader reader)
      {
        SetRecipeAction.Core core;
        if (reader.TryStartClassDeserialization<SetRecipeAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, SetRecipeAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<SetRecipeAction.Core>(this, "m_action", (object) SetRecipeAction.Deserialize(reader));
        this.m_cmd = Option<MachineSetRecipeActiveCmd>.Deserialize(reader);
        reader.SetField<SetRecipeAction.Core>(this, "m_inputScheduler", (object) reader.ReadGenericAs<IInputScheduler>());
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        SetRecipeAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetRecipeAction.Core) obj).SerializeData(writer));
        SetRecipeAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetRecipeAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
