// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.Actions.SetupTerrainDesignationsAction
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Input;
using Mafi.Core.Map;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted.Actions
{
  [GenerateSerializer(false, null, 0)]
  public class SetupTerrainDesignationsAction : IScriptedAiPlayerAction
  {
    private readonly StaticEntityProto.ID? m_mineTowerId;
    private readonly Proto.ID m_designationId;
    private readonly Option<string> m_mineTowerName;
    private readonly Proto.ID? m_terrainResourceId;
    private readonly RectangleTerrainArea2i? m_explicitArea;
    private readonly DesignationType m_type;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Type ActionCoreType => typeof (SetupTerrainDesignationsAction.Core);

    public string Description => "Dump setup";

    /// <summary>Without mining tower. Area specified explicitly.</summary>
    public SetupTerrainDesignationsAction(
      Proto.ID designationId,
      RectangleTerrainArea2i area,
      DesignationType type)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_designationId = designationId;
      this.m_explicitArea = new RectangleTerrainArea2i?(area);
      this.m_type = type;
    }

    /// <summary>With mining tower. Area matches given resource.</summary>
    public SetupTerrainDesignationsAction(
      StaticEntityProto.ID mineTowerId,
      Option<string> mineTowerName,
      Proto.ID designationId,
      DesignationType type,
      Proto.ID terrainResourceId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_mineTowerId = new StaticEntityProto.ID?(mineTowerId);
      this.m_mineTowerName = mineTowerName;
      this.m_designationId = designationId;
      this.m_type = type;
      this.m_terrainResourceId = new Proto.ID?(terrainResourceId);
    }

    public static void Serialize(SetupTerrainDesignationsAction value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetupTerrainDesignationsAction>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetupTerrainDesignationsAction.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Proto.ID.Serialize(this.m_designationId, writer);
      writer.WriteNullableStruct<RectangleTerrainArea2i>(this.m_explicitArea);
      writer.WriteNullableStruct<StaticEntityProto.ID>(this.m_mineTowerId);
      Option<string>.Serialize(this.m_mineTowerName, writer);
      writer.WriteNullableStruct<Proto.ID>(this.m_terrainResourceId);
      writer.WriteInt((int) this.m_type);
    }

    public static SetupTerrainDesignationsAction Deserialize(BlobReader reader)
    {
      SetupTerrainDesignationsAction designationsAction;
      if (reader.TryStartClassDeserialization<SetupTerrainDesignationsAction>(out designationsAction))
        reader.EnqueueDataDeserialization((object) designationsAction, SetupTerrainDesignationsAction.s_deserializeDataDelayedAction);
      return designationsAction;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<SetupTerrainDesignationsAction>(this, "m_designationId", (object) Proto.ID.Deserialize(reader));
      reader.SetField<SetupTerrainDesignationsAction>(this, "m_explicitArea", (object) reader.ReadNullableStruct<RectangleTerrainArea2i>());
      reader.SetField<SetupTerrainDesignationsAction>(this, "m_mineTowerId", (object) reader.ReadNullableStruct<StaticEntityProto.ID>());
      reader.SetField<SetupTerrainDesignationsAction>(this, "m_mineTowerName", (object) Option<string>.Deserialize(reader));
      reader.SetField<SetupTerrainDesignationsAction>(this, "m_terrainResourceId", (object) reader.ReadNullableStruct<Proto.ID>());
      reader.SetField<SetupTerrainDesignationsAction>(this, "m_type", (object) (DesignationType) reader.ReadInt());
    }

    static SetupTerrainDesignationsAction()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetupTerrainDesignationsAction.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetupTerrainDesignationsAction) obj).SerializeData(writer));
      SetupTerrainDesignationsAction.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetupTerrainDesignationsAction) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Core : IScriptedAiPlayerActionCore
    {
      private readonly SetupTerrainDesignationsAction m_action;
      private readonly InputScheduler m_inputScheduler;
      private readonly TestMapGeneratorConfig m_testMapGeneratorConfig;
      private Option<CreateStaticEntityCmd> m_buildMineTowerCmd;
      private Option<MineTowerAreaChangeCmd> m_changeAreaCmd;
      private Option<AddTerrainDesignationsCmd> m_designationsCmd;
      private RectangleTerrainArea2i? m_area;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Core(
        SetupTerrainDesignationsAction action,
        InputScheduler inputScheduler,
        TestMapGeneratorConfig testMapGeneratorConfig)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_action = action;
        this.m_inputScheduler = inputScheduler;
        this.m_testMapGeneratorConfig = testMapGeneratorConfig;
      }

      public bool Perform(ScriptedAiPlayer player)
      {
        if (!this.m_area.HasValue)
        {
          Assert.That<bool>(this.m_action.m_terrainResourceId.HasValue).IsNotEqualTo<bool>(this.m_action.m_explicitArea.HasValue);
          if (this.m_action.m_terrainResourceId.HasValue)
          {
            this.m_area = TestMapGenerator.GetResourceArea(this.m_action.m_terrainResourceId.Value, this.m_testMapGeneratorConfig);
            if (!this.m_area.HasValue)
            {
              Log.Error(string.Format("Failed to find mining area for `{0}`.", (object) this.m_action.m_terrainResourceId.Value));
              return true;
            }
          }
          else
            this.m_area = new RectangleTerrainArea2i?(this.m_action.m_explicitArea.Value);
        }
        if (this.m_action.m_mineTowerId.HasValue)
        {
          if (this.m_buildMineTowerCmd.IsNone)
          {
            Tile2i tile = this.m_area.Value.Origin.AddX(this.m_area.Value.Size.X / 2);
            tile = this.m_area.Value.Origin.Y <= 0 ? tile.AddY(this.m_area.Value.Size.Y + 10) : tile.AddY(-10);
            this.m_buildMineTowerCmd = (Option<CreateStaticEntityCmd>) this.m_inputScheduler.ScheduleInputCmd<CreateStaticEntityCmd>(new CreateStaticEntityCmd(this.m_action.m_mineTowerId.Value, new TileTransform(player.GetSurfaceTile(tile), Rotation90.Deg0)));
            return false;
          }
          if (!this.m_buildMineTowerCmd.Value.IsProcessed)
            return false;
          if (this.m_changeAreaCmd.IsNone)
          {
            this.m_changeAreaCmd = (Option<MineTowerAreaChangeCmd>) this.m_inputScheduler.ScheduleInputCmd<MineTowerAreaChangeCmd>(new MineTowerAreaChangeCmd(this.m_buildMineTowerCmd.Value.Result, this.m_area.Value.ExtendBy(TerrainDesignation.Size)));
            if (this.m_action.m_mineTowerName.HasValue)
              player.RegisterNamedEntity(this.m_buildMineTowerCmd.Value.Result, this.m_action.m_mineTowerName.Value);
            return false;
          }
          if (!this.m_changeAreaCmd.Value.IsProcessed)
            return false;
        }
        if (this.m_designationsCmd.IsNone)
        {
          RectangleTerrainArea2i rectangleTerrainArea2i = this.m_area.Value;
          this.m_designationsCmd = (Option<AddTerrainDesignationsCmd>) this.m_inputScheduler.ScheduleInputCmd<AddTerrainDesignationsCmd>(new AddTerrainDesignationsCmd(this.m_action.m_designationId, DesignationDataFactory.CreateArea(this.m_action.m_type, rectangleTerrainArea2i.Origin, rectangleTerrainArea2i.Origin + rectangleTerrainArea2i.Size, this.m_testMapGeneratorConfig.LandHeight, this.m_area.Value.Origin.Y > 0 ? Direction90.PlusY : Direction90.MinusY).ToImmutableArrayAndClear()));
        }
        return this.m_designationsCmd.Value.IsProcessed;
      }

      public static void Serialize(SetupTerrainDesignationsAction.Core value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<SetupTerrainDesignationsAction.Core>(value))
          return;
        writer.EnqueueDataSerialization((object) value, SetupTerrainDesignationsAction.Core.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        SetupTerrainDesignationsAction.Serialize(this.m_action, writer);
        writer.WriteNullableStruct<RectangleTerrainArea2i>(this.m_area);
        Option<CreateStaticEntityCmd>.Serialize(this.m_buildMineTowerCmd, writer);
        Option<MineTowerAreaChangeCmd>.Serialize(this.m_changeAreaCmd, writer);
        Option<AddTerrainDesignationsCmd>.Serialize(this.m_designationsCmd, writer);
        InputScheduler.Serialize(this.m_inputScheduler, writer);
        TestMapGeneratorConfig.Serialize(this.m_testMapGeneratorConfig, writer);
      }

      public static SetupTerrainDesignationsAction.Core Deserialize(BlobReader reader)
      {
        SetupTerrainDesignationsAction.Core core;
        if (reader.TryStartClassDeserialization<SetupTerrainDesignationsAction.Core>(out core))
          reader.EnqueueDataDeserialization((object) core, SetupTerrainDesignationsAction.Core.s_deserializeDataDelayedAction);
        return core;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<SetupTerrainDesignationsAction.Core>(this, "m_action", (object) SetupTerrainDesignationsAction.Deserialize(reader));
        this.m_area = reader.ReadNullableStruct<RectangleTerrainArea2i>();
        this.m_buildMineTowerCmd = Option<CreateStaticEntityCmd>.Deserialize(reader);
        this.m_changeAreaCmd = Option<MineTowerAreaChangeCmd>.Deserialize(reader);
        this.m_designationsCmd = Option<AddTerrainDesignationsCmd>.Deserialize(reader);
        reader.SetField<SetupTerrainDesignationsAction.Core>(this, "m_inputScheduler", (object) InputScheduler.Deserialize(reader));
        reader.SetField<SetupTerrainDesignationsAction.Core>(this, "m_testMapGeneratorConfig", (object) TestMapGeneratorConfig.Deserialize(reader));
      }

      static Core()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        SetupTerrainDesignationsAction.Core.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetupTerrainDesignationsAction.Core) obj).SerializeData(writer));
        SetupTerrainDesignationsAction.Core.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetupTerrainDesignationsAction.Core) obj).DeserializeData(reader));
      }
    }
  }
}
